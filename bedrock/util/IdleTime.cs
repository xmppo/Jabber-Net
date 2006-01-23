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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace bedrock.util
{
    /// <summary>
    /// Idle time calculations.
    /// </summary>
	public class IdleTime
	{
        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public int cbSize;
            public int dwTime;
        }

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// Get the number of seconds since last user input (mouse or keyboard) system-wide.
        /// </summary>
        /// <returns></returns>
        public static double GetIdleTime()
        {
            LASTINPUTINFO lii = new LASTINPUTINFO();
            lii.cbSize = Marshal.SizeOf(lii.GetType());
            if (!GetLastInputInfo(ref lii))
                throw new ApplicationException("Error executing GetLastInputInfo");
            return (Environment.TickCount - lii.dwTime) / 1000.0;
        }

        private System.Timers.Timer m_timer = null;
        private int m_notifySecs;

        public IdleTime(int pollSecs, int notifySecs)
        {
            m_notifySecs = notifySecs;
            m_timer = new System.Timers.Timer(pollSecs);
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
        }

        void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            double idle = GetIdleTime();
            if 
        }
	}
}
