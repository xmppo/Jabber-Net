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

using bedrock.util;

namespace jabber.protocol
{
    /// <summary>
    /// Namespace constants for http://etherx.jabber.org/streams.
    /// </summary>
    [RCS(@"$Header$")]
    public class URI
    {
        /// <summary>
        /// XHTML namespace
        /// </summary>
        public const string XHTML  = "http://www.w3.org/1999/xhtml";
        /// <summary>
        /// stream:stream
        /// </summary>
        public const string STREAM = "http://etherx.jabber.org/streams";
        /// <summary>
        /// Jabber client connections
        /// </summary>
        public const string CLIENT = "jabber:client";
        /// <summary>
        /// Jabber component connections
        /// </summary>
        public const string ACCEPT = "jabber:component:accept";
        /// <summary>
        /// S2S connection
        /// </summary>
        public const string SERVER = "jabber:server"; 
        /// <summary>
        /// S2S dialback
        /// </summary>
        public const string DIALBACK = "jabber:server:dialback";
        /// <summary>
        /// SASL support
        /// </summary>
        public const string SASL = "http://www.iana.org/assignments/sasl-mechanisms";

        // IQ
        /// <summary>
        /// Authentication
        /// </summary>
        public const string AUTH     = "jabber:iq:auth";
        /// <summary>
        /// Roster manipulation
        /// </summary>
        public const string ROSTER   = "jabber:iq:roster";
        /// <summary>
        /// Register users
        /// </summary>
        public const string REGISTER = "jabber:iq:register";
        /// <summary>
        /// Out-of-band (file transfer)
        /// </summary>
        public const string OOB      = "jabber:iq:oob";
        /// <summary>
        /// Server agents
        /// </summary>
        public const string AGENTS   = "jabber:iq:agents";
        /// <summary>
        /// Client or server current time
        /// </summary>
        public const string TIME     = "jabber:iq:time";
        /// <summary>
        /// Last activity
        /// </summary>
        public const string LAST     = "jabber:iq:last";
        /// <summary>
        /// Client or server version
        /// </summary>
        public const string VERSION  = "jabber:iq:version";
        /// <summary>
        /// Jabber Browsing
        /// </summary>
        public const string BROWSE   = "jabber:iq:browse";
		/// <summary>
		/// Profile information
		/// </summary>
		public const string VCARD    = "vcard-temp";

        /// <summary>
        /// Discover items from an entity.
        /// </summary>
        public const string DISCO_ITEMS = "http://jabber.org/protocol/disco#items";
        /// <summary>
        /// Discover info about an entity item.
        /// </summary>
        public const string DISCO_INFO = "http://jabber.org/protocol/disco#info";

        // X
        /// <summary>
        /// Offline message timestamping.
        /// </summary>
        public const string XDELAY   = "jabber:x:delay";
        /// <summary>
        /// Out-of-band (file transfer)
        /// </summary>
        public const string XOOB     = "jabber:x:oob";
        /// <summary>
        /// Send roster entries to another user.
        /// </summary>
        public const string XROSTER  = "jabber:x:roster";
        /// <summary>
        /// The jabber:x:event namespace qualifies extensions used to request and respond to 
        /// events relating to the delivery, display, and composition of messages.
        /// </summary>
        public const string XEVENT = "jabber:x:event";
        /// <summary>
        /// jabber:x:data, as described in JEP-0004.
        /// </summary>
        public const string XDATA = "jabber:x:data";
    }
}
