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
     * <iq type="result" to="romeo@montague.net/orchard" 
     *                   from="juliet@capulet.com/balcony"
     *                   id="i_time_001">
     *   <query xmlns="jabber:iq:time">
     *     <utc>20020214T23:55:06</utc>
     *     <tz>WET</tz>
     *     <display>14 Feb 2002 11:55:06 PM</display>
     *   </query>
     * </iq>
     */
    /// <summary>
    /// IQ packet with an time query element inside.
    /// </summary>
    [RCS(@"$Header$")]
    public class TimeIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create a time IQ
        /// </summary>
        /// <param name="doc"></param>
        public TimeIQ(XmlDocument doc) : base(doc)
        {
            this.Query = new Time(doc);
        }
    }

    /// <summary>
    /// A time query element.
    /// </summary>
    [RCS(@"$Header$")]
    public class Time : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Time(XmlDocument doc) : base("query", URI.TIME, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Time(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Universal coordinated time.  (More or less GMT).
        /// </summary>
        //TODO: return System.DateTime?
        public string UTC 
        {
            get { return GetElem("utc"); }
            set { SetElem("utc", value); }
        }

        /// <summary>
        /// Timezone
        /// </summary>
        //TODO: return System.TimeZone?
        public string TZ 
        {
            get { return GetElem("tz"); }
            set { SetElem("tz", value); }
        }

        /// <summary>
        /// Human-readable date/time.
        /// </summary>
        public string Display
        {
            get { return GetElem("display"); }
            set { SetElem("display", value); }
        }
    }
}
