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


namespace stringprep.unicode
{
    /// <summary>
    /// Combine combining characters, where possible.
    /// Note: this is still Black Magic, as far as I can tell.
    /// </summary>
	public class Compose
	{
        private static bool s_init = false;
        private static int  s_firstStart = -1;
        private static int  s_firstSingleStart = -1;
        private static int  s_secondStart = -1;
        private static int  s_secondSingleStart = -1;
        private static short[,] s_data = null;
        private static char[,] s_array = null;
        private static byte[] s_table = null;
        private static char[,] s_firstSingle = null;
        private static char[,] s_secondSingle = null;
        private static object s_lock = new object();

        private static int Index(char c)
        {
            int p = c >> 8;
            if (p >= s_table.Length)
                return 0;
            if (s_table[p] == 255)
                return 0;
            else
                return s_data[s_table[p], c & 0xff];
        }

        private static bool Between(int x, int start, int end)
        {
            return (x >= start) && (x < end);
        }

        /// <summary>
        /// Combine two characters together, if possible.
        /// </summary>
        /// <param name="a">First character to combine</param>
        /// <param name="b">Second character to combine</param>
        /// <param name="result">The combined character, if method returns true.  Otherwise, undefined.</param>
        /// <returns>True if combination occurred</returns>
        public static bool Combine(char a, char b, out char result)
        {
            if (! s_init)
            {
                lock (s_lock)
                {
                    if (! s_init)
                    {
                        s_firstStart = (short) ResourceLoader.LoadRes("Compose.FIRST_START");
                        s_firstSingleStart = (short) ResourceLoader.LoadRes("Compose.FIRST_SINGLE_START");
                        s_secondStart = (short) ResourceLoader.LoadRes("Compose.SECOND_START");
                        s_secondSingleStart = (short) ResourceLoader.LoadRes("Compose.SECOND_SINGLE_START");
                        s_data = (short[,]) ResourceLoader.LoadRes("Compose.Data");
                        s_array = (char[,]) ResourceLoader.LoadRes("Compose.Array");
                        s_table = (byte[]) ResourceLoader.LoadRes("Compose.Table");
                        s_firstSingle = (char[,]) ResourceLoader.LoadRes("Compose.FirstSingle");
                        s_secondSingle = (char[,]) ResourceLoader.LoadRes("Compose.SecondSingle");
                        s_init = true;
                    }
                }
            }

            // FIRST_START..FIRST_SINGLE_START: 
            // FIRST_SINGLE_START..SECOND_START: look up a to see if b matches
            // SECOND_START..SECOND_SINGLE_START: 
            // SECOND_SINGLE_START..: look up b to see if a matches

            int index_a = Index(a);
            // for stuff in this range, there is only one possible combination for the character
            // on the left
            if (Between(index_a, s_firstSingleStart, s_secondStart))
            {
                int offset = index_a - s_firstSingleStart;
                if (b == s_firstSingle[offset,0])
                {
                    result = s_firstSingle[offset,1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            int index_b = Index(b);
            // for this range, only one possible combination to the right.
            if (index_b >= s_secondSingleStart)
            {
                int offset = index_b - s_secondSingleStart;
                if (a == s_secondSingle[offset,0])
                {
                    result =  s_secondSingle[offset,1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            if (Between(index_a, s_firstStart, s_firstSingleStart) &&
                Between(index_b, s_secondStart, s_secondSingleStart))
            {
                char res = s_array[index_a - s_firstStart, index_b - s_secondStart];

                if (res != '\x0')
                {
                    result = res;
                    return true;
                }
            }

            result = '\x0';
            return false;
        }
    }
}