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
        public void SetUp()
        {
            Element.ResetID();
        }
        [Test] public void Test_Create()
        {
            Message msg = new Message(doc);
            msg.Html = "foo";
            Assert.AreEqual("<message id=\"JN_1\"><html xmlns=\"http://jabber.org/protocol/xhtml-im\"><body xmlns=\"http://www.w3.org/1999/xhtml\">foo</body></html><body>foo</body></message>", msg.ToString());
            // TODO: deal with the namespace problem here
            msg.Html = "f<a href=\"http://www.jabber.org\">o</a>o";
            Assert.AreEqual("<message id=\"JN_1\"><html xmlns=\"http://jabber.org/protocol/xhtml-im\"><body xmlns=\"http://www.w3.org/1999/xhtml\">f<a href=\"http://www.jabber.org\">o</a>o</body></html><body>foo</body></message>", msg.ToString());
            Assert.AreEqual("f<a href=\"http://www.jabber.org\">o</a>o", msg.Html);
        }
        [Test] public void Test_NullBody()
        {
            Message msg = new Message(doc);
            Assert.AreEqual(null, msg.Body);
            msg.Body = "foo";
            Assert.AreEqual("foo", msg.Body);
            msg.Body = null;
            Assert.AreEqual(null, msg.Body);
        }
        [Test] public void Test_Normal()
        {
            Message msg = new Message(doc);
            Assert.AreEqual(MessageType.normal, msg.Type);
            Assert.AreEqual("", msg.GetAttribute("type"));
            msg.Type = MessageType.chat;
            Assert.AreEqual(MessageType.chat, msg.Type);
            Assert.AreEqual("chat", msg.GetAttribute("type"));
            msg.Type = MessageType.normal;
            Assert.AreEqual(MessageType.normal, msg.Type);
            Assert.AreEqual("", msg.GetAttribute("type"));
        }
        [Test] public void Test_Escape()
        {
            Message msg = new Message(doc);
            msg.Body = "&";
            Assert.AreEqual("<message id=\"JN_1\"><body>&amp;</body></message>", msg.ToString());            
            msg.RemoveChild(msg["body"]);                   
            Assert.AreEqual("<message id=\"JN_1\"></message>", msg.ToString());
            try
            {
                msg.Html = "&";
                Assert.Fail("should have thrown an exception");
            }
            catch
            {
                Assert.IsTrue(true, "Threw exception, as expected");
            }
        }
    }
}