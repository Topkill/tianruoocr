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
    /// 火山翻译接口（无需密钥）
    /// </summary>
    public static class VolcanoTranslator
    {
        private static readonly HttpClient HttpClient;
        private static readonly string TranslateUrl = "https://translate.volcengine.com/crx/translate/v1";
        
        // 支持的源语言（基于官方文档）
        private static readonly HashSet<string> SupportedSourceLanguages = new HashSet<string>
        {
            "zh", "zh-Hant", "zh-Hans", "en", "ja", "ko", "es", "fr", "de", "tr",
            "ru", "pt", "vi", "id", "th", "ms", "ar", "hi", "it", "he", "pl",
            "nl", "cs", "sv", "no", "da", "fi", "hu", "el", "bg", "sk", "lt",
            "lv", "et", "sl", "hr", "ro", "uk", "sr", "ta", "te", "bn", "my",
            "km", "mn", "ur", "fa", "sw", "tl", "bo"
        };
        
        // 支持的翻译方向（基于官方文档，部分语言单向）
        private static readonly Dictionary<string, HashSet<string>> SupportedTranslations = new Dictionary<string, HashSet<string>>
        {
            // 中文互译语言
            ["zh"] = new HashSet<string> { "zh-Hant", "en", "ja", "ko", "es", "fr", "de", "tr", "ru", "pt", "vi", "id", "th", "ms", "ar", "hi", "it", "he", "pl", "nl", "cs", "sv", "no", "da", "fi", "hu", "el", "bg", "sk", "lt", "lv", "et", "sl", "hr", "ro", "uk", "sr", "ta", "te", "bn", "my", "km", "mn", "ur", "fa", "sw", "tl" },
            ["zh-Hant"] = new HashSet<string> { "zh", "zh-Hans", "en", "ja", "ko", "es", "fr", "de", "tr", "ru", "pt", "vi", "id", "th", "ms", "ar", "hi", "it", "he", "pl", "nl", "cs", "sv", "no", "da", "fi", "hu", "el", "bg", "sk", "lt", "lv", "et", "sl", "hr", "ro", "uk", "sr", "ta", "te", "bn", "my", "km", "mn", "ur", "fa", "sw", "tl" },
            ["zh-Hans"] = new HashSet<string> { "zh-Hant", "en", "ja", "ko", "es", "fr", "de", "tr", "ru", "pt", "vi", "id", "th", "ms", "ar", "hi", "it", "he", "pl", "nl", "cs", "sv", "no", "da", "fi", "hu", "el", "bg", "sk", "lt", "lv", "et", "sl", "hr", "ro", "uk", "sr", "ta", "te", "bn", "my", "km", "mn", "ur", "fa", "sw", "tl" },
            
            // 英文互译语言
            ["en"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "ja", "ko", "es", "fr", "de", "tr", "ru", "pt", "vi", "id", "th", "ms", "ar", "hi", "it", "he", "pl", "nl", "cs", "sv", "no", "da", "fi", "hu", "el", "bg", "sk", "lt", "lv", "et", "sl", "hr", "ro", "uk", "sr", "ta", "te", "bn", "my", "km", "mn", "ur", "fa", "sw", "tl" },
            
            // 其他主要语言互译
            ["ja"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "ko" },
            ["ko"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "ja" },
            ["es"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "fr", "de", "it", "pt", "ru", "ar" },
            ["fr"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "es", "de", "it", "pt", "ru", "ar" },
            ["de"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "es", "fr", "it", "pt", "ru", "ar" },
            ["tr"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "ar" },
            ["ru"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "es", "fr", "de", "it", "pt", "ar" },
            ["pt"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "es", "fr", "de", "it", "ru", "ar" },
            ["vi"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["id"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["th"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["ms"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["ar"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "es", "fr", "de", "tr", "ru", "pt", "it" },
            ["hi"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["it"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en", "es", "fr", "de", "pt", "ru", "ar" },
            
            // 以下语言主要与中文和英文互译
            ["he"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["pl"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["nl"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["cs"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["sv"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["no"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["da"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["fi"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["hu"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["el"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["bg"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["sk"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["lt"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["lv"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["et"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["sl"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["hr"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["ro"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["uk"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["sr"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["ta"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["te"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["bn"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["my"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["km"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["mn"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["ur"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["fa"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["sw"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            ["tl"] = new HashSet<string> { "zh", "zh-Hant", "zh-Hans", "en" },
            
            // 藏语只能翻译到中文（单向）
            ["bo"] = new HashSet<string> { "zh", "zh-Hans" }
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
            { "he", "he" },
            { "pl", "pl" },
            { "nl", "nl" },
            { "cs", "cs" },
            { "sv", "sv" },
            { "no", "no" },
            { "da", "da" },
            { "fi", "fi" },
            { "hu", "hu" },
            { "el", "el" },
            { "bg", "bg" },
            { "sk", "sk" },
            { "lt", "lt" },
            { "lv", "lv" },
            { "et", "et" },
            { "sl", "sl" },
            { "hr", "hr" },
            { "ro", "ro" },
            { "uk", "uk" },
            { "sr", "sr" },
            { "ta", "ta" },
            { "te", "te" },
            { "bn", "bn" },
            { "my", "my" },
            { "km", "km" },
            { "mn", "mn" },
            { "ur", "ur" },
            { "fa", "fa" },
            { "sw", "sw" },
            { "tl", "tl" },
            { "bo", "bo" },
            { "auto", "" }  // 火山翻译通过不发送source_language实现自动检测
        };

        static VolcanoTranslator()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
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
                
                // 检查源语言是否支持（如果不是自动检测）
                bool isAutoDetect = string.IsNullOrEmpty(from) || from == "auto" || fromLanguage == "auto";
                
                if (!isAutoDetect && !SupportedSourceLanguages.Contains(from))
                {
                    return $"翻译失败：不支持的源语言 ({fromLanguage})";
                }
                
                // 如果不是自动检测，检查翻译方向是否支持
                if (!isAutoDetect && SupportedTranslations.ContainsKey(from))
                {
                    if (!SupportedTranslations[from].Contains(to))
                    {
                        return $"翻译失败：不支持的翻译方向 ({fromLanguage} → {toLanguage})";
                    }
                }

                // 构建请求体
                // 如果是自动检测，不发送source_language字段（火山API会自动检测）
                dynamic requestBody;
                if (isAutoDetect)
                {
                    requestBody = new
                    {
                        target_language = to,
                        text = text
                    };
                }
                else
                {
                    requestBody = new
                    {
                        source_language = from,
                        target_language = to,
                        text = text
                    };
                }

                using (var request = new HttpRequestMessage(HttpMethod.Post, TranslateUrl))
                {
                    // 设置请求头
                    request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
                    request.Headers.TryAddWithoutValidation("Origin", "chrome-extension://klgfhbiooeogfpknjdcbablpceialkdj");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "none");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                    
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
                            var translation = result["translation"];
                            if (translation != null)
                            {
                                return translation.ToString().Trim();
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
                            var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            
                            // 尝试解析错误信息
                            try
                            {
                                var errorResult = JObject.Parse(errorContent);
                                var errorMsg = errorResult["message"] ?? errorResult["error"];
                                if (errorMsg != null)
                                {
                                    return $"翻译失败: {errorMsg}";
                                }
                            }
                            catch
                            {
                                // 忽略解析错误
                            }
                            
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
                return "";  // 火山翻译通过不发送source_language实现自动检测
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
    }
}