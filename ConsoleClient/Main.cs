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
    }
}
