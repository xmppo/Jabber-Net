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
using jabber.protocol.client;
using jabber.protocol.iq;

namespace test.jabber.protocol.client
{
    /// <summary>
    /// Summary description for IQTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class IQTest
    {
        XmlDocument doc = new XmlDocument();

        [Test]
        public void Create()
        {
			Element.ResetID();

			IQ iq = new IQ(doc);
            Assert.AreEqual("<iq id=\"JN_1\" type=\"get\" />", iq.ToString());
            iq = new IQ(doc);
            Assert.AreEqual("<iq id=\"JN_2\" type=\"get\" />", iq.ToString());
            iq.Query = new Auth(doc);
            Assert.AreEqual("<iq id=\"JN_2\" type=\"get\"><query xmlns=\"jabber:iq:auth\" /></iq>", iq.ToString());
        }
    }
}