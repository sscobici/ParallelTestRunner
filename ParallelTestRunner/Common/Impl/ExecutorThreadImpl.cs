using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace ParallelTestRunner.Common.Impl
{
    public sealed class ExecutorThreadImpl : IExecutorThread, IDisposable
    {
        private IExecutor executor;
        private RunData runData;
        private Task task;

        public ExecutorThreadImpl(IExecutor executor, RunData runData)
        {
            this.executor = executor;
            this.runData = runData;
        }

        public void Launch(IThreadEnder ender)
        {
            task = new Task((o) => executor.Run(o as RunData), (object)runData);
            task.ContinueWith((t) => ender.OnEnded(this));
            task.Start();
        }

        public IExecutor GetExecutor()
        {
            return executor;
        }

        public Task GetTask()
        {
            return task;
        }

        public RunData GetRunData()
        {
            return runData;
        }

        public void Dispose()
        {
            if (task != null)
            {
                task.Dispose();
            }
        }
    }
}
