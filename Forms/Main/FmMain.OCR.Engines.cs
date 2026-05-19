using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;

namespace TrOCR
{
    public partial class FmMain
    {

		/// <summary>
		/// 使用腾讯云OCR服务识别屏幕截图中的文本内容
		/// </summary>
		public void OCR_Tencent()
		{
			Image imageToProcess = image_screen;
			Image tempBitmap = null;

			try
			{
				split_txt = "";
				typeset_txt = "";

				// 判断是否使用高精度模式
				bool isAccurate = (interface_flag == "腾讯-高精度");
				string secretId = isAccurate ? StaticValue.TX_ACCURATE_API_ID : StaticValue.TX_API_ID;
				string secretKey = isAccurate ? StaticValue.TX_ACCURATE_API_KEY : StaticValue.TX_API_KEY;
				string language = isAccurate ? StaticValue.TX_ACCURATE_LANGUAGE : StaticValue.TX_LANGUAGE;
				string apiType = isAccurate ? "GeneralAccurateOCR" : "GeneralBasicOCR";

				// 检查密钥是否已配置
				if (string.IsNullOrEmpty(secretId) || string.IsNullOrEmpty(secretKey))
				{
					typeset_txt = isAccurate ? "***请在设置中输入腾讯云高精度版密钥***" : "***请在设置中输入腾讯云密钥***";
					split_txt = typeset_txt;
					return;
				}

				// 调整图像尺寸以适应OCR识别要求
				if (imageToProcess.Width > 90 && imageToProcess.Height < 90)
				{
					tempBitmap = new Bitmap(imageToProcess.Width, 300);
					using (Graphics graphics = Graphics.FromImage(tempBitmap))
					{
						graphics.DrawImage(imageToProcess, 5, 0, imageToProcess.Width, imageToProcess.Height);
					}
					imageToProcess = tempBitmap;
				}
				else if (imageToProcess.Width <= 90 && imageToProcess.Height >= 90)
				{
					tempBitmap = new Bitmap(300, imageToProcess.Height);
					using (Graphics graphics2 = Graphics.FromImage(tempBitmap))
					{
						graphics2.DrawImage(imageToProcess, 0, 5, imageToProcess.Width, imageToProcess.Height);
					}
					imageToProcess = tempBitmap;
				}
				else if (imageToProcess.Width < 90 && imageToProcess.Height < 90)
				{
					tempBitmap = new Bitmap(300, 300);
					using (Graphics graphics3 = Graphics.FromImage(tempBitmap))
					{
						graphics3.DrawImage(imageToProcess, 5, 5, imageToProcess.Width, imageToProcess.Height);
					}
					imageToProcess = tempBitmap;
				}

				// 将图像转换为字节数组并调用腾讯OCR接口
				byte[] imageBytes = OcrHelper.ImgToBytes(imageToProcess);

				string result = TencentOcrHelper.Ocr(imageBytes, secretId, secretKey, apiType, language);
				typeset_txt = result;
				split_txt = result;
			}
			catch (Exception ex)
			{
				typeset_txt = $"***腾讯OCR识别出错: {ex.Message}***";
				split_txt = typeset_txt;
				if (esc == "退出")
				{
					esc = "";
				}
			}
			finally
			{
				tempBitmap?.Dispose();
			}
		}


		/// <summary>
		/// 使用微信OCR服务识别屏幕截图中的文本内容
		/// </summary>
		public void OCR_WeChat()
		{
			try
			{
				split_txt = "";
				typeset_txt = "";
				// 将图像转换为字节数组并调用微信OCR接口
				byte[] imageBytes = OcrHelper.ImgToBytes(image_screen);
				string result = OcrHelper.WeChat(imageBytes).GetAwaiter().GetResult();
				typeset_txt = result;
				split_txt = result;
			}
			catch (Exception ex)
			{
				if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
				{
					typeset_txt = split_txt="***微信OCR仅支持64位系统,不支持32位系统***";
					return;
					
				}
				typeset_txt = $"***微信OCR识别出错: {ex.Message}***";
				if (esc == "退出")
				{
					esc = "";
				}
			}
		}


		/// <summary>
		/// 使用白描OCR服务识别屏幕截图中的文本内容
		/// </summary>
		public void OCR_Baimiao()
		{
			try
			{
				split_txt = "";
				typeset_txt = "";
				
				// 将图像转换为字节数组并调用白描OCR接口
				byte[] imageBytes = OcrHelper.ImgToBytes(image_screen);
				// 调用已重构的、无参数的Baimiao方法
				string result = OcrHelper.Baimiao(imageBytes).GetAwaiter().GetResult();
				typeset_txt = result;
				split_txt = result;
			}
			catch (Exception ex)
			{
				typeset_txt = $"***白描OCR识别出错: {ex.Message}***";
				split_txt = typeset_txt;
				if (esc == "退出")
				{
					esc = "";
				}
			}
		}


		/// <summary>
		/// 使用百度OCR服务识别屏幕截图中的文本内容
		/// 调用百度OCR通用文字识别API进行文字识别，并根据识别结果更新文本框内容
		/// </summary>
		public void OCR_baidu()
		{
			split_txt = "";
			try
			{
		  				// 从 StaticValue 读取语言类型
		  				string languageType = StaticValue.BD_LANGUAGE;

		  var imageBytes = OcrHelper.ImgToBytes(image_screen);
		  // 调用已重构的、无密钥参数的方法
		  var result = BaiduOcrHelper.GeneralBasic(imageBytes, languageType);

		  if (!string.IsNullOrEmpty(result))
		  {
					if (result.StartsWith("***") || result.Contains("错误") || result.Contains("失败"))
					{
						// 错误信息直接显示
						if (esc != "退出")
						{
							RichBoxBody.Text = result;
						}
						else
						{
							RichBoxBody.Text = "***该区域未发现文本***";
							esc = "";
						}
					}
					else
					{
						// 处理识别结果
						ProcessOcrResult(result);
					}
				}
				else
				{
					RichBoxBody.Text = "***百度OCR识别失败***";
				}
			}
			catch (Exception ex)
			{
				if (esc != "退出")
				{
					RichBoxBody.Text = "***该区域未发现文本或者密钥次数用尽***";
				}
				else
				{
					RichBoxBody.Text = "***该区域未发现文本***";
					esc = "";
				}
			}
		}


		/// <summary>
		/// 百度OCR高精度版
		/// 使用百度OCR高精度版服务识别屏幕截图中的文本内容
		/// </summary>
		public void OCR_baidu_accurate()
		{
			split_txt = "";
			try
			{
		              // 从 StaticValue 读取高精度版设置
		              string languageType = StaticValue.BD_ACCURATE_LANGUAGE;

		  var imageBytes = OcrHelper.ImgToBytes(image_screen);
		  // 调用已重构的、无密钥参数的方法
		  var result = BaiduOcrHelper.AccurateBasic(imageBytes, languageType);

		  if (!string.IsNullOrEmpty(result))
		  {
					if (result.StartsWith("***") || result.Contains("错误") || result.Contains("失败"))
					{
						// 错误信息直接显示
						if (esc != "退出")
						{
							RichBoxBody.Text = result;
						}
						else
						{
							RichBoxBody.Text = "***该区域未发现文本***";
							esc = "";
						}
					}
					else
					{
						// 处理识别结果
						ProcessOcrResult(result);
					}
				}
				else
				{
					RichBoxBody.Text = "***百度高精度OCR识别失败***";
				}
			}
			catch (Exception ex)
			{
				if (esc != "退出")
				{
					RichBoxBody.Text = "***该区域未发现文本或者密钥次数用尽***";
				}
				else
				{
					RichBoxBody.Text = "***该区域未发现文本***";
					esc = "";
				}
			}
		}


        /// <summary>
		/// 处理OCR识别结果
		/// 将OCR识别出的文本结果进行处理和格式化
        /// </summary>
		/// <param name="result">OCR识别出的原始文本结果</param>
        private void ProcessOcrResult(string result)
        {
			// 将纯文本结果转换为之前的格式进行处理
			var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var jArray = new JArray();
            foreach (var line in lines)
            {
				if (!string.IsNullOrWhiteSpace(line))
                {
                    var jObject = new JObject();
					jObject["words"] = line;
                    jArray.Add(jObject);
                }
            }

            if (jArray.Count > 0)
            {
                checked_txt(jArray, 1, "words");
            }
            else
            {
                split_txt = "";
                typeset_txt = "";
            }
        }


        /// <summary>
        /// PaddleOCR离线识别方法
        /// 使用PaddleOCR引擎进行本地离线文字识别
        /// </summary>
        public void OCR_PaddleOCR()
		{
			split_txt = "";
			try
			{
				var result = PaddleOCRHelper.RecognizeText(image_screen);
				image_screen?.Dispose();
				// GC.Collect();
				// GC.WaitForPendingFinalizers();

				if (!string.IsNullOrEmpty(result))
				{
					if (result.StartsWith("***") || result.Contains("错误") || result.Contains("失败"))
					{
						// 错误信息直接显示
						if (esc != "退出")
						{
							// RichBoxBody.Text = result;//这里有bug，所以改为下面两行代码
							typeset_txt = result;
							split_txt = typeset_txt; // 必须也把这个变量设置一下
						}
						else
						{
							typeset_txt = "***该区域未发现文本***";
							split_txt = typeset_txt;
							esc = "";
						}
					}
					else
					{
                        // 处理识别结果
                        //ProcessOcrResult(result);
                        // 匹配 1次 或 多次 连续的换行
                        typeset_txt = Regex.Replace(result, @"(\r\n)+", "\r\n");
                        split_txt = typeset_txt;
                    }
				}
				else
				{ //这里应该也要改，先标记一下，暂时不改
					RichBoxBody.Text = "***PaddleOCR识别失败***";
				}
			}
			catch (Exception ex)
			{
				//if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
				//{
				//    typeset_txt = split_txt = "***PaddleOCR仅支持64位系统,不支持32位系统***";
				//    return;

				//}
				if (esc != "退出")
				{
					//RichBoxBody.Text = "***PaddleOCR识别失败: " + ex.Message + "***";这里有bug，所以改为下面两行代码
					typeset_txt = "***PaddleOCR识别失败: " + ex.Message + "***";
					split_txt = typeset_txt; // 必须也把这个变量设置一下
				}
				else
				{
					typeset_txt = "***该区域未发现文本***";
					split_txt = typeset_txt;
					esc = "";
				}
            }
			 TrOCRUtils.CleanMemory();
		}


		/// <summary>
		/// PaddleOCR2离线识别方法
		/// 使用Sdcb.PaddleOCR引擎进行本地离线文字识别
		/// </summary>
		public void OCR_PaddleOCR2()
		{
			split_txt = "";
			try
			{
				var result = PaddleOCR2Helper.RecognizeText(image_screen);
				image_screen?.Dispose();
                // GC.Collect();
                // GC.WaitForPendingFinalizers();

                if (!string.IsNullOrEmpty(result))
				{
					if (result.StartsWith("***") || result.Contains("错误") || result.Contains("失败"))
					{
						// 错误信息直接显示
						if (esc != "退出")
						{
							typeset_txt = result;
                    		split_txt = typeset_txt;
						}
						else
						{
							typeset_txt = "***该区域未发现文本***";
							split_txt = typeset_txt;
							esc = "";
						}
					}
					else
					{
                        // 处理识别结果
                        //ProcessOcrResult(result);
                        // 匹配 1次 或 多次 连续的换行
                        typeset_txt = Regex.Replace(result, @"(\r\n)+", "\r\n");
                        split_txt = typeset_txt;
                    }
				}
				else
				{
					RichBoxBody.Text = "***PaddleOCR2识别失败***";
				}
            }
			catch (Exception ex)
			{
                //if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
                //{
                //    typeset_txt = split_txt = "***PaddleOCR2仅支持64位系统,不支持32位系统***";
                //    return;

                //}
                if (esc != "退出")
				{
                    typeset_txt = "***PaddleOCR2识别失败: " + ex.Message + "***";
                    split_txt = typeset_txt;
                }
				else
				{
                    typeset_txt = "***该区域未发现文本***";
                    split_txt = typeset_txt;
                    esc = "";
                }
            }
			 TrOCRUtils.CleanMemory();
		}


		/// <summary>
		/// RapidOCR离线识别方法
		/// 使用RapidOCR引擎进行本地离线文字识别
		/// </summary>
		public void OCR_RapidOCR()
		{
			split_txt = "";
			try
			{
				var result = RapidOCRHelper.RecognizeText(image_screen);
				
				if (!string.IsNullOrEmpty(result))
				{
					if (result.StartsWith("***") || result.Contains("错误") || result.Contains("失败"))
					{
						// 错误信息直接显示
						if (esc != "退出")
						{
							typeset_txt = result;
                   		split_txt = typeset_txt; // 必须也把这个变量设置一下
						}
						else
						{
							typeset_txt = "***该区域未发现文本***";
							split_txt = typeset_txt;
							esc = "";
						}
					}
					else
					{
                        // 处理识别结果
                        //ProcessOcrResult(result);
                        // 匹配 1次 或 多次 连续的换行
                        typeset_txt = Regex.Replace(result, @"(\r\n)+", "\r\n");
                        split_txt = typeset_txt;
                    }
				}
				else
				{
					RichBoxBody.Text = "***RapidOCR识别失败***";
				}
            }
			catch (Exception ex)
			{
				if (esc != "退出")
				{
                   typeset_txt = "***RapidOCR识别失败: " + ex.Message + "***";
                   split_txt = typeset_txt; // 必须也把这个变量设置一下
               }
				else
				{
                   typeset_txt = "***该区域未发现文本***";
                   split_txt = typeset_txt;
                   esc = "";
               }
            }
			 TrOCRUtils.CleanMemory();
		}


		/// <summary>
		/// 有道OCR识别方法
		/// 调用有道OCR接口进行文字识别，对图像进行预处理以提高识别准确率
		/// </summary>
		public void OCR_youdao()
		{
			try
			{
				split_txt = "";
				var image = image_screen;
				// 对过小的图像进行填充以达到合适的识别尺寸
				if (image.Width > 90 && image.Height < 90)
				{
					var bitmap = new Bitmap(image.Width, 200);
					var graphics = Graphics.FromImage(bitmap);
					graphics.DrawImage(image, 5, 0, image.Width, image.Height);
					graphics.Save();
					graphics.Dispose();
					image = new Bitmap(bitmap);
				}
				else if (image.Width <= 90 && image.Height >= 90)
				{
					var bitmap2 = new Bitmap(200, image.Height);
					var graphics2 = Graphics.FromImage(bitmap2);
					graphics2.DrawImage(image, 0, 5, image.Width, image.Height);
					graphics2.Save();
					graphics2.Dispose();
					image = new Bitmap(bitmap2);
				}
				else if (image.Width < 90 && image.Height < 90)
				{
					var bitmap3 = new Bitmap(200, 200);
					var graphics3 = Graphics.FromImage(bitmap3);
					graphics3.DrawImage(image, 5, 5, image.Width, image.Height);
					graphics3.Save();
					graphics3.Dispose();
					image = new Bitmap(bitmap3);
				}
				else
				{
					image = image_screen;
				}
				var i = image.Width;
				var j = image.Height;
				// 对图像进行放大处理以提高识别准确率
				if (i < 600)
				{
					while (i < 600)
					{
						j *= 2;
						i *= 2;
					}
				}
				if (j < 120)
				{
					while (j < 120)
					{
						j *= 2;
						i *= 2;
					}
				}
				var bitmap4 = new Bitmap(i, j);
				var graphics4 = Graphics.FromImage(bitmap4);
				graphics4.DrawImage(image, 0, 0, i, j);
				graphics4.Save();
				graphics4.Dispose();
				image = new Bitmap(bitmap4);
				var inArray = OcrHelper.ImgToBytes(image);
				var data = "imgBase=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(inArray)) + "&lang=auto&company=";
				var value = CommonHelper.PostStrData("http://aidemo.youdao.com/ocrapi1", data, "",
					"http://aidemo.youdao.com/ocrdemo");
				var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["lines"].ToString());
				checked_txt(jArray, 1, "words");
				image.Dispose();
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
		/// 执行搜狗OCR识别功能
		/// 调用OCR识别接口并处理识别结果，根据设置决定是否分段显示
		/// </summary>
		public void SougouOCR()
		{
			try
			{
				split_txt = "";
				Image image = ZoomImage((Bitmap)image_screen, 120, 120);
				//var value = OcrHelper.SgOcr(image);
				var value = OcrHelper.SgBasicOpenOcr(image);
				var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["result"].ToString());
				if (IniHelper.GetValue("工具栏", "分段") == "True")
				{
					checked_location_sougou(jArray, 1, "content", "frame");
				}
				else
				{
					checked_txt(jArray, 1, "content");
				}
				image.Dispose();
			}
			catch
			{
				if (esc != "退出")
				{
					RichBoxBody.Text = "***该区域未发现文本***";
				}
				else
				{
					RichBoxBody.Text = "***该区域未发现文本***";
					esc = "";
				}
			}
		}


		/// <summary>
		/// 合并三个字节数组为一个数组
		/// </summary>
		/// <param name="a">第一个字节数组</param>
		/// <param name="b">第二个字节数组</param>
		/// <param name="c">第三个字节数组</param>
		/// <returns>合并后的字节数组</returns>
		public static byte[] MergeByte(byte[] a, byte[] b, byte[] c)
		{
			var array = new byte[a.Length + b.Length + c.Length];
			a.CopyTo(array, 0);
			b.CopyTo(array, a.Length);
			c.CopyTo(array, a.Length + b.Length);
			return array;
		}


		/// <summary>
		/// 百度手写识别
		/// </summary>
		public void OCR_baidu_handwriting()
		{
		    split_txt = "";
			try
			{
				var imageBytes = OcrHelper.ImgToBytes(image_screen);
				// 【修改】将配置的语言传递给识别方法
				var result = BaiduOcrHelper.Handwriting(imageBytes, StaticValue.BD_HANDWRITING_LANGUAGE);
				// ProcessOcrResult(result); //不需要再次处理
				split_txt = typeset_txt = result;
				
		    }
			catch (Exception ex)
			{
				typeset_txt = $"***百度手写识别出错: {ex.Message}***";
				split_txt = typeset_txt;
			}
		}


		/// <summary>
		/// 使用OCR技术识别图像中的数学公式
		/// </summary>
		public void OCR_Math()
		{
			split_txt = "";
			try
			{
				var img = image_screen;
				var inArray = OcrHelper.ImgToBytes(img);
				// 构造发送到Mathpix API的JSON数据
				var s = "{\t\"formats\": [\"latex_styled\", \"text\"],\t\"metadata\": {\t\t\"count\": 0,\t\t\"platform\": \"windows 10\",\t\t\"skip_recrop\": true,\t\t\"user_id\": \"\",\t\t\"version\": \"snip.windows@01.02.0027\"\t},\t\"ocr\": [\"text\", \"math\"],\t\"src\": \"data:image/jpeg;base64," + Convert.ToBase64String(inArray) + "\"}";
				var bytes = Encoding.UTF8.GetBytes(s);
				// 创建并配置HTTP请求
				var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.mathpix.com/v3/latex");
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Timeout = 8000;
				httpWebRequest.ReadWriteTimeout = 5000;
				httpWebRequest.Headers.Add("app_id: mathpix_chrome");
				httpWebRequest.Headers.Add("app_key: 85948264c5d443573286752fbe8df361");
				using (var requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				// 发送请求并获取响应
				var responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
				var value = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")).ReadToEnd();
				responseStream.Close();
				// 解析响应结果，提取LaTeX格式的数学公式
				var text = "$" + ((JObject)JsonConvert.DeserializeObject(value))["latex_styled"] + "$";
				split_txt = text;
				typeset_txt = text;
			}
			catch
			{
				// 处理异常情况并显示相应错误信息
				if (esc != "退出")
				{
					RichBoxBody.Text = "***该区域未发现文本或者密钥次数用尽***";
				}
				else
				{
					RichBoxBody.Text = "***该区域未发现文本***";
					esc = "";
				}
			}
		}

    }
}
