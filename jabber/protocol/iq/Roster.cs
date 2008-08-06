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

namespace jabber.protocol.iq
{
    /// <summary>
    /// IQ packet with a roster query element inside.
    /// </summary>
    [SVN(@"$Id$")]
    public class RosterIQ : jabber.protocol.client.TypedIQ<Roster>
    {
        /// <summary>
        /// Create a roster IQ.
        /// </summary>
        /// <param name="doc"></param>
        public RosterIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// A roster query element.
    /// </summary>
    [SVN(@"$Id$")]
    public class Roster : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Roster(XmlDocument doc) : base("query", URI.ROSTER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Roster(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Add a roster item
        /// </summary>
        /// <returns></returns>
        public Item AddItem()
        {
            return CreateChildElement<Item>();
        }

        /// <summary>
        /// List of roster items
        /// </summary>
        /// <returns></returns>
        public Item[] GetItems()
        {
            return GetElements<Item>().ToArray();
        }
    }

    /// <summary>
    /// The current status of the subscription related to this item.
    /// </summary>
    [SVN(@"$Id$")]
    public enum Subscription
    {
        /// <summary>
        /// No subscription state has been specified.
        /// </summary>
        UNSPECIFIED = -1,
        /// <summary>
        /// Subscription to this person.  They are a lurkee.
        /// </summary>
        to,
        /// <summary>
        /// Subscription from this person.  They are a lurker.
        /// </summary>
        from,
        /// <summary>
        /// subscriptions in both ways.
        /// </summary>
        both,
        /// <summary>
        /// No subscription yet.  Often an Ask on this item.
        /// </summary>
        none,
        /// <summary>
        /// Remove this subscription from the local roster.
        /// </summary>
        remove,
    }

    /// <summary>
    /// An optional attribute specifying the current status of a request to this contact.
    /// </summary>
    [SVN(@"$Id$")]
    public enum Ask
    {
        /// <summary>
        /// No Ask specified.
        /// </summary>
        NONE = -1,
        /// <summary>
        /// this entity is asking to subscribe to that contact's presence
        /// </summary>
        subscribe,
        /// <summary>
        /// this entity is asking unsubscribe from that contact's presence
        /// </summary>
        unsubscribe
    }

    /// <summary>
    /// Roster items.
    /// </summary>
    [SVN(@"$Id$")]
    public class Item : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Item(XmlDocument doc) : base("item", URI.ROSTER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Item(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Item JID
        /// </summary>
        public JID JID
        {
            get { return GetAttr("jid"); }
            set { this.SetAttr("jid", value); }
        }

        /// <summary>
        /// The user's nick
        /// </summary>
        public string Nickname
        {
            get { return GetAttr("name"); }
            set { SetAttr("name", value); }
        }

        /// <summary>
        /// How are we subscribed?
        /// </summary>
        public Subscription Subscription
        {
            get { return GetEnumAttr<Subscription>("subscription"); }
            set { SetEnumAttr("subscription", value); }
        }

        /// <summary>
        /// Pending?
        /// </summary>
        public Ask Ask
        {
            get { return GetEnumAttr<Ask>("ask"); }
            set { SetEnumAttr("ask", value); }
        }

        /// <summary>
        /// Add an item group, or return an existing group with the given name
        /// </summary>
        /// <returns></returns>
        public Group AddGroup(string name)
        {
            Group g = GetGroup(name);
            if (g == null)
            {
                g = CreateChildElement<Group>();
                g.GroupName = name;
            }
            return g;
        }

        /// <summary>
        /// Remove a group of the given name.  Does nothing if that group is not found.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveGroup(string name)
        {
            foreach (Group g in GetElements<Group>())
            {
                if (g.GroupName == name)
                {
                    this.RemoveChild(g);
                    return;
                }
            }
        }

        /// <summary>
        /// List of item groups
        /// </summary>
        /// <returns></returns>
        public Group[] GetGroups()
        {
            return GetElements<Group>().ToArray();
        }

        /// <summary>
        /// Is this item in the specified group?
        /// </summary>
        /// <param name="name">The name of the group to check</param>
        /// <returns></returns>
        public bool HasGroup(string name)
        {
            foreach (Group g in GetElements<Group>())
            {
                if (g.GroupName == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get the group object of the given name in this item.
        /// If there is no group of that name, returns null.
        /// </summary>
        /// <param name="name">The name of the group to return</param>
        /// <returns>null if none found.</returns>
        public Group GetGroup(string name)
        {
            foreach (Group g in GetElements<Group>())
            {
                if (g.GroupName == name)
                    return g;
            }
            return null;
        }
    }

    /// <summary>
    /// Roster item groups.  &lt;group&gt;GroupName&lt;/group&gt;
    /// </summary>
    [SVN(@"$Id$")]
    public class Group : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Group(XmlDocument doc) : base("group", URI.ROSTER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Group(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Name of the group.
        /// </summary>
        public string GroupName
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }
}
