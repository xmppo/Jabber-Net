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
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;

using bedrock.util;
using jabber.connection;

namespace muzzle
{
    /// <summary>
    /// A UserControl that references an XmppStream.
    /// </summary>
    [SVN(@"$Id$")]
    public class StreamControl : System.Windows.Forms.UserControl
	{
        /// <summary>
        /// The XmppStream for this control.  Set at design time when a subclass control is dragged onto a form.
        /// </summary>
        protected XmppStream m_stream = null;

        /// <summary>
        /// The XmppStream was changed.  Often at design time.  The object will be this StreamControl.
        /// </summary>
        public event bedrock.ObjectHandler OnStreamChanged;

        /// <summary>
        /// The JabberClient or JabberService to hook up to.
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
                    this.Stream = StreamComponent.GetStreamFromHost(host);
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
