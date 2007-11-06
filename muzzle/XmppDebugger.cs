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
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

using bedrock.util;

namespace muzzle
{
    /// <summary>
    /// Debug stream for XMPP, so I don't have write it every time.
    /// </summary>
    [SVN(@"$Id$")]
    public class XmppDebugger : StreamControl
    {
        private RichTextBox rtSend;
        private Splitter splitter1;
        private BottomScrollRichText rtDebug;

        private Color m_sendColor = Color.Blue;
        private Color m_recvColor = Color.Orange;
        private Color m_textColor = Color.Black;
        private Color m_errColor = Color.Red;
        private Color m_otherColor = Color.Green;
        private string m_send = "SEND:";
        private string m_recv = "RECV:";
        private string m_err = "ERROR:";
        private string m_last = "";

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Create
        /// </summary>
        public XmppDebugger()
        {
            InitializeComponent();
            this.OnStreamChanged += new bedrock.ObjectHandler(XmppDebugger_OnStreamChanged);
        }

        /// <summary>
        /// What color to use for the "SEND:" string.
        /// </summary>
        [Category("Appearance")]
        public Color SendColor
        {
            get { return m_sendColor; }
            set { m_sendColor = value; }
        }

        /// <summary>
        /// What color to use for the "RECV:" string.
        /// </summary>
        [Category("Appearance")]
        public Color ReceiveColor
        {
            get { return m_recvColor; }
            set { m_recvColor = value; }
        }

        /// <summary>
        /// What color to use for the "ERROR:" string.
        /// </summary>
        [Category("Appearance")]
        public Color ErrorColor
        {
            get { return m_errColor; }
            set { m_errColor = value; }
        }

        /// <summary>
        /// What color to use for the sent and received text.
        /// </summary>
        [Category("Appearance")]
        public Color TextColor
        {
            get { return m_textColor; }
            set { m_textColor = value; }
        }

        /// <summary>
        /// What color to use for other text inserted
        /// </summary>
        [Category("Appearance")]
        public Color OtherColor
        {
            get { return m_otherColor; }
            set { m_otherColor = value; }
        }

        /// <summary>
        /// The string to prefix on sent bytes.
        /// </summary>
        [Category("Text")]
        [DefaultValue("SEND:")]
        public string SendPrefix
        {
            get { return m_send; }
            set { m_send = value; }
        }

        /// <summary>
        /// The string to prefix on sent bytes.
        /// </summary>
        [Category("Text")]
        [DefaultValue("RECV:")]
        public string ReceivePrefix
        {
            get { return m_recv; }
            set { m_recv = value; }
        }

        /// <summary>
        /// The string to prefix on errors.
        /// </summary>
        [Category("Text")]
        [DefaultValue("ERROR:")]
        public string ErrorPrefix
        {
            get { return m_err; }
            set { m_err = value; }
        }

        private void XmppDebugger_OnStreamChanged(object sender)
        {
            if (m_stream == null)
                return;

            m_stream.OnConnect += new jabber.connection.StanzaStreamHandler(m_stream_OnConnect);
            m_stream.OnReadText += new bedrock.TextHandler(m_stream_OnReadText);
            m_stream.OnWriteText += new bedrock.TextHandler(m_stream_OnWriteText);
            m_stream.OnError += new bedrock.ExceptionHandler(m_stream_OnError);
        }

        private void Write(Color color, string tag, string text)
        {
            Debug.WriteLine(tag + " " + text);

            rtDebug.SelectionColor = color;
            rtDebug.AppendText(tag);
            rtDebug.AppendText(" ");
            rtDebug.SelectionColor = m_textColor;
            rtDebug.AppendText(text);
            rtDebug.AppendMaybeScroll("\r\n");
        }

        /// <summary>
        /// Write an error to the log.
        /// </summary>
        /// <param name="error"></param>
        public void WriteError(string error)
        {
            Write(m_errColor, m_err, error);
        }

        private void m_stream_OnError(object sender, Exception ex)
        {
            WriteError(ex.ToString());
        }

        private void m_stream_OnConnect(object sender, jabber.connection.StanzaStream stream)
        {
            // I think this is right.  Double check.
            rtDebug.Clear();
        }

        private void m_stream_OnReadText(object sender, string txt)
        {
            // keepalive
            if (txt == " ")
                return;

            Write(m_recvColor, m_recv, txt);
        }

        private void m_stream_OnWriteText(object sender, string txt)
        {
            // keepalive
            if (txt == " ")
                return;

            Write(m_sendColor, m_send, txt);
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
        /// Clear both text boxes
        /// </summary>
        public void Clear()
        {
            rtDebug.Clear();
            rtSend.Clear();
        }

        /// <summary>
        /// Write other text to the debug log
        /// </summary>
        /// <param name="tag">The tag to prefix with</param>
        /// <param name="text">The text after the tag</param>
        public void Write(string tag, string text)
        {
            Write(m_otherColor, tag, text);
        }

        private XmlElement ValidateXML()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rtSend.Text);
                XmlElement elem = doc.DocumentElement;
                if (elem != null)
                {
                    return elem;
                }
            }
            catch (XmlException ex)
            {
                int offset = ex.LinePosition;
                for (int i=0; (i<ex.LineNumber-1) && (i < rtSend.Lines.Length); i++)
                {
                    offset += rtSend.Lines[i].Length + 2;
                }
                rtSend.Select(offset, 1);
            }
            return null;
        }

        private void ValidateAndSend()
        {
            this.UseWaitCursor = true;
            XmlElement elem = ValidateXML();
            if (elem != null)
            {
                m_stream.Write(elem);
                rtSend.Clear();
            }
            this.UseWaitCursor = false;
        }

        private void rtSend_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && e.Control)
            {
                ValidateAndSend();
            }
            else if ((e.KeyCode == Keys.Delete) && e.Control)
            {
                Clear();
            }
        }

        private void Search(string txt)
        {
            string t = (txt == null) ? m_last : txt;
            if (t == "")
                return;
            m_last = t;
            int start = rtDebug.SelectionStart + 1;
            if ((start < 0) || (start > rtDebug.Text.Length))
                start = 0;
            int offset = rtDebug.Text.IndexOf(t, start);
            if (offset < 0)
            {
                Console.Beep();
                offset = 0;
            }
            rtDebug.Select(offset, t.Length);
            rtDebug.ScrollToCaret();
        }

        private void rtDebug_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && e.Control)
            {
                Clear();
            }
            else if ((e.KeyCode == Keys.F) && e.Control)
            {
                InputBox inp = new InputBox();
                if (inp.ShowDialog("Find text", "Find:", "") != DialogResult.OK)
                    return;
                Search(inp.Value);
            }
            else if (e.KeyCode == Keys.F3)
            {
                Search(null);
            }
        }

        private void XmppDebugger_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && e.Control)
            {
                Clear();
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtSend = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.rtDebug = new muzzle.BottomScrollRichText();
            this.SuspendLayout();
            // 
            // rtSend
            // 
            this.rtSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtSend.Location = new System.Drawing.Point(0, 110);
            this.rtSend.Name = "rtSend";
            this.rtSend.Size = new System.Drawing.Size(150, 40);
            this.rtSend.TabIndex = 0;
            this.rtSend.Text = "";
            this.rtSend.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rtSend_KeyUp);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 107);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(150, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // rtDebug
            // 
            this.rtDebug.BackColor = System.Drawing.SystemColors.Window;
            this.rtDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtDebug.Location = new System.Drawing.Point(0, 0);
            this.rtDebug.Name = "rtDebug";
            this.rtDebug.ReadOnly = true;
            this.rtDebug.Size = new System.Drawing.Size(150, 107);
            this.rtDebug.TabIndex = 2;
            this.rtDebug.Text = "";
            this.rtDebug.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rtDebug_KeyUp);
            // 
            // XmppDebugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtDebug);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.rtSend);
            this.Name = "XmppDebugger";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.XmppDebugger_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
