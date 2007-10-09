/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;


using bedrock.util;

namespace jabber.protocol
{
    /// <summary>
    /// Namespace constants for http://etherx.jabber.org/streams.
    /// </summary>
    [SVN(@"$Id$")]
    public class URI
    {
        /// <summary>
        /// XHTML namespace, for &lt;body&gt; element
        /// </summary>
        public const string XHTML  = "http://www.w3.org/1999/xhtml";
        /// <summary>
        /// XHTML-IM namespace, for &lt;html&gt; element
        /// </summary>
        public const string XHTML_IM = "http://jabber.org/protocol/xhtml-im";
        /// <summary>
        /// stream:stream
        /// </summary>
        public const string STREAM = "http://etherx.jabber.org/streams";
        /// <summary>
        /// Start-TLS feature namespace
        /// </summary>
        public const string START_TLS = "urn:ietf:params:xml:ns:xmpp-tls";
        /// <summary>
        /// XEP-138 compression feature namespace.  Not the same as for the protocol!
        /// </summary>
        public const string COMPRESS_FEATURE = "http://jabber.org/features/compress";
        /// <summary>
        /// XEP-138 compression protocol namespace.  Not the same as the feature!
        /// </summary>
        public const string COMPRESS = "http://jabber.org/protocol/compress";
        /// <summary>
        /// SASL feature namespace
        /// </summary>
        public const string SASL = "urn:ietf:params:xml:ns:xmpp-sasl";
        /// <summary>
        /// Start a session
        /// </summary>
        public const string SESSION = "urn:ietf:params:xml:ns:xmpp-session";
        /// <summary>
        /// Bind a resource
        /// </summary>
        public const string BIND = "urn:ietf:params:xml:ns:xmpp-bind";
        /// <summary>
        /// Stanza errors.  See RFC 3920, section 9.3.
        /// </summary>
        public const string STANZA_ERROR = "urn:ietf:params:xml:ns:xmpp-stanzas";
        /// <summary>
        /// Stream errors.  See RFC 3920, section 4.7.
        /// </summary>
        public const string STREAM_ERROR = "urn:ietf:params:xml:ns:xmpp-streams";
        /// <summary>
        /// Jabber client connections
        /// </summary>
        public const string CLIENT = "jabber:client";
        /// <summary>
        /// Jabber HTTP Binding connections
        /// </summary>
        public const string HTTP_BIND = "http://jabber.org/protocol/httpbind";
        /// <summary>
        /// Jabber component connections
        /// </summary>
        public const string ACCEPT = "jabber:component:accept";
        /// <summary>
        /// Jabber component connections, from the router
        /// </summary>
        public const string CONNECT = "jabber:component:connect";
        /// <summary>
        /// S2S connection
        /// </summary>
        public const string SERVER = "jabber:server";
        /// <summary>
        /// S2S dialback
        /// </summary>
        public const string DIALBACK = "jabber:server:dialback";
        // IQ
        /// <summary>
        /// Authentication
        /// </summary>
        public const string AUTH     = "jabber:iq:auth";
        /// <summary>
        /// Roster manipulation
        /// </summary>
        public const string ROSTER   = "jabber:iq:roster";
        /// <summary>
        /// Register users
        /// </summary>
        public const string REGISTER = "jabber:iq:register";
        /// <summary>
        /// Out-of-band (file transfer)
        /// </summary>
        public const string OOB      = "jabber:iq:oob";
        /// <summary>
        /// Server agents
        /// </summary>
        public const string AGENTS   = "jabber:iq:agents";
        /// <summary>
        /// Client or server current time
        /// </summary>
        public const string TIME     = "jabber:iq:time";
        /// <summary>
        /// Last activity
        /// </summary>
        public const string LAST     = "jabber:iq:last";
        /// <summary>
        /// Client or server version
        /// </summary>
        public const string VERSION  = "jabber:iq:version";
        /// <summary>
        /// Jabber Browsing
        /// </summary>
        public const string BROWSE   = "jabber:iq:browse";
        /// <summary>
        /// Profile information
        /// </summary>
        public const string VCARD    = "vcard-temp";

        /// <summary>
        /// Geographic locaiotn (lat/long).
        /// See XEP-80 (http://www.xmpp.org/extensions/xep-0080.html)
        /// </summary>
        public const string GEOLOC   = "http://jabber.org/protocol/geoloc";

        /// <summary>
        /// Discover items from an entity.
        /// </summary>
        public const string DISCO_ITEMS = "http://jabber.org/protocol/disco#items";
        /// <summary>
        /// Discover info about an entity item.
        /// </summary>
        public const string DISCO_INFO = "http://jabber.org/protocol/disco#info";

        // X
        /// <summary>
        /// Offline message timestamping.
        /// </summary>
        public const string XDELAY   = "jabber:x:delay";
        /// <summary>
        /// Out-of-band (file transfer)
        /// </summary>
        public const string XOOB     = "jabber:x:oob";
        /// <summary>
        /// Send roster entries to another user.
        /// </summary>
        public const string XROSTER  = "jabber:x:roster";
        /// <summary>
        /// The jabber:x:event namespace qualifies extensions used to request and respond to
        /// events relating to the delivery, display, and composition of messages.
        /// </summary>
        public const string XEVENT = "jabber:x:event";
        /// <summary>
        /// jabber:x:data, as described in XEP-0004.
        /// </summary>
        public const string XDATA = "jabber:x:data";

        /// <summary>
        /// jabber:iq:search.
        /// See XEP-55 (http://www.xmpp.org/extensions/xep-0055.html)
        /// </summary>
        public const string SEARCH = "jabber:iq:search";

        /// <summary>
        /// Multi-user chat.
        /// See XEP-45 (http://www.xmpp.org/extensions/xep-0045.html)
        /// </summary>
        public const string MUC = "http://jabber.org/protocol/muc";
        /// <summary>
        /// Multi-user chat user functions.
        /// See XEP-45 (http://www.xmpp.org/extensions/xep-0045.html)
        /// </summary>
        public const string MUC_USER = "http://jabber.org/protocol/muc#user";
        /// <summary>
        /// Multi-user chat admin functions.
        /// See XEP-45 (http://www.xmpp.org/extensions/xep-0045.html)
        /// </summary>
        public const string MUC_ADMIN = "http://jabber.org/protocol/muc#admin";
        /// <summary>
        /// Multi-user chat owner functions.
        /// See XEP-45 (http://www.xmpp.org/extensions/xep-0045.html)
        /// </summary>
        public const string MUC_OWNER = "http://jabber.org/protocol/muc#owner";

        /// <summary>
        /// Entity Capabilities.
        /// See XEP-115 (http://www.xmpp.org/extensions/xep-0115.html)
        /// </summary>
        public const string CAPS = "http://jabber.org/protocol/caps";

        /// <summary>
        /// Publish/Subscribe
        /// See XEP-0060 (http://www.xmpp.org/extensions/xep-0060.html)
        /// </summary>
        public const string PUBSUB = "http://jabber.org/protocol/pubsub";

    }
}
