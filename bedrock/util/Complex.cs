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
namespace bedrock.util
{
    using System;
    /// <summary>
    /// Class to do math on complex numbers.  Lots of optimizations, many from 
    /// the numerical methods literature.  Sorry, but I've lost the citations by now.
    /// </summary>
    [RCS(@"$Header$")]
    public class Complex : IFormattable
    {
        private double m_real;
        private double m_imag;
        
        // Double.Epsilon is too small
        private static double s_tolerance = 1E-15;
        
        /// <summary>
        /// Create a complex number from a real part and an imaginary part.
        /// Both parts use double-precision.
        /// </summary>
        /// <param name="real">Real part</param>
        /// <param name="imag">Imaginary part.  Multiplied by "i" and added to real.</param>
        public Complex(double real, double imag)
        {
            m_real = real;
            m_imag = imag;
        }
        /// <summary>
        /// Complex number with imaginary part of 0.
        /// </summary>
        /// <param name="real">Real part</param>
        public Complex(double real) : this(real, 0.0)
        {
        }
        /// <summary>
        /// Create a complex number from a polar representation.
        /// </summary>
        /// <param name="magnitude">The magnitude of the polar representation</param>
        /// <param name="radianAngle">The angle, in radians, of the polar representation</param>
        public static Complex Polar(double magnitude, double radianAngle)
        {
            return new Complex(magnitude * Math.Cos(radianAngle),
                               magnitude * Math.Sin(radianAngle));
        }
        
        /// <summary>
        /// The real part of the complex number
        /// </summary>
        public double Real
        {
            get { return m_real; }
            set { m_real = value; }
        }
        
        /// <summary>
        /// The imaginary part of the complex number
        /// </summary>
        public double Imaginary
        {
            get { return m_imag; }
            set { m_imag = value; }
        }
        /// <summary>
        /// Get a new complex number that is the conjugate (Imaginary *= -1) of the current.
        /// </summary>
        public Complex Conjugate()
        {
            return new Complex(m_real, -m_imag);
        }
        /// <summary>
        /// Return the absoulte value of the complex number.
        /// </summary>
        public double Abs()
        {
            return Abs(m_real, m_imag);
        }
        /// <summary>
        /// sqrt(first^2 + second^2), with optimizations
        /// </summary>
        /// <param name="first">first number</param>
        /// <param name="second">second number</param>
        private static double Abs(double first, double second)
        {
            // avoid double math wherever possible...
            //return Math.Sqrt((first * first) + (second * second));
            first = Math.Abs(first);
            second = Math.Abs(second);
            
            if (first == 0d)
            {
                return second;
            }
            if (second == 0d)
            {
                return first;
            }
            if (first > second)
            {
                double temp = second / first;
                return first * Math.Sqrt(1d + (temp * temp));
            }
            else
            {
                double temp = first / second;
                return second * Math.Sqrt(1d + (temp * temp));
            }
        }
        
        /// <summary>
        /// Angle, in radians, of the current value.
        /// </summary>
        public double Arg()
        {
            return Math.Atan2(m_imag, m_real);
        }
        
        /// <summary>
        /// The square root of the current value.
        /// </summary>
        public Complex Sqrt()
        {
            //return Math.Sqrt(this.Abs()) *
            //    new Complex( Math.Cos(this.Arg()/2),
            //                 Math.Sin(this.Arg()/2));
            if ((m_real == 0d) && (m_imag == 0d))
            {
                return new Complex(0d, 0d);
            }
            else
            {
                double ar = Math.Abs(m_real);
                double ai = Math.Abs(m_imag);
                double temp;
                double w;
                
                if (ar >= ai)
                {
                    temp = ai / ar;
                    w = Math.Sqrt(ar) *
                        Math.Sqrt(0.5d * (1d + Math.Sqrt(1d + (temp * temp))));
                }
                else
                {
                    temp = ar / ai;
                    w = Math.Sqrt(ai) *
                        Math.Sqrt(0.5d * (temp + Math.Sqrt(1d + (temp * temp))));
                }
                if (m_real > 0d)
                {
                    return new Complex(w, m_imag / (2d * w));
                }
                else
                {
                    double r = (m_imag >= 0d) ? w : -w;
                    return new Complex(r, m_imag / (2d * r));
                }
            }
        }
        /// <summary>
        /// Raise the current value to a power.
        /// </summary>
        /// <param name="exponent">The power to raise to.</param>
        public Complex Pow(double exponent)
        {
            double real = exponent * Math.Log(this.Abs());
            double imag = exponent * this.Arg();
            double scalar = Math.Exp(real);
            return new Complex(scalar * Math.Cos(imag), scalar * Math.Sin(imag));
        }
        /// <summary>
        /// Raise the current value to a power.
        /// </summary>
        /// <param name="exponent">The power to raise to.</param>
        public Complex Pow(Complex exponent)
        {
            double real   = Math.Log(this.Abs());
            double imag   = this.Arg();
            double r2     = (real * exponent.m_real) - (imag * exponent.m_imag);
            double i2     = (real * exponent.m_imag) + (imag * exponent.m_real);
            double scalar = Math.Exp(r2);
            return new Complex(scalar * Math.Cos(i2), scalar * Math.Sin(i2));
        }
        
        /// <summary>
        /// Returns e raised to the specified power.
        /// </summary>
        public Complex Exp()
        {
            return Math.Exp(m_real) *
                new Complex( Math.Cos(m_imag), Math.Sin(m_imag));   
        }
        /// <summary>
        /// 1 / (the current value)
        /// </summary>
        public Complex Inverse()
        {
            double scalar;
            double ratio;
            
            if (Math.Abs(m_real) >= Math.Abs(m_imag))
            {
                ratio  = m_imag / m_real;
                scalar = 1d / (m_real + m_imag * ratio);
                return new Complex(scalar, -scalar * ratio);
            }
            else
            {
                ratio = m_real / m_imag;
                scalar = 1d / (m_real * ratio + m_imag);
                return new Complex(scalar * ratio, -scalar);
            }       
        }
        
        /// <summary>
        /// Returns the natural (base e) logarithm of the current value.
        /// </summary>
        public Complex Log()
        {
            return new Complex(Math.Log(this.Abs()), this.Arg());
        }
        /// <summary>
        /// Returns the sine of the current value.
        /// </summary>
        public Complex Sin()
        {
            Complex iz = this * Complex.i;
            Complex izn = -iz;
            return (iz.Exp() - izn.Exp()) / new Complex(0,2);
        }
        /// <summary>
        /// Returns the cosine of the current value.
        /// </summary>
        public Complex Cos()
        {
            Complex iz = this * Complex.i;
            Complex izn = -iz;
            return (iz.Exp() + izn.Exp()) / 2.0;
        }
        /// <summary>
        /// Returns the tangent of the current value.
        /// </summary>
        public Complex Tan()
        {
            return this.Sin() / this.Cos();
        }
        /// <summary>
        /// Returns the hyperbolic sin of the current value.
        /// </summary>
        public Complex Sinh()
        {
            return (this.Exp() - (-this).Exp()) / 2d;
        }
        /// <summary>
        /// Returns the hyperbolic cosine of the current value.
        /// </summary>
        public Complex Cosh()
        {
            return (this.Exp() + (-this).Exp()) / 2d;
        }
        /// <summary>
        /// Returns the hyperbolic tangent of the current value.
        /// </summary>
        public Complex Tanh()
        {
            return this.Sinh() / this.Cosh();
        }
        /// <summary>
        /// Returns the arc sine of the current value.
        /// </summary>
        public Complex Asin()
        {
            // TODO: if anyone cares about this function, some of it
            // should probably be inlined and streamlined.
            Complex I = i;
            return -I * ((this*I) + (1 - (this * this)).Sqrt()).Log();
        }
        /// <summary>
        /// Returns the arc cosine of the current value.
        /// </summary>
        public Complex Acos()
        {
            // TODO: if anyone cares about this function, some of it
            // should probably be inlined and streamlined.
            Complex I = i;
            return -I * (this + I * (1 - (this*this)).Sqrt()).Log();
        }
        /// <summary>
        /// Returns the arc tangent of the current value.
        /// </summary>
        public Complex Atan()
        {
            // TODO: if anyone cares about this function, some of it
            // should probably be inlined and streamlined.
            Complex I = i;
            return -I/2 * ((I - this)/(I + this)).Log();
        }
        /// <summary>
        /// Returns the arc hyperbolic sine of the current value.
        /// </summary>
        public Complex Asinh()
        {
            return (this + ((this*this) + 1).Sqrt()).Log();
        }
        
        /// <summary>
        /// Returns the arc hyperbolic cosine of the current value.
        /// </summary>
        public Complex Acosh()
        {
            return 2d * (((this+1d) / 2d).Sqrt() +
                         ((this-1) / 2d).Sqrt()).Log();
            // Gar.  This one didn't work.  Perhaps it isn't returning the
            // "pricipal" value.
            //return (this + ((this*this) - 1).Sqrt()).Log();
        }
        /// <summary>
        /// Returns the arc hyperbolic tangent of the current value.
        /// </summary>
        public Complex Atanh()
        {
            return ((1+this) / (1-this)).Log() / 2d;
        }
        
        /// <summary>
        /// Is the current value Not a Number?
        /// </summary>
        public bool IsNaN()
        {
            return Double.IsNaN(m_real) || Double.IsNaN(m_imag);
        }
        /// <summary>
        /// Is the current value infinite?
        /// </summary>
        public bool IsInfinity()
        {
            return Double.IsInfinity(m_real) || Double.IsInfinity(m_imag);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((int)m_imag << 16) ^ (int) m_real;
        }

        /// <summary>
        /// Format as string like "x + yi".
        /// </summary>
        public override string ToString()
        {
            return this.ToString(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="sop"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider sop) 
        {
            if (this.IsNaN())
                return "NaN";
            if (this.IsInfinity())
                return "Infinity";
            
            if (m_imag == 0d)
                return m_real.ToString(format, sop);
            if (m_real == 0d)
                return m_imag.ToString(format, sop) + "i";
            if (m_imag < 0.0)
            {
                return m_real.ToString(format, sop) + " - " +
                       (-m_imag).ToString(format, sop) + "i";
            }
                
            return m_real.ToString(format, sop) + " + " +
                   m_imag.ToString(format, sop) + "i";
        }
        /// <summary>
        /// Do a half-assed job of assessing equality, using the current Tolerance value.  
        /// Will work with other Complex numbers or doubles.
        /// </summary>
        /// <param name="other">The other object to compare against.  Must be double or Complex.</param>
        public override bool Equals(object other)
        {
            if (other is Complex)
            {
                Complex o = (Complex) other;
                // performance optimization for "identical" numbers"
                if ((o.m_real == m_real) && (o.m_imag == m_imag))
                    return true;
                return Equals(o, s_tolerance);
            }
            double d = (double) other;  // can fire exception
            if (m_imag != 0.0)
                return false;
            return Math.Abs(m_real - d) < s_tolerance;
        }
        /// <summary>
        /// Is this number within a tolerance of being equal to another Complex number?
        /// </summary>
        /// <param name="other">The other Complex to comapare against.</param>
        /// <param name="tolerance">The tolerance to be within.</param>
        public bool Equals(Complex other, double tolerance)
        {
            return (this - other).Abs() < tolerance;
        }
        
        /// <summary>
        /// Calls Equals().
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">Complex</param>
        public static bool operator==(Complex first, Complex second)
        {
            return first.Equals(second);
        }
        /// <summary>
        /// Calls !Equals().
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">Complex</param>
        public static bool operator!=(Complex first, Complex second)
        {
            return !first.Equals(second);
        }
        
        /// <summary>
        /// Adds two complex numbers.
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">Complex</param>
        public static Complex operator+(Complex first, Complex second)
        {
            return new Complex(first.m_real + second.m_real,
                               first.m_imag + second.m_imag);
        }
        /// <summary>
        /// Subtracts two complex numbers.
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">Complex</param>
        public static Complex operator-(Complex first, Complex second)
        {
            return new Complex(first.m_real - second.m_real,
                               first.m_imag - second.m_imag);
        }
        /// <summary>
        /// Negates a complex number.
        /// </summary>
        /// <param name="first">Complex</param>
        public static Complex operator-(Complex first)
        {
            return new Complex(-first.m_real, -first.m_imag);
        }
        /// <summary>
        /// Multiplies two complex numbers.
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">Complex</param>
        public static Complex operator*(Complex first, Complex second)
        {
            return new Complex((first.m_real * second.m_real) -
                               (first.m_imag * second.m_imag),
                               (first.m_real * second.m_imag) +
                               (first.m_imag * second.m_real));
        }
        /// <summary>
        /// Multiplies a complex number and a Complex.
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">double</param>
        public static Complex operator*(Complex first, double second)
        {
            return new Complex(first.m_real * second, first.m_imag * second);
        }
        /// <summary>
        /// Divides two Complex numbers.
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">Complex</param>
        public static Complex operator/(Complex first, Complex second)
        {
            //return (first * second.Conjugate()) /
            //    ((second.m_real * second.m_real) +
            //     (second.m_imag * second.m_imag));
            double scalar;
            double ratio;
            
            if (Math.Abs(second.m_real) >= Math.Abs(second.m_imag))
            {
                ratio = second.m_imag / second.m_real;
                scalar = 1d / (second.m_real + (second.m_imag * ratio));
                return new Complex(scalar * (first.m_real + (first.m_imag*ratio)),
                                   scalar * (first.m_imag - (first.m_real*ratio)));
            }
            else
            {
                ratio = second.m_real / second.m_imag;
                scalar = 1d / ((second.m_real * ratio) + second.m_imag);
                return new Complex(scalar * (first.m_real*ratio + first.m_imag),
                                   scalar * (first.m_imag*ratio - first.m_real));
            }
        }
        /// <summary>
        /// Divides a Complex number by a double.
        /// </summary>
        /// <param name="first">Complex</param>
        /// <param name="second">double</param>
        public static Complex operator/(Complex first, double second)
        {
            return new Complex(first.m_real / second, first.m_imag / second);
        }
        /// <summary>
        /// Converts a double to a real Complex number.
        /// </summary>
        /// <param name="real">Real part</param>
        public static implicit operator Complex(double real)
        {
            return new Complex(real);
        }
        /// <summary>
        /// Constant for sqrt(-1).
        /// </summary>
        public static Complex i
        {
            get { return new Complex(0, 1); }
        }
        /// <summary>
        /// Tolerance value for Equals().
        /// </summary>
        public static double Tolerance
        {
            get { return s_tolerance; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException
                        ("Tolerance must be greater than 0");
                s_tolerance = value;
            }
        }
    }
}
