using System;
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
    internal class ModuleInfo
    {
        public string baseName;
        
        public string fileName;
        
        public IntPtr baseOfDll;
        
        public IntPtr entryPoint;
        
        public int sizeOfImage;

#pragma warning disable 0649
        public int Id;
#pragma warning restore 0649

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ModuleInfo()
        {
        }
    }
}
