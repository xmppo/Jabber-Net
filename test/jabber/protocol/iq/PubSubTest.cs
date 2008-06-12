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
using System.Xml;
using bedrock.util;
using jabber.protocol.iq;
using NUnit.Framework;

namespace test.jabber.protocol.iq
{
    [TestFixture]
    [SVN(@"$Id$")]
    public class PubSubTest
    {
        private const string NODE = "TestNode";

        private XmlDocument doc;

        [SetUp]
        public void Setup()
        {
            doc = new XmlDocument();
        }

        [Test]
        public void AffiliationsTest()
        {
            PubSubIQ iq = new PubSubIQ(doc, PubSubCommandType.affiliations, NODE);
            Affiliations test = iq.Command as Affiliations;
            Assert.IsNotNull(test);
        }

        [Test]
        public void PubSubCreateTest()
        {
            PubSubIQ iq = new PubSubIQ(doc, PubSubCommandType.create, NODE);
            Assert.IsFalse(((Create)iq.Command).HasConfigure);

            Create create = (Create)iq.Command;

            create.HasConfigure = true;
            Assert.IsTrue(create.HasConfigure);
            Assert.IsNotNull(create.GetConfiguration());
        }

    }
}
