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
using NUnit.Framework;
using stringprep;
using stringprep.steps;

namespace test.stringprep
{
	[TestFixture]
	public class TestNodeprep
	{
        private static System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private Profile nodeprep = new XmppNode();

        private void TryOne(string input, string expected)
        {
            string output = nodeprep.Prepare(input, 0);
            Assertion.AssertEquals(expected, output);
        }

        public void Test_Good()
        {
            TryOne("HILDJJ", "hildjj");
            TryOne("hildjj", "hildjj");
        }

        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Dquote()
        {
            TryOne("\"", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Amp()
        {
            TryOne("&", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Squote()
        {
            TryOne("'", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Slash()
        {
            TryOne("/", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Colon()
        {
            TryOne(":", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Less()
        {
            TryOne("<", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Greater()
        {
            TryOne(">", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_At()
        {
            TryOne("@", null);
        }
    }
}
#endif