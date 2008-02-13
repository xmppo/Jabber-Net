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
    [SVN(@"$Id$")]
    [TestFixture]
    public class PresenceManagerTest
    {
        XmlDocument doc = new XmlDocument();

        JID bare = "foo@bar";
        JID baz  = "foo@bar/baz";
        JID boo  = "foo@bar/boo";


        [Test]
        public void Test_Create()
        {
            PresenceManager pp = new PresenceManager();
            Assert.AreEqual("jabber.client.PresenceManager", pp.GetType().FullName);
        }
        [Test]
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
        [Test]
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
        [Test]
        public void TestUserHost()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("foo", "bar", null);
            pres.From = f;
            pp.AddPresence(pres);
            Assert.AreEqual("foo@bar", pp[f.Bare].From.ToString());
        }
        [Test]
        public void TestHost()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID("bar");
            pres.From = f;
            pp.AddPresence(pres);
            Assert.AreEqual("bar", pp[f.Bare].From.ToString());
        }
        [Test]
        public void TestHostResource()
        {
            PresenceManager pp = new PresenceManager();
            Presence pres = new Presence(doc);
            JID f = new JID(null, "bar", "baz");
            pres.From = f;
            pp.AddPresence(pres);
            Assert.AreEqual("bar/baz", pp[f.Bare].From.ToString());
        }
        [Test]
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
        [Test]
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

        [Test]
        public void TestGetAll()
        {
            PresenceManager pp = new PresenceManager();

            Presence pres = new Presence(doc);
            pres.From = baz;
            pp.AddPresence(pres);

            pres = new Presence(doc);
            pres.From = boo;
            pp.AddPresence(pres);

            Presence[] pa = pp.GetAll(bare);
            Assert.AreEqual(2, pa.Length);
            Assert.AreEqual(pa[0].GetType(), typeof(Presence));
        }

        [Test]
        public void TestNewPrimaryAlgorithm()
        {
            PresenceManager pp = new PresenceManager();

            Presence pres = new Presence(doc);
            pres.From = baz;
            pres.IntPriority = 1;
            pp.AddPresence(pres);
            Assert.AreEqual(1, pp[bare].IntPriority);
            Assert.AreEqual(baz, pp[bare].From);

            pres = new Presence(doc);
            pres.From = boo;
            pres.IntPriority = 2;
            pp.AddPresence(pres);
            // duh.
            Assert.AreEqual(2, pp[bare].IntPriority);
            Assert.AreEqual(boo, pp[bare].From);

            pres = new Presence(doc);
            pres.From = boo;
            pres.IntPriority = 0;
            pp.AddPresence(pres);
            Assert.AreEqual(1, pp[bare].IntPriority);
            Assert.AreEqual(baz, pp[bare].From); // ooo

            pres = new Presence(doc);
            pres.From = boo;
            pres.Type = PresenceType.unavailable;
            pp.AddPresence(pres);
            Assert.AreEqual(1, pp[bare].IntPriority);
            Assert.AreEqual(baz, pp[bare].From);

            pres = new Presence(doc);
            pres.From = baz;
            pres.IntPriority = -1;
            pp.AddPresence(pres);
            Assert.AreEqual(null, pp[bare]);

            pres = new Presence(doc);
            pres.From = baz;
            pres.Type = PresenceType.unavailable;
            pp.AddPresence(pres);
            Assert.AreEqual(0, pp.GetAll(bare).Length);
        }

        [Test]
        public void TestComparisons()
        {
            PresenceManager pp = new PresenceManager();

            Presence pres = new Presence(doc);
            pres.From = baz;
            pres.IntPriority = -1;
            pp.AddPresence(pres);
            Assert.AreEqual(null, pp[bare]);

            pres = new Presence(doc);
            pres.From = boo;
            pres.IntPriority = 0;
            pres.Show = "away";
            pp.AddPresence(pres);
            Assert.AreEqual(boo, pp[bare].From);

            pres = new Presence(doc);
            pres.From = baz;
            pres.IntPriority = 0;
            pres.Show = "xa";
            pp.AddPresence(pres);
            Assert.AreEqual(boo, pp[bare].From);

            pres = new Presence(doc);
            pres.From = boo;
            pres.IntPriority = 1;
            pp.AddPresence(pres);
            Assert.AreEqual(boo, pp[bare].From);
        }
    }
}
