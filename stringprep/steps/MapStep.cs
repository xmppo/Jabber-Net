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

namespace stringprep.steps
{
    /// <summary>
    /// A stringprep profile step to map one input character into 0 or more output characters.
    /// </summary>
    public class MapStep : ProfileStep
    {
        private CharMap[] m_table;

        /// <summary>
        /// Create a MapStep that doesn't look at flags.
        /// </summary>
        /// <param name="tab">Mapping table</param>
        /// <param name="name">Name of the step</param>
        public MapStep(CharMap[] tab, string name) : this(tab, name, 0, false)
        {
        }

        /// <summary>
        /// Create a MapStep that might look at flags.
        /// </summary>
        /// <param name="tab">Mapping table</param>
        /// <param name="name">name of the step</param>
        /// <param name="flags">Flags that apply for this step.  If the flag specified here is set in the Prepare method, no-op.</param>
        /// <param name="inverted">Are the flags inverted?</param>
        public MapStep(CharMap[] tab, string name, ProfileFlags flags, bool inverted) : base(name, flags, inverted)
        {
            m_table = tab;
        }

        /// <summary>
        /// Perform mapping for each character of input.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="flags"></param>
        public override void Prepare(System.Text.StringBuilder result, ProfileFlags flags)
        {
            if (IsBitSet(flags))
                return;

            // From RFC3454, section 3: 
            // Mapped characters are not re-scanned during the mapping step.  That
            // is, if character A at position X is mapped to character B, character
            // B which is now at position X is not checked against the mapping
            // table.

            int pos;
            for (int i=0; i<result.Length; i++)
            {
                pos = FindCharacterInTable(result[i]);
                if (pos < 0)
                    continue;
                if (m_table[pos].map0 == '\x0')
                {
                    result.Remove(i, 1);
                    i--;
                }
                else
                {
                    result[i] = m_table[pos].map0;
                    if (m_table[pos].map != null)
                    {
                        result.Insert(i+1, m_table[pos].map);
                        i += m_table[pos].map.Length;
                    }
                }
            }
            /*
            while ((pos = FindStringInTable(result, out i)) != -1)
            {
                if (m_table[i].map0 == '\x0')
                {
                    result.Remove(pos, 1);
                }
                else
                {
                    result[pos] = m_table[i].map0;
                    if (m_table[i].map != null)
                    {
                        result.Insert(pos+1, m_table[i].map);
                    }
                }
            }
            */

        }

        protected int FindCharacterInTable(char c)
        {
            int pos = Array.BinarySearch(m_table, c);
            if (pos < 0)
                pos = -1;
            return pos;
        }

        /*
        protected int FindStringInTable(StringBuilder s, out int tablepos)
        {
            int pos;
             
            for (int j=0; j<s.Length; j++)
            {
                if ((pos = FindCharacterInTable(s[j])) != -1)
                {
                    tablepos = pos;
                    return j;
                }
            }
            tablepos = -1;
            return -1;
        }
        */
    }
}
