using System;
using System.Xml;

using jabber;
using jabber.protocol;

namespace jabber.protocol.iq
{
    /// <summary>
    /// For nodes with a node access model of "whitelist", if the requesting 
    /// entity is not on the whitelist then the service MUST return a 
    /// not-allowed error, specifying a pubsub-specific error condition of closed-node.
    /// </summary>
	public class ClosedNode : Element
	{
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public ClosedNode(XmlDocument doc)
            : base("closed-node", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ClosedNode(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
	}

    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationRequired : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public ConfigurationRequired(XmlDocument doc)
            : base("configuration-required", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ConfigurationRequired(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class InvalidJID : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public InvalidJID(XmlDocument doc)
            : base("invalid-jid", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public InvalidJID(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvalidOptions : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public InvalidOptions(XmlDocument doc)
            : base("invalid-options", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public InvalidOptions(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvalidPayload : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public InvalidPayload(XmlDocument doc)
            : base("invalid-payload", URI.PUBSUB_ERRORS, doc)
        {
            
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public InvalidPayload(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvalidSubid : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public InvalidSubid(XmlDocument doc)
            : base("invalid-subid", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public InvalidSubid(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ItemForbidden : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public ItemForbidden(XmlDocument doc)
            : base("item-forbidden", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ItemForbidden(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ItemRequired : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public ItemRequired(XmlDocument doc)
            : base("item-required", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ItemRequired(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JIDRequired : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public JIDRequired(XmlDocument doc)
            : base("jid-required", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public JIDRequired(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MaxItemsExceeded : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public MaxItemsExceeded(XmlDocument doc)
            : base("max-items-exceeded", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public MaxItemsExceeded(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MaxNodesExceeded : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public MaxNodesExceeded(XmlDocument doc)
            : base("max-nodes-exceeded", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public MaxNodesExceeded(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NodeIDRequired : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public NodeIDRequired(XmlDocument doc)
            : base("nodeid-required", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public NodeIDRequired(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class NotInRosterGroup : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public NotInRosterGroup(XmlDocument doc)
            : base("not-in-roster-group", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public NotInRosterGroup(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NotSubscribed : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public NotSubscribed(XmlDocument doc)
            : base("not-subscribed", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public NotSubscribed(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PayloadTooBig : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public PayloadTooBig(XmlDocument doc)
            : base("payload-too-big", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PayloadTooBig(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PayloadRequired : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public PayloadRequired(XmlDocument doc)
            : base("payload-required", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PayloadRequired(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PendingSubscription : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public PendingSubscription(XmlDocument doc)
            : base("pending-subscription", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PendingSubscription(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PresenceSubscriptionRequired : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public PresenceSubscriptionRequired(XmlDocument doc)
            : base("presence-subscription-required", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PresenceSubscriptionRequired(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubidRequired : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public SubidRequired(XmlDocument doc)
            : base("subid-required", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public SubidRequired(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }
    
    /// <summary>
    /// Supported features
    /// </summary>
    [Dash]
    public enum PubSubFeature
    {
        /// <summary>
        /// None specified
        /// </summary>
        UNSPECIFIED = -1,
        /// <summary>
        /// Access authorizations
        /// </summary>
        access_authorize,
        /// <summary>
        /// Open Access
        /// </summary>
        access_open,
        /// <summary>
        /// Presence-based access control
        /// </summary>
        access_presence,
        /// <summary>
        /// Roster-based access control
        /// </summary>
        access_roster,
        /// <summary>
        /// Whitelist-based access control
        /// </summary>
        access_whitelist,
        /// <summary>
        /// Auto-creation of nodes
        /// </summary>
        auto_create,
        /// <summary>
        /// Auto-subscription to nodes
        /// </summary>
        auto_subscribe,
        /// <summary>
        /// Collection support
        /// </summary>
        collections,
        /// <summary>
        /// Configuration
        /// </summary>
        config_node,
        /// <summary>
        /// Create and configure atomically
        /// </summary>
        create_and_configure,
        /// <summary>
        /// Node creation
        /// </summary>
        create_nodes,
        /// <summary>
        /// Delete items
        /// </summary>
        delete_any,
        /// <summary>
        /// Delete nodes
        /// </summary>
        delete_nodes,
        /// <summary>
        /// Notify on some criteria, only
        /// </summary>
        filtered_notifications,
        /// <summary>
        /// Process pending subscription requests
        /// </summary>
        get_pending,
        /// <summary>
        /// The server can create unused node names
        /// </summary>
        instant_nodes,
        /// <summary>
        /// Items have IDs
        /// </summary>
        item_ids,
        /// <summary>
        /// Geting the last published item
        /// </summary>
        last_published,
        /// <summary>
        /// Time-based subscriptions are supported.
        /// </summary>
        leased_subscription,
        /// <summary>
        /// Node owners may manage subscriptions
        /// </summary>
        manage_subscriptions,
        /// <summary>
        /// The member affiliation is supported
        /// </summary>
        member_affiliation,
        /// <summary>
        /// Node meta-data is supported.
        /// </summary>
        meta_data,
        /// <summary>
        /// Node owners may modify affiliations.
        /// </summary>
        modify_affiliations,
        /// <summary>
        /// A single leaf node may be associated with multiple collections
        /// </summary>
        multi_collection,
        /// <summary>
        /// A single entity may subscribe to a node multiple times.
        /// </summary>
        multi_subscribe,
        /// <summary>
        /// The outcast affiliation is supported
        /// </summary>
        outcast_affiliation,
        /// <summary>
        /// Persistent items are supported.
        /// </summary>
        persistent_items,
        /// <summary>
        /// Presence-based delivery of event notifications is supported
        /// </summary>
        presence_notifications,
        /// <summary>
        /// Authorized contacts are automatically subscribed to a user's virtual pubsub service.
        /// </summary>
        presence_subscribe,
        /// <summary>
        /// Publishing items is supported (note: not valid for collection nodes).
        /// </summary>
        publish,
        /// <summary>
        /// Publishing an item with options is supported.
        /// </summary>
        publish_options,
        /// <summary>
        /// The publisher affiliation is supported.
        /// </summary>
        publisher_affiliation,
        /// <summary>
        /// Purging of nodes is supported.
        /// </summary>
        purge_nodes,
        /// <summary>
        /// Item retraction is supported.
        /// </summary>
        retract_items,
        /// <summary>
        /// Retrieval of current affiliations is supported.
        /// </summary>
        retrieve_affiliations,
        /// <summary>
        /// Retrieval of default node configuration is supported.
        /// </summary>
        retrieve_default,
        /// <summary>
        /// Item retrieval is supported.
        /// </summary>
        retrieve_items,
        /// <summary>
        /// Retrieval of current subscriptions is supported.
        /// </summary>
        retrieve_subscriptions,
        /// <summary>
        /// Subscribing and unsubscribing are supported.
        /// </summary>
        subscribe,
        /// <summary>
        /// Configuration of subscription options is supported.
        /// </summary>
        subscription_options,
        /// <summary>
        /// Notification of subscription state changes is supported.
        /// </summary>
        subscription_notifications,
    }

    /// <summary>
    /// 
    /// </summary>
    public class Unsupported : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public Unsupported(XmlDocument doc)
            : base("unsupported", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Unsupported(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Which feature was unsupported?
        /// </summary>
        public PubSubFeature Feature
        {
            get { return GetEnumAttr<PubSubFeature>("feature"); }
            set { SetEnumAttr("feature", value); }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UnsupportedAccessModel : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public UnsupportedAccessModel(XmlDocument doc)
            : base("unsupported-access-model", URI.PUBSUB_ERRORS, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public UnsupportedAccessModel(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }
}
