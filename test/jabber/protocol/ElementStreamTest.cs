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
using System;

using System.Text;
using System.Xml;
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

        [Test] public void Test_Partial()
        {
            fail = false;
            AsynchElementStream m_ElementStream = new AsynchElementStream();
            m_ElementStream.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            m_ElementStream.Push(ENC.GetBytes("<stream>"));
            m_ElementStream.Push(ENC.GetBytes("<te"));

            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(! fail);
        }

        /*
        [Test] public void Test_NullBody()
        {
            fail = false;
            AsynchElementStream m_ElementStream = new AsynchElementStream();
            m_ElementStream.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            m_ElementStream.Push(ENC.GetBytes("<str"));
            m_ElementStream.Push(ENC.GetBytes("eam/>"));

            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(! fail);
        }
*/

        /* The server should protect from these.  Good thing, since
         * it doesn't work.  :|
        [Test] public void Test_Comment()
        {
            fail = false;
            ElementStream m_ElementStream = new ElementStream();
            m_ElementStream.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            m_ElementStream.Push(ENC.GetBytes("<stream><!-- <foo/>"));
            m_ElementStream.Push(ENC.GetBytes(" --></stream>"));

            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(! fail);
        }
*/
        void jabOnEnd(object s)
        {
            fail = true;
        }

    }
}
