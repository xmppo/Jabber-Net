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

namespace stringprep.steps
{
    /// <summary>
    /// There was a problem with the Bidirection nature of a string to be prepped.
    /// </summary>
    public class BidiException : Exception
    {
        /// <summary>
        /// Create a new BidiException
        /// </summary>
        /// <param name="message"></param>
        public BidiException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// A stringprep profile step to check for Bidirectional correctness.  
    /// If the NO_BIDI flag is set, this is a no-op.
    /// </summary>
    public class BidiStep : ProfileStep
	{
        private static BidiProbibitStep m_prohibit = new BidiProbibitStep();
        private static BidiRALStep      m_ral      = new BidiRALStep();
        private static BidiLCatStep     m_lcat     = new BidiLCatStep();

        /// <summary>
        /// Create a new BidiStep.
        /// </summary>
        public BidiStep() : base("BIDI", ProfileFlags.NO_BIDI, true)
        {
        }

        /// <summary>
        /// Perform BiDi checks.
        /// 
        /// From RFC 3454, Section 6:
        /// In any profile that specifies bidirectional character handling, all
        /// three of the following requirements MUST be met:
        /// <ol>
        /// <li>The characters in section 5.8 MUST be prohibited.</li>
        /// <li>If a string contains any RandALCat character, the string MUST NOT
        /// contain any LCat character.</li>
        /// <li> If a string contains any RandALCat character, a RandALCat
        /// character MUST be the first character of the string, and a
        /// RandALCat character MUST be the last character of the string.</li>
        /// </ol>
        /// </summary>
        /// <param name="result">Result is modified in place.</param>
        /// <param name="flags">Skip this step if NO_BIDI is step in flags</param>
        /// <exception cref="BidiException">A BiDi problem exists</exception>
        public override void Prepare(System.Text.StringBuilder result, ProfileFlags flags)
        {
            if (!IsBitSet(flags))
                return;

            // prohibit section 5.8
            m_prohibit.Prepare(result, flags);

            if (m_ral.FindStringInTable(result) >= 0)
            {
                // If a string contains any RandALCat character, the string MUST NOT
                // contain any LCat character.
                if (m_lcat.FindStringInTable(result) >= 0)
                {
                    throw new BidiException("String contains both L and RAL characters");
                }

                m_ral.CheckEnds(result);
            }

        }

        private class BidiProbibitStep : ProhibitStep
        {
            public BidiProbibitStep() : base(stringprep.RFC3454.C_8, "C.8")
            {
            }
        }

        private class BidiRALStep : ProhibitStep
        {
            public BidiRALStep() : base(stringprep.RFC3454.D_1, "D.1")
            {
            }

            public void CheckEnds(System.Text.StringBuilder result)
            {
                //  3) If a string contains any RandALCat character, a RandALCat
                // character MUST be the first character of the string, and a
                // RandALCat character MUST be the last character of the string.
                if (!Contains(result[0]) || !Contains(result[result.Length - 1]))
                {
                    throw new BidiException("Bidi string does not start/end with RAL characters");
                }
            }
        }

        class BidiLCatStep : ProhibitStep
        {
            public BidiLCatStep() : base(stringprep.RFC3454.D_2, "D.2")
            {
            }
        }
    }


}
#endif