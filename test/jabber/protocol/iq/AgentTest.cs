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
using jabber;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace test.jabber.protocol.iq
{
    /// <summary>
    /// Test Agents
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class AgentTest
    {
        XmlDocument doc = new XmlDocument();
        [SetUp]
        private void SetUp()
        {
            Element.ResetID();
        }
        public void Test_Create()
        {
            AgentsQuery r = new AgentsQuery(doc);
            Assertion.AssertEquals("<query xmlns=\"jabber:iq:agents\" />", r.ToString());
        }
    
        public void Test_Item()
        {
            AgentsIQ aiq = new AgentsIQ(doc);
            AgentsQuery q = (AgentsQuery) aiq.Query;
            Agent a = q.AddAgent();
            a.JID = new JID("hildjj@jabber.com");
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"get\"><query xmlns=\"jabber:iq:agents\">" +
                "<agent jid=\"hildjj@jabber.com\" /></query></iq>",
                aiq.ToString());
        }
        public void Test_GetItems()
        {
            AgentsIQ aiq = new AgentsIQ(doc);
            AgentsQuery r = (AgentsQuery) aiq.Query;
            Agent a = r.AddAgent();
            a.JID = new JID("hildjj@jabber.com");
            a = r.AddAgent();
            a.JID = new JID("hildjj@jabber.org");
            Agent[] agents = r.GetAgents();
            Assertion.AssertEquals(agents.Length, 2);
            Assertion.AssertEquals(agents[0].JID, "hildjj@jabber.com");
            Assertion.AssertEquals(agents[1].JID, "hildjj@jabber.org");
        }
        public void Test_Transport()
        {
            AgentsIQ aiq = new AgentsIQ(doc);
            aiq.Type = IQType.result;
            AgentsQuery r = (AgentsQuery) aiq.Query;
            Agent a = r.AddAgent();
            a.JID = new JID("hildjj@jabber.com");
            a.Transport = true;
            Assertion.AssertEquals(a.Transport, true);
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"result\"><query xmlns=\"jabber:iq:agents\">" +
                "<agent jid=\"hildjj@jabber.com\"><transport /></agent></query></iq>",
                aiq.ToString());
            a.Transport = false;
            Assertion.AssertEquals(a.Transport, false);
            a.Groupchat = true;
            Assertion.AssertEquals(a.Groupchat, true);
            Assertion.AssertEquals("<iq id=\"JN_1\" type=\"result\"><query xmlns=\"jabber:iq:agents\">" +
                "<agent jid=\"hildjj@jabber.com\"><groupchat /></agent></query></iq>",
                aiq.ToString());
        }
    }
}