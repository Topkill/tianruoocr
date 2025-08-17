using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrOCR.Helper
{
    /// <summary>
    /// 百度OCR帮助类，支持通用文字识别（标准版）和高精度版
    /// </summary>
    public static class BaiduOcrHelper
    {
        // access_token缓存（内存缓存）
        private static string accessTokenCache = null;
        private static DateTime accessTokenExpiry = DateTime.MinValue;
        private static string cachedApiKey = null;
        private static string cachedSecretKey = null;

        // 高精度版access_token缓存
        private static string accessTokenCacheHighAccuracy = null;
        private static DateTime accessTokenExpiryHighAccuracy = DateTime.MinValue;
        private static string cachedApiKeyHighAccuracy = null;
        private static string cachedSecretKeyHighAccuracy = null;

        /// <summary>
        /// 获取或刷新access_token（双重缓存机制）
        /// </summary>
        private static string GetAccessToken(string apiKey, string secretKey, bool isHighAccuracy = false)
        {
            try
            {
                // 选择对应的缓存
                ref string tokenCache = ref (isHighAccuracy ? ref accessTokenCacheHighAccuracy : ref accessTokenCache);
                ref DateTime tokenExpiry = ref (isHighAccuracy ? ref accessTokenExpiryHighAccuracy : ref accessTokenExpiry);
                ref string cachedKey = ref (isHighAccuracy ? ref cachedApiKeyHighAccuracy : ref cachedApiKey);
                ref string cachedSecret = ref (isHighAccuracy ? ref cachedSecretKeyHighAccuracy : ref cachedSecretKey);

                // 1. 检查内存缓存
                if (!string.IsNullOrEmpty(tokenCache) &&
                    cachedKey == apiKey &&
                    cachedSecret == secretKey &&
                    DateTime.Now < tokenExpiry)
                {
                    return tokenCache;
                }

                // 2. 从配置文件读取持久化的token
                string configSection = isHighAccuracy ? "密钥_百度高精度" : "密钥_百度";
                string savedToken = IniHelper.GetValue(configSection, "access_token");
                string savedExpiry = IniHelper.GetValue(configSection, "token_expiry");
                string savedApiKey = IniHelper.GetValue(configSection, "cached_api_key");

                // 检查持久化的token是否有效
                if (!string.IsNullOrEmpty(savedToken) && savedToken != "发生错误" &&
                    savedApiKey == apiKey &&
                    DateTime.TryParse(savedExpiry, out DateTime expiry) &&
                    DateTime.Now < expiry)
                {
                    // 恢复到内存缓存
                    tokenCache = savedToken;
                    tokenExpiry = expiry;
                    cachedKey = apiKey;
                    cachedSecret = secretKey;
                    return savedToken;
                }

                // 3. 获取新的access_token
                string url = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={apiKey}&client_secret={secretKey}";
                string response = CommonHelper.GetHtmlContent(url);
                
                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                JObject json = JObject.Parse(response);
                if (json["access_token"] != null)
                {
                    string newToken = json["access_token"].ToString();
                    int expiresIn = json["expires_in"]?.ToObject<int>() ?? 2592000; // 默认30天
                    
                    // 设置过期时间为29天（留1天缓冲）
                    DateTime newExpiry = DateTime.Now.AddSeconds(expiresIn - 86400);
                    
                    // 缓存到内存
                    tokenCache = newToken;
                    tokenExpiry = newExpiry;
                    cachedKey = apiKey;
                    cachedSecret = secretKey;

                    // 持久化到配置文件
                    IniHelper.SetValue(configSection, "access_token", newToken);
                    IniHelper.SetValue(configSection, "token_expiry", newExpiry.ToString("yyyy-MM-dd HH:mm:ss"));
                    IniHelper.SetValue(configSection, "cached_api_key", apiKey);

                    return newToken;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取百度access_token失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 清除access_token缓存
        /// </summary>
        public static void ClearAccessTokenCache(bool isHighAccuracy = false)
        {
            if (isHighAccuracy)
            {
                accessTokenCacheHighAccuracy = null;
                accessTokenExpiryHighAccuracy = DateTime.MinValue;
                cachedApiKeyHighAccuracy = null;
                cachedSecretKeyHighAccuracy = null;
                
                IniHelper.SetValue("密钥_百度高精度", "access_token", "");
                IniHelper.SetValue("密钥_百度高精度", "token_expiry", "");
                IniHelper.SetValue("密钥_百度高精度", "cached_api_key", "");
            }
            else
            {
                accessTokenCache = null;
                accessTokenExpiry = DateTime.MinValue;
                cachedApiKey = null;
                cachedSecretKey = null;
                
                IniHelper.SetValue("密钥_百度", "access_token", "");
                IniHelper.SetValue("密钥_百度", "token_expiry", "");
                IniHelper.SetValue("密钥_百度", "cached_api_key", "");
            }
        }

        /// <summary>
        /// 通用文字识别（标准版）
        /// </summary>
        public static string GeneralBasic(byte[] imageBytes, string apiKey, string secretKey, string languageType = null)
        {
            try
            {
                // 获取access_token
                string accessToken = GetAccessToken(apiKey, secretKey, false);
                if (string.IsNullOrEmpty(accessToken))
                {
                    return "获取百度access_token失败，请检查API Key和Secret Key";
                }

                // 如果没有指定语言，从配置文件读取
                if (string.IsNullOrEmpty(languageType))
                {
                    languageType = IniHelper.GetValue("百度接口设置", "language_type");
                    if (string.IsNullOrEmpty(languageType) || languageType == "发生错误")
                    {
                        languageType = "CHN_ENG"; // 默认中英文混合
                    }
                }

                // 构建请求
                string url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token={accessToken}";
                string imageBase64 = Convert.ToBase64String(imageBytes);
                string postData;

                // 根据百度OCR API文档，`detect_language` 和 `language_type` 是互斥参数？
                // 当 `detect_language` 为 `true` 时，不应传入 `language_type`，反之亦然。
                // 此处逻辑确保每次请求只使用其中一个参数。
                if (languageType == "auto_detect")
                {
                    // 当用户选择自动检测时，使用 detect_language=true
                    postData = $"image={HttpUtility.UrlEncode(imageBase64)}&detect_language=true";
                }
                else
                {
                    // 当用户选择特定语言时，使用 language_type
                    postData = $"image={HttpUtility.UrlEncode(imageBase64)}&language_type={languageType}";
                }

                // 发送请求
                string response = CommonHelper.PostStrData(url, postData);
                if (string.IsNullOrEmpty(response))
                {
                    return "百度OCR请求失败";
                }

                // 解析响应
                JObject json = JObject.Parse(response);
                
                // 检查是否有错误
                if (json["error_code"] != null)
                {
                    string errorCode = json["error_code"].ToString();
                    string errorMsg = json["error_msg"]?.ToString() ?? "未知错误";
                    
                    // 如果是token失效，清除缓存并重试一次
                    if (errorCode == "110" || errorCode == "111")
                    {
                        ClearAccessTokenCache(false);
                        accessToken = GetAccessToken(apiKey, secretKey, false);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token={accessToken}";
                            response = CommonHelper.PostStrData(url, postData);
                            if (!string.IsNullOrEmpty(response))
                            {
                                json = JObject.Parse(response);
                                if (json["error_code"] == null)
                                {
                                    goto ProcessResult;
                                }
                            }
                        }
                    }
                    
                    return $"百度OCR错误 {errorCode}: {errorMsg}";
                }

                ProcessResult:
                // 提取识别结果
                var wordsResult = json["words_result"] as JArray;
                if (wordsResult == null || wordsResult.Count == 0)
                {
                    return "***该区域未发现文本***";
                }

                StringBuilder sb = new StringBuilder();
                foreach (var item in wordsResult)
                {
                    if (item["words"] != null)
                    {
                        sb.AppendLine(item["words"].ToString());
                    }
                }

                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"百度OCR异常: {ex.Message}";
            }
        }

        /// <summary>
        /// 通用文字识别（高精度版）
        /// </summary>
        public static string AccurateBasic(byte[] imageBytes, string apiKey, string secretKey, string languageType = null)
        {
            try
            {
                // 获取access_token
                string accessToken = GetAccessToken(apiKey, secretKey, true);
                if (string.IsNullOrEmpty(accessToken))
                {
                    return "获取百度高精度access_token失败，请检查API Key和Secret Key";
                }

                // 如果没有指定语言，从配置文件读取
                if (string.IsNullOrEmpty(languageType))
                {
                    languageType = IniHelper.GetValue("百度高精度接口设置", "language_type");
                    if (string.IsNullOrEmpty(languageType) || languageType == "发生错误")
                    {
                        languageType = "CHN_ENG"; // 高精度版默认中英文
                    }
                }

                // 构建请求
                string url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic?access_token={accessToken}";
                string imageBase64 = Convert.ToBase64String(imageBytes);
                string postData = $"image={HttpUtility.UrlEncode(imageBase64)}&language_type={languageType}";

                // 发送请求
                string response = CommonHelper.PostStrData(url, postData);
                if (string.IsNullOrEmpty(response))
                {
                    return "百度高精度OCR请求失败";
                }

                // 解析响应
                JObject json = JObject.Parse(response);
                
                // 检查是否有错误
                if (json["error_code"] != null)
                {
                    string errorCode = json["error_code"].ToString();
                    string errorMsg = json["error_msg"]?.ToString() ?? "未知错误";
                    
                    // 如果是token失效，清除缓存并重试一次
                    if (errorCode == "110" || errorCode == "111")
                    {
                        ClearAccessTokenCache(true);
                        accessToken = GetAccessToken(apiKey, secretKey, true);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic?access_token={accessToken}";
                            response = CommonHelper.PostStrData(url, postData);
                            if (!string.IsNullOrEmpty(response))
                            {
                                json = JObject.Parse(response);
                                if (json["error_code"] == null)
                                {
                                    goto ProcessResult;
                                }
                            }
                        }
                    }
                    
                    return $"百度高精度OCR错误 {errorCode}: {errorMsg}";
                }

                ProcessResult:
                // 提取识别结果
                var wordsResult = json["words_result"] as JArray;
                if (wordsResult == null || wordsResult.Count == 0)
                {
                    return "***该区域未发现文本***";
                }

                StringBuilder sb = new StringBuilder();
                foreach (var item in wordsResult)
                {
                    if (item["words"] != null)
                    {
                        sb.AppendLine(item["words"].ToString());
                    }
                }

                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"百度高精度OCR异常: {ex.Message}";
            }
        }

        /// <summary>
        /// 异步验证百度API密钥的有效性
        /// </summary>
        public static async System.Threading.Tasks.Task<bool> VerifyKeys(string apiKey, string secretKey)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey))
            {
                return false;
            }

            try
            {
                string url = $"https://aip.baidubce.com/oauth/2.0/token";
                string data = $"grant_type=client_credentials&client_id={apiKey}&client_secret={secretKey}";

                using (var client = new System.Net.Http.HttpClient())
                {
                    var content = new System.Net.Http.StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(jsonString);
                        return json["access_token"] != null;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证百度密钥时发生异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取支持的语言列表（标准版）
        /// </summary>
        public static Dictionary<string, string> GetStandardLanguages()
        {
            return new Dictionary<string, string>
            {
                { "auto_detect", "自动检测" },
                { "CHN_ENG", "中英文混合" },
                { "ENG", "英文" },
                { "JAP", "日语" },
                { "KOR", "韩语" },
                { "FRE", "法语" },
                { "SPA", "西班牙语" },
                { "POR", "葡萄牙语" },
                { "GER", "德语" },
                { "ITA", "意大利语" },
                { "RUS", "俄语" }
            };
        }

        /// <summary>
        /// 获取支持的语言列表（高精度版）
        /// </summary>
        public static Dictionary<string, string> GetAccurateLanguages()
        {
            return new Dictionary<string, string>
            {
                { "auto_detect", "自动检测" },
                { "CHN_ENG", "中英文混合" },
                { "ENG", "英文" },
                { "JAP", "日语" },
                { "KOR", "韩语" },
                { "FRE", "法语" },
                { "SPA", "西班牙语" },
                { "POR", "葡萄牙语" },
                { "GER", "德语" },
                { "ITA", "意大利语" },
                { "RUS", "俄语" },
                { "DAN", "丹麦语" },
                { "DUT", "荷兰语" },
                { "MAL", "马来语" },
                { "SWE", "瑞典语" },
                { "IND", "印尼语" },
                { "POL", "波兰语" },
                { "ROM", "罗马尼亚语" },
                { "TUR", "土耳其语" },
                { "GRE", "希腊语" },
                { "HUN", "匈牙利语" },
                { "THA", "泰语" },
                { "VIE", "越南语" },
                { "ARA", "阿拉伯语" },
                { "HIN", "印地语" }
            };
        }
    }
}