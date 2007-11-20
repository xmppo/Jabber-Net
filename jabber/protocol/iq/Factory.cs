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
using jabber.protocol;

namespace jabber.protocol.iq
{
    /// <summary>
    /// ElementFactory for all currently supported IQ namespaces.
    /// </summary>
    [SVN(@"$Id$")]
    public class Factory : IPacketTypes
    {
        private static QnameType[] s_qnt = new QnameType[]
        {
            new QnameType("query", URI.AUTH,     typeof(jabber.protocol.iq.Auth)),
            new QnameType("query", URI.REGISTER, typeof(jabber.protocol.iq.Register)),
            new QnameType("query", URI.ROSTER,   typeof(jabber.protocol.iq.Roster)),
            new QnameType("item",  URI.ROSTER,   typeof(jabber.protocol.iq.Item)),
            new QnameType("group", URI.ROSTER,   typeof(jabber.protocol.iq.Group)),
            new QnameType("query", URI.AGENTS,   typeof(jabber.protocol.iq.AgentsQuery)),
            new QnameType("agent", URI.AGENTS,   typeof(jabber.protocol.iq.Agent)),
            new QnameType("query", URI.OOB,      typeof(jabber.protocol.iq.OOB)),
            new QnameType("query", URI.TIME,     typeof(jabber.protocol.iq.Time)),
            new QnameType("query", URI.VERSION,  typeof(jabber.protocol.iq.Version)),
            new QnameType("query", URI.LAST,     typeof(jabber.protocol.iq.Last)),
            new QnameType("item",  URI.BROWSE,   typeof(jabber.protocol.iq.Browse)),
            new QnameType("geoloc",URI.GEOLOC,   typeof(jabber.protocol.iq.GeoLoc)),

            // VCard
            new QnameType("vCard", URI.VCARD, typeof(jabber.protocol.iq.VCard)),
            new QnameType("N",     URI.VCARD, typeof(jabber.protocol.iq.VCard.VName)),
            new QnameType("ORG",   URI.VCARD, typeof(jabber.protocol.iq.VCard.VOrganization)),
            new QnameType("TEL",   URI.VCARD, typeof(jabber.protocol.iq.VCard.VTelephone)),
            new QnameType("EMAIL", URI.VCARD, typeof(jabber.protocol.iq.VCard.VEmail)),
            new QnameType("GEO",   URI.VCARD, typeof(jabber.protocol.iq.VCard.VGeo)),
            new QnameType("PHOTO", URI.VCARD, typeof(jabber.protocol.iq.VCard.VPhoto)),
            new QnameType("ADR", URI.VCARD, typeof(jabber.protocol.iq.VCard.VAddress)),

            // Disco
            new QnameType("query",    URI.DISCO_ITEMS, typeof(jabber.protocol.iq.DiscoItems)),
            new QnameType("item",     URI.DISCO_ITEMS, typeof(jabber.protocol.iq.DiscoItem)),
            new QnameType("query",    URI.DISCO_INFO, typeof(jabber.protocol.iq.DiscoInfo)),
            new QnameType("identity", URI.DISCO_INFO, typeof(jabber.protocol.iq.DiscoIdentity)),
            new QnameType("feature",  URI.DISCO_INFO, typeof(jabber.protocol.iq.DiscoFeature)),

            // PubSub
            new QnameType("pubsub",        URI.PUBSUB, typeof(jabber.protocol.iq.PubSub)),
            new QnameType("affiliations",  URI.PUBSUB, typeof(jabber.protocol.iq.Affiliations)),
            new QnameType("create",        URI.PUBSUB, typeof(jabber.protocol.iq.Create)),
            new QnameType("items",         URI.PUBSUB, typeof(jabber.protocol.iq.Items)),
            new QnameType("publish",       URI.PUBSUB, typeof(jabber.protocol.iq.Publish)),
            new QnameType("retract",       URI.PUBSUB, typeof(jabber.protocol.iq.Retract)),
            new QnameType("subscribe",     URI.PUBSUB, typeof(jabber.protocol.iq.Subscribe)),
            new QnameType("subscriptions", URI.PUBSUB, typeof(jabber.protocol.iq.Subscriptions)),
            new QnameType("unsubscribe",   URI.PUBSUB, typeof(jabber.protocol.iq.Unsubscribe)),

            new QnameType("configure",     URI.PUBSUB, typeof(jabber.protocol.iq.Configure)),
            new QnameType("options",       URI.PUBSUB, typeof(jabber.protocol.iq.PubSubOptions)),
            new QnameType("affiliation",   URI.PUBSUB, typeof(jabber.protocol.iq.Affiliation)),
            new QnameType("item",          URI.PUBSUB, typeof(jabber.protocol.iq.PubSubItem)),
            new QnameType("subscription",  URI.PUBSUB, typeof(jabber.protocol.iq.PubSubSubscription)),

            new QnameType("event", URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.PubSubEvent)),
            new QnameType("items", URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.Items)),

            // Multi-user chat
            new QnameType("x",       URI.MUC, typeof(jabber.protocol.iq.ChatX)),
            new QnameType("history", URI.MUC, typeof(jabber.protocol.iq.History)),

            new QnameType("x",       URI.MUC_USER, typeof(jabber.protocol.iq.UserX)),
            new QnameType("decline", URI.MUC_USER, typeof(jabber.protocol.iq.Decline)),
            new QnameType("invite",  URI.MUC_USER, typeof(jabber.protocol.iq.Invite)),
            new QnameType("destroy", URI.MUC_USER, typeof(jabber.protocol.iq.Destroy)),
            new QnameType("item",    URI.MUC_USER, typeof(jabber.protocol.iq.ChatItem)),
            new QnameType("actor",   URI.MUC_USER, typeof(jabber.protocol.iq.ChatActor)),
            new QnameType("status",  URI.MUC_USER, typeof(jabber.protocol.iq.ChatStatus)),

            new QnameType("query",   URI.MUC_ADMIN, typeof(jabber.protocol.iq.AdminQuery)),
            new QnameType("item",    URI.MUC_ADMIN, typeof(jabber.protocol.iq.AdminItem)),

            new QnameType("query",   URI.MUC_OWNER, typeof(jabber.protocol.iq.OwnerQuery)),
            new QnameType("destroy", URI.MUC_OWNER, typeof(jabber.protocol.iq.OwnerDestroy)),
        };

        QnameType[] IPacketTypes.Types { get { return s_qnt; } }
    }
}
