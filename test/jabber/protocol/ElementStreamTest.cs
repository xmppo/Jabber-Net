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
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
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
        
        public void Test_Partial()
        {
            ElementStream m_ElementStream = new ElementStream();
            m_ElementStream.OnElement += new ProtocolHandler(jabOnElement);
            m_ElementStream.OnDocumentStart += new ProtocolHandler(jabOnStart);
            m_ElementStream.OnDocumentEnd += new ObjectHandler(jabOnEnd);
            Console.WriteLine("Pushing <test>");
            byte[] m_buf = Encoding.ASCII.GetBytes("<stream>");
            m_ElementStream.Push(m_buf);
            Console.WriteLine("Pushing incomplete packet");
            m_buf = Encoding.ASCII.GetBytes("<te");
            m_ElementStream.Push(m_buf);
            Console.WriteLine("End of test");

            Assertion.Assert(! fail);
        }

        void jabOnElement(object s, XmlElement e)
        {
        }

        void jabOnStart(object s, XmlElement e)
        {
        }

        void jabOnEnd(object s)
        {
            Console.WriteLine("OnEnd");
            fail = true;
        }

    }
}
