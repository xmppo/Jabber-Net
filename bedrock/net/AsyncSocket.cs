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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using bedrock.util;
namespace bedrock.net
{
    /// <summary>
    /// Lame exception, since I couldn't find one I liked.
    /// </summary>
    [RCS(@"$Header$")]
    [Serializable]
    public class AsyncSocketConnectionException : System.SystemException
    {
        /// <summary>
        /// Create a new exception instance.
        /// </summary>
        /// <param name="description"></param>
        public AsyncSocketConnectionException(string description) : base(description)
        {
        }

        /// <summary>
        /// Create a new exception instance.
        /// </summary>
        public AsyncSocketConnectionException() : base()
        {
        }

        /// <summary>
        /// Create a new exception instance, wrapping another exception.
        /// </summary>
        /// <param name="description">Desecription of the exception</param>
        /// <param name="e">Inner exception</param>
        public AsyncSocketConnectionException(string description, Exception e) : base(description, e)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AsyncSocketConnectionException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="ctx">The contextual information about the source or destination.</param>
        protected AsyncSocketConnectionException(System.Runtime.Serialization.SerializationInfo info, 
            System.Runtime.Serialization.StreamingContext ctx) : 
            base(info, ctx)
        {
        }
    }
    /// <summary>
    /// An asynchronous socket, which calls a listener class when interesting things happen.
    /// </summary>
    [RCS(@"$Header$")]
    public class AsyncSocket : BaseSocket, IComparable
    {
        /// <summary>
        /// Socket states.
        /// </summary>
        [RCS(@"$Header$")]
        private enum State
        {
            /// <summary>
            /// Socket has been created.
            /// </summary>
            Created, 
            /// <summary>
            /// Socket is listening for new connections
            /// </summary>
            Listening,
            /// <summary>
            /// Doing DNS lookup
            /// </summary>
            Resolving,
            /// <summary>
            /// Attempting to connect
            /// </summary>
            Connecting,
            /// <summary>
            /// Connected to a peer.  The running state.
            /// </summary>
            Connected,
            /// <summary>
            /// Shutting down the socket.
            /// </summary>
            Closing,
            /// <summary>
            /// Closed down.
            /// </summary>
            Closed,
            /// <summary>
            /// An error ocurred.
            /// </summary>
            Error
        }

        private byte[]               m_buf        = new byte[4096];
        private State                m_state      = State.Created;
        private Socket               m_sock       = null;
        private SocketWatcher        m_watcher    = null;
        private Address              m_addr;
        private Guid                 m_id         = Guid.NewGuid();
        private bool                 m_reading    = false;

        /// <summary>
        /// Called from SocketWatcher.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="listener">The listener for this socket</param>
        public AsyncSocket(SocketWatcher w, ISocketEventListener listener) : base(listener)
        {
            m_watcher = w;
        }

        private AsyncSocket(SocketWatcher w) : base()
        {
            m_watcher = w;
        }

        /*
        /// <summary>
        /// Return the state of the socket.  WARNING: don't use this.
        /// </summary>
        public State Socket_State
        {
            get
            {
                return m_state;
            }
        }
        */

        /// <summary>
        /// For connect sockets, the remote address.  For Accept sockets, the local address.
        /// </summary>
        public Address Address
        {
            get
            {
                return m_addr;
            }
        }

        /// <summary>
        /// Sets the specified option to the specified value.
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, 
                                    SocketOptionName optionName, 
                                    byte[] optionValue)
        {
            m_sock.SetSocketOption(optionLevel, optionName, optionValue);
        }

        /// <summary>
        /// Sets the specified option to the specified value.
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, 
                                    SocketOptionName optionName, 
                                    int optionValue)
        {
            m_sock.SetSocketOption(optionLevel, optionName, optionValue);
        }

        /// <summary>
        /// Sets the specified option to the specified value.
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, 
                                    SocketOptionName optionName, 
                                    object optionValue)
        {
            m_sock.SetSocketOption(optionLevel, optionName, optionValue);
        }

        /// <summary>
        /// Prepare to start accepting inbound requests.  Call RequestAccept() to start the async process.
        /// </summary>
        /// <param name="addr">Address to listen on</param>
        /// <param name="backlog">The Maximum length of the queue of pending connections</param>
        public override void Accept(Address addr, int backlog)
        {
            m_addr = addr;
            m_sock = new Socket(AddressFamily.InterNetwork, 
                                SocketType.Stream, 
                                ProtocolType.Tcp);
            m_sock.Blocking = false;
            // Always reuse address.
            m_sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            m_sock.Bind(m_addr.Endpoint);
            m_sock.Listen(backlog);
            m_state = State.Listening;
            m_watcher.RegisterSocket(this);
        }

        /// <summary>
        /// Start the flow of async accepts.  Flow will continue while 
        /// Listener.OnAccept() returns true.  Otherwise, call RequestAccept() again
        /// to continue.
        /// </summary>
        public override void RequestAccept()
        {
            lock (this)
            {
                if (m_state != State.Listening)
                {
                    throw new InvalidOperationException("Not a listen socket");
                }
                m_sock.BeginAccept(new AsyncCallback(ExecuteAccept), null);
            }
        }

        /// <summary>
        /// We got a connection from outside.  Add it to the SocketWatcher.
        /// </summary>
        /// <param name="ar"></param>
        private void ExecuteAccept(IAsyncResult ar)
        {
            Socket cli = m_sock.EndAccept(ar);
            cli.Blocking = false;

            AsyncSocket cliCon = new AsyncSocket(m_watcher);
            cliCon.m_addr = m_addr;
            cliCon.Address.IP = ((IPEndPoint) cli.RemoteEndPoint).Address;
            cliCon.m_sock = cli;
            cliCon.m_state = State.Connected;

            ISocketEventListener l = m_listener.GetListener(cliCon);
            if (l == null)
            {
                // if the listener returns null, close the socket and quit, instead of
                // asserting.
                cli.Close();
                RequestAccept();
                return;
            }

            cliCon.m_listener = l;

            try
            {
                m_watcher.RegisterSocket(cliCon);
            }
            catch (InvalidOperationException)
            {
                // m_watcher out of slots.
                cliCon.AsyncClose();

                // don't set state
                // they really don't need this error, we don't think.
                // Error(e);

                // tell the watcher that when it gets its act together,
                // we'd appreciate it if it would restart the RequestAccept().
                m_watcher.PendingAccept(this);
                return;
            }

            if (l.OnAccept(cliCon))
            {
                RequestAccept();
            }
        }

        /// <summary>
        /// Outbound connection.  Eventually calls Listener.OnConnect() when 
        /// the connection comes up.  Don't forget to call RequestRead() in
        /// OnConnect()!
        /// </summary>
        /// <param name="addr"></param>
        public override void Connect(Address addr)
        {
            m_sock = new Socket(AddressFamily.InterNetwork, 
                                SocketType.Stream, 
                                ProtocolType.Tcp);
            // well, of course this isn't right.
            m_sock.SetSocketOption(SocketOptionLevel.Socket, 
                                   SocketOptionName.ReceiveBuffer, 
                                   4 * m_buf.Length);
            m_sock.Blocking = false;
            m_state = State.Resolving;
            m_watcher.RegisterSocket(this);
            addr.Resolve(new AddressResolved(OnConnectResolved));
        }

        /// <summary>
        /// Address resolution finished.  Try connecting.
        /// </summary>
        /// <param name="addr"></param>
        private void OnConnectResolved(Address addr)
        {
            lock (this)
            {
                if (m_state != State.Resolving)
                {
                    // closed in the mean time.   Probably not an error.
                    return;
                }
                if ((addr == null) || (addr.IP == null))
                {
                    FireError(new AsyncSocketConnectionException("Bad host: " + addr.Hostname));
                }
                else
                {
                    m_addr = addr;
                    m_state = State.Connecting;
                    m_sock.BeginConnect(m_addr.Endpoint, new AsyncCallback(ExecuteConnect), null);
                }
            }
        }
        /// <summary>
        /// Connection complete.
        /// </summary>
        /// <remarks>This is called solely by an async socket thread</remarks>
        /// <param name="ar"></param>
        private void ExecuteConnect(IAsyncResult ar)
        {
            lock (this)
            {
                try
                {
                    m_sock.EndConnect(ar);
                }
                catch (SocketException e)
                {
                    if (m_state != State.Connecting)
                    {
                        // closed in the mean time.   Probably not an error.
                        return;
                    }
                    FireError(e);
                    return;
                }
                if (m_sock.Connected)
                {
                    m_state = State.Connected;
                    m_listener.OnConnect(this);
                }
                else
                {
                    AsyncClose();
                    FireError(new AsyncSocketConnectionException("could not connect"));
                }
            }
        }
        /// <summary>
        /// Start an async read from the socket.  Listener.OnRead() is eventually called
        /// when data arrives.
        /// </summary>
        public override void RequestRead()
        {
            lock (this)
            {
                if (m_reading)
                {
                    throw new InvalidOperationException("Cannot call RequestRead while another read is pending.");
                }
                if (m_state != State.Connected)
                {
                    throw new InvalidOperationException("Socket not connected.");
                }

                m_reading = true;
            }
            try
            {

                m_sock.BeginReceive(m_buf, 0, m_buf.Length, 
                    SocketFlags.None, new AsyncCallback(GotData), null);
            }
            catch (SocketException e)
            {
                Close();

                // TODO: re-learn what these error codes were for.
                // I think they had to do with certain states on 
                // shutdown, and recovering gracefully from those states.
                // 10053 = An established connection was aborted by the 
                //         software in your host machine.
                // 10054 = An existing connection was forcibly closed 
                //         by the remote host.
                if ((e.ErrorCode != 10053) &&
                    (e.ErrorCode != 10054))
                {
                    throw;
                }
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// Some data arrived.
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void GotData(IAsyncResult ar)
        {
            lock (this)
            {
                m_reading = false;
            }

            int count;
            try
            {
                count = m_sock.EndReceive(ar);
            }
            catch (SocketException e)
            {
                AsyncClose();

                // closed in middle of read
                if (e.ErrorCode != 64)
                {
                    FireError(e);
                }
                return;
            }
            catch(ObjectDisposedException)
            {
                //object already disposed, just exit
                return;
            }
            catch (Exception e)
            {
                AsyncClose();
                FireError(e);
                return;
            }
            if (count > 0)
            {
                //byte[] ret = new byte[count];
                //Buffer.BlockCopy(m_buf, 0, ret, 0, count);

                if (m_listener.OnRead(this, m_buf, 0, count))
                {
                    RequestRead();
                }
            }
            else
            {
                AsyncClose();
            }
        }

        /// <summary>
        /// Async write to the socket.  Listener.OnWrite will be called eventually
        /// when the data has been written.  A trimmed copy is made of the data, internally.
        /// </summary>
        /// <param name="buf">Buffer to output</param>
        /// <param name="offset">Offset into buffer</param>
        /// <param name="len">Number of bytes to output</param>
        public override void Write(byte[] buf, int offset, int len)
        {
            lock (this)
            {
                if (m_state != State.Connected)
                {
                    throw new InvalidOperationException("Socket must be connected before writing");
                }

                // make copy, since we might be a while in async-land
                byte[] ret = new byte[len];
                Buffer.BlockCopy(buf, offset, ret, 0, len);
                try
                {
                    m_sock.BeginSend(ret, 0, ret.Length, 
                        SocketFlags.None, new AsyncCallback(WroteData), ret);
                }
                catch (SocketException e)
                {
                    Close();

                    // closed in middle of write
                    if (e.ErrorCode != 10054)
                    {
                        FireError(e);
                    }
                    return;
                }
                catch (Exception e)
                {
                    Close();
                    FireError(e);
                    return;
                }
            }
        }
        
        /// <summary>
        /// Data was written.
        /// </summary>
        /// <param name="ar"></param>
        private void WroteData(IAsyncResult ar)
        {
            int count;
            try
            {
                count = m_sock.EndSend(ar);
            }
            catch (SocketException)
            {
                AsyncClose();
                return;
            }
            catch (ObjectDisposedException)
            {
                AsyncClose();
                return;
            }
            catch (Exception e)
            {
                AsyncClose();
                FireError(e);
                return;
            }
            if (count > 0)
            {
                byte[] buf = (byte[]) ar.AsyncState;
                m_listener.OnWrite(this, buf, 0, buf.Length);
            }
            else
            {
                AsyncClose();
            }
        }

        /// <summary>
        /// Close the socket.  This is NOT async.  .Net doesn't have async closes.  
        /// But, it can be *called* async, particularly from GotData.
        /// Attempts to do a shutdown() first.
        /// </summary>
        public override void Close()
        {
            lock (this)
            {
                /*
                switch (m_state)
                {
                case State.Closed:
                    throw new InvalidOperationException("Socket already closed");
                case State.Closing:
                    throw new InvalidOperationException("Socket already closing");
                }
                */

                State oldState = m_state;

                if (m_sock.Connected)
                {
                    m_state = State.Closing;
                    try
                    {
                        m_sock.Shutdown(SocketShutdown.Both);
                    }
                    catch {}
                    try
                    {
                        m_sock.Close();
                    }
                    catch {}
                }

                if (oldState <= State.Connected)
                    m_listener.OnClose(this);

                m_watcher.CleanupSocket(this);
                m_state = State.Closed;
            }
        }


        /// <summary>
        /// Close, called from async places, so that Errors get fired, appropriately.
        /// </summary>
        protected void AsyncClose()
        {
            try
            {
                Close();
            }
            catch(Exception e)
            {
                FireError(e);
            }
        }

        /// <summary>
        /// Error occurred in the class.  Send to Listener.
        /// </summary>
        /// <param name="e"></param>
        protected void FireError(Exception e)
        {
            lock (this)
            {
                m_state = State.Error;
            }
            if (e is SocketException)
            {
                Debug.WriteLine("Sock errno: " + ((SocketException) e).ErrorCode);
            }
            m_watcher.CleanupSocket(this);
            m_listener.OnError(this, e);
        }

        /// <summary>
        /// In case SocketWatcher wants to use a HashTable.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_id.GetHashCode();
        }

        #region IComparable
        int IComparable.CompareTo(object val)
        {
            if (val == null)
                return 1;

            AsyncSocket sock = val as AsyncSocket;
            if ((object)sock == null)
                throw new ArgumentException("value compared to is not an AsyncSocket", "val");
    
            return this.m_id.CompareTo(sock.m_id);
        }

        /// <summary>
        /// IComparable's need to implement Equals().  This checks the guid's for each socket to see 
        /// if they are the same.
        /// </summary>
        /// <param name="val">The AsyncSocket to check against.</param>
        /// <returns></returns>
        public override bool Equals(object val)
        {
            AsyncSocket sock = val as AsyncSocket;
            if (sock == null)
                return false;
            return (this.m_id == sock.m_id);
        }

        /// <summary>
        /// IComparable's need to implement ==.  Checks for guid equality.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator==(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return ((object)two == null);
            if ((object)two == null)
                return false;

            return (one.m_id == two.m_id);
        }

        /// <summary>
        /// IComparable's need to implement comparison operators.  Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator!=(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return ((object)two != null);
            if ((object)two == null)
                return true;

            return (one.m_id != two.m_id);
        }

        /// <summary>
        /// IComparable's need to implement comparison operators.  Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator<(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
            {
                return ((object)two != null);
            }
            return (((IComparable)one).CompareTo(two) < 0);
        }
        /// <summary>
        /// IComparable's need to implement comparison operators.  Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator<=(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return true;

            return (((IComparable)one).CompareTo(two) <= 0);
        }
        /// <summary>
        /// IComparable's need to implement comparison operators.  Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator>(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return false;
            return (((IComparable)one).CompareTo(two) > 0);
        }
        /// <summary>
        /// IComparable's need to implement comparison operators.  Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator>=(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
            {
                return (two == null);
            }
            return (((IComparable)one).CompareTo(two) >= 0);
        }

        #endregion

		/// <summary>
		/// Retrieve the socketwatcher used by this instance of AsyncSocket
		/// </summary>
		public SocketWatcher SocketWatcher
		{
			get
			{
				return m_watcher;
			}
		}
    }
}
