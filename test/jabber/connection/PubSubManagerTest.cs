/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System.Collections;
using System.Reflection;
using System.Xml;

using bedrock.util;

using jabber;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;

using NUnit.Framework;
using Rhino.Mocks;

namespace test.jabber.connection
{
    [TestFixture]
    [SVN(@"$Id$")]
    public class PubSubManagerTest
    {
        private const string PUB_SUB_XMLNS = "http://jabber.org/protocol/pubsub";

        private XmppStream stream;

        private MockRepository mocks;
        private IIQTracker tracker;
        private XmlDocument doc;

        private JID jid;
        private static readonly string NODE = "test";

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            stream = mocks.DynamicMock<XmppStream>();
            tracker = mocks.DynamicMock<IIQTracker>();
            doc = new XmlDocument();

            jid = new JID("test.example.com");
        }

        [Test]
        public void GetNodeTest()
        {
            PubSubManager mgr = GetPubSubMgr();

            PubSubNode node = mgr.GetNode(jid, NODE, 0);
            Assert.AreNotEqual(node, null);
        }

        private delegate T Func<A0, A1, A2, T>(A0 arg0, A1 arg1, A2 arg2);

        [Test]
        public void CreateNodeTest()
        {
            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(delegate(IQ iq, IqCB cb, object cbArg)
                    {
                        string id = iq.GetAttribute("id");
                        string original = iq.OuterXml.Replace(" ", "");
                        string comparison = GetCreateNodeIQ(id).Replace(" ", "");
                        return original == comparison;
                    });
            }

            using (mocks.Playback())
            {
                PubSubManager mgr = GetPubSubMgr();
                PubSubNode node = mgr.GetNode(jid, NODE, 0);
                node.Create();
            }
        }

        [Test]
        public void RemoveNodeTest()
        {
            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(delegate(IQ iq, IqCB cb, object cbArg)
                    {
                        string id = iq.GetAttribute("id");
                        string original = iq.OuterXml.Replace(" ", "");
                        string comparison = GetRemoveNodeIq(id).Replace(" ", "");
                        return original == comparison;
                    });
            }

            using (mocks.Playback())
            {
                PubSubManager mgr = GetPubSubMgr();
                mgr.RemoveNode(jid, NODE, null);
            }
        }

        [Test]
        public void AddRemoveNodeTest()
        {
            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(delegate
                {
                    return true;
                });
            }

            using (mocks.Playback())
            {
                PubSubManager mgr = GetPubSubMgr();
                mgr.GetNode(jid, NODE, 0);

                FieldInfo fieldInfo = mgr.GetType().GetField("m_nodes",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                IDictionary nodes = (IDictionary)fieldInfo.GetValue(mgr);
                if (nodes == null || nodes.Count != 1)
                    Assert.Fail("The GetNode function failed");

                mgr.RemoveNode(jid, NODE, null);
                Assert.IsTrue(nodes[new JIDNode(jid, NODE)] == null);
            }
        }

        [Test]
        public void DeleteNodeTest()
        {
            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(delegate(IQ iq, IqCB cb, object cbArg)
                    {
                        string id = iq.GetAttribute("id");
                        string original = iq.OuterXml;
                        return original.Replace(" ", "") == GetRemoveNodeIq(id).Replace(" ", "");
                    });
            }

            using (mocks.Playback())
            {
                PubSubManager mgr = GetPubSubMgr();
                mgr.GetNode(jid, NODE, 0).Delete();
            }
        }

        private const string PUB_SUB_ITEM = "TestItem";
        private const string PUB_SUB_ITEM_XMLNS = "TestUri";

        [Test]
        public void PublishItemTest()
        {
            XmlElement pubItem = CreateTestItem();

            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(delegate(IQ iq, IqCB cb, object cbArg)
                    {
                        string id = iq.GetAttribute("id");
                        string original = iq.OuterXml.Replace(" ", "");
                        string comparison = GetPublishItemIq(id).Replace(" ", "");
                        return original == comparison;
                    });
            }

            using (mocks.Playback())
            {
                PubSubManager mgr = GetPubSubMgr();
                mgr.GetNode(jid, NODE, 0).PublishItem(null, pubItem);
            }
        }

        private static XmlElement CreateTestItem()
        {
            XmlDocument myDoc = new XmlDocument();
            return myDoc.CreateElement(PUB_SUB_ITEM, PUB_SUB_ITEM_XMLNS);
        }

        private const string PUB_SUB_ID = "TestId";

        [Test]
        public void DeleteItemTest()
        {
            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(delegate(IQ iq, IqCB cb, object cbArg)
                    {
                        string id = iq.GetAttribute("id");
                        string original = iq.OuterXml.Replace(" ", "");
                        string comparison = GetDeleteItemString(id).Replace(" ", "");
                        return original == comparison;
                    });
            }

            using (mocks.Playback())
            {
                PubSubManager mgr = GetPubSubMgr();
                mgr.GetNode(jid, NODE, 0).DeleteItem(PUB_SUB_ID);
            }
        }

        private string GetDeleteItemString(string id)
        {
            return string.Format(
                "<iq id=\"{0}\" type=\"set\" to=\"{1}\">"+
                    "<pubsub xmlns=\"{2}\">"+
                        "<retract node=\"{3}\">"+
                            "<item id=\"{4}\"/>"+
                        "</retract>"+
                    "</pubsub>"+
                "</iq>",
                id, jid, PUB_SUB_XMLNS, NODE, PUB_SUB_ID);
        }

        private string GetPublishItemIq(string id)
        {
            return string.Format(
                "<iq id=\"{0}\" type=\"set\" to=\"{1}\">" +
                    "<pubsub xmlns=\"{2}\">"+
                        "<publish node=\"{3}\">"+
                            "<item>"+
                                "<{4} xmlns=\"{5}\"/>"+
                            "</item>"+
                        "</publish>"+
                    "</pubsub>"+
                "</iq>",
                id, jid, PUB_SUB_XMLNS, NODE, PUB_SUB_ITEM, PUB_SUB_ITEM_XMLNS);
        }

        private void SetupTrackerBeginIq(Func<IQ, IqCB, object, bool> func)
        {
            Expect.Call(stream.Tracker).Return(tracker);
            tracker.BeginIQ(null, null, null);
            LastCall.Callback(func);
        }

        private string GetRemoveNodeIq(string id)
        {
            return string.Format(
                "<iq id=\"{0}\" type=\"set\" to=\"{1}\">"+
                    "<pubsub xmlns=\"{2}\">"+
                        "<delete node=\"{3}\"/>"+
                    "</pubsub>"+
                "</iq>",
                id, jid, URI.PUBSUB_OWNER, NODE);
        }

        private string GetCreateNodeIQ(string id)
        {
            return
                string.Format(
                    "<iq id=\"{0}\" type=\"set\" to=\"{1}\">"+
                        "<pubsub xmlns=\"{2}\">"+
                            "<create node=\"{3}\"/>"+
                            "<configure/>"+
                        "</pubsub>"+
                    "</iq>",
                    id, jid, PUB_SUB_XMLNS, NODE);
        }

        private PubSubManager GetPubSubMgr()
        {
            PubSubManager mgr = new PubSubManager();
            mgr.Stream = stream;
            return mgr;
        }
    }
}
