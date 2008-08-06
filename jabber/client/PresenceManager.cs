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
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using gen = System.Collections.Generic;
using System.Diagnostics;

using bedrock.util;
using bedrock.collections;

using jabber.protocol.client;
using jabber.protocol.x;
using jabber.protocol.iq;
using jabber.connection;

namespace jabber.client
{

    /// <summary>
    /// Informs the client of a change of derived primary session for a user.
    /// </summary>
    /// <param name="sender">The PresenceManager object that sent the update</param>
    /// <param name="bare">The bare JID (node@domain) of the user whose presence changed</param>
    public delegate void PrimarySessionHandler(object sender, JID bare);

    /// <summary>
    /// Specifies the presence proxy database.
    /// </summary>
    [SVN(@"$Id$")]
    public class PresenceManager : jabber.connection.StreamComponent, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
#pragma warning disable 0414
        private System.ComponentModel.Container components = null;
#pragma warning restore 0414
 
        private Tree m_items = new Tree();
        private CapsManager m_caps = null;

        /// <summary>
        /// Constructs a PresenceManager object and adds it to a container.
        /// </summary>
        /// <param name="container">Parent container.</param>
        public PresenceManager(System.ComponentModel.IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Constructs a new PresenceManager object.
        /// </summary>
        public PresenceManager()
        {
            InitializeComponent();

            this.OnStreamChanged += new bedrock.ObjectHandler(PresenceManager_OnStreamChanged);
        }

        private void PresenceManager_OnStreamChanged(object sender)
        {
            JabberClient cli = m_stream as JabberClient;
            if (cli == null)
                return;

            cli.OnPresence += new PresenceHandler(GotPresence);
            cli.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
        }

        /// <summary>
        /// Gets or sets the JabberClient associated with the Presence Manager.
        /// </summary>
        [Description("Gets or sets the JabberClient associated with the Presence Manager.")]
        [Category("Jabber")]
        [Browsable(false)]
        [Obsolete("Use the Stream property instead")]
        [ReadOnly(true)]
        public JabberClient Client
        {
            get { return (JabberClient)this.Stream; }
            set { this.Stream = value; }
        }

        /// <summary>
        /// The RosterManager for this view
        /// </summary>
        [Category("Jabber")]
        public CapsManager CapsManager
        {
            get
            {
                // If we are running in the designer, let's try to auto-hook a CapsManager
                if ((m_caps == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    this.CapsManager = (CapsManager)jabber.connection.StreamComponent.GetComponentFromHost(host, typeof(CapsManager));
                }
                return m_caps;
            }
            set
            {
                if ((object)m_caps == (object)value)
                    return;
                m_caps = value;
            }
        }

        /// <summary>
        /// Gets the current presence state as a string.
        /// </summary>
        /// <returns>string in the format '{bare JID}={list of presence stanzas}, ...'</returns>
        public override string ToString()
        {
            return m_items.ToString();
        }

        /// <summary>
        /// Informs the client that the primary session has changed for a user.
        /// </summary>
        public event PrimarySessionHandler OnPrimarySessionChange;

        private void GotDisconnect(object sender)
        {
            lock(this)
                m_items.Clear();
        }

        /// <summary>
        /// Adds a new available or unavailable presence packet to the database.
        /// </summary>
        /// <param name="p">Presence stanza to add.</param>
        public void AddPresence(Presence p)
        {
            // can't use .From, since that will cause a JID parse.
            Debug.Assert(p.GetAttribute("from") != "",
                "Do not call AddPresence by hand.  I can tell you are doing that because you didn't put a from address on your presence packet, and all presences from the server have a from address.");
            GotPresence(this, p);
        }

        private void GotPresence(object sender, Presence p)
        {
            PresenceType t = p.Type;
            if ((t != PresenceType.available) &&
                (t != PresenceType.unavailable))
                return;

            JID f = p.From;
            lock (this)
            {
                UserPresenceManager upm = (UserPresenceManager)m_items[f.Bare];

                if (t == PresenceType.available)
                {
                    if (upm == null)
                    {
                        upm = new UserPresenceManager(f.Bare);
                        m_items[f.Bare] = upm;
                    }

                    upm.AddPresence(p, this);
                }
                else
                {
                    if (upm != null)
                    {
                        upm.RemovePresence(p, this);
                        if (upm.Count == 0)
                        {
                            m_items.Remove(f.Bare);
                        }
                    }
                }
            }
        }

        private void FireOnPrimarySessionChange(JID from)
        {
            if (OnPrimarySessionChange != null)
                OnPrimarySessionChange(this, from);
        }

        /// <summary>
        /// Determines if a specified JID is online with any resources.
        /// This performs better than retrieving the particular associated presence packet.
        /// </summary>
        /// <param name="jid">The JID to look up.</param>
        /// <returns>If true, the user is online; otherwise the user is offline</returns>
        public bool IsAvailable(JID jid)
        {
            lock (this)
            {
                return (m_items[jid.Bare] != null);
            }
        }

        /// <summary>
        /// Gets the primary presence if given a bare JID.
        /// If given a FQJ, returns the associated presence.
        /// </summary>
        public Presence this[JID jid]
        {
            get
            {
                lock (this)
                {
                    UserPresenceManager upm = (UserPresenceManager)m_items[jid.Bare];
                    if (upm == null)
                        return null;
                    return upm[jid.Resource];
                }
            }
        }

        /// <summary>
        /// Get the features associated with the JID.  If a bare JID is passed in, this will be 
        /// a union of all of the features for all of the resources of this user.  Otherwise,
        /// it will be the features for the given resource.
        /// 
        /// Requires a CapsManager to be set before use.
        /// </summary>
        /// <param name="jid"></param>
        /// <returns>Null if no features are known.</returns>
        public StringSet GetFeatures(JID jid)
        {
            if (m_caps == null)
                return null;

            lock (this)
            {
                UserPresenceManager upm = (UserPresenceManager)m_items[jid.Bare];
                if (upm == null)
                    return null;
                return upm.GetFeatures(m_caps, jid.Resource);
            }
        }

        /// <summary>
        /// Does the given JID implement the given feature?  Bare JID asks if any
        /// resource of that user implements that feature.  Full JID asks if the
        /// given resource implements that feature.
        /// 
        /// Requires a CapsManager to be set before use.
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="featureURI"></param>
        /// <returns>True if the feaure is implemented</returns>
        public bool HasFeature(JID jid, string featureURI)
        {
            StringSet feats = GetFeatures(jid);
            if (feats == null)
                return false;
            return feats[featureURI];
        }

        /// <summary>
        /// Get the most available full JID that implements the given feature.  Unlike
        /// most routines in PresenceManager, may also return JIDs that have negative
        /// presence.  If a full JID is specified, this is effectively the same as
        /// HasFeature, but null will be returned if the feature isn't implemented.
        /// 
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="featureURI"></param>
        /// <returns>null if none found</returns>
        public JID GetFeatureJID(JID jid, string featureURI)
        {
            if (jid.Resource != null)
            {
                if (HasFeature(jid, featureURI))
                    return jid;
                return null;
            }

            lock (this)
            {
                UserPresenceManager upm = (UserPresenceManager)m_items[jid.Bare];
                if (upm == null)
                    return null;
                return upm.GetFeatureJID(m_caps, featureURI);
            }
        }

        /// <summary>
        /// Gets all of the current presence stanzas for the given user.
        /// </summary>
        /// <param name="jid">User who's presence stanzas you want.</param>
        /// <returns>Array of presence stanzas for the given user.</returns>
        public Presence[] GetAll(JID jid)
        {
            UserPresenceManager upm = (UserPresenceManager)m_items[jid.Bare];
            if (upm == null)
                return new Presence[0];
            return upm.GetAll();
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
        /// Iterate over all of the JIDs we have not-unavilable presence from.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new UserPresenceManagerEnumerator(m_items.Values);
        }

        private class UserPresenceManagerEnumerator : IEnumerator
        {
            private IEnumerator m_enum;

            public UserPresenceManagerEnumerator(ICollection values)
            {
                m_enum = values.GetEnumerator();
            }

            #region IEnumerator Members
            public object Current
            {
                get
                {
                    UserPresenceManager m = (UserPresenceManager)m_enum.Current;
                    if (m == null)
                        return null;
                    return m.JID;
                }
            }

            public bool MoveNext()
            {
                return m_enum.MoveNext();
            }

            public void Reset()
            {
                m_enum.Reset();
            }

            #endregion
        }

        /// <summary>
        /// Manage the presence for all of the resources of a user.  No locking is performed,
        /// since PresenceManager is already doing locking.
        ///
        /// The intent of this class is to be able to deliver the last presence stanza
        /// from the "most available" resource.
        /// Note that negative priority sessions are never the most available.
        /// </summary>
        private class UserPresenceManager
        {
            // List sorted by presence availability, in ascending order.
            // most-available is always last.
            private gen.LinkedList<Presence> m_all = new gen.LinkedList<Presence>();
            private JID m_jid = null;

            public UserPresenceManager(JID jid)
            {
                Debug.Assert(jid.Resource == null);
                m_jid = jid;
            }

            public JID JID
            {
                get { return m_jid; }
            }

            public override string ToString()
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();
                sw.WriteLine("{");
                foreach (Presence p in m_all)
                    sw.WriteLine(p.OuterXml);
                sw.WriteLine("}");
                return sw.ToString();
            }

            private void Primary(Presence p, PresenceManager handler)
            {
                Debug.Assert((p == null) ? true : (p.IntPriority >= 0), "Primary presence is always positive priority");
                handler.FireOnPrimarySessionChange(m_jid);
            }

            private gen.LinkedListNode<Presence> Find(string resource)
            {
                for (gen.LinkedListNode<Presence> n = m_all.First; n != null; n = n.Next)
                {
                    if (n.Value.From.Resource == resource)
                        return n;
                }
                return null;
            }

            public void AddPresence(Presence p, PresenceManager handler)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.available);

                // If this is an update, remove the existing one.
                // we'll add the new one back in, in the correct place.
                gen.LinkedListNode<Presence> n = Find(res);
                if (n != null)
                    m_all.Remove(n);


                gen.LinkedListNode<Presence> inserted = new gen.LinkedListNode<Presence>(p);
                for (n = m_all.First; n != null; n = n.Next)
                {
                    if (p < n.Value)
                    {
                        m_all.AddBefore(n, inserted);
                        break;
                    }
                }

                // This is the highest one.
                if (inserted.List == null)
                {
                    m_all.AddLast(inserted);
                    if (p.IntPriority >= 0)
                        Primary(p, handler);
                }
            }

            public void RemovePresence(Presence p, PresenceManager handler)
            {
                JID from = p.From;
                string res = from.Resource;
                Debug.Assert(p.Type == PresenceType.unavailable);

                gen.LinkedListNode<Presence> n = Find(res);

                // unavail for a resource we haven't gotten presence from.
                if (n == null)
                    return;

                gen.LinkedListNode<Presence> last = m_all.Last;
                m_all.Remove(n);

                if (last == n)
                {
                    // current high-pri.
                    if ((m_all.Last != null) && (m_all.Last.Value.IntPriority >= 0))
                        Primary(m_all.Last.Value, handler);
                    else
                    {
                        // last non-negative presence went away
                        if (n.Value.IntPriority >= 0)
                            Primary(null, handler);
                    }
                }
            }

            public int Count
            {
                get { return m_all.Count; }
            }

            public Presence this[string Resource]
            {
                get
                {
                    gen.LinkedListNode<Presence> n;
                    if (Resource == null)
                    {
                        // get highest non-negative for this bare JID.
                        n = m_all.Last;

                        if ((n != null) && (n.Value.IntPriority >= 0))
                            return n.Value;
                    }
                    else
                    {
                        n = Find(Resource);
                        if (n != null)
                            return n.Value;
                    }
                    return null;
                }
            }

            public Presence[] GetAll()
            {
                Presence[] all = new Presence[m_all.Count];
                m_all.CopyTo(all, 0);
                return all;
            }

            private StringSet GetFeatures(CapsManager caps, Presence p)
            {
                if (p == null)
                    return null;

                Caps c = p.GetChildElement<Caps>();
                if (c == null)
                    return null;

                DiscoInfo di = caps[c.Version];
                if (di == null)
                    return null;

                return di.FeatureSet;
            }

            public StringSet GetFeatures(CapsManager caps, string resource)
            {
                if (resource == null)
                    return GetAllFeatures(caps);

                return GetFeatures(caps, this[resource]);
            }

            public StringSet GetAllFeatures(CapsManager caps)
            {
                if (caps == null)
                    throw new ArgumentNullException("caps");

                StringSet features = new StringSet();
                foreach (Presence p in m_all)
                {
                    StringSet f = GetFeatures(caps, p);
                    if (f != null)
                        features.Add(f);
                }
                return features;
            }

            public JID GetFeatureJID(CapsManager caps, string featureURI)
            {
                gen.LinkedListNode<Presence> n;
                for (n = m_all.Last; n != null; n = n.Previous)
                {
                    StringSet f = GetFeatures(caps, n.Value);
                    if ((f != null) && f.Contains(featureURI))
                        return n.Value.From;
                }
                return null;
            }
        }
    }
}
