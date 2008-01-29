/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
    /// Listen for stanza and connection events
    /// </summary>
    [SVN(@"$Id$")]
    public interface IStanzaEventListener
    {
        /// <summary>
        /// Get or set properties on the listener.
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
        /// Text was written to the server.  Use for debugging only.
        /// Will NOT be complete nodes at a time.
        /// </summary>
        void BytesWritten(byte[] buf, int offset, int len);

        /// <summary>
        /// Informs the client that a new stream was initialized.
        /// You can add your packet factories to it.
        /// </summary>
        /// <param name="stream">The stream that was initialized.</param>
        void StreamInit(ElementStream stream);

        /// <summary>
        /// An error has occurred.
        /// </summary>
        /// <param name="e"></param>
        void Errored(Exception e);

        /// <summary>
        /// The stream has been closed.
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
    /// Base stream for reading and writing full stanzas.
    /// </summary>
    [SVN(@"$Id$")]
    public abstract class StanzaStream
    {
        /// <summary>
        /// Text encoding.  Always UTF-8 for XMPP.
        /// </summary>
        protected readonly Encoding ENC = Encoding.UTF8;

        /// <summary>
        /// Where to fire events.
        /// </summary>
        protected IStanzaEventListener m_listener = null;

        /// <summary>
        /// Factory to create StanzaStream's.
        /// </summary>
        /// <param name="kind">How to connect?  Socket?  Polling?</param>
        /// <param name="listener">Connection event listeners</param>
        /// <returns>StanzaStream used to connect to an XMPP server and send stanzas</returns>
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
        /// Create a new stanza stream.
        /// </summary>
        /// <param name="listener"></param>
        protected StanzaStream(IStanzaEventListener listener)
        {
            Debug.Assert(listener != null);
            m_listener = listener;
        }

        /// <summary>
        /// Start the ball rolling.
        /// </summary>
        abstract public void Connect();

        /// <summary>
        /// Listen for an inbound connection.  Only implemented by socket types for now.
        /// </summary>
        virtual public void Accept()
        {
            throw new NotImplementedException("Accept not implemented on this stream type");
        }

        /// <summary>
        /// Is it legal to call Accept() at the moment?
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
        /// Handshake compression now.
        /// </summary>
        virtual public void StartCompression()
        {
            throw new NotImplementedException("Start-TLS not implemented on this stream type");
        }

        /// <summary>
        /// New stream:stream.
        /// </summary>
        virtual public void InitializeStream()
        {
        }

        /// <summary>
        /// Write a stream:stream.  Some underlying implementations will ignore this,
        /// but may pull out pertinent data.
        /// </summary>
        /// <param name="stream"></param>
        abstract public void WriteStartTag(jabber.protocol.stream.Stream stream);

        /// <summary>
        /// Write an entire element.
        /// </summary>
        /// <param name="elem"></param>
        abstract public void Write(XmlElement elem);

        /// <summary>
        /// Write raw string.
        /// </summary>
        /// <param name="str"></param>
        abstract public void Write(string str);

        /// <summary>
        /// Closes the stream.
        /// </summary>
        /// <param name="clean">If true, send the stream:stream close packet.</param>
        abstract public void Close(bool clean);

        /// <summary>
        /// Is the stream connected?  (for some loose value of "connected")
        /// </summary>
        abstract public bool Connected
        {
            get;
        }

        /// <summary>
        /// Does this stream support start-tls?
        /// </summary>
        virtual public bool SupportsTLS
        {
            get { return false; }
        }

        /// <summary>
        /// Does this stream support XEP-138 compression?
        /// </summary>
        virtual public bool SupportsCompression
        {
            get { return false; }
        }
    }

    /// <summary>
    /// Something happened on a StanzaStream.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="stream"></param>
    public delegate void StanzaStreamHandler(object sender, StanzaStream stream);
}
