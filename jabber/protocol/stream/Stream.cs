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
using System.Security.Cryptography;
using System.Xml;
using bedrock.util;
using jabber.protocol;

namespace jabber.protocol.stream
{
    /// <summary>
    /// The fabled stream:stream packet.  Id's get assigned automatically on allocation.
    /// </summary>
    [RCS(@"$Header$")]
    public class Stream : Packet
    {
        private static readonly RandomNumberGenerator RNG = RandomNumberGenerator.Create();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xmlns"></param>
        public Stream(XmlDocument doc, string xmlns) :
            base("stream", new XmlQualifiedName("stream", jabber.protocol.URI.STREAM), doc)
        {
            byte[] buf = new byte[4];
            RNG.GetBytes(buf);
            ID = HexString(buf);
            NS = xmlns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Stream(string prefix, XmlQualifiedName qname, XmlDocument doc) : 
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Default stream namespace.  xmlns=''.
        /// </summary>
        public string NS
        {
            get { return this.GetAttribute("xmlns"); }
            set { this.SetAttribute("xmlns", value); }
        }
    }
}
