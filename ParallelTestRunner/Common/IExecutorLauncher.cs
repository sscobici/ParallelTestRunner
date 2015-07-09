using System;

namespace ParallelTestRunner.Common
{
    public interface IExecutorLauncher
    {
        bool LaunchExecutor(RunData data);

        void WaitForAnyThreadToComplete();

        void WaitForAll();

        bool HasFreeThreads();
    }
}