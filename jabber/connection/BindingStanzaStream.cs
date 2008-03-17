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
	        return new XEP124Socket(this);
	    }

        public override void WriteStartTag(Stream stream)
        {
            // We don't send the <stream:stream> tag in XEP 124.
            XEP124Socket mySock = ((XEP124Socket) Socket);
            mySock.StartStream = true;
            mySock.NS = stream.NS;
        }

        public override void Close(bool clean)
        {
            // We don't send the </stream:stream> tag in XEP 124.
            base.Close(false);
        }
	}
}
