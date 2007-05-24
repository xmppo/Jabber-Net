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
	public class CapsManager: Component
	{	
        /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

        private string m_node;
        private string m_version;
        private FeatureSet m_base = new FeatureSet();
        
        private jabber.connection.XmppStream m_stream = null;
        private Hashtable m_bundles = new Hashtable();
		
        /// <summary>
        /// Constructor
        /// </summary>
		public CapsManager()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
		public CapsManager(IContainer container)
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


        /// <summary>
        /// The base features of this client, associated with Node#Version.
        /// </summary>
        public FeatureSet BaseFeatures
        {
            get { return m_base; }
        }

        /// <summary>
        /// The features associated with an extension.
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
        public string Node
        {
            get { return m_node; }
            set { m_node = value; }
        }

        /// <summary>
        /// Current version number for this client.
        /// </summary>
        public string Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        /// <summary>
        /// The list of extensions that are currently turned on.
        /// </summary>
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

        /// <summary>
        /// The JabberClient to hook up to.
        /// </summary>
        [Description("The JabberClient or JabberService to hook up to.")]
        [Category("Jabber")]
        public virtual XmppStream Stream
        {
            get
            {
                // If we are running in the designer, let's try to get an invoke control
                // from the environment.  VB programmers can't seem to follow directions.
                if ((this.m_stream == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is XmppStream)
                                {
                                    m_stream = (XmppStream)c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_stream;
            }
            set
            {
                m_stream = value;
                jabber.client.JabberClient jc = m_stream as jabber.client.JabberClient;
                if (jc != null)
                {
                    jc.OnBeforePresenceOut += new jabber.client.PresenceHandler(jc_OnBeforePresenceOut);
                    jc.OnIQ += new jabber.client.IQHandler(jc_OnIQ);
                }
            }
        }


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

        void jc_OnIQ(object sender, IQ iq)
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

        void jc_OnBeforePresenceOut(object sender, Presence pres)
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
