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
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Diagnostics;

using bedrock.util;
using bedrock.collections;

using jabber.protocol.client;

namespace jabber.client
{

    /// <summary>
    /// Informs the client of a change of derived primary session for a user.
    /// </summary>
    /// <param name="sender">The PresenceManager object that sent the update</param>
    /// <param name="bare">The bare JID (node@domain) of the user whose presence changed</param>
    public delegate void PrimarySessionHandler(object sender, JID bare);

    /// <summary>
    /// Specifies the presence proxy database.
    /// </summary>
    [SVN(@"$Id$")]
    public class PresenceManager : jabber.connection.StreamComponent, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Tree m_items = new Tree();

        /// <summary>
        /// Constructs a PresenceManager object and adds it to a container.
        /// </summary>
        /// <param name="container">Parent container.</param>
        public PresenceManager(System.ComponentModel.IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Constructs a new PresenceManager object.
        /// </summary>
        public PresenceManager()
        {
            InitializeComponent();

            this.OnStreamChanged += new bedrock.ObjectHandler(PresenceManager_OnStreamChanged);
        }

        private void PresenceManager_OnStreamChanged(object sender)
        {
            JabberClient cli = m_stream as JabberClient;
            if (cli == null)
                return;

            cli.OnPresence += new PresenceHandler(GotPresence);
            cli.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
        }

        /// <summary>
        /// Gets or sets the JabberClient associated with the Presence Manager.
        /// </summary>
        [Description("Gets or sets the JabberClient associated with the Presence Manager.")]
        [Category("Jabber")]
        [Browsable(false)]
        [Obsolete("Use the Stream property instead")]
        [ReadOnly(true)]
        public JabberClient Client
        {
            get { return (JabberClient)this.Stream; }
            set { this.Stream = value; }
        }

        /// <summary>
        /// Gets the current presence state as a string.
        /// </summary>
        /// <returns>string in the format '{bare JID}={list of presence stanzas}, ...'</returns>
        public override string ToString()
        {
            return m_items.ToString();
        }

        /// <summary>
        /// Informs the client that the primary session has changed for a user.
        /// </summary>
        public event PrimarySessionHandler OnPrimarySessionChange;

        private void GotDisconnect(object sender)
        {
            lock(this)
                m_items.Clear();
        }

        /// <summary>
        /// Adds a new available or unavailable presence packet to the database.
        /// </summary>
        /// <param name="p">Presence stanza to add.</param>
        public void AddPresence(Presence p)
        {
            // can't use .From, since that will cause a JID parse.
            Debug.Assert(p.GetAttribute("from") != "",
                "Do not call AddPresence by hand.  I can tell you are doing that because you didn't put a from address on your presence packet, and all presences from the server have a from address.");
            GotPresence(this, p);
        }

        private void GotPresence(object sender, Presence p)
        {
            PresenceType t = p.Type;
            if ((t != PresenceType.available) &&
                (t != PresenceType.unavailable))
                return;

            JID f = p.From;
            lock (this)
            {
                UserPresenceManager upm = (UserPresenceManager)m_items[f.Bare];

                if (t == PresenceType.available)
                {
                    if (upm == null)
                    {
                        upm = new UserPresenceManager(f.Bare);
                        m_items[f.Bare] = upm;
                    }

                    upm.AddPresence(p, this);
                }
                else
                {
                    if (upm != null)
                    {
                        upm.RemovePresence(p, this);
                        if (upm.Count == 0)
                        {
                            m_items.Remove(f.Bare);
                        }
                    }
                }
            }
        }

        private void FireOnPrimarySessionChange(JID from)
        {
            if (OnPrimarySessionChange != null)
                OnPrimarySessionChange(this, from);
        }

        /// <summary>
        /// Determines if a specified JID is online with any resources.
        /// This performs better than retrieving the particular associated presence packet.
        /// </summary>
        /// <param name="jid">The JID to look up.</param>
        /// <returns>If true, the user is online; otherwise the user is offline</returns>
        public bool IsAvailable(JID jid)
        {
            lock (this)
            {
                return (m_items[jid.Bare] != null);
            }
        }

        /// <summary>
        /// Gets the primary presence if given a bare JID.
        /// If given a FQJ, returns the associated presence.
        /// </summary>
        public Presence this[JID jid]
        {
            get
            {
                lock (this)
                {
                    UserPresenceManager upm = (UserPresenceManager)m_items[jid.Bare];
                    if (upm == null)
                        return null;
                    return upm[jid.Resource];
                }
            }
        }

        /// <summary>
        /// Gets all of the current presence stanzas for the given user.
        /// </summary>
        /// <param name="jid">User who's presence stanzas you want.</param>
        /// <returns>Array of presence stanzas for the given user.</returns>
        public Presence[] GetAll(JID jid)
        {
            UserPresenceManager upm = (UserPresenceManager)m_items[jid.Bare];
            if (upm == null)
                return new Presence[0];
            return upm.GetAll();
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
        /// Iterate over all of the JIDs we have not-unavilable presence from.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new UserPresenceManagerEnumerator(m_items.Values);
        }

        private class UserPresenceManagerEnumerator : IEnumerator
        {
            private IEnumerator m_enum;

            public UserPresenceManagerEnumerator(ICollection values)
            {
                m_enum = values.GetEnumerator();
            }

            #region IEnumerator Members
            public object Current
            {
                get
                {
                    UserPresenceManager m = (UserPresenceManager)m_enum.Current;
                    if (m == null)
                        return null;
                    return m.JID;
                }
            }

            public bool MoveNext()
            {
                return m_enum.MoveNext();
            }

            public void Reset()
            {
                m_enum.Reset();
            }

            #endregion
        }

        /// <summary>
        /// Manage the presence for all of the resources of a user.  No locking is performed,
        /// since PresenceManager is already doing locking.
        ///
        /// The intent of this class is to be able to deliver the last presence stanza
        /// from the "most available" resource.
        /// Note that negative priority sessions are never the most available.
        /// </summary>
        private class UserPresenceManager
        {
            private Tree m_items = new Tree();
            private Presence m_pres = null;
            private JID m_jid = null;

            public UserPresenceManager(JID jid)
            {
                Debug.Assert(jid.Resource == null);
                m_jid = jid;
            }

            public JID JID
            {
                get { return m_jid; }
            }

            public override string ToString()
            {
                return "{\r\n" + m_items.ToString() + "\r\n}";
            }

            private void Primary(Presence p, PresenceManager handler)
            {
                Debug.Assert((p == null) ? true : (p.IntPriority >= 0), "Primary presence is always positive priority");
                m_pres = p;
                handler.FireOnPrimarySessionChange(m_jid);
            }

            public void AddPresence(Presence p, PresenceManager handler)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.available);

                // this is probably a service of some kind.  presumably, there will
                // only ever be one resource.
                if (res == null)
                {
                    if (p.IntPriority >= 0)
                        Primary(p, handler);
                    return;
                }

                // Tree can't overwrite. Have to delete first.
                m_items.Remove(res);
                m_items[res] = p;

                // first one is always highest
                if (m_pres == null)
                {
                    if (p.IntPriority >= 0)
                        Primary(p, handler);
                    return;
                }

                // Otherwise, recalc
                SetHighest(handler);
            }

            public void RemovePresence(Presence p, PresenceManager handler)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.unavailable);

                if (res != null)
                    m_items.Remove(res);

                if (m_pres == null)
                    return;

                if (m_pres.From.Resource == res)
                {
                    SetHighest(handler);
                }
            }

            private void SetHighest(PresenceManager handler)
            {
                Presence p = null;
                foreach (DictionaryEntry de in m_items)
                {
                    Presence tp = (Presence)de.Value;
                    if (tp.IntPriority < 0)
                        continue;

                    if (p == null)
                        p = tp;
                    else
                    {
                        if (tp > p)
                            p = tp;
                    }
                }
                Primary(p, handler);
            }

            public int Count
            {
                get
                {
                    if (m_items.Count > 0)
                        return m_items.Count;
                    if (m_pres == null)
                        return 0;
                    return 1;
                }
            }

            public Presence this[string Resource]
            {
                get
                {
                    if (Resource == null)
                        return m_pres;
                    return (Presence) m_items[Resource];
                }
            }

            public Presence[] GetAll()
            {
                Presence[] all;
                if (m_items.Count > 0)
                    all = new Presence[m_items.Count];
                else if (m_pres == null)
                    return new Presence[0];
                else
                    return new Presence[] {m_pres};

                m_items.CopyTo(all, 0);
                return all;
            }
        }
    }
}
