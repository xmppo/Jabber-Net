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
using bedrock.util;
using NUnit.Framework;
using je = jabber.JIDFormatException;
using jabber;

namespace test.jabber
{
    /// <summary>
    /// Summary description for JIDTest.
    /// </summary>
    [RCS(@"$Header$")]
    public class JIDTest : TestCase
    {
        public JIDTest(string name) : base(name) {}
        public static ITest Suite
        {
            get { return new TestSuite(typeof(JIDTest)); }
        }
        public void Test_Create()
        {
            JID j = new JID("foo", "jabber.com", "there");
            AssertEquals("foo@jabber.com/there", j.ToString());
            j = new JID(null, "jabber.com", null);
            AssertEquals("jabber.com", j.ToString());
            j = new JID("foo", "jabber.com", null);
            AssertEquals("foo@jabber.com", j.ToString());
            j = new JID(null, "jabber.com", "there");
            AssertEquals("jabber.com/there", j.ToString());
        }
        public void Test_Parse_1()
        {
            JID j = new JID("foo");
            AssertEquals(null, j.User);
            AssertEquals("foo", j.Server);
            AssertEquals(null, j.Resource);
        }
        public void Test_Parse_2()
        {
            JID j = new JID("foo/bar");
            AssertEquals(null, j.User);
            AssertEquals("foo", j.Server);
            AssertEquals("bar", j.Resource);
        }
        public void Test_Parse_3()
        {
            JID j = new JID("boo@foo");
            AssertEquals("boo", j.User);
            AssertEquals("foo", j.Server);
            AssertEquals(null, j.Resource);
        }
        public void Test_Parse_4()
        {
            JID j = new JID("boo@foo/bar");
            AssertEquals("boo", j.User);
            AssertEquals("foo", j.Server);
            AssertEquals("bar", j.Resource);
        }
        public void Test_Parse_5()
        {
            JID j = new JID("foo/bar@baz");
            AssertEquals(null, j.User);
            AssertEquals("foo", j.Server);
            AssertEquals("bar@baz", j.Resource);
        }
        public void Test_Parse_6()
        {
            JID j = new JID("boo@foo/bar@baz");
            AssertEquals("boo", j.User);
            AssertEquals("foo", j.Server);
            AssertEquals("bar@baz", j.Resource);
        }
        public void Test_Parse_7()
        {
            JID j = new JID("boo@foo/bar/baz");
            AssertEquals("boo", j.User);
            AssertEquals("foo", j.Server);
            AssertEquals("bar/baz", j.Resource);
        }
        public void Test_Parse_8()
        {
            JID j = new JID("boo/foo@bar@baz");
            AssertEquals(null, j.User);
            AssertEquals("boo", j.Server);
            AssertEquals("foo@bar@baz", j.Resource);
        }
        public void Test_Parse_9()
        {
            JID j = new JID("boo/foo@bar");
            AssertEquals(null, j.User);
            AssertEquals("boo", j.Server);
            AssertEquals("foo@bar", j.Resource);
        }
        public void Test_Parse_10()
        {
            JID j = new JID("boo/foo/bar");
            AssertEquals(null, j.User);
            AssertEquals("boo", j.Server);
            AssertEquals("foo/bar", j.Resource);
        }
        public void Test_Parse_11()
        {
            JID j = new JID("boo//foo");
            AssertEquals(null, j.User);
            AssertEquals("boo", j.Server);
            AssertEquals("/foo", j.Resource);
        }
        public void Test_Parse_12()
        {
            JID j = new JID("boo/");
            AssertEquals(null, j.User);
            AssertEquals("boo", j.Server);
            AssertEquals("", j.Resource);
        }
        public void Test_Parse_13()
        {
            JID j = new JID("boo@foo/");
            AssertEquals("boo", j.User);
            AssertEquals("foo", j.Server);
            AssertEquals("", j.Resource);
        }

        public void Test_NoHost()
        {
            try
            {
                JID j = new JID("foo@");
                string u = j.User;
                Assert(false);
            }
            catch (JIDFormatException)
            {
                Assert(true);
            }
            catch (Exception)
            {
                Assert(false);
            }
        }
        public void Test_AtAt()
        {
            try
            {
                JID j = new JID("boo@@foo");
                string u = j.User;
                Assert(false);
            }
            catch (JIDFormatException)
            {
                Assert(true);
            }
            catch (Exception)
            {
                Assert(false);
            }
        }
        public void Test_TwoAt()
        {
            try
            {
                JID j = new JID("boo@foo@bar");
                string u = j.User;
                Assert(false);
            }
            catch (JIDFormatException)
            {
                Assert(true);
            }
            catch (Exception)
            {
                Assert(false);
            }
        }
        public void Test_Compare_Equal()
        {
            JID j = new JID("foo@bar/baz");
            AssertEquals(0, j.CompareTo(j));
            AssertEquals(0, j.CompareTo(new JID("foo@bar/baz")));
            j = new JID("foo@bar/");
            AssertEquals(0, j.CompareTo(j));
            AssertEquals(0, j.CompareTo(new JID("foo@bar/")));
            j = new JID("foo@bar");
            AssertEquals(0, j.CompareTo(j));
            AssertEquals(0, j.CompareTo(new JID("foo@bar")));
            j = new JID("bar");
            AssertEquals(0, j.CompareTo(j));
            AssertEquals(0, j.CompareTo(new JID("bar")));
            j = new JID("foo/bar");
            AssertEquals(0, j.CompareTo(j));
            AssertEquals(0, j.CompareTo(new JID("foo/bar")));
            AssertEquals(true, j >= new JID("foo/bar"));
            AssertEquals(true, j <= new JID("foo/bar"));
        }
        public void Test_Compare_Less()
        {
            JID j = new JID("foo@bar/baz");
            AssertEquals(-1, j.CompareTo(new JID("foo@bas/baz")));
            AssertEquals(-1, j.CompareTo(new JID("fop@bar/baz")));
            AssertEquals(-1, j.CompareTo(new JID("foo@bar/bazz")));
            j = new JID("foo@bar");
            AssertEquals(-1, j.CompareTo(new JID("foo@bas/baz")));
            AssertEquals(-1, j.CompareTo(new JID("foo@bas")));
            AssertEquals(-1, j.CompareTo(new JID("fop@bar/baz")));
            AssertEquals(-1, j.CompareTo(new JID("fop@bar")));
            AssertEquals(-1, j.CompareTo(new JID("foo@bar/baz")));
            AssertEquals(-1, j.CompareTo(new JID("foo@bar/")));
            j = new JID("bar");
            AssertEquals(-1, j.CompareTo(new JID("foo@bar/baz")));
            AssertEquals(-1, j.CompareTo(new JID("foo@bar/")));
            AssertEquals(-1, j.CompareTo(new JID("foo@bar")));
            AssertEquals(-1, j.CompareTo(new JID("bas")));
            AssertEquals(-1, j.CompareTo(new JID("bas/baz")));
            AssertEquals(-1, j.CompareTo(new JID("bar/baz")));
            AssertEquals(true, j < new JID("foo@bar/baz"));
            AssertEquals(true, j <= new JID("foo@bar/baz"));
        }
        public void Test_Compare_Greater()
        {
            JID j = new JID("foo@bar/baz");
            AssertEquals(1, j.CompareTo(new JID("foo@bap/baz")));
            AssertEquals(1, j.CompareTo(new JID("fon@bar/baz")));
            AssertEquals(1, j.CompareTo(new JID("foo@bar/bay")));
            AssertEquals(1, j.CompareTo(new JID("foo@bar/")));
            AssertEquals(1, j.CompareTo(new JID("foo@bar")));
            AssertEquals(1, j.CompareTo(new JID("bar")));
            AssertEquals(1, j.CompareTo(new JID("bar/baz")));
            j = new JID("foo@bar/");
            AssertEquals(1, j.CompareTo(new JID("foo@bap/baz")));
            AssertEquals(1, j.CompareTo(new JID("fon@bar/baz")));
            AssertEquals(1, j.CompareTo(new JID("foo@bar")));
            AssertEquals(1, j.CompareTo(new JID("bar")));
            AssertEquals(1, j.CompareTo(new JID("bar/baz")));
            j = new JID("foo@bar");
            AssertEquals(1, j.CompareTo(new JID("foo@bap/baz")));
            AssertEquals(1, j.CompareTo(new JID("fon@bar/baz")));
            AssertEquals(1, j.CompareTo(new JID("bar")));
            AssertEquals(1, j.CompareTo(new JID("bar/baz")));
            AssertEquals(true, j > new JID("foo@bap/baz"));
            AssertEquals(true, j >= new JID("foo@bap/baz"));
            // /me runs out of interest.
        }
        public void Test_BadCompare()
        {
            try
            {
                JID j = new JID("foo@boo/bar");
                j.CompareTo("foo@boo/bar");
                Assert(false);
            }
            catch (ArgumentException)
            {
                Assert(true);
            }
            catch (Exception)
            {
                Assert(false);
            }
        }
        public void Test_Insensitive()
        {
            JID j = new JID("foo@boo/bar");
            AssertEquals(0, j.CompareTo(new JID("foo@BOO/bar")));
            AssertEquals(0, j.CompareTo(new JID("FOO@boo/bar")));
            AssertEquals(0, j.CompareTo(new JID("FOO@BOO/bar")));
            AssertEquals(-1, j.CompareTo(new JID("FOO@BOO/BAR")));
        }

    }
}
