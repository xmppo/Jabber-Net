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
    [RCS(@"$Header$")]
    public class RosterManager : System.ComponentModel.Component
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private JabberClient m_client = null;
        private Tree m_items = new Tree();

        /// <summary>
        /// Create a roster manager inside a container
        /// </summary>
        /// <param name="container"></param>
        public RosterManager(System.ComponentModel.IContainer container)
        {
            // Required for Windows.Forms Class Composition Designer support
            container.Add(this);
            InitializeComponent();

        }

        /// <summary>
        /// Create a roster manager
        /// </summary>
        public RosterManager()
        {
            // Required for Windows.Forms Class Composition Designer support
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
                m_client.OnIQ += new IQHandler(GotIQ);
				m_client.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
			}
        }

        /// <summary>
        /// Convenience event for new roster items.
        /// </summary>
        [Description("Convenience event for new roster items.")]
        [Category("Jabber")]        
        public event RosterItemHandler OnRosterItem;

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

		private void GotDisconnect(object sender)
		{
			lock (this)
				m_items.Clear();
		}

        private void GotIQ(object sender, IQ iq)
        {
            if ((iq.Query == null) ||
                (iq.Query.NamespaceURI != jabber.protocol.URI.ROSTER) ||
                ((iq.Type != IQType.result) && (iq.Type != IQType.set)))
                return;

            Roster r = (Roster) iq.Query;
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
						m_items[i.JID] = i;
					}
				}
                if (OnRosterItem != null)
                    OnRosterItem(this, i);
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
    }
}
