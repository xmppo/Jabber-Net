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
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Xml;

using bedrock.util;

namespace jabber.protocol.x
{
    /// <summary>
    /// A delay x element.
    /// </summary>
    [RCS(@"$Header$")]
    public class Delay : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Delay(XmlDocument doc) : base("x", URI.XDELAY, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Delay(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// From whom?
        /// </summary>
        public string From 
        {
            get { return GetAttribute("from"); }
            set { SetAttribute("from", value); }
        }

        /// <summary>
        /// Date/time stamp.
        /// </summary>
        public DateTime Stamp 
        {
            get { return JabberDate(GetAttribute("stamp")); }
            set { SetAttribute("stamp", JabberDate(value)); }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Desc 
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }
}
