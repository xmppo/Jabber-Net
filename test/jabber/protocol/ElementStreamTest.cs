/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
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
    [RCS(@"$Header$")]
    [TestFixture]
    public class ElementStreamTest
    {
        private bool fail = false;
        private System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        public void Test_Partial()
        {
            fail = false;
            AsynchElementStream m_ElementStream = new AsynchElementStream();
            m_ElementStream.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            m_ElementStream.Push(ENC.GetBytes("<stream>"));
            m_ElementStream.Push(ENC.GetBytes("<te"));

            System.Threading.Thread.Sleep(500);
            Assertion.Assert(! fail);
        }

        public void Test_NullBody()
        {
            fail = false;
            AsynchElementStream m_ElementStream = new AsynchElementStream();
            m_ElementStream.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            m_ElementStream.Push(ENC.GetBytes("<str"));
            m_ElementStream.Push(ENC.GetBytes("eam/>"));

            System.Threading.Thread.Sleep(500);
            Assertion.Assert(! fail);
        }

        /* The server should protect from these.  Good thing, since
         * it doesn't work.  :|
        public void Test_Comment()
        {
            fail = false;
            ElementStream m_ElementStream = new ElementStream();
            m_ElementStream.OnDocumentEnd += new ObjectHandler(jabOnEnd);

            m_ElementStream.Push(ENC.GetBytes("<stream><!-- <foo/>"));
            m_ElementStream.Push(ENC.GetBytes(" --></stream>"));

            System.Threading.Thread.Sleep(500);
            Assertion.Assert(! fail);
        }
*/
        void jabOnEnd(object s)
        {
            fail = true;
        }

    }
}