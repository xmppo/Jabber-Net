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
using jabber.protocol;

namespace jabber.protocol.stream
{
    /// <summary>
    /// These error conditions may be read by constrained clients. 
    /// They are used for connection manager problems, abstracting stream errors, 
    /// communication problems between the connection manager and the server, 
    /// and invalid client requests (binding syntax errors, possible attacks, etc.)
    /// </summary>
    [Dash]
    public enum ConditionType
    {
        /// <summary>
        ///  None specified
        /// </summary>
        UNSPECIFIED = -1,
        /// <summary>
        /// The target domain specified in the 'to' attribute or the target host or
        /// port specified in the 'route' attribute is no longer serviced by 
        /// the connection manager.
        /// </summary>
        host_gone,
        /// <summary>
        /// The target domain specified in the 'to' attribute or the target host
        /// or port specified in the 'route' attribute is unknown to the connection manager.
        /// </summary>
        host_unknown,
        /// <summary>
        /// The initialization element lacks a 'to' or 'route' attribute (or the 
        /// attribute has no value) but the connection manager requires one.
        /// </summary>
        improper_addressing,
        /// <summary>
        /// The connection manager has experienced an internal error that prevents 
        /// it from servicing the request.
        /// </summary>
        internal_server_error,
        /// <summary>
        /// The connection manager was unable to connect to, or unable to 
        /// connect securely to, or has lost its connection to, the server.
        /// </summary>
        remote_connection_failed,
        /// <summary>
        /// Encapsulates an error in the protocol being transported.
        /// </summary>
        remote_stream_error,
        /// <summary>
        /// The connection manager does not operate at this URI 
        /// (e.g., the connection manager accepts only SSL or TLS connections at 
        /// some https: URI rather than the http: URI requested by the client). 
        /// The client may try POSTing to the URI in the content of the <uri/> child element.
        /// </summary>
        see_other_uri,
        /// <summary>
        /// The connection manager is being shut down. All active HTTP sessions are
        /// being terminated. No new sessions can be created.
        /// </summary>
        system_shutdown,
        /// <summary>
        /// The error is not one of those defined herein; the connection manager SHOULD 
        /// include application-specific information in the content of the <body/> wrapper.
        /// </summary>
        undefined_condition
    };

    /// <summary>
    /// Is this an error or a termination?
    /// </summary>
    public enum BodyType
    {
        /// <summary>
        /// None specified
        /// </summary>
        UNSPECIFIED = -1,
        /// <summary>
        /// Error encapsulated in response
        /// </summary>
        error,
        /// <summary>
        /// Terminate the stream
        /// </summary>
        terminate
    };
    
    /// <summary>
    /// An HTTP Binding body element, which encapsulates stanzas.
    /// See XEP-124 and XEP-206 for details.
    /// </summary>
    [SVN(@"$id$")]
    public class Body : Element
    {
        /// <summary>
        /// Create for outbound
        /// </summary>
        /// <param name="doc"></param>
        public Body(XmlDocument doc) : base("body", URI.HTTP_BIND, doc)
        {
        }

        /// <summary>
        /// Create inbound instance
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Body(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The child elements of the body.  0 or more stanzas.
        /// </summary>
        public ElementList Contents
        {
            get { return new ElementList(this); }
            set
            {
                InnerXml = "";
                foreach (XmlElement el in value)
                    AddChild(el);
            }
        }

        /// <summary>
        /// The content encodings that server can handle.
        /// </summary>
        public string[] Accept
        {
            get { return GetAttr("accept").Split(new char[] {','}); }
            set { SetAttr("accept", string.Join(",", value)); }
        }

        /// <summary>
        /// Acknowledgement of a given RID.
        /// </summary>
        public long Ack
        {
            get { return GetLongAttr("ack"); }
            set { SetLongAttr("ack", value); }
        }

        /// <summary>
        /// Stream ID for digest auth calculations
        /// </summary>
        public string AuthID
        {
            get { return GetAttr("authid"); }
            set { SetAttr("authid", value); }
        }

        /// <summary>
        /// The charsets supported by the server.  Almost always just UTF8, if it exists.
        /// </summary>
        public string[] Charsets
        {
            get { return GetAttr("accept").Split(new char[] {' '}); }
            set { SetAttr("accept", string.Join(" ", value)); }
        }

        /// <summary>
        /// The error condition, if this is an error.
        /// </summary>
        public ConditionType Condition
        {
            get { return GetEnumAttr<ConditionType>("condition"); }
            set { SetEnumAttr("condition", value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get { return GetAttr("content"); }
            set { SetAttr("content", value); }
        }

        /// <summary>
        /// A client MAY include a 'from' attribute to enable the 
        /// connection manager to forward its identity to the server.
        /// </summary>
        public string From
        {
            get { return GetAttr("from"); }
            set { SetAttr("from", value); }
        }

        /// <summary>
        /// This attribute specifies the maximum number of requests the connection manager 
        /// is allowed to keep waiting at any one time during the session. If the client 
        /// is not able to use HTTP Pipelining then this SHOULD be set to "1".
        /// </summary>
        public int Hold
        {
            get { return GetIntAttr("hold"); }
            set { SetIntAttr("hold", value); }
        }

        /// <summary>
        /// This attribute specifies the longest allowable inactivity period (in seconds). 
        /// This enables the client to ensure that the periods with no requests pending 
        /// are never too long (see Polling Sessions and Inactivity).
        /// </summary>
        public int Inactivity
        {
            get { return GetIntAttr("inactivity"); }
            set { SetIntAttr("inactivity", value); }
        }
        
        /// <summary>
        /// The client MUST set the 'key' attribute of all subsequent requests to the 
        /// value of the next key in the generated sequence (decrementing from K(n-1) 
        /// towards K(1) with each request sent).
        /// </summary>
        public string Key
        {
            get { return GetAttr("key"); }
            set { SetAttr("key", value); }
        }
        
        /// <summary>
        /// If the connection manager supports session pausing (see Inactivity) then it 
        /// SHOULD advertise that to the client by including a 'maxpause' attribute in 
        /// the session creation response element. The value of the attribute indicates 
        /// the maximum length of a temporary session pause (in seconds) that a client
        /// MAY request.
        /// </summary>
        public int MaxPause
        {
            get { return GetIntAttr("maxpause"); }
            set { SetIntAttr("maxpause", value); }
        }
        
        /// <summary>
        /// The client MUST set the 'newkey' attribute of the first request in the session to the value K(n).
        /// </summary>
        public string NewKey
        {
            get { return GetAttr("newkey"); }
            set { SetAttr("newkey", value); }
        }
        
        /// <summary>
        /// If a client encounters an exceptional temporary situation during which it 
        /// will be unable to send requests to the connection manager for a period of 
        /// time greater than the maximum inactivity period (e.g., while a runtime 
        /// environment changes from one web page to another), and if the connection 
        /// manager included a 'maxpause' attribute in its Session Creation Response,
        /// then the client MAY request a temporary increase to the maximum inactivity 
        /// period by including a 'pause' attribute in a request.
        /// </summary>
        public int Pause
        {
            get { return GetIntAttr("pause"); }
            set { SetIntAttr("pause", value); }
        }
        
        /// <summary>
        /// This attribute specifies the shortest allowable polling interval (in seconds). 
        /// This enables the client to not send empty request elements more often than desired.
        /// </summary>
        public int Polling
        {
            get { return GetIntAttr("polling"); }
            set { SetIntAttr("polling", value); }
        }
        
        /// <summary>
        /// After receiving a request with an 'ack' value less than the 'rid' of the last
        /// request that it has already responded to, the connection manager MAY inform 
        /// the client of the situation by sending its next response immediately instead
        /// of waiting until it has stanzas to send to the client (e.g., if some time
        /// has passed since it responded). In this case it SHOULD include a 'report'
        /// attribute set to one greater than the 'ack' attribute it received from the 
        /// client, and a 'time' attribute set to the number of milliseconds since it 
        /// sent the response associated with the 'report' attribute.
        /// </summary>
        public int Report
        {
            get { return GetIntAttr("report"); }
            set { SetIntAttr("report", value); }
        }
        
        /// <summary>
        /// This attribute enables the connection manager to limit the number of 
        /// simultaneous requests the client makes. The RECOMMENDED values are 
        /// either "2" or one more than the value of the 'hold' attribute specified 
        /// in the session request.
        /// </summary>
        public int Requests
        {
            get { return GetIntAttr("requests"); }
            set { SetIntAttr("requests", value); }
        }

        /// <summary>
        /// Request ID.  Needs to start out random, and increment by one for each new BOSH
        /// request.
        /// </summary>
        public long RID
        {
            get { return GetLongAttr("rid"); }
            set { SetLongAttr("rid", value); }
        }

        /// <summary>
        /// connection manager MAY be configured to enable sessions with more than one 
        /// server in different domains.  When requesting a session with such a 'proxy' 
        /// connection manager, a client SHOULD include a 'route' attribute that 
        /// specifies the protocol, hostname, and port of the server with which it 
        /// wants to communicate, formatted as "proto:host:port" 
        /// (e.g., "xmpp:jabber.org:9999").
        /// </summary>
        public string Route
        {
            get { return GetAttr("route"); }
            set { SetAttr("route", value); }
        }

        /// <summary>
        /// A client MAY include a 'secure' attribute to specify that communications 
        /// between the connection manager and the server must be "secure". (Note: The
        /// 'secure' attribute is of type xs:boolean (see XML Schema Part 2) and the 
        /// default value is "false". [17]) If a connection manager receives a session
        /// request with the 'secure' attribute set to "true" or "1", then it MUST 
        /// respond to the client with a remote-connection-failed error as soon as it
        /// determines that it cannot communicate in a secure way with the server.
        /// </summary>
        public bool Secure
        {
            get
            {
                string s = GetAttr("secure");
                switch (s)
                {
                case "true":
                    return true;
                case "1":
                    return true;
                default:
                    return false;
                }
            }
            set
            {
                if (value)
                    SetAttr("secure", "true");
                else
                    SetAttr("secure", null);
            }
        }

        /// <summary>
        /// Stream ID
        /// </summary>
        public string SID
        {
            get { return GetAttr("sid"); }
            set { SetAttr("sid", value); }
        }
        
        /// <summary>
        /// If a connection manager supports the multi-streams feature, it MUST 
        /// include a 'stream' attribute in its Session Creation Response. If a
        /// client does not receive the 'stream' attribute then it MUST assume 
        /// that the connection manager does not support the feature. [22]
        /// 
        /// The 'stream' attribute identifies the first stream to be opened for 
        /// the session. The value of each 'stream' attribute MUST be an opaque 
        /// and unpredictable name that is unique within the context of the 
        /// connection manager application.
        /// </summary>
        public string Stream
        {
            get { return GetAttr("stream"); }
            set { SetAttr("stream", value); }
        }
        
        /// <summary>
        /// After receiving a request with an 'ack' value less than the 'rid' 
        /// of the last request that it has already responded to, the connection 
        /// manager MAY inform the client of the situation by sending its next
        /// response immediately instead of waiting until it has stanzas to
        /// send to the client (e.g., if some time has passed since it responded).
        /// In this case it SHOULD include a 'report' attribute set to one greater
        /// than the 'ack' attribute it received from the client, and a 'time' 
        /// attribute set to the number of milliseconds since it sent the response
        /// associated with the 'report' attribute.
        /// </summary>
        public int Time
        {
            get { return GetIntAttr("time"); }
            set { SetIntAttr("time", value); }
        }

        /// <summary>
        /// This attribute specifies the target domain of the first stream.
        /// </summary>
        public string To
        {
            get { return GetAttr("to"); }
            set { SetAttr("to", value); }
        }

        /// <summary>
        /// At any time, the client MAY gracefully terminate the session by sending a <body/> 
        /// element with a 'type' attribute set to "terminate". The termination request 
        /// MAY include one or more stanzas that the connection manager MUST forward to 
        /// the server to ensure graceful logoff.
        /// </summary>
        public BodyType Type
        {
            get { return GetEnumAttr<BodyType>("type"); }
            set { SetEnumAttr("type", value); }         
        }
        
        /// <summary>
        /// This attribute specifies the highest version of the BOSH protocol 
        /// that the client supports. The numbering scheme is "major.minor" 
        /// (where the minor number MAY be incremented higher than a single digit, 
        /// so it MUST be treated as a separate integer). Note: The 'ver' attribute 
        /// should not be confused with the version of any protocol being transported.
        /// </summary>
        public string Ver
        {
            get { return GetAttr("ver"); }
            set { SetAttr("ver", value); }
        }

        /// <summary>
        /// This attribute specifies the longest time (in seconds) that the connection
        /// manager is allowed to wait before responding to any request during the session. 
        /// This enables the client to limit the delay before it discovers any network failure, 
        /// and to prevent its HTTP/TCP connection from expiring due to inactivity.
        /// </summary>
        public int Wait
        {
            get { return GetIntAttr("wait"); }
            set { SetIntAttr("wait", value); }
        }
   }
}