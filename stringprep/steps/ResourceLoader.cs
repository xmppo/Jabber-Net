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
using System.Resources;
using System.Reflection;

namespace stringprep.steps
{
	class ResourceLoader
	{
        private const string RFC3454 = "stringprep.steps.rfc3454";
        private static ResourceManager m_rfc_res = null;

        private static ResourceManager Resources
        {
            get
            {
                if (m_rfc_res == null)
                {
                    lock (RFC3454)
                    {
                        if (m_rfc_res == null)
                            m_rfc_res = new ResourceManager(RFC3454, Assembly.GetExecutingAssembly());
                    }
                }
                return m_rfc_res;
            }
        }

        public static object LoadRes(string name)
        {
            return Resources.GetObject(name);
        }
    }
}
