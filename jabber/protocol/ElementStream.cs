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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
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
        /// <summary>
        /// The stream to read from
        /// </summary>
        protected Stream      m_stream;

        /// <summary>
        /// The document to create elements in
        /// </summary>
        protected XmlDocument m_doc;

        /// <summary>
        /// The actual parsing logic
        /// </summary>
        protected XmlReader   m_reader;

        /// <summary>
        /// The factory that creates DOM elements of the right subclass 
        /// </summary>
        protected XmlLoader   m_loader;

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
        protected ElementStream(Stream stream)
        {
            m_stream = stream;
            m_doc    = new XmlDocument();
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
        /// Fire the OnDocumentStart event
        /// </summary>
        /// <param name="stream"></param>
        protected void FireOnDocumentStart(XmlElement stream)
        {
            if (OnDocumentStart != null)
                OnDocumentStart(this, stream);
        }

        /// <summary>
        /// Fire the OnElement event
        /// </summary>
        /// <param name="elem"></param>
        protected void FireOnElement(XmlElement elem)
        {
            if (OnElement != null)
                OnElement(this, elem);
        }

        /// <summary>
        /// Fire the OnDocumentEnd event
        /// </summary>
        protected void FireOnDocumentEnd()
        {
            if (OnDocumentEnd != null)
                OnDocumentEnd(this);
        }
    }
}
