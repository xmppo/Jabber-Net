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
            if (ComposeData.Table[p] == -1)
                return 0;
            else
                return ComposeData.Data[ComposeData.Table[p], c & 0xff];
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

            index_a = Index(a);
            if ((index_a >= ComposeData.FIRST_SINGLE_START) && (index_a < ComposeData.SECOND_START))
            {
                if (b == ComposeData.FirstSingle[index_a - ComposeData.FIRST_SINGLE_START, 0])
                {
                    result = ComposeData.FirstSingle[index_a - ComposeData.FIRST_SINGLE_START, 1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            index_b = Index(b);
            if (index_b >= ComposeData.SECOND_SINGLE_START)
            {
                if (a ==
                    ComposeData.SecondSingle[index_b - ComposeData.SECOND_SINGLE_START, 0])
                {
                    result =  ComposeData.SecondSingle[index_b - ComposeData.SECOND_SINGLE_START, 1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            if ((index_a >= ComposeData.FIRST_START) && 
                (index_a < ComposeData.FIRST_SINGLE_START) && 
                (index_b >= ComposeData.SECOND_START) && 
                (index_a < ComposeData.SECOND_SINGLE_START))
            {
                char res = ComposeData.Array[index_a - ComposeData.FIRST_START, 
                    index_b - ComposeData.SECOND_START];

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
