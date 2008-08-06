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
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;


using bedrock.util;
using jabber.protocol;

namespace jabber.protocol.stream
{
    /// <summary>
    /// ElementFactory for http://etherx.jabber.org/streams
    /// </summary>
    [SVN(@"$Id$")]
    public class Factory : jabber.protocol.IPacketTypes
    {
        private static QnameType[] s_qnt = new QnameType[]
        {
            new QnameType("stream",     URI.STREAM,    typeof(Stream)),
            new QnameType("error",      URI.STREAM,    typeof(Error)),
            new QnameType("features",   URI.STREAM,    typeof(Features)),

            new QnameType("starttls",   URI.START_TLS, typeof(StartTLS)),
            new QnameType("proceed",    URI.START_TLS, typeof(Proceed)),
            new QnameType("failure",    URI.START_TLS, typeof(TLSFailure)),

            new QnameType("compression", URI.COMPRESS_FEATURE, typeof(Compression)),
            new QnameType("failure",    URI.COMPRESS,  typeof(CompressionFailure)),
            new QnameType("compress",   URI.COMPRESS,  typeof(Compressed)),
            new QnameType("compressed", URI.COMPRESS,  typeof(Compressed)),

            new QnameType("mechanisms", URI.SASL,      typeof(Mechanisms)),
            new QnameType("mechanism",  URI.SASL,      typeof(Mechanism)),
            new QnameType("auth",       URI.SASL,      typeof(Auth)),
            new QnameType("challenge",  URI.SASL,      typeof(Challenge)),
            new QnameType("response",   URI.SASL,      typeof(Response)),
            new QnameType("failure",    URI.SASL,      typeof(SASLFailure)),
            new QnameType("abort",      URI.SASL,      typeof(Abort)),
            new QnameType("success",    URI.SASL,      typeof(Success)),

            new QnameType("session",    URI.SESSION,   typeof(Session)),
            new QnameType("bind",       URI.BIND,      typeof(Bind)),

            new QnameType("body",       URI.HTTP_BIND, typeof(Body)),
        };
        QnameType[] IPacketTypes.Types { get { return s_qnt; } }
    }
}
