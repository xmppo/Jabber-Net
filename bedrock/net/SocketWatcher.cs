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
 * Portions Copyright (c) 2002 David Waite.
 * 
 * Acknowledgements
 * 
 * Special thanks to Dave Smith (dizzyd) for the design work.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Diagnostics;
using bedrock.util;
namespace bedrock.net
{
    /// <summary>
    /// A collection of sockets.  This makes a lot more sense in the poll() version (Unix/C) since
    /// you need to have a place to collect all of the sockets and call poll().  Here, it's just
    /// convenience functions.
    /// </summary>
    [RCS(@"$Header$")]
    public class SocketWatcher : IDisposable
    {
        private enum State
        {
            Running,
            Shutdown,
            Stopped
        };

        private AsyncSocket m_pending = null;
        private IList       m_socks;
        private object      m_lock = new object();
        private int         m_maxSocks;

        /// <summary>
        /// Create a new instance, which will manage an unlimited number of sockets.
        /// </summary>
        public SocketWatcher()
        {
            m_maxSocks = -1;
            m_socks = new ArrayList();
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="maxsockets">Maximum number of sockets to watch.  In this version, 
        /// this is mostly for rate-limiting purposes.</param>
        public SocketWatcher(int maxsockets)
        {
            // replace with linkedlist?
            m_maxSocks = maxsockets;
            m_socks = new ArrayList(m_maxSocks);
        }

        /// <summary>
        /// The maximum number of sockets watched.  Throws 
        /// InvalidOperationException if the new value is fewer than the number 
        /// currently open.  -1 means no limit.
        /// </summary>
        public int MaxSockets
        {
            get { return m_maxSocks; }
            set 
            { 
                if ((value >= 0) && (m_socks.Count >= value))
                    throw new InvalidOperationException("Too many sockets: " + m_socks.Count);
                
                m_maxSocks = value; 
            }
        }

        /// <summary>
        /// Create a socket that is listening for inbound connections.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <param name="reuseaddr">Set the ReuseAddr socket option?</param>
        /// <returns>A socket that is ready for calling RequestAccept()</returns>
        public AsyncSocket CreateListenSocket(ISocketEventListener listener,
                                              Address              addr,
                                              bool                 reuseaddr)
        {
            //Debug.Assert(m_maxSocks > 1);
            AsyncSocket result = new AsyncSocket(this, listener);
            result.Accept(addr, reuseaddr);
            return result;
        }

        /// <summary>
        /// Create an outbound socket.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <returns>Socket that is in the process of connecting</returns>
        public AsyncSocket CreateConnectSocket(ISocketEventListener listener,
                                               Address              addr)
        {
            return CreateConnectSocket(listener, addr, -1);
        }

        /// <summary>
        /// Create an outbound socket.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <param name="keepAlive">Timeout, in milliseconds between keep alives.</param>
        /// <returns>Socket that is in the process of connecting</returns>
        public AsyncSocket CreateConnectSocket(ISocketEventListener listener,
                                               Address              addr,
                                               int                  keepAlive)
        {
            AsyncSocket result;
   
            // Create the socket:
            result = new AsyncSocket(this, listener);
            result.KeepAlive = keepAlive;
            // Start the connect process:
            result.Connect(addr);
            return result;
        }

        /// <summary>
        /// Called by AsyncSocket when a new connection is received on a listen socket.
        /// </summary>
        /// <param name="s">New socket connection</param>
        public void RegisterSocket(AsyncSocket s)
        {
            
            lock (m_lock)
            {
                if ((m_maxSocks >= 0) && (m_socks.Count >= m_maxSocks))
                    throw new InvalidOperationException("Too many sockets: " + m_socks.Count);
                m_socks.Add(s);
            }
        }

        /// <summary>
        /// Called by AsyncSocket when a socket is closed.
        /// </summary>
        /// <param name="s">Closed socket</param>
        public void CleanupSocket(AsyncSocket s)
        {
            lock (m_lock)
            {
                m_socks.Remove(s);
                
                if (m_pending != null)
                {
                    m_pending.RequestAccept();
                    m_pending = null;
                }
            }
        }

        /// <summary>
        /// Called by AsyncSocket when this class is full, and the listening AsyncSocket 
        /// socket would like to be restarted when there are slots free.
        /// </summary>
        /// <param name="s">Listening socket</param>
        public void PendingAccept(AsyncSocket s)
        {
            lock (m_lock)
            {
                Debug.Assert(m_pending == null, "you currently can't have more than one listen socket in a socketwatcher at a time");
                m_pending = s;
            }
        }

        /// <summary>
        /// Or close.  Potato, tomato.  This is useful if you want to use using().
        /// </summary>
        public void Dispose()
        {
            lock (m_lock)
            {
                m_pending = null;
                foreach (AsyncSocket s in m_socks)
                {
                    s.Close();
                }
            }
        }        
    }
}
