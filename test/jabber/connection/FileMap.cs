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

using NUnit.Framework;
using bedrock.io;
using bedrock.util;
using System.Xml;

using jabber.connection;
using jabber.protocol;
using jabber.protocol.iq;

namespace test.jabber.connection
{
    [SVN(@"$Id$")]
    [TestFixture]
    public class FileMapTest
    {
        XmlDocument doc = new XmlDocument();

        DiscoInfo Element
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                global::jabber.protocol.iq.DiscoInfo di = new global::jabber.protocol.iq.DiscoInfo(doc);
                di.AddFeature(global::jabber.protocol.URI.DISCO_INFO);
                di.AddFeature(global::jabber.protocol.URI.DISCO_ITEMS);
                return di;
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNull()
        {
            FileMap<Element> fm = new FileMap<Element>("test.xml", null);
            Assert.IsNotNull(fm);
            FileMap<DiscoInfo> fm2 = new FileMap<DiscoInfo>("test.xml", null);
        }

        [Test]
        public void TestCreate()
        {
            ElementFactory ef = new ElementFactory();
            ef.AddType(new global::jabber.protocol.iq.Factory());

            string g = new Guid().ToString();
            FileMap<DiscoInfo> fm = new FileMap<DiscoInfo>("test.xml", ef);
            fm.Clear();
            Assert.AreEqual(0, fm.Count);

            fm[g] = Element;
            Assert.IsTrue(fm.Contains(g));
            Assert.IsFalse(fm.Contains("foo"));
            Assert.IsInstanceOfType(typeof(DiscoInfo), fm[g]);
            Assert.AreEqual(1, fm.Count);

            // re-read, to reparse
            fm = new FileMap<DiscoInfo>("test.xml", ef);
            Assert.IsTrue(fm.Contains(g));
            Assert.IsInstanceOfType(typeof(DiscoInfo), fm[g]);

            fm[g] = null;
            Assert.AreEqual(1, fm.Count);

            fm.Remove(g);
            Assert.AreEqual(0, fm.Count);
        }
    }
}
