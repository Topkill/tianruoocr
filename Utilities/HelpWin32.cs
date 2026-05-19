using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TrOCR.Helper
{

    public class HelpWin32
    {
        // GDI / Display

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

        [DllImport("User32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr hObj);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        // Window messages

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam)
        {
            return HelpWin32.SendMessage(hWnd, msg, wParam, "");
        }

        // Clipboard

        [DllImport("User32.dll")]
        public static extern int SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        // Window / Focus

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        // Hotkeys

        [DllImport("user32.dll ", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll ", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        // Input

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        // Multimedia

        [DllImport("winmm.dll")]
        public static extern long mciSendString(string command, StringBuilder returnString, int returnSize, IntPtr hwndCallback);

        // Locale / String

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        // Timing

        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();

        // Structs

        public struct Size
        {

            public Size(int x, int y)
            {
                cx = x;
                cy = y;
            }

            public int cx;

            public int cy;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {

            public byte BlendOp;

            public byte BlendFlags;

            public byte SourceConstantAlpha;

            public byte AlphaFormat;
        }

        public struct Point
        {

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public int x;

            public int y;
        }

        // Enums

        [Flags]
        public enum KeyModifiers
        {

            None = 0,

            Alt = 1,

            Ctrl = 2,

            Shift = 4,

            WindowsKey = 8
        }

        // Inner classes

        public class IniFileHelper
        {

            [DllImport("kernel32")]
            public static extern int GetPrivateProfileString(string sectionName, string key, string defaultValue, byte[] returnBuffer, int size, string filePath);

            [DllImport("kernel32")]
            public static extern long WritePrivateProfileString(string sectionName, string key, string value, string filePath);

            public static string GetValue(string sectionName, string key, string filePath)
            {
                byte[] array = new byte[2048];
                int privateProfileString = GetPrivateProfileString(sectionName, key, "发生错误", array, 999, filePath);
                return Encoding.Default.GetString(array, 0, privateProfileString);
            }

            public static bool SetValue(string sectionName, string key, string value, string filePath)
            {
                bool result;
                try
                {
                    result = ((int)WritePrivateProfileString(sectionName, key, value, filePath) > 0);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }

            public static bool RemoveSection(string sectionName, string filePath)
            {
                bool result;
                try
                {
                    result = ((int)WritePrivateProfileString(sectionName, null, "", filePath) > 0);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }

            public static bool Removekey(string sectionName, string key, string filePath)
            {
                bool result;
                try
                {
                    result = ((int)WritePrivateProfileString(sectionName, key, null, filePath) > 0);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }
    }
}
