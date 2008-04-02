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
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Xml;

using bedrock.util;
using jabber.protocol.client;

namespace jabber.connection
{
    /// <summary>
    /// Informs the client that a response to an IQ request has been received.
    /// </summary>
    public delegate void IqCB(object sender, IQ iq, object data);

    /// <summary>
    /// Informs the client that an IQ has timed out.
    /// </summary>
    [SVN(@"$Id$")]
    public class IQTimeoutException : Exception
    {
        /// <summary>
        /// Creates a new timeout exception.
        /// </summary>
        /// <param name="message">Description of the error.</param>
        public IQTimeoutException(string message)
            : base(message)
        {
        }
    }

    ///<summary>
    /// Represents the interface for tracking an IQ packet.
    ///</summary>
    [SVN(@"$Id$")]
    public interface IIQTracker
    {
        ///<summary>
        /// Does an asynchronous IQ call.
        ///</summary>
        ///<param name="iq">IQ packet to send.</param>
        ///<param name="cb">Callback to execute when the result comes back.</param>
        ///<param name="cbArg">Arguments to pass to the callback.</param>
        void BeginIQ(IQ iq, IqCB cb, object cbArg);

        ///<summary>
        /// Does a synchronous IQ call.
        ///</summary>
        ///<param name="iqp">IQ packet to send.</param>
        ///<param name="millisecondsTimeout">Time, in milliseconds, to wait for the response.</param>
        ///<returns>The IQ packet that was sent back.</returns>
        IQ IQ(IQ iqp, int millisecondsTimeout);
    }

    /// <summary>
    /// Tracks outstanding IQ requests.
    /// </summary>
    [SVN(@"$Id$")]
    public class IQTracker: IIQTracker
    {
        private Hashtable  m_pending = new Hashtable();
        private XmppStream m_cli     = null;

        /// <summary>
        /// Creates a new IQ tracker.
        /// </summary>
        /// <param name="stream">The client to send/receive on</param>
        public IQTracker(XmppStream stream)
        {
            m_cli = stream;
            m_cli.OnProtocol += new jabber.protocol.ProtocolHandler(OnIQ);
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

            td.Call(this, iq);
        }

        /// <summary>
        /// Starts an IQ request.
        /// </summary>
        /// <param name="iq">IQ to send.</param>
        /// <param name="cb">Callback to use when a response comes back.</param>
        /// <param name="cbArg">Arguments to the callback.</param>
        public void BeginIQ(IQ iq, IqCB cb, object cbArg)
        {
            // if no callback, ignore response.
            if (cb != null)
            {
                TrackerData td = new TrackerData(cb, cbArg);
                lock (m_pending)
                {
                    m_pending[iq.ID] = td;
                }
            }
            m_cli.Write(iq);
        }

        /// <summary>
        /// Sends an IQ request and waits for the response.
        /// </summary>
        /// <param name="iqp">An IQ packet to send, and wait for.</param>
        /// <param name="millisecondsTimeout">Time to wait for response, in milliseconds</param>
        /// <returns>An IQ in reponse to the sent IQ.</returns>
        public IQ IQ(IQ iqp, int millisecondsTimeout)
        {
            AutoResetEvent are = new AutoResetEvent(false);
            TrackerData td = new TrackerData(SignalEvent, are);
            string id = iqp.ID;
            lock (m_pending)
            {
                m_pending[id] = td;
            }
            m_cli.Write(iqp);

            if (!are.WaitOne(millisecondsTimeout, true))
            {
                throw new Exception("Timeout waiting for IQ response");
            }

            lock (m_pending)
            {
                IQ resp = (IQ) m_pending[id];
                m_pending.Remove(id);
                return resp;
            }
        }

        private void SignalEvent(object sender, IQ iq, object data)
        {
            m_pending[iq.ID] = iq;
            ((AutoResetEvent)data).Set();
        }

        /// <summary>
        /// Internal state for a pending tracker request
        /// </summary>
        [SVN(@"$Id$")]
        public class TrackerData
        {
            private IqCB  cb;
            private object data;

            /// <summary>
            /// Create a tracker data instance.
            /// </summary>
            /// <param name="callback"></param>
            /// <param name="state"></param>
            public TrackerData(IqCB callback, object state)
            {
                Debug.Assert(callback != null);
                cb = callback;
                data = state;
            }

            /// <summary>
            /// Call the callback.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="iq"></param>
            public void Call(object sender, IQ iq)
            {
                cb(sender, iq, data);
            }
        }
    }
}
