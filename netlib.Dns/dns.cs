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
        public DnsRecordType    RecordType;// 2 bytes

        /// <summary>
        /// Gets or sets the data length.
        /// </summary>
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U2)]
        public ushort   DataLength;// 2 bytes

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
        public uint Ttl;// 4 bytes

        /// <summary>
        /// Reserved.
        /// </summary>
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public uint Reserved;// 4 bytes
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
    ///     <item>Win32 API Exceptions that occurred when calling the DnsQuery API</item>
    ///     <item>Exceptions of other types that occurred when working with
    ///     the <see cref="DnsRequest"/> and <see cref="DnsResponse"/>
    ///     classes.</item>
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
        ///     ...
        /// }
        /// catch(DnsException dnsEx)
        /// {
        ///     int errcode = dnsEx.ErrorCode;
        ///     if (! Enum.IsDefined(typeof(DnsQueryReturnCode), errcode))
        ///     {
        ///         //defined in winerror.h
        ///         Console.WriteLine("WIN32 Error: {0}", errcode);
        ///         return;
        ///     }
        ///
        ///     DnsQueryReturnCode errretcode = (DnsQueryReturnCode) errcode;
        ///     if (errretcode == DnsQueryReturnCode.SUCCESS)
        ///     {
        ///         //inner exception contains the goodies
        ///         Console.WriteLine(dnsEx.InnerException.ToString());
        ///         return;
        ///     }
        ///
        ///     //dns error
        ///     Console.WriteLine("DNS Error: {0}", errretcode.ToString("g"));
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
        /// Canonical Name record
        /// </summary>
        CNAME      = 0x0005,      //  5

        /// <summary>
        /// Start Of Authority record
        /// </summary>
        SOA        = 0x0006,      //  6

        /// <summary>
        /// Pointer record
        /// </summary>
        PTR        = 0x000c,      //  12

        /// <summary>
        /// Mail Exchange record
        /// </summary>
        MX         = 0x000f,      //  15

        /// <summary>
        /// Text record
        /// </summary>
        TEXT       = 0x0010,      //  16

        //  RFC 2052    (Service location)
        /// <summary>
        /// Server record
        /// </summary>
        SRV        = 0x0021,      //  33

        /// <summary>
        /// All records
        /// </summary>
        ALL        = 0x00ff,      //  255

        /// <summary>
        /// Any records
        /// </summary>
        ANY        = 0x00ff,      //  255
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
    ///     <listheader>
    ///         <term>RecordType</term>
    ///         <term>RecordData</term>
    ///     </listheader>
    ///     <item>
    ///         <term>A</term>
    ///         <description><see cref="netlib.Dns.Records.ARecord"/></description>
    ///     </item>
    ///     <item>
    ///         <term>CNAME</term>
    ///         <description><see cref="netlib.Dns.Records.PTRRecord"/></description>
    ///     </item>
    ///     <item>
    ///         <term>PTR</term>
    ///         <description><see cref="netlib.Dns.Records.PTRRecord"/></description>
    ///     </item>
    ///     <item>
    ///         <term>MX</term>
    ///         <description><see cref="netlib.Dns.Records.MXRecord"/></description>
    ///     </item>
    ///     <item>
    ///         <term>SOA</term>
    ///         <description><see cref="netlib.Dns.Records.SOARecord"/></description>
    ///     </item>
    ///     <item>
    ///         <term>SRV</term>
    ///         <description><see cref="netlib.Dns.Records.SRVRecord"/></description>
    ///     </item>
    ///     <item>
    ///         <term>TEXT</term>
    ///         <description><see cref="netlib.Dns.Records.TXTRecord"/></description>
    ///     </item>
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
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero</term>
        ///         <description>This instance is less than obj. The <see cref="RecordData"/>
        ///         types do not match.</description>
        ///     </item>
        ///     <item>
        ///         <term>Zero</term>
        ///         <description>This instance is equal to obj. </description>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero</term>
        ///         <description>This instance is greater than obj. The <see cref="RecordType"/>
        ///         do not match.</description>
        ///     </item>
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
    ///         3/8/2003 v1.1 (C#) - Released on 5/31/2003
    /// </para>
    /// <para>
    /// Created by: Bill Gearhart. Based on code by Patrik Lundin.
    /// See version 1.0 remarks below. Specific attention was given
    /// to the exposed interface which got a 110% overhaul.
    /// </para>
    /// <para>
    /// Notable changes from the previous version:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             structs filled with constants were changed to enums
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             .net datatypes were changed to c# datatypes
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             every object is now in it's own *.cs file
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             custom collections and exceptions added
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             better object orientation - request and response classes
    ///             created for the dns query request/response session so that
    ///             it follows the .NET model
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             eliminated duplicate recs returned by an ALL query
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             bad api return code enumeration added
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             ToString() overridden to provide meaningful info for many
    ///             of the dns data structs
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             documentation and notes were created for all classes
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             added check to ensure code only runs on w2k or better
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             obsolete DNS record types are now marked as such
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             newer enum values added to DnsQueryType enum
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             compiled html documentation was written which always takes
    ///             20 times longer than writing the code does.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             this list of changes was compiled by your's truly...
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             smoothed out object and member names so they were more
    ///             intuitive - for instance: DNS_MX_DATA became MXRecord
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             added call to DnsRecordListFree API to free resources after
    ///             DnsQuery call
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             altered DnsQuery API call to allow for servers other than the
    ///             local DNS server from being queried
    ///         </description>
    ///     </item>
    /// </list>
    /// </para>
    /// <para>
    ///     4/15/2002 v1.0 (C#)
    /// </para>
    /// <para>
    /// Created by: Patrik Lundin
    /// </para>
    /// <para>
    /// Based on code found at:
    /// <a href="http://www.c-sharpcorner.com/Code/2002/April/DnsResolver.asp">http://www.c-sharpcorner.com/Code/2002/April/DnsResolver.asp</a>
    ///
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             Initial implementation.
    ///         </description>
    ///     </item>
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
    ///     class __loader
    ///     {
    ///         static void Main()
    ///         {
    ///             try
    ///             {
    ///                 DnsRequest request = new DnsRequest();
    ///                 request.TreatAsFQDN=true;
    ///                 request.BypassCache=true;
    ///                 request.Servers.Add("dns.compresolve.com");
    ///                 request._domain = "aspemporium.com";
    ///                 DnsResponse response = request.GetResponse();
    ///
    ///                 Console.WriteLine("Addresses");
    ///                 Console.WriteLine("--------------------------");
    ///                 foreach(ARecord addr in response.ARecords)
    ///                     Console.WriteLine("\t{0}", addr.ToString());
    ///                 Console.WriteLine();
    ///
    ///                 Console.WriteLine("Name Servers");
    ///                 Console.WriteLine("--------------------------");
    ///                 foreach(PTRRecord ns in response.NSRecords)
    ///                     Console.WriteLine("\t{0}", ns.ToString());
    ///                 Console.WriteLine();
    ///
    ///                 Console.WriteLine("Mail Exchanges");
    ///                 Console.WriteLine("--------------------------");
    ///                 foreach(MXRecord exchange in response.MXRecords)
    ///                     Console.WriteLine("\t{0}", exchange.ToString());
    ///                 Console.WriteLine();
    ///
    ///                 Console.WriteLine("Canonical Names");
    ///                 Console.WriteLine("--------------------------");
    ///                 foreach(PTRRecord cname in response.GetRecords(DnsRecordType.CNAME))
    ///                     Console.WriteLine("\t{0}", cname.ToString());
    ///                 Console.WriteLine();
    ///
    ///                 Console.WriteLine("Start of Authority Records");
    ///                 Console.WriteLine("--------------------------");
    ///                 foreach(SOARecord soa in response.GetRecords(DnsRecordType.SOA))
    ///                     Console.WriteLine("\t{0}", soa.ToString());
    ///                 Console.WriteLine();
    ///
    ///                 //foreach(DnsWrapper wrap in response.RawRecords)
    ///                 //{
    ///                 //  Console.WriteLine(wrap.RecordType);
    ///                 //}
    ///
    ///                 response = null;
    ///                 request = null;
    ///             }
    ///             catch(DnsException ex)
    ///             {
    ///                 Console.WriteLine("EXCEPTION DOING DNS QUERY:");
    ///                 Console.WriteLine("\t{0}", ((DnsQueryReturnCode) ex.ErrorCode).ToString("g"));
    ///
    ///                 if (ex.InnerException != null)
    ///                     Console.WriteLine(ex.InnerException.ToString());
    ///             }
    ///         }
    ///     }
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
        /// on the lookup.
        /// </summary>
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

            IntPtr ppQueryResultsSet = IntPtr.Zero;
            try
            {
                uint ret = DnsQuery(strDomain, dnstype, querytype, IntPtr.Zero, ref ppQueryResultsSet, IntPtr.Zero);
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
                case DnsRecordType.PTR:
                    Data = (PTRRecord)Marshal.PtrToStructure( ptr, typeof(PTRRecord) );
                    break;

                case DnsRecordType.MX:
                    Data = (MXRecord)Marshal.PtrToStructure( ptr, typeof(MXRecord) );
                    break;

                case DnsRecordType.SOA:
                    Data = (SOARecord)Marshal.PtrToStructure( ptr, typeof(SOARecord) );
                    break;

                case DnsRecordType.SRV:
                    Data = (SRVRecord)Marshal.PtrToStructure( ptr, typeof(SRVRecord) );
                    break;

                case DnsRecordType.TEXT:
                    Data = (TXTRecord)Marshal.PtrToStructure(ptr, typeof(TXTRecord));
                    break;

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
        /// is wrapped by the <see cref="GetRecords"/> method.
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
        ///     string s = record.ToString();
        /// }
        /// </code>
        /// The following table indicates the DNS record type you can expect to get
        /// back based on the <see cref="DnsRecordType"/> requested. Any items returning
        /// null are not currently supported.
        /// <list type="table">
        ///     <listheader>
        ///         <term>DnsRecordType enumeration value</term>
        ///         <term>GetRecords() returns</term>
        ///     </listheader>
        ///     <item>
        ///         <term>A</term>
        ///         <description><see cref="netlib.Dns.Records.ARecord"/></description>
        ///     </item>
        ///     <item>
        ///         <term>CNAME</term>
        ///         <description><see cref="netlib.Dns.Records.PTRRecord"/></description>
        ///     </item>
        ///     <item>
        ///         <term>PTR</term>
        ///         <description><see cref="netlib.Dns.Records.PTRRecord"/></description>
        ///     </item>
        ///     <item>
        ///         <term>MX</term>
        ///         <description><see cref="netlib.Dns.Records.MXRecord"/></description>
        ///     </item>
        ///     <item>
        ///         <term>SRV</term>
        ///         <description><see cref="netlib.Dns.Records.SRVRecord"/></description>
        ///     </item>
        ///     <item>
        ///         <term>TEXT</term>
        ///         <description><see cref="netlib.Dns.Records.TXTRecord"/></description>
        ///     </item>
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
        /// Gets all the <see cref="TXTRecord"/> for the queried domain.
        /// </summary>
        /// <remarks>
        /// Uses the <see cref="GetRecords"/> method to retrieve an
        /// array of <see cref="TXTRecord"/>s representing all the Address
        /// records for the domain.
        /// </remarks>
        /// <value>An array of <see cref="SRVRecord"/> objects.</value>
        public TXTRecord[] TXTRecords
        {
            get
            {
                ArrayList arr = GetRecords(DnsRecordType.TEXT);
                return (TXTRecord[])arr.ToArray(typeof(TXTRecord));
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
    }

    namespace Records
    {
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
    }
}
