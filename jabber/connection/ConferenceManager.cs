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
        /// Create a manager
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
            r = new Room(this.Stream, roomAndNick);
            m_rooms[roomAndNick] = r;
            return r;
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
            error
        }

        private STATE m_state = STATE.start;
        private JID m_jid;
        private string m_room; // room@conference, bare JID.
        private XmppStream m_stream;
        private bool m_default = false;

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="roomAndNick">room@conference/nick, where "nick" is the desred nickname in the room.</param>
        public Room(XmppStream stream, JID roomAndNick)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (roomAndNick == null)
                throw new ArgumentNullException("roomAndNick");
            if (roomAndNick.Resource == null)
                roomAndNick.Resource = m_stream.JID.User;
            m_stream = stream;
            m_jid = roomAndNick;
            m_room = m_jid.Bare;
            m_stream.OnProtocol += new jabber.protocol.ProtocolHandler(m_stream_OnProtocol);
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

        private void m_stream_OnProtocol(object sender, System.Xml.XmlElement rp)
        {
            JID from = (JID)rp.GetAttribute("from");
            if (from.Bare != m_room)
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

                if (m_state == STATE.join)
                {
                    UserX x = p["x", URI.MUC_USER] as UserX;
                    if (x == null)
                    {
                        // Old server.  Hope for the best.
                        m_state = STATE.running;
                    }
                    else
                    {
/*
<presence
    from='darkcave@macbeth.shakespeare.lit/firstwitch'
    to='crone1@shakespeare.lit/desktop'>
  <x xmlns='http://jabber.org/protocol/muc#user'>
    <item affiliation='owner'
          role='moderator'/>
    <status code='201'/>
  </x>
</presence>
 */
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
                }
            }

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
                iq.To = m_room;
                iq.Type = IQType.set;
                OwnerQuery q = (OwnerQuery)iq.Query;
                q.Form.Type = jabber.protocol.x.XDataType.submit;
                m_stream.Tracker.BeginIQ(iq, new IqCB(Configured), null);
            }
            else
            { // "Reserved" room.  Get the config form.
/*
<iq from='crone1@shakespeare.lit/desktop'
    id='create1'
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
    }
}
