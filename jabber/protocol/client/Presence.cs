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

namespace jabber.protocol.client
{
    /// <summary>
    /// Presence type attribute
    /// </summary>
    [RCS(@"$Header$")]
    public enum PresenceType
    {
        /// <summary>
        /// None specified
        /// </summary>
        available = -1,
        /// <summary>
        /// May I subscribe to you?
        /// </summary>
        subscribe,
        /// <summary>
        /// Yes, you may subscribe.
        /// </summary>
        subscribed,
        /// <summary>
        /// Unsubscribe from this entity.
        /// </summary>
        unsubscribe,
        /// <summary>
        /// No, you may not subscribe.
        /// </summary>
        unsubscribed,
        /// <summary>
        /// Offline
        /// </summary>
        unavailable,
        /// <summary>
        /// server-side only.
        /// </summary>
        probe,
        /// <summary>
        /// A presence error.
        /// </summary>
        error,
        /// <summary>
        /// Invisible presence: we're unavailable to them, but still see 
        /// theirs.
        /// </summary>
        invisible
    }

    /// <summary>
    /// Client presence packet.
    /// </summary>
    [RCS(@"$Header$")]
    public class Presence : Packet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Presence(XmlDocument doc) : 
            base("presence", doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Presence(string prefix, XmlQualifiedName qname, XmlDocument doc) : 
            base(qname.Name, doc)
        {
        }

        /// <summary>
        /// Presence type
        /// </summary>
        public PresenceType Type
        {
            get { return (PresenceType) GetEnumAttr("type", typeof(PresenceType)); }
            set 
            { 
                if (value == PresenceType.available)
                    RemoveAttribute("type");
                else
                    SetAttribute("type", value.ToString()); 
            }
        }

        /// <summary>
        /// Presence status
        /// </summary>
        public string Status
        {
            get { return GetElem("status"); }
            set { SetElem("status", value); }
        }

        /// <summary>
        /// Presence show
        /// </summary>
        public string Show
        {
            get { return GetElem("show"); }
            set { SetElem("show", value); }
        }

        /// <summary>
        /// Priority for this resource.
        /// </summary>
        public string Priority
        {
            get { return GetElem("priority"); }
            set { SetElem("priority", value); }
        }

        /// <summary>
        /// Presence error.
        /// </summary>
        public Error Error
        {
            get { return (Error) this["error"]; }
            set 
            {
                this.Type = PresenceType.error;    
                ReplaceChild(value); 
            }
        }
    }
}
