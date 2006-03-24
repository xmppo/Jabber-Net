/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;
using System.Threading;
using System.Xml;

using bedrock.util;
using jabber;
using jabber.client;

namespace ConsoleClient
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Class1
    {
        [CommandLine("j", "user@host Jabber ID", true)]
        public string jid = null;

        [CommandLine("p", "Password", true)]
        public string pass = null;

        public Class1(string[] args)
        {
            bedrock.net.AsyncSocket.UntrustedRootOK = true;
            JabberClient jc = new JabberClient();
            jc.OnReadText += new bedrock.TextHandler(jc_OnReadText);
            jc.OnWriteText += new bedrock.TextHandler(jc_OnWriteText);
            jc.OnError +=new bedrock.ExceptionHandler(jc_OnError);
            //            jc.AutoStartTLS = false;
            jc.AutoReconnect = 3f;

            GetOpt go = new GetOpt(this);
            try
            {
                go.Process(args);
            }
            catch (ArgumentException)
            {
                go.UsageExit();
            }

            JID j = new JID(jid);
            jc.User = j.User;
            jc.Server = j.Server;
            jc.Resource = "Jabber.Net Console Client";
            jc.Password = pass;
            jc.Connect();

            string line;
            while ((line = Console.ReadLine()) != "")
            {
                try
                {
                    // TODO: deal with stanzas that span lines... keep parsing until we have a full "doc".
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(line);
                    XmlElement elem = doc.DocumentElement;
                    if (elem != null)
                        jc.Write(elem);
                }
                catch (XmlException ex)
                {
                    Console.WriteLine("Invalid XML: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            new Class1(args);
        }

        private void jc_OnReadText(object sender, string txt)
        {
            if (txt != " ")
                Console.WriteLine("RECV: " + txt);
        }

        private void jc_OnWriteText(object sender, string txt)
        {
            if (txt != " ")
                Console.WriteLine("SENT: " + txt);
        }

        private void jc_OnError(object sender, Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.ToString());
            Environment.Exit(1);
        }
    }
}
