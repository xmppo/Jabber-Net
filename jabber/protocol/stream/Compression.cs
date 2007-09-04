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

using bedrock.util;

namespace jabber.protocol.stream
{
    /// <summary>
    /// XEP-138 compression.
    /// </summary>
    [SVN(@"$Id$")]    
	public class Compression : Element
    {
        /// <summary>
        /// Create a new compression feature element.
        /// </summary>
        /// <param name="doc"></param>
        public Compression(XmlDocument doc) :
            base("", new XmlQualifiedName("compression", jabber.protocol.URI.COMPRESS_FEATURE), doc)
        {
        }

        /// <summary>
        /// Create a new compression element.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Compression(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The list of methods offered by the server.  Typically today, this will be one string: "zlib".
        /// </summary>
        public string[] Methods
        {
            get
            {
                XmlNodeList nl = GetElementsByTagName("method", URI.COMPRESS_FEATURE);
                string[] meths = new string[nl.Count];
                int i=0;
                foreach (XmlElement m in nl)
                {
                    meths[i] = m.InnerText;
                    i++;
                }
                return meths;
            }
            set
            {
                this.RemoveElems("method", URI.COMPRESS_FEATURE);
                foreach (string m in value)
                    SetElem("method", m);
            }
        }

        /// <summary>
        /// Does this compression element have the given method in it?
        /// </summary>
        /// <param name="method">The method to search for.  Typically: "zlib"</param>
        /// <returns></returns>
        public bool HasMethod(string method)
        {
            foreach (XmlElement meth in GetElementsByTagName("method", URI.COMPRESS_FEATURE))
            {
                if (meth.InnerText == method)
                    return true;
            }
            return false;
        }
	}

    /// <summary>
    /// XEP-138 compression failure.
    /// </summary>
    [SVN(@"$Id$")]
    public class CompressionFailure : Element
    {
        /// <summary>
        /// Create a new compression element.
        /// </summary>
        /// <param name="doc"></param>
        public CompressionFailure(XmlDocument doc) :
            base("", new XmlQualifiedName("failure", jabber.protocol.URI.COMPRESS), doc)
        {
        }

        /// <summary>
        /// Create a new compression element.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public CompressionFailure(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The error described by this failure.  One of:
        /// setup-failed
        /// processing-failed
        /// unsupported-method
        /// </summary>
        public string Error
        {
            get { return GetFirstChildElement().Name; }
            set
            {
                this.RemoveAll();
                this.AddChild(this.OwnerDocument.CreateElement(value));
            }
        }
    }

    /// <summary>
    /// XEP-138 compression start.
    /// </summary>
    [SVN(@"$Id$")]
    public class Compress : Element
    {
        /// <summary>
        /// Create a new compress element.
        /// </summary>
        /// <param name="doc"></param>
        public Compress(XmlDocument doc) :
            base("", new XmlQualifiedName("compress", jabber.protocol.URI.COMPRESS), doc)
        {
        }

        /// <summary>
        /// Create a new compress element.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Compress(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// What compression method should be used?
        /// </summary>
        public string Method
        {
            get { return GetElem("method"); }
            set { SetElem("method", value); }
        }
    }


    /// <summary>
    /// XEP-138 compression success.
    /// </summary>
    [SVN(@"$Id$")]
    public class Compressed : Element
    {
        /// <summary>
        /// Create a new compression element.
        /// </summary>
        /// <param name="doc"></param>
        public Compressed(XmlDocument doc) :
            base("", new XmlQualifiedName("compressed", jabber.protocol.URI.COMPRESS), doc)
        {
        }

        /// <summary>
        /// Create a new compression element.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Compressed(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }
}
