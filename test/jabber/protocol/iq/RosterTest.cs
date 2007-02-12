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
using NUnit.Framework;

using bedrock.util;
using jabber;
using jabber.protocol;
using jabber.protocol.iq;

namespace test.jabber.protocol.iq
{
    /// <summary>
    /// Summary description for RosterTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class RosterTest
    {
        XmlDocument doc = new XmlDocument();
        [SetUp]
        public void SetUp()
        {
            Element.ResetID();
        }
        [Test] public void Test_Create()
        {
            Roster r = new Roster(doc);
            Assert.AreEqual("<query xmlns=\"jabber:iq:roster\" />", r.ToString());
        }

        [Test] public void Test_Item()
        {
            RosterIQ riq = new RosterIQ(doc);
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            i.JID = new JID("hildjj@jabber.com");
            Assert.AreEqual("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\" /></query></iq>",
                riq.ToString());
        }
        [Test] public void Test_GetItems()
        {
            RosterIQ riq = new RosterIQ(doc);
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            i.JID = new JID("hildjj@jabber.com");
            i = r.AddItem();
            i.Subscription = Subscription.from;
            i.JID = new JID("hildjj@jabber.org");
            i.Subscription = Subscription.both;
            Item[] items = r.GetItems();
            Assert.AreEqual(items.Length, 2);
            Assert.AreEqual(items[0].JID, "hildjj@jabber.com");
            Assert.AreEqual(items[1].JID, "hildjj@jabber.org");
        }
        [Test] public void Test_Groups()
        {
            RosterIQ riq = new RosterIQ(doc);
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            i.JID = new JID("hildjj@jabber.com");
            Group g = i.AddGroup("foo");
            Assert.AreEqual("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\"><group>foo</group></item></query></iq>",
                riq.ToString());
            g = i.AddGroup("foo");
            Assert.AreEqual("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\"><group>foo</group></item></query></iq>",
                riq.ToString());
            g = i.AddGroup("bar");
            Assert.AreEqual("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\"><group>foo</group><group>bar</group></item></query></iq>",
                riq.ToString());
            Assert.AreEqual(2, i.GetGroups().Length);
            Assert.AreEqual("foo", i.GetGroup("foo").GroupName);
            Assert.AreEqual("bar", i.GetGroup("bar").GroupName);
            i.RemoveGroup("foo");
            Assert.AreEqual(1, i.GetGroups().Length);
            Assert.AreEqual(null, i.GetGroup("foo"));
        }
        [Test] public void Test_Ask()
        {
            RosterIQ riq = new RosterIQ(doc);
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            Assert.AreEqual("", i.GetAttribute("ask"));
            Assert.AreEqual(Ask.NONE, i.Ask);
            i.Ask = Ask.subscribe;
            Assert.AreEqual("subscribe", i.GetAttribute("ask"));
            Assert.AreEqual(Ask.subscribe, i.Ask);
            i.Ask = Ask.NONE;
            Assert.AreEqual("", i.GetAttribute("ask"));
            Assert.AreEqual(Ask.NONE, i.Ask);
        }
    }
}
