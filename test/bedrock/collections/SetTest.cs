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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
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