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
using jabber.protocol;

namespace test.jabber.protocol
{
    /// <summary>
    /// Test the EnumParser
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class EnumParserTest
    {
        enum foo
        {
            NONE = -1,
            bar,
            baz
        }

        enum bar
        {
            NONE = -1,
            [XML("moo")]
            bloo,
            [XML("")]
            goo
        }

        [Dash]
        enum doo
        {
            moo_vie,
        }

        [Test]
        public void ParsePlain()
        {
            foo f = EnumParser.Parse<foo>("bar");
            Assert.AreEqual(foo.bar, f);
            f = EnumParser.Parse<foo>("blah");
            Assert.AreEqual(foo.NONE, f);
        }

        [Test]
        public void ParseAttr()
        {
            bar b = EnumParser.Parse<bar>("moo");
            Assert.AreEqual(bar.bloo, b);
            b = EnumParser.Parse<bar>("bloo");
            Assert.AreEqual(bar.bloo, b);
        }

        [Test]
        public void Strings()
        {
            string s = EnumParser.ToString(foo.bar);
            Assert.AreEqual("bar", s);

            s = EnumParser.ToString(bar.bloo);
            Assert.AreEqual("moo", s);
        }

        [Test]
        public void XML()
        {
            XmlDocument doc = new XmlDocument();
            Element e = new Element("test", doc);
            e.SetAttribute("bloo", EnumParser.ToString(bar.NONE));
            Assert.AreEqual("", e.GetAttribute("bloo"));
            e.SetAttribute("bloo", EnumParser.ToString(bar.bloo));
            Assert.AreEqual("moo", e.GetAttribute("bloo"));
            e.SetAttribute("bloo", EnumParser.ToString(bar.NONE));
            Assert.AreEqual("", e.GetAttribute("bloo"));
        }

        [Test]
        public void Dashes()
        {
            doo d = doo.moo_vie;
            string s = EnumParser.ToString(d);
            Assert.AreEqual("moo-vie", s);
            d = EnumParser.Parse<doo>(s);
            Assert.AreEqual(doo.moo_vie, d);
        }

        [Test]
        public void Null()
        {
            string s = EnumParser.ToString(foo.NONE);
            Assert.IsNull(s);
            foo f = EnumParser.Parse<foo>("");
            Assert.AreEqual(foo.NONE, f);
            bar b = EnumParser.Parse<bar>("");
            Assert.AreEqual(bar.goo, b);
        }
    }
}
