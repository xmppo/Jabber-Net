/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;
using bedrock.util;

namespace bedrock.net
{
    /// <summary>
    /// XEP-0124 Error conditions
    /// </summary>
    [SVN(@"$Id$")]
    public class XEP124Exception : WebException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="reason"></param>
        public XEP124Exception(string reason)
            : base(reason)
        {
        }
    }

    /// <summary>
    /// Make a XEP-124 (http://www.xmpp.org/extensions/xep-0124.html) polling "connection" look like a socket.
    /// TODO: get rid of the PipeStream, if possible.
    /// </summary>
    [SVN(@"$Id$")]
    public class XEP124Socket : BaseSocket, IHttpSocket
    {
        private const string CONTENT_TYPE = "text/xml; charset=utf-8";
        private const string METHOD = "POST";

        private readonly RandomNumberGenerator s_rng = RNGCryptoServiceProvider.Create();

        private readonly Queue<WriteBuf> m_writeQ = new Queue<WriteBuf>();
        private readonly Object m_lock = new Object();
        private Thread m_thread = null;
        private readonly int m_hold = 5;
        private int m_wait = 60;
        private int m_requests = 0;
        private int m_polling = 10;
        private int m_maxPoll = 30;
        private int m_minPoll = 1;
        private string m_url = null;
        private string[] m_keys = null;
        private int m_numKeys = 512;
        private int m_curKey = 511;
        private bool m_running = false;
        private Int64 m_rid = -1;
        private string m_sid = null;
        private WebProxy m_proxy = null;
        private X509Certificate m_remote_cert = null;
        private bool m_StartStream = false;
        private string m_NS;
        private string m_authid;
        private int m_inactivity;

        ///<summary>
        /// Informs the socket that we are dealing with the start tag.
        ///</summary>
        public bool StartStream
        {
            get { return m_StartStream; }
            set { m_StartStream = value; }
        }

        ///<summary>
        /// Gets or sets the NS used by the stream:stream tag.
        ///</summary>
        public string NS
        {
            get { return m_NS; }
            set { m_NS = value; }
        }

        /// <summary>
        /// Create an instance
        /// </summary>
        /// <param name="listener"></param>
        public XEP124Socket(ISocketEventListener listener)
        {
            Debug.Assert(listener != null);
            m_listener = listener;

            ServicePointManager.DefaultConnectionLimit = 10;
        }

        /// <summary>
        /// Maximum time between polls, in seconds
        /// </summary>
        public int MaxPoll
        {
            get { return m_maxPoll; }
            set { m_maxPoll = value; }
        }

        /// <summary>
        /// Minimum time between polls, in seconds
        /// </summary>
        public int MinPoll
        {
            get { return m_minPoll; }
            set { m_minPoll = value; }
        }

        /// <summary>
        /// The URL to poll
        /// </summary>
        public string URL
        {
            get { return m_url; }
            set { m_url = value; }
        }

        /// <summary>
        /// The number of keys to generate at a time.  Higher numbers use more memory,
        /// and more CPU to generate keys, less often.  Defaults to 512.
        /// </summary>
        public int NumKeys
        {
            get { return m_numKeys; }
            set { m_numKeys = value; }
        }

        /// <summary>
        /// Proxy information.  My guess is if you leave this null, the IE proxy
        /// info may be used.  Not tested.
        /// </summary>
        public WebProxy Proxy
        {
            get { return m_proxy; }
            set { m_proxy = value; }
        }

        /// <summary>
        /// Accept a socket.  Not implemented.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="backlog"></param>
        public override void Accept(Address addr, int backlog)
        {
            throw new NotImplementedException("HTTP binding server not implemented yet");
        }

        /// <summary>
        /// Stop polling.
        /// </summary>
        public override void Close()
        {
            lock (m_lock)
            {
                m_terminate = true;
                Monitor.Pulse(m_lock);
            }
            m_listener.OnClose(this);
        }

        private Stream m_stream;
        private int m_connected = 0;
        private bool m_terminate = false;

        private bool WebConnected
        {
            get { return m_connected > 0; }
            set
            {
                if (value)
                    m_connected++;
                else
                    m_connected--;

                if (m_connected < 0)
                    m_connected = 0;
            }
        }

        /// <summary>
        /// Start polling
        /// </summary>
        /// <param name="addr"></param>
        public override void Connect(Address addr)
        {
            Debug.Assert(m_url != null);
            m_curKey = -1;

            if (m_thread == null)
            {
                m_running = true;
                m_thread = new Thread(PollThread);
                m_thread.IsBackground = false;
                m_thread.Name = "XEP124 Thread";
                m_thread.Start();
            }

            m_listener.OnConnect(this);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void RequestAccept()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start reading.
        /// </summary>
        public override void RequestRead()
        {
            if (!m_running)
                throw new InvalidOperationException("Call Connect() first");
        }

#if !NO_SSL

        /// <summary>
        /// Start TLS over this connection.  Not implemented.
        /// </summary>
        public override void StartTLS()
        {
            throw new NotImplementedException();
        }
#endif

        /// <summary>
        /// Start compression over this connection.  Not implemented.
        /// </summary>
        public override void StartCompression()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send bytes to the jabber server
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        public override void Write(byte[] buf, int offset, int len)
        {
            lock (m_lock)
            {
                m_writeQ.Enqueue(new WriteBuf(buf, offset, len));
                Monitor.PulseAll(m_lock);
            }
        }

        private void GenKeys()
        {
            byte[] seed = new byte[32];
            SHA1 sha = SHA1.Create();
            Encoding ENC = Encoding.ASCII; // All US-ASCII.  No need for UTF8.
            string prev;

            // K(n, seed) = Base64Encode(SHA1(K(n - 1, seed))), for n > 0
            // K(0, seed) = seed, which is client-determined

            s_rng.GetBytes(seed);
            prev = Convert.ToBase64String(seed);
            m_keys = new string[m_numKeys];
            for (int i = 0; i < m_numKeys; i++)
            {
                m_keys[i] = Convert.ToBase64String(sha.ComputeHash(ENC.GetBytes(prev)));
                prev = m_keys[i];
            }
            m_curKey = m_numKeys - 1;
        }

        private const string HTTPBIND_NS = "http://jabber.org/protocol/httpbind";

        /// <summary>
        /// Keep polling until
        /// </summary>
        private void PollThread()
        {
            lock (m_lock)
            {
                while (m_running)
                {
                    while (m_writeQ.Count == 0 && WebConnected && !m_terminate)
                    {
                        Monitor.Wait(m_lock, m_polling * 1000);
                    }

                    string body = CreateOpenBodyTag();

                    HttpWebRequest m_request = (HttpWebRequest)WebRequest.Create(m_url);
                    m_request.ContentType = CONTENT_TYPE;
                    m_request.Method = METHOD;
                    m_request.KeepAlive = true;
                    m_request.ContentType = "text/xml; charset=utf-8";
                    //req.Timeout = m_wait * 1000;

                    m_request.CachePolicy =
                        new System.Net.Cache.HttpRequestCachePolicy(
                            System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);

                    if (m_proxy != null)
                        m_request.Proxy = m_proxy;

                    using (m_stream = m_request.GetRequestStream())
                    {
                        WriteBuf b = new WriteBuf(body);

                        try
                        {
                            m_stream.Write(b.buf, b.offset, b.len);
                        }
                        catch (WebException ex)
                        {
                            Debug.WriteLine(ex);
                            Debug.WriteLine(ex.Status);
                            throw;
                        }

                        foreach (WriteBuf buf in m_writeQ)
                        {
                            m_stream.Write(buf.buf, buf.offset, buf.len);
                            m_listener.OnWrite(this, buf.buf, buf.offset, buf.len);
                        }
                        m_writeQ.Clear();

                        b = new WriteBuf("</body>");
                        m_stream.Write(b.buf, b.offset, b.len);
                    }

                    WebConnected = true;
                    ThreadPool.QueueUserWorkItem(GetResponse, m_request);
                }
            }
        }

        private string CreateOpenBodyTag()
        {
            StringBuilder body = new StringBuilder();
            body.AppendFormat("<body xmlns='{0}' ", HTTPBIND_NS);

            lock (this)
            {
                if (m_rid == -1)
                {
                    Random rnd = new Random();
                    m_rid = rnd.Next();
                    Console.WriteLine("new RID: {0}", m_rid);

                    Uri uri = new Uri(m_url);

                    body.AppendFormat("content='{0}' xmlns:xmpp='urn:xmpp:xbosh'",
                        CONTENT_TYPE);
                    body.AppendFormat(" to='{0}' wait='{1}' hold='{2}'",
                        uri.Host, m_wait, m_hold);
                    body.Append(" xml:lang='en' xmpp:version='1.0'");
                }
                else
                {
                    m_rid++;

                    if (m_sid != null)
                    {
                        body.AppendFormat(" sid='{0}'", m_sid);
                    }
                }
                body.AppendFormat(" rid='{0}'", m_rid);
            }
            if (m_terminate)
            {
                body.Append(" type='terminate' ");
                m_running = false;
            }

            body.Append(">");

            if (m_terminate)
            {
                // Add the unavailable presence.
                body.Append("<presence type='unavailable' xmlns='jabber:client'/>");
            }

            return body.ToString();
        }

        private void GetResponse(object parameter)
        {
            try
            {
                HttpWebRequest m_request = parameter as HttpWebRequest;

                //if (m_request != null)
                //    m_request.BeginGetResponse(GotResponse, m_request);

                if (m_request != null)
                {
                    using (HttpWebResponse response =
                        (HttpWebResponse)m_request.GetResponse())
                    {
                        ReadData(response);
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.KeepAliveFailure)
                    m_listener.OnError(this, ex);
            }
            finally
            {
                lock (m_lock)
                {
                    WebConnected = false;

                    if (!WebConnected)
                        Monitor.Pulse(m_lock);
                }
            }
        }

        /// <summary>
        /// Descripton, including URL.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "XEP-0124 socket: " + m_url;
        }


        private void GotResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;

            try
            {
                using (HttpWebResponse response =
                    (HttpWebResponse)request.EndGetResponse(result))
                {
                    ReadData(response);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.KeepAliveFailure)
                    m_listener.OnError(this, ex);

                return;
            }
        }

        private void ReadData(HttpWebResponse response)
        {
            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    m_listener.OnError(this, new WebException("Invalid HTTP return code: " + response.StatusCode));
                    return;
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string xml = reader.ReadToEnd();
                    reader.Close();

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    XmlElement bodyElement = null;
                    if (doc.ChildNodes.Count == 1)
                    {
                        bodyElement = doc.ChildNodes[0] as XmlElement;
                    }

                    if (bodyElement == null)
                    {
                        return;
                    }

                    string content = "";
                    foreach (XmlElement node in bodyElement.ChildNodes)
                    {
                        content += node.OuterXml;
                    }
                    content = content.Replace("xmlns=\"" + HTTPBIND_NS + "\"", "");

                    if (doc.DocumentElement.Attributes["sid"] != null)
                        m_sid = doc.DocumentElement.Attributes["sid"].Value;

                    if (doc.DocumentElement.Attributes["authid"] != null)
                        m_authid = doc.DocumentElement.Attributes["authid"].Value;

                    if (doc.DocumentElement.Attributes["requests"] != null)
                        m_requests = int.Parse(doc.DocumentElement.Attributes["requests"].Value);

                    if (doc.DocumentElement.Attributes["wait"] != null)
                        m_wait = int.Parse(doc.DocumentElement.Attributes["wait"].Value);

                    if (doc.DocumentElement.Attributes["polling"] != null)
                        m_polling = int.Parse(doc.DocumentElement.Attributes["polling"].Value);

                    if (doc.DocumentElement.Attributes["inactivity"] != null)
                        m_inactivity = int.Parse(doc.DocumentElement.Attributes["inactivity"].Value);

                    if (content != "")
                    {
                        WriteBuf buf;
                        if (StartStream)
                        {
                            StartStream = false;
                            jabber.protocol.stream.Stream stream =
                                new jabber.protocol.stream.Stream(new XmlDocument(), NS);
                            stream.Version = "1.0";
                            stream.ID = bodyElement.GetAttribute("authid");

                            buf = new WriteBuf(stream.StartTag());
                            if (!m_listener.OnRead(this, buf.buf, 0, buf.len))
                            {
                                Close();
                                return;
                            }
                        }

                        buf = new WriteBuf(content);

                        if (!m_listener.OnRead(this, buf.buf, 0, buf.len))
                        {
                            Close();
                            return;
                        }
                    }
                    //buf = new WriteBuf(xml);
                    //if (!m_listener.OnRead(this, buf.buf, 0, buf.len))
                    //{
                    //    Close();
                    //    return;
                    //}
                }
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.KeepAliveFailure)
                    m_listener.OnError(this, ex);
            }
        }

        /// <summary>
        /// Are we connected?
        /// </summary>
        public override bool Connected
        {
            get
            { return m_running; }
        }

        /// <summary>
        /// The certificate from the server.
        /// </summary>
        public X509Certificate RemoteCertificate
        {
            get { return m_remote_cert; }
            set { m_remote_cert = value; }
        }

        private class WriteBuf
        {
            public readonly byte[] buf;
            public readonly int offset;
            public readonly int len;

            public WriteBuf(byte[] buf, int offset, int len)
            {
                this.buf = buf;
                this.offset = offset;
                this.len = len;
            }

            public WriteBuf(string b)
            {
                buf = Encoding.UTF8.GetBytes(b);
                offset = 0;
                len = buf.Length;
            }

            public static WriteBuf operator +(WriteBuf a, WriteBuf b)
            {
                int size = a.len + b.len;

                byte[] newArray = new byte[size];
                Array.Copy(a.buf, a.offset, newArray, 0, a.len);
                Array.Copy(b.buf, b.offset, newArray, a.len, b.len);

                return new WriteBuf(newArray, 0, size);
            }
        }
    }
}
