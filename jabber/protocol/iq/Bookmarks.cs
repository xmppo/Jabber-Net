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
using System.Diagnostics;
using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
    /// <summary>
    /// An IQ in jabber:iq:private, with a bookmarks body.
    /// </summary>
    [SVN(@"$Id$")]
    public class BookmarksIQ : PrivateIQ
	{
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public BookmarksIQ(XmlDocument doc) : base(doc)
        {
            this.Instruction.AddChild(new Bookmarks(doc));
        }

        /// <summary>
        /// Get the bookmarks element.
        /// </summary>
        public Bookmarks Bookmarks
        {
            get { return this.Instruction.GetChildElement<Bookmarks>(); }
        }
	}

    /// <summary>
    /// The bookmarks to be stored.
    /// </summary>
    [SVN(@"$Id$")]
    public class Bookmarks : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public Bookmarks(XmlDocument doc) :
            base("storage", URI.BOOKMARKS, doc)
        {
        }

        /// <summary>
        /// Create for inbound.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Bookmarks(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Get all of the URLs contained in the bookmark list.
        /// </summary>
        /// <returns></returns>
        public BookmarkURL[] GetUrls()
        {
            return this.GetElements<BookmarkURL>().ToArray();
        }

        /// <summary>
        /// Add a URL bookmark
        /// </summary>
        /// <param name="URL">The URL to add</param>
        /// <param name="name">Descriptive text</param>
        /// <returns></returns>
        public BookmarkURL AddURL(string URL, string name)
        {
            BookmarkURL u = new BookmarkURL(this.OwnerDocument);
            u.URL = URL;
            u.URLName = name;
            this.AddChild(u);
            return u;
        }

        /// <summary>
        /// Get all of the conferences contained in the bookmark list.
        /// </summary>
        /// <returns></returns>
        public BookmarkConference[] GetConferences()
        {
            return this.GetElements<BookmarkConference>().ToArray();
        }

        /// <summary>
        /// Add a conference room to the bookmark list
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="name"></param>
        /// <param name="autoJoin"></param>
        /// <param name="nick"></param>
        /// <returns></returns>
        public BookmarkConference AddConference(JID jid, string name, bool autoJoin, string nick)
        {
            BookmarkConference c = new BookmarkConference(this.OwnerDocument);
            c.JID = jid;
            c.ConferenceName = name;
            c.AutoJoin = autoJoin;
            if (nick != null)
                c.Nick = nick;
            this.AddChild(c);
            return c;
        }

        /// <summary>
        /// Get all of the notes contained in the bookmark list.
        /// </summary>
        /// <returns></returns>
        public BookmarkNote[] GetNotes()
        {
            return this.GetElements<BookmarkNote>().ToArray();
        }

        /// <summary>
        /// Add a note to the bookmark list.
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public BookmarkNote AddNote(JID jid, string text)
        {
            BookmarkNote n = new BookmarkNote(this.OwnerDocument);
            n.JID = jid;
            n.Text = text;
            this.AddChild(n);
            return n;
        }
    }

    /// <summary>
    /// A URL stored in bookmarks.
    /// </summary>
    [SVN(@"$Id$")]
    public class BookmarkURL : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public BookmarkURL(XmlDocument doc) :
            base("url", URI.BOOKMARKS, doc)
        {
        }

        /// <summary>
        /// Create for inbound.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public BookmarkURL(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The HTTP or HTTPS URL of the web page, according to spec.
        /// In practice, any URI.
        /// </summary>
        public string URL
        {
            get { return GetAttr("url"); }
            set { SetAttr("url", value); }
        }

        /// <summary>
        /// A friendly name for the bookmark.
        /// </summary>
        public string URLName
        {
            get { return GetAttr("name"); }
            set { SetAttr("name", value); }
        }
    }

    /// <summary>
    /// A conference room name stored in bookmarks
    /// </summary>
    [SVN(@"$Id$")]
    public class BookmarkConference : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public BookmarkConference(XmlDocument doc) :
            base("conference", URI.BOOKMARKS, doc)
        {
        }

        /// <summary>
        /// Create for inbound.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public BookmarkConference(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Should the room be joined on startup?
        /// </summary>
        public bool AutoJoin
        {
            get 
            {
                string aj = GetAttr("autojoin");
                return (aj == "true") || (aj == "1");
            }
            set 
            {
                if (value)
                    SetAttr("autojoin", "true");
                else
                    RemoveAttribute("autojoin");
            }
        }

        /// <summary>
        /// The room@service JID of the room.
        /// </summary>
        public JID JID
        {
            get { return (JID)GetAttr("jid"); }
            set { SetAttr("jid", value); }
        }

        /// <summary>
        /// A friendly name for the bookmark.
        /// </summary>
        public string ConferenceName
        {
            get { return GetAttr("name"); }
            set { SetAttr("name", value); }
        }

        /// <summary>
        /// The user's preferred roomnick for the chatroom.
        /// </summary>
        public string Nick
        {
            get { return GetElem("nick");  }
            set { SetElem("nick", value);  }
        }

        /// <summary>
        /// Plain-text string for the password needed to enter a password-protected room. 
        /// For security reasons, use of this element is NOT RECOMMENDED.
        /// 
        /// TODO: should this be marked Obsolete?
        /// </summary>
        public string Password
        {
            get { return GetElem("password"); }
            set { SetElem("password", value); }
        }
    }

    /// <summary>
    /// A note stored in bookmarks.  Un-specified, but hinted at in version 1.1 of XEP-48.
    /// </summary>
    [SVN(@"$Id$")]
    public class BookmarkNote : Element
    {
        /// <summary>
        /// Create for outbound.
        /// </summary>
        /// <param name="doc"></param>
        public BookmarkNote(XmlDocument doc) :
            base("note", URI.BOOKMARKS, doc)
        {
            Modified = Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Create for inbound.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public BookmarkNote(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The JID to which the note pertains.
        /// </summary>
        public JID JID
        {
            get { return (JID)GetAttr("jid"); }
            set { SetAttr("jid", value); }
        }

        /// <summary>
        /// The UTC date/time the note was created.
        /// </summary>
        public DateTime Created
        {
            get { return GetDateTimeAttr("cdate"); }
            set { SetDateTimeAttr("cdate", value); }
        }

        /// <summary>
        /// The UTC date/time the note last modified.
        /// </summary>
        public DateTime Modified
        {
            get { return GetDateTimeAttr("mdate"); }
            set { SetDateTimeAttr("mdate", value); }
        }

        /// <summary>
        /// The text of the note.
        /// </summary>
        public string Text
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }
}
