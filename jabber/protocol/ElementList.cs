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
 * Portions Copyright (c) 2003 Joe Hildebrand.
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

namespace jabber.protocol
{
	/// <summary>
	/// Replacement for XmlElementList that removes the safety belt of checking for changes during traversal, 
	/// but removes the big old memory leak in MS's implementation.  Also, only returns first-level children, 
	/// rather than all children below here with the given name.  Thanks, MS.
	/// </summary>
	public class ElementList : XmlNodeList
	{
        private XmlElement m_parent = null;
        private string m_name       = null;
        private string m_uri        = null;

        /// <summary>
        /// Create an element list that is for all child elements.
        /// </summary>
        /// <param name="parent">Parent to search</param>
		public ElementList(XmlElement parent)
		{
            m_parent = parent;
		}

        /// <summary>
        /// Create an element list that is for all child elements with the given name;
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        public ElementList(XmlElement parent, string name) : this(parent)
        {
            m_name = name;
        }

        /// <summary>
        /// Create an element list that is for all child elements with the given name and namespace URI.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="namespaceURI"></param>
        public ElementList(XmlElement parent, string name, string namespaceURI) : this(parent)
        {
            m_name = name;
            m_uri = namespaceURI;
        }

        /// <summary>
        /// Get the next node in the enumeration.  Pass in null to start.
        /// </summary>
        /// <param name="start">Starting point for search.</param>
        /// <returns></returns>
        public XmlNode GetNextNode(XmlNode start)
        {
            XmlNode n = (start == null) ? m_parent.FirstChild : start.NextSibling;
            while ((n != null) && !this.IsMatch(n))
            {
                n = n.NextSibling;
            }
            return n;
        } 

        private bool IsMatch(XmlNode curNode)
        {
            if (curNode.NodeType != XmlNodeType.Element)
                return false;

            if (m_uri == null) 
                return (curNode.LocalName == m_name);
        
            return (curNode.LocalName == m_name) && (curNode.NamespaceURI == m_uri);
        } 

        /// <summary>
        /// Enumerate over the matching children.
        /// </summary>
        /// <returns></returns>
        public override System.Collections.IEnumerator GetEnumerator()
        {
            return new ElementListEnumerator(this);
        }

        /// <summary>
        /// Number of matching children.
        /// </summary>
        public override int Count
        {
            get
            {
                int c = 0;
                XmlNode n = null;
                while ((n = GetNextNode(n)) != null)
                {
                    c++;
                }
                return c;
            }
        }

        /// <summary>
        /// Retrieve a given child.
        /// </summary>
        public override XmlNode this[int i]
        {
            get { return Item(i); }
        }

        /// <summary>
        /// Retrieve a given child.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override XmlNode Item(int index)
        {
            int c = 0;
            XmlNode n = m_parent.FirstChild;
            while (n != null)
            {
                if (c == index)
                    return n;
                c++;
                n = GetNextNode(n);
            }
            return null;
        }

        private class ElementListEnumerator : IEnumerator
        {
            private ElementList m_list;
            private XmlNode m_cur = null;

            public ElementListEnumerator(ElementList list)
            {
                m_list = list;
            }

            #region IEnumerator Members

            public void Reset()
            {
                m_cur = null;
            }

            public object Current
            {
                get { return m_cur; }
            }

            public bool MoveNext()
            {
                m_cur = m_list.GetNextNode(m_cur);
                return (m_cur != null);
            }

            #endregion
        }
	}
}
