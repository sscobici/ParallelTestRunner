using System.Collections.Generic;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.Common
{
    public interface ISummaryCalculator
    {
        ResultSummary Calculate(IList<ResultFile> files);
    }
}