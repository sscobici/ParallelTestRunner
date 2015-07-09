using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;

namespace ParallelTestRunner.Process2
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    internal static class NtProcessInfoHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        internal class SystemProcessInformation
        {
            internal uint NextEntryOffset;
            internal uint NumberOfThreads;
            private long SpareLi1;
            private long SpareLi2;
            private long SpareLi3;
            private long CreateTime;
            private long UserTime;
            private long KernelTime;
            internal ushort NameLength;
            internal ushort MaximumNameLength;
            internal IntPtr NamePtr;
            internal int BasePriority;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;
            internal uint HandleCount;
            internal uint SessionId;
            internal UIntPtr PageDirectoryBase;
            internal UIntPtr PeakVirtualSize;
            internal UIntPtr VirtualSize;
            internal uint PageFaultCount;
            internal UIntPtr PeakWorkingSetSize;
            internal UIntPtr WorkingSetSize;
            internal UIntPtr QuotaPeakPagedPoolUsage;
            internal UIntPtr QuotaPagedPoolUsage;
            internal UIntPtr QuotaPeakNonPagedPoolUsage;
            internal UIntPtr QuotaNonPagedPoolUsage;
            internal UIntPtr PagefileUsage;
            internal UIntPtr PeakPagefileUsage;
            internal UIntPtr PrivatePageCount;
            private long ReadOperationCount;
            private long WriteOperationCount;
            private long OtherOperationCount;
            private long ReadTransferCount;
            private long WriteTransferCount;
            private long OtherTransferCount;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public SystemProcessInformation()
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal class SystemThreadInformation
        {
            private long KernelTime;
            private long UserTime;
            private long CreateTime;
            private uint WaitTime;
            internal IntPtr StartAddress;
            internal IntPtr UniqueProcess;
            internal IntPtr UniqueThread;
            internal int Priority;
            internal int BasePriority;
            internal uint ContextSwitches;
            internal uint ThreadState;
            internal uint WaitReason;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            public SystemThreadInformation()
            {
            }
        }

        public static ProcessInfo[] GetProcessInfos()
        {
            int num = 131072;
            int requiredSize = 0;
            GCHandle gCHandle = default(GCHandle);
            ProcessInfo[] processInfos;
            try
            {
                int num2;
                do
                {
                    long[] value = new long[(num + 7) / 8];
                    gCHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
                    num2 = NativeMethods.NtQuerySystemInformation(5, gCHandle.AddrOfPinnedObject(), num, out requiredSize);
                    if (num2 == -1073741820)
                    {
                        if (gCHandle.IsAllocated)
                        {
                            gCHandle.Free();
                        }
                        num = NtProcessInfoHelper.GetNewBufferSize(num, requiredSize);
                    }
                }
                while (num2 == -1073741820);
                if (num2 < 0)
                {
                    throw new InvalidOperationException("CouldntGetProcessInfos", new Win32Exception(num2));
                }
                processInfos = NtProcessInfoHelper.GetProcessInfos(gCHandle.AddrOfPinnedObject());
            }
            finally
            {
                if (gCHandle.IsAllocated)
                {
                    gCHandle.Free();
                }
            }
            return processInfos;
        }

        internal static string GetProcessShortName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            int num = -1;
            int num2 = -1;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '\\')
                {
                    num = i;
                }
                else
                {
                    if (name[i] == '.')
                    {
                        num2 = i;
                    }
                }
            }
            if (num2 == -1)
            {
                num2 = name.Length - 1;
            }
            else
            {
                string b = name.Substring(num2);
                if (string.Equals(".exe", b, StringComparison.OrdinalIgnoreCase))
                {
                    num2--;
                }
                else
                {
                    num2 = name.Length - 1;
                }
            }
            if (num == -1)
            {
                num = 0;
            }
            else
            {
                num++;
            }
            return name.Substring(num, num2 - num + 1);
        }

        private static int GetNewBufferSize(int existingBufferSize, int requiredSize)
        {
            if (requiredSize == 0)
            {
                int num = existingBufferSize * 2;
                if (num < existingBufferSize)
                {
                    throw new OutOfMemoryException();
                }
                return num;
            }
            else
            {
                int num2 = requiredSize + 10240;
                if (num2 < requiredSize)
                {
                    throw new OutOfMemoryException();
                }
                return num2;
            }
        }
        private static ProcessInfo[] GetProcessInfos(IntPtr dataPtr)
        {
            Hashtable hashtable = new Hashtable(60);
            long num = 0L;
            while (true)
            {
                IntPtr intPtr = (IntPtr)((long)dataPtr + num);
                NtProcessInfoHelper.SystemProcessInformation systemProcessInformation = new NtProcessInfoHelper.SystemProcessInformation();
                Marshal.PtrToStructure(intPtr, systemProcessInformation);
                ProcessInfo processInfo = new ProcessInfo();
                processInfo.processId = systemProcessInformation.UniqueProcessId.ToInt32();
                processInfo.handleCount = (int)systemProcessInformation.HandleCount;
                processInfo.sessionId = (int)systemProcessInformation.SessionId;
                processInfo.poolPagedBytes = (long)((ulong)systemProcessInformation.QuotaPagedPoolUsage);
                processInfo.poolNonpagedBytes = (long)((ulong)systemProcessInformation.QuotaNonPagedPoolUsage);
                processInfo.virtualBytes = (long)((ulong)systemProcessInformation.VirtualSize);
                processInfo.virtualBytesPeak = (long)((ulong)systemProcessInformation.PeakVirtualSize);
                processInfo.workingSetPeak = (long)((ulong)systemProcessInformation.PeakWorkingSetSize);
                processInfo.workingSet = (long)((ulong)systemProcessInformation.WorkingSetSize);
                processInfo.pageFileBytesPeak = (long)((ulong)systemProcessInformation.PeakPagefileUsage);
                processInfo.pageFileBytes = (long)((ulong)systemProcessInformation.PagefileUsage);
                processInfo.privateBytes = (long)((ulong)systemProcessInformation.PrivatePageCount);
                processInfo.basePriority = systemProcessInformation.BasePriority;
                if (systemProcessInformation.NamePtr == IntPtr.Zero)
                {
                    if (processInfo.processId == NtProcessManager.SystemProcessID)
                    {
                        processInfo.processName = "System";
                    }
                    else
                    {
                        if (processInfo.processId == 0)
                        {
                            processInfo.processName = "Idle";
                        }
                        else
                        {
                            processInfo.processName = processInfo.processId.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                }
                else
                {
                    string text = NtProcessInfoHelper.GetProcessShortName(Marshal.PtrToStringUni(systemProcessInformation.NamePtr, (int)(systemProcessInformation.NameLength / 2)));
                    processInfo.processName = text;
                }
                hashtable[processInfo.processId] = processInfo;
                intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemProcessInformation));
                int num2 = 0;
                while ((long)num2 < (long)((ulong)systemProcessInformation.NumberOfThreads))
                {
                    NtProcessInfoHelper.SystemThreadInformation systemThreadInformation = new NtProcessInfoHelper.SystemThreadInformation();
                    Marshal.PtrToStructure(intPtr, systemThreadInformation);
                    ThreadInfo threadInfo = new ThreadInfo();
                    threadInfo.processId = (int)systemThreadInformation.UniqueProcess;
                    threadInfo.threadId = (int)systemThreadInformation.UniqueThread;
                    threadInfo.basePriority = systemThreadInformation.BasePriority;
                    threadInfo.currentPriority = systemThreadInformation.Priority;
                    threadInfo.startAddress = systemThreadInformation.StartAddress;
                    threadInfo.threadState = (ThreadState)systemThreadInformation.ThreadState;
                    threadInfo.threadWaitReason = NtProcessManager.GetThreadWaitReason((int)systemThreadInformation.WaitReason);
                    processInfo.threadInfoList.Add(threadInfo);
                    intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemThreadInformation));
                    num2++;
                }
                if (systemProcessInformation.NextEntryOffset == 0u)
                {
                    break;
                }
                num += (long)((ulong)systemProcessInformation.NextEntryOffset);
            }
            ProcessInfo[] array = new ProcessInfo[hashtable.Values.Count];
            hashtable.Values.CopyTo(array, 0);
            return array;
        }
    }
}
