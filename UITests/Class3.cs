using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    [TestClassGroup("Group1", Exclusive = true)]
    public class Class3
    {
        [TestMethod]
        public void Class3TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class3TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class3TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }

        [TestMethod]
        public void Class3TestMethod4()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(30, 20);
        }
    }
}
