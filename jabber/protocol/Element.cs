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
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using bedrock.util;

namespace jabber.protocol
{
    /// <summary>
    /// An XmlElement with type-safe accessors.  This class is not much use by itself,
    /// but provides a number of utility functions for its descendants.
    /// </summary>
    [SVN(@"$Id$")]
    public class Element : XmlElement
    {
        /// <summary>
        /// UTF-8 encoding used throughout.
        /// </summary>
        protected static readonly Encoding ENCODING = Encoding.UTF8;

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Element(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname.Name, qname.Namespace, doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="doc"></param>
        public Element(string localName, XmlDocument doc) :
            base("", localName, "", doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="namespaceURI"></param>
        /// <param name="doc"></param>
        public Element(string localName, string namespaceURI, XmlDocument doc) :
            base("", localName, namespaceURI, doc)
        {
        }

        /// <summary>
        /// The xml:lang of this element.
        /// </summary>
        public string Lang
        {
            get
            {
                if (!HasAttribute("lang", URI.XML))
                    return null;
                return GetAttribute("lang", URI.XML);
            }
            set
            {
                if (HasAttribute("lang", URI.XML))
                    RemoveAttribute("lang", URI.XML);
                if (value != null)
                {
                    XmlAttribute attr = OwnerDocument.CreateAttribute("xml:lang", URI.XML);
                    attr.Value = value;
                    this.Attributes.Append(attr);
                }
            }
        }

        /// <summary>
        /// Add a child element.  The element can be from a different document.
        /// </summary>
        /// <param name="value"></param>
        public void AddChild(XmlElement value)
        {
            if (this.OwnerDocument == value.OwnerDocument)
            {
                this.AppendChild(value);
            }
            else
            {
                this.AppendChild(this.OwnerDocument.ImportNode(value, true));
            }
        }

        /// <summary>
        /// Returns an XmlNodeList containing a list of child elements that match the specified localname and namespace URI.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="namespaceURI"></param>
        /// <returns></returns>
        public override XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
        {
            return new ElementList(this, localName, namespaceURI);
        }

        /// <summary>
        /// Returns an XmlNodeList containing a list of child elements that match the specified localname.
        /// </summary>
        /// <param name="localName"></param>
        /// <returns></returns>
        public override XmlNodeList GetElementsByTagName(string localName)
        {
            return new ElementList(this, localName);
        }


        /// <summary>
        /// Get the text contents of a sub-element.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetElem(string name)
        {
            XmlElement e = this[name];
            if (e == null)
                return null;
            if (!e.HasChildNodes)
                return null;
            return e.InnerText;
        }
        /// <summary>
        /// Sets the text contents of a sub-element.
        /// Note: Do not use this if you want the sub-element to have a type that is not XmlElement.
        /// Instead use <see cref="SetElem(string,string,Type)"/>
        /// </summary>
        /// <param name="name">The element tag.</param>
        /// <param name="value">The inner text of the element.</param>
        protected void SetElem(string name, string value)
        {
            XmlElement e = GetOrCreateElement(name, null, null);
            e.RemoveAll();

            if (value != null)
                e.InnerText = value;
        }

        /// <summary>
        /// Sets the text contents of a sub-element.
        /// </summary>
        /// <param name="name">The element tag.</param>
        /// <param name="value">The inner text of the element.</param>
        /// <param name="typeToCreate">If the element doesn't exist, create it with this type.  If null, then just use an XmlElement.</param>
        protected void SetElem(string name, string value, Type typeToCreate)
        {
            XmlElement e = GetOrCreateElement(name, null, typeToCreate);
            e.RemoveAll();

            if (value != null)
                e.InnerText = value;
        }

        /// <summary>
        /// If the named element exists as a child, return it.  Otherwise, gin up
        /// a new instance of the given class (which must be a subclass of XmlElement)
        /// add it as a child, and return the result.  Will often be paired with
        /// ReplaceChild as the setter.
        /// </summary>
        /// <remarks>
        /// This seems kind of around-the-barn.  Wish there was an easier way to do this,
        /// rather than having to get the constructor, and whatnot.  Hopefully it won't
        /// be called all that often, so the speed issue won't be too bad.
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="xmlns">Namespace URI.  Null to use parent's.</param>
        /// <param name="typeToCreate">If the element doesn't exist, create it with this type.  If null, then just use an XmlElement.</param>
        /// <returns></returns>
        protected XmlElement GetOrCreateElement(string name, string xmlns, Type typeToCreate)
        {
            string ns = (xmlns!=null) ? xmlns : NamespaceURI;
            XmlElement child = this[name, ns];
            if (child != null)
                return child;

            if (typeToCreate == null)
                child = this.OwnerDocument.CreateElement(name, ns);
            else
            {
                Debug.Assert(typeToCreate.IsSubclassOf(typeof(XmlElement)));

                ConstructorInfo constructor = typeToCreate.GetConstructor(new Type[] { typeof(XmlDocument) });
                Debug.Assert(constructor != null);
                child = constructor.Invoke(new object[] { this.OwnerDocument }) as XmlElement;
                Debug.Assert(child != null);
            }

            this.AppendChild(child);
            return child;
        }

        /// <summary>
        /// Replaces the first element that has the same name
        /// with the passed in element.
        /// </summary>
        /// <param name="elem">The new element</param>
        /// <returns>The replaced element</returns>
        protected XmlElement ReplaceChild(XmlElement elem)
        {
            XmlElement old = this[elem.Name, elem.NamespaceURI];
            if (old != null)
            {
                this.RemoveChild(old);
            }
            if (elem != null)
                AddChild(elem);
            return old;
        }

        /// <summary>
        /// Remove a child element
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The old element, or null if it didn't exist.</returns>
        protected XmlElement RemoveElem(string name)
        {
            XmlElement e = this[name];
            if (e != null)
                this.RemoveChild(e);
            return e;
        }
        /// <summary>
        /// Removes all of the matching elements from this element.
        /// </summary>
        /// <param name="name">Element local name</param>
        protected void RemoveElems(string name)
        {
            XmlNodeList nl = this.ChildNodes;
            foreach (XmlNode n in nl)
            {
                if (n.NodeType != XmlNodeType.Element)
                    continue;
                if (n.Name == name)
                    this.RemoveChild(n);
            }
        }
        /// <summary>
        /// Removes all of the matching elements from this element.
        /// </summary>
        /// <param name="name">Element local name</param>
        /// <param name="namespaceURI">Element namespace URI.</param>
        protected void RemoveElems(string name, string namespaceURI)
        {
            XmlNodeList nl = this.ChildNodes;
            foreach (XmlNode n in nl)
            {
                if (n.NodeType != XmlNodeType.Element)
                    continue;
                if ((n.Name == name) && (n.NamespaceURI == namespaceURI))
                    this.RemoveChild(n);
            }
        }

        /// <summary>
        /// I think GetAttribute should return null if the attribute is not found.
        /// Use carefully, when translating to external semantics.
        /// </summary>
        /// <param name="name"></param>
        protected string GetAttr(string name)
        {
            if (!HasAttribute(name))
                return null;
            return GetAttribute(name);
        }

        /// <summary>
        /// I think calling SetAttr with null or "" should remove the attribute.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetAttr(string name, string value)
        {
            if ((value == null) || (value == ""))
                // testing shows this is safe for non-existing attributes.
                RemoveAttribute(name);
            else
                SetAttribute(name, value);
        }

        /// <summary>
        /// Get the value of an attribute, as a value in the given Enum type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        protected object GetEnumAttr(string name, Type enumType)
        {
            string a = this.GetAttribute(name);
            if ((a == null) || (a.Length == 0))
                return -1;
            try
            {
                return Enum.Parse(enumType, a, true);
            }
            catch (ArgumentException)
            {
                return -1;
            }
        }

        /// <summary>
        /// Set the value of an attribute, with the value being a enum instance.
        /// The enum in question should have an entry with int value -1, which
        /// corresponds to no attribute.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetEnumAttr(string name, object value)
        {
            Debug.Assert(value.GetType().IsSubclassOf(typeof(Enum)));
            if ((int)value == -1)
                RemoveAttribute(name);
            else
                SetAttribute(name, value.ToString());
        }

        /// <summary>
        /// Get the value of a given attribute, as an integer.  Returns -1 for
        /// most errors.   TODO: should this throw exceptions?
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected int GetIntAttr(string name)
        {
            string a = this.GetAttribute(name);
            if ((a == null) || (a.Length == 0))
                return -1;
            try
            {
                return int.Parse(a);
            }
            catch (FormatException)
            {
                return -1;
            }
            catch (OverflowException)
            {
                return -1;
            }
        }
        /// <summary>
        /// Set the value of a given attribute, as an integer.  Use -1
        /// to remove the attribute.
        /// </summary>
        /// <param name="name">The attribute name</param>
        /// <param name="val">The integer to set</param>
        /// <returns></returns>
        protected void SetIntAttr(string name, int val)
        {
            if (val < 0)
                // testing shows this is safe for non-existing attributes.
                RemoveAttribute(name);
            else
                SetAttribute(name, val.ToString());
        }

        /// <summary>
        /// Get an attribute cast to DateTime, using the DateTime profile
        /// of XEP-82.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>DateTime.MinValue if attribute not found.</returns>
        /// <exception cref="FormatException">Invalid format</exception>
        protected DateTime GetDateTimeAttr(string name)
        {
            string val = GetAttr(name);
            if (val == null)
                return DateTime.MinValue;
            return DateTimeProfile(val);
        }

        /// <summary>
        /// Set with DateTime.MinValue to remove the attribute
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetDateTimeAttr(string name, DateTime value)
        {
            if (value == DateTime.MinValue)
                // testing shows this is safe for non-existing attributes.
                RemoveAttribute(name);
            else
                SetAttribute(name, DateTimeProfile(value));
        }

        /// <summary>
        /// Convert the given array of bytes into a string, having two characters
        /// for each byte, corresponding to the hex representation of that byte.
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string HexString(byte[] buf)
        {
            // it seems like there ought to be a better way to do this.
            StringBuilder sb = new StringBuilder();
            foreach (byte b in buf)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Convert the given array of bytes into a string, having two characters
        /// for each byte, corresponding to the hex representation of that byte.
        /// </summary>
        /// <param name="buf">The byte buffer</param>
        /// <param name="offset">The offset into the buffer for the start</param>
        /// <param name="length">The number of bytes to read, starting at the offset.</param>
        /// <returns></returns>
        public static string HexString(byte[] buf, int offset, int length)
        {
            // it seems like there ought to be a better way to do this.
            StringBuilder sb = new StringBuilder();
            for (int i=offset; i < length; i++)
            {
                sb.Append(buf[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Compute the SHA1 hash of the id and secret concatenated together.
        /// </summary>
        /// <param name="id">UTF8-encoded id</param>
        /// <param name="secret">UTF8-encoded secret</param>
        /// <returns></returns>
        public static string ShaHash(string id, string secret)
        {
            Debug.Assert(id != null);
            Debug.Assert(secret != null);
            SHA1 sha = SHA1.Create();
            byte[] hash = sha.ComputeHash(ENCODING.GetBytes(id + secret));
            return HexString(hash);
        }

        /// <summary>
        /// Compute a 0K hash
        /// </summary>
        /// <param name="password">The secret to hash in</param>
        /// <param name="token">The token to permute the hash</param>
        /// <param name="sequence">Number of times to hash</param>
        /// <returns></returns>
        public static string ZeroK(string password, string token, int sequence)
        {
            Debug.Assert(password != null);
            Debug.Assert(token != null);
            SHA1 sha = SHA1.Create();
            string hash = HexString(sha.ComputeHash(ENCODING.GetBytes(password)));
            hash = HexString(sha.ComputeHash(ENCODING.GetBytes(hash + token)));
            for (int i = 0; i < sequence; i++)
            {
                hash = HexString(sha.ComputeHash(ENCODING.GetBytes(hash)));
            }
            return hash;
        }

        /// <summary>
        /// Return a DateTime version of the given Jabber date.  Example date: 20020504T20:39:42
        /// </summary>
        /// <param name="dt">The pseudo-ISO-8601 formatted date (no milliseconds)</param>
        /// <returns>A (usually UTC) DateTime</returns>
        public static DateTime JabberDate(string dt)
        {
            if ((dt == null) || (dt == ""))
                return DateTime.MinValue;
            try
            {
                return new DateTime(int.Parse(dt.Substring(0, 4)),
                                    int.Parse(dt.Substring(4, 2)),
                                    int.Parse(dt.Substring(6, 2)),
                                    int.Parse(dt.Substring(9,2)),
                                    int.Parse(dt.Substring(12,2)),
                                    int.Parse(dt.Substring(15,2)));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// Get a jabber-formated date for the DateTime.   Example date: 20020504T20:39:42
        /// </summary>
        /// <param name="dt">The (usually UTC) DateTime to format</param>
        /// <returns>The pseudo-ISO-8601 formatted date (no milliseconds)</returns>
        public static string JabberDate(DateTime dt)
        {
            return string.Format("{0:yyyy}{0:MM}{0:dd}T{0:HH}:{0:mm}:{0:ss}", dt);
        }

        /// <summary>
        /// XEP-82 Date/Time profile: http://www.xmpp.org/extensions/xep-0082.html#sect-id2601974
        /// CCYY-MM-DDThh:mm:ss[.sss]TZD
        /// 1969-07-21T02:56:15Z
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime DateTimeProfile(string dt)
        {
            string[] fmts =
            {
                "yyyy-MM-dd",
                "yyyy-MM-ddTHH:mm:sszzz",
                "yyyy-MM-ddTHH:mm:ss.fffzzz",
                "HH:mm:ss",
                "HH:mm:ss.fff",
                "HH:mm:sszzz",
                "HH:mm:ss.fffzzz",
            };
            string arg = dt.Replace("Z", "+00:00");
            return DateTime.ParseExact(arg, fmts, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
        }

        /// <summary>
        /// XEP-82 Date/Time profile: http://www.xmpp.org/extensions/xep-0082.html#sect-id2601974
        /// CCYY-MM-DDThh:mm:ss[.sss]TZD
        /// 1969-07-21T02:56:15Z
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeProfile(DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        /// <summary>
        /// The XML for the packet.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.OuterXml;
        }

        /// <summary>
        /// Return just the start tag for the element.
        /// </summary>
        /// <returns></returns>
        public string StartTag()
        {
            StringBuilder sb = new StringBuilder("<");
            sb.Append(this.Name);
            if (this.NamespaceURI != null)
            {
                sb.Append(" xmlns");
                if (this.Prefix != null)
                {
                    sb.Append(":");
                    sb.Append(this.Prefix);
                }
                sb.Append("=\"");
                sb.Append(this.NamespaceURI);
                sb.Append("\"");
            }
            foreach (XmlAttribute attr in this.Attributes)
            {
                sb.Append(" ");
                sb.Append(attr.Name);
                sb.Append("=\"");
                sb.Append(attr.Value);
                sb.Append("\"");
            }
            sb.Append(">");
            return sb.ToString();
        }

        /// <summary>
        /// Get the first child element of this element.
        /// </summary>
        /// <returns>null if none found.</returns>
        public XmlElement GetFirstChildElement()
        {
            foreach (XmlNode n in this)
            {
                if (n.NodeType == XmlNodeType.Element)
                    return (XmlElement) n;
            }
            return null;
        }

        private static readonly Type[] s_constructor_parms =
            new Type[]
            {
                typeof(string),
                typeof(XmlQualifiedName),
                typeof(XmlDocument)
            };

        /// <summary>
        /// Clone this node, preserving type information.
        /// </summary>
        /// <param name="deep">Clone child nodes too?</param>
        /// <returns>Cloned node, with type info intact</returns>
        public override XmlNode CloneNode(bool deep)
        {
            return CloneNode(deep, this.OwnerDocument);
        }

        /// <summary>
        /// Clone this node into the target document, preserving type information.
        /// </summary>
        /// <param name="deep"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlNode CloneNode(bool deep, XmlDocument doc)
        {
            ConstructorInfo ci = this.GetType().GetConstructor(s_constructor_parms);
            if (ci == null)
                return doc.ImportNode(this, deep);
            if (ci.DeclaringType != this.GetType())
            {
                Debug.WriteLine("Bad type: " + ci.DeclaringType.ToString());
            }
            XmlElement el = (Element)ci.Invoke(new object[] { this.Prefix, new XmlQualifiedName(this.LocalName, this.NamespaceURI), doc });
            if (el.GetType() != this.GetType())
            {
                Debug.Assert(el.GetType() == this.GetType());
            }

            if (el.IsEmpty != this.IsEmpty)
                el.IsEmpty = this.IsEmpty;


            if (this.HasAttributes)
            {
                foreach (XmlAttribute attr in this.Attributes)
                    el.Attributes.Append((XmlAttribute)doc.ImportNode(attr, true));
            }

            if (deep)
            {
                foreach (XmlNode n in this.ChildNodes)
                {
                    if (n is Element)
                    {
                        el.AppendChild(((Element)n).CloneNode(deep, doc));
                    }
                    else
                    {
                        doc.ImportNode(n, deep);
                    }
                }
            }
            return el;

        }

        /// <summary>
        /// System-wide one-up counter, for numbering packets.
        /// </summary>
        static int s_counter = 0;
        /// <summary>
        /// Reset the packet ID counter.  This is ONLY to be used for test cases!   No locking!
        /// </summary>
        [Conditional("DEBUG")]
        public static void ResetID()
        {
            s_counter = 0;
        }

        /// <summary>
        /// Increment the ID counter, and get the new value.
        /// </summary>
        /// <returns>The new ID.</returns>
        public static string NextID()
        {
            System.Threading.Interlocked.Increment(ref s_counter);
            return "JN_" + s_counter.ToString();
        }
    }
}
