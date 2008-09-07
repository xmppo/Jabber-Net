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
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;
using System.Diagnostics;
using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
    /// <summary>
    /// Private storage IQ.
    /// See XEP-0049 (http://www.xmpp.org/extensions/xep-0049.html)
    /// </summary>
    [SVN(@"$Id$")]
    public class PrivateIQ : jabber.protocol.client.TypedIQ<Private>
    {
        /// <summary>
        /// Create an IQ for the jabber:iq:private namespace.
        /// Make sure to add a body to the query before sending.
        /// </summary>
        /// <param name="doc"></param>
        public PrivateIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// Private storage query.
    /// See XEP-0049 (http://www.xmpp.org/extensions/xep-0049.html)
    /// </summary>
    [SVN(@"$Id$")]
    public class Private : Element
    {
        /// <summary>
        /// Create for outbound
        /// </summary>
        /// <param name="doc"></param>
        public Private(XmlDocument doc) : 
            base("query", URI.PRIVATE, doc)
        {
        }

        /// <summary>
        /// Create for inbound.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Private(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }
}
