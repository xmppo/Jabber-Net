/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Collections;

namespace stringprep.unicode
{
    /// <summary>
    /// Decomposition data for NFKC.
    /// </summary>
    public class Decompose
    {
        private static bool s_init = false;
        private static char[][] s_offsets = null;
        private static string[] s_expansion = null;
        private static IComparer s_comparer = new CharArrayComparer();

        /// <summary>
        /// Look up the expansion, if any, for the given character.
        /// </summary>
        /// <param name="ch">The character to find</param>
        /// <returns>the expansion, or null if none found.</returns>
        public static string Find(char ch)
        {
            if (!s_init)
            {
                lock(s_comparer)
                {
                    if (!s_init)
                    {
                        s_offsets = (char[][]) ResourceLoader.LoadRes("Decompose.Offsets");
                        s_expansion = (string[]) ResourceLoader.LoadRes("Decompose.Expansion");
                        s_init = true;
                    }
                }
            }

            int offset = Array.BinarySearch(s_offsets, ch, s_comparer);
            if (offset < 0)
                return null;

            return s_expansion[s_offsets[offset][1]];
        }

        private class CharArrayComparer : IComparer
        {
            #region IComparer Members
            public int Compare(object x, object y)
            {
                return ((char[])x)[0].CompareTo(y);
            }
            #endregion
        }
    }
}
