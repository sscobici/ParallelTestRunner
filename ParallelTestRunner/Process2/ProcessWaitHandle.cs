using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
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
    internal class ProcessWaitHandle : WaitHandle
    {
        internal ProcessWaitHandle(SafeProcessHandle processHandle)
        {
            SafeWaitHandle safeWaitHandle = null;
            if (!NativeMethods.DuplicateHandle(new HandleRef(this, NativeMethods.GetCurrentProcess()), processHandle, new HandleRef(this, NativeMethods.GetCurrentProcess()), out safeWaitHandle, 0, false, 2))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            base.SafeWaitHandle = safeWaitHandle;
        }
    }
}
