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
    /*
     * <iq type="set" to="horatio@denmark" from="sailor@sea" id="i_oob_001">
     *   <query xmlns="jabber:iq:oob">
     *     <url>http://denmark/act4/letter-1.html</url>
     *     <desc>There's a letter for you sir.</desc>
     *   </query>
     * </iq>
     */
    /// <summary>
    /// IQ packet with an oob query element inside.
    /// </summary>
    [RCS(@"$Header$")]
    public class OobIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create an OOB IQ.
        /// </summary>
        /// <param name="doc"></param>
        public OobIQ(XmlDocument doc) : base(doc)
        {
            this.Query = new OOB(doc);
        }
    }

    /// <summary>
    /// An oob query element for file transfer.
    /// </summary>
    [RCS(@"$Header$")]
    public class OOB : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public OOB(XmlDocument doc) : base("query", URI.OOB, doc)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OOB(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// URL to send/receive from
        /// </summary>
        public string Url 
        {
            get { return GetElem("url"); }
            set { SetElem("url", value); }
        }

        /// <summary>
        /// File description
        /// </summary>
        public string Desc 
        {
            get { return GetElem("desc"); }
            set { SetElem("desc", value); }
        }
    }
}
