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

using bedrock.collections;
using bedrock.util;

using jabber.protocol.client;
using jabber.protocol.iq;

namespace jabber.client
{
    /// <summary>
    /// Delegate for receiving roster items.
    /// </summary>
    public delegate void RosterItemHandler(object sender, Item ri);

    /// <summary>
    /// Summary description for RosterManager.
    /// </summary>
    [SVN(@"$Id$")]
    public class RosterManager : jabber.connection.StreamComponent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Tree m_items = new Tree();

        /// <summary>
        /// Create a roster manager inside a container
        /// </summary>
        /// <param name="container"></param>
        public RosterManager(System.ComponentModel.IContainer container) : this()
        {
            // Required for Windows.Forms Class Composition Designer support
            container.Add(this);
        }

        /// <summary>
        /// Create a roster manager
        /// </summary>
        public RosterManager()
        {
            // Required for Windows.Forms Class Composition Designer support
            InitializeComponent();
            this.OnStreamChanged += new bedrock.ObjectHandler(RosterManager_OnStreamChanged);
        }

        private void RosterManager_OnStreamChanged(object sender)
        {
            JabberClient cli = m_stream as JabberClient;
            if (cli == null)
                return;
            cli.OnIQ += new IQHandler(GotIQ);
            cli.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
        }

        /// <summary>
        /// The JabberClient to hook up to.
        /// </summary>
        [Description("The JabberClient to hook up to.")]
        [Category("Jabber")]
        [Browsable(false)]
        [Obsolete("Use the Stream property instead")]
        public JabberClient Client
        {
            get { return (JabberClient) this.Stream; }
            set { this.Stream = value; }
        }

        /// <summary>
        /// Convenience event for new roster items.
        /// </summary>
        [Description("Convenience event for new roster items.")]
        [Category("Jabber")]
        public event RosterItemHandler OnRosterItem;

        /// <summary>
        /// Fired when a roster result starts, before any OnRosterItem events fire.
        /// This will not fire for type='set', which is probably what you want.
        /// </summary>
        [Description("Roster result about to start being processed.")]
        [Category("Jabber")]
        public event bedrock.ObjectHandler OnRosterBegin;

        /// <summary>
        /// Fired when a roster result is completed being processed.
        /// </summary>
        [Description("Roster result finished being processed.")]
        [Category("Jabber")]
        public event bedrock.ObjectHandler OnRosterEnd;

        /// <summary>
        /// Get the currently-known version of a roster item for this jid.
        /// </summary>
        public Item this[JID jid]
        {
            get
            {
                lock (this)
                    return (Item) m_items[jid];
            }
        }

        /// <summary>
        /// Get the number of items currently in the roster.
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                    return m_items.Count;
            }
        }

        private void GotDisconnect(object sender)
        {
            lock (this)
                m_items.Clear();
        }

        /// <summary>
        /// Add a new roster item to the database.
        /// </summary>
        /// <param name="iq">An IQ containing a roster query.</param>
        public void AddRoster(IQ iq)
        {
            GotIQ(this, iq);
        }

        private void GotIQ(object sender, IQ iq)
        {
            if ((iq.Query == null) ||
                (iq.Query.NamespaceURI != jabber.protocol.URI.ROSTER) ||
                ((iq.Type != IQType.result) && (iq.Type != IQType.set)))
                return;

            iq.Handled = true;
            Roster r = (Roster) iq.Query;
            if ((iq.Type == IQType.result) && (OnRosterBegin != null))
                OnRosterBegin(this);

            foreach (Item i in r.GetItems())
            {
                lock (this)
                {
                    if (i.Subscription == Subscription.remove)
                    {
                        m_items.Remove(i.JID);
                    }
                    else
                    {
                        if (m_items.Contains(i.JID))
                            m_items.Remove(i.JID);
                        m_items[i.JID] = i;
                    }
                }
                if (OnRosterItem != null)
                    OnRosterItem(this, i);
            }

            if ((iq.Type == IQType.result) && (OnRosterEnd != null))
                OnRosterEnd(this);
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
