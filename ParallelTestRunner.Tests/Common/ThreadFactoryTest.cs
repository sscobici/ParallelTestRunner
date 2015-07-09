using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Impl;

namespace ParallelTestRunner.Tests.Common
{
    [TestClass]
    public class ThreadFactoryTest : TestBase
    {
        private ThreadFactoryImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new ThreadFactoryImpl();
        }

        [TestMethod]
        public void Create()
        {
            IExecutor executor = Stub<IExecutor>();
            IThreadStarter starter = Stub<IThreadStarter>();
            RunData data = new RunData();

            starter.Expect((m) => m.OnStarted(Arg<IExecutorThread>.Is.NotNull));

            IExecutorThread thread = VerifyTarget(() => target.Create(executor, data, starter));
            Assert.AreEqual(data, thread.GetRunData());
            Assert.AreEqual(executor, thread.GetExecutor());
            Assert.IsNull(thread.GetTask());
        }

        [TestMethod]
        public void GetTaskArray()
        {
            IExecutorThread a = Stub<IExecutorThread>();
            IExecutorThread b = Stub<IExecutorThread>();
            IExecutorThread c = Stub<IExecutorThread>();
            IList<IExecutorThread> input = new List<IExecutorThread>() { a, b, c };

            Task taskA = new Task(() => Console.WriteLine("TASK_A"));
            Task taskB = new Task(() => Console.WriteLine("TASK_B"));
            Task taskC = new Task(() => Console.WriteLine("TASK_C"));

            a.Expect((m) => m.GetTask()).Return(taskA);
            b.Expect((m) => m.GetTask()).Return(taskB);
            c.Expect((m) => m.GetTask()).Return(taskC);

            Task[] tasks = VerifyTarget(() => target.GetTaskArray(input));
            Assert.AreEqual(taskA, tasks[0]);
            Assert.AreEqual(taskB, tasks[1]);
            Assert.AreEqual(taskC, tasks[2]);
        }

        [TestMethod]
        public void WaitAny()
        {
            target.WaitAny(new Task[0]);
        }

        [TestMethod]
        public void WaitAll()
        {
            target.WaitAll(new Task[0]);
        }

        [TestMethod]
        public void CanLaunch_NoThreads()
        {
            IList<IExecutorThread> threads = new List<IExecutorThread>();
            RunData data = new RunData() { Groups = new HashSet<string>() { "group1" } };
            bool actual = target.CanLaunch(threads, data);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CanLaunch_NoGroups()
        {
            IList<IExecutorThread> threads = new List<IExecutorThread>() { Stub<IExecutorThread>() };
            RunData data = new RunData() { Groups = new HashSet<string>() };
            bool actual = target.CanLaunch(threads, data);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CanLaunch_2ThreadsWithoutGroups()
        {
            IExecutorThread threadA = Stub<IExecutorThread>();
            IExecutorThread threadB = Stub<IExecutorThread>();
            threadA.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() });
            threadB.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() });
            IList<IExecutorThread> threads = new List<IExecutorThread>() { threadA, threadB };
            RunData data = new RunData() { Groups = new HashSet<string>() { "Group1" } };

            bool actual = VerifyTarget(() => target.CanLaunch(threads, data));
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CanLaunch_2ThreadsWithOtherGroups()
        {
            IExecutorThread threadA = Stub<IExecutorThread>();
            IExecutorThread threadB = Stub<IExecutorThread>();
            threadA.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() { "Group2" } });
            threadB.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() { "Group3" } });
            IList<IExecutorThread> threads = new List<IExecutorThread>() { threadA, threadB };
            RunData data = new RunData() { Groups = new HashSet<string>() { "Group1" } };

            bool actual = VerifyTarget(() => target.CanLaunch(threads, data));
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CanLaunch_2ThreadsWithTheGroup()
        {
            IExecutorThread threadA = Stub<IExecutorThread>();
            IExecutorThread threadB = Stub<IExecutorThread>();
            threadA.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() { "Group2" } });
            threadB.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() { "Group1" } });
            IList<IExecutorThread> threads = new List<IExecutorThread>() { threadA, threadB };
            RunData data = new RunData() { Groups = new HashSet<string>() { "Group1" } };

            bool actual = VerifyTarget(() => target.CanLaunch(threads, data));
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void CanLaunch_2ThreadsWithOtherGroups_RunDataIsExclusive()
        {
            IExecutorThread threadA = Stub<IExecutorThread>();
            IExecutorThread threadB = Stub<IExecutorThread>();
            threadA.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() { "Group2" } });
            IList<IExecutorThread> threads = new List<IExecutorThread>() { threadA, threadB };
            RunData data = new RunData() { Groups = new HashSet<string>() { "Group1" }, Exclusive = true };

            bool actual = VerifyTarget(() => target.CanLaunch(threads, data));
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void CanLaunch_2ThreadsWithoutGroups_RunDataIsExclusive()
        {
            IExecutorThread threadA = Stub<IExecutorThread>();
            IExecutorThread threadB = Stub<IExecutorThread>();
            threadA.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() });
            threadB.Expect((m) => m.GetRunData()).Return(new RunData() { Groups = new HashSet<string>() });
            IList<IExecutorThread> threads = new List<IExecutorThread>() { threadA, threadB };
            RunData data = new RunData() { Groups = new HashSet<string>() { "Group1" }, Exclusive = true };

            bool actual = VerifyTarget(() => target.CanLaunch(threads, data));
            Assert.IsTrue(actual);
        }
    }
}