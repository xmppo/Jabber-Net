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
            get { return m_client; }
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

        private class UserPresenceManager
        {
            private Tree m_items = new Tree();
            private Presence m_pres = null;

            public void AddPresence(Presence p)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.available);
                lock (this)
                {
                    if (res == null)
                    {
                        m_pres = p;
                        return;
                    }
                    else
                    {
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
                }
            }

            public void RemovePresence(Presence p)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.unavailable);
                lock (this)
                {
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
            }

            public int Count
            {
                get 
                {
                    lock (this)
                    {
                        if (m_items.Count > 0)
                            return m_items.Count;
                        if (m_pres == null)
                            return 0;
                        return 1;
                    }
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
