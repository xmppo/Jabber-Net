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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

using bedrock.util;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using jabber.client;
using jabber.protocol.x;
using System.ComponentModel.Design;

namespace jabber.connection
{

    /// <summary>
    /// An error occurred with a presence sent to a room.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="pres"></param>
    public delegate void RoomPresenceHandler(Room room, Presence pres);

    /// <summary>
    /// Notifies the client that a room configuration form has been received.
    /// </summary>
    /// <param name="room">Room associated with the configuration.</param>
    /// <param name="parent">Contains an x:data child with the form.</param>
    /// <returns>null to take the defaults, otherwise the IQ response</returns>
    public delegate IQ ConfigureRoom(Room room, IQ parent);

    /// <summary>
    /// An event, like join or leave, has happened to a room.
    /// </summary>
    /// <param name="room">The room the event is for</param>
    public delegate void RoomEvent(Room room);

    /// <summary>
    /// An event, like join or leave, has happened to a room.
    /// </summary>
    /// <param name="room">The room the event is for</param>
    /// <param name="state">State passed in by the caller, or null if none.</param>
    public delegate void RoomStateEvent(Room room, object state);

    /// <summary>
    /// A participant-related callback.
    /// </summary>
    /// <param name="room">The room the event is for</param>
    /// <param name="participants">The participants in the response</param>
    /// <param name="state">State passed in by the caller, or null if none.</param>
    public delegate void RoomParticipantsEvent(Room room, ParticipantCollection participants, object state);

    /// <summary>
    /// Manages a set of conference rooms
    /// </summary>
    [SVN(@"$Id$")]
    public class ConferenceManager : StreamComponent
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Hashtable m_rooms = new Hashtable();

        /// <summary>
        /// Creates a new conference manager.
        /// </summary>
        public ConferenceManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new conference manager in a container
        /// </summary>
        /// <param name="container">Parent container.</param>
        public ConferenceManager(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

        /// <summary> 
        /// Performs tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        /// <summary>
        /// Finished joining the room, including all potential configuration.
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event RoomEvent OnJoin;

        /// <summary>
        /// Finished leaving the room, or was kicked/banned, or the room server went down cleanly.
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event RoomPresenceHandler OnLeave;

        /// <summary>
        /// Error in response to a room join, nick change, or presence update.
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event RoomPresenceHandler OnPresenceError;

        /// <summary>
        /// Room configuration form received.  It is up to the listener call FinishConfig().
        /// The IQ in the callback is the parent of the x:data element.
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event ConfigureRoom OnRoomConfig;

        /// <summary>
        /// A message broadcast to all in the room
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event MessageHandler OnRoomMessage;

        /// <summary>
        /// A side-chat message.
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event MessageHandler OnPrivateMessage;

        /// <summary>
        /// An admin message from the room itself.  Typically status change sorts of things
        /// like kick/ban.
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event MessageHandler OnAdminMessage;

        /// <summary>
        /// A message that was sent by this user to the room, echo'd back.
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event MessageHandler OnSelfMessage;

        /// <summary>
        /// The subject of the room has been changed
        /// If set, will be added to each room created through the manager.
        /// </summary>
        [Category("Room")]
        public event MessageHandler OnSubjectChange;

        /// <summary>
        /// Joins a conference room.
        /// </summary>
        /// <param name="roomAndNick">room@conference/nick, where "nick" is the desred nickname in the room.</param>
        /// <returns>
        /// If already joined, the existing room will be returned.
        /// If not, a Room object will be returned in the joining state.
        /// </returns>
        public Room GetRoom(JID roomAndNick)
        {
            if (roomAndNick == null)
                throw new ArgumentNullException("roomAndNick");

            Room r = (Room)m_rooms[roomAndNick];
            if (r != null)
                return r;

            // If no resource specified, pick up the user's name from their JID
            if (roomAndNick.Resource == null)
                roomAndNick.Resource = m_stream.JID.User;

            r = new Room(this, this.Stream, roomAndNick);
            r.OnJoin += OnJoin;
            r.OnLeave += OnLeave;
            r.OnPresenceError += OnPresenceError;
            r.OnRoomConfig += OnRoomConfig;
            r.OnRoomMessage += OnRoomMessage;
            r.OnPrivateMessage += OnPrivateMessage;
            r.OnAdminMessage += OnAdminMessage;
            r.OnSelfMessage += OnSelfMessage;
            r.OnSubjectChange += OnSubjectChange;
            m_rooms[roomAndNick] = r;
            return r;
        }

        /// <summary>
        /// Determines whether or not the conference room is being managed
        /// by this ConferenceManager.
        /// </summary>
        /// <param name="roomAndNick">Room to look for.</param>
        /// <returns>True if the room is being managed.</returns>
        public bool HasRoom(JID roomAndNick)
        {
            return m_rooms.ContainsKey(roomAndNick);
        }

        /// <summary>
        /// Removes the room from the list.
        /// Should most often be called by the Room.Leave() method.
        /// If the room does not exist, no exception is thrown.
        /// </summary>
        /// <param name="roomAndNick">Room to remove.</param>
        public void RemoveRoom(JID roomAndNick)
        {
            m_rooms.Remove(roomAndNick);
        }

        private class UniqueState
        {
            public string Nick;
            public RoomStateEvent Callback;
            public object State;

            public UniqueState(string nick, RoomStateEvent callback, object state)
            {
                this.Nick = nick;
                this.Callback = callback;
                this.State = state;
            }
        }

        /// <summary>
        /// Get a unique room name from the given server, and create a Room
        /// object for that room with the given nick.  You'll be called back on
        /// "callback" when complete; the Room will be null if there was an error
        /// or timeout.
        /// 
        /// Note: the server should implement the feature http://jabber.org/protocol/muc#unique,
        /// or this will return an error.  To work around, just create a room with a Guid for
        /// a name.
        /// </summary>
        /// <param name="server">The server to send the request to</param>
        /// <param name="nick">The nickname desired in the new room</param>
        /// <param name="callback">A callback to be called when the room is created</param>
        /// <param name="state">State object to be passed back when the callback fires</param>
        public void GetUniqueRoom(string server, string nick, RoomStateEvent callback, object state)
        {
            if (server == null)
                throw new ArgumentNullException("server");
            if (nick == null)
                throw new ArgumentNullException("nick");
            if (callback == null)
                throw new ArgumentNullException("callback");

/*
<iq from='crone1@shakespeare.lit/desktop'
    id='unique1'
    to='macbeth.shakespeare.lit'
    type='get'>
  <unique xmlns='http://jabber.org/protocol/muc#unique'/>
</iq>
 */
            UniqueIQ iq = new UniqueIQ(m_stream.Document);
            iq.To = server;
            m_stream.Tracker.BeginIQ(iq, new IqCB(GotUnique), new UniqueState(nick, callback, state));
        }

        private void GotUnique(object sender, IQ iq, object state)
        {
            UniqueState us = (UniqueState)state;
            if ((iq == null) || (iq.Type == IQType.error))
            {
                us.Callback(null, us.State);
                return;
            }

/*
<iq from='macbeth.shakespeare.lit'
    id='unique1'
    to='crone1@shakespeare.lit/desktop'
    type='result'>
  <unique xmlns='http://jabber.org/protocol/muc#unique'>
    6d9423a55f499b29ad20bf7b2bdea4f4b885ead1
  </unique>
</iq>
 */
            UniqueRoom unique = (UniqueRoom)iq.Query;
            Room r = GetRoom(new JID(unique.RoomNode, iq.From.Server, us.Nick));
            us.Callback(r, us.State);
        }
	}

    /// <summary>
    /// Manages a multi-user conference room.  See XEP-0045 (http://www.xmpp.org/extensions/xep-0045.html).
    /// </summary>
    public class Room
    {
        private enum STATE
        {
            start,
            join,
            configGet,
            configSet,
            running,
            leaving,
            error
        }

        private STATE m_state = STATE.start;
        /// <summary>
        /// Nick JID.  room@conference/nick.
        /// </summary>
        private JID m_jid;  
        /// <summary>
        /// Bare room JID.  room@conference
        /// </summary>
        private JID m_room; 
        private XmppStream m_stream;
        private bool m_default = false;
        private ConferenceManager m_manager;
        private Message m_subject;
        private ParticipantCollection m_participants = new ParticipantCollection();
        private object m_tag;

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="stream"></param>
        /// <param name="roomAndNick">room@conference/nick, where "nick" is the desred nickname in the room.</param>
        internal Room(ConferenceManager manager, XmppStream stream, JID roomAndNick)
        {
            m_manager = manager;
            m_stream = stream;
            m_jid = roomAndNick;
            m_room = new JID(m_jid.User, m_jid.Server, null);
            m_stream.OnProtocol += new jabber.protocol.ProtocolHandler(m_stream_OnProtocol);
            JabberClient jc = m_stream as JabberClient;
            if (jc != null)
                jc.OnAfterPresenceOut += new jabber.client.PresenceHandler(m_stream_OnAfterPresenceOut);
        }

        /// <summary>
        /// Finished joining the room, including all potential configuration.
        /// </summary>
        public event RoomEvent OnJoin;

        /// <summary>
        /// Finished leaving the room, or was kicked/banned, or the room server went down cleanly.
        /// </summary>
        public event RoomPresenceHandler OnLeave;

        /// <summary>
        /// Informs the client that an error in response to a room join,
        /// nick change, or presence update has occurred.
        /// </summary>
        public event RoomPresenceHandler OnPresenceError;

        /// <summary>
        /// Informs the client that the room configuration form was received.
        /// It is up to the listener to call the FinishConfig() method.
        /// The IQ in the callback is the parent of the x:data element.
        /// </summary>
        public event ConfigureRoom OnRoomConfig;

        /// <summary>
        /// A message broadcast to all in the room
        /// </summary>
        public event MessageHandler OnRoomMessage;

        /// <summary>
        /// A message that was sent by this user to the room, echo'd back.
        /// </summary>
        public event MessageHandler OnSelfMessage;

        /// <summary>
        /// A side-chat message.
        /// </summary>
        public event MessageHandler OnPrivateMessage;

        /// <summary>
        /// An admin message from the room itself.  Typically status change sorts of things
        /// like kick/ban.
        /// </summary>
        public event MessageHandler OnAdminMessage;

        /// <summary>
        /// The subject of the room has been changed
        /// </summary>
        public event MessageHandler OnSubjectChange;

        /// <summary>
        /// Determines whether to use the default conference room configuration
        /// or to retrieve the configuration form from the XMPP server.
        /// </summary>
        [DefaultValue(false)]
        public bool DefaultConfig
        {
            get { return m_default; }
            set { m_default = value; }
        }

        /// <summary>
        /// The subject of the room.  Set has the side-effect of sending to the server.
        /// </summary>
        public string Subject
        {
            get { return m_subject.Subject; }
            set
            {
                Message m = new Message(m_stream.Document);
                m.To = m_room;
                m.Type = MessageType.groupchat;
                m.Subject = value;
                m.Body = "/me has changed the subject to: " + value;
                m_stream.Write(m);
            }
        }

        /// <summary>
        /// The full JID of the user in the room.  room@service/nick
        /// </summary>
        public JID RoomAndNick
        {
            get { return m_jid; }
        }

        /// <summary>
        /// The nickname that others in the room will see for you.
        /// Set has the side-effect of changing the nickname on the server.
        /// </summary>
        public string Nickname
        {
            get { return m_jid.Resource; }
            set 
            {
                m_jid = new JID(m_jid.User, m_jid.Server, value);
                Presence p = new Presence(m_stream.Document);
                p.To = m_jid;
                m_stream.Write(p);
            }                
        }

        /// <summary>
        /// Current room participants.
        /// </summary>
        /// <returns></returns>
        public ParticipantCollection Participants
        {
            get { return m_participants; }
        }

        /// <summary>
        /// Extra data associated with the room.
        /// </summary>
        public object Tag
        {
            get { return m_tag; }
            set { m_tag = value; }
        }

        /// <summary>
        /// Whenver we change presence, send the new presence to the room, including
        /// caps etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pres"></param>
        private void m_stream_OnAfterPresenceOut(object sender, Presence pres)
        {
            Presence p = (Presence)pres.CloneNode(true);
            p.To = m_room;
            m_stream.Write(p);
        }

        private void m_stream_OnProtocol(object sender, System.Xml.XmlElement rp)
        {
            // There isn't always a from address.  iq:roster, for example.
            string af = rp.GetAttribute("from");
            if (af == "")
                return;
            JID from = new JID(af);
            if (from.Bare != (string)m_room)
                return;  // not for this room.

            switch (rp.LocalName)
            {
            case "presence":
                Presence p = (Presence)rp;
                if (p.Error != null)
                {
                    m_state = STATE.error;
                    if (OnPresenceError != null)
                        OnPresenceError(this, p);
                    return;
                }

                m_participants.Modify(p);

                // if this is ours
                if (p.From == m_jid)
                {
                    switch (m_state)
                    {
                    case STATE.join:
                        OnJoinPresence(p);
                        break;
                    case STATE.leaving:
                        OnLeavePresence(p);
                        break;
                    case STATE.running:
                        if (p.Type == PresenceType.unavailable)
                            OnLeavePresence(p);
                        break;
                    }
                }
                break;
            case "message":
                Message m = (Message)rp;
                if (m.Type == MessageType.groupchat)
                {
                    if (m.Subject != null)
                    {
                        if (OnSubjectChange != null)
                            OnSubjectChange(this, m);
                        m_subject = m;
                    }
                    else if (m.From == m_jid)
                    {
                        if (OnSelfMessage != null)
                            OnSelfMessage(this, m);
                    }
                    else
                    {
                        if (OnRoomMessage != null)
                            OnRoomMessage(this, m);
                    }
                }
                else
                {
                    if (m.From.Resource == null)
                    {
                        // room notification of some kind
                        if (OnAdminMessage != null)
                            OnAdminMessage(this, m);
                    }
                    else
                    {
                        if (OnPrivateMessage != null)
                            OnPrivateMessage(this, m);
                    }
                }
                break;
            case "iq":
                // TODO: IQs the room sends to us.
                break;
            }
        }

        private void OnJoinPresence(Presence p)
        {
            // from is always us.
/*
<presence
    to='crone1@shakespeare.lit/desktop'>
  <x xmlns='http://jabber.org/protocol/muc#user'>
    <item affiliation='owner'
          role='moderator'/>
    <status code='201'/>
  </x>
</presence>
 */
            UserX x = p["x", URI.MUC_USER] as UserX;
            if (x == null)
            {
                // Old server.  Hope for the best.
                if (OnJoin != null)
                    OnJoin(this);
                return;
            }

            if (x.HasStatus(RoomStatus.CREATED))
            {
                // room was created.  this must be me.
                if (m_default || (OnRoomConfig == null))
                    FinishConfigDefault();
                else
                    Configure();
                return;
            }

            // if it wasn't created, and this is mine, we must be running.
            m_state = STATE.running;
            if (OnJoin != null)
                OnJoin(this);
        }

        private void OnLeavePresence(Presence p)
        {
/*
<presence
    to='hag66@shakespeare.lit/pda'
    type='unavailable'>
  <x xmlns='http://jabber.org/protocol/muc#user'>
    <item affiliation='member' role='none'/>
    <status code='110'/>
  </x>
</presence>
 */
            // not quite an assert.  some sort of race.
            if (p.Type != PresenceType.unavailable)
                return;

            m_stream.OnProtocol -= new jabber.protocol.ProtocolHandler(m_stream_OnProtocol);
            jabber.client.JabberClient jc = m_stream as jabber.client.JabberClient;
            if (jc != null)
                jc.OnAfterPresenceOut -= new jabber.client.PresenceHandler(m_stream_OnAfterPresenceOut);
            m_manager.RemoveRoom(m_jid); // should cause this object to GC.
            if (OnLeave != null)
                OnLeave(this, p);
        }

        /// <summary>
        /// Configures the room. OnRoomConfig MUST be set first. 
        /// OnRoomConfig will be called back in the GUI thread if there is an
        /// InvokeControl on your XmppStream.  Make sure that OnRoomConfig does not
        /// return until it has the answer, typically by popping up a modal dialog
        /// with the x:data form.
        /// </summary>
        public void Configure()
        {
            if (OnRoomConfig == null)
                throw new ArgumentNullException("Must set OnRoomConfig before calling Configure()", "OnRoomConfig");

/*
<iq id='create1'
    to='darkcave@macbeth.shakespeare.lit'
    type='get'>
  <query xmlns='http://jabber.org/protocol/muc#owner'/>
</iq>
 */
            m_state = STATE.configGet;
            OwnerIQ iq = new OwnerIQ(m_stream.Document);
            iq.Type = IQType.get;
            iq.To = m_room;
            m_stream.Tracker.BeginIQ(iq, new IqCB(ConfigForm), null);
        }

        private void ConfigForm(object sender, IQ iq, object context)
        {
            // We should always be on the GUI thread.  
            // XmppStream should invoke before calling OnProtocol in the Tracker.
            Debug.Assert((m_stream.InvokeControl == null) || (!m_stream.InvokeControl.InvokeRequired));

            IQ resp = OnRoomConfig(this, iq);
            if (resp == null)
            {
                FinishConfigDefault();
                return;
            }

            m_state = STATE.configSet;
            resp.To = m_room;
            resp.Type = IQType.set;
            resp.From = null;
            m_stream.Tracker.BeginIQ(resp, new IqCB(Configured), null);
        }

        private void Configured(object sender, IQ iq, object context)
        {
            if (iq.Type != IQType.result)
            {
                m_state = STATE.error;
                // TODO: fire an error
                return;
            }

            if (m_state != STATE.running)
            {
                // reconfigs don't call OnJoin
                m_state = STATE.running;
                if (OnJoin != null)
                    OnJoin(this);
            }
        }

        /// <summary>
        /// Finish up configuration, taking the default room config.  Also known as
        /// an "Instant Room".  Suitable for use if the user cancels the configuration
        /// request, perhaps.
        /// </summary>
        private void FinishConfigDefault()
        {
/*
<iq from='crone1@shakespeare.lit/desktop'
    id='create1'
    to='darkcave@macbeth.shakespeare.lit'
    type='set'>
  <query xmlns='http://jabber.org/protocol/muc#owner'>
    <x xmlns='jabber:x:data' type='submit'/>
  </query>
</iq> 
 */
            m_state = STATE.configSet;
            OwnerIQ iq = new OwnerIQ(m_stream.Document);
            iq.Type = IQType.set;
            iq.To = m_room;
            OwnerQuery oq = (OwnerQuery)iq.Query;
            Data form = oq.Form;
            form.Type = XDataType.submit;
            m_stream.Tracker.BeginIQ(iq, new IqCB(Configured), null);
        }

        /// <summary>
        /// Joins the room.  If the room is created, Configure() will
        /// be called automatically.
        /// </summary>
        public void Join()
        {
            if (m_state == STATE.running)
                return;

            m_state = STATE.join;
            RoomPresence pres = new RoomPresence(m_stream.Document, m_jid);
            m_stream.Write(pres);
        }

        /// <summary>
        /// Exits the room.  This cleans up the entry in the ConferenceManager, as well.
        /// </summary>
        /// <param name="reason">Reason for leaving the room.  May be null for no reason.</param>
        public void Leave(string reason)
        {
            m_state = STATE.leaving;

/*
<presence
    to='darkcave@macbeth.shakespeare.lit/oldhag'
    type='unavailable'>
  <status>gone where the goblins go</status>
</presence>
 */
            Presence p = new Presence(m_stream.Document);
            p.To = m_jid;
            p.Type = PresenceType.unavailable;
            if (reason != null)
                p.Status = reason;
            m_stream.Write(p);


            // cleanup done when unavailable/110 received.
        }

        /// <summary>
        /// Sends a message to everyone currently in the room.
        /// </summary>
        /// <param name="body">The message text to send.</param>
        public void PublicMessage(string body)
        {
            if (m_state != STATE.running)
                throw new InvalidOperationException("Must be in running state to send message: " + m_state.ToString());
/*
<message
    to='darkcave@macbeth.shakespeare.lit'
    type='groupchat'>
  <body>Harpier cries: 'tis time, 'tis time.</body>
</message>
 */
            if (body == null)
                throw new ArgumentNullException("body");
            Message m = new Message(m_stream.Document);
            m.To = m_room;
            m.Type = MessageType.groupchat;
            m.Body = body;
            m_stream.Write(m);
        }

        /// <summary>
        /// Sends a private message to a single user in the room.
        /// </summary>
        /// <param name="nick">The nickname of the user to send a private message to.</param>
        /// <param name="body">The message body to send.</param>
        public void PrivateMessage(string nick, string body)
        {
            if (m_state != STATE.running)
                throw new InvalidOperationException("Must be in running state to send message: " + m_state.ToString());

/*
<message
    to='darkcave@macbeth.shakespeare.lit/firstwitch'
    type='chat'>
  <body>I'll give thee a wind.</body>
</message>
 */
            if (nick == null)
                throw new ArgumentNullException("nick");
            if (body == null)
                throw new ArgumentNullException("body");

            Message m = new Message(m_stream.Document);
            m.To = new JID(m_room.User, m_room.Server, nick);
            m.Type = MessageType.chat;
            m.Body = body;
            m_stream.Write(m);
        }

        /// <summary>
        /// Invite a user to join the room.
        /// </summary>
        /// <param name="invitee">The JID of the person to invite</param>
        /// <param name="reason">The reason for the invite, or null for none.</param>
        public void Invite(JID invitee, string reason)
        {
            if (m_state != STATE.running)
                throw new InvalidOperationException("Must be in running state to send invite: " + m_state.ToString());

            if (invitee == null)
                throw new ArgumentNullException("invitee");
/*
<message
    from='crone1@shakespeare.lit/desktop'
    to='darkcave@macbeth.shakespeare.lit'>
  <x xmlns='http://jabber.org/protocol/muc#user'>
    <invite to='hecate@shakespeare.lit'>
      <reason>
        Hey Hecate, this is the place for all good witches!
      </reason>
    </invite>
  </x>
</message>
 */
            Message m = new Message(m_stream.Document);
            m.To = m_room;
            UserX x = new UserX(m_stream.Document);
            x.AddInvite(invitee, reason);
            m.AddChild(x);
            m_stream.Write(m);
        }

#region Moderator use cases
        /// <summary>
        /// Change the role of a user in the room, by nickname.  Must be a moderator.
        /// </summary>
        /// <param name="nick">The nickname of the user to modify.</param>
        /// <param name="role">The new role</param>
        /// <param name="reason">The reason for the change</param>
        public void ChangeRole(string nick, RoomRole role, string reason)
        {
            if (m_state != STATE.running)
                throw new InvalidOperationException("Must be in running state to change role: " + m_state.ToString());

            if (nick == null)
                throw new ArgumentNullException("nick");
            if (role == RoomRole.UNSPECIFIED)
                throw new ArgumentNullException("role");
/*
<iq from='fluellen@shakespeare.lit/pda'
    id='kick1'
    to='harfleur@henryv.shakespeare.lit'
    type='set'>
  <query xmlns='http://jabber.org/protocol/muc#admin'>
    <item nick='pistol' role='none'>
      <reason>Avaunt, you cullion!</reason>
    </item>
  </query>
</iq>
*/
            RoomAdminIQ iq = new RoomAdminIQ(m_stream.Document);
            iq.To = m_room;
            iq.Type = IQType.set;
            AdminQuery query = (AdminQuery) iq.Query;
            AdminItem item = query.AddItem();
            item.Nick = nick;
            item.Role = role;
            item.Reason = reason;
            m_stream.Tracker.BeginIQ(iq, null, null);
        }

        /// <summary>
        /// Kick the given user from the room, based on their nickname.
        /// </summary>
        /// <param name="nick">The nickname of the person to kick</param>
        /// <param name="reason">The reason for kicking, or null for none.</param>
        public void Kick(string nick, string reason)
        {
            ChangeRole(nick, RoomRole.none, reason);
        }

        /// <summary>
        /// Disallow a user from speaking; remove their "voice".
        /// </summary>
        /// <param name="nick">The nickname of the person to mute</param>
        /// <param name="reason">The reason for the muting</param>
        public void RevokeVoice(string nick, string reason)
        {
            ChangeRole(nick, RoomRole.visitor, reason);
        }

        /// <summary>
        /// Un-mute a muted user.  Give them "voice".
        /// </summary>
        /// <param name="nick">The nicname of the person to unmute</param>
        /// <param name="reason">The reason for the change</param>
        public void GrantVoice(string nick, string reason)
        {
            ChangeRole(nick, RoomRole.participant, reason);
        }

        private class RetrieveParticipantsState
        {
            public RoomParticipantsEvent Callback;
            public object State;

            public RetrieveParticipantsState(RoomParticipantsEvent callback, object state)
            {
                this.Callback = callback;
                this.State = state;
            }
        }

        /// <summary>
        /// Retrieve all of the parties with a given role.
        /// Modify the affiliations of persons in this list, then call ModifyRoles
        /// </summary>
        /// <param name="role">The role to search for</param>
        /// <param name="callback">A callback to receive the participant list</param>
        /// <param name="state">Caller state information</param>
        public void RetrieveListByRole(RoomRole role, RoomParticipantsEvent callback, object state)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");
/*
<iq from='bard@shakespeare.lit/globe'
    id='voice3'
    to='goodfolk@chat.shakespeare.lit'
    type='get'>
  <query xmlns='http://jabber.org/protocol/muc#admin'>
    <item role='participant'/>
  </query>
</iq>
*/
            RoomAdminIQ iq = new RoomAdminIQ(m_stream.Document);
            iq.To = m_room;
            AdminQuery query = (AdminQuery)iq.Query;
            query.AddItem().Role = role;
            m_stream.Tracker.BeginIQ(iq, new IqCB(GotList), new RetrieveParticipantsState(callback, state));
        }

        private void GotList(object sender, IQ iq, object state)
        {
            RetrieveParticipantsState rps = (RetrieveParticipantsState)state;
            if (iq.Type == IQType.error)
            {
                rps.Callback(this, null, rps.State);
                return;
            }
/*
<iq from='southampton@henryv.shakespeare.lit'
    id='ban2'
    to='kinghenryv@shakespeare.lit/throne'
    type='result'>
  <query xmlns='http://jabber.org/protocol/muc#admin'>
    <item affiliation='outcast'
          jid='earlofcambridge@shakespeare.lit'>
      <reason>Treason</reason>
    </item>
  </query>
</iq>
*/
            ParticipantCollection parties = new ParticipantCollection();
            AdminQuery query = (AdminQuery)iq.Query;
            foreach (AdminItem item in query.GetItems())
            {
                Presence pres = new Presence(m_stream.Document);
                pres.From = new JID(m_jid.User, m_jid.Server, item.Nick);
                UserX x = new UserX(m_stream.Document);
                RoomItem xi = x.RoomItem;
                xi.Role = item.Role;
                xi.Affiliation = item.Affiliation;
                xi.Nick = item.Nick;
                xi.JID = item.JID;
                pres.AppendChild(x);
                parties.Modify(pres);
            }
            rps.Callback(this, parties, rps.State);
        }

        /// <summary>
        /// Modify the roles of the parties in this list.
        /// To use, retrive a ParticipantCollection, change the roles
        /// of the parties in that collection, then pass that modified 
        /// collection in here.
        /// </summary>
        /// <param name="parties">The modified participant collection</param>
        /// <param name="reason">The reason for the change</param>
        /// <param name="callback">A callback to call when complete.  Will have a null IQ if there were no changes to make.</param>
        /// <param name="state">Caller's state information</param>
        public void ModifyRoles(ParticipantCollection parties, string reason, IqCB callback, object state)
        {
/*
<iq from='bard@shakespeare.lit/globe'
    id='voice4'
    to='goodfolk@chat.shakespeare.lit'
    type='set'>
  <query xmlns='http://jabber.org/protocol/muc#admin'>
    <item nick='Hecate'
          role='visitor'/>
    <item nick='rosencrantz'
          role='participant'>
      <reason>A worthy fellow.</reason>
    </item>
    <item nick='guildenstern'
          role='participant'>
      <reason>A worthy fellow.</reason>
    </item>
  </query>
</iq>
*/
            RoomAdminIQ iq = new RoomAdminIQ(m_stream.Document);
            iq.To = m_room;
            iq.Type = IQType.set;
            AdminQuery query = (AdminQuery)iq.Query;

            int count = 0;
            foreach (RoomParticipant party in parties)
            {
                if (party.Changed)
                {
                    count++;
                    AdminItem item = query.AddItem();
                    item.Nick = party.Nick;
                    item.Role = party.Role;
                    item.Reason = reason;
                }
            }
            if (count > 0)
                m_stream.Tracker.BeginIQ(iq, callback, state);
            else
                callback(this, null, state);
        }

#endregion

#region Admin use cases
        /// <summary>
        /// Change the affiliation (long-term) with the room of a user, based on their real JID.
        /// </summary>
        /// <param name="jid">The bare JID of the user of which to change the affiliation</param>
        /// <param name="affiliation">The new affiliation</param>
        /// <param name="reason">The reason for the change</param>
        public void ChangeAffiliation(JID jid, RoomAffiliation affiliation, string reason)
        {
            if (m_state != STATE.running)
                throw new InvalidOperationException("Must be in running state to change affiliation: " + m_state.ToString());
            if (jid == null)
                throw new ArgumentNullException("jid");
            if (affiliation == RoomAffiliation.UNSPECIFIED)
                throw new ArgumentNullException("affiliation");
/*
<iq from='kinghenryv@shakespeare.lit/throne'
    id='ban1'
    to='southampton@henryv.shakespeare.lit'
    type='set'>
  <query xmlns='http://jabber.org/protocol/muc#admin'>
    <item affiliation='outcast'
          jid='earlofcambridge@shakespeare.lit'>
      <reason>Treason</reason>
    </item>
  </query>
</iq>
 */
            RoomAdminIQ iq = new RoomAdminIQ(m_stream.Document);
            iq.To = m_room;
            iq.Type = IQType.set;
            AdminQuery query = (AdminQuery)iq.Query;
            AdminItem item = query.AddItem();
            item.JID = jid;
            item.Affiliation = affiliation;
            item.Reason = reason;
            m_stream.Tracker.BeginIQ(iq, null, null);
        }

        /// <summary>
        /// Ban a user from re-joining the room.  Must be an admin.
        /// </summary>
        /// <param name="jid">The bare JID of the user to ban</param>
        /// <param name="reason">The reason for the shunning</param>
        public void Ban(JID jid, string reason)
        {
            ChangeAffiliation(jid, RoomAffiliation.outcast, reason);
        }

        /// <summary>
        /// Make this user a member of the room.
        /// </summary>
        /// <param name="jid">The bare jid of the user to grant membership to.</param>
        /// <param name="reason"></param>
        public void GrantMembership(JID jid, string reason)
        {
            ChangeAffiliation(jid, RoomAffiliation.member, reason);
        }

        /// <summary>
        /// Remove the membership privileges of the given user
        /// </summary>
        /// <param name="jid">The bare jid of the user to revoke the membership of.</param>
        /// <param name="reason"></param>
        public void RevokeMembership(JID jid, string reason)
        {
            // Or "Dismember".
            ChangeAffiliation(jid, RoomAffiliation.none, reason);
        }

        /// <summary>
        /// Make this user a moderator of the room.
        /// </summary>
        /// <param name="nick">The nickname of the user to change</param>
        public void MakeModerator(string nick)
        {
            ChangeRole(nick, RoomRole.moderator, null);
        }

        /// <summary>
        /// Retrieve all of the parties with a given affiliiation.
        /// Modify the affiliations of persons in this list, then call ModifyAffiliations
        /// </summary>
        /// <param name="affiliation">The affiliation to search for</param>
        /// <param name="callback">A callback to receive the participant list</param>
        /// <param name="state">Caller state information</param>
        public void RetrieveListByAffiliation(RoomAffiliation affiliation, RoomParticipantsEvent callback, object state)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");
/*
<iq from='kinghenryv@shakespeare.lit/throne'
    id='ban2'
    to='southampton@henryv.shakespeare.lit'
    type='get'>
  <query xmlns='http://jabber.org/protocol/muc#admin'>
    <item affiliation='outcast'/>
  </query>
</iq>       
*/
            RoomAdminIQ iq = new RoomAdminIQ(m_stream.Document);
            iq.To = m_room;
            AdminQuery query = (AdminQuery)iq.Query;
            query.AddItem().Affiliation = affiliation;
            m_stream.Tracker.BeginIQ(iq, new IqCB(GotList), new RetrieveParticipantsState(callback, state));
        }

        /// <summary>
        /// Modify the roles of the parties in this list.
        /// To use, retrive a ParticipantCollection, change the roles
        /// of the parties in that collection, then pass that modified 
        /// collection in here.
        /// </summary>
        /// <param name="parties">The modified participant collection</param>
        /// <param name="reason">The reason for the change</param>
        /// <param name="callback">A callback to call when complete.  Will have a null IQ if there were no changes to make.</param>
        /// <param name="state">Caller's state information</param>
        public void ModifyAffiliations(ParticipantCollection parties, string reason, IqCB callback, object state)
        {
/*
<iq from='southampton@henryv.shakespeare.lit'
    id='ban2'
    to='kinghenryv@shakespeare.lit/throne'
    type='result'>
  <query xmlns='http://jabber.org/protocol/muc#admin'>
    <item affiliation='outcast'
          jid='earlofcambridge@shakespeare.lit'>
      <reason>Treason</reason>
    </item>
  </query>
</iq>
*/
            RoomAdminIQ iq = new RoomAdminIQ(m_stream.Document);
            iq.To = m_room;
            iq.Type = IQType.set;
            AdminQuery query = (AdminQuery)iq.Query;

            int count = 0;
            foreach (RoomParticipant party in parties)
            {
                if (party.Changed && (party.RealJID != null))
                {
                    count++;
                    AdminItem item = query.AddItem();
                    item.JID = party.RealJID;
                    item.Affiliation = party.Affiliation;
                    item.Reason = reason;
                }
            }
            if (count > 0)
                m_stream.Tracker.BeginIQ(iq, callback, state);
            else
                callback(this, null, state);
        }
#endregion
    }

    /// <summary>
    /// A list of all of the current participants.
    /// </summary>
    public class ParticipantCollection : IEnumerable
    {
        private Hashtable m_hash = new Hashtable();

        /// <summary>
        /// Get a participant by their room@service/nick JID.
        /// </summary>
        /// <param name="nickJid">room@service/nick</param>
        /// <returns>Participant object</returns>
        public RoomParticipant this[JID nickJid]
        {
            get
            {
                return (RoomParticipant)m_hash[nickJid];
            }
        }

        /// <summary>
        /// Add a participant to the list, indexed by full nick JID.
        /// </summary>
        /// <param name="pres">The latest presence</param>
        /// <returns>The associated participant.</returns>
        internal RoomParticipant Modify(Presence pres)
        {
            JID from = pres.From;
            RoomParticipant party = (RoomParticipant)m_hash[from];
            if (party != null)
            {
                party.Presence = pres;
                if (pres.Type == PresenceType.unavailable)
                    m_hash.Remove(from);
            }
            else
            {
                party = new RoomParticipant(pres);
                // XCP will send unavails from registered users that 
                // are not currently online.
                if (pres.Type != PresenceType.unavailable)
                    m_hash[from] = party;
            }
            return party;
        }

        /// <summary>
        /// Get all of the participants that are in a given room role.
        /// </summary>
        /// <param name="role">The role to search for</param>
        /// <returns></returns>
        public RoomParticipant[] GetParticipantsByRole(RoomRole role)
        {
            ArrayList res = new ArrayList(m_hash.Count);
            foreach (RoomParticipant party in m_hash.Values)
            {
                if (party.Role == role)
                    res.Add(party);
            }
            return (RoomParticipant[])res.ToArray(typeof(RoomParticipant));
        }

        /// <summary>
        /// Get all of the participants that are in a given room affiliation.
        /// </summary>
        /// <param name="affiliation">The role to search for</param>
        /// <returns></returns>
        public RoomParticipant[] GetParticipantsByAffiliation(RoomAffiliation affiliation)
        {
            ArrayList res = new ArrayList(m_hash.Count);
            foreach (RoomParticipant party in m_hash.Values)
            {
                if (party.Affiliation == affiliation)
                    res.Add(party);
            }
            return (RoomParticipant[])res.ToArray(typeof(RoomParticipant));
        }

        #region IEnumerable Members
        /// <summary>
        /// Enumerate over all of the participants
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return m_hash.Values.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Someone who is currently in or associated with a room.
    /// </summary>
    public class RoomParticipant
    {
        private Presence m_presence;
        private bool m_changed = false;

        /// <summary>
        /// Create a participant from the last presence received for that user.
        /// </summary>
        /// <param name="pres"></param>
        public RoomParticipant(Presence pres)
        {
            if (pres == null)
                throw new ArgumentNullException("Presence must nut be null", "pres");
            m_presence = pres;
        }

        /// <summary>
        /// Last presence received for this user.
        /// </summary>
        public Presence Presence
        {
            get { return m_presence; }
            set 
            {
                m_presence = value;
                m_changed = false;
            }
        }

        /// <summary>
        /// Has this participant's role or affiliation been changed?
        /// </summary>
        public bool Changed
        {
            get { return m_changed; }
        }

        /// <summary>
        /// The muc#user item in the presence.
        /// </summary>
        protected RoomItem Item 
        {
            get 
            {
                UserX x = (UserX)m_presence["x", URI.MUC_USER];
                if (x == null)
                    return null;
                return x.RoomItem;
            }
        }

        /// <summary>
        /// Nickname of the user
        /// </summary>
        public string Nick
        {
            get { return m_presence.From.Resource; }
        }

        /// <summary>
        /// Affiliation of the user.
        /// </summary>
        public RoomAffiliation Affiliation
        {
            get
            {
                RoomItem item = Item;
                if (item == null)
                    return RoomAffiliation.UNSPECIFIED;
                return item.Affiliation;
            }
            set
            {
                RoomItem item = Item;
                if (item == null)
                    return;
                if (item.Affiliation != value)
                {
                    item.Affiliation = value;
                    m_changed = true;
                }
            }
        }

        /// <summary>
        /// Role of the user.
        /// </summary>
        public RoomRole Role
        {
            get
            {
                RoomItem item = Item;
                if (item == null)
                    return RoomRole.UNSPECIFIED;
                return item.Role;
            }
            set
            {
                RoomItem item = Item;
                if (item == null)
                    return;
                if (item.Role != value)
                {
                    item.Role = value;
                    m_changed = true;
                }
            }
        }

        /// <summary>
        /// room@server/nick of the user.
        /// </summary>
        public JID NickJID
        {
            get { return m_presence.From; }
        }

        /// <summary>
        /// The real JID of the user, if this is a non-anonymous room.
        /// </summary>
        public JID RealJID
        {
            get
            {
/*
<presence
    from='darkcave@macbeth.shakespeare.lit/thirdwitch'
    to='crone1@shakespeare.lit/desktop'>
  <x xmlns='http://jabber.org/protocol/muc#user'>
    <item affiliation='none'
          jid='hag66@shakespeare.lit/pda'
          role='participant'/>
  </x>
</presence>
 */
                RoomItem item = Item;
                if (item == null)
                    return null;
                return item.JID;
            }
        }

        /// <summary>
        /// The nick JID or nick (real).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            JID nick = NickJID;
            JID real = RealJID;
            if (real != null)
                return string.Format("{0} ({1})", nick, real);
            return nick;
        }
    }
}
