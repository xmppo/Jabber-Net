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
using JabberNet.jabber.protocol.iq;
using NUnit.Framework;

namespace JabberNet.Test.jabber.protocol.iq
{
    [TestFixture]
    public class TimeTest
    {
        [Test]
        public void UTC()
        {
            XmlDocument doc = new XmlDocument();
            TimeIQ iq = new TimeIQ(doc);
            Time t = iq.Instruction;
            t.AddChild(doc.CreateElement("utc", t.NamespaceURI));
            Assert.AreEqual(DateTime.MinValue, t.UTC);
            DateTime start = DateTime.UtcNow;
            t.SetCurrentTime();

            // SetCurrentTime only stores seconds portion, whereas UtcNow has all
            // kinds of precision.  Are we within a second of being correct?
            TimeSpan ts = t.UTC - start;
            Assert.IsTrue(Math.Abs(ts.TotalSeconds) < 1.0);
        }
    }
}
