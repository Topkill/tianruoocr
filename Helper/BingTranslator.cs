using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace TrOCR.Helper
{
    public static class BingTranslator
    {
        private static readonly Uri TranslatorPageUri = new Uri("https://www.bing.com/translator");
        private static readonly HttpClient HttpClient;
        private static BingCredentials _credentials;
        private static Uri _translatorApiBaseUri = new Uri("https://www.bing.com/");

        static BingTranslator()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = false  // 禁用自动cookie管理，我们手动处理
            };

            HttpClient = new HttpClient(handler);
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            HttpClient.DefaultRequestHeaders.Referrer = TranslatorPageUri;
        }

        // 手动管理的cookie存储
        private static readonly Dictionary<string, string> _cookies = new Dictionary<string, string>();
        private static readonly object _cookieLock = new object();
        private static readonly SemaphoreSlim CredentialsSemaphore = new SemaphoreSlim(1, 1);
        private static DateTime _credentialsExpiration;

        private class BingCredentials
        {
            public string Token { get; }
            public string Key { get; }
            public string ImpressionGuid { get; }
            public string Market { get; }

            public BingCredentials(string token, string key, string impressionGuid, string market = null)
            {
                Token = token;
                Key = key;
                ImpressionGuid = impressionGuid;
                Market = market ?? "en-US"; // 默认市场
            }
        }

        public static async Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            try
            {
                // 直接按换行符分割文本
                var chunks = SplitTextByNewlines(text, 1000);
                var translationTasks = new List<Task<string>>();

                foreach (var chunk in chunks)
                {
                    translationTasks.Add(TranslateChunkAsync(chunk, fromLanguage, toLanguage));
                }

                var translatedChunks = await Task.WhenAll(translationTasks).ConfigureAwait(false);

                // 保持原始的换行符格式
                return string.Join("", translatedChunks);
            }
            catch (Exception e)
            {
                return $"Translation failed: {e.Message}";
            }
        }

        private static IEnumerable<string> SplitTextByNewlines(string text, int maxLength)
        {
            // 保留原始换行符，按行分割
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var currentChunk = "";
            var isFirstLine = true;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                // 确定使用哪种换行符（如果不是最后一行）
                var newlineChar = i < lines.Length - 1 ? "\n" : "";
                var lineWithNewline = line + newlineChar;

                // 如果当前块加上新行会超过最大长度
                if (!isFirstLine && currentChunk.Length + lineWithNewline.Length > maxLength)
                {
                    if (currentChunk.Length > 0)
                    {
                        yield return currentChunk;
                        currentChunk = "";
                        isFirstLine = true;
                    }
                }

                // 如果单行本身就超过最大长度，需要分割
                if (lineWithNewline.Length > maxLength)
                {
                    // 先返回当前累积的内容
                    if (currentChunk.Length > 0)
                    {
                        yield return currentChunk;
                        currentChunk = "";
                        isFirstLine = true;
                    }

                    // 分割长行
                    var remaining = line;
                    while (remaining.Length > 0)
                    {
                        var chunkSize = Math.Min(maxLength - (remaining == line && i < lines.Length - 1 ? 1 : 0), remaining.Length);
                        var chunk = remaining.Substring(0, chunkSize);
                        remaining = remaining.Substring(chunkSize);

                        // 如果这是原行的最后一部分且不是最后一行，添加换行符
                        if (remaining.Length == 0 && i < lines.Length - 1)
                        {
                            chunk += "\n";
                        }

                        yield return chunk;
                    }
                    continue;
                }

                currentChunk += lineWithNewline;
                isFirstLine = false;
            }

            if (currentChunk.Length > 0)
            {
                yield return currentChunk;
            }
        }

        private static async Task<string> TranslateChunkAsync(string text, string fromLanguage, string toLanguage)
        {
            var credentials = await GetOrUpdateCredentialsAsync(toLanguage).ConfigureAwait(false);
            var fromLang = fromLanguage == "auto" ? "auto-detect" : fromLanguage;
            var targetLang = toLanguage == "zh-CN" ? "zh-Hans" : toLanguage;

            // 在发送请求前再次更新cookie，确保mkt参数正确
            UpdateMarketCookie(credentials.Market);

            var body = new Dictionary<string, string>
            {
                { "fromLang", fromLang },
                { "text", text },
                { "to", targetLang },
                { "tryFetchingGenderDebiasedTranslations", "true" },
                { "token", credentials.Token },
                { "key", credentials.Key }
            };

            var requestUri = new Uri(_translatorApiBaseUri, $"ttranslatev3?isVertical=1&IG={credentials.ImpressionGuid}&IID=translator.5028.1");

            // 创建请求消息，手动设置Cookie头
            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                request.Content = new FormUrlEncodedContent(body);

                // 构建Cookie头
                var cookieHeader = BuildCookieHeader(credentials.Market);
                if (!string.IsNullOrEmpty(cookieHeader))
                {
                    request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);
                }

                using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                {
                    // The POST request can also be redirected
                    if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400 && response.Headers.Location != null)
                    {
                        var redirectUri = response.Headers.Location;

                        // 创建重定向请求，同样手动设置Cookie
                        using (var redirectRequest = new HttpRequestMessage(HttpMethod.Post, redirectUri))
                        {
                            redirectRequest.Content = new FormUrlEncodedContent(body);

                            // 为重定向请求也设置Cookie
                            var redirectCookieHeader = BuildCookieHeader(credentials.Market);
                            if (!string.IsNullOrEmpty(redirectCookieHeader))
                            {
                                redirectRequest.Headers.TryAddWithoutValidation("Cookie", redirectCookieHeader);
                            }

                            using (var redirectedResponse = await HttpClient.SendAsync(redirectRequest).ConfigureAwait(false))
                            {
                                redirectedResponse.EnsureSuccessStatusCode();
                                var responseString = await redirectedResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                                return ParseTranslationResponse(responseString);
                            }
                        }
                    }

                    response.EnsureSuccessStatusCode();
                    var originalResponseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return ParseTranslationResponse(originalResponseString);
                }
            }
        }

        private static string ParseTranslationResponse(string responseString)
        {
            var jsonResponse = JArray.Parse(responseString);
            if (jsonResponse.Count > 0 && jsonResponse[0]["translations"] is JArray translations && translations.Count > 0)
            {
                return translations[0]["text"]?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        private static async Task<BingCredentials> GetOrUpdateCredentialsAsync(string targetLanguage = null)
        {
            // 如果凭证有效，直接返回（UpdateMarketCookie会确保mkt正确）
            if (_credentials != null && DateTime.UtcNow < _credentialsExpiration)
            {
                // 确保cookie中的mkt与目标语言匹配
                var finalMarket = GetMarketFromLanguage(targetLanguage, _credentials.Market);
                UpdateMarketCookie(finalMarket);
                return _credentials;
            }

            await CredentialsSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                // Double-check after acquiring the lock
                if (_credentials != null && DateTime.UtcNow < _credentialsExpiration)
                {
                    var finalMarket = GetMarketFromLanguage(targetLanguage, _credentials.Market);
                    UpdateMarketCookie(finalMarket);
                    return _credentials;
                }

                // 清除旧的cookies，开始新的会话
                lock (_cookieLock)
                {
                    _cookies.Clear();
                }

                // 创建请求消息
                using (var request = new HttpRequestMessage(HttpMethod.Get, TranslatorPageUri))
                {
                    using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        HttpResponseMessage finalResponse = response;
                        HttpResponseMessage redirectedResponse = null;
                        try
                        {
                            // 处理重定向
                            if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400 && response.Headers.Location != null)
                            {
                                var redirectUri = response.Headers.Location;
                                if (!redirectUri.IsAbsoluteUri)
                                {
                                    redirectUri = new Uri(response.RequestMessage.RequestUri, redirectUri);
                                }
                                _translatorApiBaseUri = new Uri(redirectUri.GetLeftPart(UriPartial.Authority));

                                using (var redirectRequest = new HttpRequestMessage(HttpMethod.Get, redirectUri))
                                {
                                    redirectedResponse = await HttpClient.SendAsync(redirectRequest).ConfigureAwait(false);
                                    finalResponse = redirectedResponse;
                                }
                            }

                            finalResponse.EnsureSuccessStatusCode();

                            // 解析并存储Set-Cookie头
                            if (finalResponse.Headers.TryGetValues("Set-Cookie", out var setCookies))
                            {
                                ParseAndStoreCookies(setCookies);
                            }

                            var html = await finalResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                            var igMatch = Regex.Match(html, @"IG:""([a-fA-F0-9]{32})""");
                            if (!igMatch.Success) throw new Exception("Unable to find Bing IG value.");

                            var paramsMatch = Regex.Match(html, @"var params_AbusePreventionHelper\s*=\s*\[(\d+),""([^""]+)"",(\d+)\];");
                            if (!paramsMatch.Success) throw new Exception("Unable to find Bing credentials (key/token/expiration).");

                         
                            // 1. 从HTML中提取市场信息作为备用值
                            string extractedMarket = null;
                            var mktMatch = Regex.Match(html, @"mkt['""\s]*[:=]['""\s]*([a-zA-Z]{2}-[a-zA-Z]{2})", RegexOptions.IgnoreCase);
                            if (mktMatch.Success)
                            {
                                extractedMarket = mktMatch.Groups[1].Value;
                            }
                            else
                            {
                                mktMatch = Regex.Match(html, @"[&?]mkt=([a-zA-Z]{2}-[a-zA-Z]{2})", RegexOptions.IgnoreCase);
                                if (mktMatch.Success)
                                {
                                    extractedMarket = mktMatch.Groups[1].Value;
                                }
                            }

                            // 2. 调用新方法来决定最终的市场
                            string finalMarket = GetMarketFromLanguage(targetLanguage, extractedMarket);
                        

                            var key = paramsMatch.Groups[1].Value;
                            var token = paramsMatch.Groups[2].Value;
                            var expirationMs = double.Parse(paramsMatch.Groups[3].Value);

                            _credentials = new BingCredentials(token, key, igMatch.Groups[1].Value, finalMarket);
                            _credentialsExpiration = DateTime.UtcNow.AddMilliseconds(expirationMs);

                            // 确保cookie中的mkt参数是最新的
                            UpdateMarketCookie(finalMarket);

                            return _credentials;
                        }
                        finally
                        {
                            redirectedResponse?.Dispose();
                        }
                    }
                }
            }
            finally
            {
                CredentialsSemaphore.Release();
            }
        }
        private static string GetMarketFromLanguage(string language, string htmlMarket)
        {
            // 如果htmlMarket为空，则设置一个最终的备用值
            string fallbackMarket = string.IsNullOrEmpty(htmlMarket) ? "en-US" : htmlMarket;

            // 根据目标语言映射到对应的市场代码
            switch (language?.ToLower())
            {
                case "zh-cn":
                case "zh-hans":
                    return "zh-CN";
                case "zh-tw":
                case "zh-hant":
                    return "zh-TW";
                case "en":
                case "en-us":
                    return "en-US";
                case "ja":
                case "ja-jp":
                    return "ja-JP";
                case "ko":
                case "ko-kr":
                    return "ko-KR";
                case "fr":
                case "fr-fr":
                    return "fr-FR";
                case "de":
                case "de-de":
                    return "de-DE";
                case "es":
                case "es-es":
                    return "es-ES";
                case "ru":
                case "ru-ru":
                    return "ru-RU";
                case "pt":
                case "pt-br":
                    return "pt-BR";
                case "it":
                case "it-it":
                    return "it-IT";
                case "ar":
                case "ar-sa":
                    return "ar-SA";
                // 你可以在这里添加更多语言到市场的映射
                default:
                    return fallbackMarket; // 默认使用从HTML提取的市场，如果提取失败则用 "en-US"
            }
        }
        private static void UpdateMarketCookie(string market)
        {
            lock (_cookieLock)
            {
                // 从现有的_EDGE_S cookie中提取SID
                string sidValue = null;
                if (_cookies.ContainsKey("_EDGE_S"))
                {
                    var edgeSValue = _cookies["_EDGE_S"];
                    // 由于ParseAndStoreCookies已经清理过，这里的edgeSValue应该是"SID=..."
                    var sidMatch = System.Text.RegularExpressions.Regex.Match(edgeSValue, @"SID=([^&;]+)");
                    if (sidMatch.Success)
                    {
                        sidValue = sidMatch.Groups[1].Value;
                    }
                }

                // 如果没有SID，生成一个新的
                if (string.IsNullOrEmpty(sidValue))
                {
                    sidValue = System.Guid.NewGuid().ToString("N").ToUpper();
                }

                // 重新构建并设置正确格式的_EDGE_S cookie：SID=xxx&mkt=xxx
                _cookies["_EDGE_S"] = $"SID={sidValue}&mkt=zh-CN";
            }
        }

        private static void ParseAndStoreCookies(IEnumerable<string> setCookieHeaders)
        {
            lock (_cookieLock)
            {
                foreach (var setCookie in setCookieHeaders)
                {
                    var parts = setCookie.Split(';');
                    if (parts.Length > 0)
                    {
                        var nameValue = parts[0].Split(new[] { '=' }, 2);
                        if (nameValue.Length == 2)
                        {
                            var name = nameValue[0].Trim();
                            var value = nameValue[1].Trim();

                            // CHANGE: 精确处理 _EDGE_S Cookie
                            if (name == "_EDGE_S")
                            {
                                // 无论原始值是什么 (例如 F=1&SID=...)，我们只找SID
                                var sidMatch = Regex.Match(value, @"SID=([^&;]+)");
                                if (sidMatch.Success)
                                {
                                    // 只存储 SID=... 部分，丢弃 F=1 等其他参数
                                    _cookies[name] = sidMatch.Value;
                                }
                                // 如果没有找到SID，我们就不存储这个cookie，因为它不符合我们的需要
                            }
                            else
                            {
                                _cookies[name] = value;
                            }
                        }
                    }
                }
            }
        }

        private static string BuildCookieHeader(string market) // market参数现在是可选的，因为UpdateMarketCookie已处理
        {
            lock (_cookieLock)
            {
                // CHANGE: 大幅简化。UpdateMarketCookie已经处理了所有复杂的逻辑。
                // 这个方法现在只负责将字典转换为Cookie头字符串。

                // 在构建前最后一次确保 mkt 是最新的
                if (!string.IsNullOrEmpty(market))
                {
                    UpdateMarketCookie(market);
                }

                if (_cookies.Count == 0)
                    return string.Empty;

                return string.Join("; ", _cookies.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }
        }
    }
}