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
	/// <summary>
	/// Summary description for TestGeneric.
	/// </summary>
	[TestFixture]
	public class TestGeneric
	{
        private static System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private Profile generic = new Generic();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="input">string with one UTF-8 byte per char, to enable easy cut-n-paste from the libidn tests.</param>
        /// <param name="expected"></param>
        private void TryOne(string input, string expected)
        {
            TryOne(input, expected, 0);
        }

        private void TryOne(string input, string expected, ProfileFlags flags)
        {
            byte[] buf = new byte[input.Length];
            for (int i=0; i<input.Length; i++)
            {
                buf[i] = (byte) input[i];
            }
            string in_enc = ENC.GetString(buf);
            string output = generic.Prepare(in_enc, flags);

            buf = new byte[expected.Length];
            for (int i=0; i<expected.Length; i++)
            {
                buf[i] = (byte) expected[i];
            }
            string ex = ENC.GetString(buf);

            Assertion.AssertEquals(ex, output);
        }


        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_NoSpace()
        {
            // Test of prohibited ASCII character U+0020
            TryOne("\x20", null);
        }

        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_NoNFKCSpace()
        {
            // Test of NFKC U+00A0 and prohibited character U+0020
            TryOne("\xC2\xA0", null);
        }

        public void Test_Works()
        {
            // Case map + normalization
            TryOne("\xC2\xB5", "\xCE\xBC");
        }

        public void Test_NoNFKC()
        {
            // case_nonfkc
            TryOne("\xC2\xB5", "\xCE\xBC", ProfileFlags.NO_NFKC);
        }

        public void Test_NFKC()
        {
            // NFKC test "Latin Small Letter Turned A"
            TryOne("\xC2\xAA", "a");
        }

        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Unassigned_01()
        {
            // unassigned code point U+0221
            TryOne("\xC8\xA1", null);
        }

        public void Test_Unassigned_02()
        {
            // Unassigned code point U+0221
            TryOne("\xC8\xA1", "\xC8\xA1", ProfileFlags.NO_UNASSIGNED);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        public void Test_Unassigned_03()
        {
            // Unassigned code point U+0236
            TryOne("\xC8\xB6", null);
        }
        public void Test_Unassigned_04()
        {
            // unassigned code point U+0236",
            TryOne("\xC8\xB6", "\xC8\xB6", ProfileFlags.NO_UNASSIGNED);
        }

        [ExpectedException(typeof(BidiException))]
        public void Test_Bidi()
        {
            // bidi both RandALCat and LCat  U+0627 U+00AA U+0628
            TryOne("\xD8\xA7\xC2\xAA\xD8\xA8", null);
        }

        public void Test_NoBidi()
        {
            // bidi both RandALCat and LCat  U+0627 U+00AA U+0628
            string actual = generic.Prepare("\x0627\x00aa\x0628", ProfileFlags.NO_BIDI);
            Assertion.AssertEquals("\x0627a\x0628", actual);
        }

	}
}
#endif