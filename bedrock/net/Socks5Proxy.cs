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

namespace bedrock.net
{
	/// <summary>
	/// Proxy object for sockets that want to do SOCKS proxying.
	/// </summary>
	public class Socks5Proxy : BaseSocket, ISocketEventListener
	{
        private BaseSocket m_sock = null;
        private ISocketEventListener m_chain = null;

        private string         m_host = null;
        private int            m_port = 1080;
        private string         m_username = null;
        private string         m_password = null;

        /// <summary>
        /// Wrap an existing socket event listener with SOCKS.  Make SURE to set Socket after this.
        /// </summary>
        /// <param name="chain">Event listener to pass events through to.</param>
		public Socks5Proxy(ISocketEventListener chain) : base(chain)
		{
		}

        /// <summary>
        /// The lower level socket
        /// </summary>
        public BaseSocket Socket
        {
            get { return m_sock; }
            set { m_sock = value; }
        }

        /// <summary>
        /// the host running the proxy
        /// </summary>
        public string Host
        {
            get { return m_host; }
            set { m_host = value; }
        }

        /// <summary>
        /// the port to talk to the proxy host on
        /// </summary>
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        /// <summary>
        /// the auth username for the socks5 proxy
        /// </summary>
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        /// <summary>
        /// the auth password for the socks5 proxy
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        /// <summary>
        /// Prepare to start accepting inbound requests.  Call RequestAccept() to start the async process.
        /// </summary>
        /// <param name="addr">Address to listen on</param>
        /// <param name="backlog">The Maximum length of the queue of pending connections</param>
        public override void Accept(bedrock.net.Address addr, int backlog)
        {
            m_sock.Accept(addr, backlog);
        }

        /// <summary>
        /// Close the socket.  This is NOT async.  .Net doesn't have async closes.  
        /// But, it can be *called* async, particularly from GotData.
        /// Attempts to do a shutdown() first.
        /// </summary>
        public override void Close()
        {
            m_sock.Close();
        }

        /// <summary>
        /// Outbound connection.  Eventually calls Listener.OnConnect() when 
        /// the connection comes up.  Don't forget to call RequestRead() in
        /// OnConnect()!
        /// </summary>
        /// <param name="addr"></param>
        public override void Connect(bedrock.net.Address addr)
        {
            m_sock.Connect(addr);
        }

        /// <summary>
        /// Start the flow of async accepts.  Flow will continue while 
        /// Listener.OnAccept() returns true.  Otherwise, call RequestAccept() again
        /// to continue.
        /// </summary>
        public override void RequestAccept()
        {
            m_sock.RequestAccept();
        }

        /// <summary>
        /// Start an async read from the socket.  Listener.OnRead() is eventually called
        /// when data arrives.
        /// </summary>
        public override void RequestRead()
        {
            m_sock.RequestRead();
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
            m_sock.Write(buf, offset, len);
        }

        #region Implementation of ISocketEventListener
        void ISocketEventListener.OnInit(bedrock.net.AsyncSocket newSock)
        {
            m_chain.OnInit(newSock);
        }

        bedrock.net.ISocketEventListener ISocketEventListener.GetListener(bedrock.net.AsyncSocket newSock)
        {
            return m_chain.GetListener(newSock);
        }

        bool ISocketEventListener.OnAccept(bedrock.net.AsyncSocket newsocket)
        {
            return m_chain.OnAccept(newsocket);
        }

        void ISocketEventListener.OnConnect(bedrock.net.AsyncSocket sock)
        {
            m_chain.OnConnect(sock);
        }

        void ISocketEventListener.OnClose(bedrock.net.AsyncSocket sock)
        {
            m_chain.OnClose(sock);
        }

        void ISocketEventListener.OnError(bedrock.net.AsyncSocket sock, System.Exception ex)
        {
            m_chain.OnError(sock, ex);
        }

        bool ISocketEventListener.OnRead(bedrock.net.AsyncSocket sock, byte[] buf, int offset, int length)
        {
            return m_chain.OnRead(sock, buf, offset, length);
        }

        void ISocketEventListener.OnWrite(bedrock.net.AsyncSocket sock, byte[] buf, int offset, int length)
        {
            m_chain.OnWrite(sock, buf, offset, length);
        }
        #endregion
	}
}
