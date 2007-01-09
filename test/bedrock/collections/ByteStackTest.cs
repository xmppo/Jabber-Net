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


using NUnit.Framework;
using bedrock.collections;
using bedrock.util;

namespace test.bedrock.collections
{
    /// <summary>
    ///    Summary description for TemplateTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class ByteStackTest
    {
        private System.Text.Encoding ENC = System.Text.Encoding.Default;

        [Test] public void Test_Main()
        {
            ByteStack bs = new ByteStack();
            Assert.AreEqual(0, bs.Count);
            bs.Push((byte) 'a');
            Assert.AreEqual(1, bs.Count);
            byte b = bs.Pop();
            Assert.AreEqual(b, (byte)'a');
            Assert.AreEqual(0, bs.Count);
        }
        [Test] public void Test_Empty()
        {
            ByteStack bs = new ByteStack();
            byte[] buf = bs;
            Assert.AreEqual(0, buf.Length);
        }
        [Test] public void Test_Init()
        {
            ByteStack bs = new ByteStack(ENC.GetBytes("foo"));
            Assert.AreEqual("foo", bs.ToString());
            bs.Push((byte) 't');
            Assert.AreEqual("foot", bs.ToString());
            bs = new ByteStack(ENC.GetBytes("f"));
            bs.Push((byte) 't');
            Assert.AreEqual("ft", bs.ToString());
        }
        [Test] public void Test_Growth()
        {
            ByteStack bs = new ByteStack(4);

            bs.Push((byte) 'b');
            Assert.AreEqual("b", bs.ToString());
            bs.Push((byte) 'c');
            Assert.AreEqual("bc", bs.ToString());
            bs.Push((byte) 'd');
            Assert.AreEqual("bcd", bs.ToString());
            bs.Push((byte) 'e');
            Assert.AreEqual("bcde", bs.ToString());
            bs.Push((byte) 'b');
            Assert.AreEqual("bcdeb", bs.ToString());
            bs.Push((byte) 'c');
            Assert.AreEqual("bcdebc", bs.ToString());
            bs.Push((byte) 'd');
            Assert.AreEqual("bcdebcd", bs.ToString());
            bs.Push((byte) 'e');
            Assert.AreEqual("bcdebcde", bs.ToString());
            bs.Push((byte) 'b');
            bs.Push((byte) 'c');
            bs.Push((byte) 'd');
            bs.Push((byte) 'e');
            bs.Push((byte) 'b');
            bs.Push((byte) 'c');
            bs.Push((byte) 'd');
            bs.Push((byte) 'e');
            Assert.AreEqual("bcdebcdebcdebcde", bs.ToString());
        }
    }
}
