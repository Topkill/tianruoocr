using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics; // 用于获取 StartupPath
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrOCR.Helper.Models;

namespace TrOCR.Helper
{
    internal class OpenAICompatibleHelper
    {


        /// <summary>
        ///  OCR 接口 ，支持多接口切换和高级参数
        /// </summary>
        /// <param name="image">截图</param>
        /// <param name="apiUrl">API 地址</param>
        /// <param name="apiKey">API 密钥</param>
        /// <param name="modelName">模型名称</param>
        /// <param name="AIMode">模式</param>
        /// <returns>识别结果文本</returns>
        public static string OCR(
            Image image,
            string apiUrl,
            string apiKey,
            string modelName,
            AIMode mode,
            Action<string> onTokenUpdate = null)
        {
            // 1. 基础校验
            if (image == null) return "错误：图片为空";
            if (string.IsNullOrEmpty(apiUrl)) return "错误：API 地址未配置";
            if (string.IsNullOrEmpty(apiKey)) return "错误：API Key 未配置";
            if (string.IsNullOrEmpty(modelName)) return "错误：模型未配置";
            if (mode == null) return "错误：模式配置为空";
            // 可选：加上这一行安全检查 
            // 如果model漏了PromptOrder，添加一个PromptOrder，如果PromptOrder里count为0，增加一个保底顺序
            mode.EnsureDefaultOrder();


            try
            {
                string systemPrompt = mode.system_prompt;
                string userPrompt = mode.prompt;
                string assistantPrompt = mode.assistant_prompt;

                // 3. 图片转 Base64
                string base64Image = ImageToBase64(image);
                // 4. 构造 Messages 数组 (基于 PromptOrder 顺序动态组装)
                var messages = new List<object>();
                bool hasUserMessage = false; // 标记是否已添加用户消息

                // 遍历顺序列表 (PromptOrder 里的 key 已经是小写的了)
                foreach (var key in mode.PromptOrder)
                {
                    switch (key)
                    {
                        case "system_prompt":
                            if (!string.IsNullOrEmpty(mode.system_prompt))
                            {
                                messages.Add(new { role = "system", content = mode.system_prompt });
                            }
                            break;

                        case "assistant_prompt":
                            if (!string.IsNullOrEmpty(mode.assistant_prompt))
                            {
                                messages.Add(new { role = "assistant", content = mode.assistant_prompt });
                            }
                            break;

                        case "prompt": // 对应 API 的 User 角色
                            hasUserMessage = true;

                            // 构造 User Content (混合图文)
                            var userContent = new List<object>();

                            // A. 添加文本 (如果配置了 prompt)
                            if (!string.IsNullOrEmpty(mode.prompt))
                            {
                                userContent.Add(new { type = "text", text = mode.prompt });
                            }

                            // B.  保底图片 (必须添加) 
                            userContent.Add(new
                            {
                                type = "image_url",
                                image_url = new { url = $"data:image/jpeg;base64,{base64Image}" }
                            });

                            messages.Add(new { role = "user", content = userContent });
                            break;
                    }
                }

                // 5.  最终兜底检查 
                // 如果循环跑完了，PromptOrder 里竟然没有 "prompt" 这个键 (用户配置漏写了)，
                // 我们必须手动补发图片，否则 OCR 无法进行。
                if (!hasUserMessage)
                {
                    var fallbackContent = new List<object>();
                    fallbackContent.Add(new
                    {
                        type = "image_url",
                        image_url = new { url = $"data:image/jpeg;base64,{base64Image}" }
                    });
                    messages.Add(new { role = "user", content = fallbackContent });
                }

              
                // 5. 构造请求体 (Request Body)
                // 5.1. 直接定义包含所有字段的匿名对象 (哪怕是 null 也没关系)
                var requestBody = new
                {
                    model = modelName,
                    messages = messages,         
                    temperature = mode.temperature,
                    // 三态逻辑修改 
                    thinking = (mode.enable_thinking == true)
                    ? (object)new { type = "enabled" }        // 情况1: True -> 发送 enabled (可加 budget_tokens)
                    : (mode.enable_thinking == false)
                        ? (object)new { type = "disabled" }   // 情况2: False -> 发送 disabled
                        : null,                               // 情况3: Null  -> 发送 null (被 Json 忽略)
                    stream = mode.stream
                };

                // 序列化 JSON
                // 5.2配置序列化设置：忽略 Null 值，过滤掉所有 value 为 null 的字段
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                string jsonPostData = JsonConvert.SerializeObject(requestBody,jsonSettings);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonPostData);

                // 6. 发起 HTTP 请求
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + apiKey);
                request.ContentLength = byteArray.Length;
                request.Timeout = 60000; // 60秒超时

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // 7.  智能响应处理 
                StringBuilder sb = new StringBuilder();

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) // 强转为 HttpWebResponse 以方便获取 Headers
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    // A. 获取 Content-Type
                    string contentType = response.ContentType?.ToLower() ?? "";

                    // B. 判断是否为流式响应 (SSE)
                    bool isEventStream = contentType.Contains("text/event-stream");

                    // === 分支 1：处理流式响应 ===
                    if (isEventStream)
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (string.IsNullOrEmpty(line)) continue;
                            if (line == "data: [DONE]") break;

                            if (line.StartsWith("data: "))
                            {
                                string json = line.Substring(6);
                                try
                                {
                                    JObject obj = JObject.Parse(json);
                                    // 流式格式：choices[0].delta.content
                                    var content = obj["choices"]?[0]?["delta"]?["content"]?.ToString();
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        sb.Append(content);
                                        // 触发回调，通知 UI 线程更新
                                        onTokenUpdate?.Invoke(content);
                                    }
                                }
                                catch { /* 忽略流式解析中的单行错误 */ }
                            }
                        }
                    }
                    // === 分支 2：处理普通 JSON 响应 ===
                    else
                    {
                        // 一次性读取所有内容
                        string jsonResponse = reader.ReadToEnd();
                        try
                        {
                            JObject obj = JObject.Parse(jsonResponse);

                            // 1. 检查是否有错误信息
                            if (obj["error"] != null)
                            {
                                return $"API 报错: {obj["error"]["message"]}";
                            }

                            // 2. 普通格式：choices[0].message.content
                            // 注意：这里是 message，不是 delta
                            var content = obj["choices"]?[0]?["message"]?["content"]?.ToString();

                            if (!string.IsNullOrEmpty(content))
                            {
                                sb.Append(content);
                            }
                        }
                        catch (Exception ex)
                        {
                            // 如果解析失败，返回原始文本方便调试
                            return $"解析响应失败: {ex.Message} \n原始内容: {jsonResponse}";
                        }
                    }
                }

                return sb.ToString().Trim();
            }
            catch (WebException webEx)
            {
                // 增加对 WebException 的详细处理，读取服务器返回的错误文本
                if (webEx.Response != null)
                {
                    using (var errStream = webEx.Response.GetResponseStream())
                    using (var reader = new StreamReader(errStream))
                    {
                        return $"请求被拒绝: {reader.ReadToEnd()}";
                    }
                }
                return $"网络错误: {webEx.Message}";
            }
            catch (Exception ex)
            {
                return $"API 请求失败: {ex.Message}";
            }
        }
       

        /// <summary>
        /// 将图片转换为 Base64 字符串
        /// </summary>
        private static string ImageToBase64(Image image)
        {
            try
            {
                if (image == null) return null;
                using (MemoryStream m = new MemoryStream())
                {
                    // 统一保存为 Jpeg 格式以减小体积，或者使用 image.RawFormat
                    image.Save(m, ImageFormat.Jpeg); 
                    byte[] imageBytes = m.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch
            {
                return null;
            }
        }

        
    }

  
    
}