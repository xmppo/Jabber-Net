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
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System.Xml;
using jabber;
using jabber.client;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace test.jabber.connection
{
    [TestFixture]
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

                Assert.IsTrue(packet.OuterXml.Replace(" ", "") ==
                              GetPresenceWithCaps(cm.Ver).Replace(" ", ""));
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
                        return arg0.OuterXml.Replace(" ", "") ==
                               GetIQResponse(nodever).Replace(" ", "");
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
                         "hash=\"sha-1\" xmlns=\"{4}\"/>" +
                    "</presence>",
                    PRIORITY, STATUS, ver, NODE, URI.CAPS);
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

    }
}
