/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net can be used under either JOSL or the GPL.
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
