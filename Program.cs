using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            try
            {
                // 设置应用程序视觉样式，放在main函数一开始，try之前也行，会更符合惯例，懒得改了。
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // ====================【核心修复开始】====================
                // 在调用 SetDefaultDllDirectories 锁定 DLL 搜索路径之前，
                // 强制访问一次 System.Drawing，触发 GDI+ 的加载。
                // 解决 Win7 下 "无法找到字体"?"" 的崩溃问题。
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
                    Task.Factory.StartNew(UpdateChecker.CheckUpdate);
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
                    File.AppendAllText(logPath2, logEntry, System.Text.Encoding.UTF8);
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
        /// 将值为"发生错误"的配置项恢复为默认值
        /// </summary>
        private static void DealErrorConfig()
        {
            // 配置项默认值表：(section, key, defaultValue)
            var defaults = new (string section, string key, string defaultValue)[]
            {
                ("配置", "接口", "搜狗"),
                ("配置", "开机自启", "True"),
                ("配置", "快速翻译", "True"),
                ("配置", "识别弹窗", "True"),
                ("配置", "窗体动画", "窗体"),
                ("配置", "记录数目", "20"),
                ("配置", "自动保存", "True"),
                ("常规识别", "AutoCopyOcrResult", "False"),
                ("常规翻译", "AutoCopyOcrTranslation", "False"),
                ("配置", "AutoCopyInputTranslation", "False"),
                ("配置", "翻译接口", "Bing2"),
                ("快捷键", "文字识别", "F4"),
                ("快捷键", "翻译文本", "F9"),
                ("快捷键", "记录界面", "请按下快捷键"),
                ("快捷键", "识别界面", "请按下快捷键"),
                ("快捷键", "静默识别", "请按下快捷键"),
                ("快捷键", "截图翻译", "请按下快捷键"),
                ("快捷键", "输入翻译", "请按下快捷键"),
                ("密钥_百度", "secret_id", "YsZKG1wha34PlDOPYaIrIIKO"),
                ("密钥_百度", "secret_key", "HPRZtdOHrdnnETVsZM2Nx7vbDkMfxrkD"),
                ("代理", "代理类型", "系统代理"),
                ("代理", "服务器", ""),
                ("代理", "端口", ""),
                ("代理", "需要密码", "False"),
                ("代理", "服务器账号", ""),
                ("代理", "服务器密码", ""),
                ("更新", "检测更新", "True"),
                ("更新", "更新间隔", "True"),
                ("更新", "间隔时间", "24"),
                ("更新", "更新地址", "https://github.com/Topkill/tianruoocr/releases"),
                ("更新", "CheckPreRelease", "False"),
                ("截图音效", "自动保存", "True"),
                ("截图音效", "音效路径", "Data\\screenshot.wav"),
                ("截图音效", "粘贴板", "False"),
                ("工具栏", "合并", "False"),
                ("工具栏", "拆分", "False"),
                ("工具栏", "检查", "False"),
                ("工具栏", "翻译", "False"),
                ("工具栏", "分段", "False"),
                ("工具栏", "分栏", "False"),
                ("工具栏", "顶置", "True"),
                ("取色器", "类型", "RGB"),
                ("特殊", "ali_cookie", "cna=noXhE38FHGkCAXDve7YaZ8Tn; cnz=noXhE4/VhB8CAbZ773ApeV14; isg=BGJi2c2YTeeP6FG7S_Re8kveu-jEs2bNwToQnKz7jlWAfwL5lEO23eh9q3km9N5l; "),
                ("特殊", "ali_account", ""),
                ("特殊", "ali_password", ""),
            };

            foreach (var (section, key, defaultValue) in defaults)
            {
                if (IniHelper.GetValue(section, key) == "发生错误")
                    IniHelper.SetValue(section, key, defaultValue);
            }

            // 特殊处理：需要运行时路径拼接的配置项
            if (IniHelper.GetValue("配置", "截图位置") == "发生错误")
            {
                IniHelper.SetValue("配置", "截图位置", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            }
        }
    }
}
