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
 * Portions Copyright (c) 2003 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using jabber.protocol.x;

namespace muzzle
{
	/// <summary>
	/// Summary description for XData.
	/// </summary>
	public class XDataForm : System.Windows.Forms.Form
	{
        private static Regex WS = new Regex("[ \r\n\t]+", RegexOptions.Compiled);

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Panel pnlFields;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// Create an x:data form with no contents.
        /// </summary>
		public XDataForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

        /// <summary>
        /// Create an x:data form from the given XML
        /// </summary>
        /// <param name="x"></param>
        public XDataForm(jabber.protocol.x.Data x) : this()
        {
            this.SuspendLayout();
            if (x.Title != null)
                this.Text = x.Title;
            if (x.Instructions == null)
                lblInstructions.Visible = false;
            else
                lblInstructions.Text = DeWhitespace(x.Instructions);

            pnlFields.SuspendLayout();
            Field[] fields = x.GetFields();
            FormField ff = null;
            for (int i=fields.Length-1; i>=0; i--)
            {
                ff = new FormField(fields[i], this, i);
            }
            panel1.TabIndex = fields.Length;
            pnlFields.ResumeLayout(true);
            this.ResumeLayout(true);
            if (ff != null)
                ff.Focus();
        }

        private string DeWhitespace(string input)
        {
            return WS.Replace(input, " ");
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.pnlFields = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 234);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 32);
            this.panel1.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(212, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(132, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInstructions.Location = new System.Drawing.Point(0, 0);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(0, 16);
            this.lblInstructions.TabIndex = 1;
            // 
            // pnlFields
            // 
            this.pnlFields.AutoScroll = true;
            this.pnlFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFields.Location = new System.Drawing.Point(0, 16);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(292, 218);
            this.pnlFields.TabIndex = 0;
            // 
            // XDataForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFields);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.Name = "XDataForm";
            this.Text = "XData Form";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private class FormField
        {
            private FieldType m_type;
            private string m_var;
            private string m_val;
            private XDataForm m_form;
            private Control m_control;

            public FormField(Field f, XDataForm form, int tabIndex)
            {
                m_type = f.Type;
                m_var = f.Var;
                m_val = f.Val;
                m_form = form;

                if (m_type != FieldType.hidden)
                {
                    Panel p = new Panel();
                    p.Parent = m_form.pnlFields;
                    p.TabIndex = tabIndex;

                    TextBox txt = new TextBox();
                    txt.Text = m_val;

                    m_control = txt;

                    m_control.Parent = p;
                    m_control.Dock = DockStyle.Fill;
                    p.Controls.Add(m_control);

                    Label caption = new Label();
                    caption.Parent = p;
                    if (f.Label != "")
                        caption.Text = f.Label + ":";
                    else
                        caption.Text = f.Var + ":";
                    caption.TextAlign = ContentAlignment.MiddleLeft;
                    caption.Dock = DockStyle.Left;
                    p.Controls.Add(caption);


                    p.Height = m_form.btnOK.Height;
                    p.Dock = DockStyle.Top;
                    m_form.pnlFields.Controls.Add(p);
                }
            }

            public void Focus()
            {
                m_control.Focus();
            }

            public Field GetField()
            {
                return null;
            }
        }
	}
}
