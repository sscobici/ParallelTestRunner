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
    [SecurityCritical]
    internal sealed class SafeLocalAllocHandle : SafeBuffer
    {
        internal static SafeLocalAllocHandle InvalidHandle
        {
            get
            {
                return new SafeLocalAllocHandle(IntPtr.Zero);
            }
        }
        private SafeLocalAllocHandle()
            : base(true)
        {
        }
        internal SafeLocalAllocHandle(IntPtr handle)
            : base(true)
        {
            base.SetHandle(handle);
        }
        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            return Win32Native.LocalFree(handle) == IntPtr.Zero;
        }
    }
}
