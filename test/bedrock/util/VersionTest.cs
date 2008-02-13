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
using System;

using NUnit.Framework;
using bedrock.util;
namespace test.bedrock.util
{
    /// <summary>
    ///    Summary description for AssemblyXMLTest.
    /// </summary>
    [SVN(@"$Id$")]
    [TestFixture]
    public class VersionTest
    {
        [StarTeam("$" + "Header" + "$")]
            private class vFoo
        {

        }
        [StarTeam(Archive="$" + @"Header: C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs" + "$",
             Author="$" + "Author: Joe Hildebrand" + "$",
             DateString="02/12/2001 6:25:04 PM",
             Revision="1")]
            private class vInd
        {

        }
        [StarTeam("$" + @"Header: C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs, 1.11, 02/12/2001 6:25:04 PM, Joe Hildebrand" + "$")]
            private class vBar
        {

        }
        [StarTeam("$" + @"Header: C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs, 11, 02/12/2001 6:25:04 PM, Joe Hildebrand" + "$")]
            private class vBaz
        {

        }
        [RCS("$" + @"Header: /u1/html/cvsroot/www.cyclic.com/RCS-html/info-ref.html,v 1.1 2001/02/12 18:25:04 kingdon Exp " + "$")]
            private class RCSVer
        {

        }
        [SourceSafe("$" + @"Header: /t.cs 1     2/12/01 6:25p Hildebzj " + "$")]
            private class VssVer
        {

        }

        [SVN("$" + @"Id: calc.c 148 2002-07-28 21:30:43Z sally " + "$")]
        private class SvnVer
        {

        }


        [Test] public void Test_Empty()
        {
            SourceVersionAttribute foo = SourceVersionAttribute.GetVersion(typeof(vFoo));
            Assert.AreEqual(null,               foo.Revision, "Revision");
            Assert.AreEqual(DateTime.MinValue,  foo.Date,     "Date");
            Assert.AreEqual(null,               foo.Author,   "Date");
            Assert.AreEqual(null,               foo.Version,  "Version");
            Assert.AreEqual(null,               foo.Archive,  "Archive");
        }
        [Test] public void Test_Full()
        {
            SourceVersionAttribute bar = SourceVersionAttribute.GetVersion(typeof(vBar));
            Assert.AreEqual("1.11",             bar.Revision);
            Assert.AreEqual(
                new DateTime(2001, 2, 12, 18, 25, 4),
                bar.Date);
            Assert.AreEqual("Joe Hildebrand",   bar.Author);
            Assert.AreEqual(new Version(1, 11), bar.Version);
            Assert.AreEqual(@"C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs", bar.Archive);
        }
        [Test] public void Test_Ind()
        {
            SourceVersionAttribute ind = SourceVersionAttribute.GetVersion(typeof(vInd));
            Assert.AreEqual(
                "$" + @"Header: C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs, 1, 02/12/2001 6:25:04 PM, Joe Hildebrand" + "$",
                ind.ToString());
        }
        [Test] public void Test_PlainRev()
        {
            SourceVersionAttribute baz = SourceVersionAttribute.GetVersion(typeof(vBaz));
            Assert.AreEqual("11",               baz.Revision);
            Assert.AreEqual(
                new DateTime(2001, 2, 12, 18, 25, 4),
                baz.Date);
            Assert.AreEqual("Joe Hildebrand",   baz.Author);
            Assert.AreEqual(new Version(1, 11), baz.Version);
            Assert.AreEqual(@"C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs", baz.Archive);
        }
        [Test] public void Test_RCS()
        {
            RCSAttribute c = (RCSAttribute) SourceVersionAttribute.GetVersion(typeof(RCSVer));
            Assert.AreEqual("1.1",               c.Revision);
            Assert.AreEqual(
                new DateTime(2001, 2, 12, 18, 25, 4),
                c.Date);
            Assert.AreEqual("kingdon",           c.Author);
            Assert.AreEqual(new Version(1, 1),   c.Version);
            Assert.AreEqual(@"/u1/html/cvsroot/www.cyclic.com/RCS-html/info-ref.html,v", c.Archive);
            Assert.AreEqual("Exp",               c.State);
        }
        [Test] public void Test_VSS()
        {
            SourceSafeAttribute c = (SourceSafeAttribute) SourceVersionAttribute.GetVersion(typeof(VssVer));
            Assert.AreEqual("1",                 c.Revision);
            Assert.AreEqual(
                new DateTime(2001, 2, 12, 18, 25, 0),
                c.Date);
            Assert.AreEqual("Hildebzj",          c.Author);
            Assert.AreEqual(new Version(1, 1),   c.Version);
            Assert.AreEqual(@"/t.cs",            c.Archive);
        }

        [Test] public void Test_GetAll()
        {
            SourceVersionCollection tv = SourceVersionAttribute.GetVersion();
            Assert.IsTrue(tv["test.bedrock.util.VersionTest+vFoo"]   != null);
            Assert.IsTrue(tv[typeof(vBar).FullName]   != null);
            Assert.IsTrue(tv[typeof(vBaz)]   != null);
            Assert.IsTrue(tv["test.bedrock.util.VersionTest+RCSVer"] != null);
            Assert.IsTrue(tv["test.bedrock.util.VersionTest+VssVer"] != null);
            Assert.IsTrue(tv["test.bedrock.util.VersionTest+SvnVer"] != null);
            Assert.IsTrue(tv["test.bedrock.util.VersionTest+vBax"]   == null);
            foreach (string c in tv)
            {
                //Console.WriteLine("<{0}>", c);
                Assert.IsTrue(tv[c] != null);
            }
        }

        [Test]
        public void Test_SVN()
        {
            SVNAttribute c = (SVNAttribute)SourceVersionAttribute.GetVersion(typeof(SvnVer));
            Assert.AreEqual("148", c.Revision);
            Assert.AreEqual(
                new DateTime(2002, 7, 28, 21, 30, 43),
                c.Date);
            Assert.AreEqual("sally", c.Author);
            Assert.AreEqual(new Version(1, 148), c.Version);
            Assert.AreEqual(@"calc.c", c.Archive);
        }

    }
}
