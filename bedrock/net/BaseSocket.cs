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
 * Copyright (c) 2003 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Diagnostics;

namespace bedrock.net
{
	/// <summary>
	/// Base class for AsyncSocket and proxies for AsyncSocket
	/// </summary>
	public abstract class BaseSocket
	{
        /// <summary>
        /// Call through this interface when events happen.  WARNING: AsyncSocket assumes this is not NULL.
        /// </summary>
        protected ISocketEventListener m_listener = null;

        /// <summary>
        /// Only to be called by things that immediately set m_listener!
        /// </summary>
        protected BaseSocket()
        {
        }


        /// <summary>
        /// Get a stream to read from this socket synchronously.
        /// </summary>
        /// <returns></returns>
        public abstract System.IO.Stream GetStream();

        /// <summary>
        /// Construct a BaseSocket.
        /// </summary>
        /// <param name="listener"></param>
        protected BaseSocket(ISocketEventListener listener)
        {
            Debug.Assert(listener != null);
            m_listener = listener;
        }

        /// <summary>
        /// Where to send notifications of interesting things.
        /// WARNING!  Only assign to this if you are Tom Waters.
        /// </summary>
        public virtual ISocketEventListener Listener
        {
            get 
            {
                return m_listener;
            }
            set
            {
                lock (this)
                {
                    //if (m_reading)
                    //    throw new InvalidOperationException("Don't set listener while reading, Tom.");
                    m_listener = value;
                }
            }
        }

        /// <summary>
        /// Prepare to start accepting inbound requests.  Call RequestAccept() to start the async process.
        /// Default the listen queue size to 5.
        /// </summary>
        /// <param name="addr">Address to listen on</param>
        public void Accept(Address addr)
        {
            Accept(addr, 5);
        }

        /// <summary>
        /// Prepare to start accepting inbound requests.  Call RequestAccept() to start the async process.
        /// </summary>
        /// <param name="addr">Address to listen on</param>
        /// <param name="backlog">The Maximum length of the queue of pending connections</param>
        public abstract void Accept(Address addr, int backlog);

        /// <summary>
        /// Start the flow of async accepts.  Flow will continue while 
        /// Listener.OnAccept() returns true.  Otherwise, call RequestAccept() again
        /// to continue.
        /// </summary>
        public abstract void RequestAccept();

        /// <summary>
        /// Outbound connection.  Eventually calls Listener.OnConnect() when 
        /// the connection comes up.  Don't forget to call RequestRead() in
        /// OnConnect()!
        /// </summary>
        /// <param name="addr"></param>
        public abstract void Connect(Address addr);

#if !NO_SSL
        /// <summary>
        /// Start TLS processing on an open socket.
        /// </summary>
        public abstract void StartTLS();
#endif

        /// <summary>
        /// Start an async read from the socket.  Listener.OnRead() is eventually called
        /// when data arrives.
        /// </summary>
        public abstract void RequestRead();

        /// <summary>
        /// Async write to the socket.  Listener.OnWrite will be called eventually
        /// when the data has been written.  A copy is made of the data, internally.
        /// </summary>
        /// <param name="buf">Data to write</param>
        public void Write(byte[] buf)
        {
            Write(buf, 0, buf.Length);
        }

        /// <summary>
        /// Async write to the socket.  Listener.OnWrite will be called eventually
        /// when the data has been written.  A trimmed copy is made of the data, internally.
        /// </summary>
        /// <param name="buf">Buffer to output</param>
        /// <param name="offset">Offset into buffer</param>
        /// <param name="len">Number of bytes to output</param>
        public abstract void Write(byte[] buf, int offset, int len);

        /// <summary>
        /// Close the socket.  This is NOT async.  .Net doesn't have async closes.  
        /// But, it can be *called* async, particularly from GotData.
        /// Attempts to do a shutdown() first.
        /// </summary>
        public abstract void Close();
	}
}
