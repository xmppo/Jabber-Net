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
    public class ProhibitedCharacterException : Exception
    {
        public char InvalidChar = '\x00';
        
        public ProhibitedCharacterException(ProfileStep step, char c) : 
            base(string.Format("Step {0} prohibits string (character U+{1:x04}).", step.Name, (ushort) c))
        {
            InvalidChar = c;
        }
    }

    public class ProhibitStep : ProfileStep
    {
        private Prohibit[] m_table;

        public ProhibitStep(Prohibit[] tab, string name) : base(name, 0, false)
        {
            m_table = tab;
        }

        protected bool Contains(char c)
        {
            return (Array.BinarySearch(m_table, c) >= 0);
        }

        public int FindStringInTable(StringBuilder s)
        {
            for (int j=0; j<s.Length; j++)
            {
                if (Contains(s[j]))
                {
                    return j;
                }
            }
            return -1;
        }

        public override void Prepare(System.Text.StringBuilder result, ProfileFlags flags)
        {
            int j = FindStringInTable(result);
            if (j >= 0)
                throw new ProhibitedCharacterException(this, result[j]);
        }
    }


}
