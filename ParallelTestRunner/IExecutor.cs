using System;
using ParallelTestRunner.Common;

namespace ParallelTestRunner
{
    public interface IExecutor
    {
        void Run(RunData data);
    }
}