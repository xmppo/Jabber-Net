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
using System.ComponentModel;
using System.ComponentModel.Design;

using bedrock.util;
using System.Diagnostics;

namespace jabber.connection
{
    /// <summary>
    /// Manages the XmppStream as a component.
    /// </summary>
    [SVN(@"$Id$")]
    public abstract class StreamComponent : System.ComponentModel.Component
	{
        /// <summary>
        /// Finds the first component that subclasses XmppStream in Visual Studio
        /// during runtime.
        /// </summary>
        /// <param name="host">
        /// Calls GetService(typeof(IDesignerHost)) on your control to get this.
        /// </param>
        /// <returns>Null if none found</returns>
        public static XmppStream GetStreamFromHost(IDesignerHost host)
        {
            return (XmppStream)GetComponentFromHost(host, typeof(XmppStream));
        }

        /// <summary>
        /// Finds the first component that subclasses the given type at runtime.
        /// </summary>
        /// <param name="host">Call GetService(typeof(IDesignerHost)) on your control to get this.</param>
        /// <param name="type">The type to search for.</param>
        /// <returns>Null if none found</returns>
        public static Component GetComponentFromHost(IDesignerHost host, Type type)
        {
            if (host == null)
                return null;
            Debug.Assert(type != null);
            Component root = host.RootComponent as Component;
            if (root == null)
                return null;

            foreach (Component c in root.Container.Components)
            {
                if (type.IsAssignableFrom(c.GetType()))
                    return c;
            }
            return null;            
        }

        /// <summary>
        /// Retrieves the XmppStream for this control.
        /// Set at design time when a subclass control is dragged onto a form.
        /// </summary>
        protected XmppStream m_stream = null;

        /// <summary>
        /// Informs the client that the XmppStream was changed.
        /// Often at design time, the object will be this StreamControl.
        /// </summary>
        public event bedrock.ObjectHandler OnStreamChanged;

        /// <summary>
        /// Gets and sets the JabberClient or JabberService XMPP stream value.
        /// </summary>
        [Description("The JabberClient or JabberService to hook up to.")]
        [Category("Jabber")]
        public virtual XmppStream Stream
        {
            get
            {
                // If we are running in the designer, let's try to get an XmppStream control
                // from the environment.
                if ((this.m_stream == null) && DesignMode)
                {
                    IDesignerHost host = (IDesignerHost)base.GetService(typeof(IDesignerHost));
                    this.Stream = GetStreamFromHost(host);
                }
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
	}
}
