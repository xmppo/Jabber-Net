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
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
    /// <summary>
    /// An browse IQ.
    /// </summary>
    [RCS(@"$Header$")]
    public class BrowseIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create a Browse IQ.
        /// </summary>
        /// <param name="doc"></param>
        public BrowseIQ(XmlDocument doc) : base(doc)
        {
            this.Query = new Browse(doc);
        }
    }

    /// <summary>
	/// Browse IQ query.
	/// </summary>
    [RCS(@"$Header$")]
    public class Browse : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Browse(XmlDocument doc) : 
            base("query", URI.BROWSE, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Browse(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The full JabberID of the entity described.
        /// </summary>
        public JID JID
        {
            get { return new JID(GetAttribute("jid")); }
            set { SetAttribute("jid", value.ToString()); }
        }

        /// <summary>
        /// One of the categories from the category list, or a non-standard category prefixed with the string "x-". 
        /// </summary>
        public string Category
        {
            get { return GetAttribute("category"); }
            set { SetAttribute("category", value); }
        }

        /// <summary>
        /// One of the official types from the specified category, or a non-standard type prefixed with the string "x-". 
        /// </summary>
        public string Type
        {
            get { return GetAttribute("type"); }
            set { SetAttribute("type", value); }
        }

        /// <summary>
        /// A friendly name that may be used in a user interface. 
        /// </summary>
        public string BrowseName
        {
            get { return GetAttribute("name"); }
            set { SetAttribute("name", value); }
        }

        /// <summary>
        /// A string containing the version of the node, equivalent to the response provided to a 
        /// query in the 'jabber:iq:version' namespace. This is useful for servers, especially for lists of services 
        /// (see the 'service/serverlist' category/type above). 
        /// </summary>
        public string Version
        {
            get { return GetAttribute("version"); }
            set { SetAttribute("version", value); }
        }

        /// <summary>
        /// Sub-items of this item
        /// </summary>
        /// <returns></returns>
        public Browse[] GetItems()
        {
            XmlNodeList nl = GetElementsByTagName("item", URI.BROWSE);
            Browse[] items = new Browse[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                items[i] = (Browse) n;
                i++;
            }
            return items;
        }

        /// <summary>
        /// Add an item to the sub-item list.
        /// </summary>
        /// <returns></returns>
        public Browse AddItem()
        {
            Browse b = new Browse(this.OwnerDocument);
            this.AppendChild(b);
            return b;
        }

        /// <summary>
        /// The namespaces advertised by this item.
        /// </summary>
        /// <returns></returns>
        public string[] GetNamespaces()
        {
            XmlNodeList nl = GetElementsByTagName("ns", URI.BROWSE);
            string[] items = new string[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                items[i] = n.InnerText;
                i++;
            }
            return items;
        }

        /// <summary>
        /// Add a namespace to the namespaces supported by this item.
        /// </summary>
        /// <param name="ns"></param>
        public void AddNamespace(string ns)
        {
            XmlElement e = this.OwnerDocument.CreateElement(null, "ns", URI.BROWSE);
            e.InnerText = ns;
            this.AppendChild(e);
        }
    }
}
