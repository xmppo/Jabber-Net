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

namespace jabber.protocol.iq
{
    /*
     *  <iq id='l4' type='result' from='user@host'>
     *    <query xmlns='jabber:iq:last' seconds='903'>
     *      Heading home
     *    </query>
     *  </iq>
     */
    /// <summary>
    /// IQ packet with an Last query element inside.
    /// </summary>
    [RCS(@"$Header$")]
    public class LastIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create a Last IQ
        /// </summary>
        /// <param name="doc"></param>
        public LastIQ(XmlDocument doc) : base(doc)
        {
            this.Query = new Last(doc);
        }
    }

    /// <summary>
    /// A Last query element, which requests the last activity from an entity.
    /// </summary>
    [RCS(@"$Header$")]
    public class Last : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Last(XmlDocument doc) : base("query", URI.LAST, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Last(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The message inside the Last element.
        /// </summary>
        public string Message 
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }

        /// <summary>
        /// How many seconds since the last activity.
        /// </summary>
        public int Seconds
        {
            get { return Int32.Parse(GetAttribute("seconds"));  }
            set { SetAttribute("seconds", value.ToString()); }
        }
    }
}
