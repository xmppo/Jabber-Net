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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using bedrock.util;
using jabber;
using jabber.connection;

namespace Example
{
    [SVN(@"$Id$")]
    public class ConferenceForm : Form
    {
        private Label label1;
        private Label label2;
        private ComboBox cmbJID;
        private TextBox txtRoom;
        private Button btnOK;
        private Button btnCancel;

        private DiscoManager m_disco = null;
        private TextBox txtNick;
        private Label label3;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ConferenceForm()
        {
            InitializeComponent();
        }

        public DiscoManager DiscoManager
        {
            get { return m_disco; }
            set { m_disco = value; }
        }

        public JID RoomAndNick
        {
            get
            {
                return new JID(txtRoom.Text, cmbJID.Text, txtNick.Text);
            }
            set
            {
                cmbJID.Text = value.Server;
                txtRoom.Text = value.User;
                txtNick.Text = value.Resource;
            }
        }

        public string Nick
        {
            set { txtNick.Text = value; }
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
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtNick = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Conference Server:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Room:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cmbJID
            //
            this.cmbJID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbJID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbJID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbJID.Location = new System.Drawing.Point(118, 5);
            this.cmbJID.Name = "cmbJID";
            this.cmbJID.Size = new System.Drawing.Size(156, 21);
            this.cmbJID.TabIndex = 1;
            //
            // txtRoom
            //
            this.txtRoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRoom.Location = new System.Drawing.Point(118, 32);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(156, 20);
            this.txtRoom.TabIndex = 3;
            //
            // btnOK
            //
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(118, 84);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(199, 84);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // txtNick
            //
            this.txtNick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNick.Location = new System.Drawing.Point(118, 58);
            this.txtNick.Name = "txtNick";
            this.txtNick.Size = new System.Drawing.Size(156, 20);
            this.txtNick.TabIndex = 5;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Nickname:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // ConferenceForm
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(286, 117);
            this.Controls.Add(this.txtNick);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtRoom);
            this.Controls.Add(this.cmbJID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ConferenceForm";
            this.Text = "Join/Create room";
            this.Shown += new System.EventHandler(this.ConferenceForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private void ConferenceForm_Shown(object sender, EventArgs e)
        {
            cmbJID.BeginUpdate();
            cmbJID.Items.Clear();
            foreach (DiscoNode component in m_disco.Root.Children)
            {
                if (component.HasFeature(jabber.protocol.URI.MUC))
                    cmbJID.Items.Add(component.JID);
            }
            if (cmbJID.Items.Count > 0)
                cmbJID.SelectedIndex = 0;
            cmbJID.EndUpdate();
        }

    }
}
