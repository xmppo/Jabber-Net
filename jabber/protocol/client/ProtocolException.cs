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

namespace jabber.protocol.client
{
	/// <summary>
	/// A jabber error, in an IQ.
	/// </summary>
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
