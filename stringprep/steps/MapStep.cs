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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Text;
using System.Collections;

namespace stringprep.steps
{
    /// <summary>
    /// A stringprep profile step to map one input character into 0 or more output characters.
    /// </summary>
    public class MapStep : ProfileStep
    {
        private string[] m_table = null;
        private static IComparer m_comp = new CharMapComparer();

        /// <summary>
        /// Create a MapStep that doesn't look at flags.
        /// </summary>
        /// <param name="tab">Mapping table</param>
        /// <param name="name">Name of the step</param>
        public MapStep(string name) : base(name)
        {
        }

        /// <summary>
        /// Perform mapping for each character of input.
        /// </summary>
        /// <param name="result">Result is modified in place.</param>
        public override void Prepare(System.Text.StringBuilder result)
        {
            if (m_table == null)
            {
                lock (this)
                {
                    if (m_table == null)
                    {
                        m_table = (string[]) ResourceLoader.LoadRes(Name);
                    }
                }
            }

            // From RFC3454, section 3: 
            // Mapped characters are not re-scanned during the mapping step.  That
            // is, if character A at position X is mapped to character B, character
            // B which is now at position X is not checked against the mapping
            // table.

            int pos;
            string map;
            int len;
            for (int i=0; i<result.Length; i++)
            {
                pos = Array.BinarySearch(m_table, result[i], m_comp);
                if (pos < 0)
                    continue;

                map = m_table[pos];
                len = map.Length;
                if (len == 1)
                {
                    result.Remove(i, 1);
                    i--;
                }
                else
                {
                    result[i] = map[1];
                    if (len > 2) 
                    {
                        result.Insert(i+1, map.ToCharArray(2, len - 2));
                        i += len - 2;
                    }
                }
            }
        }

        private class CharMapComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                return ((string)x)[0].CompareTo(y);
            }

            #endregion

        }


    }
}