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
 * Jabber-Net is licensed under the LGPL.
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

using jabber.protocol.stream;
using jabber.connection;
using jabber.protocol;

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
    public class XEP124Socket : BaseSocket, IHttpSocket, IElementSocket, ISocketEventListener
    {
        /// <summary>
        /// Text encoding.  Always UTF-8 for XMPP.
        /// </summary>
        protected static readonly Encoding ENC = Encoding.UTF8;

        private const string CONTENT_TYPE = "text/xml; charset=utf-8";
        private const string METHOD = "POST";

        private readonly Object m_lock = new Object();

        private readonly int m_hold = 5;
        private int m_wait = 60;
        private int m_maxPoll = 30;
        private int m_minPoll = 1;
        private Uri m_uri = null;
        private bool m_running = false;
        private long m_rid = -1L;
        private string m_sid = null;
        private string m_authID = null;
        private X509Certificate m_remote_cert = null;
        private bool m_StartStream = false;
        private string m_NS;
        private string m_lang = System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag;
        private XmlDocument m_doc = new XmlDocument();

        private Uri m_proxyURI = null;
        private NetworkCredential m_proxyCredentials = null;

        private HttpSocket m_sockA = null;
        private HttpSocket m_sockB = null;
        private HttpSocket m_lastSock = null;

        /// <summary>
        /// Create an instance
        /// </summary>
        /// <param name="listener"></param>
        public XEP124Socket(ISocketEventListener listener) : base(listener)
        {
        }

        /// <summary>
        /// The xml:lang for all requests.  Defaults to the current culture's language tag.
        /// </summary>
        public string Lang
        {
            get { return m_lang; }
            set { m_lang = value; }
        }

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
            get { return m_uri.ToString(); }
            set { m_uri = new Uri(value); }
        }

        /// <summary>
        /// The URI of the HTTP proxy.  Note: HTTPS connections through a proxy are not yet supported.
        /// </summary>
        public Uri ProxyURI
        {
            get { return m_proxyURI; }
            set { m_proxyURI = value; }
        }

        /// <summary>
        /// Username/password for the proxy.
        /// </summary>
        public NetworkCredential ProxyCredentials
        {
            get { return m_proxyCredentials; }
            set { m_proxyCredentials = value; }
        }

        /// <summary>
        /// Accept a socket.  Not implemented.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="backlog"></param>
        public override void Accept(Address addr, int backlog)
        {
            throw new NotImplementedException("HTTP binding server not implemented");
        }

        private HttpSocket GetSocket()
        {
            lock (m_lock)
            {
                if (m_lastSock == m_sockA)
                {
                    m_lastSock = m_sockB;
                }
                else
                {
                    m_lastSock = m_sockA;
                }
                return m_lastSock;
            }
        }

        /// <summary>
        /// Stop polling.
        /// </summary>
        public override void Close()
        {
            Body body = CreateOpenBodyTag();
            body.Type = BodyType.terminate;

            byte[] buf = ENC.GetBytes(body.OuterXml);
            //m_listener.OnWrite(this, buf, 0, buf.Length);

            GetSocket().Execute(METHOD, m_uri, buf, 0, buf.Length, CONTENT_TYPE);
            
            m_sockA.EnqueueClose();
            m_sockB.EnqueueClose();

            lock (m_lock)
            {
                m_running = false;
                m_sockA = m_sockB = m_lastSock = null;
            }
            m_listener.OnClose(this);
        }

        /// <summary>
        /// Start polling
        /// </summary>
        /// <param name="addr">Ignored in this case.  Set URL.</param>
        public override void Connect(Address addr)
        {
            Debug.Assert(m_uri != null);

            m_rid = -1L;
            m_lastSock = null;
            m_running = false;

            // Create new ones each time, in case the URL has changed or something.
            m_sockA = new HttpSocket(this);
            m_sockB = new HttpSocket(this);

            m_sockA.ProxyURI = m_sockB.ProxyURI = m_proxyURI;
            m_sockA.ProxyCredentials = m_sockB.ProxyCredentials = m_proxyCredentials;

            m_sockA.Connect(m_uri);
            m_sockB.Connect(m_uri);
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
            // shutdown race, likely.
            if (!m_running)
                //throw new InvalidOperationException("Call Connect() first");
                return;
            if (m_sockA.IsPending || m_sockB.IsPending)
            {
                Debug.WriteLine("Skipping request, already pending");
                return;
            }

            Write(null);
        }

        /// <summary>
        /// Start TLS over this connection.  Not implemented.
        /// </summary>
        public override void StartTLS()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start compression over this connection.  Not implemented.
        /// </summary>
        public override void StartCompression()
        {
            throw new NotImplementedException();
        }

        private void FakeTimer(object state)
        {
            // HACK: stream restart is null for older versions of XEP-124.
            if (!FakeReceivedStream())
                return;

            Features f = new Features(m_doc);
            f.AddChild(new Bind(m_doc));
            f.AddChild(new Session(m_doc));
            byte[] p = ENC.GetBytes(f.OuterXml);
            if (!m_listener.OnRead(this, p, 0, p.Length))
            {
                Close();
                return;
            }
        }

        /// <summary>
        /// Send bytes to the jabber server
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        public override void Write(byte[] buf, int offset, int len)
        {
            if (buf != null)
                throw new NotImplementedException("Call Write(XmlElement)");

            // HACK
            byte[] p = ENC.GetBytes("Psuedo-stream body");
            m_listener.OnWrite(this, p, 0, p.Length);
            if (m_sid == null)
            {
                StartStream = true;
                return;
            }

            // HACK: upper levels need this to come in after the
            // return from write. Double-hack: hope this doesn't get
            // gc's before the timer fires.... :)
            
            //Timer t =
            new Timer(new TimerCallback(FakeTimer), null, 0, Timeout.Infinite);
        }

        /// <summary>
        /// Write an XML element to the socket.
        /// In this case, the element is queued, so that the write
        /// thread can pick it up.
        /// </summary>
        /// <param name="elem"></param>
        public void Write(XmlElement elem)
        {
            Body body = CreateOpenBodyTag();
            if (elem != null)
                body.AddChild(elem);

            byte[] buf = ENC.GetBytes(body.OuterXml);
            //m_listener.OnWrite(this, buf, 0, buf.Length);

            GetSocket().Execute(METHOD, m_uri, buf, 0, buf.Length, CONTENT_TYPE);
        }

        private Body CreateOpenBodyTag()
        {
            Body body = new Body(m_doc);

            long r = -1L;
            if (m_rid == -1L)
            {
                Random rnd = new Random();
                r = m_rid = (long)rnd.Next();
                body.Content = CONTENT_TYPE;
                
                body.To = m_hostid;
                body.Wait = m_wait;
                body.Hold = m_hold;
                body.Lang = m_lang;
            }
            else
            {
                r = Interlocked.Increment(ref m_rid);
                
                body.SID = m_sid;
            }
            body.RID = r;
            
            return body;
        }

        /// <summary>
        /// Descripton, including URL.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "XEP-0124 socket: " + m_uri.ToString();
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

        #region ISocketEventListener Members

        void ISocketEventListener.OnInit(BaseSocket newSock)
        {
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

        void ISocketEventListener.OnConnect(BaseSocket sock)
        {
            lock (m_lock)
            {
                if (!m_running &&
                    (m_sockA != null) && m_sockA.Connected &&
                    (m_sockB != null) && m_sockB.Connected)
                {
                    m_running = true;
                    m_lastSock = m_sockB;
                    m_listener.OnConnect(this);
                }
            }            
        }

        void ISocketEventListener.OnClose(BaseSocket sock)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void ISocketEventListener.OnError(BaseSocket sock, Exception ex)
        {
            // shutdown race.
            if (!m_running)
                return;

            m_listener.OnError(this, ex);
        }

        private bool FakeReceivedStream()
        {
            jabber.protocol.stream.Stream stream =
                new jabber.protocol.stream.Stream(m_doc, NS);
            stream.Version = "1.0";
            stream.ID = m_authID;

            byte[] sbuf = ENC.GetBytes(stream.StartTag());
            if (!m_listener.OnRead(this, sbuf, 0, sbuf.Length))
            {
                Close();
                return false;
            }
            return true;
        }

        bool ISocketEventListener.OnRead(BaseSocket sock, byte[] buf, int offset, int length)
        {
            if (!m_running)
            {
                Debug.WriteLine("shutting down.  extra bytes received.");
                return false;
            }

            // Parse out the first start tag or empty element, which will be
            // <body/>.
            xpnet.UTF8Encoding e = new xpnet.UTF8Encoding();
            xpnet.ContentToken ct = new xpnet.ContentToken();
            xpnet.TOK tok = e.tokenizeContent(buf, offset, offset + length, ct);

            if ((tok != xpnet.TOK.START_TAG_WITH_ATTS) &&
                (tok != xpnet.TOK.EMPTY_ELEMENT_WITH_ATTS))
            {
                m_listener.OnError(this, new ProtocolViolationException("Invalid HTTP binding XML.  Token type: " + tok.ToString()));
                return false;
            }

            string name = ENC.GetString(buf,
                                        offset + e.MinBytesPerChar,
                                        ct.NameEnd - offset - e.MinBytesPerChar);
            Debug.Assert(name == "body");
            Body b = new Body(m_doc);
            string val;
            int start;
            int end;
            for (int i = 0; i < ct.getAttributeSpecifiedCount(); i++)
            {
                start = ct.getAttributeNameStart(i);
                end = ct.getAttributeNameEnd(i);
                name = ENC.GetString(buf, start, end - start);

                start = ct.getAttributeValueStart(i);
                end = ct.getAttributeValueEnd(i);
                val = ENC.GetString(buf, start, end - start);

                if (!name.StartsWith("xmlns"))
                    b.SetAttribute(name, val);
            }

            if (b.SID != null)
                m_sid = b.SID;

            if (m_sid == null)
            {
                m_listener.OnError(this, new ProtocolViolationException("Invalid HTTP binding.  No SID."));
                return false;
            }

            if (b.Wait != -1)
                m_wait = b.Wait;

            if (StartStream)
            {
                StartStream = false;
                m_authID = b.AuthID;
                if (!FakeReceivedStream())
                    return false;
            }

            lock (m_lock)
            {
                if (!m_running)
                    return false;
                if (b.Type == BodyType.terminate)
                {
                    m_running = false;
                    Error err = new Error(m_doc);
                    err.AppendChild(m_doc.CreateElement(b.GetAttribute("condition"), URI.STREAM_ERROR));
                    byte[] sbuf = ENC.GetBytes(err.OuterXml);
                    m_listener.OnRead(this, sbuf, 0, sbuf.Length);
                    sbuf = ENC.GetBytes("</stream:stream>");
                    m_listener.OnRead(this, sbuf, 0, sbuf.Length);
                    Close();
                    return false;
                }
            }


            if (tok == xpnet.TOK.START_TAG_WITH_ATTS)
            {
                // len(</body>) = 7
                start = ct.TokenEnd;
                if (m_listener.OnRead(this, buf, start, offset + length - start - 7))
                    RequestRead();
            }
            else
                RequestRead();
            return true;
        }

        void ISocketEventListener.OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
        {
            m_listener.OnWrite(this, buf, offset, length);
        }

        bool ISocketEventListener.OnInvalidCertificate(BaseSocket sock, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return m_listener.OnInvalidCertificate(this, certificate, chain, sslPolicyErrors);
        }

        #endregion
    }
}
