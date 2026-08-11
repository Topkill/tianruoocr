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
        private static readonly string TranslateUrl = "https://edge.microsoft.com/translate/translatetext";
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
             // 全局禁用 Expect: 100-continue
            System.Net.ServicePointManager.Expect100Continue = false;
        }

        /// <summary>
        /// 构造 translatetext 端点请求体：JSON 字符串数组 ["text"]。
        /// 新端点要求字符串数组，旧端点的对象数组 [{Text:""}] 已被拒绝(400)。
        /// </summary>
        public static string BuildRequestBody(string text)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new[] { text });
        }

        /// <summary>
        /// 解析 translatetext 端点响应，返回译文文本；translations 缺失/为空返回空串。
        /// 响应结构与旧 Azure v3 同构：[{"translations":[{"text":"...","to":"..."}]}]。
        /// 畸形 JSON（非预期 body）抛 JArray 解析异常，由调用方 TranslateAsync 的 catch
        /// 统一处理为「翻译失败: …」——与迁移前内联解析语义一致，保留可诊断性。
        /// </summary>
        public static string ParseResponse(string json)
        {
            var result = JArray.Parse(json);
            if (result.Count > 0 && result[0]["translations"] != null)
            {
                var translations = result[0]["translations"] as JArray;
                if (translations != null && translations.Count > 0)
                {
                    return translations[0]["text"]?.ToString()?.Trim() ?? string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 翻译文本（使用 Microsoft Edge translatetext 免鉴权端点）。
        /// 错误以字符串形式返回（与项目其它翻译 provider 一致），调用方原样显示。
        /// </summary>
        public static async Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            try
            {
                // 转换语言代码：auto/空 → "" 触发服务端自动检测
                var from = ConvertToMicrosoftLangCode(fromLanguage);
                var to = ConvertToMicrosoftLangCode(toLanguage);

                // 新免鉴权端点：无需 token；query 参数 from(空=自动检测)/to/isEnterpriseClient
                var url = $"{TranslateUrl}?from={from}&to={to}&isEnterpriseClient=false";

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    // body 为字符串数组 ["text"]（新端点要求）；Content-Type 由 StringContent 自动带。
                    // User-Agent 由静态构造函数的全局 DefaultRequestHeaders 兜底，无需 request 级重复。
                    request.Content = new StringContent(BuildRequestBody(text), Encoding.UTF8, "application/json");

                    using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            return ParseResponse(responseString);
                        }
                        return $"翻译请求失败: HTTP {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"翻译失败: {ex.Message}";
            }
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

    }
}