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
    ///    Summary description for TemplateTest.
    /// </summary>
    [RCS(@"$Header$")]
    public class TrieTest : TestCase
    {
        public TrieTest(string name) : base(name) {}
        public static ITest Suite
        {
            get { return new TestSuite(typeof(TrieTest)); }
        }
        private Trie data()
        {
            Trie t = new Trie();
            t.Add("one", "one");
            t[2]    = "2";
            t["~"]  = "~";
            t["a~"] = "a~";
            t["~a"] = "~a";
            t[' ']  = " ";
            t["  "] = "  ";
            AssertEquals(t.Count, 7);
            Assert(t.Contains("~"));
            Assert(!t.Contains("~~"));
            return t;
        }
        public void Test_Type()
        {
            Trie t = new Trie();
            AssertEquals("bedrock.collections.Trie", t.GetType().FullName);
        }
        public void Test_Main() 
        {
            Trie t = new Trie();
            t["one"] = "two";
            AssertEquals("two", t["one"]);
            AssertEquals(t.Count, 1);
            t.Remove("one");
            AssertEquals(t.Count, 0);
            AssertEquals(null, t["one"]);
        }
        public void Test_Enum()
        {
            Trie t = data();
            int i = 0;
            string s;
            foreach (DictionaryEntry o in t)
            {
                s = "s|" + System.Text.Encoding.UTF8.GetString((byte[])o.Key) + 
                    "|=|" + o.Value + "|";
                Console.WriteLine(s);
                i++;
            }
            AssertEquals(7, i);
        }
        public void Test_Clear()
        {
            Trie t = new Trie();
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
            Trie t = data();
            ICollection ic = t.Values;
            AssertEquals(ic.Count, 7);
        }
        public void Test_Keys()
        {
            Trie t = data();
            ICollection ic = t.Keys;
            AssertEquals(7, ic.Count);
        }
        public void Test_Dictionary()
        {
            Trie t = data();
            // TODO: test dictionary enumerator.
        }
    }
}
