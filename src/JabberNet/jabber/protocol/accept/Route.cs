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

namespace JabberNet.jabber.protocol.accept
{
    /// <summary>
    /// The type field in a route tag.
    /// </summary>
    public enum RouteType
    {
        /// <summary>
        /// None specified
        /// </summary>
        NONE = -1,
        /// <summary>
        /// type='error'
        /// </summary>
        error,
        /// <summary>
        /// type='auth'
        /// </summary>
        auth,
        /// <summary>
        /// type='session'
        /// </summary>
        session
    }

    /// <summary>
    /// The route packet.
    /// </summary>
    public class Route : jabber.protocol.Packet
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Route(XmlDocument doc) : base("route", doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Route(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The element inside the route tag.
        /// </summary>
        public XmlElement Contents
        {
            get { return (XmlElement) this.FirstChild; }
            set
            {
                this.InnerXml = "";
                AddChild(value);
            }
        }

        /// <summary>
        /// The type attribute
        /// </summary>
        public RouteType Type
        {
            get { return GetEnumAttr<RouteType>("type"); }
            set { SetEnumAttr("type", value); }
        }
    }
}
