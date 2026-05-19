using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TrOCR.Helper; 
using OpenCvSharp;
using Sdcb.PaddleOCR;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//支持ppocrv5，支持64位，不支持32位，不需要 CPU 支持 AVX指令集，cpu不支持avx的使用此接口
namespace TrOCR.Helper
{
    /// <summary>
    /// PaddleOCR2离线识别帮助类 (基于Sdcb.PaddleOCR)
    /// 采用单例模式和懒加载，支持从自定义本地路径加载模型。
    /// </summary>
    public sealed class PaddleOCR2Helper : IDisposable
    {
        public const string DefaultModelDir = @"PaddleOCR2_data\models";
        public const string DefaultDetModel = @"PaddleOCR2_data\models\PP-OCRv5_mobile_det_infer";
        public const string DefaultClsModel = @"PaddleOCR2_data\models\ch_ppocr_mobile_v5.0_cls_infer";
        public const string DefaultRecModel = @"PaddleOCR2_data\models\PP-OCRv5_mobile_rec_infer";
        public const string DefaultKeys = @"PaddleOCR2_data\models\ppocr_keys.txt";

        // --- 1. Lazy<T> ，实现简洁的线程安全单例 ---
        private static Lazy<PaddleOCR2Helper> _lazyInstance =
            new Lazy<PaddleOCR2Helper>(() => new PaddleOCR2Helper(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static PaddleOCR2Helper Instance => _lazyInstance.Value;
        
        private static readonly object _resetLock = new object();
        private readonly PaddleOcrAll _ocrEngine;
        private bool _disposed = false;

        // --- 2. 构造函数现在负责所有初始化工作 ---
        private PaddleOCR2Helper()
        {
            // 检查系统架构
            if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
                throw new NotSupportedException("***PaddleOCR2仅支持64位系统, 不支持32位系统***");

            try
            {
                // --- 步骤1：从INI读取所有相关配置 ---
                string detModelPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR2", "Det"));
                string clsModelPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR2", "Cls"));
                string recModelPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR2", "Rec"));
                string keysPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR2", "Keys"));
                string advancedConfigPath = TrOCRUtils.ConvertToAbsolutePath(GetConfigValue("模型配置_PaddleOCR2", "AdvancedConfig"));

                // 读取独立的版本号
                string detVersionStr = GetConfigValue("模型配置_PaddleOCR2", "Det_Version");
                string clsVersionStr = GetConfigValue("模型配置_PaddleOCR2", "Cls_Version");
                string recVersionStr = GetConfigValue("模型配置_PaddleOCR2", "Rec_Version");

                // 如果模型路径配置为空，使用默认路径 (保持不变)
                if (string.IsNullOrEmpty(detModelPath) || string.IsNullOrEmpty(clsModelPath) ||
                    string.IsNullOrEmpty(recModelPath) || string.IsNullOrEmpty(keysPath))
                {
                    string modelBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PaddleOCR2_data", "models");
                    detModelPath = string.IsNullOrEmpty(detModelPath) ? Path.Combine(modelBasePath, "PP-OCRv5_mobile_det_infer") : detModelPath;
                    clsModelPath = string.IsNullOrEmpty(clsModelPath) ? Path.Combine(modelBasePath, "ch_ppocr_mobile_v5.0_cls_infer") : clsModelPath;
                    recModelPath = string.IsNullOrEmpty(recModelPath) ? Path.Combine(modelBasePath, "PP-OCRv5_mobile_rec_infer") : recModelPath;
                    keysPath = string.IsNullOrEmpty(keysPath) ? Path.Combine(modelBasePath, "ppocr_keys.txt") : keysPath;
                }

                // --- 步骤2：解析独立的版本配置 ---
                ModelVersion detVersion = ParseModelVersion(detVersionStr);
                ModelVersion clsVersion = ParseModelVersion(clsVersionStr);
                ModelVersion recVersion = ParseModelVersion(recVersionStr);

                // --- 步骤3：读取并解析JSON文件 (只执行一次) ---
                JToken paddleConfig = null;
                if (!string.IsNullOrEmpty(advancedConfigPath) && File.Exists(advancedConfigPath))
                {
                    try
                    {
                        string jsonContent = File.ReadAllText(advancedConfigPath);
                        paddleConfig = Newtonsoft.Json.Linq.JObject.Parse(jsonContent)?["PaddleOCR2"];
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"解析高级配置文件 '{advancedConfigPath}' 失败: {ex.Message}");
                    }
                }
                // --- 步骤4：根据解析结果创建设备动作 ---
                Action<PaddleConfig> device = CreateDeviceAction(paddleConfig);

                 // --- 步骤5：手动创建三个核心组件，并在创建时应用高级参数 ---

                // 5.1 创建检测器 (Detector)
                DetectionModel detModel = DetectionModel.FromDirectory(detModelPath, detVersion);
                PaddleOcrDetector detector = new PaddleOcrDetector(detModel, device);
                // 应用检测器参数 (这些是set属性, 可以在创建后修改)
                if (paddleConfig?["Detector"] != null)
                {
                    var detConfig = paddleConfig["Detector"];
                    detector.MaxSize = (int?)detConfig["MaxSize"] ?? detector.MaxSize;
                    detector.BoxScoreThreahold = (float?)detConfig["BoxScoreThreahold"] ?? detector.BoxScoreThreahold;
                    detector.BoxThreshold = (float?)detConfig["BoxThreshold"] ?? detector.BoxThreshold;
                    detector.MinSize = (int?)detConfig["MinSize"] ?? detector.MinSize;
                    detector.UnclipRatio = (float?)detConfig["UnclipRatio"] ?? detector.UnclipRatio;
                }

                // 5.2 创建分类器 (Classifier)，并在初始化时设置init-only属性
                ClassificationModel clsModel = ClassificationModel.FromDirectory(clsModelPath, clsVersion);
                // 读取JSON中的阈值，如果不存在则使用库的默认值0.75
                double rotateThreshold = (double?)paddleConfig?["Classifier"]?["RotateThreshold"] ?? 0.75;
                PaddleOcrClassifier classifier = new PaddleOcrClassifier(clsModel, device)
                {
                    // 在对象初始化器中为 init-only 属性赋值
                    RotateThreshold = rotateThreshold 
                };

                // 5.3 创建识别器 (Recognizer)
                RecognizationModel recModel = RecognizationModel.FromDirectory(recModelPath, keysPath, recVersion);
                PaddleOcrRecognizer recognizer = new PaddleOcrRecognizer(recModel, device);

                // --- 步骤6：使用创建好的三个组件组装最终的引擎 ---
                _ocrEngine = new PaddleOcrAll(detector, classifier, recognizer);

             

                // --- 步骤7：应用引擎顶层的参数 ---
                if (paddleConfig?["TopLevel"] != null)
                {
                    var topLevelConfig = paddleConfig["TopLevel"];
                    _ocrEngine.Enable180Classification = (bool?)topLevelConfig["Enable180Classification"] ?? _ocrEngine.Enable180Classification;
                    _ocrEngine.AllowRotateDetection = (bool?)topLevelConfig["AllowRotateDetection"] ?? _ocrEngine.AllowRotateDetection;
                }

                Debug.WriteLine("PaddleOCR2引擎及所有参数成功初始化。");
            }
            catch (Exception ex)
            {
                string detailedMessage = $"从自定义路径加载模型失败，请检查路径和模型文件是否完整且版本匹配。\n根本原因: {ex.Message}";
                throw new Exception(detailedMessage, ex);
            }
        }
            
        // --- 新增辅助方法1：创建设备动作 ---
        private Action<PaddleConfig> CreateDeviceAction(JToken paddleConfig)
        {
            try
            {
                if (paddleConfig?["Device"] != null)
                {
                    var deviceConfig = paddleConfig["Device"];
                    string deviceType = deviceConfig["Type"]?.Value<string>() ?? "CpuBlas";

                    switch (deviceType)
                    {   //其实不起作用，因为使用的运行时依赖不是Mkldnn，这里保留为了后续功能开发
                        case "CpuMkldnn":
                            var mkldnnParams = deviceConfig["CpuMkldnn"];
                            int cacheCapacity = mkldnnParams?["cacheCapacity"]?.Value<int>() ?? 10;
                            int cpuThreads = mkldnnParams?["cpuMathThreadCount"]?.Value<int>() ?? 0;
                            bool memOptimizedMkl = mkldnnParams?["memoryOptimized"]?.Value<bool>() ?? true; // 默认true
                            bool glogMkl = mkldnnParams?["glogEnabled"]?.Value<bool>() ?? false;// 默认false
                            Debug.WriteLine("PaddleOCR2: Using MKLDNN device.");
                            return PaddleDevice.Mkldnn(cacheCapacity, cpuThreads, memOptimizedMkl, glogMkl);

                        case "CpuBlas":
                            var blasParams = deviceConfig["CpuBlas"];
                            int blasThreads = blasParams?["cpuMathThreadCount"]?.Value<int>() ?? 0;
                            bool memOptimizedBlas = blasParams?["memoryOptimized"]?.Value<bool>() ?? true; // 从 blasParams 读取
                            bool glogBlas = blasParams?["glogEnabled"]?.Value<bool>() ?? false;          // 从 blasParams 读取
                            Debug.WriteLine("PaddleOCR2: Using BLAS device.");
                            return PaddleDevice.Blas(blasThreads, memOptimizedBlas, glogBlas);
                        //case "Gpu":
                        //运行时不支持cpu
                            
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"创建设备动作失败: {ex.Message}");
            }

            // 如果没有配置文件或解析失败，使用最安全的默认值
            Debug.WriteLine("PaddleOCR2: Using default BLAS device.");
            return PaddleDevice.Blas();
        }


        // --- 3. 公共方法恢复同步，调用更简单 ---
        /// <summary>
        /// 识别图像中的文字
        /// </summary>
        public static string RecognizeText(Image image)
        {
            try
            {
                return Instance.Execute(image);
            }
            catch (Exception ex)
            {
                Reset(); // 失败后重置，以便下次重试
                return $"***PaddleOCR2识别失败: {ex.Message}***";
            }
        }

        private string Execute(Image image)
        {
            if (_ocrEngine == null) return "***PaddleOCR2引擎未初始化***";
            if (image == null) return "***图像为空***";

            using (Mat src = ImageToMat(image))
            {
                PaddleOcrResult result = _ocrEngine.Run(src);
                Debug.WriteLine($"paddleOCR2识别结果: {result}");
                if (result?.Regions == null || result.Regions.Length == 0)
                    return "***该区域未发现文本***";

                var sb = new StringBuilder();
                foreach (var region in result.Regions)
                {
                    if (!string.IsNullOrWhiteSpace(region.Text))
                    {
                        sb.AppendLine(region.Text);
                    }
                }
                string finalResult = sb.ToString().Trim();
                return string.IsNullOrEmpty(finalResult) ? "***该区域未发现文本***" : finalResult;
            }
        }
        
        // --- 4. Reset 方法适配 Lazy<T> ---
        public static void Reset()
        {
            lock (_resetLock)
            {
                if (_lazyInstance.IsValueCreated)
                {
                    try { _lazyInstance.Value.Dispose(); }
                    catch { /* 忽略在失败实例上调用Dispose时可能发生的异常 */ }
                }
                // 关键：创建全新的Lazy实例以备下次使用
                _lazyInstance = new Lazy<PaddleOCR2Helper>(() => new PaddleOCR2Helper(), LazyThreadSafetyMode.ExecutionAndPublication);
                TrOCRUtils.CleanMemory();
            }
        }

        // --- 其他辅助方法和Dispose模式 ---
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
        /// 解析模型版本配置
        /// </summary>
        private ModelVersion ParseModelVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return ModelVersion.V5; // 默认v5

            switch (version.ToLower())
            {
                case "v2": return ModelVersion.V2;
                case "v3": return ModelVersion.V3;
                case "v4": return ModelVersion.V4;
                case "v5": return ModelVersion.V5;
                default: return ModelVersion.V5;
            }
        }

        private Mat ImageToMat(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return Cv2.ImDecode(ms.ToArray(), ImreadModes.Color);
            }
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _ocrEngine?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        ~PaddleOCR2Helper() => Dispose();
    }
}