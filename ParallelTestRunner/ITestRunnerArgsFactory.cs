using System;

namespace ParallelTestRunner
{
    public interface ITestRunnerArgsFactory
    {
        ITestRunnerArgs ParseArgs(string[] args);
    }
}