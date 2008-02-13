/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
    [SVN(@"$Id$")]
    [TestFixture]
    public class TestNameprep
    {
        private static System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private Profile nameprep = new Nameprep();

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="input">string with one UTF-8 byte per char, to enable easy cut-n-paste from the libidn tests.</param>
        /// <param name="expected"></param>
        private void TryOne(string input, string expected)
        {
            byte[] buf = new byte[input.Length];
            for (int i=0; i<input.Length; i++)
            {
                buf[i] = (byte) input[i];
            }
            string in_enc = ENC.GetString(buf);
            string output = nameprep.Prepare(in_enc);

            buf = new byte[expected.Length];
            for (int i=0; i<expected.Length; i++)
            {
                buf[i] = (byte) expected[i];
            }
            string ex = ENC.GetString(buf);

            Assert.AreEqual(ex, output);
        }

        [Test] public void Test_NFKC_CaseFold()
        {
            string result = nameprep.Prepare("Henry \x2163");
            Assert.AreEqual("henry iv", result);
        }

        [Test] public void Test_MapToNothing()
        {
            TryOne(
                "foo\xC2\xAD\xCD\x8F\xE1\xA0\x86\xE1\xA0\x8B" +
                "bar" + "\xE2\x80\x8B\xE2\x81\xA0" + "baz\xEF\xB8\x80\xEF\xB8\x88" +
                "\xEF\xB8\x8F\xEF\xBB\xBF", "foobarbaz");
        }

        [Test] public void Test_CaseFolding_01()
        {
            // Case folding ASCII U+0043 U+0041 U+0046 U+0045
            TryOne("CAFE", "cafe");
        }

        [Test] public void Test_CaseFolding_02()
        {
            // Case folding 8bit U+00DF (german sharp s)
            TryOne("\xC3\x9F", "ss");
        }

        [Test] public void Test_CaseFolding_03()
        {
            // Case folding U+0130 (turkish capital I with dot)
            TryOne("\xC4\xB0", "i\xcc\x87");
        }

        [Test] public void Test_CaseFolding_04()
        {
            // Case folding multibyte U+0143 U+037A
            TryOne("\xC5\x83\xCD\xBA", "\xC5\x84 \xCE\xB9");
        }

        [Ignore("fails, due to lack of UTF-16 in .Net")]
        [Test] public void Test_CaseFolding_05()
        {
            // Case folding U+2121 U+33C6 U+1D7BB
            TryOne("\xE2\x84\xA1\xE3\x8F\x86\xF0\x9D\x9E\xBB",
                "telc\xE2\x88\x95" + "kg\xCF\x83");
        }

        [Test] public void Test_CaseFolding_06()
        {
            // Normalization of U+006a U+030c U+00A0 U+00AA
            TryOne("\x6A\xCC\x8C\xC2\xA0\xC2\xAA", "\xC7\xB0 a");
        }

        [Test] public void Test_CaseFolding_07()
        {
            // Case folding U+1FB7 and normalization
            TryOne("\xE1\xBE\xB7", "\xE1\xBE\xB6\xCE\xB9");
        }

        [Test] public void Test_CaseFolding_08()
        {
            // Self-reverting case folding U+01F0 and normalization
            TryOne("\xC7\xB0", "\xC7\xB0");
        }

        [Test] public void Test_CaseFolding_09()
        {
            // Self-reverting case folding U+0390 and normalization
            TryOne("\xCE\x90", "\xCE\x90");
        }

        [Test] public void Test_CaseFolding_10()
        {
            // Self-reverting case folding U+03B0 and normalization
            TryOne("\xCE\xB0", "\xCE\xB0");
        }

        [Test] public void Test_CaseFolding_11()
        {
            // Self-reverting case folding U+1E96 and normalization
            TryOne("\xE1\xBA\x96", "\xE1\xBA\x96");
        }

        [Test] public void Test_CaseFolding_12()
        {
            // Self-reverting case folding U+1F56 and normalization
            TryOne("\xE1\xBD\x96", "\xE1\xBD\x96");
        }

        [Test] public void Test_Space_01()
        {
            // ASCII space character U+0020
            TryOne("\x20", "\x20");
        }

        [Test] public void Test_Space_02()
        {
            // Non-ASCII 8bit space character U+00A0
            TryOne("\xC2\xA0", "\x20");
        }

        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Space_03()
        {
            // Non-ASCII multibyte space character U+1680",
            TryOne("\xE1\x9A\x80", null);
        }

        [Test] public void Test_Space_04()
        {
            // Non-ASCII multibyte space character U+2000",
            TryOne("\xE2\x80\x80", "\x20");
        }

        [Test] public void Test_Space_05()
        {
            // Zero Width Space U+200b",
            TryOne("\xE2\x80\x8b", "");
        }

        [Test] public void Test_Space_06()
        {
            // Non-ASCII multibyte space character U+3000",
            TryOne("\xE3\x80\x80", "\x20");
        }

        [Test] public void Test_Control_01()
        {
            // ASCII control characters U+0010 U+007F",
            TryOne("\x10\x7F", "\x10\x7F");
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Control_02()
        {
            // Non-ASCII 8bit control character U+0085",
            TryOne("\xC2\x85", null);
        }

        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Control_03()
        {
            // Non-ASCII multibyte control character U+180E",
            TryOne("\xE1\xA0\x8E", null);
        }
        [Test] public void Test_Control_04()
        {
            // Zero Width No-Break Space U+FEFF",
            TryOne("\xEF\xBB\xBF", "");
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Control_05()
        {
            // Non-ASCII control character U+1D175",
            TryOne("\xF0\x9D\x85\xB5", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Plane_0_Private()
        {
            // Plane 0 private use character U+F123",
            TryOne("\xEF\x84\xA3", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Plane_15_Private()
        {
            // Plane 15 private use character U+F1234",
            TryOne("\xF3\xB1\x88\xB4", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Plane_16_Private()
        {
            // Plane 16 private use character U+10F234",
            TryOne("\xF4\x8F\x88\xB4", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_NonCharacter_01()
        {
            // Non-character code point U+8FFFE",
            TryOne("\xF2\x8F\xBF\xBE", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_NonCharacter_02 ()
        {
            // Non-character code point U+10FFFF",
            TryOne("\xF4\x8F\xBF\xBF", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Surrogate()
        {
            // Surrogate code U+DF42",
            //TryOne("\xED\xBD\x82", null);
            string input = "\uDF42";
            string output = nameprep.Prepare(input);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_NonPlaintext()
        {
            // Non-plain text character U+FFFD",
            TryOne("\xEF\xBF\xBD", null);
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_IdeographicDescription()
        {
            // Ideographic description character U+2FF5",
            TryOne("\xE2\xBF\xB5", null);
        }
        [Test] public void Test_Property()
        {
            // Display property character U+0341",
            TryOne("\xCD\x81", "\xCC\x81");
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_LTR ()
        {
            // Left-to-right mark U+200E",
            TryOne("\xE2\x80\x8E", "\xCC\x81");
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Deprecated()
        {
            // Deprecated U+202A",
            TryOne("\xE2\x80\xAA", "\xCC\x81");
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Tagging_01()
        {
            // Language tagging character U+E0001",
            TryOne("\xF3\xA0\x80\x81", "\xCC\x81");
        }
        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Tagging_02()
        {
            // Language tagging character U+E0042",
            TryOne("\xF3\xA0\x81\x82", null);
        }
        [ExpectedException(typeof(BidiException))]
        [Test] public void Test_Bidi_01()
        {
            // Bidi: RandALCat character U+05BE and LCat characters",
            TryOne("foo\xD6\xBE" + "bar", null);
        }
        [ExpectedException(typeof(BidiException))]
        [Test] public void Test_Bidi_02()
        {
            // Bidi: RandALCat character U+FD50 and LCat characters",
            TryOne("foo\xEF\xB5\x90" + "bar", null);
        }
        [Test] public void Test_Bidi_03()
        {
            // Bidi: RandALCat character U+FB38 and LCat characters",
            TryOne("foo\xEF\xB9\xB6" + "bar", "foo \xd9\x8e" + "bar");
        }
        [ExpectedException(typeof(BidiException))]
        [Test] public void Test_Bidi_04()
        {
            // Bidi: RandALCat without trailing RandALCat U+0627 U+0031",
            TryOne("\xD8\xA7\x31", null);
        }

        [Test] public void Test_Bidi_05()
        {
            // Bidi: RandALCat character U+0627 U+0031 U+0628",
            TryOne("\xD8\xA7\x31\xD8\xA8", "\xD8\xA7\x31\xD8\xA8");
        }

        [ExpectedException(typeof(ProhibitedCharacterException))]
        [Test] public void Test_Unassigned()
        {
            // Unassigned code point U+E0002",
            TryOne("\xF3\xA0\x80\x82", null);
        }

        [Test] public void Test_LargerShrinking()
        {
            // Larger test (shrinking)",
            //  TryOne("X\xC2\xAD\xC3\xDF\xC4\xB0\xE2\x84\xA1\x6a\xcc\x8c\xc2\xa0\xc2" + "\xaa\xce\xb0\xe2\x80\x80",
            //         "xssi\xcc\x87" + "tel\xc7\xb0 a\xce\xb0 ");
            TryOne("X\xC2\xAD\xC3\x9F\xC4\xB0\xE2\x84\xA1\x6a\xcc\x8c\xc2\xa0\xc2" + "\xaa\xce\xb0\xe2\x80\x80",
                "xssi\xcc\x87" + "tel\xc7\xb0 a\xce\xb0 ");

        }
        [Test] public void Test_LargerExpanding()
        {
            // Larger test (expanding)",
            TryOne("X\xC3\x9F\xe3\x8c\x96\xC4\xB0\xE2\x84\xA1\xE2\x92\x9F\xE3\x8c\x80",
                "xss\xe3\x82\xad\xe3\x83\xad\xe3\x83\xa1\xe3\x83\xbc\xe3\x83\x88" +
                "\xe3\x83\xab" + "i\xcc\x87" + "tel\x28" + "d\x29\xe3\x82\xa2\xe3\x83\x91" +
                "\xe3\x83\xbc\xe3\x83\x88");
        }

        [Test] public void Test_OldBug()
        {
            // nameprep, exposed a bug in libstringprep 0.0.5",
            TryOne("\xC2\xAA\x0A", "\x61\x0A");
        }

        [Test] public void Test_DomainName()
        {
            TryOne("ExAmPle.COM", "example.com");
        }
    }

}
#endif
