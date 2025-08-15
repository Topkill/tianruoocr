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
    /// 彩云小译翻译接口（密钥版）
    /// </summary>
    public static class CaiyunTranslator2
    {
        private static readonly HttpClient HttpClient;
        private static readonly string TranslateUrl = "https://api.interpreter.caiyunai.com/v1/translator";

        // 支持的翻译方向（基于官方文档）
        private static readonly HashSet<string> SupportedTranslations = new HashSet<string>
        {
            // 中文翻译到其他语言
            "zh2zh-Hant", "zh2en", "zh2ja", "zh2ko",
            
            // 繁体中文翻译到其他语言
            "zh-Hant2zh", "zh-Hant2en", "zh-Hant2ja", "zh-Hant2ko",
            
            // 其他语言翻译到中文或繁体
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
        
        static CaiyunTranslator2()
        {
            HttpClient = new HttpClient();
        }

        /// <summary>
        /// 翻译文本
        /// </summary>
        public static async Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage, string token)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            if (string.IsNullOrEmpty(token))
                return "翻译失败：彩云小译Token不能为空。";

            try
            {
                // 转换语言代码
                var from = ConvertLanguageCode(fromLanguage);
                var to = ConvertLanguageCode(toLanguage);
                
                // 彩云使用特殊的trans_type格式
                var transType = $"{from}2{to}";
                
                bool detect = from == "auto" || string.IsNullOrEmpty(from);
                
                if (detect)
                {
                    transType = $"auto2{to}";
                }

                // 检查是否支持该翻译方向
                if (!IsTranslationSupported(transType))
                {
                    return $"翻译失败：不支持的翻译方向 ({fromLanguage} → {toLanguage})";
                }

                // 构建请求体
                var requestBody = new
                {
                    source = new[] { text },
                    trans_type = transType,
                    detect,
                    media = "text",
                    request_id = "demo"
                };

                using (var request = new HttpRequestMessage(HttpMethod.Post, TranslateUrl))
                {
                    // 设置请求头
                    request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    request.Headers.TryAddWithoutValidation("x-authorization", "token " + token);
                    
                    // 设置请求体
                    var json = JsonConvert.SerializeObject(requestBody);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var result = JObject.Parse(responseString);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            // 提取翻译结果
                            var target = result["target"];
                            if (target is JArray targetArray && targetArray.Count > 0)
                            {
                                return targetArray[0].ToString().Trim();
                            }
                            
                            // 兼容 target 是字符串的情况
                            if (target != null && target.Type == JTokenType.String)
                            {
                                return target.ToString().Trim();
                            }
                            
                            return "翻译失败：未在响应中找到有效的翻译结果。";
                        }
                        else
                        {
                            var errorMsg = result["message"] ?? "未知错误";
                            return $"翻译请求失败: HTTP {response.StatusCode} - {errorMsg}";
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
            if (string.IsNullOrEmpty(langCode) || langCode.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                return "auto";
            }

            switch (langCode)
            {
                case "zh-CN":
                    return "zh";
                case "zh-TW":
                    return "zh-Hant";
                default:
                    // 直接返回原始代码
                    return langCode;
            }
        }
    }
}