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

using System.Diagnostics;
using System.IO;
using System.Xml;

using jabber.protocol;
using bedrock.util;

namespace jabber.protocol
{
	/// <summary>
	/// Summary description for SynchElementStream.
	/// </summary>
    [RCS(@"$Header$")]
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
//            Debug.WriteLine("--Starting new Synch parser--");
            XmlElement elem;
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
                            elem = m_loader.ReadStartTag();
//                            Debug.WriteLine("SynchParser start: " + elem.OuterXml);
                            FireOnDocumentStart(elem);
                            break;
                        case 1:  // protocol element
                            elem = (XmlElement) m_loader.ReadCurrentNode();
//                            Debug.WriteLine("SynchParser elem: " + elem.OuterXml);
                            FireOnElement(elem);
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
