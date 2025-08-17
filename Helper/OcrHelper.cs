using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using WeChatOcr;
using System.Collections.Generic;
using System.Linq;

namespace TrOCR.Helper
{
    public class OcrHelper
    {
        private static ImageOcr ocr;
        
        // 白描登录token缓存
        private static string baimiaoTokenCache = null;
        private static string baimiaoUsernameCache = null;
        private static string baimiaoPasswordCache = null;
        private static DateTime baimiaoTokenExpiry = DateTime.MinValue;

        public static void Dispose()
        {
            if (ocr != null)
            {
                ocr.Dispose();
                ocr = null;
            }
        }


        public static string SgOcr(Image img)
        {
            const string boundary = "------WebKitFormBoundary8orYTmcj8BHvQpVU";
            const string url = "http://ocr.shouji.sogou.com/v2/ocr/json";
            var header = boundary + "\r\nContent-Disposition: form-data; name=\"pic\"; filename=\"pic.jpg\"\r\nContent-Type: image/jpeg\r\n\r\n";
            const string footer = "\r\n" + boundary + "--\r\n";
            var data = FmMain.MergeByte(Encoding.ASCII.GetBytes(header), ImgToBytes(img), Encoding.ASCII.GetBytes(footer));
            return CommonHelper.PostMultiData(url, data, boundary.Substring(2));
        }

        public static string SgBasicOpenOcr(Image image)
        {
            var url = "https://deepi.sogou.com/api/sogouService";
            var referer = "https://deepi.sogou.com/?from=picsearch&tdsourcetag=s_pctim_aiomsg";
            var imageData = Convert.ToBase64String(ImgToBytes(image));
            var t = CommonHelper.GetTimeSpan(true);
            var sign = CommonHelper.Md5($"sogou_ocr_just_for_deepibasicOpenOcr{t}{imageData.Substring(0, Math.Min(1024, imageData.Length))}7f42cedccd1b3917c87aeb59e08b40ad");
            var data =
                $"image={HttpUtility.UrlEncode(imageData).Replace("+", "%2B")}&lang=zh-Chs&pid=sogou_ocr_just_for_deepi&salt={t}&service=basicOpenOcr&sign={sign}";
            // return CommonHelper.PostStrData(url, data, "", referer);
            return SgOcr(image);
        }

        public static byte[] ImgToBytes(Image img)
        {
            byte[] result;
            try
            {
                var memoryStream = new MemoryStream();
                img.Save(memoryStream, ImageFormat.Jpeg);
                var array = new byte[memoryStream.Length];
                memoryStream.Position = 0L;
                memoryStream.Read(array, 0, (int)memoryStream.Length);
                memoryStream.Close();
                result = array;
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static async Task<string> WeChat(byte[] imageBytes)
        {
            var tcs = new TaskCompletionSource<string>();
            try
            {
                if (ocr == null)
                {
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wco_data");
                    ocr = new ImageOcr(path);
                }
                ocr.Run(imageBytes, (path, result) =>
                {
                    try
                    {
                        if (result == null || result.OcrResult == null || result.OcrResult.SingleResult == null || result.OcrResult.SingleResult.Count == 0)
                        {
                            tcs.TrySetResult("***该区域未发现文本***");
                            return;
                        }
                        var list = result.OcrResult.SingleResult;
                        var sb = new StringBuilder();
                        var items = new System.Collections.Generic.List<dynamic>();

                        foreach (var item in list)
                        {
                            if (item == null || string.IsNullOrEmpty(item.SingleStrUtf8)) continue;
                            items.Add(new { Text = item.SingleStrUtf8, Left = item.Left, Top = item.Top, Right = item.Right, Bottom = item.Bottom });
                        }

                        if (items.Count > 0)
                        {
                            items.Sort((a, b) => a.Top.CompareTo(b.Top));
                            var groupedLines = new System.Collections.Generic.List<System.Collections.Generic.List<dynamic>>();
                            if (items.Count > 0)
                            {
                                var currentLine = new System.Collections.Generic.List<dynamic> { items[0] };
                                groupedLines.Add(currentLine);

                                for (int i = 1; i < items.Count; i++)
                                {
                                    var item = items[i];
                                    var lastItem = items[i - 1];
                                    float itemCenterY = (item.Top + item.Bottom) / 2;
                                    float lastItemCenterY = (lastItem.Top + lastItem.Bottom) / 2;
                                    float avgHeight = ((item.Bottom - item.Top) + (lastItem.Bottom - lastItem.Top)) / 2;

                                    if (System.Math.Abs(itemCenterY - lastItemCenterY) < avgHeight / 2)
                                    {
                                        currentLine.Add(item);
                                    }
                                    else
                                    {
                                        currentLine = new System.Collections.Generic.List<dynamic> { item };
                                        groupedLines.Add(currentLine);
                                    }
                                }
                            }

                            foreach (var line in groupedLines)
                            {
                                line.Sort((a, b) => a.Left.CompareTo(b.Left));
                                sb.AppendLine(string.Join("   ", line.ConvertAll(item => (string)item.Text)));
                            }
                        }

                        if (sb.Length == 0)
                        {
                            tcs.TrySetResult("***该区域未发现文本***");
                            return;
                        }

                        tcs.TrySetResult(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            var finishedTask = await Task.WhenAny(tcs.Task, Task.Delay(10000));
            if (finishedTask == tcs.Task)
            {
                return await tcs.Task;
            }
            return "微信OCR识别超时(10秒)";
        }

        public static async Task<string> Baimiao(byte[] imageBytes, string username, string password)
        {
            try
            {
                const string url = "https://web.baimiaoapp.com";
                // 获取或生成固定的设备UUID
                string uuid = GetOrCreateDeviceUuid();
                string loginToken = "";

                // 检查是否有有效的缓存token
                if (IsBaimiaoTokenValid(username, password))
                {
                    loginToken = baimiaoTokenCache;
                }
                else
                {
                    // 登录获取新token
                    loginToken = await BaimiaoLogin(username, password, uuid);
                    
                    if (!string.IsNullOrEmpty(loginToken))
                    {
                        // 缓存token，有效期设为240小时
                        CacheBaimiaoToken(loginToken, username, password, 14400);  // 14400分钟 = 240小时
                    }
                }
                
                if (string.IsNullOrEmpty(loginToken))
                {
                    return "***白描登录失败，请检查账号密码***";
                }

                // 获取OCR权限
                var permResult = await BaimiaoGetPermission(url, loginToken, uuid);
                if (!permResult.Success)
                {
                    return permResult.ErrorMessage;
                }

                // 执行OCR识别
                var ocrResult = await BaimiaoPerformOCR(url, loginToken, uuid, imageBytes, permResult.Engine, permResult.Token);
                return ocrResult;
            }
            catch (Exception ex)
            {
                return $"***白描OCR识别出错: {ex.Message}***";
            }
        }

        // 检查白描token是否有效
        private static bool IsBaimiaoTokenValid(string username, string password)
        {
            // 优先检查内存缓存
            if (!string.IsNullOrEmpty(baimiaoTokenCache) &&
                baimiaoUsernameCache == username &&
                baimiaoPasswordCache == password &&
                DateTime.Now < baimiaoTokenExpiry)
            {
                return true;
            }
            
            // 从配置文件读取
            string savedToken = IniHelper.GetValue("密钥_白描", "token");
            string savedExpiry = IniHelper.GetValue("密钥_白描", "token_expiry");
            string savedUsername = IniHelper.GetValue("密钥_白描", "token_username");
            
            // 检查token是否存在
            if (string.IsNullOrEmpty(savedToken) || savedToken == "发生错误")
            {
                return false;
            }
            
            // 检查是否同一账号
            if (savedUsername != username)
            {
                return false;
            }
            
            // 检查是否过期
            if (!DateTime.TryParse(savedExpiry, out DateTime expiry))
            {
                return false;
            }
            
            if (DateTime.Now >= expiry)
            {
                return false;
            }
            
            // 恢复到内存缓存以提高性能
            baimiaoTokenCache = savedToken;
            baimiaoUsernameCache = username;
            baimiaoPasswordCache = password;
            baimiaoTokenExpiry = expiry;
            
            return true;
        }
        
        // 缓存白描token
        private static void CacheBaimiaoToken(string token, string username, string password, int expiryMinutes)
        {
            // 保存到内存
            baimiaoTokenCache = token;
            baimiaoUsernameCache = username;
            baimiaoPasswordCache = password;
            baimiaoTokenExpiry = DateTime.Now.AddMinutes(expiryMinutes);
            
            // 持久化到配置文件
            IniHelper.SetValue("密钥_白描", "token", token);
            IniHelper.SetValue("密钥_白描", "token_expiry", baimiaoTokenExpiry.ToString("yyyy-MM-dd HH:mm:ss"));
            IniHelper.SetValue("密钥_白描", "token_username", username);
        }
        
        // 清除白描token缓存
        public static void ClearBaimiaoTokenCache()
        {
            // 清除内存缓存
            baimiaoTokenCache = null;
            baimiaoUsernameCache = null;
            baimiaoPasswordCache = null;
            baimiaoTokenExpiry = DateTime.MinValue;
            
            // 清除配置文件中的token
            IniHelper.SetValue("密钥_白描", "token", "");
            IniHelper.SetValue("密钥_白描", "token_expiry", "");
            IniHelper.SetValue("密钥_白描", "token_username", "");
        }

        // 获取或创建设备UUID
        private static string GetOrCreateDeviceUuid()
        {
            // 从配置文件读取UUID
            string uuid = IniHelper.GetValue("密钥_白描", "device_uuid");
            
            // 如果没有UUID或读取失败，生成新的
            if (string.IsNullOrEmpty(uuid) || uuid == "发生错误")
            {
                uuid = Guid.NewGuid().ToString();
                // 保存到配置文件
                IniHelper.SetValue("密钥_白描", "device_uuid", uuid);
            }
            
            return uuid;
        }

        public static async Task<Dictionary<string, object>> BaimiaoVerifyAccount(string username, string password)
        {
            try
            {
                var url = "https://web.baimiaoapp.com/api/user/login";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                // 使用固定的设备UUID
                request.Headers.Add("X-Auth-Uuid", GetOrCreateDeviceUuid());
                request.Headers.Add("X-Auth-Token", "");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36";
                request.Referer = "https://web.baimiaoapp.com/";
                request.Timeout = 10000; // 设置10秒超时

                var loginData = new
                {
                    username = username,
                    password = password,
                    type = System.Text.RegularExpressions.Regex.IsMatch(username, @"^[0-9]*$") ? "mobile" : "email"
                };

                string jsonData = JsonConvert.SerializeObject(loginData);
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = data.Length;

                // 使用超时控制的异步操作
                var timeoutTask = Task.Delay(10000); // 10秒超时
                
                using (var stream = await request.GetRequestStreamAsync())
                {
                    var writeTask = stream.WriteAsync(data, 0, data.Length);
                    var completedTask = await Task.WhenAny(writeTask, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        var resultDict = new Dictionary<string, object>();
                        resultDict["code"] = 408;
                        resultDict["message"] = "请求超时，请检查网络连接";
                        return resultDict;
                    }
                    
                    await writeTask;
                }

                var responseTask = request.GetResponseAsync();
                var responseCompletedTask = await Task.WhenAny(responseTask, timeoutTask);
                
                if (responseCompletedTask == timeoutTask)
                {
                    request.Abort(); // 中止请求
                    var resultDict = new Dictionary<string, object>();
                    resultDict["code"] = 408;
                    resultDict["message"] = "响应超时，请检查网络连接";
                    return resultDict;
                }

                using (var response = (HttpWebResponse)await responseTask)
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string result = await reader.ReadToEndAsync();
                    
                    // 调试输出原始响应
                    System.Diagnostics.Debug.WriteLine($"Baimiao login response: {result}");
                    
                    var json = JObject.Parse(result);
                    
                    var resultDict = new Dictionary<string, object>();
                    
                    // 白描API响应格式：code=1表示成功，其他值表示失败
                    if (json["code"] != null)
                    {
                        int code = (int)json["code"];
                        string msg = json["msg"]?.ToString() ?? json["message"]?.ToString() ?? "";
                        
                        resultDict["code"] = code;
                        resultDict["message"] = msg;
                        
                        // code=1 表示成功
                        if (code == 1 || (code == 0 && json["data"]?["token"] != null))
                        {
                            resultDict["success"] = true;
                            
                            // 如果验证成功且有token，缓存它
                            if (json["data"]?["token"] != null)
                            {
                                string token = json["data"]["token"].ToString();
                                CacheBaimiaoToken(token, username, password, 1440);  // 1440分钟 = 24小时
                            }
                        }
                        else
                        {
                            resultDict["success"] = false;
                        }
                    }
                    else
                    {
                        // 未知响应格式
                        resultDict["code"] = -1;
                        resultDict["message"] = $"未知响应格式: {result.Substring(0, Math.Min(100, result.Length))}";
                        resultDict["success"] = false;
                    }
                    
                    return resultDict;
                }
            }
            catch (WebException ex)
            {
                // 处理HTTP错误响应
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var responseStream = errorResponse.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string errorResult = await reader.ReadToEndAsync();
                        try
                        {
                            var json = JObject.Parse(errorResult);
                            var resultDict = new Dictionary<string, object>();
                            resultDict["code"] = json["code"] != null ? (int)json["code"] : -1;
                            resultDict["message"] = json["msg"]?.ToString() ?? json["message"]?.ToString() ?? "网络错误";
                            resultDict["success"] = false;
                            return resultDict;
                        }
                        catch
                        {
                            var resultDict = new Dictionary<string, object>();
                            resultDict["code"] = -1;
                            resultDict["message"] = "网络错误";
                            resultDict["success"] = false;
                            return resultDict;
                        }
                    }
                }
                else
                {
                    var resultDict = new Dictionary<string, object>();
                    resultDict["code"] = -1;
                    resultDict["message"] = ex.Message;
                    resultDict["success"] = false;
                    return resultDict;
                }
            }
            catch (Exception ex)
            {
                var resultDict = new Dictionary<string, object>();
                resultDict["code"] = -1;
                resultDict["message"] = ex.Message;
                resultDict["success"] = false;
                return resultDict;
            }
        }

        private static async Task<string> BaimiaoLogin(string username, string password, string uuid)
        {
            try
            {
                var url = "https://web.baimiaoapp.com/api/user/login";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("X-Auth-Uuid", uuid);  // 使用传入的固定UUID
                request.Headers.Add("X-Auth-Token", "");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36";
                request.Referer = "https://web.baimiaoapp.com/";
                request.Timeout = 10000; // 设置10秒超时

                var loginData = new
                {
                    username = username,
                    password = password,
                    type = System.Text.RegularExpressions.Regex.IsMatch(username, @"^[0-9]*$") ? "mobile" : "email"
                };

                string jsonData = JsonConvert.SerializeObject(loginData);
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = data.Length;

                // 添加超时控制
                var timeoutTask = Task.Delay(10000);
                
                using (var stream = await request.GetRequestStreamAsync())
                {
                    var writeTask = stream.WriteAsync(data, 0, data.Length);
                    var completedTask = await Task.WhenAny(writeTask, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        return ""; // 超时返回空
                    }
                    
                    await writeTask;
                }

                var responseTask = request.GetResponseAsync();
                var responseCompletedTask = await Task.WhenAny(responseTask, timeoutTask);
                
                if (responseCompletedTask == timeoutTask)
                {
                    request.Abort();
                    return ""; // 超时返回空
                }

                using (var response = (HttpWebResponse)await responseTask)
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string result = await reader.ReadToEndAsync();
                    var json = JObject.Parse(result);
                    if (json["data"]?["token"] != null)
                    {
                        return json["data"]["token"].ToString();
                    }
                }
            }
            catch
            {
                // 忽略错误
            }
            return "";
        }

        private class BaimiaoPermissionResult
        {
            public bool Success { get; set; }
            public string Engine { get; set; }
            public string Token { get; set; }
            public string ErrorMessage { get; set; }
        }

        private static async Task<BaimiaoPermissionResult> BaimiaoGetPermission(string baseUrl, string loginToken, string uuid)
        {
            try
            {
                var url = baseUrl + "/api/perm/single";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("X-Auth-Uuid", uuid);
                request.Headers.Add("X-Auth-Token", loginToken);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36";
                request.Referer = "https://web.baimiaoapp.com/";

                var permData = new { mode = "single" };
                string jsonData = JsonConvert.SerializeObject(permData);
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = data.Length;

                using (var stream = await request.GetRequestStreamAsync())
                {
                    await stream.WriteAsync(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string result = await reader.ReadToEndAsync();
                    var json = JObject.Parse(result);
                    if (json["data"]?["engine"] != null)
                    {
                        return new BaimiaoPermissionResult
                        {
                            Success = true,
                            Engine = json["data"]["engine"].ToString(),
                            Token = json["data"]["token"].ToString()
                        };
                    }
                    else
                    {
                        // 可能是token过期了，清除缓存
                        ClearBaimiaoTokenCache();
                        
                        return new BaimiaoPermissionResult
                        {
                            Success = false,
                            ErrorMessage = "***已经达到今日识别上限，请前往白描手机端开通会员或明天再试***"
                        };
                    }
                }
            }
            catch (WebException ex)
            {
                // 如果是401或403错误，说明token失效，清除缓存
                if (ex.Response != null)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null && (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden))
                    {
                        ClearBaimiaoTokenCache();
                    }
                }
                
                return new BaimiaoPermissionResult
                {
                    Success = false,
                    ErrorMessage = $"***获取白描权限失败: {ex.Message}***"
                };
            }
            catch (Exception ex)
            {
                return new BaimiaoPermissionResult
                {
                    Success = false,
                    ErrorMessage = $"***获取白描权限失败: {ex.Message}***"
                };
            }
        }

        private static async Task<string> BaimiaoPerformOCR(string baseUrl, string loginToken, string uuid, byte[] imageBytes, string engine, string token)
        {
            try
            {
                string base64Image = Convert.ToBase64String(imageBytes);
                string dataUrl = $"data:image/png;base64,{base64Image}";
                
                // 计算SHA1 hash
                string hash;
                using (var sha1 = System.Security.Cryptography.SHA1.Create())
                {
                    byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(dataUrl));
                    hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }

                var url = $"{baseUrl}/api/ocr/image/{engine}";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("X-Auth-Uuid", uuid);
                request.Headers.Add("X-Auth-Token", loginToken);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36";
                request.Referer = "https://web.baimiaoapp.com/";

                var ocrData = new
                {
                    batchId = "",
                    total = 1,
                    token = token,
                    hash = hash,
                    name = "tianruo_screenshot.png",
                    size = 0,
                    dataUrl = dataUrl,
                    result = new { },
                    status = "processing",
                    isSuccess = false
                };

                string jsonData = JsonConvert.SerializeObject(ocrData);
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = data.Length;

                using (var stream = await request.GetRequestStreamAsync())
                {
                    await stream.WriteAsync(data, 0, data.Length);
                }

                string jobStatusId = "";
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string result = await reader.ReadToEndAsync();
                    var json = JObject.Parse(result);
                    if (json["data"]?["jobStatusId"] != null)
                    {
                        jobStatusId = json["data"]["jobStatusId"].ToString();
                    }
                    else
                    {
                        return "***白描OCR任务创建失败***";
                    }
                }

                // 轮询获取结果
                int maxRetries = 50; // 最多等待5秒
                for (int i = 0; i < maxRetries; i++)
                {
                    await Task.Delay(100);

                    var statusUrl = $"{baseUrl}/api/ocr/image/{engine}/status?jobStatusId={jobStatusId}";
                    var statusRequest = (HttpWebRequest)WebRequest.Create(statusUrl);
                    statusRequest.Method = "GET";
                    statusRequest.Headers.Add("X-Auth-Uuid", uuid);
                    statusRequest.Headers.Add("X-Auth-Token", loginToken);
                    statusRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36";
                    statusRequest.Referer = "https://web.baimiaoapp.com/";

                    using (var statusResponse = (HttpWebResponse)await statusRequest.GetResponseAsync())
                    using (var statusStream = statusResponse.GetResponseStream())
                    using (var statusReader = new StreamReader(statusStream, Encoding.UTF8))
                    {
                        string statusResult = await statusReader.ReadToEndAsync();
                        var statusJson = JObject.Parse(statusResult);
                        
                        if (statusJson["data"]?["isEnded"] != null && (bool)statusJson["data"]["isEnded"])
                        {
                            var ydResp = statusJson["data"]["ydResp"];
                            if (ydResp?["words_result"] != null)
                            {
                                var wordsResult = ydResp["words_result"] as JArray;
                                var textBuilder = new StringBuilder();
                                foreach (var word in wordsResult)
                                {
                                    if (word["words"] != null)
                                    {
                                        textBuilder.AppendLine(word["words"].ToString());
                                    }
                                }
                                return textBuilder.ToString().TrimEnd();
                            }
                            return "***该区域未发现文本***";
                        }
                    }
                }
                
                return "***白描OCR识别超时***";
            }
            catch (Exception ex)
            {
                return $"***白描OCR执行失败: {ex.Message}***";
            }
        }
    }
}