using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
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
    [SuppressUnmanagedCodeSecurity]
    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    internal static class SafeNativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal class PROCESS_INFORMATION
        {
            public IntPtr hProcess = IntPtr.Zero;
            public IntPtr hThread = IntPtr.Zero;
            public int dwProcessId;
            public int dwThreadId;
        }
        public const int MB_RIGHT = 524288;
        public const int MB_RTLREADING = 1048576;
        public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;
        public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;
        public const int FORMAT_MESSAGE_FROM_STRING = 1024;
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;
        public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
        public const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 255;
        public const int FORMAT_MESSAGE_FROM_HMODULE = 2048;
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetTextMetrics(IntPtr hDC, [In] [Out] NativeMethods.TEXTMETRIC tm);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetStockObject(int nIndex);
        [SecurityCritical]
        public static int MessageBox(IntPtr hWnd, string text, string caption, int type)
        {
            int result;
            try
            {
                result = SafeNativeMethods.MessageBoxSystem(hWnd, text, caption, type);
            }
            catch (DllNotFoundException)
            {
                result = 0;
            }
            catch (EntryPointNotFoundException)
            {
                result = 0;
            }
            return result;
        }
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long value);
        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out long value);
        [DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string msg);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string libFilename);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool FreeLibrary(HandleRef hModule);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        public static extern bool GetComputerName(StringBuilder lpBuffer, int[] nSize);
        public unsafe static int InterlockedCompareExchange(IntPtr pDestination, int exchange, int compare)
        {
            return Interlocked.CompareExchange(ref *(int*)pDestination.ToPointer(), exchange, compare);
        }
        [DllImport("perfcounter.dll", CharSet = CharSet.Auto)]
        public static extern int FormatFromRawValue(uint dwCounterType, uint dwFormat, ref long pTimeBase, NativeMethods.PDH_RAW_COUNTER pRawValue1, NativeMethods.PDH_RAW_COUNTER pRawValue2, NativeMethods.PDH_FMT_COUNTERVALUE pFmtValue);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool IsWow64Process(SafeProcessHandle hProcess, ref bool Wow64Process);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeWaitHandle CreateSemaphore(NativeMethods.SECURITY_ATTRIBUTES lpSecurityAttributes, int initialCount, int maximumCount, string name);
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeWaitHandle OpenSemaphore(int desiredAccess, bool inheritHandle, string name);
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReleaseSemaphore(SafeWaitHandle handle, int releaseCount, out int previousCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "MessageBoxW", ExactSpelling = true)]
        private static extern int MessageBoxSystem(IntPtr hWnd, string text, string caption, int type);
    }
}
