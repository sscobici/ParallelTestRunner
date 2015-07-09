using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;

namespace ParallelTestRunner.Process2
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    internal class ThreadInfo
    {
        public int threadId;
        public int processId;
        public int basePriority;
        public int currentPriority;
        public IntPtr startAddress;
        public ThreadState threadState;
        public ThreadWaitReason threadWaitReason;
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ThreadInfo()
        {
        }
    }
}
