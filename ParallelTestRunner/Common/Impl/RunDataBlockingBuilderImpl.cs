using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelTestRunner.Common.Impl
{
    /// <summary>
    /// Intended for blocking (long-running) tests
    /// </summary>
    public class RunDataBlockingBuilderImpl : IRunDataBuilder
    {
        public ITestRunnerArgs Args { get; set; }

        public IList<RunData> Create(TestAssembly assembly)
        {
            IList<RunData> items = new List<RunData>();
            assembly.Fixtures = assembly.Fixtures.OrderBy((m) => m.Name).ToList();
            string lastFixtureName = "#";
            RunData item = null;
            foreach (TestFixture fixture in assembly.Fixtures)
            {
                if (fixture.Name.StartsWith(lastFixtureName))
                {
                    item.Groups.Add(fixture.Group);
                    item.Fixtures.Add(fixture);
                    if (fixture.Exclusive == true)
                    {
                        item.Exclusive = true;
                    }

                    continue;
                }

                item = CreateNewFromFixture(assembly, fixture);
                lastFixtureName = fixture.Name;
                items.Add(item);
            }

            return items;
        }

        private RunData CreateNewFromFixture(TestAssembly testAssembly, TestFixture fixture)
        {
            RunData item = new RunData()
            {
                Fixtures = new List<TestFixture>() { fixture },
                Root = Args.Root,
                Output = new StringBuilder(),
                Groups = new HashSet<string>(),
                Exclusive = fixture.Exclusive ?? false,
                Executable = Args.GetExecutablePath(),
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