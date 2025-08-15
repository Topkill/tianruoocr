using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TrOCR.Helper
{
    /// <summary>
    /// 新的Bing翻译接口实现（使用Microsoft Edge翻译API）
    /// </summary>
    public static class BingTranslator2
    {
        private static readonly HttpClient HttpClient;
        private static readonly string AuthUrl = "https://edge.microsoft.com/translate/auth";
        private static readonly string TranslateUrl = "https://api-edge.cognitive.microsofttranslator.com/translate";
        private static readonly string DetectUrl = "https://api-edge.cognitive.microsofttranslator.com/detect";
        
        // 语言映射表
        private static readonly Dictionary<string, string> LanguageMap = new Dictionary<string, string>
        {
            { "zh-Hans", "zh-CN" },
            { "zh-Hant", "zh-TW" },
            { "en", "en" },
            { "ja", "ja" },
            { "ko", "ko" },
            { "fr", "fr" },
            { "es", "es" },
            { "ru", "ru" },
            { "de", "de" },
            { "it", "it" },
            { "tr", "tr" },
            { "pt-pt", "pt-PT" },
            { "pt", "pt-BR" },
            { "vi", "vi" },
            { "id", "id" },
            { "th", "th" },
            { "ms", "ms" },
            { "ar", "ar" },
            { "hi", "hi" },
            { "mn-Cyrl", "mn-CY" },
            { "mn-Mong", "mn-MO" },
            { "km", "km" },
            { "nb", "nb-NO" },
            { "fa", "fa" },
            { "uk", "uk" }
        };

        // 反向映射表（用于将我们的语言代码转换为Bing的格式）
        private static readonly Dictionary<string, string> ReverseLanguageMap = new Dictionary<string, string>
        {
            { "zh-CN", "zh-Hans" },
            { "zh-TW", "zh-Hant" },
            { "en", "en" },
            { "ja", "ja" },
            { "ko", "ko" },
            { "fr", "fr" },
            { "es", "es" },
            { "ru", "ru" },
            { "de", "de" },
            { "it", "it" },
            { "tr", "tr" },
            { "pt-PT", "pt-pt" },
            { "pt-BR", "pt" },
            { "vi", "vi" },
            { "id", "id" },
            { "th", "th" },
            { "ms", "ms" },
            { "ar", "ar" },
            { "hi", "hi" },
            { "mn-CY", "mn-Cyrl" },
            { "mn-MO", "mn-Mong" },
            { "km", "km" },
            { "nb-NO", "nb" },
            { "fa", "fa" },
            { "uk", "uk" }
        };

        static BingTranslator2()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true
            };

            HttpClient = new HttpClient(handler);
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", 
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.42");
        }

        /// <summary>
        /// 获取认证Token
        /// </summary>
        private static async Task<string> GetAuthTokenAsync()
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, AuthUrl))
                {
                    request.Headers.TryAddWithoutValidation("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.42");

                    using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取Token失败: {ex.Message}");
            }
            return null;
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
                // 获取认证Token
                var token = await GetAuthTokenAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(token))
                {
                    return "获取认证Token失败";
                }

                // 转换语言代码
                var from = ConvertToMicrosoftLangCode(fromLanguage);
                var to = ConvertToMicrosoftLangCode(toLanguage);

                // 构建请求URL
                var url = $"{TranslateUrl}?api-version=3.0&from={from}&to={to}&includeSentenceLength=true";

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    // 设置请求头
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "zh-TW,zh;q=0.9,ja;q=0.8,zh-CN;q=0.7,en-US;q=0.6,en;q=0.5");
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    request.Headers.TryAddWithoutValidation("Pragma", "no-cache");
                    request.Headers.TryAddWithoutValidation("Sec-Ch-Ua", "\"Microsoft Edge\";v=\"113\", \"Chromium\";v=\"113\", \"Not-A.Brand\";v=\"24\"");
                    request.Headers.TryAddWithoutValidation("Sec-Ch-Ua-Mobile", "?0");
                    request.Headers.TryAddWithoutValidation("Sec-Ch-Ua-Platform", "\"Windows\"");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "cross-site");
                    request.Headers.TryAddWithoutValidation("Referer", "https://appsumo.com/");
                    request.Headers.TryAddWithoutValidation("Referrer-Policy", "strict-origin-when-cross-origin");
                    request.Headers.TryAddWithoutValidation("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.42");

                    // 设置请求体
                    var bodyArray = new[] { new { Text = text } };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyArray);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result = JArray.Parse(responseString);
                            
                            if (result.Count > 0 && result[0]["translations"] != null)
                            {
                                var translations = result[0]["translations"] as JArray;
                                if (translations != null && translations.Count > 0)
                                {
                                    return translations[0]["text"]?.ToString()?.Trim() ?? string.Empty;
                                }
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

            return string.Empty;
        }

        /// <summary>
        /// 检测语言
        /// </summary>
        public static async Task<string> DetectLanguageAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "en";

            try
            {
                // 获取认证Token
                var token = await GetAuthTokenAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(token))
                {
                    return "en";
                }

                // 构建请求URL
                var url = $"{DetectUrl}?api-version=3.0";

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    // 设置请求头
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "zh-TW,zh;q=0.9,ja;q=0.8,zh-CN;q=0.7,en-US;q=0.6,en;q=0.5");
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    request.Headers.TryAddWithoutValidation("Pragma", "no-cache");
                    request.Headers.TryAddWithoutValidation("Sec-Ch-Ua", "\"Microsoft Edge\";v=\"113\", \"Chromium\";v=\"113\", \"Not-A.Brand\";v=\"24\"");
                    request.Headers.TryAddWithoutValidation("Sec-Ch-Ua-Mobile", "?0");
                    request.Headers.TryAddWithoutValidation("Sec-Ch-Ua-Platform", "\"Windows\"");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "cross-site");
                    request.Headers.TryAddWithoutValidation("Referer", "https://appsumo.com/");
                    request.Headers.TryAddWithoutValidation("Referrer-Policy", "strict-origin-when-cross-origin");
                    request.Headers.TryAddWithoutValidation("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.42");

                    // 设置请求体
                    var bodyArray = new[] { new { Text = text } };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyArray);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result = JArray.Parse(responseString);
                            
                            if (result.Count > 0 && result[0]["language"] != null)
                            {
                                var detectedLang = result[0]["language"].ToString();
                                
                                // 转换为我们的语言代码格式
                                if (LanguageMap.ContainsKey(detectedLang))
                                {
                                    return LanguageMap[detectedLang];
                                }
                                return detectedLang;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"语言检测失败: {ex.Message}");
            }

            return "en";
        }

        /// <summary>
        /// 将我们的语言代码转换为Microsoft的格式
        /// </summary>
        private static string ConvertToMicrosoftLangCode(string langCode)
        {
            if (string.IsNullOrEmpty(langCode) || langCode == "auto" || langCode == "auto-detect")
            {
                return "";
            }

            if (ReverseLanguageMap.ContainsKey(langCode))
            {
                return ReverseLanguageMap[langCode];
            }

            return langCode;
        }

        /// <summary>
        /// 将Microsoft的语言代码转换为我们的格式
        /// </summary>
        private static string ConvertFromMicrosoftLangCode(string langCode)
        {
            if (string.IsNullOrEmpty(langCode))
            {
                return "auto";
            }

            if (LanguageMap.ContainsKey(langCode))
            {
                return LanguageMap[langCode];
            }

            return langCode;
        }
    }
}