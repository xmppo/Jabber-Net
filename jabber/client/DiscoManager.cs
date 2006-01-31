/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Diagnostics;

using bedrock.util;
using bedrock.collections;

using jabber.protocol.client;
using jabber.protocol.iq;

namespace jabber.client
{
    internal class Ident
    {
        public string name;
        public string category;
        public string type;
        public string GetKey()
        {
            string key = "";
            if (category != null)
                key = category;
            if (type != null)
                key = key + "/" + type;
            return key;
        }
    }

    /// <summary>
    /// A JID/Node combination.
    /// </summary>
    public class JIDNode
    {
        private JID m_jid = null;
        private string m_node = null;

        /// <summary>
        /// A JID/Node combination.
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="node"></param>
        public JIDNode(JID jid, string node)
        {
            if (jid == null)
                throw new ArgumentException("JID may not be null", "jid");
            this.m_jid = jid;
            if ((node != null) && (node != ""))
                this.m_node = node;
        }

        /// <summary>
        /// The JID.
        /// </summary>
        public JID JID
        {
            get { return m_jid; }
        }

        /// <summary>
        /// The Node.
        /// </summary>
        public string Node
        {
            get { return m_node; }
        }

        /// <summary>
        /// Get a hash key that combines the jid and the node.
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        protected static string GetKey(string jid, string node)
        {
            if ((node == null) || (node == ""))
                return jid.ToString();
            return jid + '\u0000' + node;
        }

        /// <summary>
        /// A JID/Node key for Hash lookup.
        /// </summary>
        public string Key
        {
            get { return GetKey(m_jid, m_node); }
        }

        /// <summary>
        /// Are we equal to that other jid/node.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            DiscoNode other = obj as DiscoNode;
            if (other == null)
            {
                return false;
            }

            return (m_jid == other.m_jid) && (m_node == other.m_node);
        }

        /// <summary>
        /// Hash the JID and node together, just in case.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            Debug.Assert(m_jid != null);
            int code = m_jid.GetHashCode();
            if (m_node != null)
                code ^= m_node.GetHashCode();
            return code;
        }
    }


    /// <summary>
    /// The info and children of a given JID/Node combination.
    /// 
    /// Note: if you have multiple connections in the same process, they all share the same Disco cache.
    /// This works fine in the real world today, since I don't know of any implementations that return different
    /// disco for different requestors, but it is completely legal protocol to have done so.
    /// </summary>
    [RCS(@"$Header$")]
    public class DiscoNode : JIDNode, IEnumerable
    {
        private static Tree m_items = new Tree();

        /// <summary>
        /// Children of this node.
        /// </summary>
        public Set Children = null;
        /// <summary>
        /// Features of this node.
        /// </summary>
        public Set Features = null;
        /// <summary>
        /// Identities of this node.
        /// </summary>
        public Set Identity = null;
        private string m_name = null;

        /// <summary>
        /// Create a disco node.
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="node"></param>
        public DiscoNode(JID jid, string node)
            : base(jid, node)
        {
        }

        /// <summary>
        /// Features are now available
        /// </summary>
        public event DiscoNodeHandler OnFeatures;
        /// <summary>
        /// New children are now available.
        /// </summary>
        public event DiscoNodeHandler OnItems;
        /// <summary>
        /// New identities are available.
        /// </summary>
        public event DiscoNodeHandler OnIdentities;

        /// <summary>
        /// The human-readable string from the first identity.
        /// </summary>
        public string Name
        {
            set { m_name = value; }
            get
            {
                if (m_name != null)
                    return m_name;
                if (Identity != null)
                {
                    foreach (Ident id in Identity)
                    {
                        if ((id.name != null) && (id.name != ""))
                            m_name = id.name;
                    }
                    return m_name;
                }
                string n = JID;
                if (Node != null)
                    n += "/" + Node;
                return n;
            }
        }

        /// <summary>
        /// Factory to create nodes and ensure that they are cached
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DiscoNode GetNode(JID jid, string node)
        {
            string key = GetKey(jid, node);
            DiscoNode n = (DiscoNode)m_items[key];
            if (n == null)
            {
                n = new DiscoNode(jid, node);
                m_items.Add(key, n);
            }
            return n;
        }

        /// <summary>
        /// Factory to create nodes, where the node is null, and only the JID is specified.
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public static DiscoNode GetNode(JID jid)
        {
            return GetNode(jid, null);
        }

        /// <summary>
        /// Delete the cache.
        /// </summary>
        public static void Clear()
        {
            m_items.Clear();
        }

        /// <summary>
        /// Does this node have the specified feature?
        /// </summary>
        /// <param name="URI"></param>
        /// <returns></returns>
        public bool HasFeature(string URI)
        {
            if (Features == null)
                return false;
            return Features.Contains(URI);
        }

        /// <summary>
        /// Add these features to the node. Fires OnFeatures.
        /// </summary>
        /// <param name="features"></param>
        public void AddFeatures(DiscoFeature[] features)
        {
            if (Features == null)
                Features = new Set();
            foreach (DiscoFeature f in features)
                Features.Add(f.Var);
            if (OnFeatures != null)
            {
                OnFeatures(this);
                OnFeatures = null;
            }
        }

        /// <summary>
        /// Add these identities to the node.
        /// </summary>
        /// <param name="ids"></param>
        public void AddIdentities(DiscoIdentity[] ids)
        {
            if (Identity == null)
                Identity = new Set();
            foreach (DiscoIdentity id in ids)
            {
                Ident i = new Ident();
                i.name = id.Named;
                i.category = id.Category;
                i.type = id.Type;
                Identity.Add(i);
            }
        }

        public void AddItems(DiscoItem[] items)
        {
            if (Children == null)
                Children = new Set();
            foreach (DiscoItem di in items)
            {
                DiscoNode dn = GetNode(di.Jid, di.Node);
                if ((di.Named != null) && (di.Named != ""))
                    dn.Name = di.Named;
                Children.Add(dn);
            }
            if (OnItems != null)
            {
                OnItems(this);
                OnItems = null;
            }
        }

        public IQ InfoIQ(System.Xml.XmlDocument doc)
        {
            DiscoInfoIQ iiq = new DiscoInfoIQ(doc);
            iiq.To = JID;
            iiq.Type = IQType.get;
            if (Node != null)
            {
                DiscoInfo info = (DiscoInfo)iiq.Query;
                info.Node = Node;
            }
            return iiq;
        }

        public IQ ItemsIQ(System.Xml.XmlDocument doc)
        {
            DiscoItemsIQ iiq = new DiscoItemsIQ(doc);
            iiq.To = JID;
            iiq.Type = IQType.get;
            if (Node != null)
            {
                DiscoItems items = (DiscoItems)iiq.Query;
                items.Node = Node;
            }
            return iiq;
        }

        public static IEnumerator EnumerateAll()
        {
            return m_items.GetEnumerator();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        #endregion
    }

    public delegate void DiscoNodeHandler(DiscoNode node);

    /// <summary>
    /// Disco database.
    /// TODO: once etags are finished, make all of this information cached on disk.
    /// TODO: cache JEP-115 client caps data to disk
    /// TODO: add negative caching
    /// </summary>
    [RCS(@"$Header$")]
    public class DiscoManager : System.ComponentModel.Component, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private JabberClient m_client = null;

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        /// <param name="container"></param>
        public DiscoManager(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        public DiscoManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The JabberClient to hook up to.
        /// </summary>
        [Description("The JabberClient to hook up to.")]
        [Category("Jabber")]
        public JabberClient Client
        {
            get
            {
                // If we are running in the designer, let's try to get an invoke control
                // from the environment.  VB programmers can't seem to follow directions.
                if ((this.m_client == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is JabberClient)
                                {
                                    m_client = (JabberClient)c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_client;
            }
            set
            {
                m_client = value;
                m_client.OnIQ += new IQHandler(GotIQ);
                m_client.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
                m_client.OnAuthenticate += new bedrock.ObjectHandler(m_client_OnAuthenticate);
            }
        }

        void m_client_OnAuthenticate(object sender)
        {
            DiscoNode root = DiscoNode.GetNode(m_client.Server);
            m_client.Write(root.InfoIQ(m_client.Document));
            m_client.Write(root.ItemsIQ(m_client.Document));
        }

        private void GotDisconnect(object sender)
        {
            DiscoNode.Clear();
        }

        private void GotIQ(object sender, IQ iq)
        {
            if (iq.Type != IQType.result)
                return;

            if (iq.Query == null)
                return; // broken impl on the other side shouldn't crash us.

            DiscoInfo info = iq.Query as DiscoInfo;
            if (info != null)
            {
                DiscoNode dn = DiscoNode.GetNode(iq.From, info.Node);
                GotInfo(dn, iq, info);
            }
            DiscoItems items = iq.Query as DiscoItems;
            if (items != null)
            {
                DiscoNode dn = DiscoNode.GetNode(iq.From, items.Node);
                GotItems(dn, iq, items);
            }
        }

        private void GotInfo(DiscoNode dn, IQ iq, DiscoInfo info)
        {
            dn.AddIdentities(info.GetIdentities());
            dn.AddFeatures(info.GetFeatures());
        }

        private void GotItems(DiscoNode dn, IQ iq, DiscoItems items)
        {
            dn.AddItems(items.GetItems());

            // automatically info everything we get an item for.
            foreach (DiscoNode n in dn.Children)
            {
                if (n.Features == null)
                {
                    m_client.Write(n.InfoIQ(m_client.Document));
                }
            }
        }

        public void BeginGetFeatures(DiscoNode node, DiscoNodeHandler handler)
        {
            if (handler == null)
                throw new ArgumentException("Handler must not be null", "handler");

            if (node.Features != null)
                handler(node);
            else
            {
                node.OnFeatures += handler;
                m_client.Write(node.InfoIQ(m_client.Document));
            }
        }

        public void BeginGetFeatures(JID jid, string node, DiscoNodeHandler handler)
        {
            BeginGetFeatures(DiscoNode.GetNode(jid, node), handler);
        }

        public void BeginGetItems(DiscoNode node, DiscoNodeHandler handler)
        {
            if (handler == null)
                throw new ArgumentException("Handler must not be null", "handler");

            if (node.Children != null)
                handler(node);
            else
            {
                node.OnItems += handler;
                m_client.Write(node.ItemsIQ(m_client.Document));
            }
        }

        public void BeginGetItems(JID jid, string node, DiscoNodeHandler handler)
        {
            BeginGetItems(DiscoNode.GetNode(jid, node), handler);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return DiscoNode.EnumerateAll();
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
