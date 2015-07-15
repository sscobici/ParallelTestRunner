using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Common.Impl
{
    public class ProcessStartInfoFactoryImpl : IProcessStartInfoFactory
    {
        public ProcessStartInfo CreateProcessStartInfo(RunData data)
        {
            string testNames = ConcatFixtures(data.Fixtures);
            string settings = string.Concat(data.Root, "\\", data.RunId, ".settings");
            return new ProcessStartInfo()
            {
                FileName = data.Executable,
                Arguments =
                    "\"" + data.AssemblyName + "\"" +
                    " \"/settings:" + settings + "\"" +
                    " /logger:trx" +
                    " /Tests:" + testNames,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
        }

        private string ConcatFixtures(IList<TestFixture> items)
        {
            StringBuilder text = new StringBuilder();
            bool first = true;
            foreach (TestFixture item in items)
            {
                if (!first)
                {
                    text.Append(",");
                }

                text.Append(item.Name);
                first = false;
            }

            return text.ToString();
        }
    }
}