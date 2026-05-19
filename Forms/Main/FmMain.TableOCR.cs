using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;

namespace TrOCR
{
	public partial class FmMain
	{
		/// <summary>
		/// 使用百度API进行表格OCR识别
		/// 该方法会截取当前屏幕图像，调用百度表格OCR API进行识别，并将结果处理后显示在RichBoxBody中
		/// </summary>
		public void BdTableOCR()
		{
			typeset_txt = "[消息]：表格已下载！";
			split_txt = "";
			// this.lastRecognizedTable = null; // 每次识别前清空 
    		this.lastRecognizedHeader = null;
    		this.lastRecognizedFooter = null;
			this.lastBaiduCells = null;
    		this.lastOcrProvider = "Baidu";
			try
			{
				// 获取图像字节数组
				var image = image_screen;
				var imageBytes = OcrHelper.ImgToBytes(image);

				// 调用新的表格识别方法
				// string result = BaiduOcrHelper.TableRecognition(imageBytes, out this.lastRecognizedTable, out this.lastRecognizedHeader, out this.lastRecognizedFooter, false, false);
				DataTable dummyTable; // DataTable 现在只是个附属品
				string result = BaiduOcrHelper.TableRecognition(imageBytes, out dummyTable, out this.lastRecognizedHeader, out this.lastRecognizedFooter, out this.lastBaiduCells, false, false);

				// 检查识别结果
				if (string.IsNullOrWhiteSpace(result))
				{
					typeset_txt = "***该区域未发现文本***";
				}
				else
				{
					// 设置识别结果
					typeset_txt = result;
				}
				split_txt = "";
			}
			catch (Exception ex)
			{
				typeset_txt = $"[消息]：表格识别异常: {ex.Message}";
			}
		}

		/// <summary>
		/// 腾讯表格OCR识别方法
		/// 该方法会截取当前屏幕图像，调用腾讯表格OCR API进行识别，并将结果处理后显示在RichBoxBody中
		/// </summary>
		public void TxTableOCR()
		{
			typeset_txt = "[消息]：表格已下载！";
			split_txt = "";
			// this.lastRecognizedTable = null; 
    		this.lastRecognizedHeader = null;
    		this.lastRecognizedFooter = null;
			this.lastTencentCells = null;
    		this.lastOcrProvider = "Tencent";
			try
			{
				// 获取图像字节数组
				var image = image_screen;
				var imageBytes = OcrHelper.ImgToBytes(image);

				// 调用腾讯表格识别方法
				// string result = TencentOcrHelper.TableRecognition(imageBytes,out this.lastRecognizedTable,out this.lastRecognizedHeader, out this.lastRecognizedFooter);
				DataTable dummyTable;
				string result = TencentOcrHelper.TableRecognition(imageBytes, out dummyTable, out this.lastRecognizedHeader, out this.lastRecognizedFooter, out this.lastTencentCells);
				// 检查识别结果
				if (string.IsNullOrWhiteSpace(result))
				{
					typeset_txt = "***该区域未发现文本***";
				}
				else
				{
					// 设置识别结果
					typeset_txt = result;
				}
				split_txt = "";
			}
			catch (Exception ex)
			{
				typeset_txt = $"[消息]：表格识别异常: {ex.Message}";
			}
		}

		/// <summary>
		/// 使用阿里云OCR服务识别表格
		/// </summary>
		public void OCR_ali_table()
		{
			var text = "";
			split_txt = "";
			try
			{
				var value = IniHelper.GetValue("特殊", "ali_cookie");
				var stream = BytesToStream(ImageToByteArray(BWPic((Bitmap)image_screen)));
				var str = Convert.ToBase64String(new BinaryReader(stream).ReadBytes(Convert.ToInt32(stream.Length)));
				stream.Close();
				var postStr = "{\n\t\"image\": \"" + str + "\",\n\t\"configure\": \"{\\\"format\\\":\\\"html\\\", \\\"finance\\\":false}\"\n}";
				var url = "https://predict-pai.data.aliyun.com/dp_experience_mall/ocr/ocr_table_parse";
				text = CommonHelper.PostStrData(url, postStr, value);
				typeset_txt = ((JObject)JsonConvert.DeserializeObject(CommonHelper.PostStrData(url, postStr, value)))["tables"].ToString().Replace("table tr td { border: 1px solid blue }", "table tr td {border: 0.5px black solid }").Replace("table { border: 1px solid blue }", "table { border: 0.5px black solid; border-collapse : collapse}\r\n");
				RichBoxBody.Text = "[消息]：表格已复制到粘贴板！";
			}
			catch
			{
				RichBoxBody.Text = "[消息]：阿里表格识别出错！";
				if (text.Contains("NEED_LOGIN"))
				{
					split_txt = "弹出cookie";
				}
			}
		}

		/// <summary>
		/// 表格OCR主线程处理函数
		/// 该方法处理OCR识别完成后的结果展示和相关清理工作
		/// </summary>
		public void Main_OCR_Thread_table()
		{
			ailibaba = new AliTable();
			var timeSpan = new TimeSpan(DateTime.Now.Ticks);
			var timeSpan2 = timeSpan.Subtract(ts).Duration();
			var str = string.Concat(new[]
			{
				timeSpan2.Seconds.ToString(),
				".",
				Convert.ToInt32(timeSpan2.TotalMilliseconds).ToString(),
				"秒"
			});
			// 根据配置设置窗口是否置顶
			if (StaticValue.v_topmost)
			{
				TopMost = true;
			}
			else
			{
				TopMost = false;
			}
			Text = "耗时：" + str;
			// 根据接口类型处理识别结果
			if (interface_flag == "百度表格")
			{
				// 1. 检查识别是否成功
				bool isSuccess = !string.IsNullOrWhiteSpace(typeset_txt) &&
								 !typeset_txt.Contains("***请在设置中输入百度标准版密钥或表格识别专用密钥***") &&
								 !typeset_txt.Contains("***该区域未发现文本***") &&
								 !typeset_txt.Contains("***该区域未发现表格***") &&
								 !typeset_txt.Contains("错误") &&
								 !typeset_txt.Contains("异常") &&
								 !typeset_txt.Contains("失败");

				// 如果识别成功，复制到剪贴板
				if (isSuccess)
				{
					// 显示识别结果
					RichBoxBody.Text = "[消息]：百度表格识别成功，已复制到粘贴板！可直接导出Excel文件或粘贴到Excel";

					// 提取HTML表格部分
					string htmlTable = ExtractHtmlTable(typeset_txt);

					if (!string.IsNullOrEmpty(htmlTable))
					{
						// 设置HTML格式到剪贴板，Excel可以识别并保持表格结构
						var dataObject = new DataObject();
						dataObject.SetData(DataFormats.Html, CreateHtmlClipboardData(htmlTable));
						dataObject.SetData(DataFormats.UnicodeText, typeset_txt);
						SetClipboardWithLock(dataObject);
						Debug.WriteLine("百度识别表格结果复制到剪贴板-html格式");
					}
					else
					{
						// 如果没有HTML表格，使用普通文本
						var dataObject = new DataObject();
						dataObject.SetData(DataFormats.UnicodeText, typeset_txt);
						SetClipboardWithLock(dataObject);
						Debug.WriteLine("百度识别表格结果复制到剪贴板-普通文本格式");
					}

				}
				else
				{
					// 如果失败，显示实际的错误信息
					RichBoxBody.Text = typeset_txt;
				}
			}
			else if (interface_flag == "腾讯表格")
			{
				// 检查腾讯表格识别是否成功
				bool isSuccess = !string.IsNullOrWhiteSpace(typeset_txt) &&
								 !typeset_txt.Contains("***请在设置中输入腾讯标准版密钥或表格识别专用密钥***") &&
								 !typeset_txt.Contains("***该区域未发现文本***") &&
								 !typeset_txt.Contains("***该区域未发现表格***") &&
								 !typeset_txt.Contains("错误") &&
								 !typeset_txt.Contains("异常") &&
								 !typeset_txt.Contains("失败");

				// 如果识别成功，复制到剪贴板
				if (isSuccess)
				{
					// 显示识别结果
					RichBoxBody.Text = "[消息]：腾讯表格识别成功，已复制到粘贴板！可直接导出Excel文件或粘贴到Excel";

					// 提取HTML表格部分
					string htmlTable = ExtractHtmlTable(typeset_txt);

					if (!string.IsNullOrEmpty(htmlTable))
					{
						// 设置HTML格式到剪贴板，Excel可以识别并保持表格结构
						var dataObject = new DataObject();
						dataObject.SetData(DataFormats.Html, CreateHtmlClipboardData(htmlTable));
						dataObject.SetData(DataFormats.UnicodeText, typeset_txt);
						SetClipboardWithLock(dataObject);
						Debug.WriteLine("腾讯识别表格结果复制到剪贴板-html格式");
					}
					else
					{
						// 如果没有HTML表格，使用普通文本
						var dataObject = new DataObject();
						dataObject.SetData(DataFormats.UnicodeText, typeset_txt);
						SetClipboardWithLock(dataObject);
						Debug.WriteLine("腾讯识别表格结果复制到剪贴板-普通文本格式");
					}
				}
				else
				{
					// 如果失败，显示实际的错误信息
					RichBoxBody.Text = typeset_txt;
				}
			}
			
			// 清理资源
			image_screen.Dispose();
			GC.Collect();
			StaticValue.IsCapture = false;
			FormBorderStyle = FormBorderStyle.Sizable;
			Visible = true;
			Show();
			WindowState = FormWindowState.Normal;
			// Size = new Size(form_width, form_height);
			this.Size = this.lastNormalSize;
			HelpWin32.SetForegroundWindow(Handle);
			if (interface_flag == "阿里表格")
			{
				if (split_txt == "弹出cookie")
				{
					split_txt = "";
					ailibaba.TopMost = true;
					ailibaba.getcookie = "";
					IniHelper.SetValue("特殊", "ali_cookie", ailibaba.getcookie);
					ailibaba.ShowDialog();
					HelpWin32.SetForegroundWindow(ailibaba.Handle);
					return;
				}
				SetClipboardWithLock(typeset_txt);
				Debug.WriteLine("阿里表格结果剪切板写入成功");
				CopyHtmlToClipBoard(typeset_txt);
			}
			
		}

		/// <summary>
		/// 表格OCR按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_table_Click(object sender, EventArgs e)
		{
			OCR_foreach("表格");
		}

		/// <summary>
		/// 百度表格OCR识别按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_baidutable_Click(object sender, EventArgs e)
		{
			OCR_foreach("百度表格");
		}

		/// <summary>
		/// 腾讯表格OCR识别按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_txtable_Click(object sender, EventArgs e)
		{
			OCR_foreach("腾讯表格");
		}

		/// <summary>
		/// 阿里表格OCR识别按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_ailitable_Click(object sender, EventArgs e)
		{
			OCR_foreach("阿里表格");
		}

		private void ExportExcel_Click(object sender, EventArgs e)
		{
		    // 调用我们创建的帮助类来处理导出
		    // Helper.ExcelHelper.ExportToExcel(this.lastRecognizedTable,this.lastRecognizedHeader, this.lastRecognizedFooter);
			if (this.lastOcrProvider == "Baidu")
		    {
		        Helper.ExcelHelper.ExportToExcel(this.lastBaiduCells, this.lastRecognizedHeader, this.lastRecognizedFooter,this);
		    }
		    else if (this.lastOcrProvider == "Tencent")
		    {
		        Helper.ExcelHelper.ExportToExcel(this.lastTencentCells, this.lastRecognizedHeader, this.lastRecognizedFooter,this);
		    }
		    else
		    {
		        MessageBox.Show("没有可供导出的表格数据。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
		    }
		}

		/// <summary>
		/// 将HTML内容复制到剪贴板
		/// </summary>
		/// <param name="html">要复制到剪贴板的HTML内容</param>
		public void CopyHtmlToClipBoard(string html)
		{
			var utf = Encoding.UTF8;
			// HTML剪贴板格式的标准头部信息
			var format = "Version:0.9\r\nStartHTML:{0:000000}\r\nEndHTML:{1:000000}\r\nStartFragment:{2:000000}\r\nEndFragment:{3:000000}\r\n";
			// HTML片段的开始标记和结束标记
			var text = "<html>\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=" + utf.WebName + "\">\r\n<title>HTML clipboard</title>\r\n</head>\r\n<body>\r\n<!--StartFragment-->";
			var text2 = "<!--EndFragment-->\r\n</body>\r\n</html>\r\n";
			// 计算各个部分的字节位置
			var s = string.Format(format, 0, 0, 0, 0);
			var byteCount = utf.GetByteCount(s);
			var byteCount2 = utf.GetByteCount(text);
			var byteCount3 = utf.GetByteCount(html);
			var byteCount4 = utf.GetByteCount(text2);
			// 构造完整的HTML剪贴板数据
			var s2 = string.Format(format, byteCount, byteCount + byteCount2 + byteCount3 + byteCount4, byteCount + byteCount2, byteCount + byteCount2 + byteCount3) + text + html + text2;
			var dataObject = new DataObject();
			dataObject.SetData(DataFormats.Html, new MemoryStream(utf.GetBytes(s2)));
			var data = new HtmlToText().Convert(html);
			dataObject.SetData(DataFormats.UnicodeText, data);
			SetClipboardWithLock(dataObject);
			Debug.WriteLine("识别表格结果写入剪贴板");
		}

		/// <summary>
		/// 从文本中提取HTML表格部分
		/// </summary>
		/// <param name="text">包含HTML表格的文本</param>
		/// <returns>HTML表格字符串，如果没有找到则返回空字符串</returns>
		private string ExtractHtmlTable(string text)
		{
			if (string.IsNullOrEmpty(text))
				return string.Empty;

			int startIndex = text.IndexOf("<table");
			if (startIndex == -1)
				return string.Empty;

			int endIndex = text.IndexOf("</table>", startIndex);
			if (endIndex == -1)
				return string.Empty;

			endIndex += "</table>".Length;
			return text.Substring(startIndex, endIndex - startIndex);
		}

		/// <summary>
		/// 创建HTML剪贴板数据格式
		/// </summary>
		/// <param name="htmlTable">HTML表格字符串</param>
		/// <returns>符合剪贴板格式的HTML数据</returns>
		private string CreateHtmlClipboardData(string htmlTable)
		{
			if (string.IsNullOrEmpty(htmlTable))
				return string.Empty;

			// HTML剪贴板格式需要特定的头部信息
			string htmlClipboardData = 
				"Version:0.9" + Environment.NewLine +
				"StartHTML:0000000000" + Environment.NewLine +
				"EndHTML:0000000000" + Environment.NewLine +
				"StartFragment:0000000000" + Environment.NewLine +
				"EndFragment:0000000000" + Environment.NewLine +
				"<html>" + Environment.NewLine +
				"<body>" + Environment.NewLine +
				"<!--StartFragment-->" + Environment.NewLine +
				htmlTable + Environment.NewLine +
				"<!--EndFragment-->" + Environment.NewLine +
				"</body>" + Environment.NewLine +
				"</html>";

			// 计算偏移量
			int startHTML = htmlClipboardData.IndexOf("<html>");
			int endHTML = htmlClipboardData.IndexOf("</html>") + "</html>".Length;
			int startFragment = htmlClipboardData.IndexOf("<!--StartFragment-->") + "<!--StartFragment-->".Length;
			int endFragment = htmlClipboardData.IndexOf("<!--EndFragment-->");

			// 更新偏移量（10位数字，左补0）
			htmlClipboardData = htmlClipboardData.Replace("StartHTML:0000000000", string.Format("StartHTML:{0:D10}", startHTML));
			htmlClipboardData = htmlClipboardData.Replace("EndHTML:0000000000", string.Format("EndHTML:{0:D10}", endHTML));
			htmlClipboardData = htmlClipboardData.Replace("StartFragment:0000000000", string.Format("StartFragment:{0:D10}", startFragment));
			htmlClipboardData = htmlClipboardData.Replace("EndFragment:0000000000", string.Format("EndFragment:{0:D10}", endFragment));

			return htmlClipboardData;
		}

		/// <summary>
		/// 将彩色图像转换为黑白图像
		/// </summary>
		/// <param name="mybm">需要转换的原始彩色图像</param>
		/// <returns>转换后的黑白图像</returns>
		public Bitmap BWPic(Bitmap mybm)
		{
			var bitmap = new Bitmap(mybm.Width, mybm.Height);
			// 遍历图像中的每个像素点
			for (var i = 0; i < mybm.Width; i++)
			{
				for (var j = 0; j < mybm.Height; j++)
				{
					var pixel = mybm.GetPixel(i, j);
					// 通过计算RGB三个分量的平均值来获得灰度值
					var num = (pixel.R + pixel.G + pixel.B) / 3;
					bitmap.SetPixel(i, j, Color.FromArgb(num, num, num));
				}
			}
			return bitmap;
		}

		/// <summary>
		/// 将Image对象转换为字节数组
		/// </summary>
		/// <param name="img">要转换的Image对象</param>
		/// <returns>表示图像数据的字节数组</returns>
		public static byte[] ImageToByteArray(Image img)
		{
			return (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
		}

		/// <summary>
		/// 将字节数组转换为Stream对象
		/// </summary>
		/// <param name="bytes">要转换的字节数组</param>
		/// <returns>包含字节数据的Stream对象</returns>
		public static Stream BytesToStream(byte[] bytes)
		{
			return new MemoryStream(bytes);
		}
	}
}
