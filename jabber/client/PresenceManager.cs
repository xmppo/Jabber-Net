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
    /// Presence proxy database.
    /// </summary>
    [RCS(@"$Header$")]
    public class PresenceManager : System.ComponentModel.Component, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private JabberClient m_client = null;
        private Tree m_items = new Tree();

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        /// <param name="container"></param>
        public PresenceManager(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        public PresenceManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The JabberClient to hook up to.
        /// </summary>
        [Description("The JabberClient to hook up to.")]
        [Category("Jabber")]
        public JabberClient Client
        {
            get
            {
                // If we are running in the designer, let's try to get an invoke control
                // from the environment.  VB programmers can't seem to follow directions.
                if ((this.m_client == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost) base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is JabberClient)
                                {
                                    m_client = (JabberClient) c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_client;
            }
            set
            {
                m_client = value;
                m_client.OnPresence += new PresenceHandler(GotPresence);
                m_client.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
            }
        }

        private void GotDisconnect(object sender)
        {
            lock(this)
                m_items.Clear();
        }

        /// <summary>
        /// Add a new available or unavailable presence packet to the database.
        /// </summary>
        /// <param name="p"></param>
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
                        upm = new UserPresenceManager();
                        m_items[f.Bare] = upm;
                    }

                    upm.AddPresence(p);
                }
                else
                {
                    if (upm != null)
                    {
                        upm.RemovePresence(p);
                        if (upm.Count == 0)
                        {
                            m_items.Remove(f.Bare);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Is this user online with any resource?  This performs better than retrieving
        /// the particular associated presence packet.
        /// </summary>
        /// <param name="jid">The JID to look up.</param>
        /// <returns></returns>
        public bool IsAvailable(JID jid)
        {
            lock (this)
            {
                return (m_items[jid.Bare] != null);
            }
        }

        /// <summary>
        /// If given a bare JID, get the primary presence.
        /// If given a FQJ, return the associated presence.
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        /// <summary>
        /// Manage the presence for all of the resources of a user.  No locking is performed,
        /// since PresenceManager is already doing locking.
        /// </summary>
        private class UserPresenceManager
        {
            private Tree m_items = new Tree();
            private Presence m_pres = null;

            public void AddPresence(Presence p)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.available);
                if (res == null)
                {
                    m_pres = p;
                    return;
                }

                // Tree can't overwrite. Have to delete first.
                m_items.Remove(res);
                m_items[res] = p;

                if ((m_pres == null) || (m_pres.From.Resource == res))
                {
                    m_pres = p;
                    return;
                }
                string pri = p.Priority;
                int new_pri = (pri == null) ? 0 : int.Parse(pri);
                pri = m_pres.Priority;
                int old_pri = (pri == null) ? 0 : int.Parse(pri);
                if (new_pri > old_pri)
                {
                    m_pres = p;
                }
            }

            public void RemovePresence(Presence p)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.unavailable);

                if (res != null)
                    m_items.Remove(res);

                if (m_pres.From.Resource == res)
                {
                    // this was the high pri resource.  Find the next highest.
                    m_pres = null;

                    int curp;
                    int maxp = -1;

                    foreach (DictionaryEntry de in m_items)
                    {
                        Presence tp = (Presence) de.Value;
                        string pri = tp.Priority;
                        curp = (pri == null) ? 0 : int.Parse(pri);
                        if (curp > maxp)
                        {
                            m_pres = tp;
                            maxp = curp;
                        }
                    }

                }
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
        }
    }
}
