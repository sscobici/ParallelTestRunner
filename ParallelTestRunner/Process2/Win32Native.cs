#pragma warning disable 0169
#pragma warning disable 0649
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace ParallelTestRunner.Process2
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    internal static class Win32Native
    {
        internal struct SystemTime
        {
            [MarshalAs(UnmanagedType.U2)]
            public short Year;
            [MarshalAs(UnmanagedType.U2)]
            public short Month;
            [MarshalAs(UnmanagedType.U2)]
            public short DayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public short Day;
            [MarshalAs(UnmanagedType.U2)]
            public short Hour;
            [MarshalAs(UnmanagedType.U2)]
            public short Minute;
            [MarshalAs(UnmanagedType.U2)]
            public short Second;
            [MarshalAs(UnmanagedType.U2)]
            public short Milliseconds;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TimeZoneInformation
        {
            [MarshalAs(UnmanagedType.I4)]
            public int Bias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string StandardName;
            public Win32Native.SystemTime StandardDate;
            [MarshalAs(UnmanagedType.I4)]
            public int StandardBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DaylightName;
            public Win32Native.SystemTime DaylightDate;
            [MarshalAs(UnmanagedType.I4)]
            public int DaylightBias;
            public TimeZoneInformation(Win32Native.DynamicTimeZoneInformation dtzi)
            {
                Bias = dtzi.Bias;
                StandardName = dtzi.StandardName;
                StandardDate = dtzi.StandardDate;
                StandardBias = dtzi.StandardBias;
                DaylightName = dtzi.DaylightName;
                DaylightDate = dtzi.DaylightDate;
                DaylightBias = dtzi.DaylightBias;
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DynamicTimeZoneInformation
        {
            [MarshalAs(UnmanagedType.I4)]
            public int Bias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string StandardName;
            public Win32Native.SystemTime StandardDate;
            [MarshalAs(UnmanagedType.I4)]
            public int StandardBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DaylightName;
            public Win32Native.SystemTime DaylightDate;
            [MarshalAs(UnmanagedType.I4)]
            public int DaylightBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string TimeZoneKeyName;
            [MarshalAs(UnmanagedType.Bool)]
            public bool DynamicDaylightTimeDisabled;
        }
        internal struct RegistryTimeZoneInformation
        {
            [MarshalAs(UnmanagedType.I4)]
            public int Bias;
            [MarshalAs(UnmanagedType.I4)]
            public int StandardBias;
            [MarshalAs(UnmanagedType.I4)]
            public int DaylightBias;
            public Win32Native.SystemTime StandardDate;
            public Win32Native.SystemTime DaylightDate;
            public RegistryTimeZoneInformation(Win32Native.TimeZoneInformation tzi)
            {
                Bias = tzi.Bias;
                StandardDate = tzi.StandardDate;
                StandardBias = tzi.StandardBias;
                DaylightDate = tzi.DaylightDate;
                DaylightBias = tzi.DaylightBias;
            }
            public RegistryTimeZoneInformation(byte[] bytes)
            {
                if (bytes == null || bytes.Length != 44)
                {
                    throw new ArgumentException("Argument_InvalidREG_TZI_FORMAT", "bytes");
                }
                Bias = BitConverter.ToInt32(bytes, 0);
                StandardBias = BitConverter.ToInt32(bytes, 4);
                DaylightBias = BitConverter.ToInt32(bytes, 8);
                StandardDate.Year = BitConverter.ToInt16(bytes, 12);
                StandardDate.Month = BitConverter.ToInt16(bytes, 14);
                StandardDate.DayOfWeek = BitConverter.ToInt16(bytes, 16);
                StandardDate.Day = BitConverter.ToInt16(bytes, 18);
                StandardDate.Hour = BitConverter.ToInt16(bytes, 20);
                StandardDate.Minute = BitConverter.ToInt16(bytes, 22);
                StandardDate.Second = BitConverter.ToInt16(bytes, 24);
                StandardDate.Milliseconds = BitConverter.ToInt16(bytes, 26);
                DaylightDate.Year = BitConverter.ToInt16(bytes, 28);
                DaylightDate.Month = BitConverter.ToInt16(bytes, 30);
                DaylightDate.DayOfWeek = BitConverter.ToInt16(bytes, 32);
                DaylightDate.Day = BitConverter.ToInt16(bytes, 34);
                DaylightDate.Hour = BitConverter.ToInt16(bytes, 36);
                DaylightDate.Minute = BitConverter.ToInt16(bytes, 38);
                DaylightDate.Second = BitConverter.ToInt16(bytes, 40);
                DaylightDate.Milliseconds = BitConverter.ToInt16(bytes, 42);
            }
        }

        internal struct SYSTEM_INFO
        {
            internal int dwOemId;
            internal int dwPageSize;
            internal IntPtr lpMinimumApplicationAddress;
            internal IntPtr lpMaximumApplicationAddress;
            internal IntPtr dwActiveProcessorMask;
            internal int dwNumberOfProcessors;
            internal int dwProcessorType;
            internal int dwAllocationGranularity;
            internal short wProcessorLevel;
            internal short wProcessorRevision;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class SECURITY_ATTRIBUTES
        {
            internal int nLength;
            internal unsafe byte* pSecurityDescriptor = null;
            internal int bInheritHandle;
        }
        [Serializable]
        internal struct WIN32_FILE_ATTRIBUTE_DATA
        {
            internal int fileAttributes;
            internal uint ftCreationTimeLow;
            internal uint ftCreationTimeHigh;
            internal uint ftLastAccessTimeLow;
            internal uint ftLastAccessTimeHigh;
            internal uint ftLastWriteTimeLow;
            internal uint ftLastWriteTimeHigh;
            internal int fileSizeHigh;
            internal int fileSizeLow;
            [SecurityCritical]
            internal void PopulateFrom(Win32Native.WIN32_FIND_DATA findData)
            {
                fileAttributes = findData.dwFileAttributes;
                ftCreationTimeLow = findData.ftCreationTime_dwLowDateTime;
                ftCreationTimeHigh = findData.ftCreationTime_dwHighDateTime;
                ftLastAccessTimeLow = findData.ftLastAccessTime_dwLowDateTime;
                ftLastAccessTimeHigh = findData.ftLastAccessTime_dwHighDateTime;
                ftLastWriteTimeLow = findData.ftLastWriteTime_dwLowDateTime;
                ftLastWriteTimeHigh = findData.ftLastWriteTime_dwHighDateTime;
                fileSizeHigh = findData.nFileSizeHigh;
                fileSizeLow = findData.nFileSizeLow;
            }
        }
        internal struct FILE_TIME
        {
            internal uint ftTimeLow;
            internal uint ftTimeHigh;
            public FILE_TIME(long fileTime)
            {
                ftTimeLow = (uint)fileTime;
                ftTimeHigh = (uint)(fileTime >> 32);
            }
            public long ToTicks()
            {
                return (long)(((ulong)ftTimeHigh << 32) + (ulong)ftTimeLow);
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct KERB_S4U_LOGON
        {
            internal uint MessageType;
            internal uint Flags;
            internal Win32Native.UNICODE_INTPTR_STRING ClientUpn;
            internal Win32Native.UNICODE_INTPTR_STRING ClientRealm;
        }
        internal struct LSA_OBJECT_ATTRIBUTES
        {
            internal int Length;
            internal IntPtr RootDirectory;
            internal IntPtr ObjectName;
            internal int Attributes;
            internal IntPtr SecurityDescriptor;
            internal IntPtr SecurityQualityOfService;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct UNICODE_STRING
        {
            internal ushort Length;
            internal ushort MaximumLength;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string Buffer;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct UNICODE_INTPTR_STRING
        {
            internal ushort Length;
            internal ushort MaxLength;
            internal IntPtr Buffer;
            [SecurityCritical]
            internal UNICODE_INTPTR_STRING(int stringBytes, SafeLocalAllocHandle buffer)
            {
                Length = (ushort)stringBytes;
                MaxLength = (ushort)buffer.ByteLength;
                Buffer = buffer.DangerousGetHandle();
            }
            internal UNICODE_INTPTR_STRING(int stringBytes, IntPtr buffer)
            {
                Length = (ushort)stringBytes;
                MaxLength = (ushort)stringBytes;
                Buffer = buffer;
            }
        }
        internal struct LSA_TRANSLATED_NAME
        {
            internal int Use;
            internal Win32Native.UNICODE_INTPTR_STRING Name;
            internal int DomainIndex;
        }
        internal struct LSA_TRANSLATED_SID
        {
            internal int Use;
            internal uint Rid;
            internal int DomainIndex;
        }
        internal struct LSA_TRANSLATED_SID2
        {
            internal int Use;
            internal IntPtr Sid;
            internal int DomainIndex;
            private uint Flags;
        }
        internal struct LSA_TRUST_INFORMATION
        {
            internal Win32Native.UNICODE_INTPTR_STRING Name;
            internal IntPtr Sid;
        }
        internal struct LSA_REFERENCED_DOMAIN_LIST
        {
            internal int Entries;
            internal IntPtr Domains;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LUID
        {
            internal uint LowPart;
            internal uint HighPart;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LUID_AND_ATTRIBUTES
        {
            internal Win32Native.LUID Luid;
            internal uint Attributes;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct QUOTA_LIMITS
        {
            internal IntPtr PagedPoolLimit;
            internal IntPtr NonPagedPoolLimit;
            internal IntPtr MinimumWorkingSetSize;
            internal IntPtr MaximumWorkingSetSize;
            internal IntPtr PagefileLimit;
            internal IntPtr TimeLimit;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SECURITY_LOGON_SESSION_DATA
        {
            internal uint Size;
            internal Win32Native.LUID LogonId;
            internal Win32Native.UNICODE_INTPTR_STRING UserName;
            internal Win32Native.UNICODE_INTPTR_STRING LogonDomain;
            internal Win32Native.UNICODE_INTPTR_STRING AuthenticationPackage;
            internal uint LogonType;
            internal uint Session;
            internal IntPtr Sid;
            internal long LogonTime;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SID_AND_ATTRIBUTES
        {
            internal IntPtr Sid;
            internal uint Attributes;
            internal static readonly long SizeOf = (long)Marshal.SizeOf(typeof(Win32Native.SID_AND_ATTRIBUTES));
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_GROUPS
        {
            internal uint GroupCount;
            internal Win32Native.SID_AND_ATTRIBUTES Groups;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_PRIMARY_GROUP
        {
            internal IntPtr PrimaryGroup;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_PRIVILEGE
        {
            internal uint PrivilegeCount;
            internal Win32Native.LUID_AND_ATTRIBUTES Privilege;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_SOURCE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            internal char[] Name;
            internal Win32Native.LUID SourceIdentifier;
            private const int TOKEN_SOURCE_LENGTH = 8;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_STATISTICS
        {
            internal Win32Native.LUID TokenId;
            internal Win32Native.LUID AuthenticationId;
            internal long ExpirationTime;
            internal uint TokenType;
            internal uint ImpersonationLevel;
            internal uint DynamicCharged;
            internal uint DynamicAvailable;
            internal uint GroupCount;
            internal uint PrivilegeCount;
            internal Win32Native.LUID ModifiedId;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_USER
        {
            internal Win32Native.SID_AND_ATTRIBUTES User;
        }

        internal struct MEMORY_BASIC_INFORMATION
        {
            internal unsafe void* BaseAddress;
            internal unsafe void* AllocationBase;
            internal uint AllocationProtect;
            internal UIntPtr RegionSize;
            internal uint State;
            internal uint Protect;
            internal uint Type;
        }
        internal struct NlsVersionInfoEx
        {
            internal int dwNLSVersionInfoSize;
            internal int dwNLSVersion;
            internal int dwDefinedVersion;
            internal int dwEffectiveId;
            internal Guid guidCustomVersion;
        }
        [BestFitMapping(false)]
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class WIN32_FIND_DATA
        {
            internal int dwFileAttributes;
            internal uint ftCreationTime_dwLowDateTime;
            internal uint ftCreationTime_dwHighDateTime;
            internal uint ftLastAccessTime_dwLowDateTime;
            internal uint ftLastAccessTime_dwHighDateTime;
            internal uint ftLastWriteTime_dwLowDateTime;
            internal uint ftLastWriteTime_dwHighDateTime;
            internal int nFileSizeHigh;
            internal int nFileSizeLow;
            internal int dwReserved0;
            internal int dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            internal string cAlternateFileName;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public WIN32_FIND_DATA()
            {
            }
        }
        internal delegate bool ConsoleCtrlHandlerRoutine(int controlType);
        internal struct COORD
        {
            internal short X;
            internal short Y;
        }
        internal struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }
        internal struct CONSOLE_SCREEN_BUFFER_INFO
        {
            internal Win32Native.COORD dwSize;
            internal Win32Native.COORD dwCursorPosition;
            internal short wAttributes;
            internal Win32Native.SMALL_RECT srWindow;
            internal Win32Native.COORD dwMaximumWindowSize;
        }
        internal struct CONSOLE_CURSOR_INFO
        {
            internal int dwSize;
            internal bool bVisible;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct KeyEventRecord
        {
            internal bool keyDown;
            internal short repeatCount;
            internal short virtualKeyCode;
            internal short virtualScanCode;
            internal char uChar;
            internal int controlKeyState;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct InputRecord
        {
            internal short eventType;
            internal Win32Native.KeyEventRecord keyEvent;
        }
        [Flags]
        [Serializable]
        internal enum Color : short
        {
            Black = 0,
            ForegroundBlue = 1,
            ForegroundGreen = 2,
            ForegroundRed = 4,
            ForegroundYellow = 6,
            ForegroundIntensity = 8,
            BackgroundBlue = 16,
            BackgroundGreen = 32,
            BackgroundRed = 64,
            BackgroundYellow = 96,
            BackgroundIntensity = 128,
            ForegroundMask = 15,
            BackgroundMask = 240,
            ColorMask = 255
        }
        internal struct CHAR_INFO
        {
            private ushort charData;
            private short attributes;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class USEROBJECTFLAGS
        {
            internal int fInherit;
            internal int fReserved;
            internal int dwFlags;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public USEROBJECTFLAGS()
            {
            }
        }
        internal enum SECURITY_IMPERSONATION_LEVEL
        {
            Anonymous,
            Identification,
            Impersonation,
            Delegation
        }
        [StructLayout(LayoutKind.Explicit)]
        internal struct CLAIM_SECURITY_ATTRIBUTE_INFORMATION_V1
        {
            [FieldOffset(0)]
            public IntPtr pAttributeV1;
        }
        internal struct CLAIM_SECURITY_ATTRIBUTES_INFORMATION
        {
            public ushort Version;
            public ushort Reserved;
            public uint AttributeCount;
            public Win32Native.CLAIM_SECURITY_ATTRIBUTE_INFORMATION_V1 Attribute;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CLAIM_SECURITY_ATTRIBUTE_FQBN_VALUE
        {
            public ulong Version;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Name;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CLAIM_SECURITY_ATTRIBUTE_OCTET_STRING_VALUE
        {
            public IntPtr pValue;
            public uint ValueLength;
        }
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        internal struct CLAIM_VALUES_ATTRIBUTE_V1
        {
            [FieldOffset(0)]
            public IntPtr pInt64;
            [FieldOffset(0)]
            public IntPtr pUint64;
            [FieldOffset(0)]
            public IntPtr ppString;
            [FieldOffset(0)]
            public IntPtr pFqbn;
            [FieldOffset(0)]
            public IntPtr pOctetString;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CLAIM_SECURITY_ATTRIBUTE_V1
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Name;
            public ushort ValueType;
            public ushort Reserved;
            public uint Flags;
            public uint ValueCount;
            public Win32Native.CLAIM_VALUES_ATTRIBUTE_V1 Values;
        }
        internal const int KEY_QUERY_VALUE = 1;
        internal const int KEY_SET_VALUE = 2;
        internal const int KEY_CREATE_SUB_KEY = 4;
        internal const int KEY_ENUMERATE_SUB_KEYS = 8;
        internal const int KEY_NOTIFY = 16;
        internal const int KEY_CREATE_LINK = 32;
        internal const int KEY_READ = 131097;
        internal const int KEY_WRITE = 131078;
        internal const int KEY_WOW64_64KEY = 256;
        internal const int KEY_WOW64_32KEY = 512;
        internal const int REG_OPTION_NON_VOLATILE = 0;
        internal const int REG_OPTION_VOLATILE = 1;
        internal const int REG_OPTION_CREATE_LINK = 2;
        internal const int REG_OPTION_BACKUP_RESTORE = 4;
        internal const int REG_NONE = 0;
        internal const int REG_SZ = 1;
        internal const int REG_EXPAND_SZ = 2;
        internal const int REG_BINARY = 3;
        internal const int REG_DWORD = 4;
        internal const int REG_DWORD_LITTLE_ENDIAN = 4;
        internal const int REG_DWORD_BIG_ENDIAN = 5;
        internal const int REG_LINK = 6;
        internal const int REG_MULTI_SZ = 7;
        internal const int REG_RESOURCE_LIST = 8;
        internal const int REG_FULL_RESOURCE_DESCRIPTOR = 9;
        internal const int REG_RESOURCE_REQUIREMENTS_LIST = 10;
        internal const int REG_QWORD = 11;
        internal const int HWND_BROADCAST = 65535;
        internal const int WM_SETTINGCHANGE = 26;
        internal const uint CRYPTPROTECTMEMORY_BLOCK_SIZE = 16u;
        internal const uint CRYPTPROTECTMEMORY_SAME_PROCESS = 0u;
        internal const uint CRYPTPROTECTMEMORY_CROSS_PROCESS = 1u;
        internal const uint CRYPTPROTECTMEMORY_SAME_LOGON = 2u;
        internal const int SECURITY_ANONYMOUS = 0;
        internal const int SECURITY_SQOS_PRESENT = 1048576;
        internal const string MICROSOFT_KERBEROS_NAME = "Kerberos";
        internal const uint ANONYMOUS_LOGON_LUID = 998u;
        internal const int SECURITY_ANONYMOUS_LOGON_RID = 7;
        internal const int SECURITY_AUTHENTICATED_USER_RID = 11;
        internal const int SECURITY_LOCAL_SYSTEM_RID = 18;
        internal const int SECURITY_BUILTIN_DOMAIN_RID = 32;
        internal const uint SE_PRIVILEGE_DISABLED = 0u;
        internal const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1u;
        internal const uint SE_PRIVILEGE_ENABLED = 2u;
        internal const uint SE_PRIVILEGE_USED_FOR_ACCESS = 2147483648u;
        internal const uint SE_GROUP_MANDATORY = 1u;
        internal const uint SE_GROUP_ENABLED_BY_DEFAULT = 2u;
        internal const uint SE_GROUP_ENABLED = 4u;
        internal const uint SE_GROUP_OWNER = 8u;
        internal const uint SE_GROUP_USE_FOR_DENY_ONLY = 16u;
        internal const uint SE_GROUP_LOGON_ID = 3221225472u;
        internal const uint SE_GROUP_RESOURCE = 536870912u;
        internal const uint DUPLICATE_CLOSE_SOURCE = 1u;
        internal const uint DUPLICATE_SAME_ACCESS = 2u;
        internal const uint DUPLICATE_SAME_ATTRIBUTES = 4u;
        internal const int TIME_ZONE_ID_INVALID = -1;
        internal const int TIME_ZONE_ID_UNKNOWN = 0;
        internal const int TIME_ZONE_ID_STANDARD = 1;
        internal const int TIME_ZONE_ID_DAYLIGHT = 2;
        internal const int MAX_PATH = 260;
        internal const int MUI_LANGUAGE_ID = 4;
        internal const int MUI_LANGUAGE_NAME = 8;
        internal const int MUI_PREFERRED_UI_LANGUAGES = 16;
        internal const int MUI_INSTALLED_LANGUAGES = 32;
        internal const int MUI_ALL_LANGUAGES = 64;
        internal const int MUI_LANG_NEUTRAL_PE_FILE = 256;
        internal const int MUI_NON_LANG_NEUTRAL_FILE = 512;
        internal const int LOAD_LIBRARY_AS_DATAFILE = 2;
        internal const int LOAD_STRING_MAX_LENGTH = 500;
        internal const int READ_CONTROL = 131072;
        internal const int SYNCHRONIZE = 1048576;
        internal const int STANDARD_RIGHTS_READ = 131072;
        internal const int STANDARD_RIGHTS_WRITE = 131072;
        internal const int SEMAPHORE_MODIFY_STATE = 2;
        internal const int EVENT_MODIFY_STATE = 2;
        internal const int MUTEX_MODIFY_STATE = 1;
        internal const int MUTEX_ALL_ACCESS = 2031617;
        internal const int LMEM_FIXED = 0;
        internal const int LMEM_ZEROINIT = 64;
        internal const int LPTR = 64;
        internal const string KERNEL32 = "kernel32.dll";
        internal const string USER32 = "user32.dll";
        internal const string ADVAPI32 = "advapi32.dll";
        internal const string OLE32 = "ole32.dll";
        internal const string OLEAUT32 = "oleaut32.dll";
        internal const string SHELL32 = "shell32.dll";
        internal const string SHIM = "mscoree.dll";
        internal const string CRYPT32 = "crypt32.dll";
        internal const string SECUR32 = "secur32.dll";
        internal const string NTDLL = "ntdll.dll";
        internal const string MSCORWKS = "clr.dll";
        internal const int SEM_FAILCRITICALERRORS = 1;
        internal const int FIND_STARTSWITH = 1048576;
        internal const int FIND_ENDSWITH = 2097152;
        internal const int FIND_FROMSTART = 4194304;
        internal const int FIND_FROMEND = 8388608;
        internal const int STD_INPUT_HANDLE = -10;
        internal const int STD_OUTPUT_HANDLE = -11;
        internal const int STD_ERROR_HANDLE = -12;
        internal const int CTRL_C_EVENT = 0;
        internal const int CTRL_BREAK_EVENT = 1;
        internal const int CTRL_CLOSE_EVENT = 2;
        internal const int CTRL_LOGOFF_EVENT = 5;
        internal const int CTRL_SHUTDOWN_EVENT = 6;
        internal const short KEY_EVENT = 1;
        internal const int FILE_TYPE_DISK = 1;
        internal const int FILE_TYPE_CHAR = 2;
        internal const int FILE_TYPE_PIPE = 3;
        internal const int REPLACEFILE_WRITE_THROUGH = 1;
        internal const int REPLACEFILE_IGNORE_MERGE_ERRORS = 2;
        internal const uint FILE_MAP_WRITE = 2u;
        internal const uint FILE_MAP_READ = 4u;
        internal const int FILE_ATTRIBUTE_READONLY = 1;
        internal const int FILE_ATTRIBUTE_DIRECTORY = 16;
        internal const int FILE_ATTRIBUTE_REPARSE_POINT = 1024;
        internal const int IO_REPARSE_TAG_MOUNT_POINT = -1610612733;
        internal const int PAGE_READWRITE = 4;
        internal const int MEM_COMMIT = 4096;
        internal const int MEM_RESERVE = 8192;
        internal const int MEM_RELEASE = 32768;
        internal const int MEM_FREE = 65536;
        internal const int ERROR_SUCCESS = 0;
        internal const int ERROR_INVALID_FUNCTION = 1;
        internal const int ERROR_FILE_NOT_FOUND = 2;
        internal const int ERROR_PATH_NOT_FOUND = 3;
        internal const int ERROR_ACCESS_DENIED = 5;
        internal const int ERROR_INVALID_HANDLE = 6;
        internal const int ERROR_NOT_ENOUGH_MEMORY = 8;
        internal const int ERROR_INVALID_DATA = 13;
        internal const int ERROR_INVALID_DRIVE = 15;
        internal const int ERROR_NO_MORE_FILES = 18;
        internal const int ERROR_NOT_READY = 21;
        internal const int ERROR_BAD_LENGTH = 24;
        internal const int ERROR_SHARING_VIOLATION = 32;
        internal const int ERROR_NOT_SUPPORTED = 50;
        internal const int ERROR_FILE_EXISTS = 80;
        internal const int ERROR_INVALID_PARAMETER = 87;
        internal const int ERROR_BROKEN_PIPE = 109;
        internal const int ERROR_CALL_NOT_IMPLEMENTED = 120;
        internal const int ERROR_INSUFFICIENT_BUFFER = 122;
        internal const int ERROR_INVALID_NAME = 123;
        internal const int ERROR_BAD_PATHNAME = 161;
        internal const int ERROR_ALREADY_EXISTS = 183;
        internal const int ERROR_ENVVAR_NOT_FOUND = 203;
        internal const int ERROR_FILENAME_EXCED_RANGE = 206;
        internal const int ERROR_NO_DATA = 232;
        internal const int ERROR_PIPE_NOT_CONNECTED = 233;
        internal const int ERROR_MORE_DATA = 234;
        internal const int ERROR_DIRECTORY = 267;
        internal const int ERROR_OPERATION_ABORTED = 995;
        internal const int ERROR_NOT_FOUND = 1168;
        internal const int ERROR_NO_TOKEN = 1008;
        internal const int ERROR_DLL_INIT_FAILED = 1114;
        internal const int ERROR_NON_ACCOUNT_SID = 1257;
        internal const int ERROR_NOT_ALL_ASSIGNED = 1300;
        internal const int ERROR_UNKNOWN_REVISION = 1305;
        internal const int ERROR_INVALID_OWNER = 1307;
        internal const int ERROR_INVALID_PRIMARY_GROUP = 1308;
        internal const int ERROR_NO_SUCH_PRIVILEGE = 1313;
        internal const int ERROR_PRIVILEGE_NOT_HELD = 1314;
        internal const int ERROR_NONE_MAPPED = 1332;
        internal const int ERROR_INVALID_ACL = 1336;
        internal const int ERROR_INVALID_SID = 1337;
        internal const int ERROR_INVALID_SECURITY_DESCR = 1338;
        internal const int ERROR_BAD_IMPERSONATION_LEVEL = 1346;
        internal const int ERROR_CANT_OPEN_ANONYMOUS = 1347;
        internal const int ERROR_NO_SECURITY_ON_OBJECT = 1350;
        internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 1789;
        internal const uint STATUS_SUCCESS = 0u;
        internal const uint STATUS_SOME_NOT_MAPPED = 263u;
        internal const uint STATUS_NO_MEMORY = 3221225495u;
        internal const uint STATUS_OBJECT_NAME_NOT_FOUND = 3221225524u;
        internal const uint STATUS_NONE_MAPPED = 3221225587u;
        internal const uint STATUS_INSUFFICIENT_RESOURCES = 3221225626u;
        internal const uint STATUS_ACCESS_DENIED = 3221225506u;
        internal const int INVALID_FILE_SIZE = -1;
        internal const int STATUS_ACCOUNT_RESTRICTION = -1073741714;
        internal const int LCID_SUPPORTED = 2;
        internal const int ENABLE_PROCESSED_INPUT = 1;
        internal const int ENABLE_LINE_INPUT = 2;
        internal const int ENABLE_ECHO_INPUT = 4;
        internal const int VER_PLATFORM_WIN32s = 0;
        internal const int VER_PLATFORM_WIN32_WINDOWS = 1;
        internal const int VER_PLATFORM_WIN32_NT = 2;
        internal const int VER_PLATFORM_WINCE = 3;
        internal const int VER_PLATFORM_UNIX = 10;
        internal const int VER_PLATFORM_MACOSX = 11;
        internal const int SHGFP_TYPE_CURRENT = 0;
        internal const int UOI_FLAGS = 1;
        internal const int WSF_VISIBLE = 1;
        internal const int CSIDL_FLAG_CREATE = 32768;
        internal const int CSIDL_FLAG_DONT_VERIFY = 16384;
        internal const int CSIDL_ADMINTOOLS = 48;
        internal const int CSIDL_CDBURN_AREA = 59;
        internal const int CSIDL_COMMON_ADMINTOOLS = 47;
        internal const int CSIDL_COMMON_DOCUMENTS = 46;
        internal const int CSIDL_COMMON_MUSIC = 53;
        internal const int CSIDL_COMMON_OEM_LINKS = 58;
        internal const int CSIDL_COMMON_PICTURES = 54;
        internal const int CSIDL_COMMON_STARTMENU = 22;
        internal const int CSIDL_COMMON_PROGRAMS = 23;
        internal const int CSIDL_COMMON_STARTUP = 24;
        internal const int CSIDL_COMMON_DESKTOPDIRECTORY = 25;
        internal const int CSIDL_COMMON_TEMPLATES = 45;
        internal const int CSIDL_COMMON_VIDEO = 55;
        internal const int CSIDL_FONTS = 20;
        internal const int CSIDL_MYVIDEO = 14;
        internal const int CSIDL_NETHOOD = 19;
        internal const int CSIDL_PRINTHOOD = 27;
        internal const int CSIDL_PROFILE = 40;
        internal const int CSIDL_PROGRAM_FILES_COMMONX86 = 44;
        internal const int CSIDL_PROGRAM_FILESX86 = 42;
        internal const int CSIDL_RESOURCES = 56;
        internal const int CSIDL_RESOURCES_LOCALIZED = 57;
        internal const int CSIDL_SYSTEMX86 = 41;
        internal const int CSIDL_WINDOWS = 36;
        internal const int CSIDL_APPDATA = 26;
        internal const int CSIDL_COMMON_APPDATA = 35;
        internal const int CSIDL_LOCAL_APPDATA = 28;
        internal const int CSIDL_COOKIES = 33;
        internal const int CSIDL_FAVORITES = 6;
        internal const int CSIDL_HISTORY = 34;
        internal const int CSIDL_INTERNET_CACHE = 32;
        internal const int CSIDL_PROGRAMS = 2;
        internal const int CSIDL_RECENT = 8;
        internal const int CSIDL_SENDTO = 9;
        internal const int CSIDL_STARTMENU = 11;
        internal const int CSIDL_STARTUP = 7;
        internal const int CSIDL_SYSTEM = 37;
        internal const int CSIDL_TEMPLATES = 21;
        internal const int CSIDL_DESKTOPDIRECTORY = 16;
        internal const int CSIDL_PERSONAL = 5;
        internal const int CSIDL_PROGRAM_FILES = 38;
        internal const int CSIDL_PROGRAM_FILES_COMMON = 43;
        internal const int CSIDL_DESKTOP = 0;
        internal const int CSIDL_DRIVES = 17;
        internal const int CSIDL_MYMUSIC = 13;
        internal const int CSIDL_MYPICTURES = 39;
        internal const int NameSamCompatible = 2;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_INVALID = 0;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_INT64 = 1;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_UINT64 = 2;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_STRING = 3;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_FQBN = 4;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_SID = 5;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_BOOLEAN = 6;
        internal const int CLAIM_SECURITY_ATTRIBUTE_TYPE_OCTET_STRING = 16;
        internal const int CLAIM_SECURITY_ATTRIBUTE_NON_INHERITABLE = 1;
        internal const int CLAIM_SECURITY_ATTRIBUTE_VALUE_CASE_SENSITIVE = 2;
        internal const int CLAIM_SECURITY_ATTRIBUTE_USE_FOR_DENY_ONLY = 4;
        internal const int CLAIM_SECURITY_ATTRIBUTE_DISABLED_BY_DEFAULT = 8;
        internal const int CLAIM_SECURITY_ATTRIBUTE_DISABLED = 16;
        internal const int CLAIM_SECURITY_ATTRIBUTE_MANDATORY = 32;
        internal const int CLAIM_SECURITY_ATTRIBUTE_VALID_FLAGS = 63;
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        private static readonly Version ThreadErrorModeMinOsVersion = new Version(6, 1, 7600);
        private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;
        private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;
        private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LocalFree(IntPtr handle);
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string moduleName);
        [SecurityCritical]
        internal static bool DoesWin32MethodExist(string moduleName, string methodName)
        {
            IntPtr moduleHandle = Win32Native.GetModuleHandle(moduleName);
            if (moduleHandle == IntPtr.Zero)
            {
                return false;
            }
            IntPtr procAddress = Win32Native.GetProcAddress(moduleHandle, methodName);
            return procAddress != IntPtr.Zero;
        }
        [SecurityCritical]
        internal static SafeFileHandle SafeCreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, Win32Native.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile)
        {
            SafeFileHandle safeFileHandle = Win32Native.CreateFile(lpFileName, dwDesiredAccess, dwShareMode, securityAttrs, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
            if (!safeFileHandle.IsInvalid)
            {
                int fileType = Win32Native.GetFileType(safeFileHandle);
                if (fileType != 1)
                {
                    safeFileHandle.Dispose();
                    throw new NotSupportedException("NotSupported_FileStreamOnNonFiles");
                }
            }
            return safeFileHandle;
        }
        [SecurityCritical]
        internal static SafeFileHandle UnsafeCreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, Win32Native.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile)
        {
            return Win32Native.CreateFile(lpFileName, dwDesiredAccess, dwShareMode, securityAttrs, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
        }
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll")]
        internal static extern int GetFileType(SafeFileHandle handle);
        internal static int MakeHRFromErrorCode(int errorCode)
        {
            return -2147024896 | errorCode;
        }
        internal static int SetErrorMode(int newMode)
        {
            if (Environment.OSVersion.Version >= Win32Native.ThreadErrorModeMinOsVersion)
            {
                int result;
                Win32Native.SetErrorMode_Win7AndNewer(newMode, out result);
                return result;
            }
            return Win32Native.SetErrorMode_VistaAndOlder(newMode);
        }
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string methodName);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, Win32Native.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll", EntryPoint = "SetErrorMode", ExactSpelling = true)]
        private static extern int SetErrorMode_VistaAndOlder(int newMode);
        [DllImport("kernel32.dll", EntryPoint = "SetThreadErrorMode", SetLastError = true)]
        private static extern bool SetErrorMode_Win7AndNewer(int newMode, out int oldMode);
    }
}
#pragma warning restore 0649
#pragma warning restore 0169