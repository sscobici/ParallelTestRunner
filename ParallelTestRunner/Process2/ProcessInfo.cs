using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ParallelTestRunner.Process2
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    internal class ProcessInfo
    {
        public ArrayList threadInfoList = new ArrayList();
        public int basePriority;
        public string processName;
        public int processId;
        public int handleCount;
        public long poolPagedBytes;
        public long poolNonpagedBytes;
        public long virtualBytes;
        public long virtualBytesPeak;
        public long workingSetPeak;
        public long workingSet;
        public long pageFileBytesPeak;
        public long pageFileBytes;
        public long privateBytes;
#pragma warning disable 0649
        public int mainModuleId;
#pragma warning restore 0649
        public int sessionId;
    }
}
