using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Impl;

namespace ParallelTestRunner.Tests.VSTest
{
    [TestClass]
    public class VSTestParserTest : TestBase
    {
        private VSTestParserImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new VSTestParserImpl();
        }

        [TestMethod]
        public void Parse()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            TestAssembly testAssembly = target.Parse(assembly);
            Assert.AreEqual(assembly.Location, testAssembly.Name);
          
            TestFixture testClass1 = testAssembly.Fixtures.FirstOrDefault(x => x.Name == "ParallelTestRunner.Tests.VSTest.TestClass1");
            Assert.IsNotNull(testClass1);
            Assert.AreEqual("Group2", testClass1.Group);

            TestFixture testClass2 = testAssembly.Fixtures.FirstOrDefault(x => x.Name == "ParallelTestRunner.Tests.VSTest.TestClass2");
            Assert.IsNotNull(testClass2);
            Assert.AreEqual("Group1", testClass2.Group);
            Assert.AreEqual(true, testClass2.Exclusive);
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    [TestGroup("Group2", false)]
    public class TestClass1
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    [TestGroup(Name = "Group1", Exclusive = true)]
    public class TestClass2
    {
    }
}