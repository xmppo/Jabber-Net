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
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System;
using NUnit.Framework;

using bedrock;
using bedrock.util;
using jabber.protocol;

namespace test.jabber.protocol
{
    /// <summary>
    /// Summary description for ElementStreamTest.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class ElementStreamTest
    {
        private bool fail = false;
        private System.Text.Encoding ENC = System.Text.Encoding.UTF8;
        private AutoResetEvent are = new AutoResetEvent(false);
        private int count = 0;

        /*
         * Try several ways to generate PartialTokenException.
         */
        [Test] public void Test_Partial()
        {
            fail = false;
            AsynchElementStream es = new AsynchElementStream();
            es.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            es.Push(ENC.GetBytes("<stream>"));
            es.OnElement += new ProtocolHandler(jabOnElement);
            es.Push(ENC.GetBytes("<te"));

            are.WaitOne(100, true);
            Assert.IsTrue(! fail);

            es.Push(ENC.GetBytes("st/>"));
            es.Push(ENC.GetBytes("<test>"));
            es.Push(ENC.GetBytes("</"));
            es.Push(ENC.GetBytes("test>"));
            es.Push(ENC.GetBytes("<test>&#1"));
            es.Push(ENC.GetBytes("16;est</test>"));
            es.Push(ENC.GetBytes("<test>"));
            es.Push(new byte[] {0xC5});
            es.Push(new byte[] {0x81});
            es.Push(ENC.GetBytes("</test>"));
            es.Push(ENC.GetBytes("<test f"));
            es.Push(ENC.GetBytes("oo='bar'/>"));
            es.Push(ENC.GetBytes("<test foo="));
            es.Push(ENC.GetBytes("'bar'/>"));
            es.Push(ENC.GetBytes("<test foo='"));
            es.Push(ENC.GetBytes("bar'/>"));
            es.Push(new byte[] {} );
        }

        /*
         * What happens if we try to parse more than 4k of data at once?
         */
        [Test] public void Test_Large()
        {
            AsynchElementStream es = new AsynchElementStream();
            // es.OnElement += new ProtocolHandler(jabOnElement);

            es.Push(ENC.GetBytes("<stream>"));
            byte[] buf = ENC.GetBytes("<test/>");
            MemoryStream ms = new MemoryStream();
            for (int i=0; i<1024; i++)
            {
                ms.Write(buf, 0, buf.Length);
            }
            es.Push(ms.ToArray());
        }

        [Test]
        public void Test_Large_Partial()
        {
            count = 0;
            AsynchElementStream es = new AsynchElementStream();
            es.OnElement += new ProtocolHandler(es_OnElement);
            es.Push(ENC.GetBytes("<stream>"));
            string test = "<presence from='xxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxx@myjabber.net' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxx@myjabber.net' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxx@jabber.org/Home' to='xxxxxx@jabber.org/Test'><priority>1</priority><c xmlns='http://jabber.org/protocol/caps' node='http://pidgin.im/caps' ver='2.3.1' ext='moodn nickn tunen avatar'/><x xmlns='vcard-temp:x:update'><photo>d206b82e4c1478ab033dacc55eacb1cfda7706c8</photo></x></presence><presence from='xxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cancel'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence><presence from='xxxxxxxxx@aim.ijabber.com' to='xxxxxx@jabber.org/Test' type='error'><error code='404' type='cance";
            es.Push(ENC.GetBytes(test));
            test = "l'><remote-server-not-found xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'/></error></presence>";
            es.Push(ENC.GetBytes(test));
            Assert.AreEqual(count, 20);
        }

        private void es_OnElement(object sender, XmlElement rp)
        {
            count++;
        }

        /*
        [Test] public void Test_NullBody()
        {
            fail = false;
            AsynchElementStream es = new AsynchElementStream();
            es.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            es.Push(ENC.GetBytes("<str"));
            es.Push(ENC.GetBytes("eam/>"));

            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(! fail);
        }
*/

        /* The server should protect from these.  Good thing, since
         * it doesn't work.  :|
        [Test] public void Test_Comment()
        {
            fail = false;
            ElementStream es = new ElementStream();
            es.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            es.Push(ENC.GetBytes("<stream><!-- <foo/>"));
            es.Push(ENC.GetBytes(" --></stream>"));

            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(! fail);
        }
*/
        void jabOnEnd(object s)
        {
            fail = true;
            are.Set();
        }

        void jabOnElement(Object sender, System.Xml.XmlElement elem)
        {
            Console.WriteLine(elem.OuterXml);
            Assert.AreEqual("test", elem.Name);
        }
    }
}
