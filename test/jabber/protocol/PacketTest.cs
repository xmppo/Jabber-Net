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
    public class PacketTest : TestCase
    {
        public PacketTest(string name) : base(name) {}
        public static ITest Suite
        {
            get { return new TestSuite(typeof(PacketTest)); }
        }
        XmlDocument doc = new XmlDocument();
        
        public void Test_Create()
        {
            Packet p = new Packet("foo", doc);
            AssertEquals("<foo />", p.ToString());
            p.To = "one";
            AssertEquals("<foo to=\"one\" />", p.ToString());
            p.From = "two";
            AssertEquals("<foo to=\"one\" from=\"two\" />", p.ToString());
            p.Swap();
            AssertEquals("<foo to=\"two\" from=\"one\" />", p.ToString());
        }

        public void Test_JabberDate()
        {
            string sdt = "20020504T20:39:42";
            DateTime dt = Element.JabberDate(sdt);
            AssertEquals(2002, dt.Year);
            AssertEquals(5, dt.Month);
            AssertEquals(4, dt.Day);
            AssertEquals(20, dt.Hour);
            AssertEquals(39, dt.Minute);
            AssertEquals(42, dt.Second);
            AssertEquals(sdt, Element.JabberDate(dt));
        }
    }
}
