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
 * Copyright (c) 2001 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2001 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;

namespace jabber.xml
{
    /// <summary>
    /// Get interesting events from a TagListener.  No default implementation, since 
    /// it's likely you always need all three.
    /// </summary>
    public interface IElementStreamListener
    {
        /// <summary>
        /// Root element start tag arrived.
        /// </summary>
        /// <param name="tag">The start tag, as if it were a full element.</param>
        void OnDocumentStart(System.Xml.XmlElement tag);
        /// <summary>
        /// A direct child of the root element has completely arrived.  
        /// </summary>
        /// <param name="tag">The complete element.  If there
        /// was a matching ElementFactory, this will be a subclass of XmlElement
        /// that has type-safe accessors.</param>
        void OnElement(System.Xml.XmlElement tag);
        /// <summary>
        /// The document has ended.  Long live the document.
        /// </summary>
        void OnDocumentEnd();
    }
}
