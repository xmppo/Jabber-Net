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
using jabber.protocol;

namespace jabber.protocol.stream
{
    /// <summary>
    /// ElementFactory for http://etherx.jabber.org/streams
    /// </summary>
    [RCS(@"$Header$")]
    public class Factory : jabber.protocol.IPacketTypes
    {
        private static QnameType[] s_qnt = new QnameType[] 
        {
            new QnameType("stream", URI.STREAM, typeof(Stream)),
            new QnameType("error",  URI.STREAM, typeof(Error))
        };
        QnameType[] IPacketTypes.Types { get { return s_qnt; } }
    }
}
