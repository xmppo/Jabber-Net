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

namespace jabber.protocol.client
{
    /// <summary>
    /// Invalid protocol received.
    /// </summary>
    [SVN(@"$Id$")]
    public class BadProtocolException : Exception
    {
        private XmlElement m_proto = null;
        private string m_msg = null;

        /// <summary>
        /// Create a protocol exception
        /// </summary>
        /// <param name="badProtocol">The protocol that was bad.  Typically the top-most (stanza) element.</param>
        /// <param name="message">An optional message.  May be null.</param>
        public BadProtocolException(XmlElement badProtocol, string message)
        {
            m_proto = badProtocol;
            m_msg = message;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                if (m_msg == null)
                    return string.Format("Invalid protocol: {0}", m_proto.OuterXml);
                return string.Format("Invalid protocol ({0}): {1}", m_proto.OuterXml, m_msg);
            }
        }
    }

    /// <summary>
    /// A jabber error, in an IQ.
    /// </summary>
    [SVN(@"$Id$")]
    public class IQException : Exception
    {
        // TODO: fix this up for new error codes.
        private int m_code;
        private string m_message;

        /// <summary>
        /// An authorization exception from an IQ.
        /// TODO: Add constructor for code/message
        /// TODO: understand v1 errors
        /// </summary>
        /// <param name="iq"></param>
        public IQException(IQ iq)
        {
            if (iq == null)
            {
                //timeout
                m_code = 504;
                m_message = "Request timed out";
            }
            else
            {
                Error e = iq.Error;
                m_code = e.Code;
                m_message = e.InnerText;
            }
        }

        /// <summary>
        /// The Jabber error number
        /// </summary>
        public int Code
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
