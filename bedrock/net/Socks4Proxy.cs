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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
using System.Text;
using System.Net;

namespace bedrock.net
{
	/// <summary>
	/// Proxy object for sockets that want to do SOCKS4 proxying.
	/// </summary>
	public class Socks4Proxy : ProxySocket
	{
		private enum States { None, Connecting, RequestingProxy, Running, Closed }
		private States m_state = States.None;

		/// <summary>
		/// Wrap an existing socket event listener with a Socks5 proxy.  Make SURE to set Socket after this.
		/// </summary>
		/// <param name="chain">Event listener to pass events through to.</param>
		public Socks4Proxy(ISocketEventListener chain) : base(chain)
		{
		}

		/// <summary>
		/// Saves the address passed in, and really connects to ProxyHost:ProxyPort to begin SOCKS5 handshake.
		/// </summary>
		/// <param name="addr"></param>
		public override void Connect(bedrock.net.Address addr)
		{
			m_state = States.Connecting;
			base.Connect(addr);
		}

		#region Socks4 private methods.

		/*
					+----+----+----+----+----+----+----+----+
					| VN | CD | DSTPORT |      DSTIP        |
					+----+----+----+----+----+----+----+----+
	 # of bytes:	   1    1      2              4

	VN is the version of the reply code and should be 0. CD is the result
	code with one of the following values:

		90: request granted
		91: request rejected or failed
		92: request rejected becasue SOCKS server cannot connect to
			identd on the client
		93: request rejected because the client program and identd
			report different user-ids

	 */
		private bool HandleRequestResponse(int ver, int reply)
		{
			if (ver != 0) 
			{
				Debug.WriteLine("bogus version in reply from proxy: " + ver);
				return false;
			}
			if (reply != 90) 
			{
				Debug.WriteLine("request failed on proxy: " + reply);
				return false;
			}
			Debug.WriteLine("proxy complete");
			m_state = States.Running;
			return true;
		}

		#endregion

		#region Implementation of ISocketEventListener

		/// <summary>
		/// overridden OnConnect to start off Socks5 protocol.
		/// </summary>
		/// <param name="sock"></param>
		public override void OnConnect(bedrock.net.AsyncSocket sock)
		{
			if (m_state == States.Connecting)
			{
				IPHostEntry server = Dns.Resolve(RemoteAddress.Hostname);
				IPAddress ip_addr = server.AddressList[0];
#if !OLD_CLR
				byte[] addr = ip_addr.GetAddressBytes();
#else
                byte[] addr = new byte[4];
                addr[0] = (byte)((ip_addr.Address >> 24) & 0xff);
                addr[1] = (byte)((ip_addr.Address >> 16) & 0xff);
                addr[2] = (byte)((ip_addr.Address >> 8) & 0xff);
                addr[3] = (byte)(ip_addr.Address & 0xff);
#endif
                int port = RemoteAddress.Port;
				byte [] buffer = new Byte[14];
				buffer[0] = 4;	// protocol version.
				buffer[1] = 1;	// connect.
				buffer[2] = (byte)(port >> 8);
				buffer[3] = (byte)port;
                // TODO: test byte order!
				buffer[4] = addr[3];
				buffer[5] = addr[2];
				buffer[6] = addr[1];
				buffer[7] = addr[0];
				buffer[8] = (byte)'i';
				buffer[9] = (byte)'d';
				buffer[10] = (byte)'e';
				buffer[11] = (byte)'n';
				buffer[12] = (byte)'t';
				buffer[13] = 0;

				/*
				+----+----+----+----+----+----+----+----+----+----+....+----+
				| VN | CD | DSTPORT |      DSTIP        | USERID       |NULL|
				+----+----+----+----+----+----+----+----+----+----+....+----+
	# of bytes:	   1    1      2              4           variable       1
				*/


				Write(buffer);
				RequestRead();
				m_state = States.RequestingProxy;
			}
		}

		/// <summary>
		/// Overridden OnRead to handle 4 Socks5 states... 
		/// </summary>
		/// <param name="sock"></param>
		/// <param name="buf"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public override bool OnRead(bedrock.net.AsyncSocket sock, byte[] buf, int offset, int length)
		{
			switch (m_state) 
			{
				case States.RequestingProxy:
					bool ret = HandleRequestResponse(buf[offset], buf[offset + 1]);
					if (ret) 
					{
						m_listener.OnConnect(sock);	// tell the real listener that we're connected.
						// they'll call RequestRead(), so we can return false here.
					}
					return false;
				default:
					return base.OnRead(sock, buf, offset, length);
			}
		}

		/// <summary>
		/// Overridden OnWrite to ensure that the base only gets called when in running state.
		/// </summary>
		/// <param name="sock"></param>
		/// <param name="buf"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		public override void OnWrite(bedrock.net.AsyncSocket sock, byte[] buf, int offset, int length)
		{
			if (m_state == States.Running) 
			{
				base.OnWrite(sock, buf, offset, length);
			}
		}
        #endregion
	}
}
