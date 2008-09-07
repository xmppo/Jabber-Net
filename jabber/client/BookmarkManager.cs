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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;

using bedrock.util;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.iq;
using System.Xml;

namespace jabber.client
{
    /// <summary>
    /// A new conference bookmark.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="conference"></param>
    public delegate void BookmarkConferenceDelegate(BookmarkManager manager, BookmarkConference conference);

    /// <summary>
    /// Manager bookmarks on the server, with the old-style iq:private.
    /// TODO: add support for new-style PEP.
    /// </summary>
    [SVN(@"$Id$")]    
    public class BookmarkManager : jabber.connection.StreamComponent
	{
        private bool m_autoPrivate = true;
        private ConferenceManager m_confManager;
        private Dictionary<JID, BookmarkConference> m_conferences = new Dictionary<JID, BookmarkConference>();

        /// <summary>
        /// Create
        /// </summary>
		public BookmarkManager()
		{
			InitializeComponent();
            this.OnStreamChanged += new bedrock.ObjectHandler(BookmarkManager_OnStreamChanged);
		}

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="container"></param>
		public BookmarkManager(IContainer container) : this()
		{
			container.Add(this);
		}

        /// <summary>
        /// Automatically request bookmarks using iq:private on login.
        /// </summary>
        [Category("Bookmarks")]
        [Description("Automatically request bookmarks using iq:private on login.")]
        [DefaultValue(true)]
        public bool AutoPrivate
        {
            get { return m_autoPrivate; }
            set { m_autoPrivate = value; }
        }

        /// <summary>
        /// A conference bookmark has been .
        /// </summary>
        [Category("Bookmarks")]
        [Description("A conference bookmark has been added to the list.")]
        public event BookmarkConferenceDelegate OnConferenceAdd;

        /// <summary>
        /// A conference bookmark has been removed from the list.
        /// </summary>
        [Category("Bookmarks")]
        [Description("A conference bookmark has been removed from the list.")]
        public event BookmarkConferenceDelegate OnConferenceRemove;

        /// <summary>
        /// A ConferenceManager into which to auto-join conference rooms.
        /// </summary>
        [Category("Jabber")]
        public ConferenceManager ConferenceManager
        {
            get
            {
                // If we are running in the designer, let's try to auto-hook a ConferenceManager
                if ((m_confManager == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    this.ConferenceManager = (ConferenceManager)jabber.connection.StreamComponent.GetComponentFromHost(host, typeof(ConferenceManager));
                }
                return m_confManager;
            }
            set
            {
                if ((object)m_confManager == (object)value)
                    return;
                m_confManager = value;
            }
        }

        private void BookmarkManager_OnStreamChanged(object sender)
        {
            m_stream.OnDisconnect += new bedrock.ObjectHandler(m_stream_OnDisconnect);
            m_stream.OnError += new bedrock.ExceptionHandler(m_stream_OnError);
            JabberClient cli = m_stream as JabberClient;
            if (cli == null)
                return;

            cli.OnAuthenticate += new bedrock.ObjectHandler(cli_OnAuthenticate);
        }

        private void m_stream_OnError(object sender, Exception ex)
        {
            m_conferences.Clear();
        }

        private void m_stream_OnDisconnect(object sender)
        {
            m_conferences.Clear();
        }

        private void cli_OnAuthenticate(object sender)
        {
            if (AutoPrivate)
            {
                BookmarksIQ biq = new BookmarksIQ(m_stream.Document);
                biq.Type = IQType.get;
                m_stream.Tracker.BeginIQ(biq, GotBookmarks, null);
            }
        }

        private void GotBookmarks(object sender, IQ iq, object state)
        {
            if ((iq == null) || (iq.Type != IQType.result))
                return;

            Private priv = iq.Query as Private;
            if (priv == null)
                return;

            Bookmarks bm = priv.GetChildElement<Bookmarks>();
            if (bm == null)
                return;

            foreach (BookmarkConference conf in bm.GetConferences())
            {
                if (conf.JID == null)
                    continue;

                m_conferences[conf.JID] = conf;
                if (OnConferenceAdd != null)
                    OnConferenceAdd(this, conf);
                if (conf.AutoJoin && (m_confManager != null))
                {
                    JID rJID = conf.JID;
                    JID roomAndNick = new JID(rJID.User, rJID.Server, conf.Nick);
                    Room r = m_confManager.GetRoom(roomAndNick);
                    r.Join(conf.Password);
                }
            }
        }

        /// <summary>
        /// Get the details for the given conference bookmark.
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public BookmarkConference this[JID jid]
        {
            get { return m_conferences[jid]; }
            set
            {
                BookmarkConference prev = null;
                if (value == null)
                {
                    if (m_conferences.TryGetValue(jid, out prev))
                    {
                        m_conferences.Remove(jid);
                        prev.SetAttribute("remove", "true");
                    }
                    else
                    {
                        // no-op.  Setting null on a non-existing JID.
                        return;
                    }
                }
                else
                {
                    m_conferences[jid] = prev = value;
                }

                BookmarksIQ biq = new BookmarksIQ(m_stream.Document);
                biq.Type = IQType.set;
                Bookmarks bm = biq.Bookmarks;
                foreach (BookmarkConference conf in m_conferences.Values)
                {
                    bm.AddChild((XmlElement)conf.CloneNode(true, m_stream.Document));
                }
                m_stream.Tracker.BeginIQ(biq, BookmarksSet, prev);
            }
        }

        private void BookmarksSet(object sender, IQ iq, object state)
        {
            if ((iq == null) || (iq.Type != IQType.result))
                return;

            BookmarkConference prev = state as BookmarkConference;
            if (prev == null)
                return;

            if (prev.GetAttribute("remove") == "true")
            {
                if (OnConferenceRemove != null)
                {
                    prev.RemoveAttribute("remove");
                    OnConferenceRemove(this, prev);
                }
            }
            else
            {
                if (OnConferenceAdd != null)
                    OnConferenceAdd(this, prev);
            }
        }

        /// <summary>
        /// Add a conference room to the bookmark list
        /// </summary>
        /// <param name="jid">The room@service JID of the room</param>
        /// <param name="name">Human-readable text</param>
        /// <param name="autoJoin">Join on login</param>
        /// <param name="nick">Room nickname.  May be null.</param>
        /// <returns></returns>
        public BookmarkConference AddConference(JID jid, string name, bool autoJoin, string nick)
        {
            BookmarkConference c = new BookmarkConference(m_stream.Document);
            if (jid == null)
                throw new ArgumentNullException("jid", "JID must not be null in a conference bookmark");
            c.JID = jid;
            if ((name != null) && (name != ""))
                c.ConferenceName = name;
            c.AutoJoin = autoJoin;
            if ((nick != null) && (nick != ""))
                c.Nick = nick;
            this[jid] = c;
            return c;
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
	}
}
