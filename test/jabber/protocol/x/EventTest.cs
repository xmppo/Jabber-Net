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
using jabber.protocol.x;

namespace test.jabber.protocol.x
{
    /// <summary>
    /// Summary description for AuthTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class EventTest
    {
        XmlDocument doc = new XmlDocument();
        [SetUp]
        private void SetUp()
        {
            Element.ResetID();
        }
        public void Test_Create()
        {
            Event e = new Event(doc);
            Assertion.AssertEquals("<x xmlns=\"jabber:x:event\" />", e.ToString());
            e.ID = "foo";
            Assertion.AssertEquals("<x xmlns=\"jabber:x:event\"><id>foo</id></x>", e.ToString());
            Assertion.AssertEquals("foo", e.ID);
            Assertion.AssertEquals(EventType.NONE, e.Type);
            e.Type = EventType.composing;
            Assertion.AssertEquals(EventType.composing, e.Type);
            e.Type = EventType.delivered;
            Assertion.AssertEquals(EventType.delivered, e.Type);
            Assertion.AssertEquals("<x xmlns=\"jabber:x:event\"><id>foo</id><delivered /></x>", e.ToString());
            Assertion.AssertEquals(true, e.IsDelivered);
            Assertion.AssertEquals(false, e.IsComposing);
            e.IsComposing = true;
            Assertion.AssertEquals("<x xmlns=\"jabber:x:event\"><id>foo</id><delivered /><composing /></x>", e.ToString());
            Assertion.AssertEquals(EventType.composing | EventType.delivered, e.Type);
        }
    }
}