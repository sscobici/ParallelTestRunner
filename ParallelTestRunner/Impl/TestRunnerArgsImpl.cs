using ParallelTestRunner.Common;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ParallelTestRunner.Impl
{
    public class TestRunnerArgsImpl : ITestRunnerArgs
    {
        public TestRunnerArgsImpl()
        {
            Output = "Result.trx";
            ThreadCount = 4;
            PLevel = PLevel.TestClass;
        }

        public string Provider { get; set; }
        
        public IList<string> AssemblyList { get; set; }
        
        public int ThreadCount { get; set; }
        
        public string Root { get; set; }
        
        public string Output { get; set; }

        public PLevel PLevel { get; set; }

        public string GetExecutablePath()
        {
            string exePath = ConfigurationManager.AppSettings.Get(Provider);
            if(string.IsNullOrEmpty(exePath))
            {
                // try to get it from env variable
                exePath = Environment.GetEnvironmentVariable(Provider);
            }
            if (string.IsNullOrEmpty(exePath))
            {
                throw new Exception("Couldn't find provide for " + Provider + ". Please check the provider or set envrionment variable " + Provider + " to point to the path of vstest.console.exe");
            }
            return exePath;
        }

        public bool IsValid()
        {
            if (AssemblyList == null ||
                AssemblyList.Count == 0)
            {
                Console.WriteLine("at least one DLL must be specified: c:\\work\\testassembly.dll");
                return false;
            }

            if (string.IsNullOrEmpty(Provider))
            {
                Console.WriteLine("Provider is required (see config file): provider:VSTEST_2012");
                return false;
            }

            if (ThreadCount <= 0)
            {
                Console.WriteLine("threadcount must be integer greater than 0: threadcount:4");
                return false;
            }

            if (string.IsNullOrEmpty(Root))
            {
                Console.WriteLine("root path is required: root:d:\\work");
                return false;
            }

            if (PLevel == PLevel.None)
            {
                Console.WriteLine("Level which will be considered parallel is required: plevel:testclass");
                return false;
            }

            return true;
        }
    }
}