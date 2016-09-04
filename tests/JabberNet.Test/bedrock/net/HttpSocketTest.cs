using System;
using System.Text;
using System.Threading;
using bedrock.net;
using bedrock.util;
using NUnit.Framework;

namespace JabberNet.Test.bedrock.net
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
                    sock.Write(
                        ENC.GetBytes(
                            "HTTP/1.1 200 OK\r\n" +
                            "Content-Length: 10\r\n" +
                            "Content-Type: text/plain\r\n\r\n" +
                            "1234567890"));
                }
                else if (str.Contains("22222"))
                {
                    sock.Write(
                        ENC.GetBytes(
                            "HTTP/1.1 200 OK\r\n" +
                            "Content-Length: 10\r\n" +
                            "Content-Type: text/plain\r\n\r\n" +
                            "12345"));
                    sock.Write(ENC.GetBytes("67890"));
                }
                else if (str.Contains("33333"))
                {
                    sock.Write(
                        ENC.GetBytes(
                            "HTTP/1.1 200 OK\r\n" +
                            "Content-Length: 20\r\n" +
                            "Content-Type: text/plain\r\n\r\n" +
                            "12345"));
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
            using (var watcher = new SocketWatcher())
            {
                var a = new Address("127.0.0.1", 7002);
                a.Resolve();

                var server = new ServerListener();
                var serverSock = watcher.CreateListenSocket(server, a);
                serverSock.RequestAccept();

                var resp = new ResponseListener();
                var sock = new HttpSocket(resp);

                var u = new Uri("http://127.0.0.1:7002/");
                var buf = ENC.GetBytes("11111");
                var s = (HttpSocket)sock;
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
}
