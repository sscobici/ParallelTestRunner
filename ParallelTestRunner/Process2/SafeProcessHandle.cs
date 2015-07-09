using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

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
    internal sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal static SafeProcessHandle InvalidHandle = new SafeProcessHandle(IntPtr.Zero);
        internal SafeProcessHandle()
            : base(true)
        {
        }
        internal SafeProcessHandle(IntPtr handle)
            : base(true)
        {
            base.SetHandle(handle);
        }
        internal void InitialSetHandle(IntPtr h)
        {
            handle = h;
        }
        protected override bool ReleaseHandle()
        {
            return SafeNativeMethods.CloseHandle(handle);
        }
    }
}
