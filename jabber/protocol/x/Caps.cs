using System;
using System.Xml;

using bedrock.util;


namespace jabber.protocol.x
{
    /// <summary>
    /// Entity Capabilities.  <see cref="http://www.xmpp.org/extensions/xep-0115.html"/>
    /// </summary>
    [SVN(@"$Id$")]
    public class Caps : Element
    {
        private static readonly char[] SPLIT = new char[] { ' ' };

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Caps(XmlDocument doc)
            : base("c", URI.CAPS, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Caps(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The URI that describes the entity.
        /// </summary>
        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        /// <summary>
        /// The version of the entity.
        /// </summary>
        public string Version
        {
            get { return GetAttribute("ver"); }
            set { SetAttribute("ver", value); }
        }

        /// <summary>
        /// The extensions currently on in the entity.
        /// </summary>
        public string[] Extensions
        {
            get { return GetAttribute("ext").Split(SPLIT, StringSplitOptions.RemoveEmptyEntries); }
            set { SetAttribute("ext", string.Join(" ", value)); }
        }
    }
}
