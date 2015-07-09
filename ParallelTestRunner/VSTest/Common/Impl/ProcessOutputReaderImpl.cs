using System;
using System.Diagnostics;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Common.Impl
{
    public class ProcessOutputReaderImpl : IProcessOutputReader
    {
        private RunData runData;

        public ProcessOutputReaderImpl(RunData runData)
        {
            this.runData = runData;
        }

        public void OnDataReceived(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            runData.Output.AppendLine(data);

            if (data.StartsWith("Passed  ") ||
                data.StartsWith("Failed  ") ||
                data.StartsWith("Skipped  "))
            {
                Console.WriteLine(data);
            }
        }
    }
}