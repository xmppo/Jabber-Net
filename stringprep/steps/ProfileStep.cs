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
    public abstract class ProfileStep
    {
        private ProfileFlags m_flags;
        private bool m_invert;
        private string m_name;

        public ProfileStep(string name)
        {
            m_name = name;
            m_flags = 0;
            m_invert = false;
        }

        public ProfileStep(string name, ProfileFlags flags, bool inverted)
        {
            m_name = name;
            m_flags = flags;
            m_invert = inverted;
        }

        public virtual string Name
        {
            get { return m_name; }
        }

        protected bool IsBitSet(ProfileFlags flags)
        {
            if (m_flags == 0)
                return false;

            // if m_flags all set in flags, or if invert, m_flags not all set in flags.
            return m_invert ^ ((m_flags & flags) == m_flags);
        }

        public abstract void Prepare(StringBuilder result, ProfileFlags flags);
    }
}

