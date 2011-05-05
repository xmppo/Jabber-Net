/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *  
 * This file was not authored by Cursive Systems, Inc.  jabber-net modifications to 
 * support SASL Anonymous are notated with //FF comments
 * 
 * Author(s):  Frank Failla
 * 
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

using jabber.protocol.stream;

namespace jabber.connection.sasl
{
    //FF
	public class AnonymousProcessor : SASLProcessor
	{
        public override Step step(Step s, XmlDocument doc)
        {
            Debug.Assert(s == null);
            Auth a = new Auth(doc);
            a.Mechanism = MechanismType.ANONYMOUS;
            return a;
        }
	}
}
