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
using System.Text;
using System.Xml;

using bedrock.net;
using bedrock.util;
using jabber.protocol;

namespace jabber.connection
{
    /// <summary>
    /// Specifies the connection type, such as socket, polling, and so on.
    /// </summary>
    [SVN(@"$Id$")]
    public enum ConnectionType
    {
        /// <summary>
        /// Uses "Normal" XMPP socket
        /// </summary>
        Socket,
        /// <summary>
        /// Uses HTTP Polling, as in http://www.xmpp.org/extensions/xep-0025.html
        /// </summary>
        HTTP_Polling,
        /// <summary>
        /// Uses HTTP Binding, as in http://www.xmpp.org/extensions/xep-0124.html
        /// </summary>
        HTTP_Binding
    }

    /// <summary>
    /// Listens for stanza and connection events
    /// </summary>
    [SVN(@"$Id$")]
    public interface IStanzaEventListener
    {
        /// <summary>
        /// Gets or sets properties on the listener.
        /// </summary>
        /// <param name="prop">Property name.  Look at the Options class for some ideas.</param>
        /// <returns></returns>
        object this[string prop]
        {
            get;
            set;
        }

        /// <summary>
        /// Notifies the user that one of the properties has changed.
        /// </summary>
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Informs the client that the connection to the XMPP server has finished.
        /// Time to send the stream:stream packet.
        /// </summary>
        void Connected();

        /// <summary>
        /// Informs the client that a new connection from the server has been accepted.
        /// Wait for a stream:stream.
        /// </summary>
        void Accepted();

        /// <summary>
        /// Informs the client that text was read from the XMPP server.
        /// Use for debugging only.
        /// Will NOT be complete nodes at a time.
        /// </summary>
        /// <param name="buf">Buffer containing the data read.</param>
        /// <param name="offset">Where in the buffer the read data begins.</param>
        /// <param name="len">Length of the data read.</param>
        void BytesRead(byte[] buf, int offset, int len);

        /// <summary>
        /// Informs the client that text was written to the server.
        /// Use for debugging only. Will NOT be complete nodes at a time.
        /// </summary>
        /// <param name="buf">Bytes to write out.</param>
        /// <param name="offset">The index in the buffer to start getting bytes.</param>
        /// <param name="len">The amount of bytes to write out.</param>
        void BytesWritten(byte[] buf, int offset, int len);

        /// <summary>
        /// Informs the client that a new stream was initialized.
        /// You can add your packet factories to it.
        /// </summary>
        /// <param name="stream">The stream that was initialized.</param>
        void StreamInit(ElementStream stream);

        /// <summary>
        /// Notifies the client that an error has occurred.
        /// </summary>
        /// <param name="e">The exception that caused the error.</param>
        void Errored(Exception e);

        /// <summary>
        /// Notifies the client that the session has been closed.
        /// </summary>
        void Closed();

        /// <summary>
        /// Informs the client that a doc start tag has been received.
        /// This may be "synthetic" for some backends.
        /// </summary>
        /// <param name="elem">XML element containing the start tag.</param>
        void DocumentStarted(XmlElement elem);

        /// <summary>
        /// Receives the closing stream:stream.  Probably mostly equivalent to Closed(),
        /// except if the stream is still open, you should close it at this point.
        /// May not be called for some backends.
        /// </summary>
        void DocumentEnded();

        /// <summary>
        /// Receives an XML element such as stream:features and so on.
        /// </summary>
        /// <param name="elem">The XML Element received.</param>
        void StanzaReceived(XmlElement elem);

#if NET20 || __MonoCS__
        /// <summary>
        /// An invalid peer certificate was sent during SSL/TLS neogtiation.
        /// </summary>
        /// <param name="sock">The socket that experienced the error</param>
        /// <param name="certificate">The bad certificate</param>
        /// <param name="chain">The chain of CAs for the cert</param>
        /// <param name="sslPolicyErrors">A bitfield for the erorrs in the certificate.</param>
        /// <returns>True if the cert should be accepted anyway.</returns>
        bool OnInvalidCertificate(BaseSocket sock,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors);
#endif
    }

    /// <summary>
    /// Manages the base stream for reading and writing full stanzas.
    /// </summary>
    [SVN(@"$Id$")]
    public abstract class StanzaStream
    {
        /// <summary>
        /// Text encoding.  Always UTF-8 for XMPP.
        /// </summary>
        protected readonly Encoding ENC = Encoding.UTF8;

        /// <summary>
        /// Notifies the client that an event has occurred.
        /// </summary>
        protected IStanzaEventListener m_listener = null;

        /// <summary>
        /// Creates a StanzaStream.
        /// </summary>
        /// <param name="kind">Connection type, such as socket, polling, and so on.</param>
        /// <param name="listener">Connection event listeners.</param>
        /// <returns>StanzaStream used to connect to an XMPP server and send stanzas.</returns>
        public static StanzaStream Create(ConnectionType kind, IStanzaEventListener listener)
        {
            switch (kind)
            {
            case ConnectionType.Socket:
                return new SocketStanzaStream(listener);
            case ConnectionType.HTTP_Polling:
                return new PollingStanzaStream(listener);
            // TODO: Fix broken build.
            //            case ConnectionType.HTTP_Binding:
            //                return new BindingStanzaStream(listener);
            default:
                throw new NotImplementedException("Proxy type not implemented yet: " + kind.ToString());
            }
        }

        /// <summary>
        /// Creates a new stanza stream.
        /// </summary>
        /// <param name="listener">Event listener associated with the new stanza stream.</param>
        protected StanzaStream(IStanzaEventListener listener)
        {
            Debug.Assert(listener != null);
            m_listener = listener;
        }

        /// <summary>
        /// Starts the outbound connection to the XMPP server.
        /// </summary>
        abstract public void Connect();

        /// <summary>
        /// Listens for an inbound connection.  Only implemented by socket types for now.
        /// </summary>
        virtual public void Accept()
        {
            throw new NotImplementedException("Accept not implemented on this stream type");
        }

        /// <summary>
        /// Determines whether or not the client can call the Accept() method.
        /// </summary>
        virtual public bool Acceptable
        {
            get { return false; }
        }

        /// <summary>
        /// Starts the TLS handshake.
        /// </summary>
        virtual public void StartTLS()
        {
            throw new NotImplementedException("Start-TLS not implemented on this stream type");
        }

        /// <summary>
        /// Starts the compression on the connection.
        /// </summary>
        virtual public void StartCompression()
        {
            throw new NotImplementedException("Start-TLS not implemented on this stream type");
        }

        /// <summary>
        /// Initializes a new stream:stream.
        /// </summary>
        virtual public void InitializeStream()
        {
        }

        /// <summary>
        /// Writes a stream:stream start tag.
        /// Some underlying implementations will ignore this,
        /// but may pull out pertinent data.
        /// </summary>
        /// <param name="stream">Stream containing the start tag.</param>
        abstract public void WriteStartTag(jabber.protocol.stream.Stream stream);

        /// <summary>
        /// Writes an entire XML element.
        /// </summary>
        /// <param name="elem">XML element to write out.</param>
        abstract public void Write(XmlElement elem);

        /// <summary>
        /// Writes a raw string.
        /// </summary>
        /// <param name="str">String to write out.</param>
        abstract public void Write(string str);

        /// <summary>
        /// Closes the session with the XMPP server.
        /// </summary>
        /// <param name="clean">If true, send the stream:stream close packet.</param>
        abstract public void Close(bool clean);

        /// <summary>
        /// Determines whether or not the client is connected to the XMPP server.
        /// </summary>
        abstract public bool Connected
        {
            get;
        }

        /// <summary>
        /// Determines whether or not Jabber-Net supports TLS.
        /// </summary>
        virtual public bool SupportsTLS
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether or not this stream supports compression (XEP-0138).
        /// </summary>
        virtual public bool SupportsCompression
        {
            get { return false; }
        }
    }

    /// <summary>
    /// Informs the client that something happened on a StanzaStream.
    /// </summary>
    public delegate void StanzaStreamHandler(object sender, StanzaStream stream);
}
