namespace bedrock.net
{
	interface IHttpSocket
	{
	    string URL { get; set; }
	}

    /// <summary>
    /// This socket has special support for writing XML elements.
    /// </summary>
    public interface IElementSocket
    {
        /// <summary>
        /// Write an XML element to the socket.
        /// </summary>
        /// <param name="elem"></param>
        void Write(System.Xml.XmlElement elem);
    }
}
