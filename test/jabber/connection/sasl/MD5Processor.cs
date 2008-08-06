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
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using bedrock.util;

using jabber.connection;
using jabber.connection.sasl;
using jabber.protocol.stream;

using NUnit.Framework;
using System.Xml;

namespace test.jabber.connection.sasl
{
    [TestFixture]
    [SVN(@"$Id$")]
    public class MD5ProcessorTest
    {
        [Test]
        public void TestChallenge()
        {

            XmlDocument doc = new XmlDocument();
            Challenge c = new Challenge(doc);
            c.InnerText = "cmVhbG09IndlYjIwMDMiLCBub25jZT0iWWE0anVNYzU0SG9UWDBPa1VPRDFvQT09IiwgcW9wPSJhdXRoLCBhdXRoLWludCIsIGNoYXJzZXQ9dXRmLTgsIGFsZ29yaXRobT1tZDUtc2Vzcw==";

            MD5Processor m = new MD5Processor();
            m["username"] = "test";
            m["password"] = "test";
            Step s = m.step(c, doc);
            Assert.IsNotNull(s);
            Assert.AreEqual("Ya4juMc54HoTX0OkUOD1oA==", m["nonce"]);
            Assert.AreEqual("auth, auth-int", m["qop"]);
        }
    }
}
