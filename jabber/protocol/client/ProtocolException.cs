/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using bedrock.util;

namespace jabber.protocol.client
{
    /// <summary>
    /// A jabber error, in an IQ.
    /// </summary>
    [RCS(@"$Header$")]
    public class ProtocolException : Exception
    {
        private ErrorCode m_code;
        private string m_message;

        /// <summary>
        /// An authorization exception from an IQ.
        /// TODO: Add constructor for code/message
        /// </summary>
        /// <param name="iq"></param>
        public ProtocolException(IQ iq)
        {
            Error e = iq.Error;
            m_code = e.Code;
            m_message = e.InnerText;
        }

        /// <summary>
        /// The Jabber error number
        /// </summary>
        public ErrorCode Code
        {
            get { return m_code; }
        }

        /// <summary>
        /// The text description of the message
        /// </summary>
        public string Description
        {
            get { return m_message; }
        }

        /// <summary>
        /// Return the error code and message.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Error {0}: {1}", m_code, m_message);
        }
    }
}
