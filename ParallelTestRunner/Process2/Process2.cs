#pragma warning disable 0649
#pragma warning disable 0169
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace ParallelTestRunner.Process2
{
    /// <summary>Provides access to local and remote processes and enables you to start and stop local system processes.</summary>
    /// <filterpriority>1</filterpriority>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [DefaultEvent("Exited"), DefaultProperty("StartInfo"), Designer("System.Diagnostics.Design.ProcessDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), MonitoringDescription("ProcessDesc")]
    [HostProtection(SecurityAction.LinkDemand, SharedState = true, Synchronization = true, ExternalProcessMgmt = true, SelfAffectingProcessMgmt = true), PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust"), PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class Process2 : Component
    {
        private enum StreamReadMode
        {
            undefined,
            syncMode,
            asyncMode
        }

        private enum State
        {
            HaveId = 1,
            IsLocal,
            IsNt = 4,
            HaveProcessInfo = 8,
            Exited = 16,
            Associated = 32,
            IsWin2k = 64,
            HaveNtProcessInfo = 12
        }

        private bool haveProcessId;
        private int processId;
        private bool haveProcessHandle;
        private SafeProcessHandle m_processHandle;
        private bool isRemoteMachine;
        private string machineName;
        private ProcessInfo processInfo;
        private int m_processAccess;
        private ProcessThreadCollection threads;
        private ProcessModuleCollection modules;
        private bool haveMainWindow;
        private IntPtr mainWindowHandle;
        private string mainWindowTitle;
        private bool haveWorkingSetLimits;
        private IntPtr minWorkingSet;
        private IntPtr maxWorkingSet;
        private bool haveProcessorAffinity;
        private IntPtr processorAffinity;
        private bool havePriorityClass;
        private ProcessPriorityClass priorityClass;
        private ProcessStartInfo startInfo;
        private bool watchForExit;
        private bool watchingForExit;
        private EventHandler onExited;
        private bool exited;
        private int exitCode;
        private bool signaled;
        private DateTime exitTime;
        private bool haveExitTime;
        private bool responding;
        private bool haveResponding;
        private bool priorityBoostEnabled;
        private bool havePriorityBoostEnabled;
        private bool raisedOnExited;
        private RegisteredWaitHandle registeredWaitHandle;
        private Process2.StreamReadMode outputStreamReadMode;
        private Process2.StreamReadMode errorStreamReadMode;
        private WaitHandle waitHandle;
        private ISynchronizeInvoke synchronizingObject;
        private StreamReader standardOutput;
        private StreamWriter standardInput;
        private StreamReader standardError;
        private OperatingSystem operatingSystem;
        private bool disposed;
        internal AsyncStreamReader output;
        internal AsyncStreamReader error;
        internal bool pendingOutputRead;
        internal bool pendingErrorRead;

        private static object s_CreateProcessLock = new object();

        [Browsable(true), MonitoringDescription("ProcessAssociated")]
        public event DataReceivedEventHandler OutputDataReceived;

        // System.Diagnostics.Process
        /// <summary>Gets or sets the properties to pass to the <see cref="M:System.Diagnostics.Process.Start" /> method of the <see cref="T:System.Diagnostics.Process" />.</summary>
        /// <returns>The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that represents the data with which to start the process. These arguments include the name of the executable file or document used to start the process.</returns>
        /// <exception cref="T:System.ArgumentNullException">The value that specifies the <see cref="P:System.Diagnostics.Process.StartInfo" /> is null. </exception>
        /// <filterpriority>1</filterpriority>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), MonitoringDescription("ProcessStartInfo")]
        public ProcessStartInfo StartInfo
        {
            get { return startInfo; }
            set { startInfo = value; }
        }

        // System.Diagnostics.Process
        /// <summary>Starts (or reuses) the process resource that is specified by the <see cref="P:System.Diagnostics.Process.StartInfo" /> property of this <see cref="T:System.Diagnostics.Process" /> component and associates it with the component.</summary>
        /// <returns>true if a process resource is started; false if no new process resource is started (for example, if an existing process is reused).</returns>
        /// <exception cref="T:System.InvalidOperationException">No file name was specified in the <see cref="T:System.Diagnostics.Process" /> component's <see cref="P:System.Diagnostics.Process.StartInfo" />.-or- The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> member of the <see cref="P:System.Diagnostics.Process.StartInfo" /> property is true while <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, or <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> is true. </exception>
        /// <exception cref="T:System.ComponentModel.Win32Exception">There was an error in opening the associated file. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
        /// <filterpriority>1</filterpriority>
        public bool Start()
        {
            ProcessStartInfo processStartInfo = StartInfo;
            if (processStartInfo.FileName.Length == 0)
            {
                throw new InvalidOperationException("FileNameMissing");
            }
            if (processStartInfo.UseShellExecute)
            {
                return false;
            }
            return StartWithCreateProcess(processStartInfo);
        }

        public void WaitForExit()
        {
            WaitForExit(-1);
        }

        [ComVisible(false)]
        public void BeginOutputReadLine()
        {
            if (outputStreamReadMode == Process2.StreamReadMode.undefined)
            {
                outputStreamReadMode = Process2.StreamReadMode.asyncMode;
            }
            else
            {
                if (outputStreamReadMode != Process2.StreamReadMode.asyncMode)
                {
                    throw new InvalidOperationException("CantMixSyncAsyncOperation");
                }
            }
            if (pendingOutputRead)
            {
                throw new InvalidOperationException("PendingAsyncOperation");
            }
            pendingOutputRead = true;
            if (output == null)
            {
                if (standardOutput == null)
                {
                    throw new InvalidOperationException("CantGetStandardOut");
                }
                Stream baseStream = standardOutput.BaseStream;
                output = new AsyncStreamReader(this, baseStream, new UserCallBack(OutputReadNotifyUser), standardOutput.CurrentEncoding);
            }
            output.BeginReadLine();
        }

        [Browsable(false), DefaultValue(null), MonitoringDescription("ProcessSynchronizingObject")]
        public ISynchronizeInvoke SynchronizingObject
        {
            get
            {
                if (synchronizingObject == null && base.DesignMode)
                {
                    IDesignerHost designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
                    if (designerHost != null)
                    {
                        object rootComponent = designerHost.RootComponent;
                        if (rootComponent != null && rootComponent is ISynchronizeInvoke)
                        {
                            synchronizingObject = (ISynchronizeInvoke)rootComponent;
                        }
                    }
                }
                return synchronizingObject;
            }
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                synchronizingObject = value;
            }
        }

        internal void OutputReadNotifyUser(string data)
        {
            DataReceivedEventHandler outputDataReceived = OutputDataReceived;
            if (outputDataReceived != null)
            {
                DataReceivedEventArgs dataReceivedEventArgs = new DataReceivedEventArgs(data);
                if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                {
                    SynchronizingObject.Invoke(outputDataReceived, new object[]
                    {
                        this,
                        dataReceivedEventArgs
                    });
                    return;
                }
                outputDataReceived(this, dataReceivedEventArgs);
            }
        }

        public bool WaitForExit(int milliseconds)
        {
            SafeProcessHandle safeProcessHandle = null;
            ProcessWaitHandle processWaitHandle = null;
            bool flag;
            try
            {
                safeProcessHandle = GetProcessHandle(1048576, false);
                if (safeProcessHandle.IsInvalid)
                {
                    flag = true;
                }
                else
                {
                    processWaitHandle = new ProcessWaitHandle(safeProcessHandle);
                    if (processWaitHandle.WaitOne(milliseconds, false))
                    {
                        flag = true;
                        signaled = true;
                    }
                    else
                    {
                        flag = false;
                        signaled = false;
                    }
                }
            }
            finally
            {
                if (processWaitHandle != null)
                {
                    processWaitHandle.Close();
                }
                if (output != null && milliseconds == -1)
                {
                    output.WaitUtilEOF();
                }
                if (error != null && milliseconds == -1)
                {
                    error.WaitUtilEOF();
                }
                ReleaseProcessHandle(safeProcessHandle);
            }
            if (flag && watchForExit)
            {
                RaiseOnExited();
            }
            return flag;
        }


        public bool HasExited
        {
            get
            {
                if (!exited)
                {
                    EnsureState(Process2.State.Associated);
                    SafeProcessHandle safeProcessHandle = null;
                    try
                    {
                        safeProcessHandle = GetProcessHandle(1049600, false);
                        if (safeProcessHandle.IsInvalid)
                        {
                            exited = true;
                        }
                        else
                        {
                            int num;
                            if (NativeMethods.GetExitCodeProcess(safeProcessHandle, out num) && num != 259)
                            {
                                exited = true;
                                exitCode = num;
                            }
                            else
                            {
                                if (!signaled)
                                {
                                    ProcessWaitHandle processWaitHandle = null;
                                    try
                                    {
                                        processWaitHandle = new ProcessWaitHandle(safeProcessHandle);
                                        signaled = processWaitHandle.WaitOne(0, false);
                                    }
                                    finally
                                    {
                                        if (processWaitHandle != null)
                                        {
                                            processWaitHandle.Close();
                                        }
                                    }
                                }
                                if (signaled)
                                {
                                    if (!NativeMethods.GetExitCodeProcess(safeProcessHandle, out num))
                                    {
                                        throw new Win32Exception();
                                    }
                                    exited = true;
                                    exitCode = num;
                                }
                            }
                        }
                    }
                    finally
                    {
                        ReleaseProcessHandle(safeProcessHandle);
                    }
                    if (exited)
                    {
                        RaiseOnExited();
                    }
                }
                return exited;
            }
        }

        private void ReleaseProcessHandle(SafeProcessHandle handle)
        {
            if (handle == null)
            {
                return;
            }
            if (haveProcessHandle && handle == m_processHandle)
            {
                return;
            }
            handle.Close();
        }

        private void RaiseOnExited()
        {
            if (!raisedOnExited)
            {
                bool flag = false;
                try
                {
                    Monitor.Enter(this, ref flag);
                    if (!raisedOnExited)
                    {
                        raisedOnExited = true;
                        OnExited();
                    }
                }
                finally
                {
                    if (flag)
                    {
                        Monitor.Exit(this);
                    }
                }
            }
        }

        protected void OnExited()
        {
            EventHandler eventHandler = onExited;
            if (eventHandler != null)
            {
                if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                {
                    SynchronizingObject.BeginInvoke(eventHandler, new object[]
                    {
                        this,
                        EventArgs.Empty
                    });
                    return;
                }
                eventHandler(this, EventArgs.Empty);
            }
        }

        private SafeProcessHandle GetProcessHandle(int access, bool throwIfExited)
        {
            if (haveProcessHandle)
            {
                if (throwIfExited)
                {
                    ProcessWaitHandle processWaitHandle = null;
                    try
                    {
                        processWaitHandle = new ProcessWaitHandle(m_processHandle);
                        if (processWaitHandle.WaitOne(0, false))
                        {
                            if (haveProcessId)
                            {
                                throw new InvalidOperationException("ProcessHasExited");
                            }
                            throw new InvalidOperationException("ProcessHasExitedNoId");
                        }
                    }
                    finally
                    {
                        if (processWaitHandle != null)
                        {
                            processWaitHandle.Close();
                        }
                    }
                }
                return m_processHandle;
            }
            EnsureState((Process2.State)3);
            SafeProcessHandle safeProcessHandle = SafeProcessHandle.InvalidHandle;
            safeProcessHandle = ProcessManager.OpenProcess(processId, access, throwIfExited);
            if (throwIfExited && (access & 1024) != 0 && NativeMethods.GetExitCodeProcess(safeProcessHandle, out exitCode) && exitCode != 259)
            {
                throw new InvalidOperationException();
            }
            return safeProcessHandle;
        }

        private bool StartWithCreateProcess(ProcessStartInfo startInfo)
        {
            if (startInfo.StandardOutputEncoding != null && !startInfo.RedirectStandardOutput)
            {
                throw new InvalidOperationException("StandardOutputEncodingNotAllowed");
            }
            if (startInfo.StandardErrorEncoding != null && !startInfo.RedirectStandardError)
            {
                throw new InvalidOperationException("StandardErrorEncodingNotAllowed");
            }
            StringBuilder stringBuilder = Process2.BuildCommandLine(startInfo.FileName, startInfo.Arguments);
            NativeMethods.STARTUPINFO sTARTUPINFO = new NativeMethods.STARTUPINFO();
            SafeNativeMethods.PROCESS_INFORMATION pROCESS_INFORMATION = new SafeNativeMethods.PROCESS_INFORMATION();
            SafeProcessHandle safeProcessHandle = new SafeProcessHandle();
            SafeThreadHandle safeThreadHandle = new SafeThreadHandle();
            int num = 0;
            SafeFileHandle handle = null;
            SafeFileHandle handle2 = null;
            SafeFileHandle handle3 = null;
            GCHandle gCHandle = default(GCHandle);
            lock (Process2.s_CreateProcessLock)
            {
                try
                {
                    if (startInfo.RedirectStandardInput || startInfo.RedirectStandardOutput || startInfo.RedirectStandardError)
                    {
                        if (startInfo.RedirectStandardInput)
                        {
                            CreatePipe(out handle, out sTARTUPINFO.hStdInput, true);
                        }
                        else
                        {
                            sTARTUPINFO.hStdInput = new SafeFileHandle(NativeMethods.GetStdHandle(-10), false);
                        }
                        if (startInfo.RedirectStandardOutput)
                        {
                            CreatePipe(out handle2, out sTARTUPINFO.hStdOutput, false);
                        }
                        else
                        {
                            sTARTUPINFO.hStdOutput = new SafeFileHandle(NativeMethods.GetStdHandle(-11), false);
                        }
                        if (startInfo.RedirectStandardError)
                        {
                            CreatePipe(out handle3, out sTARTUPINFO.hStdError, false);
                        }
                        else
                        {
                            sTARTUPINFO.hStdError = sTARTUPINFO.hStdOutput;
                        }
                        sTARTUPINFO.dwFlags = 256;
                    }
                    int num2 = 0;
                    if (startInfo.CreateNoWindow)
                    {
                        num2 |= 134217728;
                    }
                    IntPtr intPtr = (IntPtr)0;
                    //if (startInfo.environmentVariables != null)
                    //{
                    //    bool unicode = false;
                    //    if (ProcessManager.IsNt)
                    //    {
                    //        num2 |= 1024;
                    //        unicode = true;
                    //    }
                    //    byte[] value = EnvironmentBlock.ToByteArray(startInfo.environmentVariables, unicode);
                    //    gCHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
                    //    intPtr = gCHandle.AddrOfPinnedObject();
                    //}
                    string text = startInfo.WorkingDirectory;
                    if (text == string.Empty)
                    {
                        text = Environment.CurrentDirectory;
                    }
                    bool flag2;
                    //if (startInfo.UserName.Length != 0)
                    //{
                    //    NativeMethods.LogonFlags logonFlags = (NativeMethods.LogonFlags)0;
                    //    if (startInfo.LoadUserProfile)
                    //    {
                    //        logonFlags = NativeMethods.LogonFlags.LOGON_WITH_PROFILE;
                    //    }
                    //    IntPtr intPtr2 = IntPtr.Zero;
                    //    try
                    //    {
                    //        if (startInfo.Password == null)
                    //        {
                    //            intPtr2 = Marshal.StringToCoTaskMemUni(string.Empty);
                    //        }
                    //        else
                    //        {
                    //            intPtr2 = Marshal.SecureStringToCoTaskMemUnicode(startInfo.Password);
                    //        }
                    //        RuntimeHelpers.PrepareConstrainedRegions();
                    //        try
                    //        {
                    //        }
                    //        finally
                    //        {
                    //            flag2 = NativeMethods.CreateProcessWithLogonW(startInfo.UserName, startInfo.Domain, intPtr2, logonFlags, null, stringBuilder, num2, intPtr, text, sTARTUPINFO, pROCESS_INFORMATION);
                    //            if (!flag2)
                    //            {
                    //                num = Marshal.GetLastWin32Error();
                    //            }
                    //            if (pROCESS_INFORMATION.hProcess != (IntPtr)0 && pROCESS_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
                    //            {
                    //                safeProcessHandle.InitialSetHandle(pROCESS_INFORMATION.hProcess);
                    //            }
                    //            if (pROCESS_INFORMATION.hThread != (IntPtr)0 && pROCESS_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
                    //            {
                    //                safeThreadHandle.InitialSetHandle(pROCESS_INFORMATION.hThread);
                    //            }
                    //        }
                    //        if (flag2)
                    //        {
                    //            goto IL_3B9;
                    //        }
                    //        if (num == 193 || num == 216)
                    //        {
                    //            throw new Win32Exception(num, SR.GetString("InvalidApplication"));
                    //        }
                    //        throw new Win32Exception(num);
                    //    }
                    //    finally
                    //    {
                    //        if (intPtr2 != IntPtr.Zero)
                    //        {
                    //            Marshal.ZeroFreeCoTaskMemUnicode(intPtr2);
                    //        }
                    //    }
                    //}
                    RuntimeHelpers.PrepareConstrainedRegions();
                    try
                    {
                    }
                    finally
                    {
                        flag2 = NativeMethods.CreateProcess(null, stringBuilder, null, null, true, num2, intPtr, text, sTARTUPINFO, pROCESS_INFORMATION);
                        if (!flag2)
                        {
                            num = Marshal.GetLastWin32Error();
                        }
                        if (pROCESS_INFORMATION.hProcess != (IntPtr)0 && pROCESS_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
                        {
                            safeProcessHandle.InitialSetHandle(pROCESS_INFORMATION.hProcess);
                        }
                        if (pROCESS_INFORMATION.hThread != (IntPtr)0 && pROCESS_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
                        {
                            safeThreadHandle.InitialSetHandle(pROCESS_INFORMATION.hThread);
                        }
                    }
                    if (!flag2)
                    {
                        if (num == 193 || num == 216)
                        {
                            throw new Win32Exception(num, "InvalidApplication");
                        }
                        throw new Win32Exception(num);
                    }
                }
                finally
                {
                    if (gCHandle.IsAllocated)
                    {
                        gCHandle.Free();
                    }
                    sTARTUPINFO.Dispose();
                }
            }
            if (startInfo.RedirectStandardInput)
            {
                standardInput = new StreamWriter(new FileStream(handle, FileAccess.Write, 4096, false), Console.InputEncoding, 4096);
                standardInput.AutoFlush = true;
            }
            if (startInfo.RedirectStandardOutput)
            {
                Encoding encoding = (startInfo.StandardOutputEncoding != null) ? startInfo.StandardOutputEncoding : Console.OutputEncoding;
                standardOutput = new StreamReader(new FileStream(handle2, FileAccess.Read, 4096, false), encoding, true, 4096);
            }
            if (startInfo.RedirectStandardError)
            {
                Encoding encoding2 = (startInfo.StandardErrorEncoding != null) ? startInfo.StandardErrorEncoding : Console.OutputEncoding;
                standardError = new StreamReader(new FileStream(handle3, FileAccess.Read, 4096, false), encoding2, true, 4096);
            }
            bool result = false;
            if (!safeProcessHandle.IsInvalid)
            {
                SetProcessHandle(safeProcessHandle);
                SetProcessId(pROCESS_INFORMATION.dwProcessId);
                safeThreadHandle.Close();
                result = true;
            }
            return result;
        }

        private bool Associated
        {
            get
            {
                return haveProcessId || haveProcessHandle;
            }
        }

        private OperatingSystem OperatingSystem
        {
            get
            {
                if (operatingSystem == null)
                {
                    operatingSystem = Environment.OSVersion;
                }
                return operatingSystem;
            }
        }

        private void EnsureState(Process2.State state)
        {
            if ((state & Process2.State.IsWin2k) != (Process2.State)0 && (OperatingSystem.Platform != PlatformID.Win32NT || OperatingSystem.Version.Major < 5))
            {
                throw new PlatformNotSupportedException("Win2kRequired");
            }
            if ((state & Process2.State.IsNt) != (Process2.State)0 && OperatingSystem.Platform != PlatformID.Win32NT)
            {
                throw new PlatformNotSupportedException("WinNTRequired");
            }
            if ((state & Process2.State.Associated) != (Process2.State)0 && !Associated)
            {
                throw new InvalidOperationException("NoAssociatedProcess");
            }
            if ((state & Process2.State.HaveId) != (Process2.State)0 && !haveProcessId)
            {
                if (!haveProcessHandle)
                {
                    EnsureState(Process2.State.Associated);
                    throw new InvalidOperationException("ProcessIdRequired");
                }
                SetProcessId(ProcessManager.GetProcessIdFromHandle(m_processHandle));
            }
            if ((state & Process2.State.IsLocal) != (Process2.State)0 && isRemoteMachine)
            {
                throw new NotSupportedException("NotSupportedRemote");
            }
            if ((state & Process2.State.HaveProcessInfo) != (Process2.State)0 && processInfo == null)
            {
                if ((state & Process2.State.HaveId) == (Process2.State)0)
                {
                    EnsureState(Process2.State.HaveId);
                }
                ProcessInfo[] processInfos = ProcessManager.GetProcessInfos(machineName);
                for (int i = 0; i < processInfos.Length; i++)
                {
                    if (processInfos[i].processId == processId)
                    {
                        processInfo = processInfos[i];
                        break;
                    }
                }
                if (processInfo == null)
                {
                    throw new InvalidOperationException("NoProcessInfo");
                }
            }
            if ((state & Process2.State.Exited) != (Process2.State)0)
            {
                if (!HasExited)
                {
                    throw new InvalidOperationException("WaitTillExit");
                }
                if (!haveProcessHandle)
                {
                    throw new InvalidOperationException("NoProcessHandle");
                }
            }
        }

        private static StringBuilder BuildCommandLine(string executableFileName, string arguments)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string text = executableFileName.Trim();
            bool flag = text.StartsWith("\"", StringComparison.Ordinal) && text.EndsWith("\"", StringComparison.Ordinal);
            if (!flag)
            {
                stringBuilder.Append("\"");
            }
            stringBuilder.Append(text);
            if (!flag)
            {
                stringBuilder.Append("\"");
            }
            if (!string.IsNullOrEmpty(arguments))
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(arguments);
            }
            return stringBuilder;
        }

        private static void CreatePipeWithSecurityAttributes(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, NativeMethods.SECURITY_ATTRIBUTES lpPipeAttributes, int nSize)
        {
            bool flag = NativeMethods.CreatePipe(out hReadPipe, out hWritePipe, lpPipeAttributes, nSize);
            if (!flag || hReadPipe.IsInvalid || hWritePipe.IsInvalid)
            {
                throw new Win32Exception();
            }
        }

        private void CreatePipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
        {
            NativeMethods.SECURITY_ATTRIBUTES sECURITY_ATTRIBUTES = new NativeMethods.SECURITY_ATTRIBUTES();
            sECURITY_ATTRIBUTES.bInheritHandle = true;
            SafeFileHandle safeFileHandle = null;
            try
            {
                if (parentInputs)
                {
                    Process2.CreatePipeWithSecurityAttributes(out childHandle, out safeFileHandle, sECURITY_ATTRIBUTES, 0);
                }
                else
                {
                    Process2.CreatePipeWithSecurityAttributes(out safeFileHandle, out childHandle, sECURITY_ATTRIBUTES, 0);
                }
                if (!NativeMethods.DuplicateHandle(new HandleRef(this, NativeMethods.GetCurrentProcess()), safeFileHandle, new HandleRef(this, NativeMethods.GetCurrentProcess()), out parentHandle, 0, false, 2))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                if (safeFileHandle != null && !safeFileHandle.IsInvalid)
                {
                    safeFileHandle.Close();
                }
            }
        }

        private void SetProcessHandle(SafeProcessHandle processHandle)
        {
            m_processHandle = processHandle;
            haveProcessHandle = true;
            if (watchForExit)
            {
                EnsureWatchingForExit();
            }
        }

        private void EnsureWatchingForExit()
        {
            if (!watchingForExit)
            {
                bool flag = false;
                try
                {
                    Monitor.Enter(this, ref flag);
                    if (!watchingForExit)
                    {
                        watchingForExit = true;
                        try
                        {
                            waitHandle = new ProcessWaitHandle(m_processHandle);
                            registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(waitHandle, new WaitOrTimerCallback(CompletionCallback), null, -1, true);
                        }
                        catch
                        {
                            watchingForExit = false;
                            throw;
                        }
                    }
                }
                finally
                {
                    if (flag)
                    {
                        Monitor.Exit(this);
                    }
                }
            }
        }

        private void CompletionCallback(object context, bool wasSignaled)
        {
            StopWatchingForExit();
            //RaiseOnExited();
        }

        private void StopWatchingForExit()
        {
            if (watchingForExit)
            {
                bool flag = false;
                try
                {
                    Monitor.Enter(this, ref flag);
                    if (watchingForExit)
                    {
                        watchingForExit = false;
                        registeredWaitHandle.Unregister(null);
                        waitHandle.Close();
                        waitHandle = null;
                        registeredWaitHandle = null;
                    }
                }
                finally
                {
                    if (flag)
                    {
                        Monitor.Exit(this);
                    }
                }
            }
        }

        private void SetProcessId(int processId)
        {
            this.processId = processId;
            haveProcessId = true;
        }
    }
}
#pragma warning restore 0169
#pragma warning disable 0649