using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace bedrock.net
{
    /// <summary>
    /// Do basic HTTP processing, with a long-lived socket.
    /// TODO: the BaseSocket parameter in the listener events will always be null for now.
    /// TODO: change HttpSocket to be a is-a of AsyncSocket, not has-a.
    /// </summary>
	public class HttpSocket : BaseSocket, ISocketEventListener
	{
        private class PendingRequest
        {
            public string Method;
            public Uri URI;
            public byte[] Body;
            public string ContentType;
            public int Offset;
            public int Length;

            public WebHeaderCollection Headers = new WebHeaderCollection();
            public int Code = -1;
            public string ResponseText = null;
            public MemoryStream Response;

            public PendingRequest(string method, Uri URL, byte[] body, int offset, int len, string contentType)
            {
                this.Method = method;
                this.URI = URL;
                this.Body = (body == null) ? new byte[] { } : body;
                this.Offset = offset;
                this.Length = len;
                this.ContentType = contentType;
            }

            public int ContentLength
            {
                get
                {
                    string slen = this.Headers[HttpResponseHeader.ContentLength];
                    if (slen == null)
                    {
                        Debug.WriteLine("No Content-Length header");
                        return -1;
                    }
                    try
                    {
                        return int.Parse(slen);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Invalid Content-Length");
                        return -1;
                    }
                }
            }
        }

        private static readonly Encoding ENC = Encoding.UTF8;
        private string m_host = null;
        private Address m_addr = null;
        private bool m_ssl = false;

        private AsyncSocket m_sock = null;
        private LinkedList<PendingRequest> m_queue = new LinkedList<PendingRequest>();
        private Thread m_thread;
        private ParseState m_state = ParseState.START;
        private PendingRequest m_current = null;
        private bool m_keepRunning = true;
        private Uri m_proxyURI = null;
        private NetworkCredential m_proxyCredentials = null;
        private float m_connectRetrySec = 10.0F;
        private int m_maxErrors = 5;
        private int m_errorCount = 0;
                
        private static readonly byte[] SPACE = ENC.GetBytes(" ");
        private static readonly byte[] CRLF = ENC.GetBytes("\r\n");
        private static readonly byte[] COL_SP = ENC.GetBytes(": ");
        private static readonly byte[] SP_HTTP11_CRLF = ENC.GetBytes(" HTTP/1.1\r\n");
        private static readonly byte[] HTTP11_SP = ENC.GetBytes("HTTP/1.1 ");

        /// <summary>
        /// Create a socket.  This starts a thread for background processing, but the thread is mostly paused
        /// waiting for new requests.
        /// </summary>
        public HttpSocket(ISocketEventListener listener) : base(listener)
        {
            m_thread = new Thread(new ThreadStart(ProcessThread));
            m_thread.Name = "HTTP processing thread";
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        /// <summary>
        /// The URI of the HTTP proxy.  Note: HTTPS connections through a proxy are not yet supported.
        /// </summary>
        [DefaultValue(null)]
        public Uri ProxyURI
        {
            get { return m_proxyURI; }
            set { m_proxyURI = value; }
        }

        /// <summary>
        /// Username/password for the proxy.
        /// </summary>
        [DefaultValue(null)]
        public NetworkCredential ProxyCredentials
        {
            get { return m_proxyCredentials; }
            set { m_proxyCredentials = value; }
        }

        /// <summary>
        /// How long to wait before attempting to reconnect, in seconds.
        /// </summary>
        [DefaultValue(10.0f)]
        public float ReconnectTimeout
        {
            get { return m_connectRetrySec; }
            set { m_connectRetrySec = value; }
        }

        /// <summary>
        /// The maximum number of reconnect attempts before giving up.
        /// </summary>
        [DefaultValue(5)]
        public int MaxErrors
        {
            get { return m_maxErrors; }
            set { m_maxErrors = value; }
        }

        private void ProcessThread()
        {
            PendingRequest req;
            while (m_keepRunning)
            {
                lock (m_queue)
                {
                    req = null;
                    while (m_keepRunning && ((m_queue.Count == 0) || IsPending))
                    {
                        if (m_keepRunning && (m_addr != null) && !Connected)
                        {
                            Monitor.Wait(m_queue, (int)(m_connectRetrySec * 1000));
                            if (!m_keepRunning)
                                return;
                            Connect();
                        }
                        Monitor.Wait(m_queue);
                    }

                    if (!m_keepRunning)
                        return;

                    Debug.Assert(Connected);
                    Debug.Assert(m_queue.Count > 0);
                    Debug.Assert(!IsPending);

                    req = m_queue.First.Value;
                    m_queue.RemoveFirst();
                }
                if (req.Method == null)
                    Close();
                else
                    Send(req);
            }
        }

        /// <summary>
        /// Execute an HTTP request.
        /// </summary>
        /// <param name="method">The HTTP method verb.  E.g. "GET", "POST", etc.</param>
        /// <param name="URL">The URL to request.  MUST be for the same host as the first request.</param>
        /// <param name="body">Any data to post with the request</param>
        /// <param name="offset">The offset into body from which to start</param>
        /// <param name="len">The number of bytes to read from body, starting at offset</param>
        /// <param name="contentType">The MIME type of the supplied body</param>
        public void Execute(string method, Uri URL, byte[] body, int offset, int len, string contentType)
        {
            lock (m_queue)
            {
                PendingRequest req = new PendingRequest(method, URL, body, offset, len, contentType);
                if (m_host == null)
                    m_host = req.URI.Host;
                else if (m_host != req.URI.Host)
                    throw new InvalidOperationException("All requests must got to same host: " + m_host);
                m_queue.AddLast(req);
                Monitor.Pulse(m_queue);
            }
        }

        /// <summary>
        /// Is there an outstanding request that has not been responded to?
        /// </summary>
        public bool IsPending
        {
            get { return (m_current != null); }
        }

        /// <summary>
        /// Generally should not be used.
        /// </summary>
        /// <param name="uri"></param>
        internal void Connect(Uri uri)
        {
            m_keepRunning = true;

            if (Connected)
                return;

            m_ssl = (uri != null) && (uri.Scheme == "https");
            m_host = uri.Host;
            if (m_proxyURI != null)
            {
                // TODO: add CONNECT support here.  ShttpProxy?
                if (m_ssl)
                    throw new InvalidOperationException("Can't do SSL through proxies yet.");
                uri = m_proxyURI;
            }
            m_addr = new Address(uri.Host, uri.Port);
            Connect();
        }

        private void Connect()
        {
            if (!m_keepRunning)
                return;
            m_state = ParseState.START;
            m_sock = new AsyncSocket(null, this, m_ssl, false);
            m_sock.Connect(m_addr, m_host);
        }

        /// <summary>
        /// Shut down the socket, abandoning any outstainding requests
        /// </summary>
        public override void Close()
        {
            m_keepRunning = false;
            lock (m_queue)
            {
                m_queue.Clear();
                Monitor.Pulse(m_queue);
            }

            if (Connected)
                m_sock.Close();
            m_sock = null;
        }

        /// <summary>
        /// Close the socket after all pending requests are completed.
        /// </summary>
        public void EnqueueClose()
        {
            PendingRequest req = new PendingRequest(null, null, null, 0, 0, null);
            lock (m_queue)
            {
                m_queue.AddLast(req);
                Monitor.Pulse(m_queue);
            }
        }

        #region ISocketEventListener Members

        void ISocketEventListener.OnInit(BaseSocket newSock)
        {
            // This is the one listener event with a good socket, but it might not be the one that anyone expects.
            // The important thing is that if someone wants to set the TCP buffer sizes downstream, it 
            // will work.
            m_listener.OnInit(newSock);
        }

        ISocketEventListener ISocketEventListener.GetListener(BaseSocket newSock)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        bool ISocketEventListener.OnAccept(BaseSocket newsocket)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private static void WriteString(Stream s, string str)
        {
            byte[] buf = ENC.GetBytes(str);
            s.Write(buf, 0, buf.Length);
        }

        private void Send(PendingRequest req)
        {
            m_current = req;

            // Try to get it big enough that we don't have to allocate, without going overboard.
            MemoryStream ms = new MemoryStream(req.Length + 256);
            WriteString(ms, req.Method);
            WriteString(ms, " ");
            if (m_proxyURI == null)
                WriteString(ms, req.URI.PathAndQuery);
            else
                WriteString(ms, req.URI.ToString());
            ms.Write(SP_HTTP11_CRLF, 0, SP_HTTP11_CRLF.Length);

            WebHeaderCollection coll = new WebHeaderCollection();
            coll.Add(HttpRequestHeader.Host, req.URI.Host);
            if (req.ContentType != null)
                coll.Add(HttpRequestHeader.ContentType, req.ContentType);
            if (m_proxyCredentials != null)
            {
                byte[] creds = Encoding.ASCII.GetBytes(m_proxyCredentials.UserName + ":" + m_proxyCredentials.Password);
                coll.Add("Proxy-Authorization", "Basic " + Convert.ToBase64String(creds));
            }
            coll.Add(HttpRequestHeader.ContentLength, req.Length.ToString());

            byte[] headers = coll.ToByteArray();
            ms.Write(headers, 0, headers.Length);

            ms.Write(req.Body, req.Offset, req.Length);

            byte[] buf = ms.ToArray();

            m_sock.Write(buf);
            m_sock.RequestRead();
        }

        void ISocketEventListener.OnConnect(BaseSocket sock)
        {
            m_errorCount = 0;

            m_listener.OnConnect(null);
            lock (m_queue)
            {
                // push back.
                if (m_current != null)
                {
                    m_queue.AddFirst(m_current);
                    m_current = null;
                }
                Monitor.Pulse(m_queue);
            }
        }

        void ISocketEventListener.OnClose(BaseSocket sock)
        {
            m_sock = null;
            lock (m_queue)
            {
                Monitor.Pulse(m_queue);
            }
        }

        void ISocketEventListener.OnError(BaseSocket sock, Exception ex)
        {
            if (Interlocked.Increment(ref m_errorCount) > m_maxErrors)
            {
                m_keepRunning = false;
                m_listener.OnError(null, ex);
            }

            lock (m_queue)
            {
                Monitor.Pulse(m_queue);
            }
        }

        private bool ParseAt(byte[] buf, ref int i, int last, byte[] check, int checkoffset)
        {
            int start = i;
            for (int j = checkoffset; j < check.Length; j++)
            {
                if (i >= last)
                {
                    i = start;
                    return false;
                }

                if (buf[i++] != check[j])
                {
                    i = start;
                    return false;
                }
            }
            return true;
        }

        private string ParseTo(byte[] buf, ref int i, int last, byte[] check)
        {
            int start = i;

            // IndexOf should be in asm.
            int j;
            while (i < last)
            {
                j = Array.IndexOf(buf, check[0], i);
                if (j == -1)
                    return null;
                i = j;
                if (check.Length > 1)
                {
                    i++;
                    if (ParseAt(buf, ref i, last, check, 1))
                        return ENC.GetString(buf, start, j - start);
                }
                else
                    return ENC.GetString(buf, start, j - start);
            }
            return null;
        }

        private enum ParseState
        {
            START,
            RESPONSE,
            RESPONSE_TEXT,
            HEADER_NAME,
            HEADER_VALUE,
            BODY_START,
            BODY_CONTINUE
        }

        bool ISocketEventListener.OnRead(BaseSocket sock, byte[] buf, int offset, int length)
        {
            Debug.WriteLine("IN HTTP: " + ENC.GetString(buf, offset, length));
            int i = offset;
            string header = null;
            int last = offset + length;

            while (i < last)
            {
                // HTTP/1.1 200 OK
                // Header: value
                // Header: value
                //
                // Content
                switch (m_state)
                {
                    case ParseState.START:
                        if (!ParseAt(buf, ref i, last, HTTP11_SP, 0))
                            goto ERROR;
                        m_state = ParseState.RESPONSE;
                        break;
                    case ParseState.RESPONSE:
                        string code = ParseTo(buf, ref i, last, SPACE);
                        if (code == null)
                            goto ERROR;

                        if (code != "200")
                        {
                            Debug.WriteLine("Non-OK response from server (" + code + ").  STOP!");
                            goto ERROR;
                        }

                        try
                        {
                            // I know this can never fail.  it's here for when we
                            // implement redirects and the like.
                            m_current.Code = int.Parse(code);
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine("invalid response code");
                            goto ERROR;
                        }

                        m_state = ParseState.RESPONSE_TEXT;
                        break;
                    case ParseState.RESPONSE_TEXT:
                        m_current.ResponseText = ParseTo(buf, ref i, last, CRLF);
                        if (m_current.ResponseText == null)
                            goto ERROR;
                        m_state = ParseState.HEADER_NAME;
                        break;
                    case ParseState.HEADER_NAME:
                        if (ParseAt(buf, ref i, last, CRLF, 0))
                        {
                            m_state = ParseState.BODY_START;
                            break;
                        }
                        header = ParseTo(buf, ref i, last, COL_SP);
                        if (header == null)
                            goto ERROR;
                        m_state = ParseState.HEADER_VALUE;
                        break;
                    case ParseState.HEADER_VALUE:
                        string val = ParseTo(buf, ref i, last, CRLF);
                        if (val == null)
                            goto ERROR;
                        m_current.Headers.Add(header, val);
                        m_state = ParseState.HEADER_NAME;
                        break;
                    case ParseState.BODY_START:
                        // if we have the whole response, which is typical in XEP-124, then return it all at
                        // once, without creating a MemoryStream.
                        int len = m_current.ContentLength;
                        if (len == -1)
                            goto ERROR;
                        if (i + len <= last)
                        {
                            m_current = null;
                            m_state = ParseState.START;
                            if (!m_listener.OnRead(null, buf, i, len))
                            {
                                Close();
                                return false;
                            }
                            lock (m_queue)
                            {
                                Monitor.Pulse(m_queue);
                            }
                            return false;
                        }

                        // We got a partial response.  We're going to have to wait until OnRead is called
                        // again before we can pass a full response upstream.  Hold on to the pieces in a
                        // MemoryStream.
                        m_current.Response = new MemoryStream(len);
                        m_current.Response.Write(buf, i, last - i);
                        m_state = ParseState.BODY_CONTINUE;
                        return true;
                    case ParseState.BODY_CONTINUE:
                        m_current.Response.Write(buf, i, last - i);
                        if (m_current.Response.Length == m_current.Response.Capacity)
                        {
                            PendingRequest req = m_current;
                            m_current = null;
                            m_state = ParseState.START;

                            byte[] resp = req.Response.ToArray();
                            if (!m_listener.OnRead(null, resp, 0, resp.Length))
                            {
                                Close();
                                return false;
                            }

                            lock (m_queue)
                            {
                                Monitor.Pulse(m_queue);
                            }
                            return false;
                        }
                        return true;
                    default:
                        break;
                }
            }
            return true;

        ERROR:
            m_listener.OnError(null, new ProtocolViolationException("Error parsing HTTP response"));
            Close();
            return false;
        }

        void ISocketEventListener.OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
        {
            m_listener.OnWrite(null, buf, offset, length);
        }

        bool ISocketEventListener.OnInvalidCertificate(BaseSocket sock, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // TODO: pass up the chain
            return m_listener.OnInvalidCertificate(null, certificate, chain, sslPolicyErrors);
        }

        #endregion

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="backlog"></param>
        public override void Accept(Address addr, int backlog)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override void RequestAccept()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="addr"></param>
        public override void Connect(Address addr)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Are we currently connected? 
        /// </summary>
        public override bool Connected
        {
            get { return (m_sock != null) && (m_sock.Connected); }
        }

#if !NO_SSL
        /// <summary>
        /// Not implemented.
        /// </summary>
        public override void StartTLS()
        {
            throw new Exception("The method or operation is not implemented.");
        }
#endif

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override void StartCompression()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override void RequestRead()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        public override void Write(byte[] buf, int offset, int len)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
