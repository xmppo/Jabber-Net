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
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
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
using jabber;
using jabber.client;
using jabber.protocol.client;

namespace test.jabber.client1 // TODO: Client1 due to a bug in NUnit.  
{
    /// <summary>
    /// Summary description for PPDP.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class PPDBTest
    {
        XmlDocument doc = new XmlDocument();

        public void Test_Create()
        {
            PPDB pp = new PPDB();
            Assertion.AssertEquals("jabber.client.PPDB", pp.GetType().FullName);
        }
        public void TestAdd()
        {
            PPDB pp = new PPDB();
            Presence pres = new Presence(doc);
            JID f = new JID("foo", "bar", "baz");
            pres.From = f;
            pp.AddPresence(pres);
            Assertion.AssertEquals("foo@bar/baz", pp[f].From.ToString());
            f.Resource = null;
            Assertion.AssertEquals("foo@bar/baz", pp[f].From.ToString());
        }
		public void TestRetrieve()
		{
			PPDB pp = new PPDB();
			Presence pres = new Presence(doc);
			JID f = new JID("foo", "bar", "baz");
			pres.From = f;
			pres.Priority = "0";
			pp.AddPresence(pres);
			Assertion.AssertEquals("foo@bar/baz", pp[f.Bare].From.ToString());

			pres = new Presence(doc);
			f = new JID("foo", "bar", "bay");
			pres.From = f;
			pres.Priority = "1";
			pp.AddPresence(pres);
			Assertion.AssertEquals("foo@bar/bay", pp[f.Bare].From.ToString());

			pres = new Presence(doc);
			pres.From = f;
			pres.Type = PresenceType.unavailable;
			pp.AddPresence(pres);
			Assertion.AssertEquals("foo@bar/baz", pp[f.Bare].From.ToString());
		}
    }
}
