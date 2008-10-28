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
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using bedrock.util;
using System.Diagnostics;

namespace muzzle
{

    /// <summary>
    /// Summary description for BottomScrollRichText.
    /// </summary>
    [SVN(@"$Id$")]
    public class BottomScrollRichText : System.Windows.Forms.RichTextBox
    {
        private const int SB_HORZ             = 0;
        private const int SB_VERT             = 1;
        private const int SB_CTL              = 2;
        private const int SB_BOTH             = 3;

        private const int SB_LINEUP           = 0;
        private const int SB_LINELEFT         = 0;
        private const int SB_LINEDOWN         = 1;
        private const int SB_LINERIGHT        = 1;
        private const int SB_PAGEUP           = 2;
        private const int SB_PAGELEFT         = 2;
        private const int SB_PAGEDOWN         = 3;
        private const int SB_PAGERIGHT        = 3;
        private const int SB_THUMBPOSITION    = 4;
        private const int SB_THUMBTRACK       = 5;
        private const int SB_TOP              = 6;
        private const int SB_LEFT             = 6;
        private const int SB_BOTTOM           = 7;
        private const int SB_RIGHT            = 7;
        private const int SB_ENDSCROLL        = 8;

        private const int SIF_RANGE           = 0x0001;
        private const int SIF_PAGE            = 0x0002;
        private const int SIF_POS             = 0x0004;
        private const int SIF_DISABLENOSCROLL = 0x0008;
        private const int SIF_TRACKPOS        = 0x0010;
        private const int SIF_ALL             = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS);

        private const int WM_HSCROLL          = 0x0114;
        private const int WM_VSCROLL          = 0x0115;

        private const int EM_SETSCROLLPOS = 0x0400 + 222;

        private const int CCHILDREN_SCROLLBAR = 5;
        private const int STATE_SYSTEM_INVISIBLE   = 0x00008000;
        private const int STATE_SYSTEM_OFFSCREEN   = 0x00010000;
        private const int STATE_SYSTEM_PRESSED     = 0x00000008;
        private const int STATE_SYSTEM_UNAVAILABLE = 0x00000001;

        private const uint OBJID_CLIENT  = 0xFFFFFFFC;
        private const uint OBJID_VSCROLL = 0xFFFFFFFB;
        private const uint OBJID_HSCROLL = 0xFFFFFFFA;

        private bool m_bottom = true;
        private int m_maxLines = 500;

        /// <summary>
        /// Maximum number of lines to keep
        /// </summary>
        [Category("Appearance")]
        public int MaxLines
        {
            get { return m_maxLines; }
            set { m_maxLines = value; }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLINFO
        {
            public int  cbSize;
            public uint fMask;
            public int  nMin;
            public int  nMax;
            public uint nPage;
            public int  nPos;
            public int  nTrackPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLBARINFO
        {
            public int cbSize;
            public RECT rcScrollBar;
            public int dxyLineButton;
            public int xyThumbTop;
            public int xyThumbBottom;
            public int reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=CCHILDREN_SCROLLBAR+1)]
            public int[] rgstate;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class POINT
        {
            public int x;
            public int y;

            public POINT()
            {
            }

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, POINT lParam);

        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern bool GetScrollInfo(IntPtr hWnd, int nBar, ref SCROLLINFO lpsi);

        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern int SetScrollInfo(IntPtr hWnd, int fnBar, ref SCROLLINFO lpsi, bool fRedraw);

        [DllImport("user32", SetLastError=true, EntryPoint="GetScrollBarInfo")]
        private static extern int GetScrollBarInfo(IntPtr hWnd, uint idObject, ref SCROLLBARINFO psbi);

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Create a RichText that can scroll to the bottom easily.
        /// </summary>
        public BottomScrollRichText()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Is the text currently scrolled to the bottom?
        /// </summary>
        public bool IsAtBottom
        {
            get { return m_bottom; }
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

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        /// <summary>
        /// The message pump.  Overriden to catch the WM_VSCROLL events.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_VSCROLL)
            {
                SCROLLINFO si = GetScroll();
                m_bottom = (si.nPos + si.nPage + 5 >= si.nMax);
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Clear the text, and scroll back to the top.
        /// </summary>
        public void ClearAndScroll()
        {
            this.Text = "";
            this.Select(0, 0);
            this.ScrollToCaret();
            m_bottom = true;

            //SendMessage(this.Handle, EM_SETSCROLLPOS, 0, new POINT(0, 0));
        }

        private SCROLLINFO GetScroll()
        {
            SCROLLINFO si = new SCROLLINFO();
            si.cbSize = Marshal.SizeOf(si);
            si.fMask = SIF_PAGE | SIF_POS | SIF_RANGE;
            GetScrollInfo(this.Handle, SB_VERT, ref si);
            return si;
        }

        private SCROLLBARINFO GetBars()
        {
            SCROLLBARINFO si = new SCROLLBARINFO();
            si.cbSize = Marshal.SizeOf(si);
            int ret = GetScrollBarInfo(this.Handle, OBJID_VSCROLL, ref si);
            if (ret == 0)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return si;
        }

        /// <summary>
        /// Scroll to the bottom of the current text.
        /// </summary>
        public void ScrollToBottom()
        {
            SCROLLBARINFO sbi = GetBars();
            if (sbi.rgstate[0] == 0)
            {
                SCROLLINFO si = GetScroll();
                SendMessage(this.Handle, EM_SETSCROLLPOS, 0, new POINT(0, si.nMax - (int)si.nPage + 5));
            }
        }

        private static string EscapeRTF(string plain)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(plain.Length);

            char state = 'c';

            foreach (char c in plain)
            {
                if (state == 'r')
                {
                    if (c == '\n')
                        state = 'n';
                    else
                    {
                        state = 'c';
                    }
                }
                else if (state == 'n')
                {
                    state = 'c';
                }

                if (state == 'c')
                {
                    if (c > 127)
                    {
                        sb.Append(@"\u");
                        sb.Append((int)c);
                        sb.Append('?');
                    }
                    else
                    {
                        switch (c)
                        {
                            case '{':
                            case '}':
                            case '\\':
                                sb.Append('\\');
                                sb.Append(c);
                                break;
                            case '\r':
                                sb.Append("\\\r\n");
                                state = 'r';
                                break;
                            case '\n':
                                sb.Append("\\\r\n");
                                state = 'n';
                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                    }
                }
            }
            return sb.ToString();
        }

        private static string RTFColor(Color c)
        {
            return String.Format("\\red{0}\\green{1}\\blue{2};", c.R, c.G, c.B);
        }

        /// <summary>
        /// Append text with the given color to the end of the text area.
        /// Side effect: the existing selection is modified.  Save the selection
        /// if you want to keep it.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="text"></param>
        public void AppendText(Color c, string text)
        {
            this.SelectionLength = 0;
            this.SelectionStart = this.TextLength;
            string rtf = "{\\rtf1\\ansi{{\\colortbl ;" + RTFColor(c) +
                               "}\\cf1 " + EscapeRTF(text) + "}";
            this.SelectedRtf = rtf;
        }

        /// <summary>
        /// Append text.  If we were at the bottom, scroll to the bottom.  Otherwise leave the scroll position
        /// where it is.
        /// </summary>
        /// <param name="text"></param>
        public void AppendMaybeScroll(string text)
        {
            bool bottom = m_bottom;
            this.AppendText(text);
            if (bottom)
                ScrollToBottom();
        }

        /// <summary>
        /// Add a line that has a colored tag string, followed by a space, followed by
        /// a chunk of text in the default color, followed by a newline.
        ///
        /// Note: Although this seems kind of random, it's needed in several places.
        /// </summary>
        /// <param name="tagColor">The color to use for the tag</param>
        /// <param name="tag">The tag string</param>
        /// <param name="text">The main text</param>
        public void AppendMaybeScroll(Color tagColor, string tag, string text)
        {
            this.SuspendLayout();

            // This should always be called on the GUI thread, right?
            // Assume so.  No locking.

            bool bottom = m_bottom;
            int start = 0;
            int len = 0;
            if (!bottom)
            {
                start = this.SelectionStart;
                len = this.SelectionLength;
            }

            AppendText(tagColor, tag);
            AppendText(" ");
            AppendText(ForeColor, text);
            AppendText("\r\n");

            string[] lines = this.Lines;
            if (lines.Length > m_maxLines)
            {
                int rm = 0;
                bool ro = this.ReadOnly;
                this.ReadOnly = false;
                for (int i = 0; i < (lines.Length - m_maxLines); i++)
                {
                    rm += lines[i].Length + 1;
                }
                this.Select(0, rm);
                this.SelectedText = "";
                this.ReadOnly = ro;
            }

            if (bottom)
            {
                ScrollToBottom();
                this.SelectionStart = this.TextLength;
                this.SelectionLength = 0;
            }
            else
            {
                this.SelectionStart = start;
                this.SelectionLength = len;
            }

            this.ResumeLayout();
        }
    }
}
