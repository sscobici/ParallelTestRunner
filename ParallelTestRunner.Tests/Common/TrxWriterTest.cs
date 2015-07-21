using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Impl;
using ParallelTestRunner.Common.Trx;
using ParallelTestRunner.VSTest.Impl;

namespace ParallelTestRunner.Tests.Common
{
    public class TrxWriterTest : TestBase
    {
        private IList<ResultFile> files;
        private TrxWriter target;
        private TrxParser parser;
        private IWindowsFileHelper windowsFileHelper;

        [TestInitialize]
        public void SetUp()
        {
            target = new TrxWriter();
            parser = new TrxParser();
            target.SummaryCalculator = new SummaryCalculatorImpl();
            windowsFileHelper = Stub<IWindowsFileHelper>();
            target.WindowsFileHelper = windowsFileHelper;
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
                    },
                    Results = new List<TestResult>()
                    {
                        new TestResult() { TestName = "TEST_A1", ClassName = "CLASS_A1", Duration = "DURATION_A1", Outcome = "OUTCOME_A1", TestId = Guid.NewGuid() },
                        new TestResult() { TestName = "TEST_A2", ClassName = "CLASS_A2", Duration = "DURATION_A2", Outcome = "OUTCOME_A2", TestId = Guid.NewGuid(), ErrorMessage = "ERROR_A2", StackTrace = "STACK_TRACE_A2" },
                        new TestResult() { TestName = "TEST_A3", ClassName = "CLASS_A3", Duration = "DURATION_A3", Outcome = "OUTCOME_A3", TestId = Guid.NewGuid() },
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
                    },
                    Results = new List<TestResult>()
                    {
                        new TestResult() { TestName = "TEST_B1", ClassName = "CLASS_B1", Duration = "DURATION_B1", Outcome = "OUTCOME_B1", TestId = Guid.NewGuid() },
                        new TestResult() { TestName = "TEST_B2", ClassName = "CLASS_B2", Duration = "DURATION_B2", Outcome = "OUTCOME_B2", TestId = Guid.NewGuid(), ErrorMessage = "ERROR_B2", StackTrace = "STACK_TRACE_B2" },
                        new TestResult() { TestName = "TEST_B3", ClassName = "CLASS_B3", Duration = "DURATION_B3", Outcome = "OUTCOME_B3", TestId = Guid.NewGuid() },
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
                    },
                    Results = new List<TestResult>()
                    {
                        new TestResult() { TestName = "TEST_C1", ClassName = "CLASS_C1", Duration = "DURATION_C1", Outcome = "OUTCOME_C1", TestId = Guid.NewGuid() },
                        new TestResult() { TestName = "TEST_C2", ClassName = "CLASS_C2", Duration = "DURATION_C2", Outcome = "OUTCOME_C2", TestId = Guid.NewGuid(), ErrorMessage = "ERROR_C2", StackTrace = "STACK_TRACE_C2" },
                        new TestResult() { TestName = "TEST_C3", ClassName = "CLASS_C3", Duration = "DURATION_C3", Outcome = "OUTCOME_C3", TestId = Guid.NewGuid() },
                    }
                }
            };
        }

        [TestMethod]
        public void WriteFile()
        {
            Stream stream = new MemoryStream();
            bool haveFailedTests = target.WriteFile(files, stream);

            Assert.IsTrue(haveFailedTests);

            stream.Position = 0;
            ResultFile result = parser.Parse(stream);

            Assert.AreEqual(111, result.Summary.Total);
            Assert.AreEqual(222, result.Summary.Executed);
            Assert.AreEqual(333, result.Summary.Passed);
            Assert.AreEqual(444, result.Summary.Failed);
            Assert.AreEqual(555, result.Summary.Error);
            Assert.AreEqual(666, result.Summary.Timeout);
            Assert.AreEqual(777, result.Summary.Aborted);
            Assert.AreEqual(888, result.Summary.Inconclusive);
            Assert.AreEqual(999, result.Summary.PassedButRunAborted);
            Assert.AreEqual(1110, result.Summary.NotRunnable);
            Assert.AreEqual(2220, result.Summary.NotExecuted);
            Assert.AreEqual(3330, result.Summary.Disconnected);
            Assert.AreEqual(4440, result.Summary.Warning);
            Assert.AreEqual(5550, result.Summary.Completed);
            Assert.AreEqual(6660, result.Summary.InProgress);
            Assert.AreEqual(7770, result.Summary.Pending);
            Assert.AreEqual("Failed", result.Summary.Outcome);
            Assert.AreEqual(new DateTime(2001, 1, 21), result.Summary.StartTime);
            Assert.AreEqual(new DateTime(2003, 1, 22), result.Summary.FinishTime);

            Assert.IsNotNull(result.Results);
            Assert.AreEqual(9, result.Results.Count);

            TestResult item;
            item = result.Results[0];
            Assert.AreEqual("TEST_A1", item.TestName);
            Assert.AreEqual("CLASS_A1", item.ClassName);
            Assert.AreEqual("DURATION_A1", item.Duration);
            Assert.AreEqual("OUTCOME_A1", item.Outcome);
            Assert.IsNull(item.ErrorMessage);
            Assert.IsNull(item.StackTrace);

            item = result.Results[1];
            Assert.AreEqual("TEST_A2", item.TestName);
            Assert.AreEqual("CLASS_A2", item.ClassName);
            Assert.AreEqual("DURATION_A2", item.Duration);
            Assert.AreEqual("OUTCOME_A2", item.Outcome);
            Assert.AreEqual("ERROR_A2", item.ErrorMessage);
            Assert.AreEqual("STACK_TRACE_A2", item.StackTrace);

            item = result.Results[2];
            Assert.AreEqual("TEST_A3", item.TestName);
            Assert.AreEqual("CLASS_A3", item.ClassName);
            Assert.AreEqual("DURATION_A3", item.Duration);
            Assert.AreEqual("OUTCOME_A3", item.Outcome);
            Assert.IsNull(item.ErrorMessage);
            Assert.IsNull(item.StackTrace);

            item = result.Results[3];
            Assert.AreEqual("TEST_B1", item.TestName);
            Assert.AreEqual("CLASS_B1", item.ClassName);
            Assert.AreEqual("DURATION_B1", item.Duration);
            Assert.AreEqual("OUTCOME_B1", item.Outcome);
            Assert.IsNull(item.ErrorMessage);
            Assert.IsNull(item.StackTrace);

            item = result.Results[4];
            Assert.AreEqual("TEST_B2", item.TestName);
            Assert.AreEqual("CLASS_B2", item.ClassName);
            Assert.AreEqual("DURATION_B2", item.Duration);
            Assert.AreEqual("OUTCOME_B2", item.Outcome);
            Assert.AreEqual("ERROR_B2", item.ErrorMessage);
            Assert.AreEqual("STACK_TRACE_B2", item.StackTrace);

            item = result.Results[5];
            Assert.AreEqual("TEST_B3", item.TestName);
            Assert.AreEqual("CLASS_B3", item.ClassName);
            Assert.AreEqual("DURATION_B3", item.Duration);
            Assert.AreEqual("OUTCOME_B3", item.Outcome);
            Assert.IsNull(item.ErrorMessage);
            Assert.IsNull(item.StackTrace);

            item = result.Results[6];
            Assert.AreEqual("TEST_C1", item.TestName);
            Assert.AreEqual("CLASS_C1", item.ClassName);
            Assert.AreEqual("DURATION_C1", item.Duration);
            Assert.AreEqual("OUTCOME_C1", item.Outcome);
            Assert.IsNull(item.ErrorMessage);
            Assert.IsNull(item.StackTrace);

            item = result.Results[7];
            Assert.AreEqual("TEST_C2", item.TestName);
            Assert.AreEqual("CLASS_C2", item.ClassName);
            Assert.AreEqual("DURATION_C2", item.Duration);
            Assert.AreEqual("OUTCOME_C2", item.Outcome);
            Assert.AreEqual("ERROR_C2", item.ErrorMessage);
            Assert.AreEqual("STACK_TRACE_C2", item.StackTrace);

            item = result.Results[8];
            Assert.AreEqual("TEST_C3", item.TestName);
            Assert.AreEqual("CLASS_C3", item.ClassName);
            Assert.AreEqual("DURATION_C3", item.Duration);
            Assert.AreEqual("OUTCOME_C3", item.Outcome);
            Assert.IsNull(item.ErrorMessage);
            Assert.IsNull(item.StackTrace);
        }

        [TestMethod]
        public void OpenResultFile()
        {
            ITestRunnerArgs args = Stub<ITestRunnerArgs>();
            Stream expected = new MemoryStream();
            windowsFileHelper.Expect((m) => m.OpenResultFile(args)).Return(expected);
            Stream actual = VerifyTarget(() => target.OpenResultFile(args));
            Assert.AreEqual(expected, actual);
        }
    }
}