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

namespace jabber.protocol.accept
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
    [RCS(@"$Header$")]
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
            base(qname.Name, doc)
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
            get { return (RouteType) GetEnumAttr("type", typeof(RouteType)); }
            set 
            { 
                RouteType cur = this.Type;
                if (cur == value)
                    return;
                if (value == RouteType.NONE)
                {
                    RemoveAttribute("type");
                }
                else
                {
                    SetAttribute("type", value.ToString());
                }
            }
        }
    }
}
