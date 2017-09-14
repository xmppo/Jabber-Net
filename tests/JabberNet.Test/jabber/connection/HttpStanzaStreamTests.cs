using System;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using JabberNet.bedrock.net;
using JabberNet.jabber.connection;
using JabberNet.jabber.protocol;
using NUnit.Framework;

namespace JabberNet.Test.jabber.connection
{
    [TestFixture]
    public class HttpStanzaStreamTests
    {
        private class StanzaListener : IStanzaEventListener
        {
            public object this[string prop]
            {
                get => null;
                set { }
            }

            public void Connected()
            {
            }

            public void Accepted()
            {
            }

            public void BytesRead(byte[] buf, int offset, int len)
            {
            }

            public void BytesWritten(byte[] buf, int offset, int len)
            {
            }

            public void StreamInit(ElementStream stream)
            {
            }

            public void Errored(Exception e)
            {
            }

            public void Closed()
            {
            }

            public void DocumentStarted(XmlElement elem)
            {
            }

            public void DocumentEnded()
            {
            }

            public void StanzaReceived(XmlElement elem)
            {
            }

            public bool OnInvalidCertificate(BaseSocket sock,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors) => true;
        }

        private class SocketListener : ISocketEventListener
        {
            public void OnInit(BaseSocket newSock)
            {
            }

            public ISocketEventListener GetListener(BaseSocket newSock) => null;

            public bool OnAccept(BaseSocket newsocket) => true;

            public void OnConnect(BaseSocket sock)
            {
            }

            public void OnClose(BaseSocket sock)
            {
            }

            public void OnError(BaseSocket sock, Exception ex)
            {
            }

            public bool OnRead(BaseSocket sock, byte[] buf, int offset, int length) => true;

            public void OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
            {
            }

            public bool OnInvalidCertificate(BaseSocket sock,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors) => true;
        }

        private class TestHttpStanzaStream : HttpStanzaStream
        {
            public TestHttpStanzaStream(IStanzaEventListener listener) : base(listener)
            {
            }

            protected override BaseSocket CreateSocket()
            {
                throw new NotImplementedException();
            }

            public void OnRead(BaseSocket sock, byte[] buf, int offset, int length) =>
                ((ISocketEventListener) this).OnRead(sock, buf, offset, length);

            public void OnError(BaseSocket sock, Exception ex) => ((ISocketEventListener) this).OnError(sock, ex);
        }

        [Test]
        public void OnReadCouldBeCalledAfterOnError()
        {
            var stream = new TestHttpStanzaStream(new StanzaListener());
            stream.OnError(new HttpSocket(new SocketListener()), new Exception("test error"));
            stream.OnRead(new HttpSocket(new SocketListener()), new byte[0], 0, 0);
        }
    }
}
