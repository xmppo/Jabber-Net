/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
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

using bedrock.util;
using bedrock.collections;

using jabber.protocol.client;
using jabber.protocol.iq;

namespace jabber.client
{
    /// <summary>
    /// Disco database.
    /// TODO: once etags are finished, make all of this information cached on disk.
    /// TODO: cache JEP-115 client caps data to disk
    /// </summary>
    [RCS(@"$Header$")]
    public class DiscoManager : System.ComponentModel.Component, IEnumerable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private JabberClient m_client = null;
        private Tree m_items = new Tree();

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        /// <param name="container"></param>
        public DiscoManager(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Construct a PresenceManager object.
        /// </summary>
        public DiscoManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The JabberClient to hook up to.
        /// </summary>
        [Description("The JabberClient to hook up to.")]
        [Category("Jabber")]
        public JabberClient Client
        {
            get
            {
                // If we are running in the designer, let's try to get an invoke control
                // from the environment.  VB programmers can't seem to follow directions.
                if ((this.m_client == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    if (host != null)
                    {
                        Component root = host.RootComponent as Component;
                        if (root != null)
                        {
                            foreach (Component c in root.Container.Components)
                            {
                                if (c is JabberClient)
                                {
                                    m_client = (JabberClient)c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_client;
            }
            set
            {
                m_client = value;
                m_client.OnIQ += new IQHandler(GotIQ);
                m_client.OnDisconnect += new bedrock.ObjectHandler(GotDisconnect);
            }
        }

        private void GotDisconnect(object sender)
        {
            lock (this)
                m_items.Clear();
        }

        private void GotIQ(object sender, IQ iq)
        {
            if (iq.Type == IQType.error)
                return;

            DiscoInfo info = iq.Query as DiscoInfo;
            if (info != null)
                GotInfo(iq);
            DiscoItems items = iq.Query as DiscoItems;
            if (items != null)
                GotInfo(iq);
        }

        private void GotInfo(IQ iq)
        {

        }

        private void GetItems(IQ iq)
        {

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }
    }
}
