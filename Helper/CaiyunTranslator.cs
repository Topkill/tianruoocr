using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrOCR.Helper
{
    /// <summary>
    /// 彩云小译翻译接口（无需密钥）
    /// </summary>
    public static class CaiyunTranslator
    {
        private static readonly HttpClient HttpClient;
        private static readonly string TranslateUrl = "https://interpreter.cyapi.cn/v1/translator";
        
        // 支持的翻译方向（基于官方文档）
        private static readonly HashSet<string> SupportedTranslations = new HashSet<string>
        {
            // 中文翻译到其他语言
            "zh2zh-Hant", "zh2en", "zh2ja", "zh2ko",
            
            // 繁体中文翻译到其他语言
            "zh-Hant2zh", "zh-Hant2en", "zh-Hant2ja", "zh-Hant2ko",
            
            // 其他语言翻译到中文
            "en2zh", "en2zh-Hant",
            "ja2zh", "ja2zh-Hant",
            "ko2zh", "ko2zh-Hant",
            "de2zh", "de2zh-Hant",
            "es2zh", "es2zh-Hant",
            "fr2zh", "fr2zh-Hant",
            "it2zh", "it2zh-Hant",
            "pt2zh", "pt2zh-Hant",
            "ru2zh", "ru2zh-Hant",
            "tr2zh", "tr2zh-Hant",
            "vi2zh", "vi2zh-Hant",
            
            // auto2任何支持的目标语言
            "auto2zh", "auto2zh-Hant", "auto2en", "auto2ja", "auto2ko"
        };
        
        // 语言代码映射
        private static readonly Dictionary<string, string> LanguageMap = new Dictionary<string, string>
        {
            { "zh-CN", "zh" },
            { "zh-TW", "zh-Hant" },
            { "en", "en" },
            { "ja", "ja" },
            { "ko", "ko" },
            { "es", "es" },
            { "fr", "fr" },
            { "de", "de" },
            { "ru", "ru" },
            { "pt", "pt" },
            { "it", "it" },
            { "vi", "vi" },
            { "tr", "tr" },
            { "auto", "auto" }
        };

        static CaiyunTranslator()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                "caiyunInterpreter/5 CFNetwork/1404.0.5 Darwin/22.3.0");
        }

        /// <summary>
        /// 翻译文本
        /// </summary>
        public static async Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            try
            {
                // 转换语言代码
                var from = ConvertLanguageCode(fromLanguage);
                var to = ConvertLanguageCode(toLanguage);
                
                // 彩云使用特殊的trans_type格式
                var transType = $"{from}2{to}";
                
                // 检查是否支持该翻译方向
                if (!IsTranslationSupported(transType))
                {
                    // 如果是auto，尝试构建auto2目标语言
                    if (from == "auto" || string.IsNullOrEmpty(from))
                    {
                        transType = $"auto2{to}";
                        if (!IsTranslationSupported(transType))
                        {
                            return $"翻译失败：不支持的翻译方向 ({fromLanguage} → {toLanguage})";
                        }
                    }
                    else
                    {
                        return $"翻译失败：不支持的翻译方向 ({fromLanguage} → {toLanguage})";
                    }
                }

                // 构建请求体
                var requestBody = new
                {
                    source = text,
                    detect = from == "auto" || string.IsNullOrEmpty(from),
                    os_type = "windows",
                    device_id = GenerateDeviceId(),
                    trans_type = transType,
                    media = "text",
                    request_id = DateTimeOffset.Now.ToUnixTimeMilliseconds() % 1000000000,
                    user_id = "",
                    dict = true
                };

                using (var request = new HttpRequestMessage(HttpMethod.Post, TranslateUrl))
                {
                    // 设置请求头
                    request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    request.Headers.TryAddWithoutValidation("x-authorization", "token ssdj273ksdiwi923bsd9");
                    request.Headers.TryAddWithoutValidation("User-Agent", "caiyunInterpreter/5 CFNetwork/1404.0.5 Darwin/22.3.0");
                    
                    // 设置请求体
                    var json = JsonConvert.SerializeObject(requestBody);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result = JObject.Parse(responseString);
                            
                            // 提取翻译结果
                            var target = result["target"];
                            if (target != null)
                            {
                                return target.ToString().Trim();
                            }
                            
                            // 检查是否有错误信息
                            var errorMsg = result["message"] ?? result["error"];
                            if (errorMsg != null)
                            {
                                return $"翻译失败: {errorMsg}";
                            }
                            
                            return "翻译失败：未找到翻译结果";
                        }
                        else
                        {
                            return $"翻译请求失败: HTTP {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"翻译失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 检查是否支持该翻译方向
        /// </summary>
        private static bool IsTranslationSupported(string transType)
        {
            return SupportedTranslations.Contains(transType);
        }

        /// <summary>
        /// 转换语言代码
        /// </summary>
        private static string ConvertLanguageCode(string langCode)
        {
            if (string.IsNullOrEmpty(langCode) || langCode == "auto")
            {
                return "auto";
            }

            // 简单的语言代码映射处理
            switch (langCode)
            {
                case "zh-CN":
                    return "zh";
                case "zh-TW":
                    return "zh-Hant";
                default:
                    // 直接返回原始代码，让API自己处理
                    return langCode;
            }
        }

        /// <summary>
        /// 生成设备ID
        /// </summary>
        private static string GenerateDeviceId()
        {
            // 生成一个类似UUID的设备ID
            return Guid.NewGuid().ToString().ToUpper();
        }
    }
}