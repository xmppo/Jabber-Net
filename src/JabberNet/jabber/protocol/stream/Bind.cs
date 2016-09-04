/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See licenses/Jabber-Net_LGPLv3.txt for details.
 * --------------------------------------------------------------------------*/

using System.Xml;

namespace JabberNet.jabber.protocol.stream
{
    /// <summary>
    /// Bind start after binding
    /// </summary>
    public class Bind : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Bind(XmlDocument doc) :
            base("", new XmlQualifiedName("bind", jabber.protocol.URI.BIND), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Bind(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The resource to bind to.  Null says for the server to pick.
        /// </summary>
        public string Resource
        {
            get { return GetElem("resource"); }
            set { SetElem("resource", value); }
        }

        /// <summary>
        /// The JID that the server selected for us.
        /// </summary>
        public string JID
        {
            get { return GetElem("jid"); }
            set { SetElem("jid", value); }
        }
    }
}
