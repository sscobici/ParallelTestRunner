using System.Diagnostics;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Common
{
    public interface IProcessStartInfoFactory
    {
        ProcessStartInfo CreateProcessStartInfo(RunData data);
    }
}