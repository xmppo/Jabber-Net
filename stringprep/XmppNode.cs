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
using stringprep.steps;

namespace stringprep
{
	/// <summary>
	/// A stringprep profile for draft-ietf-xmpp-nodeprep-02, for Jabber nodes (the "user" part).
	/// </summary>
    public class XmppNode : Profile
    {
        private static readonly ProhibitStep XmppNodeprepProhibit = 
            new ProhibitStep(new char[][] 
                {   // note: these *must* be sorted by code.
                    new char[] {'"', '\x0000'},
                    new char[] {'&', '\x0000'},
                    new char[] {'\'', '\x0000'},
                    new char[] {'/', '\x0000'},
                    new char[] {':', '\x0000'},
                    new char[] {'<', '\x0000'},
                    new char[] {'>', '\x0000'},
                    new char[] {'@', '\x0000'},
                }, "XMPP Node");

        /// <summary>
        /// Create a new XmppNode profile instance.
        /// </summary>
        public XmppNode() : 
            base( new ProfileStep[] {   B_1, B_2, NFKC,
                                        C_1_1, C_1_2, C_2_1, C_2_2,
                                        C_3, C_4, C_5, C_6, C_7, C_8, C_9,
                                        XmppNodeprepProhibit,
                                        BIDI, UNASSIGNED} )
		{
		}
	}
}
