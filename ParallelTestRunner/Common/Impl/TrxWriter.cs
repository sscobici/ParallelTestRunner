using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ParallelTestRunner.Common.Trx;

namespace ParallelTestRunner.Common.Impl
{
    public class TrxWriter : ITrxWriter
    {
        private const string XmlNamespace = @"http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        
        private const string TestListId1 = "8c84fa94-04c1-424b-9868-57a2d4851a1d";
        
        private const string TestListId2 = "19431567-8539-422a-85d7-44ee4e166bda";
        
        public IWindowsFileHelper WindowsFileHelper { get; set; }
        
        public ISummaryCalculator SummaryCalculator { get; set; }

        public IStopwatch Stopwatch { get; set; }

        public void WriteFile(IList<ResultFile> files, Stream stream)
        {
            ResultSummary summary = SummaryCalculator.Calculate(files);
            
            Stopwatch.Stop();

            using (var writer = CreateDocument(stream))
            {
                WriteBody(writer, summary, files);
                WriteResults(writer, files);
                WriteDefinitions(writer, files);
                WriteEntries(writer, files);
                WriteTestLists(writer);
                WriteSummaryAndEnd(writer, summary, files);
            }

            Console.WriteLine();
            Console.WriteLine("Total tests: {0}. Passed: {1}. Failed: {2}. Skipped: {3}.", summary.Total, summary.Passed, summary.Failed, summary.Total - summary.Passed - summary.Failed);
            if (summary.Failed == 0)
            {
                Console.WriteLine("Test Run Succeded.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test Run Failed.");
                Console.ResetColor();
            }

            Console.WriteLine("Test execution time: {0}", GetReadableTimeSpan(Stopwatch.Elapsed()));
        }

        public Stream OpenResultFile(ITestRunnerArgs args)
        {
            return WindowsFileHelper.OpenResultFile(args);
        }

        private string GetReadableTimeSpan(TimeSpan value)
        {
            string duration;

            if (value.TotalMinutes < 1)
            {
                duration = value.Seconds + " Seconds";
            }
            else if (value.TotalHours < 1)
            {
                duration = value.Minutes + " Minutes, " + value.Seconds + " Seconds";
            }
            else if (value.TotalDays < 1)
            {
                duration = value.Hours + " Hours, " + value.Minutes + " Minutes";
            }
            else
            {
                duration = value.Days + " Days, " + value.Hours + " Hours";
            }

            if (duration.StartsWith("1 Seconds") || duration.EndsWith(" 1 Seconds"))
            {
                duration = duration.Replace("1 Seconds", "1 Second");
            }

            if (duration.StartsWith("1 Minutes") || duration.EndsWith(" 1 Minutes"))
            {
                duration = duration.Replace("1 Minutes", "1 Minute");
            }

            if (duration.StartsWith("1 Hours") || duration.EndsWith(" 1 Hours"))
            {
                duration = duration.Replace("1 Hours", "1 Hour");
            }

            if (duration.StartsWith("1 Days"))
            {
                duration = duration.Replace("1 Days", "1 Day");
            }

            return duration;
        }

        private XmlWriter CreateDocument(Stream stream)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
            };

            XmlWriter writer = XmlWriter.Create(stream, settings);
            writer.WriteStartDocument();
            return writer;
        }

        private void WriteBody(XmlWriter writer, ResultSummary summary, IList<ResultFile> files)
        {
            // Start TestRun Element
            writer.WriteStartElement("TestRun", XmlNamespace);
            writer.WriteAttributeString("id", Guid.NewGuid().ToString());
            writer.WriteAttributeString("name", summary.Name);
            writer.WriteAttributeString("runUser", summary.RunUser);

            string dateTimeFormat = "o";

            // Start Times Element
            writer.WriteStartElement("Times");
            writer.WriteAttributeString("creation", Stopwatch.GetStartDateTime().ToLocalTime().ToString(dateTimeFormat));
            writer.WriteAttributeString("queuing", Stopwatch.GetStartDateTime().ToLocalTime().ToString(dateTimeFormat));
            writer.WriteAttributeString("start", Stopwatch.GetStartDateTime().ToLocalTime().ToString(dateTimeFormat));
            writer.WriteAttributeString("finish", Stopwatch.GetStopDateTime().ToLocalTime().ToString(dateTimeFormat));
            writer.WriteEndElement(); // Times

            // Start TestSettings Element
            writer.WriteStartElement("TestSettings");
            writer.WriteAttributeString("name", "default");
            writer.WriteAttributeString("id", Guid.NewGuid().ToString());

            // Start Execution Element
            writer.WriteStartElement("Execution");
            writer.WriteStartElement("TestTypeSpecific");
            writer.WriteEndElement(); // TestTypeSpecific
            writer.WriteEndElement(); // Execution
            writer.WriteStartElement("Deployment");
            writer.WriteAttributeString("runDeploymentRoot", files.First().RunDeploymentRoot);
            writer.WriteEndElement(); // Deployment
            writer.WriteStartElement("Properties");
            writer.WriteEndElement(); // Properties
            writer.WriteEndElement(); // TestSettings
        }

        private void WriteSummaryAndEnd(XmlWriter writer, ResultSummary summary, IList<ResultFile> trxFileList)
        {
            writer.WriteStartElement("ResultSummary");
            writer.WriteAttributeString("outcome", summary.Outcome);
            writer.WriteStartElement("Counters");
            writer.WriteAttributeString("total", summary.Total.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("executed", summary.Executed.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("passed", summary.Passed.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("failed", summary.Failed.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("error", summary.Error.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("timeout", summary.Timeout.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("aborted", summary.Aborted.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("inconclusive", summary.Inconclusive.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("passedButRunAborted", summary.PassedButRunAborted.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("notRunnable", summary.NotRunnable.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("notExecuted", summary.NotExecuted.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("disconnected", summary.Disconnected.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("warning", summary.Warning.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("completed", summary.Completed.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("inProgress", summary.InProgress.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("pending", summary.Pending.ToString(CultureInfo.InvariantCulture));

            // writer.WriteAttributeString("duration", duration.ToString());
            writer.WriteEndElement(); // Counters
            writer.WriteStartElement("Output");
            foreach (var trxFile in trxFileList)
            {
                if (trxFile.StdOut.Any())
                {
                    foreach (var stdOut in trxFile.StdOut)
                    {
                            writer.WriteElementString("StdOut", stdOut);
                    }
                }
            }

            writer.WriteEndElement(); // Output
            writer.WriteEndElement(); // ResultSummary
            writer.WriteEndElement(); // TestRun
            writer.WriteEndDocument();
        }

        private void WriteEntries(XmlWriter writer, IList<ResultFile> trxFileList)
        {
            writer.WriteStartElement("TestEntries");
            foreach (ResultFile trxFile in trxFileList)
            {
                foreach (TestResult result in trxFile.Results)
                {
                    writer.WriteStartElement("TestEntry");
                    writer.WriteAttributeString("testId", result.TestId.ToString());
                    writer.WriteAttributeString("executionId", result.ExecutionId.ToString());
                    writer.WriteAttributeString("testListId", result.TestListId.ToString());
                    writer.WriteEndElement(); // TestEntry
                }
            }

            writer.WriteEndElement(); // TestEntries
        }

        private void WriteTestLists(XmlWriter writer)
        {
            writer.WriteStartElement("TestLists");
            writer.WriteStartElement("TestList");
            writer.WriteAttributeString("name", "Results Not in a List");
            writer.WriteAttributeString("id", TestListId1);
            writer.WriteEndElement(); // TestList
            writer.WriteStartElement("TestList");
            writer.WriteAttributeString("name", "All Loaded Results");
            writer.WriteAttributeString("id", TestListId2);
            writer.WriteEndElement(); // TestList
            writer.WriteEndElement(); // TestLists
        }

        private void WriteResults(XmlWriter writer, IList<ResultFile> trxFileList)
        {
            writer.WriteStartElement("Results");
            foreach (ResultFile trxFile in trxFileList)
            {
                foreach (TestResult result in trxFile.Results)
                {
                    writer.WriteStartElement("UnitTestResult");
                    writer.WriteAttributeString("executionId", result.ExecutionId.ToString());
                    writer.WriteAttributeString("testId", result.TestId.ToString());
                    writer.WriteAttributeString("testName", result.TestName);
                    writer.WriteAttributeString("computerName", result.ComputerName);
                    if (!string.IsNullOrEmpty(result.Duration))
                    {
                        writer.WriteAttributeString("duration", result.Duration);
                    }

                    writer.WriteAttributeString("startTime", result.StartTime);
                    writer.WriteAttributeString("endTime", result.EndTime);
                    writer.WriteAttributeString("testType", "13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b");
                    writer.WriteAttributeString("outcome", result.Outcome);
                    writer.WriteAttributeString("testListId", result.TestListId.ToString());
                    writer.WriteAttributeString("relativeResultsDirectory", result.RelativeResultDirectory.ToString());

                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        writer.WriteStartElement("Output");
                        writer.WriteStartElement("ErrorInfo");
                        writer.WriteElementString("Message", result.ErrorMessage);
                        writer.WriteElementString("StackTrace", result.StackTrace);
                        writer.WriteEndElement(); // ErrorInfo
                        writer.WriteEndElement(); // Output
                    }

                    writer.WriteEndElement(); // UnitTestResult
                }
            }

            writer.WriteEndElement(); // Results
        }

        private void WriteDefinitions(XmlWriter writer, IList<ResultFile> trxFileList)
        {
            writer.WriteStartElement("TestDefinitions");
            foreach (ResultFile trxFile in trxFileList)
            {
                foreach (TestResult result in trxFile.Results)
                {
                    writer.WriteStartElement("UnitTest");
                    writer.WriteAttributeString("name", result.TestName);
                    writer.WriteAttributeString("storage", result.Storage);
                    writer.WriteAttributeString("id", result.TestId.ToString());
                    writer.WriteStartElement("Execution");
                    writer.WriteAttributeString("id", result.ExecutionId.ToString());
                    writer.WriteEndElement(); // Execution
                    writer.WriteStartElement("TestMethod");
                    writer.WriteAttributeString("codeBase", result.CodeBase);
                    writer.WriteAttributeString("adapterTypeName", result.AdapterTypeName);
                    writer.WriteAttributeString("className", result.ClassName);
                    writer.WriteAttributeString("name", result.TestName);
                    writer.WriteEndElement(); // TestMethod
                    writer.WriteEndElement(); // UnitTest
                }
            }

            writer.WriteEndElement(); // TestDefinitions
        }
    }
}