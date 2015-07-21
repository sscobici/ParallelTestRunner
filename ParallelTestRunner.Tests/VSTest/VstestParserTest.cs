using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Impl;
using Rhino.Mocks;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace ParallelTestRunner.Tests.VSTest
{
    [TestClass]
    public class VSTestParserTest : TestBase
    {
        private VSTestParserImpl target;
        private ITestRunnerArgs args;

        [TestInitialize]
        public void SetUp()
        {
            args = MockRepository.GenerateStub<ITestRunnerArgs>();
            target = new VSTestParserImpl()
            {
                Args = args
            };
        }

        [TestMethod]
        public void Parse_TestClass()
        {
            args.Stub(m => m.PLevel).Return(PLevel.TestClass);

            Assembly assembly = Assembly.GetExecutingAssembly();
            TestAssembly testAssembly = target.Parse(assembly);
            Assert.AreEqual(assembly.Location, testAssembly.Name);
          
            TestFixture testClass1 = testAssembly.Fixtures.FirstOrDefault(x => x.Name == "ParallelTestRunner.Tests.VSTest.TestClass1");
            Assert.IsNotNull(testClass1);
            Assert.AreEqual("Group2", testClass1.Group);
            Assert.IsFalse(testClass1.Exclusive.Value);

            TestFixture testClass2 = testAssembly.Fixtures.FirstOrDefault(x => x.Name == "ParallelTestRunner.Tests.VSTest.TestClass2");
            Assert.IsNotNull(testClass2);
            Assert.AreEqual("Group1", testClass2.Group);
            Assert.AreEqual(true, testClass2.Exclusive);
        }

        [TestMethod]
        public void Parse_TestMethod()
        {
            args.Stub(m => m.PLevel).Return(PLevel.TestMethod);

            Assembly assembly = Assembly.GetExecutingAssembly();
            TestAssembly testAssembly = target.Parse(assembly);
            Assert.AreEqual(assembly.Location, testAssembly.Name);

            TestFixture testClass1TestMethod1 = testAssembly.Fixtures.FirstOrDefault(x => x.Name == "ParallelTestRunner.Tests.VSTest.TestClass1.TestMethod1");
            Assert.IsNotNull(testClass1TestMethod1);
            Assert.AreEqual("Group2", testClass1TestMethod1.Group);
            Assert.IsFalse(testClass1TestMethod1.Exclusive.Value);

            TestFixture testClass1TestMethod2 = testAssembly.Fixtures.FirstOrDefault(x => x.Name == "ParallelTestRunner.Tests.VSTest.TestClass1.TestMethod2");
            Assert.IsNotNull(testClass1TestMethod2);
            Assert.AreEqual("Group2", testClass1TestMethod2.Group);
            Assert.IsFalse(testClass1TestMethod2.Exclusive.Value);

            TestFixture testClass2TestMethod1 = testAssembly.Fixtures.FirstOrDefault(x => x.Name == "ParallelTestRunner.Tests.VSTest.TestClass2.TestMethod1");
            Assert.IsNotNull(testClass2TestMethod1);
            Assert.AreEqual("Group1", testClass2TestMethod1.Group);
            Assert.AreEqual(true, testClass2TestMethod1.Exclusive);
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    [TestGroup("Group2", false)]
    public class TestClass1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestMethod2()
        {
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    [TestGroup(Name = "Group1", Exclusive = true)]
    public class TestClass2
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}