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
using System.Windows.Forms;

using jabber.client;
using jabber.connection;

namespace Example
{
    public class ServiceDisplay : UserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private TreeView tvServices;
        private Splitter splitter2;
        private PropertyGrid pgServices;
        private DiscoManager m_disco = null;
        private JabberClient m_stream = null;

        public ServiceDisplay()
        {
            InitializeComponent();
            tvServices.ShowNodeToolTips = true;
            tvServices.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvServices_NodeMouseDoubleClick);
            tvServices.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvServices_AfterSelect);
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
            this.tvServices = new System.Windows.Forms.TreeView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pgServices = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            //
            // tvServices
            //
            this.tvServices.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvServices.Location = new System.Drawing.Point(0, 0);
            this.tvServices.Name = "tvServices";
            this.tvServices.ShowLines = false;
            this.tvServices.ShowPlusMinus = false;
            this.tvServices.ShowRootLines = false;
            this.tvServices.Size = new System.Drawing.Size(175, 281);
            this.tvServices.TabIndex = 1;
            this.tvServices.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvServices_AfterCollapse);
            this.tvServices.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvServices_AfterExpand);
            //
            // splitter2
            //
            this.splitter2.Location = new System.Drawing.Point(175, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 281);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            //
            // pgServices
            //
            this.pgServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgServices.Location = new System.Drawing.Point(178, 0);
            this.pgServices.Name = "pgServices";
            this.pgServices.Size = new System.Drawing.Size(366, 281);
            this.pgServices.TabIndex = 3;
            //
            // ServiceDisplay
            //
            this.Controls.Add(this.pgServices);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.tvServices);
            this.Name = "ServiceDisplay";
            this.Size = new System.Drawing.Size(544, 281);
            this.ResumeLayout(false);

        }

        #endregion


        /// <summary>
        /// The JabberClient or JabberService to hook up to.
        /// </summary>
        [Description("The JabberClient to hook up to.")]
        [Category("Jabber")]
        public virtual JabberClient Stream
        {
            get
            {
                // If we are running in the designer, let's try to get an XmppStream control
                // from the environment.
                if ((this.m_stream == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    this.Stream = (JabberClient)jabber.connection.StreamComponent.GetComponentFromHost(host, typeof(JabberClient));
                }
                return m_stream;
            }
            set
            {
                if ((object)m_stream != (object)value)
                {
                    m_stream = value;
                    m_stream.OnAuthenticate += new bedrock.ObjectHandler(m_stream_OnAuthenticate);
                    m_stream.OnDisconnect += new bedrock.ObjectHandler(m_stream_OnDisconnect);
                }
            }
        }

        [Category("Jabber")]
        public DiscoManager DiscoManager
        {
            get
            {
                // If we are running in the designer, let's try to get a DiscoManager control
                // from the environment.
                if ((this.m_disco == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    this.m_disco = (DiscoManager)StreamComponent.GetComponentFromHost(host, typeof(DiscoManager));
                }
                return m_disco;
            }
            set
            {
                if ((object)m_disco != (object)value)
                    m_disco = value;
            }
        }

        [Category("Appearance")]
        public ImageList ImageList
        {
            get { return tvServices.ImageList; }
            set { tvServices.ImageList = value; }
        }

        private void m_stream_OnAuthenticate(object sender)
        {
            // TODO: some of this will break in 2003.
            jabber.connection.DiscoNode dn = DiscoNode.GetNode(m_stream.Server, null);
            TreeNode tn = tvServices.Nodes.Add(dn.Key, dn.Name);
            tn.ToolTipText = dn.Key.Replace('\u0000', '\n');
            tn.Tag = dn;
            tn.ImageIndex = 8;
            tn.SelectedImageIndex = 8;
            m_disco.BeginGetFeatures(dn, new jabber.connection.DiscoNodeHandler(GotInitialFeatures));
        }

        private void m_stream_OnDisconnect(object sender)
        {
            pgServices.SelectedObject = null;
            tvServices.Nodes.Clear();
        }


        private void tvServices_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 6;
            e.Node.SelectedImageIndex = 6;
        }

        private void tvServices_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 7;
            e.Node.SelectedImageIndex = 7;
        }

        private void tvServices_NodeMouseDoubleClick(object sender,
                                             TreeNodeMouseClickEventArgs e)
        {
            jabber.connection.DiscoNode dn = (jabber.connection.DiscoNode)e.Node.Tag;
            if (dn.Children == null)
                m_disco.BeginGetItems(dn.JID, dn.Node, new jabber.connection.DiscoNodeHandler(GotItems));
        }

        private void tvServices_AfterSelect(object sender, TreeViewEventArgs e)
        {
            jabber.connection.DiscoNode dn = (jabber.connection.DiscoNode)e.Node.Tag;
            m_disco.BeginGetFeatures(dn, new jabber.connection.DiscoNodeHandler(GotInfo));
        }

        private void GotInitialFeatures(jabber.connection.DiscoNode node)
        {
            m_disco.BeginGetItems(node, new jabber.connection.DiscoNodeHandler(GotItems));
        }

        private void GotItems(jabber.connection.DiscoNode node)
        {
            // TODO: some of this will break in 2003.
            TreeNode[] nodes = tvServices.Nodes.Find(node.Key, true);
            foreach (TreeNode n in nodes)
            {
                n.ImageIndex = 7;
                n.SelectedImageIndex = 7;
                foreach (jabber.connection.DiscoNode dn in node.Children)
                {
                    TreeNode tn = n.Nodes.Add(dn.Key, dn.Name);
                    tn.ToolTipText = dn.Key.Replace('\u0000', '\n');
                    tn.Tag = dn;
                    tn.ImageIndex = 8;
                    tn.SelectedImageIndex = 8;
                }
            }
            pgServices.Refresh();
        }

        private void GotInfo(jabber.connection.DiscoNode node)
        {
            pgServices.SelectedObject = node;
        }
    }
}
