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
using System.Diagnostics;
using stringprep.steps;

namespace stringprep
{
   
 	/// <summary>
	/// Summary description for Prep.
	/// </summary>
	public class Profile
	{
        /// <summary>
        /// RFC 3454, Appendix B.1
        /// </summary>
        public static readonly MapStep B_1 = new MapStep("RFC3454.B_1");
        /// <summary>
        /// RFC 3454, Appendix B.2
        /// </summary>
        public static readonly MapStep B_2 = new MapStep("RFC3454.B_2");
        /// <summary>
        /// RFC 3454, Appendix B.3
        /// </summary>
        public static readonly MapStep B_3 = new MapStep("RFC3454.B_3");

        /// <summary>
        /// RFC 3454, Appendix C.1.1
        /// </summary>        
        public static readonly ProhibitStep C_1_1 = new ProhibitStep("RFC3454.C_1_1");
        /// <summary>
        /// RFC 3454, Appendix C.1.2
        /// </summary>        
        public static readonly ProhibitStep C_1_2 = new ProhibitStep("RFC3454.C_1_2");
        /// <summary>
        /// RFC 3454, Appendix C.2.1
        /// </summary>        
        public static readonly ProhibitStep C_2_1 = new ProhibitStep("RFC3454.C_2_1");
        /// <summary>
        /// RFC 3454, Appendix C.2.2
        /// </summary>        
        public static readonly ProhibitStep C_2_2 = new ProhibitStep("RFC3454.C_2_2");
        /// <summary>
        /// RFC 3454, Appendix C.3
        /// </summary>        
        public static readonly ProhibitStep C_3   = new ProhibitStep("RFC3454.C_3");
        /// <summary>
        /// RFC 3454, Appendix C.4
        /// </summary>        
        public static readonly ProhibitStep C_4   = new ProhibitStep("RFC3454.C_4");
        /// <summary>
        /// RFC 3454, Appendix C.5
        /// </summary>        
        public static readonly ProhibitStep C_5   = new ProhibitStep("RFC3454.C_5");
        /// <summary>
        /// RFC 3454, Appendix C.6
        /// </summary>        
        public static readonly ProhibitStep C_6   = new ProhibitStep("RFC3454.C_6");
        /// <summary>
        /// RFC 3454, Appendix C.7
        /// </summary>        
        public static readonly ProhibitStep C_7   = new ProhibitStep("RFC3454.C_7");
        /// <summary>
        /// RFC 3454, Appendix C.8
        /// </summary>        
        public static readonly ProhibitStep C_8   = new ProhibitStep("RFC3454.C_8");
        /// <summary>
        /// RFC 3454, Appendix C.9
        /// </summary>        
        public static readonly ProhibitStep C_9   = new ProhibitStep("RFC3454.C_9");

        /// <summary>
        /// RFC 3454, Section 4
        /// </summary>
        public static readonly NFKCStep NFKC = new NFKCStep();
        /// <summary>
        /// RFC 3454, Section 6
        /// </summary>
        public static readonly BidiStep BIDI = new BidiStep();
        /// <summary>
        /// RFC 3454, Section 7
        /// </summary>
        public static readonly ProhibitStep UNASSIGNED = new ProhibitStep("RFC3454.A_1");

        private ProfileStep[] m_profile;

        /// <summary>
        /// Create a new profile, with the given steps.
        /// </summary>
        /// <param name="profile">The steps to perform</param>
		public Profile(ProfileStep[] profile)
		{
            m_profile = profile;
		}

        /// <summary>
        /// Prepare a string, according to the specified profile.
        /// </summary>
        /// <param name="input">The string to prepare</param>
        /// <returns>The prepared string</returns>
        public string Prepare(string input)
        {
            StringBuilder result = new StringBuilder(input);
            Prepare(result);
            return result.ToString();
        }

        /// <summary>
        /// Prepare a string, according to the specified profile, in place.
        /// Not thread safe; make sure the input is locked, if appropriate.
        /// (this is the canonical version, that should be overriden by
        /// subclasses if necessary)
        /// </summary>
        /// <param name="result">The string to prepare in place</param>
        public virtual void Prepare(StringBuilder result)
        { 
            foreach (ProfileStep step in m_profile)
            {
                step.Prepare(result);
            }
        }
	}
}