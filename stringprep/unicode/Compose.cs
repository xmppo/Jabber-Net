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

namespace stringprep.unicode
{
    /// <summary>
    /// Combine combining characters, where possible.
    /// Note: this is still Black Magic, as far as I can tell.
    /// </summary>
	public class Compose
	{
        private static int Index(char c)
        {
            int p = c >> 8;
            if (p >= ComposeData.Table.Length)
                return 0;
            if (ComposeData.Table[p] == 255)
                return 0;
            else
                return ComposeData.Data[ComposeData.Table[p], c & 0xff];
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
            int index_a, index_b;

            // FIRST_START..FIRST_SINGLE_START: 
            // FIRST_SINGLE_START..SECOND_START: look up a to see if b matches
            // SECOND_START..SECOND_SINGLE_START: 
            // SECOND_SINGLE_START..: look up b to see if a matches

            index_a = Index(a);
            // for stuff in this range, there is only one possible combination for the character
            // on the left
            if (Between(index_a, ComposeData.FIRST_SINGLE_START, ComposeData.SECOND_START))
            {
                int offset = (index_a - ComposeData.FIRST_SINGLE_START) * 2;
                if (b == ComposeData.FirstSingle[offset])
                {
                    result = ComposeData.FirstSingle[offset + 1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            index_b = Index(b);
            // for this range, only one possible combination to the right.
            if (index_b >= ComposeData.SECOND_SINGLE_START)
            {
                int offset = (index_b - ComposeData.SECOND_SINGLE_START) * 2;
                if (a == ComposeData.SecondSingle[offset])
                {
                    result =  ComposeData.SecondSingle[offset + 1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            if (Between(index_a, ComposeData.FIRST_START, ComposeData.FIRST_SINGLE_START) &&
                Between(index_b, ComposeData.SECOND_START, ComposeData.SECOND_SINGLE_START))
            {
                int offset = (index_a - ComposeData.FIRST_START) * ComposeData.N_SECOND + 
                    (index_b - ComposeData.SECOND_START);
                char res = ComposeData.Array[offset];

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
#endif