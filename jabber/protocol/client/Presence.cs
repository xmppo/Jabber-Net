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
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Xml;

using bedrock.util;
using jabber.protocol.x;

namespace jabber.protocol.client
{
    /// <summary>
    /// Presence type attribute
    /// </summary>
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
    public class Presence : Packet, IComparable<Presence>, IComparable
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
        /// An integer version of the priority, constrained to -128..127.  0 if there was no priority element or it wasn't an integer.
        /// </summary>
        public int IntPriority
        {
            get
            {
                String pri = Priority;
                if ((pri == null) || (pri == ""))
                    return 0;
                try
                {
                    int i = int.Parse(pri);
                    if (i < -128)
                        return -128;
                    if (i > 127)
                        return 127;
                    return i;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set
            {
                SetElem("priority", value.ToString());
            }
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

        private static int IntShow(string show)
        {
            switch (show)
            {
            case "dnd":
                return 0;
            case "xa":
                return 1;
            case "away":
                return 2;
            case "chat":
                return 4;
            default:
                return 3;
            }
        }
        
        /// <summary>
        /// Date/Time stamp that the presence was originially received by the sending
        /// server, if this presence is in response to a probe.
        /// </summary>
        public DateTime Stamp
        {
            get
            {
                jabber.protocol.x.ModernDelay md = GetChildElement<jabber.protocol.x.ModernDelay>();
                if (md != null)
                    return md.Stamp;
                jabber.protocol.x.Delay delay = GetChildElement<jabber.protocol.x.Delay>();
                if (delay != null)
                    return delay.Stamp;
                return DateTime.MinValue;
            }
            set
            {
                jabber.protocol.x.ModernDelay md = GetChildElement<jabber.protocol.x.ModernDelay>();
                if (md != null)
                {
                    md.Stamp = value;
                    return;
                }
                jabber.protocol.x.Delay delay = GetChildElement<jabber.protocol.x.Delay>();
                if (delay != null)
                {
                    delay.Stamp = value;
                    return;
                }
                md = new jabber.protocol.x.ModernDelay(this.OwnerDocument);
                md.Stamp = value;
                this.AddChild(md);
            }
        }

        /// <summary>
        /// If there is a stamp, returns it, otherwise looks for and adds a new stamp element.
        /// This method should never be called for presence that is to be sent out, since it 
        /// will add non-standard protocol to the presence.
        /// </summary>
        public DateTime ReceivedTime
        {
            get
            {
                DateTime dt = this.Stamp;
                if (dt != DateTime.MinValue)
                    return dt;
                const string RECEIVED = "http://cursive.net/protocol/received";
                XmlElement el = this["x", RECEIVED];
                if (el != null)
                    return Element.DateTimeProfile(el.InnerText);
                dt = DateTime.Now;
                el = OwnerDocument.CreateElement("x", RECEIVED);
                el.InnerText = Element.DateTimeProfile(dt);
                this.AppendChild(el);
                return dt;
            }
        }

        /// <summary>
        /// Compare two presences (from the same bare JID, but from
        /// different resources), to determine which is "more
        /// available".
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator<(Presence first, Presence second)
        {
            return (((IComparable<Presence>)first).CompareTo(second) == -1);
        }

        /// <summary>
        /// Compare two presences (from the same bare JID, but from
        /// different resources), to determine which is "more
        /// available".
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator>(Presence first, Presence second)
        {
            return (((IComparable<Presence>)first).CompareTo(second) == 1);
        }

        #region IComparable<Presence> Members

        /// <summary>
        /// Compare this presence element with another, first by priority, 
        /// then by show, then by time received.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        ///    Less than zero 
        ///     This object is less than the other parameter.
        ///    Zero 
        ///     This object is equal to other. 
        ///    Greater than zero 
        ///     This object is greater than other. 
        /// </returns>
        public int CompareTo(Presence other)
        {
            /*
            Less than zero 
             This object is less than the other parameter.
 
            Zero 
             This object is equal to other. 
 
            Greater than zero 
             This object is greater than other. 

             */
            if ((object)this == (object)other)
                return 0;

            if (other == null)
                return 1;

            int tp = this.IntPriority;
            int op = other.IntPriority;
            if (tp > op)
                return 1;
            if (tp < op)
                return -1;

            // equal priority
            int ts = IntShow(this.Show);
            int os = IntShow(other.Show);

            if (ts > os)
                return 1;
            if (ts < os)
                return -1;

            // equal show
            return this.ReceivedTime.CompareTo(other.ReceivedTime);
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compare this presence element with another, first by priority, 
        /// then by show, then by time received.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        ///    Less than zero 
        ///     This object is less than the other parameter.
        ///    Zero 
        ///     This object is equal to other. 
        ///    Greater than zero 
        ///     This object is greater than other. 
        /// </returns>
        public int CompareTo(object other)
        {
            if (other is Presence)
                return CompareTo((Presence)other);
            return 1;
        }

        #endregion
    }
}
