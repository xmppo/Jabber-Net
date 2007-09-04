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
using System.IO;

using NUnit.Framework;
using bedrock.io;
using bedrock.util;

namespace test.bedrock.io
{
    /// <summary>
    /// Test the ZlibStream class.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class ZlibStreamTest
    {
        private static readonly System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        // python:
        // [ord(x) for x in zlib.compress("Hello, world")]
        private const string HELLO_STR = "Hello, world";
        private static readonly byte[] HELLO_BYTES = new byte[] 
        {
            120, 156, 243,  72, 205, 201, 201, 215,
             81,  40, 207,  47, 202,  73,   1,   0, 
             27, 212,   4, 105 
        };

        [Test]
        public void Read()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(HELLO_BYTES, 0, HELLO_BYTES.Length);
            ms.Seek(0, SeekOrigin.Begin);
            ZlibStream z = new ZlibStream(ms);
            byte[] buf = new byte[1024];
            int len = z.Read(buf, 0, buf.Length);
            Assert.Greater(len, 0);
            string str = ENC.GetString(buf, 0, len);
            Assert.AreEqual(HELLO_STR, str);
        }

        [Test]
        public void Write()
        {
            byte[] buf = ENC.GetBytes(HELLO_STR);
            MemoryStream ms = new MemoryStream();
            ZlibStream z = new ZlibStream(ms, 4);
            z.Write(buf, 0, buf.Length);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] res = ms.ToArray();
            Assert.AreEqual(HELLO_BYTES.Length, res.Length);
            int count = 0;
            foreach (byte b in res)
            {
                Assert.AreEqual(HELLO_BYTES[count++], b);
            }
        }
    }
}
