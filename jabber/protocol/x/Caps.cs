using System;
using System.Xml;

using bedrock.util;


namespace jabber.protocol.x
{
    /// <summary>
    /// Entity Capabilities.  See http://www.xmpp.org/extensions/xep-0115.html.
    /// </summary>
    [SVN(@"$Id$")]
    public class Caps : Element
    {
        private static readonly char[] SPLIT = " ".ToCharArray();

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
        /// The hash type being used, or null for pre-v1.5 of XEP-115.
        /// </summary>
        public string Hash
        {
            get { return GetAttribute("hash"); }
            set { SetAttribute("hash", value); }
        }

        /// <summary>
        /// The extensions currently on in the entity.
        /// </summary>
        public string[] Extensions
        {
            get { return GetAttribute("ext").Split(SPLIT); }
            set 
            {
                if (value.Length == 0)
                {
                    if (this.HasAttribute("ext"))
                        RemoveAttribute("ext");
                }
                else
                    SetAttribute("ext", string.Join(" ", value)); 
            }
        }

        /// <summary>
        /// All of the combinations of node#ver, node#ext.
        /// </summary>
        public string[] DiscoInfoNodes
        {
            get
            {
                string[] exts = Extensions;
                string[] nodes = new string[exts.Length + 1];
                int count = 0;
                nodes[count] = Node + "#" + Version;
                foreach (string ext in exts)
                {
                    nodes[++count] = Node + "#" + ext;
                }

                return nodes;
            }
        }
    }
}
