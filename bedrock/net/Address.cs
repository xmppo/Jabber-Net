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
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Globalization;
using bedrock.util;
namespace bedrock.net
{
    /// <summary>
    /// Callback for async DNS lookups.
    /// </summary>
    public delegate void AddressResolved(Address addr);
    /// <summary>
    /// Encapsulation and caching of IP address information.  Very similar to System.Net.IPEndPoint,
    /// but adds async DNS lookups.  
    /// TODO: add SRV?
    /// </summary>
    [RCS(@"$Header$")]
    public class Address
    {
        private string    m_hostname = null;
        private int       m_port     = -1;
        private IPAddress m_ip       = IPAddress.Any;
        /// <summary>
        /// Address for a server, corresponding to IPAddress.Any.
        /// </summary>
        /// <param name="port"></param>
        public Address(int port)
        {
            m_port = port;
        }
        /// <summary>
        /// New connection endpoint.
        /// </summary>
        /// <param name="Hostname">Host name or dotted-quad IP address</param>
        /// <param name="port">Port number</param>
        public Address(string Hostname, int port) : this(port)
        {
            Debug.Assert(Hostname != null, "must supply a host name");
            this.Hostname = Hostname;
        }
        /// <summary>
        /// Create a new connection endpoint, where the IP address is already known.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public Address(IPAddress ip, int port) : this(port)
        {
            this.IP = ip;
        }
        /// <summary>
        /// The host name.  When set, checks for dotted-quad representation, to avoid
        /// async DNS call when possible.
        /// </summary>
        public string Hostname
        {
            get { return m_hostname; }
            set 
            {
                if ((value == null) || (value == ""))
                {
                    m_hostname = null;
                    m_ip = IPAddress.Any;
                    return;
                }
                if (m_hostname != value)
                {
                    m_hostname = value;
                
                    try
                    {
                        m_ip = IPAddress.Parse(m_hostname);
                    }
                    catch (FormatException) 
                    {
                        m_ip = null;
                    }
                }
            }
        }
            
        /// <summary>
        /// Port number.
        /// TODO: add string version that looks in /etc/services (or equiv)?
        /// </summary>
        public int Port
        {
            get { return m_port; }
            set 
            {
                Debug.Assert(value > 0);
                m_port = value; 
            }
        }
        /// <summary>
        /// The binary IP address.  Gives IPAddress.Any if resolution hasn't occured, and 
        /// null if resolution failed.
        /// </summary>
        public IPAddress IP
        {
            get { return m_ip; }
            set 
            {
                m_ip = value;
                m_hostname = m_ip.ToString();
            }
        }
        /// <summary>
        /// Not implemented yet.
        /// </summary>
        public string Service
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// An IPEndPoint for making socket connections with.
        /// </summary>
        public IPEndPoint Endpoint
        {
            get
            {
                if (m_ip == null)
                    return null;
                Debug.Assert(Port > 0, "NULL_ADDRESS used");
                return new IPEndPoint(m_ip, Port);
            }
            set
            {
                m_ip = value.Address;
                Port = value.Port;
            }
        }
        /// <summary>
        /// Async DNS lookup.  IP will be null in callback on failure.  Callback will
        /// be called immediately if IP is already known (e.g. dotted-quad).
        /// </summary>
        /// <param name="callback">Called when resolution complete.</param>
        public void Resolve(AddressResolved callback)
        {
            if ((m_ip != null) && (m_ip != IPAddress.Any))
            {
                callback(this);
            }
            else
            {
                Dns.BeginResolve(m_hostname, new AsyncCallback(OnResolved), callback);
            }
        }
        /// <summary>
        /// Synchronous DNS lookup.
        /// </summary>
        public void Resolve()
        {
            if ((m_ip != null) && (m_ip != IPAddress.Any))
            {
                return;
            }
            Debug.Assert(m_hostname != null, "Must set hostname first");
            IPHostEntry iph = Dns.Resolve(m_hostname);
            // TODO: what happens here on error?
            m_ip = iph.AddressList[0];
        }
        /// <summary>
        /// Handle the async DNS response.
        /// </summary>
        /// <param name="ar"></param>
        private void OnResolved(IAsyncResult ar)
        {
            try
            {
                IPHostEntry ent = Dns.EndResolve(ar);
                if (ent.AddressList.Length <= 0)
                {
                    m_ip = null;
                }
                else
                {
                    // From docs: 
                    // When hostName is a DNS-style host name associated with multiple IP addresses, 
                    // only the first IP address that resolves to that host name is returned.
                    m_ip = ent.AddressList[0];
                }
            }
            catch (SocketException)
            {
                m_ip = null;
            }
            AddressResolved callback = (AddressResolved) ar.AsyncState;
            if (callback != null)
                callback(this);
        }
        /// <summary>
        /// Readable representation of the address.
        /// Host (IP):port
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}({1}):{2}", m_hostname, m_ip, m_port);
        }
    }
}
