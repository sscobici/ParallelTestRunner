using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Trx;
using ParallelTestRunner.Impl;

namespace ParallelTestRunner.Tests
{
    [TestClass]
    public class TestRunnerTest : TestBase
    {
        private ITestRunnerArgs args;
        private IParser parser;
        private ICollector collector;
        private ICleaner cleaner;
        private IRunDataBuilder runDataBuilder;
        private IRunDataListBuilder runDataListBuilder;
        private IExecutorLauncher executorLauncher;
        private ITrxWriter trxWriter;
        private IBreaker breaker;
        private IRunDataEnumerator enumerator;
        private IWindowsFileHelper windowsFileHelper;
        private TestRunnerImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new TestRunnerImpl();
            args = Stub<ITestRunnerArgs>();
            parser = Stub<IParser>();
            cleaner = Stub<ICleaner>();
            runDataBuilder = Stub<IRunDataBuilder>();
            runDataListBuilder = Stub<IRunDataListBuilder>();
            executorLauncher = Stub<IExecutorLauncher>();
            trxWriter = Stub<ITrxWriter>();
            breaker = Stub<IBreaker>();
            enumerator = Stub<IRunDataEnumerator>();
            windowsFileHelper = Stub<IWindowsFileHelper>();
            collector = Stub<ICollector>();

            target.Args = args;
            target.Parser = parser;
            target.Cleaner = cleaner;
            target.RunDataBuilder = runDataBuilder;
            target.RunDataListBuilder = runDataListBuilder;
            target.ExecutorLauncher = executorLauncher;
            target.TrxWriter = trxWriter;
            target.Breaker = breaker;
            target.Collector = collector;
            target.WindowsFileHelper = windowsFileHelper;
        }

        [TestMethod]
        public void Execute_NoItemsAtAll()
        {
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAll());
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_Break()
        {
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(true);
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_NoBreak_NoFreeThreads_NoItems()
        {
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAnyThreadToComplete());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAll());
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_NoBreak_HasFreeThreads_Break()
        {
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAnyThreadToComplete());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(true);
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_NoBreak_HasFreeThreads_NoBreak_NoItems()
        {
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAnyThreadToComplete());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAll());
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_NoBreak_HasFreeThreads_NoBreak_HasItems_PeekNull_NoItems()
        {
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAnyThreadToComplete());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                enumerator.Expect((m) => m.PeekNext()).Return(null);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAll());
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_NoBreak_HasFreeThreads_NoBreak_HasItems_Peek_NoItems()
        {
            RunData data = new RunData();
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAnyThreadToComplete());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                enumerator.Expect((m) => m.PeekNext()).Return(null);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAll());
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_NoBreak_HasFreeThreads_NoBreak_HasItems_Peek_NoLaunch_NoFreeThreads_NoItems()
        {
            RunData data = new RunData();
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAnyThreadToComplete());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                enumerator.Expect((m) => m.PeekNext()).Return(data);
                executorLauncher.Expect((m) => m.LaunchExecutor(data)).Return(false);
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAll());
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Execute_HasItems_NoBreak_HasFreeThreads_NoBreak_HasItems_Peek_Launch_Move_NoFreeThreads_NoItems()
        {
            RunData data = new RunData();
            using (Ordered())
            {
                runDataListBuilder.Expect((m) => m.GetEnumerator()).Return(enumerator);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAnyThreadToComplete());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(true);
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(true);
                enumerator.Expect((m) => m.PeekNext()).Return(data);
                executorLauncher.Expect((m) => m.LaunchExecutor(data)).Return(true);
                enumerator.Expect((m) => m.MoveNext());
                executorLauncher.Expect((m) => m.HasFreeThreads()).Return(false);
                enumerator.Expect((m) => m.HasItems()).Return(false);
                executorLauncher.Expect((m) => m.WaitForAll());
            }

            VerifyTarget(() => target.Execute());
        }

        [TestMethod]
        public void Parse_Brake()
        {
            breaker.Expect((m) => m.IsBreakReceived()).Return(true);

            VerifyTarget(() => target.Parse());
        }

        [TestMethod]
        public void Parse_NoBrake()
        {
            IList<string> assemblyList = new[] { "abc.dll", "abb.dll" };
            Assembly assemblyAbc = Assembly.GetAssembly(typeof(int));
            Assembly assemblyAbb = Assembly.GetAssembly(typeof(ContainerBuilder));
            TestAssembly testAssemblyAbc = new TestAssembly();
            TestAssembly testAssemblyAbb = new TestAssembly();
            IList<RunData> itemsAbc = Stub<IList<RunData>>();
            IList<RunData> itemsAbb = Stub<IList<RunData>>();
            using (Ordered())
            {
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                args.Expect(m => m.AssemblyList).Return(assemblyList);

                windowsFileHelper.Expect((m) => m.GetAssembly(assemblyList[0])).Return(assemblyAbc);
                parser.Expect((m) => m.Parse(assemblyAbc)).Return(testAssemblyAbc);
                runDataBuilder.Expect(m => m.Create(testAssemblyAbc, args)).Return(itemsAbc);
                runDataListBuilder.Expect(m => m.Add(itemsAbc));

                windowsFileHelper.Expect((m) => m.GetAssembly(assemblyList[1])).Return(assemblyAbb);
                parser.Expect((m) => m.Parse(assemblyAbb)).Return(testAssemblyAbb);
                runDataBuilder.Expect(m => m.Create(testAssemblyAbb, args)).Return(itemsAbb);
                runDataListBuilder.Expect(m => m.Add(itemsAbb));
            }

            VerifyTarget(() => target.Parse());
        }

        [TestMethod]
        public void WriteTrx_Brake()
        {
            breaker.Expect((m) => m.IsBreakReceived()).Return(true);

            VerifyTarget(() => target.WriteTrx());
        }

        [TestMethod]
        public void WriteTrx_NoBrake()
        {
            IList<RunData> items = Stub<IList<RunData>>();
            IList<ResultFile> results = Stub<IList<ResultFile>>();
            Stream stream = Stub<Stream>();
            using (Ordered())
            {
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                runDataListBuilder.Expect(m => m.GetFull()).Return(items);
                collector.Expect(m => m.Collect(items)).Return(results);
                trxWriter.Expect(m => m.OpenResultFile(args)).Return(stream);
                trxWriter.Expect(m => m.WriteFile(results, stream));
            }

            VerifyTarget(() => target.WriteTrx());
        }

        [TestMethod]
        public void Clean_Brake()
        {
            breaker.Expect((m) => m.IsBreakReceived()).Return(true);

            VerifyTarget(() => target.Clean());
        }

        [TestMethod]
        public void Clean_NoBrake()
        {
            IList<RunData> items = Stub<IList<RunData>>();
            using (Ordered())
            {
                breaker.Expect((m) => m.IsBreakReceived()).Return(false);
                runDataListBuilder.Expect(m => m.GetFull()).Return(items);
                cleaner.Expect(m => m.Clean(items));
            }

            VerifyTarget(() => target.Clean());
        }
    }
}