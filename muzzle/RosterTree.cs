/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

using bedrock.collections;
using jabber;
using jabber.client;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace muzzle
{
	/// <summary>
	/// A TreeView optimized for showing Jabber roster items.  Make sure that the 
	/// form you drop this on has a JabberClient, a PresenceManager, and a RosterManager
	/// on the form first, and this widget will automatically connect to them.
	/// </summary>
	public class RosterTree : System.Windows.Forms.TreeView
	{
        // image list offsets
        private const int OFFLINE    = 0;
        private const int ONLINE     = 1;
        private const int AWAY       = 2;
        private const int XA         = 3;
        private const int DND        = 4;
        private const int CHATTY     = 5;
        private const int EXPANDED   = 6;
        private const int COLLAPSED  = 7;

        private RosterManager   m_roster = null;
        private PresenceManager m_pres   = null;
        private JabberClient    m_client = null;

        private IDictionary m_groups = new SkipList();
        private IDictionary m_items  = new SkipList();

        private System.Windows.Forms.ImageList il;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Create a new RosterTree
        /// </summary>
		public RosterTree()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            this.ImageIndex = 1;
            this.ImageList = il;
            this.ShowRootLines = false;
            this.ShowLines = false;
            this.Sorted = true;
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RosterTree));
            this.il = new System.Windows.Forms.ImageList(this.components);
            // 
            // il
            // 
            this.il.ImageSize = new System.Drawing.Size(16, 16);
            this.il.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il.ImageStream")));
            this.il.TransparentColor = System.Drawing.Color.Magenta;

        }
		#endregion

        /// <summary>
        /// The RosterManager for this view
        /// </summary>
        [Category("Managers")]
        public RosterManager RosterManager
        {
            get 
            {
                // If we are running in the designer, let's try to auto-hook a JabberClient
                if ((m_roster == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost) base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is RosterManager)
                                {
                                    this.RosterManager = (RosterManager) c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_roster; 
            }
            set 
            { 
                m_roster = value; 
                m_roster.OnRosterBegin += new bedrock.ObjectHandler(m_roster_OnRosterBegin);
                m_roster.OnRosterEnd += new bedrock.ObjectHandler(m_roster_OnRosterEnd);
                m_roster.OnRosterItem += new RosterItemHandler(m_roster_OnRosterItem);
            }
        }

        /// <summary>
        /// The PresenceManager for this view
        /// </summary>
        [Category("Managers")]
        public PresenceManager PresenceManager
        {
            get 
            {
                // If we are running in the designer, let's try to auto-hook a JabberClient
                if ((m_roster == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost) base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is PresenceManager)
                                {
                                    this.PresenceManager = (PresenceManager) c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_pres; 
            }
            set 
            { 
                m_pres = value; 
            }
        }

        /// <summary>
        /// The PresenceManager for this view
        /// </summary>
        [Category("Managers")]
        public JabberClient Client
        {
            get 
            {
                // If we are running in the designer, let's try to auto-hook a JabberClient
                if ((m_roster == null) && DesignMode)
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
                                    this.Client = (JabberClient) c;
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
                m_client.OnDisconnect += new bedrock.ObjectHandler(m_client_OnDisconnect);
                m_client.OnPresence += new PresenceHandler(m_client_OnPresence);
            }
        }

        private void m_roster_OnRosterBegin(object sender)
        {
            this.BeginUpdate();
        }

        private void m_roster_OnRosterEnd(object sender)
        {
            this.EndUpdate();
        }

        /// <summary>
        /// After a group node is expanded, change to the down-triangle image.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            e.Node.ImageIndex = EXPANDED;
            e.Node.SelectedImageIndex = EXPANDED;

            base.OnAfterExpand (e);
        }

        /// <summary>
        /// After a group node is collapsed, change to the right-triangle image.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            e.Node.ImageIndex = COLLAPSED;
            e.Node.SelectedImageIndex = COLLAPSED;

            base.OnAfterCollapse (e);
        }

        private void m_roster_OnRosterItem(object sender, jabber.protocol.iq.Item ri)
        {
            Group[] groups = ri.GetGroups();
            if (groups.Length == 0) 
            {
                groups = new Group[] { new Group(ri.OwnerDocument) };
                groups[0].GroupName = "Unfiled";
            }

            LinkedList nodelist = (LinkedList) m_items[ri.JID.ToString()];
            if (nodelist == null) 
            {
                nodelist = new LinkedList();
                m_items.Add(ri.JID.ToString(), nodelist);
            }
            else
            {
                // update to an existing item.  remove all of them, and start over.
                foreach (ItemNode i in nodelist)
                {
                    TreeNode gn = i.Parent;
                    i.Remove();
                    if ((gn != null) && (gn.Nodes.Count == 0))
                    {
                        m_groups.Remove(gn.Text);
                        gn.Remove();
                    }
                }
                nodelist.Clear();
            }

            foreach (Group g in groups)
            {
                TreeNode gn = (TreeNode) m_groups[g.GroupName];
                if (gn == null)
                {
                    gn = new TreeNode(g.GroupName, COLLAPSED, COLLAPSED);
                    m_groups.Add(g.GroupName, gn);
                    this.Nodes.Add(gn);
                }
                ItemNode i = new ItemNode(ri);
                i.ChangePresence(m_pres[ri.JID]);
                nodelist.Add(i);
                gn.Nodes.Add(i);
            }
        }

        private void m_client_OnDisconnect(object sender)
        {
            this.Nodes.Clear();
            m_groups.Clear();
            m_items.Clear();
        }

        private void m_client_OnPresence(object sender, Presence pres)
        {
            if ((pres.Type != PresenceType.available) &&
                (pres.Type != PresenceType.unavailable))
                return;

            LinkedList nodelist = (LinkedList) m_items[pres.From.Bare];
            if (nodelist == null) 
                return;

            foreach (ItemNode n in nodelist)
            {
                n.ChangePresence(pres);
            }
        }

        /// <summary>
        /// A TreeNode to hold a RosterItem
        /// </summary>
        public class ItemNode : TreeNode
        {
            private jabber.protocol.iq.Item i;

            /// <summary>
            /// Create an ItemNode
            /// </summary>
            /// <param name="ri">The roster item to create from</param>
            public ItemNode(jabber.protocol.iq.Item ri)
            {
                i = ri;
            }

            /// <summary>
            /// The JID of this Roster Item
            /// </summary>
            public JID JID
            {
                get { return i.JID; }
            }

            /// <summary>
            /// Update this roster item with new presence information
            /// </summary>
            /// <param name="p"></param>
            public void ChangePresence(Presence p)
            {
                SelectedImageIndex = ImageIndex = getPresenceImage(p);

                if ((p == null) || (p.Status == null) || (p.Status == ""))
                    Text = i.Nickname;
                else
                    Text = i.Nickname + " (" + p.Status + ")";
            }

            private static int getPresenceImage(Presence p)
            {
                if ((p == null) || (p.Type == PresenceType.unavailable))
                    return OFFLINE;

                switch (p.Show)
                {
                    case null:
                    case "":
                        return ONLINE;
                    case "away":
                        return AWAY;
                    case "xa":
                        return XA;
                    case "dnd":
                        return DND;
                    case "chat":
                        return CHATTY;
                }

                return OFFLINE;
            }
        }        
    }
}
