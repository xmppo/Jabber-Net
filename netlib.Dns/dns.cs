using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using netlib.Dns.Records;

namespace netlib.Dns
{
    /// <summary>
	/// Represents an IPv6 IP Address
	/// </summary>
	/// <remarks>
	/// This struct is used by various classes in
	/// the <see cref="netlib.Dns.Records"/> namespace to represent
	/// IPv6 addresses.
	/// </remarks>
	public struct IP6Address
	{
		/// <summary>
		/// IP fragment 1
		/// </summary>
		/// <remarks>
		/// IP fragment 1
		/// </remarks>
		public uint IPFrag1;

		/// <summary>
		/// IP fragment 2
		/// </summary>
		/// <remarks>
		/// IP fragment 2
		/// </remarks>
		public uint IPFrag2;

		/// <summary>
		/// IP fragment 3
		/// </summary>
		/// <remarks>
		/// IP fragment 3
		/// </remarks>
		public uint IPFrag3;

		/// <summary>
		/// IP fragment 4
		/// </summary>
		/// <remarks>
		/// IP fragment 4
		/// </remarks>
		public uint IPFrag4;

		/// <summary>
		/// IP fragment 5
		/// </summary>
		/// <remarks>
		/// IP fragment 5
		/// </remarks>
		public uint IPFrag5;

		/// <summary>
		/// IP fragment 6
		/// </summary>
		/// <remarks>
		/// IP fragment 6
		/// </remarks>
		public uint IPFrag6;

		/// <summary>
		/// IP fragment 7
		/// </summary>
		/// <remarks>
		/// IP fragment 7
		/// </remarks>
		public uint IPFrag7;

		/// <summary>
		/// IP fragment 8
		/// </summary>
		/// <remarks>
		/// IP fragment 8
		/// </remarks>
		public uint IPFrag8;

		/// <summary>
		/// returns a string representation of the IP address v6
		/// </summary>
		/// <returns>a human readable ipv6 address</returns>
		/// <remarks>
		/// Used to display a human readable IPv6 address.
		/// </remarks>
		public override string ToString()
		{
			return String.Format(
				"{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
				IPFrag1.ToString("x"), 
				IPFrag2.ToString("x"),
				IPFrag3.ToString("x"), 
				IPFrag4.ToString("x"),
				IPFrag5.ToString("x"), 
				IPFrag6.ToString("x"),
				IPFrag7.ToString("x"), 
				IPFrag8.ToString("x")
				);
		}
	}

	/// <summary>
	/// Represents an array of IP addresses
	/// </summary>
	/// <remarks>
	/// This struct is used by the DnsQuery API to hold the selected
	/// DNS servers to query.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	struct IP4_Array
	{
		/// <summary>
		/// Gets or sets the number of element in the 
		/// <see cref="AddrArray"/> array.
		/// </summary>
		public int AddrCount;

		/// <summary>
		/// Gets or sets the array of IP addresses
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray)]
		public int[] AddrArray;
	}

	/// <summary>
	/// Represents a complete DNS record (DNS_RECORD)
	/// </summary>
	/// <remarks>
	/// This structure is used to hold a complete DNS record
	/// as returned from the DnsQuery API.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	struct DnsRecord
	{
		/// <summary>
		/// Gets or sets the next record.
		/// </summary>
		public IntPtr Next;// 4 bytes

		/// <summary>
		/// Gets or sets the name of the record.
		/// </summary>
		public string Name;// 4 bytes

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.U2)]		
		public DnsRecordType	RecordType;// 2 bytes

		/// <summary>
		/// Gets or sets the data length.
		/// </summary>
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.U2)]		
		public ushort	DataLength;// 2 bytes

		/// <summary>
		/// Represents the flags of a <see cref="DnsRecord"/>.
		/// </summary>
		[ StructLayout( LayoutKind.Explicit )]// 4 bytes
			internal struct DnsRecordFlags
		{
			/// <summary>
			/// Reserved.
			/// </summary>
			[ FieldOffset( 0 )]
			[MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]	
			public uint DW;

			/// <summary>
			/// Reserved.
			/// </summary>
			[ FieldOffset( 0 )]
			[MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]	
			public uint  S;
		}

		/// <summary>
		/// Gets or sets the flags.
		/// </summary>
		public DnsRecordFlags Flags;

		/// <summary>
		/// Gets or sets the TTL count
		/// </summary>
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]		
		public uint	Ttl;// 4 bytes

		/// <summary>
		/// Reserved.
		/// </summary>
		[MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]		
		public uint	Reserved;// 4 bytes
		// Can't fill in rest of the structure because if it doesn't line up, c# will complain.
	}

	/// <summary>
	/// DNS query types
	/// </summary>
	/// <remarks>
	/// This enum is used by the DnsQuery API call to describe the
	/// options to be given to a DNS server along with a query.
	/// </remarks>
	[Flags]
	enum DnsQueryType: uint
	{
		/// <summary>
		/// Standard
		/// </summary>
		STANDARD                  = 0x00000000,

		/// <summary>
		/// Accept truncated response
		/// </summary>
		ACCEPT_TRUNCATED_RESPONSE = 0x00000001,

		/// <summary>
		/// Use TCP only
		/// </summary>
		USE_TCP_ONLY              = 0x00000002,

		/// <summary>
		/// No recursion
		/// </summary>
		NO_RECURSION              = 0x00000004,

		/// <summary>
		/// Bypass cache
		/// </summary>
		BYPASS_CACHE              = 0x00000008,

		/// <summary>
		/// Cache only
		/// </summary>
		NO_WIRE_QUERY                = 0x00000010,

		/// <summary>
		/// Directs DNS to ignore the local name.
		/// </summary>
		NO_LOCAL_NAME      =      0x00000020,

		/// <summary>
		/// Prevents the DNS query from consulting the HOSTS file.
		/// </summary>
		NO_HOSTS_FILE      =      0x00000040,

		/// <summary>
		/// Prevents the DNS query from using NetBT for resolution.
		/// </summary>
		NO_NETBT           =      0x00000080,

		/// <summary>
		/// Directs DNS to perform a query using the network only, 
		/// bypassing local information.
		/// </summary>
		WIRE_ONLY          = 0x00000100,

		/// <summary>
		/// Treat as FQDN
		/// </summary>
		TREAT_AS_FQDN             = 0x00001000,

		/// <summary>
		/// Allow empty auth response
		/// </summary>
		[Obsolete]
		ALLOW_EMPTY_AUTH_RESP     = 0x00010000,

		/// <summary>
		/// Don't reset TTL values
		/// </summary>
		DONT_RESET_TTL_VALUES     = 0x00100000,

		/// <summary>
		/// Reserved.
		/// </summary>
		RESERVED                  = 0xff000000,

		/// <summary>
		/// obsolete.
		/// </summary>
		[Obsolete("use NO_WIRE_QUERY instead")]
		CACHE_ONLY = NO_WIRE_QUERY,

		/// <summary>
		/// Directs DNS to return the entire DNS response message.
		/// </summary>
		RETURN_MESSAGE     =       0x00000200

	}

	/// <summary>
	/// The possible return codes of the DNS API call. This enum can
	/// be used to decypher the <see cref="DnsException.ErrorCode"/>
	/// property's return value.
	/// </summary>
	/// <remarks>
	/// This enum is used to describe a failed return code by the
	/// DnsQuery API used in the <see cref="DnsRequest"/> class.
	/// </remarks>
	public enum DnsQueryReturnCode: ulong
	{
		/// <summary>
		/// Successful query
		/// </summary>
		SUCCESS = 0L,

		/// <summary>
		/// Base DNS error code
		/// </summary>
		UNSPECIFIED_ERROR = 9000,

		/// <summary>
		/// Base DNS error code
		/// </summary>
		MASK = 0x00002328, // 9000 or RESPONSE_CODES_BASE

		/// <summary>
		/// DNS server unable to interpret format.
		/// </summary>
		FORMAT_ERROR   =   9001L,

		/// <summary>
		/// DNS server failure.
		/// </summary>
		SERVER_FAILURE =   9002L,

		/// <summary>
		/// DNS name does not exist.
		/// </summary>
		NAME_ERROR    =    9003L,

		/// <summary>
		/// DNS request not supported by name server.
		/// </summary>
		NOT_IMPLEMENTED =  9004L,

		/// <summary>
		/// DNS operation refused.
		/// </summary>
		REFUSED       =    9005L,

		/// <summary>
		/// DNS name that ought not exist, does exist.
		/// </summary>
		YXDOMAIN     =     9006L,

		/// <summary>
		/// DNS RR set that ought not exist, does exist.
		/// </summary>
		YXRRSET     =      9007L,

		/// <summary>
		/// DNS RR set that ought to exist, does not exist.
		/// </summary>
		NXRRSET      =     9008L,

		/// <summary>
		/// DNS server not authoritative for zone.
		/// </summary>
		NOTAUTH      =     9009L,

		/// <summary>
		/// DNS name in update or prereq is not in zone.
		/// </summary>
		NOTZONE      =     9010L,

		/// <summary>
		/// DNS signature failed to verify.
		/// </summary>
		BADSIG      =      9016L,

		/// <summary>
		/// DNS bad key.
		/// </summary>
		BADKEY      =      9017L,

		/// <summary>
		/// DNS signature validity expired.
		/// </summary>
		BADTIME     =      9018L,

		/// <summary>
		/// Packet format
		/// </summary>
		PACKET_FMT_BASE = 9500,

		/// <summary>
		/// No records found for given DNS query.
		/// </summary>
		NO_RECORDS      =         9501L,

		/// <summary>
		/// Bad DNS packet.
		/// </summary>
		BAD_PACKET     =         9502L,

		/// <summary>
		/// No DNS packet.
		/// </summary>
		NO_PACKET      =         9503L,

		/// <summary>
		/// DNS error, check rcode.
		/// </summary>
		RCODE          =         9504L,

		/// <summary>
		/// Unsecured DNS packet.
		/// </summary>
		UNSECURE_PACKET   =      9505L
	}

	/// <summary>
	/// Possible arguments for the DnsRecordListFree api
	/// </summary>
	/// <remarks>
	/// This enum is used by the DnsRecordListFree API.
	/// </remarks>
	enum DnsFreeType: uint
	{
		/// <summary>
		/// Reserved.
		/// </summary>
		FreeFlat = 0,

		/// <summary>
		/// Frees the record list returned by the DnsQuery API
		/// </summary>
		FreeRecordList
	}

	/// <summary>
	/// Represents the exception that occurs when a <see cref="DnsRequest"/>
	/// fails.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The exception that occurs when a DNS request fails at any level.
	/// </para>
	/// <para>
	/// This class is used to represent two broad types of exceptions:
	/// <list type="bullet">
	///		<item>Win32 API Exceptions that occurred when calling the DnsQuery API</item>
	///		<item>Exceptions of other types that occurred when working with
	///		the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
	///		classes.</item>
	/// </list>
	/// </para>
	/// <para>
	/// Win32 errors that are DNS specific are specified in the
	/// <see cref="DnsQueryReturnCode"/> enumeration but if the 
	/// <see cref="ErrorCode"/> returned is not defined in that 
	/// enum then the number returned will be defined in WinError.h.
	/// </para>
	/// <para>
	/// Exceptions of other types are available through the 
	/// InnerException property.
	/// </para>
	/// </remarks>
	[Serializable]
	public class DnsException: ApplicationException, ISerializable
	{
		private readonly uint errcode = (uint) DnsQueryReturnCode.SUCCESS;

		/// <summary>
		/// Initializes a new instance of <see cref="DnsException"/>
		/// </summary>
		/// <remarks>
		/// Used to raise a <see cref="DnsException"/> with all the default
		/// properties. The message property will return: Unspecified
		/// DNS exception.
		/// </remarks>
		public DnsException(): base("Unspecified DNS exception")
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DnsException"/>
		/// </summary>
		/// <param name="message">the human readable description of the problem</param>
		/// <remarks>
		/// Used to raise a <see cref="DnsException"/> where the only important
		/// information is a description about the error. The <see cref="ErrorCode"/>
		/// property will return 0 or SUCCESS indicating that the DNS API calls
		/// succeeded, regardless of whether they did or did not.
		/// </remarks>
		public DnsException(string message): base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DnsException"/>
		/// </summary>
		/// <param name="message">the human readable description of the problem</param>
		/// <param name="errcode">the error code (<see cref="DnsQueryReturnCode"/>)
		/// if the DnsQuery api failed</param>
		/// <remarks>
		/// Used to raise a <see cref="DnsException"/> where the underlying DNS
		/// API call fails. In this case, the <see cref="ErrorCode"/> property
		/// is the most important information about the exception. In most cases,
		/// the number returned is a value in the <see cref="DnsQueryReturnCode"/>
		/// enum however, if it is not, the error is defined in WinError.h.
		/// </remarks>
		public DnsException(string message, uint errcode): base(message)
		{
			this.errcode = errcode;
		}

		/// <summary>
		/// Gets the error code (<see cref="DnsQueryReturnCode"/>)
		/// if the DnsQuery api failed. Will be set to success (0) if the API
		/// didn't fail but another part of the code did.
		/// </summary>
		/// <remarks>
		/// Win32 errors that are DNS specific are specified in the
		/// <see cref="DnsQueryReturnCode"/> enumeration but if the 
		/// <see cref="ErrorCode"/> returned is not defined in that 
		/// enum then the number returned will be defined in WinError.h.
		/// </remarks>
		/// <value>Value will be defined in WinError.h if not defined in the
		/// <see cref="DnsQueryReturnCode"/> enum.</value>
		/// <example>
		/// This example shows how to decypher the return of the
		/// ErrorCode property.
		/// <code>
		/// try
		/// {
		///		...
		/// }
		/// catch(DnsException dnsEx)
		/// {
		///		int errcode = dnsEx.ErrorCode;
		///		if (! Enum.IsDefined(typeof(DnsQueryReturnCode), errcode))
		///		{
		///			//defined in winerror.h
		///			Console.WriteLine("WIN32 Error: {0}", errcode);
		///			return;
		///		}
		///		
		///		DnsQueryReturnCode errretcode = (DnsQueryReturnCode) errcode;
		///		if (errretcode == DnsQueryReturnCode.SUCCESS)
		///		{
		///			//inner exception contains the goodies
		///			Console.WriteLine(dnsEx.InnerException.ToString());
		///			return;
		///		}
		///		
		///		//dns error
		///		Console.WriteLine("DNS Error: {0}", errretcode.ToString("g"));
		/// }
		/// </code>
		/// </example>
		public uint ErrorCode
		{
			get
			{
				return errcode;
			}
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DnsException"/>
		/// </summary>
		/// <param name="message">the human readable description of the 
		/// problem</param>
		/// <param name="innerException">the exception that caused the 
		/// underlying error</param>
		/// <remarks>
		/// Used to raise a <see cref="DnsException"/> where the exception is
		/// some other type but a typeof(DnsException) is desired to be raised
		/// instead. In this case, the <see cref="ErrorCode"/> property
		/// always returns 0 or SUCCESS and is a useless property.
		/// </remarks>
		public DnsException(string message, Exception innerException): base(message, innerException)
		{
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("errcode", errcode);
			base.GetObjectData(info, context);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DnsException"/> for <see cref="ISerializable"/>
		/// </summary>
		/// <param name="info">the serialization information</param>
		/// <param name="context">the context</param>
		/// <remarks>
		/// Used by the <see cref="ISerializable"/> interface.
		/// </remarks>
		public DnsException(SerializationInfo info, StreamingContext context): base(info, context)
		{
			errcode = info.GetUInt32("errcode");
		}
	}

	/// <summary>
	/// DNS record types
	/// </summary>
	/// <remarks>
	/// This enum represents all possible DNS record types that
	/// could be returned by the DnsQuery API.
	/// </remarks>
	[Flags]
	public enum DnsRecordType: ushort
	{
		/// <summary>
		/// Address record
		/// </summary>
		A          = 0x0001,      //  1

		/// <summary>
		/// Name Server record
		/// </summary>
		NS         = 0x0002,      //  2

		/// <summary>
		/// Obsolete
		/// </summary>
		[Obsolete]
		MD         = 0x0003,      //  3

		/// <summary>
		/// Obsolete
		/// </summary>
		[Obsolete]
		MF         = 0x0004,      //  4

		/// <summary>
		/// Canonical Name record
		/// </summary>
		CNAME      = 0x0005,      //  5

		/// <summary>
		/// Start Of Authority record
		/// </summary>
		SOA        = 0x0006,      //  6

		/// <summary>
		/// Mailbox record
		/// </summary>
		MB         = 0x0007,      //  7

		/// <summary>
		/// Reserved.
		/// </summary>
		MG         = 0x0008,      //  8

		/// <summary>
		/// Reserved.
		/// </summary>
		MR         = 0x0009,      //  9

		/// <summary>
		/// NULL data for a DNS resource record.
		/// </summary>
		NULL       = 0x000a,      //  10

		/// <summary>
		/// Well-Known Service record
		/// </summary>
		WKS        = 0x000b,      //  11

		/// <summary>
		/// Pointer record
		/// </summary>
		PTR        = 0x000c,      //  12

		/// <summary>
		/// Host Information record
		/// </summary>
		HINFO      = 0x000d,      //  13

		/// <summary>
		/// mail information (MINFO) record
		/// </summary>
		MINFO      = 0x000e,      //  14

		/// <summary>
		/// Mail Exchange record
		/// </summary>
		MX         = 0x000f,      //  15

		/// <summary> 
		/// Text record
		/// </summary>
		TEXT       = 0x0010,      //  16
		//  RFC 1183

		/// <summary>
		/// Responsible Person record
		/// </summary>
		RP         = 0x0011,      //  17

		/// <summary>
		/// AFS Data Base location record
		/// </summary>
		AFSDB      = 0x0012,      //  18

		/// <summary>
		/// X25
		/// </summary>
		X25        = 0x0013,      //  19

		/// <summary>
		/// ISDN
		/// </summary>
		ISDN       = 0x0014,      //  20

		/// <summary>
		/// Route Through
		/// </summary>
		RT         = 0x0015,      //  21

		/// <summary>
		/// Network service access point address record
		/// </summary>
		NSAP       = 0x0016,      //  22

		/// <summary>
		/// Obsolete
		/// </summary>
		[Obsolete]
		NSAPPTR    = 0x0017,      //  23

		/// <summary>
		/// Cryptographic signature record
		/// </summary>
		SIG        = 0x0018,      //  24

		/// <summary>
		/// Public key record
		/// </summary>
		KEY        = 0x0019,      //  25

		/// <summary>
		/// Pointer to X.400/RFC822 information record
		/// </summary>
		PX         = 0x001a,      //  26

		/// <summary>
		/// Geographical position record
		/// </summary>
		[Obsolete]
		GPOS       = 0x001b,      //  27

		/// <summary>
		/// IPv6 address record
		/// </summary>
		AAAA       = 0x001c,      //  28

		/// <summary>
		/// Location record
		/// </summary>
		LOC        = 0x001d,      //  29

		/// <summary>
		/// Next record
		/// </summary>
		NXT        = 0x001e,      //  30

		//  RFC 2052    (Service location)
		/// <summary>
		/// Server record
		/// </summary>
		SRV        = 0x0021,      //  33

		/// <summary>
		/// ATM address (ATMA) record
		/// </summary>
		ATMA       = 0x0022,      //  34

		/// <summary>
		/// TKEY resource record
		/// </summary>
		TKEY       = 0x00f9,      //  249

		/// <summary>
		/// secret key transaction authentication (TSIG) record
		/// </summary>
		TSIG       = 0x00fa,      //  250

		/// <summary>
		/// Reserved.
		/// </summary>
		IXFR       = 0x00fb,      //  251

		/// <summary>
		/// Reserved.
		/// </summary>
		AXFR       = 0x00fc,      //  252

		/// <summary>
		/// Reserved.
		/// </summary>
		MAILB      = 0x00fd,      //  253

		/// <summary>
		/// Reserved.
		/// </summary>
		MAILA      = 0x00fe,      //  254

		/// <summary>
		/// All records
		/// </summary>
		ALL        = 0x00ff,      //  255

		/// <summary>
		/// Any records
		/// </summary>
		ANY        = 0x00ff,      //  255

		/// <summary>
		/// WINS record
		/// </summary>
		WINS       = 0xff01,      //  64K - 255

		/// <summary>
		/// Windows Internet Name Service reverse-lookup record
		/// </summary>
		WINSR      = 0xff02,      //  64K - 254

		/// <summary>
		/// Windows Internet Name Service reverse-lookup record
		/// </summary>
		NBSTAT     = WINSR
	}

	/// <summary>
	/// Represents a collection of servers by hostname or ip address.
	/// </summary>
	/// <remarks>
	/// Represents a collection of DNS servers that were specified as
	/// hostnames or ip addresses. Regardless of the way the server
	/// was entered, it is resolved to an <see cref="IPAddress"/> object
	/// internally.
	/// </remarks>
	public class DnsServerCollection: CollectionBase, IEnumerable
	{
		internal IP4_Array ToIP4_Array()
		{
			ArrayList arr = new ArrayList();
			foreach(IPAddress ip in InnerList)
			{
				int ipaddr = (int) ip.Address;
				arr.Add(ipaddr);
			}

			int[] addresses = (int[]) arr.ToArray(typeof(int));

			IP4_Array ip4 = new IP4_Array();
			ip4.AddrArray = addresses;
			ip4.AddrCount = addresses.Length;
			return ip4;
		}

	    /// <summary>
		/// Adds a new hostname or ip address representing a DNS server
		/// to the collection.
		/// </summary>
		/// <param name="host">The ip address or hostname of a DNS server
		/// to add to the collection</param>
		/// <remarks>
		/// Adds a DNS server to the collection. The hostname or ip address
		/// is first resolved to one or more <see cref="IPAddress"/> instances
		/// and then added to the collection. If a given hostname resolves
		/// to 10 ip addresses, those 10 addresses will be added to the collection.
		/// </remarks>
		public void Add(string host)
		{
			IPHostEntry iphost = System.Net.Dns.Resolve(host);
			IPAddress[] ips = iphost.AddressList;
			InnerList.AddRange(ips);
		}

		/// <summary>
		/// Gets the <see cref="IPAddress"/> instance found at the current
		/// index of the collection.
		/// </summary>
		/// <remarks>
		/// Gets the <see cref="IPAddress"/> at the specified index of the
		/// collection.
		/// </remarks>
		/// <param name="idx">The index of the <see cref="IPAddress"/> to 
		/// retrieve from the collection.</param>
		/// <value>The <see cref="IPAddress"/> at the specified index.</value>
		public IPAddress this[int idx]
		{
			get
			{
				return (IPAddress) InnerList[idx];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DnsServerCollectionEnumerator(this);
		}

		class DnsServerCollectionEnumerator: IEnumerator
		{
			private int idx = -1;
			private readonly DnsServerCollection coll;

			public DnsServerCollectionEnumerator(DnsServerCollection coll)
			{
				this.coll=coll;
			}

			void IEnumerator.Reset()
			{
				idx = -1;
			}

			bool IEnumerator.MoveNext()
			{
				idx ++;

				return idx < coll.Count;
			}

			object IEnumerator.Current
			{
				get
				{
					return coll[idx];
				}
			}
		}
	}

	/// <summary>
	/// Represents a container for a DNS record of any type
	/// </summary>
	/// <remarks>
	/// The <see cref="DnsWrapper.RecordType"/> property's value
	/// helps determine what type real type of the 
	/// <see cref="DnsWrapper.RecordData"/> property returns as
	/// noted in this chart:
	/// <list type="table">
	///		<listheader>
	///			<term>RecordType</term>
	///			<term>RecordData</term>
	///		</listheader>
	///		<item>
	///			<term>A</term>
	///			<description><see cref="netlib.Dns.Records.ARecord"/></description>
	///		</item>
	///		<item>
	///			<term>CNAME</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>MB</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>MD</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>MF</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>MG</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>MR</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>NS</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>PTR</term>
    ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>HINFO</term>
    ///			<description><see cref="netlib.Dns.Records.TXTRecord"/></description>
	///		</item>
	///		<item>
	///			<term>ISDN</term>
    ///			<description><see cref="netlib.Dns.Records.TXTRecord"/></description>
	///		</item>
	///		<item>
	///			<term>X25</term>
    ///			<description><see cref="netlib.Dns.Records.TXTRecord"/></description>
	///		</item>
	///		<item>
	///			<term>MINFO</term>
    ///			<description><see cref="netlib.Dns.Records.MINFORecord"/></description>
	///		</item>
	///		<item>
	///			<term>RP</term>
    ///			<description><see cref="netlib.Dns.Records.MINFORecord"/></description>
	///		</item>
	///		<item>
	///			<term>MX</term>
    ///			<description><see cref="netlib.Dns.Records.MXRecord"/></description>
	///		</item>
	///		<item>
	///			<term>AFSDB</term>
    ///			<description><see cref="netlib.Dns.Records.MXRecord"/></description>
	///		</item>
	///		<item>
	///			<term>RT</term>
    ///			<description><see cref="netlib.Dns.Records.MXRecord"/></description>
	///		</item>
	///		<item>
	///			<term>NULL</term>
    ///			<description><see cref="netlib.Dns.Records.NULLRecord"/></description>
	///		</item>
	///		<item>
	///			<term>SOA</term>
    ///			<description><see cref="netlib.Dns.Records.SOARecord"/></description>
	///		</item>
	///		<item>
	///			<term>WKS</term>
    ///			<description><see cref="netlib.Dns.Records.WKSRecord"/></description>
	///		</item>
	///		<item>
	///			<term>AAAA</term>
    ///			<description><see cref="netlib.Dns.Records.AAAARecord"/></description>
	///		</item>
	///		<item>
	///			<term>ATMA</term>
    ///			<description><see cref="netlib.Dns.Records.ATMARecord"/></description>
	///		</item>
	///		<item>
	///			<term>NBSTAT</term>
    ///			<description><see cref="netlib.Dns.Records.WINSRRecord"/></description>
	///		</item>
	///		<item>
	///			<term>SRV</term>
    ///			<description><see cref="netlib.Dns.Records.SRVRecord"/></description>
	///		</item>
	///		<item>
	///			<term>TKEY</term>
    ///			<description><see cref="netlib.Dns.Records.TKEYRecord"/></description>
	///		</item>
	///		<item>
	///			<term>TSIG</term>
    ///			<description><see cref="netlib.Dns.Records.TSIGRecord"/></description>
	///		</item>
	///		<item>
	///			<term>WINS</term>
    ///			<description><see cref="netlib.Dns.Records.WINSRecord"/></description>
	///		</item>
	///		<item>
	///			<term>LOC</term>
    ///			<description><see cref="netlib.Dns.Records.LOCRecord"/></description>
	///		</item>
	///		<item>
	///			<term>AXFR</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>GPOS</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>IXFR</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>KEY</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>MAILA</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>MAILB</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>NSAP</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>NSAPPTR</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>NXT</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>PX</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>SIG</term>
	///			<description>null</description>
	///		</item>
	///		<item>
	///			<term>TEXT</term>
	///			<description>null</description>
	///		</item>
	/// </list>
	/// </remarks>
	public struct DnsWrapper: IComparable
	{
		/// <summary>
		/// Gets or sets the type of DNS record contained in the 
		/// <see cref="RecordData"/> property.
		/// </summary>
		/// <remarks>
		/// This property indicates the type of DNS record
		/// that the <see cref="RecordData"/> property is
		/// holding.
		/// </remarks>
		public DnsRecordType RecordType;

		/// <summary>
		/// Gets or sets the DNS record object as denoted in the 
		/// <see cref="RecordType"/> field.
		/// </summary>
		/// <remarks>
		/// This property holds the actual DNS record.
		/// </remarks>
		public object RecordData;

		/// <summary>
		/// Determines whether or not this <see cref="DnsWrapper"/>
		/// instance is equal to a specific <see cref="DnsRecordType"/>
		/// by comparing the <see cref="RecordType"/> property of the
		/// current <see cref="DnsWrapper"/> against the 
		/// <see cref="DnsRecordType"/> argument.
		/// </summary>
		/// <param name="type">The <see cref="DnsRecordType"/> to compare to.</param>
		/// <returns>A boolean indicating whether or not this <see cref="DnsWrapper"/>
		/// object contains a DNS record matching the entered type.</returns>
		/// <remarks>
		/// Determines if this <see cref="DnsWrapper"/> is of a specific
		/// <see cref="DnsRecordType"/>. The comparison does not test the
		/// <see cref="RecordData"/> field.
		/// </remarks>
		public bool Equals(DnsRecordType type)
		{
			if( RecordType == type)
				return true;

			return false;
		}

		/// <summary>
		/// Determines whether or not this <see cref="DnsWrapper"/> instance
		/// is equal to another <see cref="DnsWrapper"/> or to a 
		/// <see cref="DnsRecordType"/> instance.
		/// </summary>
		/// <param name="obj">The object to compare to this instance.</param>
		/// <returns>A boolean indicating whether or not this <see cref="DnsWrapper"/>
		/// object equals the entered object.</returns>
		/// <remarks>
		/// Determines if this <see cref="DnsWrapper"/> instance is equal to
		/// an object. If the object is a <see cref="DnsRecordType"/>, the
		/// <see cref="Equals(DnsRecordType)"/> method is used to determine
		/// equality based on the record type. If the object is a <see cref="DnsWrapper"/>
		/// object, the <see cref="CompareTo"/> method is used to determine
		/// equality. If the object is any other type, the <see cref="Object"/>
		/// class's Equal method is used for comparison.
		/// </remarks>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj is DnsRecordType)
				return Equals((DnsRecordType) obj);

			if (obj is DnsWrapper)
				return (CompareTo(obj) == 0 ? true : false);

			return base.Equals(obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable 
		/// for use in hashing algorithms and data structures like a 
		/// hash table.
		/// </summary>
		/// <returns>Integer value representing the hashcode of this 
		/// instance of <see cref="DnsWrapper"/>.</returns>
		/// <remarks>
		/// The GetHashCode method uses the hash codes of the <see cref="RecordData"/>
		/// and <see cref="RecordType"/> properties to generate a unique code
		/// for this particular record type/data combination.
		/// </remarks>
		public override int GetHashCode()
		{
			return RecordData.GetHashCode() ^ RecordType.GetHashCode();
		}

		#region IComparable Members

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the 
		/// comparands. The return value has these meanings:
		/// <list type="table">
		///		<listheader>
		///			<term>Value</term>
		///			<term>Meaning</term>
		///		</listheader>
		///		<item>
		///			<term>Less than zero</term>
		///			<description>This instance is less than obj. The <see cref="RecordData"/>
		///			types do not match.</description>
		///		</item>
		///		<item>
		///			<term>Zero</term>
		///			<description>This instance is equal to obj. </description>
		///		</item>
		///		<item>
		///			<term>Greater than zero</term>
		///			<description>This instance is greater than obj. The <see cref="RecordType"/>
		///			do not match.</description>
		///		</item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// Compares a <see cref="DnsWrapper"/> to this instance by its
		/// <see cref="RecordType"/> and <see cref="RecordData"/> properties.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// obj is not the same type as this instance.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (! (obj is DnsWrapper))
				throw new ArgumentException();

			DnsWrapper dnsw = (DnsWrapper) obj;
			if (RecordData.GetType() != dnsw.RecordData.GetType())
				return -1;

			if (RecordType != dnsw.RecordType)
				return 1;

			return 0;
		}

		#endregion
	}

	/// <summary>
	/// Represents a collection of <see cref="DnsWrapper"/> objects.
	/// </summary>
	/// <remarks>
	/// The DnsWrapperCollection is a collection of <see cref="DnsWrapper"/>
	/// objects. The resultant collection represents all of the DNS records
	/// for the given domain that was looked up. This class cannot be directly
	/// created - it is created by the <see cref="DnsRequest"/> and
	/// <see cref="DnsResponse"/> classes to hold the returned DNS
	/// records for the given domain.
	/// </remarks>
	public class DnsWrapperCollection: ReadOnlyCollectionBase, IEnumerable
	{
		internal DnsWrapperCollection()
		{
		}

		internal bool Contains(DnsWrapper w)
		{
			foreach(DnsWrapper wrapper in InnerList)
				if (w.Equals(wrapper))
					return true;

			return false;
		}

		internal void Add(DnsWrapper w)
		{
			InnerList.Add(w);
		}

		/// <summary>
		/// Gets the <see cref="DnsWrapper"/> at the specified
		/// ordinal in the collection
		/// </summary>
		/// <remarks>
		/// Gets the <see cref="DnsWrapper"/> at the specified
		/// index of the collection.
		/// </remarks>
		/// <param name="i">The index to retrieve from the collection.</param>
		/// <value>The <see cref="DnsWrapper"/> at the specified index of
		/// the collection.</value>
		public DnsWrapper this[int i]
		{
			get
			{
				return (DnsWrapper) InnerList[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DnsWrapperCollectionEnumerator(this);
		}

		class DnsWrapperCollectionEnumerator: IEnumerator
		{
			private int idx = -1;
			private readonly DnsWrapperCollection coll;

			public DnsWrapperCollectionEnumerator(DnsWrapperCollection coll)
			{
				this.coll = coll;
			}

			void IEnumerator.Reset()
			{
				idx=-1;
			}

			bool IEnumerator.MoveNext()
			{
				idx++;

				return idx < coll.Count;
			}

			object IEnumerator.Current
			{
				get
				{
					return coll[idx];
				}
			}
		}
	}

	/// <summary>
	/// Represents one DNS request. Allows for a complete DNS record lookup 
	/// on a given _Domain using the Windows API.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The DnsRequest class represents a complete DNS request for a given
	/// _Domain on a specified DNS server, including all options. The
	/// DnsRequest class uses the Windows API to do the query and the dlls
	/// used are only found on Windows 2000 or higher machines. The class
	/// will throw a <see cref="NotSupportedException"/> exception if run
	/// on an machine not capable of using the APIs that are required.
	/// </para>
	/// <para>
	/// Version Information
	/// </para>
	/// <para>
	///			3/8/2003 v1.1 (C#) - Released on 5/31/2003
	///	</para>
	///	<para>
	/// Created by: Bill Gearhart. Based on code by Patrik Lundin. 
	/// See version 1.0 remarks below. Specific attention was given
	/// to the exposed interface which got a 110% overhaul.
	/// </para>
	/// <para>
	/// Notable changes from the previous version:
	/// <list type="bullet">
	/// 	<item>
	/// 		<description>
	/// 			structs filled with constants were changed to enums
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			.net datatypes were changed to c# datatypes
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			every object is now in it's own *.cs file 
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			custom collections and exceptions added 
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			better object orientation - request and response classes 
	/// 			created for the dns query request/response session so that 
	/// 			it follows the .NET model
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			eliminated duplicate recs returned by an ALL query
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			bad api return code enumeration added
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			ToString() overridden to provide meaningful info for many 
	/// 			of the dns data structs
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			documentation and notes were created for all classes
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			added check to ensure code only runs on w2k or better
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			obsolete DNS record types are now marked as such
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			newer enum values added to DnsQueryType enum
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			compiled html documentation was written which always takes
	/// 			20 times longer than writing the code does.
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			this list of changes was compiled by your's truly...
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			smoothed out object and member names so they were more 
	/// 			intuitive - for instance: DNS_MX_DATA became MXRecord
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			added call to DnsRecordListFree API to free resources after 
	/// 			DnsQuery call
	/// 		</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>
	/// 			altered DnsQuery API call to allow for servers other than the 
	/// 			local DNS server from being queried
	/// 		</description>
	/// 	</item>
	/// </list>
	/// </para>
	/// <para>
	/// 	4/15/2002 v1.0 (C#)
	/// </para>
	/// <para>
	/// Created by: Patrik Lundin
	/// </para>
	/// <para>
	/// Based on code found at: 
	/// <a href="http://www.c-sharpcorner.com/Code/2002/April/DnsResolver.asp">http://www.c-sharpcorner.com/Code/2002/April/DnsResolver.asp</a>
	/// 		
	/// <list type="bullet">
	/// 	<item>
	/// 		<description>
	/// 			Initial implementation.
	/// 		</description>
	/// 	</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <example>
	/// Use the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> objects
	/// together to get DNS information for aspemporium.com from the nameserver
	/// where the site is hosted.
	/// <code>
	/// using System;
	/// using netlib.Dns;
	/// using netlib.Dns.Records;
	/// 
	/// namespace ClassLibrary1
	/// {
	/// 	class __loader
	/// 	{
	/// 		static void Main()
	/// 		{
	/// 			try
	/// 			{
	/// 				DnsRequest request = new DnsRequest();
	/// 				request.TreatAsFQDN=true;
	/// 				request.BypassCache=true;
	/// 				request.Servers.Add("dns.compresolve.com");
	/// 				request._domain = "aspemporium.com";
	/// 				DnsResponse response = request.GetResponse();
	/// 
	/// 				Console.WriteLine("Addresses");
	/// 				Console.WriteLine("--------------------------");
	/// 				foreach(ARecord addr in response.ARecords)
	/// 					Console.WriteLine("\t{0}", addr.ToString());
	/// 				Console.WriteLine();
	/// 
	/// 				Console.WriteLine("Name Servers");
	/// 				Console.WriteLine("--------------------------");
	/// 				foreach(PTRRecord ns in response.NSRecords)
	/// 					Console.WriteLine("\t{0}", ns.ToString());
	/// 				Console.WriteLine();
	/// 
	/// 				Console.WriteLine("Mail Exchanges");
	/// 				Console.WriteLine("--------------------------");
	/// 				foreach(MXRecord exchange in response.MXRecords)
	/// 					Console.WriteLine("\t{0}", exchange.ToString());
	/// 				Console.WriteLine();
	/// 
	/// 				Console.WriteLine("Canonical Names");
	/// 				Console.WriteLine("--------------------------");
	/// 				foreach(PTRRecord cname in response.GetRecords(DnsRecordType.CNAME))
	/// 					Console.WriteLine("\t{0}", cname.ToString());
	/// 				Console.WriteLine();
	/// 
	/// 				Console.WriteLine("Start of Authority Records");
	/// 				Console.WriteLine("--------------------------");
	/// 				foreach(SOARecord soa in response.GetRecords(DnsRecordType.SOA))
	/// 					Console.WriteLine("\t{0}", soa.ToString());
	/// 				Console.WriteLine();
	/// 
	/// 				//foreach(DnsWrapper wrap in response.RawRecords)
	/// 				//{
	/// 				//	Console.WriteLine(wrap.RecordType);
	/// 				//}
	/// 
	/// 				response = null;
	/// 				request = null;
	/// 			}
	/// 			catch(DnsException ex)
	/// 			{
	/// 				Console.WriteLine("EXCEPTION DOING DNS QUERY:");
	/// 				Console.WriteLine("\t{0}", ((DnsQueryReturnCode) ex.ErrorCode).ToString("g"));
	/// 
	/// 				if (ex.InnerException != null)
	/// 					Console.WriteLine(ex.InnerException.ToString());
	/// 			}
	/// 		}
	/// 	}
	/// }
	/// 
	/// </code>
	/// </example>
	/// 
	public class DnsRequest
	{
		/// <summary>
		/// http://msdn.microsoft.com/library/en-us/dns/dns/dnsquery.asp
		/// </summary>
		[DllImport("dnsapi", EntryPoint="DnsQuery_A")]
		private static extern uint DnsQuery( 
			[MarshalAs(UnmanagedType.LPStr)] 
			string Name, 

			[MarshalAs(UnmanagedType.U2)]
			DnsRecordType Type, 

			[MarshalAs(UnmanagedType.U4)]
			DnsQueryType Options, 

			IntPtr Servers,

			[In, Out]
			ref IntPtr QueryResultsSet,

			IntPtr Reserved 
			);

		/// <summary>
		/// http://msdn.microsoft.com/library/en-us/dns/dns/dnsrecordlistfree.asp
		/// </summary>
		[DllImport("dnsapi", EntryPoint="DnsRecordListFree")]
		private static extern void DnsRecordListFree(
			IntPtr RecordList,

			DnsFreeType FreeType
			);

		private DnsQueryType QueryType;
		private string _Domain;
		private DnsServerCollection servers;

		/// <summary>
		/// Gets a collection of DNS servers to use for the current request.
		/// If the collection contains no items, the local DNS servers are used.
		/// </summary>
		/// <remarks>
		/// If the collection contains 0 <see cref="System.Net.IPAddress"/> references,
		/// the default DNS servers are used. Otherwise, servers are used in a 
		/// decending order from their ordinal position in the collection.
		/// You can add as many DNS Servers as you need to the collection by
		/// host name or IP address.
		/// </remarks>
		/// <value>A <see cref="DnsServerCollection"/> that can be used to 
		/// manage the DNS servers that will be used for the query.</value>
		public DnsServerCollection Servers
		{
			get
			{
				return servers;
			}
		}

		/// <summary>
		/// Gets or sets whether or not to use TCP only for the query.
		/// </summary>
		/// <value>Boolean indicating whether or not to use TCP instead of UDP for the query</value>
		/// <remarks>
		/// If set to true, the DNS query will be done via TCP rather than UDP. This
		/// is useful if the DNS service you are trying to reach is running on
		/// TCP but not on UDP.
		/// </remarks>
		public bool UseTCPOnly
		{
			get
			{
				return GetSetting(DnsQueryType.USE_TCP_ONLY);
			}

			set
			{
				SetSetting(DnsQueryType.USE_TCP_ONLY, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to accept truncated results — 
		/// does not retry under TCP.
		/// </summary>
		/// <value>Boolean indicating whether or not to accept truncated results.</value>
		/// <remarks>
		/// Determines wherher or not the server will be re-queried in the event
		/// that a response was truncated.
		/// </remarks>
		public bool AcceptTruncatedResponse
		{
			get
			{
				return GetSetting(DnsQueryType.ACCEPT_TRUNCATED_RESPONSE);
			}

			set
			{
				SetSetting(DnsQueryType.ACCEPT_TRUNCATED_RESPONSE, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to perform an iterative query
		/// </summary>
		/// <value>Boolean indicating whether or not to use recursion
		/// to resolve the query.</value>
		/// <remarks>
		/// Specifically directs the DNS server not to perform 
		/// recursive resolution to resolve the query.
		/// </remarks>
		public bool NoRecursion
		{
			get
			{
				return GetSetting(DnsQueryType.NO_RECURSION);
			}

			set
			{
				SetSetting(DnsQueryType.NO_RECURSION, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to bypass the resolver cache 
		/// on the lookup. This must be set to true if you specified
		/// a server in the <see cref="Servers"/> collection.
		/// </summary>
		/// <value>Boolean indicating whether or not to bypass the cache
		/// and use the list of servers in the <see cref="Servers"/>
		/// collection.
		/// </value>
		/// <remarks>
		/// Setting this to true allows you to specify one or more DNS servers
		/// to query instead of querying the local DNS cache and server.
		/// If false is set, the list of servers is ignored and the local DNS
		/// cache and server is used to resolve the query.
		/// </remarks>
		public bool BypassCache
		{
			get
			{
				return GetSetting(DnsQueryType.BYPASS_CACHE);
			}

			set
			{
				SetSetting(DnsQueryType.BYPASS_CACHE, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to direct DNS to perform a 
		/// query on the local cache only
		/// </summary>
		/// <value>Boolean indicating whether or not to only use the
		/// DNS cache to resolve a query.</value>
		/// <remarks>
		/// This option allows you to query the local DNS cache only instead
		/// of making a DNS request over either UDP or TCP.
		/// This property represents the logical opposite of the
		/// <see cref="WireOnly"/> property.
		/// </remarks>
		public bool QueryCacheOnly
		{
			get
			{
				return GetSetting(DnsQueryType.NO_WIRE_QUERY);
			}

			set
			{
				SetSetting(DnsQueryType.NO_WIRE_QUERY, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to direct DNS to perform a 
		/// query using the network only, bypassing local information.
		/// </summary>
		/// <value>Boolean indicating whether or not to use the
		/// network only instead of local information.</value>
		/// <remarks>
		/// This property represents the logical opposite of the
		/// <see cref="QueryCacheOnly"/> property.
		/// </remarks>
		public bool WireOnly
		{
			get
			{
				return GetSetting(DnsQueryType.WIRE_ONLY);
			}

			set
			{
				SetSetting(DnsQueryType.WIRE_ONLY, value);
			}
		}


		/// <summary>
		/// Gets or sets whether or not to direct DNS to ignore the 
		/// local name.
		/// </summary>
		/// <value>Boolean indicating whether or not to ignore the local name.</value>
		/// <remarks>
		/// Determines how the DNS query handles local names.
		/// </remarks>
		public bool NoLocalName
		{
			get
			{
				return GetSetting(DnsQueryType.NO_LOCAL_NAME);
			}

			set
			{
				SetSetting(DnsQueryType.NO_LOCAL_NAME, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to prevent the DNS query from 
		/// consulting the HOSTS file.
		/// </summary>
		/// <value>Boolean indicating whether or not to deny access to
		/// the HOSTS file when querying.</value>
		/// <remarks>
		/// Determines how the DNS query handles accessing the HOSTS file when
		/// querying for DNS information.
		/// </remarks>
		public bool NoHostsFile
		{
			get
			{
				return GetSetting(DnsQueryType.NO_HOSTS_FILE);
			}

			set
			{
				SetSetting(DnsQueryType.NO_HOSTS_FILE, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to prevent the DNS query from 
		/// using NetBT for resolution.
		/// </summary>
		/// <value>Boolean indicating whether or not to deny access to
		/// NetBT during the query.</value>
		/// <remarks>
		/// Determines how the DNS query handles accessing NetBT when
		/// querying for DNS information.
		/// </remarks>
		public bool NoNetbt
		{
			get
			{
				return GetSetting(DnsQueryType.NO_NETBT);
			}

			set
			{
				SetSetting(DnsQueryType.NO_NETBT, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to direct DNS to return 
		/// the entire DNS response message.
		/// </summary>
		/// <value>Boolean indicating whether or not to return the entire
		/// response.</value>
		/// <remarks>
		/// Determines how the DNS query expects the response to be
		/// received from the server.
		/// </remarks>
		public bool QueryReturnMessage
		{
			get
			{
				return GetSetting(DnsQueryType.RETURN_MESSAGE);
			}

			set
			{
				SetSetting(DnsQueryType.RETURN_MESSAGE, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to prevent the DNS 
		/// response from attaching suffixes to the submitted 
		/// name in a name resolution process.
		/// </summary>
		/// <value>Boolean indicating whether or not to allow
		/// suffix attachment during resolution.</value>
		/// <remarks>
		/// Determines how the DNS server handles suffix appending
		/// to the submitted name during name resolution.
		/// </remarks>
		public bool TreatAsFQDN
		{
			get
			{
				return GetSetting(DnsQueryType.TREAT_AS_FQDN);
			}

			set
			{
				SetSetting(DnsQueryType.TREAT_AS_FQDN, value);
			}
		}

		/// <summary>
		/// Gets or sets whether or not to store records 
		/// with the TTL corresponding to the minimum value 
		/// TTL from among all records
		/// </summary>
		/// <value>Boolean indicating whether or not to
		/// use TTL values from all records.</value>
		/// <remarks>
		/// Determines how the DNS query handles TTL values.
		/// </remarks>
		public bool DontResetTTLValues
		{
			get
			{
				return GetSetting(DnsQueryType.DONT_RESET_TTL_VALUES);
			}

			set
			{
				SetSetting(DnsQueryType.DONT_RESET_TTL_VALUES, value);
			}
		}

		private bool GetSetting(DnsQueryType type)
		{
			DnsQueryType srchval = type;
			bool isset = (QueryType & srchval) == srchval;
			return isset;
		}

		private void SetSetting(DnsQueryType type, bool newvalue)
		{
			DnsQueryType srchval = type;
			bool isset = (QueryType & srchval) == srchval;
			bool newset = newvalue;

			//compare
			if (isset.CompareTo(newset) == 0)
				return;

			//toggle
			QueryType ^= srchval;
		}

		/// <summary>
		/// Gets or sets the _Domain to query. The _Domain must be a hostname,
		/// not an IP address.
		/// </summary>
		/// <remarks>
		/// This method is expecting a hostname, not an IP address. The
		/// system will fail with a <see cref="DnsException"/> when
		/// <see cref="GetResponse"/> is called if _domain is an IP address.
		/// </remarks>
		/// <value>String representing the _Domain that DNS information
		/// is desired for. This should be set to a hostname and not an
		/// IP Address.</value>
		public string _domain
		{
			get
			{
				return _Domain;
			}
			set
			{
				_Domain=value;
			}
		}

		/// <summary>
		/// Creates a new instance of <see cref="DnsRequest"/>
		/// </summary>
		/// <remarks>
		/// The <see cref="_domain"/> property is set to null
		/// and all other properties have their default value
		/// of false, except for <see cref="TreatAsFQDN"/> which has a value
		/// of true. The system is set to use the local DNS
		/// server for all queries.
		/// </remarks>
		public DnsRequest()
		{
			Initialize(null);
		}

		/// <summary>
		/// Creates a new instance of <see cref="DnsRequest"/>
		/// </summary>
		/// <remarks>
		/// The <see cref="_domain"/> property is set to the domain
		/// argument and all other properties have their default value
		/// of false, except for <see cref="TreatAsFQDN"/> which has a value
		/// of true. The system is set to use the local DNS
		/// server for all queries.
		/// </remarks>
		/// <param name="domain">The hostname that DNS information is desired for.
		/// This should not be an ip address. For example: yahoo.com</param>
		public DnsRequest(string domain)
		{
			Initialize(domain);
		}

		private void Initialize(string domain)
		{
			servers = new DnsServerCollection();
			_domain=domain;
			QueryType = DnsQueryType.STANDARD|DnsQueryType.TREAT_AS_FQDN;
		}

		/// <summary>
		/// Queries the local DNS server for information about 
		/// this instance of <see cref="DnsRequest"/> and returns
		/// the response as a <see cref="DnsResponse"/>
		/// </summary>
		/// <returns>A <see cref="DnsResponse"/> object containing the response 
		/// from the DNS server.</returns>
		/// <exception cref="NotSupportedException">
		/// The code is running on a machine lesser than Windows 2000
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The <see cref="_domain"/> property is null
		/// </exception>
		/// <exception cref="DnsException">
		/// The DNS query itself failed or parsing of the returned 
		/// response failed
		/// </exception>
		/// <remarks>
		/// Returns a <see cref="DnsResponse"/> representing the response
		/// from the DNS server or one of the exceptions noted in the
		/// exceptions area, the most common of which is the
		/// <see cref="DnsException"/>.
		/// </remarks>
        public DnsResponse GetResponse(DnsRecordType dnstype)
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				throw new NotSupportedException("This API is found only on Windows NT or better.");

			if (_domain == null)
				throw new ArgumentNullException();

			string strDomain = _domain;
			DnsQueryType querytype = QueryType;
			
			object Data = new object();

			IntPtr pServers = IntPtr.Zero;
			IntPtr ppQueryResultsSet = IntPtr.Zero;
			try
			{
				if (servers.Count > 0)
				{
					IP4_Array ip4 = servers.ToIP4_Array();
					//allocate memory:
					//just trying to put the IP4_Array object into
					//memory doesn't work because SizeOf bitches
					//therefore, we just bypass that and sum the
					//size of the fields of the IP4_Array struct.
					//IP4_Array consists of an int plus an array of
					//ints representing IP addresses - therefore, you
					//take the (size of the array x 4 bytes) + 4 bytes
					//and you've got the size of the struct.
					int intsize = Marshal.SizeOf(typeof(int));
					int size = intsize + (intsize * ip4.AddrCount);
					pServers = Marshal.AllocCoTaskMem(size);
					Marshal.StructureToPtr(ip4, pServers, false);
				}

				uint ret = DnsQuery(strDomain, dnstype, querytype, pServers, ref ppQueryResultsSet, IntPtr.Zero);
				if (ret != 0)
					throw new DnsException("DNS query fails", ret);

				DnsResponse resp = new DnsResponse();
				// Parse the records.
				// Call function to loop through linked list and fill an array of records
				do
				{
					// Get the DNS_RECORD
					DnsRecord dnsrec = (DnsRecord)Marshal.PtrToStructure( 
						ppQueryResultsSet, 
						typeof(DnsRecord) 
						);
					
					// Get the Data part
					GetData(ppQueryResultsSet, ref dnsrec, ref Data);

					// Wrap data in a struct with the type and data
					DnsWrapper wrapper = new DnsWrapper();
					wrapper.RecordType = dnsrec.RecordType;
					wrapper.RecordData = Data;

                    // Note: this is *supposed* to return many records of the same type.  Don't check for uniqueness.
					// Add wrapper to array
					//if (! resp.RawRecords.Contains(wrapper))
					resp.RawRecords.Add( wrapper );

					ppQueryResultsSet = dnsrec.Next;
				} while (ppQueryResultsSet != IntPtr.Zero);

				return resp;
			}
			catch(DnsException)
			{
				throw;
			}
			catch(Exception ex)
			{
				throw new DnsException("unspecified error", ex);
			}
			finally
			{
				//ensure unmanaged code cleanup occurs

				//free pointer to DNS record block
				DnsRecordListFree(ppQueryResultsSet, DnsFreeType.FreeRecordList);

				//free memory if any DNS servers were specified.
				if (pServers != IntPtr.Zero)
					Marshal.FreeCoTaskMem(pServers);
			}
		}

		private static void GetData(IntPtr ptr, ref DnsRecord dnsrec, ref object Data)
		{
			int size = ptr.ToInt32() + Marshal.SizeOf( dnsrec );
			ptr = new IntPtr(size);// Skip over the header portion of the DNS_RECORD to the data portion.
			switch ( dnsrec.RecordType )
			{
				case DnsRecordType.A:
					Data = (ARecord)Marshal.PtrToStructure( ptr, typeof(ARecord) );
					break;

				case DnsRecordType.CNAME:
				case DnsRecordType.MB:
				case DnsRecordType.MG:
				case DnsRecordType.MR: 
				case DnsRecordType.NS:
				case DnsRecordType.PTR:
					Data = (PTRRecord)Marshal.PtrToStructure( ptr, typeof(PTRRecord) );
					break;

				case DnsRecordType.HINFO:
				case DnsRecordType.ISDN:
				case DnsRecordType.X25:
					Data = (TXTRecord)Marshal.PtrToStructure( ptr, typeof(TXTRecord) );
					break;

				case DnsRecordType.MINFO:
				case DnsRecordType.RP:
					Data = (MINFORecord)Marshal.PtrToStructure( ptr, typeof(MINFORecord) );
					break;

				case DnsRecordType.MX:
				case DnsRecordType.AFSDB:
				case DnsRecordType.RT:
					Data = (MXRecord)Marshal.PtrToStructure( ptr, typeof(MXRecord) );
					break;

				case DnsRecordType.NULL:
					Data = (NULLRecord)Marshal.PtrToStructure( ptr, typeof(NULLRecord) );
					break;

				case DnsRecordType.SOA:
					Data = (SOARecord)Marshal.PtrToStructure( ptr, typeof(SOARecord) );
					break;

				case DnsRecordType.WKS:
					Data = (WKSRecord)Marshal.PtrToStructure( ptr, typeof(WKSRecord) );
					break;

				case DnsRecordType.AAAA:
					Data = (AAAARecord)Marshal.PtrToStructure( ptr, typeof(AAAARecord) );
					break;

				case DnsRecordType.ATMA:
					Data = (ATMARecord)Marshal.PtrToStructure( ptr, typeof(ATMARecord) );
					break;

				case DnsRecordType.NBSTAT:
					//case WINSR:
					Data = (WINSRRecord)Marshal.PtrToStructure( ptr, typeof(WINSRRecord) );
					break;

				case DnsRecordType.SRV:
					Data = (SRVRecord)Marshal.PtrToStructure( ptr, typeof(SRVRecord) );
					break;

				case DnsRecordType.TKEY:
					Data = (TKEYRecord)Marshal.PtrToStructure( ptr, typeof(TKEYRecord) );
					break;

				case DnsRecordType.TSIG:
					Data = (TSIGRecord)Marshal.PtrToStructure( ptr, typeof(TSIGRecord) );
					break;

				case DnsRecordType.WINS:
					Data = (WINSRecord)Marshal.PtrToStructure( ptr, typeof(WINSRecord) );
					break;

				case DnsRecordType.LOC:
					Data = (LOCRecord)Marshal.PtrToStructure( ptr, typeof(LOCRecord) );
					break;

				case DnsRecordType.AXFR:
				case DnsRecordType.IXFR:
				case DnsRecordType.KEY:
				case DnsRecordType.MAILA:
				case DnsRecordType.MAILB:
				case DnsRecordType.NSAP:
				case DnsRecordType.NXT:
				case DnsRecordType.PX:
				case DnsRecordType.SIG:
				case DnsRecordType.TEXT:
				default:
					Data = null;
					break;
			}
		}
	}

	/// <summary>
	/// Represents one DNS response. This class cannot be directly created - 
	/// it is returned by the <see cref="DnsRequest.GetResponse"/> method.
	/// </summary>
	/// <remarks>
	/// The DnsResponse class represents the information returned by a DNS 
	/// server in response to a <see cref="DnsRequest"/>. The DnsResponse
	/// class offers easy access to all of the returned DNS records for a given
	/// domain.
	/// </remarks>
	public class DnsResponse
	{
		private readonly DnsWrapperCollection rawrecords;

		internal DnsResponse()
		{
			rawrecords = new DnsWrapperCollection();
		}

		/// <summary>
		/// Gets a <see cref="DnsWrapperCollection" /> containing
		/// all of the DNS information that the server returned about
		/// the queried domain.
		/// </summary>
		/// <remarks>
		/// Returns all of the DNS records retrieved about the domain
		/// as a <see cref="DnsWrapperCollection"/>. This property
		/// is wrapped by the <see cref="GetRecords"/> method, the
		/// <see cref="ARecords"/>, <see cref="MXRecords"/>, and
		/// <see cref="NSRecords"/> properties.
		/// </remarks>
		/// <value>Gets a collection of <see cref="DnsWrapper"/> objects.</value>
		public DnsWrapperCollection RawRecords
		{
			get
			{
				return rawrecords;
			}
		}

		/// <summary>
		/// Returns a collection of DNS records of a specified
		/// <see cref="DnsRecordType"/>. The collection's data type
		/// is determined by the type of record being sought in the
		/// type argument.
		/// </summary>
		/// <param name="type">A <see cref="DnsRecordType"/> enumeration
		/// value indicating the type of DNS record to get from the list of
		/// all DNS records (available in the <see cref="RawRecords"/>
		/// property.</param>
		/// <returns>an <see cref="ArrayList"/> of one of the types
		/// specified in the <see cref="netlib.Dns.Records"/> namespace based
		/// on the <see cref="DnsRecordType"/> argument representing the
		/// type of DNS record desired.
		/// </returns>
		/// <remarks>
		/// It is recommended that you loop through the results of this
		/// method as follows for maximum convenience:
		/// <code>
        /// foreach (<see cref="netlib.Dns.Records"/> record in obj.GetRecords(<see cref="DnsRecordType"/>))
		/// {
		///		string s = record.ToString();
		/// }
		/// </code>
		/// The following table indicates the DNS record type you can expect to get
		/// back based on the <see cref="DnsRecordType"/> requested. Any items returning
		/// null are not currently supported.
		/// <list type="table">
		///		<listheader>
		///			<term>DnsRecordType enumeration value</term>
		///			<term>GetRecords() returns</term>
		///		</listheader>
		///		<item>
		///			<term>A</term>
        ///			<description><see cref="netlib.Dns.Records.ARecord"/></description>
		///		</item>
		///		<item>
		///			<term>CNAME</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>MB</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>MD</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>MF</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>MG</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>MR</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>NS</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>PTR</term>
        ///			<description><see cref="netlib.Dns.Records.PTRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>HINFO</term>
        ///			<description><see cref="netlib.Dns.Records.TXTRecord"/></description>
		///		</item>
		///		<item>
		///			<term>ISDN</term>
        ///			<description><see cref="netlib.Dns.Records.TXTRecord"/></description>
		///		</item>
		///		<item>
		///			<term>X25</term>
        ///			<description><see cref="netlib.Dns.Records.TXTRecord"/></description>
		///		</item>
		///		<item>
		///			<term>MINFO</term>
        ///			<description><see cref="netlib.Dns.Records.MINFORecord"/></description>
		///		</item>
		///		<item>
		///			<term>RP</term>
        ///			<description><see cref="netlib.Dns.Records.MINFORecord"/></description>
		///		</item>
		///		<item>
		///			<term>MX</term>
        ///			<description><see cref="netlib.Dns.Records.MXRecord"/></description>
		///		</item>
		///		<item>
		///			<term>AFSDB</term>
        ///			<description><see cref="netlib.Dns.Records.MXRecord"/></description>
		///		</item>
		///		<item>
		///			<term>RT</term>
        ///			<description><see cref="netlib.Dns.Records.MXRecord"/></description>
		///		</item>
		///		<item>
		///			<term>NULL</term>
        ///			<description><see cref="netlib.Dns.Records.NULLRecord"/></description>
		///		</item>
		///		<item>
		///			<term>SOA</term>
        ///			<description><see cref="netlib.Dns.Records.SOARecord"/></description>
		///		</item>
		///		<item>
		///			<term>WKS</term>
        ///			<description><see cref="netlib.Dns.Records.WKSRecord"/></description>
		///		</item>
		///		<item>
		///			<term>AAAA</term>
        ///			<description><see cref="netlib.Dns.Records.AAAARecord"/></description>
		///		</item>
		///		<item>
		///			<term>ATMA</term>
        ///			<description><see cref="netlib.Dns.Records.ATMARecord"/></description>
		///		</item>
		///		<item>
		///			<term>NBSTAT</term>
        ///			<description><see cref="netlib.Dns.Records.WINSRRecord"/></description>
		///		</item>
		///		<item>
		///			<term>SRV</term>
        ///			<description><see cref="netlib.Dns.Records.SRVRecord"/></description>
		///		</item>
		///		<item>
		///			<term>TKEY</term>
        ///			<description><see cref="netlib.Dns.Records.TKEYRecord"/></description>
		///		</item>
		///		<item>
		///			<term>TSIG</term>
        ///			<description><see cref="netlib.Dns.Records.TSIGRecord"/></description>
		///		</item>
		///		<item>
		///			<term>WINS</term>
        ///			<description><see cref="netlib.Dns.Records.WINSRecord"/></description>
		///		</item>
		///		<item>
		///			<term>LOC</term>
        ///			<description><see cref="netlib.Dns.Records.LOCRecord"/></description>
		///		</item>
		///		<item>
		///			<term>AXFR</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>GPOS</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>IXFR</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>KEY</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>MAILA</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>MAILB</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>NSAP</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>NSAPPTR</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>NXT</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>PX</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>SIG</term>
		///			<description>null</description>
		///		</item>
		///		<item>
		///			<term>TEXT</term>
		///			<description>null</description>
		///		</item>
		/// </list>
		/// </remarks>
		public ArrayList GetRecords(DnsRecordType type)
		{
			ArrayList arr = new ArrayList();
			foreach(DnsWrapper dnsentry in rawrecords)
				if (dnsentry.Equals(type))
					arr.Add(dnsentry.RecordData);

			return arr;
		}

		/// <summary>
		/// Gets all the <see cref="ARecord"/> for the queried domain.
		/// </summary>
		/// <remarks>
		/// Uses the <see cref="GetRecords"/> method to retrieve an
		/// array of <see cref="ARecord"/>s representing all the Address
		/// records for the domain.
		/// </remarks>
		/// <value>An array of <see cref="ARecord"/> objects.</value>
		public ARecord[] ARecords
		{
			get
			{
				ArrayList arr = GetRecords(DnsRecordType.A);
				return (ARecord[]) arr.ToArray(typeof(ARecord));
			}
		}

        /// <summary>
        /// Gets all the <see cref="SRVRecord"/> for the queried domain.
        /// </summary>
        /// <remarks>
        /// Uses the <see cref="GetRecords"/> method to retrieve an
        /// array of <see cref="SRVRecord"/>s representing all the Address
        /// records for the domain.
        /// </remarks>
        /// <value>An array of <see cref="SRVRecord"/> objects.</value>
        public SRVRecord[] SRVRecords
        {
            get
            {
                ArrayList arr = GetRecords(DnsRecordType.SRV);
                return (SRVRecord[])arr.ToArray(typeof(SRVRecord));
            }
        }

		/// <summary>
		/// Gets all the <see cref="MXRecord"/> for the queried domain.
		/// </summary>
		/// <remarks>
		/// Uses the <see cref="GetRecords"/> method to retrieve an
		/// array of <see cref="MXRecord"/>s representing all the Mail Exchanger
		/// records for the domain.
		/// </remarks>
		/// <value>An array of <see cref="MXRecord"/> objects.</value>
		public MXRecord[] MXRecords
		{
			get
			{
				ArrayList arr = GetRecords(DnsRecordType.MX);
				return (MXRecord[]) arr.ToArray(typeof(MXRecord));
			}
		}

		/// <summary>
		/// Gets all the DNS name servers for the queried domain as an
		/// array of <see cref="PTRRecord"/>s.
		/// </summary>
		/// <remarks>
		/// Uses the <see cref="GetRecords"/> method to retrieve an
		/// array of <see cref="PTRRecord"/>s representing all the Name Server
		/// records for the domain.
		/// </remarks>
		/// <value>An array of <see cref="PTRRecord"/> objects.</value>
		public PTRRecord[] NSRecords
		{
			get
			{
				ArrayList arr = GetRecords(DnsRecordType.NS);
				return (PTRRecord[]) arr.ToArray(typeof(PTRRecord));
			}
		}
	}

	namespace Records
	{
		/// <summary>
		/// Represents a DNS Well Known Service record (DNS_WKS_DATA)
		/// </summary>
		/// <remarks>
		/// The WKSRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct WKSRecord
		{
			/// <summary>
			/// Gets or sets the IP address
			/// </summary>
			/// <remarks>
			/// IP address, in the form of an IP4_ADDRESS structure. 
			/// </remarks>
			public uint IpAddress;

			/// <summary>
			/// Gets or sets the protocol
			/// </summary>
			/// <remarks>
			/// IP protocol for this record. Valid values are UDP or TCP. 
			/// </remarks>
			public char Protocol;

			/// <summary>
			/// Gets or sets the bitmask
			/// </summary>
			/// <remarks>
			/// Mask representing well known service being represented in the RR. 
			/// </remarks>
			public byte BitMask;			

			/// <summary>
			/// Returns a string representation of the service record
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// IP Address: [IPADDR] Protocol: [PROTO] BitMask: [BITMASK]
			/// where [IPADDR] = string representation of <see cref="IpAddress"/> as specified here <see cref="System.Net.IPAddress.ToString()"/>
			/// and   [PROTO] = string representation of <see cref="Protocol"/>
			/// and   [BITMASK] = hexadecimal representation of <see cref="BitMask"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format("IP Address: {0} Protocol: {1} BitMask: {2}",
					new IPAddress(IpAddress),
					Protocol,
					BitMask.ToString("x")
					);
			}
		}

		/// <summary>
		/// Represents a DNS Windows Internet Name Service reverse-lookup 
		/// (WINSR) record (DNS_WINSR_DATA)
		/// </summary>
		/// <remarks>
		/// The WINSRRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct WINSRRecord
		{
			/// <summary>
			/// Gets or sets the mapping flag
			/// </summary>
			/// <remarks>
			/// WINS mapping flag that specifies whether the record must be included 
			/// into the zone replication. It may have only two values: 0x80000000 
			/// and 0x00010000 corresponding to the replication and no-replication 
			/// (local record) flags, respectively. 
			/// </remarks>
			public uint	MappingFlag;

			/// <summary>
			/// Gets or sets the lookup timeout
			/// </summary>
			/// <remarks>
			/// Time, in seconds, that a DNS Server attempts resolution using WINS 
			/// lookup. 
			/// </remarks>
			public uint	LookupTimeout;

			/// <summary>
			/// Gets or sets the cache timeout
			/// </summary>
			/// <remarks>
			/// Time, in seconds, that a DNS Server using WINS lookup may cache the 
			/// WINS Server's response. 
			/// </remarks>
			public uint	CacheTimeout;

			/// <summary>
			/// Gets or sets the result domain name
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the domain name to append to the 
			/// returned NetBIOS name. 
			/// </remarks>
			public string ResultDomain;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// mapping flag: [FLAG] lookup timeout: [LOOKUP] cache timeout: [CACHE] result domain: [DOMAIN]
			/// where [FLAG] = string representation of <see cref="MappingFlag"/>
			/// and   [LOOKUP] = string representation of <see cref="LookupTimeout"/>
			/// and   [CACHE] = string representation of <see cref="CacheTimeout"/>
			/// and   [DOMAIN] = hexadecimal representation of <see cref="ResultDomain"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"mapping flag: {0} lookup timeout: {1} cache timeout: {2} result domain: {3}",
					MappingFlag,
					LookupTimeout,
					CacheTimeout,
					ResultDomain
					);
			}
		}

		/// <summary>
		/// Represents a DNS Windows Internet Name Service (WINS) record (DNS_WINS_DATA)
		/// </summary>
		/// <remarks>
		/// The WINSRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct WINSRecord
		{
			/// <summary>
			/// Gets or sets the mapping flag
			/// </summary>
			/// <remarks>
			/// WINS mapping flag that specifies whether the record must be 
			/// included into the zone replication. It may have only two values: 
			/// 0x80000000 and 0x00010000 corresponding to the replication and 
			/// no-replication (local record) flags, respectively. 
			/// </remarks>
			public uint	MappingFlag;

			/// <summary>
			/// Gets or sets the lookup timeout
			/// </summary>
			/// <remarks>
			/// Time, in seconds, that a DNS Server attempts resolution using 
			/// WINS lookup. 
			/// </remarks>
			public uint	LookupTimeout;

			/// <summary>
			/// Gets or sets the cache timeout
			/// </summary>
			/// <remarks>
			/// Time, in seconds, that a DNS Server using WINS lookup may cache 
			/// the WINS Server's response. 
			/// </remarks>
			public uint	CacheTimeout;

			/// <summary>
			/// Gets or sets the count of WINS servers
			/// </summary>
			/// <remarks>
			/// Number of WINS Servers listed in the WinsServers member. 
			/// </remarks>
			public uint	ServerCount;

			/// <summary>
			/// Gets or sets the WINS server array pointer
			/// </summary>
			/// <remarks>
			/// Array of WINS Servers, each of type int . 
			/// </remarks>
			public IntPtr WinsServers;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// mapping flag: [FLAG] lookup timeout: [LOOKUP] cache timeout: [CACHE] server count: [SERVERCT] server ptr: [SERVERS]
			/// where [FLAG] = string representation of <see cref="MappingFlag"/>
			/// and   [LOOKUP] = string representation of <see cref="LookupTimeout"/>
			/// and   [CACHE] = string representation of <see cref="CacheTimeout"/>
			/// and   [SERVERCT] = string representation of <see cref="ServerCount"/>
			/// and   [SERVERS] = string representation of <see cref="WinsServers"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"mapping flag: {0} lookup timeout: {1} cache timeout: {2} server count: {3} server ptr: {4}",
					MappingFlag,
					LookupTimeout,
					CacheTimeout,
					ServerCount,
					WinsServers
					);
			}
		}

		/// <summary>
		/// Represents a DNS Text record (DNS_TXT_DATA)
		/// </summary>
		/// <remarks>
		/// The TXTRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct TXTRecord
		{
			/// <summary>
			/// Gets or sets the string count
			/// </summary>
			/// <remarks>
			/// Number of strings represented in pStringArray. 
			/// </remarks>
			public uint StringCount;

			/// <summary>
			/// Gets or sets the string array
			/// </summary>
			/// <remarks>
			/// Array of strings representing the descriptive text of the 
			/// TXT resource record. 
			/// </remarks>
			public string StringArray;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// string count: [COUNT] string array: [ARR]
			/// where [COUNT] = string representation of <see cref="StringCount"/>
			/// and   [ARR] = string representation of <see cref="StringArray"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"string count: {0} string array: {1}",
					StringCount,
					StringArray
					);
			}
		}

		/// <summary>
		/// represents a secret key transaction authentication (TSIG) record (DNS_TSIG_DATA)
		/// </summary>
		/// <remarks>
		/// The TSIGRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct TSIGRecord
		{
			/// <summary>
			/// Gets or sets the name algorithm
			/// </summary>
			/// <remarks>
			/// Name of the key used in the domain name syntax. 
			/// </remarks>
			public string Algorithm;

			/// <summary>
			/// Gets or sets the algorithm packet
			/// </summary>
			/// <remarks>
			/// Pointer to the packet containing the algorithm. 
			/// </remarks>
			public IntPtr AlgorithmPacket;

			/// <summary>
			/// Gets or sets the key
			/// </summary>
			/// <remarks>
			/// Pointer to the signature. 
			/// </remarks>
			public IntPtr Key;

			/// <summary>
			/// Gets or sets the other data
			/// </summary>
			/// <remarks>
			/// Pointer to other data. This member is empty unless a BADTIME error is returned.
			/// </remarks>
			public IntPtr OtherData;

			/// <summary>
			/// Gets or sets the create time
			/// </summary>
			/// <remarks>
			/// Time the key transaction authentication was created, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds. 
			/// </remarks>
			public long CreateTime;

			/// <summary>
			/// Gets or sets the fudge time
			/// </summary>
			/// <remarks>
			/// Time, in seconds, from which the i64CreateTime may be in error. 
			/// </remarks>
			public ushort FudgeTime;

			/// <summary>
			/// Gets or sets the original XID
			/// </summary>
			/// <remarks>
			/// Original message identifier. 
			/// </remarks>
			public ushort OriginalXid;

			/// <summary>
			/// Gets or sets the error
			/// </summary>
			/// <remarks>
			/// Error, expressed in expanded RCODE that covers TSIG processing. See Remarks for more information about the TSIG resource record. 
			/// </remarks>
			public ushort Error;

			/// <summary>
			/// Gets or sets the key length
			/// </summary>
			/// <remarks>
			/// Length, in bytes, of the pSignature member. 
			/// </remarks>
			public ushort KeyLength;

			/// <summary>
			/// Gets or sets the other length
			/// </summary>
			/// <remarks>
			/// Length, in bytes, of the pOtherData member. 
			/// </remarks>
			public ushort OtherLength;

			/// <summary>
			/// Gets or sets the algorithm length
			/// </summary>
			/// <remarks>
			/// Length, in bytes, of the pNameAlgorithm member. 
			/// </remarks>
			public char AlgNameLength;

			/// <summary>
			/// Gets or sets whether or not to use packet pointers
			/// </summary>
			/// <remarks>
			/// Reserved for future use. 
			/// </remarks>
			public bool PacketPointers;
		}

		/// <summary>
		///  Represents a DNS TKEY resource record, used to 
		///  establish and delete shared-secret keys between 
		///  a DNS resolver and server. (DNS_TKEY_DATA)
		/// </summary>
		/// <remarks>
		/// The TKEYRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct TKEYRecord
		{
			/// <summary>
			/// Gets or sets the name algorithm
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the name of the algorithm 
			/// used with the key. 
			/// </remarks>
			public string Algorithm;

			/// <summary>
			/// Gets or sets the algorithm packet.
			/// </summary>
			/// <remarks>
			/// Pointer to the packet containing the algorithm. 
			/// </remarks>
			public IntPtr AlgorithmPacket;

			/// <summary>
			/// Gets or sets the key
			/// </summary>
			/// <remarks>
			/// Pointer to the key. 
			/// </remarks>
			public IntPtr Key;

			/// <summary>
			/// Gets or sets the other data
			/// </summary>
			/// <remarks>
			/// Reserved for future use. 
			/// </remarks>
			public IntPtr OtherData;

			/// <summary>
			/// Gets or sets the create time
			/// </summary>
			/// <remarks>
			/// Date and time at which the key was created, expressed in seconds 
			/// since the beginning of January 1, 1970, Greenwich Mean Time (GMT), 
			/// excluding leap seconds. 
			/// </remarks>
			public uint CreateTime;

			/// <summary>
			/// Gets or sets the expire time
			/// </summary>
			/// <remarks>
			/// Expiration date of the key, expressed in seconds since the beginning 
			/// of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds. 
			/// </remarks>
			public uint ExpireTime;

			/// <summary>
			/// Gets or sets the mode
			/// </summary>
			/// <remarks>
			/// Scheme used for key agreement or the purpose of the TKEY DNS Message. 
			/// </remarks>
			public ushort Mode;

			/// <summary>
			/// Gets or sets the error
			/// </summary>
			/// <remarks>
			/// Error, expressed in expanded RCODE that covers TSIG processing and 
			/// TKEY processing. See Remarks. 
			/// </remarks>
			public ushort Error;

			/// <summary>
			/// Gets or sets the key length
			/// </summary>
			/// <remarks>
			/// Length, in bytes, of the pSignature member. 
			/// </remarks>
			public ushort KeyLength;

			/// <summary>
			/// Gets or sets the other length
			/// </summary>
			/// <remarks>
			/// Length, in bytes, of the pOtherData member. 
			/// </remarks>
			public ushort OtherLength;

			/// <summary>
			/// Gets or sets the name algorithm's length
			/// </summary>
			/// <remarks>
			/// Length, in bytes, of the pNameAlgorithm member. 
			/// </remarks>
			public char AlgNameLength;

			/// <summary>
			/// Gets or sets whether or not to use packet pointers
			/// </summary>
			/// <remarks>
			/// Reserved for future use. 
			/// </remarks>
			public bool PacketPointers;
		}

		/// <summary>
		/// Represents a DNS Server record. (DNS_SRV_DATA)
		/// </summary>
		/// <remarks>
		/// The SRVRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct SRVRecord
		{
			/// <summary>
			/// Gets or sets the name
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the target host. 
			/// </remarks>
			public string NameNext;

			/// <summary>
			/// Gets or sets the priority
			/// </summary>
			/// <remarks>
			/// Priority of the target host specified in the owner name. Lower numbers imply higher priority. 
			/// </remarks>
			public ushort Priority;

			/// <summary>
			/// Gets or sets the weight
			/// </summary>
			/// <remarks>
			/// Weight of the target host. Useful when selecting among hosts with the same priority. The chances of using this host should be proportional to its weight. 
			/// </remarks>
			public ushort Weight;

			/// <summary>
			/// Gets or sets the port
			/// </summary>
			/// <remarks>
			/// Port used on the terget host for the service. 
			/// </remarks>
			public ushort Port;

			/// <summary>
			/// Reserved.
			/// </summary>
			/// <remarks>
			/// Reserved. Used to keep pointers DWORD aligned. 
			/// </remarks>
			public ushort Pad;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// name next: [SERVER] priority: [PRIOR] weight: [WEIGHT] port: [PORT]
			/// where [SERVER] = string representation of <see cref="NameNext"/>
			/// and   [PRIOR] = string representation of <see cref="Priority"/>
			/// and   [WEIGHT] = string representation of <see cref="Weight"/>
			/// and   [PORT] = string representation of <see cref="Port"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"name next: {0} priority: {1} weight: {2} port: {3}",
					NameNext,
					Priority,
					Weight,
					Port
					);
			}
		}

		/// <summary>
		/// Represents a DNS Start Of Authority record (DNS_SOA_DATA)
		/// </summary>
		/// <remarks>
		/// The SOARecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct SOARecord
		{
			/// <summary>
			/// Gets or sets the primary server
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the name of the authoritative 
			/// DNS server for the zone to which the record belongs. 
			/// </remarks>
			public string PrimaryServer;

			/// <summary>
			/// Gets or sets the name of the administrator
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the name of the responsible party 
			/// for the zone to which the record belongs. 
			/// </remarks>
			public string Administrator;

			/// <summary>
			/// Gets or sets the serial number
			/// </summary>
			/// <remarks>
			/// Serial number of the SOA record. 
			/// </remarks>
			public uint SerialNo;

			/// <summary>
			/// Gets or sets the refresh
			/// </summary>
			/// <remarks>
			/// Time, in seconds, before the zone containing this record should be 
			/// refreshed. 
			/// </remarks>
			public uint Refresh;

			/// <summary>
			/// Gets or sets the retry count
			/// </summary>
			/// <remarks>
			/// Time, in seconds, before retrying a failed refresh of the zone to 
			/// which this record belongs 
			/// </remarks>
			public uint Retry;

			/// <summary>
			/// Gets or sets the expiration
			/// </summary>
			/// <remarks>
			/// Time, in seconds, before an unresponsive zone is no longer authoritative. 
			/// </remarks>
			public uint Expire;

			/// <summary>
			/// Gets or sets the default ttl
			/// </summary>
			/// <remarks>
			/// Lower limit on the time, in seconds, that a DNS server or caching 
			/// resolver are allowed to cache any RRs from the zone to which this 
			/// record belongs. 
			/// </remarks>
			public uint DefaultTtl;

			/// <summary>
			/// Returns a string representation of the Start Of Authority record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// administrator: [ADMIN] TTL: [TTL] primary server: [SERVER] refresh: [REFRESH] retry: [RETRY] serial number: [SERIAL]
			/// where [ADMIN] = string representation of <see cref="Administrator"/>
			/// and   [TTL] = string representation of <see cref="DefaultTtl"/>
			/// and   [SERVER] = string representation of <see cref="PrimaryServer"/>
			/// and   [REFRESH] = string representation of <see cref="Refresh"/>
			/// and   [RETRY] = string representation of <see cref="Retry"/>
			/// and   [SERIAL] = string representation of <see cref="SerialNo"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"administrator: {0} TTL: {1} primary server: {2} refresh: {3} retry: {4} serial number: {5}",
					Administrator,
					DefaultTtl,
					PrimaryServer,
					Refresh,
					Retry,
					SerialNo
					);
			}
		}

		/// <summary>
		/// Represents a DNS Cryptographic signature record. (DNS_SIG_DATA)
		/// </summary>
		/// <remarks>
		/// The SIGRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct SIGRecord
		{
			/// <summary>
			/// Gets or sets the signer.
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the name of the signer that 
			/// generated the record 
			/// </remarks>
			public string Signer;

			/// <summary>
			/// Gets or sets the type covered
			/// </summary>
			/// <remarks>
			/// Type of RR covered by the signature 
			/// </remarks>
			public ushort	TypeCovered;

			/// <summary>
			/// Gets or sets the algorithm
			/// </summary>
			/// <remarks>
			/// Algorithm used with the key specified in the RR. The assigned values are shown in the following table. 
			/// 
			/// <list type="table">
			///		<listheader>
			///			<term>Value</term>
			///			<term>Meaning</term>
			///		</listheader>
			///		<item>
			///			<term>1</term>
			///			<description>RSA/MD5 (RFC 2537)</description>
			///		</item>
			///		<item>
			///			<term>2</term>
			///			<description>Diffie-Hellman (RFC 2539)</description>
			///		</item>
			///		<item>
			///			<term>3</term>
			///			<description>DSA (RFC 2536)</description>
			///		</item>
			///		<item>
			///			<term>4</term>
			///			<description>Elliptic curve cryptography</description>
			///		</item>
			/// </list>
			/// </remarks>
			public byte	Algorithm;

			/// <summary>
			/// Gets or sets the label count
			/// </summary>
			/// <remarks>
			/// Number of labels in the original signature RR owner name. The count does not include the NULL label for the root, nor any initial wildcards. 
			/// </remarks>
			public byte	LabelCount;

			/// <summary>
			/// Gets or sets the original ttl
			/// </summary>
			/// <remarks>
			/// TTL value of the RR set signed by the signature RR. 
			/// </remarks>
			public uint	OriginalTtl;

			/// <summary>
			/// Gets or sets the expiration
			/// </summary>
			/// <remarks>
			/// Expiration date, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds. 
			/// </remarks>
			public uint	Expiration;

			/// <summary>
			/// Gets or sets the time signed
			/// </summary>
			/// <remarks>
			/// Date and time at which the signature becomes valid, expressed in seconds since the beginning of January 1, 1970, Greenwich Mean Time (GMT), excluding leap seconds. 
			/// </remarks>
			public uint	TimeSigned;

			/// <summary>
			/// Gets or sets the key tag
			/// </summary>
			/// <remarks>
			/// Method used to choose a key that verifies a signature. See RFC 2535, Appendix C for the method used to calculate a KeyTag.
			/// </remarks>
			public ushort	KeyTag;

			/// <summary>
			/// Reserved.
			/// </summary>
			/// <remarks>
			/// Reserved. Used to keep byte field aligned. 
			/// </remarks>
			public ushort	Pad;            // keep byte field aligned

			/// <summary>
			/// Gets or sets the signature
			/// </summary>
			/// <remarks>
			/// Signature, represented in base 64, formatted as defined in RFC 2535, Appendix A. 
			/// </remarks>
			public byte	Signature;
		}

		/// <summary>
		/// Represents the DNS pointer record (DNS_PTR_DATA)
		/// </summary>
		/// <remarks>
		/// The PTRRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct PTRRecord
		{
			/// <summary>
			/// Gets or sets the hostname of the record.
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the pointer (PTR) record data.
			/// </remarks>
			public string HostName;

			/// <summary>
			/// Returns a string representation of the pointer record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// Hostname: [HOST]
			/// where [HOST] = string representation of <see cref="HostName"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format("Hostname: {0}", HostName);
			}
		}

		/// <summary>
		/// Represents the DNS Next record. (DNS_NXT_DATA)
		/// </summary>
		/// <remarks>
		/// The NXTRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct NXTRecord
		{
			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the name of the next domain. 
			/// </remarks>
			public string NameNext;

			/// <summary>
			/// Gets or sets the type bit map
			/// </summary>
			/// <remarks>
			/// Number of elements in the wTypes array. 
			/// </remarks>
			public byte TypeBitMap;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// next: [NAME] type bitmap: [BITMAP]
			/// where [NAME] = string representation of <see cref="NameNext"/>
			/// and   [BITMAP] = hexadecimal representation of <see cref="TypeBitMap"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"next: {0} type bitmap: {1}",
					NameNext,
					TypeBitMap.ToString("x")
					);
			}
		}

		/// <summary>
		/// Represents NULL data for a DNS resource record. (DNS_NULL_DATA)
		/// </summary>
		/// <remarks>
		/// The NULLRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct NULLRecord
		{
			/// <summary>
			/// Gets or sets the byte count.
			/// </summary>
			/// <remarks>
			/// Number of bytes represented in Data. 
			/// </remarks>
			public uint ByteCount;

			/// <summary>
			/// Gets or sets the data.
			/// </summary>
			/// <remarks>
			/// Null data.
			/// </remarks>
			public string Data;
											   
			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// byte count: [BYTECT] data: [DATA]
			/// where [BYTECT] = string representation of <see cref="ByteCount"/>
			/// and   [DATA] = hexadecimal representation of <see cref="Data"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"byte count: {0} data: {1}",
					ByteCount,
					Data
					);
			}
		}

		/// <summary>
		/// Represents a DNS Mail Exchange record (DNS_MX_DATA).
		/// </summary>
		/// <remarks>
		/// The MXRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct MXRecord
		{
			/// <summary>
			/// Gets or sets the exchange's host name
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the fully qualified domain name 
			/// (FQDN) of the host willing to act as a mail exchange. 
			/// </remarks>
			public string Exchange;

			/// <summary>
			/// Gets or sets the preference of the exchange.
			/// </summary>
			/// <remarks>
			/// Preference given to this resource record among others at the same 
			/// owner. Lower values are preferred. 
			/// </remarks>
			public ushort Preference;

			/// <summary>
			/// Reserved.
			/// </summary>
			/// <remarks>
			/// Reserved. Used to keep pointers DWORD aligned. 
			/// </remarks>
			public ushort Pad; // to keep dword aligned

			/// <summary>
			/// Returns a string representation of this mail exchange.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// exchange (preference): [EXCH] ([PREF])
			/// where [EXCH] = string representation of <see cref="Exchange"/>
			/// and   [PREF] = hexadecimal representation of <see cref="Preference"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"exchange (preference): {0} ({1})", 
					Exchange, 
					Preference
					);
			}
		}

		/// <summary>
		/// Represents a DNS mail information (MINFO) record (DNS_MINFO_DATA)
		/// </summary>
		/// <remarks>
		/// The MINFORecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct MINFORecord
		{
			/// <summary>
			/// Gets or sets the mailbox name
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the fully qualified domain name 
			/// (FQDN) of the mailbox responsible for the mailing list or mailbox 
			/// specified in the record's owner name. 
			/// </remarks>
			public string Mailbox;

			/// <summary>
			/// Gets or sets the error mailbox name
			/// </summary>
			/// <remarks>
			/// Pointer to a string representing the FQDN of the mailbox to receive 
			/// error messages related to the mailing list. 
			/// </remarks>
			public string ErrorsMailbox;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// mailbox: [MAILBOX] error mailbox: [ERRMAILBOX]
			/// where [MAILBOX] = string representation of <see cref="Mailbox"/>
			/// and   [ERRMAILBOX] = hexadecimal representation of <see cref="ErrorsMailbox"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"mailbox: {0} error mailbox: {1}",
					Mailbox,
					ErrorsMailbox
					);
			}
		}

		/// <summary>
		/// Represents a DNS Location record (DNS_LOC_DATA)
		/// </summary>
		/// <remarks>
		/// <para>
		/// The LOCRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </para>
		/// <para>
		/// For <see cref="LOCRecord.Altitude"/>, altitude above or 
		/// below sea level may be used as an approximation of altitude 
		/// relative to the [WGS 84] spheroid, however, there will be 
		/// differences due to the Earth's surface not being a perfect 
		/// spheroid. For example, the geoid (which sea level approximates) 
		/// for the continental US ranges from 10 meters to 50 meters below 
		/// the [WGS 84] spheroid. Adjustments to <see cref="LOCRecord.Altitude"/> 
		/// and/or <see cref="LOCRecord.VerPrec"/> will be necessary in most cases. 
		/// The Defense Mapping Agency publishes geoid height values relative 
		/// to the [WGS 84] ellipsoid.
		/// </para>
		/// <para>
		/// For more information about the LOC RR, see RFC 1876.
		/// </para>
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct LOCRecord
		{
			/// <summary>
			/// Gets or sets the version
			/// </summary>
			/// <remarks>
			/// Version number of the representation. Must be zero. 
			/// </remarks>
			public ushort Version;

			/// <summary>
			/// Gets or sets the size
			/// </summary>
			/// <remarks>
			/// Diameter of a sphere enclosing the described entity, in centimeters, 
			/// expressed as a pair of four-bit unsigned integers, each ranging from 
			/// zero to nine, with the most significant four bits representing the base 
			/// and the second number representing the power of ten by which to multiply 
			/// the base. This format allows sizes from 0e0 (&lt;1cm) to 9e9 (90,000km) 
			/// to be expressed. 
			/// </remarks>
			public ushort Size;

			/// <summary>
			/// Gets or sets the horizontal precision
			/// </summary>
			/// <remarks>
			/// Horizontal precision of the data, in centimeters, expressed using the 
			/// same representation as wSize. This is the diameter of the horizontal 
			/// circle of error, rather than a plus or minus value. Matches the 
			/// interpretation of wSize; to get a plus or minus value, divide by 2. 
			/// </remarks>
			public ushort HorPrec;

			/// <summary>
			/// Gets or sets the vertical precision
			/// </summary>
			/// <remarks>
			/// Vertical precision of the data, in centimeters, expressed using the 
			/// same representation as wSize. This value represents the total potential 
			/// vertical error, rather than a plus or minus value. Matches the 
			/// interpretation of wSize; to get a plus or minus value, divide by 2. 
			/// If altitude above or below sea level is used as an approximation for 
			/// altitude relative to the [WGS 84] ellipsoid, the precision value should 
			/// be adjusted.
			/// </remarks>
			public ushort VerPrec;

			/// <summary>
			/// Gets or sets the latitude of the location
			/// </summary>
			/// <remarks>
			/// Latitude of the center of the sphere described by wSize, expressed as a 
			/// 32-bit integer, with the most significant octet first (network standard 
			/// byte order), in thousandths of a second of arc. 2^31 represents the 
			/// equator, larger numbers are north latitude. 
			/// </remarks>
			public uint	Latitude;

			/// <summary>
			/// Gets or sets the longitude of the location
			/// </summary>
			/// <remarks>
			/// Longitude of the center of the sphere described by wSize, expressed as a 
			/// 32-bit integer, most significant octet first (network standard byte order),
			/// in thousandths of a second of arc, rounded away from the prime meridian. 
			/// 2^31 represents the prime meridian, larger numbers are east longitude.. 
			/// </remarks>
			public uint	Longitude;

			/// <summary>
			/// Gets or sets the altitude of the location
			/// </summary>
			/// <remarks>
			/// Altitude of the center of the sphere described by wSize, expressed as a 
			/// 32-bit integer, most significant octet first (network standard byte order),
			/// in centimeters, from a base of 100,000m below the [WGS 84] reference 
			/// spheroid used by GPS (semimajor axis a=6378137.0, reciprocal flattening 
			/// rf=298.257223563). See Remarks for more information. 
			/// </remarks>
			public uint	Altitude;
		}

		/// <summary>
		/// Represents a Public key DNS record (DNS_KEY_DATA)
		/// </summary>
		/// <remarks>
		/// The KEYRecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct KEYRecord
		{
			/// <summary>
			/// Gets or sets the flags
			/// </summary>
			/// <remarks>
			/// Flags used to specify mapping, as described in IETF RFC 2535. 
			/// </remarks>
			public ushort Flags;

			/// <summary>
			/// Gets or sets the protocol
			/// </summary>
			/// <remarks>
			/// Protocol for which the key specified in the resource record can be used. The assigned values are shown in the following table. 
			/// 
			/// <list type="table">
			///		<listheader>
			///			<term>Value</term>
			///			<term>Meaning</term>
			///		</listheader>
			///		<item>
			///			<term>1</term>
			///			<description>TLS</description>
			///		</item>
			///		<item>
			///			<term>2</term>
			///			<description>E-Mail</description>
			///		</item>
			///		<item>
			///			<term>3</term>
			///			<description>DNSSEC</description>
			///		</item>
			///		<item>
			///			<term>4</term>
			///			<description>IPSec</description>
			///		</item>
			///		<item>
			///			<term>255</term>
			///			<description>All protocols</description>
			///		</item>
			/// </list>
			/// </remarks>
			public byte	Protocol;

			/// <summary>
			/// Gets or sets the algorithm
			/// </summary>
			/// <remarks>
			/// Algorithm used with the key specified in the resource record. The assigned values are shown in the following table. 
			/// 
			/// <list type="table">
			///		<listheader>
			///			<term>Value</term>
			///			<term>Meaning</term>
			///		</listheader>
			///		<item>
			///			<term>1</term>
			///			<description>RSA/MD5 (RFC 2537)</description>
			///		</item>
			///		<item>
			///			<term>2</term>
			///			<description>Diffie-Hellman (RFC 2539)</description>
			///		</item>
			///		<item>
			///			<term>3</term>
			///			<description>DSA (RFC 2536)</description>
			///		</item>
			///		<item>
			///			<term>4</term>
			///			<description>Elliptic curve cryptography</description>
			///		</item>
			/// </list>
			/// </remarks>
			public byte	Algorithm;

			/// <summary>
			/// Gets or sets the key
			/// </summary>
			/// <remarks>
			/// Public key, represented in base 64 as described in Appendix A of RFC 2535.
			/// </remarks>
			public byte	Key;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// flags: [FLAGS] protocol: [PROTO] algorithm: [ALGOR] key: [KEY]
			/// where [FLAGS] = string representation of <see cref="Flags"/>
			/// and   [PROTO] = hexadecimal representation of <see cref="Protocol"/>
			/// and   [ALGOR] = string representation of <see cref="Algorithm"/>
			/// and   [KEY] = hexadecimal representation of <see cref="Key"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"flags: {0} protocol: {1} algorithm: {2} key: {3}",
					Flags,
					Protocol.ToString("x"),
					Algorithm,
					Key.ToString("x")
					);
			}
		}

		/// <summary>
		/// Represents a DNS Address record (DNS_A_DATA)
		/// </summary>
		/// <remarks>
		/// The ARecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct ARecord
		{
			/// <summary>
			/// Gets or sets the ip address.
			/// </summary>
			/// <remarks>
			/// IPv4 address, in the form of an uint datatype. 
			/// <see cref="System.Net.IPAddress"/> could be 
			/// used to fill this property.
			/// </remarks>
			public uint Address;

			/// <summary>
			/// Returns a string representation of the A Record
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// ip address: [ADDRESS]
			/// where [ADDRESS] = <see cref="System.Net.IPAddress.ToString()"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"ip address: {0}", 
					new IPAddress(Address)
					);
			}
		}

		/// <summary>
		/// Represents a IPv6 Address record (DNS_AAAA_DATA)
		/// </summary>
		/// <remarks>
		/// The AAAARecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct AAAARecord
		{
			/// <summary>
			/// Gets or sets the ip6 address
			/// </summary>
			/// <remarks>
			/// IPv6 address, in the form of an <see cref="IP6Address"/> structure. 
			/// </remarks>
			public IP6Address Address;

			/// <summary>
			/// returns a string representation of this AAAA record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// Address: [ADDRESS]
			/// where [ADDRESS] = <see cref="netlib.Dns.IP6Address.ToString"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"Address: {0}",
					Address
					);
			}
		}

		/// <summary>
		/// Represents a DNS ATM address (ATMA) record (DNS_ATMA_DATA)
		/// </summary>
		/// <remarks>
		/// The ATMARecord structure is used in conjunction with 
		/// the <see cref="DnsRequest"/> and <see cref="DnsResponse"/> 
		/// classes to programmatically manage DNS entries.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct ATMARecord
		{
			/// <summary>
			/// Gets or sets the address type
			/// </summary>
			/// <remarks>
			/// ATM address format. Two possible values are DNS_ATMA_FORMAT_E164 or DNS_ATMA_FORMAT_AESA. 
			/// </remarks>
			public byte AddressType;

			/// <summary>
			/// Gets or sets the address
			/// </summary>
			/// <remarks>
			/// ATM address. For E164, represents a NULL-terminated string of less than DNS_ATMA_MAX_ADDR_LENGTH. For AESA, its length is exactly DNS_ATMA_AESA_ADDR_LENGTH. 
			/// </remarks>
			public string Address;

			/// <summary>
			/// Returns a string representation of this record.
			/// </summary>
			/// <returns></returns>
			/// <remarks>
			/// The string returned looks like:
			/// <code>
			/// address type: [TYPE] address: [ADDRESS]
			/// where [TYPE] = hexadecimal representation of <see cref="AddressType"/>
			/// and   [ADDRESS] = string representation of <see cref="Address"/>
			/// </code>
			/// </remarks>
			public override string ToString()
			{
				return String.Format(
					"address type: {0} address: {1}",
					AddressType.ToString("x"),
					Address
					);
			}
		}
	}
}