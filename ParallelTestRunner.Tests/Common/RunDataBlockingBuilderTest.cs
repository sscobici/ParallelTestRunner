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
    public class RunDataBlockingBuilderTest : TestBase
    {
        private ITestRunnerArgs args;
        private TestAssembly assembly;
        private RunDataBlockingBuilderImpl target;

        [TestInitialize]
        public void SetUp()
        {
            args = Stub<ITestRunnerArgs>();
            target = new RunDataBlockingBuilderImpl();
            assembly = new TestAssembly()
            {
                Name = "THE_ASSEMBLY_PATH",
                Fixtures = new List<TestFixture>()
                {
                    new TestFixture() { Name = "Fixture2", Group = "groupA", Exclusive = false },
                    new TestFixture() { Name = "Fixture3", Group = null, Exclusive = false },
                    new TestFixture() { Name = "Fixture3A", Group = null, Exclusive = false },
                    new TestFixture() { Name = "Fixture1", Group = null, Exclusive = false },
                    new TestFixture() { Name = "Fixture3B", Group = "groupB", Exclusive = false },
                    new TestFixture() { Name = "Fixture4", Group = null, Exclusive = true },
                    new TestFixture() { Name = "Fixture2A", Group = null, Exclusive = true },
                    new TestFixture() { Name = "Fixture2B", Group = null, Exclusive = false }
                }
            };
        }

        [TestMethod]
        public void Create()
        {
            args.Expect((m) => m.GetExecutablePath()).Return("PATH.EXE").Repeat.AtLeastOnce();
            args.Expect((m) => m.Root).Return("ROOT").Repeat.AtLeastOnce();

            IList<RunData> actual = VerifyTarget(() => target.Create(assembly, args));
            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);

            RunData fixture1 = actual[0];
            Assert.AreEqual("THE_ASSEMBLY_PATH", fixture1.AssemblyName);
            Assert.IsNotNull(fixture1.Fixtures);
            Assert.AreEqual(1, fixture1.Fixtures.Count);
            Assert.AreEqual("ROOT", fixture1.Root);
            Assert.IsNotNull(fixture1.Output);
            Assert.IsFalse(fixture1.Exclusive);
            Assert.AreEqual("PATH.EXE", fixture1.Executable);
            Assert.AreNotEqual(Guid.Empty, fixture1.RunId);

            RunData fixture2 = actual[1];
            Assert.AreEqual("THE_ASSEMBLY_PATH", fixture2.AssemblyName);
            Assert.IsNotNull(fixture2.Fixtures);
            Assert.AreEqual(3, fixture2.Fixtures.Count);
            Assert.AreEqual("ROOT", fixture2.Root);
            Assert.IsNotNull(fixture2.Output);
            Assert.IsTrue(fixture2.Exclusive);
            Assert.AreEqual("PATH.EXE", fixture2.Executable);
            Assert.AreNotEqual(Guid.Empty, fixture2.RunId);

            RunData fixture3 = actual[2];
            Assert.AreEqual("THE_ASSEMBLY_PATH", fixture3.AssemblyName);
            Assert.IsNotNull(fixture3.Fixtures);
            Assert.AreEqual(3, fixture3.Fixtures.Count);
            Assert.AreEqual("ROOT", fixture3.Root);
            Assert.IsNotNull(fixture3.Output);
            Assert.IsFalse(fixture3.Exclusive);
            Assert.AreEqual("PATH.EXE", fixture3.Executable);
            Assert.AreNotEqual(Guid.Empty, fixture3.RunId);

            RunData fixture4 = actual[3];
            Assert.AreEqual("THE_ASSEMBLY_PATH", fixture4.AssemblyName);
            Assert.IsNotNull(fixture4.Fixtures);
            Assert.AreEqual(1, fixture4.Fixtures.Count);
            Assert.AreEqual("ROOT", fixture4.Root);
            Assert.IsNotNull(fixture4.Output);
            Assert.IsTrue(fixture4.Exclusive);
            Assert.AreEqual("PATH.EXE", fixture4.Executable);
            Assert.AreNotEqual(Guid.Empty, fixture4.RunId);
        }
    }
}