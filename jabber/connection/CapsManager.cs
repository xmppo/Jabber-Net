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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;

using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using jabber.protocol.x;

namespace jabber.connection
{
    /// <summary>
    /// List of features in a given node#ver or node#ext.
    /// </summary>
    public class FeatureSet : bedrock.collections.Set
    {

    }

    /// <summary>
    /// Manage entity capabilities information, for the local connection as well as remote ones.
    /// </summary>
	public class CapsManager: StreamComponent
	{	
        /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

        private string m_node = null;
        private string m_version = null;
        private FeatureSet m_base = new FeatureSet();
        private Hashtable m_bundles = new Hashtable();
		
        /// <summary>
        /// Constructor
        /// </summary>
		public CapsManager()
		{
			InitializeComponent();
            this.OnStreamChanged += new bedrock.ObjectHandler(CapsManager_OnStreamChanged);
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
		public CapsManager(IContainer container) : this()
		{
			container.Add(this);
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


        /// <summary>
        /// Gets the base features of this client associated with Node#Version.
        /// </summary>
        [Category("Capabilities")]
        public FeatureSet BaseFeatures
        {
            get { return m_base; }
        }

        /// <summary>
        /// Contains the features associated with an extension.
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public FeatureSet this[string ext]
        {
            get 
            {
                lock (m_bundles)
                {
                    FeatureSet set = (FeatureSet)m_bundles[ext];
                    if (set == null)
                    {
                        set = new FeatureSet();
                        m_bundles[ext] = set;
                    }
                    return set;
                }
            }
        }

        /// <summary>
        /// Node URI for this client.
        /// </summary>
        [Category("Capabilities")]
        [DefaultValue(null)]
        public string Node
        {
            get { return m_node; }
            set { m_node = value; }
        }

        /// <summary>
        /// Current version number for this client.
        /// </summary>
        [Category("Capabilities")]
        [DefaultValue(null)]
        public string Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        /// <summary>
        /// Gets the list of extensions that are currently enabled.
        /// </summary>
        [Category("Capabilities")]
        public string[] Extensions
        {
            get 
            {
                ICollection keys = m_bundles.Keys;
                string[] exts = new string[keys.Count];
                keys.CopyTo(exts, 0);
                return exts;
            }
        }

        private void CapsManager_OnStreamChanged(object sender)
        {
            jabber.client.JabberClient jc = m_stream as jabber.client.JabberClient;
            if (jc == null)
                return;

            jc.OnBeforePresenceOut += new jabber.client.PresenceHandler(jc_OnBeforePresenceOut);
            jc.OnIQ += new jabber.client.IQHandler(jc_OnIQ);
        }

        /// <summary>
        /// Determines if this is a capabilities request.
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

            if (node == Node + "#" + Version)
                return true;

            foreach (string ext in Extensions)
            {
                if (node == Node + "#" + ext)
                    return true;
            }
            return false;
        }

        private void jc_OnIQ(object sender, IQ iq)
        {
            if (!IsCaps(iq))
                return;

            DiscoInfo info = iq.Query as DiscoInfo;
            if (info != null)
            {
                if (info.Node == "")
                {
                    // union of all.
                    IQ resp = iq.GetResponse(m_stream.Document);
                    info = (DiscoInfo)resp.Query;
                    foreach (string uri in BaseFeatures)
                    {
                        info.AddFeature(uri);
                    }
                    foreach (string ext in Extensions)
                    {
                        foreach (string uri in this[ext])
                        {
                            info.AddFeature(uri);
                        }
                    }
                    m_stream.Write(resp);
                    return;
                }

                string node = info.Node;
                
                if (node == Node + "#" + Version)
                {
                    IQ resp = iq.GetResponse(m_stream.Document);
                    info = (DiscoInfo)resp.Query;
                    foreach (string uri in BaseFeatures)
                    {
                        info.AddFeature(uri);
                    }
                    m_stream.Write(resp);
                    return;
                }

                foreach (string ext in this.Extensions)
                {
                    if (node == Node + "#" + ext)
                    {
                        IQ resp = iq.GetResponse(m_stream.Document);
                        info = (DiscoInfo)resp.Query;
                        foreach (string uri in this[ext])
                        {
                            info.AddFeature(uri);
                        }
                        m_stream.Write(resp);
                        return;
                    }
                }
            }
        }

        private void jc_OnBeforePresenceOut(object sender, Presence pres)
        {
            Debug.Assert(m_node != null, "Node is required");
            Debug.Assert(m_version != null, "Version is required");

            Caps caps = new Caps(pres.OwnerDocument);
            caps.Version = m_version;
            caps.Node = m_node;
            caps.Extensions = this.Extensions;
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
