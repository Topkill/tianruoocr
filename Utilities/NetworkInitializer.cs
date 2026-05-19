using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TrOCR.Helper
{
    /// <summary>
    /// 通过 P/Invoke 调用 WSAStartup 来强制重新初始化 Winsock 网络堆栈。
    /// </summary>
    public static class NetworkInitializer
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct WSAData
        {
            public short wVersion;
            public short wHighVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
            public string szDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
            public string szSystemStatus;
            public short iMaxSockets;
            public short iMaxUdpDg;
            public IntPtr lpVendorInfo;
        }

        [DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int WSAStartup(short wVersionRequested, out WSAData wsaData);

        public static void Reinitialize()
        {
            try
            {
                short versionRequested = 514; // 等同于 C++ 中的 MAKEWORD(2, 2)
                WSAData wsaData;
                int error = WSAStartup(versionRequested, out wsaData);
                if (error != 0)
                {
                    Debug.WriteLine($"WSAStartup call failed with error: {error}");
                }
                else
                {
                    Debug.WriteLine("Successfully reinitialized Winsock.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception during network reinitialization: {ex.Message}");
            }
        }
    }
}