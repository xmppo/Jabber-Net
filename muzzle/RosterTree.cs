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
using System.ComponentModel.Design;
using System.Drawing;
using System.Diagnostics;
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

        private const string UNFILED = "Unfiled";

        private RosterManager   m_roster = null;
        private PresenceManager m_pres   = null;
        private JabberClient    m_client = null;

        private IDictionary m_groups = new SkipList();
        private IDictionary m_items  = new SkipList();

        private System.Windows.Forms.ImageList il;
        private System.Windows.Forms.ToolTip tt;
        private Color m_statusColor = Color.Teal;

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

            this.AllowDrop = true;
            this.ItemDrag += new ItemDragEventHandler(RosterTree_ItemDrag);
            this.DragEnter += new DragEventHandler(RosterTree_DragEnter);
            this.DragOver += new DragEventHandler(RosterTree_DragOver);
            this.DragDrop += new DragEventHandler(RosterTree_DragDrop);
#if NET20
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.DrawNode += new DrawTreeNodeEventHandler(RosterTree_DrawNode);
#endif
        }


#if NET20
        private void DrawGroup(DrawTreeNodeEventArgs e)
        {
            GroupNode node = (GroupNode)e.Node;
            string counts = String.Format("({0}/{1})", node.Current, node.Total);

            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
            {
                string newText = node.GroupName + " " + counts;
                if (node.Text != newText)
                    node.Text = newText;
                e.DrawDefault = true;
                return;
            }

            Graphics g = e.Graphics;
            Brush fg = new SolidBrush(this.ForeColor);
            Brush stat_fg = new SolidBrush(this.StatusColor);

            g.DrawString(node.GroupName, this.Font, fg, new Point(e.Bounds.Left, e.Bounds.Top), StringFormat.GenericTypographic);
            if (node.Total > 0)
            {
                SizeF name_size = g.MeasureString(node.GroupName, this.Font);
                g.DrawString(counts, this.Font, stat_fg, new PointF(e.Bounds.Left + name_size.Width, e.Bounds.Top), StringFormat.GenericTypographic);
            }
        }

        private void DrawItem(DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
            {
                e.DrawDefault = true;
                /*
                Brush sel = new SolidBrush(SystemColors.Highlight);
                Brush sel_fg = new SolidBrush(SystemColors.HighlightText);
                g.FillRectangle(sel, e.Bounds);
                g.DrawString(node.Text, this.Font, sel_fg, new Point(e.Bounds.Left, e.Bounds.Top), StringFormat.GenericTypographic);
                 */
                return;
            }

            ItemNode node = (ItemNode)e.Node;
            Graphics g = e.Graphics;
            Brush fg = new SolidBrush(this.ForeColor);
            Brush stat_fg = new SolidBrush(this.StatusColor);

            g.DrawString(node.Nickname, this.Font, fg, new Point(e.Bounds.Left, e.Bounds.Top), StringFormat.GenericTypographic);
            if (node.Status != null)
            {
                SizeF nick_size = g.MeasureString(node.Nickname, this.Font);
                g.DrawString("(" + node.Status + ")", this.Font, stat_fg, new PointF(e.Bounds.Left + nick_size.Width, e.Bounds.Top), StringFormat.GenericTypographic);
            }
        }
       

        private void RosterTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node is GroupNode)
                DrawGroup(e);
            else if (e.Node is ItemNode)
                DrawItem(e);
            else
                e.DrawDefault = true; // or assert(false)
        }

#endif

        private GroupNode GetDropGroup(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(ItemNode)))
                return null;

            Point pt = this.PointToClient(new Point(e.X, e.Y));
            TreeNode node = this.GetNodeAt(pt);
            while (!(node is GroupNode) && (node != null))
            {
                node = node.Parent;
            }
            if (node == null)
                return null;

            ItemNode item = e.Data.GetData(typeof(ItemNode)) as ItemNode;
            if (item.Parent == node)
                return null;
            return (GroupNode)node;
        }
        
        private void RosterTree_DragDrop(object sender, DragEventArgs e)
        {
            GroupNode group = GetDropGroup(e);
            if (group == null)
                return;
            ItemNode item = e.Data.GetData(typeof(ItemNode)) as ItemNode;
            GroupNode parent = (GroupNode)item.Parent;
            Item i = (Item)item.Item.CloneNode(true, m_client.Document);
            i.RemoveGroup(parent.GroupName);
            i.AddGroup(group.GroupName);
            m_roster.Modify(i);
        }


        private void RosterTree_DragOver(object sender, DragEventArgs e)
        {
            if (GetDropGroup(e) == null)
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }


        private void RosterTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ItemNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void RosterTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is ItemNode)
                this.DoDragDrop(e.Item, DragDropEffects.Move);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RosterTree));
            this.il = new System.Windows.Forms.ImageList(this.components);
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            //
            // il
            //
            this.il.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il.ImageStream")));
            this.il.TransparentColor = System.Drawing.Color.Magenta;
#if NET20
            this.il.Images.SetKeyName(0, "");
            this.il.Images.SetKeyName(1, "");
            this.il.Images.SetKeyName(2, "");
            this.il.Images.SetKeyName(3, "");
            this.il.Images.SetKeyName(4, "");
            this.il.Images.SetKeyName(5, "");
            this.il.Images.SetKeyName(6, "");
            this.il.Images.SetKeyName(7, "");
            this.il.Images.SetKeyName(8, "blank");
#endif
            this.ResumeLayout(false);

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
                    this.RosterManager = (RosterManager)jabber.connection.StreamComponent.GetComponentFromHost(host, typeof(RosterManager));
                }
                return m_roster;
            }
            set
            {
                m_roster = value;
                if (m_roster != null)
                {
                    m_roster.OnRosterBegin += new bedrock.ObjectHandler(m_roster_OnRosterBegin);
                    m_roster.OnRosterEnd += new bedrock.ObjectHandler(m_roster_OnRosterEnd);
                    m_roster.OnRosterItem += new RosterItemHandler(m_roster_OnRosterItem);
                }
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
                    this.PresenceManager = (PresenceManager)jabber.connection.StreamComponent.GetComponentFromHost(host, typeof(PresenceManager));
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
                if ((m_client == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost) base.GetService(typeof(IDesignerHost));
                    this.Client = (JabberClient)jabber.connection.StreamComponent.GetComponentFromHost(host, typeof(JabberClient));
                }
                return m_client;
            }
            set
            {
                m_client = value;
                if (m_client != null)
                {
                    m_client.OnDisconnect += new bedrock.ObjectHandler(m_client_OnDisconnect);
                    m_client.OnPresence += new PresenceHandler(m_client_OnPresence);
                }
            }
        }

        /// <summary>
        /// Color to draw status text with.  Not applicable until .Net 2.0.
        /// </summary>
        [Category("Appearance")]
        public Color StatusColor
        {
            get { return m_statusColor; }
            set { m_statusColor = value; }
        }

        /// <summary>
        /// Should we draw status text next to each roster item?  Not applicable until .Net 2.0.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool DrawStatus
        {
            get 
            { 
#if NET20
                return (this.DrawMode == TreeViewDrawMode.OwnerDrawText); 
#else
                return false;
#endif
            }
            set 
            {
#if NET20
                if (value)
                    this.DrawMode = TreeViewDrawMode.OwnerDrawText;
                else
                    this.DrawMode = TreeViewDrawMode.Normal;
#endif
            }
        }

        /// <summary>
        /// The group names for the roster
        /// </summary>
        public string[] Groups
        {
            get
            {
                string[] g = new string[m_groups.Count];
                m_groups.Keys.CopyTo(g, 0);
                return g;
            }
        }

        /// <summary>
        /// Add a new, empty group, if this group doesn't exist, otherwise a no-op.
        /// </summary>
        /// <param name="groupName"></param>
        public TreeNode AddGroup(string groupName)
        {
            Group g = new Group(m_client.Document);
            g.GroupName = groupName;
            return AddGroupNode(g);
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

        /// <summary>
        /// When mousing over a node, show a tooltip with the full JID.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            ItemNode node = this.GetNodeAt(e.X, e.Y) as ItemNode;
            if (node == null)
            { // none selected, or a group
                tt.SetToolTip(this, "");
                return;
            }
            if (node.JID.ToString() != tt.GetToolTip(this))
            {
                tt.SetToolTip(this, node.JID.ToString());
            }
        }

        private GroupNode AddGroupNode(Group g)
        {
            GroupNode gn = (GroupNode)m_groups[g.GroupName];
            if (gn == null)
            {
                gn = new GroupNode(g);
                m_groups.Add(g.GroupName, gn);
                this.Nodes.Add(gn);
            }
            return gn;
        }

        private void m_roster_OnRosterItem(object sender, jabber.protocol.iq.Item ri)
        {
            bool remove = (ri.Subscription == Subscription.remove);

            LinkedList nodelist = (LinkedList)m_items[ri.JID.ToString()];
            if (nodelist == null)
            {
                // First time through.
                if (!remove)
                {
                    nodelist = new LinkedList();
                    m_items.Add(ri.JID.ToString(), nodelist);
                }
            }
            else
            {
                // update to an existing item.  remove all of them, and start over.
                foreach (ItemNode i in nodelist)
                {
                    GroupNode gn = i.Parent as GroupNode;
                    i.Remove();
                    if ((gn != null) && (gn.Nodes.Count == 0))
                    {
                        m_groups.Remove(gn.GroupName);
                        gn.Remove();
                    }
                }
                nodelist.Clear();
                if (remove)
                    m_items.Remove(ri.JID.ToString());
            }

            if (remove)
                return;

            // add the new ones back
            Hashtable ghash = new Hashtable();
            Group[] groups = ri.GetGroups();
            for (int i=groups.Length-1; i>=0; i--)
            {
                if (groups[i].GroupName == "")
                    groups[i].GroupName = UNFILED;
            }

            if (groups.Length == 0)
            {
                groups = new Group[] { new Group(ri.OwnerDocument) };
                groups[0].GroupName = UNFILED;
            }

            foreach (Group g in groups)
            {
                GroupNode gn = AddGroupNode(g);
                // might have the same group twice.
                if (ghash.Contains(g.GroupName))
                    continue;
                ghash.Add(g.GroupName, g);

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
        /// A TreeNode to hold a Roster Group
        /// </summary>
        public class GroupNode : TreeNode
        {
            private jabber.protocol.iq.Group m_group;

            /// <summary>
            /// Create a GroupNode
            /// </summary>
            /// <param name="rg"></param>
            public GroupNode(jabber.protocol.iq.Group rg) : base(rg.GroupName, COLLAPSED, COLLAPSED)
            {
                m_group = rg;
            }

            /// <summary>
            /// The name of the group
            /// </summary>
            public string GroupName
            {
                get { return m_group.GroupName; }
            }

            /// <summary>
            /// Total number of members of the group
            /// </summary>
            public int Total
            {
                // TODO: what if we're not showing offline?
                get { return this.Nodes.Count; }
            }

            /// <summary>
            /// Current number of online members of the group
            /// </summary>
            public int Current
            {
                get 
                {
                    int count = 0;
                    foreach (ItemNode i in this.Nodes)
                    {
                        if (i.ImageIndex != OFFLINE)
                            count++;
                    }
                    return count;
                }
            }
        }

        /// <summary>
        /// A TreeNode to hold a RosterItem
        /// </summary>
        public class ItemNode : TreeNode
        {
            private jabber.protocol.iq.Item m_item;
            private string m_status = null;
            private string m_nick = null;

            /// <summary>
            /// Create an ItemNode
            /// </summary>
            /// <param name="ri">The roster item to create from</param>
            public ItemNode(jabber.protocol.iq.Item ri)
            {
                m_item = ri;
                m_nick = ri.Nickname;
                if (m_nick == "")
                {
                    m_nick = ri.JID.User;
                    if (m_nick == null)
                        m_nick = ri.JID.ToString(); // punt.
                }
                this.Text = m_nick;
            }

            /// <summary>
            /// The JID of this Roster Item
            /// </summary>
            public JID JID
            {
                get { return m_item.JID; }
            }

            /// <summary>
            /// Roster nickname for this user.
            /// </summary>
            public string Nickname
            {
                get { return m_nick; }
            }

            /// <summary>
            /// Last presence status for this item
            /// </summary>
            public string Status
            {
                get { return m_status; }
            }

            /// <summary>
            /// The roster item.  Please make a clone before using it.
            /// </summary>
            public Item Item
            {
                get { return m_item; }
            }

            /// <summary>
            /// Update this roster item with new presence information
            /// </summary>
            /// <param name="p"></param>
            public void ChangePresence(Presence p)
            {
                SelectedImageIndex = ImageIndex = getPresenceImage(p);

                string txt = null;
                if ((p == null) || (p.Status == null) || (p.Status == ""))
                {
                    txt = m_nick;
                    m_status = null;
                }
                else
                {
                    m_status = p.Status;
                    txt = m_nick + " (" + m_status + ")";
                }
                if (Text != txt)
                    Text = txt;
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
