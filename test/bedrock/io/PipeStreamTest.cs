/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
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
        [Test] public void Test_GT()
        {
            byte[] b = ENC.GetBytes("<foo/>");
            byte[] r = new byte[1024];
            int len;
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual(b.Length, len);
            Assert.AreEqual("<foo/>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("<foo/><foo/>");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("<foo/>", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("<foo/>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("<foo/><fo");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("<foo/>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("><foo/>");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("<fo", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual(">", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("<foo/>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("<foo><bar/></foo>");
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("<foo>", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("<bar/>", ENC.GetString(r, 0, len));
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("</foo>", ENC.GetString(r, 0, len));
        }
        [Test] public void Test_All()
        {
            byte[] b = ENC.GetBytes("Hello>");
            byte[] r = new byte[1024];
            int len;
            p.Write(b, 0, b.Length);
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual(b.Length, len);
            Assert.AreEqual("Hello>", ENC.GetString(r, 0, len));
            b = ENC.GetBytes("Hello>");
            p.Write(b);
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual(b.Length, len);
            Assert.AreEqual("Hello>", ENC.GetString(r, 0, len));
            // sync/blocking
            new Thread(new ThreadStart(delay)).Start();
            len = p.Read(r, 0, 2);
            Assert.AreEqual("Do", ENC.GetString(r, 0, len));
            
            len = p.Read(r, 0, r.Length);
            Assert.AreEqual("ne!!!>", ENC.GetString(r, 0, len));
        }
        private void delay()
        {
            Thread.Sleep(500);
            p.Write(System.Text.Encoding.ASCII.GetBytes("Done!!!>"));
        }
    }
}