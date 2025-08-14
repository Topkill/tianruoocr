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

namespace TrOCR.Helper
{
    public class OcrHelper
    {
        private static ImageOcr ocr;

        public static void Dispose()
        {
            if (ocr != null)
            {
                ocr.Dispose();
                ocr = null;
            }
        }

        public static string Tencent(byte[] image, string secretId, string secretKey)
        {
            try
            {
                var host = "ocr.tencentcloudapi.com";
                var service = "ocr";
                var action = "GeneralBasicOCR";
                var version = "2018-11-19";
                var region = "ap-guangzhou";
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                // 1. Create canonical request
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

                // 2. Create string to sign
                var algorithm = "TC3-HMAC-SHA256";
                var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToString("yyyy-MM-dd");
                var credentialScope = date + "/" + service + "/tc3_request";
                var hashedCanonicalRequest = Sha256(canonicalRequest);
                var stringToSign = algorithm + "\n" +
                                   timestamp + "\n" +
                                   credentialScope + "\n" +
                                   hashedCanonicalRequest;

                // 3. Calculate signature
                var secretDate = HmacSha256(Encoding.UTF8.GetBytes("TC3" + secretKey), Encoding.UTF8.GetBytes(date));
                var secretService = HmacSha256(secretDate, Encoding.UTF8.GetBytes(service));
                var secretSigning = HmacSha256(secretService, Encoding.UTF8.GetBytes("tc3_request"));
                var signature = BitConverter.ToString(HmacSha256(secretSigning, Encoding.UTF8.GetBytes(stringToSign))).Replace("-", "").ToLower();

                // 4. Create authorization
                var authorization = algorithm + " " +
                                    "Credential=" + secretId + "/" + credentialScope + ", " +
                                    "SignedHeaders=" + signedHeaders + ", " +
                                    "Signature=" + signature;

                // 5. Send request
                var url = "https://" + host;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Authorization", authorization);
                request.Headers.Add("X-TC-Action", action);
                request.Headers.Add("X-TC-Version", version);
                request.Headers.Add("X-TC-Timestamp", timestamp.ToString());
                request.Headers.Add("X-TC-Region", region);

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
                var action = "GeneralBasicOCR";
                var version = "2018-11-19";
                var region = "ap-guangzhou";
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                var httpRequestMethod = "POST";
                var canonicalUri = "/";
                var canonicalQueryString = "";
                var canonicalHeaders = "content-type:application/json; charset=utf-8\n" + "host:" + host + "\n";
                var signedHeaders = "content-type;host";
                var payload = "{}";

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
                request.Headers.Add("X-TC-Region", region);

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

        private static string Sha256(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private static byte[] HmacSha256(byte[] key, byte[] msg)
        {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(msg);
            }
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

            var finishedTask = await Task.WhenAny(tcs.Task, Task.Delay(20000));
            if (finishedTask == tcs.Task)
            {
                return await tcs.Task;
            }
            return "微信OCR识别超时(20秒)";
        }
    }
}