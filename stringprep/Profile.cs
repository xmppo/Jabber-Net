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
using System.Diagnostics;
using stringprep.steps;

namespace stringprep
{
    [Flags]
    public enum ProfileFlags
    {
        NO_NFKC         = (1 << 0),
        NO_BIDI         = (1 << 1),
        NO_UNASSIGNED   = (1 << 2),
    };
 
 	/// <summary>
	/// Summary description for Prep.
	/// </summary>
	public class Profile
	{
        public static readonly MapStep B_1 = new MapStep(RFC3454.B_1, "B.1");
        public static readonly MapStep B_2 = new MapStep(RFC3454.B_2, "B.2", ProfileFlags.NO_NFKC, false);
        public static readonly MapStep B_3 = new MapStep(RFC3454.B_3, "B.3", ProfileFlags.NO_NFKC, true);

        public static readonly ProhibitStep C_1_1 = new ProhibitStep(RFC3454.C_1_1, "C.1.1");
        public static readonly ProhibitStep C_1_2 = new ProhibitStep(RFC3454.C_1_2, "C.1.2");
        public static readonly ProhibitStep C_2_1 = new ProhibitStep(RFC3454.C_2_1, "C.2.1");
        public static readonly ProhibitStep C_2_2 = new ProhibitStep(RFC3454.C_2_2, "C.2.2");
        public static readonly ProhibitStep C_3   = new ProhibitStep(RFC3454.C_3, "C_3");
        public static readonly ProhibitStep C_4   = new ProhibitStep(RFC3454.C_4, "C_4");
        public static readonly ProhibitStep C_5   = new ProhibitStep(RFC3454.C_5, "C_5");
        public static readonly ProhibitStep C_6   = new ProhibitStep(RFC3454.C_6, "C_6");
        public static readonly ProhibitStep C_7   = new ProhibitStep(RFC3454.C_7, "C_7");
        public static readonly ProhibitStep C_8   = new ProhibitStep(RFC3454.C_8, "C_8");
        public static readonly ProhibitStep C_9   = new ProhibitStep(RFC3454.C_9, "C_9");

        public static readonly NFKCStep NFKC = new NFKCStep();
        public static readonly BidiStep BIDI = new BidiStep();
        public static readonly UnassignedStep UNASSIGNED = new UnassignedStep();

        private ProfileStep[] m_profile;
        //private const int MAX_MAP_CHARS = 4;

		public Profile(ProfileStep[] profile)
		{
            m_profile = profile;
		}

        public string Prepare(string input)
        {
            StringBuilder result = new StringBuilder(input);
            Prepare(result, 0);
            return result.ToString();
        }

        public void Prepare(StringBuilder result)
        {
            Prepare(result, 0);
        }

        public string Prepare(string input, ProfileFlags flags)
        {
            StringBuilder result = new StringBuilder(input);
            Prepare(result, flags);
            return result.ToString();
        }

        public virtual void Prepare(StringBuilder result, ProfileFlags flags)
        { 
            foreach (ProfileStep step in m_profile)
            {
                step.Prepare(result, flags);            
            }
        }
	}
}
