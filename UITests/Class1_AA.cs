using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    [TestClassGroup("Group2")]
    public class Class1_AA
    {
        [TestMethod]
        public void Class1_AATestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class1_AATestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class1_AATestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }
    }
}
