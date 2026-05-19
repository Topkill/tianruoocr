using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TrOCR.Helper
{
    //接口已失效
    public static class BaiduTranslator2Helper
    {

        private static readonly HttpClient httpClient;

        // 使用静态构造函数来初始化 HttpClient 和它的处理器
        static BaiduTranslator2Helper()
        {
            // 1. 创建一个 Cookie 容器
            var cookieContainer = new CookieContainer();

            // 2. 创建一个处理器，并告诉它使用我们的 Cookie 容器
            var handler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer,
                UseCookies = true, // 确保启用了 Cookie 功能
                AllowAutoRedirect = true // 允许自动重定向
            };

            // 3. 将我们的 Cookie 添加到容器中，【必须】指定是哪个域的 Cookie
            cookieContainer.SetCookies(new Uri("http://res.d.hjfile.cn"), "HJ_UID=390f25c7-c9f3-b237-f639-62bd23cd431f; HJC_USRC=uzhi; HJC_NUID=1");

            // 4. 使用配置好的处理器来创建唯一的 HttpClient 实例
            httpClient = new HttpClient(handler);
        }

        /// <summary>
        /// 调用沪江小D词典翻译接口（底层技术支持是百度）
        /// </summary>
        /// <param name="text">要翻译的文本</param>
        /// <param name="from">源语言代码</param>
        /// <param name="to">目标语言代码</param>
        /// <returns>翻译结果或错误信息</returns>
        public static async Task<string> TranslateAsync(string text, string from, string to)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            try
            {
                // 1. 构建请求URL和Body
                string url = $"http://res.d.hjfile.cn/v10/dict/translation/{from}/{to}";
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("content", text)
                });
                formContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded")
                {
                    CharSet = "UTF-8"
                };

                // 2. 创建HttpRequestMessage并设置Headers
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = formContent;

                // 添加所有必要的Headers
                request.Headers.TryAddWithoutValidation("Host", "res.d.hjfile.cn");
                request.Headers.TryAddWithoutValidation("Origin", "http://res.d.hjfile.cn");
                request.Headers.TryAddWithoutValidation("Referer", "http://res.d.hjfile.cn/app/trans");
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36");
                // 【修正】移除手动添加 Cookie 的代码，因为现在由 HttpClientHandler 自动管理
                // request.Headers.TryAddWithoutValidation("Cookie", "HJ_UID=390f25c7-c9f3-b237-f639-62bd23cd431f; HJC_USRC=uzhi; HJC_NUID=1");
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                // 3. 发送请求
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    // 4. 解析JSON结果
                    var json = JObject.Parse(jsonString);
                    var data = json["data"];

                    if (data != null && data["content"] != null)
                    {
                        return data["content"].ToString();
                    }

                    return $"[Baidu2]：API返回错误 - {jsonString.Trim()}";
                }

                return $"[Baidu2]：Http请求错误，状态码: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                return $"[Baidu2]：发生异常 - {ex.Message}";
            }
        }
    }
}