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

namespace jabber.connection
{
    /// <summary>
    /// Manage a set of conference rooms
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
        /// Creates a manager.
        /// </summary>
        public ConferenceManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a manager in a container
        /// </summary>
        /// <param name="container"></param>
        public ConferenceManager(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Join a chat room.
        /// If we are already joined, the existing room will be returned.
        /// If not, a Room object will be returned in the joining state.
        /// </summary>
        /// <param name="roomAndNick">room@conference/nick, where "nick" is the desred nickname in the room.</param>
        /// <returns></returns>
        public Room GetRoom(JID roomAndNick)
        {
            Room r = (Room)m_rooms[roomAndNick];
            if (r != null)
                return r;
            r = new Room(this, this.Stream, roomAndNick);
            m_rooms[roomAndNick] = r;
            return r;
        }

        /// <summary>
        /// Is this room being managed by this manager?
        /// </summary>
        /// <param name="roomAndNick"></param>
        /// <returns></returns>
        public bool HasRoom(JID roomAndNick)
        {
            return m_rooms.ContainsKey(roomAndNick);
        }

        /// <summary>
        /// Remove the room from the list.
        /// Should most often be called by Room.Leave()
        /// If the room does not exist, no exception is thrown.
        /// </summary>
        /// <param name="roomAndNick"></param>
        public void RemoveRoom(JID roomAndNick)
        {
            m_rooms.Remove(roomAndNick);
        }
	}

    /// <summary>
    /// An error occurred with a presence sent to a room.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="pres"></param>
    public delegate void RoomPresenceError(Room room, Presence pres);

    /// <summary>
    /// A room configuration form has been received.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="parent">Contains an x:data child with the form.</param>
    public delegate void ConfigureRoom(Room room, IQ parent);

    /// <summary>
    /// A multi-user chat room.  See XEP-0045 (http://www.xmpp.org/extensions/xep-0045.html).
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
        private JID m_jid;
        private JID m_room; // room@conference, bare JID.
        private XmppStream m_stream;
        private bool m_default = false;
        private ConferenceManager m_manager;

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="stream"></param>
        /// <param name="roomAndNick">room@conference/nick, where "nick" is the desred nickname in the room.</param>
        internal Room(ConferenceManager manager, XmppStream stream, JID roomAndNick)
        {
            Debug.Assert(manager != null);
            Debug.Assert(stream != null);
            if (roomAndNick == null)
                throw new ArgumentNullException("roomAndNick");
            if (roomAndNick.Resource == null)
                roomAndNick.Resource = m_stream.JID.User;
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
        /// Error in response to a room join, nick change, or presence update.
        /// </summary>
        public event RoomPresenceError OnPresenceError;

        /// <summary>
        /// Room configuration form received.  It is up to the listener call FinishConfig().
        /// The IQ in the callback is the parent of the x:data element.
        /// </summary>
        public event ConfigureRoom OnRoomConfig;

        /// <summary>
        /// When we configure, should we just use the default config, or should we 
        /// retrieve the configuration form.
        /// </summary>
        [DefaultValue(false)]
        public bool DefaultConfig
        {
            get { return m_default; }
            set { m_default = value; }
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
            JID from = (JID)rp.GetAttribute("from");
            if (from.Bare != (string)m_room)
                return;  // not for this room.

            Presence p = rp as Presence;
            if (p != null)
            {
                if (p.Error != null)
                {
                    m_state = STATE.error;
                    if (OnPresenceError != null)
                        OnPresenceError(this, p);
                    return;
                }

                switch (m_state)
                {
                case STATE.join:
                    OnJoinPresence(p);
                    break;
                case STATE.leaving:
                    OnLeavePresence(p);
                    break;
                }
            }
        }

        private void OnJoinPresence(Presence p)
        {
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
                m_state = STATE.running;
                return;
            }

            if (x.HasStatus(RoomStatus.CREATED))
            {
                // room was created.  this must be me.
                Configure();
            }
            else if (x.HasStatus(RoomStatus.SELF))
            {
                // if it wasn't created, and this is mine, we must be running.
                m_state = STATE.running;
            }
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
            // race.  someone entered just after we tried to leave.
            if (p.Type != PresenceType.unavailable)
                return;

            // if this is an old server, the first unavail we get
            // is the ack.  Otherwise, wait for our own.
            UserX x = p["x", URI.MUC_USER] as UserX;
            if (x != null)
            {
                if (!x.HasStatus(RoomStatus.SELF))
                    return;
            }

            m_stream.OnProtocol -= new jabber.protocol.ProtocolHandler(m_stream_OnProtocol);
            jabber.client.JabberClient jc = m_stream as jabber.client.JabberClient;
            if (jc != null)
                jc.OnAfterPresenceOut -= new jabber.client.PresenceHandler(m_stream_OnAfterPresenceOut);
            m_manager.RemoveRoom(m_jid); // should cause this object to GC.
        }

        /// <summary>
        /// Configure the room.  If the DefaultConfig is true, this will
        /// just requst the default config.  Otherwise, 
        /// </summary>
        public void Configure()
        {
            if (m_default)
            { // "Instant" room.  take the default config
/*
<iq id='create1'
    to='darkcave@macbeth.shakespeare.lit'
    type='set'>
  <query xmlns='http://jabber.org/protocol/muc#owner'>
    <x xmlns='jabber:x:data' type='submit'/>
  </query>
</iq>
 */
                m_state = STATE.configSet;
                OwnerIQ iq = new OwnerIQ(m_stream.Document);
                iq.To = m_room;
                iq.Type = IQType.set;
                OwnerQuery q = (OwnerQuery)iq.Query;
                q.Form.Type = jabber.protocol.x.XDataType.submit;
                m_stream.Tracker.BeginIQ(iq, new IqCB(Configured), null);
            }
            else
            { // "Reserved" room.  Get the config form.
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
        }

        private void Configured(object sender, IQ iq, object context)
        {
            if (iq.Type != IQType.result)
            {
                m_state = STATE.error;
                // TODO: fire an error
                return;
            }
            m_state = STATE.running;
        }

        private void ConfigForm(object sender, IQ iq, object context)
        {
            if (OnRoomConfig == null)
                throw new ArgumentNullException("OnRoomConfig");
            OnRoomConfig(this, iq);
        }

        /// <summary>
        /// Finish up configuration given an IQ that contains the result to the configuration
        /// form.
        /// </summary>
        /// <param name="iq"></param>
        public void FinishConfig(IQ iq)
        {
            m_state = STATE.configSet;
            m_stream.Tracker.BeginIQ(iq, new IqCB(Configured), null);
        }

        /// <summary>
        /// Join the room.  If the room is created, Configure() will be called automatically.
        /// </summary>
        public void Join()
        {
            lock (this)
            {
                if (m_state == STATE.running)
                    return;

                m_state = STATE.join;
            }
            RoomPresence pres = new RoomPresence(m_stream.Document, m_jid);
            m_stream.Write(pres);
        }

        /// <summary>
        /// Exit the room.  This cleans up the entry in the ConferenceManager, as well.
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
        /// Send a message to everyone currently in the room.
        /// </summary>
        /// <param name="body">The message text to send.</param>
        public void PublicMessage(string body)
        {
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
        /// Send a private message to a single user in the room.
        /// </summary>
        /// <param name="nick">The nickname of the user to private message</param>
        /// <param name="body">The message body to send</param>
        public void PrivateMessage(string nick, string body)
        {
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
    }
}
