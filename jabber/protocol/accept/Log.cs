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

namespace jabber.protocol.accept
{
    /// <summary>
    /// The type field in a log tag.
    /// </summary>
    [RCS(@"$Header$")]
    public enum LogType
    {
        /// <summary>
        /// None specified
        /// </summary>
        NONE = -1,
        /// <summary>
        /// type='warn'
        /// </summary>
        warn,
        /// <summary>
        /// type='info'
        /// </summary>
        info,
        /// <summary>
        /// type='verbose'
        /// </summary>
        verbose,
        /// <summary>
        /// type='debug'
        /// </summary>
        debug
    }

    /// <summary>
    /// The log packet.
    /// </summary>
    [RCS(@"$Header$")]
    public class Log : jabber.protocol.Packet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Log(XmlDocument doc) : base("log", doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Log(string prefix, XmlQualifiedName qname, XmlDocument doc) : 
            base(qname.Name, doc)
        {
        }

        /// <summary>
        /// The element inside the route tag.
        /// </summary>
        public XmlElement Element
        {
            get { return this["element"]; }
            set { AddChild(value); }
        }

        /// <summary>
        /// The type attribute
        /// </summary>
        public LogType Type
        {
            get { return (LogType) GetEnumAttr("type", typeof(LogType)); }
            set 
            { 
                LogType cur = this.Type;
                if (cur == value)
                    return;
                if (value == LogType.NONE)
                {
                    RemoveAttribute("type");
                }
                else
                {
                    SetAttribute("type", value.ToString());
                }
            }
        }

        /// <summary>
        /// The namespace for logging
        /// </summary>
        public string NS
        {
            get { return GetAttribute("ns"); }
            set { SetAttribute("ns", value); }
        }

        /// <summary>
        /// The server thread this came from
        /// </summary>
        public string Thread
        {
            get { return GetAttribute("thread"); }
            set { SetAttribute("thread", value); }
        }    

        /// <summary>
        /// Time sent.
        /// </summary>
        public DateTime Timestamp
        {
            get { return JabberDate(GetAttribute("timestamp")); }
            set { SetAttribute("timestamp", JabberDate(value)); }
        }

    }
}
