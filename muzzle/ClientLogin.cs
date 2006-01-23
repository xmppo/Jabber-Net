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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Xml;

using bedrock.util;
using jabber.connection;

namespace muzzle
{
    /// <summary>
    /// A login form for client connections.
    /// </summary>
    /// <example>
    /// ClientLogin l = new ClientLogin(jc);
    ///        
    /// if (l.ShowDialog(this) == DialogResult.OK) 
    /// {
    ///     jc.Connect();
    /// }
    /// </example>
    [RCS(@"$Header$")]
    public class ClientLogin : System.Windows.Forms.Form
    {
        private jabber.client.JabberClient m_cli = null;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox cbSSL;
        private System.Windows.Forms.ErrorProvider error;
        private System.Windows.Forms.ToolTip tip;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpBasic;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpNetwork;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNetworkHost;
        private System.Windows.Forms.TabPage tpProxy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbProxy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtProxyPort;
        private System.Windows.Forms.TextBox txtProxyUser;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.TextBox txtProxyHost;
        private System.Windows.Forms.CheckBox cbPlaintext;
        private System.ComponentModel.IContainer components;


        /// <summary>
        /// Create a Client Login dialog box
        /// </summary>
        public ClientLogin()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
#if NO_SSL
            cbSSL.Visible = false;
#endif
            for (ProxyType pt=ProxyType.None; pt <= ProxyType.HTTP_Polling; pt++)
            {
                cmbProxy.Items.Add(pt);
            }
            cmbProxy.SelectedItem = ProxyType.None;
        }

        /// <summary>
        /// Create a Client Login dialog box than manages the connection properties of a particular client
        /// connection.
        /// </summary>
        /// <param name="cli">The client connection to modify</param>
        public ClientLogin(jabber.client.JabberClient cli) : this()
        {
            m_cli = cli;
        }

        private void ReadCli()
        {
            if (m_cli == null)
                return;

#if !NO_SSL
            SSL = m_cli.SSL;
#endif
            User          = m_cli.User;
            Server        = m_cli.Server;
            Password      = m_cli.Password;
            Port          = m_cli.Port;
            NetworkHost   = m_cli.NetworkHost;
            PlaintextAuth = m_cli.PlaintextAuth;
            ProxyType     = m_cli.Proxy;
            ProxyHost     = m_cli.ProxyHost;
            ProxyPort     = m_cli.ProxyPort;
            ProxyUser     = m_cli.ProxyUsername;
            ProxyPassword = m_cli.ProxyPassword;
        }

        private void WriteCli()
        {
            if (m_cli == null)
                return;

            m_cli.User          = User;
            m_cli.Server        = Server;
            m_cli.Password      = Password;
            m_cli.Port          = Port;                
            m_cli.NetworkHost   = NetworkHost;
            m_cli.PlaintextAuth = PlaintextAuth;
            m_cli.Proxy         = ProxyType;
            m_cli.ProxyHost     = ProxyHost;
            m_cli.ProxyPort     = ProxyPort;
            m_cli.ProxyUsername = ProxyUser;
            m_cli.ProxyPassword = ProxyPassword;
#if !NO_SSL
            m_cli.SSL      = SSL;
#endif
        }

        private string Prop(XmlElement root, string elem)
        {
            XmlElement e = root[elem] as XmlElement;
            if (e == null)
                return null;
            return e.InnerText;
        }

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
#if !NO_SSL
            t = Prop(root, "ssl");
            if ((t != null) && (t != ""))
                SSL = bool.Parse(t);
#endif
            User = Prop(root, "user");
            Password = Prop(root, "password");
            Server = Prop(root, "server");
            NetworkHost = Prop(root, "host");
            ProxyHost = Prop(root, "proxyhost");
            ProxyUser = Prop(root, "proxyuser");
            ProxyPassword = Prop(root, "proxypassword");
            t = Prop(root, "proxyport");
            if ((t != null) && (t != ""))
                ProxyPort = int.Parse(t);

            t = Prop(root, "port");
            if ((t != null) && (t != ""))
                Port = int.Parse(t);
            t = Prop(root, "proxy");
            if ((t != null) && (t != ""))
                ProxyType = (ProxyType) ProxyType.Parse(typeof(ProxyType), t);
            t = Prop(root, "plaintext");
            if ((t != null) && (t != ""))
                PlaintextAuth = bool.Parse(t);

            WriteCli();
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

            root.AppendChild(doc.CreateElement("user")).InnerText = User;
            root.AppendChild(doc.CreateElement("password")).InnerText = Password;
            root.AppendChild(doc.CreateElement("server")).InnerText = Server;
            root.AppendChild(doc.CreateElement("port")).InnerText = Port.ToString();
            root.AppendChild(doc.CreateElement("host")).InnerText = NetworkHost;
            root.AppendChild(doc.CreateElement("proxy")).InnerText = ProxyType.ToString();
            root.AppendChild(doc.CreateElement("proxyhost")).InnerText = ProxyHost;
            root.AppendChild(doc.CreateElement("proxyport")).InnerText = ProxyPort.ToString();
            root.AppendChild(doc.CreateElement("proxyuser")).InnerText = ProxyUser;
            root.AppendChild(doc.CreateElement("proxypassword")).InnerText = ProxyPassword;
            root.AppendChild(doc.CreateElement("ssl")).InnerText = SSL.ToString();
            root.AppendChild(doc.CreateElement("plaintext")).InnerText = PlaintextAuth.ToString();

            XmlTextWriter xw = new XmlTextWriter(file, System.Text.Encoding.UTF8);
            xw.Formatting = Formatting.Indented;
            doc.WriteContentTo(xw);
            xw.Close();
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
            set { m_cli = value; ReadCli(); }
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

        /// <summary>
        /// The network host to connect to.  Null means use the server.  If doing HTTP polling,
        /// this is the URL to poll.
        /// </summary>
        public string NetworkHost
        {
            get { return (txtNetworkHost.Text == "") ? null : txtNetworkHost.Text; }
            set { txtNetworkHost.Text = value; }
        }

        
        /// <summary>
        /// Type of proxy
        /// </summary>
        public ProxyType ProxyType
        {
            get 
            {
                return (ProxyType)cmbProxy.SelectedItem;
            }
            set
            {
                cmbProxy.SelectedItem = value;
            }
        }

        /// <summary>
        /// Host to proxy through.
        /// </summary>
        public string ProxyHost
        {
            get { return (txtProxyHost.Text == "") ? null : txtProxyHost.Text; }
            set { txtProxyHost.Text = value; }
        }

        /// <summary>
        /// Host port to proxy through.
        /// </summary>
        public int ProxyPort
        {
            get { 
                string t = txtProxyPort.Text;
                if ((t != null) && (t != ""))
                    return int.Parse(txtProxyPort.Text); 
                return 0;
            }
            set { txtProxyPort.Text = value.ToString(); }
        }

        /// <summary>
        /// Proxy username.
        /// </summary>
        public string ProxyUser
        {
            get { return (txtProxyUser.Text == "") ? null : txtProxyUser.Text; }
            set { txtProxyUser.Text = value; }
        }

        /// <summary>
        /// Proxy password
        /// </summary>
        public string ProxyPassword
        {
            get { return (txtProxyPassword.Text == "") ? null : txtProxyPassword.Text; }
            set { txtProxyPassword.Text = value; }
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
        /// Allow Plaintext authentication?
        /// </summary>
        public bool PlaintextAuth
        {
            get { return cbPlaintext.Checked; }
            set { cbPlaintext.Checked = value; }
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
            this.error = new System.Windows.Forms.ErrorProvider();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.cbSSL = new System.Windows.Forms.CheckBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpBasic = new System.Windows.Forms.TabPage();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpNetwork = new System.Windows.Forms.TabPage();
            this.txtNetworkHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tpProxy = new System.Windows.Forms.TabPage();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.txtProxyUser = new System.Windows.Forms.TextBox();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.txtProxyHost = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbProxy = new System.Windows.Forms.ComboBox();
            this.cbPlaintext = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpBasic.SuspendLayout();
            this.tpNetwork.SuspendLayout();
            this.tpProxy.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.CausesValidation = false;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 182);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 40);
            this.panel1.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(176, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(120, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(48, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // error
            // 
            this.error.ContainerControl = this;
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(72, 72);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(152, 20);
            this.txtServer.TabIndex = 14;
            this.txtServer.Text = "";
            this.tip.SetToolTip(this.txtServer, "The name of the Jabber server");
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(72, 8);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(152, 20);
            this.txtUser.TabIndex = 10;
            this.txtUser.Text = "";
            this.tip.SetToolTip(this.txtUser, "The user portion of the JID only.");
            // 
            // cbSSL
            // 
            this.cbSSL.AccessibleDescription = "";
            this.cbSSL.Location = new System.Drawing.Point(8, 64);
            this.cbSSL.Name = "cbSSL";
            this.cbSSL.Size = new System.Drawing.Size(48, 24);
            this.cbSSL.TabIndex = 22;
            this.cbSSL.Text = "SSL";
            this.tip.SetToolTip(this.cbSSL, "Connect using Secure Socket Layer encryption");
            this.cbSSL.CheckedChanged += new System.EventHandler(this.cbSSL_CheckedChanged);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(88, 10);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(40, 20);
            this.txtPort.TabIndex = 21;
            this.txtPort.Text = "5222";
            this.tip.SetToolTip(this.txtPort, "TCP port to connect on");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpBasic);
            this.tabControl1.Controls.Add(this.tpNetwork);
            this.tabControl1.Controls.Add(this.tpProxy);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(240, 182);
            this.tabControl1.TabIndex = 10;
            // 
            // tpBasic
            // 
            this.tpBasic.Controls.Add(this.cbPlaintext);
            this.tpBasic.Controls.Add(this.txtPass);
            this.tpBasic.Controls.Add(this.txtServer);
            this.tpBasic.Controls.Add(this.txtUser);
            this.tpBasic.Controls.Add(this.label4);
            this.tpBasic.Controls.Add(this.label2);
            this.tpBasic.Controls.Add(this.label1);
            this.tpBasic.Location = new System.Drawing.Point(4, 22);
            this.tpBasic.Name = "tpBasic";
            this.tpBasic.Size = new System.Drawing.Size(232, 156);
            this.tpBasic.TabIndex = 0;
            this.tpBasic.Text = "Basic";
            // 
            // txtPass
            // 
            this.txtPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPass.Location = new System.Drawing.Point(72, 40);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(152, 20);
            this.txtPass.TabIndex = 12;
            this.txtPass.Text = "";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "Password:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "Server:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "User:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpNetwork
            // 
            this.tpNetwork.Controls.Add(this.txtNetworkHost);
            this.tpNetwork.Controls.Add(this.label5);
            this.tpNetwork.Controls.Add(this.cbSSL);
            this.tpNetwork.Controls.Add(this.txtPort);
            this.tpNetwork.Controls.Add(this.label3);
            this.tpNetwork.Location = new System.Drawing.Point(4, 22);
            this.tpNetwork.Name = "tpNetwork";
            this.tpNetwork.Size = new System.Drawing.Size(232, 156);
            this.tpNetwork.TabIndex = 2;
            this.tpNetwork.Text = "Network";
            // 
            // txtNetworkHost
            // 
            this.txtNetworkHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetworkHost.Location = new System.Drawing.Point(88, 37);
            this.txtNetworkHost.Name = "txtNetworkHost";
            this.txtNetworkHost.Size = new System.Drawing.Size(136, 20);
            this.txtNetworkHost.TabIndex = 24;
            this.txtNetworkHost.Text = "";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 23);
            this.label5.TabIndex = 23;
            this.label5.Text = "Network Host:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 20;
            this.label3.Text = "Port:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpProxy
            // 
            this.tpProxy.Controls.Add(this.txtProxyPassword);
            this.tpProxy.Controls.Add(this.txtProxyUser);
            this.tpProxy.Controls.Add(this.txtProxyPort);
            this.tpProxy.Controls.Add(this.txtProxyHost);
            this.tpProxy.Controls.Add(this.label10);
            this.tpProxy.Controls.Add(this.label9);
            this.tpProxy.Controls.Add(this.label8);
            this.tpProxy.Controls.Add(this.label7);
            this.tpProxy.Controls.Add(this.label6);
            this.tpProxy.Controls.Add(this.cmbProxy);
            this.tpProxy.Location = new System.Drawing.Point(4, 22);
            this.tpProxy.Name = "tpProxy";
            this.tpProxy.Size = new System.Drawing.Size(232, 156);
            this.tpProxy.TabIndex = 1;
            this.tpProxy.Text = "Proxy";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyPassword.Location = new System.Drawing.Point(72, 117);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.PasswordChar = '*';
            this.txtProxyPassword.Size = new System.Drawing.Size(152, 20);
            this.txtProxyPassword.TabIndex = 36;
            this.txtProxyPassword.Text = "";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyUser.Location = new System.Drawing.Point(72, 90);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(152, 20);
            this.txtProxyUser.TabIndex = 35;
            this.txtProxyUser.Text = "";
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyPort.Location = new System.Drawing.Point(72, 63);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(152, 20);
            this.txtProxyPort.TabIndex = 34;
            this.txtProxyPort.Text = "";
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyHost.Location = new System.Drawing.Point(72, 36);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(152, 20);
            this.txtProxyHost.TabIndex = 33;
            this.txtProxyHost.Text = "";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 116);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 23);
            this.label10.TabIndex = 32;
            this.label10.Text = "Password:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 89);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 23);
            this.label9.TabIndex = 31;
            this.label9.Text = "User:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 23);
            this.label8.TabIndex = 30;
            this.label8.Text = "Port:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 23);
            this.label7.TabIndex = 29;
            this.label7.Text = "Server:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 23);
            this.label6.TabIndex = 28;
            this.label6.Text = "Type:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbProxy
            // 
            this.cmbProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProxy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProxy.Location = new System.Drawing.Point(72, 8);
            this.cmbProxy.Name = "cmbProxy";
            this.cmbProxy.Size = new System.Drawing.Size(152, 20);
            this.cmbProxy.TabIndex = 27;
            // 
            // cbPlaintext
            // 
            this.cbPlaintext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPlaintext.Location = new System.Drawing.Point(8, 104);
            this.cbPlaintext.Name = "cbPlaintext";
            this.cbPlaintext.Size = new System.Drawing.Size(216, 32);
            this.cbPlaintext.TabIndex = 15;
            this.cbPlaintext.Text = "Allow plaintext authentication";
            // 
            // ClientLogin
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(240, 222);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ClientLogin";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.ClientLogin_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpBasic.ResumeLayout(false);
            this.tpNetwork.ResumeLayout(false);
            this.tpProxy.ResumeLayout(false);
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

            WriteCli();

            this.DialogResult = DialogResult.OK;
            this.Close();        
        }

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

        private void ClientLogin_Load(object sender, System.EventArgs e)
        {
            ReadCli();
            txtUser.Focus();
        }
    }
}
