using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Threading.Tasks;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Impl;

namespace ParallelTestRunner.Tests.Common
{
    [TestClass]
    public class ExecutorThreadTest : TestBase
    {
        private IExecutorThread target;
        private IExecutor executor;
        private RunData runData;
        private IThreadEnder ender;

        [TestInitialize]
        public void SetUp()
        {
            executor = Stub<IExecutor>();
            runData = new RunData();
            ender = Stub<IThreadEnder>();
            target = new ExecutorThreadImpl(executor, runData);
        }

        [TestMethod]
        public void Launch()
        {
            using (Ordered())
            {
                executor.Expect((m) => m.Run(runData));
                ender.Expect((m) => m.OnEnded(target));
            }

            ReplayAll();
            target.Launch(ender);
            Task task = target.GetTask();
            task.Wait();
            System.Threading.Thread.Sleep(100);
            VerifyAll();
        }

        [TestMethod]
        public void GetRunData()
        {
            RunData actual = VerifyTarget(() => target.GetRunData());
            Assert.AreEqual(runData, actual);
        }
    }
}