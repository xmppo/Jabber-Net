/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.IO;
using System.Collections;

namespace jabber.connection
{
    /// <summary>
    /// Manages HTTP Requests via XMPP (XEP-70).
    /// </summary>
    public class HttpUploader
    {
        /// <summary>
        /// Notifies the client that an upload has finished.
        /// </summary>
        public event bedrock.ObjectHandler OnUpload;

        private void ResponseCallback(IAsyncResult result)
        {
            HttpWebRequest request  = (HttpWebRequest)result.AsyncState;
            //request.GetResponse().GetResponseStream();
            if (OnUpload != null)
                OnUpload(this);
        }

        /// <summary>
        /// Uploads a file to a given URL and verifies the HTTP request
        /// through the XMPP server (XEP-0070).
        /// </summary>
        /// <param name="uri">URI to send the file to.</param>
        /// <param name="jid">JID to send as.</param>
        /// <param name="filename">File to send.</param>
        public void Upload(string uri, string jid, string filename)
        {
            //try
            //{
            StreamReader reader = new StreamReader(filename);
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(uri);

            request.Method = "POST";
            request.Headers.Add(HttpRequestHeader.Authorization,
                                "x-xmpp-auth jid=\"" + jid + "\"");

            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(reader.ReadToEnd());

            reader.Close();

            request.BeginGetResponse(new AsyncCallback(ResponseCallback),
                                     request);
            writer.Close();
            // }
            // catch (WebException)
            // {
            // }
        }
    }
}
