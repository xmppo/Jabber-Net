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

        public void Test_Count()
        {
            Element parent = Parent();
            Assertion.AssertEquals(10,  parent.GetElementsByTagName("foo").Count);
            Assertion.AssertEquals(10,  parent.GetElementsByTagName("bar").Count);
            Assertion.AssertEquals(10,  parent.GetElementsByTagName("foo", "f").Count);
            Assertion.AssertEquals(10,  parent.GetElementsByTagName("bar", "f").Count);
            Assertion.AssertEquals(0,  parent.GetElementsByTagName("bar", "g").Count);
        }
        public void Test_Enum()
        {
            Element parent = Parent();
            int c = 0;
            foreach (XmlElement e in parent.GetElementsByTagName("foo"))
            {
                Assertion.AssertEquals(c.ToString(), e.InnerText);
                c++;
            }
            Assertion.AssertEquals(10, c);
        }

        public void Test_Grandkids()
        {
            Element parent = Parent();
            Element g = new Element("foo", "f", doc);
            g.InnerText = "one";
            parent.FirstChild.AppendChild(g);
            foreach (XmlElement e in parent.GetElementsByTagName("foo"))
            {
                Assertion.Assert(e.InnerText != "one");
            }           
        }
    }
}
