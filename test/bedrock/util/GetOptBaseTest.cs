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
    public class GetOptTest : TestCase
    {
        public GetOptTest(string name) : base(name) {}
        public static ITest Suite
        {
            get { return new TestSuite(typeof(GetOptTest)); }
        }
        public void Test_Construct()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/f", "/bar", "baz"});
            AssertEquals(true, t.foo);
            AssertEquals("baz", t.bar);
        }
        public void Test_Usage()
        {
            TestGetOpt t = new TestGetOpt();
            Console.WriteLine(t.Usage);
        }
        public void Test_Int()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "1", "/other", "2", "blah"});
            AssertEquals(1, t.iSome);
            AssertEquals(2, t.other);
            AssertEquals("one", t.bar);
        }
        public void Test_Private()
        {
            // hrmph.  This should probably throw an error, if I don't have the right permissions.
            TestGetOpt t = new TestGetOpt(new string[] {"/private", "one"});
            AssertEquals("one", t.baz);
        }
        public void Test_Args()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "1", "/other", "2", "blah", "bloo", "boo"});
            AssertEquals(3, t.Args.Length);
            AssertEquals("blah", t.Args[0]);
            AssertEquals("bloo", t.Args[1]);
            AssertEquals("boo", t.Args[2]);
        }
        public void Test_Colon()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some:1", "/other:2", "blah", "bloo", "boo"});
            AssertEquals(1, t.iSome);
            AssertEquals(2, t.other);
            AssertEquals(3, t.Args.Length);
        }
        public void Test_Bool()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/a", "/b", "four", "one", "more"});
            AssertEquals(true,   go.a);
            AssertEquals(false,  go.b);
            AssertEquals(false,  go.c);
            AssertEquals(true,   go.d);
            AssertEquals(3,      go.Args.Length);
            AssertEquals("four", go.Args[0]);
            AssertEquals("one",  go.Args[1]);
            AssertEquals("more", go.Args[2]);
        }
        public void Test_DashBool()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-a", "-b", "four", "one", "more"});
            AssertEquals(true,   go.a);
            AssertEquals(false,  go.b);
            AssertEquals(false,  go.c);
            AssertEquals(true,   go.d);
            AssertEquals(3,      go.Args.Length);
            AssertEquals("four", go.Args[0]);
            AssertEquals("one",  go.Args[1]);
            AssertEquals("more", go.Args[2]);
        }
        
        public void Test_DashArgs()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-bar", "four", "-baz:one", "more"});
            AssertEquals("four", go.bar);
            AssertEquals("one",  go.baz);
            AssertEquals(null,   go.e);
            AssertEquals(1,      go.Args.Length);
            AssertEquals("more", go.Args[0]);
        }
        
        public void Test_Env()
        {
            TestGetOpt go = new TestGetOpt(null);
            Assert(!go.Args[0].StartsWith("NUnit"));
        }
        public void Test_CaseInsensitive()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-BaZ:boo"});
            AssertEquals("boo", go.baz);
        }
        public void Test_Enum()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-fb:bar"});
            AssertEquals(TestOptEnum.BAR, go.fb);
        }
        public void Test_Method()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-method"});
            AssertEquals("after", go.m);
        }
        public void Test_MethodParam()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-methodparam", "2"});
            AssertEquals(2, go.mp);
            go = new TestGetOpt(new string[] {"-methodparams", "2", "after"});
            AssertEquals(2, go.m2);
            AssertEquals("after", go.mp2);
        }
        public void Test_NonChild()
        {
            NonChild nc = new NonChild();
            GetOpt go = new GetOpt(nc);
            go.Process(new string[] {"-test", "foo"});
            AssertEquals("foo", nc.test);
        }
        public void Test_Indexer()
        {
            NonChild nc = new NonChild();
            GetOpt go = new GetOpt(nc);
            go["test"] = "foo";
            AssertEquals("foo", nc.test);
        }
        [ExpectException(typeof(FormatException))]
        public void Test_BadInt()
        {
            TestGetOpt t = new TestGetOpt(new string[] {"/some", "one"});
        }
        [ExpectException(typeof(ArgumentException))]
        public void Test_Error()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/broncos"});
        }
        [ExpectException(typeof(ArgumentException))]
        public void Test_DashError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-broncos"});
        }
        [ExpectException(typeof(IndexOutOfRangeException))]
        public void Test_InsufficientError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"/some"});
        }
        [ExpectException(typeof(IndexOutOfRangeException))]
        public void Test_DashInsufficientError()
        {
            TestGetOpt go = new TestGetOpt(new string[] {"-some"});
        }
        [ExpectException(typeof(ArgumentException))]
        public void Test_BadRequired()
        {
            BadOpt bo = new BadOpt();
            bo.Process(new string[] {});
        }
        public void Test_Required()
        {
            ReqOpt bo = new ReqOpt(new string[] {"/req:here"});
            AssertEquals("here", bo.Req);
        }
        [ExpectException(typeof(ArgumentException))]
        public void Test_RequiredNotPassed()
        {
            ReqOpt bo = new ReqOpt(new string[] {});
        }
        [ExpectException(typeof(IndexOutOfRangeException))]
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
