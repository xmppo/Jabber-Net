/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
#if !NO_STRINGPREP

using System;
using NUnit.Framework;
using stringprep;
using stringprep.steps;
using bedrock.util;

namespace test.stringprep
{
    [RCS(@"$Header$")]
    [TestFixture]
	public class TestNodeprep
	{
        private static System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private Profile nodeprep = new XmppNode();

        private void TryOne(string input, string expected)
        {
            string output = nodeprep.Prepare(input);
            Assertion.AssertEquals(expected, output);
        }

        public void Test_Good()
        {
            TryOne("HILDJJ", "hildjj");
            TryOne("hildjj", "hildjj");
			TryOne("\x226f", "\x226f"); // not greater than
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
#if !NO_STRINGPREP
		[ExpectedException(typeof(ProhibitedCharacterException))]
		public void Test_SmallGreater()
		{
			TryOne("\xfe65", null); // small greater than
		}
#endif
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_At()
        {
            TryOne("@", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Prep_Less()
        {
            // U+FE68: small commercial at
            TryOne("\xFE6b", null);
        }
    }
}
#endif