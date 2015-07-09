using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Common
{
    public interface IProcessOutputReaderFactory
    {
        IProcessOutputReader CreateReader(RunData data);
    }
}