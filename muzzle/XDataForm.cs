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

        private FormField[] m_fields = null;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Panel pnlFields;
        private System.Windows.Forms.ErrorProvider error;
        private System.Windows.Forms.ToolTip tip;
        private System.ComponentModel.IContainer components;

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
            {
                lblInstructions.Text = DeWhitespace(x.Instructions);
                lblInstructions.Resize += new EventHandler(lblInstructions_Resize);
                lblInstructions_Resize(lblInstructions, null);
            }

            pnlFields.SuspendLayout();
            Field[] fields = x.GetFields();
            m_fields = new FormField[fields.Length];
            for (int i=fields.Length-1; i>=0; i--)
            {
                m_fields[i] = new FormField(fields[i], this, i);
            }
            panel1.TabIndex = fields.Length;
            pnlFields.ResumeLayout(true);
            this.ResumeLayout(true);

            for (int i=0; i<fields.Length; i++)
            {
                if ((fields[i].Type != FieldType.hidden) &&
                    (fields[i].Type != FieldType.Fixed))
                    m_fields[i].Focus();
            }
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.pnlFields = new System.Windows.Forms.Panel();
            this.error = new System.Windows.Forms.ErrorProvider();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.CausesValidation = false;
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
            this.btnCancel.CausesValidation = false;
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
            this.lblInstructions.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblInstructions.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInstructions.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblInstructions.Location = new System.Drawing.Point(0, 0);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(292, 16);
            this.lblInstructions.TabIndex = 1;
            // 
            // pnlFields
            // 
            this.pnlFields.AutoScroll = true;
            this.pnlFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFields.DockPadding.Bottom = 3;
            this.pnlFields.DockPadding.Left = 6;
            this.pnlFields.DockPadding.Right = 6;
            this.pnlFields.DockPadding.Top = 3;
            this.pnlFields.Location = new System.Drawing.Point(0, 16);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(292, 218);
            this.pnlFields.TabIndex = 0;
            // 
            // error
            // 
            this.error.ContainerControl = this;
            // 
            // XDataForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFields);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblInstructions);
            this.HelpButton = true;
            this.Name = "XDataForm";
            this.Text = "XData Form";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            for (int i=0; i<m_fields.Length; i++)
            {
                if (!m_fields[i].Validate())
                {
                    m_fields[i].Focus();
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void lblInstructions_Resize(object sender, EventArgs e)
        {
            Graphics graphics = lblInstructions.CreateGraphics();
            SizeF s = lblInstructions.Size;
            s.Height = 0;
            SizeF textSize = graphics.MeasureString(lblInstructions.Text, lblInstructions.Font, s);
            lblInstructions.Height = (int) (textSize.Height);
        }

        private class FormField
        {
            private FieldType m_type;
            private string m_var;
            private string[] m_val;
            private XDataForm m_form;
            private bool m_required = false;
            private Control m_control = null;
            private Label m_label = null;
            private Field m_field;

            public FormField(Field f, XDataForm form, int tabIndex)
            {
                m_field = f;
                m_type = f.Type;
                m_var = f.Var;
                m_val = f.Vals;
                m_required = f.IsRequired;
                m_form = form;

                Panel p = null;
                if (m_type != FieldType.hidden)
                {
                    p = new Panel();
                    p.Parent = m_form.pnlFields;
                    p.TabIndex = tabIndex;
                }

                switch (m_type)
                {
                    case FieldType.hidden:
                        break;
                    case FieldType.boolean:
                        CheckBox cb = new CheckBox();
                        cb.Checked = f.BoolVal;
                        cb.Text = null;
                        m_control = cb;
                        break;
                    case FieldType.text_multi:
                        TextBox mtxt = new TextBox();
                        mtxt.Multiline = true;
                        mtxt.ScrollBars = ScrollBars.Vertical;
                        mtxt.Lines = m_val;
                        mtxt.Height = m_form.btnOK.Height * 3;
                        m_control = mtxt;
                        break;
                    case FieldType.text_private:
                        TextBox ptxt = new TextBox();
                        ptxt.Lines = m_val;
                        ptxt.PasswordChar = '*';
                        m_control = ptxt;
                        break;
                    case FieldType.list_single:
                        ComboBox box = new ComboBox();
                        box.DropDownStyle = ComboBoxStyle.DropDownList;
                        box.BeginUpdate();
                        string v = null;
                        if (m_val.Length > 0)
                            v = m_val[0];
                        foreach (Option o in f.GetOptions())
                        {
                            int i = box.Items.Add(o);
                            
                            if (o.Val == v)
                            {
                                box.SelectedIndex = i;
                            }
                        }
                        box.EndUpdate();
                        m_control = box;
                        break;

                    case FieldType.list_multi:
                        ListBox lb = new ListBox();
                        lb.SelectionMode = SelectionMode.MultiExtended;
                        lb.VisibleChanged += new EventHandler(lb_VisibleChanged);
                        m_control = lb;
                        break;

                    case FieldType.jid_single:
                        TextBox jtxt = new TextBox();
                        jtxt.Lines = m_val;
                        jtxt.Validating += new CancelEventHandler(jid_Validating);
                        jtxt.Validated += new EventHandler(jid_Validated);

                        m_control = jtxt;
                        break;

                   // case FieldType.jid_multi:
                     //   throw new NotImplementedException();
                        //break;

                    case FieldType.Fixed:
                        Label lbl = new Label();
                        lbl.Text = string.Join("\r\n", f.Vals);
                        lbl.Resize += new EventHandler(lbl_Resize);
                        m_control = lbl;
                        break;
                    default:
                        TextBox txt = new TextBox();
                        txt.Lines = m_val;
                        m_control = txt;
                        break;
                }

                if (m_type != FieldType.hidden)
                {

                    if (f.Desc != null)
                        form.tip.SetToolTip(m_control, f.Desc);

                    m_label = new Label();
                    m_label.Parent = p;
                    if (f.Label != "")
                        m_label.Text = f.Label + ":";
                    else if (f.Var != "")
                        m_label.Text = f.Var + ":";
                    else
                        m_label.Text = "";

                    if (m_required)
                    {
                        m_label.Text = "* " + m_label.Text;
                        m_form.error.SetIconAlignment(m_control, ErrorIconAlignment.MiddleLeft);

                        m_control.Validating += new CancelEventHandler(m_control_Validating);
                        m_control.Validated += new EventHandler(m_control_Validated);
                    }
                    Graphics graphics = m_label.CreateGraphics();
                    SizeF s = m_label.Size;
                    s.Height = 0;
                    int chars;
                    int lines;
                    SizeF textSize = graphics.MeasureString(m_label.Text, m_label.Font, s, StringFormat.GenericDefault, out chars, out lines);
                    m_label.Height = (int) (textSize.Height);

                    if (lines > 1)
                        m_label.TextAlign = ContentAlignment.MiddleLeft;
                    else
                        m_label.TextAlign = ContentAlignment.TopLeft;

                    m_label.Top = 0;
                    p.Controls.Add(m_label);

                    m_control.Parent = p;
                    m_control.Location = new Point(m_label.Width + 3, 0);
                    m_control.Width = p.Width - m_label.Width - 6;
                    m_control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    p.Controls.Add(m_control);

                    p.Height = Math.Max(m_label.Height, m_control.Height) + 4;
                    p.Dock = DockStyle.Top;
                    m_form.pnlFields.Controls.Add(p);
                }
            }

            public void Focus()
            {
                if (m_control != null)
                    m_control.Focus();
            }

            public Field GetField()
            {
                return null;
            }

            public string[] Value
            {
                get 
                {
                    if (m_control == null)
                        return m_val;
                    if (m_control is TextBox)
                        return ((TextBox)m_control).Lines;
                    if (m_control is CheckBox)
                        return new string[] { ((CheckBox)m_control).Checked ? "1" : "0" };
                    if (m_control is ComboBox)
                    {
                        Option o = (Option)((ComboBox) m_control).SelectedItem;
                        if (o == null) return new string[] {};
                        return new string[] { o.Val };
                    }

                    return null;
                }
            }

            public bool Validate()
            {
                if (m_control == null)
                    return true;
                if (!m_required)
                    return true;
                
                if ((Value == null) || (Value.Length == 0))
                {
                    m_form.error.SetError(m_control, "Required");
                    return false;
                }

                return true;
            }

            private void m_control_Validating(object sender, CancelEventArgs e)
            {
                if (!Validate())
                    e.Cancel = true;
            }

            private void m_control_Validated(object sender, EventArgs e)
            {
                m_form.error.SetError(m_control, "");
            }

            private void jid_Validated(object sender, EventArgs e)
            {
                m_form.error.SetError(m_control, "");
            }
            
            private void lbl_Resize(object sender, EventArgs e)
            {
                Label lbl = (Label) sender;

                Graphics graphics = lbl.CreateGraphics();
                SizeF s = lbl.Size;
                s.Height = 0;
                SizeF textSize = graphics.MeasureString(lbl.Text, lbl.Font, s);
                lbl.Height = (int) (textSize.Height);
                if (e != null)
                {
                    lbl.Parent.Height = Math.Max(lbl.Height, m_control.Height) + 4;
                }
            }

            private void lb_VisibleChanged(object sender, EventArgs e)
            {
                // HACK: Oh.  My.  God.
                // This was found through trial and error, and I'm NOT happy with it.
                // The deal is that there is a bug in the MS implementation of ListBox, such
                // that if you call SetSelected before the underlying window has been created, 
                // the SetSelected call gets ignored.

                // So, what we do here is wait for VisibleChanged events...  this is the only event 
                // I could find that always fires after the Handle is set.  But, it also fires before
                // the handle is set, and several times so quickly in succession that the remove
                // event handler code below can happen while there is an event still in the queue.
                // Apparently that message that is already in the queue fires this callback again, 
                // even though it's been removed.
                ListBox lb = (ListBox) sender;
                if (lb.Handle == IntPtr.Zero)
                    return;

                if (lb.Items.Count > 0)
                    return;

                lb.VisibleChanged -= new EventHandler(lb_VisibleChanged);

                lb.BeginUpdate();
                foreach (Option o in m_field.GetOptions())
                {
                    int i = lb.Items.Add(o);
                    if (m_field.IsValSet(o.Val))
                        lb.SetSelected(i, true);
                }
                lb.EndUpdate();

            }

            private void jid_Validating(object sender, CancelEventArgs e)
            {
                TextBox jtxt = (TextBox) sender;
                try
                {
                    jabber.JID j = new jabber.JID(jtxt.Text);
                }
                catch
                {
                    e.Cancel = true;
                    m_form.error.SetError(jtxt, "Invalid JID");
                }
            }
        }
    }
}
