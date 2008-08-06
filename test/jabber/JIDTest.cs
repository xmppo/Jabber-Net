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

using bedrock.util;
using NUnit.Framework;
using je = jabber.JIDFormatException;
using jabber;

namespace test.jabber
{
    /// <summary>
    /// Summary description for JIDTest.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class JIDTest
    {
        [Test] public void Test_Create()
        {
            JID j = new JID("foo", "jabber.com", "there");
            Assert.AreEqual("foo@jabber.com/there", j.ToString());
            j = new JID(null, "jabber.com", null);
            Assert.AreEqual("jabber.com", j.ToString());
            j = new JID("foo", "jabber.com", null);
            Assert.AreEqual("foo@jabber.com", j.ToString());
            j = new JID(null, "jabber.com", "there");
            Assert.AreEqual("jabber.com/there", j.ToString());
        }
        [Test] public void Test_Parse_1()
        {
            JID j = new JID("foo");
            Assert.AreEqual(null, j.User);
            Assert.AreEqual("foo", j.Server);
            Assert.AreEqual(null, j.Resource);
        }
        [Test] public void Test_Parse_2()
        {
            JID j = new JID("foo/bar");
            Assert.AreEqual(null, j.User);
            Assert.AreEqual("foo", j.Server);
            Assert.AreEqual("bar", j.Resource);
        }
        [Test] public void Test_Parse_3()
        {
            JID j = new JID("boo@foo");
            Assert.AreEqual("boo", j.User);
            Assert.AreEqual("foo", j.Server);
            Assert.AreEqual(null, j.Resource);
        }
        [Test] public void Test_Parse_4()
        {
            JID j = new JID("boo@foo/bar");
            Assert.AreEqual("boo", j.User);
            Assert.AreEqual("foo", j.Server);
            Assert.AreEqual("bar", j.Resource);
        }
        [Test] public void Test_Parse_5()
        {
            JID j = new JID("foo/bar@baz");
            Assert.AreEqual(null, j.User);
            Assert.AreEqual("foo", j.Server);
            Assert.AreEqual("bar@baz", j.Resource);
        }
        [Test] public void Test_Parse_6()
        {
            JID j = new JID("boo@foo/bar@baz");
            Assert.AreEqual("boo", j.User);
            Assert.AreEqual("foo", j.Server);
            Assert.AreEqual("bar@baz", j.Resource);
        }
        [Test] public void Test_Parse_7()
        {
            JID j = new JID("boo@foo/bar/baz");
            Assert.AreEqual("boo", j.User);
            Assert.AreEqual("foo", j.Server);
            Assert.AreEqual("bar/baz", j.Resource);
        }
        [Test] public void Test_Parse_8()
        {
            JID j = new JID("boo/foo@bar@baz");
            Assert.AreEqual(null, j.User);
            Assert.AreEqual("boo", j.Server);
            Assert.AreEqual("foo@bar@baz", j.Resource);
        }
        [Test] public void Test_Parse_9()
        {
            JID j = new JID("boo/foo@bar");
            Assert.AreEqual(null, j.User);
            Assert.AreEqual("boo", j.Server);
            Assert.AreEqual("foo@bar", j.Resource);
        }
        [Test] public void Test_Parse_10()
        {
            JID j = new JID("boo/foo/bar");
            Assert.AreEqual(null, j.User);
            Assert.AreEqual("boo", j.Server);
            Assert.AreEqual("foo/bar", j.Resource);
        }
        [Test] public void Test_Parse_11()
        {
            JID j = new JID("boo//foo");
            Assert.AreEqual(null, j.User);
            Assert.AreEqual("boo", j.Server);
            Assert.AreEqual("/foo", j.Resource);
        }
        [Test] public void Test_EmptyResource()
        {
            try
            {
                JID j = new JID("boo/");
                string u = j.User;
                Assert.IsTrue(false);
            }
            catch (JIDFormatException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }
        [Test] public void Test_EmptyResourceUser()
        {
            try
            {
                JID j = new JID("boo@foo/");
                string u = j.User;
                Assert.IsTrue(false);
            }
            catch (JIDFormatException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }

        [Test] public void Test_NoHost()
        {
            try
            {
                JID j = new JID("foo@");
                string u = j.User;
                Assert.IsTrue(false);
            }
            catch (JIDFormatException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }
        [Test] public void Test_AtAt()
        {
            try
            {
                JID j = new JID("boo@@foo");
                string u = j.User;
                Assert.IsTrue(false);
            }
            catch (JIDFormatException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }
        [Test] public void Test_TwoAt()
        {
            try
            {
                JID j = new JID("boo@foo@bar");
                string u = j.User;
                Assert.IsTrue(false);
            }
            catch (JIDFormatException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }
        [Test] public void Test_Compare_Equal()
        {
            JID j = new JID("foo@bar/baz");
            Assert.AreEqual(0, j.CompareTo(j));
            Assert.AreEqual(0, j.CompareTo(new JID("foo@bar/baz")));
            j = new JID("foo@bar");
            Assert.AreEqual(0, j.CompareTo(j));
            Assert.AreEqual(0, j.CompareTo(new JID("foo@bar")));
            Assert.IsTrue(j == new JID("foo@bar"));
            Assert.IsTrue(j == new JID("foo@BAR"));
            Assert.IsTrue(j == new JID("FOO@BAR"));
            Assert.IsTrue(j == new JID("FOO@bar"));
            Assert.AreEqual(new JID("FOO@bar").GetHashCode(), j.GetHashCode());
            j = new JID("bar");
            Assert.AreEqual(0, j.CompareTo(j));
            Assert.AreEqual(0, j.CompareTo(new JID("bar")));
            j = new JID("foo/bar");
            Assert.AreEqual(0, j.CompareTo(j));
            Assert.AreEqual(0, j.CompareTo(new JID("foo/bar")));
            Assert.AreEqual(true, j >= new JID("foo/bar"));
            Assert.AreEqual(true, j <= new JID("foo/bar"));
        }
        [Test] public void Test_Compare_Less()
        {
            JID j = new JID("foo@bar/baz");
            Assert.AreEqual(-1, j.CompareTo(new JID("foo@bas/baz")));
            Assert.AreEqual(-1, j.CompareTo(new JID("fop@bar/baz")));
            Assert.AreEqual(-1, j.CompareTo(new JID("foo@bar/bazz")));
            j = new JID("foo@bar");
            Assert.AreEqual(-1, j.CompareTo(new JID("foo@bas/baz")));
            Assert.AreEqual(-1, j.CompareTo(new JID("foo@bas")));
            Assert.AreEqual(-1, j.CompareTo(new JID("fop@bar/baz")));
            Assert.AreEqual(-1, j.CompareTo(new JID("fop@bar")));
            Assert.AreEqual(-1, j.CompareTo(new JID("foo@bar/baz")));
            j = new JID("bar");
            Assert.AreEqual(-1, j.CompareTo(new JID("foo@bar/baz")));
            Assert.AreEqual(-1, j.CompareTo(new JID("foo@bar")));
            Assert.AreEqual(-1, j.CompareTo(new JID("bas")));
            Assert.AreEqual(-1, j.CompareTo(new JID("bas/baz")));
            Assert.AreEqual(-1, j.CompareTo(new JID("bar/baz")));
            Assert.AreEqual(true, j < new JID("foo@bar/baz"));
            Assert.AreEqual(true, j <= new JID("foo@bar/baz"));
        }
        [Test] public void Test_Compare_Greater()
        {
            JID j = new JID("foo@bar/baz");
            Assert.AreEqual(1, j.CompareTo(new JID("foo@bap/baz")));
            Assert.AreEqual(1, j.CompareTo(new JID("fon@bar/baz")));
            Assert.AreEqual(1, j.CompareTo(new JID("foo@bar/bay")));
            Assert.AreEqual(1, j.CompareTo(new JID("foo@bar")));
            Assert.AreEqual(1, j.CompareTo(new JID("bar")));
            Assert.AreEqual(1, j.CompareTo(new JID("bar/baz")));
            j = new JID("foo@bar");
            Assert.AreEqual(1, j.CompareTo(new JID("foo@bap/baz")));
            Assert.AreEqual(1, j.CompareTo(new JID("fon@bar/baz")));
            Assert.AreEqual(1, j.CompareTo(new JID("bar")));
            Assert.AreEqual(1, j.CompareTo(new JID("bar/baz")));
            Assert.AreEqual(true, j > new JID("foo@bap/baz"));
            Assert.AreEqual(true, j >= new JID("foo@bap/baz"));
            // /me runs out of interest.
        }
        [Test] public void Test_BadCompare()
        {
            try
            {
                JID j = new JID("foo@boo/bar");
                j.CompareTo("foo@boo/bar");
                Assert.IsTrue(false);
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }
        [Test] public void Test_Insensitive()
        {
            JID j = new JID("foo@boo/bar");
            Assert.AreEqual(0, j.CompareTo(new JID("foo@BOO/bar")));
            Assert.AreEqual(0, j.CompareTo(new JID("FOO@boo/bar")));
            Assert.AreEqual(0, j.CompareTo(new JID("FOO@BOO/bar")));
            Assert.AreEqual(-1, j.CompareTo(new JID("FOO@BOO/BAR")));
        }

        [Test] public void Test_Config()
        {
            JID j = new JID("config@-internal");
            Assert.AreEqual("config", j.User);
            Assert.AreEqual("-internal", j.Server);
            Assert.AreEqual(null, j.Resource);
        }

        [Test] public void Test_Numeric()
        {
            JID j = new JID("support", "conference.192.168.32.109", "bob");
            Assert.AreEqual("conference.192.168.32.109", j.Server);
        }
    }
}
