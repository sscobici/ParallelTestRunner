using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    public class Class6
    {
        [TestMethod]
        public void Class6TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class6TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class6TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }

        [TestMethod]
        public void Class6TestMethod4()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(30, 20);
        }
    }
}