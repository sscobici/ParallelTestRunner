using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
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
    internal sealed class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal SafeThreadHandle()
            : base(true)
        {
        }
        internal void InitialSetHandle(IntPtr h)
        {
            base.SetHandle(h);
        }
        protected override bool ReleaseHandle()
        {
            return SafeNativeMethods.CloseHandle(handle);
        }
    }
}
