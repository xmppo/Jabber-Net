/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
        private void SetUp()
        {
            Element.ResetID();
        }
        public void Test_Create()
        {
            Roster r = new Roster(doc);
            Assertion.AssertEquals("<query xmlns=\"jabber:iq:roster\" />", r.ToString());
        }
    
        public void Test_Item()
        {
            RosterIQ riq = new RosterIQ(doc);
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            i.JID = new JID("hildjj@jabber.com");
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\" /></query></iq>",
                riq.ToString());
        }
        public void Test_GetItems()
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
            Assertion.AssertEquals(items.Length, 2);
            Assertion.AssertEquals(items[0].JID, "hildjj@jabber.com");
            Assertion.AssertEquals(items[1].JID, "hildjj@jabber.org");
        }
        public void Test_Groups()
        {
            RosterIQ riq = new RosterIQ(doc);
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            i.JID = new JID("hildjj@jabber.com");
            Group g = i.AddGroup("foo");
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\"><group>foo</group></item></query></iq>",
                riq.ToString());
            g = i.AddGroup("foo");
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\"><group>foo</group></item></query></iq>",
                riq.ToString());
            g = i.AddGroup("bar");
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:roster\">" +
                "<item jid=\"hildjj@jabber.com\"><group>foo</group><group>bar</group></item></query></iq>",
                riq.ToString());
            Assertion.AssertEquals(2, i.GetGroups().Length);
            Assertion.AssertEquals("foo", i.GetGroup("foo").GroupName);
            Assertion.AssertEquals("bar", i.GetGroup("bar").GroupName);
            i.RemoveGroup("foo");
            Assertion.AssertEquals(1, i.GetGroups().Length);
            Assertion.AssertEquals(null, i.GetGroup("foo"));
        }
        public void Test_Ask()
        {
            RosterIQ riq = new RosterIQ(doc);
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            Assertion.AssertEquals("", i.GetAttribute("ask"));
            Assertion.AssertEquals(Ask.NONE, i.Ask);
            i.Ask = Ask.subscribe;
            Assertion.AssertEquals("subscribe", i.GetAttribute("ask"));
            Assertion.AssertEquals(Ask.subscribe, i.Ask);
            i.Ask = Ask.NONE;
            Assertion.AssertEquals("", i.GetAttribute("ask"));
            Assertion.AssertEquals(Ask.NONE, i.Ask);
        }
    }
}