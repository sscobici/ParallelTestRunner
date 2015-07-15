using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ParallelTestRunner.Common;
using System.Collections.Generic;
using System.Diagnostics;
using ParallelTestRunner.VSTest.Common.Impl;

namespace ParallelTestRunner.Tests.VSTest.Common
{
    [TestClass]
    public class ProcessStartInfoFactoryTest : TestBase
    {
        private ProcessStartInfoFactoryImpl target;
        private RunData input;

        [TestInitialize]
        public void SetUp()
        {
            target = new ProcessStartInfoFactoryImpl();
            input = new RunData()
            {
                Executable = "Executable.exe",
                AssemblyName = "AssemblyName.dll",
                Fixtures = new List<TestFixture>()
                {
                    new TestFixture() { Name = "Fixture1" },
                    new TestFixture() { Name = "Fixture2" },
                    new TestFixture() { Name = "Fixture3" }
                },
                Root = "ROOT",
                RunId = Guid.NewGuid()
            };
        }

        [TestMethod]
        public void CreateProcessStartInfo()
        {
            ProcessStartInfo actual = target.CreateProcessStartInfo(input);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.CreateNoWindow);
            Assert.IsTrue(actual.RedirectStandardOutput);
            Assert.IsFalse(actual.UseShellExecute);
            Assert.AreEqual(input.Executable, actual.FileName);
            Assert.AreEqual(
                "\"" + input.AssemblyName + "\"" +
                 " \"/settings:" + string.Concat(input.Root, "\\", input.RunId, ".settings\"") +
                 " /logger:trx" +
                 " /Tests:Fixture1,Fixture2,Fixture3",
                 actual.Arguments);
        }
    }
}