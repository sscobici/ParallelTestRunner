using System.Collections.Generic;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Trx;
using ParallelTestRunner.VSTest.Common;

namespace ParallelTestRunner.VSTest.Impl
{
    public class VSTestCollectorImpl : ICollector
    {
        public ITrxParser TrxParser { get; set; }

        public IVSTestFileHelper FileHelper { get; set; }

        public IList<ResultFile> Collect(IList<RunData> items)
        {
            var trxFileList = new List<ResultFile>();

            foreach (RunData item in items)
            {
                using (var stream = FileHelper.OpenTrxFile(item))
                {
                    var file = TrxParser.Parse(stream);
                    trxFileList.Add(file);
                }
            }

            return trxFileList;
        }
    }
}