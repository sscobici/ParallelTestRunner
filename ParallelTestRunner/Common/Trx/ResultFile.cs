using System;
using System.Collections.Generic;

namespace ParallelTestRunner.Common.Trx
{
    public class ResultFile
    {
        public IList<TestResult> Results { get; set; }
        
        public ResultSummary Summary { get; set; }
        
        public IList<string> StdOut { get; set; }
        
        public string RunDeploymentRoot { get; set; }
    }
}