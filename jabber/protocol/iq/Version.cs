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
using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
    /// <summary>
    /// IQ packet with a version query element inside.
    /// </summary>
    [RCS(@"$Header$")]
    public class VersionIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create a version IQ
        /// </summary>
        /// <param name="doc"></param>
        public VersionIQ(XmlDocument doc) : base(doc)
        {
            this.Query = new Version(doc);
        }
    }

    /// <summary>
    /// A time query element.
    /// </summary>
    [RCS(@"$Header$")]
    public class Version : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Version(XmlDocument doc) : base("query", URI.VERSION, doc)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Version(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Name of the entity.
        /// </summary>
        public string EntityName 
        {
            get { return GetElem("name"); }
            set { SetElem("name", value); }
        }

        /// <summary>
        /// Enitity version.  (Version was a keyword, or something)
        /// </summary>
        public string Ver 
        {
            get { return GetElem("version"); }
            set { SetElem("version", value); }
        }

        /// <summary>
        /// Operating system of the entity.
        /// </summary>
        public string OS
        {
            get { return GetElem("os"); }
            set { SetElem("os", value); }
        }
    }
}
