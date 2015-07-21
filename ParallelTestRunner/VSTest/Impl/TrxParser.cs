using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.VSTest.Impl
{
    public class TrxParser : ITrxParser
    {
        public ResultFile Parse(Stream stream)
        {
            ResultFile trx = new ResultFile { Results = new List<TestResult>(), Summary = new ResultSummary(), StdOut = new List<string>() };

            using (XmlReader reader = XmlReader.Create(stream))
            {
                ExtractTestRunInfo(reader, trx);
                ExtractTimes(reader, trx);
                ExtractTestSettings(reader, trx);
                ExtractResults(reader, trx);
                ExtractDefinitions(reader, trx);
                ExtractSummary(reader, trx);
            }

            return trx;
        }

        private void ExtractTestRunInfo(XmlReader reader, ResultFile trx)
        {
            if (reader.ReadToFollowing("TestRun"))
            {
                trx.Summary.Name = reader.GetAttribute("name");
                trx.Summary.RunUser = reader.GetAttribute("runUser");
            }
        }

        private void ExtractTimes(XmlReader reader, ResultFile trx)
        {
            if (reader.ReadToFollowing("Times"))
            {
                trx.Summary.StartTime = DateTime.Parse(reader.GetAttribute("start"), CultureInfo.InvariantCulture);
                trx.Summary.FinishTime = DateTime.Parse(reader.GetAttribute("finish"), CultureInfo.InvariantCulture);
            }
        }

        private void ExtractTestSettings(XmlReader reader, ResultFile trx)
        {
            if (reader.ReadToFollowing("Deployment"))
            {
                trx.RunDeploymentRoot = reader.GetAttribute("runDeploymentRoot");
            }
        }

        private void ExtractSummary(XmlReader reader, ResultFile trx)
        {
            if (reader.ReadToFollowing("ResultSummary"))
            {
                ResultSummary item = trx.Summary;
                item.Outcome = reader.GetAttribute("outcome");
                if (reader.ReadToFollowing("Counters"))
                {
                    item.Total = int.Parse(reader.GetAttribute("total"));
                    item.Executed = int.Parse(reader.GetAttribute("executed"));
                    item.Passed = int.Parse(reader.GetAttribute("passed"));
                    item.Failed = int.Parse(reader.GetAttribute("failed"));
                    item.Error = int.Parse(reader.GetAttribute("error"));
                    item.Timeout = int.Parse(reader.GetAttribute("timeout"));
                    item.Aborted = int.Parse(reader.GetAttribute("aborted"));
                    item.Inconclusive = int.Parse(reader.GetAttribute("inconclusive"));
                    item.PassedButRunAborted =
                        int.Parse(reader.GetAttribute("passedButRunAborted"));
                    item.NotRunnable = int.Parse(reader.GetAttribute("notRunnable"));
                    item.NotExecuted = int.Parse(reader.GetAttribute("notExecuted"));
                    item.Disconnected = int.Parse(reader.GetAttribute("disconnected"));
                    item.Warning = int.Parse(reader.GetAttribute("warning"));
                    item.Completed = int.Parse(reader.GetAttribute("completed"));
                    item.InProgress = int.Parse(reader.GetAttribute("inProgress"));
                    item.Pending = int.Parse(reader.GetAttribute("pending"));
                }

                if (reader.ReadToFollowing("StdOut"))
                {
                    do
                    {
                        trx.StdOut.Add(reader.ReadElementContentAsString());
                    }
                    while (reader.ReadToNextSibling("StdOut"));
                }
            }
        }

        private void ExtractResults(XmlReader reader, ResultFile trx)
        {
            if (reader.ReadToFollowing("Results"))
            {
                if (reader.ReadToDescendant("UnitTestResult"))
                {
                    do
                    {
                        TestResult item = new TestResult();
                        item.ExecutionId = Guid.Parse(reader.GetAttribute("executionId"));
                        item.TestId = Guid.Parse(reader.GetAttribute("testId"));
                        item.TestName = reader.GetAttribute("testName");
                        item.Duration = reader.GetAttribute("duration");
                        item.Outcome = reader.GetAttribute("outcome");
                        item.TestListId = Guid.Parse(reader.GetAttribute("testListId"));
                        item.ComputerName = reader.GetAttribute("computerName");
                        item.StartTime = reader.GetAttribute("startTime");
                        item.EndTime = reader.GetAttribute("endTime");
                        item.RelativeResultDirectory = Guid.Parse(reader.GetAttribute("relativeResultsDirectory"));

                        using (XmlReader errorReader = reader.ReadSubtree())
                        {
                            if (errorReader.ReadToFollowing("Message"))
                            {
                                item.ErrorMessage = reader.ReadElementContentAsString();
                                errorReader.ReadToNextSibling("StackTrace");
                                item.StackTrace = errorReader.ReadElementContentAsString();
                            }
                        }

                        trx.Results.Add(item);
                    }
                    while (reader.ReadToNextSibling("UnitTestResult"));
                }
            }
        }

        private void ExtractDefinitions(XmlReader reader, ResultFile trx)
        {
            if (reader.ReadToFollowing("TestDefinitions"))
            {
                if (reader.ReadToDescendant("UnitTest"))
                {
                    do
                    {
                        var testId = Guid.Parse(reader.GetAttribute("id"));
                        var tempResult = trx.Results.First(result => result.TestId == testId);
                        tempResult.Storage = reader.GetAttribute("storage");

                        if (reader.ReadToFollowing("TestMethod"))
                        {
                            tempResult.CodeBase = reader.GetAttribute("codeBase");
                            tempResult.AdapterTypeName = reader.GetAttribute("adapterTypeName");
                            tempResult.ClassName = reader.GetAttribute("className");
                        }

                        reader.ReadToNextSibling("UnitTest");
                    }
                    while (reader.ReadToNextSibling("UnitTest"));
                }
            }
        }
    }
}