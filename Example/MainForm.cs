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

using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

using bedrock.util;
using jabber;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Example
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    [SVN(@"$Id$")]
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.StatusBar sb;
        private jabber.client.JabberClient jc;
        private jabber.client.RosterManager rm;
        private jabber.client.PresenceManager pm;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpDebug;
        private muzzle.BottomScrollRichText debug;
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
        private jabber.connection.DiscoManager dm;
        private TabPage tpServices;
        private TreeView tvServices;
        private MenuItem menuItem2;
        private bedrock.util.IdleTime m_idle;
        private PropertyGrid pgServices;
        private Splitter splitter2;
        private jabber.connection.CapsManager cm;
        private MenuItem menuItem4;
        private MenuItem menuItem3;
        private MenuItem menuItem5;
        private MenuItem menuItem6;

        private bool m_err = false;

        public MainForm()
        {
            bedrock.net.AsyncSocket.UntrustedRootOK = true;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            m_idle = new bedrock.util.IdleTime(10, 5 * 60); // check every 10 secs to see if we've been away 5 mins.
//            m_idle = new bedrock.util.IdleTime(1, 5);
            m_idle.InvokeControl = jc.InvokeControl;
            m_idle.OnIdle += new bedrock.util.SpanEventHandler(m_idle_OnIdle);
            m_idle.OnUnIdle += new bedrock.util.SpanEventHandler(m_idle_OnUnIdle);
            tvServices.ImageList = roster.ImageList;
#if NET20
            tvServices.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvServices_NodeMouseDoubleClick);
            tvServices.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvServices_AfterSelect);
#else
            jc.AutoStartTLS = false;  // Mentalis stopped working with XCP 5
#endif
            cm.Version = System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString();
            cm.BaseFeatures.Add(URI.TIME);
            cm.BaseFeatures.Add(URI.VERSION);
            cm.BaseFeatures.Add(URI.LAST);
            cm.BaseFeatures.Add(URI.DISCO_INFO);

            AppDomain.CurrentDomain.UnhandledException +=new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }


        void m_idle_OnUnIdle(object sender, TimeSpan span)
        {
            jc.Presence(PresenceType.available, "Available", null, 0);
            pnlPresence.Text = "Available";
        }

        private void m_idle_OnIdle(object sender, TimeSpan span)
        {
            jc.Presence(PresenceType.available, "Auto-away", "away", 0);
            pnlPresence.Text = "Away";
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            m_idle.Enabled = false;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.sb = new System.Windows.Forms.StatusBar();
            this.pnlCon = new System.Windows.Forms.StatusBarPanel();
            this.pnlSSL = new System.Windows.Forms.StatusBarPanel();
            this.pnlPresence = new System.Windows.Forms.StatusBarPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpRoster = new System.Windows.Forms.TabPage();
            this.roster = new muzzle.RosterTree();
            this.jc = new jabber.client.JabberClient(this.components);
            this.pm = new jabber.client.PresenceManager(this.components);
            this.rm = new jabber.client.RosterManager(this.components);
            this.tpServices = new System.Windows.Forms.TabPage();
            this.pgServices = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.tvServices = new System.Windows.Forms.TreeView();
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.debug = new muzzle.BottomScrollRichText();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.txtDebugInput = new System.Windows.Forms.TextBox();
            this.mnuPresence = new System.Windows.Forms.ContextMenu();
            this.mnuAvailable = new System.Windows.Forms.MenuItem();
            this.mnuAway = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuOffline = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.dm = new jabber.connection.DiscoManager(this.components);
            this.cm = new jabber.connection.CapsManager(this.components);
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSSL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPresence)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpRoster.SuspendLayout();
            this.tpServices.SuspendLayout();
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
            this.pnlCon.Name = "pnlCon";
            this.pnlCon.Text = "Click on \"Offline\", and select a presence to log in.";
            this.pnlCon.Width = 539;
            // 
            // pnlSSL
            // 
            this.pnlSSL.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.pnlSSL.Name = "pnlSSL";
            this.pnlSSL.Width = 30;
            // 
            // pnlPresence
            // 
            this.pnlPresence.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.pnlPresence.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.pnlPresence.Name = "pnlPresence";
            this.pnlPresence.Text = "Offline";
            this.pnlPresence.Width = 47;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpRoster);
            this.tabControl1.Controls.Add(this.tpServices);
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
            this.roster.AllowDrop = true;
            this.roster.Client = this.jc;
            this.roster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roster.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.roster.ImageIndex = 1;
            this.roster.Location = new System.Drawing.Point(0, 0);
            this.roster.Name = "roster";
            this.roster.PresenceManager = this.pm;
            this.roster.RosterManager = this.rm;
            this.roster.SelectedImageIndex = 0;
            this.roster.ShowLines = false;
            this.roster.ShowRootLines = false;
            this.roster.Size = new System.Drawing.Size(624, 390);
            this.roster.Sorted = true;
            this.roster.StatusColor = System.Drawing.Color.Teal;
            this.roster.TabIndex = 0;
            this.roster.DoubleClick += new System.EventHandler(this.roster_DoubleClick);
            // 
            // jc
            // 
            this.jc.AutoReconnect = 3F;
            this.jc.AutoStartCompression = true;
            this.jc.AutoStartTLS = true;
            this.jc.InvokeControl = this;
            this.jc.LocalCertificate = null;
            this.jc.Password = null;
            this.jc.User = null;
            this.jc.OnReadText += new bedrock.TextHandler(this.jc_OnReadText);
            this.jc.OnMessage += new jabber.client.MessageHandler(this.jc_OnMessage);
            this.jc.OnConnect += new jabber.connection.StanzaStreamHandler(this.jc_OnConnect);
            this.jc.OnAuthenticate += new bedrock.ObjectHandler(this.jc_OnAuthenticate);
            this.jc.OnAuthError += new jabber.client.IQHandler(this.jc_OnAuthError);
            this.jc.OnDisconnect += new bedrock.ObjectHandler(this.jc_OnDisconnect);
            this.jc.OnStreamError += new jabber.protocol.ProtocolHandler(this.jc_OnStreamError);
            this.jc.OnError += new bedrock.ExceptionHandler(this.jc_OnError);
            this.jc.OnRegisterInfo += new jabber.client.IQHandler(this.jc_OnRegisterInfo);
            this.jc.OnRegistered += new jabber.client.IQHandler(this.jc_OnRegistered);
            this.jc.OnIQ += new jabber.client.IQHandler(this.jc_OnIQ);
            this.jc.OnWriteText += new bedrock.TextHandler(this.jc_OnWriteText);
            // 
            // pm
            // 
            this.pm.Stream = this.jc;
            // 
            // rm
            // 
            this.rm.AutoAllow = jabber.client.AutoSubscriptionHanding.AllowIfSubscribed;
            this.rm.AutoSubscribe = true;
            this.rm.Stream = this.jc;
            this.rm.OnSubscription += new jabber.client.SubscriptionHandler(this.rm_OnSubscription);
            this.rm.OnRosterEnd += new bedrock.ObjectHandler(this.rm_OnRosterEnd);
            this.rm.OnUnsubscription += new jabber.client.UnsubscriptionHandler(this.rm_OnUnsubscription);
            // 
            // tpServices
            // 
            this.tpServices.Controls.Add(this.pgServices);
            this.tpServices.Controls.Add(this.splitter2);
            this.tpServices.Controls.Add(this.tvServices);
            this.tpServices.Location = new System.Drawing.Point(4, 22);
            this.tpServices.Name = "tpServices";
            this.tpServices.Size = new System.Drawing.Size(624, 390);
            this.tpServices.TabIndex = 2;
            this.tpServices.Text = "Services";
            // 
            // pgServices
            // 
            this.pgServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgServices.Location = new System.Drawing.Point(350, 0);
            this.pgServices.Name = "pgServices";
            this.pgServices.Size = new System.Drawing.Size(274, 390);
            this.pgServices.TabIndex = 2;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(347, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 390);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // tvServices
            // 
            this.tvServices.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvServices.Location = new System.Drawing.Point(0, 0);
            this.tvServices.Name = "tvServices";
            this.tvServices.ShowLines = false;
            this.tvServices.ShowPlusMinus = false;
            this.tvServices.ShowRootLines = false;
            this.tvServices.Size = new System.Drawing.Size(347, 390);
            this.tvServices.TabIndex = 0;
            this.tvServices.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvServices_AfterCollapse);
            this.tvServices.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvServices_AfterExpand);
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.Add(this.debug);
            this.tpDebug.Controls.Add(this.splitter1);
            this.tpDebug.Controls.Add(this.txtDebugInput);
            this.tpDebug.Location = new System.Drawing.Point(4, 22);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Size = new System.Drawing.Size(624, 390);
            this.tpDebug.TabIndex = 0;
            this.tpDebug.Text = "Debug";
            // 
            // debug
            // 
            this.debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debug.Location = new System.Drawing.Point(0, 0);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(624, 339);
            this.debug.TabIndex = 2;
            this.debug.Text = "";
            this.debug.WordWrap = false;
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
            // txtDebugInput
            // 
            this.txtDebugInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDebugInput.Location = new System.Drawing.Point(0, 342);
            this.txtDebugInput.Multiline = true;
            this.txtDebugInput.Name = "txtDebugInput";
            this.txtDebugInput.Size = new System.Drawing.Size(624, 48);
            this.txtDebugInput.TabIndex = 4;
            this.txtDebugInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDebugInput_KeyUp);
            // 
            // mnuPresence
            // 
            this.mnuPresence.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAvailable,
            this.mnuAway,
            this.menuItem4,
            this.menuItem3,
            this.menuItem5,
            this.menuItem6,
            this.menuItem1,
            this.mnuOffline,
            this.menuItem2});
            // 
            // mnuAvailable
            // 
            this.mnuAvailable.Index = 0;
            this.mnuAvailable.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuAvailable.Text = "&Available";
            this.mnuAvailable.Click += new System.EventHandler(this.mnuAvailable_Click);
            // 
            // mnuAway
            // 
            this.mnuAway.Index = 1;
            this.mnuAway.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mnuAway.Text = "A&way";
            this.mnuAway.Click += new System.EventHandler(this.mnuAway_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.Ins;
            this.menuItem3.Text = "Add &Contact";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.menuItem5.Text = "&Remove Contact";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 6;
            this.menuItem1.Text = "-";
            // 
            // mnuOffline
            // 
            this.mnuOffline.Index = 7;
            this.mnuOffline.Shortcut = System.Windows.Forms.Shortcut.F9;
            this.mnuOffline.Text = "&Offline";
            this.mnuOffline.Click += new System.EventHandler(this.mnuOffline_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 8;
            this.menuItem2.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
            this.menuItem2.Text = "E&xit";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // dm
            // 
            this.dm.Stream = this.jc;
            // 
            // cm
            // 
            this.cm.Node = "http://cursive.net/clients/csharp-example";
            this.cm.Stream = this.jc;
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 5;
            this.menuItem6.Text = "Add &Group...";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
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
            this.tpServices.ResumeLayout(false);
            this.tpDebug.ResumeLayout(false);
            this.tpDebug.PerformLayout();
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
            // keepalive
            if (txt == " ")
                return;

            Debug.WriteLine("RECV: " + txt);
            debug.SelectionColor = Color.Red;
            debug.AppendText("RECV: ");
            debug.SelectionColor = Color.Black;
            debug.AppendText(txt);
            debug.AppendMaybeScroll("\r\n");
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
            debug.AppendMaybeScroll("\r\n");
        }

        private void jc_OnAuthenticate(object sender)
        {
            pnlPresence.Text = "Available";
            pnlCon.Text = "Connected";
#if !NO_SSL
            if (jc.SSLon)
            {

                pnlSSL.Text = "SSL";
#if NET20
                System.Security.Cryptography.X509Certificates.X509Certificate cert2 =
                    (System.Security.Cryptography.X509Certificates.X509Certificate)
                    jc[jabber.connection.Options.REMOTE_CERTIFICATE];

                string cert_str = cert2.ToString(true);
                debug.AppendText("\r\nServer Certificate:\r\n-------------------\r\n");
                debug.AppendText(cert_str + "\r\n");
                pnlSSL.ToolTipText = cert_str;
#endif
            }
#endif
#if NET20
            jabber.connection.DiscoNode dn = jabber.connection.DiscoNode.GetNode(jc.Server, null);
            tvServices.ShowNodeToolTips = true;
            TreeNode tn = tvServices.Nodes.Add(dn.Key, dn.Name);
            tn.ToolTipText = dn.Key.Replace('\u0000', '\n');
            tn.Tag = dn;
            tn.ImageIndex = 8;
            tn.SelectedImageIndex = 8;
            dm.BeginGetFeatures(dn, new jabber.connection.DiscoNodeHandler(GotInitialFeatures));
#endif
            m_idle.Enabled = true;
        }

        private void GotItems(jabber.connection.DiscoNode node)
        {
#if NET20
            TreeNode[] nodes = tvServices.Nodes.Find(node.Key, true);
            foreach (TreeNode n in nodes)
            {
                n.ImageIndex = 7;
                n.SelectedImageIndex = 7;
                foreach (jabber.connection.DiscoNode dn in node.Children)
                {
                    TreeNode tn = n.Nodes.Add(dn.Key, dn.Name);
                    tn.ToolTipText = dn.Key.Replace('\u0000', '\n');
                    tn.Tag = dn;
                    tn.ImageIndex = 8;
                    tn.SelectedImageIndex = 8;
                }
            }
            pgServices.Refresh();
#endif
        }

        private void GotInitialFeatures(jabber.connection.DiscoNode node)
        {
            dm.BeginGetItems(node, new jabber.connection.DiscoNodeHandler(GotItems));
        }

        private void GotInfo(jabber.connection.DiscoNode node)
        {
#if NET20
            pgServices.SelectedObject = node;
#endif
        }
        private void jc_OnDisconnect(object sender)
        {
            m_idle.Enabled = false;
            pnlPresence.Text = "Offline";
            pnlSSL.Text = "";
            pnlSSL.ToolTipText = "";

            tvServices.Nodes.Clear();
            if (!m_err)
                pnlCon.Text = "Disconnected";
            pgServices.SelectedObject = null;
        }

        private void jc_OnError(object sender, System.Exception ex)
        {
            m_idle.Enabled = false;

#if !NO_SSL && !NET20
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

            XmlElement query = iq.Query;
            if (query == null)
                return;

            // <iq id="jcl_8" to="me" from="you" type="get"><query xmlns="jabber:iq:version"/></iq>
            if (query is jabber.protocol.iq.Version)
            {
                iq = iq.GetResponse(jc.Document);
                jabber.protocol.iq.Version ver = iq.Query as jabber.protocol.iq.Version;
                ver.OS = Environment.OSVersion.ToString();
                ver.EntityName = Application.ProductName;
                ver.Ver = Application.ProductVersion;
                jc.Write(iq);
                return;
            }
            
            if (query is jabber.protocol.iq.Time)
            {
                iq = iq.GetResponse(jc.Document);
                jabber.protocol.iq.Time tim = iq.Query as jabber.protocol.iq.Time;
                tim.SetCurrentTime();
                jc.Write(iq);
                return;
            }
            
            if (query is jabber.protocol.iq.Last)
            {
                iq = iq.GetResponse(jc.Document);
                jabber.protocol.iq.Last last = iq.Query as jabber.protocol.iq.Last;
                last.Seconds = (int)bedrock.util.IdleTime.GetIdleTime();
                jc.Write(iq);
                return;
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

        void jc_OnConnect(object sender, jabber.connection.StanzaStream stream)
        {
            m_err = false;
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
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(txtDebugInput.Text);
                    XmlElement elem = doc.DocumentElement;
                    if (elem != null)
                        jc.Write(elem);
                    txtDebugInput.Clear();
                }
                catch (XmlException ex)
                {
                    MessageBox.Show("Invalid XML: " + ex.Message);
                }
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

        private void tvServices_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 6;
            e.Node.SelectedImageIndex = 6;
        }

        private void tvServices_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 7;
            e.Node.SelectedImageIndex = 7;
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            jc.Close();
            this.Close();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            AddContact ac = new AddContact();
            ac.AllGroups = roster.Groups;
            if (ac.ShowDialog() != DialogResult.OK)
                return;

            jc.Subscribe(ac.JID, ac.Nickname, ac.SelectedGroups);
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            muzzle.RosterTree.ItemNode n = roster.SelectedNode as muzzle.RosterTree.ItemNode;
            if (n == null)
                return;
            jc.RemoveRosterItem(n.JID);
       }


        // add group
        private void menuItem6_Click(object sender, EventArgs e)
        {
            AddGroup ag = new AddGroup();
            if (ag.ShowDialog() == DialogResult.Cancel)
                return;

            if (ag.GroupName == "")
                return;

            roster.AddGroup(ag.GroupName).EnsureVisible();
        }

        private void rm_OnSubscription(jabber.client.RosterManager manager, Item ri, Presence pres)
        {
            DialogResult res = MessageBox.Show("Allow incoming presence subscription request from: " + pres.From,
                "Subscription Request",
                MessageBoxButtons.YesNoCancel);
            switch (res)
            {
            case DialogResult.Yes:
                manager.ReplyAllow(pres);
                break;
            case DialogResult.No:
                manager.ReplyDeny(pres);
                break;
            case DialogResult.Cancel:
                // do nothing;
                break;
            }
        }

#if NET20
        void tvServices_NodeMouseDoubleClick(object sender,
                                             TreeNodeMouseClickEventArgs e)
        {
            jabber.connection.DiscoNode dn = (jabber.connection.DiscoNode)e.Node.Tag;
            if (dn.Children == null)
                dm.BeginGetItems(dn.JID, dn.Node, new jabber.connection.DiscoNodeHandler(GotItems));
        }

        private void tvServices_AfterSelect(object sender, TreeViewEventArgs e)
        {
            jabber.connection.DiscoNode dn = (jabber.connection.DiscoNode)e.Node.Tag;
            dm.BeginGetFeatures(dn, new jabber.connection.DiscoNodeHandler(GotInfo));
        }

        private void rm_OnUnsubscription(jabber.client.RosterManager manager, Presence pres, ref bool remove)
        {
            MessageBox.Show(pres.From + " has removed you from their roster.", "Unsubscription notification", MessageBoxButtons.OK);
        }

#endif
    }
}
