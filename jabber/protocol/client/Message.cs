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
    /// Message type attribute
    /// </summary>
    [RCS(@"$Header$")]
    public enum MessageType
    {
        /// <summary>
        /// Normal message
        /// </summary>
        normal = -1, 
        /// <summary>
        /// Error message
        /// </summary>
        error, 
        /// <summary>
        /// Chat (one-to-one) message
        /// </summary>
        chat,
        /// <summary>
        /// Groupchat
        /// </summary>
        groupchat,
        /// <summary>
        /// Headline
        /// </summary>
        headline
    }
    /// <summary>
    /// A client-to-client message.  
    /// TODO: Some XHTML is supported by setting the .Html property,
    /// but extra xmlns="" get put everywhere at the moment.
    /// </summary>
    [RCS(@"$Header$")]
    public class Message : Packet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Message(XmlDocument doc) : base("message", doc)
        {
            ID = NextID();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Message(string prefix, XmlQualifiedName qname, XmlDocument doc) : 
            base(qname.Name, doc)
        {
        }

        /// <summary>
        /// The message type attribute
        /// </summary>
        public MessageType Type
        {
            get { return (MessageType) GetEnumAttr("type", typeof(MessageType)); }
            set 
            { 
                if (value == MessageType.normal)
                    RemoveAttribute("type");
                else
                    SetAttribute("type", value.ToString()); 
            }
        }

        /// <summary>
        /// On set, creates both an html element, and a body element, which will
        /// have the de-html'd version of the html element.
        /// </summary>
        public string Html
        {
            get
            { 
                //FIXME: return body contents
                return GetElem("html"); 
            }
            set 
            { 
                XmlElement old = this["html", URI.XHTML];
                if (old != null)
                    this.RemoveChild(old);
                XmlElement html = this.OwnerDocument.CreateElement(null, "html", URI.XHTML);
                XmlElement body = this.OwnerDocument.CreateElement(null, "body", URI.XHTML);
                body.InnerXml = value;
                html.AppendChild(body);
                this.AppendChild(html);
                // TODO: strip HTML tags
                //this.Body = body.InnerText;
            }
        }

        /// <summary>
        /// The message body
        /// </summary>
        public string Body
        {
            get { return GetElem("body"); }
            set { SetElem("body", value); }
        }

        /// <summary>
        /// The message thread
        /// TODO: some help to generate these, please.
        /// </summary>
        public string Thread
        {
            get { return GetElem("thread"); }
            set { SetElem("thread", value); }
        }
        /// <summary>
        /// The message subject
        /// </summary>
        public string Subject
        {
            get { return GetElem("subject"); }
            set { SetElem("subject", value); }
        }
        /// <summary>
        /// The first x tag, regardless of namespace.
        /// </summary>
        public XmlElement X
        {
            get { return this["x"]; }
            set { this.AddChild(value); }
        }

        /// <summary>
        /// Message error.
        /// </summary>
        public Error Error
        {
            get { return (Error) this["error"]; }
            set 
            {
                this.Type = MessageType.error;    
                ReplaceChild(value); 
            }
        }
    }
}
