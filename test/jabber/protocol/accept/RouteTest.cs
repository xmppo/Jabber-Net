/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
using jabber.protocol.accept;

namespace test.jabber.protocol.accept
{
    /// <summary>
    /// Summary description for IQTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class RouteTest
    {
        XmlDocument doc = new XmlDocument();
        [Test] public void Test_Create()
        {
            Route r = new Route(doc);
            r.Contents = doc.CreateElement("foo");
            Assert.AreEqual("<route><foo /></route>", r.OuterXml);
            XmlElement foo = r.Contents;
            Assert.AreEqual("<foo />", foo.OuterXml);
        }
    }
}