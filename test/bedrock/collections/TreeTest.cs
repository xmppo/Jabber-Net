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
    public class TreeTest: TestCase
    {
        public TreeTest(string name) : base(name) {}
        public static ITest Suite
        {
            get { return new TestSuite(typeof(TreeTest)); }
        }
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
            AssertEquals(t.Count, 7);
            Assert(t.Contains("~"));
            Assert(!t.Contains("~~"));
            return t;
        }
        public void Test_Type()
        {
            Tree t = new Tree();
            AssertEquals("bedrock.collections.Tree", t.GetType().FullName);
        }
        public void Test_Main() 
        {
            Tree t = new Tree();
            t["one"] = "two";
            AssertEquals("two", t["one"]);
            AssertEquals(t.Count, 1);
            t.Remove("one");
            AssertEquals(t.Count, 0);
            AssertEquals(null, t["one"]);
        }
        public void Test_Enum()
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
            AssertEquals(7, i);
        }
        public void Test_Clear()
        {
            Tree t = new Tree();
            AssertEquals(t.Count, 0);
            t.Clear();
            AssertEquals(t.Count, 0);
            t.Add("one", "one");
            AssertEquals(t.Count, 1);
            t.Clear();
            AssertEquals(t.Count, 0);
        }
        public void Test_Values()
        {
            Tree t = data();
            ICollection ic = t.Values;
            AssertEquals(ic.Count, 7);
            object[] o = (object[]) ic;
            AssertEquals("5 ", o[0]);
        }
        public void Test_Keys()
        {
            Tree t = data();
            ICollection ic = t.Keys;
            AssertEquals(7, ic.Count);
            object[] o = (object[]) ic;
            AssertEquals(" ", o[0]);
        }   

        public void Test_Lots_InOrder()
        {
            Tree sl = new Tree();
            for (int i=0; i<4096; i++)
            {
                sl[i] = i.ToString();
            }
            AssertEquals(4096, sl.Count);
            for (int i=0; i<4096; i++)
            {
                AssertEquals(i.ToString(), sl[i]);
            }
        }
        public void Test_Lots_Random()
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
            AssertEquals(4096, sl.Count);
            for (int i=0; i<4096; i++)
            {
                AssertEquals(i.ToString(), sl[nums[i]]);
            }
        }
        public void Test_Iteration()
        {
            Tree sl = new Tree();
            for (int i=0; i<4096; i++)
            {
                sl[i] = i.ToString();
            }
            AssertEquals(4096, sl.Count);
            int count = 0;
            foreach (DictionaryEntry de in sl)
            {
                AssertEquals(count, de.Key);
                AssertEquals(count.ToString(), de.Value);
                count++;
            }
            AssertEquals(4096, count);
        }

        public void Test_DictIteration()
        {
            Tree sl = new Tree();
            for (int i=0; i<4096; i++)
            {
                sl[i] = i.ToString();
            }
            AssertEquals(4096, sl.Count);
            int count = 0;
            IDictionaryEnumerator e = sl.GetEnumerator();
            while (e.MoveNext())
            {
                AssertEquals(count, e.Key);
                AssertEquals(count.ToString(), e.Value);
                count++;
            }
            AssertEquals(4096, count);
        }
    }
}
