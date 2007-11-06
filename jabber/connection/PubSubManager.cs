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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

using bedrock.util;
using bedrock.collections;

using jabber;
using jabber.protocol;
using jabber.protocol.iq;
using jabber.protocol.client;

namespace jabber.connection
{
    /// <summary>
    /// Manage a set of PubSub (<a href="http://www.xmpp.org/extensions/xep-0060.html">XEP-60</a>) subscriptions.
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
        private System.ComponentModel.IContainer components = null;
        private Hashtable m_nodes = new Hashtable();

        /// <summary>
        /// Create a manager
        /// </summary>
        public PubSubManager()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Create a manager in a container
        /// </summary>
        /// <param name="container"></param>
		public PubSubManager(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Subscribe to a pubsub node.  
        /// If there is already a subscription, the existing node will be returned.
        /// If not, the PubSubNode will be returned in a subscribing state.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="node"></param>
        /// <param name="maxItems">Maximum number of items to retain.  First one to call Subscribe gets their value, for now.</param>
        /// <returns></returns>
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
	}

    /// <summary>
    /// Notification about a PubSub item.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="item"></param>
    public delegate void ItemCB(PubSubNode node, PubSubItem item);

    /// <summary>
    /// A list of items with a maximum size.  Only one item with a given id will be in the 
    /// list at a given time.
    /// </summary>
    public class ItemList : ArrayList
    {
        private System.Collections.Hashtable m_index = new System.Collections.Hashtable();
        private PubSubNode m_node = null;

        /// <summary>
        /// Create an item list, which will have at most some number of items.
        /// </summary>
        /// <param name="node">The node to which this item list applies</param>
        /// <param name="maxItems">Max size of the list.  Delete notifications will be sent if this size is exceeded.</param>
        public ItemList(PubSubNode node, int maxItems) : base(maxItems)
        {
            m_node = node;
        }

        /// <summary>
        /// Makes sure that the underlying id index is in sync
        /// when an item is removed.
        /// </summary>
        /// <param name="index"></param>
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
            for (int i=index; i<this.Count; i++)
            {
                item = (PubSubItem)this[i];
                id = item.ID;
                if (id != "")
                    m_index[id] = i;
            }
        }

        /// <summary>
        /// Add to the end of the list, replacing any item with the same ID, 
        /// or bumping the oldest item if the list is full.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override int Add(object value)
        {
            PubSubItem item = value as PubSubItem;
            if (item == null)
                throw new ArgumentException("Must be an XmlElement", "value");
            string id = item.ID;
            int i;
            if (id == "")
            {
                if (this.Count == this.Capacity)
                {
                    this.RemoveAt(0);
                }
                i = base.Add(value);
                m_node.ItemAdded(item);
                return i;
            }

            RemoveId(id);
            if (this.Count == this.Capacity)
                this.RemoveAt(0);

            i = base.Add(value);
            m_index[id] = i;
            m_node.ItemAdded(item);
            return i;
        }

        /// <summary>
        /// Remove the item with the given ID.  No-op if none found with that ID.
        /// </summary>
        /// <param name="id">ID of the item to remove</param>
        public void RemoveId(string id)
        {
            object index = m_index[id];
            if (index != null)
                this.RemoveAt((int)index);
        }
    }


    /// <summary>
    /// Different possible operations on a pubsub node.
    /// </summary>
    public enum Op
    {
        /// <summary>
        /// Create a node
        /// </summary>
        CREATE,
        /// <summary>
        /// Subscribe to a node
        /// </summary>
        SUBSCRIBE,
        /// <summary>
        /// Get the current items in the node
        /// </summary>
        ITEMS,
    }

    /// <summary>
    /// A pubsub error occurred.
    /// </summary>
    public class PubSubException : Exception
    {
        /// <summary>
        /// The stanza that caused the error.
        /// </summary>
        public XmlElement Protocol = null;
        /// <summary>
        /// The operation that failed.
        /// </summary>
        public Op Operation;

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="op"></param>
        /// <param name="error"></param>
        /// <param name="elem"></param>
        public PubSubException(Op op, string error, XmlElement elem) : base(error)
        {
            Operation = op;
            Protocol = elem;
        }

        /// <summary>
        /// Get a better error string.
        /// </summary>
        public override string Message
        {
            get
            { return string.Format("PubSub error on {0}: {1}\r\nAssociated protocol: {2}", Operation, base.Message, Protocol); }
        }
    }


    /// <summary>
    /// A node to be subscribed to.  Will keep a maximum number of items.
    /// </summary>
    [SVN(@"$Id$")]
    public class PubSubNode
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
        private STATE[] m_state = new STATE[] { STATE.Start, STATE.Start, STATE.Start, };

        private XmppStream  m_stream = null;
        private JID         m_jid = null;
        private string      m_node = null;
        private ItemList    m_items = null;

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
        /// Is this node fully initialized?
        /// </summary>
        public bool IsInitialized
        {
            get { return (this[Op.CREATE] == STATE.Running) && (this[Op.SUBSCRIBE] == STATE.Running); }
        }

        /// <summary>
        /// An item has been added to the node, either on initial get or by notification.
        /// </summary>
        public event ItemCB OnItemAdd;

        /// <summary>
        /// An item has been deleted from the node, either by notification, the 
        /// list is full, or when a new item replaces an old one with the same ID.
        /// </summary>
        public event ItemCB OnItemRemove;

        /// <summary>
        /// An error occurred.
        /// </summary>
        public event bedrock.ExceptionHandler OnError;


        private STATE this[Op op]
        {
            get { return m_state[(int)op]; }
            set { m_state[(int)op] = value; }
        }

        private void FireError(Op op, string message, XmlElement protocol)
        {
            Debug.WriteLine(string.Format("Error {0}ing pubsub node: {1}", op.ToString(), message));
            this[op] = STATE.Error;

            if (OnError != null)
                OnError(this, new PubSubException(op, message, protocol));
        }

        /// <summary>
        /// Add a handler for the OnItemAdd event, and call the handler for any existing
        /// items.  To prevent races, use this rather than .OnItemAdd +=.
        /// </summary>
        /// <param name="callback"></param>
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
        /// Create the node, then subscribe if the creation succeeded or the node already existed,
        /// then retrieve the items for the node.
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
        /// Create the node, with default configuration.  
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
            m_stream.Tracker.BeginIQ(iq, new IqCB(GotCreated), null);
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
                SubscribeIfPending();
                return;
            }

            PubSub ps = iq.Query as PubSub;
            if (ps == null)
            {
                FireError(Op.CREATE, "Invalid protocol", iq);
                return;
            }
            Create c = ps["create", URI.PUBSUB] as Create;
            if (c == null)
            {
                FireError(Op.CREATE, "Invalid protocol", iq);
                return;
            }
            m_node = c.Node;
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
        /// Send a subscription request.  Items request will be sent automatically on successful subscribe.
        /// </summary>
        public void Subscribe()
        {
            lock (this)
            {
                if (!NeedsAsking(this[Op.SUBSCRIBE]))
                    return;

                this[Op.SUBSCRIBE] = STATE.Asking;
            }

            m_stream.OnProtocol += new ProtocolHandler(m_stream_OnProtocol);
            PubSubIQ iq = new PubSubIQ(m_stream.Document, PubSubCommandType.subscribe, m_node);
            iq.To = m_jid;
            iq.Type = IQType.set;
            
            jabber.protocol.iq.Subscribe sub = (jabber.protocol.iq.Subscribe) iq.Command;
            sub.JID = m_stream.JID;
            m_stream.Tracker.BeginIQ(iq, new IqCB(GotSubscribed), null);

            // don't parallelize getItems, in case sub fails.
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

            GetItemsIfPending();
        }

        private void GetItemsIfPending()
        {
            if (this[Op.ITEMS] == STATE.Pending)
                GetItems();
        }

        /// <summary>
        /// Get the items from the node
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
            m_stream.Tracker.BeginIQ(piq, new IqCB(GotItems), null);
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
        /// Just for notifications.  Probably shouldn't be called except by ItemList.
        /// </summary>
        /// <param name="item"></param>
        public void ItemAdded(PubSubItem item)
        {
            if (OnItemAdd != null)
                OnItemAdd(this, item);
        }

        /// <summary>
        /// Just for notifications.  Probably shouldn't be called except by ItemList.
        /// </summary>
        /// <param name="item"></param>
        public void ItemRemoved(PubSubItem item)
        {
            if (OnItemRemove != null)
                OnItemRemove(this, item);
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
        /// Unsubscribe from the node.
        /// </summary>
        public void Unsubscribe()
        {
        }

        /// <summary>
        /// Delete the node.
        /// </summary>
        public void Delete()
        {
        }
    }
}
