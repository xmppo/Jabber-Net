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
using System.Text;

namespace stringprep.steps
{
    /// <summary>
    /// Base class for steps in a stringprep profile.
    /// </summary>
    public abstract class ProfileStep
    {
        private string m_name;

        /// <summary>
        /// Create a named profile step, with no flags.
        /// </summary>
        /// <param name="name">The profile name</param>
        protected ProfileStep(string name)
        {
            m_name = name;
        }

        /// <summary>
        /// The name of the step.
        /// </summary>
        public virtual string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// This is the workhorse function, to be implemented in each subclass.
        /// </summary>
        /// <param name="result">Result will be modified in place</param>
        public abstract void Prepare(StringBuilder result);
    }
}
#endif