using System;
using System.Collections.Generic;
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

        static BingTranslator()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = true,
                CookieContainer = new System.Net.CookieContainer()
            };
            HttpClient = new HttpClient(handler);
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            HttpClient.DefaultRequestHeaders.Referrer = TranslatorPageUri;
        }
        private static readonly SemaphoreSlim CredentialsSemaphore = new SemaphoreSlim(1, 1);
        private static DateTime _credentialsExpiration;

        private class BingCredentials
        {
            public string Token { get; }
            public string Key { get; }
            public string ImpressionGuid { get; }

            public BingCredentials(string token, string key, string impressionGuid)
            {
                Token = token;
                Key = key;
                ImpressionGuid = impressionGuid;
            }
        }

        public static async Task<string> TranslateAsync(string text, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            try
            {
                // Use a unique GUID wrapped in brackets as a placeholder. This is more robust as it signals
                // to the translation engine that this is a self-contained token that should not be translated
                // or merged with adjacent words.
                var newlinePlaceholder = $"[{Guid.NewGuid()}]";
                var textToTranslate = text.Replace("\r\n", newlinePlaceholder).Replace("\n", newlinePlaceholder);

                var credentials = await GetOrUpdateCredentialsAsync().ConfigureAwait(false);
                var fromLang = "auto-detect";
                var targetLang = toLanguage == "zh-CN" ? "zh-Hans" : toLanguage;

                var body = new Dictionary<string, string>
                {
                    { "fromLang", fromLang },
                    { "text", textToTranslate },
                    { "to", targetLang },
                    { "tryFetchingGenderDebiasedTranslations", "true" },
                    { "token", credentials.Token },
                    { "key", credentials.Key }
                };

                var requestUri = new Uri($"https://www.bing.com/ttranslatev3?isVertical=1&IG={credentials.ImpressionGuid}&IID=translator.5028.1");

                using (var content = new FormUrlEncodedContent(body))
                {
                    using (var response = await HttpClient.PostAsync(requestUri, content).ConfigureAwait(false))
                    {
                        string translatedText = "";
                        var statusCode = (int)response.StatusCode;
                        if (statusCode == 301 || statusCode == 302 || statusCode == 303 || statusCode == 307 || statusCode == 308)
                        {
                            var redirectUri = response.Headers.Location;
                            if (redirectUri == null)
                            {
                                return "Translation failed: Redirect location not found.";
                            }

                            // Post again to the new location with the same content
                            using (var redirectedResponse = await HttpClient.PostAsync(redirectUri, content).ConfigureAwait(false))
                            {
                                redirectedResponse.EnsureSuccessStatusCode();
                                var responseString = await redirectedResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                                var jsonResponse = JArray.Parse(responseString);

                                if (jsonResponse.Count > 0 && jsonResponse[0]["translations"] is JArray translations && translations.Count > 0)
                                {
                                    translatedText = translations[0]["text"]?.ToString() ?? string.Empty;
                                }
                                else
                                {
                                    return "Translation failed: Invalid response format after redirect.";
                                }
                            }
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                            var originalResponseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var originalJsonResponse = JArray.Parse(originalResponseString);

                            if (originalJsonResponse.Count > 0 && originalJsonResponse[0]["translations"] is JArray translationsOnOriginal && translationsOnOriginal.Count > 0)
                            {
                                translatedText = translationsOnOriginal[0]["text"]?.ToString() ?? string.Empty;
                            }
                            else
                            {
                                return "Translation failed: Invalid response format.";
                            }
                        }

                        // Replace the unique placeholder back to a newline character.
                        return translatedText.Replace(newlinePlaceholder, "\n");
                    }
                }
            }
            catch (Exception e)
            {
                return $"Translation failed: {e.Message}";
            }
        }

        private static async Task<BingCredentials> GetOrUpdateCredentialsAsync()
        {
            if (_credentials != null && DateTime.UtcNow < _credentialsExpiration)
            {
                return _credentials;
            }

            await CredentialsSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                // Double-check after acquiring the lock
                if (_credentials != null && DateTime.UtcNow < _credentialsExpiration)
                {
                    return _credentials;
                }

                using (var response = await HttpClient.GetAsync(TranslatorPageUri).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var igMatch = Regex.Match(html, @"IG:""([a-fA-F0-9]{32})""");
                    if (!igMatch.Success) throw new Exception("Unable to find Bing IG value.");

                    var paramsMatch = Regex.Match(html, @"var params_AbusePreventionHelper\s*=\s*\[(\d+),""([^""]+)"",(\d+)\];");
                    if (!paramsMatch.Success) throw new Exception("Unable to find Bing credentials (key/token/expiration).");

                    var key = paramsMatch.Groups[1].Value;
                    var token = paramsMatch.Groups[2].Value;
                    var expirationMs = double.Parse(paramsMatch.Groups[3].Value);

                    _credentials = new BingCredentials(token, key, igMatch.Groups[1].Value);
                    _credentialsExpiration = DateTime.UtcNow.AddMilliseconds(expirationMs);
                    return _credentials;
                }
            }
            finally
            {
                CredentialsSemaphore.Release();
            }
        }
    }
}