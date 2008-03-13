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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using jabber.protocol.x;

namespace jabber.connection
{
    /// <summary>
    /// Manages the entity capabilities information for the local connection as well as remote ones.
    /// See XEP-0115, version 1.5 for details.
    /// </summary>
    public class CapsManager: StreamComponent
    {
        /// <summary>
        /// Defines the default hash function to use for calculating ver attributes.
        /// </summary>
        public const string DEFAULT_HASH = "sha-1";
        private const string SEP = "<";

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DiscoNode m_disco;
        private string m_hash = DEFAULT_HASH;
        private string m_ver = null;

        /// <summary>
        /// Creates a new capability manager.
        /// </summary>
        public CapsManager() : this((DiscoNode)null)
        {
        }

        /// <summary>
        /// Creates a new capability manager and associates it with a container.
        /// </summary>
        /// <param name="container">Parent container.</param>
        public CapsManager(IContainer container) : this((DiscoNode)null)
        {
            container.Add(this);
        }

        /// <summary>
        /// Create a CapsManager from an existing Disco Node.  Pass in null
        /// to use a placeholder.
        /// </summary>
        /// <param name="node"></param>
        public CapsManager(DiscoNode node)
        {
            InitializeComponent();
            this.OnStreamChanged += new bedrock.ObjectHandler(CapsManager_OnStreamChanged);

            if (node == null)
                m_disco = new DiscoNode(new JID(null, "placeholder", null), null);
            else
                m_disco = node;
        }

        /// <summary>
        /// Performs tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        /// <param name="disposing">
        /// True if managed resources should be disposed; otherwise, false.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Adds a feature to the feature list
        /// </summary>
        /// <param name="feature"></param>
        public void AddFeature(string feature)
        {
            m_ver = null;
            m_disco.AddFeature(feature);
        }

        /// <summary>
        /// Removes a feature from the feature list
        /// </summary>
        /// <param name="feature"></param>
        public void RemoveFeature(string feature)
        {
            m_ver = null;
            m_disco.RemoveFeature(feature);
        }

        /// <summary>
        /// Gets or sets the current features enabled by this entity.
        /// </summary>
        [Category("Capabilities")]
        [DefaultValue(null)]
        public string[] Features
        {
            get
            {
                if (m_disco.Features == null)
                    return new string[0];
                return m_disco.FeatureNames;
            }
            set
            {
                m_ver = null;
                m_disco.ClearFeatures();
                if (value != null)
                    foreach (string feature in value)
                        m_disco.AddFeature(feature);
            }
        }

        private static HashAlgorithm GetHasher(string name)
        {
            switch (name)
            {
            case null:
                return null;
            case "sha-1":
                return SHA1.Create();
            case "sha-256":
                return SHA256.Create();
            case "sha-512":
                return SHA512.Create();
            case "sha-384":
                return SHA384.Create();
            case "md5":
                return MD5.Create();
            }
            throw new ArgumentException("Invalid hash method: " + name, "Hash");
        }

        /// <summary>
        /// Gets or sets the hash algorithm to use.
        /// </summary>
        [Category("Capabilities")]
        [DefaultValue(DEFAULT_HASH)]
        public string Hash
        {
            get { return m_hash; }
            set
            {
                GetHasher(value);  // throws if bad.
                m_hash = value;
            }
        }

        private void CalculateVer()
        {
            if (m_hash == null)
                return;

            // 1. Initialize an empty string S.
            StringBuilder S = new StringBuilder();

            // 2. Sort the service discovery identities [16] by category and then by type
            // (if it exists) and then by xml:lang (if it exists), formatted as
            // CATEGORY '/' [TYPE] '/' [LANG] '/' [NAME]. Note that each slash is
            // included even if the TYPE, LANG, or NAME is not included.
            Ident[] ids = m_disco.GetIdentities();
            Array.Sort(ids);

            // 3. For each identity, append the 'category/type/lang/name' to S, followed by
            // the '<' character.
            foreach (Ident id in ids)
            {
                S.Append(id.Key);
                S.Append(SEP);
            }

            // 4. Sort the supported service discovery features.
            string[] features = m_disco.FeatureNames;
            Array.Sort(features);

            // 5. For each feature, append the feature to S, followed by the '<' character.
            foreach (string feature in features)
            {
                S.Append(feature);
                S.Append(SEP);
            }

            // 6. If the service discovery information response includes XEP-0128 data forms, 
            // sort the forms by the FORM_TYPE (i.e., by the XML character 
            // data of the <value/> element).
            Data[] ext = m_disco.Extensions;
            if (ext != null)
            {
                Array.Sort(ext, new FormTypeComparer());
                foreach (Data x in ext)  
                {
                    // For each extended service discovery information form:

                    // 1. Append the XML character data of the FORM_TYPE field's <value/> 
                    // element, followed by the '<' character.
                    S.Append(x.FormType);
                    S.Append(SEP);

                    // 2. Sort the fields by the value of the "var" attribute.
                    bedrock.collections.Tree fields = new bedrock.collections.Tree();
                    foreach (Field f in x.GetFields())
                        fields[f.Var] = f;

                    // 3. For each field:
                    foreach (System.Collections.DictionaryEntry entry in fields)
                    {
                        Field f = (Field)entry.Value;
                        if (f.Var == "FORM_TYPE")
                            continue;

                        // 1. Append the value of the "var" attribute, followed by the '<' character.
                        S.Append(f.Var);
                        S.Append(SEP);

                        // 2. Sort values by the XML character data of the <value/> element.
                        string[] values = f.Vals;
                        Array.Sort(values);
                        foreach (string v in values)
                        {
                            // 3. For each <value/> element, append the XML character data, followed by the '<' character.
                            S.Append(v);
                            S.Append(SEP);
                        }
                    }
                }
            }

            // Ensure that S is encoded according to the UTF-8 encoding (RFC 3269 [16]).
            byte[] input = Encoding.UTF8.GetBytes(S.ToString());

            // Compute the verification string by hashing S using the algorithm specified
            // in the 'hash' attribute (e.g., SHA-1 as defined in RFC 3174 [17]). The hashed
            // data MUST be generated with binary output and encoded using Base64 as specified
            // in Section 4 of RFC 4648 [18] (note: the Base64 output MUST NOT include
            // whitespace and MUST set padding bits to zero). [19]
            HashAlgorithm hasher = GetHasher(m_hash);
            byte[] hash = hasher.ComputeHash(input, 0, input.Length);
            m_ver = Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Returns the calculated hash over all of the caps information.
        /// </summary>
        [Category("Capabilities")]
        public string Ver
        {
            get
            {
                if (m_ver == null)
                    CalculateVer();
                return m_ver;
            }
        }

        /// <summary>
        /// Gets or sets the node URI for this client.
        /// </summary>
        [Category("Capabilities")]
        [DefaultValue(null)]
        public string Node
        {
            get { return m_disco.Node; }
            set { m_disco.Node = value; }
        }

        /// <summary>
        /// Retrieves the node#ver to look for in queries.
        /// </summary>
        [Category("Capabilities")]
        public string NodeVer
        {
            get { return Node + "#" + Ver; }
        }

        /// <summary>
        /// Adds a new identity.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="type"></param>
        /// <param name="lang"></param>
        /// <param name="name"></param>
        public void AddIdentity(string category, string type, string lang, string name)
        {
            m_ver = null;
            m_disco.AddIdentity(new Ident(name, category, type, lang));
        }

        /// <summary>
        /// Adds a new identity
        /// </summary>
        /// <param name="id"></param>
        public void AddIdentity(Ident id)
        {
            m_ver = null;
            m_disco.AddIdentity(id);
        }

        /// <summary>
        /// Gets or sets all of the identities currently supported by this manager.
        /// </summary>
        [Category("Capabilities")]
        [DefaultValue(null)]
        public Ident[] Identities
        {
            get
            {
                if (m_disco.Identity == null)
                    return new Ident[0];
                return m_disco.GetIdentities();
            }
            set
            {
                m_ver = null;
                m_disco.ClearIdentity();
                if (value != null)
                    foreach (Ident id in value)
                        m_disco.AddIdentity(id);
            }
        }

        private void CapsManager_OnStreamChanged(object sender)
        {
            m_disco.JID = m_stream.JID;

            jabber.client.JabberClient jc = m_stream as jabber.client.JabberClient;
            if (jc == null)
                return;

            jc.OnBeforePresenceOut += new jabber.client.PresenceHandler(jc_OnBeforePresenceOut);
            jc.OnIQ += new jabber.client.IQHandler(jc_OnIQ);
        }

        /// <summary>
        /// Determines whether or not this is a capabilities request.
        /// Answers true for a bare no-node disco request, as well as
        /// for requests to the correct hash.
        /// </summary>
        /// <param name="iq">XML to look through for capabilities.</param>
        /// <returns>True if this is a capabilities request.</returns>
        public bool IsCaps(IQ iq)
        {
            if (iq.Type != IQType.get)
                return false;

            DiscoInfo info = iq.Query as DiscoInfo;
            if (info == null)
                return false;

            string node = info.Node;
            if (node == "")
                return true;

            if (node == NodeVer)
                return true;

            return false;
        }

        private void jc_OnIQ(object sender, IQ iq)
        {
            if (!IsCaps(iq))
                return;

            DiscoInfo info = iq.Query as DiscoInfo;
            if (info == null)
                return;

            IQ resp = iq.GetResponse(m_stream.Document);
            info = (DiscoInfo)resp.Query;
            info.Node = NodeVer;
            foreach (Ident id in Identities)
                info.AddIdentity(id.Category, id.Type, id.Name, id.Lang);
            foreach (string uri in Features)
                info.AddFeature(uri);

            m_stream.Write(resp);
        }

        private void jc_OnBeforePresenceOut(object sender, Presence pres)
        {
            Debug.Assert(Node != null, "Node is required");

            Caps caps = new Caps(pres.OwnerDocument);
            caps.Version = Ver;
            caps.Node = Node;
            caps.Hash = m_hash;
            pres.AppendChild(caps);
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
