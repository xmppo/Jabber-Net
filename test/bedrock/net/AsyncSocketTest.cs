/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
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

        public void Test_Ops()
        {
            SocketWatcher w = new SocketWatcher(20);
            Address a = new Address("127.0.0.1", 7001);
            a.Resolve();
            AsyncSocket one = w.CreateListenSocket(this, a);
            AsyncSocket two = null;

            Assertion.Assert(one == one);
            Assertion.Assert(two == two);
            Assertion.Assert(one >= one);
            Assertion.Assert(two >= two);
            Assertion.Assert(one <= one);
            Assertion.Assert(two <= two);
            Assertion.Assert(one != two);
            Assertion.Assert(two != one);
            Assertion.Assert(one > two);
            Assertion.Assert(one >= two);
            Assertion.Assert(two < one);
            Assertion.Assert(two <= one);

            two = w.CreateListenSocket(this, a);
            Assertion.Assert(one == one);
            Assertion.Assert(two == two);
            Assertion.Assert(one >= one);
            Assertion.Assert(two >= two);
            Assertion.Assert(one <= one);
            Assertion.Assert(two <= two);
            Assertion.Assert(one != two);
            Assertion.Assert(two != one);

            int c = ((IComparable)one).CompareTo(two);
            Assertion.Assert(c != 0);
            if (c == -1)
            {
                // one less than two
                Assertion.Assert(one < two);
                Assertion.Assert(one <= two);
                Assertion.Assert(two > one);
                Assertion.Assert(two >= one);
            }
            else if (c == 1)
            {
                // one greater than two
                Assertion.Assert(one > two);
                Assertion.Assert(one >= two);
                Assertion.Assert(two < one);
                Assertion.Assert(two <= one);
            }
            else
            {
                Assertion.Assert(false);
            }
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