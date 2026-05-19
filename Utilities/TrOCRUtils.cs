using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;


namespace TrOCR.Helper
{
    public static class TrOCRUtils
    {
        // 定义 LOAD_LIBRARY_SEARCH_DEFAULT_DIRS flag
        // 这个flag告诉系统在搜索DLL时包含标准的搜索路径（如System32）
        private const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
        private const uint LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400;

        // 导入 SetDefaultDllDirectories 函数
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDefaultDllDirectories(uint DirectoryFlags);

        // 导入 AddDllDirectory 函数
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr AddDllDirectory(string NewDirectory);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetProcessWorkingSetSize(IntPtr process,
            IntPtr minimumWorkingSetSize, IntPtr maximumWorkingSetSize);

        public static void CleanMemory()
        {
            //垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //释放工作集内存
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (IntPtr)(-1), (IntPtr)(-1));
            }
        }

        /// <summary>
        /// 从Ini文件中加载配置项，如果发生错误或找不到，则返回默认值。
        /// </summary>
        /// <param name="section">Ini文件中的节名</param>
        /// <param name="key">Ini文件中的键名</param>
        /// <param name="defaultValue">发生错误时返回的默认值</param>
        /// <returns>配置值或默认值</returns>
        public static string LoadSetting(string section, string key, string defaultValue)
        {
            string value = IniHelper.GetValue(section, key);
            // 判断是否获取失败
            if (value == "发生错误")
            {
                return defaultValue;
            }
            return value;
        }
        public static bool LoadSetting(string section, string key, bool defaultValue)
        {
            string value = IniHelper.GetValue(section, key);
            // 判断是否获取失败
            if (value == "发生错误")
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static string ConvertToAbsolutePath(string path)
        {
            // 如果为空，直接返回，交给后续的“默认值逻辑”处理
            if (string.IsNullOrWhiteSpace(path)) return path;

            // 如果已经是绝对路径，直接返回
            if (Path.IsPathRooted(path)) return path;

            // 如果是相对路径，与程序运行目录拼接
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
        }
        /// <summary>
        /// 尝试将绝对路径转换为相对于程序运行目录的相对路径
        /// </summary>
        /// <param name="fullPath">选择的完整路径</param>
        /// <returns>如果是程序目录下的文件，返回相对路径；否则返回原路径</returns>
        public static string ConvertToRelativePathIfPossible(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return "";
            fullPath = fullPath.Trim().Trim('"'); // 去掉可能存在的双引号
            //如果是相对路径，直接返回
            if (!Path.IsPathRooted(fullPath))
            {
                return fullPath;
            }

            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                // 确保以反斜杠结尾，防止类似 "C:\App" 匹配 "C:\App2" 的误判
                if (!appPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    appPath += Path.DirectorySeparatorChar;
                }

                // 获取完整路径的标准形式
                string standardFullPath = Path.GetFullPath(fullPath);

                // 判断是否包含在程序目录内 (忽略大小写)
                if (standardFullPath.StartsWith(appPath, StringComparison.OrdinalIgnoreCase))
                {
                    // 截取掉前面的程序路径部分，得到相对路径
                    return standardFullPath.Substring(appPath.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }
            }
            catch
            {
                // 路径格式异常等情况，不做处理，返回原值
            }

            return fullPath;
        }

    }
}