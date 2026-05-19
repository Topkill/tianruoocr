using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;

namespace TrOCR
{
	public partial class FmMain
	{
		/// <summary>
		/// 在输入图像中查找轮廓并为每个轮廓绘制边界框，将结果绘制到目标图像上
		/// </summary>
		/// <param name="src">输入的灰度图像，用于查找轮廓</param>
		/// <param name="draw">用于绘制结果的目标图像</param>
		/// <returns>带有边界框的图像</returns>
		public Image BoundingBox(Image<Gray, byte> src, Image<Bgr, byte> draw)
		{
			Image result;
			using (var vectorOfVectorOfPoint = new VectorOfVectorOfPoint())
			{
				// 查找图像中的轮廓
				CvInvoke.FindContours(src, vectorOfVectorOfPoint, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
				Image image = draw.ToBitmap();
				var graphics = Graphics.FromImage(image);
				var size = vectorOfVectorOfPoint.Size;
				// 遍历所有轮廓并绘制边界框
				for (var i = 0; i < size; i++)
				{
					using (var vectorOfPoint = vectorOfVectorOfPoint[i])
					{
						var rectangle = CvInvoke.BoundingRectangle(vectorOfPoint);
						var x = rectangle.Location.X;
						var y = rectangle.Location.Y;
						var width = rectangle.Size.Width;
						var height = rectangle.Size.Height;
						// 只处理大于5x5像素的轮廓
						if (width > 5 || height > 5)
						{
							graphics.FillRectangle(Brushes.White, x, 0, width, image.Size.Height);
						}
					}
				}
				graphics.Dispose();
				// 创建一个稍大的新位图以容纳结果
				var bitmap = new Bitmap(image.Width + 2, image.Height + 2);
				var graphics2 = Graphics.FromImage(bitmap);
				graphics2.DrawImage(image, 1, 1, image.Width, image.Height);
				graphics2.Save();
				graphics2.Dispose();
				result = bitmap;
			}
			return result;
		}

		/// <summary>
		/// 从源图像中查找轮廓并提取感兴趣的区域图像
		/// </summary>
		/// <param name="src">输入的灰度图像，用于查找轮廓</param>
		/// <param name="draw">输出图像，用于绘制结果</param>
		public void select_image(Image<Gray, byte> src, Image<Bgr, byte> draw)
		{
			try
			{
				// 查找图像中的轮廓
				using (var vectorOfVectorOfPoint = new VectorOfVectorOfPoint())
				{
					CvInvoke.FindContours(src, vectorOfVectorOfPoint, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
					var num = vectorOfVectorOfPoint.Size / 2;
					imagelist_lenght = num;
					bool_image_count(num);
					
					// 确保临时图像目录存在
					if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\image_temp"))
					{
						Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Data\\image_temp");
					}
					
					// 清空OCR结果变量
					OCR_baidu_a = "";
					OCR_baidu_b = "";
					OCR_baidu_c = "";
					OCR_baidu_d = "";
					OCR_baidu_e = "";
					
					// 遍历所有轮廓，提取对应的图像区域
					for (var i = 0; i < num; i++)
					{
						using (var vectorOfPoint = vectorOfVectorOfPoint[i])
						{
							var rectangle = CvInvoke.BoundingRectangle(vectorOfPoint);
							if (rectangle.Size.Width > 1 && rectangle.Size.Height > 1)
							{
								var x = rectangle.Location.X;
								var y = rectangle.Location.Y;
								var width = rectangle.Size.Width;
								var height = rectangle.Size.Height;
								new Point(x, 0);
								new Point(x, image_ori.Size.Height);
								var srcRect = new Rectangle(x, 0, width, image_ori.Size.Height);
								var bitmap = new Bitmap(width + 70, srcRect.Size.Height);
								var graphics = Graphics.FromImage(bitmap);
								graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Size.Width, bitmap.Size.Height);
								graphics.DrawImage(image_ori, 30, 0, srcRect, GraphicsUnit.Pixel);
								var bitmap2 = Image.FromHbitmap(bitmap.GetHbitmap());
								bitmap2.Save("Data\\image_temp\\" + i + ".jpg", ImageFormat.Jpeg);
								bitmap2.Dispose();
								bitmap.Dispose();
								graphics.Dispose();
							}
						}
					}
					
					// 显示加载消息对话框
					var messageload = new Messageload();
					messageload.ShowDialog();
					if (messageload.DialogResult == DialogResult.OK)
					{
						// 启动后台工作线程
						var array = new[]
						{
							new ManualResetEvent(false)
						};
						ThreadPool.QueueUserWorkItem(DoWork, array[0]);
					}
				}
			}
			catch
			{
				exit_thread();
			}
		}

		/// <summary>
		/// 查找图像中的边界框
		/// 该函数使用OpenCV处理图像，通过灰度化、腐蚀、阈值处理和边缘检测等步骤，
		/// 最终识别出图像中的主要对象并绘制边界框
		/// </summary>
		/// <param name="bitmap">需要处理的原始图像</param>
		/// <returns>带有边界框标记的图像</returns>
		public Image FindBundingBox(Bitmap bitmap)
		{
            var image = bitmap.ToImage<Bgr, byte>();
            var image2 = new Image<Gray, byte>(image.Width, image.Height);
			CvInvoke.CvtColor(image, image2, ColorConversion.Bgra2Gray);
			var structuringElement = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(4, 4), new Point(1, 1));
			CvInvoke.Erode(image2, image2, structuringElement, new Point(0, 2), 1, BorderType.Reflect101, default(MCvScalar));
			CvInvoke.Threshold(image2, image2, 100.0, 255.0, (ThresholdType)9);
            var image3 = image2.ToBitmap().ToImage<Gray, byte>();
            var draw = image3.Convert<Bgr, byte>();
			var image4 = image3.Clone();
			CvInvoke.Canny(image3, image4, 255.0, 255.0, 5, true);
			return BoundingBox(image4, draw);
		}

		/// <summary>
		/// 对指定范围的图像文件进行OCR识别（处理第一部分图像）
		/// </summary>
		/// <param name="objEvent">线程同步事件对象</param>
		public void baidu_image_a(object objEvent)
		{
			try
			{
				// 批量处理第一部分图片
				for (var i = 0; i < image_num[0]; i++)
				{
					Stream stream = File.Open("Data\\image_temp\\" + i + ".jpg", FileMode.Open);
					OcrBdUseA(Image.FromStream(stream));
					stream.Close();
				}
				((ManualResetEvent)objEvent).Set();
			}
			catch
			{
				exit_thread();
			}
		}

		/// <summary>
		/// 对指定范围的图像文件进行OCR识别（处理第二部分图像）
		/// </summary>
		/// <param name="objEvent">线程同步事件对象</param>
		public void baidu_image_b(object objEvent)
		{
			try
			{
				// 批量处理第二部分图片
				for (var i = image_num[0]; i < image_num[1]; i++)
				{
					Stream stream = File.Open("Data\\image_temp\\" + i + ".jpg", FileMode.Open);
					OcrBdUseB(Image.FromStream(stream));
					stream.Close();
				}
				((ManualResetEvent)objEvent).Set();
			}
			catch
			{
				exit_thread();
			}
		}

		/// <summary>
		/// 处理image_num[1]到image_num[2]范围内的图片文件，使用OcrBdUseC进行OCR识别
		/// </summary>
		/// <param name="objEvent">用于线程同步的ManualResetEvent对象</param>
		public void BdImageC(object objEvent)
		{
			try
			{
				for (var i = image_num[1]; i < image_num[2]; i++)
				{
					Stream stream = File.Open("Data\\image_temp\\" + i + ".jpg", FileMode.Open);
					OcrBdUseC(Image.FromStream(stream));
					stream.Close();
				}
				((ManualResetEvent)objEvent).Set();
			}
			catch
			{
				exit_thread();
			}
		}

		/// <summary>
		/// 处理image_num[2]到image_num[3]范围内的图片文件，使用OcrBdUseD进行OCR识别
		/// </summary>
		/// <param name="objEvent">用于线程同步的ManualResetEvent对象</param>
		public void BdImageD(object objEvent)
		{
			try
			{
				for (var i = image_num[2]; i < image_num[3]; i++)
				{
					Stream stream = File.Open("Data\\image_temp\\" + i + ".jpg", FileMode.Open);
					OcrBdUseD(Image.FromStream(stream));
					stream.Close();
				}
				((ManualResetEvent)objEvent).Set();
			}
			catch
			{
				exit_thread();
			}
		}

		/// <summary>
		/// 处理image_num[3]到image_num[4]范围内的图片文件，使用OcrBdUseE进行OCR识别
		/// </summary>
		/// <param name="objEvent">用于线程同步的ManualResetEvent对象</param>
		public void BdImageE(object objEvent)
		{
			try
			{
				for (var i = image_num[3]; i < image_num[4]; i++)
				{
					Stream stream = File.Open("Data\\image_temp\\" + i + ".jpg", FileMode.Open);
					OcrBdUseE(Image.FromStream(stream));
					stream.Close();
				}
				((ManualResetEvent)objEvent).Set();
			}
			catch
			{
				exit_thread();
			}
		}

		private void DoWork(object state)
		{
			/// <summary>
			/// 执行OCR识别工作，处理竖排文字识别任务
			/// 启动多个线程分别处理不同区域的图片OCR识别，等待所有识别完成后整合结果
			/// </summary>
			/// <param name="state">线程状态参数</param>
			
			// 创建5个ManualResetEvent用于线程同步
			var array = new ManualResetEvent[5];
			array[0] = new ManualResetEvent(false);
			// 启动线程处理第一部分图片OCR识别
			ThreadPool.QueueUserWorkItem(baidu_image_a, array[0]);
			array[1] = new ManualResetEvent(false);
			// 启动线程处理第二部分图片OCR识别
			ThreadPool.QueueUserWorkItem(baidu_image_b, array[1]);
			array[2] = new ManualResetEvent(false);
			// 启动线程处理第三部分图片OCR识别
			ThreadPool.QueueUserWorkItem(BdImageC, array[2]);
			array[3] = new ManualResetEvent(false);
			// 启动线程处理第四部分图片OCR识别
			ThreadPool.QueueUserWorkItem(BdImageD, array[3]);
			array[4] = new ManualResetEvent(false);
			// 启动线程处理第五部分图片OCR识别
			ThreadPool.QueueUserWorkItem(BdImageE, array[4]);
			WaitHandle[] waitHandles = array;
			// 等待所有OCR识别线程完成
			WaitHandle.WaitAll(waitHandles);
			// 整合所有OCR识别结果并去除多余换行符
			shupai_Right_txt = string.Concat(OCR_baidu_a, OCR_baidu_b, OCR_baidu_c, OCR_baidu_d, OCR_baidu_e).Replace("\r\n\r\n", "");
			var text = shupai_Right_txt.TrimEnd('\n').TrimEnd('\r').TrimEnd('\n');
			// 如果识别结果包含多行文本，则进行文本方向调整
			if (text.Split(Environment.NewLine.ToCharArray()).Length > 1)
			{
				var array2 = text.Split(new[]
				{
					"\r\n"
				}, StringSplitOptions.None);
				var str = "";
				// 反转文本行顺序以适应从右到左的阅读顺序
				for (var i = 0; i < array2.Length; i++)
				{
					str = str + array2[array2.Length - i - 1].Replace("\r", "").Replace("\n", "") + "\r\n";
				}
				shupai_Left_txt = str;
			}
			fmloading.FmlClose = "窗体已关闭";
			Invoke(new OcrThread(Main_OCR_Thread_last));
			try
			{
				// 清理临时图片文件
				DeleteFile("Data\\image_temp");
			}
			catch
			{
				exit_thread();
			}
			// 释放原图资源
			image_ori.Dispose();
		}

		/// <summary>
		/// 删除指定路径的文件或目录
		/// </summary>
		/// <param name="path">要删除的文件或目录路径</param>
		public void DeleteFile(string path)
		{
			if (File.GetAttributes(path) == FileAttributes.Directory)
			{
				Directory.Delete(path, true);
				return;
			}
			File.Delete(path);
		}

		/// <summary>
		/// 根据传入的数字参数，计算并设置image_num数组的值。
		/// 该函数主要用于将输入的数字分成5个区间，每个区间包含相对均匀的数量。
		/// </summary>
		/// <param name="num">需要处理的总数</param>
		public void bool_image_count(int num)
		{
			// 当数量大于等于5时，将数据分为5个区间
			if (num >= 5)
			{
				image_num = new int[num];
				// 根据余数的不同情况，分别计算各区间边界值
				if (num - num / 5 * 5 == 0)
				{
					image_num[0] = num / 5;
					image_num[1] = num / 5 * 2;
					image_num[2] = num / 5 * 3;
					image_num[3] = num / 5 * 4;
					image_num[4] = num;
				}
				if (num - num / 5 * 5 == 1)
				{
					image_num[0] = num / 5 + 1;
					image_num[1] = num / 5 * 2;
					image_num[2] = num / 5 * 3;
					image_num[3] = num / 5 * 4;
					image_num[4] = num;
				}
				if (num - num / 5 * 5 == 2)
				{
					image_num[0] = num / 5 + 1;
					image_num[1] = num / 5 * 2 + 1;
					image_num[2] = num / 5 * 3;
					image_num[3] = num / 5 * 4;
					image_num[4] = num;
				}
				if (num - num / 5 * 5 == 3)
				{
					image_num[0] = num / 5 + 1;
					image_num[1] = num / 5 * 2 + 1;
					image_num[2] = num / 5 * 3 + 1;
					image_num[3] = num / 5 * 4;
					image_num[4] = num;
				}
				if (num - num / 5 * 5 == 4)
				{
					image_num[0] = num / 5 + 1;
					image_num[1] = num / 5 * 2 + 1;
					image_num[2] = num / 5 * 3 + 1;
					image_num[3] = num / 5 * 4 + 1;
					image_num[4] = num;
				}
			}
			// 处理数量为4的特殊情况
			if (num == 4)
			{
				image_num = new int[5];
				image_num[0] = 1;
				image_num[1] = 2;
				image_num[2] = 3;
				image_num[3] = 4;
				image_num[4] = 0;
			}
			// 处理数量为3的特殊情况
			if (num == 3)
			{
				image_num = new int[5];
				image_num[0] = 1;
				image_num[1] = 2;
				image_num[2] = 3;
				image_num[3] = 0;
				image_num[4] = 0;
			}
			// 处理数量为2的特殊情况
			if (num == 2)
			{
				image_num = new int[5];
				image_num[0] = 1;
				image_num[1] = 2;
				image_num[2] = 0;
				image_num[3] = 0;
				image_num[4] = 0;
			}
			// 处理数量为1的特殊情况
			if (num == 1)
			{
				image_num = new int[5];
				image_num[0] = 1;
				image_num[1] = 0;
				image_num[2] = 0;
				image_num[3] = 0;
				image_num[4] = 0;
			}
			// 处理数量为0的特殊情况
			if (num == 0)
			{
				image_num = new int[5];
				image_num[0] = 0;
				image_num[1] = 0;
				image_num[2] = 0;
				image_num[3] = 0;
				image_num[4] = 0;
			}
		}

		/// <summary>
		/// 退出线程处理方法，用于停止当前的截图线程并恢复窗体状态
		/// </summary>
		private void exit_thread()
		{
			try
			{
				// 停止截图操作
				StaticValue.IsCapture = false;
				esc = "退出";
				// 关闭加载窗体
				fmloading.FmlClose = "窗体已关闭";
				// 终止截图线程
				esc_thread.Abort();
			}
			catch
			{
				//
			}
			// 恢复主窗体状态
			FormBorderStyle = FormBorderStyle.Sizable;
			Visible = true;
			Show();
			WindowState = FormWindowState.Normal;
			// 重新设置翻译文本的快捷键
			if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
			{
				var value = IniHelper.GetValue("快捷键", "翻译文本");
				var text = "None";
				var text2 = "F9";
				SetHotkey(text, text2, value, 205);
			}
			// 注销热键
			HelpWin32.UnregisterHotKey(Handle, 222);
		}

		/// <summary>
		/// 缩放图像到指定尺寸
		/// </summary>
		/// <param name="bitmap1">需要缩放的原始图像</param>
		/// <param name="destHeight">目标最小高度</param>
		/// <param name="destWidth">目标最小宽度</param>
		/// <returns>缩放后的图像</returns>
		private Bitmap ZoomImage(Bitmap bitmap1, int destHeight, int destWidth)
		{
			// 获取原始图像的宽度和高度
			var num = (double)bitmap1.Width;
			var num2 = (double)bitmap1.Height;
			// 如果宽度小于目标高度，则等比例放大
			if (num < destHeight)
			{
				while (num < destHeight)
				{
					num2 *= 1.1;
					num *= 1.1;
				}
			}
			// 如果高度小于目标宽度，则等比例放大
			if (num2 < destWidth)
			{
				while (num2 < destWidth)
				{
					num2 *= 1.1;
					num *= 1.1;
				}
			}
			// 转换为整数尺寸
			var width = (int)num;
			var height = (int)num2;
			// 创建新图像并绘制缩放后的图像
			var bitmap2 = new Bitmap(width, height);
			var graphics = Graphics.FromImage(bitmap2);
			graphics.DrawImage(bitmap1, 0, 0, width, height);
			graphics.Save();
			graphics.Dispose();
			return new Bitmap(bitmap2);
		}

		/// <summary>
		/// 从指定图像中提取矩形区域并返回新的位图
		/// </summary>
		/// <param name="pic">源图像</param>
		/// <param name="rect">要提取的矩形区域</param>
		/// <returns>提取出的矩形区域位图</returns>
		public Bitmap GetRect(Image pic, Rectangle rect)
		{
			var destRect = new Rectangle(0, 0, rect.Width, rect.Height);
			var bitmap = new Bitmap(destRect.Width, destRect.Height);
			var graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.FromArgb(0, 0, 0, 0));
			graphics.DrawImage(pic, destRect, rect, GraphicsUnit.Pixel);
			graphics.Dispose();
			return bitmap;
		}

		/// <summary>
		/// 从给定图像中提取指定区域的子图像，并保存为PNG文件
		/// </summary>
		/// <param name="buildPic">源图像，用于提取子图像</param>
		/// <param name="buildRects">矩形区域数组，指定要从源图像中提取的区域</param>
		/// <returns>提取出的子图像数组</returns>
		private Bitmap[] getSubPics(Image buildPic, Rectangle[] buildRects)
		{
			var array = new Bitmap[buildRects.Length];
			for (var i = 0; i < buildRects.Length; i++)
			{
				array[i] = GetRect(buildPic, buildRects[i]);
				var filename = IniHelper.GetValue("配置", "截图位置") + "\\" + ReFileName(IniHelper.GetValue("配置", "截图位置"), "图片.Png");
				array[i].Save(filename, ImageFormat.Png);
			}
			return array;
		}

		/// <summary>
		/// 从给定图像中提取指定区域的子图像，并对每个子图像执行OCR识别
		/// </summary>
		/// <param name="buildPic">源图像，用于提取子图像</param>
		/// <param name="buildRects">矩形区域数组，指定要从源图像中提取的区域</param>
		/// <returns>提取出的子图像数组</returns>
		private Bitmap[] getSubPics_ocr(Image buildPic, Rectangle[] buildRects)
		{
			var text = "";
			var array = new Bitmap[buildRects.Length];
			var text2 = "";
			for (var i = 0; i < buildRects.Length; i++)
			{
				// 提取指定区域的子图像
				array[i] = GetRect(buildPic, buildRects[i]);
				image_screen = array[i];
				var messageload = new Messageload();
				messageload.ShowDialog();
				if (messageload.DialogResult == DialogResult.OK)
				{
					// 根据选择的OCR接口执行相应的OCR识别方法
					if (interface_flag == "搜狗")
					{
						SougouOCR();
					}
					if (interface_flag == "腾讯")
					{
						OCR_Tencent();
					}
					if (interface_flag == "有道")
					{
						OCR_youdao();
					}
					if (interface_flag == "白描")
					{
						OCR_Baimiao();
					}
					if (interface_flag == "日语" || interface_flag == "中英" || interface_flag == "韩语")
					{
						OCR_baidu();
					}
					messageload.Dispose();
				}
				// 根据配置和段落标志处理识别结果文本
				if (IniHelper.GetValue("工具栏", "分栏") == "True")
				{
					if (paragraph)
					{
						text = text + "\r\n" + typeset_txt.Trim();
						text2 = text2 + "\r\n" + split_txt.Trim() + "\r\n";
					}
					else
					{
						text += typeset_txt.Trim();
						text2 = text2 + "\r\n" + split_txt.Trim() + "\r\n";
					}
				}
				else if (paragraph)
				{
					text = text + "\r\n" + typeset_txt.Trim() + "\r\n";
					text2 = text2 + "\r\n" + split_txt.Trim() + "\r\n";
				}
				else
				{
					text = text + typeset_txt.Trim() + "\r\n";
					text2 = text2 + "\r\n" + split_txt.Trim() + "\r\n";
				}
			}
			// 整理识别结果，去除多余的换行符
			typeset_txt = text.Replace("\r\n\r\n", "\r\n");
			split_txt = text2.Replace("\r\n\r\n", "\r\n");
			fmloading.FmlClose = "窗体已关闭";
			Invoke(new OcrThread(Main_OCR_Thread_last));
			return array;
		}

		/// <summary>
		/// 查找图像中围栏区域的边界框并进行处理
		/// 该函数使用OpenCV库对输入图像进行处理，识别围栏状结构，提取对应的边界框区域用于后续OCR识别
		/// </summary>
		/// <param name="src">输入的灰度图像，用于查找轮廓</param>
		/// <param name="draw">用于绘制结果的彩色图像</param>
		/// <returns>处理后的图像，其中围栏区域被标记</returns>
		public Image BoundingBox_fences(Image<Gray, byte> src, Image<Bgr, byte> draw)
		{
			Image result;
			using (var vectorOfVectorOfPoint = new VectorOfVectorOfPoint())
			{
				CvInvoke.FindContours(src, vectorOfVectorOfPoint, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
				Image image = draw.ToBitmap();
				var graphics = Graphics.FromImage(image);
				var size = vectorOfVectorOfPoint.Size;
				for (var i = 0; i < size; i++)
				{
					using (var vectorOfPoint = vectorOfVectorOfPoint[i])
					{
						var rectangle = CvInvoke.BoundingRectangle(vectorOfPoint);
						var x = rectangle.Location.X;
						var y = rectangle.Location.Y;
						var width = rectangle.Size.Width;
						var height = rectangle.Size.Height;
						graphics.FillRectangle(Brushes.White, x, 0, width, draw.Height);
					}
				}
				graphics.Dispose();
				var bitmap = new Bitmap(image.Width + 2, image.Height + 2);
				var graphics2 = Graphics.FromImage(bitmap);
				graphics2.DrawImage(image, 1, 1, image.Width, image.Height);
				graphics2.Save();
				graphics2.Dispose();
				image.Dispose();
				src.Dispose();
				result = bitmap;
			}
			return result;
		}

		/// <summary>
		/// 查找图像中围栏区域的边界框并进行处理
		/// 该函数使用OpenCV库对输入图像进行处理，识别围栏状结构，提取对应的边界框区域用于后续OCR识别
		/// </summary>
		/// <param name="bitmap">输入的位图图像</param>
		/// <returns>处理后的图像，其中围栏区域被标记</returns>
		public Image FindBoundingBoxFences(Bitmap bitmap)
		{
			var image = bitmap.ToImage<Bgr, byte>();
			var image2 = new Image<Gray, byte>(image.Width, image.Height);
			// 将彩色图像转换为灰度图像
			CvInvoke.CvtColor(image, image2, ColorConversion.Bgra2Gray);
			// 创建结构元素用于形态学操作
			var structuringElement = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(6, 20), new Point(1, 1));
			// 对图像进行腐蚀操作，增强围栏特征
			CvInvoke.Erode(image2, image2, structuringElement, new Point(0, 2), 1, BorderType.Reflect101, default(MCvScalar));
			// 应用阈值处理将图像二值化
			CvInvoke.Threshold(image2, image2, 100.0, 255.0, (ThresholdType)9);
			var image3 = image2.ToBitmap().ToImage<Gray, byte>();
			var draw = image3.Convert<Bgr, byte>();
			// 复制图像用于边缘检测
			var image4 = image3.Clone();
			// 使用Canny算法检测图像边缘
			CvInvoke.Canny(image3, image4, 255.0, 255.0, 5, true);
			// 查找并标记边界框区域
			var image5 = BoundingBox_fences(image4, draw);
            var image6 = ((Bitmap)image5).ToImage<Gray, byte>();
            // 对标记的区域进行进一步处理
            BoundingBox_fences_Up(image6);
			// 释放资源
			image.Dispose();
			image2.Dispose();
			image3.Dispose();
			image6.Dispose();
			return image5;
		}

		/// <summary>
		/// 查找图像中的轮廓并提取对应的边界框区域用于OCR识别
		/// 该函数使用OpenCV库来查找图像中的轮廓，并为每个轮廓创建边界矩形，然后调用OCR处理函数
		/// </summary>
		/// <param name="src">输入的灰度图像，用于查找轮廓</param>
		public void BoundingBox_fences_Up(Image<Gray, byte> src)
		{
			using (var vectorOfVectorOfPoint = new VectorOfVectorOfPoint())
			{
				// 查找图像中的所有轮廓
				CvInvoke.FindContours(src, vectorOfVectorOfPoint, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
				var size = vectorOfVectorOfPoint.Size;
				// 为每个轮廓创建对应的边界矩形
				var array = new Rectangle[size];
				// 遍历所有轮廓，获取边界矩形并按相反顺序存储
				for (var i = 0; i < size; i++)
				{
					using (var vectorOfPoint = vectorOfVectorOfPoint[i])
					{
						array[size - 1 - i] = CvInvoke.BoundingRectangle(vectorOfPoint);
					}
				}
				// 对提取的子图像区域进行OCR识别处理
				getSubPics_ocr(image_screen, array);
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
	}
}
