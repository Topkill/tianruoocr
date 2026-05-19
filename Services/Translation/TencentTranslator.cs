using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrOCR.Helper
{
    /// <summary>
    /// 腾讯交互翻译接口（无需密钥）
    /// </summary>
    public static class TencentTranslator
    {
        private static readonly HttpClient HttpClient;
        private static readonly string TranslateUrl = "https://transmart.qq.com/api/imt";
        
        // 支持的源语言
        private static readonly HashSet<string> SupportedSourceLanguages = new HashSet<string>
        {
            "auto", "zh", "zh-TW", "en", "ja", "ko", "fr", "es", "it", "de",
            "tr", "ru", "pt", "vi", "id", "th", "ms", "ar", "hi"
        };
        
        // 支持的翻译方向（基于官方文档）
        private static readonly Dictionary<string, HashSet<string>> SupportedTranslations = new Dictionary<string, HashSet<string>>
        {
            ["zh"] = new HashSet<string> { "zh-TW", "en", "ja", "ko", "fr", "es", "it", "de", "tr", "ru", "pt", "vi", "id", "th", "ms", "ar" },
            ["zh-TW"] = new HashSet<string> { "zh", "en", "ja", "ko", "fr", "es", "it", "de", "tr", "ru", "pt", "vi", "id", "th", "ms", "ar" },
            ["en"] = new HashSet<string> { "zh", "zh-TW", "ja", "ko", "fr", "es", "it", "de", "tr", "ru", "pt", "vi", "id", "th", "ms", "ar", "hi" },
            ["ja"] = new HashSet<string> { "zh", "zh-TW", "en", "ko" },
            ["ko"] = new HashSet<string> { "zh", "zh-TW", "en", "ja" },
            ["fr"] = new HashSet<string> { "zh", "zh-TW", "en", "es", "it", "de", "tr", "ru", "pt" },
            ["es"] = new HashSet<string> { "zh", "zh-TW", "en", "fr", "it", "de", "tr", "ru", "pt" },
            ["it"] = new HashSet<string> { "zh", "zh-TW", "en", "fr", "es", "de", "tr", "ru", "pt" },
            ["de"] = new HashSet<string> { "zh", "zh-TW", "en", "fr", "es", "it", "tr", "ru", "pt" },
            ["tr"] = new HashSet<string> { "zh", "zh-TW", "en", "fr", "es", "it", "de", "ru", "pt" },
            ["ru"] = new HashSet<string> { "zh", "zh-TW", "en", "fr", "es", "it", "de", "tr", "pt" },
            ["pt"] = new HashSet<string> { "zh", "zh-TW", "en", "fr", "es", "it", "de", "tr", "ru" },
            ["vi"] = new HashSet<string> { "zh", "zh-TW", "en" },
            ["id"] = new HashSet<string> { "zh", "zh-TW", "en" },
            ["th"] = new HashSet<string> { "zh", "zh-TW", "en" },
            ["ms"] = new HashSet<string> { "zh", "zh-TW", "en" },
            ["ar"] = new HashSet<string> { "zh", "zh-TW", "en" },
            ["hi"] = new HashSet<string> { "en" }
        };
        
        // 语言代码映射
        private static readonly Dictionary<string, string> LanguageMap = new Dictionary<string, string>
        {
            { "zh-CN", "zh" },
            { "zh-TW", "zh-TW" },
            { "en", "en" },
            { "ja", "ja" },
            { "ko", "ko" },
            { "es", "es" },
            { "fr", "fr" },
            { "de", "de" },
            { "tr", "tr" },
            { "ru", "ru" },
            { "pt", "pt" },
            { "vi", "vi" },
            { "id", "id" },
            { "th", "th" },
            { "ms", "ms" },
            { "ar", "ar" },
            { "hi", "hi" },
            { "it", "it" },
            { "auto", "auto" }
        };

        static TencentTranslator()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
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
                
                // 检查源语言是否支持
                if (from != "auto" && !SupportedSourceLanguages.Contains(from))
                {
                    return $"翻译失败：不支持的源语言 ({fromLanguage})";
                }
                
                // 如果不是auto，检查翻译方向是否支持
                if (from != "auto" && SupportedTranslations.ContainsKey(from))
                {
                    if (!SupportedTranslations[from].Contains(to))
                    {
                        return $"翻译失败：不支持的翻译方向 ({fromLanguage} → {toLanguage})";
                    }
                }

                // 构建请求体
                var requestBody = new
                {
                    header = new
                    {
                        fn = "auto_translation",
                        client_key = $"browser-chrome-110.0.0-Windows-{Guid.NewGuid()}-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}"
                    },
                    type = "plain",
                    model_category = "normal",
                    source = new
                    {
                        lang = from,
                        text_list = new[] { text }
                    },
                    target = new
                    {
                        lang = to
                    }
                };

                using (var request = new HttpRequestMessage(HttpMethod.Post, TranslateUrl))
                {
                    // 设置请求头
                    request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    request.Headers.TryAddWithoutValidation("Referer", "https://transmart.qq.com/zh-CN/index");
                    request.Headers.TryAddWithoutValidation("Origin", "https://transmart.qq.com");
                    
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
                            var autoTranslation = result["auto_translation"];
                            if (autoTranslation != null && autoTranslation.Type == JTokenType.Array)
                            {
                                var translations = autoTranslation as JArray;
                                var translatedTexts = new List<string>();
                                
                                foreach (var item in translations)
                                {
                                    translatedTexts.Add(item.ToString());
                                }
                                
                                return string.Join("\n", translatedTexts).Trim();
                            }
                            else
                            {
                                return $"翻译失败：未找到翻译结果";
                            }
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
                    return "zh-TW";
                default:
                    // 直接返回原始代码，让API自己处理
                    return langCode;
            }
        }
    }
}