using System.Xml;
using jabber;
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
    public class DiscoManagerTest
    {
        private MockRepository mocks;
        private DiscoManager dm;
        private XmppStream stream;
        private IIQTracker tracker;
        private XmlDocument doc;

        [SetUp]
        public void setup()
        {
            mocks = new MockRepository();
            dm = new DiscoManager();

            stream = mocks.DynamicMock<XmppStream>();
            tracker = mocks.DynamicMock<IIQTracker>();
            dm.Stream = stream;

            doc = new XmlDocument();
        }

        [TearDown]
        public void cleanUp()
        {
            DiscoNode.Clear();
        }

        private readonly JID jid = new JID("test.com");
        private readonly string NODE = "TEST_NODE";
        private readonly string FEATURE = "TEST_FEATURE";

        [Test]
        public void IntialDiscoTest()
        {
            IEventRaiser onAuth;

            IQ sentIq = null;
            IqCB sentCallback = null;

            mocks.BackToRecordAll();
            using (mocks.Record())
            {
                Expect.Call(stream.Server).Return(jid);
                stream.OnAuthenticate += null;
                onAuth = LastCall.IgnoreArguments().GetEventRaiser();

                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(
                    delegate(IQ arg0, IqCB arg1, object arg2)
                    {
                        // Grab the iq and callback so this part of
                        // the code can finish. Call the callback later.
                        sentIq = arg0;
                        sentCallback = arg1;

                        string id = arg0.GetAttribute("id");
                        string original = arg0.OuterXml;
                        return original.Replace(" ", "") ==
                               GetInfoXml(id).Replace(" ", "");
                    });

                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(
                    delegate(IQ arg0, IqCB arg1, object arg2)
                    {
                        string id = arg0.GetAttribute("id");
                        string original = arg0.OuterXml;
                        return original.Replace(" ", "") ==
                               GetItemsForServiceXml(id).Replace(" ", "");
                    });
            }

            using (mocks.Playback())
            {
                DiscoManager newDm = new DiscoManager();
                newDm.Stream = stream;

                onAuth.Raise(new object[] { null });

                if (sentIq != null)
                {
                    string id = sentIq.GetAttribute("id");
                    if (sentCallback != null)
                        sentCallback(null, CreateDiscoInfoResponse(id), newDm.Root);
                }
            }
        }

        [Test]
        public void BeginGetItemsTest()
        {
            mocks.BackToRecordAll();
            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(
                    delegate(IQ arg0, IqCB arg1, object arg2)
                    {
                        string id = arg0.GetAttribute("id");
                        string original = arg0.OuterXml;
                        return original.Replace(" ", "") ==
                            GetItemsXml(id).Replace(" ", "");
                    });
            }

            using (mocks.Playback())
            {
                dm.BeginGetItems(jid, NODE, delegate { }, null);
            }
        }

        [Test]
        public void BeginGetFeaturesTest()
        {
            mocks.BackToRecordAll();
            using (mocks.Record())
            {
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(
                    delegate(IQ arg0, IqCB arg1, object arg2)
                    {
                        string id = arg0.GetAttribute("id");
                        string original = arg0.OuterXml;
                        return original.Replace(" ", "") ==
                            GetFeaturesXml(id).Replace(" ", "");
                    });
            }

            using (mocks.Playback())
            {
                dm.BeginGetFeatures(jid, NODE, delegate { }, null);
            }
        }

        [Test]
        public void BeginGetServiceTest()
        {
            mocks.BackToRecordAll();
            using (mocks.Record())
            {
                Expect.Call(stream.Server).Return(jid);
                Expect.Call(stream.Document).Return(doc);
                SetupTrackerBeginIq(
                    delegate(IQ arg0, IqCB arg1, object arg2)
                    {
                        string id = arg0.GetAttribute("id");
                        string original = arg0.OuterXml;
                        return original.Replace(" ", "") ==
                            GetItemsForServiceXml(id).Replace(" ", "");
                    });
            }

            using (mocks.Playback())
            {
                dm.BeginFindServiceWithFeature(FEATURE, delegate { }, null);
            }
        }

        private IQ CreateDiscoInfoResponse(string id)
        {
            IQ returnIq = new IQ(doc);
            returnIq.SetAttribute("id", id);
            returnIq.SetAttribute("from", jid);
            returnIq.SetAttribute("type", "result");

            DiscoInfo info = new DiscoInfo(doc);
            info.AddIdentity("server", "im", "jabber2 4.2.16.6", null);
            info.AddFeature(URI.DISCO_ITEMS);
            info.AddFeature(URI.DISCO_INFO);

            returnIq.Query = info;

            return returnIq;
        }

        private string GetInfoXml(string id)
        {
            return
                string.Format(
                    "<iq id=\"{0}\" type=\"get\" to=\"{1}\">" +
                      "<query xmlns=\"{2}\"/>" +
                    "</iq>",
                    id, jid, URI.DISCO_INFO);
        }

        private string GetItemsForServiceXml(string id)
        {
            return
                string.Format(
                    "<iq id=\"{0}\" type=\"get\" to=\"{1}\">" +
                      "<query xmlns=\"{2}\"/>" +
                    "</iq>",
                    id, jid, URI.DISCO_ITEMS);
        }

        private string GetFeaturesXml(string id)
        {
            return
                string.Format(
                    "<iq id=\"{0}\" type=\"get\" to=\"{1}\">" +
                      "<query node=\"{2}\" xmlns=\"{3}\" />" +
                    "</iq>",
                    id, jid, NODE, URI.DISCO_INFO);
        }

        private string GetItemsXml(string id)
        {
            return
                string.Format(
                    "<iq id=\"{0}\" type=\"get\" to=\"{1}\">" +
                      "<query node=\"{2}\" xmlns=\"{3}\"/>" +
                    "</iq>",
                    id, jid, NODE, URI.DISCO_ITEMS);
        }

        private delegate T Func<A0, A1, A2, T>(A0 arg0, A1 arg1, A2 arg2);
        private void SetupTrackerBeginIq(Func<IQ, IqCB, object, bool> func)
        {
            Expect.Call(stream.Tracker).Return(tracker);
            tracker.BeginIQ(null, null, null);
            LastCall.Callback(func);
        }

        [Test]
        public void IdentityLang()
        {
            DiscoIdentity id = new DiscoIdentity(doc);
            Assert.IsNull(id.Lang);
            id.Lang = "en";
            Assert.AreEqual("en", id.Lang);
            id.Lang = "el";
            Assert.AreEqual("el", id.Lang);
            id.Lang = null;
            Assert.IsNull(id.Lang);
        }
    }
}
