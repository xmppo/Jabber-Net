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
using NUnit.Framework;
using bedrock.util;
namespace test.bedrock.util
{
    /// <summary>
    ///    Summary description for GetOptBaseTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class GetOptTest
    {
        public void Test_Construct()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/f", "/bar", "baz"});
            Assertion.AssertEquals(true, t.foo);
            Assertion.AssertEquals("baz", t.bar);
        }
        public void Test_Usage()
        {
            TestGetOpt t = new TestGetOpt();
            Console.WriteLine(t.Usage);
        }
        public void Test_Int()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "1", "/other", "2", "blah"});
            Assertion.AssertEquals(1, t.iSome);
            Assertion.AssertEquals(2, t.other);
            Assertion.AssertEquals("one", t.bar);
        }
        public void Test_Private()
        {
            // hrmph.  This should probably throw an error, if I don't have the right permissions.
            TestGetOpt t = new TestGetOpt(new string[] {"/private", "one"});
            Assertion.AssertEquals("one", t.baz);
        }
        public void Test_Args()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "1", "/other", "2", "blah", "bloo", "boo"});
            Assertion.AssertEquals(3, t.Args.Length);
            Assertion.AssertEquals("blah", t.Args[0]);
            Assertion.AssertEquals("bloo", t.Args[1]);
            Assertion.AssertEquals("boo", t.Args[2]);
        }
        public void Test_Colon()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some:1", "/other:2", "blah", "bloo", "boo"});
            Assertion.AssertEquals(1, t.iSome);
            Assertion.AssertEquals(2, t.other);
            Assertion.AssertEquals(3, t.Args.Length);
        }
        public void Test_Bool()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/a", "/b", "four", "one", "more"});
            Assertion.AssertEquals(true,   go.a);
            Assertion.AssertEquals(false,  go.b);
            Assertion.AssertEquals(false,  go.c);
            Assertion.AssertEquals(true,   go.d);
            Assertion.AssertEquals(3,      go.Args.Length);
            Assertion.AssertEquals("four", go.Args[0]);
            Assertion.AssertEquals("one",  go.Args[1]);
            Assertion.AssertEquals("more", go.Args[2]);
        }
        public void Test_DashBool()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-a", "-b", "four", "one", "more"});
            Assertion.AssertEquals(true,   go.a);
            Assertion.AssertEquals(false,  go.b);
            Assertion.AssertEquals(false,  go.c);
            Assertion.AssertEquals(true,   go.d);
            Assertion.AssertEquals(3,      go.Args.Length);
            Assertion.AssertEquals("four", go.Args[0]);
            Assertion.AssertEquals("one",  go.Args[1]);
            Assertion.AssertEquals("more", go.Args[2]);
        }
        
        public void Test_DashArgs()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-bar", "four", "-baz:one", "more"});
            Assertion.AssertEquals("four", go.bar);
            Assertion.AssertEquals("one",  go.baz);
            Assertion.AssertEquals(null,   go.e);
            Assertion.AssertEquals(1,      go.Args.Length);
            Assertion.AssertEquals("more", go.Args[0]);
        }
        
        public void Test_Env()
        {
            TestGetOpt go = new TestGetOpt(null);
            Assertion.Assert(go.assembly.StartsWith("test"));
        }
        public void Test_CaseInsensitive()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-BaZ:boo"});
            Assertion.AssertEquals("boo", go.baz);
        }
        public void Test_Equals()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-baz=boo"});
            Assertion.AssertEquals("boo", go.baz);
        }
        public void Test_DoubleColon()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/baz:boo:bar"});
            Assertion.AssertEquals("boo:bar", go.baz);
        }
        public void Test_DoubleSlash()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/baz:boo/bar"});
            Assertion.AssertEquals("boo/bar", go.baz);
        }
        public void Test_NunitProblem()
        {
            TestGetOpt go = new TestGetOpt(new string[] {@"/assembly:test.dll"});
            Assertion.AssertEquals(@"test.dll", go.assembly);
        }
        public void Test_Enum()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-fb:bar"});
            Assertion.AssertEquals(TestOptEnum.BAR, go.fb);
        }
        public void Test_Method()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-method"});
            Assertion.AssertEquals("after", go.m);
        }
        public void Test_MethodParam()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-methodparam", "2"});
            Assertion.AssertEquals(2, go.mp);
            go = new TestGetOpt(new string[] {"-methodparams", "2", "after"});
            Assertion.AssertEquals(2, go.m2);
            Assertion.AssertEquals("after", go.mp2);
        }
        public void Test_NonChild()
        {
            NonChild nc = new NonChild();
            GetOpt go = new GetOpt(nc);
            go.Process(new string[] {"-test", "foo"});
            Assertion.AssertEquals("foo", nc.test);
        }
        public void Test_Indexer()
        {
            NonChild nc = new NonChild();
            GetOpt go = new GetOpt(nc);
            go["test"] = "foo";
            Assertion.AssertEquals("foo", nc.test);
        }
        [ExpectedException(typeof(FormatException))]
        public void Test_BadInt()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "one"});
        }
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Error()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/broncos"});
        }
        [ExpectedException(typeof(ArgumentException))]
        public void Test_DashError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-broncos"});
        }
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_InsufficientError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/some"});
        }
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_DashInsufficientError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-some"});
        }
        [ExpectedException(typeof(ArgumentException))]
        public void Test_BadRequired()
        {
            BadOpt bo = new BadOpt();
            bo.Process(new string[] {});
        }
        public void Test_Required()
        {
            ReqOpt bo = new ReqOpt(new string[] {"/req:here"});
            Assertion.AssertEquals("here", bo.Req);
        }
        [ExpectedException(typeof(ArgumentException))]
        public void Test_RequiredNotPassed()
        {
            ReqOpt bo = new ReqOpt(new string[] {});
        }
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_MethodNotEnoughParams()
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
