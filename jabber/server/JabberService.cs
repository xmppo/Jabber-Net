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

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Xml;

using bedrock.net;
using bedrock.util;

using jabber.protocol;
using jabber.protocol.accept;
using jabber.protocol.stream;

namespace jabber.server
{
    /// <summary>
    /// Type of connection to the server, with respect to jabberd.
    /// This list will grow over time to include
    /// queued connections, direct (in-proc) connections, etc.
    /// </summary>
    [RCS(@"$Header$")]
    public enum ComponentType
    {
        /// <summary>
        /// Jabberd will accept the connetion; the component will
        /// initiate the connection.  </summary>
        Accept,
        /// <summary>
        /// Jabberd will connect to the component; jabberd will
        /// initiate the connection.  </summary>
        Connect
    }

    /// <summary>
    /// Received a route element
    /// </summary>
    public delegate void RouteHandler(object sender, jabber.protocol.accept.Route route);

    /// <summary>
    /// Received an XDB element.
    /// </summary>
    public delegate void XdbHandler(object sender, jabber.protocol.accept.Xdb xdb);

    /// <summary>
    /// Received a Log element.
    /// </summary>
    public delegate void LogHandler(object sender, jabber.protocol.accept.Log log);

    /// <summary>
    /// Summary description for ServerComponent.
    /// </summary>
    [RCS(@"$Header$")]
    public class JabberService : 
        jabber.connection.SocketElementStream
    {
        private string        m_secret = null;
        private ComponentType m_type   = ComponentType.Accept;
        private XmlDocument   m_doc    = new XmlDocument();

        private void init()
        {
            this.OnStreamInit += new jabber.connection.StreamHandler(JabberService_OnStreamInit);
        }

        /// <summary>
        /// Create a a connect component.
        /// </summary>
        public JabberService() : base()
        {
            init();
        }

        /// <summary>
        /// Create an accept component.  (Component connects to server)
        /// </summary>
        /// <param name="host">Jabberd host to connect to</param>
        /// <param name="port">Jabberd port to connect to</param>
        /// <param name="name">Component name</param>
        /// <param name="secret">Component secret</param>
        public JabberService(string host,
            int port,
            string name,
            string secret) : base()
        {
            this.Server = name;
            this.NetworkHost = host;
            this.Port = port;

            m_secret = secret;
            m_type = ComponentType.Accept;
            init();
        }

        /// <summary>
        /// Create a connect component. (Server connects to component)
        /// </summary>
        /// <param name="port">Port jabberd will connect to</param>
        /// <param name="name">Component name</param>
        /// <param name="secret">Component secret</param>
        public JabberService(int port, string name, string secret) : base()
        {
            this.Server = name;
            this.Port = port;
            
            m_secret = secret;
            m_type   = ComponentType.Connect;
            init();
        }

        /// <summary>
        /// We received a route packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received a route packet.")]
        public event RouteHandler OnRoute;

        /// <summary>
        /// We received an XDB packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received an XDB packet.")]
        public event XdbHandler OnXdb;

        /// <summary>
        /// We received a Log packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received a Log packet.")]
        public event LogHandler OnLog;

        /// <summary>
        /// The service name.  Needs to be in the id attribute in the
        /// jabber.xml file.  </summary>
        [Description("The service name.  The id attribute in the jabber.xml file.")]
        [DefaultValue(null)]
        [Category("Component")]
        public string ComponentID
        {
            get { return Server; }
            set { Server = value; }
        }

        /// <summary>
        /// The name of the server to connect to.  
        /// </summary>
        [Description("The name of the Jabber server.")]
        [DefaultValue("jabber.com")]
        [Category("Jabber")]
        [Browsable(false)]
        public override string Server
        {
            get { return base.Server; }
            set { base.Server = value; }
        }

        /// <summary>
        /// Component secret.
        /// </summary>
        [Description("Component secret.")]
        [DefaultValue(null)]
        [Category("Component")]
        public string Secret
        {
            get { return m_secret; }
            set { m_secret = value; }
        }

        /// <summary>
        /// Is this an outgoing connection (base_accept), or an incoming
        /// connection (base_connect).
        /// </summary>
        [Description("Is this an outgoing connection (base_accept), or an incoming connection (base_connect).")]
        [DefaultValue(ComponentType.Accept)]
        [Category("Component")]
        public ComponentType Type
        {
            get { return m_type; }
            set 
            { 
                if (m_type != value)
                {
                    m_type = value; 
                    if (m_type == ComponentType.Connect)
                    {
                        this.AutoReconnect = 0;
                    }
                }
            }
        }

        /// <summary>
        /// The stream namespace for this connection.
        /// </summary>
        [Browsable(false)]
        protected override string NS
        {
            get { return URI.ACCEPT; }
        }

        /// <summary>
        /// Connect to the jabberd, or wait for it to connect to us.
        /// Either way, this call returns immediately.
        /// </summary>
        /// <param name="address">The address to connect to.</param>
        public void Connect(bedrock.net.Address address)
        {
            this.Server = address.Hostname;
            this.Port = address.Port;

            Connect();
        }

        /// <summary>
        /// Connect to the jabberd, or wait for it to connect to us.
        /// Either way, this call returns immediately.
        /// </summary>
        public override void Connect()
        {
            if (m_type == ComponentType.Accept)
                base.Connect();
            else
            {
                BeginAccept();
            }
        }

        /// <summary>
        /// Got the stream:stream.  Start the handshake.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        protected override void OnDocumentStart(object sender, System.Xml.XmlElement tag)
        {
            base.OnDocumentStart(sender, tag);

            lock (StateLock)
            {
                State = HandshakingState.Instance;
            }

            if (m_type == ComponentType.Accept)
            {
                Stream str = tag as Stream;
                Handshake hand = new Handshake(m_doc);
                hand.SetAuth(m_secret, str.ID);
                Write(hand);
            }
            else
            {
                Stream s = new Stream(m_doc, URI.ACCEPT);
                s.From = Server;
                StreamID = s.ID;
                Write(s.StartTag());
            }
        }

        private void Handshake(System.Xml.XmlElement tag)
        {
            Handshake hs = tag as Handshake;

            if (hs == null)
            {
                FireOnError(new System.Security.SecurityException("Bad protocol.  Needs handshake."));
                return;
            }

            if (m_type == ComponentType.Accept)
                IsAuthenticated = true;
            else
            {       
                string test = hs.Digest;
                string good = Element.ShaHash(StreamID, m_secret);
                if (test == good)
                {
                    IsAuthenticated = true;
                    Write(new Handshake(this.Document));
                }
                else
                {
                    Write(new Error(this.Document));
                    FireOnError(new System.Security.SecurityException("Bad handshake."));
                }
            }
        }

        /// <summary>
        /// Received an element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        protected override void OnElement(object sender, System.Xml.XmlElement tag)
        {
            lock (StateLock)
            {
                if (State == HandshakingState.Instance)
                {
                    // sets IsConnected
                    Handshake(tag);
                    return;
                }
            }

            base.OnElement(sender, tag);

            if (OnRoute != null)
            {
                Route route = tag as Route;
                if (route != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnRoute, new object[] {this, route});
                    else
                        OnRoute(this, route);
                }
            }
            // TODO: add XdbTracker stuff
            if (OnXdb != null)
            {
                Xdb xdb = tag as Xdb;
                if (xdb != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnXdb, new object[] {this, xdb});
                    else
                        OnXdb(this, xdb);
                }
            }
            if (OnLog != null)
            {
                Log log = tag as Log;
                if (log != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnLog, new object[] {this, log});
                    else
                        OnLog(this, log);
                }
            }
        }

        private void JabberService_OnStreamInit(Object sender, ElementStream stream)
        {
            stream.AddFactory(new jabber.protocol.accept.Factory());
        }
    }

    /// <summary>
    /// Waiting for handshake result.
    /// </summary>
    [RCS(@"$Header$")]
    public class HandshakingState : jabber.connection.BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new HandshakingState();
    }

    /// <summary>
    /// Waiting for socket connection.
    /// </summary>
    [RCS(@"$Header$")]
    public class AcceptingState : jabber.connection.BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new AcceptingState();
    }
}
