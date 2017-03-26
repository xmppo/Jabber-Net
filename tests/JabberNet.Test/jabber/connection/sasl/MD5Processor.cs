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
using System.Text;
using System.Xml;
using JabberNet.jabber.connection.sasl;
using JabberNet.jabber.protocol.stream;
using NUnit.Framework;

namespace JabberNet.Test.jabber.connection.sasl
{
    [TestFixture]
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

        [Test]
        public void QopAuthShouldBeAssumedIfNoQopIsPresented()
        {
            var doc = new XmlDocument();
            var challenge = "realm=\"cthulhu-3\",nonce=\"IhS5TwCvOOaJE7hdwQQYjHCU5n5tLONF4/0Jc43r\",charset=utf-8,algorithm=md5-sess";
            var c = new Challenge(doc)
            {
                InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes(challenge))
            };

            var m = new MD5Processor
            {
                ["username"] = "user2",
                ["password"] = "123"
            };
            var s = m.step(c, doc);

            Assert.IsNotNull(s);
            Assert.AreEqual("IhS5TwCvOOaJE7hdwQQYjHCU5n5tLONF4/0Jc43r", m["nonce"]);
            Assert.Null(m["qop"]);
        }
    }
}
