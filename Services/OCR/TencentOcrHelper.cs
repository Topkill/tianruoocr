using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace TrOCR.Helper
{
    public class TencentOcrHelper
    {
        public static Dictionary<string, string> GetStandardLanguages()
        {
            return new Dictionary<string, string>
            {
                { "zh", "中英混合" },
        		{ "zh_rare", "中英混合（含生僻字等）" },
        		{ "auto", "自动检测" },
        		{ "mix", "多语言混合" },
        		{ "jap", "日语" },
        		{ "kor", "韩语" },
        		{ "spa", "西班牙语" },
        		{ "fre", "法语" },
        		{ "ger", "德语" },
        		{ "por", "葡萄牙语" },
        		{ "vie", "越南语" },
        		{ "may", "马来语" },
        		{ "rus", "俄语" },
        		{ "ita", "意大利语" },
        		{ "hol", "荷兰语" },
        		{ "swe", "瑞典语" },
        		{ "fin", "芬兰语" },
        		{ "dan", "丹麦语" },
        		{ "nor", "挪威语" },
        		{ "hun", "匈牙利语" },
        		{ "tha", "泰语" },
        		{ "hi", "印地语" },
        		{ "ara", "阿拉伯语" }
            };
        }

        public static Dictionary<string, string> GetAccurateLanguages()
        {
            return new Dictionary<string, string>
            {
                { "auto", "自动检测" }
            };
        }

        public static string Ocr(byte[] image, string secretId, string secretKey, string action, string languageType)
        {
            try
            {
                var host = "ocr.tencentcloudapi.com";
                var service = "ocr";
                var version = "2018-11-19";
                // var region = "ap-guangzhou";
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                var httpRequestMethod = "POST";
                var canonicalUri = "/";
                var canonicalQueryString = "";
                var canonicalHeaders = "content-type:application/json; charset=utf-8\n" + "host:" + host + "\n";
                var signedHeaders = "content-type;host";

                var imageBase64 = Convert.ToBase64String(image);
                
                string payload;
                if (action == "GeneralAccurateOCR")
                {
                    payload = "{\"ImageBase64\":\"" + imageBase64 + "\"}";
                }
                else // GeneralBasicOCR
                {
                    payload = "{\"ImageBase64\":\"" + imageBase64 + "\",\"LanguageType\":\"" + languageType + "\"}";
                }

                var hashedRequestPayload = Sha256(payload);
                var canonicalRequest = httpRequestMethod + "\n" +
                                       canonicalUri + "\n" +
                                       canonicalQueryString + "\n" +
                                       canonicalHeaders + "\n" +
                                       signedHeaders + "\n" +
                                       hashedRequestPayload;

                var algorithm = "TC3-HMAC-SHA256";
                var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToString("yyyy-MM-dd");
                var credentialScope = date + "/" + service + "/tc3_request";
                var hashedCanonicalRequest = Sha256(canonicalRequest);
                var stringToSign = algorithm + "\n" +
                                   timestamp + "\n" +
                                   credentialScope + "\n" +
                                   hashedCanonicalRequest;

                var secretDate = HmacSha256(Encoding.UTF8.GetBytes("TC3" + secretKey), Encoding.UTF8.GetBytes(date));
                var secretService = HmacSha256(secretDate, Encoding.UTF8.GetBytes(service));
                var secretSigning = HmacSha256(secretService, Encoding.UTF8.GetBytes("tc3_request"));
                var signature = BitConverter.ToString(HmacSha256(secretSigning, Encoding.UTF8.GetBytes(stringToSign))).Replace("-", "").ToLower();

                var authorization = algorithm + " " +
                                    "Credential=" + secretId + "/" + credentialScope + ", " +
                                    "SignedHeaders=" + signedHeaders + ", " +
                                    "Signature=" + signature;

                var url = "https://" + host;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Authorization", authorization);
                request.Headers.Add("X-TC-Action", action);
                request.Headers.Add("X-TC-Version", version);
                request.Headers.Add("X-TC-Timestamp", timestamp.ToString());
                // request.Headers.Add("X-TC-Region", region);

                byte[] data = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream resStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                        {
                            string result = reader.ReadToEnd();
                            JObject jObject = JObject.Parse(result);
                            if (jObject["Response"]?["Error"] != null)
                            {
                                return "OCR Error: " + jObject["Response"]["Error"]["Message"].ToString();
                            }

                            var textDetections = jObject["Response"]?["TextDetections"];
                            if (textDetections == null)
                            {
                                return "OCR Error: No text detected.";
                            }
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in textDetections)
                            {
                                sb.AppendLine(item["DetectedText"]?.ToString());
                            }
                            return sb.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "OCR Exception: " + ex.Message;
            }
        }
        
        public static string VerifyTencentKey(string secretId, string secretKey)
        {
            try
            {
                var host = "ocr.tencentcloudapi.com";
                var service = "ocr";
                var action = "GeneralBasicOCR"; // Use standard for verification
                var version = "2018-11-19";
                // var region = "ap-guangzhou";
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                var httpRequestMethod = "POST";
                var canonicalUri = "/";
                var canonicalQueryString = "";
                var canonicalHeaders = "content-type:application/json; charset=utf-8\n" + "host:" + host + "\n";
                var signedHeaders = "content-type;host";
                var payload = "{}"; // Empty payload for verification

                var hashedRequestPayload = Sha256(payload);
                var canonicalRequest = httpRequestMethod + "\n" +
                                       canonicalUri + "\n" +
                                       canonicalQueryString + "\n" +
                                       canonicalHeaders + "\n" +
                                       signedHeaders + "\n" +
                                       hashedRequestPayload;

                var algorithm = "TC3-HMAC-SHA256";
                var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToString("yyyy-MM-dd");
                var credentialScope = date + "/" + service + "/tc3_request";
                var hashedCanonicalRequest = Sha256(canonicalRequest);
                var stringToSign = algorithm + "\n" +
                                   timestamp + "\n" +
                                   credentialScope + "\n" +
                                   hashedCanonicalRequest;

                var secretDate = HmacSha256(Encoding.UTF8.GetBytes("TC3" + secretKey), Encoding.UTF8.GetBytes(date));
                var secretService = HmacSha256(secretDate, Encoding.UTF8.GetBytes(service));
                var secretSigning = HmacSha256(secretService, Encoding.UTF8.GetBytes("tc3_request"));
                var signature = BitConverter.ToString(HmacSha256(secretSigning, Encoding.UTF8.GetBytes(stringToSign))).Replace("-", "").ToLower();

                var authorization = algorithm + " " +
                                    "Credential=" + secretId + "/" + credentialScope + ", " +
                                    "SignedHeaders=" + signedHeaders + ", " +
                                    "Signature=" + signature;

                var url = "https://" + host;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Authorization", authorization);
                request.Headers.Add("X-TC-Action", action);
                request.Headers.Add("X-TC-Version", version);
                request.Headers.Add("X-TC-Timestamp", timestamp.ToString());
                // request.Headers.Add("X-TC-Region", region);

                byte[] data = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream resStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                return "{\"Response\":{\"Error\":{\"Code\":\"SdkException\",\"Message\":\"" + ex.Message + "\"}}}";
            }
            catch (Exception ex)
            {
                return "{\"Response\":{\"Error\":{\"Code\":\"LocalException\",\"Message\":\"" + ex.Message + "\"}}}";
            }
        }
        
        private static string Sha256(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 腾讯表格识别方法
        /// </summary>
        /// <param name="image">图片字节数组</param>
        /// <returns>HTML格式的表格识别结果</returns>
        public static string TableRecognition(byte[] image,out DataTable tableResult, out List<string> headerTexts, out List<string> footerTexts,out List<TableCell> bodyCells)
        {
             // 初始化所有 out 参数
            tableResult = null;
            headerTexts = new List<string>();
            footerTexts = new List<string>();
            bodyCells = new List<TableCell>();
            try
            {
                // 获取腾讯表格识别密钥，如果为空则使用标准版密钥
                string secretId = string.IsNullOrWhiteSpace(StaticValue.TX_TABLE_API_ID) ? StaticValue.TX_API_ID : StaticValue.TX_TABLE_API_ID;
                string secretKey = string.IsNullOrWhiteSpace(StaticValue.TX_TABLE_API_KEY) ? StaticValue.TX_API_KEY : StaticValue.TX_TABLE_API_KEY;
                if (string.IsNullOrEmpty(secretId) || string.IsNullOrEmpty(secretKey))
                {
                    return "***请在设置中输入腾讯标准版密钥或表格识别专用密钥***";
                }

                var host = "ocr.tencentcloudapi.com";
                var service = "ocr";
                var version = "2018-11-19";
                var action = "RecognizeTableAccurateOCR";
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                var httpRequestMethod = "POST";
                var canonicalUri = "/";
                var canonicalQueryString = "";
                var canonicalHeaders = "content-type:application/json; charset=utf-8\n" + "host:" + host + "\n";
                var signedHeaders = "content-type;host";

                var imageBase64 = Convert.ToBase64String(image);
                var payload = "{\"ImageBase64\":\"" + imageBase64 + "\"}";

                var hashedRequestPayload = Sha256(payload);
                var canonicalRequest = httpRequestMethod + "\n" +
                                       canonicalUri + "\n" +
                                       canonicalQueryString + "\n" +
                                       canonicalHeaders + "\n" +
                                       signedHeaders + "\n" +
                                       hashedRequestPayload;

                var algorithm = "TC3-HMAC-SHA256";
                var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToString("yyyy-MM-dd");
                var credentialScope = date + "/" + service + "/tc3_request";
                var hashedCanonicalRequest = Sha256(canonicalRequest);
                var stringToSign = algorithm + "\n" +
                                   timestamp + "\n" +
                                   credentialScope + "\n" +
                                   hashedCanonicalRequest;

                var secretDate = HmacSha256(Encoding.UTF8.GetBytes("TC3" + secretKey), Encoding.UTF8.GetBytes(date));
                var secretService = HmacSha256(secretDate, Encoding.UTF8.GetBytes(service));
                var secretSigning = HmacSha256(secretService, Encoding.UTF8.GetBytes("tc3_request"));
                var signature = BitConverter.ToString(HmacSha256(secretSigning, Encoding.UTF8.GetBytes(stringToSign))).Replace("-", "").ToLower();

                var authorization = algorithm + " " +
                                    "Credential=" + secretId + "/" + credentialScope + ", " +
                                    "SignedHeaders=" + signedHeaders + ", " +
                                    "Signature=" + signature;

                var url = "https://" + host;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Authorization", authorization);
                request.Headers.Add("X-TC-Action", action);
                request.Headers.Add("X-TC-Version", version);
                request.Headers.Add("X-TC-Timestamp", timestamp.ToString());

                byte[] data = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream resStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                        {
                            string result = reader.ReadToEnd();
                            return ProcessTencentTableResult(result, out tableResult, out headerTexts, out footerTexts, out bodyCells);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "腾讯表格识别异常: " + ex.Message;
            }
        }

        /// <summary>
        /// 处理腾讯表格识别结果，生成统一的HTML表格和DataTable (新版本)
        /// </summary>
        private static string ProcessTencentTableResult(string jsonResult, out DataTable tableResult, out List<string> headerTexts, out List<string> footerTexts,out List<TableCell> bodyCells)
        {
            // 初始化
            tableResult = new DataTable();
            headerTexts = new List<string>();
            footerTexts = new List<string>();
            bodyCells = new List<TableCell>();
            try
            {
                JObject jObject = JObject.Parse(jsonResult);

                if (jObject["Response"]?["Error"] != null)
                {
                    string errorCode = jObject["Response"]["Error"]["Code"]?.ToString() ?? "UnknownCode";
                    string errorMessage = jObject["Response"]["Error"]["Message"]?.ToString() ?? "Unknown error message.";
                    return $"腾讯表格识别错误 - Code: {errorCode}, Message: {errorMessage}";
                }

                var tableDetections = jObject["Response"]?["TableDetections"];
                if (tableDetections == null || !tableDetections.HasValues)
                {
                    return "***该区域未发现表格***";
                }

                // --- 1. 分离结构化表格和独立的页眉/页脚文本块 ---
                var structuredTables = new List<JToken>();
                // --- 解析独立的 Header 和 Footer 文本块 ---
                foreach (var table in tableDetections)
                {
                    var cells = table["Cells"];
                    if (cells == null || !cells.HasValues) continue;
                    bool isStructuredTable = cells.Any(c => (c["RowTl"]?.Value<int>() ?? -1) >= 0);
                    if (isStructuredTable) { structuredTables.Add(table); }
                    else // 独立的、无坐标的文本块
                    {
                        foreach (var cell in cells)
                        {
                            string text = cell["Text"]?.Value<string>() ?? "";
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                string blockType = cell["Type"]?.Value<string>() ?? "body";
                                if (blockType == "header")
                                {
                                    headerTexts.Add(text);
                                }
                                else
                                {
                                    footerTexts.Add(text);
                                }
                            }
                        }
                    }
                }

                if (!structuredTables.Any() && !headerTexts.Any() && !footerTexts.Any())
                {
                    return "***未识别到任何表格或文本内容***";
                }

                var mainTable = structuredTables.FirstOrDefault();
                int maxCol = 1;
                int maxRow = 0;

                if (mainTable != null)
                {
                    var cells = mainTable["Cells"];
                    // var tableCells = new List<TableCell>();
                    // 注意：这里不再用 tableCells 局部变量，而是直接填充 out 参数 bodyCells

                    foreach (var cell in cells)
                    {
                        int rowTl = cell["RowTl"]?.Value<int>() ?? -1;
                        if (rowTl < 0) continue;

                        int colTl = cell["ColTl"].Value<int>();
                        int rowBr = cell["RowBr"].Value<int>();
                        int colBr = cell["ColBr"].Value<int>();

                        bodyCells.Add(new TableCell
                        {
                            Text = cell["Text"]?.Value<string>() ?? "",
                            Type = cell["Type"]?.Value<string>() ?? "body",
                            Row = rowTl,
                            Col = colTl,
                            // 注意：腾讯的 Br 是不包含的边界，所以跨度是 Br - Tl
                            RowSpan = rowBr - rowTl,
                            ColSpan = colBr - colTl
                        });
                        maxRow = Math.Max(maxRow, rowBr);
                        maxCol = Math.Max(maxCol, colBr);
                    }
                    Debug.WriteLine($"4444一共{bodyCells.Count}个单元格");


                    // --- 开始构建 DataTable ---
                    if (maxRow > 0 && maxCol > 0)
                    {
                        // 注意：腾讯的坐标是从0开始，maxRow/maxCol是不包含的边界，所以实际行/列数是 maxRow+1, maxCol+1
                        maxRow++;
                        maxCol++;

                        // 创建一个二维数组来模拟表格布局，处理合并单元格
                        string[,] grid = new string[maxRow, maxCol];

                        foreach (var cell in bodyCells)
                        {
                            if (cell.Row < maxRow && cell.Col < maxCol)
                            {
                                grid[cell.Row, cell.Col] = cell.Text;
                            }
                        }

                        // 添加列
                        for (int c = 0; c < maxCol; c++)
                        {
                            tableResult.Columns.Add($"列 {c + 1}");
                        }

                        // 添加行
                        for (int r = 0; r < maxRow; r++)
                        {
                            DataRow newRow = tableResult.NewRow();
                            for (int c = 0; c < maxCol; c++)
                            {
                                newRow[c] = grid[r, c];
                            }
                            tableResult.Rows.Add(newRow);
                        }
                    }
                    // --- 结束构建 DataTable ---
                }
                //上面构建 DataTable：maxRow++,maxCol++，最大行列数都加了1，所以构建剪贴板html格式文本传递最大行列参数时要减1
                return ProcessTencentTableResult(mainTable, bodyCells, maxRow-1, maxCol-1, headerTexts, footerTexts); // 调用原方法来获取HTML
            }
            catch (Exception ex)
            {
                return "处理腾讯表格结果时发生异常: " + ex.Message;
            }
}


                       /// <summary>
        /// 处理腾讯表格识别结果，生成统一的HTML表格（可直接粘贴到Excel）
        /// </summary>
        /// <param name="jsonResult">腾讯API返回的JSON结果</param>
        /// <returns>HTML格式的表格</returns>
        private static string ProcessTencentTableResult(JToken mainTable, List<TableCell> bodyCells, int maxRow,int maxCol,List<string> headerTexts,List<string> footerTexts)
        {
            try
            {
                // JObject jObject = JObject.Parse(jsonResult);

                // if (jObject["Response"]?["Error"] != null)
                // {
                //     string errorCode = jObject["Response"]["Error"]["Code"]?.ToString() ?? "UnknownCode";
                //     string errorMessage = jObject["Response"]["Error"]["Message"]?.ToString() ?? "Unknown error message.";
                //     return $"腾讯表格识别错误 - Code: {errorCode}, Message: {errorMessage}";
                // }

                // var tableDetections = jObject["Response"]?["TableDetections"];
                // if (tableDetections == null || !tableDetections.HasValues)
                // {
                //     return "***该区域未发现表格***";
                // }

                // --- 1. 分离结构化表格和独立的页眉/页脚文本块 ---
                // var structuredTables = new List<JToken>();
                // var headerTexts = new List<string>();
                // var footerTexts = new List<string>();

                // foreach (var table in tableDetections)
                // {
                //     var cells = table["Cells"];
                //     if (cells == null || !cells.HasValues) continue;

                //     bool isStructuredTable = cells.Any(c => (c["RowTl"]?.Value<int>() ?? -1) >= 0);

                //     if (isStructuredTable)
                //     {
                //         structuredTables.Add(table);
                //     }
                //     else // 独立的、无坐标的文本块
                //     {
                //         foreach (var cell in cells)
                //         {
                //             string text = cell["Text"]?.Value<string>() ?? "";
                //             if (!string.IsNullOrWhiteSpace(text))
                //             {
                //                 string blockType = cell["Type"]?.Value<string>() ?? "body";
                //                 if (blockType == "header")
                //                 {
                //                     headerTexts.Add(text);
                //                 }
                //                 else
                //                 {
                //                     footerTexts.Add(text);
                //                 }
                //             }
                //         }
                //     }
                // }

                // if (!structuredTables.Any() && !headerTexts.Any() && !footerTexts.Any())
                // {
                //     return "***未识别到任何表格或文本内容***";
                // }

                StringBuilder finalHtml = new StringBuilder();

                // var mainTable = structuredTables.FirstOrDefault();

                // int maxCol = 1;
                // int maxRow = 0;

                if (mainTable != null)
                {
                    //var cells = mainTable["Cells"];
                    //var tableCells = new List<TableCell>();


                    //foreach (var cell in cells)
                    //{
                    //    int rowTl = cell["RowTl"]?.Value<int>() ?? -1;
                    //    if (rowTl < 0) continue;

                    //    int colTl = cell["ColTl"].Value<int>();
                    //    int rowBr = cell["RowBr"].Value<int>();
                    //    int colBr = cell["ColBr"].Value<int>();

                    //    tableCells.Add(new TableCell
                    //    {
                    //        Text = cell["Text"]?.Value<string>() ?? "",
                    //        Type = cell["Type"]?.Value<string>() ?? "body",
                    //        Row = rowTl,
                    //        Col = colTl,
                    //        RowSpan = rowBr - rowTl,
                    //        ColSpan = colBr - colTl
                    //    });
                    //    maxRow = Math.Max(maxRow, rowBr);
                    //    maxCol = Math.Max(maxCol, colBr);
                    //}

                    if (maxRow > 0 && maxCol > 0)
                    {
                        finalHtml.AppendLine("<table border='1' style='border-collapse: collapse;'>");

                        // --- 3. 在表格顶部插入页眉行 ---
                        // 将页眉处理逻辑修改为“水平整合”
                        if (headerTexts.Any())
                        {
                            finalHtml.AppendLine("  <thead>");
                            finalHtml.AppendLine("    <tr>"); // 只创建一个 <tr> 行

                            if (headerTexts.Count == 1)
                            {
                                // 如果只有一个页眉，则跨越整个表格
                                string encodedHeaderText = System.Web.HttpUtility.HtmlEncode(headerTexts[0]);
                                finalHtml.AppendLine($"      <th colspan='{maxCol}' style='text-align: center;background-color: #f0f0f0; font-weight: bold;'>{encodedHeaderText}</th>");
                            }
                            else
                            {
                                // 如果有多个页眉，则并排排列
                                foreach (var headerText in headerTexts)
                                {
                                    string encodedHeaderText = System.Web.HttpUtility.HtmlEncode(headerText);
                                    finalHtml.AppendLine($"      <th style='text-align: center; background-color: #f0f0f0;font-weight: bold;'>{encodedHeaderText}</th>");
                                }
                            }

                            finalHtml.AppendLine("    </tr>");
                            finalHtml.AppendLine("  </thead>");
                        }

                        // --- 4. 构建表格主体 ---
                        finalHtml.AppendLine("  <tbody>");
                        var grid = new bool[maxRow, maxCol];
                        for (int r = 0; r < maxRow; r++)
                        {
                            finalHtml.AppendLine("    <tr>");
                            for (int c = 0; c < maxCol; c++)
                            {
                                if (grid[r, c]) continue;

                                //var currentCell = tableCells.FirstOrDefault(cell => cell.Row == r && cell.Col == c);
                                var currentCell = bodyCells.FirstOrDefault(cell => cell.Row == r && cell.Col == c);

                                if (currentCell != null)
                                {
                                     // --- ↓↓↓ 修正点 1：处理内容为空的现有单元格 ↓↓↓ ---
                                    string cellText = currentCell.Text;
                                    string encodedContent = System.Web.HttpUtility.HtmlEncode(cellText);
                                    if (string.IsNullOrEmpty(encodedContent))
                                    {
                                        encodedContent = "";
                                    }
                                    else
                                    {
                                        encodedContent = encodedContent.Replace("\n", "&#10;");
                                    }
                                    // --- ↑↑↑ 修正结束 ↑↑↑ ---
                                    string tag = (currentCell.Type == "header" && !headerTexts.Any()) ? "th" : "td";
                                    string rowspanAttr = currentCell.RowSpan > 1 ? $" rowspan='{currentCell.RowSpan}'" : "";
                                    string colspanAttr = currentCell.ColSpan > 1 ? $" colspan='{currentCell.ColSpan}'" : "";
                                    // string cellText = System.Web.HttpUtility.HtmlEncode(currentCell.Text).Replace("\n", "&#10;");


                                    // finalHtml.AppendLine($"      <{tag}{rowspanAttr}{colspanAttr}>{cellText}</{tag}>");
                                    finalHtml.AppendLine($"      <{tag}{rowspanAttr}{colspanAttr}>{encodedContent}</{tag}>");

                                    for (int i = 0; i < currentCell.RowSpan; i++)
                                    {
                                        for (int j = 0; j < currentCell.ColSpan; j++)
                                        {
                                            if (r + i < maxRow && c + j < maxCol)
                                            {
                                                grid[r + i, c + j] = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    finalHtml.AppendLine("      <td></td>");
                                }
                            }
                            finalHtml.AppendLine("    </tr>");
                        }
                        finalHtml.AppendLine("  </tbody>");

                        // --- 5. 在表格底部插入页脚行 ---
                        if (footerTexts.Any())
                        {
                            finalHtml.AppendLine("  <tfoot>");
                            finalHtml.AppendLine("    <tr>");
                            int footerCellCount = footerTexts.Count;
                            if (footerCellCount > 0)
                            {
                                int colsPerFooter = maxCol / footerCellCount;
                                int extraCols = maxCol % footerCellCount;
                                int colsUsed = 0;

                                for (int i = 0; i < footerCellCount; i++)
                                {
                                    int currentColspan = colsPerFooter + (i < extraCols ? 1 : 0);
                                    if (i == footerCellCount - 1) // 最后一个单元格吃掉所有剩余的列
                                    {
                                        currentColspan = maxCol - colsUsed;
                                    }
                                    if (currentColspan <= 0) continue; // 避免无效的colspan

                                    string encodedFooterText = System.Web.HttpUtility.HtmlEncode(footerTexts[i]);
                                    finalHtml.Append($"      <td colspan='{currentColspan}' style='text-align: left;background-color: #f0f0f0;'>{encodedFooterText}</td>");
                                    colsUsed += currentColspan;
                                }
                            }
                            finalHtml.AppendLine("\n    </tr>");
                            finalHtml.AppendLine("  </tfoot>");
                        }

                        finalHtml.AppendLine("</table>");
                    }
                }
                else if (headerTexts.Any() || footerTexts.Any())
                {
                    finalHtml.AppendLine(string.Join("<br />", headerTexts.Concat(footerTexts).Select(t => System.Web.HttpUtility.HtmlEncode(t))));
                }

                return finalHtml.ToString();
            }
            catch (Exception ex)
            {
                return "处理腾讯表格结果时发生异常: " + ex.Message;
            }
        }
        
        /// <summary>
        /// 内部类，用于存储解析后的单元格信息
        /// </summary>
        public class TableCell
        {
            public string Text { get; set; }
            public string Type { get; set; } // "header", "body", "footer","context"
            public int Row { get; set; }
            public int Col { get; set; }
            public int RowSpan { get; set; }
            public int ColSpan { get; set; }
        }

        private static byte[] HmacSha256(byte[] key, byte[] msg)
        {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(msg);
            }
        }
    }
}