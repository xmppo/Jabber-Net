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
    ///    Summary description for TemplateTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class TrieTest
    {
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
            Assertion.AssertEquals(t.Count, 7);
            Assertion.Assert(t.Contains("~"));
            Assertion.Assert(!t.Contains("~~"));
            return t;
        }
        public void Test_Type()
        {
            Trie t = new Trie();
            Assertion.AssertEquals("bedrock.collections.Trie", t.GetType().FullName);
        }
        public void Test_Main() 
        {
            Trie t = new Trie();
            t["one"] = "two";
            Assertion.AssertEquals("two", t["one"]);
            Assertion.AssertEquals(t.Count, 1);
            t.Remove("one");
            Assertion.AssertEquals(t.Count, 0);
            Assertion.AssertEquals(null, t["one"]);
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
            Assertion.AssertEquals(7, i);
        }
        public void Test_Clear()
        {
            Trie t = new Trie();
            Assertion.AssertEquals(t.Count, 0);
            t.Clear();
            Assertion.AssertEquals(t.Count, 0);
            t.Add("one", "one");
            Assertion.AssertEquals(t.Count, 1);
            t.Clear();
            Assertion.AssertEquals(t.Count, 0);
        }
        public void Test_Values()
        {
            Trie t = data();
            ICollection ic = t.Values;
            Assertion.AssertEquals(ic.Count, 7);
        }
        public void Test_Keys()
        {
            Trie t = data();
            ICollection ic = t.Keys;
            Assertion.AssertEquals(7, ic.Count);
        }
        public void Test_Dictionary()
        {
            Trie t = data();
            // TODO: test dictionary enumerator.
        }
    }
}