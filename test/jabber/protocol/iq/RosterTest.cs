/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
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
    }
}
