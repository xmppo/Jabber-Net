/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
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
        /// <param name="streamID"></param>
        public void SetAuth(string secret, string streamID)
        {
            this.Digest = ShaHash(streamID, secret);
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
