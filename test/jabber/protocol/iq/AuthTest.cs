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
using jabber.protocol.client;
using jabber.protocol.iq;

namespace test.jabber.protocol.iq
{
    /// <summary>
    /// Summary description for AuthTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class AuthTest
    {
        XmlDocument doc = new XmlDocument();
        [SetUp]
        private void SetUp()
        {
            Element.ResetID();
        }
        public void Test_Create()
        {
            IQ iq = new AuthIQ(doc);
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:auth\" /></iq>", iq.ToString());
        }
        public void Test_Hash()
        {
            IQ iq = new AuthIQ(doc);
            iq.Type = IQType.set;
            Auth a = (Auth) iq.Query;
            a.SetDigest("foo", "bar", "3B513636");
            a.Resource = "Home";
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"set\"><query xmlns=\"jabber:iq:auth\">" + 
                "<username>foo</username>" + 
                "<digest>37d9c887967a35d53b81f07916a309a5b8d7e8cc</digest>" + 
                "<resource>Home</resource>" + 
                "</query></iq>", 
                iq.ToString());
        }
        /*
        SENT: <iq type="get" id="JCOM_14"><query xmlns="jabber:iq:auth"><username>zeroktest</username></query></iq>
        RECV: <iq id='JCOM_14' type='result'><query xmlns='jabber:iq:auth'><username>zeroktest</username><password/><digest/><sequence>499</sequence><token>3C7A6B0A</token><resource/></query></iq>
        SENT: <iq type="set" id="JCOM_15"><query xmlns="jabber:iq:auth"><username>zeroktest</username><hash>e00c83748492a3bc7e4831c9e973d117082c3abe</hash><resource>Winjab</resource></query></iq>
        */
        public void Test_ZeroK()
        {
            IQ iq = new AuthIQ(doc);
            iq.Type = IQType.set;
            Auth a = (Auth) iq.Query;
            a.SetZeroK("zeroktest", "test", "3C7A6B0A", 499);
            a.Resource = "Winjab";
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"set\"><query xmlns=\"jabber:iq:auth\">" + 
                "<username>zeroktest</username>" + 
                "<hash>e00c83748492a3bc7e4831c9e973d117082c3abe</hash>" + 
                "<resource>Winjab</resource>" + 
                "</query></iq>", 
                iq.ToString());
        }
    }
}