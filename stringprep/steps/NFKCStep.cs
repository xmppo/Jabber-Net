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

/*
 * Assumption: UCS2.  The astral planes don't exist.  At least according to windows?
 * 
 * Look over here.  Something shiny!
 */

using System;
using System.Text;
using stringprep.unicode;

namespace stringprep.steps
{
    public class NFKCStep : ProfileStep
    {

        public NFKCStep() : base("NFKC")
        {
        }

        public override void Prepare(StringBuilder result, ProfileFlags flags)
        {
            // From Unicode TR15:
            // R1. Normalization Form C
            // The Normalization Form C for a string S is obtained by applying the following process, 
            // or any other process that leads to the same result:
            //
            // 1) Generate the canonical decomposition for the source string S according to the 
            // decomposition mappings in the latest supported version of the Unicode Character Database. 
            //
            // 2) Iterate through each character C in that decomposition, from first to last. 
            // If C is not blocked from the last starter L, and it can be primary combined with L, 
            // then replace L by the composite L-C, and remove C. 

            if ((flags & ProfileFlags.NO_NFKC) == ProfileFlags.NO_NFKC)
                return;

            int decomp;
            int len;
            int last_start = 0;
            int old_len;
            char[] insert = new char[16];
            int j;

            // Decompose
            for (int i=0; i< result.Length; i++)
            {
                char c = result[i];

                old_len = result.Length;
                decomp = Decompose.Find(c);
                if (decomp >= 0)
                {
                    // 0, 0 terminates.
                    for (len = decomp, j=0; Decompose.More(len); len += 2, j++)
                    {
                        insert[j] = Decompose.Expand(len);
                    }

                    switch (j)
                    {
                    case 0:
                         // there were no characters in the replacement.  just remove the existing char.
                        result.Remove(i, 1);
                        i--;
                        break;
                    case 1:
                        result[i] = insert[0];
                        break;
                    default:
                        // for subsequent characters, remember them all, so we can do ONE insert.
                        result[i] = insert[0];
                        result.Insert(i+1, insert, 1, j-1);
                        i += (j-1);
                        break;
                    }
                }

                if ((result.Length > 0) && (old_len < result.Length))
                {
                    if (Decompose.CombiningClass(result[old_len]) == 0)
                    {
                        Decompose.CanonicalOrdering(result, last_start, result.Length - last_start);
                        last_start = old_len;
                    }
                }
            }

            // stuff left at the end?
            if (result.Length > 0)
            {
                Decompose.CanonicalOrdering(result, last_start, result.Length - last_start);
            }

            // All decomposed and reordered.
            // Combine all combinable characters.
            if (result.Length > 0)
            {
                int i, cc;
                int last_cc = 0;
                char c;
                last_start = 0;

                for (i=0; i<result.Length; i++)
                {
                    cc = Decompose.CombiningClass(result[i]);
                    if ((i > 0) &&
                        ((last_cc == 0) || (last_cc != cc)) &&
                        Compose.Combine(result[last_start], result[i], out c))
                    {
                        result[last_start] = c;
                        result.Remove(i, 1);
                        i--;

                        if (i == last_start)
                            last_cc = 0;
                        else
                            last_cc = Decompose.CombiningClass(result[i - 1]);

                        continue;
                    }

                    if (cc == 0)
                        last_start = i;

                    last_cc = cc;
                }
            }        
        }
      
    }
}