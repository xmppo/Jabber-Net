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

using System.Threading;
using NUnit.Framework;
using bedrock.net;
using bedrock.util;
using System.Security.Cryptography.X509Certificates;

namespace test.bedrock.net
{
    /// <summary>
    ///  Not really async.
    /// </summary>
    [SVN(@"$Id$")]
    [Ignore("Fails due to certificate.")]
    [TestFixture]
    public class SSLAsyncSocketTest : ISocketEventListener
    {
        private static readonly System.Text.Encoding ENC = System.Text.Encoding.ASCII;

        private static readonly byte[] sbuf = ENC.GetBytes("01234567890123456789012345678901234567890123456789012345678901234567890123456789");
        private readonly object done = new object();
        private readonly object start = new object();
        private string success = null;
        private AsyncSocket m_listen;
        readonly Address a = new Address("localhost", 7003);

        private bool succeeded = true;
        private string errorMessage;

        [Test] public void Test_Write()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            a.Resolve();

            lock(start)
            {
                new Thread(Server).Start();
                Monitor.Wait(start);
            }

            try
            {
                lock (done)
                {
                    new Thread(Client).Start();
                    Monitor.Wait(done);

                    Assert.IsTrue(succeeded, errorMessage);
                }
            }
            finally
            {
                m_listen.Close();
            }

            Assert.AreEqual("5678901234", success);
        }

        private void Client()
        {

            SocketWatcher c_w = new SocketWatcher(20);
            c_w.Synchronous = true;

            // Note: must have a client cert in your IE cert store.
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            if (store.Certificates.Count > 0)
            {
                c_w.LocalCertificate = store.Certificates[0];
            }
            else
            {
                lock (done)
                {
                    errorMessage = "There were no certificates in the Windows Certificate Store.";
                    succeeded = false;
                    Monitor.Pulse(done);
                }

                return;
            }
            c_w.CreateConnectSocket(this, a, true, "localhost");
        }

        private void Server()
        {
            SocketWatcher s_w = new SocketWatcher(20);

            //s_w.RequireClientCert = true;

            X509Certificate2 c2;
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            X509Certificate2Collection cert = store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", true);
            if (cert.Count == 0)
            {
                c2 = new X509Certificate2("../../localhost-cert.p12", "test");
                store.Add(c2);
            }
            else
            {
                c2 = cert[0];
            }
            Assert.IsTrue(c2.HasPrivateKey);
            Assert.IsNotNull(c2.PrivateKey);
            Assert.AreEqual(typeof(X509Certificate2), c2.GetType());

            cert = store.Certificates.Find(X509FindType.FindByThumbprint, c2.GetCertHashString(), false);
            c2 = cert[0];
            Assert.AreEqual(typeof(X509Certificate2), c2.GetType());
            Assert.IsTrue(c2.HasPrivateKey);
            Assert.IsNotNull(c2.PrivateKey);
            store.Close();
            s_w.LocalCertificate = c2;
            s_w.Synchronous = true;

            m_listen = s_w.CreateListenSocket(this, a, true);
            lock(start)
            {
                Monitor.Pulse(start);
            }

            try
            {
                m_listen.RequestAccept();
            }
            catch (Exception ex)
            {
                lock (done)
                {
                    succeeded = false;
                    errorMessage = ex.Message;
                    Monitor.Pulse(done);
                }
            }
        }

        #region Implementation of ISocketEventListener
        public bool OnAccept(BaseSocket newsocket)
        {
            Assert.IsTrue(((AsyncSocket)newsocket).IsMutuallyAuthenticated);
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

        public void OnError(BaseSocket sock, Exception ex)
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

        public bool OnInvalidCertificate(BaseSocket sock,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ExceptionObject.ToString());
        }
    }
}
