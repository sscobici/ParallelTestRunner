using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;

namespace ParallelTestRunner.Process2
{
    /// <summary>Provides data for the <see cref="E:System.Diagnostics.Process.OutputDataReceived" /> and <see cref="E:System.Diagnostics.Process.ErrorDataReceived" /> events.</summary>
    /// <filterpriority>2</filterpriority>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    public class DataReceivedEventArgs : EventArgs
    {
        internal string data;
        
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DataReceivedEventArgs(string data)
        {
            this.data = data;
        }

        /// <summary>Gets the line of characters that was written to a redirected <see cref="T:System.Diagnostics.Process" /> output stream.</summary>
        /// <returns>The line that was written by an associated <see cref="T:System.Diagnostics.Process" /> to its redirected <see cref="P:System.Diagnostics.Process.StandardOutput" /> or <see cref="P:System.Diagnostics.Process.StandardError" /> stream.</returns>
        /// <filterpriority>2</filterpriority>
        public string Data
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return data;
            }
        }
    }
}
