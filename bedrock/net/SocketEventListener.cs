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
 * Special thanks to Dave Smith (dizzyd) for the design work.
 * 
 * --------------------------------------------------------------------------*/
using System;
using bedrock.util;
namespace bedrock.net
{
    /// <summary>
    /// Interface class for Socket events. Any object which
    /// implements these interfaces is eligible to recieve Socket
    /// events.  This is an interface instead of events in order
    /// to preserve symmetry with libbedrock.
    /// </summary>
    [RCS(@"$Header$")]
    public interface ISocketEventListener
    {
        /// <summary>
        /// An accept socket is about to be bound, or a connect socket is about to connect, 
        /// or an incoming socket just came in.  Use this as an opportunity to 
        /// </summary>
        /// <param name="new_sock">The new socket that is about to be connected.</param>
        void OnInit(AsyncSocket new_sock);

        /// <summary>
        /// We accepted a socket, and need to get a listener.
        /// If the return value is null, then the socket will be closed, 
        /// and RequestAccept will ALWAYS be called.
        /// </summary>
        /// <param name="new_sock">The new socket.</param>
        /// <returns>The listener for the *new* socket, as compared to 
        /// the listener for the *listen* socket</returns>
        ISocketEventListener GetListener(AsyncSocket new_sock);

        /// <summary>
        /// A new incoming connection was accepted.
        /// </summary>
        /// <param name="newsocket">Socket for new connection.</param>
        /// <returns>true if RequestAccept() should be called automatically again</returns>
        bool OnAccept(AsyncSocket newsocket);
        /// <summary>
        /// Outbound connection was connected.
        /// </summary>
        /// <param name="sock">Connected socket.</param>
        void OnConnect(AsyncSocket sock);
        /// <summary>
        /// Connection was closed.
        /// </summary>
        /// <param name="sock">Closed socket.  Already closed!</param>
        void OnClose(AsyncSocket sock);
        /// <summary>
        /// An error happened in processing.  The socket is no longer open.
        /// </summary>
        /// <param name="sock">Socket in error</param>
        /// <param name="ex">Exception that caused the error</param>
        void OnError(AsyncSocket sock, Exception ex);
        /// <summary>
        /// Bytes were read from the socket.
        /// </summary>
        /// <param name="sock">The socket that was read from.</param>
        /// <param name="buf">The bytes that were read.</param>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        /// <returns>true if RequestRead() should be called automatically again</returns>
        bool OnRead (AsyncSocket sock, byte[] buf, int offset, int length);
        /// <summary>
        /// Bytes were written to the socket.
        /// </summary>
        /// <param name="sock">The socket that was written to.</param>
        /// <param name="buf">The bytes that were written.</param>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        void OnWrite(AsyncSocket sock, byte[] buf, int offset, int length);
    }
    /// <summary>
    /// Default, empty implementation of ISocketEventListener
    /// </summary>
    [RCS(@"$Header$")]
    public class SocketEventListener : ISocketEventListener
    {
        #region Implementation of ISocketEventListener
        /// <summary>
        /// An accept socket is about to be bound, or a connect socket is about to connect, 
        /// or an incoming socket just came in.  Use this as an opportunity to 
        /// </summary>
        /// <param name="new_sock">The new socket that is about to be connected.</param>
        public virtual void OnInit(AsyncSocket new_sock)
        {
        }

        /// <summary>
        /// We accepted a socket, and need to get a listener.
        /// If the return value is null, then the socket will be closed, 
        /// and RequestAccept will ALWAYS be called.
        /// </summary>
        /// <param name="new_sock">The new socket.</param>
        /// <returns>The listener for the *new* socket, as compared to 
        /// the listener for the *listen* socket</returns>
        public virtual ISocketEventListener GetListener(AsyncSocket new_sock)
        {
            return this;
        }

        /// <summary>
        /// A new incoming connection was accepted.
        /// </summary>
        /// <param name="newsocket">Socket for new connection.</param>
        /// <returns>true if RequestAccept() should be called automatically again</returns>
        public virtual bool OnAccept(AsyncSocket newsocket)
        {
            return true;
        }

        /// <summary>
        /// Outbound connection was connected.
        /// </summary>
        /// <param name="sock">Connected socket.</param>
        public virtual void OnConnect(AsyncSocket sock)
        {
        }

        /// <summary>
        /// Connection was closed.
        /// </summary>
        /// <param name="sock">Closed socket.  Already closed!</param>
        public virtual void OnClose(AsyncSocket sock)
        {
        }

        /// <summary>
        /// An error happened in processing.  The socket is no longer open.
        /// </summary>
        /// <param name="sock">Socket in error</param>
        /// <param name="ec">Exception that caused the error</param>
        public virtual void OnError(AsyncSocket sock, System.Exception ec)
        {
        }

        /// <summary>
        /// Bytes were read from the socket.
        /// </summary>
        /// <param name="sock">The socket that was read from.</param>
        /// <param name="buf">The bytes that were read.</param>
        /// <returns>true if RequestRead() should be called automatically again</returns>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        public virtual bool OnRead(AsyncSocket sock, byte[] buf, int offset, int length)
        {
            return true;
        }

        /// <summary>
        /// Bytes were written to the socket.
        /// </summary>
        /// <param name="sock">The socket that was written to.</param>
        /// <param name="buf">The bytes that were written.</param>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        public virtual void OnWrite(AsyncSocket sock, byte[] buf, int offset, int length)
        {
        }    
        #endregion
    }
}
