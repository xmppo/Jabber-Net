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
 * Portions Copyright (c) 2002 David Waite.
 * 
 * Acknowledgements
 * 
 * Special thanks to Dave Smith (dizzyd) for the design work.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

using bedrock.util;
using bedrock.collections;

using Org.Mentalis.Security.Certificates;

namespace bedrock.net
{
    /// <summary>
    /// A collection of sockets.  This makes a lot more sense in the poll() version (Unix/C) since
    /// you need to have a place to collect all of the sockets and call poll().  Here, it's just
    /// convenience functions.
    /// </summary>
    [RCS(@"$Header$")]
    public class SocketWatcher : IDisposable
    {
        private enum State
        {
            Running,
            Shutdown,
            Stopped
        };

        private ISet        m_pending = new Set(SetImplementation.SkipList);
        private ISet        m_socks   = new Set(SetImplementation.SkipList);
        private object      m_lock = new object();
        private int         m_maxSocks;
        private Certificate m_cert = null;

        /// <summary>
        /// Create a new instance, which will manage an unlimited number of sockets.
        /// </summary>
        public SocketWatcher()
        {
            m_maxSocks = -1;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="maxsockets">Maximum number of sockets to watch.  In this version, 
        /// this is mostly for rate-limiting purposes.</param>
        public SocketWatcher(int maxsockets)
        {
            m_maxSocks = maxsockets;
        }

        /// <summary>
        /// The maximum number of sockets watched.  Throws 
        /// InvalidOperationException if the new value is fewer than the number 
        /// currently open.  -1 means no limit.
        /// </summary>
        public int MaxSockets
        {
            get { return m_maxSocks; }
            set 
            { 
                lock(m_lock)
                {
                    if ((value >= 0) && (m_socks.Count >= value))
                        throw new InvalidOperationException("Too many sockets: " + m_socks.Count);
                
                    m_maxSocks = value; 
                }
            }
        }

        /// <summary>
        /// The certificate to be used for listen sockets, with SSL on.
        /// </summary>
        public Certificate LocalCertificate
        {
            get { return m_cert; }
            set { m_cert = value; }
        }

        /// <summary>
        /// Set the certificate to be used for accept sockets.  To generate a test .pfx file using openssl,
        /// add this to openssl.conf:
        ///   <blockquote>
        ///   [ serverex ]
        ///   extendedKeyUsage=1.3.6.1.5.5.7.3.1
        ///   </blockquote>
        /// and run the following commands:
        ///   <blockquote>
        ///   openssl req -new -x509 -newkey rsa:1024 -keyout privkey.pem -out key.pem -extensions serverex
        ///   openssl pkcs12 -export -in key.pem -in key privkey.pem -name localhost -out localhost.pfx
        ///   </blockquote>
        /// If you leave the certificate null, and you are doing Accept, the SSL class will try to find a
        /// default server cert on your box.  If you have IIS installed with a cert, this might just go...
        /// </summary>
        /// <param name="filename">A .pfx or .cer file</param>
        /// <param name="password">The password, if this is a .pfx file, null if .cer file.</param>
        public void SetCertificateFile(string filename, string password)
        {
            if (!File.Exists(filename)) 
            {
                throw new CertificateException("File does not exist: " + filename);
            }
            CertificateStore cs;
            if (password != null) 
            {
                cs = CertificateStore.CreateFromPfxFile(filename, password);
            } 
            else 
            {
                cs = CertificateStore.CreateFromCerFile(filename);
            }
            m_cert = cs.FindCertificate(new string[] {"1.3.6.1.5.5.7.3.1"});
            if (m_cert == null)
                throw new CertificateException("The certificate file does not contain a server authentication certificate.");
        }

        /// <summary>
        /// Set the certificate from a system store.  Try "MY" for the ones listed in IE.
        /// </summary>
        /// <param name="storeName"></param>
        public void SetCertificateStore(string storeName)
        {
            CertificateStore cs = new CertificateStore(storeName);
            m_cert = cs.FindCertificate(new string[] {"1.3.6.1.5.5.7.3.1"});
            if (m_cert == null)
                throw new CertificateException("The certificate file does not contain a server authentication certificate.");
        }

        /// <summary>
        /// Create a socket that is listening for inbound connections.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <param name="backlog">The maximum length of the queue of pending connections</param>
        /// <param name="SSL">Do SSL3/TLS1 on connect</param>
        /// <returns>A socket that is ready for calling RequestAccept()</returns>
        public AsyncSocket CreateListenSocket(ISocketEventListener listener,
                                              Address              addr,
                                              int                  backlog,
                                              bool                 SSL)
        {
            //Debug.Assert(m_maxSocks > 1);
            AsyncSocket result = new AsyncSocket(this, listener, SSL);
            if (SSL)
            {
#if !NO_SSL
                result.LocalCertificate = m_cert;
#else
                throw new NotImplementedException("SSL not compiled in");
#endif
            }
            result.Accept(addr, backlog);
            return result;
        }

        /// <summary>
        /// Create a socket that is listening for inbound connections.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <param name="SSL">Do SSL3/TLS1 on connect</param>
        /// <returns>A socket that is ready for calling RequestAccept()</returns>
        public AsyncSocket CreateListenSocket(ISocketEventListener listener,
                                              Address              addr,
                                              bool                 SSL)
        {
            return CreateListenSocket(listener, addr, 5, SSL);
        }

        /// <summary>
        /// Create a socket that is listening for inbound connections, with no SSL/TLS.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <returns>A socket that is ready for calling RequestAccept()</returns>
        public AsyncSocket CreateListenSocket(ISocketEventListener listener,
            Address              addr)
        {
            return CreateListenSocket(listener, addr, 5, false);
        }

        /// <summary>
        /// Create a socket that is listening for inbound connections, with no SSL/TLS.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <param name="backlog">The maximum length of the queue of pending connections</param>
        /// <returns>A socket that is ready for calling RequestAccept()</returns>
        public AsyncSocket CreateListenSocket(ISocketEventListener listener,
                                              Address              addr,
                                              int                  backlog)
        {
            return CreateListenSocket(listener, addr, backlog, false);
        }

        /// <summary>
        /// Create an outbound socket.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <returns>Socket that is in the process of connecting</returns>
        public AsyncSocket CreateConnectSocket(ISocketEventListener listener,
                                               Address              addr)
        {
            return CreateConnectSocket(listener, addr, false);
        }

        /// <summary>
        /// Create an outbound socket.
        /// </summary>
        /// <param name="listener">Where to send notifications</param>
        /// <param name="addr">Address to connect to</param>
        /// <param name="SSL">Do SSL3/TLS1 on startup</param>
        /// <returns>Socket that is in the process of connecting</returns>
        public AsyncSocket CreateConnectSocket(ISocketEventListener listener,
                                               Address              addr,
                                               bool                 SSL)
        {
            AsyncSocket result;
   
            // Create the socket:
            result = new AsyncSocket(this, listener, SSL);
            if (SSL)
            {
#if !NO_SSL
                result.LocalCertificate = m_cert;
#else
                throw new NotImplementedException("SSL not compiled in");
#endif
            }
            // Start the connect process:
            result.Connect(addr);
            return result;
        }

        /// <summary>
        /// Called by AsyncSocket when a new connection is received on a listen socket.
        /// </summary>
        /// <param name="s">New socket connection</param>
        public void RegisterSocket(AsyncSocket s)
        {
            
            lock (m_lock)
            {
                if ((m_maxSocks >= 0) && (m_socks.Count >= m_maxSocks))
                    throw new InvalidOperationException("Too many sockets: " + m_socks.Count);
                m_socks.Add(s);
            }
        }

        /// <summary>
        /// Called by AsyncSocket when a socket is closed.
        /// </summary>
        /// <param name="s">Closed socket</param>
        public void CleanupSocket(AsyncSocket s)
        {
            lock (m_lock)
            {
                m_socks.Remove(s);
                
                if (m_pending.Contains(s))
                {
                    m_pending.Remove(s);
                }
                else 
                {
                    foreach (AsyncSocket sock in m_pending)
                    {
                        sock.RequestAccept();
                    }
                    m_pending.Clear();
                }
            }
        }

        /// <summary>
        /// Called by AsyncSocket when this class is full, and the listening AsyncSocket 
        /// socket would like to be restarted when there are slots free.
        /// </summary>
        /// <param name="s">Listening socket</param>
        public void PendingAccept(AsyncSocket s)
        {
            lock (m_lock)
            {
                m_pending.Add(s);
            }
        }

        /// <summary>
        /// Or close.  Potato, tomato.  This is useful if you want to use using().
        /// </summary>
        public void Dispose()
        {
            lock (m_lock)
            {
                m_pending.Clear();
                foreach (AsyncSocket s in m_socks)
                {
                    s.Close();
                }
                m_socks.Clear();
            }
        }        
    }
}
