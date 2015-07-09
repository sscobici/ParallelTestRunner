using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace ParallelTestRunner.Process2
{
    internal static class ProcessManager
    {
        public static int GetProcessIdFromHandle(SafeProcessHandle processHandle)
        {
            return NtProcessManager.GetProcessIdFromHandle(processHandle);
        }

        public static ProcessInfo[] GetProcessInfos(string machineName)
        {
            return NtProcessInfoHelper.GetProcessInfos();
        }

        public static SafeProcessHandle OpenProcess(int processId, int access, bool throwIfExited)
        {
            SafeProcessHandle safeProcessHandle = NativeMethods.OpenProcess(access, false, processId);
            int lastWin32Error = Marshal.GetLastWin32Error();
            if (!safeProcessHandle.IsInvalid)
            {
                return safeProcessHandle;
            }

            if (processId == 0)
            {
                throw new Win32Exception(5);
            }

            if (true)
            {
                throw new Win32Exception(lastWin32Error);
            }
        }
    }
}
