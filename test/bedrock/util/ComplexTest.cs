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
using System.Diagnostics;
using NUnit.Framework;
using bedrock.util;
namespace test.bedrock.util
{
    
    [RCS(@"$Header$")]
    [TestFixture]
    public class ComplexTest
    {
        public void Test_ToString()
        {
            Complex c = new Complex(2, 3);
            Assertion.AssertEquals("2 + 3i", c.ToString());
            c.Imaginary = 0;
            Assertion.AssertEquals("2", c.ToString());
            c.Imaginary = -3;
            Assertion.AssertEquals("2 - 3i", c.ToString());
            c.Real = 0;
            Assertion.AssertEquals("-3i", c.ToString());
            
            c.Imaginary = 3;
            Assertion.AssertEquals("3i", c.ToString());
            
            c.Imaginary = Double.PositiveInfinity;
            Assertion.AssertEquals("Infinity", c.ToString());
            
            c.Imaginary = Double.NaN;
            Assertion.AssertEquals("NaN", c.ToString());
        }
        
        public void Test_Equals()
        {
            Complex z = new Complex(2, 3);
            Complex w = new Complex(2, 3);
            Assertion.AssertEquals(z, w);
            Assertion.Assert(z == w);
            z = 3d;
            Assertion.Assert(z == 3d);
        }
        
        public void Test_Cast()
        {
            Complex z = new Complex(4);
            int i = 4;
            long l = 4;
            float f = 4;
            double d = 4;
            
            Assertion.Assert( z == i );
            Assertion.Assert( z == l );        
            Assertion.Assert( z == f );
            Assertion.Assert( z == d );
            Assertion.Assert( z == (Complex)i );
            Assertion.Assert( z == (Complex)l );        
            Assertion.Assert( z == (Complex)f );
            Assertion.Assert( z == (Complex)d );
        }
        
        public void Test_Add()
        {
            Complex z = new Complex(2, 3);
            Complex w = new Complex(2, -5);
            Assertion.AssertEquals(z + w, new Complex(4, -2));
        }
        public void Test_Mult()
        {
            Complex z = new Complex(2, 3);
            Complex w = new Complex(2, -5);
            Assertion.AssertEquals(new Complex(19, -4), z * w);
        }
        
        public void Test_Div()
        {
            Complex z = new Complex(3, 5);
            Complex w = new Complex(3, -1);
            Assertion.AssertEquals(new Complex(.4, 1.8), z / w);
            w = new Complex(1, 3);
            Assertion.AssertEquals(new Complex(1.8, -0.4), z / w);
            
            z = new Complex(3, 6);
            Assertion.AssertEquals(new Complex(1, 2), z / 3);
        }
        public void Test_Conj()
        {
            Complex z = new Complex(7, 5);
            Assertion.AssertEquals(new Complex(7, -5), z.Conjugate());
        }
        public void Test_Abs()
        {
            Complex z = new Complex(3, 4);
            Assertion.AssertEquals(z.Abs(),  5d, 1e-7);
            z = new Complex(99, 20);
            Assertion.AssertEquals(z.Abs(), 101d, 1e-7);
            z = new Complex(3);
            Assertion.AssertEquals(z.Abs(), 3d, 1e-7);
            z = new Complex(0,3);
            Assertion.AssertEquals(z.Abs(), 3d, 1e-7);
        }
        
        public void Test_Neg()
        {
            Complex z = new Complex(3, 4);
            Assertion.AssertEquals(new Complex(-3, -4), -z);
        }
        
        public void Test_Sqrt()
        {
            double s2 = Math.Sqrt(2);
            Complex z = new Complex(3, 4);
            Assertion.AssertEquals(new Complex(2, 1), z.Sqrt());
            z = new Complex(6,8);
            Assertion.AssertEquals(new Complex(2 * s2, s2), z.Sqrt());
            z = new Complex(0,0);
            Assertion.AssertEquals(new Complex(0,0), z.Sqrt());
            z = new Complex(2);
            Assertion.AssertEquals(new Complex(s2, 0), z.Sqrt());
            z = new Complex(0, 8);
            Assertion.AssertEquals(new Complex(2, 2), z.Sqrt());
            z = new Complex(99, 20);
            Assertion.AssertEquals(new Complex(10,1), z.Sqrt());
        }
        public void Test_Exp()
        {
            Complex z = new Complex(0, Math.PI);
            Assertion.AssertEquals(new Complex(-1), z.Exp());
            z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(1.468693939915885, 2.287355287178842),
                         z.Exp());
        }
        public void Test_Polar()
        {
            Complex z = Complex.Polar(1, Math.PI);
            Assertion.AssertEquals(new Complex(-1), z);
        }
        public void Test_Pow()
        {
            Complex z = new Complex(1, 1);
            Assertion.AssertEquals(new Complex(-2, 2), z.Pow(3));
            Assertion.AssertEquals(new Complex(-0.265653998849241, 0.3198181138561361),
                         z.Pow(new Complex(2,2)));
        }
        public void Test_Arg()
        {
            Complex z = new Complex(1, 1);
            Assertion.AssertEquals(0.7853981633974483, z.Arg());
        }
        public void Test_Log()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(0.3465735902799727, 0.7853981633974483),
                         z.Log());
        }
        public void Test_Sin()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(1.298457581415977, 0.6349639147847361),
                         z.Sin());
        }
        public void Test_Cos()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(0.8337300251311491, -0.9888977057628651),
                         z.Cos());
        }
        public void Test_Tan()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(0.2717525853195117, 1.083923327338695),
                         z.Tan());
        }
        
        public void Test_Inv()
        {
            Complex z = new Complex(1,2);
            Assertion.AssertEquals(new Complex(0.2d, -0.4d), z.Inverse());      
            z = new Complex(2,1);
            Assertion.AssertEquals(new Complex(0.4d, -0.2d), z.Inverse());      
        }
        public void Test_Sinh()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(0.6349639147847361, 1.298457581415977),
                         z.Sinh());      
            
        }
        public void Test_Cosh()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(0.8337300251311491, 0.9888977057628651),
                         z.Cosh());      
            
        }
        public void Test_Tanh()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(1.083923327338695, 0.2717525853195117),
                         z.Tanh());      
            
        }
        public void Test_Asin()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(0.6662394324925153, 1.061275061905036),
                         z.Asin());      
            
        }
        public void Test_Acos()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(0.9045568943023814, -1.061275061905036),
                         z.Acos());      
            
        }
        public void Test_Atan()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(1.017221967897851, 0.4023594781085251),
                         z.Atan());        
        }
        public void Test_Asinh()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(1.061275061905036, 0.6662394324925153),
                         z.Asinh());        
        }
        public void Test_Acosh()
        {
            Complex z = new Complex(1,1);
            Assertion.AssertEquals(new Complex(1.061275061905036, 0.9045568943023814),
                         z.Acosh());        
        }
        public void Test_Atanh()
        {
            Complex z = new Complex(.5,.5);
            Assertion.AssertEquals(new Complex(0.4023594781085251, 0.5535743588970453),
                         z.Atanh());        
        }
        public void Test_Nan()
        {
            Complex z = new Complex(Double.NaN);
            Assertion.Assert(z.IsNaN());
            z.Real = 4;
            Assertion.Assert(!z.IsNaN());
            z.Imaginary = Double.NaN;
            Assertion.Assert(z.IsNaN());      
        }
        
        public void Test_Infinity()
        {
            Complex z = new Complex(Double.PositiveInfinity);
            Assertion.Assert(z.IsInfinity());
            z.Real = 4;
            Assertion.Assert(!z.IsInfinity());
            z.Imaginary = Double.NegativeInfinity;
            Assertion.Assert(z.IsInfinity());      
        }
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_Tolerance()
        {
            Complex.Tolerance = -1;
        }

        public void Test_Format()
        {
            Complex z = new Complex(1, 2);
            string f = string.Format("{0}", z);
            Assertion.AssertEquals("1 + 2i", f);
            f = string.Format("{0:f2}", z);
            Assertion.AssertEquals("1.00 + 2.00i", f);
            z = new Complex(1, -2);
            f = string.Format("{0:c2}", z);
            Assertion.AssertEquals("$1.00 - $2.00i", f);
        }
    }
}
