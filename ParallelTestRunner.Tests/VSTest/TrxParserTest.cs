using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using ParallelTestRunner.Common.Trx;
using ParallelTestRunner.VSTest;
using ParallelTestRunner.VSTest.Impl;

namespace ParallelTestRunner.Tests.VSTest
{
    [TestClass]
    public class TrxParserTest : TestBase
    {
        private const string ErrorMessage = @"Assert.AreEqual failed. Expected:<3>. Actual:<2>. ";
        private const string StackTrace = "   at UITests.Class9.Class9TestMethod3() in d:\\work\\ParTests\\UnitTestRunner\\UITests\\Class9.cs:line 28\n";
        private const string TestName = @"Class9TestMethod1";
        private const string Duration = @"00:00:01.0048184";
        private const string ClassName = @"UITests.Class9";
        private readonly DateTime start = DateTime.Parse("2015-06-15T15:49:10.4703360+03:00");
        private readonly DateTime finish = DateTime.Parse("2015-06-15T15:49:14.4629318+03:00");
        private readonly Guid unitTest1Id = Guid.Parse("1adc189d-87ce-608e-4b5f-e31dcf2191d4");
        private ITrxParser target;

        [TestInitialize]
        public void SetUp()
        {
            target = new TrxParser();
        }

        [TestMethod]
        public void Parse()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ParallelTestRunner.Tests.Resources.testXml.trx");
            ResultFile file = target.Parse(stream);

            Assert.AreEqual(start, file.Summary.StartTime);
            Assert.AreEqual(finish, file.Summary.FinishTime);

            Assert.AreEqual(2, file.Results.Count);
            Assert.AreEqual(unitTest1Id, file.Results[0].TestId);
            Assert.AreEqual(TestName, file.Results[0].TestName);
            Assert.AreEqual(Duration, file.Results[0].Duration);
            Assert.AreEqual(ClassName, file.Results[0].ClassName);
            Assert.AreEqual(ErrorMessage, file.Results[1].ErrorMessage);
            Assert.AreEqual(StackTrace, file.Results[1].StackTrace);
            Assert.AreEqual("Passed", file.Results[0].Outcome);
            Assert.AreEqual("Failed", file.Results[1].Outcome);

            Assert.AreEqual(1, file.Summary.Total);
            Assert.AreEqual(2, file.Summary.Executed);
            Assert.AreEqual(3, file.Summary.Passed);
            Assert.AreEqual(4, file.Summary.Failed);
            Assert.AreEqual(5, file.Summary.Error);
            Assert.AreEqual(6, file.Summary.Timeout);
            Assert.AreEqual(7, file.Summary.Aborted);
            Assert.AreEqual(8, file.Summary.Inconclusive);
            Assert.AreEqual(9, file.Summary.PassedButRunAborted);
            Assert.AreEqual(10, file.Summary.NotRunnable);
            Assert.AreEqual(11, file.Summary.NotExecuted);
            Assert.AreEqual(12, file.Summary.Disconnected);
            Assert.AreEqual(13, file.Summary.Warning);
            Assert.AreEqual(14, file.Summary.Completed);
            Assert.AreEqual(15, file.Summary.InProgress);
            Assert.AreEqual(16, file.Summary.Pending);
            Assert.AreEqual(1, file.StdOut.Count);
        }
    }
}
