using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace ParallelTestRunner.Tests
{
    public abstract class TestBase
    {
        private MockRepository mocks;

        [TestInitialize]
        public void SetUpBase()
        {
            mocks = new MockRepository();
        }

        public T Stub<T>()
        {
            return mocks.Stub<T>();
        }

        public IDisposable Ordered()
        {
            return mocks.Ordered();
        }

        public void VerifyTarget(Action targetCall)
        {
            mocks.ReplayAll();
            targetCall();
            mocks.VerifyAll();
        }

        public T VerifyTarget<T>(Func<T> targetCall)
        {
            mocks.ReplayAll();
            T actual = targetCall();
            mocks.VerifyAll();
            return actual;
        }

        public void ReplayAll()
        {
            mocks.ReplayAll();
        }

        public void VerifyAll()
        {
            mocks.VerifyAll();
        }
    }
}
