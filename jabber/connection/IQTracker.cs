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
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Xml;

using jabber.protocol.client;

namespace jabber.connection
{
    /// <summary>
    /// Received a response to an IQ request.
    /// </summary>
    public delegate void IqCB(object sender, IQ iq, object data);

    /// <summary>
    /// Track outstanding IQ requests.
    /// </summary>
    public class IQTracker
    {
        private Hashtable           m_pending = new Hashtable();
        private SocketElementStream m_cli     = null;

        /// <summary>
        /// Create a new IQ tracker
        /// </summary>
        /// <param name="stream">The client to send/receive on</param>
        public IQTracker(SocketElementStream stream)
        {
            m_cli = stream;
            m_cli.OnProtocol += new jabber.xml.ProtocolHandler(OnIQ);
        }
        
        private void OnIQ(object sender, XmlElement elem)
        {
            IQ iq = elem as IQ;
            if (iq == null)
                return;

            string id = iq.ID;
            TrackerData td;

            lock (m_pending)
            {
                td = (TrackerData) m_pending[id];

                // this wasn't one that was being tracked.  
                if (td == null)
                {
                    return;
                }
                m_pending.Remove(id);
            }

            // don't need to check for null.  protected by check below.
            td.cb(this, iq, td.data);
        }

        /// <summary>
        /// Start an IQ request.
        /// </summary>
        public void BeginIQ(IQ iq, IqCB cb, object cbArg)
        {
            // if no callback, ignore response.
            if (cb != null)
            {
                TrackerData td = new TrackerData();
                td.cb   = cb;
                td.data = cbArg;
                lock (m_pending)
                {
                    m_pending[iq.ID] = td;
                }
            }
            m_cli.Write(iq);
        }

        /// <summary>
        /// Do a synchronous IQ request, which waits for a response.
        /// </summary>
        /// <param name="iq">An IQ packet to send, and wait for.</param>
        /// <param name="millisecondsTimeout">Time to wait for response, in milliseconds</param>
        public IQ IQ(IQ iq, int millisecondsTimeout)
        {
            TrackerData td = new TrackerData();
            td.cb   = new IqCB(SignalEvent);
            AutoResetEvent are = new AutoResetEvent(false);
            td.data = are;
            string id = iq.ID;
            lock (m_pending)
            {
                m_pending[id] = td;
                m_cli.Write(iq);
                if (!are.WaitOne(millisecondsTimeout, true))
                {
                    throw new Exception("Timeout waiting for IQ response");
                }
                IQ resp = (IQ) m_pending[id];
                m_pending.Remove(id);
                return resp;
            }
        }

        private void SignalEvent(object sender, IQ iq, object data)
        {
            lock (m_pending)
            {
                m_pending[iq.ID] = iq;
                ((AutoResetEvent)data).Set();
            }
        }

        private class TrackerData
        {
            public IqCB  cb;
            public object data;
        }
    }
}
