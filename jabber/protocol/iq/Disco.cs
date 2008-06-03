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
using System.Xml;

using bedrock.util;
using jabber.protocol.x;
using bedrock.collections;

namespace jabber.protocol.iq
{
    /*
     * <iq
     *     type='result'
     *     from='shakespeare.lit'
     *     to='romeo@montague.net/orchard'
     *     id='items1'>
     *   <query xmlns='http://jabber.org/protocol/disco#items' node='music'>
     *     <item
     *         jid='people.shakespeare.lit'
     *         name='Directory of Characters'/>
     *     <item
     *         jid='plays.shakespeare.lit'
     *         name='Play-Specific Chatrooms'/>
     * </iq>
     */
    /// <summary>
    /// IQ packet with a disco#items query element inside.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoItemsIQ : jabber.protocol.client.TypedIQ<DiscoItems>
    {
        /// <summary>
        /// Create a disco#items IQ
        /// </summary>
        /// <param name="doc"></param>
        public DiscoItemsIQ(XmlDocument doc) : base(doc)
        {
        }

        /// <summary>
        /// The node on the query.
        /// </summary>
        public string Node
        {
            get { return this.Instruction.Node; }
            set { this.Instruction.Node = value; }
        }
    }

    /// <summary>
    /// IQ packet with a disco#info query element inside.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoInfoIQ : jabber.protocol.client.TypedIQ<DiscoInfo>
    {
        /// <summary>
        /// Create a disco#items IQ
        /// </summary>
        /// <param name="doc"></param>
        public DiscoInfoIQ(XmlDocument doc) : base(doc)
        {
        }

        /// <summary>
        /// The node on the query.
        /// </summary>
        public string Node
        {
            get {return this.Instruction.Node; }
            set { this.Instruction.Node = value; }
        }
    }

    /*
     * <iq
     *     type='result'
     *     from='plays.shakespeare.lit'
     *     to='romeo@montague.net/orchard'
     *     id='info1'>
     *   <query xmlns='http://jabber.org/protocol/disco#info'>
     *     <identity
     *         category='conference'
     *         type='text'
     *         name='Play-Specific Chatrooms'/>
     *     <identity
     *         category='directory'
     *         type='room'
     *         name='Play-Specific Chatrooms'/>
     * 
     *     <feature var='gc-1.0'/>
     *     <feature var='http://jabber.org/protocol/muc'/>
     *     <feature var='jabber:iq:register'/>
     *     <feature var='jabber:iq:search'/>
     *     <feature var='jabber:iq:time'/>
     *     <feature var='jabber:iq:version'/>
     *   </query>
     * </iq>
     */


    /// <summary>
    /// A disco#items query element.
    /// See <a href="http://www.xmpp.org/extensions/xep-0030.html">XEP-0030</a> for more information.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoItems : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public DiscoItems(XmlDocument doc) : base("query", URI.DISCO_ITEMS, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public DiscoItems(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The sub-address of the discovered entity.
        /// </summary>
        public string Node
        {
            get { return GetAttr("node"); }
            set { SetAttr("node", value); }
        }

        /// <summary>
        /// Add a disco item
        /// </summary>
        /// <returns></returns>
        public DiscoItem AddItem()
        {
            return CreateChildElement<DiscoItem>();
        }

        /// <summary>
        /// List of disco items
        /// </summary>
        /// <returns></returns>
        public DiscoItem[] GetItems()
        {
            return GetElements<DiscoItem>().ToArray();
        }
    }

    /// <summary>
    /// Actions for iq/set in the disco#items namespace.
    /// </summary>
    [SVN(@"$Id$")]
    public enum DiscoAction
    {
        /// <summary>
        /// None specified
        /// </summary>
        NONE = -1,
        /// <summary>
        /// Remove this item
        /// </summary>
        remove,
        /// <summary>
        /// Update this item
        /// </summary>
        update
    }

    /// <summary>
    /// An item inside a disco#items result.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoItem : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public DiscoItem(XmlDocument doc) : base("item", URI.DISCO_ITEMS, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public DiscoItem(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The Jabber ID associated with the item.
        /// </summary>
        public JID Jid
        {
            get { return GetAttr("jid"); }
            set { SetAttr("jid", value.ToString()); }
        }

        /// <summary>
        /// The user-visible name of this node
        /// </summary>
        public string Named
        {
            get { return GetAttr("name"); }
            set { SetAttr("name", value); }
        }

        /// <summary>
        /// The sub-node associated with this item.
        /// </summary>
        public string Node
        {
            get { return GetAttr("node"); }
            set { SetAttr("node", value); }
        }

        /// <summary>
        /// Actions for iq/set in the disco#items namespace.
        /// </summary>
        public DiscoAction Action
        {
            get { return GetEnumAttr<DiscoAction>("action"); }
            set { SetEnumAttr("action", value); }
        }
    }

/*
<iq
    type='result'
    from='balconyscene@plays.shakespeare.lit'
    to='juliet@capulet.com/balcony'
    id='info2'>
  <query xmlns='http://jabber.org/protocol/disco#info'>
    <identity
        category='conference'
        type='text'
        name='Romeo and Juliet, Act II, Scene II'/>
    <feature var='gc-1.0'/>
    <feature var='http://jabber.org/protocol/muc'/>
    <feature var='http://jabber.org/protocol/feature-neg'/>
    <feature var='muc-password'/>
    <feature var='muc-hidden'/>
    <feature var='muc-temporary'/>
    <feature var='muc-open'/>
    <feature var='muc-unmoderated'/>
    <feature var='muc-nonanonymous'/>
    <x xmlns='jabber:x:data' type='result'>
      <field var='FORM_TYPE' type='hidden'>
        <value>http://jabber.org/network/serverinfo</value>
      </field>
      <field var='c2s_port'>
        <value>5222</value>
      </field>
    </x>
  </query>
</iq>
*/
    /// <summary>
    /// The information associated with a disco node.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoInfo : Element
    {
        private StringSet m_features = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public DiscoInfo(XmlDocument doc) : base("query", URI.DISCO_INFO, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public DiscoInfo(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The sub-node associated with this item.
        /// </summary>
        public string Node
        {
            get { return GetAttr("node"); }
            set { SetAttr("node", value); }
        }

        /// <summary>
        /// Add an identity
        /// </summary>
        /// <param name="category">The category of the identity.  Required.</param>
        /// <param name="discoType">The sub-type</param>
        /// <param name="name">A human-readable string</param>
        /// <param name="language">The xml:lang, or null to take the requestor's default</param>
        /// <returns></returns>
        public DiscoIdentity AddIdentity(string category, string discoType, string name, string language)
        {
            DiscoIdentity i = CreateChildElement<DiscoIdentity>();
            i.Category = category;
            i.Type = discoType;
            i.Named = name;
            i.Lang = language;
            return i;
        }

        /// <summary>
        /// List of identities
        /// </summary>
        /// <returns></returns>
        public DiscoIdentity[] GetIdentities()
        {
            return GetElements<DiscoIdentity>().ToArray();
        }

        /// <summary>
        /// Add a feature
        /// </summary>
        /// <returns></returns>
        public DiscoFeature AddFeature(string featureURI)
        {
            DiscoFeature i = CreateChildElement<DiscoFeature>();
            i.Var = featureURI;
            if (m_features != null)
                m_features.Add(featureURI);
            return i;
        }

        /// <summary>
        /// List of features
        /// </summary>
        /// <returns></returns>
        public DiscoFeature[] GetFeatures()
        {
            return GetElements<DiscoFeature>().ToArray();
        }

        /// <summary>
        /// Is the given feature URI supported by this entity?
        /// </summary>
        /// <param name="featureURI">The URI to check</param>
        /// <returns></returns>
        public bool HasFeature(string featureURI)
        {
            if (m_features != null)
                return m_features.Contains(featureURI);

            foreach (DiscoFeature feat in GetElements<DiscoFeature>())
            {
                if (feat.Var == featureURI)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Clear all of the features from the 
        /// </summary>
        public void ClearFeatures()
        {
            this.RemoveElems<DiscoFeature>();
            m_features = null;
        }

        /// <summary>
        /// Get or set a compressed set of features.
        /// Setting this has the side-effect of removing all existing features, and
        /// replacing them with the specified ones.
        /// </summary>
        public StringSet FeatureSet
        {
            get
            {
                if (m_features == null)
                {
                    m_features = new StringSet();
                    foreach (DiscoFeature f in GetElements<DiscoFeature>())
                        m_features.Add(f.Var);
                }
                return m_features;
            }
            set
            {
                ClearFeatures();
                m_features = new StringSet();
                foreach (string s in value)
                    AddFeature(s);
            }
        }

        /// <summary>
        /// Create a XEP-0128 x:data extension, or return the first existing one.
        /// </summary>
        /// <returns></returns>
        public Data CreateExtension()
        {
            Data d = GetOrCreateElement<Data>();
            d.Type = XDataType.result;
            return d;
        }

        /// <summary>
        /// Get or set the first XEP-0128 x:data extension.
        /// </summary>
        public Data Extension
        {
            get { return GetChildElement<jabber.protocol.x.Data>(); }
            set { ReplaceChild<Data>(value); }
        }

        /// <summary>
        /// In the unlikely event that there are multiple extensions, we need to be able
        /// to retrieve all of them.  
        /// </summary>
        /// <returns></returns>
        public Data[] GetExtensions()
        {
            return GetElements<Data>().ToArray();
        }
    }

    /// <summary>
    /// The identitiy associated with a disco node.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoIdentity : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public DiscoIdentity(XmlDocument doc) : base("identity", URI.DISCO_INFO, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public DiscoIdentity(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The user-visible name of this node
        /// </summary>
        public string Named
        {
            get { return GetAttr("name"); }
            set { SetAttr("name", value); }
        }

        /// <summary>
        /// The category of the node
        /// </summary>
        public string Category
        {
            get { return GetAttr("category"); }
            set { SetAttr("category", value); }
        }

        /// <summary>
        /// The type of the node
        /// </summary>
        public string Type
        {
            get { return GetAttr("type"); }
            set { SetAttr("type", value); }
        }

    }

    /// <summary>
    /// A feature associated with a disco node.
    /// </summary>
    [SVN(@"$Id$")]
    public class DiscoFeature : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public DiscoFeature(XmlDocument doc) : base("feature", URI.DISCO_INFO, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public DiscoFeature(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The namespace name or feature name.
        /// </summary>
        public string Var
        {
            get { return GetAttr("var"); }
            set { SetAttr("var", value); }
        }
    }
}
