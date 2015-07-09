using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelTestRunner.Common
{
    public class NativeMethods
    {
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        internal static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);
    }
}
