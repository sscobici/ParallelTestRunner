using System;
using System.Diagnostics;

namespace ParallelTestRunner.VSTest.Common
{
    public interface IProcessFactory
    {
        void StartProcessAndWait(ProcessStartInfo info, IProcessOutputReader handler);
    }
}