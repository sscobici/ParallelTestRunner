using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    [TestGroup("Group2")]
    public class Class4
    {
        [TestMethod]
        public void Class4TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class4TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class4TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }

        [TestMethod]
        public void Class4TestMethod4()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(30, 20);
        }
    }
}