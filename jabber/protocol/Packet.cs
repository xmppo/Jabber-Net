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
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Xml;

using bedrock.util;

namespace jabber.protocol
{
    /// <summary>
    /// Packets that have to/from information.
    /// </summary>
    [RCS(@"$Header$")]
    public class Packet : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Packet(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="doc"></param>
        public Packet(string localName, XmlDocument doc) :
            base(localName, doc)
        {
        }

        /// <summary>
        /// The TO address
        /// </summary>
        public JID To
        {
            get { return new JID(this.GetAttribute("to")); }
            set 
			{ 
				if (value == null)
					this.RemoveAttribute("to");
				else
					this.SetAttribute("to", value); 
			}
        }

        /// <summary>
        ///  The FROM address
        /// </summary>
        public JID From
        {
            get { return new JID(this.GetAttribute("from")); }
			set
			{ 
				if (value == null)
					this.RemoveAttribute("from");
				else
					this.SetAttribute("from", value); 
			}
		}
   
        /// <summary>
        /// The packet ID.
        /// </summary>
        public string ID
        {
            get { return this.GetAttribute("id"); }
            set { this.SetAttribute("id", value); }
        }

        /// <summary>
        /// Swap the To and the From addresses.
        /// </summary>
        public void Swap()
        {
            string tmp = this.GetAttribute("to");
            this.SetAttribute("to", this.GetAttribute("from"));
            this.SetAttribute("from", tmp);
        }
    }
}
