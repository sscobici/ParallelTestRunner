using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
namespace ParallelTestRunner.Common.Impl
{
    /// <summary>
    /// Intended for non-blocking (quick) tests
    /// </summary>
    public class RunDataNonBlockingBuilderImpl : IRunDataBuilder
    {
        public IList<RunData> Create(TestAssembly testAssembly, ITestRunnerArgs args)
        {
            testAssembly.Fixtures = testAssembly.Fixtures.OrderBy((m) => m.Group).ToList();
            IList<RunData> items = new List<RunData>();
            bool startNewRunData = true;
            RunData item = null;
            foreach (TestFixture fixture in testAssembly.Fixtures)
            {
                if (null != item)
                {
                    startNewRunData = false;
                    if (item.Fixtures.Count >= 25)
                    {
                        startNewRunData = true;
                    }
                    else if (fixture.Group != null &&
                        item.Groups.Count > 0 &&
                        item.Groups.Contains(fixture.Group))
                    {
                        startNewRunData = true;
                    }
                }

                if (startNewRunData)
                {
                    item = CreateNewFromFixture(testAssembly, fixture, args);
                    items.Add(item);
                }
                else
                {
                    item.Fixtures.Add(fixture);
                }
            }

            return items;
        }

        private RunData CreateNewFromFixture(TestAssembly testAssembly, TestFixture fixture, ITestRunnerArgs args)
        {
            RunData item = new RunData()
            {
                Fixtures = new List<TestFixture>() { fixture },
                Root = args.Root,
                Output = new StringBuilder(),
                Error = new StringBuilder(),
                Groups = new HashSet<string>(),
                Executable = args.GetExecutablePath(),
                RunId = Guid.NewGuid(),
                AssemblyName = testAssembly.Name
            };
            if (!string.IsNullOrEmpty(fixture.Group))
            {
                item.Groups.Add(fixture.Group);
            }

            return item;
        }
    }
}
*/