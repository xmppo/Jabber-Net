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

namespace jabber.protocol.client
{
    /*
    /// <summary>
    /// Error codes for IQ and message
    /// </summary>
    [SVN(@"$Id$")]
    public enum ErrorCode
    {
        /// <summary>
        ///  None specified.
        /// </summary>
        none = -1,
        /// <summary>
        /// Gone (302)
        /// </summary>
        GONE = 302,
        /// <summary>
        /// Bad request (400)
        /// </summary>
        BAD_REQUEST             = 400,
        /// <summary>
        /// Unauthorized (401)
        /// </summary>
        UNAUTHORIZED            = 401,
        /// <summary>
        /// Payment required (402)
        /// </summary>
        PAYMENT_REQUIRED        = 402,
        /// <summary>
        /// Forbidden (403)
        /// </summary>
        FORBIDDEN               = 403,
        /// <summary>
        /// Not found (404)
        /// </summary>
        NOT_FOUND               = 404,
        /// <summary>
        /// Not allowed (405)
        /// </summary>
        NOT_ALLOWED             = 405,
        /// <summary>
        /// Not acceptable (406)
        /// </summary>
        NOT_ACCEPTABLE          = 406,
        /// <summary>
        /// Registration required (407)
        /// </summary>
        REGISTRATION_REQUIRED   = 407,
        /// <summary>
        /// Request timeout (408)
        /// </summary>
        REQUEST_TIMEOUT         = 408,
        /// <summary>
        /// Conflict (409)
        /// </summary>
        CONFLICT                = 409,
        /// <summary>
        /// Internal server error (500)
        /// </summary>
        INTERNAL_SERVER_ERROR   = 500,
        /// <summary>
        /// Not implemented (501)
        /// </summary>
        NOT_IMPLEMENTED         = 501,
        /// <summary>
        /// Remote server error (502)
        /// </summary>
        REMOTE_SERVER_ERROR     = 502,
        /// <summary>
        /// Service unavailable (503)
        /// </summary>
        SERVICE_UNAVAILABLE     = 503,
        /// <summary>
        /// Remote server timeout (504)
        /// </summary>
        REMOTE_SERVER_TIMEOUT   = 504,
        /// <summary>
        /// Disconnected (510)
        /// </summary>
        DISCONNECTED            = 510
    }
     */

    /// <summary>
    /// See RFC 3920, section 9.3.2.  These are the possible error types.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// None specified (protocol error)
        /// </summary>
        NONE = -1,
        /// <summary>
        /// do not retry (the error is unrecoverable)
        /// </summary>
        cancel,
        /// <summary>
        /// proceed (the condition was only a warning)
        /// </summary>
        @continue,
        /// <summary>
        /// retry after changing the data sent
        /// </summary>
        modify,
        /// <summary>
        /// retry after providing credentials
        /// </summary>
        auth,
        /// <summary>
        /// retry after waiting (the error is temporary)
        /// </summary>
        wait
    }


    /// <summary>
    /// Error IQ
    /// </summary>
    [SVN(@"$Id$")]
    public class IQError : IQ
    {
        /// <summary>
        /// Create an error IQ with the given code and message.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="condition"></param>
        public IQError(XmlDocument doc, string condition) : base(doc)
        {
            this.Type = IQType.error;
            Error e = Error.GetStanzaError(doc, condition);
            this.AppendChild(e);
        }
    }

    /// <summary>
    /// Error in a message or IQ.
    /// </summary>
    [SVN(@"$Id$")]
    public class Error : Element
    {
        /// <summary>
        /// modify      400
        /// </summary>
        public const string BAD_REQUEST = "bad-request";
        /// <summary>
        /// cancel      409
        /// </summary>
        public const string CONFLICT = "conflict";
        /// <summary>
        /// cancel  501
        /// </summary>
        public const string FEATURE_NOT_IMPLEMENTED = "feature-not-implemented";
        /// <summary>
        /// auth    403
        /// </summary>
        public const string FORBIDDEN = "forbidden";
        /// <summary>
        ///     modify  302 (permanent)
        /// </summary>
        public const string GONE = "gone";
        /// <summary>
        ///     wait    500
        /// </summary>
        public const string INTERNAL_SERVER_ERROR = "internal-server-error";
        /// <summary>
        ///     cancel  404
        /// </summary>
        public const string ITEM_NOT_FOUND = "item-not-found";
        /// <summary>
        ///     modify  400
        /// </summary>
        public const string JID_MALFORMED = "jid-malformed";
        /// <summary>
        ///     modify  406
        /// </summary>
        public const string NOT_ACCEPTABLE = "not-acceptable";
        /// <summary>
        ///     cancel  405
        /// </summary>
        public const string NOT_ALLOWED = "not-allowed";
        /// <summary>
        ///     auth    401
        /// </summary>
        public const string NOT_AUTHORIZED = "not-authorized";
        /// <summary>
        ///     auth    402
        /// </summary>
        public const string PAYMENT_REQUIRED = "payment-required";
        /// <summary>
        ///     wait    404
        /// </summary>
        public const string RECIPIENT_UNAVAILABLE = "recipient-unavailable";
        /// <summary>
        ///     modify  302 (temporary)
        /// </summary>
        public const string REDIRECT = "redirect";
        /// <summary>
        ///     auth    407
        /// </summary>
        public const string REGISTRATION_REQUIRED = "registration-required";
        /// <summary>
        ///     cancel  404
        /// </summary>
        public const string REMOTE_SERVER_NOT_FOUND = "remote-server-not-found";
        /// <summary>
        ///     wait    504
        /// </summary>
        public const string REMOTE_SERVER_TIMEOUT = "remote-server-timeout";
        /// <summary>
        ///     wait    500
        /// </summary>
        public const string RESOURCE_CONSTRAINT = "resource-constraint";
        /// <summary>
        ///     cancel  503
        /// </summary>
        public const string SERVICE_UNAVAILABLE = "service-unavailable";
        /// <summary>
        ///     auth    407
        /// </summary>
        public const string SUBSCRIPTION_REQUIRED = "subscription-required";
        /// <summary>
        ///     [any]   500
        /// </summary>
        public const string UNDEFINED_CONDITION = "undefined-condition";
        /// <summary>
        ///     wait    400
        /// </summary>
        public const string UNEXPECTED_REQUEST = "unexpected-request";

        private static System.Collections.Hashtable s_errors = new System.Collections.Hashtable();
        private struct CodeType
        {
            public int Code;
            public ErrorType Type;
            public CodeType(int code, ErrorType type)
            {
                Code = code;
                Type = type;
            }
        }

        static Error()
        {
            // See XEP-86.  (http://www.xmpp.org/extensions/xep-0086.html)
            s_errors.Add(BAD_REQUEST, new CodeType(400, ErrorType.modify));
            s_errors.Add(CONFLICT, new CodeType(409, ErrorType.cancel));
            s_errors.Add(FEATURE_NOT_IMPLEMENTED, new CodeType(501, ErrorType.cancel));
            s_errors.Add(FORBIDDEN, new CodeType(403, ErrorType.auth));
            s_errors.Add(GONE, new CodeType(302, ErrorType.modify));
            s_errors.Add(INTERNAL_SERVER_ERROR, new CodeType(500, ErrorType.wait));
            s_errors.Add(ITEM_NOT_FOUND, new CodeType(404, ErrorType.cancel));
            s_errors.Add(JID_MALFORMED, new CodeType(400, ErrorType.modify));
            s_errors.Add(NOT_ACCEPTABLE, new CodeType(406, ErrorType.modify));
            s_errors.Add(NOT_ALLOWED, new CodeType(405, ErrorType.cancel));
            s_errors.Add(NOT_AUTHORIZED, new CodeType(401, ErrorType.auth));
            s_errors.Add(PAYMENT_REQUIRED, new CodeType(402, ErrorType.auth));
            s_errors.Add(RECIPIENT_UNAVAILABLE, new CodeType(404, ErrorType.wait));
            s_errors.Add(REDIRECT, new CodeType(302, ErrorType.modify));
            s_errors.Add(REGISTRATION_REQUIRED, new CodeType(407, ErrorType.auth));
            s_errors.Add(REMOTE_SERVER_NOT_FOUND, new CodeType(404, ErrorType.cancel));
            s_errors.Add(REMOTE_SERVER_TIMEOUT, new CodeType(504, ErrorType.wait));
            s_errors.Add(RESOURCE_CONSTRAINT, new CodeType(500, ErrorType.wait));
            s_errors.Add(SERVICE_UNAVAILABLE, new CodeType(503, ErrorType.cancel));
            s_errors.Add(SUBSCRIPTION_REQUIRED, new CodeType(407, ErrorType.auth));
            s_errors.Add(UNDEFINED_CONDITION, new CodeType(500, ErrorType.NONE));
            s_errors.Add(UNEXPECTED_REQUEST, new CodeType(400, ErrorType.wait));
        }

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
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Create an error element with the element name of the error condition.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Error GetStanzaError(XmlDocument doc, string condition)
        {
            if (!s_errors.Contains(condition))
                throw new ArgumentException("Unknown condition: " + condition, "condition");

            CodeType ct = (CodeType) s_errors[condition];
            return GetStanzaError(doc, ct.Type, ct.Code, condition);
        }

        /// <summary>
        /// Get an error element with a urn:ietf:params:xml:ns:xmpp-stanzas condition.
        /// Likely, you want the GetStanzaError(doc, condition) instead.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Error GetStanzaError(XmlDocument doc, ErrorType type, int code, string condition)
        {
            Error error = new Error(doc);
            error.ErrorType = type;
            error.Code = code;
            error.AppendChild(doc.CreateElement(condition, URI.STANZA_ERROR));
            return error;
        }

        /// <summary>
        /// Get an error stanza with a urn:ietf:params:xml:ns:xmpp-streams condition.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Error GetStreamError(XmlDocument doc, ErrorType type, int code, string condition)
        {
            Error error = new Error(doc);
            error.ErrorType = type;
            error.Code = code;
            error.AppendChild(doc.CreateElement(condition, URI.STREAM));
            return error;
        }

        /// <summary>
        /// The error code, as an integer.
        /// </summary>
        public int Code
        {
            get { return GetIntAttr("code"); }
            set { this.SetAttribute("code", value.ToString()); }
        }

        /// <summary>
        /// The type of the error
        /// </summary>
        public ErrorType ErrorType
        {
            get { return this.GetEnumAttr<ErrorType>("type"); }
            set { this.SetEnumAttr("type", value); }
        }

        /// <summary>
        /// The inner error condition element.
        /// </summary>
        public string Condition
        {
            get
            {
                foreach (XmlNode n in this.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Element)
                        continue;
                    if ((n.NamespaceURI != URI.STANZA_ERROR) &&
                        (n.NamespaceURI != URI.STREAM_ERROR))
                        continue;
                    return n.LocalName;
                }
                // uh-oh.  Old-school error.  See section 3 of XEP-86.
                switch (this.Code)
                {
                case 302: return REDIRECT;
                case 400: return BAD_REQUEST;
                case 401: return NOT_AUTHORIZED;
                case 402: return PAYMENT_REQUIRED;
                case 403: return FORBIDDEN;
                case 404: return ITEM_NOT_FOUND;
                case 405: return NOT_ALLOWED;
                case 406: return NOT_ACCEPTABLE;
                case 407: return REGISTRATION_REQUIRED;
                case 408: return REMOTE_SERVER_TIMEOUT;
                case 409: return CONFLICT;
                case 500: return INTERNAL_SERVER_ERROR;
                case 501: return FEATURE_NOT_IMPLEMENTED;
                case 502: return SERVICE_UNAVAILABLE;
                case 503: return SERVICE_UNAVAILABLE;
                case 504: return REMOTE_SERVER_TIMEOUT;
                case 510: return SERVICE_UNAVAILABLE;
                }
                // best we can do.
                return UNDEFINED_CONDITION;
            }
            set { this.InnerXml = ""; this.AddChild(GetStanzaError(this.OwnerDocument, value)); }
        }

        /// <summary>
        /// The error message.  Not used anymore (not I18N).
        /// </summary>
        public string Message
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }
}
