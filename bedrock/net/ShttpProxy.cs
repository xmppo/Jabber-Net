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

namespace bedrock.net
{
	/// <summary>
	/// Proxy object for sockets that want to do SHHTP proxying.
	/// </summary>
	public class ShttpProxy : ProxySocket
	{
		private enum States { None, Connecting, WaitingForAuth, Running, Closed }
		private States m_state = States.None;

		/// <summary>
		/// Wrap an existing socket event listener with a ShttpProxy proxy.  Make SURE to set Socket after this.
		/// </summary>
		/// <param name="chain">Event listener to pass events through to.</param>
		public ShttpProxy(ISocketEventListener chain) : base(chain)
		{
		}

		/// <summary>
		/// Remember that we're in the connecting state, let base connect to proxy, resumes in OnConnect.
		/// </summary>
		/// <param name="addr"></param>
		public override void Connect(bedrock.net.Address addr)
		{
			m_state = States.Connecting;
			base.Connect(addr);
		}

		#region Implementation of ISocketEventListener

		/// <summary>
		/// overridden OnConnect to start off SHTTP protocol.
		/// </summary>
		/// <param name="sock"></param>
		public override void OnConnect(bedrock.net.AsyncSocket sock)
		{
			if (m_state == States.Connecting)
			{ // CONNECT users.instapix.com:5222 HTTP/1.0
				m_state = States.WaitingForAuth;
				string cmd = "CONNECT " + RemoteAddress.Hostname + ":" + RemoteAddress.Port + " HTTP/1.0\r\n";
				// if authinfo is set, send it.
				if (Username != null && Username.Length > 0 && Password != null && Password.Length > 0) 
				{
					string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password));
					cmd += "Proxy-Authorization: Basic " + auth + "\r\n";
				}
				cmd += "\r\n";
				Debug.WriteLine("sending command to proxy...");
				Write(Encoding.ASCII.GetBytes(cmd));
				RequestRead();
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
				case States.WaitingForAuth:
					m_state = States.Running;
					string reply = Encoding.ASCII.GetString(buf, offset, length);
					Debug.WriteLine("proxy returned : " + reply);
					m_listener.OnConnect(sock);	// tell the real listener that we're connected.
					// they'll call RequestRead(), so we can return false here.
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
