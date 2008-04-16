using bedrock.net;
using jabber.protocol.stream;

namespace jabber.connection
{
	class BindingStanzaStream : HttpStanzaStream
	{
	    public BindingStanzaStream(IStanzaEventListener listener) : base(listener)
	    {
	    }

	    protected override BaseSocket CreateSocket()
	    {
	        XEP124Socket sock = new XEP124Socket(this);
            ProxyType pt = (ProxyType)m_listener[Options.PROXY_TYPE];
            if (pt == ProxyType.HTTP)
            {
                string host = m_listener[Options.PROXY_HOST] as string;
                int port = (int)m_listener[Options.PROXY_PORT];
                if (port == -1)
                    port = 80;
                string proxy_uri = string.Format("http://{0}:{1}/", host, port);
                sock.ProxyURI = new System.Uri(proxy_uri);
                string user = m_listener[Options.PROXY_USER] as string;
                if (user != null)
                {
                    sock.ProxyCredentials = new System.Net.NetworkCredential(user,
                        m_listener[Options.PROXY_PW] as string);
                }
            }
            return sock;
	    }

        public override void WriteStartTag(Stream stream)
        {
            // We don't send the <stream:stream> tag in XEP 124.
            XEP124Socket mySock = ((XEP124Socket) Socket);
            mySock.NS = stream.NS;
            mySock.Write(null, 0, 0);
        }

        public override void Close(bool clean)
        {
            // We don't send the </stream:stream> tag in XEP 124.
            base.Close(false);
        }
	}
}
