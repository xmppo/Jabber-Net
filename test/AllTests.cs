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
using NUnit.Framework;
using bedrock.util;
namespace test
{
    /// <summary>
    ///    Summary description for AllTests.
    /// </summary>
    [RCS(@"$Header$")]
    public class AllTests
    {
        private static Type[] TestTypes = 
        {
            //typeof(test.bedrock.dns.PacketTest),
            typeof(test.bedrock.util.ComplexTest),
            typeof(test.bedrock.util.GetOptTest),
            typeof(test.bedrock.util.VersionTest),
            typeof(test.bedrock.io.PipeStreamTest),
            typeof(test.bedrock.collections.ByteStackTest),
            typeof(test.bedrock.collections.SkipListTest),
            typeof(test.bedrock.collections.TreeTest),
            typeof(test.bedrock.collections.TrieNodeTest),
            typeof(test.bedrock.collections.TrieTest),
            typeof(test.jabber.protocol.PacketTest),
            typeof(test.jabber.JIDTest),
            typeof(test.jabber.client.PPDBTest),
            typeof(test.jabber.protocol.stream.StreamTest),
            typeof(test.jabber.protocol.accept.RouteTest),
            typeof(test.jabber.protocol.client.IQTest),
            typeof(test.jabber.protocol.client.MessageTest),
            typeof(test.jabber.protocol.client.PresenceTest),
            typeof(test.jabber.protocol.iq.AuthTest),
            typeof(test.jabber.protocol.iq.RosterTest),
            typeof(test.jabber.protocol.iq.AgentTest),
			typeof(test.jabber.protocol.x.EventTest)
        };

        public static ITest Suite
        {
            get 
            {
                TestSuite suite = new TestSuite("jubjub tests");
                foreach (Type t in TestTypes)
                {
                    suite.AddTest(new TestSuite(t));
                }
                return suite;
            }
        }
    }
}
