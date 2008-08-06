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
using System.Xml;

using bedrock.util;
using jabber;
using jabber.protocol;

namespace jabber.protocol.iq
{
    /// <summary>
    /// Publish/Subscribe.  See XEP-60: http://www.xmpp.org/extensions/xep-0060.html
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubEvent : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public PubSubEvent(XmlDocument doc)
            : base("event", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PubSubEvent(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The PubSub command associated with this instruction
        /// </summary>
        public PubSubCommand Command
        {
            get { return GetChildElement<PubSubCommand>(); }
        }
    }

    /// <summary>
    /// Notification for item deletion.
    /// </summary>
    [SVN(@"$Id$")]
    public class EventRetract : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public EventRetract(XmlDocument doc)
            : base("retract", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventRetract(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// When in an event, there may be an ID as an attribute.
        /// </summary>
        public string ID
        {
            get { return GetAttr("id"); }
            set { SetAttr("id", value); }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [SVN(@"$Id$")]
    public class EventCollection : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public EventCollection(XmlDocument doc)
            : base("collection", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventCollection(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// A collection notification
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.collection; }
        }

        /// <summary>
        /// The new node associated with the collection
        /// </summary>
        public EventAssociate Associate
        {
            get { return GetChildElement<EventAssociate>(); }
            set { ReplaceChild<EventAssociate>(value); }
        }
        /// <summary>
        /// The node removed from the collection
        /// </summary>
        public EventDisassociate Disassociate
        {
            get { return GetChildElement<EventDisassociate>(); }
            set { ReplaceChild <EventDisassociate>(value); }
        }
    }

    /// <summary>
    /// Nodes added to a collection
    /// </summary>
    [SVN(@"$Id$")]
    public class EventAssociate : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public EventAssociate(XmlDocument doc)
            : base("associate", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventAssociate(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The new node name
        /// </summary>
        public string Node
        {
            get { return GetAttr("node"); }
            set { SetAttr("node", value); }
        }

        /// <summary>
        /// An x:data form that describes the new node
        /// </summary>
        public jabber.protocol.x.Data MetaData
        {
            get { return GetChildElement<jabber.protocol.x.Data>(); }
            set { ReplaceChild<jabber.protocol.x.Data>(value); }
        }
    }

    /// <summary>
    /// Nodes removed from a collection
    /// </summary>
    [SVN(@"$Id$")]
    public class EventDisassociate : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public EventDisassociate(XmlDocument doc)
            : base("disassociate", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventDisassociate(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The removed node name
        /// </summary>
        public string Node
        {
            get { return GetAttr("node"); }
            set { SetAttr("node", value); }
        }
    }

    /// <summary>
    /// Pubsub items notification.  This is the main reason for XEP-60 to have been written.
    /// </summary>
    [SVN(@"$Id$")]
    public class EventItems : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public EventItems(XmlDocument doc)
            : base("items", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventItems(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Retrieve all of the new items
        /// </summary>
        /// <returns></returns>
        public PubSubItem[] GetItems()
        {
            return GetElements<PubSubItem>().ToArray();
        }

        /// <summary>
        /// Add a new item to the list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PubSubItem AddItem(string id)
        {
            PubSubItem item = new PubSubItem(OwnerDocument, URI.PUBSUB_EVENT);
            AddChild(item);
            item.ID = id;
            return item;
        }

        /// <summary>
        /// Get a list of id's of deleted items.
        /// </summary>
        /// <returns></returns>
        public string[] GetRetractions()
        {
            TypedElementList<EventRetract> nl = GetElements<EventRetract>();
            string[] ids = new string[nl.Count];
            int i = 0;
            foreach (EventRetract item in nl)
                ids[i++] = item.ID;
            return ids;
        }

        /// <summary>
        /// Add a new item to the list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventRetract AddRetract(string id)
        {
            EventRetract item = CreateChildElement<EventRetract>();
            item.ID = id;
            return item;
        }

        /// <summary>
        /// A configuration event
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.items; }
        }
    }

    /// <summary>
    /// New node configuration
    /// </summary>
    [SVN(@"$Id$")]
    public class EventConfiguration : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public EventConfiguration(XmlDocument doc)
            : base("configuration", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventConfiguration(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// A configuration event
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.configuration; }
        }

        /// <summary>
        /// An x:data form that describes the new configuration
        /// </summary>
        public jabber.protocol.x.Data MetaData
        {
            get { return GetChildElement<jabber.protocol.x.Data>(); }
            set { ReplaceChild<jabber.protocol.x.Data>(value); }
        }
    }

    /// <summary>
    /// All of the items in a node have been deleted.
    /// </summary>
    [SVN(@"$Id$")]
    public class EventPurge : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public EventPurge(XmlDocument doc)
            : base("purge", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventPurge(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// A purge event
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.purge; }
        }
    }

    /// <summary>
    /// Subscription state has changed
    /// </summary>
    [SVN(@"$Id$")]
    public class EventSubscription : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public EventSubscription(XmlDocument doc)
            : base("subscription", URI.PUBSUB_EVENT, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public EventSubscription(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// A subscription event
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.subscription; }
        }

        /// <summary>
        /// When does this subscription expire?  DateTime.MIN_VALUE for none.
        /// </summary>
        public DateTime Expiry
        {
            get { return GetDateTimeAttr("expiry"); }
            set { SetDateTimeAttr("expiry", value); }
        }

        /// <summary>
        /// The JID of the subscriber
        /// </summary>
        public JID JID
        {
            get { return GetAttr("jid"); }
            set { SetAttr("jid", value); }
        }

        /// <summary>
        /// The ID of the subscription
        /// </summary>
        public string SubscriptionID
        {
            get { return GetAttr("subid"); }
            set { SetAttr("subid", value); }
        }

        /// <summary>
        /// The subscription state
        /// </summary>
        public PubSubSubscriptionType Subscription
        {
            get { return GetEnumAttr<PubSubSubscriptionType>("subscription"); }
            set { SetEnumAttr("subscription", value); }
        }
    }
}
