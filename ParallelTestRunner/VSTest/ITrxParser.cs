using System.IO;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.VSTest
{
    public interface ITrxParser
    {
        ResultFile Parse(Stream stream);
    }
}