using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
         // 【新增】创建一个可供复用的静态 HttpHelper 实例
        private static readonly HttpHelper _httpHelper = new HttpHelper();
        /// <summary>
        /// 获取或刷新access_token（使用StaticValue作为一级缓存）
        /// </summary>
        private static string GetAccessToken(string apiKey, string secretKey, bool isHighAccuracy = false)
        {
            try
            {
                // 1. 选择正确的StaticValue缓存字段
                ref string tokenCache = ref (isHighAccuracy ? ref StaticValue.BaiduAccurateAccessToken : ref StaticValue.BaiduAccessToken);
                ref DateTime tokenExpiry = ref (isHighAccuracy ? ref StaticValue.BaiduAccurateAccessTokenExpiry : ref StaticValue.BaiduAccessTokenExpiry);

                // 2. 检查内存缓存是否有效
                // 注意：这里不再检查API Key是否匹配，FmMain中设置API Key时应负责清除旧Token
                if (!string.IsNullOrEmpty(tokenCache) && tokenCache != "发生错误" && DateTime.Now < tokenExpiry)
                {
                    return tokenCache;
                }

                // 3. 【修改】获取新的access_token（由GET改为POST）
                var httpItem = new HttpItem
                {
                    // 按照官方示例，将参数放在 URL 中
                    Url = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={apiKey}&client_secret={secretKey}",
                    Method = "POST",
                    // 设置官网示例中明确指定的两个 Header
                    ContentType = "application/json",
                    Accept = "application/json",
                    // 请求体为空，所以 PostData 保持默认或为空

                };

                // 直接调用 HttpHelper 发送请求
                string response = _httpHelper.GetHtml(httpItem).Html;


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

                    // 4. 更新StaticValue缓存
                    tokenCache = newToken;
                    tokenExpiry = newExpiry;

                    // 5. 持久化到配置文件
                    string configSection = isHighAccuracy ? "密钥_百度高精度" : "密钥_百度";
                    IniHelper.SetValue(configSection, "access_token", newToken);
                    IniHelper.SetValue(configSection, "token_expiry", newExpiry.ToString("yyyy-MM-dd HH:mm:ss"));

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
        /// 清除access_token缓存 (内存和配置文件)
        /// </summary>
        public static void ClearAccessTokenCache(bool isHighAccuracy = false)
        {
        	if (isHighAccuracy)
        	{
        		StaticValue.BaiduAccurateAccessToken = null;
        		StaticValue.BaiduAccurateAccessTokenExpiry = DateTime.MinValue;
        		IniHelper.SetValue("密钥_百度高精度", "access_token", "");
        		IniHelper.SetValue("密钥_百度高精度", "token_expiry", "");
        	}
        	else
        	{
        		StaticValue.BaiduAccessToken = null;
        		StaticValue.BaiduAccessTokenExpiry = DateTime.MinValue;
        		IniHelper.SetValue("密钥_百度", "access_token", "");
        		IniHelper.SetValue("密钥_百度", "token_expiry", "");
        	}
        }

        /// <summary>
        /// 通用文字识别（标准版）
        /// </summary>
        public static string GeneralBasic(byte[] imageBytes, string languageType = null)
        {
            try
            {
                string apiKey = StaticValue.BD_API_ID;
                string secretKey = StaticValue.BD_API_KEY;

                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey))
                {
                    return "***请在设置中输入百度密钥***";
                }
                
                // 获取access_token
                string accessToken = GetAccessToken(apiKey, secretKey, false);
                if (string.IsNullOrEmpty(accessToken))
                {
                    return "获取百度access_token失败，请检查API Key和Secret Key";
                }

                // 如果没有指定语言，使用 StaticValue 中的设置
                if (string.IsNullOrEmpty(languageType))
                {
                    languageType = StaticValue.BD_LANGUAGE ?? "CHN_ENG";
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
        public static string AccurateBasic(byte[] imageBytes, string languageType = null)
        {
            try
            {
                string apiKey = StaticValue.BD_ACCURATE_API_ID;
                string secretKey = StaticValue.BD_ACCURATE_API_KEY;

                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey))
                {
                    return "***请在设置中输入百度高精度版密钥***";
                }
                
                // 获取access_token
                string accessToken = GetAccessToken(apiKey, secretKey, true);
                if (string.IsNullOrEmpty(accessToken))
                {
                    return "获取百度高精度access_token失败，请检查API Key和Secret Key";
                }
                
                // 如果没有指定语言，使用 StaticValue 中的设置
                if (string.IsNullOrEmpty(languageType))
                {
                    languageType = StaticValue.BD_ACCURATE_LANGUAGE ?? "CHN_ENG";
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
        /// <summary>
        /// 手写文字识别
        /// </summary>
        /// <param name="imageBytes">图像数据</param>
        /// <param name="languageType">识别语言类型, 默认为"CHN_ENG"</param>
        /// <param name="recognizeGranularity">是否定位单字符位置, "big"或"small"</param>
        /// <param name="probability">是否返回置信度, "true"或"false"</param>
        /// <param name="detectDirection">是否检测图像朝向, "true"或"false"</param>
        /// <param name="detectAlteration">是否检测涂改痕迹, "true"或"false"</param>
        /// <returns>OCR识别结果</returns>
        public static string Handwriting(byte[] imageBytes,
            string languageType = "CHN_ENG",
            string recognizeGranularity = "big",
            string probability = "false",
            string detectDirection = "false",
            string detectAlteration = "false")
        {
            try
            {
                string apiKey;
                string secretKey;
                string accessToken;
                bool useStandardKey = false; // 用于标记Token重试时应清除哪个缓存

                // 1. 判断并选择密钥
                if (!string.IsNullOrEmpty(StaticValue.BD_HANDWRITING_API_ID) && !string.IsNullOrEmpty(StaticValue.BD_HANDWRITING_API_KEY))
                {
                    apiKey = StaticValue.BD_HANDWRITING_API_ID;
                    secretKey = StaticValue.BD_HANDWRITING_API_KEY;
                    accessToken = GetFreshAccessToken(apiKey, secretKey);
                }
                else
                {
                    apiKey = StaticValue.BD_API_ID;
                    secretKey = StaticValue.BD_API_KEY;
                    useStandardKey = true; // 标记我们正在使用标准版密钥
                    accessToken = GetAccessToken(apiKey, secretKey, false); // 【修正】调用带缓存的 GetAccessToken
                }

                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey))
                {
                    return "***请在设置中输入百度手写或标准版密钥***";
                }

           
                if (string.IsNullOrEmpty(accessToken))
                {
                    return "获取百度access_token失败，请检查密钥";
                }

                // 3. 构建请求
                string url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/handwriting?access_token={accessToken}";
                string imageBase64 = Convert.ToBase64String(imageBytes);

                // 4. 【核心修正】动态构建POST数据
                var postDataBuilder = new Dictionary<string, string>
                {
                    { "image", imageBase64 }
                };

                if (!string.IsNullOrEmpty(languageType)) postDataBuilder.Add("language_type", languageType);
                if (recognizeGranularity == "small") postDataBuilder.Add("recognize_granularity", "small");
                if (probability == "true") postDataBuilder.Add("probability", "true");
                if (detectDirection == "true") postDataBuilder.Add("detect_direction", "true");
                if (detectAlteration == "true") postDataBuilder.Add("detect_alteration", "true");

                string postData = string.Join("&", postDataBuilder.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));

                // 5. 发送请求
                string response = CommonHelper.PostStrData(url, postData);
                if (string.IsNullOrEmpty(response))
                {
                    return "百度手写识别请求失败";
                }

                // 6. 解析响应
                JObject json = JObject.Parse(response);

                // 7. 【新增】Token失效重试机制
                if (json["error_code"] != null)
                {
                    string errorCode = json["error_code"].ToString();
                    string errorMsg = json["error_msg"]?.ToString() ?? "未知错误";

                    // 如果是token失效，且我们回退到了标准版密钥，则尝试清除标准版token并重试
                    if ((errorCode == "110" || errorCode == "111") && useStandardKey)
                    {
                        ClearAccessTokenCache(false); // 清除标准版token
                        accessToken = GetAccessToken(apiKey, secretKey, false); // 重新获取标准版token（这次会走缓存逻辑）
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/handwriting?access_token={accessToken}";
                            response = CommonHelper.PostStrData(url, postData);
                            if (!string.IsNullOrEmpty(response))
                            {
                                json = JObject.Parse(response);
                                // 如果重试成功（没有error_code了），就跳转到结果处理
                                if (json["error_code"] == null)
                                {
                                    goto ProcessResult;
                                }
                            }
                        }
                    }

                    return $"百度手写识别错误 {errorCode}: {errorMsg}";
                }

                ProcessResult:
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
                return $"百度手写识别异常: {ex.Message}";
            }
        }
        /// <summary>
        /// 表格识别
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <param name="returnExcel">是否返回Excel文件（base64编码）</param>
        /// <param name="cellContents">是否输出单元格文字位置信息</param>
        /// <returns>识别结果</returns>
        public static string TableRecognition(byte[] imageBytes, out DataTable tableResult, out List<string> headerTexts, out List<string> footerTexts, out List<CellInfo> bodyCells,bool returnExcel = false, bool cellContents = false)
        {
             // 初始化所有 out 参数
            tableResult = null;
            headerTexts = new List<string>();
            footerTexts = new List<string>();
            bodyCells = new List<CellInfo>();
            try
            {
                string apiKey, secretKey, accessToken;
                bool useCustomKey = false;

                // 检查是否有自定义表格识别密钥
                if (!string.IsNullOrEmpty(StaticValue.BD_TABLE_API_ID) && !string.IsNullOrEmpty(StaticValue.BD_TABLE_API_KEY))
                {
                    // 使用自定义密钥，每次获取新的access_token
                    apiKey = StaticValue.BD_TABLE_API_ID;
                    secretKey = StaticValue.BD_TABLE_API_KEY;
                    useCustomKey = true;

                    // 直接获取新的access_token，不使用缓存
                    accessToken = GetFreshAccessToken(apiKey, secretKey);
                }
                else
                {
                    // 使用标准版密钥和缓存的access_token
                    apiKey = StaticValue.BD_API_ID;
                    secretKey = StaticValue.BD_API_KEY;

                    if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey))
                    {
                        return "***请在设置中输入百度标准版密钥或表格识别专用密钥***";
                    }

                    // 使用缓存的access_token
                    accessToken = GetAccessToken(apiKey, secretKey, false);
                }

                if (string.IsNullOrEmpty(accessToken))
                {
                    return useCustomKey ? "获取百度表格识别access_token失败，请检查表格识别专用密钥" : "获取百度access_token失败，请检查API Key和Secret Key";
                }

                // 构建请求
                string url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/table?access_token={accessToken}";
                string imageBase64 = Convert.ToBase64String(imageBytes);

                // 构建POST数据
                StringBuilder postDataBuilder = new StringBuilder();
                postDataBuilder.Append($"image={HttpUtility.UrlEncode(imageBase64)}");

                if (returnExcel)
                {
                    postDataBuilder.Append("&return_excel=true");
                }

                if (cellContents)
                {
                    postDataBuilder.Append("&cell_contents=true");
                }

                string postData = postDataBuilder.ToString();

                // 发送请求
                string response = CommonHelper.PostStrData(url, postData);
                if (string.IsNullOrEmpty(response))
                {
                    return "百度表格识别请求失败";
                }

                // 解析响应
                JObject json = JObject.Parse(response);

                // 检查是否有错误
                if (json["error_code"] != null)
                {
                    string errorCode = json["error_code"].ToString();
                    string errorMsg = json["error_msg"]?.ToString() ?? "未知错误";

                    // 如果是token失效且使用标准版缓存，清除缓存并重试一次
                    if ((errorCode == "110" || errorCode == "111") && !useCustomKey)
                    {
                        ClearAccessTokenCache(false);
                        accessToken = GetAccessToken(apiKey, secretKey, false);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/table?access_token={accessToken}";
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

                    return $"百度表格识别错误 {errorCode}: {errorMsg}";
                }

            ProcessResult:
                // 处理识别结果
                return ProcessTableResult(json, returnExcel, out tableResult, out headerTexts, out footerTexts,out bodyCells);
            }
            catch (Exception ex)
            {
                return $"百度表格识别异常: {ex.Message}";
            }
        }
        
        /// <summary>
        /// 处理表格识别结果 (新版本)
        /// </summary>
        private static string ProcessTableResult(JObject json, bool returnExcel, out DataTable tableResult,out List<string> headerTexts, out List<string> footerTexts,out List<CellInfo> bodyCells)
        {
            // 初始化
            tableResult = new DataTable();
            headerTexts = new List<string>();
            footerTexts = new List<string>();
            bodyCells = new List<CellInfo>(); // 初始化
            try
            {
                if (returnExcel && json["excel_file"] != null)
                {
                    string excelBase64 = json["excel_file"].ToString();
                    return $"Excel文件(Base64): {excelBase64}";
                }

                var tablesResult = json["tables_result"] as JArray;
                if (tablesResult == null || tablesResult.Count == 0)
                {
                    return "***该区域未发现表格***";
                }

                var firstTable = tablesResult.FirstOrDefault();
                if (firstTable == null) return "***该区域未发现表格***";

                var body = firstTable["body"] as JArray;
                var header = firstTable["header"] as JArray;

                var footer = firstTable["footer"] as JArray;
                // --- 新增：解析 Header 和 Footer 文本 ---
                if (header != null)
                {
                    foreach (var item in header)
                    {
                        headerTexts.Add(item["words"]?.ToString() ?? "");
                    }
                }
                if (footer != null)
                {
                    foreach (var item in footer)
                    {
                        footerTexts.Add(item["words"]?.ToString() ?? "");
                    }
                }
                 // --- 新增：解析 body 数据到 bodyCells 列表 ---
                if (body != null)
                {
                    foreach (var cell in body)
                    {
                        bodyCells.Add(new CellInfo
                        {
                            RowStart = cell["row_start"]?.ToObject<int>() ?? 0,
                            RowEnd = cell["row_end"]?.ToObject<int>() ?? 0,
                            ColStart = cell["col_start"]?.ToObject<int>() ?? 0,
                            ColEnd = cell["col_end"]?.ToObject<int>() ?? 0,
                            Words = cell["words"]?.ToString() ?? ""
                        });
                    }
                }

                Debug.WriteLine($"1111一共{bodyCells.Count}个单元格");

                // --- DataTable 构建逻辑现在可以简化或移除，因为我们主要依赖 bodyCells ---
                // (为了兼容性，可以暂时保留)
                // --- 开始构建 DataTable ---
                if (body != null && body.Count > 0)
                {
                    int maxCol = 0;
                    foreach (var cell in body)
                    {
                        int colEnd = cell["col_end"]?.ToObject<int>() ?? 0;
                        maxCol = Math.Max(maxCol, colEnd);
                    }

                    for (int i = 0; i < maxCol; i++)
                    {
                        tableResult.Columns.Add($"列 {i + 1}");
                    }

                    // 使用一个字典来构建每一行，键是行号
                    var rowsDict = new Dictionary<int, string[]>();

                    foreach (var cell in body)
                    {
                        int rowStart = cell["row_start"]?.ToObject<int>() ?? 0;
                        int colStart = cell["col_start"]?.ToObject<int>() ?? 0;
                        string words = cell["words"]?.ToString() ?? "";

                        if (!rowsDict.ContainsKey(rowStart))
                        {
                            rowsDict[rowStart] = new string[maxCol];
                        }
                        if (colStart < maxCol)
                        {
                            rowsDict[rowStart][colStart] = words;
                        }
                    }

                    // 按行号排序并添加到 DataTable
                    foreach (var rowPair in rowsDict.OrderBy(p => p.Key))
                    {
                        tableResult.Rows.Add(rowPair.Value);
                    }
                }
                // --- 结束构建 DataTable ---

                string completeHtml = GenerateCompleteHtmlTable(header, body, firstTable["footer"] as JArray);
                return completeHtml;
            }
            catch (Exception ex)
            {
                return $"处理表格识别结果时发生异常: {ex.Message}";
            }
        }

        /// <summary>
        /// 获取新的access_token（不使用缓存）
        /// </summary>
        private static string GetFreshAccessToken(string apiKey, string secretKey)
        {
            try
            {
                // 【修改】由GET请求改为POST请求
                var httpItem = new HttpItem
                {
                    Url = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={apiKey}&client_secret={secretKey}",
                    Method = "POST",
                    ContentType = "application/json",
                    Accept = "application/json",

                };

                string response = _httpHelper.GetHtml(httpItem).Html;

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                JObject json = JObject.Parse(response);
                if (json["access_token"] != null)
                {
                    return json["access_token"].ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取新的百度access_token失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 处理表格识别结果
        /// </summary>
        private static string ProcessTableResult(JObject json, bool returnExcel)
        {
            try
            {
                // 如果请求返回Excel文件
                if (returnExcel && json["excel_file"] != null)
                {
                    string excelBase64 = json["excel_file"].ToString();
                    return $"Excel文件(Base64): {excelBase64}";
                }

                // 处理表格数据
                var tablesResult = json["tables_result"] as JArray;
                if (tablesResult == null || tablesResult.Count == 0)
                {
                    return "***该区域未发现表格***";
                }

                StringBuilder result = new StringBuilder();
                int tableIndex = 1;

                foreach (var table in tablesResult)
                {
                    if (tablesResult.Count > 1)
                    {
                        result.AppendLine($"=== 表格 {tableIndex} ===");
                    }

                    // 生成完整的HTML表格（包含header、body、footer）
                    string completeHtmlTable = ProcessCompleteTable(table);
                    result.AppendLine(completeHtmlTable);

                    tableIndex++;
                }

                return result.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"处理表格识别结果时发生异常: {ex.Message}";
            }
        }

        /// <summary>
        /// 处理完整表格（包含header、body、footer），生成Excel兼容的HTML格式
        /// </summary>
        private static string ProcessCompleteTable(JToken table)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                
                // 处理表头
                var header = table["header"] as JArray;
                // if (header != null && header.Count > 0)
                // {
                //     result.AppendLine("【表头】");
                //     foreach (var headerItem in header)
                //     {
                //         if (headerItem["words"] != null)
                //         {
                //             result.AppendLine($"  {headerItem["words"]}");
                //         }
                //     }
                //     result.AppendLine();
                // }

                // 处理表格主体
                var body = table["body"] as JArray;
                if (body != null && body.Count > 0)
                {
                    // result.AppendLine("【表格内容】");
                    
                    // 使用改进的表格处理方法
                    string tableContent = ProcessTableBodyWithSpan(body);
                    // result.AppendLine(tableContent);
                    // result.AppendLine();
                }

                // 处理表尾
                var footer = table["footer"] as JArray;
                // if (footer != null && footer.Count > 0)
                // {
                //     result.AppendLine("【表尾】");
                //     foreach (var footerItem in footer)
                //     {
                //         if (footerItem["words"] != null)
                //         {
                //             result.AppendLine($"  {footerItem["words"]}");
                //         }
                //     }
                //     result.AppendLine();
                // }

                // 生成完整的HTML表格（包含所有部分）
                // result.AppendLine("完整HTML表格（可直接粘贴到Excel）：");
                string completeHtml = GenerateCompleteHtmlTable(header, body, footer);
                result.AppendLine(completeHtml);

                return result.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"处理完整表格时发生异常: {ex.Message}";
            }
        }

        /// <summary>
        /// 生成包含header、body、footer的完整HTML表格
        /// </summary>
        private static string GenerateCompleteHtmlTable(JArray header, JArray body, JArray footer)
        {
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.AppendLine("<table border='1' style='border-collapse: collapse;'>");

            // 首先确定表格的列数
            int maxCol = 0;
            if (body != null && body.Count > 0)
            {
                foreach (var cell in body)
                {
                    int colEnd = cell["col_end"]?.ToObject<int>() ?? 0;
                    maxCol = Math.Max(maxCol, colEnd);
                }
            }

            // 添加表头
            if (header != null && header.Count > 0)
            {
                htmlTable.AppendLine("  <thead>");
                htmlTable.AppendLine("    <tr>");
                
                if (header.Count == 1)
                {
                    // 如果只有一个表头项，跨越所有列
                    string headerText = System.Web.HttpUtility.HtmlEncode(header[0]["words"]?.ToString() ?? "");
                    htmlTable.AppendLine($"      <th colspan='{maxCol}' style='background-color: #f0f0f0; font-weight: bold; text-align: center;'>{headerText}</th>");
                }
                else
                {
                    // 如果有多个表头项，按列分布
                    foreach (var headerItem in header)
                    {
                        if (headerItem["words"] != null)
                        {
                            string headerText = System.Web.HttpUtility.HtmlEncode(headerItem["words"].ToString());
                            htmlTable.AppendLine($"      <th style='background-color: #f0f0f0; font-weight: bold;'>{headerText}</th>");
                        }
                    }
                }
                
                htmlTable.AppendLine("    </tr>");
                htmlTable.AppendLine("  </thead>");
            }

            // 添加表格主体
            if (body != null && body.Count > 0)
            {
                htmlTable.AppendLine("  <tbody>");
                
                // 解析body数据并生成表格行
                var cells = new List<CellInfo>();
                int maxRow = 0;

                foreach (var cell in body)
                {
                    var cellInfo = new CellInfo
                    {
                        RowStart = cell["row_start"]?.ToObject<int>() ?? 0,
                        RowEnd = cell["row_end"]?.ToObject<int>() ?? 0,
                        ColStart = cell["col_start"]?.ToObject<int>() ?? 0,
                        ColEnd = cell["col_end"]?.ToObject<int>() ?? 0,
                        Words = cell["words"]?.ToString() ?? ""
                    };

                    cells.Add(cellInfo);
                    maxRow = Math.Max(maxRow, cellInfo.RowEnd);
                }
                Debug.WriteLine($"2222一共{cells.Count}个单元格");

                // 创建表格矩阵来跟踪已处理的单元格
                var processedCells = new bool[maxRow, maxCol];
                
                for (int row = 0; row < maxRow; row++)
                {
                    htmlTable.AppendLine("    <tr>");
                    
                    for (int col = 0; col < maxCol; col++)
                    {
                        // 如果这个位置已经被跨度单元格占用，跳过
                        if (processedCells[row, col])
                            continue;
                            
                        // 查找当前位置的单元格信息
                        var cellInfo = cells.FirstOrDefault(c => c.RowStart == row && c.ColStart == col);
                        
                        if (cellInfo != null)
                        {
                            // 计算跨度
                            int rowSpan = cellInfo.RowEnd - cellInfo.RowStart;
                            int colSpan = cellInfo.ColEnd - cellInfo.ColStart;
                            
                            // 标记所有被这个单元格占用的位置
                            for (int r = cellInfo.RowStart; r < cellInfo.RowEnd; r++)
                            {
                                for (int c = cellInfo.ColStart; c < cellInfo.ColEnd; c++)
                                {
                                    processedCells[r, c] = true;
                                }
                            }
                            
                            // 生成单元格HTML
                            string cellContent = cellInfo.Words;
                            string encodedContent = System.Web.HttpUtility.HtmlEncode(cellContent);

                            // ↓↓↓ 核心修正：在这里判断内容是否为空 ↓↓↓
                            if (string.IsNullOrEmpty(encodedContent))
                            {
                                
                                encodedContent = "";
                            }
                            else
                            {
                                // 如果有内容，才处理换行符
                                encodedContent = encodedContent.Replace("\n", "&#10;");
                            }
                            // ↑↑↑ 核心修正结束 ↑↑↑
                            string cellHtml = "      <td";
                            if (rowSpan > 1) cellHtml += $" rowspan='{rowSpan}'";
                            if (colSpan > 1) cellHtml += $" colspan='{colSpan}'";
                            // cellHtml += $">{System.Web.HttpUtility.HtmlEncode(cellContent).Replace("\n", "&#10;")}</td>";
                            cellHtml += $">{encodedContent}</td>";
                            
                            htmlTable.AppendLine(cellHtml);
                        }
                        else
                        {
                            // 空单元格
                            htmlTable.AppendLine("      <td></td>");
                            processedCells[row, col] = true;
                        }
                    }
                    
                    htmlTable.AppendLine("    </tr>");
                }
                
                htmlTable.AppendLine("  </tbody>");
            }

            // 添加表尾
            if (footer != null && footer.Count > 0)
            {
                htmlTable.AppendLine("  <tfoot>");
                htmlTable.AppendLine("    <tr>");
                
                if (footer.Count == 1)
                {
                    // 如果只有一个表尾项，跨越所有列
                    string footerText = System.Web.HttpUtility.HtmlEncode(footer[0]["words"]?.ToString() ?? "");
                    htmlTable.AppendLine($"      <td colspan='{maxCol}' style='background-color: #f9f9f9; font-style: italic; text-align: center;'>{footerText}</td>");
                }
                else
                {
                    // 如果有多个表尾项，按列分布
                    foreach (var footerItem in footer)
                    {
                        if (footerItem["words"] != null)
                        {
                            string footerText = System.Web.HttpUtility.HtmlEncode(footerItem["words"].ToString());
                            htmlTable.AppendLine($"      <td style='background-color: #f9f9f9; font-style: italic;'>{footerText}</td>");
                        }
                    }
                }
                
                htmlTable.AppendLine("    </tr>");
                htmlTable.AppendLine("  </tfoot>");
            }

            htmlTable.AppendLine("</table>");
            return htmlTable.ToString();
        }

        /// <summary>
        /// 处理表格主体，支持单元格跨度，生成Excel兼容的HTML格式
        /// </summary>
        private static string ProcessTableBodyWithSpan(JArray body)
        {
            try
            {
                // 定义单元格信息类
                var cells = new List<CellInfo>();
                int maxRow = 0, maxCol = 0;

                // 解析所有单元格信息
                foreach (var cell in body)
                {
                    var cellInfo = new CellInfo
                    {
                        RowStart = cell["row_start"]?.ToObject<int>() ?? 0,
                        RowEnd = cell["row_end"]?.ToObject<int>() ?? 0,
                        ColStart = cell["col_start"]?.ToObject<int>() ?? 0,
                        ColEnd = cell["col_end"]?.ToObject<int>() ?? 0,
                        Words = cell["words"]?.ToString() ?? ""
                    };

                    cells.Add(cellInfo);
                    maxRow = Math.Max(maxRow, cellInfo.RowEnd);
                    maxCol = Math.Max(maxCol, cellInfo.ColEnd);
                }

                // 生成HTML表格格式（Excel兼容）
                StringBuilder htmlTable = new StringBuilder();
                htmlTable.AppendLine("<table border='1' style='border-collapse: collapse;'>");

                Debug.WriteLine($"3333一共{cells.Count}个单元格");

                // 创建表格矩阵来跟踪已处理的单元格
                var processedCells = new bool[maxRow, maxCol];
                
                for (int row = 0; row < maxRow; row++)
                {
                    htmlTable.AppendLine("  <tr>");
                    
                    for (int col = 0; col < maxCol; col++)
                    {
                        // 如果这个位置已经被跨度单元格占用，跳过
                        if (processedCells[row, col])
                            continue;
                            
                        // 查找当前位置的单元格信息
                        var cellInfo = cells.FirstOrDefault(c => c.RowStart == row && c.ColStart == col);
                        
                        if (cellInfo != null)
                        {
                            // 计算跨度：end是下一个位置的索引，所以跨度 = end - start
                            int rowSpan = cellInfo.RowEnd - cellInfo.RowStart;
                            int colSpan = cellInfo.ColEnd - cellInfo.ColStart;
                            
                            // 标记所有被这个单元格占用的位置
                            for (int r = cellInfo.RowStart; r < cellInfo.RowEnd; r++)
                            {
                                for (int c = cellInfo.ColStart; c < cellInfo.ColEnd; c++)
                                {
                                    processedCells[r, c] = true;
                                }
                            }
                            
                            // 生成单元格HTML，保留换行符
                            string cellContent = cellInfo.Words;
                            string encodedContent = System.Web.HttpUtility.HtmlEncode(cellContent);

                            // ↓↓↓ 核心修正：在这里判断内容是否为空 ↓↓↓
                            if (string.IsNullOrEmpty(encodedContent))
                            {
                                
                                encodedContent = "";
                            }
                            else
                            {
                                // 如果有内容，才处理换行符
                                encodedContent = encodedContent.Replace("\n", "&#10;");
                            }
                            // ↑↑↑ 核心修正结束 ↑↑↑
                            string cellHtml = "    <td";
                            if (rowSpan > 1) cellHtml += $" rowspan='{rowSpan}'";
                            if (colSpan > 1) cellHtml += $" colspan='{colSpan}'";
                            // cellHtml += $">{System.Web.HttpUtility.HtmlEncode(cellContent)}</td>";
                            cellHtml += $">{encodedContent}</td>";

                            
                            htmlTable.AppendLine(cellHtml);
                        }
                        else
                        {
                            // 空单元格
                            htmlTable.AppendLine("    <td></td>");
                            processedCells[row, col] = true;
                        }
                    }
                    
                    htmlTable.AppendLine("  </tr>");
                }
                
                htmlTable.AppendLine("</table>");

                // 构建最终结果
                StringBuilder result = new StringBuilder();
                
                // 添加跨度信息说明
                // result.AppendLine("单元格跨度信息：");
                foreach (var cellInfo in cells.Where(c => !string.IsNullOrWhiteSpace(c.Words)))
                {
                    int rowSpan = cellInfo.RowEnd - cellInfo.RowStart;
                    int colSpan = cellInfo.ColEnd - cellInfo.ColStart;
                    
                    if (rowSpan > 1 || colSpan > 1)
                    {
                        // 将多行文本转换为单行显示，用空格替换换行符
                        string displayText = cellInfo.Words.Replace("\n", " ");
                        string spanInfo = $"  \"{displayText}\" 位置: ({cellInfo.RowStart + 1},{GetColumnName(cellInfo.ColStart)})";
                        if (rowSpan > 1 && colSpan > 1)
                        {
                            spanInfo += $" 跨度: {rowSpan}行×{colSpan}列";
                        }
                        else if (rowSpan > 1)
                        {
                            spanInfo += $" 跨度: {rowSpan}行";
                        }
                        else if (colSpan > 1)
                        {
                            spanInfo += $" 跨度: {colSpan}列";
                        }
                        // result.AppendLine(spanInfo);
                    }
                }
                // result.AppendLine();
                
                // 添加HTML表格
                // result.AppendLine("Excel兼容表格（可直接粘贴到Excel）：");
                // result.AppendLine(htmlTable.ToString());
                
                // 添加制表符分隔的数据（作为备选方案）
                // result.AppendLine();
                // result.AppendLine("制表符分隔数据：");
                
                // 重新创建矩阵用于制表符输出
                var tableMatrix = new string[maxRow, maxCol];
                foreach (var cellInfo in cells)
                {
                    tableMatrix[cellInfo.RowStart, cellInfo.ColStart] = cellInfo.Words;
                }
                
                for (int row = 0; row < maxRow; row++)
                {
                    var rowData = new List<string>();
                    for (int col = 0; col < maxCol; col++)
                    {
                        string cellValue = tableMatrix[row, col] ?? "";
                        rowData.Add(cellValue);
                    }
                    
                    // 只输出有内容的行
                    bool hasContent = rowData.Any(cell => !string.IsNullOrWhiteSpace(cell));
                    if (hasContent)
                    {
                        result.AppendLine(string.Join("\t", rowData));
                    }
                }

                return result.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"处理表格跨度时发生异常: {ex.Message}";
            }
        }

        /// <summary>
        /// 将列索引转换为Excel风格的列名 (0->A, 1->B, ...)
        /// </summary>
        private static string GetColumnName(int columnIndex)
        {
            string columnName = "";
            while (columnIndex >= 0)
            {
                columnName = (char)('A' + (columnIndex % 26)) + columnName;
                columnIndex = columnIndex / 26 - 1;
            }
            return columnName;
        }

        /// <summary>
        /// 单元格信息类
        /// </summary>
        public class CellInfo
        {
            public int RowStart { get; set; }
            public int RowEnd { get; set; }
            public int ColStart { get; set; }
            public int ColEnd { get; set; }
            public string Words { get; set; }
        }
    }
}