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
    /// A character that is forbidden by the current stringprep profile exists in the input.
    /// </summary>
    public class ProhibitedCharacterException : Exception
    {
        public char InvalidChar = '\x00';
        
        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="step">In which step did this occur?</param>
        /// <param name="c">The offending character</param>
        public ProhibitedCharacterException(ProfileStep step, char c) : 
            base(string.Format("Step {0} prohibits string (character U+{1:x04}).", step.Name, (ushort) c))
        {
            InvalidChar = c;
        }
    }

    /// <summary>
    /// A stringprep profile step that checks for prohibited characters
    /// </summary>
    public class ProhibitStep : ProfileStep
    {
        private Prohibit[] m_table;

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="tab">The prohibit table to be checked</param>
        /// <param name="name">The name of the step (for debugging purposes)</param>
        public ProhibitStep(Prohibit[] tab, string name) : base(name, 0, false)
        {
            m_table = tab;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="tab">The prohibit table to be checked</param>
        /// <param name="name">The name of the step (for debugging purposes)</param>
        /// <param name="flags">Skip this step if these flags are set</param>
        /// <param name="invert">Invert the meaning of the flags</param>
        public ProhibitStep(Prohibit[] tab, string name, ProfileFlags flags, bool invert) : base(name, flags, invert)
        {
            m_table = tab;
        }

        /// <summary>
        /// Does this step prohibit the given character?
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if the character is prohibited</returns>
        protected bool Contains(char c)
        {
            return (Array.BinarySearch(m_table, c) >= 0);
        }

        /// <summary>
        /// Check all of the characters for prohbition.
        /// </summary>
        /// <param name="s">String to check</param>
        /// <returns>If one of the characters is prohibited, returns the index of that character.  
        /// If all are allowed, returns -1.</returns>
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

        /// <summary>
        /// Check for prohibited characters
        /// </summary>
        /// <param name="result">No modifications</param>
        /// <param name="flags"></param>
        /// <exception cref="ProhibitedCharacterException">Invalid character detected.</exception>
        public override void Prepare(System.Text.StringBuilder result, ProfileFlags flags)
        {
            if (IsBitSet(flags))
                return;

            int j = FindStringInTable(result);
            if (j >= 0)
                throw new ProhibitedCharacterException(this, result[j]);
        }
    }


}
