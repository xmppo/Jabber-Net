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
using System.Xml;

using bedrock.util;
using jabber;
using jabber.protocol;

namespace jabber.protocol.iq
{
    /// <summary>
    /// Affiliation with a MUC room, per user.
    /// </summary>
    public enum ChatAffiliation
    {
        /// <summary>
        /// No attribute specified
        /// </summary>
        UNSPECIFIED = -1,
        /// <summary>
        /// Administrator
        /// </summary>
        admin = 0,
        /// <summary>
        /// Member
        /// </summary>
        member,
        /// <summary>
        /// No affiliation
        /// </summary>
        none,
        /// <summary>
        /// Banned
        /// </summary>
        outcast,
        /// <summary>
        /// Room owner
        /// </summary>
        owner,
    }

    /// <summary>
    /// Current role in the room.  Initial role is set by affiliation, if it exits.
    /// </summary>
    public enum ChatRole
    {
        /// <summary>
        /// No attribute specified
        /// </summary>
        UNSPECIFIED = -1,
        /// <summary>
        /// Room moderator.  Can grant/revoke voice
        /// </summary>
        moderator = 0,
        /// <summary>
        /// No role
        /// </summary>
        none,
        /// <summary>
        /// Can speak
        /// </summary>
        participant,
        /// <summary>
        /// Can listen
        /// </summary>
        visitor,
    }

#region base protocol
    /// <summary>
    /// Presence to join a multi-user chat.
    /// </summary>
    [SVN(@"$Id$")]
    public class ChatPresence : jabber.protocol.client.Presence
    {
        /// <summary>
        /// Create, taking default room history, with no password.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="roomAndNick">A jid of the form room@conferenceServer/nick, where nick is the desired
        /// room nickname for this user</param>
        public ChatPresence(XmlDocument doc, JID roomAndNick)
            : base(doc)
        {
            this.To = roomAndNick;
            this.AppendChild(new ChatX(doc));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="roomAndNick"></param>
        /// <param name="password">Null for non-password rooms</param>
        /// TODO: getHistory?
        public ChatPresence(XmlDocument doc, JID roomAndNick, string password)
            : base(doc)
        {
            this.To = roomAndNick;
            ChatX x = new ChatX(doc);
            x.Password = password;
            this.AppendChild(x);
        }

        /// <summary>
        /// The X tag denoting MUC-ness.  Use this to access passord and history
        /// after creation.
        /// </summary>
        public ChatX X
        {
            get { return this["x", URI.MUC] as ChatX; }
            set { ReplaceChild(value); }
        }
    }


    /// <summary>
    /// X tag for presence when joining a room.
    /// </summary>
    [SVN(@"$Id$")]
    public class ChatX : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public ChatX(XmlDocument doc) 
            : base("x", URI.MUC, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ChatX(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Password to join room.  Null for no password.
        /// </summary>
        public string Password
        {
            get { return GetAttr("password"); }
            set { SetAttr("password", value); }
        }

        /// <summary>
        /// History options
        /// </summary>
        public History History
        {
            get { return GetOrCreateElement("history", URI.MUC_USER, typeof(History)) as History; }
            set { this.ReplaceChild(value); }
        }
    }

    /// <summary>
    /// How much history to retrieve upon joining a room.
    /// </summary>
    [SVN(@"$Id$")]
    public class History : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public History(XmlDocument doc) 
            : base("history", URI.MUC, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public History(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Maximum number of characters.  -1 if not specified.
        /// </summary>
        public int MaxChars
        {
            get { return GetIntAttr("maxchars"); }
            set { SetIntAttr("maxchars", value); }
        }
        /// <summary>
        /// Maximum number of stanzas.  -1 if not specified.
        /// </summary>
        public int MaxStanzas
        {
            get { return GetIntAttr("maxstanzas"); }
            set { SetIntAttr("maxstanzas", value); }
        }
        /// <summary>
        /// Number of seconds of history to retreive.
        /// </summary>
        public int Seconds
        {
            get { return GetIntAttr("seconds"); }
            set { SetIntAttr("seconds", value); }
        }
        /// <summary>
        /// Date of earliest history desired.
        /// DateTime.MinValue for not specified.
        /// </summary>
        public DateTime since
        {
            get { return GetDateTimeAttr("since"); }
            set { SetDateTimeAttr("since", value); }
        }
    }
#endregion


#region Users
    /// <summary>
    /// Information about users
    /// </summary>
    [SVN(@"$Id$")]
    public class UserX : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public UserX(XmlDocument doc) 
            : base("x", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public UserX(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Invite was declined
        /// </summary>
        public Decline Decline
        {
            get { return GetOrCreateElement("decline", null, typeof(Decline)) as Decline; }
            set { ReplaceChild(value); }
        }

        /// <summary>
        /// Room was destroyed
        /// </summary>
        public Destroy Destroy
        {
            get { return GetOrCreateElement("destroy", null, typeof(Destroy)) as Destroy; }
            set { ReplaceChild(value); }
        }

        /// <summary>
        /// The list of invites
        /// </summary>
        public Invite[] GetInvites()
        {
            XmlNodeList nl = GetElementsByTagName("invite", URI.MUC_USER);
            Invite[] items = new Invite[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                items[i] = (Invite)n;
                i++;
            }
            return items;
        }

        /// <summary>
        /// Add new invite
        /// </summary>
        /// <param name="to">Who to send the invite to?</param>
        /// <param name="reason">Why?  Null if none.</param>
        /// <returns></returns>
        public Invite AddInvite(JID to, string reason)
        {
            Invite inv = new Invite(this.OwnerDocument);
            inv.To = to;
            if (reason != null)
                inv.Reason = reason;
            this.AddChild(inv);
            return inv;
        }

        /// <summary>
        /// The associated item
        /// </summary>
        public ChatItem Item
        {
            get { return GetOrCreateElement("item", null, typeof(ChatItem)) as ChatItem; }
            set { ReplaceChild(value); }
        }

        /// <summary>
        /// The password to join the room.
        /// </summary>
        public string Password
        {
            get { return GetElem("password"); }
            set { SetElem("password", value); }
        }

        /// <summary>
        /// Status of the request
        /// </summary>
        public ChatStatus Status
        {
            get { return GetOrCreateElement("status", null, typeof(ChatStatus)) as ChatStatus; }
            set { ReplaceChild(value); }
        }
    }

    /// <summary>
    /// Invitee Declines Invitation
    /// </summary>
    public class Decline : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Decline(XmlDocument doc) 
            : base("decline", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Decline(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The From address
        /// </summary>
        public JID From
        {
            get
            {
                string from = this.GetAttr("from");
                if (from == null)
                    return null;
                return new JID(from);
            }
            set { SetAttr("from", (string)value); }
        }

        /// <summary>
        /// The TO address
        /// </summary>
        public JID To
        {
            get 
            {
                string to = this.GetAttr("to");
                if (to == null)
                    return null;
                return new JID(to);
            }
            set { SetAttr("to", (string)value); }
        }

        /// <summary>
        /// The reason the invitation was declined.  May be null.
        /// </summary>
        public string Reason
        {
            get { return GetElem("reason"); }
            set { SetElem("reason", value); }
        }
    }

    /// <summary>
    /// An invite to a room
    /// </summary>
    public class Invite : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Invite(XmlDocument doc)
            : base("invite", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Invite(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The From address
        /// </summary>
        public JID From
        {
            get
            {
                string from = this.GetAttr("from");
                if (from == null)
                    return null;
                return new JID(from);
            }
            set { SetAttr("from", (string)value); }
        }

        /// <summary>
        /// The TO address
        /// </summary>
        public JID To
        {
            get
            {
                string to = this.GetAttr("to");
                if (to == null)
                    return null;
                return new JID(to);
            }
            set { SetAttr("to", (string)value); }
        }

        /// <summary>
        /// The reason the invitation was declined.  May be null.
        /// </summary>
        public string Reason
        {
            get { return GetElem("reason"); }
            set { SetElem("reason", value); }
        }
    }

    /// <summary>
    /// A room was destroyed
    /// </summary>
    public class Destroy : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Destroy(XmlDocument doc)
            : base("destroy", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Destroy(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The JID of the destroyer
        /// </summary>
        public JID JID
        {
            get
            {
                string jid = this.GetAttr("jid");
                if (jid == null)
                    return null;
                return new JID(jid);
            }
            set { SetAttr("jid", (string)value); }
        }

        /// <summary>
        /// The reason the room was destroyed.  May be null.
        /// </summary>
        public string Reason
        {
            get { return GetElem("reason"); }
            set { SetElem("reason", value); }
        }
    }

    /// <summary>
    /// Item associated with a room.
    /// </summary>
    public class ChatItem : AdminItem
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public ChatItem(XmlDocument doc)
            : base("item", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ChatItem(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// This is a continuation from 1-to-1 chat.  Not widely implemented yet.
        /// </summary>
        public bool Continue
        {
            get
            {
                XmlElement c = this["continue", URI.MUC_USER];
                return (c != null);
            }
            set
            {
                if (value)
                    GetOrCreateElement("continue", URI.MUC_USER, null);
                else
                    RemoveElem("continue");
            }
        }
    }

    /// <summary>
    /// The JID associated with an item
    /// </summary>
    public class ChatActor : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public ChatActor(XmlDocument doc)
            : base("actor", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ChatActor(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The JID of the destroyer
        /// </summary>
        public JID JID
        {
            get
            {
                string jid = this.GetAttr("jid");
                if (jid == null)
                    return null;
                return new JID(jid);
            }
            set { SetAttr("jid", (string)value); }
        }
    }

    /// <summary>
    /// The status of a room operation.
    /// </summary>
    public class ChatStatus : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public ChatStatus(XmlDocument doc)
            : base("status", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ChatStatus(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Error code.
        /// </summary>
        public int Code
        {
            get { return GetIntAttr("code"); }
            set { SetIntAttr("code", value); }
        }
    }
#endregion 

#region admin
    /// <summary>
    /// An IQ with a AdminQuery inside.
    /// </summary>
    public class ChatAdminIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create a admin IQ, with a single muc#admin query element.
        /// </summary>
        /// <param name="doc"></param>
        public ChatAdminIQ(XmlDocument doc)
            : base(doc)
        {
            this.AppendChild(new AdminQuery(doc));
        }
    }

    /// <summary>
    /// Moderator use cases 
    /// </summary>
    public class AdminQuery : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public AdminQuery(XmlDocument doc)
            : base("query", URI.MUC_ADMIN, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public AdminQuery(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The list of invites
        /// </summary>
        public AdminItem[] GetItems()
        {
            XmlNodeList nl = GetElementsByTagName("item", URI.MUC_ADMIN);
            AdminItem[] items = new AdminItem[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                items[i] = (AdminItem)n;
                i++;
            }
            return items;
        }

        /// <summary>
        /// Add new item
        /// </summary>
        /// <returns></returns>
        public AdminItem AddItem()
        {
            AdminItem item = new AdminItem(this.OwnerDocument);
            this.AddChild(item);
            return item;
        }
    }


    /// <summary>
    /// Item associated with a room.
    /// </summary>
    public class AdminItem : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public AdminItem(XmlDocument doc)
            : base("item", URI.MUC_ADMIN, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public AdminItem(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Pass through.  I really wish C# would let me just call grand-superclass constructors.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="namespaceURI"></param>
        /// <param name="doc"></param>
        protected AdminItem(string localName, string namespaceURI, XmlDocument doc)
            : base(localName, namespaceURI, doc)
        {
        }

        /// <summary>
        /// The JID associated with this item
        /// </summary>
        public ChatActor Actor
        {
            get { return GetOrCreateElement("actor", null, typeof(ChatActor)) as ChatActor; }
            set { ReplaceChild(value); }
        }

        /// <summary>
        /// The reason the room was destroyed.  May be null.
        /// </summary>
        public string Reason
        {
            get { return GetElem("reason"); }
            set { SetElem("reason", value); }
        }

        /// <summary>
        /// The affiliation of the item
        /// </summary>
        public ChatAffiliation Affiliation
        {
            get { return (ChatAffiliation)GetEnumAttr("affiliation", typeof(ChatAffiliation)); }
            set { SetEnumAttr("affiliation", value); }
        }

        /// <summary>
        /// The role of the item
        /// </summary>
        public ChatRole Role
        {
            get { return (ChatRole)GetEnumAttr("role", typeof(ChatRole)); }
            set { SetEnumAttr("role", value); }
        }

        /// <summary>
        /// The JID of the item
        /// </summary>
        public JID JID
        {
            get
            {
                string jid = this.GetAttr("jid");
                if (jid == null)
                    return null;
                return new JID(jid);
            }
            set { SetAttr("jid", (string)value); }
        }

        /// <summary>
        /// The nickname of the item
        /// </summary>
        public string Nick
        {
            get { return GetAttr("nick"); }
            set { SetAttr("nick", value); }
        }
    }
#endregion

#region owner
    /// <summary>
    /// IQ with an OwnerQuery inside
    /// </summary>
    public class OwnerIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="doc"></param>
        public OwnerIQ(XmlDocument doc)
            : base(doc)
        {
            AppendChild(new OwnerQuery(doc));
        }
    }

    /// <summary>
    /// The query element inside an owner IQ.
    /// </summary>
    public class OwnerQuery : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public OwnerQuery(XmlDocument doc)
            : base("query", URI.MUC_OWNER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerQuery(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The jabber:x:data form for configuration
        /// </summary>
        public jabber.protocol.x.Data Form
        {
            get { return GetOrCreateElement("x", URI.XDATA, typeof(jabber.protocol.x.Data)) as jabber.protocol.x.Data; }
            set { ReplaceChild(value); }
        }

        /// <summary>
        /// Should we destroy the room?
        /// </summary>
        public OwnerDestroy Destroy
        {
            get { return GetOrCreateElement("destroy", URI.MUC_OWNER, typeof(OwnerDestroy)) as OwnerDestroy; }
            set { ReplaceChild(value); }
        }
    }

    /// <summary>
    /// Destroy the room
    /// </summary>
    public class OwnerDestroy : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public OwnerDestroy(XmlDocument doc)
            : base("destroy", URI.MUC_OWNER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OwnerDestroy(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Password to destroy room.  Null for no password.
        /// </summary>
        public string Password
        {
            get { return GetElem("password"); }
            set { SetElem("password", value); }
        }

        /// <summary>
        /// Reason to destroy room.  Null for no reason.
        /// </summary>
        public string Reason
        {
            get { return GetElem("reason"); }
            set { SetElem("reason", value); }
        }

        /// <summary>
        /// The JID of the destroyer.
        /// </summary>
        public JID JID
        {
            get
            {
                string jid = this.GetAttr("jid");
                if (jid == null)
                    return null;
                return new JID(jid);
            }
            set { SetAttr("jid", (string)value); }
        }
    }
#endregion

    /// <summary>
    /// Request for a unique room name.  Seems like just using a GUID on the 
    /// create request would be enough, but it's in XEP-45.
    /// </summary>
    public class UniqueIQ : jabber.protocol.client.IQ
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="doc"></param>
        public UniqueIQ(XmlDocument doc)
            : base(doc)
        {
            AppendChild(new UniqueRoom(doc));
        }    
    }

    /// <summary>
    /// A unique name for a room.
    /// </summary>
    public class UniqueRoom : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public UniqueRoom(XmlDocument doc)
            : base("unique", URI.MUC_UNIQUE, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public UniqueRoom(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }
        
        /// <summary>
        /// The room name returned by the server.  Note: must add conference server to this, 
        /// it is just the node.
        /// </summary>
        public string RoomNode
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }
}
