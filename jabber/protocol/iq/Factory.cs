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

namespace jabber.protocol.iq
{
    /// <summary>
    /// ElementFactory for all currently supported IQ namespaces.
    /// </summary>
    [RCS(@"$Header$")]
    public class Factory : IPacketTypes
    {
        private static QnameType[] s_qnt = new QnameType[] 
        {
            new QnameType("query", URI.AUTH,     typeof(jabber.protocol.iq.Auth)),
            new QnameType("query", URI.REGISTER, typeof(jabber.protocol.iq.Register)),
            new QnameType("query", URI.ROSTER,   typeof(jabber.protocol.iq.Roster)),
            new QnameType("item",  URI.ROSTER,   typeof(jabber.protocol.iq.Item)),
            new QnameType("group", URI.ROSTER,   typeof(jabber.protocol.iq.Group)),
            new QnameType("query", URI.AGENTS,   typeof(jabber.protocol.iq.AgentsQuery)),
            new QnameType("agent", URI.AGENTS,   typeof(jabber.protocol.iq.Agent)),
            new QnameType("query", URI.OOB,      typeof(jabber.protocol.iq.OOB)),
            new QnameType("query", URI.TIME,     typeof(jabber.protocol.iq.Time)),
            new QnameType("query", URI.VERSION,  typeof(jabber.protocol.iq.Version)),
            new QnameType("query", URI.LAST,     typeof(jabber.protocol.iq.Last)),
            new QnameType("item",  URI.BROWSE,   typeof(jabber.protocol.iq.Browse)),

			// VCard
			new QnameType("vCard", URI.VCARD, typeof(jabber.protocol.iq.VCard)),
			new QnameType("N",     URI.VCARD, typeof(jabber.protocol.iq.VCard.VName)),
			new QnameType("ORG",   URI.VCARD, typeof(jabber.protocol.iq.VCard.VOrganization)),
			new QnameType("TEL",   URI.VCARD, typeof(jabber.protocol.iq.VCard.VTelephone)),
            new QnameType("EMAIL", URI.VCARD, typeof(jabber.protocol.iq.VCard.VEmail)),
            new QnameType("GEO",   URI.VCARD, typeof(jabber.protocol.iq.VCard.VGeo)),
            new QnameType("PHOTO", URI.VCARD, typeof(jabber.protocol.iq.VCard.VPhoto))
    };

        QnameType[] IPacketTypes.Types { get { return s_qnt; } }
    }
}
