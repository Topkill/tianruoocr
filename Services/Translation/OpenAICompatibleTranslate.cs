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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrOCR.Helper.Models;

namespace TrOCR.Helper
{
    internal class OpenAICompatibleTranslate
    {

        /// <summary>
        /// 执行 AI 翻译 (无状态，直接使用传入的配置)
        /// </summary>
        /// <param name="inputContent">待翻译的文本</param>
        /// <param name="apiUrl">API 地址</param>
        /// <param name="apiKey">API 密钥</param>
        /// <param name="modelName">模型名称</param>
        /// <param name="mode">模式配置对象 (含Prompt, Temperature)</param>
        /// <returns>翻译结果</returns>
        public static string Translate(
            string inputContent, 
            string apiUrl,
            string apiKey,
            string modelName,
            AIMode mode,
            Action<string> onTokenUpdate = null)
        {
            // 1. 基础校验 (由于上层做了兜底，这里只需简单防御)
            //检查_currentCustomTransProvider.也行
            if (string.IsNullOrEmpty(inputContent)) return "";
            if (string.IsNullOrEmpty(apiUrl)) return "错误：翻译API 地址未配置";
            if (string.IsNullOrEmpty(apiKey)) return "错误：翻译API Key 未配置";
            if (string.IsNullOrEmpty(modelName)) return "错误：翻译模型未配置";
            if (mode == null) return "错误：翻译的模式配置为空";
            // 可选：加上这一行安全检查 
            // 如果model漏了PromptOrder，添加一个PromptOrder，如果PromptOrder里count为0，增加一个保底顺序
            mode.EnsureDefaultOrder();

            try
            {
                string systemPrompt = mode.system_prompt;
                string userPrompt = mode.prompt;
                string assistantPrompt = mode.assistant_prompt;
                // 3. 构造 Messages (基于 PromptOrder 顺序动态组装)
                var messages = new List<object>();
                bool hasUserMessage = false;

                foreach (var key in mode.PromptOrder)
                {
                    switch (key)
                    {
                        case "system_prompt":
                            if (!string.IsNullOrEmpty(systemPrompt))
                            {
                                messages.Add(new { role = "system", content = systemPrompt });
                            }
                            break;

                        case "assistant_prompt":
                            if (!string.IsNullOrEmpty(assistantPrompt))
                            {
                                messages.Add(new { role = "assistant", content = assistantPrompt });
                            }
                            break;

                        case "prompt": // 对应 User 角色
                            hasUserMessage = true;

                            // 拼接逻辑：如果有 prompt 模板，则拼接；否则直接发原文
                            string finalUserContent;
                            if (!string.IsNullOrEmpty(userPrompt))
                            {
                                // === 新增逻辑：检查是否包含 ${text} 占位符 ===
                                if (userPrompt.Contains("${text}"))
                                {
                                    // 场景 A: 提示词中明确指定了 ${text} 的位置
                                    // Replace 会将占位符替换为原文，并保留占位符前后的所有文本（包括换行符）
                                    finalUserContent = userPrompt.Replace("${text}", inputContent);
                                }
                                else
                                {
                                    // 场景 B: 提示词中没有占位符（旧逻辑）
                                    // 默认将原文拼接在提示词的最后，并加上两个换行
                                    finalUserContent = userPrompt + "\n\n" + inputContent;
                                }
                            }
                            else
                            {
                                finalUserContent = inputContent;
                            }

                            messages.Add(new { role = "user", content = finalUserContent });
                            break;
                    }
                }

                // 4.  最终兜底检查 
                // 如果用户 JSON 里没写 "prompt"，我们必须把 inputContent 发出去
                if (!hasUserMessage)
                {
                    messages.Add(new { role = "user", content = inputContent });
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
                string jsonPostData = JsonConvert.SerializeObject(requestBody, jsonSettings);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonPostData);

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
                                        onTokenUpdate?.Invoke(content); // <--- 实时通知 UI
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
    }
}