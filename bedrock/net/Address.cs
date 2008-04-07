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
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Globalization;

using bedrock.util;

#if !__MonoCS__
using netlib.Dns;
using netlib.Dns.Records;
#endif

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
    [SVN(@"$Id$")]
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
        /// <param name="hostname">Host name or dotted-quad IP address</param>
        /// <param name="port">Port number</param>
        public Address(string hostname, int port) : this(port)
        {
            Debug.Assert(hostname != null, "must supply a host name");
            this.Hostname = hostname;
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


#if !__MonoCS__
        private static SRVRecord PickSRV(SRVRecord[] srv)
        {
            // TODO: keep track of connection failures, and try the next priority down.

            if ((srv == null) || (srv.Length == 0))
                throw new ArgumentException();
            if (srv.Length == 1)
                return srv[0];

            // randomize order.  One might wish that the OS would have done this for us.
            // cf. Bob Schriter's Grandfather.
            Random rnd = new Random();
            byte[] keys = new byte[srv.Length];
            rnd.NextBytes(keys);
            Array.Sort(keys, srv);  // Permute me, Knuth!  (I wish I had a good anagram for that)

            int minpri = int.MaxValue;
            foreach (SRVRecord rec in srv)
            {
                if (rec.Priority < minpri)
                {
                    minpri = rec.Priority;
                }
            }

            int weight = 0;
            foreach (SRVRecord rec in srv)
            {
                if (rec.Priority == minpri)
                {
                    weight += rec.Weight;
                }
            }

            int pos = rnd.Next(weight);
            weight = 0;
            foreach (SRVRecord rec in srv)
            {
                if (rec.Priority == minpri)
                {
                    weight += rec.Weight;
                    if ((pos < weight) || (weight == 0))
                    {
                        return rec;
                    }
                }
            }

            throw new DnsException("No matching SRV");
        }

        /// <summary>
        /// Look up a DNS SRV record, returning the best host and port number to connect to.
        /// </summary>
        /// <param name="prefix">The SRV prefix, ending with a dot.  Example: "_xmpp-client._tcp."</param>
        /// <param name="domain">The domain to check</param>
        /// <param name="host">The host name to connect to</param>
        /// <param name="port">The port number to connect to</param>
        public static void LookupSRV(string prefix, string domain, ref string host, ref int port)
        {
            if (prefix == null)
                throw new ArgumentNullException("prefix");
            if (domain == null)
                throw new ArgumentNullException("domain");
            if (!prefix.EndsWith("."))
                throw new ArgumentOutOfRangeException("Prefix must end in '.'", "prefix");
            try
            {
                DnsRequest request = new DnsRequest(prefix + domain);
                DnsResponse response = request.GetResponse(DnsRecordType.SRV);

                SRVRecord record = PickSRV(response.SRVRecords);
                host = record.NameNext;
                port = record.Port;
                Debug.WriteLine(string.Format("SRV found: {0}:{1}", host, port));
            }
            catch
            {
                host = domain;
            }
        }

        /// <summary>
        /// Look up a DNS TXT record.
        /// </summary>
        /// <param name="prefix">The prefix, ending in '.'.  Example: "_xmppconnect."</param>
        /// <param name="domain">The domain to search</param>
        /// <param name="attribute">The attribute name to look for.  Example: "_xmpp-client-xbosh"</param>
        /// <returns></returns>
        public static string LookupTXT(string prefix, string domain, string attribute)
        {
            if (prefix == null)
                throw new ArgumentNullException("prefix");
            if (domain == null)
                throw new ArgumentNullException("domain");
            if (attribute == null)
                throw new ArgumentNullException("attribute");
            if (!prefix.EndsWith("."))
                throw new ArgumentOutOfRangeException("Prefix must end in '.'", "prefix");

            try
            {
                DnsRequest request = new DnsRequest(prefix + domain);
                DnsResponse response = request.GetResponse(DnsRecordType.TEXT);
                string attr = attribute + "=";
                foreach (TXTRecord txt in response.TXTRecords)
                {
                    if (txt.StringArray.StartsWith(attr))
                    {
                        Debug.WriteLine(string.Format("TXT found: {0}", txt.StringArray));
                        return txt.StringArray.Substring(attr.Length);
                    }
                }
            }
            catch
            {
            }
            return null;
        }
#endif

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
            if ((m_ip != null) && (m_ip != IPAddress.Any)
                && (m_ip != IPAddress.IPv6Any)
                )
            {
                callback(this);
            }
            else
                Dns.BeginGetHostEntry(m_hostname, new AsyncCallback(OnResolved), callback);
        }

        /// <summary>
        /// Synchronous DNS lookup.
        /// </summary>
        public void Resolve()
        {
            if ((m_ip != null) && 
                (m_ip != IPAddress.Any) && 
                (m_ip != IPAddress.IPv6Any))
            {
                return;
            }
            Debug.Assert(m_hostname != null, "Must set hostname first");
            IPHostEntry iph = Dns.GetHostEntry(m_hostname);

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
                IPHostEntry ent = Dns.EndGetHostEntry(ar);
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
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
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
