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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
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