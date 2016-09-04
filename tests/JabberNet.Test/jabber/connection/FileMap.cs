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
 * See licenses/Jabber-Net_LGPLv3.txt for details.
 * --------------------------------------------------------------------------*/

using System;
using System.Xml;
using JabberNet.jabber.connection;
using JabberNet.jabber.protocol;
using JabberNet.jabber.protocol.iq;
using NUnit.Framework;

namespace JabberNet.Test.jabber.connection
{
    [TestFixture]
    public class FileMapTest
    {
        XmlDocument doc = new XmlDocument();

        DiscoInfo Element
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                global::JabberNet.jabber.protocol.iq.DiscoInfo di = new global::JabberNet.jabber.protocol.iq.DiscoInfo(doc);
                di.AddFeature(global::JabberNet.jabber.protocol.URI.DISCO_INFO);
                di.AddFeature(global::JabberNet.jabber.protocol.URI.DISCO_ITEMS);
                return di;
            }
        }

        [Test]
        public void TestNull()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var fm = new FileMap<Element>("test.xml", null);
                Assert.IsNotNull(fm);
                var fm2 = new FileMap<DiscoInfo>("test.xml", null);
            });
        }

        [Test]
        public void TestCreate()
        {
            ElementFactory ef = new ElementFactory();
            ef.AddType(new global::JabberNet.jabber.protocol.iq.Factory());

            string g = new Guid().ToString();
            FileMap<DiscoInfo> fm = new FileMap<DiscoInfo>("test.xml", ef);
            fm.Clear();
            Assert.AreEqual(0, fm.Count);

            fm[g] = Element;
            Assert.IsTrue(fm.Contains(g));
            Assert.IsFalse(fm.Contains("foo"));
            Assert.IsInstanceOf<DiscoInfo>(fm[g]);
            Assert.AreEqual(1, fm.Count);

            // re-read, to reparse
            fm = new FileMap<DiscoInfo>("test.xml", ef);
            Assert.IsTrue(fm.Contains(g));
            Assert.IsInstanceOf<DiscoInfo>(fm[g]);

            fm[g] = null;
            Assert.AreEqual(1, fm.Count);

            fm.Remove(g);
            Assert.AreEqual(0, fm.Count);
        }
    }
}
