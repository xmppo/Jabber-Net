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
    /// Error codes for IQ and message
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Bad request
        /// </summary>
        BAD_REQUEST             = 400,
        /// <summary>
        /// Unauthorized
        /// </summary>
        UNAUTHORIZED            = 401,
        /// <summary>
        /// Payment required
        /// </summary>
        PAYMENT_REQUIRED        = 402,
        /// <summary>
        /// Forbidden
        /// </summary>
        FORBIDDEN               = 403,
        /// <summary>
        /// Not found
        /// </summary>
        NOT_FOUND               = 404,
        /// <summary>
        /// Not allowed
        /// </summary>
        NOT_ALLOWED             = 405,
        /// <summary>
        /// Not acceptable
        /// </summary>
        NOT_ACCEPTABLE          = 406,
        /// <summary>
        /// Registration required
        /// </summary>
        REGISTRATION_REQUIRED   = 407,
        /// <summary>
        /// Request timeout
        /// </summary>
        REQUEST_TIMEOUT         = 408,
        /// <summary>
        /// Conflict
        /// </summary>
        CONFLICT                = 409,
        /// <summary>
        /// Internal server error
        /// </summary>
        INTERNAL_SERVER_ERROR   = 500,
        /// <summary>
        /// Not implemented
        /// </summary>
        NOT_IMPLEMENTED         = 501,
        /// <summary>
        /// Remote server error
        /// </summary>
        REMOTE_SERVER_ERROR     = 502,
        /// <summary>
        /// Service unavailable
        /// </summary>
        SERVICE_UNAVAILABLE     = 503,
        /// <summary>
        /// Remote server timeout
        /// </summary>
        REMOTE_SERVER_TIMEOUT   = 504,
        /// <summary>
        /// Disconnected
        /// </summary>
        DISCONNECTED            = 510
    }

    /// <summary>
    /// Error IQ
    /// </summary>
    [RCS(@"$Header$")]
    public class IQError : IQ
    {
        /// <summary>
        /// Create an error IQ with the given code and message.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="code"></param>
        public IQError(XmlDocument doc, ErrorCode code) : base(doc)
        {
            XmlElement e = doc.CreateElement("error");
            this.Type = IQType.error;
            e.SetAttribute("code", ((int)code).ToString());
            e.InnerText = code.ToString();
            this.AppendChild(e);
        }
    }

    /// <summary>
    /// Error in a message or IQ.
    /// </summary>
    [RCS(@"$Header$")]
    public class Error : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Error(XmlDocument doc) : base("error", doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Error(string prefix, XmlQualifiedName qname, XmlDocument doc) : 
            base(qname.Name, doc)
        {
        }

        /// <summary>
        /// The error code
        /// </summary>
        public int Code
        {
            get { return GetIntAttr("code"); } 
            set { this.SetAttribute("code", value.ToString()); }
        }

        /// <summary>
        /// The error message
        /// </summary>
        public string Message
        {
            get { return this.InnerXml; }
            set { this.InnerText = value; }
        }
    }
}
