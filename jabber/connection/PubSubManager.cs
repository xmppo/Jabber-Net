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
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using bedrock.util;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace jabber.connection
{
    /// <summary>
    /// Manages a set of publish-subscribe (<a href="http://www.xmpp.org/extensions/xep-0060.html">XEP-60</a>) subscriptions.
    /// The goal is to have a list of jid/node combinations, each of which is a singleton.
    /// <example>
    /// PubSubNode node = ps.GetNode("infobroker.corp.jabber.com", "test/foo", 10);
    /// node.AddItemAddCallback(new ItemCB(node_OnItemAdd));
    /// node.OnItemRemove += new ItemCB(node_OnItemRemove);
    /// node.OnError += new bedrock.ExceptionHandler(node_OnError);
    /// node.AutomatedSubscribe();
    /// </example>
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubManager : StreamComponent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        private Hashtable m_nodes = new Hashtable();

        /// <summary>
        /// Creates a manager.
        /// </summary>
        public PubSubManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a manager in a container.
        /// </summary>
        /// <param name="container">Parent container.</param>
        public PubSubManager(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Performs tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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

        /// <summary>
        /// Subscribes to a publish-subscribe node.
        /// </summary>
        /// <param name="service">Component that handles PubSub requests.</param>
        /// <param name="node">The node on the component that the client wants to interact with.</param>
        /// <param name="maxItems">Maximum number of items to retain.  First one to call Subscribe gets their value, for now.</param>
        /// <returns>
        /// The existing node will be returned if there is already a subscription.
        /// If the node does not exist, the PubSubNode object will be returned
        /// in a subscribing state.
        /// </returns>
        public PubSubNode GetNode(JID service, string node, int maxItems)
        {
            JIDNode jn = new JIDNode(service, node);
            PubSubNode n = (PubSubNode) m_nodes[jn];
            if (n != null)
                return n;
            n = new PubSubNode(Stream, service, node, maxItems);
            m_nodes[jn] = n;
            return n;
        }

        ///<summary>
        /// Removes the publish-subscribe node from the manager and sends a delete
        /// node to the XMPP server.
        ///</summary>
        /// <param name="service">
        /// Component that handles PubSub requests.
        /// </param>
        /// <param name="node">
        /// The node on the component that the client wants to interact with.
        /// </param>
        /// <param name="errorHandler">
        /// Callback for any errors with the publish-subscribe node deletion.
        /// </param>
        public void RemoveNode(JID service, string node, bedrock.ExceptionHandler errorHandler)
        {
            JIDNode jn = new JIDNode(service, node);

            PubSubNode psNode = m_nodes[jn] as PubSubNode;
            if (psNode != null)
            {
                m_nodes[jn] = null;
            }
            else
            {
                psNode = new PubSubNode(Stream, service, node, 10);
            }

            psNode.OnError += errorHandler;

            psNode.Delete();
        }
    }

    /// <summary>
    /// Notifies the client about a publish-subscribe item.
    /// </summary>
    public delegate void ItemCB(PubSubNode node, PubSubItem item);

    /// <summary>
    /// Manages a list of items with a maximum size.  Only one item with a given ID will be in the
    /// list at a given time.
    /// </summary>
    [SVN(@"$Id$")]
    public class ItemList : ArrayList
    {
        private Hashtable m_index = new Hashtable();
        private PubSubNode m_node = null;

        /// <summary>
        /// Creates an item list, which will have at most some number of items.
        /// </summary>
        /// <param name="node">The node to which this item list applies.</param>
        /// <param name="maxItems">Maximum size of the list.  Delete notifications will be sent if this size is exceeded.</param>
        public ItemList(PubSubNode node, int maxItems) : base(maxItems)
        {
            m_node = node;
        }

        /// <summary>
        /// Makes sure that the underlying ID index is in sync
        /// when an item is removed.
        /// </summary>
        /// <param name="index">Index of PubSubItem to remove.</param>
        public override void RemoveAt(int index)
        {
            PubSubItem item = (PubSubItem)this[index];
            string id = item.GetAttribute("id");
            if (id != "")
            {
                m_index.Remove(id);
            }
            base.RemoveAt(index);
            m_node.ItemRemoved(item);

            // renumber
            for (int i=index; i<Count; i++)
            {
                item = (PubSubItem)this[i];
                id = item.ID;
                if (id != "")
                    m_index[id] = i;
            }
        }

        /// <summary>
        /// Adds to the end of the list, replacing any item with the same ID,
        /// or bumping the oldest item if the list is full.
        /// </summary>
        /// <param name="value">PubSubItem to add to the list.</param>
        /// <returns>Index where the PubSubItem was inserted.</returns>
        public override int Add(object value)
        {
            PubSubItem item = value as PubSubItem;
            if (item == null)
                throw new ArgumentException("Must be an XmlElement", "value");
            string id = item.ID;
            int i;
            if (id == "")
            {
                if (Count == Capacity)
                {
                    RemoveAt(0);
                }
                i = base.Add(value);
                m_node.ItemAdded(item);
                return i;
            }

            // RemoveId(id);
            if (!m_index.Contains(id) && (Count == Capacity))
                RemoveAt(0);

            i = base.Add(value);
            m_index[id] = i;
            m_node.ItemAdded(item);
            return i;
        }

        /// <summary>
        /// Removes the item with the given ID.
        /// No exception is thrown if no item is found with that ID.
        /// </summary>
        /// <param name="id">ID of the item to remove</param>
        public void RemoveId(string id)
        {
            object index = m_index[id];
            if (index != null)
                RemoveAt((int)index);
        }

        /// <summary>
        /// Gets or sets the contents of the specified item.
        /// </summary>
        /// <param name="id">Id of the PubSubItem.</param>
        /// <returns>XmlElement representing the contents of the PubSubItem.</returns>
        public XmlElement this[string id]
        {
            get
            {
                object index = m_index[id];
                if (index == null)
                    return null;
                PubSubItem item = this[(int)index] as PubSubItem;
                if (item == null)
                    return null;
                return item.Contents;
            }
            set
            {
                // wrap an item around the contents.
                PubSubItem item = new PubSubItem(value.OwnerDocument);
                item.Contents = value;
                item.ID = id;
                Add(item);
            }
        }
    }


    /// <summary>
    /// Contains the different possible operations on a publish-subscribe node.
    /// </summary>
    public enum Op
    {
        /// <summary>
        /// Creates a node
        /// </summary>
        CREATE,
        /// <summary>
        /// Subscribes to a node
        /// </summary>
        SUBSCRIBE,
        /// <summary>
        /// Gets the current items in the node
        /// </summary>
        ITEMS,
        /// <summary>
        /// Deletes a node
        /// </summary>
        DELETE,
        /// <summary>
        /// Deletes an item from the node
        /// </summary>
        DELETE_ITEM,
        /// <summary>
        /// Publishes an item to a node
        /// </summary>
        PUBLISH_ITEM,
    }

    /// <summary>
    /// Informs the client that a publish-subscribe error occurred.
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubException : Exception
    {
        /// <summary>
        /// Contains the stanza that caused the error.
        /// </summary>
        public XmlElement Protocol = null;
        /// <summary>
        /// Contains the operation that failed.
        /// </summary>
        public Op Operation;

        /// <summary>
        /// Creates a new publish-subscribe exception.
        /// </summary>
        /// <param name="op">The operation that failed.</param>
        /// <param name="error">A description of the error.</param>
        /// <param name="elem">The stanza that caused the error.</param>
        public PubSubException(Op op, string error, XmlElement elem) : base(error)
        {
            Operation = op;
            Protocol = elem;
        }

        /// <summary>
        /// Gets the error string.
        /// </summary>
        public override string Message
        {
            get
            { return string.Format("PubSub error on {0}: {1}\r\nAssociated protocol: {2}", Operation, base.Message, Protocol); }
        }
    }


    /// <summary>
    /// Manages a node to be subscribed to.  Will keep a maximum number of items.
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubNode : IEnumerable
    {
        private enum STATE
        {
            Start,
            Pending,
            Asking,
            Running,
            Error,
        }

        // indexed by op.
        private STATE[] m_state = new STATE[] { STATE.Start, STATE.Start, STATE.Start, STATE.Start};

        private XmppStream  m_stream = null;
        private JID         m_jid = null;
        private string      m_node = null;
        private ItemList    m_items = null;

        ///<summary>
        /// Retrieves the component that handles publish-subscribe requests.
        ///</summary>
        public JID Jid
        {
            get { return m_jid; }
        }

        ///<summary>
        /// Retrieves the node to interact with as defined by XEP-60.
        ///</summary>
        public string Node
        {
            get { return m_node; }
        }

        /// <summary>
        /// Create a Node.  Next, call Create and/or Subscribe.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="jid"></param>
        /// <param name="node"></param>
        /// <param name="maxItems"></param>
        internal PubSubNode(XmppStream stream, JID jid, string node, int maxItems)
        {
            if (stream == null)
                throw new ArgumentException("must not be null", "stream");
            if (jid == null)
                throw new ArgumentException("must not be null", "jid");
            if (node == null)
                throw new ArgumentException("must not be null", "node");
            if (node == "")
                throw new ArgumentException("must not be empty", "node");

            m_stream = stream;
            m_jid = jid;
            m_node = node;
            m_items = new ItemList(this, maxItems);
        }

        /// <summary>
        /// Determines whether or not this node is fully initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return (this[Op.CREATE] == STATE.Running) && (this[Op.SUBSCRIBE] == STATE.Running); }
        }

        /// <summary>
        /// Informs the client that an item has been added to the node,
        /// either on initial get or by notification.
        /// NOTE: This may fire for the same item more than once.
        /// </summary>
        public event ItemCB OnItemAdd;

        /// <summary>
        /// Informs the client that an item has been deleted from the node,
        /// either by notification or the list being full.
        /// </summary>
        public event ItemCB OnItemRemove;

        /// <summary>
        /// Notifies the client that an error occurred.
        /// </summary>
        public event bedrock.ExceptionHandler OnError;


        private STATE this[Op op]
        {
            get { return m_state[(int)op]; }
            set { m_state[(int)op] = value; }
        }

        private void FireError(Op op, string message, XmlElement protocol)
        {
            Debug.WriteLine(string.Format("Error {0}ing pubsub node: {1}", op, message));
            this[op] = STATE.Error;

            if (OnError != null)
                OnError(this, new PubSubException(op, message, protocol));
        }

        /// <summary>
        /// Adds a handler for the OnItemAdd event, and calls the handler for any existing
        /// items.  To prevent races, use this rather than .OnItemAdd +=.
        /// </summary>
        /// <param name="callback">Callback to call with every item.</param>
        public void AddItemAddCallback(ItemCB callback)
        {
            if (callback == null)
                throw new ArgumentException("Must not be null", "callback");

            OnItemAdd += callback;
            foreach (PubSubItem item in m_items)
            {
                callback(this, item);
            }
        }

        /// <summary>
        /// Creates the node then subscribes. If the creation succeeded, or if the node
        /// already exists, retrieve the items for the node.
        ///
        /// This is the typical starting point.  Please make sure to register callbacks before calling
        /// this function.
        /// </summary>
        public void AutomatedSubscribe()
        {
            lock (this)
            {
                if ((this[Op.SUBSCRIBE] == STATE.Start) || (this[Op.SUBSCRIBE] == STATE.Error))
                    this[Op.SUBSCRIBE] = STATE.Pending;
                if ((this[Op.ITEMS] == STATE.Start) || (this[Op.ITEMS] == STATE.Error))
                    this[Op.ITEMS] = STATE.Pending;
            }
            Create();
        }

        /// <summary>
        /// Creates the node with default configuration.
        /// </summary>
        public void Create()
        {
            lock (this)
            {
                if (!NeedsAsking(this[Op.CREATE]))
                {
                    SubscribeIfPending();
                    return;
                }

                this[Op.CREATE] = STATE.Asking;
            }
/*
<iq type='set'
    from='hamlet@denmark.lit/elsinore'
    to='pubsub.shakespeare.lit'
    id='create1'>
    <pubsub xmlns='http://jabber.org/protocol/pubsub'>
      <create node='princely_musings'/>
      <configure/>
    </pubsub>
</iq>
 */
            PubSubIQ iq = new PubSubIQ(m_stream.Document, PubSubCommandType.create, m_node);
            iq.To = m_jid;
            iq.Type = IQType.set;
            iq.Query.AppendChild(m_stream.Document.CreateElement("configure", URI.PUBSUB));
            m_stream.Tracker.BeginIQ(iq, GotCreated, null);
        }

        private void GotCreated(object sender, IQ iq, object state)
        {
            if (iq.Type != IQType.result)
            {
                // Type=error with conflict is basically a no-op.
                if (iq.Type != IQType.error)
                {
                    FireError(Op.CREATE, "Create failed, invalid protocol", iq);
                    return;
                }
                Error err = iq.Error;
                if (err == null)
                {
                    FireError(Op.CREATE, "Create failed, unknown error", iq);
                    return;
                }
                if (err.Condition != Error.CONFLICT)
                {
                    FireError(Op.CREATE, "Error creating node", iq);
                    return;
                }
            }
            else
            {
                PubSub ps = iq.Query as PubSub;
                if (ps != null)
                {
                    Create c = ps["create", URI.PUBSUB] as Create;
                    if (c != null)
                        m_node = c.Node;
                }
            }

            this[Op.CREATE] = STATE.Running;
            SubscribeIfPending();
        }

        private bool NeedsAsking(STATE state)
        {
            switch (state)
            {
            case STATE.Start:
            case STATE.Pending:
                return true;
            case STATE.Asking:
            case STATE.Running:
                return false;
            case STATE.Error:
                Debug.WriteLine("Retrying create after error.  Hope you've changed perms or something in the mean time.");
                return true;
            }
            return true;
        }

        private void SubscribeIfPending()
        {
            if (this[Op.SUBSCRIBE] == STATE.Pending)
                Subscribe();
        }

        /// <summary>
        /// Sends a subscription request.
        /// Items request will be sent automatically on successful subscribe.
        /// </summary>
        public void Subscribe()
        {
            lock (this)
            {
                if (!NeedsAsking(this[Op.SUBSCRIBE]))
                    return;

                this[Op.SUBSCRIBE] = STATE.Asking;
            }

            PubSubIQ iq = createCommand(PubSubCommandType.subscribe);
            addInfo(iq);
            m_stream.Tracker.BeginIQ(iq, GotSubscribed, null);
            // don't parallelize getItems, in case sub fails.
        }

        private PubSubIQ createCommand(PubSubCommandType type)
        {
            m_stream.OnProtocol += m_stream_OnProtocol;
            PubSubIQ iq = new PubSubIQ(m_stream.Document, type, m_node);
            iq.To = m_jid;
            iq.Type = IQType.set;

            return iq;
        }

        private void addInfo(PubSubIQ iq)
        {
            Subscribe sub = (Subscribe) iq.Command;
            sub.JID = m_stream.JID;
        }

        private void GotSubscribed(object sender, IQ iq, object state)
        {
            if (iq.Type != IQType.result)
            {
                FireError(Op.SUBSCRIBE, "Subscription failed", iq);
                return;
            }

            PubSub ps = iq.Query as PubSub;
            if (ps == null)
            {
                FireError(Op.SUBSCRIBE, "Invalid protocol", iq);
                return;
            }

            PubSubSubscriptionType subType;
            PubSubSubscription sub = ps["subscription", URI.PUBSUB] as PubSubSubscription;
            if (sub != null)
                subType = sub.Type;
            else
            {
                XmlElement ent = ps["entity", URI.PUBSUB];
                if (ent == null)
                {
                    FireError(Op.SUBSCRIBE, "Invalid protocol", iq);
                    return;
                }
                string s = ent.GetAttribute("subscription");
                if (s == "")
                    subType = PubSubSubscriptionType.NONE_SPECIFIED;
                else
                    subType = (PubSubSubscriptionType)Enum.Parse(typeof(PubSubSubscriptionType), s);
            }

            switch (subType)
            {
            case PubSubSubscriptionType.NONE_SPECIFIED:
            case PubSubSubscriptionType.subscribed:
                break;
            case PubSubSubscriptionType.pending:
                FireError(Op.SUBSCRIBE, "Subscription pending authorization", iq);
                return;
            case PubSubSubscriptionType.unconfigured:
                FireError(Op.SUBSCRIBE, "Subscription configuration required.  Not implemented yet.", iq);
                return;
            }

            this[Op.SUBSCRIBE] = STATE.Running;
            GetItemsIfPending();
        }

        private void GetItemsIfPending()
        {
            if (this[Op.ITEMS] == STATE.Pending)
                GetItems();
        }

        /// <summary>
        /// Gets the items from the node on the XMPP server.
        /// </summary>
        public void GetItems()
        {
            lock (this)
            {
                if (!NeedsAsking(this[Op.ITEMS]))
                    return;
                this[Op.ITEMS] = STATE.Asking;
            }
            PubSubIQ piq = new PubSubIQ(m_stream.Document, PubSubCommandType.items, m_node);
            piq.To = m_jid;
            piq.Type = IQType.get;
            m_stream.Tracker.BeginIQ(piq, GotItems, null);
        }

        private void GotItems(object sender, IQ iq, object state)
        {
            if (iq.Type != IQType.result)
            {
                FireError(Op.ITEMS, "Error retrieving items", iq);
                return;
            }

            PubSub ps = iq["pubsub", URI.PUBSUB] as PubSub;
            if (ps == null)
            {
                FireError(Op.ITEMS, "Invalid pubsub protocol", iq);
                return;
            }
            PubSubItemCommand items = ps["items", URI.PUBSUB] as PubSubItemCommand;
            if (items == null)
            {
                // That doesn't really hurt us, I guess.  No items.  Keep going.
                this[Op.ITEMS] = STATE.Running;
                return;
            }

            if (items.Node != m_node)
            {
                FireError(Op.ITEMS, "Non-matching node.  Probably a server bug.", iq);
                return;
            }

            foreach (PubSubItem item in items.GetItems())
                m_items.Add(item);

            this[Op.ITEMS] = STATE.Running;
        }

        /// <summary>
        /// Notifies the client that an item has been add to this PubSubNode.
        /// </summary>
        /// <param name="item">Item that was added.</param>
        public void ItemAdded(PubSubItem item)
        {
            if (OnItemAdd != null)
                OnItemAdd(this, item);
        }

        /// <summary>
        /// Notifies the client that an item has been removed from this PubSubNode.
        /// </summary>
        /// <param name="item">Item that was removed.</param>
        public void ItemRemoved(PubSubItem item)
        {
            if (OnItemRemove != null)
                OnItemRemove(this, item);
        }

        /// <summary>
        /// Returns the contents of the specified item
        /// </summary>
        /// <param name="id">Index of the element to retrieve.</param>
        /// <returns>XmlElement contents.</returns>
        public XmlElement this[string id]
        {
            get { return m_items[id]; }
            set
            {
                // TODO: publish, and reset ID when it comes back.
                m_items[id] = value;
            }
        }

        private void m_stream_OnProtocol(object sender, XmlElement rp)
        {
            if (rp.Name != "message")
                return;
            PubSubEvent evt = rp["event", URI.PUBSUB_EVENT] as PubSubEvent;
            if (evt == null)
                return;

            Items items = evt.Items;
            if (items == null)
                return;
            if (items.Node != m_node)
                return;

            // OK, it's for us.  Might be a new one or a retraction.
            // Shrug, even if we're sent a mix, it shouldn't hurt anything.

            /*
            <message from='pubsub.shakespeare.lit' to='bernardo@denmark.lit' id='bar'>
              <event xmlns='http://jabber.org/protocol/pubsub#event'>
                <items node='princely_musings'>
                  <retract id='ae890ac52d0df67ed7cfdf51b644e901'/>
                </items>
              </event>
            </message>
             */
            foreach (string id in items.GetRetractions())
                m_items.RemoveId(id);

            foreach (PubSubItem item in items.GetItems())
                m_items.Add(item);
        }

        /// <summary>
        /// Unsubscribes from the node.
        /// </summary>
        public void Unsubscribe()
        {
            PubSubIQ iq = createCommand(PubSubCommandType.unsubscribe);
            m_stream.Tracker.BeginIQ(iq, GotUnsubsribed, null);
        }

        private void GotUnsubsribed(object sender, IQ iq, object data)
        {
            //TODO: Report back error
        }

        /// <summary>
        /// Deletes the node from the XMPP server.
        /// </summary>
        public void Delete()
        {
            PubSubIQ iq = createCommand(PubSubCommandType.delete);
            m_stream.Tracker.BeginIQ(iq, GotDelete, null);
        }

        private void GotDelete(object sender, IQ iq, object data)
        {
            if (iq.Type != IQType.result)
            {
                FireError(Op.DELETE, "Delete failed", iq);
                return;
            }
        }

        /// <summary>
        /// Deletes a single item from the XMPP server.
        /// </summary>
        /// <param name="id">Id of item.</param>
        public void DeleteItem(string id)
        {
            PubSubIQ iq = createCommand(PubSubCommandType.retract);
            Retract retract = (Retract)iq.Command;
            retract.AddItem(id);
            m_stream.Tracker.BeginIQ(iq, OnDeleteNode, null);
        }

        private void OnDeleteNode(object sender, IQ iq, object data)
        {
            if (iq.Type != IQType.result)
            {
                string msg = string.Format("Error deleting pubsub item: {0}", iq.Error.Condition);
                Debug.WriteLine(msg);

                if (OnError != null)
                    OnError(this, new PubSubException(Op.DELETE_ITEM, msg, iq));
                return;
            }
        }

        /// <summary>
        /// Publishes an item to the node.
        /// </summary>
        /// <param name="id">If null, the server will assign an item ID.</param>
        /// <param name="contents">The XML inside the item.  Should be in a new namespace.</param>
        public void PublishItem(string id, XmlElement contents)
        {
            PubSubIQ iq = createCommand(PubSubCommandType.publish);
            Publish pub = (Publish)iq.Command;
            PubSubItem item = new PubSubItem(m_stream.Document);
            if (id != null)
                item.ID = id;
            item.AddChild(contents);
            pub.AddChild(item);
            m_stream.Tracker.BeginIQ(iq, new IqCB(OnPublished), null);
        }

        private void OnPublished(object sender, IQ iq, object data)
        {
            if (iq.Type != IQType.result)
            {
                string msg = string.Format("Error publishing pubsub item: {0}", iq.Error.Condition);
                Debug.WriteLine(msg);

                if (OnError != null)
                    OnError(this, new PubSubException(Op.PUBLISH_ITEM, msg, iq));
                return;
            }
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        #endregion
    }
}
