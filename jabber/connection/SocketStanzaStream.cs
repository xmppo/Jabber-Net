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
using System.Diagnostics;
using System.Threading;
using System.Xml;

using bedrock.net;
using bedrock.util;
using jabber.protocol;

namespace jabber.connection
{
    /// <summary>
    /// Contains the types of proxies Jabber-Net supports.  This is only for socket connections.
    /// </summary>
    [SVN(@"$Id$")]
    public enum ProxyType
    {
        /// <summary>
        /// no proxy
        /// </summary>
        None,
        /// <summary>
        /// SOCKS4 as in http://archive.socks.permeo.com/protocol/socks4.protocol
        /// </summary>
        Socks4,
        /// <summary>
        /// SOCKS5 as in http://archive.socks.permeo.com/rfc/rfc1928.txt
        /// </summary>
        Socks5,
        /// <summary>
        /// HTTP CONNECT
        /// </summary>
        HTTP,
    }


    /// <summary>
    /// "Standard" XMPP socket for outbound connections.
    /// </summary>
    [SVN(@"$Id$")]
    public class SocketStanzaStream : StanzaStream, ISocketEventListener
    {
        private AsynchElementStream m_elements = null;
        private BaseSocket          m_sock     = null;
        private BaseSocket          m_accept   = null;
        private Timer               m_timer    = null;

        /// <summary>
        /// Create a new one.
        /// </summary>
        /// <param name="listener"></param>
        internal SocketStanzaStream(IStanzaEventListener listener) : base(listener)
        {
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Determines if the socket is connected.
        /// </summary>
        public override bool Connected
        {
            get { return ASock.Connected;  }
        }

        /// <summary>
        /// Determines whether or not Jabber-Net supports StartTLS.
        /// </summary>
        public override bool SupportsTLS
        {
            get
            {
#if NO_SSL
                return false;
#else
                return true;
#endif
            }
        }

        /// <summary>
        /// Determines whether or not Jabber-Net supports compression.
        /// </summary>
        public override bool SupportsCompression
        {
            get
            {
                try
                {
                    return bedrock.io.ZlibStream.Supported;
                }
                catch
                {
                    Debug.WriteLine("WARNING: zlib.net.dll missing!");
                    return false;
                }
            }
        }

        private AsyncSocket ASock
        {
            get
            {
                if (m_sock is ProxySocket)
                    return ((ProxySocket)m_sock).Socket as AsyncSocket;
                else
                    return m_sock as AsyncSocket;
            }
        }

        /// <summary>
        /// Initializes the element stream.  This is the place to add factories.
        /// </summary>
        public override void InitializeStream()
        {
            bool first = (m_elements == null);
            m_elements = new AsynchElementStream();
            m_elements.OnDocumentStart += new ProtocolHandler(m_elements_OnDocumentStart);
            m_elements.OnDocumentEnd += new bedrock.ObjectHandler(m_elements_OnDocumentEnd);
            m_elements.OnElement += new ProtocolHandler(m_elements_OnElement);
            m_elements.OnError += new bedrock.ExceptionHandler(m_elements_OnError);

            m_listener.StreamInit(m_elements);

            Debug.Assert(this.Connected);
            if (first)
                m_sock.RequestRead();

        }

        /// <summary>
        /// Connects to the XMPP server.
        /// </summary>
        public override void Connect()
        {
            m_elements = null;
            int port = (int)m_listener[Options.PORT];
            Debug.Assert(port > 0);
            //m_sslOn = m_ssl;

            ProxySocket proxy = null;
            ProxyType pt = (ProxyType)m_listener[Options.PROXY_TYPE];
            switch (pt)
            {
            case ProxyType.Socks4:
                proxy = new Socks4Proxy(this);
                break;

            case ProxyType.Socks5:
                proxy = new Socks5Proxy(this);
                break;

            case ProxyType.HTTP:
                proxy = new ShttpProxy(this);
                break;

                /*
            case ProxyType.HTTP_Polling:
                XEP25Socket j25s = new XEP25Socket(this);
                if (m_ProxyHost != null)
                {
                    System.Net.WebProxy wp = new System.Net.WebProxy();
                    wp.Address = new Uri("http://" + m_ProxyHost + ":" + m_ProxyPort);
                    if (m_ProxyUsername != null)
                    {
                        wp.Credentials = new System.Net.NetworkCredential(m_ProxyUsername, m_ProxyPassword);
                    }
                    j25s.Proxy = wp;
                }
                j25s.URL = m_server;
                m_sock = j25s;
                break;
                */
            case ProxyType.None:
                m_sock = new AsyncSocket(null, this, (bool)m_listener[Options.SSL], false);

                ((AsyncSocket)m_sock).LocalCertificate = m_listener[Options.LOCAL_CERTIFICATE] as
                    System.Security.Cryptography.X509Certificates.X509Certificate2;

                ((AsyncSocket)m_sock).CertificateGui = (bool)m_listener[Options.CERTIFICATE_GUI];
                break;

            default:
                throw new ArgumentException("no handler for proxy type: " + pt, "ProxyType");
            }

            if (proxy != null)
            {
                proxy.Socket = new AsyncSocket(null, proxy, (bool)m_listener[Options.SSL], false);
                ((AsyncSocket)proxy.Socket).LocalCertificate = m_listener[Options.LOCAL_CERTIFICATE] as
                    System.Security.Cryptography.X509Certificates.X509Certificate2;

                proxy.Host = m_listener[Options.PROXY_HOST] as string;
                proxy.Port = (int)m_listener[Options.PROXY_PORT];
                proxy.Username = m_listener[Options.PROXY_USER] as string;
                proxy.Password = m_listener[Options.PROXY_PW] as string;
                m_sock = proxy;
            }

            string to = (string)m_listener[Options.TO];
            Debug.Assert(to != null);

            string host = (string)m_listener[Options.NETWORK_HOST];
            if ((host == null) || (host == ""))
            {
#if __MonoCS__
                host = to;
#else
                try
                {
                    Address.LookupSRV((string)m_listener[Options.SRV_PREFIX], to, ref host, ref port);
                }
                catch
                {
                    Debug.WriteLine("WARNING: netlib.Dns.dll missing");
                    host = to;
                }
#endif
            }

            Address addr = new Address(host, port);
            m_sock.Connect(addr, (string)m_listener[Options.SERVER_ID]);
        }

        /// <summary>
        /// Listens for an inbound connection.
        /// </summary>
        public override void Accept()
        {
            if (m_accept == null)
            {
                m_accept = new AsyncSocket(null, this, (bool)m_listener[Options.SSL], false);
                ((AsyncSocket)m_accept).LocalCertificate = m_listener[Options.LOCAL_CERTIFICATE] as
                    System.Security.Cryptography.X509Certificates.X509Certificate2;

                Address addr = new Address((string)m_listener[Options.NETWORK_HOST],
                    (int)m_listener[Options.PORT]);

                m_accept.Accept(addr);
            }
            m_accept.RequestAccept();
        }

        /// <summary>
        /// Determines if the method Accept() can be called now.
        /// </summary>
        public override bool Acceptable
        {
            get
            {
                return (m_accept != null);
            }
        }

        /// <summary>
        /// Writes the given string to the socket after UTF-8 encoding.
        /// </summary>
        /// <param name="str">String to write out.</param>
        public override void Write(string str)
        {
            int keep = (int)m_listener[Options.CURRENT_KEEP_ALIVE];
            if (keep > 0)
                m_timer.Change(keep, keep);
            m_sock.Write(ENC.GetBytes(str));
        }

        /// <summary>
        /// Writes a stream:stream.
        /// </summary>
        /// <param name="stream">Stream containing the stream:stream packet to send.</param>
        public override void WriteStartTag(jabber.protocol.stream.Stream stream)
        {
            Write(stream.StartTag());
        }

        /// <summary>
        /// Writes a full stanza.
        /// </summary>
        /// <param name="elem">XML stanza to write.</param>
        public override void Write(XmlElement elem)
        {
            if (m_sock is IElementSocket)
                ((IElementSocket)m_sock).Write(elem);
            else
                Write(elem.OuterXml);
        }

        /// <summary>
        /// Closes the session with the XMPP server.
        /// </summary>
        /// <param name="clean">Sends the close stanza to the XMPP server if true.</param>
        public override void Close(bool clean)
        {
            // Note: socket should still be connected, excepts for races.  Revist.
            if (clean)
                Write("</stream:stream>");
            m_sock.Close();
        }

        private void DoKeepAlive(object state)
        {
            if ((m_sock != null) && this.Connected && ((int)m_listener[Options.CURRENT_KEEP_ALIVE] > 0))
                m_sock.Write(new byte[] { 32 });
        }

#if !NO_SSL
        /// <summary>
        /// Negotiates Start-TLS with the other endpoint.
        /// </summary>
        public override void StartTLS()
        {
            m_sock.StartTLS();
            AsyncSocket s = ASock;

            Debug.Assert(s != null);
            m_listener[Options.REMOTE_CERTIFICATE] = s.RemoteCertificate;
        }
#endif

        /// <summary>
        /// Starts compressing outgoing traffic for this connection with the XMPP server.
        /// </summary>
        public override void StartCompression()
        {
            m_sock.StartCompression();
        }

        #region ElementStream handlers
        private void m_elements_OnDocumentStart(object sender, XmlElement rp)
        {
            m_listener.DocumentStarted(rp);
        }

        private void m_elements_OnDocumentEnd(object sender)
        {
            m_listener.DocumentEnded();
        }

        private void m_elements_OnElement(object sender, XmlElement rp)
        {
            m_listener.StanzaReceived(rp);
        }

        private void m_elements_OnError(object sender, Exception ex)
        {
            // XML parse error.
            m_timer.Change(Timeout.Infinite, Timeout.Infinite);
            m_listener.Errored(ex);
        }
        #endregion

        #region ISocketEventListener Members

        void ISocketEventListener.OnInit(BaseSocket newSock)
        {
        }

        ISocketEventListener ISocketEventListener.GetListener(BaseSocket newSock)
        {
            return this;
        }

        bool ISocketEventListener.OnAccept(BaseSocket newsocket)
        {
            m_sock = newsocket;
            InitializeStream();
            m_listener.Accepted();

            // Don't accept any more connections until this one closes
            // yes, it will look like we're still listening until the old sock is free'd by GC.
            // don't want OnClose() to fire, though, so we can't close the previous sock.
            return false;
        }

        void ISocketEventListener.OnConnect(BaseSocket sock)
        {
#if !NO_SSL
            if ((bool)m_listener[Options.SSL])
            {
                AsyncSocket s = sock as AsyncSocket;
                m_listener[Options.REMOTE_CERTIFICATE] = s.RemoteCertificate;
            }
#endif
            m_listener.Connected();
        }

        void ISocketEventListener.OnClose(BaseSocket sock)
        {
            //System.Windows.Forms.Application.DoEvents();
            //System.Threading.Thread.Sleep(1000);
            m_listener[Options.REMOTE_CERTIFICATE] = null;
            //m_elements = null;
            m_timer.Change(Timeout.Infinite, Timeout.Infinite);
            m_listener.Closed();
        }

        void ISocketEventListener.OnError(BaseSocket sock, Exception ex)
        {
            m_listener[Options.REMOTE_CERTIFICATE] = null;
            //m_elements = null;
            m_timer.Change(Timeout.Infinite, Timeout.Infinite);
            m_listener.Errored(ex);
        }

        bool ISocketEventListener.OnRead(BaseSocket sock, byte[] buf, int offset, int length)
        {
            int tim = (int)m_listener[Options.KEEP_ALIVE];
            if (tim > 0)
                m_timer.Change(tim, tim);

            m_listener.BytesRead(buf, offset, length);
            try
            {
                m_elements.Push(buf, offset, length);
            }
            catch (Exception e)
            {
                ((ISocketEventListener)this).OnError(sock, e);
                sock.Close();
                return false;
            }
            return true;
        }

        void ISocketEventListener.OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
        {
            int tim = (int)m_listener[Options.KEEP_ALIVE];
            if (tim > 0)
                m_timer.Change(tim, tim);

            m_listener.BytesWritten(buf, offset, length);
        }

        /// <summary>
        /// An invalid peer certificate was sent during SSL/TLS neogtiation.
        /// </summary>
        /// <param name="sock">The socket that experienced the error</param>
        /// <param name="certificate">The bad certificate</param>
        /// <param name="chain">The chain of CAs for the cert</param>
        /// <param name="sslPolicyErrors">A bitfield for the erorrs in the certificate.</param>
        /// <returns>True if the cert should be accepted anyway.</returns>
        bool ISocketEventListener.OnInvalidCertificate(BaseSocket sock,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return m_listener.OnInvalidCertificate(sock, certificate, chain, sslPolicyErrors);
        }
        #endregion
    }
}
