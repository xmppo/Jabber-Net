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

using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using jabber;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Example
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private bedrock.net.BaseSocket m_sock;

        private System.Windows.Forms.StatusBar sb;
        private jabber.client.JabberClient jc;
        private jabber.client.RosterManager rm;
        private jabber.client.PresenceManager pm;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpDebug;
        private System.Windows.Forms.RichTextBox debug;
        private System.Windows.Forms.TabPage tpRoster;
        private System.Windows.Forms.StatusBarPanel pnlCon;
        private System.Windows.Forms.StatusBarPanel pnlPresence;
        private System.Windows.Forms.ContextMenu mnuPresence;
        private System.Windows.Forms.MenuItem mnuAvailable;
        private System.Windows.Forms.MenuItem mnuAway;
        private System.Windows.Forms.MenuItem mnuOffline;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox txtDebugInput;
        private muzzle.RosterTree roster;
        private System.Windows.Forms.StatusBarPanel pnlSSL;

        private bool m_err = false;

        public MainForm()
        {
            bedrock.net.AsyncSocket.UntrustedRootOK = true;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //jc.AutoStartTLS = false;
            AppDomain.CurrentDomain.UnhandledException +=new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.sb = new System.Windows.Forms.StatusBar();
            this.pnlCon = new System.Windows.Forms.StatusBarPanel();
            this.pnlSSL = new System.Windows.Forms.StatusBarPanel();
            this.pnlPresence = new System.Windows.Forms.StatusBarPanel();
            this.jc = new jabber.client.JabberClient(this.components);
            this.rm = new jabber.client.RosterManager(this.components);
            this.pm = new jabber.client.PresenceManager(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpRoster = new System.Windows.Forms.TabPage();
            this.roster = new muzzle.RosterTree();
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.debug = new System.Windows.Forms.RichTextBox();
            this.txtDebugInput = new System.Windows.Forms.TextBox();
            this.mnuPresence = new System.Windows.Forms.ContextMenu();
            this.mnuAvailable = new System.Windows.Forms.MenuItem();
            this.mnuAway = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuOffline = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSSL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPresence)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpRoster.SuspendLayout();
            this.tpDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // sb
            // 
            this.sb.Location = new System.Drawing.Point(0, 416);
            this.sb.Name = "sb";
            this.sb.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                  this.pnlCon,
                                                                                  this.pnlSSL,
                                                                                  this.pnlPresence});
            this.sb.ShowPanels = true;
            this.sb.Size = new System.Drawing.Size(632, 22);
            this.sb.TabIndex = 0;
            this.sb.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.sb_PanelClick);
            // 
            // pnlCon
            // 
            this.pnlCon.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.pnlCon.Text = "Click on \"Offline\", and select a presence to log in.";
            this.pnlCon.Width = 539;
            // 
            // pnlSSL
            // 
            this.pnlSSL.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.pnlSSL.Width = 30;
            // 
            // pnlPresence
            // 
            this.pnlPresence.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.pnlPresence.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.pnlPresence.Text = "Offline";
            this.pnlPresence.Width = 47;
            // 
            // jc
            // 
            this.jc.AutoReconnect = 3F;
            this.jc.InvokeControl = this;
            this.jc.LocalCertificate = null;
            this.jc.Password = null;
            this.jc.User = null;
            this.jc.OnAuthError += new jabber.client.IQHandler(this.jc_OnAuthError);
            this.jc.OnReadText += new bedrock.TextHandler(this.jc_OnReadText);
            this.jc.OnRegisterInfo += new jabber.client.IQHandler(this.jc_OnRegisterInfo);
            this.jc.OnWriteText += new bedrock.TextHandler(this.jc_OnWriteText);
            this.jc.OnAuthenticate += new bedrock.ObjectHandler(this.jc_OnAuthenticate);
            this.jc.OnMessage += new jabber.client.MessageHandler(this.jc_OnMessage);
            this.jc.OnIQ += new jabber.client.IQHandler(this.jc_OnIQ);
            this.jc.OnDisconnect += new bedrock.ObjectHandler(this.jc_OnDisconnect);
            this.jc.OnRegistered += new jabber.client.IQHandler(this.jc_OnRegistered);
            this.jc.OnStreamError += new jabber.protocol.ProtocolHandler(this.jc_OnStreamError);
            this.jc.OnError += new bedrock.ExceptionHandler(this.jc_OnError);
            this.jc.OnConnect += new bedrock.net.AsyncSocketHandler(this.jc_OnConnect);
            // 
            // rm
            // 
            this.rm.Client = this.jc;
            this.rm.OnRosterEnd += new bedrock.ObjectHandler(this.rm_OnRosterEnd);
            // 
            // pm
            // 
            this.pm.Client = this.jc;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpRoster);
            this.tabControl1.Controls.Add(this.tpDebug);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 416);
            this.tabControl1.TabIndex = 2;
            // 
            // tpRoster
            // 
            this.tpRoster.Controls.Add(this.roster);
            this.tpRoster.Location = new System.Drawing.Point(4, 22);
            this.tpRoster.Name = "tpRoster";
            this.tpRoster.Size = new System.Drawing.Size(624, 390);
            this.tpRoster.TabIndex = 1;
            this.tpRoster.Text = "Roster";
            // 
            // roster
            // 
            this.roster.Client = this.jc;
            this.roster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roster.ImageIndex = 1;
            this.roster.Location = new System.Drawing.Point(0, 0);
            this.roster.Name = "roster";
            this.roster.PresenceManager = this.pm;
            this.roster.RosterManager = this.rm;
            this.roster.ShowLines = false;
            this.roster.ShowRootLines = false;
            this.roster.Size = new System.Drawing.Size(624, 390);
            this.roster.Sorted = true;
            this.roster.TabIndex = 0;
            this.roster.DoubleClick += new System.EventHandler(this.roster_DoubleClick);
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.Add(this.splitter1);
            this.tpDebug.Controls.Add(this.debug);
            this.tpDebug.Controls.Add(this.txtDebugInput);
            this.tpDebug.Location = new System.Drawing.Point(4, 22);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Size = new System.Drawing.Size(624, 390);
            this.tpDebug.TabIndex = 0;
            this.tpDebug.Text = "Debug";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 339);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(624, 3);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // debug
            // 
            this.debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debug.Location = new System.Drawing.Point(0, 0);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(624, 342);
            this.debug.TabIndex = 2;
            this.debug.Text = "";
            this.debug.WordWrap = false;
            // 
            // txtDebugInput
            // 
            this.txtDebugInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDebugInput.Location = new System.Drawing.Point(0, 342);
            this.txtDebugInput.Multiline = true;
            this.txtDebugInput.Name = "txtDebugInput";
            this.txtDebugInput.Size = new System.Drawing.Size(624, 48);
            this.txtDebugInput.TabIndex = 4;
            this.txtDebugInput.Text = "";
            this.txtDebugInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDebugInput_KeyUp);
            // 
            // mnuPresence
            // 
            this.mnuPresence.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                        this.mnuAvailable,
                                                                                        this.mnuAway,
                                                                                        this.menuItem1,
                                                                                        this.mnuOffline});
            // 
            // mnuAvailable
            // 
            this.mnuAvailable.Index = 0;
            this.mnuAvailable.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuAvailable.Text = "Available";
            this.mnuAvailable.Click += new System.EventHandler(this.mnuAvailable_Click);
            // 
            // mnuAway
            // 
            this.mnuAway.Index = 1;
            this.mnuAway.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mnuAway.Text = "Away";
            this.mnuAway.Click += new System.EventHandler(this.mnuAway_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "-";
            // 
            // mnuOffline
            // 
            this.mnuOffline.Index = 3;
            this.mnuOffline.Text = "Offline";
            this.mnuOffline.Click += new System.EventHandler(this.mnuOffline_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 438);
            this.ContextMenu = this.mnuPresence;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.sb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.pnlCon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSSL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPresence)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpRoster.ResumeLayout(false);
            this.tpDebug.ResumeLayout(false);
            this.ResumeLayout(false);

        }
                #endregion

        /// <summary>
        /// The MainForm entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new MainForm());
        }

        private void Connect()
        {
            muzzle.ClientLogin log = new muzzle.ClientLogin(jc);
            log.ReadFromFile("login.xml");
            if (log.ShowDialog() == DialogResult.OK)
            {
                log.WriteToFile("login.xml");
                jc.Connect();
            }
        }

        private void jc_OnReadText(object sender, string txt)
        {
            Debug.WriteLine("RECV: " + txt);
            debug.SelectionColor = Color.Red;
            debug.AppendText("RECV: ");
            debug.SelectionColor = Color.Black;
            debug.AppendText(txt);
            debug.AppendText("\r\n");        
        }

        private void jc_OnWriteText(object sender, string txt)
        {
            // keepalive
            if (txt == " ")
                return;

            Debug.WriteLine("SEND: " + txt);
            debug.SelectionColor = Color.Blue;
            debug.AppendText("SEND: ");
            debug.SelectionColor = Color.Black;
            debug.AppendText(txt);
            debug.AppendText("\r\n");
        }


        private void jc_OnAuthenticate(object sender)
        {
            pnlPresence.Text = "Available";
            pnlCon.Text = "Connected";
#if !NO_SSL
            if (jc.SSLon)
            {

                pnlSSL.Text = "SSL";
                string cert = ((bedrock.net.AsyncSocket)m_sock).RemoteCertificate.ToString(true);
                debug.AppendText("\r\nServer Certificate:\r\n-------------------\r\n");
                debug.AppendText(cert + "\r\n");
                pnlSSL.ToolTipText = cert;
            }
#endif
        }

        private void jc_OnDisconnect(object sender)
        {
            pnlPresence.Text = "Offline";
            pnlSSL.Text = "";
            pnlSSL.ToolTipText = "";

            if (!m_err)
                pnlCon.Text = "Disconnected";
        }

        private void jc_OnError(object sender, System.Exception ex)
        {
#if !NO_SSL
            if (ex is Org.Mentalis.Security.Certificates.CertificateException)
                m_err = true;
#endif

            pnlCon.Text = "Error: " + ex.Message;
            debug.SelectionColor = Color.Green;
            debug.AppendText("ERROR: ");
            debug.SelectionColor = Color.Black;
            debug.AppendText(ex.ToString());
            debug.AppendText("\r\n");
        }

        private void jc_OnAuthError(object sender, jabber.protocol.client.IQ iq)
        {
            if (MessageBox.Show(this, 
                "Create new account?", 
                "Authentication error", 
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                jc.Register(new JID(jc.User, jc.Server, null));
            }
            else
            {
                jc.Close();
                Connect();
            }
        }

        private void jc_OnRegistered(object sender, jabber.protocol.client.IQ iq)
        {
            if (iq.Type == IQType.result)
                jc.Login();
            else
                pnlCon.Text = "Registration error";
        }

        private void jc_OnRegisterInfo(object sender, jabber.protocol.client.IQ iq)
        {
            Register r = iq.Query as Register;
            Debug.Assert(r != null);
            r.Password = jc.Password;
        }

        private void jc_OnMessage(object sender, jabber.protocol.client.Message msg)
        {
            jabber.protocol.x.Data x = msg["x", URI.XDATA] as jabber.protocol.x.Data;
            if (x != null)
            {
                muzzle.XDataForm f = new muzzle.XDataForm(msg);
                f.ShowDialog(this);
                jc.Write(f.GetResponse());
            }
            else
                MessageBox.Show(this, msg.Body, msg.From, MessageBoxButtons.OK);
        }

        private void jc_OnIQ(object sender, jabber.protocol.client.IQ iq)
        {
            if (iq.Type != IQType.get)
                return;

            // <iq id="jcl_8" to="me" from="you" type="get"><query xmlns="jabber:iq:version"/></iq>
            jabber.protocol.iq.Version ver = iq.Query as jabber.protocol.iq.Version;
            if (ver != null)
            {
                iq.Swap();
                iq.Type = IQType.result;
                ver.OS = Environment.OSVersion.ToString();
                ver.EntityName = Application.ProductName;
                ver.Ver = Application.ProductVersion;
                jc.Write(iq);
            }
            else
            {
                iq.Swap();
                iq.Type = IQType.error;
                iq.Error.Code = ErrorCode.NOT_IMPLEMENTED;
                jc.Write(iq);
            }
        }

        private void roster_DoubleClick(object sender, System.EventArgs e)
        {
            muzzle.RosterTree.ItemNode n = roster.SelectedNode as muzzle.RosterTree.ItemNode;
            if (n == null)
                return;
            new SendMessage(jc, n.JID).Show();
        }

        private void sb_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
        {
            if (e.StatusBarPanel != pnlPresence)
                return;
            mnuPresence.Show(sb, new Point(e.X, e.Y));
        }

        private void mnuAvailable_Click(object sender, System.EventArgs e)
        {
            if (jc.IsAuthenticated)
            {
                jc.Presence(PresenceType.available, "Available", null, 0);
                pnlPresence.Text = "Available";
            }
            else
                Connect();
        }

        private void mnuAway_Click(object sender, System.EventArgs e)
        {
            if (jc.IsAuthenticated)
            {
                jc.Presence(PresenceType.available, "Away", "away", 0);
                pnlPresence.Text = "Away";
            }        
            else
                Connect();
        }

        private void mnuOffline_Click(object sender, System.EventArgs e)
        {
            if (jc.IsAuthenticated)
                jc.Close();
        }

        private void jc_OnConnect(object sender, bedrock.net.BaseSocket sock)
        {
            if (sock == null)
                return;

            m_sock = sock;
            m_err = false;
            debug.AppendText("Connected to: " + sock.ToString() + "\r\n");
        }

        private void jc_OnStreamError(object sender, System.Xml.XmlElement rp)
        {
            m_err = true;
            pnlCon.Text = "Stream error: " + rp.InnerText;
        }

        private void txtDebugInput_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && e.Control)
            {
                jc.Write(txtDebugInput.Text);
                txtDebugInput.Clear();
            }
        
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "Unhandled exception: " + e.GetType().ToString());
        }

        private void rm_OnRosterEnd(object sender)
        {
            roster.ExpandAll();
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (jc.IsAuthenticated)
                jc.Close();
        }
    }
}
