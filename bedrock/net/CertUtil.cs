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
 * --------------------------------------------------------------------------*/
using System;
using Org.Mentalis.Security.Certificates;

namespace bedrock.net
{
	/// <summary>
	/// Utilities for creating certificates
	/// </summary>
	public class CertUtil
	{
        /// <summary>
        /// Can this cert be used for server authentication?
        /// </summary>
        private const string OID_PKIX_KP_SERVER_AUTH = "1.3.6.1.5.5.7.3.1";
        /// <summary>
        /// Can this cert be used for client authentication?
        /// </summary>
        private const string OID_PKIX_KP_CLIENT_AUTH = "1.3.6.1.5.5.7.3.2";

        /// <summary>
        /// Find a server certificate in the given store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static Certificate FindServerCert(CertificateStore store)
        {
            // return store.FindCertificate(new string[] {OID_PKIX_KP_SERVER_AUTH});
            return store.FindCertificateByUsage(new string[] {OID_PKIX_KP_SERVER_AUTH});
        }

        /// <summary>
        /// Find a client certificate in the given store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static Certificate FindClientCert(CertificateStore store)
        {
            //return store.FindCertificate(new string[] {OID_PKIX_KP_CLIENT_AUTH});
            return store.FindCertificateByUsage(new string[] {OID_PKIX_KP_CLIENT_AUTH});
        }
    }
}
