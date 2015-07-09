using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management;

namespace ParallelTestRunner.Common.Impl
{
    public class BreakerImpl : IBreaker
    {
        private bool breakReceived = false;

        public BreakerImpl()
        {
            NativeMethods.HandlerRoutine handerRoutine = new NativeMethods.HandlerRoutine(OnBreakReceived);
            GC.KeepAlive(handerRoutine);
            NativeMethods.SetConsoleCtrlHandler(handerRoutine, true);
        }

        public bool IsBreakReceived()
        {
            lock (this)
            {
                return breakReceived;
            }
        }

        public void Break()
        {
            lock (this)
            {
                breakReceived = true;
            }

            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + Process.GetCurrentProcess().Id);

            foreach (ManagementObject mo in mos.Get())
            {
                int pid = Convert.ToInt32(mo["ProcessID"]);
                try
                {
                    Process.GetProcessById(pid).Kill();
                }
                catch
                {
                }
            }
        }

        private bool OnBreakReceived(NativeMethods.CtrlTypes ctrlType)
        {
            Break();
            return true;
        }
    }
}