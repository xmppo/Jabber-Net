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
using System.Xml;

using bedrock.util;

using jabber;
using jabber.client;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using jabber.protocol.x;

using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace test.jabber.connection
{
    [TestFixture]
    [SVN(@"$Id$")]
    public class CapsManagerTest
    {
        private MockRepository mocks;

        private JabberClient stream;
        private XmlDocument doc;

        private readonly JID TO_JID = new JID("user1@test.com");
        private readonly JID FROM_JID = new JID("user2@test.com");
        
        private const string TEST_ID = "TEST_ID";
        
        private const string NODE = "http://jm.jabber.com/caps";
        private const int PRIORITY = 2;
        private const string STATUS = "Ready to Chat";

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();

            stream = mocks.DynamicMock<JabberClient>();

            doc = new XmlDocument();
        }

        [Test]
        public void OnBeforePresenceOutTest()
        {
            IEventRaiser presHandler;
            using (mocks.Record())
            {
                stream.OnBeforePresenceOut += null;
                presHandler = LastCall.IgnoreArguments().GetEventRaiser();
            }

            using (mocks.Playback())
            {
                CapsManager cm = new CapsManager();
                cm.Stream = stream;
                cm.Node = NODE;

                Presence packet = CreatePresencePacket();
                presHandler.Raise(new object[] { null, packet });

                string original = packet.OuterXml.Replace(" ", "");
                string comparison = GetPresenceWithCaps(cm.Ver).Replace(" ", "");
                Assert.IsTrue(original == comparison);
            }
        }

        private delegate T Func<A0, T>(A0 arg0);

        [Test]
        public void IqRequestTest()
        {
            string nodever = "";

            IEventRaiser iqEvent;
            using (mocks.Record())
            {
                stream.OnIQ += null;
                iqEvent = LastCall.IgnoreArguments().GetEventRaiser();

                Expect.Call(stream.Document).Return(doc);
                stream.Write((XmlElement)null);
                LastCall.Callback((Func<XmlElement, bool>)
                    delegate(XmlElement arg0)
                        {
                            string original = arg0.OuterXml.Replace(" ", "");
                            string comparison = GetIQResponse(nodever).Replace(" ", "");
                            return original == comparison;
                        });
            }

            using (mocks.Playback())
            {
                CapsManager cm = new CapsManager();
                cm.Stream = stream;
                cm.Node = NODE;

                nodever = cm.NodeVer;

                iqEvent.Raise(new object[] { null, CreateIqRequest() });
            }
        }

        private string GetIQResponse(string nodever)
        {
            return
                string.Format(
                    "<iq id=\"{0}\" type=\"result\" from=\"{1}\" to=\"{2}\">" +
                      "<query xmlns=\"{3}\" node=\"{4}\"/>" +
                    "</iq>",
                    TEST_ID, TO_JID, FROM_JID, URI.DISCO_INFO, nodever);
        }

        private IQ CreateIqRequest()
        {
            IQ iq = new IQ(doc);
            iq.To = TO_JID;
            iq.From = FROM_JID;
            iq.Type = IQType.get;
            iq.ID = TEST_ID;

            DiscoInfo info = new DiscoInfo(doc);
            info.SetAttribute("xmlns", URI.DISCO_INFO);
            iq.Query = info;

            return iq;
        }

        private static string GetPresenceWithCaps(string ver)
        {
            return
                string.Format(
                    "<presence>" +
                      "<priority>{0}</priority>" +
                      "<status>{1}</status>" +
                      "<c ver=\"{2}\"" +
                         "node=\"{3}\"" +
                         "hash=\"sha-1\"" +
                         "xmlns=\"{4}\"/>" +
                    "</presence>",
                    PRIORITY, STATUS, ver, NODE, URI.CAPS, URI.CLIENT);
        }

        private Presence CreatePresencePacket()
        {
            /*
            <presence>
              <priority>2</priority>
              <status>Ready to Chat</status>
            </presence>"
            */

            Presence packet = new Presence(doc);
            packet.IntPriority = PRIORITY;
            packet.Status = STATUS;

            return packet;
        }

        [Test]
        public void SimpleGenerationExample()
        {
            CapsManager cm = new CapsManager();
            cm.AddIdentity("client", "pc", null, "Exodus 0.9.1");
            cm.AddFeature("http://jabber.org/protocol/muc");
            cm.AddFeature("http://jabber.org/protocol/disco#info");
            cm.AddFeature("http://jabber.org/protocol/disco#items");
            Assert.AreEqual("SrFo9ar2CCk2EnOH4q4QANeuxLQ=", cm.Ver);
        }

        [Test]
        public void ComplexGenerationExample()
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<book xml:lang='en'/>");
            XmlElement book = doc.DocumentElement;
            foreach (XmlAttribute attr in book.Attributes)
            {
                System.Console.WriteLine(attr.Name);
            }

            XmlElement root = doc.DocumentElement;

            DiscoInfo info = new DiscoInfo(doc);
            info.AddFeature("http://jabber.org/protocol/muc");
            info.AddFeature("http://jabber.org/protocol/disco#info");
            info.AddFeature("http://jabber.org/protocol/disco#items");
            info.AddIdentity("client", "pc", "Psi 0.9.1", "en");
            info.AddIdentity("client", "pc", "\u03a8 0.9.1", "el");
            Data x = info.CreateExtension();
            x.FormType = "urn:xmpp:dataforms:softwareinfo";
            x.AddField("ip_version").Vals = new string[] { "ipv4", "ipv6" };
            x.AddField("os").Val = "Mac";
            x.AddField("os_version").Val = "10.5.1";
            x.AddField("software").Val = "Psi";
            x.AddField("software_version").Val = "0.11";

            DiscoNode dn = new DiscoNode(new JID(null, "placeholder", null), null);
            dn.AddInfo(info);

            CapsManager cm = new CapsManager(dn);
            Assert.AreEqual("8lu+88MRxmKM7yO3MEzY7YmTsWs=", cm.Ver);
        }
    }
}
