using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelTestRunner.Common.Impl
{
    public class ThreadFactoryImpl : IThreadFactory
    {
        public IExecutorThread Create(IExecutor executor, RunData data, IThreadStarter starter)
        {
            IExecutorThread thread = new ExecutorThreadImpl(executor, data);
            starter.OnStarted(thread);
            return thread;
        }

        public Task[] GetTaskArray(IList<IExecutorThread> items)
        {
            int index = 0;
            Task[] array = new Task[items.Count];
            foreach (IExecutorThread item in items)
            {
                array[index] = item.GetTask();
                index++;
            }

            return array;
        }

        public bool CanLaunch(IList<IExecutorThread> threads, RunData runData)
        {
            if (runData.Groups.Count == 0 ||
                   threads.Count == 0)
            {
                return true;
            }

            foreach (IExecutorThread item in threads)
            {
                RunData data = item.GetRunData();
                if (data.Exclusive ||
                    (data.Groups.Count > 0 && runData.Exclusive))
                {
                    return false;
                }

                foreach (string group in data.Groups)
                {
                    if (runData.Groups.Contains(group))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void WaitAny(Task[] array)
        {
            Task.WaitAny(array);
        }

        public void WaitAll(Task[] array)
        {
            Task.WaitAll(array);
        }
    }
}