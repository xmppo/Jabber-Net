using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace muzzle
{

	/// <summary>
	/// Summary description for BottomScrollRichText.
	/// </summary>
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

        private bool m_bottom = true;

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

			// TODO: Add any initialization after the InitializeComponent call

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

        private SCROLLINFO GetScroll()
        {
            SCROLLINFO si = new SCROLLINFO();
            si.cbSize = Marshal.SizeOf(si);
            si.fMask = SIF_PAGE | SIF_POS | SIF_RANGE;
            GetScrollInfo(this.Handle, SB_VERT, ref si);
            return si;
        }

        /// <summary>
        /// Scroll to the bottom of the current text.
        /// </summary>
        public void ScrollToBottom()
        {
            SCROLLINFO si = GetScroll();
            SendMessage(this.Handle, EM_SETSCROLLPOS, 0, new POINT(0, si.nMax - (int)si.nPage + 5));
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
	}
}
