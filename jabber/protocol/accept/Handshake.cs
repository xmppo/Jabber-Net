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

namespace jabber.protocol.accept
{
    /// <summary>
    /// The handshake tag, including digest calculation.  Call SetAuth() to calculate 
    /// the SHA1 hash.
    /// </summary>
    [RCS(@"$Header$")]
    public class Handshake : jabber.protocol.Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Handshake(XmlDocument doc) : base("handshake", doc)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Handshake(string prefix, XmlQualifiedName qname, XmlDocument doc) : 
            base(qname.Name, doc)
        {
        }

        /// <summary>
        /// Set the auth information for this handshake tag, 
        /// performing the digest operation.
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="StreamID"></param>
        public void SetAuth(string secret, string StreamID)
        {
            this.Digest = ShaHash(StreamID, secret);
        }

        /// <summary>
        /// The digest.
        /// </summary>
        public string Digest
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }
}
