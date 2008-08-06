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
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using bedrock.util;
using jabber;

namespace Example
{
    [SVN(@"$Id$")]
    public class AddContact : Form
    {

        private Label label1;
        private TextBox txtJID;
        private Label label2;
        private TextBox txtNickname;
        private Label label3;
        private CheckedListBox lbGroups;
        private Button btnOK;
        private Button btnCancel;
        private TextBox txtGroup;
        private Button btnAdd;

        private string m_domain = null;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public AddContact()
        {
            InitializeComponent();
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

        /// <summary>
        /// The Jabber ID to subscribe to.
        /// </summary>
        public JID JID
        {
            get
            {
                return new JID(txtJID.Text);
            }
            set
            {
                txtJID.Text = value.ToString();
            }
        }

        public string Nickname
        {
            get
            {
                return txtNickname.Text;
            }
            set
            {
                txtNickname.Text = value;
            }
        }

        /// <summary>
        /// All of the groups, checked or not.
        /// </summary>
        public string[] AllGroups
        {
            get
            {
                string[] items = new string[lbGroups.Items.Count];
                lbGroups.Items.CopyTo(items, 0);
                return items;
            }
            set
            {
                lbGroups.BeginUpdate();
                lbGroups.Items.Clear();
                lbGroups.Items.AddRange(value);
                lbGroups.EndUpdate();
            }
        }

        /// <summary>
        /// The groups that have been selected
        /// </summary>
        public string[] SelectedGroups
        {
            get
            {
                string[] items = new string[lbGroups.CheckedItems.Count];
                lbGroups.CheckedItems.CopyTo(items, 0);
                return items;
            }
            set
            {
                lbGroups.BeginUpdate();
                lbGroups.ClearSelected();
                for (int i=0; i<lbGroups.Items.Count; i++ )
                {
                    for (int j=0; j<value.Length; j++)
                    {
                        if (((string)lbGroups.Items[i]) == value[j])
                        {
                            lbGroups.SetItemChecked(i, true);
                        }
                    }
                }
                lbGroups.EndUpdate();
            }
        }

        /// <summary>
        /// Use this domain, if one isn't provided in the JID.
        /// </summary>
        public string DefaultDomain
        {
            get { return m_domain; }
            set { m_domain = value; }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtJID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbGroups = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "JID:";
            //
            // txtJID
            //
            this.txtJID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJID.Location = new System.Drawing.Point(86, 6);
            this.txtJID.Name = "txtJID";
            this.txtJID.Size = new System.Drawing.Size(269, 20);
            this.txtJID.TabIndex = 1;
            this.txtJID.Leave += new System.EventHandler(this.txtJID_Leave);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nickname:";
            //
            // txtNickname
            //
            this.txtNickname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNickname.Location = new System.Drawing.Point(86, 32);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(269, 20);
            this.txtNickname.TabIndex = 3;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Groups:";
            //
            // lbGroups
            //
            this.lbGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbGroups.Location = new System.Drawing.Point(86, 58);
            this.lbGroups.Name = "lbGroups";
            this.lbGroups.Size = new System.Drawing.Size(269, 124);
            this.lbGroups.Sorted = true;
            this.lbGroups.TabIndex = 5;
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(199, 215);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(280, 216);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            //
            // txtGroup
            //
            this.txtGroup.Location = new System.Drawing.Point(86, 190);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(269, 20);
            this.txtGroup.TabIndex = 6;
            this.txtGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroup_KeyDown);
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(12, 188);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add Group";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // AddContact
            //
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(367, 250);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtGroup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbGroups);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNickname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtJID);
            this.Controls.Add(this.label1);
            this.Name = "AddContact";
            this.Text = "Add Contact";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string g = txtGroup.Text.Trim();
            if (g != "")
            {
                int item = lbGroups.Items.Add(g, true);
                lbGroups.TopIndex = item;
                txtGroup.Clear();
            }
        }

        private void txtGroup_KeyDown(object sender, KeyEventArgs e)
        {
            // TODO: this doesn't actually work.
            if (e.KeyCode == Keys.Return)
            {
                btnAdd_Click(null, null);
#if NET_20
                e.SuppressKeyPress = true;
#endif
            }
        }

        private void txtJID_Leave(object sender, EventArgs e)
        {
            if (!txtJID.Text.Contains("@") && (m_domain != null))
            {
                txtJID.Text = txtJID.Text + "@" + m_domain;
            }
            if (txtNickname.Text == "")
            {
                JID jid = new JID(txtJID.Text);
                txtNickname.Text = jid.User;
            }
        }

    }
}
