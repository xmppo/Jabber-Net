/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
        
        public void Test_Create()
        {
            Packet p = new Packet("foo", doc);
            Assertion.AssertEquals("<foo />", p.ToString());
            p.To = "one";
            Assertion.AssertEquals("<foo to=\"one\" />", p.ToString());
            p.From = "two";
            Assertion.AssertEquals("<foo to=\"one\" from=\"two\" />", p.ToString());
            p.Swap();
            Assertion.AssertEquals("<foo to=\"two\" from=\"one\" />", p.ToString());
        }

        public void Test_JabberDate()
        {
            string sdt = "20020504T20:39:42";
            DateTime dt = Element.JabberDate(sdt);
            Assertion.AssertEquals(2002, dt.Year);
            Assertion.AssertEquals(5, dt.Month);
            Assertion.AssertEquals(4, dt.Day);
            Assertion.AssertEquals(20, dt.Hour);
            Assertion.AssertEquals(39, dt.Minute);
            Assertion.AssertEquals(42, dt.Second);
            Assertion.AssertEquals(sdt, Element.JabberDate(dt));
        }
    }
}