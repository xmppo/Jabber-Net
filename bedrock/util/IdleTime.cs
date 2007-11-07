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
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

namespace bedrock.util
{
    /// <summary>
    /// TimeSpan event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="span"></param>
    public delegate void SpanEventHandler(object sender, TimeSpan span);

    /// <summary>
    /// Idle time calculations and notifications.
    /// </summary>
    [SVN(@"$Id$")]
    public class IdleTime : System.ComponentModel.Component
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

        /// <summary>
        /// Fired when user has been idle (mouse, keyboard) for the configured number of seconds.
        /// </summary>
        public event SpanEventHandler OnIdle;

        /// <summary>
        /// Fired when the user comes back.
        /// </summary>
        public event SpanEventHandler OnUnIdle;

        private const double DEFAULT_POLL = 5;      // 5s in s.
        private const double DEFAULT_IDLE = 5 * 60; // 5m in s.

        private System.Timers.Timer m_timer = null;
        private double m_notifySecs = DEFAULT_IDLE;
        private bool m_idle = false;
        private DateTime m_idleStart = DateTime.MinValue;
        private ISynchronizeInvoke m_invoker = null;

        /// <summary>
        /// Create an idle timer with the default timouts.
        /// </summary>
        public IdleTime()
        {
            m_timer = new System.Timers.Timer(DEFAULT_POLL * 1000.0);
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
        }

        /// <summary>
        /// Create an idle timer.  Make sure to set Enabled = true to start.
        /// </summary>
        /// <param name="pollSecs">Every pollSecs seconds, poll to see how long we've been away.</param>
        /// <param name="notifySecs">If we've been away notifySecs seconds, fire notification.</param>
        public IdleTime(int pollSecs, int notifySecs) : this()
        {
            if (pollSecs > notifySecs)
                throw new ArgumentException("Poll more often than you notify.");
            PollInterval = pollSecs;
            IdleLength = notifySecs;
        }

        /// <summary>
        /// Is the timer running?
        /// </summary>
        [Category("Logic")]
        [DefaultValue(false)]
        public bool Enabled
        {
            get { return m_timer.Enabled; }
            set { m_timer.Enabled = value; }
        }

        /// <summary>
        /// Time, in seconds, between checking for 
        /// </summary>
        [Category("Time")]
        [DefaultValue(DEFAULT_POLL)]
        public double PollInterval
        {
            get { return m_timer.Interval / 1000.0; }
            set { m_timer.Interval = value * 1000.0; }
        }

        /// <summary>
        /// The amount of time (in seconds) the computer can be idle before OnIdle is fired.
        /// </summary>
        [Category("Time")]
        [DefaultValue(DEFAULT_IDLE)]
        public double IdleLength
        {
            get { return m_notifySecs;  }
            set { m_notifySecs = value;  }
        }

        /// <summary>
        /// Are we currently idle?
        /// </summary>
        [Category("Logic")]
        public bool IsIdle
        {
            get { return m_idle; }
        }

        /// <summary>
        /// Invoke() all callbacks on this control.
        /// </summary>
        [Description("Invoke all callbacks on this control")]
        [DefaultValue(null)]
        [Category("Logic")]
        public ISynchronizeInvoke InvokeControl
        {
            get
            {
                // If we are running in the designer, let's try to get
                // an invoke control from the environment.  VB
                // programmers can't seem to follow directions.
                if ((this.m_invoker == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        object root = host.RootComponent;
                        if ((root != null) && (root is ISynchronizeInvoke))
                        {
                            m_invoker = (ISynchronizeInvoke)root;
                            // TODO: fire some sort of propertyChanged event,
                            // so that old code gets cleaned up correctly.
                        }
                    }
                }
                return m_invoker;
            }
            set { m_invoker = value; }
        }


        private void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            double idle = GetIdleTime();
            if (m_idle)
            {
                if (idle < PollInterval)
                {
                    m_idle = false;
                    if (OnUnIdle != null)
                    {
                        TimeSpan span = DateTime.Now - m_idleStart;
                        if ((m_invoker != null) &&
                            (m_invoker.InvokeRequired))
                        {
                            m_invoker.Invoke(OnUnIdle, new object[] { this, span });
                        }
                        else
                            OnUnIdle(this, span);
                    }
                    m_idleStart = DateTime.MinValue;
                }
            }
            else
            {
                if (idle > m_notifySecs)
                {
                    m_idle = true;
                    m_idleStart = DateTime.Now;
                    if (OnIdle != null)
                    {
                        TimeSpan span = new TimeSpan((long)(idle * 1000L));
                        if ((m_invoker != null) &&
                            (m_invoker.InvokeRequired))
                        {
                            m_invoker.Invoke(OnIdle, new object[] { this, span });
                        }
                        else
                            OnIdle(this, span);
                    }
                }
            }
        }
    }
}
