/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;

using bedrock.util;
using jabber.protocol;

namespace jabber.protocol.x
{
	/// <summary>
	/// ElementFactory for all currently supported IQ namespaces.
	/// </summary>
    [RCS(@"$Header$")]
    public class Factory : IPacketTypes
	{
		private static QnameType[] s_qnt = new QnameType[] 
		{
			new QnameType("x",     URI.XDELAY,    typeof(jabber.protocol.x.Delay)),
			new QnameType("x",     URI.XEVENT,    typeof(jabber.protocol.x.Event)),
			new QnameType("x",     URI.XOOB,      typeof(jabber.protocol.iq.OOB)),
            new QnameType("x",     URI.XROSTER,   typeof(jabber.protocol.iq.Roster)),
            new QnameType("item",  URI.XROSTER,   typeof(jabber.protocol.iq.Item)),
            new QnameType("group", URI.XROSTER,   typeof(jabber.protocol.iq.Group)),

            new QnameType("x",     URI.XDATA,     typeof(jabber.protocol.x.Data)),
            new QnameType("field", URI.XDATA,     typeof(jabber.protocol.x.Field)),
            new QnameType("option",URI.XDATA,     typeof(jabber.protocol.x.Option)),
        };
		QnameType[] IPacketTypes.Types { get { return s_qnt; } }
	}
}
