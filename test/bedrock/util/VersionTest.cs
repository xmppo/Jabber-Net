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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
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
        public void Test_Empty() 
        {
            SourceVersionAttribute foo = SourceVersionAttribute.GetVersion(typeof(vFoo));
            Assertion.AssertEquals("Revision", null,               foo.Revision);
            Assertion.AssertEquals("Date",     DateTime.MinValue,  foo.Date);
            Assertion.AssertEquals("Author",   null,               foo.Author);
            Assertion.AssertEquals("Version",  null,               foo.Version);
            Assertion.AssertEquals("Archive",  null,               foo.Archive);
        }
        public void Test_Full()
        {
            SourceVersionAttribute bar = SourceVersionAttribute.GetVersion(typeof(vBar));
            Assertion.AssertEquals("Revision", "1.11",             bar.Revision);
            Assertion.AssertEquals("Date",
                new DateTime(2001, 2, 12, 18, 25, 4), 
                bar.Date);
            Assertion.AssertEquals("Author",   "Joe Hildebrand",   bar.Author);
            Assertion.AssertEquals("Version",  new Version(1, 11), bar.Version);
            Assertion.AssertEquals("Archive",  @"C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs", bar.Archive);
        }
        public void Test_Ind()
        {
            SourceVersionAttribute ind = SourceVersionAttribute.GetVersion(typeof(vInd));
            Assertion.AssertEquals("ToString", 
                "$" + @"Header: C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs, 1, 02/12/2001 6:25:04 PM, Joe Hildebrand" + "$",
                ind.ToString());
        }
        public void Test_PlainRev()
        {
            SourceVersionAttribute baz = SourceVersionAttribute.GetVersion(typeof(vBaz));
            Assertion.AssertEquals("Revision", "11",               baz.Revision);
            Assertion.AssertEquals("Date",
                new DateTime(2001, 2, 12, 18, 25, 4), 
                baz.Date);
            Assertion.AssertEquals("Author",   "Joe Hildebrand",   baz.Author);
            Assertion.AssertEquals("Version",  new Version(1, 11), baz.Version);
            Assertion.AssertEquals("Archive",  @"C:\Prj\Dognapper\Microsoft\Common\src\com.ilg.Util\Test\VersionTest.cs", baz.Archive);
        }
        public void Test_RCS()
        {
            RCSAttribute c = (RCSAttribute) SourceVersionAttribute.GetVersion(typeof(RCSVer));
            Assertion.AssertEquals("Revision", "1.1",               c.Revision);
            Assertion.AssertEquals("Date",
                new DateTime(2001, 2, 12, 18, 25, 4), 
                c.Date);
            Assertion.AssertEquals("Author",   "kingdon",           c.Author);
            Assertion.AssertEquals("Version",  new Version(1, 1),   c.Version);
            Assertion.AssertEquals("Archive",  @"/u1/html/cvsroot/www.cyclic.com/RCS-html/info-ref.html,v", c.Archive);
            Assertion.AssertEquals("State",    "Exp",               c.State);
        }
        public void Test_VSS()
        {
            SourceSafeAttribute c = (SourceSafeAttribute) SourceVersionAttribute.GetVersion(typeof(VssVer));
            Assertion.AssertEquals("Revision", "1",                 c.Revision);
            Assertion.AssertEquals("Date",
                new DateTime(2001, 2, 12, 18, 25, 0), 
                c.Date);
            Assertion.AssertEquals("Author",   "Hildebzj",          c.Author);
            Assertion.AssertEquals("Version",  new Version(1, 1),   c.Version);
            Assertion.AssertEquals("Archive",  @"/t.cs",            c.Archive);
        }
        
        public void Test_GetAll()
        {
            SourceVersionCollection tv = SourceVersionAttribute.GetVersion();
            Assertion.Assert(tv["test.bedrock.util.VersionTest+vFoo"]   != null);
            Assertion.Assert(tv[typeof(vBar).FullName]   != null);
            Assertion.Assert(tv[typeof(vBaz)]   != null);
            Assertion.Assert(tv["test.bedrock.util.VersionTest+RCSVer"] != null);
            Assertion.Assert(tv["test.bedrock.util.VersionTest+VssVer"] != null);
            Assertion.Assert(tv["test.bedrock.util.VersionTest+vBax"]   == null);
            foreach (string c in tv)
            {
                //Console.WriteLine("<{0}>", c);
                Assertion.Assert(tv[c] != null);
            }
        }
    }
}