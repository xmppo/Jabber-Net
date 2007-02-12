/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
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
using stringprep.unicode;

namespace test.stringprep
{
    /// <summary>
    /// Summary description for TestGeneric.
    /// </summary>
    [TestFixture]
    public class TestDecompose
    {
        public void Test_Decomp()
        {
            char[] d = Decompose.Find('\x2000');
            Assertion.AssertNotNull(d);
            Assertion.AssertEquals(1, d.Length);
            Assertion.AssertEquals('\x0020', d[0]);
        }
    }
}
#endif
