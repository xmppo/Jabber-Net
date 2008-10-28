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
using System.Diagnostics;
using System.Xml;
using bedrock.net;
using bedrock.util;
using jabber.protocol;

namespace jabber.connection
{
    /// <summary>
    /// Manages the HTTP Polling XMPP stream.
    /// </summary>
    [SVN(@"$Id$")]
    public abstract class HttpStanzaStream : StanzaStream, ISocketEventListener
    {
        private AsynchElementStream m_elements = null;
        private BaseSocket m_sock = null;

        /// <summary>
        /// The underlying socket
        /// </summary>
        protected BaseSocket Socket
        {
            get { return m_sock; }
        }

        /// <summary>
        /// Create a new one.
        /// </summary>
        /// <param name="listener"></param>
        internal HttpStanzaStream(IStanzaEventListener listener)
            : base(listener)
        {
        }

        /// <summary>
        /// Determines whether or not the client is connected to the XMPP server.
        /// </summary>
        public override bool Connected
        {
            get { return m_sock.Connected; }
        }

        /// <summary>
        /// Supports Start-TLS if SSL is enabled.
        /// </summary>
        public override bool SupportsTLS
        {
            get { return false; }
        }

        /// <summary>
        /// Sets up the element stream.  This is the place to add factories.
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
        /// Connects the outbound socket.
        /// </summary>
        public override void Connect()
        {
            int port = (int)m_listener[Options.PORT];
            Debug.Assert(port > 0);

            m_sock = CreateSocket();


            string to = (string)m_listener[Options.TO];
            Debug.Assert(to != null);

            string host = (string)m_listener[Options.NETWORK_HOST];
            if ((host == null) || (host == ""))
                host = to;

            string url = (string)m_listener[Options.POLL_URL];
            if ((url == null) || (url == ""))
            {
#if !__MonoCS__
                url = Address.LookupTXT("_xmppconnect.", to, "_xmpp-client-xbosh");
                if (url == null)
#endif
                    throw new ArgumentNullException("URL not found in DNS, and not specified", "URL");
            }
            ((IHttpSocket)m_sock).URL = url;

            //Address addr = new Address(host, port);
            m_sock.Connect(null, (string)m_listener[Options.SERVER_ID]);
        }

        /// <summary>
        /// Create a socket of the correct type.
        /// </summary>
        /// <returns></returns>
        protected abstract BaseSocket CreateSocket();

        /// <summary>
        /// Listens for an inbound connection.
        /// </summary>
        public override void Accept()
        {
            AsyncSocket s = new AsyncSocket(null, this, (bool)m_listener[Options.SSL], false);
            s.LocalCertificate = m_listener[Options.LOCAL_CERTIFICATE] as
                System.Security.Cryptography.X509Certificates.X509Certificate2;

            m_sock = s;
            m_sock.Accept(new Address((int)m_listener[Options.PORT]));
            m_sock.RequestAccept();
        }

        /// <summary>
        /// Writes a string to the stream.
        /// </summary>
        /// <param name="str">
        /// The string to write; this will be transcoded to UTF-8.
        /// </param>
        public override void Write(string str)
        {
            //int keep = (int)m_listener[Options.KEEP_ALIVE];
            m_sock.Write(ENC.GetBytes(str));
        }

        /// <summary>
        /// Writes a stream:stream start tag.
        /// </summary>
        /// <param name="stream">Stream containing the stream:stream packet to send.</param>
        public override void WriteStartTag(jabber.protocol.stream.Stream stream)
        {
            Write(stream.StartTag());
        }

        /// <summary>
        /// Writes a full stanza.
        /// </summary>
        /// <param name="elem">The stanza to write out.</param>
        public override void Write(XmlElement elem)
        {
            if (m_sock is IElementSocket)
                ((IElementSocket)m_sock).Write(elem);
            else
                Write(elem.OuterXml);
        }

        /// <summary>
        /// Closes the socket connection.
        /// </summary>
        /// <param name="clean">Sends the stream:stream close packet if true.</param>
        public override void Close(bool clean)
        {
            // TODO: socket should still be connected, excepts for races.  Revist.
            if (clean)
                Write("</stream:stream>");
            m_sock.Close();
        }

#if !NO_SSL
        /// <summary>
        /// Negotiates Start-TLS with the other endpoint.
        /// </summary>
        public override void StartTLS()
        {
            //m_sock.StartTLS();
            //XEP25Socket s = Sock;

            //Debug.Assert(s != null);
            //m_listener[Options.REMOTE_CERTIFICATE] = s.RemoteCertificate;
        }
#endif

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
                XEP25Socket s = sock as XEP25Socket;

                m_listener[Options.REMOTE_CERTIFICATE] = s.RemoteCertificate;
            }
#endif
            m_listener.Connected();
        }

        void ISocketEventListener.OnClose(BaseSocket sock)
        {
            m_listener[Options.REMOTE_CERTIFICATE] = null;
            m_elements = null;
            m_listener.Closed();
        }

        void ISocketEventListener.OnError(BaseSocket sock, Exception ex)
        {
            m_listener[Options.REMOTE_CERTIFICATE] = null;
            m_elements = null;
            m_listener.Errored(ex);
        }

        bool ISocketEventListener.OnRead(BaseSocket sock, byte[] buf, int offset, int length)
        {
            Debug.Assert(m_listener != null);
            Debug.Assert(m_elements != null);
            m_listener.BytesRead(buf, offset, length);
            m_elements.Push(buf, offset, length);
            return true;
        }

        void ISocketEventListener.OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
        {
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

