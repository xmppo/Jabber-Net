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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using bedrock.util;
using jabber;
using jabber.connection;

namespace Example
{
    [SVN(@"$Id$")]
    public class PubSubSubcribeForm : Form
    {
        private Label label1;
        private Label label2;
        private ComboBox cmbJID;
        private TextBox txtNode;
        private Button btnOK;
        private Button btnCancel;

        private DiscoManager m_disco = null;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public PubSubSubcribeForm()
        {
            InitializeComponent();
        }

        public DiscoManager DiscoManager
        {
            get { return m_disco; }
            set { m_disco = value; }
        }

        public JID JID
        {
            get { return (JID)cmbJID.Text; }
            set { cmbJID.Text = value.ToString(); }
        }

        public string Node
        {
            get { return txtNode.Text; }
            set { txtNode.Text = value; }
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbJID = new System.Windows.Forms.ComboBox();
            this.txtNode = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PubSub Service JID:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Node:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbJID
            // 
            this.cmbJID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbJID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbJID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbJID.Location = new System.Drawing.Point(124, 6);
            this.cmbJID.Name = "cmbJID";
            this.cmbJID.Size = new System.Drawing.Size(141, 21);
            this.cmbJID.TabIndex = 4;
            // 
            // txtNode
            // 
            this.txtNode.Location = new System.Drawing.Point(124, 32);
            this.txtNode.Name = "txtNode";
            this.txtNode.Size = new System.Drawing.Size(141, 20);
            this.txtNode.TabIndex = 5;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(109, 58);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(190, 58);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // PubSub
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(277, 90);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtNode);
            this.Controls.Add(this.cmbJID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PubSub";
            this.Text = "Subscribe to PubSub Node";
            this.Shown += new System.EventHandler(this.PubSub_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void PubSub_Shown(object sender, EventArgs e)
        {
            cmbJID.BeginUpdate();
            cmbJID.Items.Clear();
            foreach (DiscoNode component in m_disco.Root.Children)
            {
                if (component.HasFeature(jabber.protocol.URI.PUBSUB))
                    cmbJID.Items.Add(component.JID);
            }
            if (cmbJID.Items.Count > 0)
                cmbJID.SelectedIndex = 0;
            cmbJID.EndUpdate();
        }

    }
}