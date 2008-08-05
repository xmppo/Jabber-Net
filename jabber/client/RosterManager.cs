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

using bedrock.collections;
using bedrock.util;

using jabber.protocol.client;
using jabber.protocol.iq;

namespace jabber.client
{
    /// <summary>
    /// Informs the client of roster items.
    /// </summary>
    public delegate void RosterItemHandler(object sender, Item ri);

    /// <summary>
    /// Informs the client of a subscription requests.
    /// </summary>
    /// <param name="manager">The RosterManager than detected the subscription</param>
    /// <param name="ri">The affected roster item, in its current state.  Null if not found.</param>
    /// <param name="pres">The inbound presence stanza</param>
    public delegate void SubscriptionHandler(RosterManager manager, Item ri, Presence pres);

    /// <summary>
    /// Manages unsubscription notifications.
    /// </summary>
    /// <param name="manager">The RosterManager than detected the subscription</param>
    /// <param name="remove">Set this to false to prevent the user being removed from the roster.</param>
    /// <param name="pres">The inbound presence stanza</param>
    public delegate void UnsubscriptionHandler(RosterManager manager, Presence pres, ref bool remove);

    /// <summary>
    /// Determines how the RosterManager deals with incoming subscriptions.
    /// </summary>
    public enum AutoSubscriptionHanding
    {
        /// <summary>
        /// Do not do any automatic processing
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Reply with a subscribed to every subscribe
        /// </summary>
        AllowAll,
        /// <summary>
        /// Reply with an unsubscribed to every subscribe
        /// </summary>
        DenyAll,
        /// <summary>
        /// If the user is either subscribed or trying to subscribe to another user,
        /// allow the other user's subscription.
        /// Otherwise, treat as NONE, and fire the OnSubscribe event.
        /// </summary>
        AllowIfSubscribed,
    }

    /// <summary>
    /// Manages the roster of the client.
    /// </summary>
    [SVN(@"$Id$")]
    public class RosterManager : jabber.connection.StreamComponent, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
#pragma warning disable 0414
        private System.ComponentModel.Container components = null;
#pragma warning restore 0414
        private Tree m_items = new Tree();
        private AutoSubscriptionHanding m_autoAllow = AutoSubscriptionHanding.NONE;
        private bool m_autoSubscribe = false;

        /// <summary>
        /// Creates a new roster manager inside a container.
        /// </summary>
        /// <param name="container">Parent container</param>
        public RosterManager(System.ComponentModel.IContainer container) : this()
        {
            // Required for Windows.Forms Class Composition Designer support
            container.Add(this);
        }

        /// <summary>
        /// Creates a new roster manager.
        /// </summary>
        public RosterManager()
        {
            // Required for Windows.Forms Class Composition Designer support
            InitializeComponent();
            this.OnStreamChanged += new bedrock.ObjectHandler(RosterManager_OnStreamChanged);
        }

        private void RosterManager_OnStreamChanged(object sender)
        {
            JabberClient cli = m_stream as JabberClient;
            if (cli == null)
                return;
            cli.OnIQ += new IQHandler(GotIQ);
            cli.OnPresence += new PresenceHandler(cli_OnPresence);
            cli.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
        }


        /// <summary>
        /// Gets or sets the Jabber client associated with the Roster Manager.
        /// </summary>
        [Description("The JabberClient to hook up to.")]
        [Category("Jabber")]
        [Browsable(false)]
        [Obsolete("Use the Stream property instead")]
        [ReadOnly(true)]
        public JabberClient Client
        {
            get { return (JabberClient) this.Stream; }
            set { this.Stream = value; }
        }

        /// <summary>
        /// Gets the AutoSubscription Handling value for inbound subscriptions.
        /// </summary>
        [Description("How to handle inbound subscriptions")]
        [Category("Jabber")]
        [DefaultValue(AutoSubscriptionHanding.NONE)]
        public AutoSubscriptionHanding AutoAllow
        {
            get { return m_autoAllow; }
            set { m_autoAllow = value; }
        }

        /// <summary>
        /// Determines whether auto-subscribe is enabled or disabled for a user.
        /// </summary>
        [Description("Should we subscribe to a user whenever we allow a subscription from them?")]
        [Category("Jabber")]
        [DefaultValue(false)]
        public bool AutoSubscribe
        {
            get { return m_autoSubscribe; }
            set { m_autoSubscribe = value; }
        }

        /// <summary>
        /// Informs the client that is has new roster items.
        /// </summary>
        [Description("Convenience event for new roster items.")]
        [Category("Jabber")]
        public event RosterItemHandler OnRosterItem;

        /// <summary>
        /// Informs the client when a roster result starts, before any OnRosterItem events fire.
        /// This will not fire for type='set'.
        /// </summary>
        [Description("Roster result about to start being processed.")]
        [Category("Jabber")]
        public event bedrock.ObjectHandler OnRosterBegin;

        /// <summary>
        /// Informs the client that the roster has been retrieved from the XMPP server.
        /// </summary>
        [Description("Roster result finished being processed.")]
        [Category("Jabber")]
        public event bedrock.ObjectHandler OnRosterEnd;

        /// <summary>
        /// Informs the client that a subscription request was received that cannot be auto-handled.
        /// </summary>
        [Description("Subscription request received that cannot be auto-handled")]
        [Category("Jabber")]
        public event SubscriptionHandler OnSubscription;

        /// <summary>
        /// Informs the client that an Unsubscribe/Unsubscribed notification from another
        /// user. By default, the user will be removed from the roster after this event
        /// fires. To prevent this, you need to set the remove property to false.
        /// </summary>
        [Description("Unsubscribe/Unsubscribed notification from other user")]
        [Category("Jabber")]
        public event UnsubscriptionHandler OnUnsubscription;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return m_items.ToString();
        }

        /// <summary>
        /// Gets the currently-known version of a roster item for this JID.
        /// </summary>
        public Item this[JID jid]
        {
            get
            {
                lock (this)
                    return (Item) m_items[jid];
            }
        }

        /// <summary>
        /// Gets the number of items currently in the roster.
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                    return m_items.Count;
            }
        }

        private void GotDisconnect(object sender)
        {
            lock (this)
                m_items.Clear();
        }

        private void cli_OnPresence(object sender, Presence pres)
        {
            PresenceType typ = pres.Type;
            switch (typ)
            {
            case PresenceType.available:
            case PresenceType.unavailable:
            case PresenceType.error:
            case PresenceType.probe:
                return;
            case PresenceType.subscribe:
                switch (m_autoAllow)
                {
                case AutoSubscriptionHanding.AllowAll:
                    ReplyAllow(pres);
                    return;
                case AutoSubscriptionHanding.DenyAll:
                    ReplyDeny(pres);
                    return;
                case AutoSubscriptionHanding.NONE:
                    if (OnSubscription != null)
                        OnSubscription(this, this[pres.From], pres);
                    return;
                case AutoSubscriptionHanding.AllowIfSubscribed:
                    Item ri = this[pres.From];
                    if (ri != null)
                    {
                        switch (ri.Subscription)
                        {
                        case Subscription.to:
                            ReplyAllow(pres);
                            return;
                        case Subscription.from:
                        case Subscription.both:
                            // Almost an assert
                            throw new InvalidOperationException("Server sent a presence subscribe for an already-subscribed contact");
                        case Subscription.none:
                            if (ri.Ask == Ask.subscribe)
                            {
                                ReplyAllow(pres);
                                return;
                            }
                            break;
                        }
                    }
                    if (OnSubscription != null)
                        OnSubscription(this, ri, pres);
                    break;
                }
                break;
            case PresenceType.subscribed:
                // This is the new ack case.
                Presence sub_ack = new Presence(m_stream.Document);
                sub_ack.To = pres.From;
                sub_ack.Type = PresenceType.subscribe;
                Write(sub_ack);                
                break;
            case PresenceType.unsubscribe:
                // ack.  we'll likely get an unsubscribed soon, anyway.
                Presence un_ack = new Presence(m_stream.Document);
                un_ack.To = pres.From;
                un_ack.Type = PresenceType.unsubscribed;
                Write(un_ack);
                break;
            case PresenceType.unsubscribed:
                bool remove = true;
                if (OnUnsubscription != null)
                    OnUnsubscription(this, pres, ref remove);

                if (remove)
                    Remove(pres.From);
                break;
            }
        }

        /// <summary>
        /// Adds a new roster item to the database.
        /// </summary>
        /// <param name="iq">An IQ containing a roster query.</param>
        public void AddRoster(IQ iq)
        {
            GotIQ(this, iq);
        }

        private void GotIQ(object sender, IQ iq)
        {
            if ((iq.Query == null) ||
                (iq.Query.NamespaceURI != jabber.protocol.URI.ROSTER) ||
                ((iq.Type != IQType.result) && (iq.Type != IQType.set)))
                return;

            iq.Handled = true;
            Roster r = (Roster) iq.Query;
            if ((iq.Type == IQType.result) && (OnRosterBegin != null))
                OnRosterBegin(this);

            foreach (Item i in r.GetItems())
            {
                lock (this)
                {
                    if (i.Subscription == Subscription.remove)
                    {
                        m_items.Remove(i.JID);
                    }
                    else
                    {
                        if (m_items.Contains(i.JID))
                            m_items.Remove(i.JID);
                        m_items[i.JID] = i;
                    }
                }
                if (OnRosterItem != null)
                    OnRosterItem(this, i);
            }

            if ((iq.Type == IQType.result) && (OnRosterEnd != null))
                OnRosterEnd(this);
        }

        /// <summary>
        /// Allows the subscription request and sends a subscribed to the user.
        /// </summary>
        /// <param name="pres">
        /// The presence packet containing the subscription request.
        /// </param>
        public void ReplyAllow(Presence pres)
        {
            Debug.Assert(pres.Type == PresenceType.subscribe);
            Presence reply = new Presence(m_stream.Document);
            reply.To = pres.From;
            reply.Type = PresenceType.subscribed;
            Write(reply);

            if (m_autoSubscribe)
            {
                Presence sub = new Presence(m_stream.Document);
                sub.To = pres.From;
                sub.Type = PresenceType.subscribe;
                Write(sub);
            }
        }

        /// <summary>
        /// Denies the subscription request.
        /// </summary>
        /// <param name="pres">
        /// The presence packet containing the subscription request.
        /// </param>
        public void ReplyDeny(Presence pres)
        {
            Debug.Assert(pres.Type == PresenceType.subscribe);
            Presence reply = new Presence(m_stream.Document);
            reply.To = pres.From;
            reply.Type = PresenceType.unsubscribed;
            Write(reply);
        }

        /// <summary>
        /// Remove a contact from the roster
        /// </summary>
        /// <param name="jid">Typically just a user@host JID</param>
        public void Remove(JID jid)
        {
/*
C: <iq from='juliet@example.com/balcony' type='set' id='delete_1'>
     <query xmlns='jabber:iq:roster'>
       <item jid='nurse@example.com' subscription='remove'/>
     </query>
   </iq>
 */
            RosterIQ iq = new RosterIQ(m_stream.Document);
            iq.Type = IQType.set;
            Roster r = iq.Instruction;
            Item item = r.AddItem();
            item.JID = jid;
            item.Subscription = Subscription.remove;
            Write(iq);  // ignore response
        }

        /// <summary>
        /// Modifies the roster item to look like the given roster item.
        /// This does not modify the model,
        /// but waits for roster pushes from the XMPP server.
        /// </summary>
        /// <param name="item">Roster item that will appear in the roster.</param>
        public void Modify(Item item)
        {
            RosterIQ iq = new RosterIQ(m_stream.Document);
            iq.Type = IQType.set;
            Roster r = iq.Instruction;
            r.AppendChild(item);
            Write(iq);  // ignore response
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

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.Keys.GetEnumerator();
        }

        #endregion
    }
}
