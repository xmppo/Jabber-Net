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
            Assert.AreEqual(t.Count, 7);
            Assert.IsTrue(t.Contains("~"));
            Assert.IsTrue(!t.Contains("~~"));
            return t;
        }
        [Test] public void Test_Type()
        {
            Trie t = new Trie();
            Assert.AreEqual("bedrock.collections.Trie", t.GetType().FullName);
        }
        [Test] public void Test_Main() 
        {
            Trie t = new Trie();
            t["one"] = "two";
            Assert.AreEqual("two", t["one"]);
            Assert.AreEqual(t.Count, 1);
            t.Remove("one");
            Assert.AreEqual(t.Count, 0);
            Assert.AreEqual(null, t["one"]);
        }
        [Test] public void Test_Enum()
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
            Assert.AreEqual(7, i);
        }
        [Test] public void Test_Clear()
        {
            Trie t = new Trie();
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
            Trie t = data();
            ICollection ic = t.Values;
            Assert.AreEqual(ic.Count, 7);
        }
        [Test] public void Test_Keys()
        {
            Trie t = data();
            ICollection ic = t.Keys;
            Assert.AreEqual(7, ic.Count);
        }
        [Test] public void Test_Dictionary()
        {
            Trie t = data();
            // TODO: test dictionary enumerator.
        }
    }
}