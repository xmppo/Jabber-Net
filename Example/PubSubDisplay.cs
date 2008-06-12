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
using System.Diagnostics;
using System.Windows.Forms;
using jabber.connection;

using bedrock.util;

namespace Example
{
    [SVN(@"$Id$")]
    public class PubSubDisplay : UserControl
    {
        private ListBox lbID;
        private Splitter splitter1;
        private RichTextBox rtItem;

        private PubSubNode m_node = null;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

        public PubSubDisplay()
        {
            InitializeComponent();
        }

        public PubSubNode Node
        {
            get { return m_node; }
            set
            {
                if (m_node == value)
                    return;
                if (m_node != null)
                {
                    m_node.OnItemAdd -= m_node_OnItemAdd;
                    m_node.OnItemRemove -= m_node_OnItemRemove;
                }
                m_node = value;
                m_node.OnItemAdd += m_node_OnItemAdd;
                m_node.OnItemRemove += m_node_OnItemRemove;
                m_node.AutomatedSubscribe();
            }
        }

        private void m_node_OnItemAdd(PubSubNode node, jabber.protocol.iq.PubSubItem item)
        {
            // OnItemRemove should have fired first, so no reason to remove it here.
            // Hopefully.
            Debug.Assert(lbID.Items.IndexOf(item.ID) == -1);
            lbID.Items.Add(item.ID);
        }

        private void m_node_OnItemRemove(PubSubNode node, jabber.protocol.iq.PubSubItem item)
        {
            int index = lbID.Items.IndexOf(item.ID);
            if (lbID.SelectedIndex == index)
                rtItem.Clear();
            if (index >= 0)
                lbID.Items.RemoveAt(index);
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
            this.lbID = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.rtItem = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            //
            // lbID
            //
            this.lbID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbID.FormattingEnabled = true;
            this.lbID.IntegralHeight = false;
            this.lbID.Location = new System.Drawing.Point(0, 0);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(120, 170);
            this.lbID.TabIndex = 0;
            this.lbID.SelectedIndexChanged += new System.EventHandler(this.lbID_SelectedIndexChanged);
            //
            // splitter1
            //
            this.splitter1.Location = new System.Drawing.Point(120, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 170);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            //
            // rtItem
            //
            this.rtItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtItem.Location = new System.Drawing.Point(123, 0);
            this.rtItem.Name = "rtItem";
            this.rtItem.Size = new System.Drawing.Size(236, 170);
            this.rtItem.TabIndex = 2;
            this.rtItem.Text = "";
            //
            // PubSubDisplay
            //
            this.Controls.Add(this.rtItem);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.lbID);
            this.Name = "PubSubDisplay";
            this.Size = new System.Drawing.Size(359, 170);
            this.ResumeLayout(false);

        }

        #endregion

        private void lbID_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtItem.Clear();
            if (lbID.SelectedIndex == -1)
                return;
            // TODO: XML2RTF
            rtItem.Text = m_node[(string)lbID.SelectedItem].OuterXml;
        }
    }
}
