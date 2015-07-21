using System.Collections.Generic;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.Common
{
    public interface IRunDataBuilder
    {
        IList<RunData> Create(TestAssembly assembly);
    }
}