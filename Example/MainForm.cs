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
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

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
        private System.Windows.Forms.StatusBar sb;
        private jabber.client.JabberClient jc;
        private jabber.client.RosterManager rm;
        private jabber.client.PresenceManager pm;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpDebug;
        private System.Windows.Forms.RichTextBox debug;
        private System.Windows.Forms.TabPage tpRoster;
        private System.Windows.Forms.TreeView roster;
        private System.Windows.Forms.ImageList ilPresence;
        private System.Windows.Forms.StatusBarPanel pnlCon;
        private System.Windows.Forms.StatusBarPanel pnlPresence;
        private System.Windows.Forms.ContextMenu mnuPresence;
        private System.Windows.Forms.MenuItem mnuAvailable;
        private System.Windows.Forms.MenuItem mnuAway;
        private System.Windows.Forms.MenuItem mnuOffline;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.ComponentModel.IContainer components;

        private bool m_err = false;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            this.pnlPresence = new System.Windows.Forms.StatusBarPanel();
            this.jc = new jabber.client.JabberClient(this.components);
            this.rm = new jabber.client.RosterManager(this.components);
            this.pm = new jabber.client.PresenceManager(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpRoster = new System.Windows.Forms.TabPage();
            this.roster = new System.Windows.Forms.TreeView();
            this.ilPresence = new System.Windows.Forms.ImageList(this.components);
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.debug = new System.Windows.Forms.RichTextBox();
            this.mnuPresence = new System.Windows.Forms.ContextMenu();
            this.mnuAvailable = new System.Windows.Forms.MenuItem();
            this.mnuAway = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuOffline = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPresence)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpRoster.SuspendLayout();
            this.tpDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // sb
            // 
            this.sb.Location = new System.Drawing.Point(0, 244);
            this.sb.Name = "sb";
            this.sb.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                  this.pnlCon,
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
            this.pnlCon.Width = 569;
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
            this.jc.OnPresence += new jabber.client.PresenceHandler(this.jc_OnPresence);
            this.jc.OnRegistered += new jabber.client.IQHandler(this.jc_OnRegistered);
            this.jc.OnStreamError += new jabber.protocol.ProtocolHandler(this.jc_OnStreamError);
            this.jc.OnError += new bedrock.ExceptionHandler(this.jc_OnError);
            this.jc.OnConnect += new bedrock.net.AsyncSocketHandler(this.jc_OnConnect);
            // 
            // rm
            // 
            this.rm.Client = this.jc;
            this.rm.OnRosterBegin += new bedrock.ObjectHandler(this.rm_OnRosterBegin);
            this.rm.OnRosterEnd += new bedrock.ObjectHandler(this.rm_OnRosterEnd);
            this.rm.OnRosterItem += new jabber.client.RosterItemHandler(this.rm_OnRosterItem);
            // 
            // pm
            // 
            this.pm.Client = this.jc;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                      this.tpRoster,
                                                                                      this.tpDebug});
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 244);
            this.tabControl1.TabIndex = 2;
            // 
            // tpRoster
            // 
            this.tpRoster.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                   this.roster});
            this.tpRoster.Location = new System.Drawing.Point(4, 22);
            this.tpRoster.Name = "tpRoster";
            this.tpRoster.Size = new System.Drawing.Size(624, 218);
            this.tpRoster.TabIndex = 1;
            this.tpRoster.Text = "Roster";
            // 
            // roster
            // 
            this.roster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roster.ImageIndex = 1;
            this.roster.ImageList = this.ilPresence;
            this.roster.Name = "roster";
            this.roster.ShowRootLines = false;
            this.roster.Size = new System.Drawing.Size(624, 218);
            this.roster.TabIndex = 0;
            this.roster.DoubleClick += new System.EventHandler(this.roster_DoubleClick);
            // 
            // ilPresence
            // 
            this.ilPresence.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilPresence.ImageSize = new System.Drawing.Size(20, 20);
            this.ilPresence.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilPresence.ImageStream")));
            this.ilPresence.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                  this.debug});
            this.tpDebug.Location = new System.Drawing.Point(4, 22);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Size = new System.Drawing.Size(624, 218);
            this.tpDebug.TabIndex = 0;
            this.tpDebug.Text = "Debug";
            // 
            // debug
            // 
            this.debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debug.Name = "debug";
            this.debug.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.debug.Size = new System.Drawing.Size(624, 218);
            this.debug.TabIndex = 2;
            this.debug.Text = "";
            this.debug.WordWrap = false;
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
            this.ClientSize = new System.Drawing.Size(632, 266);
            this.ContextMenu = this.mnuPresence;
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.tabControl1,
                                                                          this.sb});
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.pnlCon)).EndInit();
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
            muzzle.ClientLogin log = new muzzle.ClientLogin();
            log.User = jc.User;
            log.Password = jc.Password;
            log.Server = jc.Server;
            log.Port = jc.Port;
            log.SSL = jc.SSL;
            
            if (log.ShowDialog() == DialogResult.OK)
            {
                jc.User = log.User;
                jc.Password = log.Password;
                jc.Server = log.Server;
                jc.Port = log.Port;
                jc.SSL = log.SSL;
                jc.Connect();
            }
        }

        private void jc_OnReadText(object sender, string txt)
        {
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
        }

        private void jc_OnDisconnect(object sender)
        {
            roster.Nodes.Clear();
            pnlPresence.Text = "Offline";
            if (!m_err)
                pnlCon.Text = "Disconnected";
        }

        private void jc_OnError(object sender, System.Exception ex)
        {
            if (ex is Org.Mentalis.Security.Certificates.CertificateException)
                m_err = true;

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

        private void rm_OnRosterBegin(object sender)
        {
            roster.BeginUpdate();
        }

        private void rm_OnRosterEnd(object sender)
        {
            roster.EndUpdate();
        }

        private void rm_OnRosterItem(object sender, jabber.protocol.iq.Item ri)
        {
            roster.Nodes.Add(new RosterNode(ri, pm));
        }

        private void jc_OnPresence(object sender, jabber.protocol.client.Presence pres)
        {
            string bareFrom = pres.From.Bare;
            foreach (RosterNode n in roster.Nodes)
            {
                if (n.jid == bareFrom)
                {
                    n.ImageIndex = (pres.Type == PresenceType.available) ? 0 : 1;
                    n.SelectedImageIndex = n.ImageIndex;
                    break;
                }
            }
        }

        private void roster_DoubleClick(object sender, System.EventArgs e)
        {
            RosterNode n = roster.SelectedNode as RosterNode;
            if (n == null)
                return;
            new SendMessage(jc, n.jid).Show();
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

        private void jc_OnConnect(object sender, bedrock.net.AsyncSocket sock)
        {
            m_err = false;
            debug.AppendText("Connected to: " + sock.Address.IP + ":" + sock.Address.Port + "\r\n");
            
            if (sock.SSL)
            {
                debug.AppendText("\r\nServer Certificate:\r\n-------------------\r\n");
                debug.AppendText(sock.RemoteCertificate.ToString(true) + "\r\n");
            }
        }

        private void jc_OnStreamError(object sender, System.Xml.XmlElement rp)
        {
            m_err = true;
            pnlCon.Text = "Stream error: " + rp.InnerText;
        }
    }

    public class RosterNode : TreeNode
    {
        private jabber.protocol.iq.Item i;
        private jabber.client.PresenceManager p;
        public string jid;

        public RosterNode(jabber.protocol.iq.Item ri, jabber.client.PresenceManager pm) :
            base(ri.Nickname, (pm[ri.JID] == null) ? 1 : 0, (pm[ri.JID] == null) ? 1 : 0)
        {
            i = ri;
            p = pm;
            jid = i.JID;
        }
    }
}
