using ParallelTestRunner.Common.Trx;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParallelTestRunner.Common
{
    public interface ITrxWriter
    {
        bool WriteFile(IList<ResultFile> files, Stream stream);

        Stream OpenResultFile(ITestRunnerArgs args);
    }
}