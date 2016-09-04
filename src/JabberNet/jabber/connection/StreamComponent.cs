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
 * See licenses/Jabber-Net_LGPLv3.txt for details.
 * --------------------------------------------------------------------------*/

using System.Xml;
using JabberNet.jabber.protocol.client;

namespace JabberNet.jabber.connection
{
    /// <summary>
    /// Manages the XmppStream as a component.
    /// </summary>
    public abstract class StreamComponent
    {
        /// <summary>
        /// Retrieves the XmppStream for this control.
        /// Set at design time when a subclass control is dragged onto a form.
        /// </summary>
        protected XmppStream m_stream = null;

        /// <summary>
        /// Informs the client that the XmppStream was changed.
        /// Often at design time, the object will be this StreamComponent.
        /// </summary>
        public event bedrock.ObjectHandler OnStreamChanged;

        /// <summary>
        /// Gets and sets the JabberClient or JabberService XMPP stream value.
        /// </summary>
        public virtual XmppStream Stream
        {
            get
            {
                return m_stream;
            }
            set
            {
                if ((object)m_stream != (object)value)
                {
                    m_stream = value;
                    if (OnStreamChanged != null)
                        OnStreamChanged(this);
                }
            }
        }

        private JID m_overrideFrom = null;

        /// <summary>
        /// Override the from address that will be stamped on outbound packets.
        /// Unless your server implemets XEP-193, you shouldn't use this for
        /// client connections.
        /// </summary>
        public JID OverrideFrom
        {
            get { return m_overrideFrom; }
            set { m_overrideFrom = value; }
        }

        /// <summary>
        /// Write the specified stanza to the stream.
        /// If the from address hasn't been set, and an OverrideFrom has been set,
        /// the from address will be set to the value of OverrideFrom.
        /// </summary>
        /// <param name="elem"></param>
        public void Write(XmlElement elem)
        {
            if ((m_overrideFrom != null) && (elem.GetAttribute("from") == ""))
                elem.SetAttribute("from", m_overrideFrom);
            m_stream.Write(elem);
        }

        ///<summary>
        /// Does an asynchronous IQ call.
        /// If the from address hasn't been set, and an OverrideFrom has been set,
        /// the from address will be set to the value of OverrideFrom.
        ///</summary>
        ///<param name="iq">IQ packet to send.</param>
        ///<param name="cb">Callback to execute when the result comes back.</param>
        ///<param name="cbArg">Arguments to pass to the callback.</param>
        public void BeginIQ(IQ iq, IqCB cb, object cbArg)
        {
            if ((m_overrideFrom != null) && (iq.From == null))
                iq.From = m_overrideFrom;
            m_stream.Tracker.BeginIQ(iq, cb, cbArg);
        }
    }
}
