/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
            base(qname.Name, doc)  // Note:  *NOT* base(prefix, qname, doc), so that xpath matches are easier
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
