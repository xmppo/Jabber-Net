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
    /// A map from an input character to zero or more output characters.  The output characters
    /// are split into the first character and subsequent characters, to facilitate replacement
    /// and subsequent insertion.  This should turn out to be a slight perf win for chars less
    /// than U+00FF (western Europe and US).
    /// </summary>
    public struct CharMap : IComparable
    {
        /// <summary>
        /// The char to map from.
        /// </summary>
        public char ch;
        /// <summary>
        /// The first character to map to.  Subsequent characters will be in map.
        /// 0 if there is no mapping (i.e. prohibit table)
        /// </summary>
        public char map0;
        /// <summary>
        /// The 1..n characters to map to.  null if there are no more characters.
        /// </summary>
        public string map;  /* remaining characters of map string */

        /// <summary>
        /// Map from first character in string to remaing chars in string.
        /// </summary>
        /// <param name="fromto"></param>
        public CharMap(string fromto)
        {
            ch = fromto[0];
            int len = fromto.Length;
            switch (len)
            {
            case 1:
                map0 = '\x0';
                map = null;
                break;
            case 2:
                map0 = fromto[1];
                map = null;
                break;
            default:
                map0 = fromto[1];
                map = fromto.Substring(2);
                break;
            }
        }

        /// <summary>
        /// Map to nothing.
        /// </summary>
        /// <param name="s"></param>
        public CharMap(char s)
        {
            ch = s;
            map0 = '\x0';
            map = null;
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
                return ch.CompareTo(obj);
            }
            if (obj is CharMap)
            {
                CharMap cm = (CharMap) obj;
                return ch.CompareTo(cm.ch);
            }
            throw new ArgumentException("Bad class", "obj");
        }
    }
}
#endif