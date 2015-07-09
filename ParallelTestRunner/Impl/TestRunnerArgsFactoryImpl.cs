using System;
using System.Collections.Generic;

namespace ParallelTestRunner.Impl
{
    public class TestRunnerArgsFactoryImpl : ITestRunnerArgsFactory
    {
        public ITestRunnerArgs ParseArgs(string[] args)
        {
            TestRunnerArgsImpl data = new TestRunnerArgsImpl();
            data.AssemblyList = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                string item = args[i].Replace("\"", string.Empty);
                if (item.StartsWith("provider:"))
                {
                    data.Provider = item.Remove(0, 9);
                }
                else if (item.StartsWith("threadcount:"))
                {
                    int number;
                    int.TryParse(item.Remove(0, 12), out number);
                    data.ThreadCount = number;
                }
                else if (item.StartsWith("root:"))
                {
                    data.Root = item.Remove(0, 5);
                }
                else if (item.StartsWith("out:"))
                {
                    data.Output = item.Remove(0, 4);
                }
                else
                {
                    string[] pathList = item.Split(' ');
                    for (int j = 0; j < pathList.Length; j++)
                    {
                        data.AssemblyList.Add(pathList[j]);
                    }
                }
            }

            return data;
        }
    }
}