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
using System.Xml;
using System.Text.RegularExpressions;
using NUnit.Framework;

using bedrock.util;
using jabber.protocol;
using jabber.protocol.stream;
using fact = jabber.protocol.stream.Factory;

namespace test.jabber.protocol.stream
{
    /// <summary>
    /// Summary description for StreamTest.
    /// </summary>
    [RCS(@"$Header$")]
    [TestFixture]
    public class StreamTest
    {
        XmlDocument doc = new XmlDocument();
        public void Test_Create()
        {
            Stream s = new Stream(doc, "jabber:client");
            Assertion.Assert(s.ToString(), 
                   Regex.IsMatch(s.ToString(), 
                                 "<stream:stream id=\"[a-z0-9]+\" xmlns=\"jabber:client\" xmlns:stream=\"http://etherx\\.jabber\\.org/streams\" />",
                                 RegexOptions.IgnoreCase));
        }
        public void Test_Error()
        {
            Error err = new Error(doc);
            err.Message = "foo";
            Assertion.AssertEquals("<stream:error " + 
                "xmlns:stream=\"http://etherx.jabber.org/streams\">foo</stream:error>", err.ToString());
            ElementFactory sf = new ElementFactory();
            sf.AddType(new fact());
            XmlQualifiedName qname = new XmlQualifiedName(err.LocalName, err.NamespaceURI);
            Element p = (Element) sf.GetElement(err.Prefix, qname, doc);
            Assertion.AssertEquals(typeof(Error), p.GetType());
        }
        public void Test_StartTag()
        {
            Stream s = new Stream(doc, "jabber:client");
            Assertion.Assert(s.StartTag(), 
                   Regex.IsMatch(s.StartTag(), 
                                 "<stream:stream xmlns:stream=\"http://etherx\\.jabber\\.org/streams\" id=\"[a-z0-9]+\" xmlns=\"jabber:client\">",
                                 RegexOptions.IgnoreCase));
        }
    }
}
