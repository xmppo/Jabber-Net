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
namespace bedrock
{
    /// <summary>
    /// Delegate for memebers that just have a sender
    /// </summary>
    public delegate void ObjectHandler(object sender);
    /// <summary>
    /// Delegate for members that receive a string
    /// </summary>
    public delegate void TextHandler(object sender, string txt);
    /// <summary>
    /// Delegate for methods that get a block of bytes
    /// </summary>
    public delegate void ByteHandler(object sender, byte[] buf);
    /// <summary>
    /// Delegate for members that receive an exception
    /// </summary>
    public delegate void ExceptionHandler(object sender, Exception ex);
}
