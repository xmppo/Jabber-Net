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

using System.Collections;
using NUnit.Framework;
using bedrock.collections;
using bedrock.util;

namespace test.bedrock.collections
{
    /// <summary>
    /// Summary description for TreeTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class TreeTest
    {
        private Tree data()
        {
            Tree t = new Tree();
            t.Add("one", "one");
            t["2"]  = "12";
            t["~"]  = "2~";
            t["a~"] = "3a~";
            t["~a"] = "4~a";
            t[" "]  = "5 ";
            t["  "] = "6  ";
            Assert.AreEqual(t.Count, 7);
            Assert.IsTrue(t.Contains("~"));
            Assert.IsTrue(!t.Contains("~~"));
            return t;
        }
        [Test] public void Test_Type()
        {
            Tree t = new Tree();
            Assert.AreEqual("bedrock.collections.Tree", t.GetType().FullName);
        }
        [Test] public void Test_Main() 
        {
            Tree t = new Tree();
            t["one"] = "two";
            Assert.AreEqual("two", t["one"]);
            Assert.AreEqual(t.Count, 1);
            t.Remove("one");
            Assert.AreEqual(t.Count, 0);
            Assert.AreEqual(null, t["one"]);
        }
        [Test] public void Test_Enum()
        {
            Tree t = data();
            int i = 0;
            string s;
            foreach (DictionaryEntry o in t)
            {
                s = "s|" + o.Key + "|=|" + o.Value + "|";
                Console.WriteLine(s);
                i++;
            }
            Assert.AreEqual(7, i);
        }
        [Test] public void Test_Clear()
        {
            Tree t = new Tree();
            Assert.AreEqual(t.Count, 0);
            t.Clear();
            Assert.AreEqual(t.Count, 0);
            t.Add("one", "one");
            Assert.AreEqual(t.Count, 1);
            t.Clear();
            Assert.AreEqual(t.Count, 0);
        }
        [Test] public void Test_Values()
        {
            Tree t = data();
            ICollection ic = t.Values;
            Assert.AreEqual(ic.Count, 7);
            object[] o = (object[]) ic;
            Assert.AreEqual("5 ", o[0]);
        }
        [Test] public void Test_Keys()
        {
            Tree t = data();
            ICollection ic = t.Keys;
            Assert.AreEqual(7, ic.Count);
            object[] o = (object[]) ic;
            Assert.AreEqual(" ", o[0]);
        }   

        [Test] public void Test_Lots_InOrder()
        {
            Tree sl = new Tree();
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
            Tree sl = new Tree();
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
            Tree sl = new Tree();
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
            Tree sl = new Tree();
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

        [ExpectedException(typeof(ArgumentNullException))]
        [Test] public void Test_Null()
        {
            Tree sl = new Tree();
            sl[null] = "n";
        }
    }
}