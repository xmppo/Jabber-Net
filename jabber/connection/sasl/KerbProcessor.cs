using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Xml;
using jabber.protocol.stream;
using HANDLE = System.IntPtr;

namespace jabber.connection.sasl
{

    ///<summary>
    /// Uses Kerberos authentication ot log into XMPP server.
    ///</summary>
    public class KerbProcessor : SASLProcessor
    {
        /// <summary>
        /// Should we use the existing Windows (kerberos) credentials?
        /// </summary>
        public const string USE_WINDOWS_CREDS = "USE_WINDOWS_CREDS";

        private readonly SSPIHelper kerbClient;
        ///<summary>
        /// Creates a new KerbProcessor
        ///</summary>
        ///<param name="remotePrincipal">Remote principal that represents the XMPP server.</param>
        public KerbProcessor(string remotePrincipal)
        {
            kerbClient = new SSPIHelper(remotePrincipal);
        }

        /// <summary>
        /// Perform the next step
        /// </summary>
        /// <param name="s">Null if it's the initial response</param>
        /// <param name="doc">Document to create Steps in</param>
        /// <returns>XML to send to the XMPP server.</returns>
        public override Step step(Step s, XmlDocument doc)
        {
            byte[] outBytes;
            byte[] inBytes = null;

            Step returnStep;

            if (s == null)
            {
                // First step.
                returnStep = new Auth(doc);
                ((Auth)returnStep).Mechanism = MechanismType.GSSAPI;

                SetCredentials();
            }
            else
            {
                returnStep = new Response(doc);
                inBytes = s.Bytes;
            }

            kerbClient.ExecuteKerberos(inBytes, out outBytes);
            returnStep.Bytes = outBytes;

            return returnStep;
        }

        private void SetCredentials()
        {
            // Username should be in the form of "DOMAIN\USERNAME"
            // If it isn't, we'll assume it is the username and get
            // the domain from somewhere else.
            string username = this[USERNAME];
            int sepIndex = username.IndexOf('\\');
            kerbClient.Username = username.Substring(sepIndex + 1);

            if (sepIndex != -1)
                kerbClient.Domain = username.Substring(0, sepIndex);
            else
            {
                // Domain wasn't specified. Use the current user's domain.
                string currentName = WindowsIdentity.GetCurrent().Name;
                kerbClient.Domain = currentName.Substring(0, currentName.IndexOf('\\'));
            }
            kerbClient.Password = this[PASSWORD];
            kerbClient.UseWindowsCreds = Boolean.Parse(this[USE_WINDOWS_CREDS]);
        }
    }

    internal enum SecBufferType
    {
        SECBUFFER_VERSION = 0,
        SECBUFFER_EMPTY = 0,
        SECBUFFER_DATA = 1,
        SECBUFFER_TOKEN = 2,
        SECBUFFER_PADDING = 9,
        SECBUFFER_STREAM = 10
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SecHandle //=PCtxtHandle
    {
        uint dwLower;
        uint dwUpper;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SecBuffer : IDisposable
    {
        public int cbBuffer;
        public int BufferType;
        public IntPtr pvBuffer;


        public SecBuffer(int bufferSize)
        {
            cbBuffer = bufferSize;
            BufferType = (int)SecBufferType.SECBUFFER_TOKEN;
            pvBuffer = Marshal.AllocHGlobal(bufferSize);
        }

        public SecBuffer(byte[] secBufferBytes)
        {
            cbBuffer = secBufferBytes.Length;
            BufferType = (int)SecBufferType.SECBUFFER_TOKEN;
            pvBuffer = Marshal.AllocHGlobal(cbBuffer);
            Marshal.Copy(secBufferBytes, 0, pvBuffer, cbBuffer);
        }

        public SecBuffer(byte[] secBufferBytes, SecBufferType bufferType)
        {
            BufferType = (int)bufferType;

            if (secBufferBytes != null && secBufferBytes.Length != 0)
            {
                cbBuffer = secBufferBytes.Length;
                pvBuffer = Marshal.AllocHGlobal(cbBuffer);
                Marshal.Copy(secBufferBytes, 0, pvBuffer, cbBuffer);
            }
            else
            {
                cbBuffer = 0;
                pvBuffer = HANDLE.Zero;
            }
        }

        public void Dispose()
        {
            if (pvBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pvBuffer);
                pvBuffer = IntPtr.Zero;
            }
        }
    }

    internal struct MultipleSecBufferHelper
    {
        public byte[] Buffer;
        public SecBufferType BufferType;

        public MultipleSecBufferHelper(byte[] buffer, SecBufferType bufferType)
        {
            Buffer = buffer;
            BufferType = bufferType;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct SecBufferDesc : IDisposable
    {

        public int ulVersion;
        public int cBuffers;
        public IntPtr pBuffers; //Point to SecBuffer

        public SecBufferDesc(int bufferSize)
        {
            ulVersion = (int)SecBufferType.SECBUFFER_VERSION;
            cBuffers = 1;
            SecBuffer ThisSecBuffer = new SecBuffer(bufferSize);
            pBuffers = Marshal.AllocHGlobal(Marshal.SizeOf(ThisSecBuffer));
            Marshal.StructureToPtr(ThisSecBuffer, pBuffers, false);
        }

        public SecBufferDesc(byte[] secBufferBytes)
        {
            ulVersion = (int)SecBufferType.SECBUFFER_VERSION;
            cBuffers = 1;
            SecBuffer ThisSecBuffer = new SecBuffer(secBufferBytes);
            pBuffers = Marshal.AllocHGlobal(Marshal.SizeOf(ThisSecBuffer));
            Marshal.StructureToPtr(ThisSecBuffer, pBuffers, false);
        }

        internal SecBufferDesc(MultipleSecBufferHelper[] secBufferBytesArray)
        {
            if (secBufferBytesArray == null || secBufferBytesArray.Length == 0)
            {
                throw new ArgumentException("secBufferBytesArray cannot be null or 0 length");
            }

            ulVersion = (int)SecBufferType.SECBUFFER_VERSION;
            cBuffers = secBufferBytesArray.Length;

            //Allocate memory for SecBuffer Array....
            pBuffers = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecBuffer)) * cBuffers);

            for (int Index = 0; Index < secBufferBytesArray.Length; Index++)
            {
                //Super hack: Now allocate memory for the individual SecBuffers
                //and just copy the bit values to the SecBuffer array!!!
                SecBuffer ThisSecBuffer = new SecBuffer(secBufferBytesArray[Index].Buffer,
                    secBufferBytesArray[Index].BufferType);

                //We will write out bits in the following order:
                //int cbBuffer;
                //int BufferType;
                //pvBuffer;
                //Note: that we won't be releasing the memory allocated by ThisSecBuffer until we
                //are disposed...
                int CurrentOffset = Index * Marshal.SizeOf(typeof(SecBuffer));
                Marshal.WriteInt32(pBuffers, CurrentOffset, ThisSecBuffer.cbBuffer);

                int length = CurrentOffset + Marshal.SizeOf(ThisSecBuffer.cbBuffer);
                Marshal.WriteInt32(pBuffers, length, ThisSecBuffer.BufferType);

                length = CurrentOffset + Marshal.SizeOf(ThisSecBuffer.cbBuffer) +
                         Marshal.SizeOf(ThisSecBuffer.BufferType);
                Marshal.WriteIntPtr(pBuffers, length, ThisSecBuffer.pvBuffer);
            }
        }

        public void Dispose()
        {
            if (pBuffers != IntPtr.Zero)
            {
                if (cBuffers == 1)
                {
                    SecBuffer ThisSecBuffer =
                        (SecBuffer)Marshal.PtrToStructure(pBuffers, typeof(SecBuffer));
                    ThisSecBuffer.Dispose();
                }
                else
                {
                    // Since we aren't sending any messages using the kerberos encrypt/decrypt.
                    // The 1st buffer is going to be empty. We can skip it.
                    for (int Index = 1; Index < cBuffers; Index++)
                    {
                        //The bits were written out the following order:
                        //int cbBuffer;
                        //int BufferType;
                        //pvBuffer;
                        //What we need to do here is to grab a hold of the pvBuffer allocate by the individual
                        //SecBuffer and release it...
                        int CurrentOffset = Index * Marshal.SizeOf(typeof(SecBuffer));

                        int totalLength = CurrentOffset + Marshal.SizeOf(typeof(int)) +
                                          Marshal.SizeOf(typeof(int));
                        IntPtr SecBufferpvBuffer = Marshal.ReadIntPtr(pBuffers, totalLength);
                        Marshal.FreeHGlobal(SecBufferpvBuffer);
                    }
                }

                Marshal.FreeHGlobal(pBuffers);
                pBuffers = IntPtr.Zero;
            }
        }

        public byte[] GetSecBufferByteArray()
        {
            byte[] Buffer = null;

            if (pBuffers == IntPtr.Zero)
            {
                throw new InvalidOperationException("Object has already been disposed!!!");
            }

            if (cBuffers == 1)
            {
                SecBuffer ThisSecBuffer = (SecBuffer)Marshal.PtrToStructure(pBuffers, typeof(SecBuffer));

                if (ThisSecBuffer.cbBuffer > 0)
                {
                    Buffer = new byte[ThisSecBuffer.cbBuffer];
                    Marshal.Copy(ThisSecBuffer.pvBuffer, Buffer, 0, ThisSecBuffer.cbBuffer);
                }
            }
            else
            {
                int BytesToAllocate = 0;

                for (int Index = 0; Index < cBuffers; Index++)
                {
                    //The bits were written out the following order:
                    //int cbBuffer;
                    //int BufferType;
                    //pvBuffer;
                    //What we need to do here calculate the total number of bytes we need to copy...
                    int CurrentOffset = Index * Marshal.SizeOf(typeof(SecBuffer));
                    BytesToAllocate += Marshal.ReadInt32(pBuffers, CurrentOffset);
                }

                Buffer = new byte[BytesToAllocate];

                for (int Index = 0, BufferIndex = 0; Index < cBuffers; Index++)
                {
                    //The bits were written out the following order:
                    //int cbBuffer;
                    //int BufferType;
                    //pvBuffer;
                    //Now iterate over the individual buffers and put them together into a
                    //byte array...
                    int CurrentOffset = Index * Marshal.SizeOf(typeof(SecBuffer));
                    int BytesToCopy = Marshal.ReadInt32(pBuffers, CurrentOffset);
                    int length = CurrentOffset + Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int));
                    IntPtr SecBufferpvBuffer = Marshal.ReadIntPtr(pBuffers, length);
                    Marshal.Copy(SecBufferpvBuffer, Buffer, BufferIndex, BytesToCopy);
                    BufferIndex += BytesToCopy;
                }
            }

            return (Buffer);
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_INTEGER
    {
        public uint LowPart;
        public int HighPart;
        public SECURITY_INTEGER(int dummy)
        {
            LowPart = 0;
            HighPart = 0;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_HANDLE
    {
        public uint LowPart;
        public uint HighPart;
        public SECURITY_HANDLE(int dummy)
        {
            LowPart = HighPart = 0;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct SecPkgContext_Sizes
    {
        public uint cbMaxToken;
        public uint cbMaxSignature;
        public uint cbBlockSize;
        public uint cbSecurityTrailer;
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct SEC_WINNT_AUTH_IDENTITY
    {
        public string User;
        public int UserLength;
        public string Domain;
        public int DomainLength;
        public string Password;
        public int PasswordLength;
        public int Flags;
    }

    internal class SSPIHelper
    {
        public const int TOKEN_QUERY = 0x00008;
        public const uint SEC_E_OK = 0;
        public const uint SEC_E_INVALID_HANDLE = 0x80090301;
        public const uint SEC_E_LOGON_DENIED = 0x8009030C;
        public const uint SEC_I_CONTINUE_NEEDED = 0x90312;
        public const uint SEC_I_COMPLETE_NEEDED = 0x90313;
        public const uint SEC_I_COMPLETE_AND_CONTINUE = 0x90314;

        public const uint SECQOP_WRAP_NO_ENCRYPT = 0x80000001;

        const int SECPKG_CRED_OUTBOUND = 2;
        private const int SECURITY_NETWORK_DREP = 0x0;
        const int MAX_TOKEN_SIZE = 12288;
        //For AcquireCredentialsHandle in 3er Parameter "fCredentialUse"

        SECURITY_HANDLE _hOutboundCred = new SECURITY_HANDLE(0);
        public SECURITY_HANDLE _hClientContext = new SECURITY_HANDLE(0);

        public const int ISC_REQ_DELEGATE = 0x00000001;
        public const int ISC_REQ_MUTUAL_AUTH = 0x00000002;
        public const int ISC_REQ_REPLAY_DETECT = 0x00000004;
        public const int ISC_REQ_SEQUENCE_DETECT = 0x00000008;
        public const int ISC_REQ_CONFIDENTIALITY = 0x00000010;
        public const int ISC_REQ_USE_SESSION_KEY = 0x00000020;
        public const int ISC_REQ_PROMPT_FOR_CREDS = 0x00000040;
        public const int ISC_REQ_USE_SUPPLIED_CREDS = 0x00000080;
        public const int ISC_REQ_ALLOCATE_MEMORY = 0x00000100;
        public const int ISC_REQ_USE_DCE_STYLE = 0x00000200;
        public const int ISC_REQ_DATAGRAM = 0x00000400;
        public const int ISC_REQ_CONNECTION = 0x00000800;
        public const int ISC_REQ_CALL_LEVEL = 0x00001000;
        public const int ISC_REQ_FRAGMENT_SUPPLIED = 0x00002000;
        public const int ISC_REQ_EXTENDED_ERROR = 0x00004000;
        public const int ISC_REQ_STREAM = 0x00008000;
        public const int ISC_REQ_INTEGRITY = 0x00010000;
        public const int ISC_REQ_IDENTIFY = 0x00020000;
        public const int ISC_REQ_NULL_SESSION = 0x00040000;
        public const int ISC_REQ_MANUAL_CRED_VALIDATION = 0x00080000;
        public const int ISC_REQ_RESERVED1 = 0x00100000;
        public const int ISC_REQ_FRAGMENT_TO_FIT = 0x00200000;

        public const int SECPKG_ATTR_SIZES = 0;

        public const int STANDARD_CONTEXT_ATTRIBUTES = ISC_REQ_MUTUAL_AUTH;

        bool _bGotClientCredentials = false;

        [DllImport("secur32", CharSet = CharSet.Auto)]
        static extern uint AcquireCredentialsHandle(
            string pszPrincipal, //SEC_CHAR*
            string pszPackage, //SEC_CHAR* //"Kerberos","NTLM","Negotiative"
            int fCredentialUse,
            IntPtr PAuthenticationID,//_LUID AuthenticationID,//pvLogonID, //PLUID
            ref SEC_WINNT_AUTH_IDENTITY pAuthData,//PVOID
            int pGetKeyFn, //SEC_GET_KEY_FN
            IntPtr pvGetKeyArgument, //PVOID
            ref SECURITY_HANDLE phCredential, //SecHandle //PCtxtHandle ref
            ref SECURITY_INTEGER ptsExpiry); //PTimeStamp //TimeStamp ref

        [DllImport("secur32", CharSet = CharSet.Auto)]
        static extern uint AcquireCredentialsHandle(
            string pszPrincipal, //SEC_CHAR*
            string pszPackage, //SEC_CHAR* //"Kerberos","NTLM","Negotiative"
            int fCredentialUse,
            IntPtr PAuthenticationID,//_LUID AuthenticationID,//pvLogonID, //PLUID
            IntPtr pAuthData,//PVOID
            int pGetKeyFn, //SEC_GET_KEY_FN
            IntPtr pvGetKeyArgument, //PVOID
            ref SECURITY_HANDLE phCredential, //SecHandle //PCtxtHandle ref
            ref SECURITY_INTEGER ptsExpiry); //PTimeStamp //TimeStamp ref

        [DllImport("secur32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint InitializeSecurityContext(
            ref SECURITY_HANDLE phCredential,//PCredHandle
            IntPtr phContext, //PCtxtHandle
            string pszTargetName,
            int fContextReq,
            int Reserved1,
            int TargetDataRep,
            IntPtr pInput, //PSecBufferDesc SecBufferDesc
            int Reserved2,
            out SECURITY_HANDLE phNewContext, //PCtxtHandle
            out SecBufferDesc pOutput, //PSecBufferDesc SecBufferDesc
            out uint pfContextAttr, //managed ulong == 64 bits!!!
            out SECURITY_INTEGER ptsExpiry); //PTimeStamp

        [DllImport("secur32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint InitializeSecurityContext(
            ref SECURITY_HANDLE phCredential,//PCredHandle
            ref SECURITY_HANDLE phContext, //PCtxtHandle
            string pszTargetName,
            int fContextReq,
            int Reserved1,
            int TargetDataRep,
            ref SecBufferDesc SecBufferDesc, //PSecBufferDesc SecBufferDesc
            int Reserved2,
            out SECURITY_HANDLE phNewContext, //PCtxtHandle
            out SecBufferDesc pOutput, //PSecBufferDesc SecBufferDesc
            out uint pfContextAttr, //managed ulong == 64 bits!!!
            out SECURITY_INTEGER ptsExpiry); //PTimeStamp

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern int AcceptSecurityContext(ref SECURITY_HANDLE phCredential,
                                                IntPtr phContext,
                                                ref SecBufferDesc pInput,
                                                uint fContextReq,
                                                uint TargetDataRep,
                                                out SECURITY_HANDLE phNewContext,
                                                out SecBufferDesc pOutput,
                                                out uint pfContextAttr,    //managed ulong == 64 bits!!!
                                                out SECURITY_INTEGER ptsTimeStamp);

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern int AcceptSecurityContext(ref SECURITY_HANDLE phCredential,
                                                ref SECURITY_HANDLE phContext,
                                                ref SecBufferDesc pInput,
                                                uint fContextReq,
                                                uint TargetDataRep,
                                                out SECURITY_HANDLE phNewContext,
                                                out SecBufferDesc pOutput,
                                                out uint pfContextAttr,    //managed ulong == 64 bits!!!
                                                out SECURITY_INTEGER ptsTimeStamp);

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int ImpersonateSecurityContext(ref SECURITY_HANDLE phContext);

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int QueryContextAttributes(ref SECURITY_HANDLE phContext,
                                                        uint ulAttribute,
                                                        out SecPkgContext_Sizes pContextAttributes);

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int EncryptMessage(ref SECURITY_HANDLE phContext,
                                                uint fQOP,        //managed ulong == 64 bits!!!
                                                ref SecBufferDesc pMessage,
                                                uint MessageSeqNo);    //managed ulong == 64 bits!!!

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int DecryptMessage(ref SECURITY_HANDLE phContext,
                                                 ref SecBufferDesc pMessage,
                                                 uint MessageSeqNo,
                                                 out uint pfQOP);

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int MakeSignature(ref SECURITY_HANDLE phContext,          // Context to use
                                                uint fQOP,         // Quality of Protection
                                                ref SecBufferDesc pMessage,        // Message to sign
                                                uint MessageSeqNo);      // Message Sequence Num.

        [DllImport("secur32.Dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int VerifySignature(ref SECURITY_HANDLE phContext,          // Context to use
                                                ref SecBufferDesc pMessage,        // Message to sign
                                                uint MessageSeqNo,            // Message Sequence Num.
                                                out uint pfQOP);      // Quality of Protection


        readonly string _sAccountName = WindowsIdentity.GetCurrent().Name;

        public SSPIHelper()
        {

        }

        public SSPIHelper(string sRemotePrincipal)
        {
            _sAccountName = sRemotePrincipal;
        }

        private string sUsername;
        public string Username
        {
            set { sUsername = value; }
            get { return sUsername; }
        }
        private string sDomain;
        public string Domain
        {
            set { sDomain = value; }
            get { return sDomain; }
        }
        private string sPassword;
        public string Password
        {
            set { sPassword = value; }
            get { return sPassword; }
        }

        private bool bUseWindowsCreds = false;
        public bool UseWindowsCreds
        {
            set { bUseWindowsCreds = value; }
            get { return bUseWindowsCreds; }
        }

        public void ExecuteKerberos(byte[] inToken, out byte[] outToken)
        {
            if (InitializeKerberosStage)
            {
                InitializeClient(inToken, out outToken);
            }
            else
            {
                if (inToken == null)
                {
                    throw new Exception("Kerberos failure: Incoming bytes can't be null.");
                }

                DecryptMessage(0, inToken, out outToken);

                inToken = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                EncryptMessage(inToken, out outToken);
            }
        }

        private void InitializeClient(byte[] serverToken, out byte[] clientToken)
        {
            clientToken = null;

            SECURITY_INTEGER ClientLifeTime = new SECURITY_INTEGER(0);

            if (!_bGotClientCredentials)
            {
                uint returnValue;

                if (!UseWindowsCreds)
                {
                    SEC_WINNT_AUTH_IDENTITY ident = new SEC_WINNT_AUTH_IDENTITY();
                    ident.User = Username;
                    ident.UserLength = ident.User.Length;
                    ident.Domain = Domain;
                    ident.DomainLength = ident.Domain.Length;
                    ident.Password = Password;
                    ident.PasswordLength = ident.Password.Length;
                    ident.Flags = 0x1;

                    returnValue = AcquireCredentialsHandle(null, "Kerberos", SECPKG_CRED_OUTBOUND,
                                                               IntPtr.Zero, ref ident, 0, IntPtr.Zero,
                                                               ref _hOutboundCred, ref ClientLifeTime);
                }
                else
                {
                    returnValue = AcquireCredentialsHandle(null, "Kerberos", SECPKG_CRED_OUTBOUND,
                                                           HANDLE.Zero, HANDLE.Zero, 0, HANDLE.Zero,
                                                           ref _hOutboundCred, ref ClientLifeTime);
                }

                if (returnValue != SEC_E_OK)
                {
                    throw new Exception("Couldn't acquire client credentials");
                }

                _bGotClientCredentials = true;
            }

            uint ss;

            SecBufferDesc ClientToken = new SecBufferDesc(MAX_TOKEN_SIZE);

            try
            {
                uint ContextAttributes;

                if (serverToken == null)
                {
                    ss = InitializeSecurityContext(ref _hOutboundCred,
                        IntPtr.Zero,
                        _sAccountName,// null string pszTargetName,
                        STANDARD_CONTEXT_ATTRIBUTES,
                        0,//int Reserved1,
                        SECURITY_NETWORK_DREP, //int TargetDataRep
                        IntPtr.Zero,    //Always zero first time around...
                        0, //int Reserved2,
                        out _hClientContext, //pHandle CtxtHandle = SecHandle
                        out ClientToken,//ref SecBufferDesc pOutput, //PSecBufferDesc
                        out ContextAttributes,//ref int pfContextAttr,
                        out ClientLifeTime); //ref IntPtr ptsExpiry ); //PTimeStamp

                }
                else
                {
                    SecBufferDesc ServerToken = new SecBufferDesc(serverToken);

                    try
                    {
                        ss = InitializeSecurityContext(ref _hOutboundCred,
                            ref _hClientContext,
                            _sAccountName,// null string pszTargetName,
                            STANDARD_CONTEXT_ATTRIBUTES,
                            0,//int Reserved1,
                            SECURITY_NETWORK_DREP,//int TargetDataRep
                            ref ServerToken,    //Always zero first time around...
                            0, //int Reserved2,
                            out _hClientContext, //pHandle CtxtHandle = SecHandle
                            out ClientToken,//ref SecBufferDesc pOutput, //PSecBufferDesc
                            out ContextAttributes,//ref int pfContextAttr,
                            out ClientLifeTime); //ref IntPtr ptsExpiry ); //PTimeStamp
                    }
                    finally
                    {
                        ServerToken.Dispose();
                    }
                }

                if (ss == SEC_E_LOGON_DENIED)
                {
                    throw new Exception("Bad username, password or domain.");
                }
                else if (ss != SEC_E_OK && ss != SEC_I_CONTINUE_NEEDED)
                {
                    throw new Exception("InitializeSecurityContext() failed!!!");
                }

                clientToken = ClientToken.GetSecBufferByteArray();
            }
            finally
            {
                ClientToken.Dispose();
            }

            InitializeKerberosStage = ss != SEC_E_OK;
        }

        private bool bInitializeKerberosStage = true;
        private bool InitializeKerberosStage
        {
            get { return bInitializeKerberosStage; }
            set { bInitializeKerberosStage = value; }
        }

        public void EncryptMessage(byte[] message, out byte[] encryptedBuffer)
        {
            encryptedBuffer = null;

            SECURITY_HANDLE EncryptionContext = _hClientContext;

            SecPkgContext_Sizes ContextSizes;

            if (QueryContextAttributes(ref EncryptionContext,
                   SECPKG_ATTR_SIZES, out ContextSizes) != SEC_E_OK)
            {
                throw new Exception("QueryContextAttribute() failed!!!");
            }

            MultipleSecBufferHelper[] ThisSecHelper = new MultipleSecBufferHelper[]
                    {
                        new MultipleSecBufferHelper(new byte[ContextSizes.cbSecurityTrailer],
                                                    SecBufferType.SECBUFFER_TOKEN),
                        new MultipleSecBufferHelper(message, SecBufferType.SECBUFFER_DATA),
                        new MultipleSecBufferHelper(new byte[ContextSizes.cbBlockSize],
                                                    SecBufferType.SECBUFFER_PADDING)
                    };

            SecBufferDesc DescBuffer = new SecBufferDesc(ThisSecHelper);

            try
            {
                if (EncryptMessage(ref EncryptionContext,
                        SECQOP_WRAP_NO_ENCRYPT, ref DescBuffer, 0) != SEC_E_OK)
                {
                    throw new Exception("EncryptMessage() failed!!!");
                }

                encryptedBuffer = DescBuffer.GetSecBufferByteArray();
            }
            finally
            {
                DescBuffer.Dispose();
            }
        }

        public void DecryptMessage(int messageLength, byte[] encryptedBuffer, out byte[] decryptedBuffer)
        {
            decryptedBuffer = null;

            SECURITY_HANDLE DecryptionContext = _hClientContext;

            byte[] EncryptedMessage = new byte[messageLength];
            Array.Copy(encryptedBuffer, 0, EncryptedMessage, 0, messageLength);

            int SecurityTrailerLength = encryptedBuffer.Length - messageLength;

            byte[] SecurityTrailer = new byte[SecurityTrailerLength];
            Array.Copy(encryptedBuffer, messageLength, SecurityTrailer, 0, SecurityTrailerLength);

            MultipleSecBufferHelper[] ThisSecHelper = new MultipleSecBufferHelper[]
                    {
                        new MultipleSecBufferHelper(EncryptedMessage, SecBufferType.SECBUFFER_DATA),
                        new MultipleSecBufferHelper(SecurityTrailer, SecBufferType.SECBUFFER_STREAM)
                    };

            SecBufferDesc DescBuffer = new SecBufferDesc(ThisSecHelper);
            try
            {
                uint EncryptionQuality;

                if (DecryptMessage(ref DecryptionContext, ref DescBuffer, 0, out EncryptionQuality) != SEC_E_OK)
                {
                    throw new Exception("DecryptMessage() failed!!!");
                }

                decryptedBuffer = new byte[messageLength];
                Array.Copy(DescBuffer.GetSecBufferByteArray(), 0, decryptedBuffer, 0, messageLength);
            }
            finally
            {
                DescBuffer.Dispose();
            }
        }
    }
}


