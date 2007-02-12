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

using System.Text;
using System.Xml;
using NUnit.Framework;

using bedrock;
using bedrock.util;
using jabber.protocol;

namespace test.jabber.protocol
{
    /// <summary>
    /// Summary description for ElementListTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class ElementListTest
    {
        private XmlDocument doc = new XmlDocument();

        private Element Parent()
        {
            Element parent = new Element("rent", "f", doc);
            Element child;
            for (int i=0; i<10; i++)
            {
                child = new Element("foo", "f", doc);
                child.InnerText = i.ToString();
                parent.AppendChild(child);
                child = new Element("bar", "f", doc);
                child.InnerText = i.ToString();
                parent.AppendChild(child);
            }
            return parent;
        }

        [Test] public void Test_Count()
        {
            Element parent = Parent();
            Assert.AreEqual(10,  parent.GetElementsByTagName("foo").Count);
            Assert.AreEqual(10,  parent.GetElementsByTagName("bar").Count);
            Assert.AreEqual(10,  parent.GetElementsByTagName("foo", "f").Count);
            Assert.AreEqual(10,  parent.GetElementsByTagName("bar", "f").Count);
            Assert.AreEqual(0,  parent.GetElementsByTagName("bar", "g").Count);
        }
        [Test] public void Test_Enum()
        {
            Element parent = Parent();
            int c = 0;
            foreach (XmlElement e in parent.GetElementsByTagName("foo"))
            {
                Assert.AreEqual(c.ToString(), e.InnerText);
                c++;
            }
            Assert.AreEqual(10, c);
        }

        [Test] public void Test_Grandkids()
        {
            Element parent = Parent();
            Element g = new Element("foo", "f", doc);
            g.InnerText = "one";
            parent.FirstChild.AppendChild(g);
            foreach (XmlElement e in parent.GetElementsByTagName("foo"))
            {
                Assert.IsTrue(e.InnerText != "one");
            }
        }
    }
}
