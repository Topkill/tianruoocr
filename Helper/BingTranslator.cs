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
        private const string NewlinePlaceholder = "\uE001";
        private static readonly HttpClient HttpClient;
        private static BingCredentials _credentials;
        private static Uri _translatorApiBaseUri = new Uri("https://www.bing.com/");

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

        public static async Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            try
            {
                var textWithPlaceholders = text.Replace("\r\n", NewlinePlaceholder).Replace("\n", NewlinePlaceholder);
                // var textWithPlaceholders = text.Replace("\r\n", "\r\n" + NewlinePlaceholder).Replace("\n", "\n" + NewlinePlaceholder);

                var chunks = SplitText(textWithPlaceholders, 1000, NewlinePlaceholder);
                var translationTasks = new List<Task<string>>();

                foreach (var chunk in chunks)
                {
                    translationTasks.Add(TranslateChunkAsync(chunk, fromLanguage, toLanguage));
                }

                var translatedChunks = await Task.WhenAll(translationTasks).ConfigureAwait(false);
                var combined = string.Join("", translatedChunks);

                return combined.Replace(NewlinePlaceholder, "\n");
                // return combined.Replace("\r\n" + NewlinePlaceholder, "\n").Replace("\n" + NewlinePlaceholder,"\n").Replace(NewlinePlaceholder,"\n");
            }
            catch (Exception e)
            {
                return $"Translation failed: {e.Message}";
            }
        }

        private static IEnumerable<string> SplitText(string text, int maxLength, string separator)
        {
            var lines = text.Split(new[] { separator }, StringSplitOptions.None);
            var currentChunk = "";

            foreach (var line in lines)
            {
                if (currentChunk.Length + line.Length + separator.Length > maxLength)
                {
                    if (currentChunk.Length > 0)
                    {
                        yield return currentChunk;
                        currentChunk = "";
                    }

                    if (line.Length > maxLength)
                    {
                        // if a single line is too long, split it by length
                        for (int i = 0; i < line.Length; i += maxLength)
                        {
                            yield return line.Substring(i, Math.Min(maxLength, line.Length - i));
                        }
                        continue;
                    }
                }
                currentChunk += (currentChunk.Length > 0 ? separator : "") + line;
            }

            if (currentChunk.Length > 0)
            {
                yield return currentChunk;
            }
        }

        private static async Task<string> TranslateChunkAsync(string text, string fromLanguage, string toLanguage)
        {
            var credentials = await GetOrUpdateCredentialsAsync().ConfigureAwait(false);
            var fromLang = fromLanguage == "auto" ? "auto-detect" : fromLanguage;
            var targetLang = toLanguage == "zh-CN" ? "zh-Hans" : toLanguage;

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

            using (var content = new FormUrlEncodedContent(body))
            {
                using (var response = await HttpClient.PostAsync(requestUri, content).ConfigureAwait(false))
                {
                    // The POST request can also be redirected
                    if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400 && response.Headers.Location != null)
                    {
                        var redirectUri = response.Headers.Location;
                        using (var redirectedContent = new FormUrlEncodedContent(body))
                        using (var redirectedResponse = await HttpClient.PostAsync(redirectUri, redirectedContent).ConfigureAwait(false))
                        {
                            redirectedResponse.EnsureSuccessStatusCode();
                            var responseString = await redirectedResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                            return ParseTranslationResponse(responseString);
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
                    HttpResponseMessage finalResponse = response;
                    HttpResponseMessage redirectedResponse = null;
                    try
                    {
                        // Manually handle redirection to get credentials from the correct regional domain (e.g., cn.bing.com)
                        if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400 && response.Headers.Location != null)
                        {
                            var redirectUri = response.Headers.Location;
                            if (!redirectUri.IsAbsoluteUri)
                            {
                                redirectUri = new Uri(response.RequestMessage.RequestUri, redirectUri);
                            }
                            _translatorApiBaseUri = new Uri(redirectUri.GetLeftPart(UriPartial.Authority));
                            redirectedResponse = await HttpClient.GetAsync(redirectUri).ConfigureAwait(false);
                            finalResponse = redirectedResponse;
                        }

                        finalResponse.EnsureSuccessStatusCode();
                        var html = await finalResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

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
                    finally
                    {
                        redirectedResponse?.Dispose();
                    }
                }
            }
            finally
            {
                CredentialsSemaphore.Release();
            }
        }
    }
}