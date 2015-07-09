#pragma warning disable 0649
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace ParallelTestRunner.Process2
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class TEXTMETRIC
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public char tmFirstChar;
            public char tmLastChar;
            public char tmDefaultChar;
            public char tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public TEXTMETRIC()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class STARTUPINFO
        {
            public int cb;
            public IntPtr lpReserved = IntPtr.Zero;
            public IntPtr lpDesktop = IntPtr.Zero;
            public IntPtr lpTitle = IntPtr.Zero;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2 = IntPtr.Zero;
            public SafeFileHandle hStdInput = new SafeFileHandle(IntPtr.Zero, false);
            public SafeFileHandle hStdOutput = new SafeFileHandle(IntPtr.Zero, false);
            public SafeFileHandle hStdError = new SafeFileHandle(IntPtr.Zero, false);
            public STARTUPINFO()
            {
                cb = Marshal.SizeOf(this);
            }
            public void Dispose()
            {
                if (hStdInput != null && !hStdInput.IsInvalid)
                {
                    hStdInput.Close();
                    hStdInput = null;
                }
                if (hStdOutput != null && !hStdOutput.IsInvalid)
                {
                    hStdOutput.Close();
                    hStdOutput = null;
                }
                if (hStdError != null && !hStdError.IsInvalid)
                {
                    hStdError.Close();
                    hStdError = null;
                }
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class SECURITY_ATTRIBUTES
        {
            public int nLength = 12;
            public SafeLocalMemHandle lpSecurityDescriptor = new SafeLocalMemHandle(IntPtr.Zero, false);
            public bool bInheritHandle;
        }
        [Flags]
        internal enum LogonFlags
        {
            LOGON_WITH_PROFILE = 1,
            LOGON_NETCREDENTIALS_ONLY = 2
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class WNDCLASS_I
        {
            public int style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance = IntPtr.Zero;
            public IntPtr hIcon = IntPtr.Zero;
            public IntPtr hCursor = IntPtr.Zero;
            public IntPtr hbrBackground = IntPtr.Zero;
            public IntPtr lpszMenuName = IntPtr.Zero;
            public IntPtr lpszClassName = IntPtr.Zero;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class WNDCLASS
        {
            public int style;
            public NativeMethods.WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance = IntPtr.Zero;
            public IntPtr hIcon = IntPtr.Zero;
            public IntPtr hCursor = IntPtr.Zero;
            public IntPtr hbrBackground = IntPtr.Zero;
            public string lpszMenuName;
            public string lpszClassName;
        }
        public struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }
        public enum StructFormatEnum
        {
            Ansi = 1,
            Unicode,
            Auto
        }
        public enum StructFormat
        {
            Ansi = 1,
            Unicode,
            Auto
        }
        public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public delegate int ConHndlr(int signalType);
        [StructLayout(LayoutKind.Sequential)]
        public class PDH_RAW_COUNTER
        {
            public int CStatus;
            public long TimeStamp;
            public long FirstValue;
            public long SecondValue;
            public int MultiCount;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public PDH_RAW_COUNTER()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public class PDH_FMT_COUNTERVALUE
        {
            public int CStatus;
            public double data;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public PDH_FMT_COUNTERVALUE()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_COUNTER_BLOCK
        {
            public int ByteLength;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public PERF_COUNTER_BLOCK()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_COUNTER_DEFINITION
        {
            public int ByteLength;
            public int CounterNameTitleIndex;
            public int CounterNameTitlePtr;
            public int CounterHelpTitleIndex;
            public int CounterHelpTitlePtr;
            public int DefaultScale;
            public int DetailLevel;
            public int CounterType;
            public int CounterSize;
            public int CounterOffset;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public PERF_COUNTER_DEFINITION()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_DATA_BLOCK
        {
            public int Signature1;
            public int Signature2;
            public int LittleEndian;
            public int Version;
            public int Revision;
            public int TotalByteLength;
            public int HeaderLength;
            public int NumObjectTypes;
            public int DefaultObject;
            public NativeMethods.SYSTEMTIME SystemTime;
            public int pad1;
            public long PerfTime;
            public long PerfFreq;
            public long PerfTime100nSec;
            public int SystemNameLength;
            public int SystemNameOffset;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public PERF_DATA_BLOCK()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_INSTANCE_DEFINITION
        {
            public int ByteLength;
            public int ParentObjectTitleIndex;
            public int ParentObjectInstance;
            public int UniqueID;
            public int NameOffset;
            public int NameLength;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public PERF_INSTANCE_DEFINITION()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_OBJECT_TYPE
        {
            public int TotalByteLength;
            public int DefinitionLength;
            public int HeaderLength;
            public int ObjectNameTitleIndex;
            public int ObjectNameTitlePtr;
            public int ObjectHelpTitleIndex;
            public int ObjectHelpTitlePtr;
            public int DetailLevel;
            public int NumCounters;
            public int DefaultCounter;
            public int NumInstances;
            public int CodePage;
            public long PerfTime;
            public long PerfFreq;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public PERF_OBJECT_TYPE()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class NtModuleInfo
        {
            public IntPtr BaseOfDll = (IntPtr)0;
            public int SizeOfImage;
            public IntPtr EntryPoint = (IntPtr)0;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class WinProcessEntry
        {
            public const int sizeofFileName = 260;
            public int dwSize;
            public int cntUsage;
            public int th32ProcessID;
            public IntPtr th32DefaultHeapID = (IntPtr)0;
            public int th32ModuleID;
            public int cntThreads;
            public int th32ParentProcessID;
            public int pcPriClassBase;
            public int dwFlags;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class WinThreadEntry
        {
            public int dwSize;
            public int cntUsage;
            public int th32ThreadID;
            public int th32OwnerProcessID;
            public int tpBasePri;
            public int tpDeltaPri;
            public int dwFlags;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public WinThreadEntry()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class WinModuleEntry
        {
            public const int sizeofModuleName = 256;
            public const int sizeofFileName = 260;
            public int dwSize;
            public int th32ModuleID;
            public int th32ProcessID;
            public int GlblcntUsage;
            public int ProccntUsage;
            public IntPtr modBaseAddr = (IntPtr)0;
            public int modBaseSize;
            public IntPtr hModule = (IntPtr)0;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class ShellExecuteInfo
        {
            public int cbSize;
            public int fMask;
            public IntPtr hwnd = (IntPtr)0;
            public IntPtr lpVerb = (IntPtr)0;
            public IntPtr lpFile = (IntPtr)0;
            public IntPtr lpParameters = (IntPtr)0;
            public IntPtr lpDirectory = (IntPtr)0;
            public int nShow;
            public IntPtr hInstApp = (IntPtr)0;
            public IntPtr lpIDList = (IntPtr)0;
            public IntPtr lpClass = (IntPtr)0;
            public IntPtr hkeyClass = (IntPtr)0;
            public int dwHotKey;
            public IntPtr hIcon = (IntPtr)0;
            public IntPtr hProcess = (IntPtr)0;
            public ShellExecuteInfo()
            {
                cbSize = Marshal.SizeOf(this);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class NtProcessBasicInfo
        {
            public int ExitStatus;
            public IntPtr PebBaseAddress = (IntPtr)0;
            public IntPtr AffinityMask = (IntPtr)0;
            public int BasePriority;
            public IntPtr UniqueProcessId = (IntPtr)0;
            public IntPtr InheritedFromUniqueProcessId = (IntPtr)0;
        }
        internal struct LUID
        {
            public int LowPart;
            public int HighPart;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class TokenPrivileges
        {
            public int PrivilegeCount = 1;
            public NativeMethods.LUID Luid;
            public int Attributes;
        }
        internal delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);
        [StructLayout(LayoutKind.Sequential)]
        internal class SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
            public override string ToString()
            {
                return string.Concat(new string[]
                {
                    "[SYSTEMTIME: ",
                    wDay.ToString(CultureInfo.CurrentCulture),
                    "/",
                    wMonth.ToString(CultureInfo.CurrentCulture),
                    "/",
                    wYear.ToString(CultureInfo.CurrentCulture),
                    " ",
                    wHour.ToString(CultureInfo.CurrentCulture),
                    ":",
                    wMinute.ToString(CultureInfo.CurrentCulture),
                    ":",
                    wSecond.ToString(CultureInfo.CurrentCulture),
                    "]"
                });
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public SYSTEMTIME()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class VS_FIXEDFILEINFO
        {
            public int dwSignature;
            public int dwStructVersion;
            public int dwFileVersionMS;
            public int dwFileVersionLS;
            public int dwProductVersionMS;
            public int dwProductVersionLS;
            public int dwFileFlagsMask;
            public int dwFileFlags;
            public int dwFileOS;
            public int dwFileType;
            public int dwFileSubtype;
            public int dwFileDateMS;
            public int dwFileDateLS;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public VS_FIXEDFILEINFO()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class USEROBJECTFLAGS
        {
            public int fInherit;
            public int fReserved;
            public int dwFlags;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public USEROBJECTFLAGS()
            {
            }
        }
        internal static class Util
        {
            public static int HIWORD(int n)
            {
                return n >> 16 & 65535;
            }
            public static int LOWORD(int n)
            {
                return n & 65535;
            }
        }
        internal struct MEMORY_BASIC_INFORMATION
        {
            internal IntPtr BaseAddress;
            internal IntPtr AllocationBase;
            internal uint AllocationProtect;
            internal UIntPtr RegionSize;
            internal uint State;
            internal uint Protect;
            internal uint Type;
        }
        public const int DEFAULT_GUI_FONT = 17;
        public const int SM_CYSCREEN = 1;
        public const int GENERIC_READ = -2147483648;
        public const int GENERIC_WRITE = 1073741824;
        public const int FILE_SHARE_READ = 1;
        public const int FILE_SHARE_WRITE = 2;
        public const int FILE_SHARE_DELETE = 4;
        public const int S_OK = 0;
        public const int E_ABORT = -2147467260;
        public const int E_NOTIMPL = -2147467263;
        public const int CREATE_ALWAYS = 2;
        public const int FILE_ATTRIBUTE_NORMAL = 128;
        public const int STARTF_USESTDHANDLES = 256;
        public const int STD_INPUT_HANDLE = -10;
        public const int STD_OUTPUT_HANDLE = -11;
        public const int STD_ERROR_HANDLE = -12;
        public const int STILL_ACTIVE = 259;
        public const int SW_HIDE = 0;
        public const int WAIT_OBJECT_0 = 0;
        public const int WAIT_FAILED = -1;
        public const int WAIT_TIMEOUT = 258;
        public const int WAIT_ABANDONED = 128;
        public const int WAIT_ABANDONED_0 = 128;
        public const int MOVEFILE_REPLACE_EXISTING = 1;
        public const int ERROR_CLASS_ALREADY_EXISTS = 1410;
        public const int ERROR_NONE_MAPPED = 1332;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int ERROR_INVALID_NAME = 123;
        public const int ERROR_PROC_NOT_FOUND = 127;
        public const int ERROR_BAD_EXE_FORMAT = 193;
        public const int ERROR_EXE_MACHINE_TYPE_MISMATCH = 216;
        public const int MAX_PATH = 260;
        public const int UIS_SET = 1;
        public const int WSF_VISIBLE = 1;
        public const int UIS_CLEAR = 2;
        public const int UISF_HIDEFOCUS = 1;
        public const int UISF_HIDEACCEL = 2;
        public const int USERCLASSTYPE_FULL = 1;
        public const int UOI_FLAGS = 1;
        public const int COLOR_WINDOW = 5;
        public const int WS_POPUP = -2147483648;
        public const int WS_VISIBLE = 268435456;
        public const int WM_SETTINGCHANGE = 26;
        public const int WM_SYSCOLORCHANGE = 21;
        public const int WM_QUERYENDSESSION = 17;
        public const int WM_QUIT = 18;
        public const int WM_ENDSESSION = 22;
        public const int WM_POWERBROADCAST = 536;
        public const int WM_COMPACTING = 65;
        public const int WM_DISPLAYCHANGE = 126;
        public const int WM_FONTCHANGE = 29;
        public const int WM_PALETTECHANGED = 785;
        public const int WM_TIMECHANGE = 30;
        public const int WM_THEMECHANGED = 794;
        public const int WM_WTSSESSION_CHANGE = 689;
        public const int ENDSESSION_LOGOFF = -2147483648;
        public const int WM_TIMER = 275;
        public const int WM_USER = 1024;
        public const int WM_CREATETIMER = 1025;
        public const int WM_KILLTIMER = 1026;
        public const int WM_REFLECT = 8192;
        public const int WTS_CONSOLE_CONNECT = 1;
        public const int WTS_CONSOLE_DISCONNECT = 2;
        public const int WTS_REMOTE_CONNECT = 3;
        public const int WTS_REMOTE_DISCONNECT = 4;
        public const int WTS_SESSION_LOGON = 5;
        public const int WTS_SESSION_LOGOFF = 6;
        public const int WTS_SESSION_LOCK = 7;
        public const int WTS_SESSION_UNLOCK = 8;
        public const int WTS_SESSION_REMOTE_CONTROL = 9;
        public const int NOTIFY_FOR_THIS_SESSION = 0;
        public const int CTRL_C_EVENT = 0;
        public const int CTRL_BREAK_EVENT = 1;
        public const int CTRL_CLOSE_EVENT = 2;
        public const int CTRL_LOGOFF_EVENT = 5;
        public const int CTRL_SHUTDOWN_EVENT = 6;
        public const int SPI_GETBEEP = 1;
        public const int SPI_SETBEEP = 2;
        public const int SPI_GETMOUSE = 3;
        public const int SPI_SETMOUSE = 4;
        public const int SPI_GETBORDER = 5;
        public const int SPI_SETBORDER = 6;
        public const int SPI_GETKEYBOARDSPEED = 10;
        public const int SPI_SETKEYBOARDSPEED = 11;
        public const int SPI_LANGDRIVER = 12;
        public const int SPI_ICONHORIZONTALSPACING = 13;
        public const int SPI_GETSCREENSAVETIMEOUT = 14;
        public const int SPI_SETSCREENSAVETIMEOUT = 15;
        public const int SPI_GETSCREENSAVEACTIVE = 16;
        public const int SPI_SETSCREENSAVEACTIVE = 17;
        public const int SPI_GETGRIDGRANULARITY = 18;
        public const int SPI_SETGRIDGRANULARITY = 19;
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPI_SETDESKPATTERN = 21;
        public const int SPI_GETKEYBOARDDELAY = 22;
        public const int SPI_SETKEYBOARDDELAY = 23;
        public const int SPI_ICONVERTICALSPACING = 24;
        public const int SPI_GETICONTITLEWRAP = 25;
        public const int SPI_SETICONTITLEWRAP = 26;
        public const int SPI_GETMENUDROPALIGNMENT = 27;
        public const int SPI_SETMENUDROPALIGNMENT = 28;
        public const int SPI_SETDOUBLECLKWIDTH = 29;
        public const int SPI_SETDOUBLECLKHEIGHT = 30;
        public const int SPI_GETICONTITLELOGFONT = 31;
        public const int SPI_SETDOUBLECLICKTIME = 32;
        public const int SPI_SETMOUSEBUTTONSWAP = 33;
        public const int SPI_SETICONTITLELOGFONT = 34;
        public const int SPI_GETFASTTASKSWITCH = 35;
        public const int SPI_SETFASTTASKSWITCH = 36;
        public const int SPI_SETDRAGFULLWINDOWS = 37;
        public const int SPI_GETDRAGFULLWINDOWS = 38;
        public const int SPI_GETNONCLIENTMETRICS = 41;
        public const int SPI_SETNONCLIENTMETRICS = 42;
        public const int SPI_GETMINIMIZEDMETRICS = 43;
        public const int SPI_SETMINIMIZEDMETRICS = 44;
        public const int SPI_GETICONMETRICS = 45;
        public const int SPI_SETICONMETRICS = 46;
        public const int SPI_SETWORKAREA = 47;
        public const int SPI_GETWORKAREA = 48;
        public const int SPI_SETPENWINDOWS = 49;
        public const int SPI_GETHIGHCONTRAST = 66;
        public const int SPI_SETHIGHCONTRAST = 67;
        public const int SPI_GETKEYBOARDPREF = 68;
        public const int SPI_SETKEYBOARDPREF = 69;
        public const int SPI_GETSCREENREADER = 70;
        public const int SPI_SETSCREENREADER = 71;
        public const int SPI_GETANIMATION = 72;
        public const int SPI_SETANIMATION = 73;
        public const int SPI_GETFONTSMOOTHING = 74;
        public const int SPI_SETFONTSMOOTHING = 75;
        public const int SPI_SETDRAGWIDTH = 76;
        public const int SPI_SETDRAGHEIGHT = 77;
        public const int SPI_SETHANDHELD = 78;
        public const int SPI_GETLOWPOWERTIMEOUT = 79;
        public const int SPI_GETPOWEROFFTIMEOUT = 80;
        public const int SPI_SETLOWPOWERTIMEOUT = 81;
        public const int SPI_SETPOWEROFFTIMEOUT = 82;
        public const int SPI_GETLOWPOWERACTIVE = 83;
        public const int SPI_GETPOWEROFFACTIVE = 84;
        public const int SPI_SETLOWPOWERACTIVE = 85;
        public const int SPI_SETPOWEROFFACTIVE = 86;
        public const int SPI_SETCURSORS = 87;
        public const int SPI_SETICONS = 88;
        public const int SPI_GETDEFAULTINPUTLANG = 89;
        public const int SPI_SETDEFAULTINPUTLANG = 90;
        public const int SPI_SETLANGTOGGLE = 91;
        public const int SPI_GETWINDOWSEXTENSION = 92;
        public const int SPI_SETMOUSETRAILS = 93;
        public const int SPI_GETMOUSETRAILS = 94;
        public const int SPI_SETSCREENSAVERRUNNING = 97;
        public const int SPI_SCREENSAVERRUNNING = 97;
        public const int SPI_GETFILTERKEYS = 50;
        public const int SPI_SETFILTERKEYS = 51;
        public const int SPI_GETTOGGLEKEYS = 52;
        public const int SPI_SETTOGGLEKEYS = 53;
        public const int SPI_GETMOUSEKEYS = 54;
        public const int SPI_SETMOUSEKEYS = 55;
        public const int SPI_GETSHOWSOUNDS = 56;
        public const int SPI_SETSHOWSOUNDS = 57;
        public const int SPI_GETSTICKYKEYS = 58;
        public const int SPI_SETSTICKYKEYS = 59;
        public const int SPI_GETACCESSTIMEOUT = 60;
        public const int SPI_SETACCESSTIMEOUT = 61;
        public const int SPI_GETSERIALKEYS = 62;
        public const int SPI_SETSERIALKEYS = 63;
        public const int SPI_GETSOUNDSENTRY = 64;
        public const int SPI_SETSOUNDSENTRY = 65;
        public const int SPI_GETSNAPTODEFBUTTON = 95;
        public const int SPI_SETSNAPTODEFBUTTON = 96;
        public const int SPI_GETMOUSEHOVERWIDTH = 98;
        public const int SPI_SETMOUSEHOVERWIDTH = 99;
        public const int SPI_GETMOUSEHOVERHEIGHT = 100;
        public const int SPI_SETMOUSEHOVERHEIGHT = 101;
        public const int SPI_GETMOUSEHOVERTIME = 102;
        public const int SPI_SETMOUSEHOVERTIME = 103;
        public const int SPI_GETWHEELSCROLLLINES = 104;
        public const int SPI_SETWHEELSCROLLLINES = 105;
        public const int SPI_GETMENUSHOWDELAY = 106;
        public const int SPI_SETMENUSHOWDELAY = 107;
        public const int SPI_GETSHOWIMEUI = 110;
        public const int SPI_SETSHOWIMEUI = 111;
        public const int SPI_GETMOUSESPEED = 112;
        public const int SPI_SETMOUSESPEED = 113;
        public const int SPI_GETSCREENSAVERRUNNING = 114;
        public const int SPI_GETDESKWALLPAPER = 115;
        public const int SPI_GETACTIVEWINDOWTRACKING = 4096;
        public const int SPI_SETACTIVEWINDOWTRACKING = 4097;
        public const int SPI_GETMENUANIMATION = 4098;
        public const int SPI_SETMENUANIMATION = 4099;
        public const int SPI_GETCOMBOBOXANIMATION = 4100;
        public const int SPI_SETCOMBOBOXANIMATION = 4101;
        public const int SPI_GETLISTBOXSMOOTHSCROLLING = 4102;
        public const int SPI_SETLISTBOXSMOOTHSCROLLING = 4103;
        public const int SPI_GETGRADIENTCAPTIONS = 4104;
        public const int SPI_SETGRADIENTCAPTIONS = 4105;
        public const int SPI_GETKEYBOARDCUES = 4106;
        public const int SPI_SETKEYBOARDCUES = 4107;
        public const int SPI_GETMENUUNDERLINES = 4106;
        public const int SPI_SETMENUUNDERLINES = 4107;
        public const int SPI_GETACTIVEWNDTRKZORDER = 4108;
        public const int SPI_SETACTIVEWNDTRKZORDER = 4109;
        public const int SPI_GETHOTTRACKING = 4110;
        public const int SPI_SETHOTTRACKING = 4111;
        public const int SPI_GETMENUFADE = 4114;
        public const int SPI_SETMENUFADE = 4115;
        public const int SPI_GETSELECTIONFADE = 4116;
        public const int SPI_SETSELECTIONFADE = 4117;
        public const int SPI_GETTOOLTIPANIMATION = 4118;
        public const int SPI_SETTOOLTIPANIMATION = 4119;
        public const int SPI_GETTOOLTIPFADE = 4120;
        public const int SPI_SETTOOLTIPFADE = 4121;
        public const int SPI_GETCURSORSHADOW = 4122;
        public const int SPI_SETCURSORSHADOW = 4123;
        public const int SPI_GETUIEFFECTS = 4158;
        public const int SPI_SETUIEFFECTS = 4159;
        public const int SPI_GETFOREGROUNDLOCKTIMEOUT = 8192;
        public const int SPI_SETFOREGROUNDLOCKTIMEOUT = 8193;
        public const int SPI_GETACTIVEWNDTRKTIMEOUT = 8194;
        public const int SPI_SETACTIVEWNDTRKTIMEOUT = 8195;
        public const int SPI_GETFOREGROUNDFLASHCOUNT = 8196;
        public const int SPI_SETFOREGROUNDFLASHCOUNT = 8197;
        public const int SPI_GETCARETWIDTH = 8198;
        public const int SPI_SETCARETWIDTH = 8199;
        public const uint STATUS_INFO_LENGTH_MISMATCH = 3221225476u;
        public const int PBT_APMQUERYSUSPEND = 0;
        public const int PBT_APMQUERYSTANDBY = 1;
        public const int PBT_APMQUERYSUSPENDFAILED = 2;
        public const int PBT_APMQUERYSTANDBYFAILED = 3;
        public const int PBT_APMSUSPEND = 4;
        public const int PBT_APMSTANDBY = 5;
        public const int PBT_APMRESUMECRITICAL = 6;
        public const int PBT_APMRESUMESUSPEND = 7;
        public const int PBT_APMRESUMESTANDBY = 8;
        public const int PBT_APMBATTERYLOW = 9;
        public const int PBT_APMPOWERSTATUSCHANGE = 10;
        public const int PBT_APMOEMEVENT = 11;
        public const int STARTF_USESHOWWINDOW = 1;
        public const int FILE_MAP_WRITE = 2;
        public const int FILE_MAP_READ = 4;
        public const int PAGE_READWRITE = 4;
        public const int GENERIC_EXECUTE = 536870912;
        public const int GENERIC_ALL = 268435456;
        public const int ERROR_NOT_READY = 21;
        public const int ERROR_LOCK_FAILED = 167;
        public const int ERROR_BUSY = 170;
        public const int IMPERSONATION_LEVEL_SecurityAnonymous = 0;
        public const int IMPERSONATION_LEVEL_SecurityIdentification = 1;
        public const int IMPERSONATION_LEVEL_SecurityImpersonation = 2;
        public const int IMPERSONATION_LEVEL_SecurityDelegation = 3;
        public const int TOKEN_TYPE_TokenPrimary = 1;
        public const int TOKEN_TYPE_TokenImpersonation = 2;
        public const int TOKEN_ALL_ACCESS = 983551;
        public const int TOKEN_EXECUTE = 131072;
        public const int TOKEN_READ = 131080;
        public const int TOKEN_IMPERSONATE = 4;
        public const int PIPE_ACCESS_INBOUND = 1;
        public const int PIPE_ACCESS_OUTBOUND = 2;
        public const int PIPE_ACCESS_DUPLEX = 3;
        public const int PIPE_WAIT = 0;
        public const int PIPE_NOWAIT = 1;
        public const int PIPE_READMODE_BYTE = 0;
        public const int PIPE_READMODE_MESSAGE = 2;
        public const int PIPE_TYPE_BYTE = 0;
        public const int PIPE_TYPE_MESSAGE = 4;
        public const int PIPE_SINGLE_INSTANCES = 1;
        public const int PIPE_UNLIMITED_INSTANCES = 255;
        public const int FILE_FLAG_OVERLAPPED = 1073741824;
        public const int PM_REMOVE = 1;
        public const int QS_KEY = 1;
        public const int QS_MOUSEMOVE = 2;
        public const int QS_MOUSEBUTTON = 4;
        public const int QS_POSTMESSAGE = 8;
        public const int QS_TIMER = 16;
        public const int QS_PAINT = 32;
        public const int QS_SENDMESSAGE = 64;
        public const int QS_HOTKEY = 128;
        public const int QS_ALLPOSTMESSAGE = 256;
        public const int QS_MOUSE = 6;
        public const int QS_INPUT = 7;
        public const int QS_ALLEVENTS = 191;
        public const int QS_ALLINPUT = 255;
        public const int MWMO_INPUTAVAILABLE = 4;
        internal const byte ONESTOPBIT = 0;
        internal const byte ONE5STOPBITS = 1;
        internal const byte TWOSTOPBITS = 2;
        internal const int DTR_CONTROL_DISABLE = 0;
        internal const int DTR_CONTROL_ENABLE = 1;
        internal const int DTR_CONTROL_HANDSHAKE = 2;
        internal const int RTS_CONTROL_DISABLE = 0;
        internal const int RTS_CONTROL_ENABLE = 1;
        internal const int RTS_CONTROL_HANDSHAKE = 2;
        internal const int RTS_CONTROL_TOGGLE = 3;
        internal const int MS_CTS_ON = 16;
        internal const int MS_DSR_ON = 32;
        internal const int MS_RING_ON = 64;
        internal const int MS_RLSD_ON = 128;
        internal const byte EOFCHAR = 26;
        internal const int FBINARY = 0;
        internal const int FPARITY = 1;
        internal const int FOUTXCTSFLOW = 2;
        internal const int FOUTXDSRFLOW = 3;
        internal const int FDTRCONTROL = 4;
        internal const int FDSRSENSITIVITY = 6;
        internal const int FTXCONTINUEONXOFF = 7;
        internal const int FOUTX = 8;
        internal const int FINX = 9;
        internal const int FERRORCHAR = 10;
        internal const int FNULL = 11;
        internal const int FRTSCONTROL = 12;
        internal const int FABORTONOERROR = 14;
        internal const int FDUMMY2 = 15;
        internal const int PURGE_TXABORT = 1;
        internal const int PURGE_RXABORT = 2;
        internal const int PURGE_TXCLEAR = 4;
        internal const int PURGE_RXCLEAR = 8;
        internal const byte DEFAULTXONCHAR = 17;
        internal const byte DEFAULTXOFFCHAR = 19;
        internal const int SETRTS = 3;
        internal const int CLRRTS = 4;
        internal const int SETDTR = 5;
        internal const int CLRDTR = 6;
        internal const int EV_RXCHAR = 1;
        internal const int EV_RXFLAG = 2;
        internal const int EV_CTS = 8;
        internal const int EV_DSR = 16;
        internal const int EV_RLSD = 32;
        internal const int EV_BREAK = 64;
        internal const int EV_ERR = 128;
        internal const int EV_RING = 256;
        internal const int ALL_EVENTS = 507;
        internal const int CE_RXOVER = 1;
        internal const int CE_OVERRUN = 2;
        internal const int CE_PARITY = 4;
        internal const int CE_FRAME = 8;
        internal const int CE_BREAK = 16;
        internal const int CE_TXFULL = 256;
        internal const int MAXDWORD = -1;
        internal const int NOPARITY = 0;
        internal const int ODDPARITY = 1;
        internal const int EVENPARITY = 2;
        internal const int MARKPARITY = 3;
        internal const int SPACEPARITY = 4;
        internal const int SDDL_REVISION_1 = 1;
        public const int SECURITY_DESCRIPTOR_REVISION = 1;
        public const int HKEY_PERFORMANCE_DATA = -2147483644;
        public const int DWORD_SIZE = 4;
        public const int LARGE_INTEGER_SIZE = 8;
        public const int PERF_NO_INSTANCES = -1;
        public const int PERF_SIZE_DWORD = 0;
        public const int PERF_SIZE_LARGE = 256;
        public const int PERF_SIZE_ZERO = 512;
        public const int PERF_SIZE_VARIABLE_LEN = 768;
        public const int PERF_NO_UNIQUE_ID = -1;
        public const int PERF_TYPE_NUMBER = 0;
        public const int PERF_TYPE_COUNTER = 1024;
        public const int PERF_TYPE_TEXT = 2048;
        public const int PERF_TYPE_ZERO = 3072;
        public const int PERF_NUMBER_HEX = 0;
        public const int PERF_NUMBER_DECIMAL = 65536;
        public const int PERF_NUMBER_DEC_1000 = 131072;
        public const int PERF_COUNTER_VALUE = 0;
        public const int PERF_COUNTER_RATE = 65536;
        public const int PERF_COUNTER_FRACTION = 131072;
        public const int PERF_COUNTER_BASE = 196608;
        public const int PERF_COUNTER_ELAPSED = 262144;
        public const int PERF_COUNTER_QUEUELEN = 327680;
        public const int PERF_COUNTER_HISTOGRAM = 393216;
        public const int PERF_COUNTER_PRECISION = 458752;
        public const int PERF_TEXT_UNICODE = 0;
        public const int PERF_TEXT_ASCII = 65536;
        public const int PERF_TIMER_TICK = 0;
        public const int PERF_TIMER_100NS = 1048576;
        public const int PERF_OBJECT_TIMER = 2097152;
        public const int PERF_DELTA_COUNTER = 4194304;
        public const int PERF_DELTA_BASE = 8388608;
        public const int PERF_INVERSE_COUNTER = 16777216;
        public const int PERF_MULTI_COUNTER = 33554432;
        public const int PERF_DISPLAY_NO_SUFFIX = 0;
        public const int PERF_DISPLAY_PER_SEC = 268435456;
        public const int PERF_DISPLAY_PERCENT = 536870912;
        public const int PERF_DISPLAY_SECONDS = 805306368;
        public const int PERF_DISPLAY_NOSHOW = 1073741824;
        public const int PERF_COUNTER_COUNTER = 272696320;
        public const int PERF_COUNTER_TIMER = 541132032;
        public const int PERF_COUNTER_QUEUELEN_TYPE = 4523008;
        public const int PERF_COUNTER_LARGE_QUEUELEN_TYPE = 4523264;
        public const int PERF_COUNTER_100NS_QUEUELEN_TYPE = 5571840;
        public const int PERF_COUNTER_OBJ_TIME_QUEUELEN_TYPE = 6620416;
        public const int PERF_COUNTER_BULK_COUNT = 272696576;
        public const int PERF_COUNTER_TEXT = 2816;
        public const int PERF_COUNTER_RAWCOUNT = 65536;
        public const int PERF_COUNTER_LARGE_RAWCOUNT = 65792;
        public const int PERF_COUNTER_RAWCOUNT_HEX = 0;
        public const int PERF_COUNTER_LARGE_RAWCOUNT_HEX = 256;
        public const int PERF_SAMPLE_FRACTION = 549585920;
        public const int PERF_SAMPLE_COUNTER = 4260864;
        public const int PERF_COUNTER_NODATA = 1073742336;
        public const int PERF_COUNTER_TIMER_INV = 557909248;
        public const int PERF_SAMPLE_BASE = 1073939457;
        public const int PERF_AVERAGE_TIMER = 805438464;
        public const int PERF_AVERAGE_BASE = 1073939458;
        public const int PERF_OBJ_TIME_TIMER = 543229184;
        public const int PERF_AVERAGE_BULK = 1073874176;
        public const int PERF_OBJ_TIME_TIME = 543229184;
        public const int PERF_100NSEC_TIMER = 542180608;
        public const int PERF_100NSEC_TIMER_INV = 558957824;
        public const int PERF_COUNTER_MULTI_TIMER = 574686464;
        public const int PERF_COUNTER_MULTI_TIMER_INV = 591463680;
        public const int PERF_COUNTER_MULTI_BASE = 1107494144;
        public const int PERF_100NSEC_MULTI_TIMER = 575735040;
        public const int PERF_100NSEC_MULTI_TIMER_INV = 592512256;
        public const int PERF_RAW_FRACTION = 537003008;
        public const int PERF_LARGE_RAW_FRACTION = 537003264;
        public const int PERF_RAW_BASE = 1073939459;
        public const int PERF_LARGE_RAW_BASE = 1073939712;
        public const int PERF_ELAPSED_TIME = 807666944;
        public const int PERF_COUNTER_DELTA = 4195328;
        public const int PERF_COUNTER_LARGE_DELTA = 4195584;
        public const int PERF_PRECISION_SYSTEM_TIMER = 541525248;
        public const int PERF_PRECISION_100NS_TIMER = 542573824;
        public const int PERF_PRECISION_OBJECT_TIMER = 543622400;
        public const uint PDH_FMT_DOUBLE = 512u;
        public const uint PDH_FMT_NOSCALE = 4096u;
        public const uint PDH_FMT_NOCAP100 = 32768u;
        public const int PERF_DETAIL_NOVICE = 100;
        public const int PERF_DETAIL_ADVANCED = 200;
        public const int PERF_DETAIL_EXPERT = 300;
        public const int PERF_DETAIL_WIZARD = 400;
        public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;
        public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;
        public const int FORMAT_MESSAGE_FROM_STRING = 1024;
        public const int FORMAT_MESSAGE_FROM_HMODULE = 2048;
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;
        public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
        public const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 255;
        public const int LOAD_WITH_ALTERED_SEARCH_PATH = 8;
        public const int LOAD_LIBRARY_AS_DATAFILE = 2;
        public const int SEEK_READ = 2;
        public const int FORWARDS_READ = 4;
        public const int BACKWARDS_READ = 8;
        public const int ERROR_EVENTLOG_FILE_CHANGED = 1503;
        public const int NtPerfCounterSizeDword = 0;
        public const int NtPerfCounterSizeLarge = 256;
        public const int SHGFI_USEFILEATTRIBUTES = 16;
        public const int SHGFI_TYPENAME = 1024;
        public const int NtQueryProcessBasicInfo = 0;
        public const int NtQuerySystemProcessInformation = 5;
        public const int SEE_MASK_CLASSNAME = 1;
        public const int SEE_MASK_CLASSKEY = 3;
        public const int SEE_MASK_IDLIST = 4;
        public const int SEE_MASK_INVOKEIDLIST = 12;
        public const int SEE_MASK_ICON = 16;
        public const int SEE_MASK_HOTKEY = 32;
        public const int SEE_MASK_NOCLOSEPROCESS = 64;
        public const int SEE_MASK_CONNECTNETDRV = 128;
        public const int SEE_MASK_FLAG_DDEWAIT = 256;
        public const int SEE_MASK_DOENVSUBST = 512;
        public const int SEE_MASK_FLAG_NO_UI = 1024;
        public const int SEE_MASK_UNICODE = 16384;
        public const int SEE_MASK_NO_CONSOLE = 32768;
        public const int SEE_MASK_ASYNCOK = 1048576;
        public const int TH32CS_SNAPHEAPLIST = 1;
        public const int TH32CS_SNAPPROCESS = 2;
        public const int TH32CS_SNAPTHREAD = 4;
        public const int TH32CS_SNAPMODULE = 8;
        public const int TH32CS_INHERIT = -2147483648;
        public const int PROCESS_TERMINATE = 1;
        public const int PROCESS_CREATE_THREAD = 2;
        public const int PROCESS_SET_SESSIONID = 4;
        public const int PROCESS_VM_OPERATION = 8;
        public const int PROCESS_VM_READ = 16;
        public const int PROCESS_VM_WRITE = 32;
        public const int PROCESS_DUP_HANDLE = 64;
        public const int PROCESS_CREATE_PROCESS = 128;
        public const int PROCESS_SET_QUOTA = 256;
        public const int PROCESS_SET_INFORMATION = 512;
        public const int PROCESS_QUERY_INFORMATION = 1024;
        public const int PROCESS_QUERY_LIMITED_INFORMATION = 4096;
        public const int STANDARD_RIGHTS_REQUIRED = 983040;
        public const int SYNCHRONIZE = 1048576;
        public const int PROCESS_ALL_ACCESS = 2035711;
        public const int THREAD_TERMINATE = 1;
        public const int THREAD_SUSPEND_RESUME = 2;
        public const int THREAD_GET_CONTEXT = 8;
        public const int THREAD_SET_CONTEXT = 16;
        public const int THREAD_SET_INFORMATION = 32;
        public const int THREAD_QUERY_INFORMATION = 64;
        public const int THREAD_SET_THREAD_TOKEN = 128;
        public const int THREAD_IMPERSONATE = 256;
        public const int THREAD_DIRECT_IMPERSONATION = 512;
        public const int REG_BINARY = 3;
        public const int REG_MULTI_SZ = 7;
        public const int READ_CONTROL = 131072;
        public const int STANDARD_RIGHTS_READ = 131072;
        public const int KEY_QUERY_VALUE = 1;
        public const int KEY_ENUMERATE_SUB_KEYS = 8;
        public const int KEY_NOTIFY = 16;
        public const int KEY_READ = 131097;
        public const int ERROR_BROKEN_PIPE = 109;
        public const int ERROR_NO_DATA = 232;
        public const int ERROR_HANDLE_EOF = 38;
        public const int ERROR_IO_INCOMPLETE = 996;
        public const int ERROR_IO_PENDING = 997;
        public const int ERROR_FILE_EXISTS = 80;
        public const int ERROR_FILENAME_EXCED_RANGE = 206;
        public const int ERROR_MORE_DATA = 234;
        public const int ERROR_CANCELLED = 1223;
        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_PATH_NOT_FOUND = 3;
        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_INVALID_HANDLE = 6;
        public const int ERROR_NOT_ENOUGH_MEMORY = 8;
        public const int ERROR_BAD_COMMAND = 22;
        public const int ERROR_SHARING_VIOLATION = 32;
        public const int ERROR_OPERATION_ABORTED = 995;
        public const int ERROR_NO_ASSOCIATION = 1155;
        public const int ERROR_DLL_NOT_FOUND = 1157;
        public const int ERROR_DDE_FAIL = 1156;
        public const int ERROR_INVALID_PARAMETER = 87;
        public const int ERROR_PARTIAL_COPY = 299;
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_ALREADY_EXISTS = 183;
        public const int ERROR_COUNTER_TIMEOUT = 1121;
        public const int DUPLICATE_CLOSE_SOURCE = 1;
        public const int DUPLICATE_SAME_ACCESS = 2;
        public const int RPC_S_SERVER_UNAVAILABLE = 1722;
        public const int RPC_S_CALL_FAILED = 1726;
        public const int PDH_NO_DATA = -2147481643;
        public const int PDH_CALC_NEGATIVE_DENOMINATOR = -2147481642;
        public const int PDH_CALC_NEGATIVE_VALUE = -2147481640;
        public const int SE_ERR_FNF = 2;
        public const int SE_ERR_PNF = 3;
        public const int SE_ERR_ACCESSDENIED = 5;
        public const int SE_ERR_OOM = 8;
        public const int SE_ERR_DLLNOTFOUND = 32;
        public const int SE_ERR_SHARE = 26;
        public const int SE_ERR_ASSOCINCOMPLETE = 27;
        public const int SE_ERR_DDETIMEOUT = 28;
        public const int SE_ERR_DDEFAIL = 29;
        public const int SE_ERR_DDEBUSY = 30;
        public const int SE_ERR_NOASSOC = 31;
        public const int SE_PRIVILEGE_ENABLED = 2;
        public const int LOGON32_LOGON_BATCH = 4;
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int TOKEN_ADJUST_PRIVILEGES = 32;
        public const int TOKEN_QUERY = 8;
        public const int CREATE_NO_WINDOW = 134217728;
        public const int CREATE_SUSPENDED = 4;
        public const int CREATE_UNICODE_ENVIRONMENT = 1024;
        public const int SMTO_ABORTIFHUNG = 2;
        public const int GWL_STYLE = -16;
        public const int GCL_WNDPROC = -24;
        public const int GWL_WNDPROC = -4;
        public const int WS_DISABLED = 134217728;
        public const int WM_NULL = 0;
        public const int WM_CLOSE = 16;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_NORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;
        public const int SW_MAX = 10;
        public const int GW_OWNER = 4;
        public const int WHITENESS = 16711778;
        public const int VS_FILE_INFO = 16;
        public const int VS_VERSION_INFO = 1;
        public const int VS_USER_DEFINED = 100;
        public const int VS_FFI_SIGNATURE = -17890115;
        public const int VS_FFI_STRUCVERSION = 65536;
        public const int VS_FFI_FILEFLAGSMASK = 63;
        public const int VS_FF_DEBUG = 1;
        public const int VS_FF_PRERELEASE = 2;
        public const int VS_FF_PATCHED = 4;
        public const int VS_FF_PRIVATEBUILD = 8;
        public const int VS_FF_INFOINFERRED = 16;
        public const int VS_FF_SPECIALBUILD = 32;
        public const int VFT_UNKNOWN = 0;
        public const int VFT_APP = 1;
        public const int VFT_DLL = 2;
        public const int VFT_DRV = 3;
        public const int VFT_FONT = 4;
        public const int VFT_VXD = 5;
        public const int VFT_STATIC_LIB = 7;
        public const int VFT2_UNKNOWN = 0;
        public const int VFT2_DRV_PRINTER = 1;
        public const int VFT2_DRV_KEYBOARD = 2;
        public const int VFT2_DRV_LANGUAGE = 3;
        public const int VFT2_DRV_DISPLAY = 4;
        public const int VFT2_DRV_MOUSE = 5;
        public const int VFT2_DRV_NETWORK = 6;
        public const int VFT2_DRV_SYSTEM = 7;
        public const int VFT2_DRV_INSTALLABLE = 8;
        public const int VFT2_DRV_SOUND = 9;
        public const int VFT2_DRV_COMM = 10;
        public const int VFT2_DRV_INPUTMETHOD = 11;
        public const int VFT2_FONT_RASTER = 1;
        public const int VFT2_FONT_VECTOR = 2;
        public const int VFT2_FONT_TRUETYPE = 3;
        public const int GMEM_FIXED = 0;
        public const int GMEM_MOVEABLE = 2;
        public const int GMEM_NOCOMPACT = 16;
        public const int GMEM_NODISCARD = 32;
        public const int GMEM_ZEROINIT = 64;
        public const int GMEM_MODIFY = 128;
        public const int GMEM_DISCARDABLE = 256;
        public const int GMEM_NOT_BANKED = 4096;
        public const int GMEM_SHARE = 8192;
        public const int GMEM_DDESHARE = 8192;
        public const int GMEM_NOTIFY = 16384;
        public const int GMEM_LOWER = 4096;
        public const int GMEM_VALID_FLAGS = 32626;
        public const int GMEM_INVALID_HANDLE = 32768;
        public const int GHND = 66;
        public const int GPTR = 64;
        public const int GMEM_DISCARDED = 16384;
        public const int GMEM_LOCKCOUNT = 255;
        public const int UOI_NAME = 2;
        public const int UOI_TYPE = 3;
        public const int UOI_USER_SID = 4;
        public const int VER_PLATFORM_WIN32_NT = 2;
        public static readonly HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        public static readonly IntPtr HKEY_LOCAL_MACHINE = (IntPtr)(-2147483646);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetExitCodeProcess(SafeProcessHandle processHandle, out int exitCode);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetProcessTimes(SafeProcessHandle handle, out long creation, out long exit, out long kernel, out long user);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetThreadTimes(SafeThreadHandle handle, out long creation, out long exit, out long kernel, out long user);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetStdHandle(int whichHandle);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreatePipe(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, NativeMethods.SECURITY_ATTRIBUTES lpPipeAttributes, int nSize);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreateProcess([MarshalAs(UnmanagedType.LPTStr)] string lpApplicationName, StringBuilder lpCommandLine, NativeMethods.SECURITY_ATTRIBUTES lpProcessAttributes, NativeMethods.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, int dwCreationFlags, IntPtr lpEnvironment, [MarshalAs(UnmanagedType.LPTStr)] string lpCurrentDirectory, NativeMethods.STARTUPINFO lpStartupInfo, SafeNativeMethods.PROCESS_INFORMATION lpProcessInformation);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool TerminateProcess(SafeProcessHandle processHandle, int exitCode);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetCurrentProcessId();
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();
        internal static string GetLocalPath(string fileName)
        {
            Uri uri = new Uri(fileName);
            return uri.LocalPath + uri.Fragment;
        }
        [SuppressUnmanagedCodeSecurity]
        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreateProcessAsUser(SafeHandle hToken, string lpApplicationName, string lpCommandLine, NativeMethods.SECURITY_ATTRIBUTES lpProcessAttributes, NativeMethods.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, int dwCreationFlags, HandleRef lpEnvironment, string lpCurrentDirectory, NativeMethods.STARTUPINFO lpStartupInfo, SafeNativeMethods.PROCESS_INFORMATION lpProcessInformation);
        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        internal static extern bool CreateProcessWithLogonW(string userName, string domain, IntPtr password, NativeMethods.LogonFlags logonFlags, [MarshalAs(UnmanagedType.LPTStr)] string appName, StringBuilder cmdLine, int creationFlags, IntPtr environmentBlock, [MarshalAs(UnmanagedType.LPTStr)] string lpCurrentDirectory, NativeMethods.STARTUPINFO lpStartupInfo, SafeNativeMethods.PROCESS_INFORMATION lpProcessInformation);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WaitForInputIdle(SafeProcessHandle handle, int milliseconds);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeProcessHandle OpenProcess(int access, bool inherit, int processId);
        [DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumProcessModules(SafeProcessHandle handle, IntPtr modules, int size, ref int needed);
        [DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumProcesses(int[] processIds, int size, out int needed);
        [DllImport("psapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetModuleFileNameEx(HandleRef processHandle, HandleRef moduleHandle, StringBuilder baseName, int size);
        [DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetModuleInformation(SafeProcessHandle processHandle, HandleRef moduleHandle, NativeMethods.NtModuleInfo ntModuleInfo, int size);
        [DllImport("psapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetModuleBaseName(SafeProcessHandle processHandle, HandleRef moduleHandle, StringBuilder baseName, int size);
        [DllImport("psapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetModuleFileNameEx(SafeProcessHandle processHandle, HandleRef moduleHandle, StringBuilder baseName, int size);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetProcessWorkingSetSize(SafeProcessHandle handle, IntPtr min, IntPtr max);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetProcessWorkingSetSize(SafeProcessHandle handle, out IntPtr min, out IntPtr max);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetProcessAffinityMask(SafeProcessHandle handle, IntPtr mask);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetProcessAffinityMask(SafeProcessHandle handle, out IntPtr processMask, out IntPtr systemMask);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetThreadPriorityBoost(SafeThreadHandle handle, out bool disabled);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetThreadPriorityBoost(SafeThreadHandle handle, bool disabled);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetProcessPriorityBoost(SafeProcessHandle handle, out bool disabled);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetProcessPriorityBoost(SafeProcessHandle handle, bool disabled);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeThreadHandle OpenThread(int access, bool inherit, int threadId);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetThreadPriority(SafeThreadHandle handle, int priority);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetThreadPriority(SafeThreadHandle handle);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetThreadAffinityMask(SafeThreadHandle handle, HandleRef mask);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetThreadIdealProcessor(SafeThreadHandle handle, int processor);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(int flags, int processId);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Process32First(HandleRef handle, IntPtr entry);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Process32Next(HandleRef handle, IntPtr entry);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Thread32First(HandleRef handle, NativeMethods.WinThreadEntry entry);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Thread32Next(HandleRef handle, NativeMethods.WinThreadEntry entry);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Module32First(HandleRef handle, IntPtr entry);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Module32Next(HandleRef handle, IntPtr entry);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetPriorityClass(SafeProcessHandle handle);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetPriorityClass(SafeProcessHandle handle, int priorityClass);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumWindows(NativeMethods.EnumThreadWindowsCallback callback, IntPtr extraData);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShellExecuteEx(NativeMethods.ShellExecuteInfo info);
        [DllImport("ntdll.dll", CharSet = CharSet.Auto)]
        public static extern int NtQueryInformationProcess(SafeProcessHandle processHandle, int query, NativeMethods.NtProcessBasicInfo info, int size, int[] returnedSize);
        [DllImport("ntdll.dll", CharSet = CharSet.Auto)]
        public static extern int NtQuerySystemInformation(int query, IntPtr dataPtr, int size, out int returnedSize);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, NativeMethods.SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, SafeFileHandle hTemplateFile);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeFileHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeWaitHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool OpenProcessToken(HandleRef ProcessHandle, int DesiredAccess, out IntPtr TokenHandle);
        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LookupPrivilegeValue([MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPTStr)] string lpName, out NativeMethods.LUID lpLuid);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(HandleRef TokenHandle, bool DisableAllPrivileges, NativeMethods.TokenPrivileges NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam, int flags, int timeout, out IntPtr pdwResult);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(HandleRef hWnd, int nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);
    }
}
#pragma warning restore 0649