/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
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
        private string        m_name   = null;
        private string        m_secret = null;
        private ComponentType m_type   = ComponentType.Accept;
        private XmlDocument   m_doc    = new XmlDocument();

        /// <summary>
        /// Create a a connect component.
        /// </summary>
        public JabberService() : base()
        {
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
            this.Server = host;
            this.Port = port;
            
            m_name   = name;
            m_secret = secret;
            m_type = ComponentType.Accept;
        }

        /// <summary>
        /// Create a connect component. (Server connects to component)
        /// </summary>
        /// <param name="port">Port jabberd will connect to</param>
        /// <param name="name">Component name</param>
        /// <param name="secret">Component secret</param>
        public JabberService(int port, string name, string secret) : base()
        {
            this.Port = port;
            
            m_name   = name;
            m_secret = secret;
            m_type   = ComponentType.Connect;
        }

        /// <summary>
        /// Initialize the element stream.
        /// </summary>
        protected override void InitializeStream()
        {
            base.InitializeStream();

            AddFactory(new jabber.protocol.accept.Factory());
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
            get { return m_name; }
            set { m_name = value; }
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
                m_type = value; 
                if (m_type == ComponentType.Connect)
                    this.AutoReconnect = -1;
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
        /// The host for the to attribute of the stream:stream for this connection.
        /// </summary>
        [Browsable(false)]
        protected override string Host
        {
            get { return m_name; }
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
                s.From = m_name;
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
                    CheckedInvoke(OnRoute, new object[] {this, route});
            }
            // TODO: add XdbTracker stuff
            if (OnXdb != null)
            {
                Xdb xdb = tag as Xdb;
                if (xdb != null)
                    CheckedInvoke(OnXdb, new object[] {this, xdb});
            }
            if (OnLog != null)
            {
                Log log = tag as Log;
                if (log != null)
                    CheckedInvoke(OnLog, new object[] {this, log});
            }
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
