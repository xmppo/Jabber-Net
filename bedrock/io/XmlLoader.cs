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
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Xml;

using bedrock.util;
using bedrock.io;
using jabber.protocol;

namespace bedrock.io
{
    /// <summary>
    /// Reverse-engineered from the Beta1 System.Xml.XmlLoader, since that's private 
    /// and the version accessible from System.Xml.XmlDocument.ReadNode()
    /// is wrapped in a call to set and clear IsLoading, which makes it 
    /// non-reentrant.  At least that's what I think is happening.  The 
    /// symptom is that ReadNode() doesn't return the last element until 
    /// the next element comes in.
    /// 
    /// I left some stuff unimplemented; mostly default attributes from a DTD.  
    /// Those classes are private, too in System.Xml.  Thanks, guys.  Oh, and 
    /// entity references.  I don't believe in them, therefore they must not exist.
    /// 
    /// I've now made enough changes to this implementation that it probably will
    /// have to stay around.  Someone should probably re-write it without the 
    /// reverse-engineering.
    /// </summary>
    [RCS(@"$Header$")]
    public class XmlLoader
    {
        private XmlReader     reader;
        private XmlDocument   doc;
        private bool          PreserveWhitespace = true;
        private XmlLoaderMode loadMode           = XmlLoaderMode.Regular;
        private ElementFactory m_factory          = null;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="r">The reader to read from</param>
        /// <param name="d">The XmlDocument to create nodes in.  The nodes are 
        /// not added to the document root, though</param>
        public XmlLoader(XmlReader r, XmlDocument d)
        {
            reader = r;
            doc    = d;
        }

        /// <summary>
        /// The packet factory for this loader.
        /// </summary>
        public ElementFactory Factory
        {
            get 
            { 
                if (m_factory == null)
                    m_factory = new ElementFactory();
                return m_factory; 
            }
            set { m_factory = value; }
        }

        /// <summary>
        /// Read just a start tag, along with that tag's attributes.
        /// This is useful for &lt;stream:stream&gt;
        /// </summary>
        /// <returns></returns>
        public XmlElement ReadStartTag()
        {
            XmlElement tag = null;
            XmlQualifiedName qname  = new XmlQualifiedName(reader.LocalName, 
                                                           reader.NamespaceURI);
            tag = m_factory.GetElement(reader.Prefix, qname, doc);
            ReadAttributes(tag);
            return tag;
        }

        /// <summary>
        /// Read the current node fully, including attributes and sub-elements.
        /// </summary>
        /// <returns></returns>
        public XmlNode ReadCurrentNode()
        {
            XmlNode                 lastNode = null;
            XmlAttribute            attr;
            XmlEntityReference      eRef;
            String                  version;
            String                  encoding;
            String                  standalone;
            XmlDocumentType         V_11;
            XmlNodeType             nodeType = reader.NodeType;
            switch (nodeType)
            {
            case XmlNodeType.Element:
                XmlElement currentElem = null;
                XmlQualifiedName qname = new XmlQualifiedName(reader.LocalName, 
                                                              reader.NamespaceURI);
                currentElem = m_factory.GetElement(reader.Prefix, qname, doc);
                bool isEmpty = currentElem.IsEmpty = reader.IsEmptyElement;
                ReadAttributes(currentElem);
                
                if (! isEmpty)
                {
                    LoadChildren(currentElem);
                }
                lastNode = currentElem;
                break;
                
            case XmlNodeType.Attribute:
                if (reader.IsDefault)
                {
                    throw new NotImplementedException();
                    /* // XmlUnspecifiedAttribute is private.
                    XmlUnspecifiedAttribute uAttr =
                        new XmlUnspecifiedAttribute(reader.Prefix,
                                                    reader.LocalName,
                                                    reader.NamespaceURI,
                                                    doc);
                    this.LoadAttributeChildren(uAttr);
                    uAttr.SetSpecified = false;
                    lastNode = uAttr;
                    */
                }
                else
                {
                    attr = doc.CreateAttribute(reader.Prefix,
                                               reader.LocalName,
                                               reader.NamespaceURI);
                    this.LoadAttributeChildren(attr);
                    lastNode = attr;
                }
                break;
            case XmlNodeType.SignificantWhitespace:
                if (this.PreserveWhitespace)
                {
                    lastNode = doc.CreateSignificantWhitespace(reader.Value);
                }
                else
                {
                    do
                    {
                        if (! reader.Read())
                        {
                            return null;
                        }
                    }
                    while (reader.NodeType == XmlNodeType.SignificantWhitespace);
                    lastNode = this.ReadCurrentNode();
                }
                break;
            case XmlNodeType.Whitespace:
                if (this.PreserveWhitespace)
                {
                    lastNode = doc.CreateWhitespace(reader.Value);
                }
                else
                {
                    do
                    {
                        if (! reader.Read())
                        {
                            return null;
                        }
                    }
                    while (reader.NodeType == XmlNodeType.Whitespace);
                    lastNode = this.ReadCurrentNode();
                }
                break;
            case XmlNodeType.Text:
                lastNode = doc.CreateTextNode(reader.Value);
                break;
                
            case XmlNodeType.CDATA:
                lastNode = doc.CreateCDataSection(reader.Value);
                break;
            case XmlNodeType.EntityReference:
                eRef = doc.CreateEntityReference(reader.Name);
                if ((this.loadMode == XmlLoaderMode.ExpandEnity) || 
                    (this.loadMode == XmlLoaderMode.ExpandEntityReference))
                {
                    this.ExpandEntityReference(eRef);
                }
                else
                {
                    reader.ResolveEntity();
                    this.LoadChildren(eRef);
                }
                lastNode = eRef;
                break;
            case XmlNodeType.XmlDeclaration:
                version = encoding = standalone =null;
                ParseXmlDeclarationValue(reader.Value,
                                         out version,
                                         out encoding,
                                         out standalone);
                lastNode = doc.CreateXmlDeclaration(version, encoding, standalone);
                break;
                
            case XmlNodeType.ProcessingInstruction:
                lastNode = doc.CreateProcessingInstruction(reader.LocalName,
                                                           reader.Value);
                break;
            case XmlNodeType.Comment:
                lastNode = doc.CreateComment(reader.Value);
                break;
            case XmlNodeType.DocumentType:
                V_11 = doc.CreateDocumentType(reader.LocalName,
                                              String.Empty,
                                              String.Empty,
                                              String.Empty);
                V_11.Value = reader.Value;
                this.LoadDocumentType(V_11);
                lastNode = V_11;        
                break;
            case XmlNodeType.EndElement:
            case XmlNodeType.EndEntity:
                break;
            default:
                throw new InvalidOperationException("Unknown node type: " + nodeType);
            }
            return lastNode;
        }
        private void LoadChildren(XmlNode parent)
        {
            XmlNode lastNode = null;
            while (reader.Read() &&
                   ((lastNode = this.ReadCurrentNode()) != null))
            {
                if (parent.NodeType != XmlNodeType.Document)
                {
                    parent.AppendChild(lastNode);
                }                
            }
        }
        private void LoadAttributeChildren(XmlNode parent)
        {
            XmlEntityReference V_0;
            XmlNodeType V_1;
            while (reader.ReadAttributeValue())
            {
                V_1 = reader.NodeType;
                switch (V_1)
                {
                case XmlNodeType.EndEntity:
                    return;
                    
                case XmlNodeType.Text:
                    parent.AppendChild(doc.CreateTextNode(reader.Value));
                    break;
                case XmlNodeType.EntityReference:
                    V_0 = doc.CreateEntityReference(reader.LocalName);
                    if ((loadMode == XmlLoaderMode.ExpandEnity) || 
                        (loadMode == XmlLoaderMode.ExpandEntityReference))
                    {
                        this.ExpandEntityReference(V_0);
                    }
                    else
                    {
                        reader.ResolveEntity();
                        this.LoadAttributeChildren(V_0);
                    }
                    parent.AppendChild(V_0);
                    break;
                    
                case XmlNodeType.CDATA:
                default:
                    throw new InvalidOperationException();
                }
            }
        }
        
        private static void ParseXmlDeclarationValue(String strValue,
                                                     out String version,
                                                     out String encoding,
                                                     out String standalone)
        {
            XmlTextReader r;
            string        n;
            
            version    = null;
            encoding   = null;
            standalone = null;
            r = new XmlTextReader(new System.IO.StringReader("<foo " + strValue + " />"));
            try
            {
                r.Read();
                
                for (int i=0; i<r.AttributeCount; i++)
                {
                    r.MoveToAttribute(i);
                    n = r.Name;
                    if (n != null)
                    {
                        n = String.IsInterned(n);
                        if (n == "version")
                        {
                            if (version != null)
                            {
                                throw new Exception();
                            }
                            version = r.Value;
                        }
                        else if(n == "encoding")
                        {
                            if (encoding != null)
                                throw new Exception();
                            encoding = r.Value;
                        }
                        else if (n == "standalone")
                        {
                            if (standalone != null)
                                throw new Exception();
                            standalone = r.Value;
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                
                if (r.Read())
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
            r.Close();
        }
        private void  LoadDocumentType(XmlDocumentType dtNode)
        {
            throw new NotImplementedException("No DTD support, yet");
        }
        private void ExpandEntityReference(XmlEntityReference eref)
        {
            throw new NotImplementedException("Way too much work for something I don't plan on using");            
        }
        private enum XmlLoaderMode
        {
            Regular = 0,
            ExpandEnity = 1,
            ExpandEntityReference = 2
        }
        private void ReadAttributes(XmlElement parent)
        {
            XmlAttribute currentAttr;
            bool moreAttributes = reader.MoveToFirstAttribute();
            while (moreAttributes)
            {
                currentAttr = (XmlAttribute) this.ReadCurrentNode();
                parent.SetAttributeNode(currentAttr);
                moreAttributes = reader.MoveToNextAttribute();
            }
        }
    }
}
