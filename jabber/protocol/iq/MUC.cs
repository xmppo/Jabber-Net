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
#region enums
    /// <summary>
    /// Affiliation with a MUC room, per user.
    /// </summary>
    public enum RoomAffiliation
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
    public enum RoomRole
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

    /// <summary>
    /// Possible room status values.
    /// </summary>
    public enum RoomStatus
    {
        /// <summary>
        /// An invalid or unknown RoomStatus
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        /// Inform user that any occupant is allowed to see the user's full JID.
        /// </summary>
        NON_ANONYMOUS_JOIN = 100,

        /// <summary>
        /// Inform user that his or her affiliation changed while not in the room
        /// </summary>
        AFILLIATION_CHANGE = 101,

        /// <summary>
        /// Inform occupants that room now shows unavailable members
        /// </summary>
        SHOW_UNAVAILABLE = 102,

        /// <summary>
        /// Inform occupants that room now does not show unavailable members
        /// </summary>
        NO_SHOW_UNAVAILABLE = 103,

        /// <summary>
        /// Inform occupants that a non-privacy-related room configuration change has occurred
        /// </summary>
        PRIVACY_CHANGE = 104,

        /// <summary>
        /// Inform user that presence refers to one of its own room occupants
        /// </summary>
        SELF = 110,

        /// <summary>
        /// Inform occupants that room logging is now enabled
        /// </summary>
        LOGGING_ENABLED = 170,

        /// <summary>
        /// Inform occupants that room logging is now disabled
        /// </summary>
        LOGGING_DISABLED = 171,

        /// <summary>
        /// Inform occupants that the room is now non-anonymous
        /// </summary>
        NON_ANONYMOUS = 172,

        /// <summary>
        /// Inform occupants that the room is now semi-anonymous
        /// </summary>
        SEMI_ANONYMOUS = 173,

        /// <summary>
        /// Inform occupants that the room is now fully-anonymous
        /// </summary>
        ANONYMOUS = 174,

        /// <summary>
        /// Inform user that a new room has been created
        /// </summary>
        CREATED = 201,

        /// <summary>
        /// Inform user that service has assigned or modified occupant's roomnick
        /// </summary>
        NICK_CHANGED = 210,

        /// <summary>
        /// Inform user that he or she has been banned from the room
        /// </summary>
        BANNED = 301,

        /// <summary>
        /// Inform all occupants of new room nickname
        /// </summary>
        NEW_NICK = 303,

        /// <summary>
        /// Inform user that he or she has been kicked from the room
        /// </summary>
        KICKED = 307,

        /// <summary>
        /// Inform user that he or she is being removed from the room
        /// because of an affiliation change
        /// </summary>
        REMOVED_AFFILIATION = 321,

        /// <summary>
        /// Inform user that he or she is being removed from the room
        /// because the room has been changed to members-only and the user
        /// is not a member
        /// </summary>
        REMOVED_NONMEMBER = 322,

        /// <summary>
        /// Inform user that he or she is being removed from the room
        /// because of a system shutdown
        /// </summary>
        REMOVED_SHUTDOWN = 332,
    }
#endregion

#region base protocol
    /// <summary>
    /// Presence to join a multi-user chat.
    /// </summary>
    [SVN(@"$Id$")]
    public class RoomPresence : jabber.protocol.client.Presence
    {
        /// <summary>
        /// Create, taking default room history, with no password.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="roomAndNick">A jid of the form room@conferenceServer/nick, where nick is the desired
        /// room nickname for this user</param>
        public RoomPresence(XmlDocument doc, JID roomAndNick)
            : base(doc)
        {
            this.To = roomAndNick;
            this.CreateChildElement<RoomX>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="roomAndNick"></param>
        /// <param name="password">Null for non-password rooms</param>
        /// TODO: getHistory?
        public RoomPresence(XmlDocument doc, JID roomAndNick, string password)
            : base(doc)
        {
            this.To = roomAndNick;
            this.CreateChildElement<RoomX>().Password = password;
        }

        /// <summary>
        /// The X tag denoting MUC-ness.  Use this to access passord and history
        /// after creation.
        /// </summary>
        public RoomX X
        {
            get { return GetChildElement<RoomX>(); }
            set { ReplaceChild<RoomX>(value); }
        }
    }


    /// <summary>
    /// X tag for presence when joining a room.
    /// </summary>
    [SVN(@"$Id$")]
    public class RoomX : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public RoomX(XmlDocument doc)
            : base("x", URI.MUC, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public RoomX(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Password to join room.  Null for no password.
        /// </summary>
        public string Password
        {
            get { return GetElem("password"); }
            set { SetElem("password", value); }
        }

        /// <summary>
        /// Add a history element, or return the existing one.
        /// </summary>
        /// <returns></returns>
        public History AddHistory()
        {
            return GetOrCreateElement<History>();
        }

        /// <summary>
        /// History options
        /// </summary>
        public History History
        {
            get { return GetChildElement<History>(); }
            set { this.ReplaceChild<History>(value); }
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
        /// Add a decline element, or return the existing one.
        /// </summary>
        /// <returns></returns>
        public Decline AddDecline()
        {
            return GetOrCreateElement<Decline>();
        }

        /// <summary>
        /// Invite was declined
        /// </summary>
        public Decline Decline
        {
            get { return GetChildElement<Decline>(); }
            set { ReplaceChild<Decline>(value); }
        }

        /// <summary>
        /// Add a destroy element, or return the existing one.
        /// </summary>
        /// <returns></returns>
        public Destroy AddDestroy()
        {
            return GetOrCreateElement<Destroy>();
        }

        /// <summary>
        /// Room was destroyed
        /// </summary>
        public Destroy Destroy
        {
            get { return GetChildElement<Destroy>();  }
            set { ReplaceChild<Destroy>(value); }
        }

        /// <summary>
        /// The list of invites
        /// </summary>
        public Invite[] GetInvites()
        {
            return GetElements<Invite>().ToArray();
        }

        /// <summary>
        /// Add new invite
        /// </summary>
        /// <param name="to">Who to send the invite to?</param>
        /// <param name="reason">Why?  Null if none.</param>
        /// <returns></returns>
        public Invite AddInvite(JID to, string reason)
        {
            Invite inv = CreateChildElement<Invite>();
            inv.To = to;
            if (reason != null)
                inv.Reason = reason;
            return inv;
        }

        /// <summary>
        /// Add a room item element, or return the existing one.
        /// </summary>
        /// <returns></returns>
        public RoomItem AddRoomItem()
        {
            return GetOrCreateElement<RoomItem>();
        }

        /// <summary>
        /// The associated item
        /// </summary>
        public RoomItem RoomItem
        {
            get { return GetChildElement<RoomItem>(); }
            set { ReplaceChild<RoomItem>(value); }
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
        /// Sorted list of statuses of the request.
        /// </summary>
        /// <exception cref="FormatException">Invalid code</exception>
        public RoomStatus[] Status
        {
            get
            {
                XmlNodeList nl = this.GetElementsByTagName("status");
                RoomStatus[] ret = new RoomStatus[nl.Count];
                int i = 0;
                foreach (XmlElement status in nl)
                {
                    try
                    {
                        ret[i] = (RoomStatus)int.Parse(status.GetAttribute("code"));
                    }
                    catch
                    {
                        ret[i] = RoomStatus.UNKNOWN;
                    }
                    i++;
                }
                Array.Sort(ret);
                return ret;
            }
            set
            {
                RemoveElems("status");
                foreach (RoomStatus i in value)
                {
                    XmlElement status = this.OwnerDocument.CreateElement("status");
                    status.SetAttribute("code", ((int)i).ToString());
                    this.AppendChild(status);
                }
            }
        }

        /// <summary>
        /// Did we receive a given status?
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool HasStatus(RoomStatus status)
        {
            string s = ((int)status).ToString();
            XmlNodeList nl = this.GetElementsByTagName("status");
            foreach (XmlElement stat in nl)
            {
                if (s == stat.GetAttribute("code"))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Invitee Declines Invitation
    /// </summary>
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
    public class RoomItem : AdminItem
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public RoomItem(XmlDocument doc)
            : base("item", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public RoomItem(string prefix, XmlQualifiedName qname, XmlDocument doc)
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
    [SVN(@"$Id$")]
    public class RoomActor : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public RoomActor(XmlDocument doc)
            : base("actor", URI.MUC_USER, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public RoomActor(string prefix, XmlQualifiedName qname, XmlDocument doc)
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
#endregion

#region admin
    /// <summary>
    /// An IQ with a AdminQuery inside.
    /// </summary>
    [SVN(@"$Id$")]
    public class RoomAdminIQ : jabber.protocol.client.TypedIQ<AdminQuery>
    {
        /// <summary>
        /// Create a admin IQ, with a single muc#admin query element.
        /// </summary>
        /// <param name="doc"></param>
        public RoomAdminIQ(XmlDocument doc)
            : base(doc)
        {
        }
    }

    /// <summary>
    /// Moderator use cases
    /// </summary>
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
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
        public RoomActor Actor
        {
            get { return GetOrCreateElement("actor", null, typeof(RoomActor)) as RoomActor; }
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
        public RoomAffiliation Affiliation
        {
            get { return (RoomAffiliation)GetEnumAttr("affiliation", typeof(RoomAffiliation)); }
            set { SetEnumAttr("affiliation", value); }
        }

        /// <summary>
        /// The role of the item
        /// </summary>
        public RoomRole Role
        {
            get { return (RoomRole)GetEnumAttr("role", typeof(RoomRole)); }
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
    [SVN(@"$Id$")]
    public class OwnerIQ : jabber.protocol.client.TypedIQ<OwnerQuery>
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="doc"></param>
        public OwnerIQ(XmlDocument doc)
            : base(doc)
        {
        }
    }

    /// <summary>
    /// The query element inside an owner IQ.
    /// </summary>
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
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
    [SVN(@"$Id$")]
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
