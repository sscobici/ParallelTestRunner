using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ParallelTestRunner.Common.Impl;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.Tests.Common
{
    [TestClass]
    public class SummaryCalculatorTest : TestBase
    {
        private IList<ResultFile> files;
        private SummaryCalculatorImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new SummaryCalculatorImpl();
            files = new List<ResultFile>()
            {
                new ResultFile()
                {
                    Summary = new ResultSummary()
                    {
                        Outcome = "Passed",
                        Total = 1,
                        Executed = 2,
                        Passed = 3,
                        Failed = 4,
                        Error = 5,
                        Timeout = 6,
                        Aborted = 7,
                        Inconclusive = 8,
                        PassedButRunAborted = 9,
                        NotRunnable = 10,
                        NotExecuted = 20,
                        Disconnected = 30,
                        Warning = 40,
                        Completed = 50,
                        InProgress = 60,
                        Pending = 70,
                        StartTime = new DateTime(2001, 1, 21),
                        FinishTime = new DateTime(2001, 1, 22),
                    }
                },
                new ResultFile()
                {
                    Summary = new ResultSummary()
                    {
                        Outcome = "Failed",
                        Total = 10,
                        Executed = 20,
                        Passed = 30,
                        Failed = 40,
                        Error = 50,
                        Timeout = 60,
                        Aborted = 70,
                        Inconclusive = 80,
                        PassedButRunAborted = 90,
                        NotRunnable = 100,
                        NotExecuted = 200,
                        Disconnected = 300,
                        Warning = 400,
                        Completed = 500,
                        InProgress = 600,
                        Pending = 700,
                        StartTime = new DateTime(2002, 1, 21),
                        FinishTime = new DateTime(2002, 1, 22),
                    }
                },
                new ResultFile()
                {
                    Summary = new ResultSummary()
                    {
                        Outcome = "Passed",
                        Total = 100,
                        Executed = 200,
                        Passed = 300,
                        Failed = 400,
                        Error = 500,
                        Timeout = 600,
                        Aborted = 700,
                        Inconclusive = 800,
                        PassedButRunAborted = 900,
                        NotRunnable = 1000,
                        NotExecuted = 2000,
                        Disconnected = 3000,
                        Warning = 4000,
                        Completed = 5000,
                        InProgress = 6000,
                        Pending = 7000,
                        StartTime = new DateTime(2003, 1, 21),
                        FinishTime = new DateTime(2003, 1, 22),
                    }
                }
            };
        }

        [TestMethod]
        public void Calculate()
        {
            ResultSummary summary = target.Calculate(files);
            Assert.AreEqual(111, summary.Total);
            Assert.AreEqual(222, summary.Executed);
            Assert.AreEqual(333, summary.Passed);
            Assert.AreEqual(444, summary.Failed);
            Assert.AreEqual(555, summary.Error);
            Assert.AreEqual(666, summary.Timeout);
            Assert.AreEqual(777, summary.Aborted);
            Assert.AreEqual(888, summary.Inconclusive);
            Assert.AreEqual(999, summary.PassedButRunAborted);
            Assert.AreEqual(1110, summary.NotRunnable);
            Assert.AreEqual(2220, summary.NotExecuted);
            Assert.AreEqual(3330, summary.Disconnected);
            Assert.AreEqual(4440, summary.Warning);
            Assert.AreEqual(5550, summary.Completed);
            Assert.AreEqual(6660, summary.InProgress);
            Assert.AreEqual(7770, summary.Pending);
            Assert.AreEqual("Failed", summary.Outcome);
            Assert.AreEqual(new DateTime(2001, 1, 21), summary.StartTime);
            Assert.AreEqual(new DateTime(2003, 1, 22), summary.FinishTime);
        }
    }
}