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

using jabber;
using jabber.client;

namespace ConsoleClient
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Class1
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            JabberClient jc = new JabberClient();
            jc.OnReadText += new bedrock.TextHandler(jc_OnReadText);
            jc.OnWriteText += new bedrock.TextHandler(jc_OnWriteText);
            jc.OnError +=new bedrock.ExceptionHandler(jc_OnError);
            jc.AutoStartTLS = false;
            Console.Write("User: ");
            jc.User = Console.ReadLine();
            Console.Write("Server: ");
            jc.Server = Console.ReadLine();
            Console.Write("Password: ");
            jc.Password = Console.ReadLine();
            jc.Connect();
            string line;
            while((line = Console.ReadLine()) != "")
                jc.Write(line);
        }

        private static void jc_OnReadText(object sender, string txt)
        {
            if (txt != " ")
                Console.WriteLine("RECV: " + txt);
        }

        private static void jc_OnWriteText(object sender, string txt)
        {
            if (txt != " ")
                Console.WriteLine("SENT: " + txt);
        }

        private static void jc_OnError(object sender, Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.ToString());
            Environment.Exit(1);
        }
    }
}
