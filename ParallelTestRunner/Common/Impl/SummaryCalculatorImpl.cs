using System;
using System.Collections.Generic;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.Common.Impl
{
    public class SummaryCalculatorImpl : ISummaryCalculator
    {
        public ResultSummary Calculate(IList<ResultFile> files)
        {
            DateTime startTime = DateTime.Now;
            DateTime finishTime = DateTime.MinValue;
            ResultSummary summary = new ResultSummary();
            foreach (ResultFile file in files)
            {
                if (file.Summary.Outcome == "Failed")
                {
                    summary.Outcome = file.Summary.Outcome;
                }

                summary.Total += file.Summary.Total;
                summary.Executed += file.Summary.Executed;
                summary.Passed += file.Summary.Passed;
                summary.Failed += file.Summary.Failed;
                summary.Error += file.Summary.Error;
                summary.Timeout += file.Summary.Timeout;
                summary.Aborted += file.Summary.Aborted;
                summary.Inconclusive += file.Summary.Inconclusive;
                summary.PassedButRunAborted += file.Summary.PassedButRunAborted;
                summary.NotRunnable += file.Summary.NotRunnable;
                summary.NotExecuted += file.Summary.NotExecuted;
                summary.Disconnected += file.Summary.Disconnected;
                summary.Warning += file.Summary.Warning;
                summary.Completed += file.Summary.Completed;
                summary.InProgress += file.Summary.InProgress;
                summary.Pending += file.Summary.Pending;

                if (file.Summary.StartTime < startTime)
                {
                    startTime = file.Summary.StartTime;
                }

                if (file.Summary.FinishTime > finishTime)
                {
                    finishTime = file.Summary.FinishTime;
                }
            }

            summary.StartTime = startTime;
            summary.FinishTime = finishTime;
            return summary;
        }
    }
}