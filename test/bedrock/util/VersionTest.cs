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
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using NUnit.Framework;
using bedrock.util;
namespace test.bedrock.util
{
    /// <summary>
    ///    Summary description for AssemblyXMLTest.
    /// </summary>
    [RCS(@"$Header$")]
    public class VersionTest : TestCase
    {
        public VersionTest(string name) : base(name) {}
        public static ITest Suite
        {
            get { return new TestSuite(typeof(VersionTest)); }
        }
    
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
        public void Test_Empty() 
        {
            SourceVersionAttribute foo = SourceVersionAttribute.GetVersion(typeof(vFoo));
            AssertEquals("Revision", null,               foo.Revision);
            AssertEquals("Date",     DateTime.MinValue,  foo.Date);
            AssertEquals("Author",   null,               foo.Author);
            AssertEquals("Version",  null,               foo.Version);
            AssertEquals("Archive",  null,               foo.Archive);
        }
        public void Test_Full()
        {
            SourceVersionAttribute bar = SourceVersionAttribute.GetVersion(typeof(vBar));
            AssertEquals("Revision", "1.11",             bar.Revision);
            AssertEquals("Date",
                         new DateTime(2001, 2, 12, 18, 25, 4), 
                         bar.Date);
            AssertEquals("Author",   "Joe Hildebrand",   bar.Author);
            AssertEquals("Version",  new Version(1, 11), bar.Version);
            AssertEquals("Archive",  @"C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs", bar.Archive);
        }
        public void Test_Ind()
        {
            SourceVersionAttribute ind = SourceVersionAttribute.GetVersion(typeof(vInd));
            AssertEquals("ToString", 
                         "$" + @"Header: C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs, 1, 02/12/2001 6:25:04 PM, Joe Hildebrand" + "$",
                         ind.ToString());
        }
        public void Test_PlainRev()
        {
            SourceVersionAttribute baz = SourceVersionAttribute.GetVersion(typeof(vBaz));
            AssertEquals("Revision", "11",               baz.Revision);
            AssertEquals("Date",
                         new DateTime(2001, 2, 12, 18, 25, 4), 
                         baz.Date);
            AssertEquals("Author",   "Joe Hildebrand",   baz.Author);
            AssertEquals("Version",  new Version(1, 11), baz.Version);
            AssertEquals("Archive",  @"C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs", baz.Archive);
        }
        public void Test_RCS()
        {
            RCSAttribute c = (RCSAttribute) SourceVersionAttribute.GetVersion(typeof(RCSVer));
            AssertEquals("Revision", "1.1",               c.Revision);
            AssertEquals("Date",
                         new DateTime(2001, 2, 12, 18, 25, 4), 
                         c.Date);
            AssertEquals("Author",   "kingdon",           c.Author);
            AssertEquals("Version",  new Version(1, 1),   c.Version);
            AssertEquals("Archive",  @"/u1/html/cvsroot/www.cyclic.com/RCS-html/info-ref.html,v", c.Archive);
            AssertEquals("State",    "Exp",               c.State);
        }
        public void Test_VSS()
        {
            SourceSafeAttribute c = (SourceSafeAttribute) SourceVersionAttribute.GetVersion(typeof(VssVer));
            AssertEquals("Revision", "1",                 c.Revision);
            AssertEquals("Date",
                         new DateTime(2001, 2, 12, 18, 25, 0), 
                         c.Date);
            AssertEquals("Author",   "Hildebzj",          c.Author);
            AssertEquals("Version",  new Version(1, 1),   c.Version);
            AssertEquals("Archive",  @"/t.cs",            c.Archive);
        }
        
        public void Test_GetAll()
        {
            SourceVersionCollection tv = SourceVersionAttribute.GetVersion();
            Assert(tv["test.bedrock.util.VersionTest+vFoo"]   != null);
            Assert(tv[typeof(vBar).FullName]   != null);
            Assert(tv[typeof(vBaz)]   != null);
            Assert(tv["test.bedrock.util.VersionTest+RCSVer"] != null);
            Assert(tv["test.bedrock.util.VersionTest+VssVer"] != null);
            Assert(tv["test.bedrock.util.VersionTest+vBax"]   == null);
            foreach (string c in tv)
            {
                //Console.WriteLine("<{0}>", c);
                Assert(tv[c] != null);
            }
        }
    }
}
