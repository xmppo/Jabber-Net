using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

using jabber.protocol;

namespace jabber.protocol
{
	/// <summary>
	/// Summary description for SynchElementStream.
	/// </summary>
	public class SynchElementStream : ElementStream
	{
        /// <summary>
        /// Create a parser that reads from the input stream synchronously, in a single thread.
        /// </summary>
        /// <param name="input"></param>
        public SynchElementStream(Stream input) : base(input)
        {
        }

        /// <summary>
        /// Start parsing.  WARNING: this blocks until the stream is disconnected or end stream:stream is received.
        /// </summary>
        public void Start()
        {
            try
            {
                // This isn't in the constructor so that this isn't called until there's something ready to parse.  
                // This avoids the deadlock of waiting for the BOM at the start of the input.
                //m_reader = new XmlTextReader(m_stream);
                //m_loader.Reader = m_reader;

                while (m_reader.Read())
                {
                    if (m_reader.NodeType != XmlNodeType.Element)
                        continue;
                    switch (m_reader.Depth)
                    {
                        case 0:  // stream:stream
                            FireOnDocumentStart(m_loader.ReadStartTag());
                            break;
                        case 1:  // protocol element
                            FireOnElement((XmlElement) m_loader.ReadCurrentNode());
                            break;
                        default:
                            throw new InvalidOperationException("Protocol de-synchronized: " + m_reader.Name);
                    }
                    //TODO: detect the end of document, and call OnDocumentEnd();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                FireOnDocumentEnd();
                try
                {
                    m_stream.Close();
                }
                catch (Exception e1)
                {
                    Debug.WriteLine(e1.ToString());
                }
                return;
            }
        }
    }
}
