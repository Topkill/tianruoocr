using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;

namespace TrOCR
{
    /// <summary>
    /// 应用程序更新检查器
    /// 负责检查GitHub发布页面的新版本并提示用户更新
    /// </summary>
    internal static class UpdateChecker
    {
        /// <summary>
        /// 检查应用程序更新
        /// </summary>
        public static void CheckUpdate()
        {
            try
            {
                // 1. 读取配置确定是否检查预发布版
                bool checkPreRelease = false;
                try
                {
                    // 从ini文件中读取配置，如果读取失败或值为"发生错误"，则默认为false
                    string settingValue = IniHelper.GetValue("更新", "CheckPreRelease");
                    if (settingValue != "发生错误" && !string.IsNullOrEmpty(settingValue))
                    {
                        checkPreRelease = Convert.ToBoolean(settingValue);
                    }
                }
                catch
                {
                    // 如果转换失败，保持默认值 false
                }

                // 2. 根据配置选择 API URL
                string apiUrl;
                if (checkPreRelease)
                {
                    // 获取所有版本列表（包括预发布版），最新的在最前面
                    apiUrl = "https://gh-proxy.com/https://api.github.com/repos/Topkill/tianruoocr/releases";
                    Debug.WriteLine("获取所有版本列表");
                }
                else
                {
                    // 只获取最新的稳定版
                    apiUrl = "https://gh-proxy.com/https://api.github.com/repos/Topkill/tianruoocr/releases/latest";
                    Debug.WriteLine("获取最新的稳定版");
                }

                // 获取最新版本信息
                var request = WebRequest.Create(apiUrl) as HttpWebRequest;
                request.UserAgent = "TianruoOCR";  // GitHub API 要求设置 User-Agent
                request.Accept = "application/vnd.github.v3+json";
                request.Timeout = 10000;  // 10秒超时

                // --- ETag 缓存机制开始 ---

                // 2a. 根据 API URL 读取对应的 ETag
                // 我们为 "所有版本" 和 "最新稳定版" 分别存储 ETag
                string etagKey = checkPreRelease ? "ETag_Releases" : "ETag_Latest";
                string storedEtag = IniHelper.GetValue("更新", etagKey);

                // 2b. 如果存在已保存的 ETag，将其加入到请求头中
                if (!string.IsNullOrEmpty(storedEtag) && storedEtag != "发生错误")
                {
                    request.Headers.Add("If-None-Match", storedEtag);
                    Debug.WriteLine($"Found ETag: {storedEtag}. Adding If-None-Match header.");
                }

                // --- ETag 缓存机制结束 ---
                using (var response = request.GetResponse())
                {
                    // 2c. 请求成功 (200 OK), 保存新的 ETag
                    string newEtag = response.Headers["ETag"];
                    if (!string.IsNullOrEmpty(newEtag))
                    {
                        IniHelper.SetValue("更新", etagKey, newEtag);
                        Debug.WriteLine($"Request successful. Saving new ETag: {newEtag}");
                    }
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var jsonText = reader.ReadToEnd();
                        JObject releaseData;

                        // 3. 处理不同的 JSON 响应结构
                        if (checkPreRelease)
                        {
                            // API /releases 返回一个数组，取第一个元素
                            var jsonArray = JArray.Parse(jsonText);
                            if (jsonArray.Count == 0)
                            {
                                CommonHelper.ShowHelpMsg("未找到任何发行版本。");
                                return;
                            }
                            releaseData = jsonArray[0] as JObject;
                        }
                        else
                        {
                            // API /releases/latest 返回单个对象
                            releaseData = JObject.Parse(jsonText);
                        }

                        // 4. 使用解析到的 releaseData 进行后续操作
                        // 获取版本号（去掉前面的 'v'）
                        var tagName = releaseData["tag_name"].Value<string>();
                        var newVersion = tagName.TrimStart('v', 'V');
                        var curVersion = Application.ProductVersion.Split('+')[0];

                        // 5. 调用新的、健壮的版本比较方法
                        if (!CheckVersion(newVersion, curVersion))
                        {
                            CommonHelper.ShowHelpMsg("当前已是最新版本");
                            return;
                        }

                        // 判断是否为预发布版，并添加到提示信息中
                        bool isPreRelease = releaseData["prerelease"].Value<bool>();
                        string preReleaseTag = isPreRelease ? " [预览版]" : "";
                        CommonHelper.ShowHelpMsg($"有新版本：{newVersion}{preReleaseTag}");

                        // 获取下载链接
                        var htmlUrl = releaseData["html_url"].Value<string>();
                        var assets = releaseData["assets"] as JArray;
                        string downloadUrl = null;

                        // 查找 exe 或 zip 文件的下载链接
                        if (assets != null && assets.Count > 0)
                        {
                            foreach (var asset in assets)
                            {
                                var name = asset["name"].Value<string>();
                                if (name.EndsWith(".exe") || name.EndsWith(".zip") || name.EndsWith(".rar") || name.EndsWith(".7z"))
                                {
                                    downloadUrl = asset["browser_download_url"].Value<string>();
                                    break;
                                }
                            }
                        }

                        // 获取更新说明（限制长度）
                        var body = releaseData["body"]?.Value<string>() ?? "无更新说明";
                        if (body.Length > 500)
                        {
                            body = body.Substring(0, 500) + "...";
                        }

                        // 显示更新提示
                        var message = $"发现新版本：{newVersion}{preReleaseTag}\n"; // 添加预览版标记
                        message += $"当前版本：{curVersion}\n\n";
                        if (isPreRelease)
                        {
                            message += "⚠️ 请注意：这是一个预览版本，用于测试新功能，可能存在未知问题或不稳定情况。建议普通用户等待正式版。\n\n";
                        }
                        message += $"更新内容：\n{body}\n\n";
                        message += "是否前往下载页面？";

                        if (MessageBox.Show(message, "发现新版本", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            // 如果有直接下载链接，使用下载链接；否则打开发布页面
                            Process.Start(downloadUrl ?? htmlUrl);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // --- ETag 缓存机制 - 处理 304 响应 ---

                // 2d. 检查是否收到了 304 Not Modified 响应
                if (ex.Response is HttpWebResponse httpResponse && httpResponse.StatusCode == HttpStatusCode.NotModified)
                {
                    // 这是缓存命中的情况，意味着远端没有新版本。
                    // 这种响应不计入速率限制，是我们期望的结果。
                    Debug.WriteLine("ETag matched. No new release found (304 Not Modified).");
                    CommonHelper.ShowHelpMsg("当前已是最新版本"); // 或者直接 return; 静默退出
                    return;
                }

                // --- ETag 缓存机制结束 ---
                // 网络错误，静默失败或显示简单提示
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    // 超时不提示，避免影响用户体验
                    return;
                }
                CommonHelper.ShowHelpMsg("检查更新失败，请检查网络连接");
            }
            catch (Exception ex)
            {
                // 其他错误
                CommonHelper.ShowHelpMsg($"检查更新时出错：{ex.Message}");
            }
        }

        /// <summary>
        /// 比较版本号大小，支持预发布标签 (e.g., -beta, -rc.1)
        /// </summary>
        /// <param name="newVersionStr">新版本号</param>
        /// <param name="curVersionStr">当前版本号</param>
        /// <returns>如果有新版本返回true，否则返回false</returns>
        public static bool CheckVersion(string newVersionStr, string curVersionStr)
        {
            try
            {
                // 1. 将版本号在第一个 '-' 处分割，分离出 [数字部分] 和 [预发布标签]
                var newVersionParts = newVersionStr.Split(new[] { '-' }, 2);
                var curVersionParts = curVersionStr.Split(new[] { '-' }, 2);

                var newNumericPart = newVersionParts[0];
                var curNumericPart = curVersionParts[0];

                // 2. 使用 System.Version 类来安全、准确地比较数字部分
                var newVer = new Version(newNumericPart);
                var curVer = new Version(curNumericPart);

                int comparison = newVer.CompareTo(curVer);

                // 3. 如果数字部分不相等，直接得出结论
                if (comparison != 0)
                {
                    // 例如: 6.1 vs 6.0, 或者 5.9 vs 6.0
                    return comparison > 0;
                }

                // --- 至此，数字部分完全相同 (例如, 都是 6.0.0) ---

                // 4. 根据预发布标签的有无来判断
                bool newIsPreRelease = newVersionParts.Length > 1;
                bool curIsPreRelease = curVersionParts.Length > 1;

                // 规则: 稳定版 > 预发布版
                if (!newIsPreRelease && curIsPreRelease)
                {
                    // 新版是稳定版 (6.0.0), 当前是预发布版 (6.0.0-beta) -> 新版更"新"
                    return true;
                }

                if (newIsPreRelease && !curIsPreRelease)
                {
                    // 新版是预发布版 (6.0.0-beta), 当前是稳定版 (6.0.0) -> 新版不更"新"
                    return false;
                }

                if (!newIsPreRelease && !curIsPreRelease)
                {
                    // 两者都是同版本的稳定版 (6.0.0 vs 6.0.0) -> 没有更新
                    return false;
                }

                // 5. 两者都是同一数字版本的预发布版，比较标签
                // 例如: 6.0.0-rc.1 vs 6.0.0-beta.2
                // 简单的字符串比较可以覆盖大部分情况 ("rc.1" > "beta.2")
                return string.Compare(newVersionParts[1], curVersionParts[1], StringComparison.OrdinalIgnoreCase) > 0;
            }
            catch (Exception ex)
            {
                // 如果任何解析失败，回退到简单的字符串比较作为保险措施
                Debug.WriteLine($"版本比较时发生错误: {ex.Message}。回退到字符串比较。");
                // 确保不会因为预发布标签导致错误的回退结果
                // 例如 "6.0-beta" vs "6.0"，简单的字符串比较会认为 "-beta"更大，这是错误的
                // 所以我们只在解析失败时做一个最基础的、不含标签的比较
                try
                {
                    var newNumericPart = newVersionStr.Split('-')[0];
                    var curNumericPart = curVersionStr.Split('-')[0];
                    return new Version(newNumericPart).CompareTo(new Version(curNumericPart)) > 0;
                }
                catch
                {
                    // 如果连这个都失败了，那只能放弃比较
                    return false;
                }
            }
        }
    }
}
