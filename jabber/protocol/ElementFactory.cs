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
using System.Diagnostics;
using System.Reflection;
using System.Xml;

using bedrock.util;

namespace jabber.protocol
{
    /// <summary>
    /// Qname to type mapping.
    /// </summary>
    [RCS(@"$Header$")]
    public class QnameType
    {
        /// <summary>
        /// Element name
        /// </summary>
        protected internal string Name;
        /// <summary>
        /// Element namespace URI
        /// </summary>
        protected internal string NS;
        /// <summary>
        /// Type to create for NS/Name pair
        /// </summary>
        protected internal Type  ElementType;

        /// <summary>
        /// Create a QnameType
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <param name="typ"></param>
        public QnameType(string name, string ns, Type typ)
        {
            this.Name  = name;
            this.NS    = ns;
            this.ElementType = typ;
        }
    }

    /// <summary>
    /// Interface for packet factories to implement.
    /// </summary>
    [RCS(@"$Header$")]
    public interface IPacketTypes
    {
        /// <summary>
        /// QName to type mappings.
        /// </summary>
        QnameType[] Types { get; }
    }

    /// <summary>
    /// A ElementFactory is a class that knows how to create packet instances of
    /// a wide variety of different types.
    /// </summary>
    [RCS(@"$Header$")]
    public class ElementFactory 
    {
        private Hashtable m_types = new Hashtable();
        private static readonly Type[] s_constructorTypes = 
            new Type[] { typeof(string), 
                           typeof(XmlQualifiedName), 
                           typeof(XmlDocument) };
        /// <summary>
        /// Add a type to the packet factory.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="ns"></param>
        /// <param name="t"></param>
        public void AddType(string localName, string ns, Type t)
        {
            Debug.Assert(t.IsSubclassOf(typeof(Element)));
            ConstructorInfo ci = t.GetConstructor(s_constructorTypes);
            Debug.Assert(ci != null);
            m_types.Add(new XmlQualifiedName(localName, ns), ci);
        }
        /// <summary>
        /// Add a type to the packet factory.
        /// </summary>
        /// <param name="qname"></param>
        /// <param name="t"></param>
        public void AddType(XmlQualifiedName qname, Type t)
        {
            Debug.Assert(t.IsSubclassOf(typeof(Element)));
            ConstructorInfo ci = t.GetConstructor(s_constructorTypes);
            Debug.Assert(ci != null);
            m_types.Add(qname, ci);
        }
        /// <summary>
        /// Add a type to the packet factory.
        /// </summary>
        /// <param name="qname"></param>
        /// <param name="ci"></param>
        public void AddType(XmlQualifiedName qname, ConstructorInfo ci)
        {
            Debug.Assert(ci != null);
            m_types.Add(qname, ci);
        }
        /// <summary>
        /// Add a type to the packet factory.
        /// </summary>
        /// <param name="list"></param>
        public void AddType(IPacketTypes list)
        {
            foreach (QnameType qn in list.Types)
            {
                this.AddType(qn.Name, qn.NS, qn.ElementType);
            }
        }
        /*
        public void AddType(ElementFactory pf)
        {
            foreach (DictionaryEntry ent in (IDictionary)pf.m_types)
            {
                m_types.Add(ent.Key, ent.Value);
            }
        }
*/
        /// <summary>
        /// Create an element of the appropriate type, based on the qname of the packet.
        /// </summary>
        /// <param name="prefix">The namespace prefix for the element</param>
        /// <param name="qname">The namespaceURI/element name pair</param>
        /// <param name="doc">The document to create the element in.</param>
        /// <returns></returns>
        public Element GetElement(string prefix, XmlQualifiedName qname, XmlDocument doc)
        {
            ConstructorInfo ci = (ConstructorInfo) m_types[qname];
            if (ci == null)
            {
                return new Element(prefix, qname, doc);
            }
            return (Element) ci.Invoke
                (new object[] {prefix, qname, doc});
        }

        /// <summary>
        /// Get a constructor for the appropriate type for the given qname.
        /// </summary>
        public ConstructorInfo this[XmlQualifiedName qname]
        {
            get { return (ConstructorInfo) m_types[qname]; }
        }
    }
}
