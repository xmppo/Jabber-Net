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
        [Test] public void Test_Create()
        {
            Stream s = new Stream(doc, "jabber:client");
            Assert.IsTrue(
                Regex.IsMatch(s.ToString(),
                "<stream:stream id=\"[a-z0-9]+\" xmlns=\"jabber:client\" xmlns:stream=\"http://etherx\\.jabber\\.org/streams\" />",
                RegexOptions.IgnoreCase), s.ToString());
        }
        [Test] public void Test_Error()
        {
            Error err = new Error(doc);
            err.Message = "foo";
            Assert.AreEqual("<stream:error " +
                "xmlns:stream=\"http://etherx.jabber.org/streams\">foo</stream:error>", err.ToString());
            ElementFactory sf = new ElementFactory();
            sf.AddType(new fact());
            XmlQualifiedName qname = new XmlQualifiedName(err.LocalName, err.NamespaceURI);
            Element p = (Element) sf.GetElement(err.Prefix, qname, doc);
            Assert.AreEqual(typeof(Error), p.GetType());
        }
        [Test] public void Test_StartTag()
        {
            Stream s = new Stream(doc, "jabber:client");
            Assert.IsTrue(
                Regex.IsMatch(s.StartTag(),
                "<stream:stream xmlns:stream=\"http://etherx\\.jabber\\.org/streams\" id=\"[a-z0-9]+\" xmlns=\"jabber:client\">",
                RegexOptions.IgnoreCase), s.StartTag());
        }
    }
}
