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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
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
using jabber.client;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace test.jabber.client1 // TODO: Client1 due to a bug in NUnit.  
{
    /// <summary>
    /// Summary description for PPDP.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class RosterManagerTest
    {
        XmlDocument doc = new XmlDocument();

        public void Test_Create()
        {
            RosterManager rm = new RosterManager();
            Assertion.AssertEquals("jabber.client.RosterManager", rm.GetType().FullName);
        }
        public void TestAdd()
        {
            RosterManager rm = new RosterManager();

            RosterIQ riq = new RosterIQ(doc);
            riq.Type = IQType.set;
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            i.JID = new JID("foo", "bar", null);
            i.Nickname = "FOO";
            i.Subscription = Subscription.both;

            rm.AddRoster(riq);
            Assertion.AssertEquals(Subscription.both, rm["foo@bar"].Subscription);
            Assertion.AssertEquals("FOO", rm["foo@bar"].Nickname);

            riq = new RosterIQ(doc);
            riq.Type = IQType.set;
            r = (Roster) riq.Query;
            i = r.AddItem();
            i.JID = new JID("foo", "bar", null);
            i.Nickname = "BAR";
            i.Subscription = Subscription.to;
            rm.AddRoster(riq);
            Assertion.AssertEquals(Subscription.to, rm["foo@bar"].Subscription);
            Assertion.AssertEquals("BAR", rm["foo@bar"].Nickname);
        }
        public void TestNumeric()
        {
            RosterManager rm = new RosterManager();

            RosterIQ riq = new RosterIQ(doc);
            riq.Type = IQType.set;
            Roster r = (Roster) riq.Query;
            Item i = r.AddItem();
            i.JID = new JID("support", "conference.192.168.32.109", null);
            i.Nickname = "FOO";
            i.Subscription = Subscription.both;

            rm.AddRoster(riq);
            Assertion.AssertEquals(Subscription.both, rm["support@conference.192.168.32.109"].Subscription);
            Assertion.AssertEquals("FOO", rm["support@conference.192.168.32.109"].Nickname);
        }
    }
}