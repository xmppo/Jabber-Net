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
#if NET20 || __MonoCS__
#define MODERN
#define SSL
#elif !NO_SSL
#define MENTALIS
#define SSL
#endif

using System;

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Xml;
using bedrock.util;

using jabber.protocol;
using jabber.protocol.stream;
using jabber.connection.sasl;

#if MODERN
using System.Security.Cryptography.X509Certificates;
#elif MENTALIS
using Org.Mentalis.Security.Certificates;
#endif

namespace jabber.connection
{
    /// <summary>
    /// Informs the client about events that happen on an ElementStream.
    /// </summary>
    public delegate void StreamHandler(Object sender, ElementStream stream);

    /// <summary>
    /// Manages option names.  These must be well-formed XML element names.
    /// </summary>
    [SVN(@"$Id$")]
    public abstract class Options
    {
        /// <summary>
        /// Contains the default namespace for this connection.
        /// </summary>
        public const string NAMESPACE = "namespace";

        /// <summary>
        /// Contains the network hostname or IP address of the XMPP server to connect to.
        /// </summary>
        public const string NETWORK_HOST = "network_host";
        /// <summary>
        /// Contains the identity of the item that the client is connecting to.
        /// For components, the component ID.
        /// </summary>
        public const string TO = "to";
        /// <summary>
        /// Contains the server identity that is expected on the X.509 certificate
        /// from the XMPP server.
        /// </summary>
        public const string SERVER_ID    = "tls.cn";
        /// <summary>
        /// Determines the keep-alive interval in seconds.
        /// </summary>
        public const string KEEP_ALIVE   = "keep_alive";
        /// <summary>
        /// Don't start keep-alives until we're fully authenticated.  This is what the SocketStanzaStream actually checks.
        /// </summary>
        public const string CURRENT_KEEP_ALIVE = "current_keep_alive";
        /// <summary>
        /// Contains the port number to connect to or to listen on.
        /// </summary>
        public const string PORT         = "port";
        /// <summary>
        /// Uses SSL on connection if set to true.
        /// </summary>
        public const string SSL          = "ssl";
        /// <summary>
        /// Uses Start-TLS on connection if set to true and
        /// the server supports it.
        /// </summary>
        public const string AUTO_TLS     = "tls.auto";
        /// <summary>
        /// Starts the XMPP stream compression (XEP-138) on connection
        /// if set to true.
        /// </summary>
        public const string AUTO_COMPRESS = "compress.auto";
        /// <summary>
        /// Allows plaintext authentication for connecting to the XMPP server.
        /// </summary>
        public const string PLAINTEXT    = "plaintext";
        /// <summary>
        /// Attempts a SASL connection if set to true and the feature is available
        /// from the XMPP server. If the server doesn't support SASL, the connection
        /// will move to a fallback mechanism.
        /// </summary>
        public const string SASL = "sasl";
        /// <summary>
        /// Requires SASL authentication on this connection if set to true.
        /// There is no fallback mechanism. If the server doesn't support SASL,
        /// the connection attempt will fail.
        /// </summary>
        public const string REQUIRE_SASL = "sasl.require";
        /// <summary>
        /// Contains the list of SASL Mechanisms such as Digest-MD5, Plain and so on.
        /// </summary>
        public const string SASL_MECHANISMS = "sasl.mechanisms";

        /// <summary>
        /// Contains the username to connect as.
        /// </summary>
        public const string USER     = "user";
        /// <summary>
        /// Contains the password for the user, or secret for the component.
        /// </summary>
        public const string PASSWORD = "password";
        /// <summary>
        /// Contains the connecting resource which is used to identify a unique connection.
        /// </summary>
        public const string RESOURCE = "resource";
        /// <summary>
        /// Contains the presence default priority for this connection.
        /// </summary>
        public const string PRIORITY = "priority";
        /// <summary>
        /// Contains the DNS Service/Protocol to prepend to domain.
        /// Example: _xmpp-client._tcp.
        /// </summary>
        public const string SRV_PREFIX = "srv.prefix";
        /// <summary>
        /// Allows auto-login to be used for the connection to the XMPP server
        /// if set to true.
        /// </summary>
        public const string AUTO_LOGIN    = "auto.login";
        /// <summary>
        /// Retrieves the roster items from the XMPP server on
        /// connection if set to true.
        /// </summary>
        public const string AUTO_ROSTER   = "auto.roster";
        /// <summary>
        /// Sends back 501/feature-not-implemented to the XMPP server if
        /// there are IQs that have not been handled if set to true.
        /// </summary>
        public const string AUTO_IQ_ERRORS   = "auto.iq_errors";
        /// <summary>
        /// Sends the presence on connection if set to true.
        /// </summary>
        public const string AUTO_PRESENCE = "auto.presence";

        /// <summary>
        /// Contains the certificate for our side of the SSL/TLS negotiation.
        /// </summary>
        public const string LOCAL_CERTIFICATE   = "certificate.local";
        /// <summary>
        /// Contains the remote certificate that the XMPP server sent to the client.
        /// </summary>
        public const string REMOTE_CERTIFICATE  = "certificate.remote";
        /// <summary>
        /// Uses x509 selection dialog box when a certificate is requested
        /// if set to true.
        /// </summary>
        public const string CERTIFICATE_GUI = "certificate.gui";
        /// <summary>
        /// Contains the number of seconds to wait before attempting a reconnect.
        /// </summary>
        public const string RECONNECT_TIMEOUT   = "reconnect_timeout";
        /// <summary>
        /// Determines the connection type (sockets, HTTP polling, or HTTP binding).
        /// </summary>
        public const string CONNECTION_TYPE     = "connection";
        /// <summary>
        /// Contains the URL to poll on, or bind to.
        /// </summary>
        public const string POLL_URL            = "poll.url";
        /// <summary>
        /// Connects to the XMPP server or listen for connections.
        /// </summary>
        public const string COMPONENT_DIRECTION = "component.dir";
        /// <summary>
        /// Contains the logical JID associated with this connection.
        /// </summary>
        public const string JID = "jid";

        /// <summary>
        /// Contains the proxy type, such as none, SOCKS5 and so on.
        /// </summary>
        public const string PROXY_TYPE = "proxy.type";
        /// <summary>
        /// Contains the hostname or IP address of the proxy.
        /// </summary>
        public const string PROXY_HOST = "proxy.host";
        /// <summary>
        /// Contains the port number for the proxy.
        /// </summary>
        public const string PROXY_PORT = "proxy.port";
        /// <summary>
        /// Contains the username for the proxy server.
        /// </summary>
        public const string PROXY_USER = "proxy.user";
        /// <summary>
        /// Contains the password for the proxy server.
        /// </summary>
        public const string PROXY_PW   = "proxy.password";
    }

    /// <summary>
    /// Manages the XMPP stream of the connection.
    /// </summary>
    [SVN(@"$Id$")]
    abstract public class XmppStream :
        System.ComponentModel.Component,
        IStanzaEventListener
    {
        private static readonly object[][] DEFAULTS = new object[][] {
            new object[] {Options.TO, "jabber.com"},
            new object[] {Options.KEEP_ALIVE, 30000},
            new object[] {Options.CURRENT_KEEP_ALIVE, -1},
            new object[] {Options.PORT, 5222},
            new object[] {Options.RECONNECT_TIMEOUT, 30000},
            new object[] {Options.PROXY_PORT, 1080},
            new object[] {Options.SSL, false},
            new object[] {Options.SASL, true},
            new object[] {Options.REQUIRE_SASL, false},
            new object[] {Options.PLAINTEXT, false},
            new object[] {Options.AUTO_TLS, true},
#if !NO_COMPRESSION
            new object[] {Options.AUTO_COMPRESS, true},
#endif
            new object[] {Options.CERTIFICATE_GUI, true},
            new object[] {Options.PROXY_TYPE, ProxyType.None},
            new object[] {Options.CONNECTION_TYPE, ConnectionType.Socket},
        };

        /// <summary>
        /// Contains the character encoding for the XMPP stream.
        /// Currently, it is set to UTF-8.
        /// </summary>
        protected static readonly System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private StanzaStream m_stanzas = null;
        private IQTracker m_tracker = null;

        private XmlDocument m_doc        = new XmlDocument();
        private BaseState   m_state      = ClosedState.Instance;
        private IDictionary m_properties = new Hashtable();

        private string m_streamID = null;
        private object m_stateLock = new object();
        private ArrayList m_callbacks = new ArrayList();

        private Timer m_reconnectTimer = null;
        private bool m_reconnect = false;
        private bool m_sslOn = false;
        private bool m_compressionOn = false;

        private XmlNamespaceManager m_ns;
        private ISynchronizeInvoke m_invoker = null;

        // XMPP v1 stuff
        private string m_serverVersion = null;
        private SASLProcessor m_saslProc = null;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = new System.ComponentModel.Container();

        /// <summary>
        /// Creates a new XMPP stream and associates it with the parent control.
        /// </summary>
        /// <param name="container">Parent control.</param>
        public XmppStream(System.ComponentModel.IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Sets defaults in bulk.
        /// </summary>
        /// <param name="defaults">Array of objects to replace to defaults with.</param>
        protected void SetDefaults(object[][] defaults)
        {
            foreach (object[] def in defaults)
            {
                this[(string)def[0]] = def[1];
            }
        }

        /// <summary>
        /// Creates a new SocketElementStream.
        /// </summary>
        public XmppStream()
        {
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_tracker = new IQTracker(this);

            SetDefaults(DEFAULTS);
        }

        /// <summary>
        /// Sets or retrieves a connection property.
        /// You have to know the type of the property based on the name.
        /// For example, PORT is an integer.
        /// </summary>
        /// <param name="prop">The property to get or set.</param>
        /// <returns></returns>
        public object this[string prop]
        {
            get
            {
                if (!m_properties.Contains(prop))
                    return null;
                return m_properties[prop];
            }
            set
            {
                m_properties[prop] = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }

        /// <summary>
        /// Informs the client that a property changed on the instance.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /*
        /// <summary>
        /// Create a SocketElementStream out of an accepted socket.
        /// </summary>
        /// <param name="aso"></param>
        public XmppStream(BaseSocket aso)
        {
            m_accept = m_sock = null;
            if (aso is AsyncSocket)
            {
                m_watcher = ((AsyncSocket)aso).SocketWatcher;
            }
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
            InitializeStream();
            m_state = jabber.connection.AcceptingState.Instance;
        }

        /// <summary>
        /// Create a SocketElementStream with an existing SocketWatcher, so that you can do
        /// lots of concurrent connections.
        /// </summary>
        /// <param name="watcher"></param>
        public XmppStream(SocketWatcher watcher)
        {
            m_watcher = watcher;
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
        }
        */

        /// <summary>
        /// Informs the client when text has been written to the XMPP server.
        /// Use for debugging only.
        /// Will NOT be complete nodes at a time.
        /// </summary>
        [Category("Debug")]
        public event bedrock.TextHandler OnWriteText;

        /// <summary>
        /// Informs the client that text was read from the server.
        /// Use for debugging only. Will NOT be complete nodes at a time.
        /// </summary>
        [Category("Debug")]
        public event bedrock.TextHandler OnReadText;

        /// <summary>
        /// Informs the client that a new stream has been inialized.
        /// You can add your packet factories to the new stream.
        /// NOTE: You may NOT make calls to the GUI in this callback, unless you
        /// call Invoke.  Make sure you add your packet factories before
        /// calling Invoke, however.
        /// </summary>
        [Category("Stream")]
        public event StreamHandler OnStreamInit;

        /// <summary>
        /// Informs the client that an error occurred when processing.
        /// The connection has been closed.
        /// </summary>
        [Category("Stream")]
        public event bedrock.ExceptionHandler OnError;

        /// <summary>
        /// Notifies the client about every jabber packet.
        /// This is a union of the OnPresence, OnMessage, and OnIQ methods.
        /// Use this *or* the other 3 methods, but not both, as a matter of style.
        /// </summary>
        [Category("Stream")]
        public event ProtocolHandler OnProtocol;

        /// <summary>
        /// Notifies the client that the stream header, as a packet,
        /// has been received.  Can be called multiple  times for
        /// a single session, with XMPP.
        /// </summary>
        [Category("Stream")]
        public event ProtocolHandler OnStreamHeader;

        /// <summary>
        /// Notifies the client that the SASL handshake has started.
        /// </summary>
        protected event SASLProcessorHandler OnSASLStart;

        /// <summary>
        /// Gets notified of the end of a SASL handshake.
        /// </summary>
        protected event FeaturesHandler OnSASLEnd;

        /// <summary>
        /// Gets notified when SASL login fails.
        /// </summary>
        protected event ProtocolHandler OnSASLError;

        /// <summary>
        /// Informs the client that it received a stream:error packet.
        /// </summary>
        [Category("Stream")]
        [Description("We received stream:error packet.")]
        public event ProtocolHandler OnStreamError;

        /// <summary>
        /// Informs the client that the connection is complete and the user is authenticated.
        /// </summary>
        [Category("Stream")]
        public event bedrock.ObjectHandler OnAuthenticate;

        /// <summary>
        /// Informs the client that the connection is connected,
        /// but no stream:stream has been sent yet.
        /// </summary>
        [Category("Stream")]
        public event StanzaStreamHandler OnConnect;

        /// <summary>
        /// Informs the client that the connection is disconnected.
        /// </summary>
        [Category("Stream")]
        public event bedrock.ObjectHandler OnDisconnect;

#if MODERN
        /// <summary>
        /// An invalid cert was received from the other side.  Set this event and return true to 
        /// use the cert anyway.  If the event is not set, an ugly user interface will be displayed.
        /// </summary>
        [Category("Stream")]
        public event System.Net.Security.RemoteCertificateValidationCallback OnInvalidCertificate;
#endif

        /// <summary>
        /// Gets the tracker for sending IQ packets.
        /// </summary>
        [Browsable(false)]
        public IIQTracker Tracker
        {
            get { return m_tracker; }
        }

        /// <summary>
        /// Gets or sets the name of the XMPP server to connect to.
        /// </summary>
        [Description("Gets or sets the name of the XMPP server to connect to.")]
        [DefaultValue("jabber.com")]
        [Category("Jabber")]
        public virtual string Server
        {
            get { return this[Options.TO] as string; }
            set { this[Options.TO] = value; }
        }

        /// <summary>
        /// Gets or sets the network host address to use on the "to" attribute
        /// of the stream:stream. You can put the network hostname or IP address
        /// of the XMPP server to connect to. If none is specified, the Server will
        /// be used. Eventually, when SRV is supported, this will be deprecated.
        /// </summary>
        [Description("")]
        [DefaultValue(null)]
        [Category("Jabber")]
        public string NetworkHost
        {
            get { return this[Options.NETWORK_HOST] as string; }
            set { this[Options.NETWORK_HOST] = value; }
        }

        /// <summary>
        /// Specifies the TCP port to connect to.
        /// </summary>
        [Description("Specifies the TCP port to connect to.")]
        [DefaultValue(5222)]
        [Category("Jabber")]
        public int Port
        {
            get { return (int)this[Options.PORT]; }
            set { this[Options.PORT] = value; }
        }

        /// <summary>
        /// Specifies whether plaintext authentication is used for connecting
        /// to the XMPP server.
        /// </summary>
        [Description("Allow plaintext authentication?")]
        [DefaultValue(false)]
        [Category("Jabber")]
        public bool PlaintextAuth
        {
            get { return (bool)this[Options.PLAINTEXT]; }
            set { this[Options.PLAINTEXT] = value; }
        }

        /// <summary>
        /// Determines whether or not the current connection is secured with SSL/TLS.
        /// </summary>
        [Browsable(false)]
        public bool SSLon
        {
            get { return m_sslOn; }
        }

        /// <summary>
        /// Gets the JID from the connection.
        /// </summary>
        [Browsable(false)]
        public JID JID
        {
            // Make sure to set this option in subclasses.
            get 
            { 
                object j = this[Options.JID];
                if (j == null)
                    return null;
                if (j is JID)
                    return (JID)j;
                if (j is string)
                    return new JID((string)j);
                Debug.Assert(false, "Unknown JID type", j.GetType().ToString());
                return null;
            }
        }

        /// <summary>
        /// Determines whether or not the current connection uses
        /// XMPP stream compression (XEP-138).
        /// </summary>
        [Browsable(false)]
        public bool CompressionOn
        {
            get { return m_compressionOn; }
        }

        /// <summary>
        /// Determines whether SSL3/TLS1 authentication is used when a user
        /// connects to the XMPP server.
        /// </summary>
        [Description("Do SSL3/TLS1 on startup.")]
        [DefaultValue(false)]
        [Category("Jabber")]
        public bool SSL
        {
            get { return (bool)this[Options.SSL]; }
            set
            {
#if NO_SSL
                Debug.Assert(!value, "SSL support not compiled in");
#endif
                this[Options.SSL] = value;
            }
        }

        /// <summary>
        /// Allows Start-TLS on connection if the server supports it and if set to true.
        /// </summary>
        [Browsable(false)]
        public bool AutoStartTLS
        {
            get { return (bool)this[Options.AUTO_TLS]; }
            set { this[Options.AUTO_TLS] = value; }
        }

        /// <summary>
        /// Allows start compression on connection if the server supports it and
        /// is set to true.
        /// </summary>
        [Browsable(false)]
        public bool AutoStartCompression
        {
            get { return (bool)this[Options.AUTO_COMPRESS]; }
            set { this[Options.AUTO_COMPRESS] = value; }
        }

#if MODERN
        /// <summary>
        /// Gets or sets the certificate to be used for the local
        /// side of sockets when SSL is enabled.
        /// </summary>
        [Browsable(false)]
        public X509Certificate LocalCertificate
        {
            get { return this[Options.LOCAL_CERTIFICATE] as X509Certificate; }
            set { this[Options.LOCAL_CERTIFICATE] = value; }
        }

        /// <summary>
        /// Sets the certificate to be used for accept sockets.  To
        /// generate a test .pfx file using OpenSSL, add this to
        /// openssl.conf:
        ///   <blockquote>
        ///   [ serverex ]
        ///   extendedKeyUsage=1.3.6.1.5.5.7.3.1
        ///   </blockquote>
        /// and run the following commands:
        ///   <blockquote>
        ///   openssl req -new -x509 -newkey rsa:1024 -keyout
        ///     privkey.pem -out key.pem -extensions serverex
        ///   openssl pkcs12 -export -in key.pem -inkey privkey.pem
        ///     -name localhost -out localhost.pfx
        ///   </blockquote>
        /// If you leave the certificate null, and you are doing
        /// Accept, the SSL class will try to find a default server
        /// certificate on your box.
        /// </summary>
        /// <param name="filename">A .pfx or .cer file.</param>
        /// <param name="password">The password, if this is a .pfx
        /// file, null if .cer file.</param>
        public void SetCertificateFile(string filename,
                                       string password)
        {
#if __MonoCS__
            byte[] data = null;
            using (FileStream fs = File.OpenRead (filename))
            {
                data = new byte [fs.Length];
                fs.Read (data, 0, data.Length);
                fs.Close ();
            }

            Mono.Security.X509.PKCS12 pfx = new Mono.Security.X509.PKCS12(data, password);
            if (pfx.Certificates.Count > 0)
                this[Options.LOCAL_CERTIFICATE] = new X509Certificate(pfx.Certificates[0].RawData);
#else
            this[Options.LOCAL_CERTIFICATE] = new X509Certificate2(filename, password);
#endif
        }

#elif MENTALIS
        /// <summary>
        /// The certificate to be used for the local side of sockets,
        /// with SSL on.
        /// </summary>
        [Browsable(false)]
        public Certificate LocalCertificate
        {

            get { return this[Options.LOCAL_CERTIFICATE] as Certificate; }
            set { this[Options.LOCAL_CERTIFICATE] = value; }
        }


        /// <summary>
        /// Set the certificate to be used for accept sockets.  To generate a test .pfx file using openssl,
        /// add this to openssl.conf:
        ///   <blockquote>
        ///   [ serverex ]
        ///   extendedKeyUsage=1.3.6.1.5.5.7.3.1
        ///   </blockquote>
        /// and run the following commands:
        ///   <blockquote>
        ///   openssl req -new -x509 -newkey rsa:1024 -keyout privkey.pem -out key.pem -extensions serverex
        ///   openssl pkcs12 -export -in key.pem -inkey privkey.pem -name localhost -out localhost.pfx
        ///   </blockquote>
        /// If you leave the certificate null, and you are doing
        /// Accept, the SSL class will try to find a default server
        /// cert on your box.  If you have IIS installed with a cert,
        /// this might just go...
        /// </summary>
        /// <param name="filename">A .pfx or .cer file</param>
        /// <param name="password">The password, if this is a .pfx
        /// file, null if .cer file.</param>
        public void SetCertificateFile(string filename, string password)
        {
                        if (!File.Exists(filename))
                        {
                                throw new CertificateException("File does not exist: " + filename);
                        }
                        CertificateStore store;
                        if (password != null)
                        {
                                store = CertificateStore.CreateFromPfxFile(filename, password);
                        }
                        else
                        {
                                store = CertificateStore.CreateFromCerFile(filename);
                        }
                        this[Options.LOCAL_CERTIFICATE] = bedrock.net.CertUtil.FindServerCert(store);
                        if (this[Options.LOCAL_CERTIFICATE] == null)
                                throw new CertificateException("The certificate file does not contain a server authentication certificate.");
                }
#endif

        /// <summary>
        /// Calls Invoke() for all callbacks on this control.
        /// </summary>
        [Description("Invoke all callbacks on this control")]
        [DefaultValue(null)]
        [Category("Jabber")]
        public ISynchronizeInvoke InvokeControl
        {
            get
            {
                // If we are running in the designer, let's try to get
                // an invoke control from the environment.  VB
                // programmers can't seem to follow directions.
                if ((this.m_invoker == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        object root = host.RootComponent;
                        if ((root != null) && (root is ISynchronizeInvoke))
                        {
                            m_invoker = (ISynchronizeInvoke)root;
                            // TODO: fire some sort of propertyChanged event,
                            // so that old code gets cleaned up correctly.
                        }
                    }
                }
                return m_invoker;
            }
            set { m_invoker = value; }
        }

        /// <summary>
        /// Gets or sets the keep-alive interval in seconds.
        /// </summary>
        [Description("Gets or sets the keep-alive interval in seconds")]
        [Category("Jabber")]
        [DefaultValue(20f)]
        public float KeepAlive
        {
            get { return ((int)this[Options.KEEP_ALIVE]) / 1000f; }
            set { this[Options.KEEP_ALIVE] = (int)(value * 1000f); }
        }

        /// <summary>
        /// Gets or sets the number of seconds before automatically
        /// reconnecting if the connection drops.
        /// -1 to disable, 0 for immediate.
        /// </summary>
        [Description("Automatically reconnect a connection.")]
        [DefaultValue(30)]
        [Category("Automation")]
        public float AutoReconnect
        {
            get { return ((int)this[Options.RECONNECT_TIMEOUT]) / 1000f; }
            set { this[Options.RECONNECT_TIMEOUT] = (int)(value * 1000f); }
        }

        /// <summary>
        /// Gets or sets the proxy type, such as none, SOCKS5 and so on.
        /// </summary>
        [Description("Gets or sets the proxy type, such as none, SOCKS5 and so on.")]
        [DefaultValue(ProxyType.None)]
        [Category("Proxy")]
        public ProxyType Proxy
        {
            get { return (ProxyType)this[Options.PROXY_TYPE]; }
            set { this[Options.PROXY_TYPE] = value; }
        }

        /// <summary>
        /// Gets or sets the connection type, such as Socket, HTTP polling and so on.
        /// </summary>
        [Description("Gets or sets the connection type, such as Socket, HTTP polling and so on.")]
        [DefaultValue(ConnectionType.Socket)]
        [Category("Proxy")]
        public ConnectionType Connection
        {
            get { return (ConnectionType)this[Options.CONNECTION_TYPE]; }
            set { this[Options.CONNECTION_TYPE] = value; }
        }

        /// <summary>
        /// Gets or sets the hostname running the proxy.
        /// </summary>
        [Description("Gets or sets the hostname running the proxy.")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyHost
        {
            get { return this[Options.PROXY_HOST] as string; }
            set { this[Options.PROXY_HOST] = value; }
        }

        /// <summary>
        /// Gets or sets the port number of the proxy host.
        /// </summary>
        [Description("Gets or sets the port number of the proxy host.")]
        [DefaultValue(1080)]
        [Category("Proxy")]
        public int ProxyPort
        {
            get { return (int)this[Options.PROXY_PORT]; }
            set { this[Options.PROXY_PORT] = value; }
        }

        /// <summary>
        /// Gets or sets the authentication username for the SOCKS5 proxy.
        /// </summary>
        [Description("Gets or sets the authentication username for the SOCKS5 proxy.")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyUsername
        {
            get { return this[Options.PROXY_USER] as string; }
            set { this[Options.PROXY_USER] = value; }
        }

        /// <summary>
        /// Gets or sets the authentication password for the SOCKS5 proxy.
        /// </summary>
        [Description("the auth password for the socks5 proxy")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyPassword
        {
            get { return this[Options.PROXY_PW] as string; }
            set { this[Options.PROXY_PW] = value; }
        }

        /// <summary>
        /// Gets or sets the ID attribute from the
        /// stream:stream element sent by the XMPP server.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public string StreamID
        {
            get { return m_streamID; }
            set { m_streamID = value; }
        }

        /// <summary>
        /// Retrieves the outbound document.
        /// </summary>
        [Browsable(false)]
        public XmlDocument Document
        {
            get { return m_doc; }
        }

        /// <summary>
        /// Gets or sets the current state of the connection.
        /// Lock on StateLock before accessing.
        /// </summary>
        [Browsable(false)]
        protected virtual BaseState State
        {
            get { return m_state; }
            set { m_state = value;
            // Debug.WriteLine("New state: " + m_state.ToString());
            }
        }

        /// <summary>
        /// Gets the lock for the state information.
        /// </summary>
        [Browsable(false)]
        protected object StateLock
        {
            get { return m_stateLock; }
        }

        /// <summary>
        /// Gets or sets the state to authenticated.  Locks on StateLock
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool IsAuthenticated
        {
            get
            {
                lock (StateLock)
                {
                    return (State == RunningState.Instance);
                }
            }
            set
            {
                bool close = false;
                lock (StateLock)
                {
                    if (value)
                    {
                        State = RunningState.Instance;
                    }
                    else
                        close = true;
                }
                if (close)
                    Close();
                if (value && (OnAuthenticate != null))
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnAuthenticate, new object[] { this });
                    else
                        OnAuthenticate(this);
                }
                this[Options.CURRENT_KEEP_ALIVE] = this[Options.KEEP_ALIVE];
            }
        }

        /// <summary>
        /// Returns the namespace for this connection.
        /// </summary>
        [Browsable(false)]
        protected abstract string NS
        {
            get;
        }

        /// <summary>
        /// Determines whether or not SASL is required for connecting to the XMPP server.
        /// </summary>
        [Description("Determines if SASL is required for connecting to the XMPP server.")]
        [DefaultValue(false)]
        public bool RequiresSASL
        {
            get { return (bool)this[Options.REQUIRE_SASL]; }
            set { this[Options.REQUIRE_SASL] = value; }
        }

        /// <summary>
        /// Gets the version number of the XMPP server.
        /// </summary>
        [Description("Gets the version number of the XMPP server.")]
        [DefaultValue(null)]
        public string ServerVersion
        {
            get { return m_serverVersion; }
        }

        /// <summary>
        /// Writes just the start tag of the given XML element.
        /// Typically only used for &lt;stream:stream&gt;.
        /// </summary>
        /// <param name="elem">&lt;stream:stream%gt; XML element.</param>
        public void WriteStartTag(jabber.protocol.stream.Stream elem)
        {
            m_stanzas.WriteStartTag(elem);
        }

        /// <summary>
        /// Sends the given packet to the server.
        /// </summary>
        /// <param name="elem">The XML element to send.</param>
        public virtual void Write(XmlElement elem)
        {
            m_stanzas.Write(elem);
        }

        /// <summary>
        /// Sends a raw string.
        /// </summary>
        /// <param name="str">The string to send.</param>
        public void Write(string str)
        {
            m_stanzas.Write(str);
        }

        /// <summary>
        /// Starts connecting to the XMPP server.  This is done asyncronously.
        /// </summary>
        public virtual void Connect()
        {
            this[Options.CURRENT_KEEP_ALIVE] = -1;
            m_stanzas = StanzaStream.Create(this.Connection, this);
            lock (StateLock)
            {
                State = ConnectingState.Instance;
                m_reconnect = ((int)this[Options.RECONNECT_TIMEOUT] >= 0);
            }
            m_stanzas.Connect();
        }

        /// <summary>
        /// Listens for connections from the XMPP server and is used for components only.
        /// </summary>
        protected virtual void Accept()
        {
            if ((m_stanzas == null) || (!m_stanzas.Acceptable))
                m_stanzas = StanzaStream.Create(this.Connection, this);
            lock (StateLock)
            {
                this.State = AcceptingState.Instance;
                m_reconnect = ((int)this[Options.RECONNECT_TIMEOUT] >= 0);
            }
            m_stanzas.Accept();
        }

        /// <summary>
        /// If autoReconnect is on, start the timer for reconnect now.
        /// </summary>
        private void TryReconnect()
        {
            // close was not requested, or autoreconnect turned on.
            if (m_reconnect)
            {
                if (m_reconnectTimer != null)
                    m_reconnectTimer.Dispose();

                m_reconnectTimer = new System.Threading.Timer(
                        new System.Threading.TimerCallback(Reconnect),
                        null,
                        (int)this[Options.RECONNECT_TIMEOUT],
                        System.Threading.Timeout.Infinite);
            }
        }

        /// <summary>
        /// Closes down the connection with the XMPP server with a clean shutdown.
        /// </summary>
        public virtual void Close()
        {
            Close(true);
        }

        /// <summary>
        /// Closes down the connection.
        /// </summary>
        /// <param name="clean">True for graceful shutdown</param>
        public virtual void Close(bool clean)
        {
            bool doClose = false;
            bool doStream = false;

            lock (StateLock)
            {
                if ((State == RunningState.Instance) && (clean))
                {
                    m_reconnect = false;
                    doStream = true;
                }
                if (m_state != ClosedState.Instance)
                {
                    State = ClosingState.Instance;
                    doClose = true;
                }
            }

            if ((m_stanzas != null) && m_stanzas.Connected && doClose)
            {
                m_stanzas.Close(doStream);
            }
        }

        /// <summary>
        /// Invokes the given method on the Invoker, and does some exception handling.
        /// </summary>
        /// <param name="method">Method to call on the invoker thread.</param>
        /// <param name="args">Arguments to pass to the method.</param>
        protected void CheckedInvoke(MulticastDelegate method, object[] args)
        {
            try
            {
                Debug.Assert(m_invoker != null, "Check for this.InvokeControl == null before calling CheckedInvoke");
                Debug.Assert(m_invoker.InvokeRequired, "Check for InvokeRequired before calling CheckedInvoke");

                m_invoker.BeginInvoke(method, args);
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                Debug.WriteLine("Exception passed along by XmppStream: " + e.ToString());
                throw e.InnerException;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception in XmppStream: " + e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Determines whether or not a callback needs to be on the GUI thread.
        /// </summary>
        /// <returns>
        /// True if the invoke control is set and the current thread
        /// is not the GUI thread.
        /// </returns>
        protected bool InvokeRequired
        {
            get
            {
                if (m_invoker == null)
                    return false;
                return m_invoker.InvokeRequired;
            }
        }

        /// <summary>
        /// Informs the client that the first tag of the XML document has been received.
        /// </summary>
        /// <param name="sender">Caller of this function.</param>
        /// <param name="elem">The XML element that was received.</param>
        protected virtual void OnDocumentStart(object sender, System.Xml.XmlElement elem)
        {
            bool hack = false;

            if (elem is jabber.protocol.stream.Stream)
            {
                jabber.protocol.stream.Stream str = elem as jabber.protocol.stream.Stream;

                m_streamID = str.ID;
                m_serverVersion = str.Version;

                // See XMPP-core section 4.4.1.  We'll accept 1.x
                if (m_serverVersion.StartsWith("1."))
                {
                    lock (m_stateLock)
                    {
                        if (State == SASLState.Instance)
                            // already authed.  last stream restart.
                            State = SASLAuthedState.Instance;
                        else
                            State = jabber.connection.ServerFeaturesState.Instance;
                    }
                }
                else
                {
                    lock (m_stateLock)
                    {
                        State = NonSASLAuthState.Instance;
                    }
                    hack = true;
                }
                if (OnStreamHeader != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnStreamHeader, new object[] { this, elem });
                    else
                        OnStreamHeader(this, elem);
                }
                CheckAll(elem);

                if (hack && (OnSASLStart != null))
                {
                    OnSASLStart(this, null); // Hack.  Old-style auth for jabberclient.
                }
            }
// TODO: Fix broken build
/*
            else if (elem is jabber.protocol.httpbind.Body)
            {
                jabber.protocol.httpbind.Body body = elem as jabber.protocol.httpbind.Body;

                m_streamID = body.AuthID;
            }
*/
        }

        /// <summary>
        /// Informs the client that an XML element was received and
        /// invokes the OnProtocol event.
        /// </summary>
        /// <param name="sender">The object that called this method.</param>
        /// <param name="tag">XML element that contains the new tag.</param>
        protected virtual void OnElement(object sender, System.Xml.XmlElement tag)
        {
            //Debug.WriteLine(tag.OuterXml);

            if (tag is jabber.protocol.stream.Error)
            {
                // Stream error.  Race condition!  Two cases:
                // 1) OnClose has already fired, in which case we are in ClosedState, and the reconnect timer is pending.
                // 2) OnClose hasn't fired, in which case we trick it into not starting the reconnect timer.
                lock (m_stateLock)
                {
                    if (m_state != ClosedState.Instance)
                    {
                        State = ClosingState.Instance;
                    }
                    else if (m_reconnectTimer != null)
                    {
                        Debug.WriteLine("Disposing of timer");
                        m_reconnectTimer.Dispose();
                    }
                }

                if (OnStreamError != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnStreamError, new object[] { this, tag });
                    else
                        OnStreamError(this, tag);
                }
                return;
            }

            if (State == ServerFeaturesState.Instance)
            {
                Features f = tag as Features;
                if (f == null)
                {
                    FireOnError(new InvalidOperationException("Expecting stream:features from a version='1.0' server"));
                    return;
                }

#if SSL
                // don't do starttls if we're already on an SSL socket.
                // bad server setup, but no skin off our teeth, we're already
                // SSL'd.  Also, start-tls won't work when polling.
                if ((bool)this[Options.AUTO_TLS] &&
                    (f.StartTLS != null) &&
                    (!m_sslOn) &&
                    m_stanzas.SupportsTLS)
                {
                    // start-tls
                    lock (m_stateLock)
                    {
                        State = StartTLSState.Instance;
                    }
                    this.Write(new StartTLS(m_doc));
                    return;
                }
#endif

#if !NO_COMPRESSION
                Compression comp = f.Compression;
                if ((bool)this[Options.AUTO_COMPRESS] &&
                    (comp != null) &&
                    comp.HasMethod("zlib") &&
                    (!m_compressionOn) &&
                    m_stanzas.SupportsCompression )
                {
                    // start-tls
                    lock (m_stateLock)
                    {
                        State = CompressionState.Instance;
                    }
                    Compress c = new Compress(m_doc);
                    c.Method = "zlib";
                    this.Write(c);
                    return;
                }
#endif

                // not authenticated yet.  Note: we'll get a stream:features
                // after the last sasl restart, so we shouldn't try to iq:auth
                // at that point.
                if (!IsAuthenticated)
                {
                    Mechanisms ms = f.Mechanisms;
                    m_saslProc = null;

                    MechanismType types = MechanismType.NONE;

                    if (ms != null)
                    {
                        // if SASL_MECHANISMS is set in the options, it is the limited set 
                        // of mechanisms we're willing to try.  Mask them off of the offered set.
                        object smt = this[Options.SASL_MECHANISMS];
                        if (smt != null)
                            types = (MechanismType)smt & ms.Types;
                        else
                            types = ms.Types;
                    }

                    if ((types != MechanismType.NONE) && ((bool)this[Options.SASL]))
                    {
                        lock (m_stateLock)
                        {
                            State = SASLState.Instance;
                        }
                        m_saslProc = SASLProcessor.createProcessor(types, m_sslOn || (bool)this[Options.PLAINTEXT]);
                        if (m_saslProc == null)
                        {

                            FireOnError(new NotImplementedException("No implemented mechanisms in: " + types.ToString()));
                            return;
                        }
                        if (OnSASLStart != null)
                            OnSASLStart(this, m_saslProc);

                        try
                        {
                            Step s = m_saslProc.step(null, this.Document);
                            if (s != null)
                                this.Write(s);
                        }
                        catch (Exception e)
                        {
                            FireOnError(new SASLException(e.Message));
                            return;
                        }
                    }

                    if (m_saslProc == null)
                    { // no SASL mechanisms.  Try iq:auth.
                        if ((bool)this[Options.REQUIRE_SASL])
                        {
                            FireOnError(new SASLException("No SASL mechanisms available"));
                            return;
                        }
                        lock (m_stateLock)
                        {
                            State = NonSASLAuthState.Instance;
                        }
                        if (OnSASLStart != null)
                            OnSASLStart(this, null); // HACK: old-style auth for jabberclient.
                    }
                }
            }
            else if (State == SASLState.Instance)
            {
                if (tag is Success)
                {
                    // restart the stream again
                    SendNewStreamHeader();
                }
                else if (tag is SASLFailure)
                {
                    m_saslProc = null;

                    lock (m_stateLock)
                    {
                        State = SASLFailedState.Instance;
                    }
                    SASLFailure sf = tag as SASLFailure;
                    // TODO: I18N
                    if (OnSASLError != null)
                    {
                        m_reconnect = false;
                        OnSASLError(this, sf);
                    }
                    else
                        FireOnError(new SASLException("SASL failure: " + sf.InnerXml));
                    return;
                }
                else if (tag is Step)
                {
                    try
                    {
                        Step s = m_saslProc.step(tag as Step, this.Document);
                        if (s != null)
                            Write(s);
                    }
                    catch (Exception e)
                    {
                        FireOnError(new SASLException(e.Message));
                        return;
                    }
                }
                else
                {
                    m_saslProc = null;
                    FireOnError(new SASLException("Invalid SASL protocol"));
                    return;
                }
            }
#if SSL
            else if (State == StartTLSState.Instance)
            {
                switch (tag.Name)
                {
                case "proceed":
                    if (!StartTLS())
                        return;
                    SendNewStreamHeader();
                    break;
                case "failure":
                    FireOnError(new AuthenticationFailedException());
                    return;
                }
            }
#endif

#if !NO_COMPRESSION
            else if (State == CompressionState.Instance)
            {
                switch (tag.Name)
                {
                case "compressed":
                    if (!StartCompression())
                        return;
                    SendNewStreamHeader();
                    break;
                case "failure":
                    CompressionFailure fail = tag as CompressionFailure;
                    FireOnError(new bedrock.io.CompressionFailedException(fail.Error));
                    return;
                }

            }
#endif
            else if (State == SASLAuthedState.Instance)
            {
                Features f = tag as Features;
                if (f == null)
                {
                    FireOnError(new InvalidOperationException("Expecting stream:features from a version='1.0' server"));
                    return;
                }
                if (OnSASLEnd != null)
                    OnSASLEnd(this, f);
                m_saslProc = null;
            }
            else
            {
                if (OnProtocol != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnProtocol, new object[] { this, tag });
                    else
                        OnProtocol(this, tag);
                }
            }
            CheckAll(tag);
        }

        /// <summary>
        /// Begins the TLS handshake, either client-side or server-side.
        /// </summary>
        /// <returns>True if StartTLS worked.</returns>
        protected bool StartTLS()
        {
            try
            {
                m_stanzas.StartTLS();
            }
            catch (Exception e)
            {
                m_reconnect = false;
                if (e.InnerException != null)
                    FireOnError(e.InnerException);
                else
                    FireOnError(e);
                return false;
            }
            m_sslOn = true;
            return true;
        }

        /// <summary>
        /// Begins compressing the XMPP stream.
        /// </summary>
        /// <returns>If True, compression was successful, otherwise False.</returns>
        protected bool StartCompression()
        {
            try
            {
                m_stanzas.StartCompression();
            }
            catch (Exception e)
            {
                m_reconnect = false;
                FireOnError(e);
                return false;
            }
            m_compressionOn = true;
            return true;
        }

        /// <summary>
        /// Gets ready for a new stream:stream by starting a new XML document.
        /// Needed after Start-TLS or compression, for example.
        /// </summary>
        protected void InitializeStream()
        {
            try
            {
                m_stanzas.InitializeStream();
            }
            catch (Exception e)
            {
                FireOnError(e);
            }
        }

        /// <summary>
        /// Sends a new XMPP stream header.
        /// </summary>
        protected void SendNewStreamHeader()
        {
            jabber.protocol.stream.Stream str = new jabber.protocol.stream.Stream(m_doc, NS);
            str.To = new JID((string)this[Options.TO]);
            str.Version = "1.0";
            m_stanzas.WriteStartTag(str);
            InitializeStream();
        }

        /// <summary>
        /// Informs the client of XMPP stream errors through the OnError event.
        /// </summary>
        /// <param name="e">Error that occurred.</param>
        protected void FireOnError(Exception e)
        {
            m_reconnect = false;

            // ignore spurious IO errors on shutdown.
            if (((State == ClosingState.Instance) || (State == ClosedState.Instance)) &&
                ((e is System.IO.IOException) || (e.InnerException is System.IO.IOException)))
                return;

            if (OnError != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnError, new object[] { this, e });
                else
                    OnError(this, e);
            }

            if ((State != ClosingState.Instance) && (State == ClosedState.Instance))
                Close(false);
        }

        private void Reconnect(object state)
        {
            // prevent double-connects
            if (this.State == ClosedState.Instance)
                Connect();
        }

        /// <summary>
        /// Registers a callback, so that if a packet arrives that matches the given xpath expression,
        /// the callback fires.  Use <see cref="AddNamespace"/> to add namespace prefixes.
        /// </summary>
        /// <example>jc.AddCallback("self::iq[@type='result']/roster:query", new ProtocolHandler(GotRoster));</example>
        /// <param name="xpath">The xpath expression to search for</param>
        /// <param name="cb">The callback to call when the xpath matches</param>
        /// <returns>A guid that can be used to unregister the callback</returns>
        public Guid AddCallback(string xpath, ProtocolHandler cb)
        {
            CallbackData cbd = new CallbackData(xpath, cb);
            m_callbacks.Add(cbd);
            return cbd.Guid;
        }

        /// <summary>
        /// Removes a callback added with <see cref="AddCallback"/>.
        /// </summary>
        /// <param name="guid">GUID representing the callback to remove.</param>
        public void RemoveCallback(Guid guid)
        {
            int count = 0;
            foreach (CallbackData cbd in m_callbacks)
            {
                if (cbd.Guid == guid)
                {
                    m_callbacks.RemoveAt(count);
                    return;
                }
                count++;
            }
            throw new ArgumentException("Unknown Guid", "guid");
        }

        /// <summary>
        /// Adds a namespace prefix, for use with callback xpath expressions added
        /// with <see cref="AddCallback"/>.
        /// </summary>
        /// <param name="prefix">The prefix to use.</param>
        /// <param name="uri">The URI associated with the prefix.</param>
        public void AddNamespace(string prefix, string uri)
        {
            m_ns.AddNamespace(prefix, uri);
        }

        private void CheckAll(XmlElement elem)
        {
            foreach (CallbackData cbd in m_callbacks)
            {
                cbd.Check(this, elem);
            }
        }

        private class CallbackData
        {
            private Guid m_guid = Guid.NewGuid();
            private ProtocolHandler m_cb;
            private string m_xpath;

            public CallbackData(string xpath, ProtocolHandler cb)
            {
                Debug.Assert(cb != null);
                m_cb = cb;
                m_xpath = xpath;
            }

            public Guid Guid
            {
                get { return m_guid; }
            }

            public string XPath
            {
                get { return m_xpath; }
            }

            public void Check(XmppStream sender, XmlElement elem)
            {
                try
                {
                    XmlNode n = elem.SelectSingleNode(m_xpath, sender.m_ns);
                    if (n != null)
                    {
                        if (sender.InvokeRequired)
                            sender.CheckedInvoke(m_cb, new object[] { sender, elem });
                        else
                            m_cb(sender, elem);
                    }
                }
                catch (Exception e)
                {
                    sender.FireOnError(e);
                }
            }
        }

        #region IStanzaEventListener Members

        void IStanzaEventListener.Connected()
        {
            lock (m_stateLock)
            {
                this.State = ConnectedState.Instance;
                if ((bool)this[Options.SSL])
                    m_sslOn = true;
            }

            if (OnConnect != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnConnect, new Object[] { this, m_stanzas });
                else
                    OnConnect(this, m_stanzas);
            }

            SendNewStreamHeader();
        }

        void IStanzaEventListener.Accepted()
        {
            lock (StateLock)
            {
                Debug.Assert(this.State == AcceptingState.Instance, this.State.GetType().ToString());

                this.State = ConnectedState.Instance;
            }

            if (OnConnect != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnConnect, new object[] { this, m_stanzas });
                else
                {
                    // Um.  This cast might not be right, but I don't want to break backward compatibility
                    // if I don't have to by changing the delegate interface.
                    OnConnect(this, m_stanzas);
                }
            }
        }

        void IStanzaEventListener.BytesRead(byte[] buf, int offset, int count)
        {
            if (OnReadText != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnReadText, new object[] { this, ENC.GetString(buf, offset, count) });
                else
                    OnReadText(this, ENC.GetString(buf, offset, count));
            }
        }

        void IStanzaEventListener.BytesWritten(byte[] buf, int offset, int count)
        {
            if (OnWriteText != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnWriteText, new object[] { this, ENC.GetString(buf, offset, count) });
                else
                    OnWriteText(this, ENC.GetString(buf, offset, count));
            }
        }

        void IStanzaEventListener.StreamInit(ElementStream stream)
        {
            if (OnStreamInit != null)
            {
                // Race condition.  Make sure not to make GUI calls in OnStreamInit
                /*
                if (InvokeRequired)
                    CheckedInvoke(OnStreamInit, new object[] { this, stream });
                else
              */
                    OnStreamInit(this, stream);
            }
        }

        void IStanzaEventListener.Errored(Exception ex)
        {
            m_reconnect = false;

            lock (m_stateLock)
            {
                State = ClosedState.Instance;
                if ((m_stanzas != null) && (!m_stanzas.Acceptable))
                    m_stanzas = null;
            }

            if (OnError != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnError, new object[] { this, ex });
                else
                    OnError(this, ex);
            }

            // TODO: Figure out what the "good" errors are, and try to
            // reconnect.  There are too many "bad" errors to just let this fly.
            //TryReconnect();
        }

        void IStanzaEventListener.Closed()
        {
            lock (StateLock)
            {
                State = ClosedState.Instance;
                if ((m_stanzas != null) && (!m_stanzas.Acceptable))
                    m_stanzas = null;
                m_sslOn = false;
                m_compressionOn = false;
            }

            if (OnDisconnect != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnDisconnect, new object[] { this });
                else
                    OnDisconnect(this);
            }

            TryReconnect();
        }

        void IStanzaEventListener.DocumentStarted(XmlElement elem)
        {
            // The OnDocumentStart logic stays outside the listener, so that it can be
            // more easily overriden by subclasses.
            OnDocumentStart(m_stanzas, elem);
        }

        void IStanzaEventListener.DocumentEnded()
        {
            lock (StateLock)
            {
                State = ClosingState.Instance;
                // TODO: Validate this, with current parser:

                // No need to close stream any more.  AElfred does this for us, even though
                // the docs say it doesn't.

                //if (m_sock != null)
                //m_sock.Close();
            }
        }

        void IStanzaEventListener.StanzaReceived(XmlElement elem)
        {
            // The OnElement logic stays outside the listener, so that it can be
            // more easily overriden by subclasses.
                OnElement(m_stanzas, elem);
        }

#if MODERN
        private bool ShowCertificatePrompt(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
#if NET20 && !__MonoCS__
            CertificatePrompt cp = new CertificatePrompt((X509Certificate2)certificate, chain, sslPolicyErrors);
            return (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK);
#else
            return false;
#endif
        }
     
        bool IStanzaEventListener.OnInvalidCertificate(bedrock.net.BaseSocket sock,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (OnInvalidCertificate != null)
            {
                if ((m_invoker == null) || (!m_invoker.InvokeRequired))
                    return OnInvalidCertificate(sock, certificate, chain, sslPolicyErrors);
                try
                {
                    // Note: can't use CheckedInvoke here, since we need the return value.  We'll wait for the response.
                    return (bool)m_invoker.Invoke(OnInvalidCertificate, new object[] { sock, certificate, chain, sslPolicyErrors });
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception passed along by XmppStream: " + e.ToString());
                    return false;
                }
            }
            if ((m_invoker == null) || (!m_invoker.InvokeRequired))
                return ShowCertificatePrompt(sock, certificate, chain, sslPolicyErrors);

            return (bool)m_invoker.Invoke(new System.Net.Security.RemoteCertificateValidationCallback(ShowCertificatePrompt), new object[]{ sock, certificate, chain, sslPolicyErrors });
        }
#endif

        #endregion
    }
}
