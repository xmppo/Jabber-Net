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
using System.Threading;
using NUnit.Framework;
using bedrock.net;
using bedrock.util;
namespace test.bedrock.net
{
    /// <summary>
    ///    Summary description for AsyncSocketTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class AsyncSocketTest : ISocketEventListener
    {
        private static System.Text.Encoding ENC = System.Text.Encoding.ASCII;

        private static byte[] sbuf = ENC.GetBytes("01234567890123456789012345678901234567890123456789012345678901234567890123456789");
        private object done = new object();
        private string success = null;

        public void Test_Write()
        {
            SocketWatcher w = new SocketWatcher(20);
            Address a = new Address("127.0.0.1", 7001);
            a.Resolve();
            AsyncSocket listen = w.CreateListenSocket(this, a);
            listen.RequestAccept();
            AsyncSocket connect = w.CreateConnectSocket(this, a);
            lock(done)
            {
                Monitor.Wait(done);
            }
            Assertion.AssertEquals("5678901234", success);
        }

        #region Implementation of ISocketEventListener
        public bool OnAccept(AsyncSocket newsocket)
        {
            newsocket.RequestRead();
            return false;
        }

        public bool OnRead(AsyncSocket sock, byte[] buf, int offset, int length)
        {
            success = ENC.GetString(buf, offset, length);
            sock.Close();
            lock(done)
            {
                Monitor.Pulse(done);
            }
            return false;
        }

        public void OnWrite(AsyncSocket sock, byte[] buf, int offset, int length)
        {
            System.Diagnostics.Debug.WriteLine(ENC.GetString(buf, offset, length));
            sock.Close();
        }

        public void OnError(AsyncSocket sock, System.Exception ex)
        {
                
        }

        public void OnConnect(AsyncSocket sock)
        {
            sock.Write(sbuf, 5, 10);
        }

        public void OnClose(AsyncSocket sock)
        {
                
        }

        public void OnInit(AsyncSocket new_sock)
        {
                
        }

        public ISocketEventListener GetListener(AsyncSocket new_sock)
        {
            return this;
        }
        #endregion
    }
}