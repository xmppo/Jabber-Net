/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
using bedrock.util;
namespace test.bedrock.util
{
    /// <summary>
    ///    Summary description for GetOptBaseTest.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class GetOptTest
    {
        [Test] public void Test_Construct()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/f", "/bar", "baz"});
            Assert.AreEqual(true, t.foo);
            Assert.AreEqual("baz", t.bar);
        }
        [Test] public void Test_Usage()
        {
            TestGetOpt t = new TestGetOpt();
            Console.WriteLine(t.Usage);
        }
        [Test] public void Test_Int()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "1", "/other", "2", "blah"});
            Assert.AreEqual(1, t.iSome);
            Assert.AreEqual(2, t.other);
            Assert.AreEqual("one", t.bar);
        }
        [Test] public void Test_Private()
        {
            // hrmph.  This should probably throw an error, if I don't have the right permissions.
            TestGetOpt t = new TestGetOpt(new string[] {"/private", "one"});
            Assert.AreEqual("one", t.baz);
        }
        [Test] public void Test_Args()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "1", "/other", "2", "blah", "bloo", "boo"});
            Assert.AreEqual(3, t.Args.Length);
            Assert.AreEqual("blah", t.Args[0]);
            Assert.AreEqual("bloo", t.Args[1]);
            Assert.AreEqual("boo", t.Args[2]);
        }
        [Test] public void Test_Colon()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some:1", "/other:2", "blah", "bloo", "boo"});
            Assert.AreEqual(1, t.iSome);
            Assert.AreEqual(2, t.other);
            Assert.AreEqual(3, t.Args.Length);
        }
        [Test] public void Test_Bool()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/a", "/b", "four", "one", "more"});
            Assert.AreEqual(true,   go.a);
            Assert.AreEqual(false,  go.b);
            Assert.AreEqual(false,  go.c);
            Assert.AreEqual(true,   go.d);
            Assert.AreEqual(3,      go.Args.Length);
            Assert.AreEqual("four", go.Args[0]);
            Assert.AreEqual("one",  go.Args[1]);
            Assert.AreEqual("more", go.Args[2]);
        }
        [Test] public void Test_DashBool()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-a", "-b", "four", "one", "more"});
            Assert.AreEqual(true,   go.a);
            Assert.AreEqual(false,  go.b);
            Assert.AreEqual(false,  go.c);
            Assert.AreEqual(true,   go.d);
            Assert.AreEqual(3,      go.Args.Length);
            Assert.AreEqual("four", go.Args[0]);
            Assert.AreEqual("one",  go.Args[1]);
            Assert.AreEqual("more", go.Args[2]);
        }

        [Test] public void Test_DashArgs()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-bar", "four", "-baz:one", "more"});
            Assert.AreEqual("four", go.bar);
            Assert.AreEqual("one",  go.baz);
            Assert.AreEqual(null,   go.e);
            Assert.AreEqual(1,      go.Args.Length);
            Assert.AreEqual("more", go.Args[0]);
        }

        [Test] public void Test_Env()
        {
            TestGetOpt go = new TestGetOpt(null);

            Assert.IsTrue(go.Args[0].StartsWith("test"));
        }
        [Test] public void Test_CaseInsensitive()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-BaZ:boo"});
            Assert.AreEqual("boo", go.baz);
        }
        [Test] public void Test_Equals()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-baz=boo"});
            Assert.AreEqual("boo", go.baz);
        }
        [Test] public void Test_DoubleColon()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/baz:boo:bar"});
            Assert.AreEqual("boo:bar", go.baz);
        }
        [Test] public void Test_DoubleSlash()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/baz:boo/bar"});
            Assert.AreEqual("boo/bar", go.baz);
        }
        [Test] public void Test_NunitProblem()
        {
            TestGetOpt go = new TestGetOpt(new string[] {@"/assembly:test.dll"});
            Assert.AreEqual(@"test.dll", go.assembly);
        }
        [Test] public void Test_Enum()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-fb:bar"});
            Assert.AreEqual(TestOptEnum.BAR, go.fb);
        }
        [Test] public void Test_Method()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-method"});
            Assert.AreEqual("after", go.m);
        }
        [Test] public void Test_MethodParam()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-methodparam", "2"});
            Assert.AreEqual(2, go.mp);
            go = new TestGetOpt(new string[] {"-methodparams", "2", "after"});
            Assert.AreEqual(2, go.m2);
            Assert.AreEqual("after", go.mp2);
        }
        [Test] public void Test_NonChild()
        {
            NonChild nc = new NonChild();
            GetOpt go = new GetOpt(nc);
            go.Process(new string[] {"-test", "foo"});
            Assert.AreEqual("foo", nc.test);
        }
        [Test] public void Test_Indexer()
        {
            NonChild nc = new NonChild();
            GetOpt go = new GetOpt(nc);
            go["test"] = "foo";
            Assert.AreEqual("foo", nc.test);
        }
        [ExpectedException(typeof(FormatException))]
        [Test] public void Test_BadInt()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "one"});
        }
        [ExpectedException(typeof(ArgumentException))]
        [Test] public void Test_Error()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/broncos"});
        }
        [ExpectedException(typeof(ArgumentException))]
        [Test] public void Test_DashError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-broncos"});
        }
        [ExpectedException(typeof(IndexOutOfRangeException))]
        [Test] public void Test_InsufficientError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/some"});
        }
        [ExpectedException(typeof(IndexOutOfRangeException))]
        [Test] public void Test_DashInsufficientError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-some"});
        }
        [ExpectedException(typeof(ArgumentException))]
        [Test] public void Test_BadRequired()
        {
            BadOpt bo = new BadOpt();
            bo.Process(new string[] {});
        }
        [Test] public void Test_Required()
        {
            ReqOpt bo = new ReqOpt(new string[] {"/req:here"});
            Assert.AreEqual("here", bo.Req);
        }
        [ExpectedException(typeof(ArgumentException))]
        [Test] public void Test_RequiredNotPassed()
        {
            ReqOpt bo = new ReqOpt(new string[] {});
        }
        [ExpectedException(typeof(IndexOutOfRangeException))]
        [Test] public void Test_MethodNotEnoughParams()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-methodparams", "2"});
        }
    }
    public enum TestOptEnum
    {
        FOO,
        BAR
    }

    public class TestGetOpt : GetOpt
    {
        [CommandLine("f", Description = "Foo or not")]
        public bool foo = false;
        [CommandLine(Description = "What's in a bar?")]
        public string bar = "one";
        [CommandLine("private")]
        private string m_baz = "default";
        [CommandLine("baz", "A property test")]
        public string baz
        {
            get
            {
                return m_baz;
            }
            set
            {
                m_baz = value;
            }
        }
        public int iNone;
        [CommandLine("some", Description = "Integer param")]
        public int iSome = 0;
        private int m_other = 3;
        [CommandLine]
        public int other
        {
            get
            {
                return m_other;
            }
            set
            {
                m_other = value;
            }
        }
        [CommandLine]
        public bool a = false;
        [CommandLine]
        public bool b = true;
        [CommandLine]
        public bool c = false;
        [CommandLine]
        public bool d = true;
        [CommandLine]
        public string e = null;

        [CommandLine]
        public TestOptEnum fb = TestOptEnum.FOO;

        [CommandLine]
        public string assembly;

        public string m = "before";
        public int mp = -1;

        public int m2 = -1;
        public string mp2 = "before";

        [CommandLine]
        public void method()
        {
            m = "after";
        }
        [CommandLine]
        public void methodparam(int one)
        {
            mp = one;
        }
        [CommandLine]
        public void methodparams(int one, string two)
        {
            m2 = one;
            mp2 = two;
        }
        public TestGetOpt() : base() {}
        public TestGetOpt(string[] args) : base(args) {}
    }
    public class  BadOpt : GetOpt
    {
        [CommandLine("bad", "A required parameter", true)]
        public string Bad = "Bad";
        public BadOpt() : base() {}
        public BadOpt(string[] args) : base(args) {}
    }
    public class  ReqOpt : GetOpt
    {
        [CommandLine(Required = true)]
        public string Req = null;
        public ReqOpt() : base() {}
        public ReqOpt(string[] args) : base(args) {}
    }
    public class NonChild
    {
        [CommandLine]
        public string test = null;
    }
    public class TooManyParams : GetOpt
    {
        public int mp = -1;
        public TooManyParams() : base() {}
        public TooManyParams(string[] args) : base(args) {}
    }
}
