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
    /// Manages a service discovery (disco) identity. See <a href="http://www.xmpp.org/extensions/xep-0030.html">XEP-0030</a> for more information.
    /// </summary>
    public class Ident : IComparable
    {
        private string m_name;
        private string m_category;
        private string m_type;
        private string m_lang;

        /// <summary>
        /// Create a new identity from its constituent parts.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="type"></param>
        /// <param name="lang"></param>
        public Ident(string name, string category, string type, string lang)
        {
            m_category = (category == null) ? "" : category;
            m_name = (name == null) ? "" : name;
            m_type = (type == null) ? "" : type;
            m_lang = (lang == null) ? "" : lang;
        }

        /// <summary>
        /// Create a new, empty identity
        /// </summary>
        public Ident() : this("", "", "", "")
        {
        }

        /// <summary>
        /// Create an identity from protocol
        /// </summary>
        /// <param name="id"></param>
        public Ident(DiscoIdentity id) : this(id.Named, id.Category, id.Type, id.Lang)
        {
        }

        /// <summary>
        /// Retrieves the string representation of the Ident (category/type/lang/name).
        /// </summary>
        /// <returns></returns>
        [Category("Capabilities")]
        public string Key
        {
            get
            {
                return string.Format("{0}/{1}/{2}/{3}", m_category, m_type, m_lang, m_name);
            }
        }

        /// <summary>
        /// Contains the description of the entity.
        /// </summary>
        [Category("Text")]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Contains the capabilities category, such as server,
        /// client, gateway, directory and so on.
        /// </summary>
        [Category("Identity")]
        public string Category
        {
            get { return m_category; }
            set { m_category = value; }
        }

        /// <summary>
        /// Contains the entity type.
        /// </summary>
        [Category("Identity")]
        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// xml:lang language of this identity
        /// </summary>
        [Category("Text")]
        public string Lang
        {
            get { return m_lang; }
            set { m_lang = value; }
        }

        /// <summary>
        /// Does this identity have the given category and type?
        /// </summary>
        /// <param name="category">The category to compare</param>
        /// <param name="type">The type to compare</param>
        /// <returns></returns>
        public bool Matches(string category, string type)
        {
            return (m_category == category) && (m_type == type);
        }

        #region IComparable Members
        /// <summary>
        /// Compare to another identity, by comparing the string-ified versions
        /// of each.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if ((object)this == obj)
                return 0;
            Ident other = obj as Ident;
            if (other == null)
                return 1;
            return Key.CompareTo(other.Key);
        }
        #endregion

        /// <summary>
        /// Is this identity equal to that one?  If two are the same except for
        /// language, they are different by this method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (this.CompareTo(obj) == 0);
        }

        /// <summary>
        /// Hash over the string version of the identity.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        /// <summary>
        /// A slash-separated version of the name, with the unset parts omitted.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(m_category);
            sb.Append("/");
            sb.Append(m_type);

            if ((m_lang != null) && (m_lang != ""))
            {
                sb.Append("/");
                sb.Append(m_lang);
            }

            if ((m_name != null) && (m_name != ""))
            {
                sb.Append("/");
                sb.Append(m_name);
            }

            return sb.ToString(); ;
        }
    }

    /// <summary>
    /// Manages a JID and Node combination.
    /// </summary>
    [SVN(@"$Id$")]
    public class JIDNode
    {
        private JID m_jid = null;
        private string m_node = null;

        /// <summary>
        /// Creates a new JID/Node combination.
        /// </summary>
        /// <param name="jid">JID to associate with JIDNode.</param>
        /// <param name="node">Node to associate with JIDNode.</param>
        public JIDNode(JID jid, string node)
        {
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
            set { m_jid = value; }
        }

        /// <summary>
        /// Gets the Node.
        /// </summary>
        [Category("Identity")]
        public string Node
        {
            get { return m_node; }
            set { m_node = value; }
        }

        /// <summary>
        /// Retrieves a hash key that combines the JID and the node.
        /// </summary>
        /// <param name="jid">JID to use in the hash code.</param>
        /// <param name="node">Node to use in the hash code.</param>
        /// <returns>The hash code.</returns>
        protected static string GetKey(string jid, string node)
        {
            if ((node == null) || (node == ""))
            {
                if (jid == null)
                    return null;
                return jid.ToString();
            }
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
            int code = 0;
            if (m_jid != null)
                code = m_jid.GetHashCode();
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
    /// Manages the information and children of a given JID/Node combination.
    ///
    /// NOTE: If you have multiple connections in the same process, they all share the same Disco cache.
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
        private jabber.protocol.x.Data[] m_extensions;

        private ArrayList m_featureCallbacks = new ArrayList();
        private ArrayList m_itemCallbacks = new ArrayList();
        private ArrayList m_identCallbacks = new ArrayList();

        /// <summary>
        /// Creates a disco node.
        /// </summary>
        /// <param name="jid">JID associated with this JIDNode.</param>
        /// <param name="node">node associated with this JIDNode.</param>
        public DiscoNode(JID jid, string node)
            : base(jid, node)
        {
        }

        private class NodeCallback
        {
            public DiscoManager manager;
            public DiscoNodeHandler callback;
            public object state;

            public NodeCallback(DiscoManager m, DiscoNodeHandler h, object s)
            {
                Debug.Assert(h != null);
                manager = m;
                callback = h;
                state = s;
            }

            public void Call(DiscoNode node)
            {
                callback(manager, node, state);
            }
        }

        /// <summary>
        /// Add a callback for when features are received.
        /// 
        /// Calls the callback immediately if the features have already been retrieved.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns>True if there were no features yet, and the callback was queued.</returns>
        public bool AddFeatureCallback(DiscoManager manager, DiscoNodeHandler callback, object state)
        {
            lock (this)
            {
                if (Features != null)
                {
                    if (callback != null)
                        callback(manager, this, state);
                    return false;
                }
                else
                {
                    m_featureCallbacks.Add(new NodeCallback(manager, callback, state));
                    return true;
                }
            }
        }

        /// <summary>
        /// Add a callback for when items are received.
        /// 
        /// Calls the callback immediately if the items have already been retrieved.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns>True if there were no items yet, and the callback was queued.</returns>
        public bool AddItemsCallback(DiscoManager manager, DiscoNodeHandler callback, object state)
        {
            lock (this)
            {
                if (Children != null)
                {
                    if (callback != null)
                        callback(manager, this, state);
                    return false;
                }
                else
                {
                    m_itemCallbacks.Add(new NodeCallback(manager, callback, state));
                    return true;
                }
            }
        }

        /// <summary>
        /// Add a callback for when identities are received.
        /// 
        /// Calls the callback immediately if the features have already been retrieved.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns>True if there were no identities yet, and the callback was queued.</returns>
        public bool AddIdentityCallback(DiscoManager manager, DiscoNodeHandler callback, object state)
        {
            lock (this)
            {
                if (Identities != null)
                {
                    if (callback != null)
                        callback(manager, this, state);
                    return false;
                }
                else
                {
                    m_identCallbacks.Add(new NodeCallback(manager, callback, state));
                    return true;
                }
            }
        }

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
                        if ((id.Name != null) && (id.Name != ""))
                            m_name = id.Name;
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
        /// Determines whether or not the disco#info packet has been sent.
        /// </summary>
        [Category("Status")]
        public bool PendingInfo
        {
            get { return m_pendingInfo; }
        }

        /// <summary>
        /// Determines whether or not the disco#items packet has been sent.
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
                Features.CopyTo(names, 0);
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
                    names[count++] = i.Key;
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
            if (Identity == null)
                return new Ident[0];

            Ident[] ret = new Ident[Identity.Count];
            int count = 0;
            foreach (Ident i in Identity)
            {
                ret[count++] = i;
            }
            return ret;
        }

        /// <summary>
        /// Determines whether or not this node has the given category and type among its identities.
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
                if (i.Matches(category, type))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the x:data extensions of the disco information.
        /// </summary>
        public jabber.protocol.x.Data[] Extensions
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

        private void DoCallbacks(ArrayList callbacks)
        {
            lock (this)
            {
                foreach (NodeCallback cb in callbacks)
                    cb.Call(this);
                callbacks.Clear();
            }
        }

        /// <summary>
        /// Pulls all of the data out of the given protocol response.
        /// </summary>
        /// <param name="info">If null, just calls callbacks</param>
        public void AddInfo(DiscoInfo info)
        {
            if (info == null)
            {
                AddIdentities(null);
                AddFeatures(null);
                return;
            }
            Extensions = info.GetExtensions();
            AddIdentities(info.GetIdentities());
            AddFeatures(info.GetFeatures());
        }

        /// <summary>
        /// Add a single feature to the node.
        /// Does not fire OnFeatures, since this should mostly be used by
        /// things that are not querying externally.
        /// </summary>
        /// <param name="feature">The feature URI to add</param>
        public void AddFeature(string feature)
        {
            if (Features == null)
                Features = new Set();
            Features.Add(feature);
        }

        /// <summary>
        /// Remove a single feature from the node.
        /// Does not fire OnFeatures, since this should mostly be used by
        /// things that are not querying externally.
        /// 
        /// No exception should be thrown if the feature doesn't exist.
        /// </summary>
        /// <param name="feature">The feature URI to remove</param>
        public void RemoveFeature(string feature)
        {
            if (Features == null)
                return;
            Features.Remove(feature);
        }

        /// <summary>
        /// Adds these features to the node. Calls the OnFeatures event.
        /// </summary>
        /// <param name="features">Features to add to this node.</param>
        public void AddFeatures(DiscoFeature[] features)
        {
            if (Features == null)
                Features = new Set();

            // features may be null when used from outside.
            if (features != null)
            {
                foreach (DiscoFeature f in features)
                    Features.Add(f.Var);
            }

            DoCallbacks(m_featureCallbacks);
        }

        /// <summary>
        /// Clear out any features already in the list.
        /// </summary>
        public void ClearFeatures()
        {
            Features = null;
        }

        /// <summary>
        /// Adds these identities to the node.
        /// </summary>
        /// <param name="id">Identities to add.</param>
        public void AddIdentity(Ident id)
        {
            if (Identity == null)
                Identity = new Set();
            Identity.Add(id);
        }

        /// <summary>
        /// Add these identities to the node.
        /// Fires OnIdentities
        /// </summary>
        /// <param name="ids">Identities to add.</param>
        public void AddIdentities(DiscoIdentity[] ids)
        {
            if (Identity == null)
                Identity = new Set();

            // ids may be null when used from outside.
            if (ids != null)
            {
                foreach (DiscoIdentity id in ids)
                    Identity.Add(new Ident(id));
            }

            DoCallbacks(m_identCallbacks);
        }

        /// <summary>
        /// Clear out any identities already in the list.
        /// </summary>
        public void ClearIdentity()
        {
            Identity = null;
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

            // items may be null when used from outside.
            if (items != null)
            {
                foreach (DiscoItem di in items)
                    AddItem(di);
            }

            DoCallbacks(m_itemCallbacks);
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
    /// Represents a callback with a new disco node.
    /// </summary>
    /// <param name="sender">The DiscoManager managing this node</param>
    /// <param name="node">The node that changed</param>
    /// <param name="state">State passed in to the Begin request.</param>
    public delegate void DiscoNodeHandler(DiscoManager sender, DiscoNode node, object state);

    /// <summary>
    /// Manages the discovery (disco) database.
    /// </summary>
    // TODO: once etags are finished, make all of this information cached on disk.
    // TODO: cache XEP-115 client caps data to disk
    // TODO: add negative caching
    [SVN(@"$Id$")]
    public class DiscoManager : StreamComponent, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private DiscoNode m_root = null;

        /// <summary>
        /// Creates a new DiscoManager and associates it with a parent container.
        /// </summary>
        /// <param name="container">Parent container.</param>
        public DiscoManager(System.ComponentModel.IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Creates a new DiscoManager.
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
            lock (node)
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
        }

        private void RequestItems(DiscoNode node)
        {
            lock (node)
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
                dn.AddInfo(null);
                return;
            }

            dn.AddInfo(iq.Query as DiscoInfo);

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
                id.Name = agent.Description;
                switch (agent.Service)
                {
                case "groupchat":
                    id.Category = "conference";
                    id.Type = "text";
                    child.Identity.Add(id);
                    break;
                case "jud":
                    id.Category = "directory";
                    id.Type = "user";
                    child.Identity.Add(id);
                    break;
                case null:
                case "":
                    break;
                default:
                    // guess this is a transport
                    id.Category = "gateway";
                    id.Type = agent.Service;
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
                    if (id.Category != "gateway")
                    {
                        Ident tid = new Ident();
                        tid.Name = id.Name;
                        tid.Category = "gateway";
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
        /// Retrieves the features associated with this node and
        /// then calls back on the handler.
        /// If the information is in the cache, handler gets called right now.
        /// </summary>
        /// <param name="node">Node to look for.</param>
        /// <param name="handler">Callback to use afterwards.</param>
        /// <param name="state">Context to pass back to caller when complete</param>
        public void BeginGetFeatures(DiscoNode node, DiscoNodeHandler handler, object state)
        {
            if (node == null)
                node = m_root;

            if (node.AddFeatureCallback(this, handler, state))
                RequestInfo(node);
        }

        /// <summary>
        /// Retrieves the features associated with this JID and node and
        /// then calls back on the handler.
        /// If the information is in the cache, handler gets called right now.
        /// </summary>
        /// <param name="jid">JID to look for.</param>
        /// <param name="node">Node to look for.</param>
        /// <param name="handler">Callback to use afterwards.</param>
        /// <param name="state">Context to pass back to caller when complete</param>
        public void BeginGetFeatures(JID jid, string node, DiscoNodeHandler handler, object state)
        {
            BeginGetFeatures(DiscoNode.GetNode(jid, node), handler, state);
        }

        /// <summary>
        /// Retrieves the child items associated with this node,
        /// and then calls back on the handler.
        /// If the information is in the cache, handler gets
        /// called right now.
        /// </summary>
        /// <param name="node">Disco node to search.</param>
        /// <param name="handler">Callback that gets called with the items.</param>
        /// <param name="state">Context to pass back to caller when complete</param>
        public void BeginGetItems(DiscoNode node, DiscoNodeHandler handler, object state)
        {
            if (node == null)
                node = m_root;

            if (node.AddItemsCallback(this, handler, state))
                RequestItems(node);
        }

        /// <summary>
        /// Retrieves the child items associated with this node and JID,
        /// and then calls back on the handler.
        /// If the information is in the cache, handler gets
        /// called right now.
        /// </summary>
        /// <param name="jid">JID of Service to query.</param>
        /// <param name="node">Node on the service to interact with.</param>
        /// <param name="handler">Callback that gets called with the items.</param>
        /// <param name="state">Context to pass back to caller when complete</param>
        public void BeginGetItems(JID jid, string node, DiscoNodeHandler handler, object state)
        {
            BeginGetItems(DiscoNode.GetNode(jid, node), handler, state);
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

            public void GotFeatures(DiscoManager manager, DiscoNode node, object state)
            {

                // yes, yes, this may call the handler more than once in multi-threaded world.  Punt for now.
                if (m_handler != null)
                {
                    if (node.HasFeature(m_URI))
                    {
                        m_handler(manager, node, state);
                        m_handler = null;
                    }
                }

                if (Interlocked.Decrement(ref m_outstanding) == 0)
                {
                    if (m_handler != null)
                        m_handler(manager, null, state);
                }
            }

            public void GotRootItems(DiscoManager manager, DiscoNode node, object state)
            {
                m_outstanding = node.Children.Count;
                foreach (DiscoNode n in node.Children)
                    manager.BeginGetFeatures(n, new DiscoNodeHandler(GotFeatures), state);
            }
        }

        /// <summary>
        /// Finds a component that implements a given feature, which is a child of
        /// the root. This will call back on the first match.  It will call back
        /// with null if none are found.
        /// </summary>
        /// <param name="featureURI">Feature to look for.</param>
        /// <param name="handler">Callback to use when finished.</param>
        /// <param name="state">Context to pass back to caller when complete</param>
        public void BeginFindServiceWithFeature(string featureURI, DiscoNodeHandler handler, object state)
        {
            if (handler == null)
                return;  // prove I *didn't* call it. :)

            FindServiceRequest req = new FindServiceRequest(this, featureURI, handler);
            if (m_root == null)
                m_root = DiscoNode.GetNode(m_stream.Server);
            BeginGetItems(m_root, new DiscoNodeHandler(req.GotRootItems), state);  // hopefully enough to prevent GC.
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
