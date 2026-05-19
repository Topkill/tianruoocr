//存储一些疑似无用的代码片段

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text; // 确保引用
using System.Threading;
using System.Web;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
    public partial class FmMain
    {
        /// <summary>
		/// 使用百度OCR服务识别屏幕截图中的文本内容（备用方法，已弃用，忽略即可））
		/// </summary>
		public void OCR_baidu_bak()
        {
            split_txt = "";
            try
            {
                var str = "CHN_ENG";
                split_txt = "";
                var image = image_screen;
                var array = OcrHelper.ImgToBytes(image);
                // 根据界面标识设置语言类型
                switch (interface_flag)
                {
                    case "中英":
                        str = "CHN_ENG";
                        break;
                    case "日语":
                        str = "JAP";
                        break;
                    case "韩语":
                        str = "KOR";
                        break;
                }
                // 构造请求数据并发送到百度OCR接口
                var data = "type=general_location&image=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(array)) + "&language_type=" + str;
                var value = CommonHelper.PostStrData("http://ai.baidu.com/tech/ocr/general", data);
                var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["data"]["words_result"].ToString());
                var str2 = "";
                var str3 = "";
                // 处理OCR识别结果
                foreach (var arr in jArray)
                {
                    var jObject = JObject.Parse(arr.ToString());
                    var array2 = jObject["words"].ToString().ToCharArray();
                    if (!char.IsPunctuation(array2[array2.Length - 1]))
                    {
                        if (!contain_ch(jObject["words"].ToString()))
                        {
                            str3 = str3 + jObject["words"].ToString().Trim() + " ";
                        }
                        else
                        {
                            str3 += jObject["words"].ToString();
                        }
                    }
                    else if (own_punctuation(array2[array2.Length - 1].ToString()))
                    {
                        if (!contain_ch(jObject["words"].ToString()))
                        {
                            str3 = str3 + jObject["words"].ToString().Trim() + " ";
                        }
                        else
                        {
                            str3 += jObject["words"].ToString();
                        }
                    }
                    else
                    {
                        str3 = str3 + jObject["words"] + "\r\n";
                    }
                    str2 = str2 + jObject["words"] + "\r\n";
                }
                split_txt = str2;
                typeset_txt = str3;
            }
            catch
            {
                if (esc != "退出")
                {
                    if (RichBoxBody.Text != "***该区域未发现文本***")
                    {
                        RichBoxBody.Text = "***该区域未发现文本***";
                    }
                }
                else
                {
                    if (RichBoxBody.Text != "***该区域未发现文本***")
                    {
                        RichBoxBody.Text = "***该区域未发现文本***";
                    }
                    esc = "";
                }
            }
        }
        /// <summary>
		/// 使用百度OCR识别图片内容，并将结果添加到OCR_baidu_b变量中
		/// </summary>
		/// <param name="image">需要识别的图片</param>
		public void OcrBdUseB(Image image)
        {
            try
            {
                var str = "CHN_ENG";
                var array = OcrHelper.ImgToBytes(image);
                var data = "type=general_location&image=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(array)) + "&language_type=" + str;
                var url = "http://ai.baidu.com/aidemo";
                var referer = "http://ai.baidu.com/tech/ocr/general";
                var value = CommonHelper.PostStrData(url, data, "", referer);
                var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["data"]["words_result"].ToString());
                var text = "";
                var array2 = new string[jArray.Count];
                for (var i = 0; i < jArray.Count; i++)
                {
                    var jObject = JObject.Parse(jArray[i].ToString());
                    text += jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                    array2[jArray.Count - 1 - i] = jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                }
                OCR_baidu_b = (OCR_baidu_b + text + "\r\n").Replace("\r\n\r\n", "");
                Thread.Sleep(10);
            }
            catch (Exception)
            {
                //
            }
        }

        /// <summary>
        /// 使用百度OCR识别图片内容，并将结果添加到OCR_baidu_a变量中
        /// </summary>
        /// <param name="image">需要识别的图片</param>
        public void OcrBdUseA(Image image)
        {
            try
            {
                var str = "CHN_ENG";
                var array = OcrHelper.ImgToBytes(image);
                var data = "type=general_location&image=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(array)) + "&language_type=" + str;
                var bytes = Encoding.UTF8.GetBytes(data);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ai.baidu.com/tech/ocr/general");
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.GetResponse().Close();
                var url = "http://ai.baidu.com/aidemo";
                var referer = "http://ai.baidu.com/tech/ocr/general";
                var value = CommonHelper.PostStrData(url, data, "", referer);
                var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["data"]["words_result"].ToString());
                var text = "";
                var array2 = new string[jArray.Count];
                for (var i = 0; i < jArray.Count; i++)
                {
                    var jObject = JObject.Parse(jArray[i].ToString());
                    text += jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                    array2[jArray.Count - 1 - i] = jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                }
                OCR_baidu_a = (OCR_baidu_a + text + "\r\n").Replace("\r\n\r\n", "");
                Thread.Sleep(10);
            }
            catch (Exception)
            {
                //
            }
        }
        /// <summary>
		/// 使用百度OCR识别图片内容，并将结果添加到OCR_baidu_e变量中
		/// </summary>
		/// <param name="image">需要识别的图片</param>
		public void OcrBdUseE(Image image)
        {
            try
            {
                var str = "CHN_ENG";
                var array = OcrHelper.ImgToBytes(image);
                var data = "type=general_location&image=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(array)) + "&language_type=" + str;
                var url = "http://ai.baidu.com/aidemo";
                var referer = "http://ai.baidu.com/tech/ocr/general";
                var value = CommonHelper.PostStrData(url, data, "", referer);
                var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["data"]["words_result"].ToString());
                var text = "";
                var array2 = new string[jArray.Count];
                for (var i = 0; i < jArray.Count; i++)
                {
                    var jObject = JObject.Parse(jArray[i].ToString());
                    text += jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                    array2[jArray.Count - 1 - i] = jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                }
                OCR_baidu_e = (OCR_baidu_e + text + "\r\n").Replace("\r\n\r\n", "");
                Thread.Sleep(10);
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// 使用百度OCR识别图片内容，并将结果添加到OCR_baidu_d变量中
        /// </summary>
        /// <param name="image">需要识别的图片</param>
        public void OcrBdUseD(Image image)
        {
            try
            {
                var str = "CHN_ENG";
                var array = OcrHelper.ImgToBytes(image);
                var data = "type=general_location&image=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(array)) + "&language_type=" + str;
                var url = "http://ai.baidu.com/aidemo";
                var referer = "http://ai.baidu.com/tech/ocr/general";
                var value = CommonHelper.PostStrData(url, data, "", referer);
                var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["data"]["words_result"].ToString());
                var text = "";
                var array2 = new string[jArray.Count];
                for (var i = 0; i < jArray.Count; i++)
                {
                    var jObject = JObject.Parse(jArray[i].ToString());
                    text += jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                    array2[jArray.Count - 1 - i] = jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                }
                OCR_baidu_d = (OCR_baidu_d + text + "\r\n").Replace("\r\n\r\n", "");
                Thread.Sleep(10);
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// 使用百度OCR识别图片内容，并将结果添加到OCR_baidu_c变量中
        /// </summary>
        /// <param name="image">需要识别的图片</param>
        public void OcrBdUseC(Image image)
        {
            try
            {
                var str = "CHN_ENG";
                var array = OcrHelper.ImgToBytes(image);
                var data = "type=general_location&image=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(array)) + "&language_type=" + str;
                var url = "http://ai.baidu.com/aidemo";
                var referer = "http://ai.baidu.com/tech/ocr/general";
                var value = CommonHelper.PostStrData(url, data, "", referer);
                var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["data"]["words_result"].ToString());
                var text = "";
                var array2 = new string[jArray.Count];
                for (var i = 0; i < jArray.Count; i++)
                {
                    var jObject = JObject.Parse(jArray[i].ToString());
                    text += jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                    array2[jArray.Count - 1 - i] = jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
                }
                OCR_baidu_c = (OCR_baidu_c + text + "\r\n").Replace("\r\n\r\n", "");
                Thread.Sleep(10);
            }
            catch
            {
                //
            }
        }
    }
}