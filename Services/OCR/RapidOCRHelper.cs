using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OcrLiteLib;
using System.Threading;
using System.Drawing.Imaging;
using System.Diagnostics;
using Newtonsoft.Json.Linq; 
//支持ppocrv4，不支持ppocrv5，支持32位和64位
namespace TrOCR.Helper
{
    /// <summary>
    /// RapidOCR离线识别帮助类 (基于Lazy<T>的现代线程安全版本)
    /// </summary>
    public sealed class RapidOCRHelper : IDisposable
    {
        public const string DefaultModelDir = @"RapidOCR_data\models";
        public const string DefaultDetModel = @"RapidOCR_data\models\ch_PP-OCRv4_det_infer.onnx";
        public const string DefaultClsModel = @"RapidOCR_data\models\ch_ppocr_mobile_v2.0_cls_infer.onnx";
        public const string DefaultRecModel = @"RapidOCR_data\models\ch_PP-OCRv4_rec_infer.onnx";
        public const string DefaultKeys = @"RapidOCR_data\models\ppocr_keys_v1.txt";

        #region --- 1. 使用 Lazy<T> 实现单例 ---
        private static Lazy<RapidOCRHelper> _lazyInstance =
            new Lazy<RapidOCRHelper>(() => new RapidOCRHelper(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static RapidOCRHelper Instance => _lazyInstance.Value;

        private static readonly object _resetLock = new object();
        #endregion

        #region --- 私有字段 ---
        private OcrLite _ocrEngine;
        private bool _disposed = false;

        // 保留为默认参数
        private int _padding = 50;
        private int _imgResize = 1024;
        private float _boxScoreThresh = 0.5f;
        private float _boxThresh = 0.3f;
        private float _unClipRatio = 1.6f;
        private bool _doAngle = true;
        private bool _mostAngle = true;
        private int numThreads = 4;
        #endregion

        #region --- 2. 构造函数负责所有初始化工作 ---
        private RapidOCRHelper()
        {
            // --- 修复：在每次初始化时都重新声明DLL搜索路径 ---
            var processArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;
            if (processArchitecture == System.Runtime.InteropServices.Architecture.X64)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RapidOCR_data", "win_x64");
                if (Directory.Exists(path))
                {
                    // 假设 TrOCRUtils.AddDllDirectory 是 Program.cs 中 AddDllDirectory 的包装器
                    TrOCRUtils.AddDllDirectory(path); 
                }
            }
            else if (processArchitecture == System.Runtime.InteropServices.Architecture.X86)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RapidOCR_data", "win_x86");
                if (Directory.Exists(path))
                {
                    TrOCRUtils.AddDllDirectory(path);
                }
            }
            // --- 修复结束 ---
            // --- 步骤1：从INI读取所有路径配置 ---
            string detPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_RapidOCR", "Det"));
            string clsPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_RapidOCR", "Cls"));
            string recPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_RapidOCR", "Rec"));
            string keysPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_RapidOCR", "Keys"));
            string advancedConfigPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_RapidOCR", "AdvancedConfig"));

            // 如果模型路径配置为空，使用默认路径 (逻辑保持不变)
            if (string.IsNullOrEmpty(detPath) || string.IsNullOrEmpty(clsPath) || 
                string.IsNullOrEmpty(recPath) || string.IsNullOrEmpty(keysPath))
            {
                string modelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RapidOCR_data", "models");
                var defaultModelFiles = GetModelFilePaths(modelsPath);

                detPath = string.IsNullOrEmpty(detPath) ? defaultModelFiles.DetPath : detPath;
                clsPath = string.IsNullOrEmpty(clsPath) ? defaultModelFiles.ClsPath : clsPath;
                recPath = string.IsNullOrEmpty(recPath) ? defaultModelFiles.RecPath : recPath;
                keysPath = string.IsNullOrEmpty(keysPath) ? defaultModelFiles.KeysPath : keysPath;
            }

            // --- 步骤2：加载并应用高级JSON参数 ---
            if (!string.IsNullOrEmpty(advancedConfigPath) && File.Exists(advancedConfigPath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(advancedConfigPath);
                    var configJson = Newtonsoft.Json.Linq.JObject.Parse(jsonContent);
                    var rapidConfig = configJson["RapidOCR"];

                    if (rapidConfig != null)
                    {
                        // 从JSON读取参数，如果JSON中缺少某个键，则使用字段的当前默认值
                        int padding = rapidConfig["padding"]?.Value<int>() ?? this._padding;
                        int imgResize = rapidConfig["imgResize"]?.Value<int>() ?? this._imgResize;
                        float boxScoreThresh = rapidConfig["boxScoreThresh"]?.Value<float>() ?? this._boxScoreThresh;
                        float boxThresh = rapidConfig["boxThresh"]?.Value<float>() ?? this._boxThresh;
                        float unClipRatio = rapidConfig["unClipRatio"]?.Value<float>() ?? this._unClipRatio;
                        bool doAngle = rapidConfig["doAngle"]?.Value<bool>() ?? this._doAngle;
                        bool mostAngle = rapidConfig["mostAngle"]?.Value<bool>() ?? this._mostAngle;

                        // numThreads 是初始化参数，需要在这里直接更新
                        this.numThreads = rapidConfig["numThreads"]?.Value<int>() ?? this.numThreads;

                        // 使用 SetParameters 方法更新其他参数字段
                        SetParameters(padding, imgResize, boxScoreThresh, boxThresh, unClipRatio, doAngle, mostAngle);

                        Debug.WriteLine($"成功加载并应用RapidOCR高级配置文件: {advancedConfigPath}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"解析或应用RapidOCR高级配置文件 '{advancedConfigPath}' 失败: {ex.Message}");
                    // 即使JSON解析失败，程序也会继续使用硬编码的默认参数运行
                }
            }


            // 验证模型文件
            var modelFiles = new ModelFilePaths
            {
                DetPath = detPath,
                ClsPath = clsPath,
                RecPath = recPath,
                KeysPath = keysPath
            };

            if (!ValidateModelFiles(modelFiles))
            {
                // 如果模型文件缺失，抛出异常，Lazy<T> 会捕获并缓存这个异常
                throw new FileNotFoundException("RapidOCR模型文件缺失，无法初始化引擎。");
            }

            // --- 步骤3：使用最终确定的参数初始化引擎 ---
            try
            {
                _ocrEngine = new OcrLite();
                _ocrEngine.InitModels(
                    modelFiles.DetPath,
                    modelFiles.ClsPath,
                    modelFiles.RecPath,
                    modelFiles.KeysPath,
                    numThreads
                    );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"RapidOCR引擎初始化失败: {ex.Message}", ex);
            }
        }
        #endregion

        #region --- 3. 新增和修改的识别方法 ---
        /// <summary>
        /// 静态方法：使用默认参数识别图像
        /// </summary>
        public static string RecognizeText(Image image)
        {
            try
            {
                // 调用使用默认参数的实例方法
                var result = Instance.Recognize(image);
                Debug.WriteLine($"RapidOCR识别结果: {result?.StrRes}");
                return result?.StrRes?.Trim() ?? "***该区域未发现文本***";
            }
            catch (Exception ex)
            {
                
                // 告知用户初始化失败，并自动重置，以便下次重试
                Reset(); // 关键：重置实例，以便下次有机会重新初始化
                
                return $"***RapidOCR识别失败: {ex.Message}***";
            }
        }

        /// <summary>
        /// /// 【新增重载】静态方法：使用JSON文件配置参数来识别图像
        /// </summary>
         public static string RecognizeText(Image image, string jsonFilePath)
        {
            try
            {
                // 调用使用JSON配置的实例方法
                var result = Instance.Recognize(image, jsonFilePath);
                Debug.WriteLine($"RapidOCR识别结果: {result?.StrRes}");
                return result?.StrRes?.Trim() ?? "***该区域未发现文本***";
            }
            catch (Exception ex)
            {
                
                // 告知用户初始化失败，并自动重置，以便下次重试
                Reset(); // 关键：重置实例，以便下次有机会重新初始化
                
                return $"***RapidOCR识别失败: {ex.Message}***";
            }
        }

        /// <summary>
        /// /// /// 【新增重载】静态方法：传入所有参数来识别图像
        /// </summary>
        public static string RecognizeText(Image image, int padding, int imgResize, float boxScoreThresh, float boxThresh, float unClipRatio, bool doAngle, bool mostAngle)
        {
            try
            {
                // 调用传入所有参数的实例方法
                var result = Instance.Recognize(image, padding, imgResize, boxScoreThresh, boxThresh, unClipRatio, doAngle, mostAngle);
                Debug.WriteLine($"RapidOCR识别结果: {result?.StrRes}");
                return result?.StrRes?.Trim() ?? "***该区域未发现文本***";
            }
            catch (Exception ex)
            {
                
                // 可告知用户初始化失败，并自动重置，以便下次重试
                Reset(); // 关键：重置实例，以便下次有机会重新初始化
                
                return $"***RapidOCR识别失败: {ex.Message}***";
            }
        }

        // -------- 实例方法 (被静态方法调用，执行具体操作) --------

        /// <summary>
        /// 【补全】实例方法：使用类中存储的默认参数进行识别
        /// </summary>
        private OcrResult Recognize(Image image)
        {
            if (_ocrEngine == null) throw new InvalidOperationException("OCR引擎未初始化。");
            if (image == null) throw new ArgumentNullException(nameof(image));

            // 将Image转换为Bitmap并调用引擎
            using (var bitmap = new Bitmap(image))
            {
                return _ocrEngine.Detect(bitmap, _padding, _imgResize, _boxScoreThresh, _boxThresh, _unClipRatio, _doAngle, _mostAngle);
            }
        }

        /// <summary>
        /// 【新增】实例方法：使用JSON文件配置进行识别
        /// </summary>
        private OcrResult Recognize(Image image, string jsonFilePath)
        {
            if (_ocrEngine == null) throw new InvalidOperationException("OCR引擎未初始化。");
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (!File.Exists(jsonFilePath)) throw new FileNotFoundException("JSON配置文件不存在。", jsonFilePath);

            using (var bitmap = new Bitmap(image))
            {
                // 调用OcrLite中新增的json重载方法
                return _ocrEngine.Detect(bitmap, jsonFilePath);
            }
        }

        /// <summary>
        /// 【新增】实例方法：使用传入的完整参数进行识别
        /// </summary>
        private OcrResult Recognize(Image image, int padding, int imgResize, float boxScoreThresh, float boxThresh, float unClipRatio, bool doAngle, bool mostAngle)
        {
            if (_ocrEngine == null) throw new InvalidOperationException("OCR引擎未初始化。");
            if (image == null) throw new ArgumentNullException(nameof(image));
            
            using (var bitmap = new Bitmap(image))
            {
                return _ocrEngine.Detect(bitmap, padding, imgResize, boxScoreThresh, boxThresh, unClipRatio, doAngle, mostAngle);
            }
        }

        #endregion

        /// <summary>
        /// 设置全局默认参数 (此方法仍然有效)
        /// </summary>
        public void SetParameters(
            int padding = 50,
            int imgResize = 1024,
            float boxScoreThresh = 0.5f,
            float boxThresh = 0.3f,
            float unClipRatio = 1.6f,
            bool doAngle = true,
            bool mostAngle = true)
        {
            _padding = padding;
            _imgResize = imgResize;
            _boxScoreThresh = boxScoreThresh;
            _boxThresh = boxThresh;
            _unClipRatio = unClipRatio;
            _doAngle = doAngle;
            _mostAngle = mostAngle;
        }

        #region --- 4. 资源释放与重置 ---
        public static void Reset()
        {
            lock (_resetLock)
            {
                if (_lazyInstance.IsValueCreated)
                {
                    try
                    {
                        // 尝试获取并释放旧实例。
                        // 如果初始化成功，这里会正常执行 Dispose。
                        // 如果初始化失败，访问 .Value 会抛出异常。
                        _lazyInstance.Value.Dispose();
                    }
                    catch
                    {
                        // 捕获访问 .Value 时的异常。
                        // 这说明实例本身就是坏的，没有资源需要释放，
                        // 所以我们可以安全地忽略这个异常，继续执行重置的核心逻辑。
                    }                   
                }
                _lazyInstance = new Lazy<RapidOCRHelper>(() => new RapidOCRHelper(), LazyThreadSafetyMode.ExecutionAndPublication);
                TrOCRUtils.CleanMemory();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 在此清理托管资源 (如果未来有的话)
                }

                
                _ocrEngine = null;
                _disposed = true;
            }
        }

        ~RapidOCRHelper()
        {
            try
            {
                Dispose(false);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"RapidOCR Error in finalizer: {ex.Message}");               
                return;
            }
            
        }
        #endregion

        #region --- 私有辅助方法 ---
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

        /// <summary>
        /// 模型文件路径结构
        /// </summary>
        private struct ModelFilePaths
        {
            public string DetPath { get; set; }
            public string ClsPath { get; set; }
            public string RecPath { get; set; }
            public string KeysPath { get; set; }
        }

        private (string DetPath, string ClsPath, string RecPath, string KeysPath) GetModelFilePaths(string modelsPath)
        {
            return (
                DetPath: Path.Combine(modelsPath, "ch_PP-OCRv4_det_infer.onnx"),
                ClsPath: Path.Combine(modelsPath, "ch_ppocr_mobile_v2.0_cls_infer.onnx"),
                RecPath: Path.Combine(modelsPath, "ch_PP-OCRv4_rec_infer.onnx"),
                KeysPath: Path.Combine(modelsPath, "ppocr_keys_v1.txt")
            );
        }

        private bool ValidateModelFiles(ModelFilePaths modelFiles)
        {
            var missingFiles = new System.Collections.Generic.List<string>();
            if (!File.Exists(modelFiles.DetPath)) missingFiles.Add($"检测模型: {modelFiles.DetPath}");
            if (!File.Exists(modelFiles.ClsPath)) missingFiles.Add($"分类模型: {modelFiles.ClsPath}");
            if (!File.Exists(modelFiles.RecPath)) missingFiles.Add($"识别模型: {modelFiles.RecPath}");
            if (!File.Exists(modelFiles.KeysPath)) missingFiles.Add($"字典文件: {modelFiles.KeysPath}");
            if (missingFiles.Count > 0)
            {
                string message = "以下模型文件不存在:\n" + string.Join("\n", missingFiles);
                MessageBox.Show(message, "模型文件缺失", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        #endregion
    }
}