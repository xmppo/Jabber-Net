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
using System.Threading;
using NUnit.Framework;
using bedrock.io;
using bedrock.util;
namespace test.bedrock.io
{
    /// <summary>
    ///    Summary description for PipeStreamTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class PipeStreamTest
    {
        private PipeStream p = new PipeStream();
        private static System.Text.Encoding ENC = System.Text.Encoding.ASCII;
        public void Test_GT()
        {
            byte[] b = ENC.GetBytes("<foo/>");
            byte[] r = new byte[1024];
            int len;
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals(b.Length, len);
            Assertion.AssertEquals("<foo/>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("<foo/><foo/>");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("<foo/>", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("<foo/>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("<foo/><fo");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("<foo/>", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("<fo", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("><foo/>");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals(">", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("<foo/>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("<foo><bar/></foo>");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("<foo>", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("<bar/>", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("</foo>", ENC.GetString(r, 0, len));
        }
        public void Test_All()
        {
            byte[] b = ENC.GetBytes("Hello");
            byte[] r = new byte[1024];
            int len;
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals(b.Length, len);
            Assertion.AssertEquals("Hello", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("Hello");
            p.Write(b);
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals(b.Length, len);
            Assertion.AssertEquals("Hello", ENC.GetString(r, 0, len));
            // sync/blocking
            new Thread(new ThreadStart(delay)).Start();
            len = p.Read(r, 0, 2);
            Assertion.AssertEquals("Do", ENC.GetString(r, 0, len));
            
            len = p.Read(r, 0, r.Length);
            Assertion.AssertEquals("ne!!!", ENC.GetString(r, 0, len));
        }
        private void delay()
        {
            Thread.Sleep(500);
            p.Write(System.Text.Encoding.ASCII.GetBytes("Done!!!"));
        }
    }
}
