/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
using System.Text;
using NUnit.Framework;
using stringprep;
using stringprep.steps;
using bedrock.util;

namespace test.stringprep
{
    [RCS(@"$Header$")]
    [TestFixture]
    public class TestNFKC
    {
        System.Text.Encoding UTF8 = System.Text.Encoding.UTF8;
        NFKCStep step = new NFKCStep();

        private void TryOne(string input, string expected)
        {
            StringBuilder i = new StringBuilder(input);
            step.Prepare(i);
            Assert.AreEqual(expected, i.ToString());
        }

        [Test] public void Test_1()
        {
            TryOne(UTF8.GetString(new byte[] {0xC2, 0xB5}), UTF8.GetString(new byte[] {0xCE, 0xBC}));
        }

        [Test] public void Test_2()
        {
            TryOne(UTF8.GetString(new byte[] {0xC2, 0xAA}), UTF8.GetString(new byte[] {0x61}));
        }

        [Test] public void Test_Normalization()
        {
            // http://www.unicode.org/unicode/faq/normalization.html
            TryOne("\x03D3", "\x038E");
        }

        [Test] public void Test_Gurmukhi()
        {
            // http://www.unicode.org/charts/normalization/
            TryOne("\x0a59", "\x0a16\x0a3c");
            TryOne("\x0a5a", "\x0a17\x0a3c");
            TryOne("\x0a5b", "\x0a1c\x0A3C");
            TryOne("\x0A5E", "\x0A2B\x0A3C");
            TryOne("\x0A33", "\x0A32\x0A3C");
            TryOne("\x0A36", "\x0A38\x0A3C");
        }

        [Test] public void Test_Unchanged()
        {
            TryOne("\x0144\x0020\x03b9", "\x0144\x0020\x03b9");
        }

        [Test] public void Test_Navajo()
        {
            // a + ogonek + acute ==> a-ogonek + acute 
            TryOne("\x0061\x0328\x0301", "\x0105\x0301");
            // i + ogonek + acute ==> i-ogonek + acute
            TryOne("i\x0328\x0301", "\x012F\x0301");
        }

        [Test] public void Test_CannonicalOrdering()
        {
            // From Unicode 3.2, section 3.10

            // The NFKC stuff does a Compose after these are specified, though.

            // a-diaeresis + underdot => a + underdot + diaeresis
            TryOne("\x00e4\x0323", "\x1ea1\x0308"); 
            TryOne("\x00e4\x0323\x00e4\x0323\x00e4\x0323", "\x1ea1\x0308\x1ea1\x0308\x1ea1\x0308"); 

            // a + diaeresis + underdot => a + underdot + diaeresis
            TryOne("a\x0308\x0323", "\x1ea1\x0308");

            // a + underdot + diaeresis => a + underdot + diaeresis
            TryOne("a\x0323\x0308", "\x1ea1\x0308");

            // a-underdot + diaeresis => a + underdot + diaeresis
            TryOne("\x1ea1\x0308", "\x1ea1\x0308");


            // a-diaeresis + breve => a + diaeresis + breve
            TryOne("\x00e4\x0306", "\x00e4\x0306");

            // a + diaeresis + breve => a + diaeresis + breve
            TryOne("a\x0308\x0306", "\x00e4\x0306");

            // a + breve + diaeresis => a + breve + diaeresis
            TryOne("a\x0306\x0308", "\x0103\x0308");

            // a-breve + diaeresis => a + breve + diaeresis
            TryOne("\x0103\x0308", "\x0103\x0308");
        }

        [Test] public void Test_TR15_Annex_1()
        {
            TryOne("\x1E0A", "\x1E0A"); //a
            TryOne("D\x0307", "\x1E0A"); //b
            TryOne("\x1E0C\x0307", "\x1E0C\x0307"); //c
            TryOne("\x1E0A\x0323", "\x1E0C\x0307"); //d
            TryOne("D\x0307\x0323", "\x1E0C\x0307"); //e
            TryOne("D\x0307\x031B\x0323", "\x1E0C\x031B\x0307"); //f
            TryOne("\x1E14", "\x1E14"); //g
            TryOne("\x0112\x0300", "\x1E14"); //h
            TryOne("\x00C8\x0304", "\x00C8\x0304"); //i
            TryOne("\x212B", "\x00C5"); //j
            TryOne("\x00C5", "\x00C5"); //k

            TryOne("Äffin", "Äffin"); //l'
            TryOne("Ä\xFB03n", "Äffin"); //m'
            TryOne("Henry IV", "Henry IV"); //n'
            TryOne("Henry \x2163", "Henry IV"); //o'
            TryOne("\x30AC", "\x30AC"); //p' ga
            TryOne("\x30AB\x3099", "\x30AC"); //q' ka + ten
            TryOne("\xFF76\xFF9E", "\x30AC"); //r' hw_ka + hw_ten
            TryOne("\x30AB\xFF9E", "\x30AC"); //s' ka + hw_ten
            TryOne("\xFF76\x3099", "\x30AC"); //t' hw_ka + ten
            // TryOne("", ""); // can't find "kaks": I think it's Hangul.
        }

        // http://www.unicode.org/review/pr-29.html
        [Test] public void Test_PR29()
        {
            //TODO: Try again with NFC, rather than NFKC.
            TryOne("\x1100\x0300\x1161", "\x1100\x0300\x1161");
        }
    }
}
#endif