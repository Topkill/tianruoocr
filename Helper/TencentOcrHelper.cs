using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

using System.Collections.Generic;

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

        private static byte[] HmacSha256(byte[] key, byte[] msg)
        {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(msg);
            }
        }
    }
}