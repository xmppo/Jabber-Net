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
using System.Collections;
using System.Diagnostics;
using System.Xml;

using bedrock.util;
using bedrock.net;

using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace jabber.client
{
    /// <summary>
    /// Received a presence packet
    /// </summary>
    public delegate void PresenceHandler(Object sender, Presence pres);
    /// <summary>
    /// Received a message
    /// </summary>
    public delegate void MessageHandler(Object sender, Message msg);
    /// <summary>
    /// Received an IQ
    /// </summary>
    public delegate void IQHandler(Object sender, IQ iq);

    /// <summary>
    /// A component for clients to use to access the Jabber server.
    /// Install this in your Toolbox, drop onto a form, a service,
    /// etc.  Hook into the OnProtocol event.  Call Connect().
    /// </summary>
    [RCS(@"$Header$")]
    public class JabberClient : 
        SocketElementStream
    {
        private string m_user       = null;
        private string m_password   = null;
        private string m_resource   = "Jabber.Net";
        private int    m_priority   = 0;

        private bool m_plaintext  = false;
        private bool m_autoLogin  = true;
        private bool m_autoRoster = true;
        private bool m_autoPres   = true;
        private bool m_autoAgents = true;

        private Agent[] m_agents = null;
        private IQTracker m_tracker = null;


        /// <summary>
        /// Required for Windows.Forms Class Composition Designer support
        /// </summary>
        /// <param name="container"></param>
        public JabberClient(System.ComponentModel.IContainer container) :
            base(container)
        {
            m_tracker = new IQTracker(this);
        }

        /// <summary>
        /// Required for Windows.Forms Class Composition Designer support
        /// </summary>
        public JabberClient() : base()
        {
            m_tracker = new IQTracker(this);
        }

        /// <summary>
        /// Create a new JabberClient, reusing an existing SocketWatcher.
        /// </summary>
        /// <param name="watcher">SocketWatcher to use.</param>
        public JabberClient(SocketWatcher watcher) : base(watcher)
        {
            m_tracker = new IQTracker(this);
        }

        /// <summary>
        /// We received a presence packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received a presence packet.")]
        public event PresenceHandler OnPresence;

        /// <summary>
        /// We received a message packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received a message packet.")]
        public event MessageHandler OnMessage;

        /// <summary>
        /// We received an IQ packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received an IQ packet.")]
        public event IQHandler OnIQ;

        /// <summary>
        /// Authentication failed.  The connection is not 
        /// terminated if there is an auth error and there 
        /// is at least one event handler for this event.
        /// </summary>
        [Category("Protocol")]
        [Description("Authentication failed.")]
        public event IQHandler OnAuthError;

        /// <summary>
        /// Agents query returned.  JabberClient.Agents is now valid.
        /// </summary>
        [Category("Protocol")]
        [Description("We received the answer to our agents query.")]
        public event IQHandler OnAgents;

        /// <summary>
        /// AutoLogin is false, and it's time to log in.
        /// This callback will receive the results of the IQ type=get 
        /// in the jabber:iq:auth namespace.  When login is complete, 
        /// set IsConnected to true.  If there is a login error, call 
        /// FireAuthError().
        /// </summary>
        [Category("Protocol")]
        [Description("AutoLogin is false, and it's time to log in.")]
        public event bedrock.ObjectHandler OnLoginRequired;

        /// <summary>
        /// After calling Register(), the registration succeeded or failed.
        /// </summary>
        [Category("Protocol")]
        [Description("After calling Register(), the registration succeeded or failed.")]
        public event IQHandler OnRegistered;

        /// <summary>
        /// After calling Register, information about the user is required.  Fill in the given IQ
        /// with the requested information.
        /// </summary>
        [Category("Protocol")]
        [Description("After calling Register, information about the user is required.")]
        public event IQHandler OnRegisterInfo;

        /// <summary>
        /// The username to connect as.
        /// </summary>
        [Description("The username to connect as.")]
        [Category("Jabber")]
        public string User
        {
            get { return m_user; }
            set { m_user = value; }
        }

        /// <summary>
        /// Priority for this connection.
        /// </summary>
        [Description("Priority for this connection.")]
        [Category("Jabber")]
        [DefaultValue(0)]
        public int Priority
        {
            get { return m_priority; }
            set { m_priority = value; }
        }

        /// <summary>
        /// The password to use for connecting.  
        /// This may be sent across the wire plaintext, if the 
        /// server doesn't support digest and PlaintextAuth is true.
        /// </summary>
        [Description("The password to use for connecting.  " +
             "This may be sent across the wire plaintext, " +
             "if the server doesn't support digest, " +
             "and PlaintextAuth is true")]
        [Category("Jabber")]
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
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
        /// Automatically log in on connection.
        /// </summary>
        [Description("Automatically log in on connection.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoLogin
        {
            get { return m_autoLogin; }
            set { m_autoLogin = value; }
        }
        
        /// <summary>
        /// Automatically retrieve roster on connection.
        /// </summary>
        [Description("Automatically retrieve roster on connection.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoRoster
        {
            get { return m_autoRoster; }
            set { m_autoRoster = value; }
        }

        /// <summary>
        /// Automatically send presence on connection.
        /// </summary>
        [Description("Automatically send presence on connection.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoPresence
        {
            get { return m_autoPres; }
            set { m_autoPres = value; }
        }

        /// <summary>
        /// Automatically send agents request on connection.
        /// </summary>
        [Description("Automatically send agents request on connection.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoAgents
        {
            get { return m_autoAgents; }
            set { m_autoAgents = value; }
        }
        
        /// <summary>
        /// The connecting resource.  
        /// Used to identify a unique connection.
        /// </summary>
        [Description("The connecting resource.  " + 
             "Used to identify a unique connection.")]
        [DefaultValue("Jabber.Net")]
        [Category("Jabber")]
        public string Resource
        {
            get { return m_resource; }
            set { m_resource = value; }
        }


        /// <summary>
        /// The list of agents available at the server
        /// </summary>
        [Browsable(false)]
        public Agent[] Agents
        {
            get { return m_agents; }
        }

        /// <summary>
        /// Let's track IQ packets.
        /// </summary>
        [Browsable(false)]
        public IQTracker Tracker
        {
            get { return m_tracker; }
        }

        /// <summary>
        /// The stream namespace for this connection.
        /// </summary>
        [Browsable(false)]
        protected override string NS
        {
            get { return URI.CLIENT; }
        }

        /// <summary>
        /// Are we currently connected?
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public override bool IsAuthenticated
        {
            get { return base.IsAuthenticated; }
            set
            {
                base.IsAuthenticated = value;
                if (value)
                {
                    if (m_autoAgents)
                        GetAgents();
                    if (m_autoRoster)
                        GetRoster();
                    if (m_autoPres)
                        Presence(PresenceType.available,
                            "online", null, m_priority);
                }
            }
        }

        /// <summary>
        /// A new stream was created.  Let's initialize it.
        /// </summary>
        protected override void InitializeStream()
        {
            base.InitializeStream();
            AddFactory(new jabber.protocol.client.Factory());
            AddFactory(new jabber.protocol.iq.Factory());
            AddFactory(new jabber.protocol.x.Factory());
        }

        /// <summary>
        /// Connect to the server.  This happens asynchronously, and
        /// could take a couple of seconds to get the full handshake
        /// completed.  This will auth, send presence, and request
        /// roster info, if the Auto* properties are set.
        /// </summary>
        public override void Connect()
        {
            base.Connect();
        }

        /// <summary>
        /// Close down the connection, as gracefully as possible.
        /// </summary>
        public override void Close()
        {
            if (IsAuthenticated)
            {
                Presence p = new Presence(Document);
                p.Status = "offline";
                Write(p);
            }
            base.Close();
        }

        /// <summary>
        /// Initiate the auth process.
        /// </summary>
        public void Login()
        {
            Debug.Assert(m_user != null);
            Debug.Assert(m_password != null);
            Debug.Assert(m_resource != null);

            AuthIQ aiq = new AuthIQ(Document);
            aiq.Type = IQType.get;
            Auth a = (Auth) aiq.Query;
            a.Username = m_user;

            lock (StateLock)
            {
                State = GetAuthState.Instance;
            }
            Tracker.BeginIQ(aiq, new IqCB(OnGetAuth), null);
        }

        /// <summary>
        /// Send a presence packet to the server
        /// </summary>
        /// <param name="t">What kind?</param>
        /// <param name="status">How to show us?</param>
        /// <param name="show">away, dnd, etc.</param>
        /// <param name="priority">How to prioritize this connection.
        /// Higher number mean higher priority.  0 minumum.</param>
        public void Presence(PresenceType t,
            string status,
            string show,
            int priority)
        {
            if (IsAuthenticated) 
            {
                Presence p = new Presence(Document);
                if (status != null)
                    p.Status = status;
                if (t != PresenceType.available)
                {
                    p.Type = t;
                }
                if (show != null)
                    p.Show = show;
                p.Priority = priority.ToString();
                Write(p);
            }
            else
            {
                throw new InvalidOperationException("Client must be authenticated before sending presence.");
            }
        }

        /// <summary>
        /// Send a message packet to another user
        /// </summary>
        /// <param name="t">What kind?</param>
        /// <param name="to">Who to send it to?</param>
        /// <param name="body">The message.</param>
        public void Message(MessageType t,
            string to,
            string body)
        {
            if (IsAuthenticated) 
            {
                Message msg = new Message(Document);
                msg.Type = t;
                msg.To = to;
                msg.Body = body;
                Write(msg);
            }
            else
            {
                throw new InvalidOperationException("Client must be authenticated before sending messages.");
            }
        }
 
        /// <summary>
        /// Send a message packet to another user
        /// </summary>
        /// <param name="to">Who to send it to?</param>
        /// <param name="body">The message.</param>
        public void Message(
            string to,
            string body)
        {
            Message(MessageType.chat, to, body);
        }

        /// <summary>
        /// Request a new copy of the roster.
        /// </summary>
        public void GetRoster()
        {
            if (IsAuthenticated) 
            {
                RosterIQ riq = new RosterIQ(Document);
                riq.Type = IQType.get;
                Write(riq);
            }
            else
            {
                throw new InvalidOperationException("Client must be authenticated before getting roster.");
            }
        }
        
        /// <summary>
        /// Request a list of agents from the server
        /// </summary>
        public void GetAgents()
        {
            AgentsIQ aiq = new AgentsIQ(Document);
            aiq.Type = IQType.get;
            aiq.To = this.Server;
            Tracker.BeginIQ(aiq, new IqCB(GotAgents), null);
        }

        private void GotAgents(object sender, IQ iq, object state)
        {
            AgentsQuery aq = (AgentsQuery) iq["query", URI.AGENTS];
            if (iq.Type != IQType.error)
            {
                m_agents = aq.GetAgents();
                if (OnAgents != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnAgents, new object[] {this, iq});
                    else
                        OnAgents(this, iq);
                }
            }
            else
            {
                FireOnError(new ProtocolException(iq));
            }
        }

        /// <summary>
        /// Attempt to register a new user.  This will fire OnRegisterInfo to retrieve 
        /// information about the new user, and OnRegistered when the registration is complete or failed.
        /// </summary>
        /// <param name="jid">The user to register</param>
        public void Register(JID jid)
        {
            RegisterIQ iq = new RegisterIQ(Document);
            Register reg = (Register)iq.Query;
            iq.Type = IQType.get;
            iq.To = jid.Server;

            reg.Username = jid.User;
            Tracker.BeginIQ(iq, new IqCB(OnGetRegister), jid);
        }

        private void OnGetRegister(object sender, IQ iq, object data)
        {
            if (iq.Type == IQType.error)
            {
                if (OnRegistered != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnRegistered, new object[] {this, iq});
                    else
                        OnRegistered(this, iq);
                }
            }
            else if (iq.Type == IQType.result)
            {
                if (OnRegisterInfo == null)
                    throw new InvalidOperationException("Please set OnRegisterInfo if you are going to use Register()");

                JID jid = (JID) data;
                iq.Type = IQType.set;
                iq.From = null;
                iq.To = jid.Server;
                iq.ID = Element.NextID();
                Register r = iq.Query as Register;
                Debug.Assert(r != null);
                r.Username = jid.User;

                if (InvokeRequired)
                    CheckedInvoke(OnRegisterInfo, new object[] {this, iq});
                else
                    OnRegisterInfo(this, iq);

                Tracker.BeginIQ(iq, new IqCB(OnSetRegister), jid);
            }
        }

        private void OnSetRegister(object sender, IQ iq, object data)
        {
            if (OnRegistered == null)
                return;

            if (InvokeRequired)
                CheckedInvoke(OnRegistered, new object[] {this, iq});
            else
                OnRegistered(this, iq);
        }

        /// <summary>
        /// Receieved the stream:stream.  Start the login process.
        /// TODO: allow for registration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        protected override void OnDocumentStart(object sender,
            System.Xml.XmlElement tag)
        {
            base.OnDocumentStart(sender, tag);

            if (m_autoLogin)
                Login();
            else
            {
                lock (StateLock)
                {
                    State = ManualLoginState.Instance;
                }
                if (OnLoginRequired != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnLoginRequired, new object[]{this});
                    else
                        OnLoginRequired(this);
                }
                else
                    FireOnError(new InvalidOperationException("If AutoLogin is false, you must supply a OnLoginRequired event handler"));
            }
        }

        private void OnGetAuth(object sender, IQ i, object data)
        {
            if (i.Type != IQType.result)
            {
                FireAuthError(i);
                return;
            }

            Auth res = i.Query as Auth;
            if (res == null)
            {
                FireOnError(new InvalidOperationException("Invalid IQ result type"));
                return;
            }

            AuthIQ aiq = new AuthIQ(Document);
            aiq.Type = IQType.set;
            Auth a = (Auth) aiq.Query;
                
            if ((res["sequence"] != null) && (res["token"] != null))
            {
                a.SetZeroK(m_user, m_password, res.Token, res.Sequence);
            }
            else if (res["digest"] != null)
            {
                a.SetDigest(m_user, m_password, StreamID);
            }
            else if (m_plaintext && (res["password"] != null))
            {
                a.SetAuth(m_user, m_password);
            }
            else
            {
                FireOnError(new NotImplementedException("Authentication method not implemented for:\n" + i));
            }
            if (res["resource"] != null)
                a.Resource = m_resource;
            a.Username = m_user;

            lock (StateLock)
            {
                State = SetAuthState.Instance;
            }
            Tracker.BeginIQ(aiq, new IqCB(OnSetAuth), null);
        }

        private void OnSetAuth(object sender, IQ i, object data)
        {
            if (i.Type != IQType.result)
                FireAuthError(i);
            else
                IsAuthenticated = true;
        }

        /// <summary>
        /// An element was received.
        /// Look for Presence, Message, and IQ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        protected override void OnElement(object sender, System.Xml.XmlElement tag)
        {
            base.OnElement(sender, tag);

            if (OnPresence != null)
            {
                Presence p = tag as Presence;
                if (p != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnPresence, new object[] {this, p});
                    else
                        OnPresence(this, p);
                    return;
                }
            }
            if (OnMessage != null)
            {
                Message m = tag as Message;
                if (m != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnMessage, new object[] {this, m});
                    else
                        OnMessage(this, m);
                    return;
                }
            }
            if (OnIQ != null)
            {
                IQ i = tag as IQ;
                if (i != null)
                {
                    if (InvokeRequired)
                        CheckedInvoke(OnIQ, new object[] {this, i});
                    else
                        OnIQ(this, i);
                    return;
                }
            }
        }

        /// <summary>
        /// An error occurred authenticating.
        /// This is public so that manual authenticators 
        /// can fire errors using the same events.
        /// </summary>
        /// <param name="i"></param>
        public void FireAuthError(IQ i)
        {
            if (OnAuthError != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnAuthError, new object[] {this, i});
                else
                    OnAuthError(this, i);
            }
            else
                FireOnError(new ProtocolException(i));
        }
    }

    /// <summary>
    /// Getting authorization information
    /// </summary>
    [RCS(@"$Header$")]
    public class GetAuthState : jabber.connection.BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new GetAuthState();
    }
    
    /// <summary>
    /// Setting authorization information
    /// </summary>
    [RCS(@"$Header$")]
    public class SetAuthState : jabber.connection.BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new SetAuthState();
    }
    
    /// <summary>
    /// Waiting for manual login.
    /// </summary>
    [RCS(@"$Header$")]
    public class ManualLoginState : jabber.connection.BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new ManualLoginState();
    }
}
