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
using jabber.protocol;
using jabber.protocol.iq;

namespace test.jabber.protocol.iq
{
	/// <summary>
	/// Summary description for RosterTest.
	/// </summary>
	[RCS(@"$Header$")]
	[TestFixture]
	public class RegisterTest
	{
		XmlDocument doc = new XmlDocument();
		[SetUp]
		private void SetUp()
		{
			Element.ResetID();
		}
		public void Test_Create()
		{
			Register r = new Register(doc);
			Assertion.AssertEquals("<query xmlns=\"jabber:iq:register\" />", r.ToString());
		}
		public void Test_Registered()
		{
			Register r = new Register(doc);
			r.Registered = true;
			Assertion.AssertEquals("<query xmlns=\"jabber:iq:register\"><registered /></query>", r.ToString());
			Assertion.Assert(r.Registered);
		}
	}
}
