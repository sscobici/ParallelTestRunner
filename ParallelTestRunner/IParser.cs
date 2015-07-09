using System.Reflection;
using ParallelTestRunner.Common;

namespace ParallelTestRunner
{
    public interface IParser
    {
        TestAssembly Parse(Assembly assembly);
    }
}