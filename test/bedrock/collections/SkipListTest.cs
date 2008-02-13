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
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using NUnit.Framework;
using bedrock.collections;
using bedrock.util;
using System.Collections;

namespace test.bedrock.collections
{
    /// <summary>
    /// Summary description for SkipListTest.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class SkipListTest
    {
        private System.Text.Encoding ENC = System.Text.Encoding.Default;

        [ExpectedException(typeof(ArgumentNullException))]
        [Test] public void Test_Null_Key()
        {
            SkipList sl = new SkipList();
            sl.Add(null, "one");
        }

        [ExpectedException(typeof(ArgumentException))]
        [Test] public void Test_Key_Twice()
        {
            SkipList sl = new SkipList();
            sl.Add("one", "one");
            sl.Add("one", "one");
        }

        [Test] public void Test_Add()
        {
            SkipList sl = new SkipList();
            Assert.AreEqual(0, sl.Count);
            sl.Add("1", "bar");
            Assert.AreEqual(1, sl.Count);
            sl.Add("2", "baz");
            Assert.AreEqual(2, sl.Count);
        }
        [Test] public void Test_Get()
        {
            SkipList sl = new SkipList();
            sl.Add("1", "bar");
            Assert.AreEqual("bar", sl["1"]);
        }
        [Test] public void Test_Lots_InOrder()
        {
            SkipList sl = new SkipList();
            for (int i=0; i<4096; i++)
            {
                sl[i] = i.ToString();
            }
            Assert.AreEqual(4096, sl.Count);
            for (int i=0; i<4096; i++)
            {
                Assert.AreEqual(i.ToString(), sl[i]);
            }
        }
        [Test] public void Test_Lots_Random()
        {
            SkipList sl = new SkipList();
            Random r = new Random();
            int[] nums = new int[4096];

            for (int i=0; i<4096; i++)
            {
                nums[i] = r.Next(10000);
                while (sl.Contains(nums[i]))
                {
                    nums[i] = r.Next(10000);
                }
                sl[nums[i]] = i.ToString();
            }
            Assert.AreEqual(4096, sl.Count);
            for (int i=0; i<4096; i++)
            {
                Assert.AreEqual(i.ToString(), sl[nums[i]]);
            }
        }
        [Test] public void Test_Iteration()
        {
            SkipList sl = new SkipList();
            for (int i=0; i<4096; i++)
            {
                sl[i] = i.ToString();
            }
            Assert.AreEqual(4096, sl.Count);
            int count = 0;
            foreach (DictionaryEntry de in sl)
            {
                Assert.AreEqual(count, de.Key);
                Assert.AreEqual(count.ToString(), de.Value);
                count++;
            }
            Assert.AreEqual(4096, count);
        }

        [Test] public void Test_DictIteration()
        {
            SkipList sl = new SkipList();
            for (int i=0; i<4096; i++)
            {
                sl[i] = i.ToString();
            }
            Assert.AreEqual(4096, sl.Count);
            int count = 0;
            IDictionaryEnumerator e = sl.GetEnumerator();
            while (e.MoveNext())
            {
                Assert.AreEqual(count, e.Key);
                Assert.AreEqual(count.ToString(), e.Value);
                count++;
            }
            Assert.AreEqual(4096, count);
        }

        [Test] public void Test_Remove()
        {
            SkipList sl = new SkipList();
            sl[0] = 0;
            Assert.AreEqual(1, sl.Count);
            sl.Remove(0);
            Assert.AreEqual(0, sl.Count);
        }

        [Test] public void Test_Clear()
        {
            SkipList sl = new SkipList();
            for (int i=0; i<4096; i++)
            {
                sl[i] = i.ToString();
            }
            Assert.AreEqual(4096, sl.Count);
            sl.Clear();
            Assert.AreEqual(0, sl.Count);
        }
    }
}
