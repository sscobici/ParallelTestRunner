using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.Impl
{
    public class TestRunnerImpl : ITestRunner
    {
        public int ResultCode { get; set; }

        public ITestRunnerArgs Args { get; set; }
        
        public IParser Parser { get; set; }
        
        public ICollector Collector { get; set; }
        
        public ICleaner Cleaner { get; set; }
        
        public IRunDataBuilder RunDataBuilder { get; set; }
        
        public IRunDataListBuilder RunDataListBuilder { get; set; }
        
        public IExecutorLauncher ExecutorLauncher { get; set; }
        
        public ITrxWriter TrxWriter { get; set; }
        
        public IBreaker Breaker { get; set; }
        
        public IWindowsFileHelper WindowsFileHelper { get; set; }

        public void Parse()
        {
            if (Breaker.IsBreakReceived())
            {
                return;
            }

            foreach (string assemblyPath in Args.AssemblyList)
            {
                Assembly assembly = WindowsFileHelper.GetAssembly(assemblyPath);
                TestAssembly testAssembly = Parser.Parse(assembly);
                IList<RunData> items = RunDataBuilder.Create(testAssembly);
                RunDataListBuilder.Add(items);
            }
        }

        public void Execute()
        {
            if (WindowsFileHelper.FolderExist(Args.Root) == false)
            {
                WindowsFileHelper.CreateFolder(Args.Root);
            }

            IRunDataEnumerator enumerator = RunDataListBuilder.GetEnumerator();

            if (enumerator.HasItems() == false)
            {
                Console.WriteLine("Test Classes where not found, nothing to run");
                return;
            }

            while (enumerator.HasItems())
            {
                if (Breaker.IsBreakReceived())
                {
                    return;
                }

                ExecutorLauncher.WaitForAnyThreadToComplete();
                while (ExecutorLauncher.HasFreeThreads())
                {
                    if (Breaker.IsBreakReceived())
                    {
                        return;
                    }

                    if (!enumerator.HasItems())
                    {
                        break;
                    }

                    RunData data = enumerator.PeekNext();
                    if (null == data)
                    {
                        // if peeked all items till the end, but none of them was valid to launch a thread (due to concurrent groups)
                        break;
                    }

                    if (ExecutorLauncher.LaunchExecutor(data))
                    {
                        // if launching succeded (group was valid)
                        enumerator.MoveNext();
                    }
                }
            }

            ExecutorLauncher.WaitForAll();
        }

        public void WriteTrx()
        {
            if (Breaker.IsBreakReceived())
            {
                return;
            }

            IList<RunData> items = RunDataListBuilder.GetFull();
            IList<ResultFile> results = Collector.Collect(items);

            if (results.Count == 0)
            {
                Console.WriteLine("No results where generated, nothing to merge to the output trx");
                return;
            }

            using (Stream stream = TrxWriter.OpenResultFile(Args))
            {
                // Console.WriteLine("Results File: " + Args.Root + "\\" + Args.Output);
                if (TrxWriter.WriteFile(results, stream) == false)
                {
                    ResultCode = 3;
                }
            }
        }

        public void Clean()
        {
            if (Breaker.IsBreakReceived())
            {
                return;
            }

            IList<RunData> items = RunDataListBuilder.GetFull();
            Cleaner.Clean(items);
        }
    }
}