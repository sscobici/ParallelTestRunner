using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace ParallelTestRunner.Process2
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    internal static class NtProcessManager
    {
        private enum ValueId
        {
            Unknown = -1,
            HandleCount,
            PoolPagedBytes,
            PoolNonpagedBytes,
            ElapsedTime,
            VirtualBytesPeak,
            VirtualBytes,
            PrivateBytes,
            PageFileBytes,
            PageFileBytesPeak,
            WorkingSetPeak,
            WorkingSet,
            ThreadId,
            ProcessId,
            BasePriority,
            CurrentPriority,
            UserTime,
            PrivilegedTime,
            StartAddress,
            ThreadState,
            ThreadWaitReason
        }
        internal const int IdleProcessID = 0;
        private static Hashtable valueIds;
        private const int ProcessPerfCounterId = 230;
        private const int ThreadPerfCounterId = 232;
        private const string PerfCounterQueryString = "230 232";
        internal static int SystemProcessID
        {
            get
            {
                return 4;
            }
        }
        public static int[] GetProcessIds()
        {
            int[] array = new int[256];
            int num;
            while (NativeMethods.EnumProcesses(array, array.Length * 4, out num))
            {
                if (num != array.Length * 4)
                {
                    int[] array2 = new int[num / 4];
                    Array.Copy(array, array2, array2.Length);
                    return array2;
                }
                array = new int[array.Length * 2];
            }
            throw new Win32Exception();
        }
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static ModuleInfo[] GetModuleInfos(int processId)
        {
            return NtProcessManager.GetModuleInfos(processId, false);
        }
        public static ModuleInfo GetFirstModuleInfo(int processId)
        {
            ModuleInfo[] moduleInfos = NtProcessManager.GetModuleInfos(processId, true);
            if (moduleInfos.Length == 0)
            {
                return null;
            }
            return moduleInfos[0];
        }
        private static ModuleInfo[] GetModuleInfos(int processId, bool firstModuleOnly)
        {
            if (processId == NtProcessManager.SystemProcessID || processId == 0)
            {
                throw new Win32Exception(-2147467259, "EnumProcessModuleFailed");
            }
            SafeProcessHandle safeProcessHandle = SafeProcessHandle.InvalidHandle;
            ModuleInfo[] result;
            try
            {
                safeProcessHandle = ProcessManager.OpenProcess(processId, 1040, true);
                IntPtr[] array = new IntPtr[64];
                GCHandle gCHandle = default(GCHandle);
                int num = 0;
                while (true)
                {
                    bool flag = false;
                    try
                    {
                        gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
                        flag = NativeMethods.EnumProcessModules(safeProcessHandle, gCHandle.AddrOfPinnedObject(), array.Length * IntPtr.Size, ref num);
                        if (!flag)
                        {
                            bool flag2 = false;
                            bool flag3 = false;
                            if (true)
                            {
                                SafeProcessHandle safeProcessHandle2 = SafeProcessHandle.InvalidHandle;
                                try
                                {
                                    safeProcessHandle2 = ProcessManager.OpenProcess(NativeMethods.GetCurrentProcessId(), 1024, true);
                                    if (!SafeNativeMethods.IsWow64Process(safeProcessHandle2, ref flag2))
                                    {
                                        throw new Win32Exception();
                                    }
                                    if (!SafeNativeMethods.IsWow64Process(safeProcessHandle, ref flag3))
                                    {
                                        throw new Win32Exception();
                                    }
                                    if (flag2 && !flag3)
                                    {
                                        throw new Win32Exception(299, "EnumProcessModuleFailedDueToWow");
                                    }
                                }
                                finally
                                {
                                    if (safeProcessHandle2 != SafeProcessHandle.InvalidHandle)
                                    {
                                        safeProcessHandle2.Close();
                                    }
                                }
                            }
                            for (int i = 0; i < 50; i++)
                            {
                                flag = NativeMethods.EnumProcessModules(safeProcessHandle, gCHandle.AddrOfPinnedObject(), array.Length * IntPtr.Size, ref num);
                                if (flag)
                                {
                                    break;
                                }
                                Thread.Sleep(1);
                            }
                        }
                    }
                    finally
                    {
                        gCHandle.Free();
                    }
                    if (!flag)
                    {
                        break;
                    }
                    num /= IntPtr.Size;
                    if (num <= array.Length)
                    {
                        goto IL_157;
                    }
                    array = new IntPtr[array.Length * 2];
                }
                throw new Win32Exception();
            IL_157:
                ArrayList arrayList = new ArrayList();
                for (int j = 0; j < num; j++)
                {
                    try
                    {
                        ModuleInfo moduleInfo = new ModuleInfo();
                        IntPtr handle = array[j];
                        NativeMethods.NtModuleInfo ntModuleInfo = new NativeMethods.NtModuleInfo();
                        if (!NativeMethods.GetModuleInformation(safeProcessHandle, new HandleRef(null, handle), ntModuleInfo, Marshal.SizeOf(ntModuleInfo)))
                        {
                            throw new Win32Exception();
                        }
                        moduleInfo.sizeOfImage = ntModuleInfo.SizeOfImage;
                        moduleInfo.entryPoint = ntModuleInfo.EntryPoint;
                        moduleInfo.baseOfDll = ntModuleInfo.BaseOfDll;
                        StringBuilder stringBuilder = new StringBuilder(1024);
                        if (NativeMethods.GetModuleBaseName(safeProcessHandle, new HandleRef(null, handle), stringBuilder, stringBuilder.Capacity * 2) == 0)
                        {
                            throw new Win32Exception();
                        }
                        moduleInfo.baseName = stringBuilder.ToString();
                        StringBuilder stringBuilder2 = new StringBuilder(1024);
                        if (NativeMethods.GetModuleFileNameEx(safeProcessHandle, new HandleRef(null, handle), stringBuilder2, stringBuilder2.Capacity * 2) == 0)
                        {
                            throw new Win32Exception();
                        }
                        moduleInfo.fileName = stringBuilder2.ToString();
                        if (string.Compare(moduleInfo.fileName, "\\SystemRoot\\System32\\smss.exe", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            moduleInfo.fileName = Path.Combine(Environment.SystemDirectory, "smss.exe");
                        }
                        if (moduleInfo.fileName != null && moduleInfo.fileName.Length >= 4 && moduleInfo.fileName.StartsWith("\\\\?\\", StringComparison.Ordinal))
                        {
                            moduleInfo.fileName = moduleInfo.fileName.Substring(4);
                        }
                        arrayList.Add(moduleInfo);
                    }
                    catch (Win32Exception ex)
                    {
                        if (ex.NativeErrorCode != 6 && ex.NativeErrorCode != 299)
                        {
                            throw;
                        }
                    }
                    if (firstModuleOnly)
                    {
                        break;
                    }
                }
                ModuleInfo[] array2 = new ModuleInfo[arrayList.Count];
                arrayList.CopyTo(array2, 0);
                result = array2;
            }
            finally
            {
                if (!safeProcessHandle.IsInvalid)
                {
                    safeProcessHandle.Close();
                }
            }
            return result;
        }
        public static int GetProcessIdFromHandle(SafeProcessHandle processHandle)
        {
            NativeMethods.NtProcessBasicInfo ntProcessBasicInfo = new NativeMethods.NtProcessBasicInfo();
            int num = NativeMethods.NtQueryInformationProcess(processHandle, 0, ntProcessBasicInfo, Marshal.SizeOf(ntProcessBasicInfo), null);
            if (num != 0)
            {
                throw new InvalidOperationException("CantGetProcessId", new Win32Exception(num));
            }
            return ntProcessBasicInfo.UniqueProcessId.ToInt32();
        }
        internal static ThreadWaitReason GetThreadWaitReason(int value)
        {
            switch (value)
            {
                case 0:
                case 7:
                    return ThreadWaitReason.Executive;
                case 1:
                case 8:
                    return ThreadWaitReason.FreePage;
                case 2:
                case 9:
                    return ThreadWaitReason.PageIn;
                case 3:
                case 10:
                    return ThreadWaitReason.SystemAllocation;
                case 4:
                case 11:
                    return ThreadWaitReason.ExecutionDelay;
                case 5:
                case 12:
                    return ThreadWaitReason.Suspended;
                case 6:
                case 13:
                    return ThreadWaitReason.UserRequest;
                case 14:
                    return ThreadWaitReason.EventPairHigh;
                case 15:
                    return ThreadWaitReason.EventPairLow;
                case 16:
                    return ThreadWaitReason.LpcReceive;
                case 17:
                    return ThreadWaitReason.LpcReply;
                case 18:
                    return ThreadWaitReason.VirtualMemory;
                case 19:
                    return ThreadWaitReason.PageOut;
                default:
                    return ThreadWaitReason.Unknown;
            }
        }
        static NtProcessManager()
        {
            NtProcessManager.valueIds = new Hashtable();
            NtProcessManager.valueIds.Add("Handle Count", NtProcessManager.ValueId.HandleCount);
            NtProcessManager.valueIds.Add("Pool Paged Bytes", NtProcessManager.ValueId.PoolPagedBytes);
            NtProcessManager.valueIds.Add("Pool Nonpaged Bytes", NtProcessManager.ValueId.PoolNonpagedBytes);
            NtProcessManager.valueIds.Add("Elapsed Time", NtProcessManager.ValueId.ElapsedTime);
            NtProcessManager.valueIds.Add("Virtual Bytes Peak", NtProcessManager.ValueId.VirtualBytesPeak);
            NtProcessManager.valueIds.Add("Virtual Bytes", NtProcessManager.ValueId.VirtualBytes);
            NtProcessManager.valueIds.Add("Private Bytes", NtProcessManager.ValueId.PrivateBytes);
            NtProcessManager.valueIds.Add("Page File Bytes", NtProcessManager.ValueId.PageFileBytes);
            NtProcessManager.valueIds.Add("Page File Bytes Peak", NtProcessManager.ValueId.PageFileBytesPeak);
            NtProcessManager.valueIds.Add("Working Set Peak", NtProcessManager.ValueId.WorkingSetPeak);
            NtProcessManager.valueIds.Add("Working Set", NtProcessManager.ValueId.WorkingSet);
            NtProcessManager.valueIds.Add("ID Thread", NtProcessManager.ValueId.ThreadId);
            NtProcessManager.valueIds.Add("ID Process", NtProcessManager.ValueId.ProcessId);
            NtProcessManager.valueIds.Add("Priority Base", NtProcessManager.ValueId.BasePriority);
            NtProcessManager.valueIds.Add("Priority Current", NtProcessManager.ValueId.CurrentPriority);
            NtProcessManager.valueIds.Add("% User Time", NtProcessManager.ValueId.UserTime);
            NtProcessManager.valueIds.Add("% Privileged Time", NtProcessManager.ValueId.PrivilegedTime);
            NtProcessManager.valueIds.Add("Start Address", NtProcessManager.ValueId.StartAddress);
            NtProcessManager.valueIds.Add("Thread State", NtProcessManager.ValueId.ThreadState);
            NtProcessManager.valueIds.Add("Thread Wait Reason", NtProcessManager.ValueId.ThreadWaitReason);
        }

        private static ThreadInfo GetThreadInfo(NativeMethods.PERF_OBJECT_TYPE type, IntPtr instancePtr, NativeMethods.PERF_COUNTER_DEFINITION[] counters)
        {
            ThreadInfo threadInfo = new ThreadInfo();
            for (int i = 0; i < counters.Length; i++)
            {
                NativeMethods.PERF_COUNTER_DEFINITION pERF_COUNTER_DEFINITION = counters[i];
                long num = NtProcessManager.ReadCounterValue(pERF_COUNTER_DEFINITION.CounterType, (IntPtr)((long)instancePtr + (long)pERF_COUNTER_DEFINITION.CounterOffset));
                switch (pERF_COUNTER_DEFINITION.CounterNameTitlePtr)
                {
                    case 11:
                        threadInfo.threadId = (int)num;
                        break;
                    case 12:
                        threadInfo.processId = (int)num;
                        break;
                    case 13:
                        threadInfo.basePriority = (int)num;
                        break;
                    case 14:
                        threadInfo.currentPriority = (int)num;
                        break;
                    case 17:
                        threadInfo.startAddress = (IntPtr)num;
                        break;
                    case 18:
                        threadInfo.threadState = (System.Diagnostics.ThreadState)num;
                        break;
                    case 19:
                        threadInfo.threadWaitReason = NtProcessManager.GetThreadWaitReason((int)num);
                        break;
                }
            }
            return threadInfo;
        }
        private static ProcessInfo GetProcessInfo(NativeMethods.PERF_OBJECT_TYPE type, IntPtr instancePtr, NativeMethods.PERF_COUNTER_DEFINITION[] counters)
        {
            ProcessInfo processInfo = new ProcessInfo();
            for (int i = 0; i < counters.Length; i++)
            {
                NativeMethods.PERF_COUNTER_DEFINITION pERF_COUNTER_DEFINITION = counters[i];
                long num = NtProcessManager.ReadCounterValue(pERF_COUNTER_DEFINITION.CounterType, (IntPtr)((long)instancePtr + (long)pERF_COUNTER_DEFINITION.CounterOffset));
                switch (pERF_COUNTER_DEFINITION.CounterNameTitlePtr)
                {
                    case 0:
                        processInfo.handleCount = (int)num;
                        break;
                    case 1:
                        processInfo.poolPagedBytes = num;
                        break;
                    case 2:
                        processInfo.poolNonpagedBytes = num;
                        break;
                    case 4:
                        processInfo.virtualBytesPeak = num;
                        break;
                    case 5:
                        processInfo.virtualBytes = num;
                        break;
                    case 6:
                        processInfo.privateBytes = num;
                        break;
                    case 7:
                        processInfo.pageFileBytes = num;
                        break;
                    case 8:
                        processInfo.pageFileBytesPeak = num;
                        break;
                    case 9:
                        processInfo.workingSetPeak = num;
                        break;
                    case 10:
                        processInfo.workingSet = num;
                        break;
                    case 12:
                        processInfo.processId = (int)num;
                        break;
                    case 13:
                        processInfo.basePriority = (int)num;
                        break;
                }
            }
            return processInfo;
        }
        private static NtProcessManager.ValueId GetValueId(string counterName)
        {
            if (counterName != null)
            {
                object obj = NtProcessManager.valueIds[counterName];
                if (obj != null)
                {
                    return (NtProcessManager.ValueId)obj;
                }
            }
            return NtProcessManager.ValueId.Unknown;
        }
        private static long ReadCounterValue(int counterType, IntPtr dataPtr)
        {
            if ((counterType & 256) != 0)
            {
                return Marshal.ReadInt64(dataPtr);
            }
            return (long)Marshal.ReadInt32(dataPtr);
        }
    }
}
