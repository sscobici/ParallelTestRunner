using System.Collections.Generic;
using System.IO;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.Common
{
    public interface ITrxWriter
    {
        void WriteFile(IList<ResultFile> files, Stream stream);

        Stream OpenResultFile(ITestRunnerArgs args);
    }
}