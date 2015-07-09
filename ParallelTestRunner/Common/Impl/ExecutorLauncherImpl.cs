using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelTestRunner.Common.Impl
{
    public class ExecutorLauncherImpl : IExecutorLauncher, IThreadStarter, IThreadEnder
    {
        public ExecutorLauncherImpl()
        {
            Threads = new List<IExecutorThread>();
        }

        public IList<IExecutorThread> Threads { get; set; }
        
        public ITestRunnerArgs Args { get; set; }
        
        public IExecutor Executor { get; set; }
        
        public IThreadFactory ThreadFactory { get; set; }

        public bool LaunchExecutor(RunData runData)
        {
            IExecutorThread thread = null;
            lock (Threads)
            {
                if (!ThreadFactory.CanLaunch(Threads, runData))
                {
                    return false;
                }

                thread = ThreadFactory.Create(Executor, runData, this);
            }

            thread.Launch(this);
            return true;
        }

        public void WaitForAnyThreadToComplete()
        {
            Task[] array;
            lock (Threads)
            {
                array = ThreadFactory.GetTaskArray(Threads);
            }

            ThreadFactory.WaitAny(array);
        }

        public void WaitForAll()
        {
            Task[] array;
            lock (Threads)
            {
                array = ThreadFactory.GetTaskArray(Threads);
            }

            ThreadFactory.WaitAll(array);
        }

        public bool HasFreeThreads()
        {
            lock (Threads)
            {
                return Threads.Count < Args.ThreadCount;
            }
        }

        public void OnStarted(IExecutorThread thread)
        {
            Threads.Add(thread);
        }

        public void OnEnded(IExecutorThread thread)
        {
            lock (Threads)
            {
                Threads.Remove(thread);
            }
        }
    }
}