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
using jabber.connection.sasl;

namespace jabber.client
{
    /// <summary>
    /// Informs the client that a presence packet has been received.
    /// </summary>
    public delegate void PresenceHandler(Object sender, Presence pres);
    /// <summary>
    /// Informst the client that a message has been received.
    /// </summary>
    public delegate void MessageHandler(Object sender, Message msg);
    /// <summary>
    /// Informs the client that an IQ has been received.
    /// </summary>
    public delegate void IQHandler(Object sender, IQ iq);
    /// <summary>
    /// Need more information for registration.  Return false to cancel.
    /// </summary>
    public delegate bool RegisterInfoHandler(Object sender, Register register);

    /// <summary>
    /// Provides a component for clients to use to access the XMPP server.
    /// You can install this in your Toolbox, drop onto a form, a service, and so on.
    /// This class hooks into the OnProtocol event and calls the Connect() method.
    /// </summary>
    [SVN(@"$Id$")]
    public class JabberClient : XmppStream
    {
        private static readonly object[][] DEFAULTS = new object[][] {
            new object[] {Options.RESOURCE, "Jabber.Net"},
            new object[] {Options.PRIORITY, 0},
            new object[] {Options.AUTO_LOGIN, true},
            new object[] {Options.AUTO_ROSTER, true},
            new object[] {Options.AUTO_IQ_ERRORS, true},
            new object[] {Options.AUTO_PRESENCE, true},
            new object[] {Options.PROXY_PORT, 1080},
            new object[] {Options.SRV_PREFIX, "_xmpp-client._tcp."},
        };

        private void init()
        {
            SetDefaults(DEFAULTS);

            this.OnSASLStart += new jabber.connection.sasl.SASLProcessorHandler(JabberClient_OnSASLStart);
            this.OnSASLEnd += new jabber.protocol.stream.FeaturesHandler(JabberClient_OnSASLEnd);
            this.OnSASLError += new ProtocolHandler(JabberClient_OnSASLError);
            this.OnStreamInit += new StreamHandler(JabberClient_OnStreamInit);
        }

        /// <summary>
        /// Creates a new Jabber client and associates it with the parent window.
        /// Required for Windows.Forms Class Composition Designer support
        /// </summary>
        /// <param name="container">Parent container.</param>
        public JabberClient(System.ComponentModel.IContainer container) :
            base(container)
        {
            init();
        }

        /// <summary>
        /// Creates a new JabberClient.
        /// Required for Windows.Forms Class Composition Designer support.
        /// </summary>
        public JabberClient() : base()
        {
            init();
        }

        /*
        /// <summary>
        /// Create a new JabberClient, reusing an existing SocketWatcher.
        /// </summary>
        /// <param name="watcher">SocketWatcher to use.</param>
        public JabberClient(SocketWatcher watcher) : base(watcher)
        {
            init();
        }
        */

        /// <summary>
        /// Informs the client that it received a presence packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received a presence packet.")]
        public event PresenceHandler OnPresence;

        /// <summary>
        /// Informs the client that it received a message packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received a message packet.")]
        public event MessageHandler OnMessage;

        /// <summary>
        /// Informs the client that it received an IQ packet.
        /// </summary>
        [Category("Protocol")]
        [Description("We received an IQ packet.")]
        public event IQHandler OnIQ;

        /// <summary>
        /// Informs the client that the authentication has failed. The connection is not
        /// terminated if there is an authentication error, and there
        /// is at least one event handler for this event.
        /// </summary>
        [Category("Protocol")]
        [Description("Authentication failed.")]
        public event ProtocolHandler OnAuthError;

        /// <summary>
        /// Informs the client that the presence is about to be sent.
        /// This gives a chance to modify outbound presence (fore example, entity caps).
        /// </summary>
        [Category("Protocol")]
        [Description("Presence is about to be sent.  This gives a chance to modify outbound presence (e.g. entity caps)")]
        public event PresenceHandler OnBeforePresenceOut;

        /// <summary>
        /// Informs the client that the presence has been sent.
        /// This gives a chance to send presence to other things, such as chat rooms.
        /// </summary>
        [Category("Protocol")]
        [Description("Informs the client that the presence has been sent.  This gives a chance to send presence to other things, such as chat rooms.")]
        public event PresenceHandler OnAfterPresenceOut;

        /// <summary>
        /// Determines if SutoLogin is false, and if it is time to log in.
        /// This callback will receive the results of the IQ type=get
        /// in the jabber:iq:auth namespace.  When login is completed,
        /// IsConnected property is set to true.  If there is a login error, the
        /// FireAuthError() method is called.
        /// </summary>
        [Category("Protocol")]
        [Description("AutoLogin is false, and it's time to log in.")]
        public event bedrock.ObjectHandler OnLoginRequired;

        /// <summary>
        /// Informs the client if the registration succeeded or failed.
        /// </summary>
        [Category("Protocol")]
        [Description("After calling Register(), the registration succeeded or failed.")]
        public event IQHandler OnRegistered;

        /// <summary>
        /// Allows the user to enter registration requested information before sending to the XMPP server.
        ///
        /// WARNING: Make sure you do not return from this handler until the IQ is filled in.
        /// It is now safe to call UI elements, since this callback is now on the GUI thread if
        /// the InvokeControl is set.
        /// </summary>
        [Category("Protocol")]
        [Description("After calling Register, information about the user is required.")]
        public event RegisterInfoHandler OnRegisterInfo;

        /// <summary>
        /// Retrieves/Sets the username to connect as.
        /// </summary>
        [Description("The username to connect as.")]
        [Category("Jabber")]
        public string User
        {
            get { return this[Options.USER] as string; }
            set { this[Options.USER] = value; }
        }

        /// <summary>
        /// Gets the priority for this connection.
        /// </summary>
        [Description("Priority for this connection.")]
        [Category("Jabber")]
        [DefaultValue(0)]
        public int Priority
        {
            get { return (int)this[Options.PRIORITY]; }
            set { this[Options.PRIORITY] = value; }
        }

        /// <summary>
        /// Gets or sets the password to use for connecting to the XMPP server.
        /// This may be sent across the wire plaintext if the XMPP
        /// server doesn't support digest and PlaintextAuth is set to true.
        /// </summary>
        [Description("The password to use for connecting.  " +
             "This may be sent across the wire plaintext, " +
             "if the server doesn't support digest, " +
             "and PlaintextAuth is true")]
        [Category("Jabber")]
        [PasswordPropertyText]
        public string Password
        {
            get { return this[Options.PASSWORD] as string; }
            set { this[Options.PASSWORD] = value; }
        }

        /// <summary>
        /// Allows auto-login to be used for the connection to the XMPP server if set to true.
        /// </summary>
        [Description("Automatically log in on connection.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoLogin
        {
            get { return (bool)this[Options.AUTO_LOGIN]; }
            set { this[Options.AUTO_LOGIN] = value; }
        }

        /// <summary>
        /// Retrieves the roster on connection.
        /// </summary>
        [Description("Retrieves the roster on connection.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoRoster
        {
            get { return (bool)this[Options.AUTO_ROSTER]; }
            set { this[Options.AUTO_ROSTER] = value; }
        }

        /// <summary>
        /// Sends 501/feature-not-implemented back to the client when an IQ
        /// has not been handled if set to true.
        /// </summary>
        [Description("Automatically send back 501/feature-not-implemented to IQs that have not been handled.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoIQErrors
        {
            get { return (bool)this[Options.AUTO_IQ_ERRORS]; }
            set { this[Options.AUTO_IQ_ERRORS] = value; }
        }

        /// <summary>
        /// Sends presence information when the connection has been established
        /// if set to true.
        /// </summary>
        [Description("Automatically send presence on connection.")]
        [DefaultValue(true)]
        [Category("Automation")]
        public bool AutoPresence
        {
            get { return (bool)this[Options.AUTO_PRESENCE]; }
            set { this[Options.AUTO_PRESENCE] = value; }
        }

        /// <summary>
        /// Gets or sets the connecting resource.
        /// Used to identify a unique connection.
        /// </summary>
        [Description("Gets or sets the connecting resource.  " +
             "Used to identify a unique connection.")]
        [DefaultValue("Jabber.Net")]
        [Category("Jabber")]
        public string Resource
        {
            get { return this[Options.RESOURCE] as string; }
            set { this[Options.RESOURCE] = value; }
        }

        /// <summary>
        /// Gets the stream namespace for this connection.
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
                    if (AutoRoster)
                        GetRoster();
                    if (AutoPresence)
                        Presence(PresenceType.available,
                            "online", null, Priority);
                }
            }
        }

        /// <summary>
        /// Connects to the XMPP server.  This happens asynchronously, and
        /// could take a couple of seconds to get the full handshake
        /// completed.  This will authenticate, send presence, and request
        /// roster info, if the Auto* properties are set.
        /// </summary>
        public override void Connect()
        {
            this[Options.AUTO_LOGIN_THISPASS] = this[Options.AUTO_LOGIN];
            this[Options.SERVER_ID] = this[Options.TO];
            base.Connect();
        }

        /// <summary>
        /// Closes down the connection.
        /// </summary>
        public override void Close()
        {
            if (IsAuthenticated)
            {
                Presence p = new Presence(Document);
                p.Type = PresenceType.unavailable;
                p.Status = "offline";
                Write(p);
            }
            base.Close();
        }

        /// <summary>
        /// Initiates the authentication process.
        /// </summary>
        public void Login()
        {
            Debug.Assert(User != null, "Username must not be null for XEP-78 authentication");
            Debug.Assert(Password != null, "Password must not be null for XEP-78 authentication");
            Debug.Assert(Resource != null, "Resource must not be null for XEP-78 authentication");

            this[Options.AUTO_LOGIN_THISPASS] = true;

            if (State == ManualSASLLoginState.Instance)
            {
                ProcessFeatures();
                return;
            }

            this[Options.JID] = new JID(User, Server, Resource);

            AuthIQ aiq = new AuthIQ(Document);
            aiq.Type = IQType.get;
            Auth a = aiq.Instruction;
            a.Username = User;

            lock (StateLock)
            {
                State = GetAuthState.Instance;
            }
            Tracker.BeginIQ(aiq, new IqCB(OnGetAuth), null);
        }

        /// <summary>
        /// Sends a presence packet to the XMPP server.
        /// </summary>
        /// <param name="t">The type of presence.</param>
        /// <param name="status">Determines the status of the presence.</param>
        /// <param name="show">Shows the available, away, dnd and so on status.</param>
        /// <param name="priority">Prioritizes this connection.
        /// Higher number mean higher priority. 0 minumum, 127 max.
        /// -1 means this is a presence-only connection.</param>
        public void Presence(PresenceType t,
            string status,
            string show,
            int priority)
        {
            if (IsAuthenticated)
            {
                if ((priority < -128) || (priority > 127))
                {
                    throw new ArgumentException("Priority must be -128 to 127", "priority");
                }

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

                if (OnBeforePresenceOut != null)
                    OnBeforePresenceOut(this, p);
                Write(p);
                if (OnAfterPresenceOut != null)
                    OnAfterPresenceOut(this, p);
            }
            else
            {
                throw new InvalidOperationException("Client must be authenticated before sending presence.");
            }
        }

        /// <summary>
        /// Sends a certain type of message packet to another user.
        /// </summary>
        /// <param name="t">The type of message.</param>
        /// <param name="to">The JID to send the message to.</param>
        /// <param name="body">The body of the message.</param>
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
        /// Sends a message packet to another user
        /// </summary>
        /// <param name="to">The JID to send the message to.</param>
        /// <param name="body">The body of the message.</param>
        public void Message(
            string to,
            string body)
        {
            Message(MessageType.chat, to, body);
        }

        /// <summary>
        /// Requests a new copy of the roster.
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
        /// Sends a presence subscription request and updates the roster
        /// for a new roster contact.
        /// </summary>
        /// <param name="to">The JID of the contact (required)</param>
        /// <param name="nickname">The nickname to show for the contact.</param>
        /// <param name="groups">A list of groups to put the contact in.  May be null.  Hint: new string[] {"foo", "bar"}</param>
        public void Subscribe(JID to, string nickname, string[] groups)
        {
            Debug.Assert(to != null);

            RosterIQ riq = new RosterIQ(Document);
            riq.Type = IQType.set;
            Roster r = riq.Instruction;
            Item i = r.AddItem();
            i.JID = to;
            if (nickname != null)
                i.Nickname = nickname;
            if (groups != null)
            {
                foreach (string g in groups)
                    i.AddGroup(g);
            }
            Write(riq); // don't care about result.  we should get a iq/response and a roster push.

            Presence pres = new Presence(Document);
            pres.To = to;
            pres.Type = PresenceType.subscribe;
            Write(pres);
        }

        /// <summary>
        /// Removes a contact from the roster.
        /// This will also remove the subscription for that contact being removed.
        /// </summary>
        /// <param name="to">The JID to remove</param>
        public void RemoveRosterItem(JID to)
        {
            Debug.Assert(to != null);

/*
<iq from='juliet@example.com/balcony' type='set' id='roster_4'>
  <query xmlns='jabber:iq:roster'>
    <item jid='nurse@example.com' subscription='remove'/>
  </query>
</iq>
 */
            RosterIQ riq = new RosterIQ(Document);
            riq.Type = IQType.set;
            Roster r = riq.Instruction;
            Item i = r.AddItem();
            i.JID = to;
            i.Subscription = Subscription.remove;
            Write(riq); // don't care about result.  we should get a iq/response and a roster push.
        }

        /// <summary>
        /// Requests a list of agents from the XMPP server.
        /// </summary>
        public void GetAgents()
        {
            DiscoInfoIQ diq = new DiscoInfoIQ(Document);
            diq.Type = IQType.get;
            diq.To = this.Server;
            Tracker.BeginIQ(diq, new IqCB(GotDiscoInfo), null);
        }

        private void GotDiscoInfo(object sender, IQ iq, object state)
        {
            bool error = false;
            if (iq.Type == IQType.error)
                error = true;
            else
            {
                DiscoInfo info = iq.Query as DiscoInfo;
                if (info == null)
                    error = true;
                else
                {
                    if (!info.HasFeature(URI.DISCO_ITEMS))
                        error = true;  // wow.  weird server.

                    // TODO: stash away features for this node in discomanager?
                }
            }

            if (error)
            {
                // TODO: check the error type that jabberd1.4 or XCP 2.x return
            }
        }


        /// <summary>
        /// Attempts to register a new user.  This will fire
        /// OnRegisterInfo to retrieve information about the
        /// new user, and OnRegistered when the registration
        /// is completed or failed.
        /// </summary>
        /// <param name="jid">The user to register.</param>
        public void Register(JID jid)
        {
            RegisterIQ iq = new RegisterIQ(Document);
            Register reg = iq.Instruction;
            iq.Type = IQType.get;
            iq.To = jid.Server;

            reg.Username = jid.User;
            Tracker.BeginIQ(iq, new IqCB(OnGetRegister), jid);
        }

        private void OnGetRegister(object sender, IQ iq, object data)
        {
            if (iq == null)
            {
                FireOnError(new IQTimeoutException((JID) data));
                return;
            }

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
                JID jid = (JID) data;
                iq.Type = IQType.set;
                iq.From = null;
                iq.To = jid.Server;
                iq.ID = Element.NextID();
                Register r = iq.Query as Register;
                if (r == null)
                    throw new BadProtocolException(iq, "Expected a register response");

                jabber.protocol.x.Data xdata = r["x", URI.XDATA] as jabber.protocol.x.Data;
                jabber.protocol.x.Field f;
                if (xdata != null)
                {
                    f = xdata.GetField("username");
                    if (f != null)
                        f.Val = jid.User;
                    f = xdata.GetField("password");
                    if (f != null)
                        f.Val = this.Password;
                }
                else
                {
                    r.Username = jid.User;
                    r.Password = this.Password;
                }

                bool res = true;
                if (OnRegisterInfo != null)
                {
                    if (InvokeRequired)
                        // Don't use CheckedInvoke, since we want this to be synchronous
                        res = (bool)this.InvokeControl.Invoke(OnRegisterInfo, new object[] { this, r });
                    else
                        res = OnRegisterInfo(this, r);
                    if (xdata != null)
                    {
                        f = xdata.GetField("username");
                        if (f != null)
                        {
                            this.User = f.Val;
                        }
                        f = xdata.GetField("password");
                        if (f != null)
                            this.Password = f.Val;
                    }
                    else
                    {
                        this.User = r.Username;
                        this.Password = r.Password;
                    }
                }
                if (!res)
                {
                    this.Close();
                    return;
                }
                if (xdata != null)
                    xdata.Type = jabber.protocol.x.XDataType.result;
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

        private void OnGetAuth(object sender, IQ i, object data)
        {
            if ((i == null) || (i.Type != IQType.result))
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
            Auth a = aiq.Instruction;

            if ((res["sequence"] != null) && (res["token"] != null))
            {
                a.SetZeroK(User, Password, res.Token, res.Sequence);
            }
            else if (res["digest"] != null)
            {
                a.SetDigest(User, Password, StreamID);
            }
            else if (res["password"] != null)
            {
                if (!SSLon && !this.PlaintextAuth)
                {
                    FireOnError(new AuthenticationFailedException("Plaintext authentication forbidden."));
                    return;
                }
                a.SetAuth(User, Password);
            }
            else
            {
                FireOnError(new NotImplementedException("Authentication method not implemented for:\n" + i));
                return;
            }
            if (res["resource"] != null)
                a.Resource = Resource;
            a.Username = User;

            lock (StateLock)
            {
                State = SetAuthState.Instance;
            }
            Tracker.BeginIQ(aiq, new IqCB(OnSetAuth), null);
        }

        private void OnSetAuth(object sender, IQ i, object data)
        {
            if ((i == null) || (i.Type != IQType.result))
                FireAuthError(i);
            else
                IsAuthenticated = true;
        }

        /// <summary>
        /// Sorts the XML element looking for Presence, Message, and IQ packets.
        /// </summary>
        /// <param name="sender">The object calling this method.</param>
        /// <param name="tag">The XML element containing a stanza.</param>
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

            IQ i = tag as IQ;
            if (i != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(new IQHandler(FireOnIQ) , new object[] { this, i });
                else
                    FireOnIQ(this, i);
                return;
            }
        }

        private void FireOnIQ(object sender, IQ iq)
        {
            // We know we're on the GUI thread.
            if (OnIQ != null)
                OnIQ(this, iq);

            if (AutoIQErrors)
            {
                if (!iq.Handled &&
                    iq.HasAttribute("from") &&   // Belt.  Suspenders.  Don't respond to roster pushes.
                    ((iq.Type == IQType.get) || (iq.Type == IQType.set)))
                {
                    Write(iq.GetErrorResponse(this.Document, Error.FEATURE_NOT_IMPLEMENTED));
                }
            }
        }

        /// <summary>
        /// Informs the client that an error occurred during authentication.
        /// This is public so that manual authenticators
        /// can fire errors using the same events.
        /// </summary>
        /// <param name="i">Xml element containing the error message.</param>
        public void FireAuthError(XmlElement i)
        {
            if (OnAuthError != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnAuthError, new object[] { this, i });
                else
                    OnAuthError(this, i);
            }
            else
            {
                IQ iq = i as IQ;
                if (iq != null)
                    FireOnError(new IQException(iq));
                else
                    FireOnError(new AuthenticationFailedException(i.OuterXml));
            }
        }

        void JabberClient_OnSASLError(object sender, XmlElement rp)
        {
            FireAuthError(rp);
        }

        private void LoginRequired(BaseState newState)
        {
            lock (StateLock)
            {
                State = newState;
            }

            if (OnLoginRequired != null)
            {
                if (InvokeRequired)
                    CheckedInvoke(OnLoginRequired, new object[] { this });
                else
                    OnLoginRequired(this);
            }
            else
            {
                FireOnError(new InvalidOperationException("If AutoLogin is false, you must supply a OnLoginRequired event handler"));
            }
        }

        private void JabberClient_OnSASLStart(Object sender, jabber.connection.sasl.SASLProcessor proc)
        {
            BaseState s = null;
            lock (StateLock)
            {
                s = State;
            }

            // HACK: fire OnSASLStart with state of NonSASLAuthState to initiate old-style auth.
            if (s == NonSASLAuthState.Instance)
            {
                if ((bool)this[Options.AUTO_LOGIN_THISPASS])
                    Login();
                else
                    LoginRequired(ManualLoginState.Instance);
            }
            else
            {
                if ((bool)this[Options.AUTO_LOGIN_THISPASS])
                {
                    // TODO: integrate SASL params into XmppStream params
                    proc[SASLProcessor.USERNAME] = User;
                    proc[SASLProcessor.PASSWORD] = Password;
                    proc[MD5Processor.REALM] = this.Server;
                    object creds = this[KerbProcessor.USE_WINDOWS_CREDS];
                    if (creds == null)
                        creds = false;
                    proc[KerbProcessor.USE_WINDOWS_CREDS] = creds.ToString();
                }
                else
                {
                    LoginRequired(ManualSASLLoginState.Instance);
                }
            }
        }

        private void JabberClient_OnSASLEnd(Object sender, jabber.protocol.stream.Features feat)
        {
            lock (StateLock)
            {
                State = BindState.Instance;
            }
            if (feat["bind", URI.BIND] != null)
            {
                IQ iq = new IQ(this.Document);
                iq.Type = IQType.set;

                jabber.protocol.stream.Bind bind = new jabber.protocol.stream.Bind(this.Document);
                if ((Resource != null) && (Resource != ""))
                    bind.Resource = Resource;

                iq.AddChild(bind);
                this.Tracker.BeginIQ(iq, new IqCB(GotResource), feat);
            }
            else if (feat["session", URI.SESSION] != null)
            {
                IQ iq = new IQ(this.Document);
                iq.Type = IQType.set;
                iq.AddChild(new jabber.protocol.stream.Session(this.Document));
                this.Tracker.BeginIQ(iq, new IqCB(GotSession), feat);
            }
            else
                IsAuthenticated = true;
        }

        private void GotResource(object sender, IQ iq, object state)
        {

            jabber.protocol.stream.Features feat =
                state as jabber.protocol.stream.Features;

            if (iq == null)
            {
                FireOnError(new AuthenticationFailedException("Timeout authenticating"));
                return;
            }
            if (iq.Type != IQType.result)
            {
                Error err = iq.Error;
                if (err == null)
                    FireOnError(new AuthenticationFailedException("Unknown error binding resource"));
                else
                    FireOnError(new AuthenticationFailedException("Error binding resource: " + err.OuterXml));
                return;
            }

            XmlElement bind = iq["bind", URI.BIND];
            if (bind == null)
            {
                FireOnError(new AuthenticationFailedException("No binding returned.  Server implementation error."));
                return;
            }
            XmlElement jid = bind["jid"];
            if (jid == null)
            {
                FireOnError(new AuthenticationFailedException("No jid returned from binding.  Server implementation error."));
                return;
            }
            this[Options.JID] = new JID(jid.InnerText);

            if (feat["session", URI.SESSION] != null)
            {
                IQ iqs = new IQ(this.Document);
                iqs.Type = IQType.set;
                iqs.AddChild(new jabber.protocol.stream.Session(this.Document));
                this.Tracker.BeginIQ(iqs, new IqCB(GotSession), feat);
            }
            else
                IsAuthenticated = true;
        }

        private void GotSession(object sender, IQ iq, object state)
        {
            if ((iq != null) && (iq.Type == IQType.result))
                IsAuthenticated = true;
            else
                FireOnError(new AuthenticationFailedException());
        }

        private void JabberClient_OnStreamInit(Object sender, ElementStream stream)
        {
            stream.AddFactory(new jabber.protocol.client.Factory());
            stream.AddFactory(new jabber.protocol.iq.Factory());
            stream.AddFactory(new jabber.protocol.x.Factory());

        }
    }

    /// <summary>
    /// Contains the "Getting authorization" information.
    /// </summary>
    [SVN(@"$Id$")]
    public class GetAuthState : jabber.connection.BaseState
    {
        /// <summary>
        /// Gets the instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new GetAuthState();
    }

    /// <summary>
    /// Contains the "Setting authorization" information.
    /// </summary>
    [SVN(@"$Id$")]
    public class SetAuthState : jabber.connection.BaseState
    {
        /// <summary>
        /// Gets the instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new SetAuthState();
    }

    /// <summary>
    /// Informs the client that the JabberClient is in
    /// the "Waiting for manual login" state.
    /// </summary>
    [SVN(@"$Id$")]
    public class ManualLoginState : jabber.connection.BaseState
    {
        /// <summary>
        /// Gets the instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new ManualLoginState();
    }

    /// <summary>
    /// Informs the client that the JabberClient is in
    /// the "Waiting for manual login" state, but when Login()
    /// happens, it should try SASL.
    /// </summary>
    [SVN(@"$Id$")]
    public class ManualSASLLoginState : jabber.connection.BaseState
    {
        /// <summary>
        /// Gets the instance that is always used.
        /// </summary>
        public static readonly jabber.connection.BaseState Instance = new ManualSASLLoginState();
    }

}
