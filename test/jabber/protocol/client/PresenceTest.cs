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
using jabber.protocol.client;

namespace test.jabber.protocol.client
{
    /// <summary>
    /// Summary description for PresenceTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class PresenceTest
    {
        XmlDocument doc = new XmlDocument();
        public void Test_Create()
        {
            Presence p = new Presence(doc);
            p.Type   = PresenceType.available;
            p.Status = "foo";
            Assertion.AssertEquals("<presence><status>foo</status></presence>", p.ToString());
        }

        public void Test_Available()
        {
            Presence p = new Presence(doc);
            Assertion.AssertEquals(PresenceType.available, p.Type);
            Assertion.AssertEquals("", p.GetAttribute("type"));
            p.Type = PresenceType.unavailable;
            Assertion.AssertEquals(PresenceType.unavailable, p.Type);
            Assertion.AssertEquals("unavailable", p.GetAttribute("type"));
            p.Type = PresenceType.available;
            Assertion.AssertEquals(PresenceType.available, p.Type);
            Assertion.AssertEquals("", p.GetAttribute("type"));
        }    
    }
}