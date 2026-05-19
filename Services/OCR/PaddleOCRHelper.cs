using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using PaddleOCRSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading; // <-- 引入命名空间以使用 LazyThreadSafetyMode
//支持ppocrv5，支持64位，不支持32位，需要 CPU 支持 AVX指令集
namespace TrOCR.Helper
{
    /// <summary>
    /// PaddleOCR离线识别帮助类 (使用 Lazy<T> 实现的最终线程安全版本)
    /// 采用单例模式和懒加载，支持资源回收
    /// </summary>
    public sealed class PaddleOCRHelper : IDisposable
    {
        // === 新增：公开默认路径常量 (注意：这里使用相对路径，方便 UI 显示和处理) ===
        public const string DefaultModelDir = @"PaddleOCR_data\win_x64\inference";
        public const string DefaultDetModel = @"PaddleOCR_data\win_x64\inference\PP-OCRv5_mobile_det_infer";
        public const string DefaultClsModel = @"PaddleOCR_data\win_x64\inference\ch_ppocr_mobile_v5.0_cls_infer";
        public const string DefaultRecModel = @"PaddleOCR_data\win_x64\inference\PP-OCRv5_mobile_rec_infer";
        public const string DefaultKeys = @"PaddleOCR_data\win_x64\inference\ppocr_keys.txt";

        // --- 修改点 : 使用 Lazy<T> 实现单例 ---
        // 替换掉原来的 _instance 和 _lock 字段
        private static Lazy<PaddleOCRHelper> _lazyInstance =
            new Lazy<PaddleOCRHelper>(() => new PaddleOCRHelper(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static PaddleOCRHelper Instance => _lazyInstance.Value;

        // --- 新增: 为 Reset 方法的原子性操作保留一个锁 ---
        private static readonly object _resetLock = new object();

        private PaddleOCREngine _engine;
        private readonly Architecture _architecture;
        private bool _disposed = false;

        // 构造函数依然是私有的
        private PaddleOCRHelper()
        {
            // --- 优化：在每次初始化时都重新声明DLL搜索路径 ---
            var processArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;
            if (processArchitecture == System.Runtime.InteropServices.Architecture.X64)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR_data", "win_x64");
                if (Directory.Exists(path))
                {
                    // TrOCRUtils.AddDllDirectory 是 Program.cs 中 AddDllDirectory 的包装器
                    TrOCRUtils.AddDllDirectory(path);
                }
            }
            else if (processArchitecture == System.Runtime.InteropServices.Architecture.X86)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR_data", "win_x86");
                if (Directory.Exists(path))
                {
                    TrOCRUtils.AddDllDirectory(path);
                }
            }
            // --- 优化结束 ---

            _architecture = RuntimeInformation.ProcessArchitecture ;

            if (_architecture != Architecture.X64)
                throw new NotSupportedException("***PaddleOCR仅支持64位系统, 不支持32位系统***");

            InitializeEngine();
        }

        // InitializeEngine, Execute, ImageToBytes 等实例方法保持不变...
        private void InitializeEngine()
        {
            try
            {
                // 从INI配置文件读取模型路径及配置
                string detPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR", "Det"));
                string clsPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR", "Cls"));
                string recPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR", "Rec"));
                string keysPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR", "Keys"));
                string advancedConfigPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR", "AdvancedConfig"));

                // 如果配置为空，使用默认路径
                if (string.IsNullOrEmpty(detPath) || string.IsNullOrEmpty(clsPath) ||
                    string.IsNullOrEmpty(recPath) || string.IsNullOrEmpty(keysPath))
                {
                    // 使用默认路径
                    string rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR_data", "win_x64");
                    string modelPath = Path.Combine(rootDir, "inference");

                    detPath = string.IsNullOrEmpty(detPath) ? Path.Combine(modelPath, "PP-OCRv5_mobile_det_infer") : detPath;
                    clsPath = string.IsNullOrEmpty(clsPath) ? Path.Combine(modelPath, "ch_ppocr_mobile_v5.0_cls_infer") : clsPath;
                    recPath = string.IsNullOrEmpty(recPath) ? Path.Combine(modelPath, "PP-OCRv5_mobile_rec_infer") : recPath;
                    keysPath = string.IsNullOrEmpty(keysPath) ? Path.Combine(modelPath, "ppocr_keys.txt") : keysPath;
                }

                // 创建模型配置对象
                OCRModelConfig config = new OCRModelConfig();
                config.det_infer = detPath;
                config.cls_infer = clsPath;
                config.rec_infer = recPath;
                config.keys = keysPath;

                // --- 加载用户指定的高级参数JSON文件 ---
                string ocrParamsJson = ""; // 默认为空，引擎将使用内部默认参数(exe所在目录+\inference\PaddleOCR.config.json)，如果这文件不存在就报错，所以我要修改默认文件路径

                // 检查用户是否配置了高级参数文件路径，并且该文件确实存在
                if (!string.IsNullOrEmpty(advancedConfigPath) && File.Exists(advancedConfigPath))
                {
                    try
                    {
                        // 读取用户指定的JSON文件内容
                        ocrParamsJson = File.ReadAllText(advancedConfigPath);
                        Debug.WriteLine($"成功加载高级配置文件: {advancedConfigPath}");
                    }
                    catch (Exception ex)
                    {
                        // 如果读取文件失败，记录日志，但程序继续使用默认参数运行
                        Debug.WriteLine($"读取高级配置文件 '{advancedConfigPath}' 失败: {ex.Message}");
                    }
                }
                else
                {
                    ocrParamsJson=Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR_data", "win_x64", "inference", "PaddleOCR.config.json");
                    Debug.WriteLine($"高级配置文件为空，使用默认配置文件目录: {ocrParamsJson}");
                }
                
                // 使用模型配置和高级参数初始化引擎
                _engine = new PaddleOCREngine(config, ocrParamsJson);
            }
            catch (Exception ex)
            {
                // 初始化失败时，确保 _engine 为 null，以便 Execute 方法能正确报告错误。
                _engine = null;

                //throw new Exception($"PaddleOCR引擎初始化失败: {ex.Message}");
                // 直接 "throw;" 会保留原始异常的完整堆栈信息
                Debug.WriteLine($"[PaddleOCRHelper] InitializeEngine 失败: {ex}"); // 增加日志
                throw;
            }
        }

        /// <summary>
        /// 获取配置值的辅助方法
        /// </summary>
        private string GetConfigValue(string section, string key)
        {
            try
            {
                var value = TrOCR.Helper.IniHelper.GetValue(section, key);
                return value == "发生错误" ? "" : value;
            }
            catch
            {
                return "";
            }
        }
        public static string RecognizeText(Image image)
        {
            try
            {
                return Instance.Execute(image);
            }
            catch (Exception ex)
            {
                // 关键：捕获初始化或执行期间的任何异常
                // 调用 Reset() 来清除“中毒”的 Lazy 实例
                Reset();
                // 关键：使用 ex.ToString() 来包含完整的堆栈信息
                Debug.WriteLine($"[PaddleOCRHelper] RecognizeText 失败: {ex}"); // 增加日志
                return $"***PaddleOCR识别失败: {ex.ToString()}***";
            }
        }
      
        private string Execute(Image image)
        {
            try
            {   
                //用不到了
                // if (_architecture != Architecture.X64)
                //     return "***PaddleOCR不支持32位系统，请使用64位系统***";

                // 此处的检查现在可以捕获初始化失败的情况。
                if (_engine == null)
                    return "***PaddleOCR引擎未初始化***";

                if (image == null)
                    return "***图像为空***";

                // byte[] imageBytes = ImageToBytes(image);
                // var ocrResult = _engine.DetectText(imageBytes);
                var ocrResult = _engine.DetectText((Bitmap)image);
                Debug.WriteLine($"paddleOCR识别结果：{ocrResult}");
                if (ocrResult?.TextBlocks == null || ocrResult.TextBlocks.Count == 0)
                    return "***该区域未发现文本***"; 
                var sb = new StringBuilder();
                foreach (var textBlock in ocrResult.TextBlocks)
                {
                    if (!string.IsNullOrWhiteSpace(textBlock.Text))
                        sb.AppendLine(textBlock.Text);
                }

                string result = sb.ToString().Trim();
                Debug.WriteLine($"paddleOCR识别结果：{result}");
                return string.IsNullOrEmpty(result) ? "***该区域未发现文本***" : result;
            }
            catch (Exception ex)
            {
                return $"***PaddleOCR识别失败: {ex.Message}***";
            }
        }
        private byte[] ImageToBytes(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }


        // 完整的 Dispose 模式和终结器 (最佳实践)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) { /* 清理托管资源 */ }
                _engine?.Dispose();
                _engine = null;
                _disposed = true;
            }
        }
        ~PaddleOCRHelper()
        {
            try
            {
                Dispose(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PaddleOCR Error in finalizer: {ex.Message}");
                return;
            }
            
        } 


        // --- 修改点 3: 更新 Reset 方法以适配 Lazy<T> ---
        public static void Reset()
        {
            // 锁定以确保 Reset 操作的原子性，防止多个线程同时重置
            lock (_resetLock)
            {
                // 检查实例是否已经被创建过
                if (_lazyInstance.IsValueCreated)
                {
                    try
                    {
                        // 尝试释放旧实例。如果初始化失败，访问.Value会抛出异常
                        _lazyInstance.Value.Dispose();
                    }
                    catch
                    {
                        // 忽略异常，因为实例本身就是坏的，继续执行重置核心逻辑
                    }
                }
                // 创建一个全新的 Lazy<T> 实例来替换旧的。
                // 下一次访问 .Instance 时，会自动创建新的 PaddleOCRHelper 对象。
                _lazyInstance = new Lazy<PaddleOCRHelper>(() => new PaddleOCRHelper(), LazyThreadSafetyMode.ExecutionAndPublication);
                // 强制垃圾回收
                TrOCRUtils.CleanMemory();
            }
        }

        public static bool IsSupported()
        {
            return RuntimeInformation.ProcessArchitecture == Architecture.X64;
        }
    }
}