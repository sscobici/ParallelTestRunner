using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

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
    internal sealed class SafeLocalMemHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal SafeLocalMemHandle()
            : base(true)
        {
        }
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeLocalMemHandle(IntPtr existingHandle, bool ownsHandle)
            : base(ownsHandle)
        {
            base.SetHandle(existingHandle);
        }
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);
        protected override bool ReleaseHandle()
        {
            return SafeLocalMemHandle.LocalFree(handle) == IntPtr.Zero;
        }
    }
}
