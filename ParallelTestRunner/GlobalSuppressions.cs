// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "ParallelTestRunner.Process2.NativeMethods+STARTUPINFO", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "ParallelTestRunner.Process2.NativeMethods+SECURITY_ATTRIBUTES", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member", Target = "ParallelTestRunner.Process2.Process2.#HasExited", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope = "member", Target = "ParallelTestRunner.Process2.Process2.#RaiseOnExited()", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope = "member", Target = "ParallelTestRunner.Process2.Process2.#EnsureWatchingForExit()", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope = "member", Target = "ParallelTestRunner.Process2.Process2.#StopWatchingForExit()", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.SafeLocalMemHandle.#LocalFree(System.IntPtr)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.SharedUtils.#WaitForSingleObjectDontCallThis(Microsoft.Win32.SafeHandles.SafeWaitHandle,System.Int32)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.Win32Native.#LocalFree(System.IntPtr)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.Win32Native.#GetModuleHandle(System.String)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.Win32Native.#GetFileType(Microsoft.Win32.SafeHandles.SafeFileHandle)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.Win32Native.#GetProcAddress(System.IntPtr,System.String)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.Win32Native.#CreateFile(System.String,System.Int32,System.IO.FileShare,ParallelTestRunner.Process2.Win32Native+SECURITY_ATTRIBUTES,System.IO.FileMode,System.Int32,System.IntPtr)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.Win32Native.#SetErrorMode_VistaAndOlder(System.Int32)", Justification = "Reviewed. Suppression is OK here.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "ParallelTestRunner.Process2.Win32Native.#SetErrorMode_Win7AndNewer(System.Int32,System.Int32&)", Justification = "Reviewed. Suppression is OK here.")]
