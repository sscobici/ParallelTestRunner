using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Impl;

namespace ParallelTestRunner.Tests.Common
{
    [TestClass]
    public class ExecutorLauncherTest : TestBase
    {
        private ExecutorLauncherImpl target;
        private IThreadFactory threadFactory;
        private ITestRunnerArgs args;
        private IExecutor executor;
        private IList<IExecutorThread> threads;
        private IExecutorThread thread;

        [TestInitialize]
        public void SetUp()
        {
            target = new ExecutorLauncherImpl();
            threadFactory = Stub<IThreadFactory>();
            args = Stub<ITestRunnerArgs>();
            executor = Stub<IExecutor>();
            threads = Stub<IList<IExecutorThread>>();
            thread = Stub<IExecutorThread>();

            target.ThreadFactory = threadFactory;
            target.Args = args;
            target.Executor = executor;
            target.Threads = threads;
        }

        [TestMethod]
        public void LaunchExecutor_CanTestIsTrue()
        {
            RunData input = new RunData();
            threadFactory.Expect((m) => m.CanLaunch(threads, input)).Return(true);
            threadFactory.Expect((m) => m.Create(executor, input, target)).Return(thread);
            thread.Expect((m) => m.Launch(target));

            bool actual = VerifyTarget(() => target.LaunchExecutor(input));
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void LaunchExecutor_CanTestIsFalse()
        {
            RunData input = new RunData();
            threadFactory.Expect((m) => m.CanLaunch(threads, input)).Return(false);

            bool actual = VerifyTarget(() => target.LaunchExecutor(input));
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void WaitForAnyThreadToComplete()
        {
            Task[] tasks = new Task[10];
            threadFactory.Expect((m) => m.GetTaskArray(threads)).Return(tasks);
            threadFactory.Expect((m) => m.WaitAny(tasks));

            VerifyTarget(() => target.WaitForAnyThreadToComplete());
        }

        [TestMethod]
        public void WaitForAll()
        {
            Task[] tasks = new Task[10];
            threadFactory.Expect((m) => m.GetTaskArray(threads)).Return(tasks);
            threadFactory.Expect((m) => m.WaitAll(tasks));

            VerifyTarget(() => target.WaitForAll());
        }

        [TestMethod]
        public void HasFreeThreads_Yes()
        {
            threads.Expect((m) => m.Count).Return(3);
            args.Expect((m) => m.ThreadCount).Return(5);

            bool actual = VerifyTarget(() => target.HasFreeThreads());
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void HasFreeThreads_No_Same()
        {
            threads.Expect((m) => m.Count).Return(5);
            args.Expect((m) => m.ThreadCount).Return(5);

            bool actual = VerifyTarget(() => target.HasFreeThreads());
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void HasFreeThreads_No()
        {
            threads.Expect((m) => m.Count).Return(6);
            args.Expect((m) => m.ThreadCount).Return(5);

            bool actual = VerifyTarget(() => target.HasFreeThreads());
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void OnStarted()
        {
            threads.Expect((m) => m.Add(thread));
            VerifyTarget(() => target.OnStarted(thread));
        }

        [TestMethod]
        public void OnEnded()
        {
            threads.Expect((m) => m.Remove(thread)).Return(true);
            VerifyTarget(() => target.OnEnded(thread));
        }
    }
}