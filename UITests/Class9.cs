using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    public class Class9
    {
        [TestMethod]
        public void Class9TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class9TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class9TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }

        [TestMethod]
        public void Class9TestMethod4()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(30, 20);
        }
    }
}
