using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using bedrock.net;
using bedrock.util;
using System.Threading;


namespace test.bedrock.net
{
    [SVN(@"$Id$")]
    [TestFixture]
    public class HttpSocketTest
    {
        private static readonly Encoding ENC = Encoding.UTF8;

        private class ServerListener: ISocketEventListener
        {
            #region ISocketEventListener Members

            public void OnInit(BaseSocket newSock)
            {
            }

            public ISocketEventListener GetListener(BaseSocket newSock)
            {
                return this;
            }

            public bool OnAccept(BaseSocket newsocket)
            {
                AsyncSocket s = (AsyncSocket)newsocket;
                newsocket.RequestRead();
                return true;
            }

            public void OnConnect(BaseSocket sock)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public void OnClose(BaseSocket sock)
            {
                
            }

            public void OnError(BaseSocket sock, Exception ex)
            {
                throw ex;
            }

            public bool OnRead(BaseSocket sock, byte[] buf, int offset, int length)
            {
                string str = ENC.GetString(buf, offset, length);
                Console.WriteLine("SR: " + str);
                if (str.Contains("11111"))
                {
                sock.Write(ENC.GetBytes(@"HTTP/1.1 200 OK
Content-Length: 10
Content-Type: text/plain

1234567890"));
                }
                else if (str.Contains("22222"))
                {
                    sock.Write(ENC.GetBytes(@"HTTP/1.1 200 OK
Content-Length: 10
Content-Type: text/plain

12345"));
                    sock.Write(ENC.GetBytes("67890"));
                }
                else if (str.Contains("33333"))
                {
                    sock.Write(ENC.GetBytes(@"HTTP/1.1 200 OK
Content-Length: 20
Content-Type: text/plain

12345"));
                    // Turning off Nagle didn't fix this.  Mrmph.
                    Thread.Sleep(300);
                    sock.Write(ENC.GetBytes("67890"));
                    Thread.Sleep(300);
                    sock.Write(ENC.GetBytes("12345"));
                    Thread.Sleep(300);
                    sock.Write(ENC.GetBytes("67890"));
                }
                return true;
            }

            public void OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
            {
                Console.WriteLine("SW: " + ENC.GetString(buf, offset, length));
            }

            public bool OnInvalidCertificate(BaseSocket sock, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            #endregion
        }

        private class ResponseListener: ISocketEventListener
        {
            public string Last = null;
            public AutoResetEvent Event = new AutoResetEvent(false);


            #region ISocketEventListener Members

            public void OnInit(BaseSocket newSock)
            {
            }

            public ISocketEventListener GetListener(BaseSocket newSock)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public bool OnAccept(BaseSocket newsocket)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public void OnConnect(BaseSocket sock)
            {
            }

            public void OnClose(BaseSocket sock)
            {
            }

            public void OnError(BaseSocket sock, Exception ex)
            {
                throw ex;
            }

            public bool OnRead(BaseSocket sock, byte[] buf, int offset, int length)
            {
                Last = ENC.GetString(buf, offset, length);
                Console.WriteLine("RR: " + Last);
                Event.Set();
                return true;
            }

            public void OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
            {
                Console.WriteLine("RW: " + ENC.GetString(buf, offset, length));
            }

            public bool OnInvalidCertificate(BaseSocket sock, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            #endregion
        }

        [Test]
        public void Test_Full_Response()
        {
            SocketWatcher watcher = new SocketWatcher();
            Address a = new Address("127.0.0.1", 7002);
            a.Resolve();

            ServerListener server = new ServerListener();
            AsyncSocket server_sock = watcher.CreateListenSocket(server, a);
            server_sock.RequestAccept();

            ResponseListener resp = new ResponseListener();
            HttpSocket sock = new HttpSocket(resp);

            Uri u = new Uri("http://localhost:7002/");
            byte[] buf = ENC.GetBytes("11111");
            HttpSocket s = (HttpSocket)sock;
            s.Execute("GET", u, buf, 0, buf.Length, "text/plain");
            resp.Event.WaitOne();
            Assert.AreEqual("1234567890", resp.Last);

            resp.Last = null;
            buf = ENC.GetBytes("22222");
            s.Execute("GET", u, buf, 0, buf.Length, "text/plain");
            resp.Event.WaitOne();
            Assert.AreEqual("1234567890", resp.Last);

            resp.Last = null;
            buf = ENC.GetBytes("33333");
            s.Execute("GET", u, buf, 0, buf.Length, "text/plain");
            resp.Event.WaitOne();
            Assert.AreEqual("12345678901234567890", resp.Last);
        }
    }
}
