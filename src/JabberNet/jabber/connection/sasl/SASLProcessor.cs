/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See licenses/Jabber-Net_LGPLv3.txt for details.
 * --------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Text;
using System.Xml;
using JabberNet.jabber.protocol.stream;

namespace JabberNet.jabber.connection.sasl
{
    /// <summary>
    /// A SASL processor instance has been created.  Fill it with information, like USERNAME and PASSWORD.
    /// </summary>
    public delegate void SASLProcessorHandler(Object sender, SASLProcessor proc);

    /// <summary>
    /// Some sort of SASL error
    /// </summary>
    public class SASLException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public SASLException(string message) : base(message){}

        /// <summary>
        ///
        /// </summary>
        public SASLException() : base(){}
    }

    /// <summary>
    /// Authentication failed.
    /// </summary>
    public class AuthenticationFailedException : SASLException
    {
        /// <summary>
        ///
        /// </summary>
        public AuthenticationFailedException() : base()
        {}

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public AuthenticationFailedException(string message) : base(message)
        {}
    }

    /// <summary>
    /// A required directive wasn't supplied.
    /// </summary>
    public class MissingDirectiveException : SASLException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public MissingDirectiveException(string message) : base(message)
        {}
    }

    /// <summary>
    /// Server sent an invalid challenge
    /// </summary>
    public class InvalidServerChallengeException : SASLException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public InvalidServerChallengeException(string message) : base(message)
        {}
    }
    /// <summary>
    /// Summary description for SASLProcessor.
    /// </summary>
    public abstract class SASLProcessor
    {
        /// <summary>
        /// SASL username
        /// </summary>
        public const string USERNAME = "username";
        /// <summary>
        /// SASL password
        /// </summary>
        public const string PASSWORD = "password";


        /// <summary>
        ///
        /// </summary>
        private Hashtable m_directives = new Hashtable();

        /// <summary>
        /// Create a new SASLProcessor, of the best type possible
        /// </summary>
        /// <param name="mechs">The mechanisms supported by the server</param>
        /// <param name="mt">The types the server implements</param>
        /// <param name="plaintextOK">Is it ok to select insecure types?</param>
        /// <param name="useClientCertificate">
        /// <c>true</c> if the connection have an associated local client certificate.
        /// </param>
        /// <param name="useAnonymous"></param>
        /// <returns></returns>
        public static SASLProcessor createProcessor(
            Mechanisms mechs,
            MechanismType mt,
            bool plaintextOK,
            bool useClientCertificate,
            bool useAnonymous)
        {
            if (useAnonymous && (mt & MechanismType.ANONYMOUS) == MechanismType.ANONYMOUS)
            {
                return new AnonymousProcessor();
            }

            if (mt.HasFlag(MechanismType.EXTERNAL) && useClientCertificate)
            {
                return new ExternalProcessor();
            }
            else if ((mt & MechanismType.GSSAPI) == MechanismType.GSSAPI)
            {
                string RemotePrincipal = "";
                foreach (Mechanism mechanism in mechs.GetMechanisms())
                {
                    if (mechanism.MechanismName == "GSSAPI")
                    {
                        RemotePrincipal = mechanism.GetAttribute("kerb:principal");
                        break;
                    }
                }
                return new KerbProcessor(RemotePrincipal);
            }
            if ((mt & MechanismType.DIGEST_MD5) == MechanismType.DIGEST_MD5)
            {
                return new MD5Processor();
            }
            else if (plaintextOK && ((mt & MechanismType.PLAIN) == MechanismType.PLAIN))
            {
                return new PlainProcessor();
            }
            return null;
        }

        /// <summary>
        /// Data for performing SASL challenges and responses.
        /// </summary>
        public string this[string directive]
        {
            get { return (string) m_directives[directive]; }
            set { m_directives[directive] = value; }
        }

        /// <summary>
        /// Perform the next step
        /// </summary>
        /// <param name="s">Null if it's the initial response</param>
        /// <param name="doc">Document to create Steps in</param>
        /// <returns></returns>
        public abstract Step step(Step s, XmlDocument doc);

        /// <summary>
        /// byte array as a hex string, two chars per byte.
        /// </summary>
        /// <param name="buf">Byte array</param>
        /// <returns></returns>
        protected static string HexString(byte[] buf)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in buf)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
