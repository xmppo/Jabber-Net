/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
using System.Windows.Forms;
using System.Xml;

namespace muzzle
{
    /// <summary>
    /// A login form for client connections.
    /// </summary>
    /// <example>
    /// ComponentLogin l = new ComponentLogin(jc);
    ///        
    /// if (l.ShowDialog(this) == DialogResult.OK) 
    /// {
    ///     jc.Connect();
    /// }
    /// </example>
    public class ComponentLogin : System.Windows.Forms.Form
    {
        private jabber.server.JabberService m_service = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.CheckBox chkListen;
        private System.Windows.Forms.ErrorProvider error;
        private System.Windows.Forms.ToolTip tip;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Create a Client Login dialog box
        /// </summary>
        public ComponentLogin()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Create a Client Login dialog box that manages a component
        /// </summary>
        /// <param name="service">The component to manage</param>
        public ComponentLogin(jabber.server.JabberService service) : this()
        {
            m_service = service;
        }

        /// <summary>
        /// The component being managed by this dialog.
        /// </summary>
        public jabber.server.JabberService Component
        {
            get 
            {
                // If we are running in the designer, let's try to auto-hook a JabberClient
                if ((m_service == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost) base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is jabber.server.JabberService)
                                {
                                    m_service = (jabber.server.JabberService) c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_service; 
            }
            set 
            { 
                m_service = value; 
                ReadService(); 
            }
        }

        /// <summary>
        /// The service ID of the component
        /// </summary>
        public string ComponentID
        {
            get { return txtUser.Text; }
            set { txtUser.Text = value; }
        }

        /// <summary>
        /// The server name for the connection
        /// </summary>
        public string Host
        {
            get { return txtServer.Text; }
            set { txtServer.Text = value; }
        }

        /// <summary>
        /// The password for the connection
        /// </summary>
        public string Secret
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

        /// <summary>
        /// Listen for inbound connections from the router?
        /// </summary>
        public bool Listen
        {
            get { return chkListen.Checked; }
            set { chkListen.Checked = value; }
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkListen = new System.Windows.Forms.CheckBox();
            this.error = new System.Windows.Forms.ErrorProvider();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "ID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Port:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(88, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.CausesValidation = false;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(140, 156);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(56, 64);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(120, 20);
            this.txtUser.TabIndex = 2;
            this.txtUser.Text = "";
            this.tip.SetToolTip(this.txtUser, "Service ID for this component");
            this.txtUser.Validating += new System.ComponentModel.CancelEventHandler(this.Required);
            this.txtUser.Validated += new System.EventHandler(this.ClearError);
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(56, 8);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(120, 20);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "";
            this.tip.SetToolTip(this.txtServer, "DNS name or IP address of router to connect to.  Not required if in Listen mode.");
            this.txtServer.Validating += new System.ComponentModel.CancelEventHandler(this.Required);
            this.txtServer.Validated += new System.EventHandler(this.ClearError);
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(56, 36);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(120, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "";
            this.tip.SetToolTip(this.txtPort, "TCP port to connect to, or port to listen on if in Listen mode.");
            this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(this.txtPort_Validating);
            this.txtPort.Validated += new System.EventHandler(this.ClearError);
            // 
            // txtPass
            // 
            this.txtPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPass.Location = new System.Drawing.Point(56, 92);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(120, 20);
            this.txtPass.TabIndex = 3;
            this.txtPass.Text = "";
            this.tip.SetToolTip(this.txtPass, "Secret shared with router");
            this.txtPass.Validating += new System.ComponentModel.CancelEventHandler(this.Required);
            this.txtPass.Validated += new System.EventHandler(this.ClearError);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 23);
            this.label4.TabIndex = 8;
            this.label4.Text = "Secret:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkListen
            // 
            this.chkListen.Location = new System.Drawing.Point(56, 120);
            this.chkListen.Name = "chkListen";
            this.chkListen.Size = new System.Drawing.Size(56, 24);
            this.chkListen.TabIndex = 9;
            this.chkListen.Text = "Listen";
            this.tip.SetToolTip(this.chkListen, "Open a listen socket, and wait for the router to contact us?");
            // 
            // error
            // 
            this.error.ContainerControl = this;
            // 
            // ComponentLogin
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(192, 190);
            this.Controls.Add(this.chkListen);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ComponentLogin";
            this.Text = "Connection";
            this.Load += new System.EventHandler(this.ComponentLogin_Load);
            this.ResumeLayout(false);

        }
#endregion

        /// <summary>
        /// Set the connection properties from an XML config file.
        /// TODO: Replace this with a better ConfigFile implementation that can write.
        /// </summary>
        /// <param name="file"></param>
        public void ReadFromFile(string file)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(file);
            }
            catch (XmlException)
            {
                return;
            }
            catch (System.IO.FileNotFoundException)
            {
                return;
            }

            XmlElement root = doc.DocumentElement;
            if (root == null)
                return;

            string t;
            ComponentID = Prop(root, "ComponentID");
            Host = Prop(root, "Host");
            Secret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Prop(root, "Secret")));
            t = Prop(root, "Port");
            if ((t != null) && (t != ""))
                Port = int.Parse(t);

            WriteService();
        }

        private string Prop(XmlElement root, string elem)
        {
            XmlElement e = root[elem] as XmlElement;
            if (e == null)
                return null;
            return e.InnerText;
        }

        /// <summary>
        /// Write the current connection properties to an XML config file.
        /// TODO: Replace this with a better ConfigFile implementation that can write.
        /// </summary>
        /// <param name="file"></param>
        public void WriteToFile(string file)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = (XmlElement) doc.CreateElement("login");
            doc.AppendChild(root);

            root.AppendChild(doc.CreateElement("ComponentID")).InnerText = ComponentID;
            root.AppendChild(doc.CreateElement("Host")).InnerText = Host;
            root.AppendChild(doc.CreateElement("Secret")).InnerText = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Secret));
            root.AppendChild(doc.CreateElement("Port")).InnerText = Port.ToString();

            XmlTextWriter xw = new XmlTextWriter(file, System.Text.Encoding.UTF8);
            xw.Formatting = Formatting.Indented;
            doc.WriteContentTo(xw);
            xw.Close();
        }

        private void ReadService()
        {
            if (m_service == null)
                return;

            ComponentID = m_service.ComponentID;
            Host        = m_service.NetworkHost;
            Secret      = m_service.Secret;
            Port        = m_service.Port;
        }

        private void WriteService()
        {
            if (m_service == null)
                return;

            m_service.ComponentID = ComponentID;
            m_service.NetworkHost = Host;
            m_service.Secret      = Secret;
            m_service.Port        = Port;
            m_service.Type        = Listen ? 
                jabber.server.ComponentType.Connect : jabber.server.ComponentType.Accept;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (!this.Validate())
                return;

            WriteService();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ClearError(object sender, System.EventArgs e)
        {
            error.SetError((Control)sender, "");
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

        private void ComponentLogin_Load(object sender, System.EventArgs e)
        {
            ReadService();
            txtServer.Focus();
        }
    }
}
