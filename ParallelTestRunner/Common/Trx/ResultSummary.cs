using System;

namespace ParallelTestRunner.Common.Trx
{
    public class ResultSummary
    {
        public string Outcome { get; set; }

        public int Total { get; set; }

        public int Executed { get; set; }

        public int Passed { get; set; }

        public int Failed { get; set; }

        public int Error { get; set; }

        public int Timeout { get; set; }

        public int Aborted { get; set; }

        public int Inconclusive { get; set; }

        public int PassedButRunAborted { get; set; }

        public int NotRunnable { get; set; }

        public int NotExecuted { get; set; }

        public int Disconnected { get; set; }

        public int Warning { get; set; }

        public int Completed { get; set; }

        public int InProgress { get; set; }

        public int Pending { get; set; }

        public string Name { get; set; }

        public string RunUser { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }
    }
}