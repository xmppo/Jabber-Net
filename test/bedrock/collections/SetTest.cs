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

using NUnit.Framework;
using bedrock.collections;
using bedrock.util;

namespace test.bedrock.collections
{
    /// <summary>
    /// Summary description for SetTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class SetTest
    {
        //[ExpectedException(typeof(ArgumentException))]
        public void Test_Hashtable_Double_Add()
        {
            Set s = new Set(SetImplementation.Hashtable);
            Assertion.AssertEquals(0, s.Count);
            s.Add("one");
            Assertion.AssertEquals(1, s.Count);
            s.Add("one");
        }
        //[ExpectedException(typeof(ArgumentException))]
        public void Test_SkipList_Double_Add()
        {
            Set s = new Set(SetImplementation.SkipList);
            Assertion.AssertEquals(0, s.Count);
            s.Add("one");
            Assertion.AssertEquals(1, s.Count);
            s.Add("one");
        }
        //[ExpectedException(typeof(ArgumentException))]
        public void Test_Tree_Double_Add()
        {
            Set s = new Set(SetImplementation.Tree);
            Assertion.AssertEquals(0, s.Count);
            s.Add("one");
            Assertion.AssertEquals(1, s.Count);
            s.Add("one");
        }

        private void all(SetImplementation i)
        {
            Set s = new Set(i);
            Assertion.AssertEquals(0, s.Count);

            s.Add("one");
            Assertion.AssertEquals(1, s.Count);
            Assertion.Assert(s.Contains("one"));
            Assertion.Assert(!s.Contains("two"));
            Assertion.Assert(!s.Contains("three"));

            s.Add("two");
            Assertion.AssertEquals(2, s.Count);
            Assertion.Assert(s.Contains("one"));
            Assertion.Assert(s.Contains("two"));
            Assertion.Assert(!s.Contains("three"));

            s.Remove("one");
            Assertion.AssertEquals(1, s.Count);
            Assertion.Assert(!s.Contains("one"));
            Assertion.Assert(s.Contains("two"));
            Assertion.Assert(!s.Contains("three"));

            s.Add("one");
            Assertion.AssertEquals(2, s.Count);
            Assertion.Assert(s.Contains("one"));
            Assertion.Assert(s.Contains("two"));
            Assertion.Assert(!s.Contains("three"));

            s.Add("one");
            Assertion.AssertEquals(2, s.Count);
            Assertion.Assert(s.Contains("one"));
            Assertion.Assert(s.Contains("two"));
            Assertion.Assert(!s.Contains("three"));

            int count = 0;
            foreach (string str in s)
            {
                count++;
                Assertion.AssertEquals(3, str.Length);
            }
            Assertion.AssertEquals(2, count);
        }

        public void Test_Hashtable()
        {
            all(SetImplementation.Hashtable);
        }
        public void Test_Skiplist()
        {
            all(SetImplementation.SkipList);
        }
        public void Test_Tree()
        {
            all(SetImplementation.Tree);
        }
    }
}