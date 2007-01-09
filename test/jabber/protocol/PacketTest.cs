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
using jabber.protocol;

namespace test.jabber.protocol
{
    /// <summary>
    /// Summary description for PacketTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class PacketTest
    {
        XmlDocument doc = new XmlDocument();

        [Test] public void Test_Create()
        {
            Packet p = new Packet("foo", doc);
            Assert.AreEqual("<foo />", p.ToString());
            p.To = "one";
            Assert.AreEqual("<foo to=\"one\" />", p.ToString());
            p.From = "two";
            Assert.AreEqual("<foo to=\"one\" from=\"two\" />", p.ToString());
            p.Swap();
            Assert.AreEqual("<foo to=\"two\" from=\"one\" />", p.ToString());
        }

        [Test] public void Test_JabberDate()
        {
            string sdt = "20020504T20:39:42";
            DateTime dt = Element.JabberDate(sdt);
            Assert.AreEqual(2002, dt.Year);
            Assert.AreEqual(5, dt.Month);
            Assert.AreEqual(4, dt.Day);
            Assert.AreEqual(20, dt.Hour);
            Assert.AreEqual(39, dt.Minute);
            Assert.AreEqual(42, dt.Second);
            Assert.AreEqual(sdt, Element.JabberDate(dt));
        }
        [Test] public void Test_DateTimeProfile()
        {
            string sdt = "2002-05-04T20:39:42.050Z";
            DateTime dt = Element.DateTimeProfile(sdt);
            Assert.AreEqual(2002, dt.Year);
            Assert.AreEqual(5, dt.Month);
            Assert.AreEqual(4, dt.Day);
            Assert.AreEqual(20, dt.Hour);
            Assert.AreEqual(39, dt.Minute);
            Assert.AreEqual(42, dt.Second);
            Assert.AreEqual(sdt, Element.DateTimeProfile(dt));
        }
    }
}
