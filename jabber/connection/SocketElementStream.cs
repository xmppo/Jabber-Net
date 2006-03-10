/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using System.Xml;
using bedrock.util;
using bedrock.net;

using jabber.protocol;
using jabber.protocol.stream;
using jabber.connection.sasl;

#if NET20 || __MonoCS__
using System.Security.Cryptography.X509Certificates;
#elif !NO_SSL
using Org.Mentalis.Security.Certificates;
#endif

namespace jabber.connection
{
    /// <summary>
    /// The types of proxies we support.
    /// </summary>
    public enum ProxyType
    {
        /// <summary>
        /// no proxy
        /// </summary>
        None,
        /// <summary>
        /// socks4 as in http://archive.socks.permeo.com/protocol/socks4.protocol
        /// </summary>
        Socks4,
        /// <summary>
        /// socks5 as in http://archive.socks.permeo.com/rfc/rfc1928.txt
        /// </summary>
        Socks5,
        /// <summary>
        /// shttp as in http://rfc-2660.rfc-list.com/rfc-2660.htm
        /// </summary>
        SHTTP,
        /// <summary>
        /// HTTP Polling, as in http://www.jabber.org/jeps/jep-0025.html
        /// </summary>
        HTTP_Polling
    }

    /// <summary>
    /// A handler for events that happen on an ElementStream.
    /// </summary>
    public delegate void StreamHandler(Object sender, ElementStream stream);

    /// <summary>
    /// Summary description for SocketElementStream.
    /// </summary>
    [RCS(@"$Header$")]
    abstract public class SocketElementStream :
        System.ComponentModel.Component,
        ISocketEventListener
    {
        /// <summary>
        /// Character encoding.  UTF-8.
        /// </summary>
        protected static readonly System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private SocketWatcher m_watcher = null;
        private BaseSocket m_sock = null;
        private BaseSocket m_accept = null;
        private XmlDocument m_doc = new XmlDocument();
        private ElementStream m_stream = null;
        private BaseState m_state = ClosedState.Instance;
        private string m_server = null;
        private string m_to = "jabber.com";
        private string m_streamID = null;
        private object m_stateLock = new object();
        private ArrayList m_callbacks = new ArrayList();
        private int m_keepAlive = 20000;
        private Timer m_timer = null;
        private Timer m_reconnectTimer = null;

        private int m_port = 5222;
        private int m_autoReconnect = 30000;
        private bool m_reconnect = false;

        private ProxyType m_ProxyType = ProxyType.None;
        private string m_ProxyHost = null;
        private int m_ProxyPort = 1080;
        private string m_ProxyUsername = null;
        private string m_ProxyPassword = null;
        private bool m_ssl = false;
        private bool m_sslOn = false;
        private bool m_autoStartTLS = true;
        private bool m_plaintext = false;

        // XMPP v1 stuff
        private string m_serverVersion = null;
        private bool m_requireSASL = false;
        private SASLProcessor m_saslProc = null;

        private XmlNamespaceManager m_ns;

        private ISynchronizeInvoke m_invoker = null;
        //private XmlTextWriter m_writer = new XmlTextWriter(Console.Out);

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = new System.ComponentModel.Container();

        /// <summary>
        /// Required for Windows.Forms Class Composition Designer support
        /// </summary>
        /// <param name="container"></param>
        public SocketElementStream(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            m_watcher = new SocketWatcher();
            m_watcher.Synchronous = false;
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Create a SocketElementStream
        /// </summary>
        public SocketElementStream()
        {
            m_watcher = new SocketWatcher();
            m_watcher.Synchronous = false;
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Create a SocketElementStream out of an accepted socket.
        /// </summary>
        /// <param name="aso"></param>
        public SocketElementStream(BaseSocket aso)
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
        public SocketElementStream(SocketWatcher watcher)
        {
            m_watcher = watcher;
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Set up the element stream.  This is the place to add factories.
        /// </summary>
        protected virtual void InitializeStream()
        {
            m_stream = new AsynchElementStream();
            m_stream.OnDocumentStart += new ProtocolHandler(OnDocumentStart);
            m_stream.OnDocumentEnd += new bedrock.ObjectHandler(OnDocumentEnd);
            m_stream.OnElement += new ProtocolHandler(OnElement);
            m_stream.OnError += new bedrock.ExceptionHandler(m_stream_OnError);

            if (OnStreamInit != null)
                OnStreamInit(this, m_stream);
        }

        /// <summary>
        /// Text was written to the server.  Use for debugging only.  
        /// Will NOT be complete nodes at a time.
        /// </summary>
        [Category("Debug")]
        public event bedrock.TextHandler OnWriteText;

        /// <summary>
        /// Text was read from the server.  Use for debugging only.  
        /// Will NOT be complete nodes at a time.
        /// </summary>
        [Category("Debug")]
        public event bedrock.TextHandler OnReadText;

        /// <summary>
        /// A new stream was initialized.  Add your packet factories to it.
        /// </summary>
        [Category("Stream")]
        public event StreamHandler OnStreamInit;

        /// <summary>
        /// Some error occurred when processing.  
        /// The connection has been closed.
        /// </summary>
        [Category("Stream")]
        public event bedrock.ExceptionHandler OnError;

        /// <summary>
        /// Get notified for every jabber packet.  
        /// This is a union of OnPresence, OnMessage, and OnIQ.
        /// Use this *or* the others, but not both, as a matter of style.
        /// </summary>
        [Category("Stream")]
        public event ProtocolHandler OnProtocol;

        /// <summary>
        /// Get notified of the stream header, as a packet.  Can be called multiple
        /// times for a single session, with XMPP.
        /// </summary>
        [Category("Stream")]
        public event ProtocolHandler OnStreamHeader;

        /// <summary>
        /// Get notified of the start of a SASL handshake.
        /// </summary>
        protected event SASLProcessorHandler OnSASLStart;

        /// <summary>
        /// Get notified of the end of a SASL handshake.
        /// </summary>
        protected event FeaturesHandler OnSASLEnd;

        /// <summary>
        /// Get notified of a SASL error.
        /// </summary>
        protected event SASLProcessorHandler OnSASLFailure;

        /// <summary>
        /// We received a stream:error packet.
        /// </summary>
        [Category("Stream")]
        [Description("We received stream:error packet.")]
        public event ProtocolHandler OnStreamError;

        /// <summary>
        /// The connection is complete, and the user is authenticated.
        /// </summary>
        [Category("Stream")]
        public event bedrock.ObjectHandler OnAuthenticate;

        /// <summary>
        /// The connection is connected, but no stream:stream has been sent, yet.
        /// </summary>
        [Category("Stream")]
        public event bedrock.net.AsyncSocketHandler OnConnect;

        /// <summary>
        /// The connection is disconnected
        /// </summary>
        [Category("Stream")]
        public event bedrock.ObjectHandler OnDisconnect;

        /// <summary>
        /// The name of the server to connect to.  
        /// </summary>
        [Description("The name of the Jabber server.")]
        [DefaultValue("jabber.com")]
        [Category("Jabber")]
        public virtual string Server
        {
            get { return m_to; }
            set { m_to = value; }
        }

        /// <summary>
        /// The address to use on the "to" attribute of the stream:stream.  
        /// If using HTTP polling, put the URL here.  If not, you can put
        /// the network hostname or IP address of the server to connect to.
        /// If none is specified, the Server will be used.
        /// </summary>
        [Description("")]
        [DefaultValue(null)]
        [Category("Jabber")]
        public string NetworkHost
        {
            get { return m_server; }
            set { m_server = value; }
        }

        /// <summary>
        /// The TCP port to connect to.
        /// </summary>
        [Description("The TCP port to connect to.")]
        [DefaultValue(5222)]
        [Category("Jabber")]
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        /// <summary>
        /// Allow plaintext authentication?
        /// </summary>
        [Description("Allow plaintext authentication?")]
        [DefaultValue(false)]
        [Category("Jabber")]
        public bool PlaintextAuth
        {
            get { return m_plaintext; }
            set { m_plaintext = value; }
        }

        /// <summary>
        /// Is the current connection SSL/TLS protected?
        /// </summary>
        [Description("Is the current connection SSL/TLS protected?")]
        [DefaultValue(false)]
        [Category("Jabber")]
        public bool SSLon
        {
            get { return m_sslOn; }
        }

        /// <summary>
        /// Do SSL3/TLS1 on startup.
        /// </summary>
        [Description("Do SSL3/TLS1 on startup.")]
        [DefaultValue(false)]
        [Category("Jabber")]
        public bool SSL
        {
            get { return m_ssl; }
            set
            {
#if NO_SSL
                Debug.Assert(!value, "SSL support not compiled in");
#endif
                m_ssl = value;
            }
        }

        /// <summary>
        /// Allow Start-TLS on connection, if the server supports it
        /// </summary>
        [Browsable(false)]
        public bool AutoStartTLS
        {
            get { return m_autoStartTLS; }
            set { m_autoStartTLS = value; }
        }

#if NET20 || __MonoCS__
        /// <summary>
        /// The certificate to be used for the local side of sockets, with SSL on.
        /// </summary>
        [Browsable(false)]
        public X509Certificate LocalCertificate
        {
            get { return (m_watcher == null) ? null : m_watcher.LocalCertificate; }
            set { if (m_watcher != null) m_watcher.LocalCertificate = value; }
        }

        /// <summary>
        /// Set the certificate to be used for accept sockets.  To
        /// generate a test .pfx file using openssl, add this to
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
        /// cert on your box.  If you have IIS installed with a cert,
        /// this might just go... 
        /// </summary>
        /// <param name="filename">A .pfx or .cer file</param>
        /// <param name="password">The password, if this is a .pfx
        /// file, null if .cer file.</param> 
        public void SetCertificateFile(string filename,
                                       string password)
        {
            if (m_watcher != null)
                m_watcher.SetCertificateFile(filename, password);
        }
#elif !NO_SSL
        /// <summary>
        /// The certificate to be used for the local side of sockets,
        /// with SSL on. 
        /// </summary>
        [Browsable(false)]
        public Certificate LocalCertificate
        {
            get { return (m_watcher == null) ?
                    null : m_watcher.LocalCertificate; }
            set { if (m_watcher != null) m_watcher.LocalCertificate = value; }
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
            if (m_watcher != null)
                m_watcher.SetCertificateFile(filename, password);
        }
#endif

        /// <summary>
        /// Invoke() all callbacks on this control.
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
        /// Time, in seconds, between keep-alive spaces.
        /// </summary>
        [Description("Time, in seconds, between keep-alive spaces.")]
        [Category("Jabber")]
        [DefaultValue(20f)]
        public float KeepAlive
        {
            get { return m_keepAlive / 1000f; }
            set
            {
                lock (m_stateLock)
                {
                    m_keepAlive = (int)(value * 1000f);
                    if (value <= 0)
                    {
                        m_timer.Change(Timeout.Infinite, Timeout.Infinite);
                        m_timer.Dispose();
                        m_timer = null;
                    }
                    else
                    {
                        m_timer = new Timer(new TimerCallback(DoKeepAlive), null, m_keepAlive, m_keepAlive);
                    }
                }
            }
        }

        /// <summary>
        /// Seconds before automatically reconnecting if the connection drops.  -1 to disable, 0 for immediate.
        /// </summary>
        [Description("Automatically reconnect a connection.")]
        [DefaultValue(30)]
        [Category("Automation")]
        public float AutoReconnect
        {
            get { return m_autoReconnect / 1000f; }
            set { m_autoReconnect = (int)(value * 1000f); }
        }

        /// <summary>
        /// the type of proxy... none, socks5
        /// </summary>
        [Description("The type of proxy... none, socks5, etc.")]
        [DefaultValue(ProxyType.None)]
        [Category("Proxy")]
        public ProxyType Proxy
        {
            get { return m_ProxyType; }
            set { m_ProxyType = value; }
        }

        /// <summary>
        /// the host running the proxy
        /// </summary>
        [Description("the host running the proxy")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyHost
        {
            get { return m_ProxyHost; }
            set { m_ProxyHost = value; }
        }

        /// <summary>
        /// the port to talk to the proxy host on
        /// </summary>
        [Description("the port to talk to the proxy host on")]
        [DefaultValue(1080)]
        [Category("Proxy")]
        public int ProxyPort
        {
            get { return m_ProxyPort; }
            set { m_ProxyPort = value; }
        }

        /// <summary>
        /// the auth username for the socks5 proxy
        /// </summary>
        [Description("the auth username for the socks5 proxy")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyUsername
        {
            get { return m_ProxyUsername; }
            set { m_ProxyUsername = value; }
        }

        /// <summary>
        /// the auth password for the socks5 proxy
        /// </summary>
        [Description("the auth password for the socks5 proxy")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyPassword
        {
            get { return m_ProxyPassword; }
            set { m_ProxyPassword = value; }
        }

        /// <summary>
        /// The id attribute from the stream:stream element sent by the server.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public string StreamID
        {
            get { return m_streamID; }
            set { m_streamID = value; }
        }

        /// <summary>
        /// The outbound document.
        /// </summary>
        [Browsable(false)]
        public XmlDocument Document
        {
            get { return m_doc; }
        }

        /// <summary>
        /// The current state of the connection.  
        /// Lock on StateLock before accessing.
        /// </summary>
        [Browsable(false)]
        protected virtual BaseState State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// A lock for the state info.
        /// </summary>
        [Browsable(false)]
        protected object StateLock
        {
            get { return m_stateLock; }
        }

        private string GetHost()
        {
            if ((m_server == null) || (m_server == ""))
                return m_to;
            return m_server;
        }

        /// <summary>
        /// The expected X.509 CN for the server.
        /// </summary>
        /// <returns></returns>
        protected virtual string ServerIdentity
        {
            get { return m_to; }
        }

        /// <summary>
        /// Have we authenticated?  Locks on StateLock
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool IsAuthenticated
        {
            get
            {
                lock (StateLock)
                {
                    return (m_state == RunningState.Instance);
                }
            }
            set
            {
                bool close = false;
                lock (StateLock)
                {
                    if (value)
                    {
                        m_state = RunningState.Instance;
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
            }
        }

        /// <summary>
        /// The namespace for this connection.
        /// </summary>
        [Browsable(false)]
        protected abstract string NS
        {
            get;
        }

        /// <summary>
        /// Is SASL required?  This will default to true in the future.
        /// </summary>
        [Description("Is SASL required?  This will default to true in the future.")]
        [DefaultValue(false)]
        public bool RequiresSASL
        {
            get { return m_requireSASL; }
            set { m_requireSASL = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("The version string returned in the server's open stream element")]
        [DefaultValue(null)]
        public string ServerVersion
        {
            get { return m_serverVersion; }
        }

#if false
        /// <summary>
        /// Add new packet types that the incoming stream knows how to create.  
        /// If you create your own packet types, create a packet factory as well. 
        /// </summary>
        /// <param name="factory"></param>
        public void AddFactory(IPacketTypes factory)
        {
            m_stream.AddFactory(factory);
        }

        /// <summary>
        /// Add a type to the packet factory.
        /// </summary>
        /// <param name="localName">Local Name (e.g. query)</param>
        /// <param name="ns">Namespace URI (e.g. jabber:iq:roster)</param>
        /// <param name="t">Type to create</param>
        public void AddType(string localName, string ns, Type t)
        {
            m_stream.AddType(localName, ns, t);
        }
#endif

        /// <summary>
        /// Write these bytes to the socket.
        /// </summary>
        /// <param name="buf"></param>
        public virtual void Write(byte[] buf)
        {
            Debug.Assert(m_sock != null);
            m_sock.Write(buf);
        }

        /// <summary>
        /// Write this string to the socket, encoded as UTF-8.
        /// </summary>
        /// <param name="buf"></param>
        public virtual void Write(string buf)
        {
            Debug.Assert(m_sock != null);
            m_sock.Write(ENC.GetBytes(buf));
        }

        /// <summary>
        /// Send the given packet to the server.
        /// </summary>
        /// <param name="elem"></param>
        public virtual void Write(XmlElement elem)
        {
            Write(elem.OuterXml);
        }

        /// <summary>
        /// Start connecting to the server.  This is async.
        /// </summary>
        public virtual void Connect()
        {
            Debug.Assert(m_port > 0);
            Debug.Assert(m_to != null);
            m_sslOn = m_ssl;

            lock (StateLock)
            {
                m_state = ConnectingState.Instance;
                m_reconnect = (m_autoReconnect >= 0);

                ProxySocket proxy = null;
                switch (m_ProxyType)
                {
                case ProxyType.Socks4:
                    proxy = new Socks4Proxy(this);
                    break;

                case ProxyType.Socks5:
                    proxy = new Socks5Proxy(this);
                    break;

                case ProxyType.SHTTP:
                    proxy = new ShttpProxy(this);
                    break;

                case ProxyType.HTTP_Polling:
                    JEP25Socket j25s = new JEP25Socket(this);
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

                case ProxyType.None:
                    m_sock = new AsyncSocket(m_watcher, this, m_sslOn, false);
                    break;

                default:
                    throw new ArgumentException("no handler for proxy type: " + m_ProxyType, "ProxyType");
                }

                if (proxy != null)
                {
                    proxy.Socket = new AsyncSocket(m_watcher, proxy);
                    proxy.Host = m_ProxyHost;
                    proxy.Port = m_ProxyPort;
                    proxy.Username = m_ProxyUsername;
                    m_sock = proxy;
                }

                Address addr = new Address(GetHost(), m_port);
                m_sock.Connect(addr, this.ServerIdentity);
            }
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
                        m_autoReconnect,
                        System.Threading.Timeout.Infinite);
            }
        }

        /// <summary>
        /// Close down the connection, as gracefully as possible.
        /// </summary>
        public virtual void Close()
        {
            Close(true);
        }

        /// <summary>
        /// Close down the connection
        /// </summary>
        /// <param name="clean">true for graceful shutdown</param>
        public virtual void Close(bool clean)
        {
            bool doClose = false;
            bool doStream = false;
            lock (StateLock)
            {
                if ((m_state == RunningState.Instance) && (clean))
                {
                    m_reconnect = false;
                    doStream = true;
                }
                if (m_state != ClosedState.Instance)
                {
                    m_state = ClosingState.Instance;
                    doClose = true;
                }
            }
            if (doStream && (m_sock != null))
                Write("</stream:stream>");
            if (doClose && (m_sock != null))
                m_sock.Close();
        }

        /// <summary>
        /// Invokes the given method on the Invoker, and does some exception handling.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
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
                Debug.WriteLine("Exception passed along by SocketElementStream: " + e.ToString());
                throw e.InnerException;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception in SocketElementStream: " + e.ToString());
                throw e;
            }
        }

        /// <summary>
        /// To call callbacks, do we need to call Invoke to get onto the GUI thread?
        /// Only if InvokeControl is set, and we aren't on the GUI thread already.
        /// </summary>
        /// <returns></returns>
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
        /// Got the initial start tag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag">The start tag as if it had been a full element, 
        /// with no child elements.  This can now be called multiple times for a single
        /// session.</param>
        protected virtual void OnDocumentStart(object sender, System.Xml.XmlElement tag)
        {
            bool hack = false;
            jabber.protocol.stream.Stream str = tag as jabber.protocol.stream.Stream;
            if (str == null)
                return;
            m_streamID = str.ID;
            m_serverVersion = str.Version;

            // See XMPP-core section 4.4.1.  We'll accept 1.x
            if (m_serverVersion.StartsWith("1."))
            {
                lock (m_stateLock)
                {
                    if (m_state == SASLState.Instance)
                        // already authed.  last stream restart.
                        m_state = SASLAuthedState.Instance;
                    else
                        m_state = jabber.connection.ServerFeaturesState.Instance;
                }
            }
            else
            {
                lock (m_stateLock)
                {
                    m_state = NonSASLAuthState.Instance;
                }
                hack = true;
            }

            if (OnStreamHeader != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnStreamHeader, new object[] { this, tag });
                else
                    OnStreamHeader(this, tag);
            }
            CheckAll(tag);

            if (hack && (OnSASLStart != null))
            {
                OnSASLStart(this, null); // Hack.  Old-style auth for jabberclient.
            }
        }

        /// <summary>
        /// The end tag was received.
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnDocumentEnd(object sender)
        {
            lock (StateLock)
            {
                m_state = ClosingState.Instance;
                // No need to close stream any more.  AElfred does this for us, even though
                // the docs say it doesn't.
                //if (m_sock != null)
                //m_sock.Close();
            }
        }

        /// <summary>
        /// We received an element.  Invoke the OnProtocol event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        protected virtual void OnElement(object sender, System.Xml.XmlElement tag)
        {
            if (tag is jabber.protocol.stream.Error)
            {
                // Stream error.  Race condition!  Two cases:
                // 1) OnClose has already fired, in which case we are in ClosedState, and the reconnect timer is pending.
                // 2) OnClose hasn't fired, in which case we trick it into not starting the reconnect timer.
                lock (m_stateLock)
                {
                    if (m_state != ClosedState.Instance)
                    {
                        m_state = ClosingState.Instance;
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

            if (m_state == ServerFeaturesState.Instance)
            {
                Features f = tag as Features;
                if (f == null)
                {
                    FireOnError(new InvalidOperationException("Expecting stream:features from a version='1.0' server"));
                    return;
                }

#if !NO_SSL || NET20 || __MonoCS__
                // don't do starttls if we're already on an SSL socket.
                // bad server setup, but no skin off our teeth, we're already
                // SSL'd.  Also, start-tls won't work when polling.
                if (m_autoStartTLS && (f.StartTLS != null) && (!m_sslOn) && (m_ProxyType != ProxyType.HTTP_Polling))
                {
                    // start-tls
                    lock (m_stateLock)
                    {
                        m_state = StartTLSState.Instance;
                    }
                    this.Write(new StartTLS(m_doc));
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
                    if (ms != null)
                    {
                        lock (m_stateLock)
                        {
                            m_state = SASLState.Instance;
                        }
                        m_saslProc = SASLProcessor.createProcessor(ms.Types, m_sslOn || m_plaintext);
                        if (m_saslProc == null)
                        {

                            FireOnError(new NotImplementedException("No implemented mechanisms in: " + ms.Types.ToString()));
                            return;
                        }
                        if (OnSASLStart != null)
                            OnSASLStart(this, m_saslProc);
                        Step s = m_saslProc.step(null, this.Document);
                        if (s != null)
                            this.Write(s);
                    }

                    if (m_saslProc == null)
                    { // no SASL mechanisms.  Try iq:auth.
                        if (m_requireSASL)
                        {
                            FireOnError(new SASLException("No SASL mechanisms available"));
                            return;
                        }
                        lock (m_stateLock)
                        {
                            m_state = NonSASLAuthState.Instance;
                        }
                        if (OnSASLStart != null)
                            OnSASLStart(this, null); // HACK: old-style auth for jabberclient.
                    }
                }
            }
            else if (m_state == SASLState.Instance)
            {
                if (tag is Success)
                {
                    // restart the stream again
                    SendNewStreamHeader();
                }
                else if (tag is SASLFailure)
                {
                    m_saslProc = null;
                    // TODO: Add an OnSASLAuthFailure
                    SASLFailure sf = tag as SASLFailure;
                    // TODO: I18N
                    FireOnError(new SASLException("SASL failure: " + sf.InnerXml));
                    return;
                }
                else if (tag is Step)
                {
                    Step s = m_saslProc.step(tag as Step, this.Document);
                    if (s != null)
                        Write(s);
                }
                else
                {
                    m_saslProc = null;
                    FireOnError(new SASLException("Invalid SASL protocol"));
                    return;
                }
            }
#if !NO_SSL || NET20 || __MonoCS__
            else if (m_state == StartTLSState.Instance)
            {
                switch (tag.Name)
                {
                case "proceed":
                    try
                    {
                        m_sock.StartTLS();
                    }
                    catch (Exception e)
                    {
                        m_reconnect = false;
                        FireOnError(e);
                        return;
                    }
                    m_sslOn = true;
                    SendNewStreamHeader();
                    break;
                case "failure":
                    FireOnError(new AuthenticationFailedException());
                    return;
                }
            }
#endif
            else if (m_state == SASLAuthedState.Instance)
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
        /// The SASLClient is reporting an exception
        /// </summary>
        /// <param name="e"></param>
        public void OnSASLException(ApplicationException e)
        {
            // lets throw the exception
            FireOnError(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void OnSASLException(string message)
        {
            // lets throw it!
            FireOnError(new ApplicationException(message));
        }

        /// <summary>
        /// Send a stream:stream
        /// </summary>
        protected void SendNewStreamHeader()
        {
            jabber.protocol.stream.Stream str = new jabber.protocol.stream.Stream(m_doc, NS);
            str.To = m_to;
            str.Version = "1.0";
            Write(str.StartTag());
            m_timer.Change(m_keepAlive, m_keepAlive);

            InitializeStream();

            if (m_state == ConnectedState.Instance)
                m_sock.RequestRead();
        }

        /// <summary>
        /// Fire the OnError event.
        /// </summary>
        /// <param name="e"></param>
        protected void FireOnError(Exception e)
        {
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

        /// <summary>
        /// Start accepting connections.
        /// </summary>
        protected virtual void BeginAccept()
        {
            lock (StateLock)
            {
                this.State = AcceptingState.Instance;
                m_accept = m_watcher.CreateListenSocket(this, new Address(this.Port));
            }

            m_accept.RequestAccept();
        }

        #region Implementation of ISocketEventListener
        /// <summary>
        /// An accept socket is about to be bound, or a connect socket is about to connect, 
        /// or an incoming socket just came in.  Use this as an opportunity to 
        /// </summary>
        /// <param name="new_sock">The new socket that is about to be connected.</param>
        void ISocketEventListener.OnInit(BaseSocket new_sock)
        {
        }

        ISocketEventListener ISocketEventListener.GetListener(BaseSocket listen_sock)
        {
            return this;
        }

        bool ISocketEventListener.OnAccept(bedrock.net.BaseSocket newsocket)
        {
            lock (StateLock)
            {
                Debug.Assert(this.State == AcceptingState.Instance, this.State.GetType().ToString());

                this.State = ConnectedState.Instance;
            }

            // Don't accept any more connections until this one closes
            if (m_accept != null)
            {
                m_accept.Close();
            }

            m_sock = newsocket;
            InitializeStream();

            if (OnConnect != null)
            {
                Debug.Assert(m_sock is AsyncSocket);

                if (InvokeRequired)
                    CheckedInvoke(OnConnect, new object[] { this, m_sock });
                else
                {
                    // Um.  This cast might not be right, but I don't want to break backward compatibility 
                    // if I don't have to by changing the delegate interface.
                    OnConnect(this, (AsyncSocket)m_sock);
                }
            }

            m_sock.RequestRead();

            return false;
        }

        private void ReadComplete(object sender, byte[] buf, int offset, int count)
        {
            m_timer.Change(m_keepAlive, m_keepAlive);

            if (OnReadText != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnReadText, new object[] { m_sock, ENC.GetString(buf, offset, count) });
                else
                    OnReadText(m_sock, ENC.GetString(buf, offset, count));
            }
        }

        bool ISocketEventListener.OnRead(bedrock.net.BaseSocket sock, byte[] buf, int offset, int count)
        {
            ReadComplete(sock, buf, offset, count);
            ((AsynchElementStream)m_stream).Push(buf, offset, count);

            return true;
        }

        void ISocketEventListener.OnWrite(bedrock.net.BaseSocket sock, byte[] buf, int offset, int count)
        {
            m_timer.Change(m_keepAlive, m_keepAlive);

            if (OnWriteText != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnWriteText, new object[] { sock, ENC.GetString(buf, offset, count) });
                else
                    OnWriteText(sock, ENC.GetString(buf, offset, count));
            }
        }

        void ISocketEventListener.OnError(bedrock.net.BaseSocket sock, System.Exception ex)
        {
            m_reconnect = false;

            lock (m_stateLock)
            {
                m_timer.Change(Timeout.Infinite, Timeout.Infinite);

                m_state = ClosedState.Instance;
                m_sock = null;
            }

            if (OnError != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnError, new object[] { sock, ex });
                else
                    OnError(sock, ex);
            }

            // TODO: Figure out what the "good" errors are, and try to 
            // reconnect.  There are too many "bad" errors to just let this fly.
            //TryReconnect();
        }

        void ISocketEventListener.OnConnect(bedrock.net.BaseSocket sock)
        {
            Debug.Assert(sock != null);

            lock (m_stateLock)
            {
                this.State = ConnectedState.Instance;
            }

            if (OnConnect != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnConnect, new Object[] { this, sock });
                else
                    OnConnect(this, sock);
            }
            SendNewStreamHeader();
        }

        private void Reconnect(object state)
        {
            // prevent double-connects
            if (this.State == ClosedState.Instance)
                Connect();
        }

        void ISocketEventListener.OnClose(bedrock.net.BaseSocket sock)
        {
            lock (StateLock)
            {
                m_timer.Change(Timeout.Infinite, Timeout.Infinite);
                m_state = ClosedState.Instance;
                m_sock = null;

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
        #endregion


        /// <summary>
        /// Register a callback, so that if a packet arrives that matches the given xpath expression,
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
        /// Remove a callback added with <see cref="AddCallback"/>.
        /// </summary>
        /// <param name="guid"></param>
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
        /// Add a namespace prefix, for use with callback xpath expressions added with <see cref="AddCallback"/>.
        /// </summary>
        /// <param name="prefix">The prefix to use</param>
        /// <param name="uri">The URI associated with the prefix</param>
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

        private void DoKeepAlive(object state)
        {
            lock (m_stateLock)
            {
                if (m_state != RunningState.Instance)
                {
                    return;
                }
            }
            Write(new byte[] { 32 });
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

            public void Check(SocketElementStream sender, XmlElement elem)
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

        private void m_stream_OnError(object sender, Exception ex)
        {
            FireOnError(ex);
            TryReconnect();
        }
    }
}
