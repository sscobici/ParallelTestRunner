using System;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Common.Impl
{
    public class ProcessOutputReaderFactoryImpl : IProcessOutputReaderFactory
    {
        public IProcessOutputReader CreateReader(RunData data)
        {
            return new ProcessOutputReaderImpl(data);
        }
    }
}