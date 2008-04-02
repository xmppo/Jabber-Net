using System;
using System.Text;
using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
    /// <summary>
    /// A type-safe PubSub IQ for owner actions.
    /// </summary>
    /// <typeparam name="T">The type of command to create</typeparam>
    public class OwnerPubSubCommandIQ<T> : jabber.protocol.client.TypedIQ<OwnerPubSub<T>>
        where T : PubSubCommand
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="doc"></param>
        public OwnerPubSubCommandIQ(XmlDocument doc)
            : base(doc)
        {
        }

        /// <summary>
        /// Create, with node
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="node"></param>
        public OwnerPubSubCommandIQ(XmlDocument doc, string node)
            : base(doc)
        {
            Command.Node = node;
        }

        /// <summary>
        /// The command inside the pubsub element.
        /// </summary>
        public T Command
        {
            get { return Instruction.Command; }
            set { Instruction.Command = value; }
        }
    }

    /// <summary>
    /// A type-safe pubsub element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OwnerPubSub<T> : Element
        where T : PubSubCommand
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="doc"></param>
        public OwnerPubSub(XmlDocument doc)
            : base("pubsub", URI.PUBSUB_OWNER, doc)
        {
            CreateChildElement<T>();
        }

        /// <summary>
        /// The pubsub command
        /// </summary>
        public T Command
        {
            get { return GetChildElement<T>(); }
            set { ReplaceChild<T>(value); }
        }

        /// <summary>
        /// The type of pubsub command
        /// </summary>
        public PubSubCommandType CommandType
        {
            get { return Command.CommandType; }
        }
    }

    /// <summary>
    /// The pubsub container for owner operations.
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubOwner : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public PubSubOwner(XmlDocument doc)
            : base("pubsub", URI.PUBSUB_OWNER, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PubSubOwner(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The command inside.
        /// </summary>
        public PubSubCommand Command
        {
            get { return GetChildElement<PubSubCommand>(); }
        }

        /// <summary>
        /// The type of the included command
        /// </summary>
        public PubSubCommandType CommandType
        {
            get { return Command.CommandType; }
        }
    }

    /// <summary>
    /// Affiliations of all folks associated with a node
    /// </summary>
    [SVN(@"$Id$")]
    public class OwnerAffliliations : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public OwnerAffliliations(XmlDocument doc)
            : base("affiliations", URI.PUBSUB_OWNER, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerAffliliations(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Affiliations command
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.affiliations; }
        }

        /// <summary>
        /// Retrieve all of the affiliations
        /// </summary>
        /// <returns></returns>
        public OwnerAffiliation[] GetAffiliations()
        {
            return GetElements<OwnerAffiliation>().ToArray();
        }

        /// <summary>
        /// Add a new affiliation to the list.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public OwnerAffiliation AddAffiliation(AffiliationType type, string node)
        {
            OwnerAffiliation afil = CreateChildElement<OwnerAffiliation>();
            afil.Type = type;
            afil.Node = node;
            return afil;
        }
    }

    /// <summary>
    /// An affiliation for another user, retrieved by the owner.
    /// </summary>
    [SVN(@"$Id$")]
    public class OwnerAffiliation : Affiliation
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public OwnerAffiliation(XmlDocument doc)
            : base(doc, URI.PUBSUB_OWNER)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerAffiliation(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The JID of the affiliate.
        /// </summary>
        public JID JID
        {
            get { return GetAttr("jid"); }
            set { SetAttr("jid", value); }
        }
    }

    /// <summary>
    /// Owner-level configuration
    /// </summary>
    [SVN(@"$Id$")]
    public class OwnerConfigure : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public OwnerConfigure(XmlDocument doc)
            : base("configure", URI.PUBSUB_OWNER, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerConfigure(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Configure
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.configure; }
        }

        /// <summary>
        /// An x:data form that describes the node
        /// </summary>
        public jabber.protocol.x.Data MetaData
        {
            get { return GetChildElement<jabber.protocol.x.Data>(); }
            set { ReplaceChild<jabber.protocol.x.Data>(value); }
        }
    }

    /// <summary>
    /// The default configuration parameters
    /// </summary>
    [SVN(@"$Id$")]
    public class OwnerDefault : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public OwnerDefault(XmlDocument doc)
            : base("default", URI.PUBSUB_OWNER, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerDefault(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Configure
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.defaults; }
        }

        /// <summary>
        /// An x:data form that describes the node
        /// </summary>
        public jabber.protocol.x.Data MetaData
        {
            get { return GetChildElement<jabber.protocol.x.Data>(); }
            set { ReplaceChild<jabber.protocol.x.Data>(value); }
        }
    }

    /// <summary>
    /// Delete a node
    /// </summary>
    [SVN(@"$Id$")]
    public class OwnerDelete : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public OwnerDelete(XmlDocument doc)
            : base("delete", URI.PUBSUB_OWNER, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerDelete(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Delete
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.delete; }
        }
    }

    /// <summary>
    /// Purge all items from a node
    /// </summary>
    [SVN(@"$Id$")]
    public class OwnerPurge : PubSubCommand
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public OwnerPurge(XmlDocument doc)
            : base("purge", URI.PUBSUB_OWNER, doc)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerPurge(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Purge
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.purge;  }
        }
    }

    /// <summary>
    /// The subscription list
    /// </summary>
    [SVN(@"$Id$")]
    public class OwnerSubscriptions : Subscriptions
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public OwnerSubscriptions(XmlDocument doc)
            : base(doc, URI.PUBSUB_OWNER)
        {
        }

        /// <summary>
        /// Create for inbound
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerSubscriptions(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
    }

}
    