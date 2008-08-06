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
 * Jabber-Net is licensed under the LGPL.
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

            // Pubsub event notifications
            new QnameType("event",         URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.PubSubEvent)),
            new QnameType("associate",     URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventAssociate)),
            new QnameType("collection",    URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventCollection)),
            new QnameType("configuration", URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventConfiguration)),
            new QnameType("disassociate",  URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventDisassociate)),
            new QnameType("items",         URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventItems)),
            new QnameType("item",          URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.PubSubItem)),
            new QnameType("purge",         URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventPurge)),
            new QnameType("retract",       URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventRetract)),
            new QnameType("subscription",  URI.PUBSUB_EVENT, typeof(jabber.protocol.iq.EventSubscription)),

            // Pubsub owner use cases
            new QnameType("pubsub",        URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.PubSubOwner)),
            new QnameType("affiliations",  URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.OwnerAffliliations)),
            new QnameType("affiliation",   URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.OwnerAffiliation)),
            new QnameType("configure",     URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.OwnerConfigure)),
            new QnameType("default",       URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.OwnerDefault)),
            new QnameType("delete",        URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.OwnerDelete)),
            new QnameType("purge",         URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.OwnerPurge)),
            new QnameType("subscriptions", URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.OwnerSubscriptions)),
            new QnameType("subscription",  URI.PUBSUB_OWNER, typeof(jabber.protocol.iq.PubSubSubscription)),

            // Pubsub errors
            new QnameType("closed-node",                    URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.ClosedNode)),
            new QnameType("configuration-required",         URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.ConfigurationRequired)),
            new QnameType("invalid-jid",                    URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.InvalidJID)),
            new QnameType("invalid-options",                URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.InvalidOptions)),
            new QnameType("invalid-payload",                URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.InvalidPayload)),
            new QnameType("invalid-subid",                  URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.InvalidSubid)),
            new QnameType("item-forbidden",                 URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.ItemForbidden)),
            new QnameType("item-required",                  URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.ItemRequired)),
            new QnameType("jid-required",                   URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.JIDRequired)),
            new QnameType("max-items-exceeded",             URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.MaxItemsExceeded)),
            new QnameType("max-nodes-exceeded",             URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.MaxNodesExceeded)),
            new QnameType("nodeid-required",                URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.NodeIDRequired)),
            new QnameType("not-in-roster-group",            URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.NotInRosterGroup)),
            new QnameType("not-subscribed",                 URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.NotSubscribed)),
            new QnameType("payload-too-big",                URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.PayloadTooBig)),
            new QnameType("payload-required",               URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.PayloadRequired)),
            new QnameType("pending-subscription",           URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.PendingSubscription)),
            new QnameType("presence-subscription-required", URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.PresenceSubscriptionRequired)),
            new QnameType("subid-required",                 URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.SubidRequired)),
            new QnameType("unsupported",                    URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.Unsupported)),
            new QnameType("unsupported-access-model",       URI.PUBSUB_ERRORS, typeof(jabber.protocol.iq.UnsupportedAccessModel)),

            // Multi-user chat
            new QnameType("x",       URI.MUC, typeof(jabber.protocol.iq.RoomX)),
            new QnameType("history", URI.MUC, typeof(jabber.protocol.iq.History)),

            new QnameType("x",       URI.MUC_USER, typeof(jabber.protocol.iq.UserX)),
            new QnameType("decline", URI.MUC_USER, typeof(jabber.protocol.iq.Decline)),
            new QnameType("invite",  URI.MUC_USER, typeof(jabber.protocol.iq.Invite)),
            new QnameType("destroy", URI.MUC_USER, typeof(jabber.protocol.iq.Destroy)),
            new QnameType("item",    URI.MUC_USER, typeof(jabber.protocol.iq.RoomItem)),
            new QnameType("actor",   URI.MUC_USER, typeof(jabber.protocol.iq.RoomActor)),

            new QnameType("query",   URI.MUC_ADMIN, typeof(jabber.protocol.iq.AdminQuery)),
            new QnameType("item",    URI.MUC_ADMIN, typeof(jabber.protocol.iq.AdminItem)),

            new QnameType("query",   URI.MUC_OWNER, typeof(jabber.protocol.iq.OwnerQuery)),
            new QnameType("destroy", URI.MUC_OWNER, typeof(jabber.protocol.iq.OwnerDestroy)),
        };

        QnameType[] IPacketTypes.Types { get { return s_qnt; } }
    }
}
