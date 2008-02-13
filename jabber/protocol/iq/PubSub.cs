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

namespace jabber.protocol.iq
{
    /// <summary>
    /// Different pubsub operations
    /// </summary>
    public enum PubSubCommandType
    {
        /// <summary>
        /// Retrieve the affiliations.  See: http://www.xmpp.org/extensions/xep-0060.html#entity-affiliations
        /// </summary>
        affiliations,
        /// <summary>
        /// Create a node. See: http://www.xmpp.org/extensions/xep-0060.html#owner-create
        /// </summary>
        create,
        /// <summary>
        /// Retrieve the items for a node. See http://www.xmpp.org/extensions/xep-0060.html#subscriber-retrieve
        /// </summary>
        items,
        /// <summary>
        /// Publish to a node.  See http://www.xmpp.org/extensions/xep-0060.html#publisher-publish
        /// </summary>
        publish,
        /// <summary>
        /// Delete an item from a node.  See: http://www.xmpp.org/extensions/xep-0060.html#publisher-delete
        /// </summary>
        retract,
        /// <summary>
        /// Subscribe to a node. See: http://www.xmpp.org/extensions/xep-0060.html#subscriber-subscribe
        /// </summary>
        subscribe,
        /// <summary>
        /// Retrieve subscriptions.  See: http://www.xmpp.org/extensions/xep-0060.html#entity-subscriptions
        /// </summary>
        subscriptions,
        /// <summary>
        /// Unsubscribe from a node.  See: http://www.xmpp.org/extensions/xep-0060.html#subscriber-unsubscribe
        /// </summary>
        unsubscribe,
        /// <summary>
        /// Delete a node. See: http://www.xmpp.org/extensions/xep-0060.html#owner-delete
        /// </summary>
        delete
    }

    /// <summary>
    /// A PubSub IQ
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create a pubsub IQ, with a single pubsub query element.
        /// </summary>
        /// <param name="doc"></param>
        public PubSubIQ(XmlDocument doc)
            : base(doc)
        {
            this.Query = new PubSub(doc);
        }

        /// <summary>
        /// Create a pubsub IQ, with a pubusub query element and the given subelement.
        /// </summary>
        /// <param name="doc">Document to create in</param>
        /// <param name="command">The pubsub command</param>
        /// <param name="node">Add this as a node attrbute of the command</param>
        public PubSubIQ(XmlDocument doc, PubSubCommandType command, string node)
            : base(doc)
        {
            this.Query = new PubSub(doc);

            PubSubCommand cmd = null;
            switch (command)
            {
            case PubSubCommandType.affiliations:
                cmd = new Affiliations(doc);
                break;
            case PubSubCommandType.create:
                cmd = new Create(doc);
                break;
            case PubSubCommandType.items:
                cmd = new Items(doc);
                break;
            case PubSubCommandType.publish:
                cmd = new Publish(doc);
                break;
            case PubSubCommandType.retract:
                cmd = new Retract(doc);
                break;
            case PubSubCommandType.subscribe:
                cmd = new Subscribe(doc);
                break;
            case PubSubCommandType.subscriptions:
                cmd = new Subscriptions(doc);
                break;
            case PubSubCommandType.unsubscribe:
                cmd = new Unsubscribe(doc);
                break;
            case PubSubCommandType.delete:
                cmd = new Delete(doc);
                break;
            }

            if (node != null)
                cmd.Node = node;
            this.Query.AppendChild(cmd);
        }

        /// <summary>
        /// Get the command from the pubsub element.
        /// </summary>
        public PubSubCommand Command
        {
            get
            {
                PubSub ps = (PubSub)this.Query;
                if (ps == null)
                    return null;
                return ps.Command;
            }
        }
    }

    internal class Delete : PubSubCommand
    {
        public Delete(XmlDocument doc) : base("delete", URI.PUBSUB, doc)
        {
        }

        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.delete; }
        }
    }

    /// <summary>
    /// Publish/Subscribe.  See XEP-60: http://www.xmpp.org/extensions/xep-0060.html
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSub : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public PubSub(XmlDocument doc) : base("pubsub", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PubSub(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The
        /// </summary>
        public PubSubCommand Command
        {
            get
            {
                if (!this.HasChildNodes)
                    return null;

                foreach (XmlNode child in this.ChildNodes)
                {
                    if (child is PubSubCommand)
                        return (PubSubCommand)child;
                }
                return null;
            }
        }
    }

    /// <summary>
    /// A PubSub command
    /// </summary>
    [SVN(@"$Id$")]
    public abstract class PubSubCommand : Element
    {
        /// <summary>
        /// Create a pubsub command.  Should not be called directly.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        protected PubSubCommand(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Create a pubsub command.  Should not be called directly.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="ns"></param>
        /// <param name="doc"></param>
        protected PubSubCommand(string prefix, string ns, XmlDocument doc)
            : base(prefix, ns, doc)
        {
        }

        /// <summary>
        /// The node this command applies to.
        /// </summary>
        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public abstract PubSubCommandType CommandType
        {
            get;
        }
    }


    /// <summary>
    /// Retrieve the affiliations.  See: http://www.xmpp.org/extensions/xep-0060.html#entity-affiliations
    /// </summary>
    [SVN(@"$Id$")]
    public class Affiliations : PubSubCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Affiliations(XmlDocument doc)
            : base("affiliations", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Affiliations(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.affiliations; }
        }

        /// <summary>
        /// Retrieve all of the affiliations
        /// </summary>
        /// <returns></returns>
        public Affiliation[] GetAffiliations()
        {
            XmlNodeList nl = GetElementsByTagName("affiliation", URI.PUBSUB);
            Affiliation[] items = new Affiliation[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                items[i] = (Affiliation)n;
                i++;
            }
            return items;
        }

        /// <summary>
        /// Add a new affiliation to the list.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public Affiliation AddAffiliation(AffiliationType type, string node)
        {
            Affiliation afil = new Affiliation(this.OwnerDocument);
            afil.Type = type;
            afil.Node = node;
            AddChild(afil);
            return afil;
        }
    }

    /// <summary>
    /// What affiliation does an entity have with respect to a node?
    /// </summary>
    [SVN(@"$Id$")]
    public enum AffiliationType
    {
        /// <summary>
        /// No affiliation specified
        /// </summary>
        NONE_SPECIFIED=-1,
        /// <summary>
        /// Can receive
        /// </summary>
        member=0,
        /// <summary>
        /// No affiliation
        /// </summary>
        none,
        /// <summary>
        /// Can't join
        /// </summary>
        outcast,
        /// <summary>
        /// All permisions
        /// </summary>
        owner,
        /// <summary>
        /// Can publish
        /// </summary>
        publisher,
    }

    /// <summary>
    /// The actual affiliation.
    /// </summary>
    [SVN(@"$Id$")]
    public class Affiliation : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Affiliation(XmlDocument doc)
            : base("affiliation", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Affiliation(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The node this affiliation applies to.
        /// </summary>
        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        /// <summary>
        /// Which affiliation?
        /// </summary>
        public AffiliationType Type
        {
            get { return (AffiliationType)GetEnumAttr("affiliation", typeof(AffiliationType)); }
            set
            {
                if (value == AffiliationType.NONE_SPECIFIED)
                    RemoveAttribute("affiliation");
                else
                    SetAttribute("affiliation", value.ToString());
            }
        }

    }

    /// <summary>
    /// Create a node. See: http://www.xmpp.org/extensions/xep-0060.html#owner-create
    /// </summary>
    [SVN(@"$Id$")]
    public class Create : PubSubCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Create(XmlDocument doc)
            : base("create", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Create(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.create; }
        }

        /// <summary>
        /// Does the element have a configure sibling?
        /// </summary>
        public bool HasConfigure
        {
            get
            {
                Configure config = GetConfiguration();
                return (config != null);
            }
            set
            {
                Configure config = GetConfiguration();
                if (value)
                {
                    if (config != null)
                        return;
                    config = new Configure(this.OwnerDocument);
                    ParentNode.AppendChild(config);
                }
                else
                {
                    if (config == null)
                        return;
                    ParentNode.RemoveChild(config);
                }
            }
        }

        /// <summary>
        /// Get the configuration.  Null if none exists.
        /// </summary>
        public Configure GetConfiguration()
        {
            return ParentNode["configure", URI.PUBSUB] as Configure;
        }
    }

    /// <summary>
    /// Configuring a pubsub node.  If the default is desired, it will be empty.  Otherwise it will contain an x:data.
    /// </summary>
    public class Configure : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Configure(XmlDocument doc) : base("configure", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Configure(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

    }

    /// <summary>
    /// Commands that deal with items.
    /// </summary>
    public abstract class PubSubItemCommand : PubSubCommand
    {
        /// <summary>
        /// Create a pubsub command.  Should not be called directly.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        protected PubSubItemCommand(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Create a pubsub command.  Should not be called directly.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="ns"></param>
        /// <param name="doc"></param>
        protected PubSubItemCommand(string prefix, string ns, XmlDocument doc)
            : base(prefix, ns, doc)
        {
        }

        /// <summary>
        /// Retrieve all of the items
        /// </summary>
        /// <returns></returns>
        public PubSubItem[] GetItems()
        {
            // Might be PUBSUB or PUBSUB_EVENT
            XmlNodeList nl = GetElementsByTagName("item", this.NamespaceURI);
            PubSubItem[] items = new PubSubItem[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                items[i] = (PubSubItem)n;
                i++;
            }
            return items;
        }

        /// <summary>
        /// Add a new item to the list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PubSubItem AddItem(string id)
        {
            PubSubItem item = new PubSubItem(this.OwnerDocument);
            if (id != null)
                item.ID = id;
            AddChild(item);
            return item;
        }

        /// <summary>
        /// Get a list of id's of deleted items.
        /// </summary>
        /// <returns></returns>
        public string[] GetRetractions()
        {
            // Might be PUBSUB or PUBSUB_EVENT
            XmlNodeList nl = GetElementsByTagName("retract", this.NamespaceURI);
            string[] ids = new string[nl.Count];
            int i=0;
            foreach (XmlElement n in nl)
            {
                ids[i] = n.GetAttribute("id");
                i++;
            }
            return ids;
        }

    }


    /// <summary>
    /// Retrieve the items for a node. See http://www.xmpp.org/extensions/xep-0060.html#subscriber-retrieve
    /// </summary>
    [SVN(@"$Id$")]
    public class Items : PubSubItemCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Items(XmlDocument doc)
            : base("items", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Items(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.items; }
        }

        /// <summary>
        /// The subscription ID these items apply to.
        /// </summary>
        public string SubID
        {
            get { return GetAttribute("subid"); }
            set { SetAttribute("subid", value); }
        }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        public int MaxItems
        {
            get { return GetIntAttr("max_items"); }
            set { SetAttribute("max_items", value.ToString()); }
        }
    }


    /// <summary>
    /// The items in a node
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubItem : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public PubSubItem(XmlDocument doc)
            : base("item", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PubSubItem(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The item id number
        /// </summary>
        public string ID
        {
            get { return GetAttribute("id"); }
            set { SetAttribute("id", value); }
        }

        /// <summary>
        /// The actual contents to publish.  Make sure to set a namespace!
        /// </summary>
        public XmlElement Contents
        {
            get { return GetFirstChildElement(); }
            set { this.InnerXml = ""; this.AddChild(value); }
        }
    }

    /// <summary>
    /// Publish to a node.  See http://www.xmpp.org/extensions/xep-0060.html#publisher-publish
    /// </summary>
    [SVN(@"$Id$")]
    public class Publish : PubSubItemCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Publish(XmlDocument doc)
            : base("publish", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Publish(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.publish; }
        }
    }

    /// <summary>
    /// Delete an item from a node.  See: http://www.xmpp.org/extensions/xep-0060.html#publisher-delete
    /// </summary>
    [SVN(@"$Id$")]
    public class Retract : PubSubItemCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Retract(XmlDocument doc)
            : base("retract", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Retract(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.retract; }
        }

        /// <summary>
        /// Don notifications?
        /// </summary>
        public bool Notify
        {
            get
            {
                string notify = GetAttribute("notify");
                if (notify == "true")
                    return true;
                if (notify == "1")
                    return true;
                return false;
            }
            set { SetAttribute("notify", value ? "true": "false"); }
        }
    }

    /// <summary>
    /// Subscribe to a node. See: http://www.xmpp.org/extensions/xep-0060.html#subscriber-subscribe
    /// </summary>
    [SVN(@"$Id$")]
    public class Subscribe : PubSubCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Subscribe(XmlDocument doc)
            : base("subscribe", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Subscribe(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.subscribe; }
        }

        /// <summary>
        /// The Jabber ID for this subscription
        /// </summary>
        public JID JID
        {
            get { return new JID(GetAttribute("jid")); }
            set { SetAttribute("jid", value.ToString()); }
        }

        /// <summary>
        /// Does the element have a options sibling?
        /// </summary>
        public bool HasOptions
        {
            get
            {
                PubSubOptions opts = GetOptions();
                return (opts != null);
            }
            set
            {
                PubSubOptions opts = GetOptions();
                if (value)
                {
                    if (opts != null)
                        return;
                    opts = new PubSubOptions(this.OwnerDocument);
                    ParentNode.AppendChild(opts);
                }
                else
                {
                    if (opts == null)
                        return;
                    ParentNode.RemoveChild(opts);
                }
            }
        }

        /// <summary>
        /// Get the configuration.  Null if none exists.
        /// </summary>
        public PubSubOptions GetOptions()
        {
            return ParentNode["options", URI.PUBSUB] as PubSubOptions;
        }
    }

    /// <summary>
    /// PubSub subscription options
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubOptions : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public PubSubOptions(XmlDocument doc)
            : base("options", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PubSubOptions(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The node these options apply to.
        /// </summary>
        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        /// <summary>
        /// The Jabber ID these options apply to.
        /// </summary>
        public JID JID
        {
            get { return new JID(GetAttribute("jid")); }
            set { SetAttribute("jid", value.ToString()); }
        }

        /// <summary>
        /// The subscription ID these options apply to.
        /// </summary>
        public string SubID
        {
            get { return GetAttribute("subid"); }
            set { SetAttribute("subid", value); }
        }


        /// <summary>
        /// Does the element have an XData child?
        /// </summary>
        public bool HasXData
        {
            get
            {
                jabber.protocol.x.Data xdata = GetXData();
                return (xdata != null);
            }
            set
            {
                jabber.protocol.x.Data xdata = GetXData();
                if (value)
                {
                    if (xdata != null)
                        return;
                    xdata = new jabber.protocol.x.Data(this.OwnerDocument);
                    xdata.Type = jabber.protocol.x.XDataType.submit;
                    ParentNode.AppendChild(xdata);
                }
                else
                {
                    if (xdata == null)
                        return;
                    ParentNode.RemoveChild(xdata);
                }
            }
        }

        /// <summary>
        /// Get the XData child, if it exists.
        /// </summary>
        /// <returns></returns>
        public jabber.protocol.x.Data GetXData()
        {
            return this["x", URI.XDATA] as jabber.protocol.x.Data;
        }

    }

    /// <summary>
    /// Retrieve subscriptions.  See: http://www.xmpp.org/extensions/xep-0060.html#entity-subscriptions
    /// </summary>
    [SVN(@"$Id$")]
    public class Subscriptions : PubSubCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Subscriptions(XmlDocument doc)
            : base("subscriptions", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Subscriptions(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.subscriptions; }
        }

        /// <summary>
        /// Retrieve all of the subscriptions
        /// </summary>
        /// <returns></returns>
        public PubSubSubscription[] GetSubscriptions()
        {
            XmlNodeList nl = GetElementsByTagName("subscription", URI.PUBSUB);
            PubSubSubscription[] items = new PubSubSubscription[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                items[i] = (PubSubSubscription)n;
                i++;
            }
            return items;
        }

        /// <summary>
        /// Add a new subscription to the list
        /// </summary>
        /// <returns></returns>
        public PubSubSubscription AddSubscription()
        {
            PubSubSubscription item = new PubSubSubscription(this.OwnerDocument);
            AddChild(item);
            return item;
        }
    }


    /// <summary>
    /// A single subscription
    /// </summary>
    public class PubSubSubscription : Element
    {
      /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public PubSubSubscription(XmlDocument doc)
            : base("subscription", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public PubSubSubscription(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }


        /// <summary>
        /// The node these options apply to.
        /// </summary>
        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        /// <summary>
        /// The Jabber ID these options apply to.
        /// </summary>
        public JID JID
        {
            get { return new JID(GetAttribute("jid")); }
            set { SetAttribute("jid", value.ToString()); }
        }

        /// <summary>
        /// The subscription ID these options apply to.
        /// </summary>
        public string SubID
        {
            get { return GetAttribute("subid"); }
            set { SetAttribute("subid", value); }
        }

        /// <summary>
        /// The subscription state
        /// </summary>
        public PubSubSubscriptionType Type
        {
            get { return (PubSubSubscriptionType)GetEnumAttr("subscription", typeof(PubSubSubscriptionType)); }
            set { SetAttribute("subscription", value.ToString()); }
        }
    }

    /// <summary>
    /// The subscription state of a given pubsub node.
    /// </summary>
    public enum PubSubSubscriptionType
    {
        /// <summary>
        /// No type given
        /// </summary>
        NONE_SPECIFIED = -1,
        /// <summary>
        /// No subscription
        /// </summary>
        none = 0,
        /// <summary>
        /// Sub is pending
        /// </summary>
        pending,
        /// <summary>
        /// Subscribed
        /// </summary>
        subscribed,
        /// <summary>
        /// Subscription needs to be configured
        /// </summary>
        unconfigured
    }

    /// <summary>
    /// Unsubscribe from a node.  See: http://www.xmpp.org/extensions/xep-0060.html#subscriber-unsubscribe
    /// </summary>
    [SVN(@"$Id$")]
    public class Unsubscribe : PubSubCommand
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Unsubscribe(XmlDocument doc)
            : base("unsubscribe", URI.PUBSUB, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Unsubscribe(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What type of command?
        /// </summary>
        public override PubSubCommandType CommandType
        {
            get { return PubSubCommandType.unsubscribe; }
        }

        /// <summary>
        /// The Jabber ID these options apply to.
        /// </summary>
        public JID JID
        {
            get { return new JID(GetAttribute("jid")); }
            set { SetAttribute("jid", value.ToString()); }
        }

        /// <summary>
        /// The subscription ID these options apply to.
        /// </summary>
        public string SubID
        {
            get { return GetAttribute("subid"); }
            set { SetAttribute("subid", value); }
        }
    }

    /// <summary>
    /// A pubsub event notifification.
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubEvent : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public PubSubEvent(XmlDocument doc) : base("event", URI.PUBSUB_EVENT, doc)
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
        /// Get the items for this event.
        /// </summary>
        public Items Items
        {
            get { return this["items", URI.PUBSUB_EVENT] as Items; }
        }
    }
}
