using System.Collections.Generic;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner
{
    public interface ICollector
    {
        IList<ResultFile> Collect(IList<RunData> items);
    }
}