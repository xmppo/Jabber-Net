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

using System.Xml;
using bedrock.util;
using jabber.protocol.accept;
using NUnit.Framework;

namespace JabberNet.Test.jabber.protocol.accept
{
    /// <summary>
    /// Summary description for IQTest.
    /// </summary>
    [SVN(@"$Id$")]
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
