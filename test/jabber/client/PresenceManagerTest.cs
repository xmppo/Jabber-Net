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
using jabber.client;
using jabber.protocol.client;

namespace test.jabber.client1 // TODO: Client1 due to a bug in NUnit.
{
    /// <summary>
    /// Summary description for PPDP.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class PresenceManagerTest
    {
        XmlDocument doc = new XmlDocument();

        [Test] public void Test_Create()
        {
            PresenceManager pp = new PresenceManager();
            Assert.AreEqual("jabber.client.PresenceManager", pp.GetType().FullName);
        }
        public void TestAdd()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("foo", "bar", "baz");
            pres.From = f;
            pp.AddPresence(pres);
            Assert.AreEqual("foo@bar/baz", pp[f].From.ToString());
            f.Resource = null;
            Assert.AreEqual("foo@bar/baz", pp[f].From.ToString());

            pres = new Presence(doc);
            pres.Status = "wandering";
            pres.From = new JID("foo", "bar", "baz");
            pp.AddPresence(pres);
            Assert.AreEqual("wandering", pp[f].Status);
        }
        public void TestRetrieve()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("foo", "bar", "baz");
            pres.From = f;
            pres.Priority = "0";
            pp.AddPresence(pres);
            Assert.AreEqual("foo@bar/baz", pp[f.Bare].From.ToString());

            pres = new Presence(doc);
            f = new JID("foo", "bar", "bay");
            pres.From = f;
            pres.Priority = "1";
            pp.AddPresence(pres);
            Assert.AreEqual("foo@bar/bay", pp[f.Bare].From.ToString());

            pres = new Presence(doc);
            pres.From = f;
            pres.Type = PresenceType.unavailable;
            pp.AddPresence(pres);
            Assert.AreEqual("foo@bar/baz", pp[f.Bare].From.ToString());
        }
        public void TestUserHost()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("foo", "bar", null);
            pres.From = f;
            pp.AddPresence(pres);
            Assert.AreEqual("foo@bar", pp[f.Bare].From.ToString());
        }
        public void TestHost()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("bar");
            pres.From = f;
            pp.AddPresence(pres);
            Assert.AreEqual("bar", pp[f.Bare].From.ToString());
        }
        public void TestHostResource()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID(null, "bar", "baz");
            pres.From = f;
            pp.AddPresence(pres);
            Assert.AreEqual("bar/baz", pp[f.Bare].From.ToString());
        }
        public void TestRemove()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("foo", "bar", "baz");
            pres.From = f;
            pres.Status = "Working";
            pp.AddPresence(pres);
            Assert.AreEqual("foo@bar/baz", pp[f].From.ToString());
            f.Resource = null;
            Assert.AreEqual("foo@bar/baz", pp[f].From.ToString());

            pres = new Presence(doc);
            pres.Status = "wandering";
            pres.From = new JID("foo", "bar", "boo");
            pp.AddPresence(pres);
            Assert.AreEqual("Working", pp[f].Status);
            pres.Priority = "2";
            pp.AddPresence(pres);
            Assert.AreEqual("wandering", pp[f].Status);
            pres.Type = PresenceType.unavailable;
            pp.AddPresence(pres);
            Assert.AreEqual("Working", pp[f].Status);
        }
        public void TestNumeric()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("support", "conference.192.168.32.109", "bob");
            pres.From = f;
            pres.Status = "Working";
            pp.AddPresence(pres);
            Assert.AreEqual("support@conference.192.168.32.109/bob", pp[f].From.ToString());
            f.Resource = null;
            Assert.AreEqual("support@conference.192.168.32.109/bob", pp[f].From.ToString());
        }
    }
}
