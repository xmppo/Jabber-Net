/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to Dave Smith (dizzyd) for the design work.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;

using bedrock.io;
using bedrock.util;
using jabber.protocol;

namespace jabber.protocol
{
    /// <summary>
    /// A packet was received.  The specified element will likely be a sub-class
    /// of XmlElement, if the packet is found in the packet factory.
    /// </summary>
    public delegate void ProtocolHandler(Object sender, System.Xml.XmlElement rp);

    /// <summary>
    /// Async XML parsing, according to jabber protocol rules of "interesting". 
    /// The root node fires IElementStreamListener.OnDocumentStart(), and each
    /// direct child of the root fires IElementStreamListener.OnTag().
    /// </summary>
    [RCS(@"$Header$")]
    public class ElementStream
    {
        private object             m_parseLock = new object();
        private PipeStream         m_stream;
        private XmlDocument        m_doc;
        private XmlReader          m_reader;
        private XmlLoader          m_loader;

        /// <summary>
        /// The document started.  This will have a full element, even
        /// though only the start tag has been received.
        /// </summary>
        public event ProtocolHandler       OnDocumentStart;

        /// <summary>
        /// The document has completed.  
        /// TODO: This isn't fired as often as it needs to be, yet.
        /// </summary>
        public event bedrock.ObjectHandler OnDocumentEnd;

        /// <summary>
        /// A protocol element (child of the doc root) has been received.
        /// </summary>
        public event ProtocolHandler       OnElement;

        /// <summary>
        /// Create a parser that will report events to the listener.  
        /// </summary>
        public ElementStream()
        {
            m_doc    = new XmlDocument();
            m_stream = new PipeStream(true);
            m_reader = new XmlTextReader(m_stream);
            m_loader = new XmlLoader(m_reader, m_doc);
            m_loader.Factory.AddType(new jabber.protocol.stream.Factory());
        }

        /// <summary>
        /// The document being read into.  This document is used for creating nodes, 
        /// but does not actually contain the nodes.
        /// </summary>
        public XmlDocument Document
        {
            get { return m_doc; }
        }

        /// <summary>
        /// Add PacketFactories to get XmlElements with type-safe accessors, for
        /// all of the namespaces you care about.
        /// </summary>
        /// <param name="pf"></param>
        public void AddFactory(IPacketTypes pf)
        {
            m_loader.Factory.AddType(pf);
        }

		/// <summary>
		/// Add a type to the packet factory.
		/// </summary>
		/// <param name="localName">Local Name (e.g. query)</param>
		/// <param name="ns">Namespace URI (e.g. jabber:iq:roster)</param>
		/// <param name="t">Type to create</param>
		public void AddType(string localName, string ns, Type t)
		{
			m_loader.Factory.AddType(localName, ns, t);
		}

        /// <summary>
        /// Put bytes into the parser.
        /// If in synchronous mode, after storing a copy of the given buffer, 
        /// the parser is called in this thread.  Otherwise, an async wakeup
        /// is given to the parser.
        /// </summary>
        /// <param name="buf">The bytes to put into the parse stream</param>
        public void Push(byte[] buf)
        {
            Debug.Assert(m_stream != null);
            if (buf.Length > 0)
            {
                m_stream.Write(buf);
                
                // heh.  I was thinking about doing this for scale,
                // but it also has the nice side-effect of getting
                // around any potential dead-locks here.
                ThreadPool.QueueUserWorkItem(new WaitCallback(Parse));
            }
        }

        /// <summary>
        /// Put bytes into the parser.
        /// If in synchronous mode, after storing a copy of the given buffer, 
        /// the parser is called in this thread.  Otherwise, an async wakeup
        /// is given to the parser.
        /// </summary>
        /// <param name="buf">The bytes to put into the parse stream</param>
        /// <param name="offset">Offset into buf to start at</param>
        /// <param name="length">Number of bytes to write</param>
        public void Push(byte[] buf, int offset, int length)
        {
            Debug.Assert(m_stream != null);
            if (length > 0)
            {
                m_stream.Write(buf, offset, length);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Parse));
            }
        }

        /// <summary>
        /// Read one element from the pipe stream.  This may block if a partial
        /// protocol element has come in, but should pick back up when 
        /// HandleReceive is called again.
        /// </summary>
        private void Parse(object ignored)
        {
            lock (m_parseLock)
            {
                try
                {
                    while (!m_stream.IsEmpty)
                    {
                        // skip extraneous nonsense.  For the
                        // stream:stream tag, we read the whole start
                        // tag.  For other protocol elements, we read
                        // the entire tag, including children, in one
                        // gulp.  quit (don't block) if we get to the
                        // end of the nonsense between protocol tags.
                        m_stream.AutoClose = true;
                        bool more;
                        while (more = m_reader.Read())
                        {
                            if (m_reader.NodeType == XmlNodeType.Element)
                                break;
                        }
                        if (!more)
                        {
                            return;
                        }
                        // block when we're in a tag we care about.
                        m_stream.AutoClose = false;
                        switch (m_reader.Depth)
                        {
                        case 0:  // stream:stream
                            XmlElement stream = m_loader.ReadStartTag();
                            if (OnDocumentStart != null)
                                OnDocumentStart(this, stream);
                            break;
                        case 1:  // protocol element
                            XmlElement tag = (XmlElement) m_loader.ReadCurrentNode();
                            if (OnElement != null)
                                OnElement(this, tag);
                            break;
                        default:
                            throw new InvalidOperationException("Protocol de-synchronized: " + m_reader.Name);
                        }
                        //TODO: detect the end of document, and call OnDocumentEnd();
                    }
                }
                catch(XmlException e)
                {
                    Tracer.Trace(TraceLevel.Verbose, e.ToString());
                    if (OnDocumentEnd != null)
                        OnDocumentEnd(this);
                    m_stream.Close();
                    return;
                }
            }
        }
        
    }
}
