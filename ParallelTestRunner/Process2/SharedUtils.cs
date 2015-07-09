using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
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
    internal static class SharedUtils
    {
        internal const int UnknownEnvironment = 0;
        internal const int W2kEnvironment = 1;
        internal const int NtEnvironment = 2;
        internal const int NonNtEnvironment = 3;
        private static volatile int environment = 0;
        private static object s_InternalSyncObject;
        private static object InternalSyncObject
        {
            get
            {
                if (SharedUtils.s_InternalSyncObject == null)
                {
                    object value = new object();
                    Interlocked.CompareExchange(ref SharedUtils.s_InternalSyncObject, value, null);
                }
                return SharedUtils.s_InternalSyncObject;
            }
        }
        internal static int CurrentEnvironment
        {
            get
            {
                if (SharedUtils.environment == 0)
                {
                    lock (SharedUtils.InternalSyncObject)
                    {
                        if (SharedUtils.environment == 0)
                        {
                            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            {
                                if (Environment.OSVersion.Version.Major >= 5)
                                {
                                    SharedUtils.environment = 1;
                                }
                                else
                                {
                                    SharedUtils.environment = 2;
                                }
                            }
                            else
                            {
                                SharedUtils.environment = 3;
                            }
                        }
                    }
                }
                return SharedUtils.environment;
            }
        }
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal static Win32Exception CreateSafeWin32Exception()
        {
            return SharedUtils.CreateSafeWin32Exception(0);
        }
        internal static Win32Exception CreateSafeWin32Exception(int error)
        {
            Win32Exception result = null;
            SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
            securityPermission.Assert();
            try
            {
                if (error == 0)
                {
                    result = new Win32Exception();
                }
                else
                {
                    result = new Win32Exception(error);
                }
            }
            finally
            {
                CodeAccessPermission.RevertAssert();
            }
            return result;
        }
        internal static void CheckEnvironment()
        {
            if (SharedUtils.CurrentEnvironment == 3)
            {
                throw new PlatformNotSupportedException("WinNTRequired");
            }
        }
        internal static void CheckNtEnvironment()
        {
            if (SharedUtils.CurrentEnvironment == 2)
            {
                throw new PlatformNotSupportedException("Win2000Required");
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject", ExactSpelling = true, SetLastError = true)]
        private static extern int WaitForSingleObjectDontCallThis(SafeWaitHandle handle, int timeout);
        internal static string GetLatestBuildDllDirectory(string machineName)
        {
            string result = "";
            RegistryKey registryKey = null;
            RegistryKey registryKey2 = null;
            RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
            registryPermission.Assert();
            try
            {
                if (machineName.Equals("."))
                {
                    return SharedUtils.GetLocalBuildDirectory();
                }
                registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName);
                if (registryKey == null)
                {
                    throw new InvalidOperationException("RegKeyMissingShort");
                }
                registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework");
                if (registryKey2 != null)
                {
                    string text = (string)registryKey2.GetValue("InstallRoot");
                    if (text != null && text != string.Empty)
                    {
                        string text2 = string.Concat(new object[]
                        {
                            "v",
                            Environment.Version.Major,
                            ".",
                            Environment.Version.Minor
                        });
                        RegistryKey registryKey3 = registryKey2.OpenSubKey("policy");
                        string text3 = null;
                        if (registryKey3 != null)
                        {
                            try
                            {
                                RegistryKey registryKey4 = registryKey3.OpenSubKey(text2);
                                if (registryKey4 != null)
                                {
                                    try
                                    {
                                        text3 = text2 + "." + SharedUtils.GetLargestBuildNumberFromKey(registryKey4);
                                        goto IL_29A;
                                    }
                                    finally
                                    {
                                        registryKey4.Close();
                                    }
                                }
                                string[] subKeyNames = registryKey3.GetSubKeyNames();
                                int[] array = new int[]
                                {
                                    -1,
                                    -1,
                                    -1
                                };
                                for (int i = 0; i < subKeyNames.Length; i++)
                                {
                                    string text4 = subKeyNames[i];
                                    if (text4.Length > 1 && text4[0] == 'v' && text4.Contains("."))
                                    {
                                        int[] array2 = new int[]
                                        {
                                            -1,
                                            -1,
                                            -1
                                        };
                                        string[] array3 = text4.Substring(1).Split(new char[]
                                        {
                                            '.'
                                        });
                                        if (array3.Length == 2 && int.TryParse(array3[0], out array2[0]) && int.TryParse(array3[1], out array2[1]))
                                        {
                                            RegistryKey registryKey5 = registryKey3.OpenSubKey(text4);
                                            if (registryKey5 != null)
                                            {
                                                try
                                                {
                                                    array2[2] = SharedUtils.GetLargestBuildNumberFromKey(registryKey5);
                                                    if (array2[0] > array[0] || (array2[0] == array[0] && array2[1] > array[1]))
                                                    {
                                                        array = array2;
                                                    }
                                                }
                                                finally
                                                {
                                                    registryKey5.Close();
                                                }
                                            }
                                        }
                                    }
                                }
                                text3 = string.Concat(new object[]
                                {
                                    "v",
                                    array[0],
                                    ".",
                                    array[1],
                                    ".",
                                    array[2]
                                });
                            IL_29A: ;
                            }
                            finally
                            {
                                registryKey3.Close();
                            }
                            if (text3 != null && text3 != string.Empty)
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                stringBuilder.Append(text);
                                if (!text.EndsWith("\\", StringComparison.Ordinal))
                                {
                                    stringBuilder.Append("\\");
                                }
                                stringBuilder.Append(text3);
                                result = stringBuilder.ToString();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (registryKey2 != null)
                {
                    registryKey2.Close();
                }
                if (registryKey != null)
                {
                    registryKey.Close();
                }
                CodeAccessPermission.RevertAssert();
            }
            return result;
        }
        private static bool SafeWaitForMutex(Mutex mutexIn, ref Mutex mutexOut)
        {
            while (SharedUtils.SafeWaitForMutexOnce(mutexIn, ref mutexOut))
            {
                if (mutexOut != null)
                {
                    return true;
                }
                Thread.Sleep(0);
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool SafeWaitForMutexOnce(Mutex mutexIn, ref Mutex mutexOut)
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            bool result;
            try
            {
            }
            finally
            {
                Thread.BeginCriticalRegion();
                Thread.BeginThreadAffinity();
                int num = SharedUtils.WaitForSingleObjectDontCallThis(mutexIn.SafeWaitHandle, 500);
                int num2 = num;
                if (num2 != 0 && num2 != 128)
                {
                    result = (num2 == 258);
                }
                else
                {
                    mutexOut = mutexIn;
                    result = true;
                }
                if (mutexOut == null)
                {
                    Thread.EndThreadAffinity();
                    Thread.EndCriticalRegion();
                }
            }
            return result;
        }
        private static int GetLargestBuildNumberFromKey(RegistryKey rootKey)
        {
            int num = -1;
            string[] valueNames = rootKey.GetValueNames();
            for (int i = 0; i < valueNames.Length; i++)
            {
                int num2;
                if (int.TryParse(valueNames[i], out num2))
                {
                    num = ((num > num2) ? num : num2);
                }
            }
            return num;
        }
        private static string GetLocalBuildDirectory()
        {
            return RuntimeEnvironment.GetRuntimeDirectory();
        }
    }
}
