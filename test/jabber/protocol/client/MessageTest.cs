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

namespace test.jabber.protocol.client
{
    /// <summary>
    /// Summary description for MessageTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class MessageTest
    {
        XmlDocument doc = new XmlDocument();
        [SetUp]
        private void SetUp()
        {
            Element.ResetID();
        }
        public void Test_Create()
        {
            Message msg = new Message(doc);
            msg.Html = "foo";
            // TODO: deal with the namespace problem here
            // msg.Html = "f<b>o</b>o";
            Assertion.AssertEquals("<message id=\"JN_1\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><body>foo</body></html></message>", msg.ToString());
        }
        public void Test_NullBody()
        {
            Message msg = new Message(doc);
            Assertion.AssertEquals(null, msg.Body);
            msg.Body = "foo";
            Assertion.AssertEquals("foo", msg.Body);
            msg.Body = null;
            Assertion.AssertEquals(null, msg.Body);
        }
        public void Test_Normal()
        {
            Message msg = new Message(doc);
            Assertion.AssertEquals(MessageType.normal, msg.Type);
            Assertion.AssertEquals("", msg.GetAttribute("type"));
            msg.Type = MessageType.chat;
            Assertion.AssertEquals(MessageType.chat, msg.Type);
            Assertion.AssertEquals("chat", msg.GetAttribute("type"));
            msg.Type = MessageType.normal;
            Assertion.AssertEquals(MessageType.normal, msg.Type);
            Assertion.AssertEquals("", msg.GetAttribute("type"));
        }
        public void Test_Escape()
        {
            Message msg = new Message(doc);
            msg.Body = "&";
            Assertion.AssertEquals("<message id=\"JN_1\"><body>&amp;</body></message>", msg.ToString());            
            msg.RemoveChild(msg["body"]);                   
            Assertion.AssertEquals("<message id=\"JN_1\"></message>", msg.ToString());
            try
            {
                msg.Html = "&";
                Assertion.Assert("should have thrown an exception", false);
            }
            catch
            {
                Assertion.Assert("Threw exception, as expected", true);
            }
        }
    }
}