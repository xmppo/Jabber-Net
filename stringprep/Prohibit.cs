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
#if !NO_STRINGPREP
using System;

namespace stringprep
{

    /// <summary>
    /// A character (or character range) to be prohibited in an input string.
    /// </summary>
    public struct Prohibit : IComparable
    {
        private char m_start;
        private char m_end;

        /// <summary>
        /// Entry for a single character.
        /// </summary>
        /// <param name="s">The character to prohibit.</param>
        public Prohibit(char s)
        {
            m_start = s;
            m_end = '\x0';
        }

        /// <summary>
        /// Character range, from m_start to m_end inclusive.
        /// </summary>
        /// <param name="start">The start of the range</param>
        /// <param name="end">The end of the range</param>
        public Prohibit(char start, char end)
        {
            this.m_start = start;
            this.m_end = end;
        }

        /// <summary>
        /// Used for Array.BinarySearch, to look up a character.  This is a perversion of the spirit of
        /// BinarySearch, but it seems to work just fine.  :)
        /// </summary>
        /// <param name="obj">The character to compare to this instance</param>
        /// <returns>-1, 0, 1 for less, equal, more</returns>
        int IComparable.CompareTo(object obj)
        {
            if (obj is char)
            {
                char c = (char) obj;
                if (m_end == '\x0') 
                { // a single character version.  just compare to that char.
                    return m_start.CompareTo(c);
                }

                if (c < m_start)
                    return 1;
                if (c > m_end)
                    return -1;

                // must be in the range.  Hit!
                return 0;
            }
            else if (obj is Prohibit)
            {
                // well.. if this is a Prohibit, then just check the m_starts, since there 
                // shouldn't be any overlap.
                Prohibit p = (Prohibit) obj;
                return m_start.CompareTo(p.m_start);
            }
            throw new ArgumentException("Bad class", "obj");
        }
    }
}
#endif