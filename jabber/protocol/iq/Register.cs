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

    /// <summary>
    /// IQ packet with a register query element inside.
    /// </summary>
    [RCS(@"$Header$")]
    public class RegisterIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create a Register IQ.
        /// </summary>
        /// <param name="doc"></param>
        public RegisterIQ(XmlDocument doc) : base(doc)
        {
            this.Query = new Register(doc);
        }
    }

    /// <summary>
    /// User registration
    /// </summary>
    [RCS(@"$Header$")]
    public class Register : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Register(XmlDocument doc) : 
            base("query", URI.REGISTER, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Register(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Instructions to the user.
        /// </summary>
        public string Instructions
        {
            get { return GetElem("instructions"); }
            set { SetElem("instructions", value); }
        }

        /// <summary>
        /// Username to register
        /// </summary>
        public string Username
        {
            get { return GetElem("username"); }
            set { SetElem("username", value); }
        }

        /// <summary>
        /// User nickname
        /// </summary>
        public string Nick
        {
            get { return GetElem("nick"); }
            set { SetElem("nick", value); }
        }

        /// <summary>
        /// User password
        /// </summary>
        public string Password
        {
            get { return GetElem("password"); }
            set { SetElem("password", value); }
        }

        /// <summary>
        /// The name element.... what's this for?
        /// </summary>
        public string JName
        {
            get { return GetElem("name"); }
            set { SetElem("name", value); }
        }

        /// <summary>
        /// The first name
        /// </summary>
        public string First
        {
            get { return GetElem("first"); }
            set { SetElem("first", value); }
        }

        /// <summary>
        /// Last name
        /// </summary>
        public string Last
        {
            get { return GetElem("last"); }
            set { SetElem("last", value); }
        }

        /// <summary>
        /// E-mail address
        /// TODO: add format checking?
        /// </summary>
        public string Email
        {
            get { return GetElem("email"); }
            set { SetElem("email", value); }
        }

        /// <summary>
        /// User's mailing address
        /// </summary>
        public string Address
        {
            get { return GetElem("address"); }
            set { SetElem("address", value); }
        }

        /// <summary>
        /// User's city
        /// </summary>
        public string City
        {
            get { return GetElem("city"); }
            set { SetElem("city", value); }
        }

        /// <summary>
        /// User's state
        /// </summary>
        public string State
        {
            get { return GetElem("state"); }
            set { SetElem("state", value); }
        }

        /// <summary>
        /// User's zip code
        /// </summary>
        public string Zip
        {
            get { return GetElem("zip"); }
            set { SetElem("zip", value); }
        }

        /// <summary>
        /// User's phone number
        /// </summary>
        public string Phone
        {
            get { return GetElem("phone"); }
            set { SetElem("phone", value); }
        }

        /// <summary>
        /// URL for user
        /// </summary>
        public string Url
        {
            get { return GetElem("url"); }
            set { SetElem("url", value); }
        }

        /// <summary>
        /// Current date
        /// </summary>
        public string Date
        {
            get { return GetElem("date"); }
            set { SetElem("date", value); }
        }

        /// <summary>
        /// Miscellaneous information
        /// </summary>
        public string Misc
        {
            get { return GetElem("misc"); }
            set { SetElem("misc", value); }
        }

        /// <summary>
        /// Text... what is this used for?
        /// </summary>
        public string Text
        {
            get { return GetElem("text"); }
            set { SetElem("text", value); }
        }

        /// <summary>
        /// Public key?
        /// </summary>
        public string Key
        {
            get { return GetElem("key"); }
            set { SetElem("key", value); }
        }

        /// <summary>
        /// Is the user already registered?
        /// </summary>
        public bool Registered
        {
            get { return (this["registered"] != null); }
            set 
            {  
                if (value)
                {
                    SetElem("registered", null);
                }
                else
                {
                    XmlNode child = this["registered"];
                    if (child != null)
                        RemoveChild(child);
                }
            }
        }

        /// <summary>
        /// Remove the current user.
        /// </summary>
        public bool Remove
        {
            get { return GetElem("remove") != null; }
            set 
            {  
                if (value)
                {
                    SetElem("remove", null);
                }
                else
                {
                    XmlNode child = this["remove"];
                    if (child != null)
                        RemoveChild(child);
                }
            }
        }
    }
}
