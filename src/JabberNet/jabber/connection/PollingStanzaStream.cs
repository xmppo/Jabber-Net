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

using JabberNet.bedrock.net;

namespace JabberNet.jabber.connection
{
    /// <summary>
    /// Manages the HTTP Polling XMPP stream.
    /// </summary>
    public class PollingStanzaStream : HttpStanzaStream
    {
        ///<summary>
        /// Creates a PollingStanzaStream
        ///</summary>
        ///<param name="listener">Listener associated with PollingStanzaStream.</param>
        public PollingStanzaStream(IStanzaEventListener listener) : base(listener)
        {
        }

        /// <summary>
        /// Create a XEP25Socket.
        /// </summary>
        /// <returns></returns>
        protected override BaseSocket CreateSocket()
        {
            return new XEP25Socket(this);
        }
    }
}

