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
using System;

using NUnit.Framework;
using bedrock.collections;
using bedrock.util;

namespace test.bedrock.collections
{
    [TestFixture]
    [SVN(@"$Id$")]
    public class StringSetTest
    {
        [Test]
        public void Create()
        {
            StringSet ss = new StringSet();
            Assert.AreEqual(0, ss.Count);
            Assert.AreEqual("", ss.ToString());
        }

        [Test]
        public void Add()
        {
            StringSet ss = new StringSet();
            ss.Add("foo");
            ss.Add("foo");
            ss.Add("bar");
            Assert.IsTrue(ss["foo"]);
            Assert.AreEqual(2, ss.Count);
            Assert.AreEqual("foo\r\nbar\r\n", ss.ToString());
            ss.Remove("bar");
            Assert.AreEqual(1, ss.Count);
            Assert.IsFalse(ss["fool"]);

            ss = new StringSet(new string[] { "foo", "bar"});
            ss.Add(new StringSet("baz"));
            Assert.AreEqual(3, ss.Count);
        }

        [Test]
        public void Merge()
        {
            StringSet ss = new StringSet();
            ss.Add("foo");
            StringSet so = new StringSet();
            so.Add("bar");
            so.Add("baz");
            ss.Add(so);
            Assert.AreEqual(3, ss.Count);

            so = new StringSet();
            so.Add("boo");
            so.Add("baz");
            ss.Add(so);
            Assert.AreEqual(4, ss.Count);

            ss.Remove(so);
            Assert.AreEqual(2, ss.Count);
            Assert.IsTrue(ss["foo"]);
            Assert.IsTrue(ss["bar"]);
            Assert.IsFalse(ss["boo"]);
            Assert.IsFalse(ss["baz"]);
            Assert.IsFalse(ss["bloo"]);
        }

        [Test]
        public void Enumerate()
        {
            StringSet ss = new StringSet();

            int i = 0;
            foreach (string s in ss)
            {
                i++;
                Console.WriteLine(s);
            }
            Assert.AreEqual(0, i);

            ss.Add("bloo");
            ss.Add("foo");
            ss.Add("bar");
            foreach (string s in ss)
            {
                i++;
                Console.WriteLine(s);
            }
            Assert.AreEqual(3, i);
            string[] arr = ss.GetStrings();
            Array.Sort(arr);
            Assert.AreEqual("bar", arr[0]);
            Assert.AreEqual("bloo", arr[1]);
            Assert.AreEqual("foo", arr[2]);
            Assert.AreEqual(3, arr.Length);
        }

        [Test]
        public void Operators()
        {
            StringSet ss = new StringSet();
            ss = ss + "bloo" + "bar";
            Assert.AreEqual(2, ss.Count);
            StringSet so = ss;
            Assert.AreEqual(ss, so);
            Assert.AreEqual(ss.GetHashCode(), so.GetHashCode());
            so = new StringSet(so);
            Assert.AreEqual(ss, so);
            Assert.AreEqual(ss.GetHashCode(), so.GetHashCode());
            so = ss - "bar";
            Assert.AreEqual(1, so.Count);
            Assert.AreNotEqual(ss, so);
            Assert.IsTrue(ss != so);
            Assert.AreNotEqual(ss.GetHashCode(), so.GetHashCode());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null()
        {
            StringSet ss = new StringSet();
            ss.Add((string)null);
        }
        
        [Test]
        public void Empty()
        {
            StringSet ss = new StringSet(new string[] { "" });
            Assert.AreEqual(1, ss.Count);
            ss = new StringSet((string[])null);
            Assert.AreEqual(0, ss.Count);
        }
    }
}
