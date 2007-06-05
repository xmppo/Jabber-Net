/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
#if NET20
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
#endif

namespace test.bedrock.net
{
#if !NO_SSL
    /// <summary>
    ///  Not really async.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class SSLAsyncSocketTest : ISocketEventListener
    {
        private static System.Text.Encoding ENC = System.Text.Encoding.ASCII;

        private static byte[] sbuf = ENC.GetBytes("01234567890123456789012345678901234567890123456789012345678901234567890123456789");
        private object done = new object();
        private object start = new object();
        private string success = null;
        private AsyncSocket m_listen;
        private AsyncSocket m_cli;
        Address a = new Address("localhost", 7003);

        [Test] public void Test_Write()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            bool old_ok = AsyncSocket.UntrustedRootOK;
            AsyncSocket.UntrustedRootOK = true;
            a.Resolve();

            new Thread(new ThreadStart(Server)).Start();
            lock(start)
            {
                Monitor.Wait(start);
            }

            new Thread(new ThreadStart(Client)).Start();

            lock(done)
            {
                Monitor.Wait(done);
            }

            m_listen.Close();
            Assert.AreEqual("5678901234", success);
            AsyncSocket.UntrustedRootOK = old_ok;
        }

        private void Client()
        {
            SocketWatcher c_w = new SocketWatcher(20);
            c_w.Synchronous = true;
#if NET20
            // Note: must have a client cert in your IE cert store.
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            c_w.LocalCertificate = store.Certificates[0];
#endif
            m_cli = c_w.CreateConnectSocket(this, a, true, "localhost");
        }

        private void Server()
        {
            SocketWatcher s_w = new SocketWatcher(20);

            //s_w.RequireClientCert = true;

#if NET20
            X509Certificate2 c2;
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            X509CertificateCollection cert = store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", true);
            if (cert.Count == 0)
            {
                c2 = new X509Certificate2("../../localhost-cert.p12", "test");
                store.Add(c2);
            }
            else
            {
                c2 = (X509Certificate2) cert[0];
            }
            Assert.IsTrue(c2.HasPrivateKey);
            Assert.IsNotNull(c2.PrivateKey);
            Assert.IsInstanceOfType(typeof(X509Certificate2), c2);

            cert = store.Certificates.Find(X509FindType.FindByThumbprint, c2.GetCertHashString(), false);
            c2 = (X509Certificate2) cert[0];
            Assert.IsInstanceOfType(typeof(X509Certificate2), c2);
            Assert.IsTrue(c2.HasPrivateKey);
            Assert.IsNotNull(c2.PrivateKey);
            store.Close();
            s_w.LocalCertificate = c2;
#else
            s_w.SetCertificateFile("../../localhost-cert.p12", "test");
#endif
            s_w.Synchronous = true;
            m_listen = s_w.CreateListenSocket(this, a, true);
            lock(start)
            {
                Monitor.Pulse(start);
            }

            m_listen.RequestAccept();
        }

        #region Implementation of ISocketEventListener
        public bool OnAccept(BaseSocket newsocket)
        {
#if NET20
            Assert.IsTrue(((AsyncSocket)newsocket).IsMutuallyAuthenticated);
#endif
            newsocket.RequestRead();
            return false;
        }

        public bool OnRead(BaseSocket sock, byte[] buf, int offset, int length)
        {
            success = ENC.GetString(buf, offset, length);
            lock(done)
            {
                Monitor.Pulse(done);
            }
            return false;
        }

        public void OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
        {
            System.Diagnostics.Debug.WriteLine(ENC.GetString(buf, offset, length));
            sock.Close();
        }

        public void OnError(BaseSocket sock, System.Exception ex)
        {

        }

        public void OnConnect(BaseSocket sock)
        {
            sock.Write(sbuf, 5, 10);
        }

        public void OnClose(BaseSocket sock)
        {

        }

        public void OnInit(BaseSocket new_sock)
        {

        }

        public ISocketEventListener GetListener(BaseSocket new_sock)
        {
            return this;
        }
        #endregion

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ExceptionObject.ToString());
        }
    }
#endif
}
