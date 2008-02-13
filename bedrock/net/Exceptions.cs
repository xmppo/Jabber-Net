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
using bedrock.util;

namespace bedrock.net
{
    /// <summary>
    /// Lame exception, since I couldn't find one I liked.
    /// </summary>
    [SVN(@"$Id$")]
    [Serializable]
    public class AsyncSocketConnectionException : System.SystemException
    {
        /// <summary>
        /// Create a new exception instance.
        /// </summary>
        /// <param name="description"></param>
        public AsyncSocketConnectionException(string description)
            : base(description)
        {
        }

        /// <summary>
        /// Create a new exception instance.
        /// </summary>
        public AsyncSocketConnectionException()
            : base()
        {
        }

        /// <summary>
        /// Create a new exception instance, wrapping another exception.
        /// </summary>
        /// <param name="description">Desecription of the exception</param>
        /// <param name="e">Inner exception</param>
        public AsyncSocketConnectionException(string description, Exception e)
            : base(description, e)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// AsyncSocketConnectionException class with serialized
        /// data.
        /// </summary>
        /// <param name="info">The object that holds the serialized
        /// object data.</param>
        /// <param name="ctx">The contextual information about the
        /// source or destination.</param>
        protected AsyncSocketConnectionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext ctx)
            :
            base(info, ctx)
        {
        }
    }
}
