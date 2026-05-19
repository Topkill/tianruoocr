using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TrOCR.Helper
{
    public static class WebDavHelper
    {
        // WebDav 备份文件名固定前缀
        private const string BackupPrefix = "TrOCR_Data_Backup_";
        // 定义云端备份的子文件夹名称
        private const string RemoteFolderName = "TrOCR_Backups";

        // HttpClient 设为单例，避免 Socket 耗尽
        private static readonly HttpClient _client;

        static WebDavHelper()
        {
        
            // 保留原有的协议支持，并强制开启 TLS 1.2
            //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            _client = new HttpClient();
            // 文件上传下载耗时不可预估，超时时间设置为无限时间，防止因网速慢导致文件传输中断。
            _client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
        }

        /// <summary>
        /// 备份：将指定文件夹下的特定文件打包并上传
        /// </summary>
        public static async Task<bool> BackupConfigAsync(string url, string user, string pass, string sourceDir, string[] filePatterns)
        {
           
            // 规范化 URL，确保以 / 结尾
            string baseUrl = url.EndsWith("/") ? url : url + "/";

            // 1. 生成带时间戳的压缩包路径（临时文件）
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string zipFileName = $"{BackupPrefix}{timeStamp}.zip";
            string tempZipPath = Path.Combine(Path.GetTempPath(), zipFileName);

            // 确保源路径以分隔符结尾
            string finalSourceDir = sourceDir.EndsWith(Path.DirectorySeparatorChar.ToString())
                ? sourceDir
                : sourceDir + Path.DirectorySeparatorChar;

            try
            {
                // 2. 创建压缩包
                if (File.Exists(tempZipPath)) File.Delete(tempZipPath);
                bool hasFile = false;
                // 使用 HashSet 记录已添加的文件路径，防止重复（比如 *.* 和 *.txt 同时存在时）
                var addedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                using (var zip = ZipFile.Open(tempZipPath, ZipArchiveMode.Create))
                {
                    foreach (var pattern in filePatterns)
                    {
                        string[] files;
                        try
                        {
                            files = Directory.GetFiles(finalSourceDir, pattern, SearchOption.AllDirectories);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"遍历文件发生错误：{ex.Message}"); 
                            continue;

                        }

                        foreach (var file in files)
                        {
                            if (file.Equals(tempZipPath, StringComparison.OrdinalIgnoreCase)) continue;
                            if (addedFiles.Contains(file)) continue;

                            // 计算相对路径，保持目录结构
                            string entryName = file.Substring(finalSourceDir.Length).Replace('\\', '/');

                            // 使用 FileShare.ReadWrite 读取，防止文件被占用导致报错
                            try
                            {
                                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    var entry = zip.CreateEntry(entryName);
                                    using (var entryStream = entry.Open())
                                    {
                                        fileStream.CopyTo(entryStream);
                                    }
                                }
                                addedFiles.Add(file);
                                hasFile = true;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"备份中断！\n无法读取文件：{entryName}\n错误原因：{ex.Message}");                                // 个别文件失败不中断整个流程，但可以记录日志
                            }
                        }
                    }
                }

                if (!hasFile)
                {
                    if (File.Exists(tempZipPath)) File.Delete(tempZipPath);
                    MessageBox.Show("未找到任何配置文件，备份取消。", "提示");
                    return false;
                }

                // 3. 上传到 WebDav
                // 拼接目标文件夹 URL (例如 https://dav.jianguoyun.com/dav/TrOCR_Backups/)
                string targetFolderUrl = baseUrl + RemoteFolderName + "/";

                // 确保目录存在
                await EnsureDirectoryExistsAsync(targetFolderUrl, user, pass);

                // 上传文件
                string uploadUrl = targetFolderUrl + zipFileName;

                using (var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl))
                {
                    request.Headers.Authorization = GetAuthHeader(user, pass);

                    using (var fs = new FileStream(tempZipPath, FileMode.Open, FileAccess.Read))
                    {
                        // 使用 StreamContent 上传
                        request.Content = new StreamContent(fs);
                        var response = await _client.SendAsync(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            string errorMsg = await response.Content.ReadAsStringAsync();
                            throw new Exception($"上传失败 [{response.StatusCode}]: {errorMsg}");
                        }
                    }
                }
                return true;
            }
            finally
            {
                // 4. 清理临时文件
                if (File.Exists(tempZipPath))
                {
                    try { File.Delete(tempZipPath); } catch { }
                }
            }
        }

        /// <summary>
        /// 恢复：自动查找文件名排序最大的文件（即最新时间戳）进行下载
        /// </summary>
        public static async Task<bool> RestoreLatestConfigAsync(string url, string user, string pass, string targetDir)
        {
            string baseUrl = url.EndsWith("/") ? url : url + "/";
            string targetFolderUrl = baseUrl + RemoteFolderName + "/";
            string latestFileUrl = "";

            // 1. 发送 PROPFIND 请求获取文件列表
            using (var request = new HttpRequestMessage(new HttpMethod("PROPFIND"), targetFolderUrl))
            {
                request.Headers.Authorization = GetAuthHeader(user, pass);
                request.Headers.Add("Depth", "1");

                var response = await _client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception($"云端不存在备份文件夹 ({RemoteFolderName})，请先执行备份。");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"连接 WebDav 服务器失败: {response.StatusCode}");
                }

                // 2. 解析 XML
                string xmlContent = await response.Content.ReadAsStringAsync();
                latestFileUrl = FindLatestBackupUrl(xmlContent, targetFolderUrl);
            }

            if (string.IsNullOrEmpty(latestFileUrl))
            {
                throw new Exception("服务器上未找到以 'TrOCR_Backup_' 开头的有效备份文件。");
            }

            // 3. 下载文件
            string tempZipPath = Path.Combine(Path.GetTempPath(), "TrOCR_Restore_Temp.zip");
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, latestFileUrl))
                {
                    request.Headers.Authorization = GetAuthHeader(user, pass);

                    var response = await _client.SendAsync(request);
                    if (!response.IsSuccessStatusCode) throw new Exception($"下载失败: {response.StatusCode}");

                    using (var fs = new FileStream(tempZipPath, FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }

                // 4. 解压覆盖
                if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

                using (var zip = ZipFile.OpenRead(tempZipPath))
                {
                    foreach (var entry in zip.Entries)
                    {
                        // 防止 Zip Slip 漏洞 (路径遍历攻击)
                        string destinationPath = Path.Combine(targetDir, entry.FullName);
                        string fullDestPath = Path.GetFullPath(destinationPath);
                        string fullTargetDir = Path.GetFullPath(targetDir);

                        if (!fullDestPath.StartsWith(fullTargetDir)) continue;

                        // 确保目标文件的文件夹存在
                        string entryDir = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(entryDir)) Directory.CreateDirectory(entryDir);

                        // 覆盖模式提取
                        entry.ExtractToFile(destinationPath, true);
                    }
                }
                return true;
            }
            finally
            {
                if (File.Exists(tempZipPath)) File.Delete(tempZipPath);
            }
        }

        /// <summary>
        /// 解析 WebDav XML，找到最新的备份 URL
        /// </summary>
        private static string FindLatestBackupUrl(string xmlContent, string folderUrl)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlContent);

                // 【核心修复】忽略 XML 命名空间查找 href 节点
                // 很多 WebDav 实现（如 OwnCloud/NextCloud）可能返回不同的前缀
                var hrefs = doc.Descendants()
                               .Where(x => x.Name.LocalName == "href")
                               .Select(x => x.Value)
                               .ToList();

                var backupFiles = hrefs
                    .Select(u => Uri.UnescapeDataString(u)) // 解码 URL
                    .Select(u =>
                    {
                        // 提取纯文件名用于判断和排序，防止路径干扰
                        // 注意：WebDav 返回的可能是 "/dav/path/file.zip"
                        string fileName = u.TrimEnd('/').Split('/').Last();
                        return new { Url = u, FileName = fileName };
                    })
                    .Where(x => x.FileName.StartsWith(BackupPrefix) && x.FileName.EndsWith(".zip"))
                    .OrderByDescending(x => x.FileName) // 按文件名(时间戳)倒序
                    .ToList();

                if (backupFiles.Count == 0) return null;

                string rawUrl = backupFiles.First().Url;

                // 【核心修复】使用 Uri 类处理路径拼接
                Uri baseUri = new Uri(folderUrl);

                // 如果 XML 返回的是绝对路径 (http://...)
                if (Uri.TryCreate(rawUrl, UriKind.Absolute, out Uri absoluteUri))
                {
                    return absoluteUri.ToString();
                }
                else
                {
                    // 如果是相对路径，安全合并
                    return new Uri(baseUri, rawUrl).ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("XML解析失败: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 确保 WebDav 目录存在 (MKCOL)
        /// </summary>
        private static async Task EnsureDirectoryExistsAsync(string folderUrl, string user, string pass)
        {
            using (var request = new HttpRequestMessage(new HttpMethod("MKCOL"), folderUrl))
            {
                request.Headers.Authorization = GetAuthHeader(user, pass);

                var response = await _client.SendAsync(request);

                // 201 Created: 创建成功
                // 405 Method Not Allowed: 目录已存在 (标准行为)
                if (response.StatusCode == HttpStatusCode.Created ||
                    response.StatusCode == HttpStatusCode.MethodNotAllowed)
                {
                    return;
                }

                // 409 Conflict: 父目录不存在
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("备份文件夹创建失败：父目录不存在。请检查 WebDav 根路径配置。");
                }

                // 其他错误则抛出
                if (!response.IsSuccessStatusCode)
                {
                    // 再次确认一下是否存在（有些服务器 MKCOL 失败但不一定是错的）
                    using (var headReq = new HttpRequestMessage(HttpMethod.Head, folderUrl))
                    {
                        headReq.Headers.Authorization = GetAuthHeader(user, pass);
                        var headRes = await _client.SendAsync(headReq);
                        if (headRes.IsSuccessStatusCode) return;
                    }

                    throw new Exception($"无法创建备份目录 [{response.StatusCode}]");
                }
            }
        }

        /// <summary>
        /// 生成 Basic Auth 请求头
        /// </summary>
        private static AuthenticationHeaderValue GetAuthHeader(string user, string pass)
        {
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}"));
            return new AuthenticationHeaderValue("Basic", authValue);
        }
    }
}