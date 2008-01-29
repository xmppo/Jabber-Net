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

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Threading;

using bedrock.util;
using bedrock.collections;

using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace jabber.connection
{
    /// <summary>
    /// A disco identity.  See XEP-0030.
    /// </summary>
    public class Ident
    {
        /// <summary>
        /// Contains the description of the entity.
        /// </summary>
        public string name;
        /// <summary>
        /// Contains the capabilities category, such as server,
        /// client, gateway, directory and so on. 
        /// </summary>
        public string category;
        /// <summary>
        /// Contains the entity type.
        /// </summary>
        public string type;

        /// <summary>
        /// Retrieves the string representation of the Ident (category/type).
        /// </summary>
        /// <returns></returns>
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
    [SVN(@"$Id$")]
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
        /// Gets the JID.
        /// </summary>
        [Category("Identity")]
        public JID JID
        {
            get { return m_jid; }
        }

        /// <summary>
        /// Gets the Node.
        /// </summary>
        [Category("Identity")]
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
        /// Gets the JID/Node key for Hash lookup.
        /// </summary>
        [Browsable(false)]
        public string Key
        {
            get { return GetKey(m_jid, m_node); }
        }

        /// <summary>
        /// Determines if both the jid and the node are equal.
        /// </summary>
        /// <param name="obj">JIDNode to compare to.</param>
        /// <returns>True if both the jid and the node are equal.</returns>
        public override bool Equals(object obj)
        {
            JIDNode other = obj as JIDNode;
            if (other == null)
            {
                return false;
            }

            return (m_jid == other.m_jid) && (m_node == other.m_node);
        }

        /// <summary>
        /// Serves as a hash function to combine the JID and node together.
        /// GetHashCode() is suitable for use in hashing algorithms and
        /// data structures like a hash table.
        /// </summary>
        /// <returns>The hash code of this JIDNode.</returns>
        public override int GetHashCode()
        {
            Debug.Assert(m_jid != null);
            int code = m_jid.GetHashCode();
            if (m_node != null)
                code ^= m_node.GetHashCode();
            return code;
        }

        /// <summary>
        /// Returns a string representing the JID/Node.
        /// </summary>
        /// <returns>String representing the JID/Node.</returns>
        public override string ToString()
        {
            return JID + "/" + Node;
        }
    }


    /// <summary>
    /// The info and children of a given JID/Node combination.
    ///
    /// Note: if you have multiple connections in the same process, they all share the same Disco cache.
    /// This works fine in the real world today, since I don't know of any implementations that return different
    /// disco for different requestors, but it is completely legal protocol to have done so.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoNode : JIDNode, IEnumerable
    {
        private static Tree m_items = new Tree();

        /// <summary>
        /// Contains the children of this node.
        /// </summary>
        public Set Children = null;
        /// <summary>
        /// Contains the Features of this node.
        /// </summary>
        public Set Features = null;
        /// <summary>
        /// Contains the identities of this node.
        /// </summary>
        public Set Identity = null;
        private string m_name = null;
        private bool m_pendingItems = false;
        private bool m_pendingInfo = false;
        private jabber.protocol.x.Data m_extensions;

        /// <summary>
        /// Creates a disco node.
        /// </summary>
        /// <param name="jid">JID associated with this JIDNode.</param>
        /// <param name="node">node associated with this JIDNode.</param>
        public DiscoNode(JID jid, string node)
            : base(jid, node)
        {
        }

        /// <summary>
        /// Informs the client that features are now available from the XMPP server.
        /// </summary>
        public event DiscoNodeHandler OnFeatures;
        /// <summary>
        /// Informs the client that new children are now available.
        /// </summary>
        public event DiscoNodeHandler OnItems;
        /// <summary>
        /// Informs the client that new identities are available.
        /// </summary>
        public event DiscoNodeHandler OnIdentities;

        /// <summary>
        /// Gets or sets the string representation of the first identity.
        /// </summary>
        [Category("Info")]
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
        /// Determines whether the disco#info packet has been sent.
        /// </summary>
        [Category("Status")]
        public bool PendingInfo
        {
            get { return m_pendingInfo; }
        }

        /// <summary>
        /// Determines whether the disco#items packet has been sent.
        /// </summary>
        [Category("Status")]
        public bool PendingItems
        {
            get { return m_pendingItems; }
        }

        /// <summary>
        /// Retrieves the features associated with this node.
        /// </summary>
        [Category("Info")]
        public string[] FeatureNames
        {
            get
            {
                if (Features == null)
                    return new string[0];
                string[] names = new string[Features.Count];
                int count = 0;
                foreach (string s in Features)
                {
                    names[count++] = s;
                }
                return names;
            }
        }

        /// <summary>
        /// Retrieves the disco identities of the node.
        /// </summary>
        [Category("Info")]
        public string[] Identities
        {
            get
            {
                if (Identity == null)
                    return new string[0];
                string[] names = new string[Identity.Count];
                int count = 0;
                foreach (Ident i in Identity)
                {
                    names[count++] = i.GetKey();
                }
                return names;
            }
        }

        /// <summary>
        /// Retrieves an identity object for each identity of the node.  
        /// </summary>
        /// <returns>List of identities associated with this node.</returns>
        public Ident[] GetIdentities()
        {
            Ident[] ret = new Ident[Identity.Count];
            int count = 0;
            foreach (Ident i in Identity)
            {
                ret[count++] = i;
            }
            return ret;
        }

        /// <summary>
        /// Determines if this node has the given category and type among its identities.
        /// </summary>
        /// <param name="category">Category to look for.</param>
        /// <param name="type">Type to look for.</param>
        /// <returns>The node contains the category and the type if true.</returns>
        public bool HasIdentity(string category, string type)
        {
            if (Identity == null)
                return false;
            foreach (Ident i in Identity)
            {
                if ((i.category == category) && (i.type == type))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the x:data extensions of the disco information.
        /// </summary>
        public jabber.protocol.x.Data Extensions
        {
            get
            {
                return m_extensions;
            }
            set
            {
                m_extensions = value;
            }
        }

        /// <summary>
        /// Creates nodes and ensure that they are cached.
        /// </summary>
        /// <param name="jid">JID associated with DiscoNode.</param>
        /// <param name="node">Node associated with DiscoNode.</param>
        /// <returns>
        /// If DiscoNode exists, returns the found node.
        /// Otherwise it creates the node and return it.
        /// </returns>
        public static DiscoNode GetNode(JID jid, string node)
        {
            lock (m_items)
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
        }

        /// <summary>
        /// Creates nodes where only the JID is specified.
        /// </summary>
        /// <param name="jid">JID associated with DiscoNode.</param>
        /// <returns>
        /// If DiscoNode exists, returns the found node.
        /// Otherwise it creates the node and return it.
        /// </returns>
        public static DiscoNode GetNode(JID jid)
        {
            return GetNode(jid, null);
        }

        /// <summary>
        /// Deletes the cache.
        /// </summary>
        public static void Clear()
        {
            m_items.Clear();
        }

        /// <summary>
        /// Determines if this node has the specified feature.
        /// </summary>
        /// <param name="URI">Feature to look for.</param>
        /// <returns>The node has this feature if true.</returns>
        public bool HasFeature(string URI)
        {
            if (Features == null)
                return false;
            return Features.Contains(URI);
        }

        /// <summary>
        /// Adds these features to the node. Calls the OnFeatures event.
        /// </summary>
        /// <param name="features">Features to add to this node.</param>
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
        /// Adds these identities to the node.
        /// </summary>
        /// <param name="ids">Identities to add.</param>
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

            if (OnIdentities != null)
            {
                OnIdentities(this);
                OnIdentities = null;
            }
        }

        internal DiscoNode AddItem(DiscoItem di)
        {
            DiscoNode dn = GetNode(di.Jid, di.Node);
            if ((di.Named != null) && (di.Named != ""))
                dn.Name = di.Named;
            Children.Add(dn);
            return dn;
        }

        /// <summary>
        /// Adds the given items to the cache.
        /// </summary>
        /// <param name="items">Items to add.</param>
        public void AddItems(DiscoItem[] items)
        {
            if (Children == null)
                Children = new Set();

            foreach (DiscoItem di in items)
                AddItem(di);

            if (OnItems != null)
            {
                OnItems(this);
                OnItems = null;
            }
        }

        /// <summary>
        /// Creates a disco#info IQ packet.
        /// </summary>
        /// <param name="doc">XmlDocument to create the XML elements with.</param>
        /// <returns>XML representing the disco#info request.</returns>
        public IQ InfoIQ(System.Xml.XmlDocument doc)
        {
            m_pendingInfo = true;
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

        /// <summary>
        /// Creates a disco#items IQ packet.
        /// </summary>
        /// <param name="doc">XmlDocument used to create the XML Elements.</param>
        /// <returns>XML element representing the disco#items request.</returns>
        public IQ ItemsIQ(System.Xml.XmlDocument doc)
        {
            m_pendingItems = true;

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

        /// <summary>
        /// Gets all of the items.
        /// </summary>
        /// <returns>Tree enumerator to loop over.</returns>
        public static IEnumerator EnumerateAll()
        {
            return m_items.GetEnumerator();
        }

        #region IEnumerable Members

        /// <summary>
        /// Gets an enumerator across all items.
        /// </summary>
        /// <returns>Set enumerator to loop over.</returns>
        public IEnumerator GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Callback with a new disco node.
    /// </summary>
    /// <param name="node"></param>
    public delegate void DiscoNodeHandler(DiscoNode node);

    /// <summary>
    /// Disco database.
    /// TODO: once etags are finished, make all of this information cached on disk.
    /// TODO: cache XEP-115 client caps data to disk
    /// TODO: add negative caching
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoManager : StreamComponent, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private DiscoNode m_root = null;

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        /// <param name="container"></param>
        public DiscoManager(System.ComponentModel.IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        public DiscoManager()
        {
            InitializeComponent();
            this.OnStreamChanged +=new bedrock.ObjectHandler(DiscoManager_OnStreamChanged);
        }


        /// <summary>
        /// Gets the root node.  This is probably the server that the client is
        /// connected to. If the Children property of this root node is null,
        /// the disco#items request has not returned an answer. Register on this
        /// node's OnFeatures callback.
        /// </summary>
        public DiscoNode Root
        {
            get { return m_root; }
        }

        private void DiscoManager_OnStreamChanged(object sender)
        {
            if (m_stream == null)
                return;
            m_stream.OnAuthenticate += new bedrock.ObjectHandler(m_client_OnAuthenticate);
        }

        private void m_client_OnAuthenticate(object sender)
        {
            if (m_root == null)
                m_root = DiscoNode.GetNode(m_stream.Server);
            RequestInfo(m_root);
        }

        private void GotDisconnect(object sender)
        {
            m_root = null;
            DiscoNode.Clear();
        }

        private void RequestInfo(DiscoNode node)
        {
            if (!node.PendingInfo)
            {
                IQ iq = node.InfoIQ(m_stream.Document);
                jabber.server.JabberService js = m_stream as jabber.server.JabberService;
                if (js != null)
                    iq.From = js.ComponentID;
                m_stream.Tracker.BeginIQ(iq,
                                         new jabber.connection.IqCB(GotInfo),
                                         node);
            }
        }

        private void RequestItems(DiscoNode node)
        {
            if (!node.PendingItems)
            {
                IQ iq = node.ItemsIQ(m_stream.Document);
                jabber.server.JabberService js = m_stream as jabber.server.JabberService;
                if (js != null)
                    iq.From = js.ComponentID;
                m_stream.Tracker.BeginIQ(iq,
                                         new jabber.connection.IqCB(GotItems),
                                         node);
            }
        }


        private void GotInfo(object sender, IQ iq, object onode)
        {
            DiscoNode dn = onode as DiscoNode;
            Debug.Assert(dn != null);

            if (iq.Type == IQType.error)
            {
                if (dn == m_root)
                {
                    // root node.
                    // Try agents.
                    Error err = iq.Error;
                    if (err != null)
                    {
                        string cond = err.Condition;
                        if ((cond == Error.FEATURE_NOT_IMPLEMENTED) ||
                            (cond == Error.SERVICE_UNAVAILABLE))
                        {
                            IQ aiq = new AgentsIQ(m_stream.Document);
                            m_stream.Tracker.BeginIQ(aiq, new jabber.connection.IqCB(GotAgents), m_root);
                            return;
                        }
                    }
                }
            }
            if (iq.Type != IQType.result)
            {
                // protocol error
                dn.AddIdentities(null);
                dn.AddFeatures(null);
                return;
            }

            DiscoInfo info = iq.Query as DiscoInfo;
            if (info == null)
            {
                // protocol error
                dn.AddIdentities(null);
                dn.AddFeatures(null);
                return;
            }

            jabber.protocol.x.Data ext = info["x", URI.XDATA] as jabber.protocol.x.Data;
            if (ext != null)
                dn.Extensions = ext;

            dn.AddIdentities(info.GetIdentities());
            dn.AddFeatures(info.GetFeatures());

            if (dn == m_root)
                RequestItems(m_root);
        }

        private void GotItems(object sender, IQ iq, object onode)
        {
            DiscoNode dn = onode as DiscoNode;
            Debug.Assert(dn != null);

            if (iq.Type != IQType.result)
            {
                // protocol error
                dn.AddItems(null);
                return;
            }

            DiscoItems items = iq.Query as DiscoItems;
            if (items == null)
            {
                // protocol error
                dn.AddItems(null);
                return;
            }

            dn.AddItems(items.GetItems());

            // automatically info everything we get an item for.
            foreach (DiscoNode n in dn.Children)
            {
                if (n.Features == null)
                {
                    RequestInfo(n);
                }
            }
        }

        private void GotAgents(object sender, IQ iq, object onode)
        {
            DiscoNode dn = onode as DiscoNode;
            Debug.Assert(dn != null);

            if (iq.Type != IQType.result)
            {
                dn.AddItems(null);
                return;
            }

            AgentsQuery aq = iq.Query as AgentsQuery;
            if (aq == null)
            {
                dn.AddItems(null);
                return;
            }

            if (dn.Children == null)
                dn.Children = new Set();

            foreach (Agent agent in aq.GetAgents())
            {
                DiscoItem di = new DiscoItem(m_stream.Document);
                di.Jid = agent.JID;
                di.Named = agent.AgentName;

                DiscoNode child = dn.AddItem(di);
                if (child.Features == null)
                    child.Features = new Set();
                if (child.Identity == null)
                    child.Identity = new Set();

                Ident id = new Ident();
                id.name = agent.Description;
                switch (agent.Service)
                {
                case "groupchat":
                    id.category = "conference";
                    id.type = "text";
                    child.Identity.Add(id);
                    break;
                case "jud":
                    id.category = "directory";
                    id.type = "user";
                    child.Identity.Add(id);
                    break;
                case null:
                case "":
                    break;
                default:
                    // guess this is a transport
                    id.category = "gateway";
                    id.type = agent.Service;
                    child.Identity.Add(id);
                    break;
                }

                if (agent.Register)
                    child.Features.Add(URI.REGISTER);
                if (agent.Search)
                    child.Features.Add(URI.SEARCH);
                if (agent.Groupchat)
                    child.Features.Add(URI.MUC);
                if (agent.Transport)
                {
                    if (id.category != "gateway")
                    {
                        Ident tid = new Ident();
                        tid.name = id.name;
                        tid.category = "gateway";
                        child.Identity.Add(tid);
                    }
                }

                foreach (XmlElement ns in agent.GetElementsByTagName("ns"))
                {
                    child.Features.Add(ns.InnerText);
                }
                child.AddItems(null);
                child.AddIdentities(null);
                child.AddFeatures(null);
            }
            dn.AddItems(null);
            dn.AddIdentities(null);
            dn.AddFeatures(null);
        }

        /// <summary>
        /// Make a call to get the feaures to this node, and call back on handler.
        /// If the information is in the cache, handler gets called right now.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="handler"></param>
        public void BeginGetFeatures(DiscoNode node, DiscoNodeHandler handler)
        {
            if (node.Features != null)
            {
                if (handler != null)
                    handler(node);
            }
            else
            {
                if (handler != null)
                    node.OnFeatures += handler;
                RequestInfo(node);
            }
        }

        /// <summary>
        /// Make a call to get the feaures to this node, and call back on handler.
        /// If the information is in the cache, handler gets called right now.
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="node"></param>
        /// <param name="handler"></param>
        public void BeginGetFeatures(JID jid, string node, DiscoNodeHandler handler)
        {
            BeginGetFeatures(DiscoNode.GetNode(jid, node), handler);
        }

        /// <summary>
        /// Makes a call to get the child items of this node, and then calls
        /// back on the handler. If the information is in the cache, handler gets
        /// called right now.
        /// </summary>
        /// <param name="node">PubSub node.</param>
        /// <param name="handler">Callback that gets called with the items.</param>
        public void BeginGetItems(DiscoNode node, DiscoNodeHandler handler)
        {
            if (node.Children != null)
            {
                if (handler != null)
                    handler(node);
            }
            else
            {
                if (handler != null)
                    node.OnItems += handler;
                RequestItems(node);
            }
        }

        /// <summary>
        /// Makes a call to get the child items of this node and JID, and then calls
        /// back on the handler. If the information is in the cache, handler gets
        /// called right now.
        /// </summary>
        /// <param name="jid">JID of PubSub handler.</param>
        /// <param name="node">Node on the PubSub handler to interact with.</param>
        /// <param name="handler">Callback that gets called with the items.</param>
        public void BeginGetItems(JID jid, string node, DiscoNodeHandler handler)
        {
            BeginGetItems(DiscoNode.GetNode(jid, node), handler);
        }

        private class FindServiceRequest
        {
            private DiscoManager m_manager;
            private string m_URI;
            private DiscoNodeHandler m_handler;
            private int m_outstanding = 0;

            public FindServiceRequest(DiscoManager man, string featureURI, DiscoNodeHandler handler)
            {
                m_manager = man;
                m_URI = featureURI;
                m_handler = handler;
            }

            public void GotFeatures(DiscoNode node)
            {

                // yes, yes, this may call the handler more than once in multi-threaded world.  Punt for now.
                if (m_handler != null)
                {
                    if (node.HasFeature(m_URI))
                    {
                        m_handler(node);
                        m_handler = null;
                    }
                }

                if (Interlocked.Decrement(ref m_outstanding) == 0)
                {
                    if (m_handler != null)
                        m_handler(null);
                }
            }

            public void GotRootItems(DiscoNode node)
            {
                m_outstanding = node.Children.Count;
                foreach (DiscoNode n in node.Children)
                    m_manager.BeginGetFeatures(n, new DiscoNodeHandler(GotFeatures));
            }
        }

        /// <summary>
        /// Looks for a component that implements a given feature, which is a child of the root.
        /// This will call back on the first match.  It will call back with null if none 
        /// are found.
        /// </summary>
        /// <param name="featureURI">Feature to look for.</param>
        /// <param name="handler">Callback to use when finished.</param>
        public void BeginFindServiceWithFeature(string featureURI, DiscoNodeHandler handler)
        {
            if (handler == null)
                return;  // prove I *didn't* call it. :)

            FindServiceRequest req = new FindServiceRequest(this, featureURI, handler);
            if (m_root == null)
                m_root = DiscoNode.GetNode(m_stream.Server);
            BeginGetItems(m_root, new DiscoNodeHandler(req.GotRootItems));  // hopefully enough to prevent GC.
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
