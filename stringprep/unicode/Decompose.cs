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
 * Portions Copyright (c) 2003 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Text;
using System.Diagnostics;

namespace stringprep.unicode
{
    /// <summary>
    /// Hold an offset into a decomposition table for a given character.
    /// Also, provide static functions for decomposition.
    /// For example, Angstrom -> A + ring.
    /// </summary>
    public struct Decompose : IComparable
    {
        private char m_ch;
        private int m_offset;

        /// <summary>
        /// Hold the index into the Offsets table for this character.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="offset"></param>
        public Decompose(char ch, int offset)
        {
            m_ch = ch;
            m_offset = offset;
        }

        /// <summary>
        /// Used for BinarySearch.
        /// </summary>
        /// <param name="obj">A char or Decompose to compare against</param>
        /// <returns>-1, 0, 1 for less, equal, greater</returns>
        /// <exception cref="ArgumentException">obj is not a char or Decompose</exception>
        int IComparable.CompareTo(object obj)
        {
            if (obj is char)
                return m_ch.CompareTo(obj);
            if (obj is Decompose)
                return m_ch.CompareTo(((Decompose)obj).m_ch);
            
            throw new ArgumentException("Invalid type", "obj");
        }

        /// <summary>
        /// Look up the given character, and return the offset for it.
        /// </summary>
        /// <param name="ch">Character to loop up</param>
        /// <returns>-1 if not found</returns>
        public static int Find(char ch)
        {
            int offset = Array.BinarySearch(DecomposeData.Offsets, ch);
            if (offset >= 0) 
            {
                return DecomposeData.Offsets[offset].m_offset;
            }
            
            return -1;
        }

        /// <summary>
        /// What is the combining class for the given character?
        /// </summary>
        /// <param name="c">Character to look up</param>
        /// <returns>Combining class for this character</returns>
        public static int CombiningClass(char c) 
        {
            int page = c >> 8;
            if (DecomposeData.CombiningClasses[page] == 255)
                return 0;
            else
                return DecomposeData.Data[DecomposeData.CombiningClasses[page], c & 0xff];
        }

        /// <summary>
        /// Reorder characters in the given range into their correct cannonical ordering with
        /// respect to combining class.
        /// </summary>
        /// <param name="buf">Buffer to reorder</param>
        /// <param name="start">Start of segment to reorder</param>
        /// <param name="len">Lenght of segment to reorder</param>
        public static void CanonicalOrdering(StringBuilder buf, int start, int len)
        {
            int i, j;
            bool swap = false;
            int p_a, p_b;
            char t;
            int stop = start + len - 1;

            // From Unicode 3.0, section 3.10
            // R1 For each character x in D, let p(x) be the combining class of x
            // R2 Wenever any pair (A, B) of adjacent characters in D is such that p(B)!=0 and
            //    p(A)>p(B), exchange those characters
            // R3 Repeat step R2 until no exchanges can be made among any of the characters in D


            do 
            {
                swap = false;
                p_a = CombiningClass(buf[start]);

                for (i = start; i < stop; i++)
                {
                    p_b = CombiningClass(buf[i + 1]);
                    if ((p_b != 0) && (p_a > p_b))
                    {
                        for (j = i; j > 0; j--)
                        {
                            if (CombiningClass(buf[j]) <= p_b)
                                break;

                            t = buf[j + 1];
                            buf[j + 1] = buf[j];
                            buf[j] = t;
                            swap = true;
                        }
                        /* We're re-entering the loop looking at the old
                           character again.  */
                        p_b = p_a;
                    }
                    p_a = p_b;
                }
            } while (swap);
        }
    }
}
