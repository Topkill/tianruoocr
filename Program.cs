﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using TrOCR.Helper;

namespace TrOCR
{
    /// <summary>
    /// 应用程序的主入口点和核心初始化类
    /// 负责应用程序启动、异常处理、配置初始化、更新检查等核心功能
    /// </summary>
    internal static class Program
    {
        // 导入 Windows API 函数
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //关键修改: 在这里添加 LoadLibrary 的声明，废弃，也无用了
         public static extern IntPtr LoadLibrary(string lpFileName);
        // 无用：private static extern bool SetDllDirectory(string lpPathName);
        /// <summary>
        /// DPI缩放因子
        /// </summary>
        public static float Factor = 1.0f;

        /// <summary>
        /// 应用程序入口点
        /// </summary>
        /// <param name="args">命令行参数</param>
        [STAThread]
        public static void Main(string[] args)
        {
           /* 
            *另一种定义dll依赖查找路径的方法，尝试修复使用收纳法后， PaddleOCR2初始化找不到依赖的问题。
            *经过测试，没有修复成功，把所有dll移动到lib文件夹后，PaddleOCR2初始化还是找不到依赖，只能把它相关的sdcb.paddle等dll依旧放在exe同级目录了
            try
            {
                // 1. 获取 lib 文件夹的绝对路径
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var libPath = Path.Combine(baseDir, "lib");

                // 2. 同时也把 PaddleOCR_data 的路径加进去（如果你还没把里面的东西移到 lib）
                // 如果你已经把 PaddleOCR_data 里的 dll 都移到 lib 了，下面这两行可以不要
                //var paddle64 = Path.Combine(baseDir, "PaddleOCR_data", "win_x64");
                //var rapid64 = Path.Combine(baseDir, "RapidOCR_data", "win_x64");

                // 3. 获取当前进程的环境变量 PATH
                string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";

                // 4. 把我们的路径拼接到最前面 (优先搜索我们的目录)
                // 注意：用分号 ; 分隔
                string newPathEnv = $"{libPath};{paddle64};{rapid64};{pathEnv}";

                // 5. 设置回当前进程的环境变量 (只影响本软件，不影响系统其他软件)
                //Environment.SetEnvironmentVariable("PATH", newPathEnv);//同下一行代码
                // 明确告诉所有人：我只改当前进程的变量，不污染系统，
                Environment.SetEnvironmentVariable("PATH", newPathEnv, EnvironmentVariableTarget.Process);

                // 【调试用】打印一下看看有没有设置成功
                System.Diagnostics.Debug.WriteLine($"PATH patched: {libPath}");
            }
            catch (Exception ex)
            {
                // 就算出错也不要阻断程序启动，万一系统环境本来就是好的呢
                System.Diagnostics.Debug.WriteLine($"设置环境变量失败: {ex.Message}");
            }*/
            try
            {
                // 设置应用程序视觉样式，放在main函数一开始，try之前也行，会更符合惯例，懒得改了。
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
               
                // ====================【核心修复开始】====================
                // 在调用 SetDefaultDllDirectories 锁定 DLL 搜索路径之前，
                // 强制访问一次 System.Drawing，触发 GDI+ 的加载。
                // 解决 Win7 下 "无法找到字体“?”" 的崩溃问题。
                // Win7 兼容性补丁
                // 必须在 SetDefaultDllDirectories 之前执行！
                // 作用：强制触发 .NET 加载并初始化 GDI+ 及其字体缓存。
                // 如果不加这句，在打过安全补丁的 Win7 上，后续锁定 DLL 路径会导致
                // GDI+ 无法加载字体依赖，引发 "ArgumentException: 无法找到字体" 崩溃。
                // =======================================================================
                try
                {
                    //变量无意义，只是为了强制访问一次
                    var _ = System.Drawing.SystemFonts.DefaultFont;
                }
                catch
                {
                    // 即使失败也不要阻断程序启动，这只是一个预热操作
                }
                //或者变一下变量名：
                /*try
                {
                   //变量无意义，只是为了强制访问一次
                   var forceLoad = System.Drawing.SystemFonts.DefaultFont;
                }
                catch
                {
                   // 即使出错也不要阻断，但这通常能成功预加载 gdiplus.dll
                }*/
                // -------------------------------------------------------------------------
                // 2.  开启 TLS 1.2 (解决win7系统 OpenAI等接口报错,
                // 经过测试，解决失败，win7的加密套件太老了，即使win7开启tls1.2，目标网站还是不认
                //目前只找到一个办法在win7，使用nginx反向代理目标网站(api地址)，然后软件设置接口填入nginx,这样能使用nginx的加密套件，跳过win7旧的加密套件
                //不知道clash这类代理软件行不行，解决原理是能跳过使用win7系统的老旧的加密套件，使用目标网站支持的新加密套件，满足这个原理的软件应该都行，不只nginx)
                // -------------------------------------------------------------------------
                // 下面代码写不写都行，写一下吧。
                // 注意ServicePointManager是全局的，只要有一处地方更改就行了。ServicePointManager.SecurityProtocol 是一个 静态（Static） 的全局属性
                // 我发现有些helper类里又设置了ServicePointManager，会覆盖掉program.cs里设置的。
                // 所以其实项目里有一处设置ServicePointManager的就行了，可以删掉helper类里的或者删掉program里的，我懒得改了。其实保留program里的删掉其他类里的更好一点
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol =
                        System.Net.SecurityProtocolType.Tls12 |
                        System.Net.SecurityProtocolType.Tls11 |
                        System.Net.SecurityProtocolType.Tls;
                }
                catch
                {
                    /// 兼容写法，防止老版 .NET 编译报错
                    System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
                }

                // ====================【核心修复结束】====================
                // 在程序启动的最开始告诉 Debug 和 Trace 将所有输出写入到 "debug_log.txt" 文件中
                // 1. 先确保 Data 文件夹存在
                string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                if (!Directory.Exists(dataDir))
                {
                    Directory.CreateDirectory(dataDir);
                }
                // 2. 然后再初始化日志
                string logPath = Path.Combine(dataDir, "debug_log.txt");
                var listener = new TextWriterTraceListener(logPath);
                Debug.Listeners.Add(listener);
                //Trace.Listeners.Add(listener)
                // 设置 AutoFlush <b>非常重要</b>，否则可能文件里什么都看不到
                Debug.AutoFlush = true;
                Debug.WriteLine("===== 应用程序启动，日志开始 =====");

                //【新增】使用 NativeLibrary.Load 抢先加载 Sdcb.PaddleOCR 的核心DLL
                //    这必须是 Main 方法中的第一件事。
                // ==========================================================
                //  PreloadPaddleOcrNativeLibs();
                // 1. 启用新的DLL搜索模式，这是使用 AddDllDirectory 的前提
                // 我们告诉系统，除了我们手动添加的目录，也别忘了搜索默认的系统目录
                TrOCRUtils.SetDefaultDllDirectories(0x00001000 | 0x00000400); // LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_USER_DIRS

                // 2. 根据当前进程的架构，动态地构建需要搜索的路径列表
                var pathsToAdd = new System.Collections.Generic.List<string>();
                var processArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;

                if (processArchitecture == System.Runtime.InteropServices.Architecture.X64)
                {
                    // 程序是64位进程，只添加64位的路径
                    pathsToAdd.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR_data", "win_x64"));
                    //pathsToAdd.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR2_data", "win_x64"));
                    //这里对PaddleOCR2无效，改用PreloadPaddleOcrNativeLibs()
                    pathsToAdd.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RapidOCR_data", "win_x64"));
                }
                else if (processArchitecture == System.Runtime.InteropServices.Architecture.X86)
                {
                    // 程序是32位进程，只添加32位的路径
                    pathsToAdd.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RapidOCR_data", "win_x86"));
                }

                // 通用的非托管/原生DLL /C++ DLL (比如 opencv, onnxruntime 等) exe同级目录找不到就去 lib 文件夹里找
                pathsToAdd.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib"));
                // 3. 遍历列表，将所有存在的目录添加到搜索路径中
                foreach (var path in pathsToAdd)
                {
                    if (Directory.Exists(path))
                    {
                        TrOCRUtils.AddDllDirectory(path);
                    }
                }
                // 设置异常处理模式和事件处理程序
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                // 检查是否已经运行了程序实例
                var eventName = "TianruoOcrInstance_" + Application.ExecutablePath.Replace(Path.DirectorySeparatorChar, '_');
                var programStarted = new EventWaitHandle(false, EventResetMode.AutoReset, eventName, out var needNew);
                if (!needNew)
                {
                    programStarted.Set();
                    CommonHelper.ShowHelpMsg("软件已经运行");
                    return;
                }

                // 初始化配置文件
                InitConfig();
                DealErrorConfig();
                StaticValue.LoadConfig();

                
                var version = Environment.OSVersion.Version;
                var value = new Version("6.1");
                Factor = CommonHelper.GetDpiFactor();
                if (version.CompareTo(value) >= 0)
                {
                    CommonHelper.SetProcessDPIAware();
                }

                // 处理启动参数
                if (args.Length != 0 && args[0] == "更新")
                {
                    new FmSetting
                    {
                        Start_set = ""
                    }.ShowDialog();
                }

                // 启动更新检查任务并运行主窗体
                // 只有当配置为 True 时，才启动自动更新检查
                if (IniHelper.GetValue("更新", "检测更新") == "True")
                {
                    Task.Factory.StartNew(CheckUpdate);
                }
                Application.Run(new FmMain());
            }
            catch (Exception ex)
            {
                // 记录详细的异常信息到日志文件
                try
                {
                    var logPath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "error.log");
                    var logDir = Path.GetDirectoryName(logPath2);
                    if (!Directory.Exists(logDir))
                    {
                        Directory.CreateDirectory(logDir);
                    }

                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 未处理异常:{ex}{new string('=', 80)}";
                    File.AppendAllText(logPath2, logEntry, Encoding.UTF8);
                }
                catch
                {
                    // 如果日志记录失败，忽略错误
                }

                // 显示用户友好的错误信息
                var errorMsg = $"程序启动时发生错误:{ex.Message}，详细信息已记录到 Data/error.log 文件中。";
                MessageBox.Show(errorMsg, "TrOCR 启动错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 预加载 Sdcb.PaddleOCR 需要的本地C++库，废弃，无用，查看源码发现这个库是写死的读取dll的目录位置，抢先加载也改变不了
        /// </summary>
        // private static void PreloadPaddleOcrNativeLibs()
        // {
        //     if (RuntimeInformation.ProcessArchitecture != Architecture.X64) return;

        //     string customPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR2_data", "win_x64");
        //     if (!Directory.Exists(customPath)) return;

        //     var libsToLoad = new[]
        //     {
        //         "common.dll",
        //         "onnxruntime.dll",
        //         "onnxruntime_providers_shared.dll",
        //         "openblas.dll",
        //         "opencv_videoio_ffmpeg4110_64.dll",
        //         "OpenCvSharpExtern.dll",
        //         "paddle2onnx.dll",
        //         "paddle_inference_c.dll"
        //     };

        //     foreach (var libName in libsToLoad)
        //     {
        //         string fullPath = Path.Combine(customPath, libName);
        //         try
        //         {
        //             if (File.Exists(fullPath))
        //             {
        //                 // --- 关键修改 2: 使用 LoadLibrary 代替 NativeLibrary.Load ---
        //                 IntPtr handle = LoadLibrary(fullPath);

        //                 // 检查加载是否成功。如果返回的句柄是0，则加载失败。
        //                 if (handle == IntPtr.Zero)
        //                 {
        //                     // 获取详细的Windows错误代码，方便调试
        //                     int errorCode = Marshal.GetLastWin32Error();
        //                     throw new DllNotFoundException($"无法加载本地库 '{fullPath}'。Windows 错误代码: {errorCode}");
        //                 }
        //                 System.Diagnostics.Debug.WriteLine($"Successfully pre-loaded: {fullPath}");
        //             }
        //             else
        //             {
        //                 System.Diagnostics.Debug.WriteLine($"Warning: Pre-load file not found: {fullPath}");
        //             }
        //         }
        //         catch (Exception ex)
        //         {
        //             MessageBox.Show($"抢先加载本地库失败: {fullPath}\n\n错误: {ex.Message}",
        //                             "致命错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //         }
        //     }
        // }
        /// <summary>
        /// 处理线程异常事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">线程异常事件参数</param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show("捕获到线程异常: " + e.Exception.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 处理未处理的异常事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">未处理异常事件参数</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("捕获到未经处理的异常: " + e.ExceptionObject.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        /// <summary>
        /// 检查应用程序更新
        /// </summary>
        public static void CheckUpdate()
        {
            try
            {
                // 1. 读取配置确定是否检查预发布版
                bool checkPreRelease = false;
                try
                {
                    // 从ini文件中读取配置，如果读取失败或值为"发生错误"，则默认为false
                    string settingValue = IniHelper.GetValue("更新", "CheckPreRelease");
                    if (settingValue != "发生错误" && !string.IsNullOrEmpty(settingValue))
                    {
                        checkPreRelease = Convert.ToBoolean(settingValue);
                    }
                }
                catch
                {
                    // 如果转换失败，保持默认值 false
                }

                // 2. 根据配置选择 API URL
                string apiUrl;
                if (checkPreRelease)
                {
                    // 获取所有版本列表（包括预发布版），最新的在最前面
                    apiUrl = "https://gh-proxy.com/https://api.github.com/repos/Topkill/tianruoocr/releases";
                    Debug.WriteLine("获取所有版本列表");
                }
                else
                {
                    // 只获取最新的稳定版
                    apiUrl = "https://gh-proxy.com/https://api.github.com/repos/Topkill/tianruoocr/releases/latest";
                    Debug.WriteLine("获取最新的稳定版");
                }

                // 获取最新版本信息
                var request = System.Net.WebRequest.Create(apiUrl) as System.Net.HttpWebRequest;
                request.UserAgent = "TianruoOCR";  // GitHub API 要求设置 User-Agent
                request.Accept = "application/vnd.github.v3+json";
                request.Timeout = 10000;  // 10秒超时

                // --- ETag 缓存机制开始 ---

                // 2a. 根据 API URL 读取对应的 ETag
                // 我们为 "所有版本" 和 "最新稳定版" 分别存储 ETag
                string etagKey = checkPreRelease ? "ETag_Releases" : "ETag_Latest";
                string storedEtag = IniHelper.GetValue("更新", etagKey);

                // 2b. 如果存在已保存的 ETag，将其加入到请求头中
                if (!string.IsNullOrEmpty(storedEtag) && storedEtag != "发生错误")
                {
                    request.Headers.Add("If-None-Match", storedEtag);
                    Debug.WriteLine($"Found ETag: {storedEtag}. Adding If-None-Match header.");
                }

                // --- ETag 缓存机制结束 ---
                using (var response = request.GetResponse())
                {
                    // 2c. 请求成功 (200 OK), 保存新的 ETag
                    string newEtag = response.Headers["ETag"];
                    if (!string.IsNullOrEmpty(newEtag))
                    {
                        IniHelper.SetValue("更新", etagKey, newEtag);
                        Debug.WriteLine($"Request successful. Saving new ETag: {newEtag}");
                    }
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var jsonText = reader.ReadToEnd();
                        JObject releaseData;

                        // 3. 处理不同的 JSON 响应结构
                        if (checkPreRelease)
                        {
                            // API /releases 返回一个数组，取第一个元素
                            var jsonArray = JArray.Parse(jsonText);
                            if (jsonArray.Count == 0)
                            {
                                CommonHelper.ShowHelpMsg("未找到任何发行版本。");
                                return;
                            }
                            releaseData = jsonArray[0] as JObject;
                        }
                        else
                        {
                            // API /releases/latest 返回单个对象
                            releaseData = JObject.Parse(jsonText);
                        }

                        // 4. 使用解析到的 releaseData 进行后续操作
                        // 获取版本号（去掉前面的 'v'）
                        var tagName = releaseData["tag_name"].Value<string>();
                        var newVersion = tagName.TrimStart('v', 'V');
                        var curVersion = System.Windows.Forms.Application.ProductVersion.Split('+')[0];

                        // 5. 调用新的、健壮的版本比较方法
                        if (!CheckVersion(newVersion, curVersion))
                        {
                            CommonHelper.ShowHelpMsg("当前已是最新版本");
                            return;
                        }

                        // 判断是否为预发布版，并添加到提示信息中
                        bool isPreRelease = releaseData["prerelease"].Value<bool>();
                        string preReleaseTag = isPreRelease ? " [预览版]" : "";
                        CommonHelper.ShowHelpMsg($"有新版本：{newVersion}{preReleaseTag}");

                        // 获取下载链接
                        var htmlUrl = releaseData["html_url"].Value<string>();
                        var assets = releaseData["assets"] as JArray;
                        string downloadUrl = null;

                        // 查找 exe 或 zip 文件的下载链接
                        if (assets != null && assets.Count > 0)
                        {
                            foreach (var asset in assets)
                            {
                                var name = asset["name"].Value<string>();
                                if (name.EndsWith(".exe") || name.EndsWith(".zip") || name.EndsWith(".rar") || name.EndsWith(".7z"))
                                {
                                    downloadUrl = asset["browser_download_url"].Value<string>();
                                    break;
                                }
                            }
                        }

                        // 获取更新说明（限制长度）
                        var body = releaseData["body"]?.Value<string>() ?? "无更新说明";
                        if (body.Length > 500)
                        {
                            body = body.Substring(0, 500) + "...";
                        }

                        // 显示更新提示
                        var message = $"发现新版本：{newVersion}{preReleaseTag}\n"; // 添加预览版标记
                        message += $"当前版本：{curVersion}\n\n";
                        if (isPreRelease)
                        {
                            message += "⚠️ 请注意：这是一个预览版本，用于测试新功能，可能存在未知问题或不稳定情况。建议普通用户等待正式版。\n\n";
                        }
                        message += $"更新内容：\n{body}\n\n";
                        message += "是否前往下载页面？";

                        if (MessageBox.Show(message, "发现新版本", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            // 如果有直接下载链接，使用下载链接；否则打开发布页面
                            Process.Start(downloadUrl ?? htmlUrl);
                        }
                    }
                }
            }
            catch (System.Net.WebException ex)
            {
                // --- ETag 缓存机制 - 处理 304 响应 ---

                // 2d. 检查是否收到了 304 Not Modified 响应
                if (ex.Response is HttpWebResponse httpResponse && httpResponse.StatusCode == HttpStatusCode.NotModified)
                {
                    // 这是缓存命中的情况，意味着远端没有新版本。
                    // 这种响应不计入速率限制，是我们期望的结果。
                    Debug.WriteLine("ETag matched. No new release found (304 Not Modified).");
                    CommonHelper.ShowHelpMsg("当前已是最新版本"); // 或者直接 return; 静默退出
                    return;
                }

                // --- ETag 缓存机制结束 ---
                // 网络错误，静默失败或显示简单提示
                if (ex.Status == System.Net.WebExceptionStatus.Timeout)
                {
                    // 超时不提示，避免影响用户体验
                    return;
                }
                CommonHelper.ShowHelpMsg("检查更新失败，请检查网络连接");
            }
            catch (Exception ex)
            {
                // 其他错误
                CommonHelper.ShowHelpMsg($"检查更新时出错：{ex.Message}");
            }
        }

        /// <summary>
        /// 比较版本号大小，支持预发布标签 (e.g., -beta, -rc.1)
        /// </summary>
        /// <param name="newVersionStr">新版本号</param>
        /// <param name="curVersionStr">当前版本号</param>
        /// <returns>如果有新版本返回true，否则返回false</returns>
        private static bool CheckVersion(string newVersionStr, string curVersionStr)
        {
            try
            {
                // 1. 将版本号在第一个 '-' 处分割，分离出 [数字部分] 和 [预发布标签]
                var newVersionParts = newVersionStr.Split(new[] { '-' }, 2);
                var curVersionParts = curVersionStr.Split(new[] { '-' }, 2);

                var newNumericPart = newVersionParts[0];
                var curNumericPart = curVersionParts[0];

                // 2. 使用 System.Version 类来安全、准确地比较数字部分
                var newVer = new Version(newNumericPart);
                var curVer = new Version(curNumericPart);

                int comparison = newVer.CompareTo(curVer);

                // 3. 如果数字部分不相等，直接得出结论
                if (comparison != 0)
                {
                    // 例如: 6.1 vs 6.0, 或者 5.9 vs 6.0
                    return comparison > 0;
                }

                // --- 至此，数字部分完全相同 (例如, 都是 6.0.0) ---

                // 4. 根据预发布标签的有无来判断
                bool newIsPreRelease = newVersionParts.Length > 1;
                bool curIsPreRelease = curVersionParts.Length > 1;

                // 规则: 稳定版 > 预发布版
                if (!newIsPreRelease && curIsPreRelease)
                {
                    // 新版是稳定版 (6.0.0), 当前是预发布版 (6.0.0-beta) -> 新版更"新"
                    return true;
                }

                if (newIsPreRelease && !curIsPreRelease)
                {
                    // 新版是预发布版 (6.0.0-beta), 当前是稳定版 (6.0.0) -> 新版不更"新"
                    return false;
                }

                if (!newIsPreRelease && !curIsPreRelease)
                {
                    // 两者都是同版本的稳定版 (6.0.0 vs 6.0.0) -> 没有更新
                    return false;
                }

                // 5. 两者都是同一数字版本的预发布版，比较标签
                // 例如: 6.0.0-rc.1 vs 6.0.0-beta.2
                // 简单的字符串比较可以覆盖大部分情况 ("rc.1" > "beta.2")
                return string.Compare(newVersionParts[1], curVersionParts[1], StringComparison.OrdinalIgnoreCase) > 0;
            }
            catch (Exception ex)
            {
                // 如果任何解析失败，回退到简单的字符串比较作为保险措施
                System.Diagnostics.Debug.WriteLine($"版本比较时发生错误: {ex.Message}。回退到字符串比较。");
                // 确保不会因为预发布标签导致错误的回退结果
                // 例如 "6.0-beta" vs "6.0"，简单的字符串比较会认为 "-beta"更大，这是错误的
                // 所以我们只在解析失败时做一个最基础的、不含标签的比较
                try
                {
                    var newNumericPart = newVersionStr.Split('-')[0];
                    var curNumericPart = curVersionStr.Split('-')[0];
                    return new Version(newNumericPart).CompareTo(new Version(curNumericPart)) > 0;
                }
                catch
                {
                    // 如果连这个都失败了，那只能放弃比较
                    return false;
                }
            }
        }
        /// <summary>
        /// 初始化配置文件(config.ini)，如果配置文件不存在则创建它
        /// </summary>
        private static void InitConfig()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Data");
            }

            if (!File.Exists(path))
            {
                using (File.Create(path))
                {
                }

                // 设置默认配置值
                IniHelper.SetValue("配置", "接口", "搜狗");
                IniHelper.SetValue("配置", "开机自启", "True");
                IniHelper.SetValue("配置", "快速翻译", "True");
                IniHelper.SetValue("配置", "识别弹窗", "True");
                IniHelper.SetValue("配置", "窗体动画", "窗体");
                IniHelper.SetValue("配置", "记录数目", "20");
                IniHelper.SetValue("配置", "自动保存", "True");
                IniHelper.SetValue("配置", "截图位置", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
                IniHelper.SetValue("配置", "翻译接口", "Bing2");
                IniHelper.SetValue("常规识别", "AutoCopyOcrResult", "False");
                // IniHelper.SetValue("常规识别", "AutoTranslateOcrResult", "False");
                IniHelper.SetValue("常规翻译", "AutoCopyOcrTranslation", "False");
                IniHelper.SetValue("配置", "AutoCopyInputTranslation", "False");
                IniHelper.SetValue("快捷键", "文字识别", "F4");
                IniHelper.SetValue("快捷键", "翻译文本", "F9");
                IniHelper.SetValue("快捷键", "记录界面", "请按下快捷键");
                IniHelper.SetValue("快捷键", "识别界面", "请按下快捷键");
                IniHelper.SetValue("快捷键", "静默识别", "请按下快捷键");
                IniHelper.SetValue("快捷键", "输入翻译", "请按下快捷键");
                IniHelper.SetValue("快捷键", "截图翻译", "请按下快捷键");
                IniHelper.SetValue("密钥_百度", "secret_id", "YsZKG1wha34PlDOPYaIrIIKO");
                IniHelper.SetValue("密钥_百度", "secret_key", "HPRZtdOHrdnnETVsZM2Nx7vbDkMfxrkD");
                IniHelper.SetValue("代理", "代理类型", "系统代理");
                IniHelper.SetValue("代理", "服务器", "");
                IniHelper.SetValue("代理", "端口", "");
                IniHelper.SetValue("代理", "需要密码", "False");
                IniHelper.SetValue("代理", "服务器账号", "");
                IniHelper.SetValue("代理", "服务器密码", "");
                IniHelper.SetValue("更新", "检测更新", "True");
                IniHelper.SetValue("更新", "更新间隔", "True");
                IniHelper.SetValue("更新", "间隔时间", "24");
                IniHelper.SetValue("更新", "更新地址", "https://github.com/Topkill/tianruoocr/releases");
                IniHelper.SetValue("更新", "CheckPreRelease","False");
                IniHelper.SetValue("截图音效", "自动保存", "True");
                IniHelper.SetValue("截图音效", "音效路径", "Data\\screenshot.wav");
                IniHelper.SetValue("截图音效", "粘贴板", "False");
                IniHelper.SetValue("工具栏", "合并", "False");
                IniHelper.SetValue("工具栏", "分段", "False");
                IniHelper.SetValue("工具栏", "分栏", "False");
                IniHelper.SetValue("工具栏", "拆分", "False");
                IniHelper.SetValue("工具栏", "检查", "False");
                IniHelper.SetValue("工具栏", "翻译", "False");
                IniHelper.SetValue("工具栏", "顶置", "True");
                IniHelper.SetValue("取色器", "类型", "RGB");
            }
        }

        /// <summary>
        /// 读取配置文件，处理错误的配置项
        /// </summary>
        private static void DealErrorConfig()
        {
            // 恢复发生错误的配置项为默认值
            if (IniHelper.GetValue("配置", "接口") == "发生错误")
            {
                IniHelper.SetValue("配置", "接口", "搜狗");
            }

            if (IniHelper.GetValue("配置", "开机自启") == "发生错误")
            {
                IniHelper.SetValue("配置", "开机自启", "True");
            }

            if (IniHelper.GetValue("配置", "快速翻译") == "发生错误")
            {
                IniHelper.SetValue("配置", "快速翻译", "True");
            }

            if (IniHelper.GetValue("配置", "识别弹窗") == "发生错误")
            {
                IniHelper.SetValue("配置", "识别弹窗", "True");
            }

            if (IniHelper.GetValue("配置", "窗体动画") == "发生错误")
            {
                IniHelper.SetValue("配置", "窗体动画", "窗体");
            }

            if (IniHelper.GetValue("配置", "记录数目") == "发生错误")
            {
                IniHelper.SetValue("配置", "记录数目", "20");
            }

            if (IniHelper.GetValue("配置", "自动保存") == "发生错误")
            {
                IniHelper.SetValue("配置", "自动保存", "True");
            }

            if (IniHelper.GetValue("常规识别", "AutoCopyOcrResult") == "发生错误")
            {
                IniHelper.SetValue("常规识别", "AutoCopyOcrResult", "False");
            }

            // if (IniHelper.GetValue("常规识别", "AutoTranslateOcrResult") == "发生错误")
            // {
            //     IniHelper.SetValue("常规识别", "AutoTranslateOcrResult", "False");
            // }

            if (IniHelper.GetValue("常规翻译", "AutoCopyOcrTranslation") == "发生错误")
            {
                IniHelper.SetValue("常规翻译", "AutoCopyOcrTranslation", "False");
            }

            if (IniHelper.GetValue("配置", "AutoCopyInputTranslation") == "发生错误")
            {
                IniHelper.SetValue("配置", "AutoCopyInputTranslation", "False");
            }

            if (IniHelper.GetValue("配置", "翻译接口") == "发生错误")
            {
                IniHelper.SetValue("配置", "翻译接口", "Bing2");
            }

            if (IniHelper.GetValue("配置", "截图位置") == "发生错误")
            {
                IniHelper.SetValue("配置", "截图位置", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            }

            if (IniHelper.GetValue("快捷键", "文字识别") == "发生错误")
            {
                IniHelper.SetValue("快捷键", "文字识别", "F4");
            }

            if (IniHelper.GetValue("快捷键", "翻译文本") == "发生错误")
            {
                IniHelper.SetValue("快捷键", "翻译文本", "F9");
            }

            if (IniHelper.GetValue("快捷键", "记录界面") == "发生错误")
            {
                IniHelper.SetValue("快捷键", "记录界面", "请按下快捷键");
            }

            if (IniHelper.GetValue("快捷键", "识别界面") == "发生错误")
            {
                IniHelper.SetValue("快捷键", "识别界面", "请按下快捷键");
            }

            if (IniHelper.GetValue("快捷键", "静默识别") == "发生错误")
            {
                IniHelper.SetValue("快捷键", "静默识别", "请按下快捷键");
            }
            if (IniHelper.GetValue("快捷键", "截图翻译") == "发生错误")
            {
                IniHelper.SetValue("快捷键", "截图翻译", "请按下快捷键");
            }


            if (IniHelper.GetValue("快捷键", "输入翻译") == "发生错误")
            {
                IniHelper.SetValue("快捷键", "输入翻译", "请按下快捷键");
            }

            if (IniHelper.GetValue("密钥_百度", "secret_id") == "发生错误")
            {
                IniHelper.SetValue("密钥_百度", "secret_id", "YsZKG1wha34PlDOPYaIrIIKO");
            }

            if (IniHelper.GetValue("密钥_百度", "secret_key") == "发生错误")
            {
                IniHelper.SetValue("密钥_百度", "secret_key", "HPRZtdOHrdnnETVsZM2Nx7vbDkMfxrkD");
            }

            if (IniHelper.GetValue("代理", "代理类型") == "发生错误")
            {
                IniHelper.SetValue("代理", "代理类型", "系统代理");
            }

            if (IniHelper.GetValue("代理", "服务器") == "发生错误")
            {
                IniHelper.SetValue("代理", "服务器", "");
            }

            if (IniHelper.GetValue("代理", "端口") == "发生错误")
            {
                IniHelper.SetValue("代理", "端口", "");
            }

            if (IniHelper.GetValue("代理", "需要密码") == "发生错误")
            {
                IniHelper.SetValue("代理", "需要密码", "False");
            }

            if (IniHelper.GetValue("代理", "服务器账号") == "发生错误")
            {
                IniHelper.SetValue("代理", "服务器账号", "");
            }

            if (IniHelper.GetValue("代理", "服务器密码") == "发生错误")
            {
                IniHelper.SetValue("代理", "服务器密码", "");
            }

            if (IniHelper.GetValue("更新", "检测更新") == "发生错误")
            {
                IniHelper.SetValue("更新", "检测更新", "True");
            }

            if (IniHelper.GetValue("更新", "更新间隔") == "发生错误")
            {
                IniHelper.SetValue("更新", "更新间隔", "True");
            }

            if (IniHelper.GetValue("更新", "间隔时间") == "发生错误")
            {
                IniHelper.SetValue("更新", "间隔时间", "24");
            }

            if (IniHelper.GetValue("更新", "更新地址") == "发生错误")
            {
                IniHelper.SetValue("更新", "更新地址", "https://github.com/Topkill/tianruoocr/releases");
            }
            if (IniHelper.GetValue("更新", "CheckPreRelease") == "发生错误")
            {
                IniHelper.SetValue("更新", "CheckPreRelease", "False");
            }

            if (IniHelper.GetValue("截图音效", "自动保存") == "发生错误")
            {
                IniHelper.SetValue("截图音效", "自动保存", "True");
            }

            if (IniHelper.GetValue("截图音效", "音效路径") == "发生错误")
            {
                IniHelper.SetValue("截图音效", "音效路径", "Data\\screenshot.wav");
            }

            if (IniHelper.GetValue("截图音效", "粘贴板") == "发生错误")
            {
                IniHelper.SetValue("截图音效", "粘贴板", "False");
            }

            if (IniHelper.GetValue("工具栏", "合并") == "发生错误")
            {
                IniHelper.SetValue("工具栏", "合并", "False");
            }

            if (IniHelper.GetValue("工具栏", "拆分") == "发生错误")
            {
                IniHelper.SetValue("工具栏", "拆分", "False");
            }

            if (IniHelper.GetValue("工具栏", "检查") == "发生错误")
            {
                IniHelper.SetValue("工具栏", "检查", "False");
            }

            if (IniHelper.GetValue("工具栏", "翻译") == "发生错误")
            {
                IniHelper.SetValue("工具栏", "翻译", "False");
            }

            if (IniHelper.GetValue("工具栏", "分段") == "发生错误")
            {
                IniHelper.SetValue("工具栏", "分段", "False");
            }

            if (IniHelper.GetValue("工具栏", "分栏") == "发生错误")
            {
                IniHelper.SetValue("工具栏", "分栏", "False");
            }

            if (IniHelper.GetValue("工具栏", "顶置") == "发生错误")
            {
                IniHelper.SetValue("工具栏", "顶置", "True");
            }

            if (IniHelper.GetValue("取色器", "类型") == "发生错误")
            {
                IniHelper.SetValue("取色器", "类型", "RGB");
            }

            if (IniHelper.GetValue("特殊", "ali_cookie") == "发生错误")
            {
                IniHelper.SetValue("特殊", "ali_cookie",
                    "cna=noXhE38FHGkCAXDve7YaZ8Tn; cnz=noXhE4/VhB8CAbZ773ApeV14; isg=BGJi2c2YTeeP6FG7S_Re8kveu-jEs2bNwToQnKz7jlWAfwL5lEO23eh9q3km9N5l; ");
            }

            if (IniHelper.GetValue("特殊", "ali_account") == "发生错误")
            {
                IniHelper.SetValue("特殊", "ali_account", "");
            }

            if (IniHelper.GetValue("特殊", "ali_password") == "发生错误")
            {
                IniHelper.SetValue("特殊", "ali_password", "");
            }
        }
    }
}