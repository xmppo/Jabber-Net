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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace muzzle
{
    /// <summary>
    /// A login form for client connections.
    /// </summary>
    /// <example>
    /// ClientLogin l = new ClientLogin();
    ///        
    /// l.User = jc.User;
    /// l.Port = jc.Port;
    /// l.Server = jc.Server;
    /// l.Password = jc.Password;
    /// 
    /// if (l.ShowDialog(this) == DialogResult.OK) 
    /// {
    ///     jc.User = l.User;
    ///     jc.Port = l.Port;
    ///     jc.Server = l.Server;
    ///     jc.Password = l.Password;
    ///     jc.Connect();
    /// }
    /// </example>
    public class ClientLogin : System.Windows.Forms.Form
    {
        private jabber.client.JabberClient m_cli = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
#if !NO_SSL
        private System.Windows.Forms.CheckBox cbSSL;
        private System.Windows.Forms.ErrorProvider error;
        private System.Windows.Forms.ToolTip tip;
        private System.ComponentModel.IContainer components;
#endif


        /// <summary>
        /// Create a Client Login dialog box
        /// </summary>
        public ClientLogin()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Create a Client Login dialog box than manages the connection properties of a particular client
        /// connection.
        /// </summary>
        /// <param name="cli">The client connection to modify</param>
        public ClientLogin(jabber.client.JabberClient cli) : this()
        {
            SetAll(cli);
        }

        private void SetAll(jabber.client.JabberClient cli)
        {
            m_cli    = cli;
            User     = m_cli.User;
            Server   = m_cli.Server;
            Password = m_cli.Password;
            Port     = m_cli.Port;
#if !NO_SSL
            SSL      = m_cli.SSL;
#endif
        }

        /// <summary>
        /// The client connection to manage
        /// </summary>
        public jabber.client.JabberClient Client
        {
            get 
            {
                // If we are running in the designer, let's try to auto-hook a JabberClient
                if ((m_cli == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost) base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is jabber.client.JabberClient)
                                {
                                    m_cli = (jabber.client.JabberClient) c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_cli; 
            }
            set { SetAll(value); }
        }

        /// <summary>
        /// The user name for the connection
        /// </summary>
        public string User
        {
            get { return txtUser.Text; }
            set { txtUser.Text = value; }
        }

        /// <summary>
        /// The server name for the connection
        /// </summary>
        public string Server
        {
            get { return txtServer.Text; }
            set { txtServer.Text = value; }
        }

        /// <summary>
        /// The password for the connection
        /// </summary>
        public string Password
        {
            get { return txtPass.Text; }
            set { txtPass.Text = value; }
        }

        /// <summary>
        /// The port number for the connection
        /// </summary>
        public int Port
        {
            get { return int.Parse(txtPort.Text); }
            set { txtPort.Text = value.ToString(); }
        }

#if !NO_SSL
        /// <summary>
        /// Do SSL3/TLS1 on Connect?
        /// </summary>
        public bool SSL
        {
            get { return cbSSL.Checked; }
            set { cbSSL.Checked = value; }
        }
#endif

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbSSL = new System.Windows.Forms.CheckBox();
            this.error = new System.Windows.Forms.ErrorProvider();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "User:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Server:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port:";
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(67, 8);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(141, 20);
            this.txtUser.TabIndex = 1;
            this.txtUser.Text = "";
            this.txtUser.Validating += new System.ComponentModel.CancelEventHandler(this.Required);
            this.txtUser.Validated += new System.EventHandler(this.ClearError);
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(67, 70);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(141, 20);
            this.txtServer.TabIndex = 5;
            this.txtServer.Text = "";
            this.tip.SetToolTip(this.txtServer, "DNS name of server to connect to");
            this.txtServer.Validating += new System.ComponentModel.CancelEventHandler(this.Required);
            this.txtServer.Validated += new System.EventHandler(this.ClearError);
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(67, 101);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(141, 20);
            this.txtPort.TabIndex = 7;
            this.txtPort.Text = "5222";
            this.tip.SetToolTip(this.txtPort, "TCP port to connect on");
            this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(this.txtPort_Validating);
            this.txtPort.Validated += new System.EventHandler(this.ClearError);
            // 
            // txtPass
            // 
            this.txtPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPass.Location = new System.Drawing.Point(67, 39);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(141, 20);
            this.txtPass.TabIndex = 3;
            this.txtPass.Text = "";
            this.txtPass.Validating += new System.ComponentModel.CancelEventHandler(this.Required);
            this.txtPass.Validated += new System.EventHandler(this.ClearError);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Password:";
            // 
            // panel1
            // 
            this.panel1.CausesValidation = false;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 158);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 40);
            this.panel1.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(160, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(104, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(48, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbSSL
            // 
            this.cbSSL.AccessibleDescription = "";
            this.cbSSL.Location = new System.Drawing.Point(67, 128);
            this.cbSSL.Name = "cbSSL";
            this.cbSSL.Size = new System.Drawing.Size(101, 24);
            this.cbSSL.TabIndex = 8;
            this.cbSSL.Text = "SSL";
            this.tip.SetToolTip(this.cbSSL, "Connect using Secure Socket Layer encryption");
            this.cbSSL.Validated += new System.EventHandler(this.ClearError);
            this.cbSSL.CheckedChanged += new System.EventHandler(this.cbSSL_CheckedChanged);
            // 
            // error
            // 
            this.error.ContainerControl = this;
            // 
            // ClientLogin
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(224, 198);
            this.Controls.Add(this.cbSSL);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ClientLogin";
            this.Text = "Login";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();        
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (!this.Validate())
                return;

            if (m_cli != null)
            {
                m_cli.User     = User;
                m_cli.Server   = Server;
                m_cli.Password = Password;
                m_cli.Port     = Port;
#if !NO_SSL
                m_cli.SSL      = SSL;
#endif
            }
            this.DialogResult = DialogResult.OK;
            this.Close();        
        }

#if !NO_SSL
        private void cbSSL_CheckedChanged(object sender, System.EventArgs e)
        {
            if (cbSSL.Checked)
            {
                if (txtPort.Text == "5222")
                    txtPort.Text = "5223";
            }
            else
            {
                if (txtPort.Text == "5223")
                    txtPort.Text = "5222";
            }
        }

        private void ClearError(object sender, System.EventArgs e)
        {
            error.SetError((Control)sender, "");
        }

        private void txtPort_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string txt = txtPort.Text;
            if ((txt == null) || (txt == ""))
            {
                e.Cancel = true;
                error.SetError(txtPort, "Port is required");
                return;
            }
            foreach (char c in txt)
            {
                if (!char.IsNumber(c))
                {
                    txtPort.SelectAll();
                    e.Cancel = true;
                    error.SetError(txtPort, "Port must be a number");
                    return;
                }
            }

            try
            {
                int port = int.Parse(txtPort.Text);
                if ((port < 1) || (port > 65535))
                {
                    e.Cancel = true;
                    error.SetError(txtPort, "Port must be between 1 and 65535");
                }
            }
            catch (FormatException)
            {
                e.Cancel = true;
                error.SetError(txtPort, "Port must be a number");
            }
            catch (OverflowException)
            {
                e.Cancel = true;
                error.SetError(txtPort, "Port must be between 1 and 65535");
            }
            catch
            {
                e.Cancel = true;
                error.SetError(txtPort, "Unknown error");
            }
        }

        private void Required(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox box = (TextBox) sender;
            if ((box.Text == null) || (box.Text == ""))
            {
                e.Cancel = true;
                error.SetError(box, "Required");
            }
        }
#endif
    }
}
