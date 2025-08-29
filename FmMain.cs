using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShareX.ScreenCaptureLib;
using TrOCR.Helper;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using Timer = System.Windows.Forms.Timer;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;
// ReSharper disable StringLiteralTypo

namespace TrOCR
{
	public sealed partial class FmMain
	{
// ====================================================================================================================
		// **构造函数与窗体事件**
		//
		// 负责窗体的初始化、加载、关闭以及核心窗口消息处理（WndProc）。
		// - FmMain(): 初始化组件、设置初始状态、加载配置、注册剪贴板查看器和热键。
		// - Load_Click(): 处理窗体加载事件，最小化并隐藏窗体。
		// - WndProc(): 窗口过程函数，用于处理系统消息，如热键、剪贴板变化、窗口状态改变等。
		// ====================================================================================================================
		#region 构造函数与窗体事件
		/// <summary>
		/// 初始化FmMain窗体实例，设置初始状态，加载配置并注册剪贴板监视器
		/// </summary>
		public FmMain()
		{
			// 初始化标志位
			set_merge = false;
			set_split = false;
			set_split = false;
			StaticValue.IsCapture = false;
			pinyin_flag = false;
			tranclick = false;
			
			// 初始化同步事件和图像列表
			are = new AutoResetEvent(false);
			imagelist = new List<Image>();
			
			// 从配置文件读取记录数目并初始化笔记数组
			StaticValue.NoteCount = Convert.ToInt32(IniHelper.GetValue("配置", "记录数目"));
			baidu_flags = "";
			esc = "";
			voice_count = 0;
			fmNote = new FmNote();
			pubnote = new string[StaticValue.NoteCount];
			for (var i = 0; i < StaticValue.NoteCount; i++)
			{
				pubnote[i] = "";
			}
			StaticValue.v_note = pubnote;
			StaticValue.mainHandle = Handle;
			
			// 设置字体大小
			Font = new Font(Font.Name, 9f / StaticValue.DpiFactor, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
			googleTranslate_txt = "";
			num_ok = 0;
			F_factor = Program.Factor;
			components = null;
			
			// 初始化组件和系统设置
			InitializeComponent();
			nextClipboardViewer = (IntPtr)HelpWin32.SetClipboardViewer((int)Handle);
			InitMinimize();
			InitConfig();
			
			// 设置窗口初始状态为最小化并隐藏
			WindowState = FormWindowState.Minimized;
			Visible = false;
			split_txt = "";
			MinimumSize = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
			speak_copy = false;
			
			// 初始化OCR功能
			OCR_foreach("");
		}

		/// <summary>
		/// 点击加载按钮时触发的事件处理函数，将窗体最小化并隐藏
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void Load_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
			Visible = false;
		}

		/// <summary>
		/// 重写Windows窗体的消息处理方法，用于处理发送到窗口的各种消息
		/// </summary>
		/// <param name="m">包含Windows消息信息的Message结构体引用</param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 953)
			{
				speaking = false;
			}
			if (m.Msg == 274 && (int)m.WParam == 61536)
			{
				WindowState = FormWindowState.Minimized;
				Visible = false;
				return;
			}
			if (m.Msg == 600 && (int)m.WParam == 725)
			{
				if (IniHelper.GetValue("工具栏", "顶置") == "True")
				{
					TopMost = true;
					return;
				}
				TopMost = false;
				return;
			}

			if (m.Msg == 786 && m.WParam.ToInt32() == 530 && RichBoxBody.Text != null)
			{
				p_note(RichBoxBody.Text);
				StaticValue.v_note = pubnote;
				if (fmNote.Created)
				{
					fmNote.TextNote = "";
				}
			}
			if (m.Msg == 786 && m.WParam.ToInt32() == 520)
			{
				fmNote.Show();
				fmNote.Focus();
				fmNote.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - fmNote.Width, Screen.PrimaryScreen.WorkingArea.Height - fmNote.Height);
				fmNote.WindowState = FormWindowState.Normal;
				return;
			}
			if (m.Msg == 786 && m.WParam.ToInt32() == 580)
			{
				HelpWin32.UnregisterHotKey(Handle, 205);
				change_QQ_screenshot = false;
				FormBorderStyle = FormBorderStyle.None;
				Hide();
				if (transtalate_fla == "开启")
				{
					form_width = Width / 2;
				}
				else
				{
					form_width = Width;
				}
				form_height = Height;
				minico.Visible = false;
				minico.Visible = true;
				menu.Close();
				menu_copy.Close();
				auto_fla = "开启";
				split_txt = "";
				// 避免不必要的文本更新
				if (RichBoxBody.Text != "***该区域未发现文本***")
				{
					RichBoxBody.Text = "***该区域未发现文本***";
				}
				RichBoxBody_T.Text = "";
				typeset_txt = "";
				transtalate_fla = "关闭";
				Trans_close.PerformClick();
				Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
				FormBorderStyle = FormBorderStyle.Sizable;
				StaticValue.IsCapture = true;
				image_screen = StaticValue.image_OCR;
				if (IniHelper.GetValue("工具栏", "分栏") == "True")
				{
					minico.Visible = true;
					thread = new Thread(ShowLoading);
					thread.Start();
					ts = new TimeSpan(DateTime.Now.Ticks);
					var image = image_screen;
					var image2 = new Bitmap(image.Width, image.Height);
					var graphics = Graphics.FromImage(image2);
					graphics.DrawImage(image, 0, 0, image.Width, image.Height);
					graphics.Save();
					graphics.Dispose();
					image_ori = image2;
					((Bitmap)FindBoundingBoxFences((Bitmap)image)).Save("Data\\分栏预览图.jpg");
				}
				else
				{
					minico.Visible = true;
					thread = new Thread(ShowLoading);
					thread.Start();
					ts = new TimeSpan(DateTime.Now.Ticks);
					var messageLoad = new Messageload();
					messageLoad.ShowDialog();
					if (messageLoad.DialogResult == DialogResult.OK)
					{
						esc_thread = new Thread(Main_OCR_Thread);
						esc_thread.Start();
					}
				}
			}
			if (m.Msg == 786 && m.WParam.ToInt32() == 590 && speak_copyb == "朗读")
			{
				TTS();
				return;
			}
			if (m.Msg == 786 && m.WParam.ToInt32() == 511)
			{
				MinimumSize = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
				transtalate_fla = "关闭";
				RichBoxBody.Dock = DockStyle.Fill;
				RichBoxBody_T.Visible = false;
				PictureBox1.Visible = false;
				RichBoxBody_T.Text = "";
				if (WindowState == FormWindowState.Maximized)
				{
					WindowState = FormWindowState.Normal;
				}
				Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
			}
			if (m.Msg == 786 && m.WParam.ToInt32() == 512)
			{
				TransClick();
			}
			if (m.Msg == 786 && m.WParam.ToInt32() == 518)
			{
				if (ActiveControl.Name == "htmlTextBoxBody")
				{
					htmltxt = RichBoxBody.Text;
				}
				if (ActiveControl.Name == "rich_trans")
				{
					htmltxt = RichBoxBody_T.Text;
				}
				if (htmltxt == "")
				{
					return;
				}
				TTS();
			}
			if (m.Msg == 161)
			{
				HelpWin32.SetForegroundWindow(Handle);
				base.WndProc(ref m);
				return;
			}
			if (m.Msg != 163)
			{
				if (m.Msg == 786 && m.WParam.ToInt32() == 222)
				{
					try
					{
						StaticValue.IsCapture = false;
						esc = "退出";
						fmloading.FmlClose = "窗体已关闭";
						esc_thread.Abort();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
					FormBorderStyle = FormBorderStyle.Sizable;
					Visible = true;
					Show();
					WindowState = FormWindowState.Normal;
					if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
					{
						var value = IniHelper.GetValue("快捷键", "翻译文本");
						var text = "None";
						var text2 = "F9";
						SetHotkey(text, text2, value, 205);
					}
					HelpWin32.UnregisterHotKey(Handle, 222);
				}
				if (m.Msg == 786 && m.WParam.ToInt32() == 200)
				{
					HelpWin32.UnregisterHotKey(Handle, 205);
					menu.Hide();
					RichBoxBody.Hide = "";
					RichBoxBody_T.Hide = "";
					MainOCRQuickScreenShots();
				}
				if (m.Msg == 786 && m.WParam.ToInt32() == 206)
				{
					if (!fmNote.Visible || Focused)
					{
						fmNote.Show();
						fmNote.WindowState = FormWindowState.Normal;
						fmNote.Visible = true;
					}
					else
					{
						fmNote.Hide();
						fmNote.WindowState = FormWindowState.Minimized;
						fmNote.Visible = false;
					}
				}
				if (m.Msg == 786 && m.WParam.ToInt32() == 235)
				{
					if (!Visible)
					{
						TopMost = true;
						Show();
						WindowState = FormWindowState.Normal;
						Visible = true;
						Thread.Sleep(100);
						if (IniHelper.GetValue("工具栏", "顶置") == "False")
						{
							TopMost = false;
							return;
						}
					}
					else
					{
						Hide();
						Visible = false;
					}
				}
				if (m.Msg == 786 && m.WParam.ToInt32() == 205)
				{
					翻译文本();
				}
				if (m.Msg == 786 && m.WParam.ToInt32() == 240)
				{
					trayInputTranslateClick(null, null);
				}
				base.WndProc(ref m);
				return;
			}
			if (transtalate_fla == "开启")
			{
				WindowState = FormWindowState.Normal;
				Size = new Size((int)font_base.Width * 23 * 2, (int)font_base.Height * 24);
				Location = (Point)new Size(Screen.PrimaryScreen.Bounds.Width / 2 - Screen.PrimaryScreen.Bounds.Width / 10 * 2, Screen.PrimaryScreen.Bounds.Height / 2 - Screen.PrimaryScreen.Bounds.Height / 6);
				return;
			}
			WindowState = FormWindowState.Normal;
			Location = (Point)new Size(Screen.PrimaryScreen.Bounds.Width / 2 - Screen.PrimaryScreen.Bounds.Width / 10, Screen.PrimaryScreen.Bounds.Height / 2 - Screen.PrimaryScreen.Bounds.Height / 6);
			Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
		}
		#endregion
// ====================================================================================================================
		// **托盘菜单事件**
		//
		// 管理系统托盘图标的右键菜单及其事件处理。
		// - InitMinimize(): 初始化托盘菜单，添加“输入翻译”、“显示”、“设置”、“更新”、“帮助”和“退出”等菜单项。
		// - trayInputTranslateClick(): 处理“输入翻译”菜单项的点击事件，重置并显示翻译窗口。
		// - trayShowClick(): 处理“显示”菜单项的点击事件，显示主窗口。
		// - trayExitClick(): 处理“退出”菜单项的点击事件，保存配置、释放资源并终止应用程序。
		// ====================================================================================================================
		#region 托盘菜单事件
		/// <summary>
		/// 初始化系统托盘菜单项
		/// 创建包含输入翻译、显示、设置、更新、帮助和退出等功能的托盘右键菜单
		/// </summary>
		public void InitMinimize()
		{
			try
			{
				var menuItems = new[]
				{
					new MenuItem("输入翻译", trayInputTranslateClick),
					new MenuItem("显示", trayShowClick),
					new MenuItem("设置", tray_Set_Click),
					new MenuItem("更新", tray_update_Click),
					new MenuItem("帮助", tray_help_Click),
					new MenuItem("退出", trayExitClick)
				};
				minico.ContextMenu = new ContextMenu(menuItems);
			}
			catch (Exception ex)
			{
				MessageBox.Show("InitMinimize()" + ex.Message);
			}
		}

		/// <summary>
		/// 托盘菜单"输入翻译"选项点击事件处理函数
		/// 重置翻译界面并显示主输入窗口，根据配置填充剪贴板内容
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void trayInputTranslateClick(object sender, EventArgs e)
		{
			// 1. 始终重置翻译界面，确保只显示主输入窗口
			transtalate_fla = "关闭";
			RichBoxBody.Dock = DockStyle.Fill;
			RichBoxBody_T.Visible = false;
			PictureBox1.Visible = false;
			RichBoxBody_T.Text = "";

			// 2. 恢复原始窗口大小
			if (WindowState == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Normal;
			}
			MinimumSize = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
			Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);

			// 3. 根据设置填充剪贴板内容
			if (IniHelper.GetValue("配置", "InputTranslateClipboard") == "True")
			{
				if (Clipboard.ContainsText())
				{
					RichBoxBody.Text = Clipboard.GetText();
				}
				else
				{
					RichBoxBody.Text = "";
				}
			}
			else
			{
				RichBoxBody.Text = "";
			}

			// 4. 显示并激活窗口
			Show();
			Activate();
			Visible = true;
			WindowState = FormWindowState.Normal;
			TopMost = IniHelper.GetValue("工具栏", "顶置") == "True";
		}

		/// <summary>
		/// 托盘菜单"显示"选项点击事件处理函数
		/// 显示并激活主窗口
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void trayShowClick(object sender, EventArgs e)
		{
			Show();
			Activate();
			Visible = true;
			WindowState = FormWindowState.Normal;
			TopMost = IniHelper.GetValue("工具栏", "顶置") == "True";
		}

		/// <summary>
		/// 托盘菜单"退出"选项点击事件处理函数
		/// 释放资源并退出应用程序
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void trayExitClick(object sender, EventArgs e)
		{
			minico.Dispose();
			saveIniFile();
			OcrHelper.Dispose();
			Process.GetCurrentProcess().Kill();
		}
		#endregion
// ====================================================================================================================
		// **主菜单事件**
		//
		// 处理主文本框（RichBoxBody）的右键上下文菜单事件。
		// - MainCopyClick(): 实现“复制”功能。
		// - Main_SelectAll_Click(): 实现“全选”功能。
		// - Main_paste_Click(): 实现“粘贴”功能。
		// ====================================================================================================================
		#region 主菜单事件
		/// <summary>
		/// 复制操作事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void MainCopyClick(object sender, EventArgs e)
		{
			RichBoxBody.Focus();
			RichBoxBody.richTextBox1.Copy();
		}

		/// <summary>
		/// 全选操作事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void Main_SelectAll_Click(object sender, EventArgs e)
		{
			RichBoxBody.Focus();
			RichBoxBody.richTextBox1.SelectAll();
		}

		/// <summary>
		/// 粘贴操作事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void Main_paste_Click(object sender, EventArgs e)
		{
			RichBoxBody.Focus();
			RichBoxBody.richTextBox1.Paste();
		}
		#endregion
// ====================================================================================================================
		// **OCR 引擎调用**
		//
		// 包含调用不同 OCR 服务（腾讯、微信、白描、百度等）的实现方法。
		// - OCR_Tencent(): 调用腾讯云 OCR API（通用版与高精度版）进行文字识别。
		// - OCR_WeChat(): 调用微信 OCR API 进行文字识别。
		// - OCR_Baimiao(): 调用白描 OCR API 进行文字识别。
		// - OCR_baidu(), OCR_baidu_accurate(): 调用百度标准版和高精度版OCR API。
		// - OCR_youdao(): 调用有道 OCR API 进行文字识别。
		// ====================================================================================================================
		#region OCR 引擎实现
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
		#endregion

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

// ====================================================================================================================
		// **OCR 接口切换 (事件)**
		//
		// 包含用户在界面上选择不同 OCR 引擎的事件处理程序。
		// 每个事件处理程序通过调用 OCR_foreach(string name) 方法来更新当前使用的 OCR 接口。
		// ====================================================================================================================
		#region OCR 接口切换 (事件)
		/// <summary>
		/// 搜狗OCR接口选择事件处理函数
		/// 切换当前OCR接口为搜狗OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_sougou_Click(object sender, EventArgs e)
		{
			OCR_foreach("搜狗");
		}

		/// <summary>
		/// 腾讯OCR接口选择事件处理函数
		/// 切换当前OCR接口为腾讯OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_tencent_Click(object sender, EventArgs e)
		{
			OCR_foreach("腾讯");
		}

		/// <summary>
		/// 腾讯高精度OCR接口选择事件处理函数
		/// 切换当前OCR接口为腾讯高精度OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_tencent_accurate_Click(object sender, EventArgs e)
		{
			OCR_foreach("腾讯-高精度");
		}

		/// <summary>
		/// 百度OCR接口选择事件处理函数
		/// 切换当前OCR接口为百度OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_Click(object sender, EventArgs e)
		{
		}


		/// <summary>
		/// 有道OCR接口选择事件处理函数
		/// 切换当前OCR接口为有道OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_youdao_Click(object sender, EventArgs e)
		{
			OCR_foreach("有道");
		}

		/// <summary>
		/// 微信OCR接口选择事件处理函数
		/// 切换当前OCR接口为微信OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_wechat_Click(object sender, EventArgs e)
		{
			OCR_foreach("微信");
		}

		/// <summary>
		/// 白描OCR接口选择事件处理函数
		/// 切换当前OCR接口为白描OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_baimiao_Click(object sender, EventArgs e)
		{
			OCR_foreach("白描");
		}

		/// <summary>
		/// 百度高精度OCR接口选择事件处理函数
		/// 切换当前OCR接口为百度高精度OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_accurate_Click(object sender, EventArgs e)
		{
			OCR_foreach("百度-高精度");
		}
		#endregion
// ====================================================================================================================
		// **文本操作与格式化**
		//
		// 提供中英文标点符号转换等文本处理功能。
		// - change_Chinese_Click(): 将文本中的英文标点符号转换为中文标点。
		// - change_English_Click(): 将文本中的中文标点符号转换为英文标点。
		// - punctuation_ch_en(): 具体的中文转英文标点实现。
		// ====================================================================================================================
		#region 文本操作与格式化
		/// <summary>
		/// 将文本中的标点符号转换为中文标点格式
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_Chinese_Click(object sender, EventArgs e)
		{
			language = "中文标点";
			// 只有当文本内容不为空时才执行标点符号转换
			if (typeset_txt != "")
			{
				RichBoxBody.Text = punctuation_en_ch_x(RichBoxBody.Text);
				RichBoxBody.Text = punctuation_quotation(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将文本中的标点符号转换为英文标点格式
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_English_Click(object sender, EventArgs e)
		{
			language = "英文标点";
			// 只有当文本内容不为空时才执行标点符号转换
			if (typeset_txt != "")
			{
				RichBoxBody.Text = punctuation_ch_en(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将中文标点符号转换为对应的英文标点符号
		/// </summary>
		/// <param name="text">需要转换的文本</param>
		/// <returns>转换后的文本</returns>
		public static string punctuation_ch_en(string text)
		{
			// 将字符串转换为字符数组以便逐个处理
			var array = text.ToCharArray();
			// 定义中文标点符号字符串
			var chinesePunctuation = "：。；，？！“”‘’【】（）";
			// 定义对应的英文标点符号字符串
			var englishPunctuation = ":.;,?!\"\"''[]()";
			
			// 遍历每个字符，查找是否为需要转换的中文标点
			for (var i = 0; i < array.Length; i++)
			{
				// 查找当前字符在中文标点字符串中的位置
				var num = chinesePunctuation.IndexOf(array[i]);
				// 如果找到了对应的中文标点，则替换为对应的英文标点
				if (num != -1)
				{
					array[i] = englishPunctuation[num];
				}
			}
			// 将处理后的字符数组重新组合成字符串并返回
			return new string(array);
		}
		#endregion
// ====================================================================================================================
		// **配置文件与初始化**
		//
		// 负责加载和保存应用程序的配置信息（config.ini）。
		// - saveIniFile(): 保存当前配置到 ini 文件。
		// - LoadTranslateConfig(): 从 ini 文件加载所有翻译服务的配置（源语言、目标语言、密钥等）。
		// - InitConfig(): 在程序启动时初始化所有配置，包括 OCR 接口、翻译接口、热键和各 API 密钥。
		// - tray_Set_Click(): 处理托盘菜单中的“设置”点击事件，打开设置窗口并重新加载所有配置。
		// ====================================================================================================================
		#region 配置文件与初始化
		/// <summary>
		/// 保存当前选择的OCR接口配置到配置文件中
		/// </summary>
		public void saveIniFile()
		{
			IniHelper.SetValue("配置", "接口", interface_flag);
		}

		/// <summary>
		/// 加载翻译配置信息
		/// 从配置文件中读取各翻译服务的配置信息，包括源语言、目标语言和密钥信息，并存储到静态变量中
		/// </summary>
		private void LoadTranslateConfig()
		{
			StaticValue.Translate_Configs.Clear();
			var services = new[] { "Google", "Baidu", "Tencent", "Bing", "Bing2", "Microsoft", "Yandex", "TencentInteractive", "Caiyun", "Caiyun2", "Volcano" };
			foreach (var service in services)
			{
				string section = "Translate_" + service;
				string source = IniHelper.GetValue(section, "Source");
				string target = IniHelper.GetValue(section, "Target");
				string appId = "";
				string apiKey = "";

				// 根据不同的服务读取不同的密钥名
				if (service == "Baidu")
				{
					appId = IniHelper.GetValue(section, "APP_ID");
					apiKey = IniHelper.GetValue(section, "APP_KEY");
				}
				else if (service == "Tencent")
				{
					appId = IniHelper.GetValue(section, "SecretId");
					apiKey = IniHelper.GetValue(section, "SecretKey");
				}
				else if (service == "Caiyun2")
				{
					// 彩云小译2使用Token作为密钥
					apiKey = IniHelper.GetValue(section, "Token");
				}
				else
				{
					// 其他服务的默认或通用密钥名
					appId = IniHelper.GetValue(section, "APP_ID");
					apiKey = IniHelper.GetValue(section, "API_KEY");
				}

				StaticValue.Translate_Configs[service] = new StaticValue.TranslateConfig
				{
					Source = (source == "发生错误" || string.IsNullOrEmpty(source)) ? "auto" : source,
					Target = (target == "发生错误" || string.IsNullOrEmpty(target)) ? "自动判断" : target,
					AppId = (appId == "发生错误") ? "" : appId,
					ApiKey = (apiKey == "发生错误") ? "" : apiKey
				};
			}
		}

		/// <summary>
		/// 初始化应用程序配置，包括OCR接口、翻译接口、快捷键和各种API密钥
		/// </summary>
		private void InitConfig()
		{
			// 初始化API菜单
			InitializeApiMenus();
			
			// 初始化OCR接口配置
			interface_flag = IniHelper.GetValue("配置", "接口");
			if (interface_flag == "发生错误")
			{
				IniHelper.SetValue("配置", "接口", "搜狗");
				OCR_foreach("搜狗");
			}
			else
			{
				OCR_foreach(interface_flag);
			}
			
			// 初始化翻译接口配置
			StaticValue.Translate_Current_API = IniHelper.GetValue("配置", "翻译接口");
			if (StaticValue.Translate_Current_API == "发生错误")
			{
				StaticValue.Translate_Current_API = "谷歌";
			}
			Trans_foreach(StaticValue.Translate_Current_API);
			LoadTranslateConfig();
			
			// 初始化快捷键配置
			var filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";
			if (IniHelper.GetValue("快捷键", "文字识别") != "请按下快捷键")
			{
				var value = IniHelper.GetValue("快捷键", "文字识别");
				var text = "None";
				var text2 = "F4";
				SetHotkey(text, text2, value, 200);
			}
			if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
			{
				var value2 = IniHelper.GetValue("快捷键", "翻译文本");
				var text3 = "None";
				var text4 = "F7";
				SetHotkey(text3, text4, value2, 205);
			}
			if (IniHelper.GetValue("快捷键", "记录界面") != "请按下快捷键")
			{
				var value3 = IniHelper.GetValue("快捷键", "记录界面");
				var text5 = "None";
				var text6 = "F8";
				SetHotkey(text5, text6, value3, 206);
			}
			if (IniHelper.GetValue("快捷键", "识别界面") != "请按下快捷键")
			{
				var value4 = IniHelper.GetValue("快捷键", "识别界面");
				var text7 = "None";
				var text8 = "F11";
				SetHotkey(text7, text8, value4, 235);
			}
			if (IniHelper.GetValue("快捷键", "输入翻译") != "请按下快捷键")
			{
				var value5 = IniHelper.GetValue("快捷键", "输入翻译");
				// 移除令人困惑的默认键 F1，因为SetHotkey函数会直接解析 value5 字符串
				SetHotkey("None", "", value5, 240);
			}
			
			// --- 加载OCR密钥 ---
			// 加载百度OCR密钥
			StaticValue.BD_API_ID = IniHelper.GetValue("密钥_百度", "secret_id");
			if (StaticValue.BD_API_ID == "发生错误")
			{
				StaticValue.BD_API_ID = "";
			}
			StaticValue.BD_API_KEY = IniHelper.GetValue("密钥_百度", "secret_key");
			if (StaticValue.BD_API_KEY == "发生错误")
			{
				StaticValue.BD_API_KEY = "";
			}
			StaticValue.BD_LANGUAGE = IniHelper.GetValue("密钥_百度", "language_code");
			if (StaticValue.BD_LANGUAGE == "发生错误")
			{
				StaticValue.BD_LANGUAGE = "CHN_ENG";
			}

			// 加载腾讯OCR密钥
			StaticValue.TX_API_ID = IniHelper.GetValue("密钥_腾讯", "secret_id");
			if (StaticValue.TX_API_ID == "发生错误")
			{
				StaticValue.TX_API_ID = "";
			}
			StaticValue.TX_API_KEY = IniHelper.GetValue("密钥_腾讯", "secret_key");
			if (StaticValue.TX_API_KEY == "发生错误")
			{
				StaticValue.TX_API_KEY = "";
			}
			StaticValue.TX_LANGUAGE = IniHelper.GetValue("密钥_腾讯", "language_code");
			if (StaticValue.TX_LANGUAGE == "发生错误")
			{
				StaticValue.TX_LANGUAGE = "zh";
			}

			// 加载腾讯高精度OCR密钥
			StaticValue.TX_ACCURATE_API_ID = IniHelper.GetValue("密钥_腾讯高精度", "secret_id");
			if (StaticValue.TX_ACCURATE_API_ID == "发生错误")
			{
				StaticValue.TX_ACCURATE_API_ID = "";
			}
			StaticValue.TX_ACCURATE_API_KEY = IniHelper.GetValue("密钥_腾讯高精度", "secret_key");
			if (StaticValue.TX_ACCURATE_API_KEY == "发生错误")
			{
				StaticValue.TX_ACCURATE_API_KEY = "";
			}
			StaticValue.TX_ACCURATE_LANGUAGE = IniHelper.GetValue("密钥_腾讯高精度", "language");
			if (StaticValue.TX_ACCURATE_LANGUAGE == "发生错误")
			{
				StaticValue.TX_ACCURATE_LANGUAGE = "auto";
			}

			// 加载百度高精度OCR密钥
			StaticValue.BD_ACCURATE_API_ID = IniHelper.GetValue("密钥_百度高精度", "secret_id");
			if (StaticValue.BD_ACCURATE_API_ID == "发生错误")
			 {
			    StaticValue.BD_ACCURATE_API_ID = "";
			 }
			StaticValue.BD_ACCURATE_API_KEY = IniHelper.GetValue("密钥_百度高精度", "secret_key");
			if (StaticValue.BD_ACCURATE_API_KEY == "发生错误")
			{
			    StaticValue.BD_ACCURATE_API_KEY = "";
			}
			StaticValue.BD_ACCURATE_LANGUAGE = IniHelper.GetValue("密钥_百度高精度", "language_code");
			if (StaticValue.BD_ACCURATE_LANGUAGE == "发生错误")
			{
			    StaticValue.BD_ACCURATE_LANGUAGE = "CHN_ENG";
			}
	
			// --- 加载白描OCR凭据 ---
			StaticValue.BaimiaoUsername = IniHelper.GetValue("密钥_白描", "username");
			if (StaticValue.BaimiaoUsername == "发生错误") StaticValue.BaimiaoUsername = "";

			StaticValue.BaimiaoPassword = IniHelper.GetValue("密钥_白描", "password");
			if (StaticValue.BaimiaoPassword == "发生错误") StaticValue.BaimiaoPassword = "";

			// 加载持久化的token信息
			string savedToken = IniHelper.GetValue("密钥_白描", "token");
			string savedExpiry = IniHelper.GetValue("密钥_白描", "token_expiry");
			string savedUsername = IniHelper.GetValue("密钥_白描", "token_username");
			string savedUuid = IniHelper.GetValue("密钥_白描", "device_uuid");

			if (!string.IsNullOrEmpty(savedToken) && savedToken != "发生错误" &&
			    !string.IsNullOrEmpty(savedUsername) && savedUsername != "发生错误" &&
			    savedUsername == StaticValue.BaimiaoUsername && // 确保token属于当前用户
			    DateTime.TryParse(savedExpiry, out DateTime expiry) && DateTime.Now < expiry)
			{
			 StaticValue.BaimiaoToken = savedToken;
			 StaticValue.BaimiaoTokenExpiry = expiry;
			}

			if (!string.IsNullOrEmpty(savedUuid) && savedUuid != "发生错误")
			{
			 StaticValue.BaimiaoDeviceUuid = savedUuid;
			}
		}

		/// <summary>
		/// 检查并替换文本中的中文冒号为英文冒号
		/// 当中文冒号前后都是英文字符或标点符号时，将其替换为英文冒号
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>处理后的文本，其中符合条件的中文冒号已被替换为英文冒号</returns>
		public static string check_ch_en(string text)
		{
			var array = text.ToCharArray();
			for (var i = 0; i < array.Length; i++)
			{
				var num = "：".IndexOf(array[i]);
				if (num != -1 && i - 1 >= 0 && i + 1 < array.Length && contain_en(array[i - 1].ToString()) && contain_en(array[i + 1].ToString()))
				{
					array[i] = ":"[num];
				}
				if (num != -1 && i - 1 >= 0 && i + 1 < array.Length && contain_en(array[i - 1].ToString()) && contain_punctuation(array[i + 1].ToString()))
				{
					array[i] = ":"[num];
				}
			}
			return new string(array);
		}

		/// <summary>
		/// 托盘设置菜单点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void tray_Set_Click(object sender, EventArgs e)
		{
			// 取消注册所有热键
			HelpWin32.UnregisterHotKey(Handle, 200);
			HelpWin32.UnregisterHotKey(Handle, 205);
			HelpWin32.UnregisterHotKey(Handle, 206);
			HelpWin32.UnregisterHotKey(Handle, 235);
			HelpWin32.UnregisterHotKey(Handle, 240);
			WindowState = FormWindowState.Minimized;
			var fmSetting = new FmSetting();
			fmSetting.TopMost = true;
			fmSetting.ShowDialog();
			if (fmSetting.DialogResult == DialogResult.OK)
			{
				// 在重新加载配置前，保存旧的百度密钥
				string oldBaiduApiId = StaticValue.BD_API_ID;
				string oldBaiduApiKey = StaticValue.BD_API_KEY;
				string oldBaiduAccurateApiId = StaticValue.BD_ACCURATE_API_ID;
				string oldBaiduAccurateApiKey = StaticValue.BD_ACCURATE_API_KEY;

				var filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";
				StaticValue.NoteCount = Convert.ToInt32(HelpWin32.IniFileHelper.GetValue("配置", "记录数目", filePath));
				pubnote = new string[StaticValue.NoteCount];
				for (var i = 0; i < StaticValue.NoteCount; i++)
				{
					pubnote[i] = "";
				}
				StaticValue.v_note = pubnote;
				fmNote.TextNoteChange = "";
				fmNote.Location = new Point(Screen.AllScreens[0].WorkingArea.Width - fmNote.Width, Screen.AllScreens[0].WorkingArea.Height - fmNote.Height);
				// 重新注册热键
				if (IniHelper.GetValue("快捷键", "文字识别") != "请按下快捷键")
				{
					var value = IniHelper.GetValue("快捷键", "文字识别");
					var text = "None";
					var text2 = "F4";
					SetHotkey(text, text2, value, 200);
				}
				if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
				{
					var value2 = IniHelper.GetValue("快捷键", "翻译文本");
					var text3 = "None";
					var text4 = "F9";
					SetHotkey(text3, text4, value2, 205);
				}
				if (IniHelper.GetValue("快捷键", "记录界面") != "请按下快捷键")
				{
					var value3 = IniHelper.GetValue("快捷键", "记录界面");
					var text5 = "None";
					var text6 = "F8";
					SetHotkey(text5, text6, value3, 206);
				}
				if (IniHelper.GetValue("快捷键", "识别界面") != "请按下快捷键")
				{
					var value4 = IniHelper.GetValue("快捷键", "识别界面");
					var text7 = "None";
					var text8 = "F11";
					SetHotkey(text7, text8, value4, 235);
				}
				if (IniHelper.GetValue("快捷键", "输入翻译") != "请按下快捷键")
				{
					var value5 = IniHelper.GetValue("快捷键", "输入翻译");
					// 移除令人困惑的默认键 F1，因为SetHotkey函数会直接解析 value5 字符串
					SetHotkey("None", "", value5, 240);
				}
				// --- 重新加载所有API密钥 ---
				// --- 加载OCR密钥 ---
				StaticValue.BD_API_ID = IniHelper.GetValue("密钥_百度", "secret_id");
				if (StaticValue.BD_API_ID == "发生错误")
				{
					StaticValue.BD_API_ID = "";
				}
				StaticValue.BD_API_KEY = IniHelper.GetValue("密钥_百度", "secret_key");
				if (StaticValue.BD_API_KEY == "发生错误")
				{
					StaticValue.BD_API_KEY = "";
				}
				// 如果百度标准版密钥发生变化，清除旧的Token缓存
				if (StaticValue.BD_API_ID != oldBaiduApiId || StaticValue.BD_API_KEY != oldBaiduApiKey)
				{
					BaiduOcrHelper.ClearAccessTokenCache(false);
				}
				StaticValue.BD_LANGUAGE = IniHelper.GetValue("密钥_百度", "language_code");
				if (StaticValue.BD_LANGUAGE == "发生错误")
				{
					StaticValue.BD_LANGUAGE = "CHN_ENG";
				}
	
				StaticValue.TX_API_ID = IniHelper.GetValue("密钥_腾讯", "secret_id");
				if (StaticValue.TX_API_ID == "发生错误")
				{
					StaticValue.TX_API_ID = "";
				}
				StaticValue.TX_API_KEY = IniHelper.GetValue("密钥_腾讯", "secret_key");
				if (StaticValue.TX_API_KEY == "发生错误")
				{
					StaticValue.TX_API_KEY = "";
				}
				StaticValue.TX_LANGUAGE = IniHelper.GetValue("密钥_腾讯", "language_code");
				if (StaticValue.TX_LANGUAGE == "发生错误")
				{
					StaticValue.TX_LANGUAGE = "zh";
				}
	
				StaticValue.TX_ACCURATE_API_ID = IniHelper.GetValue("密钥_腾讯高精度", "secret_id");
				if (StaticValue.TX_ACCURATE_API_ID == "发生错误")
				{
					StaticValue.TX_ACCURATE_API_ID = "";
				}
				StaticValue.TX_ACCURATE_API_KEY = IniHelper.GetValue("密钥_腾讯高精度", "secret_key");
				if (StaticValue.TX_ACCURATE_API_KEY == "发生错误")
				{
					StaticValue.TX_ACCURATE_API_KEY = "";
				}
				StaticValue.TX_ACCURATE_LANGUAGE = IniHelper.GetValue("密钥_腾讯高精度", "language");
				if (StaticValue.TX_ACCURATE_LANGUAGE == "发生错误")
				{
					StaticValue.TX_ACCURATE_LANGUAGE = "auto";
				}

				StaticValue.BD_ACCURATE_API_ID = IniHelper.GetValue("密钥_百度高精度", "secret_id");
				if (StaticValue.BD_ACCURATE_API_ID == "发生错误")
				{
				    StaticValue.BD_ACCURATE_API_ID = "";
				}
				StaticValue.BD_ACCURATE_API_KEY = IniHelper.GetValue("密钥_百度高精度", "secret_key");
				if (StaticValue.BD_ACCURATE_API_KEY == "发生错误")
				{
				    StaticValue.BD_ACCURATE_API_KEY = "";
				}
				// 如果百度高精度版密钥发生变化，清除旧的Token缓存
				if (StaticValue.BD_ACCURATE_API_ID != oldBaiduAccurateApiId || StaticValue.BD_ACCURATE_API_KEY != oldBaiduAccurateApiKey)
				{
					BaiduOcrHelper.ClearAccessTokenCache(true);
				}
				StaticValue.BD_ACCURATE_LANGUAGE = IniHelper.GetValue("密钥_百度高精度", "language_code");
				if (StaticValue.BD_ACCURATE_LANGUAGE == "发生错误")
				{
				    StaticValue.BD_ACCURATE_LANGUAGE = "CHN_ENG";
				}
	
				// --- 重新加载白描OCR凭据 ---
				string newBaimiaoUsername = IniHelper.GetValue("密钥_白描", "username");
				if (newBaimiaoUsername == "发生错误") newBaimiaoUsername = "";

				// 如果用户名发生变化，则清除旧的token缓存
				if (StaticValue.BaimiaoUsername != newBaimiaoUsername)
				{
				 OcrHelper.ClearBaimiaoTokenCache();
				}
				StaticValue.BaimiaoUsername = newBaimiaoUsername;

				StaticValue.BaimiaoPassword = IniHelper.GetValue("密钥_白描", "password");
				if (StaticValue.BaimiaoPassword == "发生错误") StaticValue.BaimiaoPassword = "";

				// 重新加载持久化的token信息
				string savedToken = IniHelper.GetValue("密钥_白描", "token");
				string savedExpiry = IniHelper.GetValue("密钥_白描", "token_expiry");
				string savedUsername = IniHelper.GetValue("密钥_白描", "token_username");
				if (!string.IsNullOrEmpty(savedToken) && savedToken != "发生错误" &&
				    !string.IsNullOrEmpty(savedUsername) && savedUsername != "发生错误" &&
				    savedUsername == StaticValue.BaimiaoUsername &&
				    DateTime.TryParse(savedExpiry, out DateTime expiry) && DateTime.Now < expiry)
				{
				 StaticValue.BaimiaoToken = savedToken;
				 StaticValue.BaimiaoTokenExpiry = expiry;
				}
				else
				{
				 StaticValue.BaimiaoToken = null;
				 StaticValue.BaimiaoTokenExpiry = DateTime.MinValue;
				}

				// 重新加载UUID
				string savedUuid = IniHelper.GetValue("密钥_白描", "device_uuid");
				StaticValue.BaimiaoDeviceUuid = (savedUuid == "发生错误" || string.IsNullOrEmpty(savedUuid)) ? null : savedUuid;

				// 重新加载翻译配置
				StaticValue.Translate_Current_API = IniHelper.GetValue("配置", "翻译接口");
				if (StaticValue.Translate_Current_API == "发生错误")
				{
					StaticValue.Translate_Current_API = "谷歌";
				}
				LoadTranslateConfig();
				InitializeApiMenus();
			}
		}

		/// <summary>
		/// 判断字符串是否为纯数字
		/// </summary>
		/// <param name="str">待检测的字符串</param>
		/// <returns>如果是纯数字返回true，否则返回false</returns>
		public static bool IsNum(string str)
		{
			return Regex.IsMatch(str, "^\\d+$");
		}

		/// <summary>
		/// 判断字符串是否为标点符号
		/// </summary>
		/// <param name="text">待检测的字符串</param>
		/// <returns>如果在预定义标点符号列表中返回true，否则返回false</returns>
		public bool own_punctuation(string text)
		{
			return ",;，、<>《》()-（）.。".IndexOf(text, StringComparison.Ordinal) != -1;
		}

		/// <summary>
		/// 处理标点符号与文字间的空格
		/// </summary>
		/// <param name="text">待处理的文本</param>
		/// <returns>处理后的文本</returns>
		public static string punctuation_Del_space(string text)
		{
			var pattern = "(?<=.)([^\\*]+)(?=.)";
			string result;
			if (Regex.Match(text, pattern).ToString().IndexOf(" ", StringComparison.Ordinal) >= 0)
			{
				// 在特定标点符号后添加空格
				text = Regex.Replace(text, "(?<=[\\p{P}*])([a-zA-Z])(?=[a-zA-Z])", " $1");
				// 清理文本末尾空格并处理特殊符号组合
				text = text.TrimEnd(null).Replace("- ", "-").Replace("_ ", "_").Replace("( ", "(").Replace("/ ", "/").Replace("\" ", "\"");
				result = text;
			}
			else
			{
				result = text;
			}
			return result;
		}

		/// <summary>
		/// 判断字符串是否包含中文字符
		/// </summary>
		/// <param name="str">待检测的字符串</param>
		/// <returns>如果包含中文字符返回true，否则返回false</returns>
		public static bool contain_ch(string str)
		{
			return Regex.IsMatch(str, "[\\u4e00-\\u9fa5]");
		}
#endregion

// ====================================================================================================================
		// **翻译功能**
		//
		// 实现了文本翻译的核心逻辑和界面交互。
		// - TransClick(): 启动翻译模式的入口，调整窗口布局以显示原文和译文两个文本框。
		// - Form_Resize(): 处理窗口大小变化事件，确保翻译界面布局正确。
		// - Trans_copy_Click(), Trans_paste_Click(), Trans_SelectAll_Click(): 翻译文本框的右键菜单功能。
		// - trans_Calculate(): 异步执行翻译的核心方法，根据当前选择的翻译服务和语言设置调用相应的翻译API。
		// - Trans_close_Click(): 关闭翻译模式，恢复原始窗口布局。
		// ====================================================================================================================
#region 翻译功能
		/// <summary>
		/// 启动翻译功能，调整窗体和控件布局以显示翻译界面
		/// </summary>
		public void TransClick()
		{
			typeset_txt = RichBoxBody.Text;
			RichBoxBody_T.Visible = true;
			WindowState = FormWindowState.Normal;
			transtalate_fla = "开启";
			RichBoxBody.Dock = DockStyle.None;
			RichBoxBody_T.Dock = DockStyle.None;
			RichBoxBody_T.BorderStyle = BorderStyle.Fixed3D;
			RichBoxBody_T.Text = "";
			RichBoxBody.Focus();
			if (num_ok == 0)
			{
				RichBoxBody.Size = new Size(ClientRectangle.Width, ClientRectangle.Height);
				Size = new Size(RichBoxBody.Width * 2, RichBoxBody.Height);
				RichBoxBody_T.Size = new Size(RichBoxBody.Width, RichBoxBody.Height);
				RichBoxBody_T.Location = (Point)new Size(RichBoxBody.Width, 0);
				RichBoxBody_T.Name = "rich_trans";
				RichBoxBody_T.TabIndex = 1;
				RichBoxBody_T.Text_flag = "我是翻译文本框";
			}
			num_ok++;
			PictureBox1.Visible = true;
			PictureBox1.BringToFront();
			MinimumSize = new Size((int)font_base.Width * 23 * 2, (int)font_base.Height * 24);
			Size = new Size((int)font_base.Width * 23 * 2, (int)font_base.Height * 24);
			CheckForIllegalCrossThreadCalls = false;
			trans_Calculate();
		}

		/// <summary>
		/// 处理窗体大小调整事件，当翻译功能开启时调整文本框大小和位置
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void Form_Resize(object sender, EventArgs e)
		{
			// 当RichBoxBody未设置停靠样式时调整大小
			if (RichBoxBody.Dock != DockStyle.Fill)
			{
				RichBoxBody.Size = new Size(ClientRectangle.Width / 2, ClientRectangle.Height);
				RichBoxBody_T.Size = new Size(RichBoxBody.Width, ClientRectangle.Height);
				RichBoxBody_T.Location = (Point)new Size(RichBoxBody.Width, 0);
			}
		}

		/// <summary>
		/// 处理翻译文本框复制操作的事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_copy_Click(object sender, EventArgs e)
		{
			RichBoxBody_T.Focus();
			RichBoxBody_T.richTextBox1.Copy();
		}

		/// <summary>
		/// 处理翻译文本框粘贴操作的事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_paste_Click(object sender, EventArgs e)
		{
			RichBoxBody_T.Focus();
			RichBoxBody_T.richTextBox1.Paste();
		}

		/// <summary>
		/// 处理翻译文本框全选操作的事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_SelectAll_Click(object sender, EventArgs e)
		{
			RichBoxBody_T.Focus();
			RichBoxBody_T.richTextBox1.SelectAll();
		}

		/// <summary>
		/// 执行翻译计算操作，根据配置和文本内容进行翻译或拼音转换
		/// </summary>
		public async void trans_Calculate()
		{
			if (pinyin_flag)
			{
				// 如果设置了拼音标志，则将文本转换为拼音
				googleTranslate_txt = HanToPinyin.GetFullPinyin(typeset_txt);
			}
			else if (string.IsNullOrWhiteSpace(typeset_txt))
			{
				// 如果文本为空或只包含空白字符，则翻译结果也为空
				googleTranslate_txt = "";
			}
			else
			{
				// 获取当前使用的翻译服务
				string transService = StaticValue.Translate_Current_API;
				string sectionName;
				// 根据翻译服务名称确定配置节名称
				switch (transService)
				{
					case "谷歌":
						sectionName = "Google";
						break;
					case "百度":
						sectionName = "Baidu";
						break;
					case "腾讯":
						sectionName = "Tencent";
						break;
					case "腾讯交互翻译":
						sectionName = "TencentInteractive";
						break;
					case "彩云小译":
						sectionName = "Caiyun";
						break;
					case "彩云小译2":
						sectionName = "Caiyun2";
						break;
					case "火山翻译":
						sectionName = "Volcano";
						break;
					default:
						sectionName = transService;
						break;
				}

				// 尝试获取翻译配置，如果不存在则使用默认配置
				if (!StaticValue.Translate_Configs.TryGetValue(sectionName, out var config))
				{
					config = new StaticValue.TranslateConfig { Source = "auto", Target = "自动判断" };
				}

				string toLang;
				string fromLang = config.Source;

				// 根据目标语言配置自动判断需要翻译成的语言
				if (config.Target == "自动判断")
				{
					toLang = "en"; // 默认翻译为英文
					if (StaticValue.ZH2EN)
					{
						//中文和英文互译逻辑
						// 中文转英文逻辑：比较中英文字符数量确定源语言
						if (ch_count(typeset_txt.Trim()) > en_count(typeset_txt.Trim()) || (en_count(typeset_txt.Trim()) == 1 && ch_count(typeset_txt.Trim()) == 1))
						{
							toLang = "en";
						}
						else
						{
							toLang = "zh-CN";
						}
					}
					else if (StaticValue.ZH2JP)
					{
						// 中文和日文互译逻辑
						// 统计中文字符和日文字符数量来判断主要语言
						string textToCheck = typeset_txt.Trim();
						int chineseCount = ch_count(textToCheck);
						// 对于日文，我们需要统计假名的数量，因为汉字在中日文都存在
						int japaneseKanaCount = 0;
						foreach (char c in textToCheck)
						{
							// 统计平假名 (U+3040-U+309F) 和片假名 (U+30A0-U+30FF)
							if ((c >= '\u3040' && c <= '\u309F') || (c >= '\u30A0' && c <= '\u30FF'))
							{
								japaneseKanaCount++;
							}
						}
						
						// 如果日文假名多于中文字符，说明是日文文本，翻译到中文
						// 否则翻译到日文
						if (japaneseKanaCount > 0 && japaneseKanaCount >= chineseCount / 2)
						{
							// 有相当数量的假名，判断为日文，翻译到中文
							toLang = "zh-CN";
						}
						else
						{
							// 中文字符占主导，翻译到日文
							toLang = "ja";
						}
					}
					else if (StaticValue.ZH2KO)
					{
						// 中文和韩文互译逻辑
						if (contain_kor(typeset_txt.Trim()))
						{
							toLang = "zh-CN";
						}
						else
						{
							toLang = "ko";
						}
					}
				}
				else
				{
					// 使用配置中指定的目标语言
					toLang = config.Target;
				}

				// 百度和腾讯翻译服务需要特殊处理语言代码
				if (transService == "百度")
				{
					if (fromLang == "zh-CN") fromLang = "zh";
					if (toLang == "zh-CN") toLang = "zh";
					if (fromLang == "ja") fromLang = "jp";
					if (toLang == "ja") toLang = "jp";
					if (fromLang == "ko") fromLang = "kor";
					if (toLang == "ko") toLang = "kor";
				}
				if (transService == "腾讯")
				{
					if (fromLang == "zh-CN") fromLang = "zh";
					if (toLang == "zh-CN") toLang = "zh";
				}

				// 根据翻译服务调用相应的翻译方法
				switch (transService)
				{
					case "谷歌":
						googleTranslate_txt = await GTranslateHelper.TranslateAsync(typeset_txt, fromLang, toLang, "google");
						break;
					case "Bing":
						googleTranslate_txt = await BingTranslator.TranslateAsync(typeset_txt, fromLang, toLang);
						break;
					case "Bing2":
					case "BingNew":
						googleTranslate_txt = await BingTranslator2.TranslateAsync(typeset_txt, fromLang, toLang);
						break;
					case "Microsoft":
						googleTranslate_txt = await GTranslateHelper.TranslateAsync(typeset_txt, fromLang, toLang, "microsoft");
						break;
					case "Yandex":
						googleTranslate_txt = await GTranslateHelper.TranslateAsync(typeset_txt, fromLang, toLang, "yandex");
						break;
					case "百度":
						googleTranslate_txt = TranslateBaidu(typeset_txt, fromLang, toLang, config.AppId, config.ApiKey);
						break;
					case "腾讯":
						googleTranslate_txt = Translate_Tencent(typeset_txt, fromLang, toLang, config.AppId, config.ApiKey);
						break;
					case "腾讯交互翻译":
						googleTranslate_txt = await TencentTranslator.TranslateAsync(typeset_txt, fromLang, toLang);
						break;
					case "彩云小译":
						googleTranslate_txt = await CaiyunTranslator.TranslateAsync(typeset_txt, fromLang, toLang);
						break;
					case "彩云小译2":
						if (string.IsNullOrEmpty(config.ApiKey))
							googleTranslate_txt = "[彩云小译2]：未配置Token";
						else
							googleTranslate_txt = await CaiyunTranslator2.TranslateAsync(typeset_txt, fromLang, toLang, config.ApiKey);
						break;
					case "火山翻译":
						googleTranslate_txt = await VolcanoTranslator.TranslateAsync(typeset_txt, fromLang, toLang);
						break;
					default:
						googleTranslate_txt = await GTranslateHelper.TranslateAsync(typeset_txt, fromLang, toLang, "google");
						break;
				}
			}

			// 隐藏进度图片并将其置于底层
			PictureBox1.Visible = false;
			PictureBox1.SendToBack();
			// 调用翻译完成后的处理方法
			Invoke(new Translate(translate_child));
			// 重置拼音标志
			pinyin_flag = false;
		}

		/// <summary>
		/// 关闭翻译功能的事件处理函数
		/// 当用户点击关闭翻译功能时，此函数将恢复主窗口到原始状态并隐藏翻译相关控件
		/// </summary>
		/// <param name="sender">触发事件的对象</param>
		/// <param name="e">事件参数</param>
		public void Trans_close_Click(object sender, EventArgs e)
		{
			MinimumSize = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
			transtalate_fla = "关闭";
			RichBoxBody.Dock = DockStyle.Fill;
			RichBoxBody_T.Visible = false;
			PictureBox1.Visible = false;
			RichBoxBody_T.Text = "";
			if (WindowState == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Normal;
			}
			Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
		}

		/// <summary>
		/// 将googleTranslate_txt的内容赋值给RichBoxBody_T控件，并清空googleTranslate_txt变量
		/// </summary>
		private void translate_child()
		{
			RichBoxBody_T.Text = googleTranslate_txt;
			googleTranslate_txt = "";
		}

		/// <summary>
		/// 显示加载窗口并运行应用程序消息循环
		/// </summary>
		private void ShowLoading()
		{
			try
			{
				fmloading = new FmLoading();
				Application.Run(fmloading);
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{
				thread.Abort();
			}
		}

		/// <summary>
		/// 检查文本中是否包含指定子字符串
		/// </summary>
		/// <param name="text">要检查的完整文本</param>
		/// <param name="subStr">要查找的子字符串</param>
		/// <returns>如果text包含subStr则返回true，否则返回false</returns>
		public bool contain(string text, string subStr)
		{
			return text.Contains(subStr);
		}

		/// <summary>
		/// 检查字符串中是否包含英文字母
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果字符串包含英文字母则返回true，否则返回false</returns>
		public static bool contain_en(string str)
		{
			return Regex.IsMatch(str, "[a-zA-Z]");
		}

		/// <summary>
		/// 检查字符串是否包含标点符号（根据中英文使用不同的标点符号集）
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果字符串包含标点符号则返回true，否则返回false</returns>
		public static bool punctuation_has_punctuation(string str)
		{
			var pattern = contain_ch(str) ? "[\\；\\，\\。\\！\\？]" : "[\\;\\,\\.\\!\\?]";
			return Regex.IsMatch(str, pattern);
		}

		/// <summary>
		/// 处理字符串中的引号符号，将英文引号替换为中文引号
		/// </summary>
		/// <param name="pStr">需要处理的字符串</param>
		/// <returns>处理后的字符串，引号已替换为中文引号</returns>
		private string punctuation_quotation(string pStr)
		{
			pStr = pStr.Replace("“", "\"").Replace("”", "\"");
			var array = pStr.Split('"');
			var text = "";
			for (var i = 1; i <= array.Length; i++)
			{
				if (i % 2 == 0)
				{
					text = text + array[i - 1] + "”";
				}
				else
				{
					text = text + array[i - 1] + "“";
				}
			}
			return text.Substring(0, text.Length - 1);
		}

		/// <summary>
		/// 检查字符串是否包含英文标点符号
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果字符串包含英文标点符号则返回true，否则返回false</returns>
		public static bool HasenPunctuation(string str)
		{
			var pattern = "[\\;\\,\\.\\!\\?]";
			return Regex.IsMatch(str, pattern);
		}

		/// <summary>
		/// 删除文本中的多余空格
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>删除多余空格后的文本</returns>
		public static string Del_Space(string text)
		{
			text = Regex.Replace(text, "([\\p{P}]+)", "**&&**$1**&&**");
			text = text.TrimEnd(null).Replace(" **&&**", "").Replace("**&&** ", "").Replace("**&&**", "");
			return text;
		}
#endregion

// ====================================================================================================================
		// **文本朗读 (TTS)**
		//
		// 实现了文本到语音的转换功能。
		// - TTS(): 启动一个新的线程来处理 TTS 请求。
		// - TTS_thread(): 在后台线程中获取要朗读的文本，检测语言，并从百度 TTS 服务下载音频数据。
		// - TTS_child(): 在 UI 线程中播放下载的音频。
		// ====================================================================================================================
#region 文本朗读 (TTS)
		/// <summary>
		/// 启动TTS文本朗读功能，在新线程中执行TTS_thread方法
		/// </summary>
		public void TTS()
		{
			new Thread(TTS_thread).Start();
		}


		/// <summary>
		/// TTS文本朗读线程函数，负责获取文本内容、检测语言、下载语音数据并调用播放方法
		/// </summary>
		public void TTS_thread()
		{
			try
			{
				// 清理文本内容，移除特殊标记
				var text = htmltxt.Replace("***", "");
				// 检测文本语言
				var lang = CommonHelper.LangDetect(text);
				//                var url = "https://fanyi.baidu.com/gettts?lan=" + lang + "&text=" + HttpUtility.UrlEncode(text) +
				//                                   "&vol=9&per=0&spd=6&pit=4&source=web&ctp=1";
				// 获取百度TTS语音合成URL
				var url = TranslateHelper.BdTts(text, lang, 5);
				// 下载语音数据
				ttsData = new WebClient().DownloadData(url);
				// 根据条件决定调用哪个播放方法
				if (speak_copyb == "朗读" || voice_count == 0)
				{
					Invoke(new Translate(Speak_child));
					speak_copyb = "";
				}
				else
				{
					Invoke(new Translate(TTS_child));
				}
				voice_count++;
			}
			catch (Exception)
			{
				MessageBox.Show("文本过长，请使用右键菜单中的选中朗读！", "提醒");
			}
		}

		/// <summary>
		/// TTS文本朗读播放函数，在UI线程中执行，负责播放已下载的语音数据
		/// </summary>
		public void TTS_child()
		{
			// 检查主文本框或翻译文本框是否有内容
			if (RichBoxBody.Text != null || RichBoxBody_T.Text != "")
			{
				// 如果正在播放，则关闭播放并返回
				if (speaking)
				{
					HelpWin32.mciSendString("close media", null, 0, IntPtr.Zero);
					speaking = false;
					return;
				}
				// 获取系统临时目录路径
				var tempPath = Path.GetTempPath();
				// 构造临时音频文件路径
				var text = tempPath + "\\声音.mp3";
				try
				{
					// 将语音数据写入临时文件
					File.WriteAllBytes(text, ttsData);
				}
				catch
				{
					// 如果写入失败，尝试使用另一个文件名
					text = tempPath + "\\声音1.mp3";
					File.WriteAllBytes(text, ttsData);
				}
				// 播放音频文件
				PlaySong(text);
				// 设置播放状态为正在播放
				speaking = true;
			}
		}
#endregion
// ====================================================================================================================
		// **截图与图像处理**
		//
		// 包含了屏幕截图、二维码扫描、图像处理和文件操作等辅助功能。
		// - CreateParams: 设置窗口样式，启用无边框窗口特性。
		// - ScanQRCode(): 扫描屏幕截图中的二维码并返回解码后的文本。
		// - SearchSelText(): 使用默认浏览器搜索选中的文本。
		// - tray_update_Click(): 检查应用程序更新。
		// - contain_jap(), contain_kor(): 判断字符串是否包含日文或韩文字符。
		// - ReFileName(), GetUniqueFileName(): 生成唯一的文件名以避免覆盖。
		// - PlaySong(): 播放音频文件。
		// ====================================================================================================================
#region 截图与图像处理
		/// <summary>
		/// 创建窗口参数，设置窗口的扩展样式
		/// </summary>
		/// <value>窗口创建参数</value>
		protected override CreateParams CreateParams
		{
			get
			{
				var createParams = base.CreateParams;
				createParams.ExStyle |= 134217728;
				return createParams;
			}
		}


		/// <summary>
		/// 将CookieCollection对象转换为字符串格式的Cookie
		/// </summary>
		/// <param name="cookie">要转换的Cookie集合</param>
		/// <returns>字符串格式的Cookie，格式为"name=value;"</returns>
		public static string CookieCollectionToStrCookie(CookieCollection cookie)
		{
			string result;
			if (cookie == null)
			{
				result = string.Empty;
			}
			else
			{
				var text = string.Empty;
				foreach (var obj in cookie)
				{
					var cookie2 = (Cookie)obj;
					text += string.Format("{0}={1};", cookie2.Name, cookie2.Value);
				}
				result = text;
			}
			return result;
		}

		/// <summary>
		/// 扫描屏幕图像中的二维码内容
		/// </summary>
		/// <returns>二维码中的文本内容，如果扫描失败则返回空字符串</returns>
		public string ScanQRCode()
		{
			var result = "";
			try
			{
				var image = new BinaryBitmap(new HybridBinarizer(new BitmapLuminanceSource((Bitmap)image_screen)));
				var result2 = new QRCodeReader().decode(image);
				if (result2 != null)
				{
					result = result2.Text;
				}
			}
			catch
			{
			}
			return result;
		}

		/// <summary>
		/// 使用百度搜索RichBoxBody控件中选中的文本
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void SearchSelText(object sender, EventArgs e)
		{
			Process.Start("https://www.baidu.com/s?wd=" + RichBoxBody.SelectText);
		}

		/// <summary>
		/// 点击托盘更新菜单项时检查程序更新
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void tray_update_Click(object sender, EventArgs e)
		{
			Program.CheckUpdate();
		}

		/// <summary>
		/// 检查字符串是否包含日文字符
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果包含日文字符则返回true，否则返回false</returns>
		public static bool contain_jap(string str)
		{
			return Regex.IsMatch(str, "[\\u3040-\\u309F]") || Regex.IsMatch(str, "[\\u30A0-\\u30FF]");
		}

		/// <summary>
		/// 检查字符串是否包含韩文字符
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果包含韩文字符则返回true，否则返回false</returns>
		public static bool contain_kor(string str)
		{
			return Regex.IsMatch(str, "[\\uac00-\\ud7ff]");
		}

		/// <summary>
		/// 删除字符串中的中文字符
		/// </summary>
		/// <param name="str">要处理的字符串</param>
		/// <returns>删除中文字符后的字符串</returns>
		public static string Del_ch(string str)
		{
			var text = str;
			if (Regex.IsMatch(str, "[\\u4e00-\\u9fa5]"))
			{
				text = string.Empty;
				var array = str.ToCharArray();
				for (var i = 0; i < array.Length; i++)
				{
					if (array[i] < '一' || array[i] > '龥')
					{
						text += array[i].ToString();
					}
				}
			}
			return text;
		}

		/// <summary>
		/// 移除字符串中的标点符号并转换为大写
		/// </summary>
		/// <param name="hexData">要处理的字符串</param>
		/// <returns>移除标点符号并转为大写的字符串</returns>
		private static string replaceStr(string hexData)
		{
			return Regex.Replace(hexData, "[\\p{P}+~$`^=|<>～｀＄＾＋＝｜＜＞￥×┊ ]", "").ToUpper();
		}

		/// <summary>
		/// 移除字符串中的各种标点符号
		/// </summary>
		/// <param name="str">要处理的字符串</param>
		/// <returns>移除标点符号后的字符串</returns>
		public static string RemovePunctuation(string str)
		{
			str = str.Replace(",", "").Replace("，", "").Replace(".", "").Replace("。", "").Replace("!", "").Replace("！", "").Replace("?", "").Replace("？", "").Replace(":", "").Replace("：", "").Replace(";", "").Replace("；", "").Replace("～", "").Replace("-", "").Replace("_", "").Replace("——", "").Replace("—", "").Replace("--", "").Replace("【", "").Replace("】", "").Replace("\\", "").Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "").Replace("#", "").Replace("$", "").Replace("、", "").Replace("‘", "").Replace("’", "").Replace("“", "").Replace("”", "");
			return str;
		}

		/// <summary>
		/// 获取唯一的文件名，如果文件已存在则在文件名后添加序号
		/// </summary>
		/// <param name="fullName">完整文件路径</param>
		/// <returns>唯一文件名</returns>
		public static string GetUniqueFileName(string fullName)
		{
			string result;
			if (!File.Exists(fullName))
			{
				result = fullName;
			}
			else
			{
				var directoryName = Path.GetDirectoryName(fullName);
				var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullName);
				var extension = Path.GetExtension(fullName);
				var num = 1;
				string text;
				do
				{
					text = Path.Combine(directoryName, string.Format("{0}[{1}].{2}", fileNameWithoutExtension, num++, extension));
				}
				while (File.Exists(text));
				result = text;
			}
			return result;
		}

		/// <summary>
		/// 重命名文件，在文件名后添加序号以避免重复
		/// </summary>
		/// <param name="strFolderPath">文件夹路径</param>
		/// <param name="strFileName">原始文件名</param>
		/// <returns>新的文件名</returns>
		public static string ReFileName(string strFolderPath, string strFileName)
		{
			var text = strFolderPath + "\\" + strFileName;
			var startIndex = text.LastIndexOf('.');
			text = text.Insert(startIndex, "_{0}");
			var num = 1;
			var path = string.Format(text, num);
			while (File.Exists(path))
			{
				path = string.Format(text, num);
				num++;
			}
			return Path.GetFileName(path);
		}

		/// <summary>
		/// 播放指定的音频文件
		/// </summary>
		/// <param name="file">音频文件路径</param>
		public void PlaySong(string file)
		{
			HelpWin32.mciSendString("close media", null, 0, IntPtr.Zero);
			HelpWin32.mciSendString("open \"" + file + "\" type mpegvideo alias media", null, 0, IntPtr.Zero);
			HelpWin32.mciSendString("play media notify", null, 0, Handle);
		}
#endregion

// ====================================================================================================================
		// **右键菜单 - 朗读事件**
		//
		// 处理原文和译文文本框中通过右键菜单触发的朗读功能。
		// - Main_Voice_Click(): 获取原文框中选中的文本并触发朗读。
		// - Trans_Voice_Click(): 获取译文框中选中的文本并触发朗读。
		// - Speak_child(): 在 UI 线程中播放朗读音频。
		// ====================================================================================================================
#region 右键菜单 - 朗读事件
		/// <summary>
		/// 主文本框语音朗读点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Main_Voice_Click(object sender, EventArgs e)
		{
			RichBoxBody.Focus();
			speak_copyb = "朗读";
			htmltxt = RichBoxBody.SelectText;
			HelpWin32.SendMessage(Handle, 786, 590);
		}

		/// <summary>
		/// 翻译文本框语音朗读点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_Voice_Click(object sender, EventArgs e)
		{
			RichBoxBody_T.Focus();
			speak_copyb = "朗读";
			htmltxt = RichBoxBody_T.SelectText;
			HelpWin32.SendMessage(Handle, 786, 590);
		}

		/// <summary>
		/// 执行文本语音朗读功能
		/// </summary>
		public void Speak_child()
		{
			// 检查主文本框或翻译文本框是否有内容
			if (RichBoxBody.Text != null || RichBoxBody_T.Text != "")
			{
				var tempPath = Path.GetTempPath();
				var text = tempPath + "\\声音.mp3";
				try
				{
					File.WriteAllBytes(text, ttsData);
				}
				catch
				{
					text = tempPath + "\\声音1.mp3";
					File.WriteAllBytes(text, ttsData);
				}
				PlaySong(text);
				speaking = true;
			}
		}

		/// <summary>
		/// 将字符串转换为简体中文
		/// </summary>
		/// <param name="source">需要转换的源字符串</param>
		/// <returns>转换后的简体中文字符串</returns>
		public static string ToSimplified(string source)
		{
			var text = new string(' ', source.Length);
			HelpWin32.LCMapString(2048, 33554432, source, source.Length, text, source.Length);
			return text;
		}

		/// <summary>
		/// 将字符串转换为繁体中文
		/// </summary>
		/// <param name="source">需要转换的源字符串</param>
		/// <returns>转换后的繁体中文字符串</returns>
		public static string ToTraditional(string source)
		{
			var text = new string(' ', source.Length);
			HelpWin32.LCMapString(2048, 67108864, source, source.Length, text, source.Length);
			return text;
		}
#endregion

// ====================================================================================================================
		// **右键菜单 - 文本转换**
		//
		// 提供文本的大小写转换和简繁体转换功能。
		// - change_zh_tra_Click(): 将文本转换为繁体。
		// - change_tra_zh_Click(): 将文本转换为简体。
		// - change_str_Upper_Click(): 将文本转换为大写。
		// - change_Upper_str_Click(): 将文本转换为小写。
		// ====================================================================================================================
#region 右键菜单 - 文本转换
		/// <summary>
		/// 将文本框中的文本转换为繁体中文
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_zh_tra_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = ToTraditional(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将文本框中的文本转换为简体中文
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_tra_zh_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = ToSimplified(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将文本框中的文本转换为大写
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_str_Upper_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = RichBoxBody.Text.ToUpper();
			}
		}

		/// <summary>
		/// 将文本框中的文本转换为小写
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_Upper_str_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = RichBoxBody.Text.ToLower();
			}
		}
#endregion

// ====================================================================================================================
		// **热键管理**
		//
		// 负责解析快捷键字符串并注册系统范围的全局热键。
		// - SetHotkey(): 核心方法，调用 Win32 API (RegisterHotKey) 来注册一个全局热键，
		//              允许用户在任何地方通过快捷键触发程序功能（如截图、翻译）。
		// ====================================================================================================================
#region 热键管理
		/// <summary>
		/// 解析快捷键字符串并返回修饰键和按键数组
		/// </summary>
		/// <param name="text">修饰键（如Ctrl、Alt等）</param>
		/// <param name="text2">按键（如A、B、F1等）</param>
		/// <param name="value">完整的快捷键字符串，格式如"Ctrl+Alt+A"或"Alt+A"</param>
		/// <returns>包含修饰键和按键的字符串数组</returns>
		public string[] hotkey(string text, string text2, string value)
		{
			var array = (value + "+").Split('+');
			if (array.Length == 3)
			{
				text = array[0];
				text2 = array[1];
			}
			if (array.Length == 2)
			{
				text = "None";
				text2 = value;
			}
			return new[]
			{
				text,
				text2
			};
		}

		/// <summary>
		/// 设置并注册全局热键
		/// </summary>
		/// <param name="text">修饰键（如Ctrl、Alt等）</param>
		/// <param name="text2">按键（如A、B、F1等）</param>
		/// <param name="value">完整的快捷键字符串，格式如"Ctrl+Alt+A"或"Alt+A"</param>
		/// <param name="flag">热键标识符</param>
		public void SetHotkey(string text, string text2, string value, int flag)
		{
			var array = (value + "+").Split('+');
			// 解析快捷键字符串，根据格式确定修饰键和按键
			if (array.Length == 3)
			{
				text = array[0];
				text2 = array[1];
			}
			if (array.Length == 2)
			{
				text = "None";
				text2 = value;
			}
			var array2 = new[]
			{
				text,
				text2
			};
			// 尝试注册热键，如果失败则提示用户
			if (!HelpWin32.RegisterHotKey(Handle, flag, (HelpWin32.KeyModifiers)Enum.Parse(typeof(HelpWin32.KeyModifiers), array2[0].Trim()), (Keys)Enum.Parse(typeof(Keys), array2[1].Trim())))
			{
				CommonHelper.ShowHelpMsg("快捷键冲突，请更换！");
			}
			HelpWin32.RegisterHotKey(Handle, flag, (HelpWin32.KeyModifiers)Enum.Parse(typeof(HelpWin32.KeyModifiers), array2[0].Trim()), (Keys)Enum.Parse(typeof(Keys), array2[1].Trim()));
		}
#endregion
// ====================================================================================================================
		// **辅助方法与工具**
		//
		// 包含一些通用的辅助方法，例如记录管理和剪贴板操作。
		// - p_note(): 将新的识别结果添加到历史记录队列中。
		// - GetTextFromClipboard(): 从系统剪贴板安全地获取文本内容，处理线程问题。
		// ====================================================================================================================
#region 辅助方法与工具
		/// <summary>
		/// 将指定的字符串添加到历史记录队列中
		/// </summary>
		/// <param name="a">要添加到历史记录的字符串</param>
		public void p_note(string a)
		{
			// 循环更新历史记录数组，实现队列效果
			for (var i = 0; i < StaticValue.NoteCount; i++)
			{
				if (i == StaticValue.NoteCount - 1)
				{
					pubnote[StaticValue.NoteCount - 1] = a;
				}
				else
				{
					pubnote[i] = pubnote[i + 1];
				}
			}
		}

		/// <summary>
		/// 从系统剪贴板安全地获取文本内容
		/// </summary>
		/// <returns>剪贴板中的文本内容，如果为空则返回null</returns>
		private string GetTextFromClipboard()
		{
			// 检查当前线程的单元状态，确保在STA模式下执行剪贴板操作
			if (Thread.CurrentThread.GetApartmentState() > ApartmentState.STA)
			{
				var thread = new Thread(delegate()
				{
					SendKeys.SendWait("^c");
					SendKeys.Flush();
				});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
			}
			else
			{
				SendKeys.SendWait("^c");
				SendKeys.Flush();
			}
			var text = Clipboard.GetText();
			text = (string.IsNullOrWhiteSpace(text) ? null : text);
			// 如果获取到文本内容，则清空剪贴板
			if (text != null)
			{
				Clipboard.Clear();
			}
			return text;
		}
#endregion

// ====================================================================================================================
		// **截图与核心OCR流程**
		//
		// 这是应用程序的核心功能所在，集成了截图、图像处理和OCR识别的完整流程。
		// - MainOCRQuickScreenShots(): 启动截图功能，隐藏主窗口，调用 ShareX 库进行区域捕捉。
		//                             根据用户的操作（如截图、贴图、保存、多区域选择等）执行不同逻辑。
		// - Main_OCR_Thread(): 截图完成后，在此线程中执行 OCR 识别。
		//                      它会先尝试扫描二维码，然后根据当前选择的 OCR 接口调用相应的识别方法。
		// - Main_OCR_Thread_last(): OCR 识别完成后，在 UI 线程中更新界面，显示识别结果，处理自动翻译、
		//                           分段合并等后续操作，并重新显示主窗口。
		// - SougouOCR(): 调用搜狗OCR。
		// - BdTableOCR(), OCR_ali_table(): 处理表格识别。
		// - select_image(), FindBundingBox(): 使用 Emgu.CV进行图像处理，用于竖排文字的识别。
		// ====================================================================================================================
#region 截图与核心OCR流程
		/// <summary>
		/// 主OCR快速截图功能
		/// 启动截图功能，隐藏主窗口，调用ShareX库进行区域捕捉
		/// 根据用户的操作（如截图、贴图、保存、多区域选择等）执行不同逻辑
		/// </summary>
		public void MainOCRQuickScreenShots()
		{
			// 如果正在截图则直接返回
			if (StaticValue.IsCapture) return;
			try
			{
				// 隐藏主窗口并准备截图
				change_QQ_screenshot = false;
				FormBorderStyle = FormBorderStyle.None;
				Visible = false;
				Thread.Sleep(100);
				
				// 根据翻译窗口状态设置窗体宽度
				if (transtalate_fla == "开启")
				{
					form_width = Width / 2;
				}
				else
				{
					form_width = Width;
				}
				
				// 初始化相关变量
				shupai_Right_txt = "";
				shupai_Left_txt = "";
				form_height = Height;
				minico.Visible = false;
				minico.Visible = true;
				menu.Close();
				menu_copy.Close();
				auto_fla = "开启";
				split_txt = "";
				
				// 避免不必要的文本更新
				if (RichBoxBody.Text != "***该区域未发现文本***")
				{
					RichBoxBody.Text = "***该区域未发现文本***";
				}
				RichBoxBody_T.Text = "";
				typeset_txt = "";
				transtalate_fla = "关闭";
				
				// 如果工具栏翻译功能关闭，则执行关闭翻译操作
				if (IniHelper.GetValue("工具栏", "翻译") == "False")
				{
					Trans_close.PerformClick();
				}
				
				// 重置窗口大小和边框样式
				Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
				FormBorderStyle = FormBorderStyle.Sizable;
				
				// 设置截图状态为进行中
				StaticValue.IsCapture = true;
				
				// 调用截图功能获取屏幕图像
				image_screen = RegionCaptureTasks.GetRegionImage_Mo(new RegionCaptureOptions
				{
					ShowMagnifier = false,
					UseSquareMagnifier = false,
					MagnifierPixelCount = 15,
					MagnifierPixelSize = 10
				}, out var modeFlag, out var point, out var buildRects);
				
				// 如果是高级截图模式，则启动高级截图窗体
				if (modeFlag == "高级截图")
				{
					var mode = RegionCaptureMode.Annotation;
					var options = new RegionCaptureOptions();
					using (var regionCaptureForm = new RegionCaptureForm(mode, options))
					{
						regionCaptureForm.Image_get = false;
						regionCaptureForm.Prepare(image_screen);
						regionCaptureForm.ShowDialog();
						image_screen = null;
						image_screen = regionCaptureForm.GetResultImage();
						modeFlag = regionCaptureForm.Mode_flag;
					}
				}
				
				// 注册ESC键作为退出截图的热键
				HelpWin32.RegisterHotKey(Handle, 222, HelpWin32.KeyModifiers.None, Keys.Escape);
				
				// 根据截图后的操作模式执行相应处理
				switch (modeFlag)
				{
					case "贴图":
						{
							// 贴图模式：创建贴图窗体并显示
							var locationPoint = new Point(point.X, point.Y);
							new FmScreenPaste(image_screen, locationPoint).Show();
							if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
							{
								var value = IniHelper.GetValue("快捷键", "翻译文本");
								var text = "None";
								var text2 = "F9";
								SetHotkey(text, text2, value, 205);
							}
							HelpWin32.UnregisterHotKey(Handle, 222);
							StaticValue.IsCapture = false;
							break;
						}
					case "区域多选" when image_screen == null:
						{
							// 区域多选但未选择区域：恢复热键并退出截图状态
							if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
							{
								var value2 = IniHelper.GetValue("快捷键", "翻译文本");
								var text3 = "None";
								var text4 = "F9";
								SetHotkey(text3, text4, value2, 205);
							}
							HelpWin32.UnregisterHotKey(Handle, 222);
							StaticValue.IsCapture = false;
							break;
						}
					case "区域多选":
						// 区域多选：启动加载线程并处理多个区域的OCR
						minico.Visible = true;
						thread = new Thread(ShowLoading);
						thread.Start();
						ts = new TimeSpan(DateTime.Now.Ticks);
						getSubPics_ocr(image_screen, buildRects);
						break;
					case "取色":
						{
							// 取色模式：恢复热键并显示颜色已复制提示
							if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
							{
								var value3 = IniHelper.GetValue("快捷键", "翻译文本");
								var text5 = "None";
								var text6 = "F9";
								SetHotkey(text5, text6, value3, 205);
							}
							HelpWin32.UnregisterHotKey(Handle, 222);
							StaticValue.IsCapture = false;
							CommonHelper.ShowHelpMsg("已复制颜色");
							break;
						}
					default:
						{
							if (image_screen == null)
							{
								// 未获取到图像：恢复热键并退出截图状态
								if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
								{
									var value4 = IniHelper.GetValue("快捷键", "翻译文本");
									var text7 = "None";
									var text8 = "F9";
									SetHotkey(text7, text8, value4, 205);
								}
								HelpWin32.UnregisterHotKey(Handle, 222);
								StaticValue.IsCapture = false;
							}
							else
							{
								// 根据不同模式标志设置相应变量
								if (modeFlag == "百度")
								{
									baidu_flags = "百度";
								}
								if (modeFlag == "拆分")
								{
									set_merge = false;
									set_split = true;
								}
								if (modeFlag == "合并")
								{
									set_merge = true;
									set_split = false;
								}
								if (modeFlag == "截图")
								{
									// 截图模式：将图像复制到剪贴板
									Clipboard.SetImage(image_screen);
									if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
									{
										var value5 = IniHelper.GetValue("快捷键", "翻译文本");
										var text9 = "None";
										var text10 = "F9";
										SetHotkey(text9, text10, value5, 205);
									}
									HelpWin32.UnregisterHotKey(Handle, 222);
									StaticValue.IsCapture = false;
									if (IniHelper.GetValue("截图音效", "粘贴板") == "True")
									{
										PlaySong(IniHelper.GetValue("截图音效", "音效路径"));
									}
									CommonHelper.ShowHelpMsg("已复制截图");
								}
								else if (modeFlag == "自动保存" && IniHelper.GetValue("配置", "自动保存") == "True")
								{
									// 自动保存模式：将图像保存到指定位置
									var filename = IniHelper.GetValue("配置", "截图位置") + "\\" + ReFileName(IniHelper.GetValue("配置", "截图位置"), "图片.Png");
									image_screen.Save(filename, ImageFormat.Png);
									StaticValue.IsCapture = false;
									if (IniHelper.GetValue("截图音效", "自动保存") == "True")
									{
										PlaySong(IniHelper.GetValue("截图音效", "音效路径"));
									}
									CommonHelper.ShowHelpMsg("已保存图片");
								}
								else if (modeFlag == "多区域自动保存" && IniHelper.GetValue("配置", "自动保存") == "True")
								{
									// 多区域自动保存模式：保存多个区域的图像
									getSubPics(image_screen, buildRects);
									StaticValue.IsCapture = false;
									if (IniHelper.GetValue("截图音效", "自动保存") == "True")
									{
										PlaySong(IniHelper.GetValue("截图音效", "音效路径"));
									}
									CommonHelper.ShowHelpMsg("已保存图片");
								}
								else if (modeFlag == "保存")
								{
									// 保存模式：弹出保存对话框让用户选择保存位置和格式
									var saveFileDialog = new SaveFileDialog();
									saveFileDialog.Filter = "png图片(*.png)|*.png|jpg图片(*.jpg)|*.jpg|bmp图片(*.bmp)|*.bmp";
									saveFileDialog.AddExtension = false;
									saveFileDialog.FileName = string.Concat("tianruo_", DateTime.Now.Year.ToString(), "-", DateTime.Now.Month.ToString(), "-", DateTime.Now.Day.ToString(), "-", DateTime.Now.Ticks.ToString());
									saveFileDialog.Title = "保存图片";
									saveFileDialog.FilterIndex = 1;
									saveFileDialog.RestoreDirectory = true;
									if (saveFileDialog.ShowDialog() == DialogResult.OK)
									{
										var extension = Path.GetExtension(saveFileDialog.FileName);
										if (extension.Equals(".jpg"))
										{
											image_screen.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
										}
										if (extension.Equals(".png"))
										{
											image_screen.Save(saveFileDialog.FileName, ImageFormat.Png);
										}
										if (extension.Equals(".bmp"))
										{
											image_screen.Save(saveFileDialog.FileName, ImageFormat.Bmp);
										}
									}
									if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
									{
										var value6 = IniHelper.GetValue("快捷键", "翻译文本");
										var text11 = "None";
										var text12 = "F9";
										SetHotkey(text11, text12, value6, 205);
									}
									HelpWin32.UnregisterHotKey(Handle, 222);
									StaticValue.IsCapture = false;
								}
								else if (image_screen != null)
								{
									// OCR识别模式：根据工具栏设置决定是否进行分栏处理
									if (IniHelper.GetValue("工具栏", "分栏") == "True")
									{
										minico.Visible = true;
										thread = new Thread(ShowLoading);
										thread.Start();
										ts = new TimeSpan(DateTime.Now.Ticks);
										var image = image_screen;
										var graphics = Graphics.FromImage(new Bitmap(image.Width, image.Height));
										graphics.DrawImage(image, 0, 0, image.Width, image.Height);
										graphics.Save();
										graphics.Dispose();
										((Bitmap)FindBoundingBoxFences((Bitmap)image)).Save("Data\\分栏预览图.jpg");
										image.Dispose();
										image_screen.Dispose();
									}
									else
									{
										// 启动OCR识别线程
										minico.Visible = true;
										thread = new Thread(ShowLoading);
										thread.Start();
										ts = new TimeSpan(DateTime.Now.Ticks);
										var messageload = new Messageload();
										messageload.ShowDialog();
										if (messageload.DialogResult == DialogResult.OK)
										{
											esc_thread = new Thread(Main_OCR_Thread);
											esc_thread.Start();
										}
									}
								}
							}

							break;
						}
				}
			}
			catch
			{
				// 发生异常时确保退出截图状态
				StaticValue.IsCapture = false;
			}
		}

		/// <summary>
		/// OCR主线程函数，根据不同的接口标识调用相应的OCR识别方法，并处理识别结果
		/// </summary>
		public void Main_OCR_Thread()
		{
			// 优先检查是否为二维码，如果是则直接返回二维码内容
			if (ScanQRCode() != "")
			{
				typeset_txt = ScanQRCode();
				RichBoxBody.Text = typeset_txt;
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			// 根据interface_flag选择不同的OCR接口进行识别
			if (interface_flag == "搜狗")
			{
				SougouOCR();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "腾讯" || interface_flag == "腾讯-高精度")
			{
				OCR_Tencent();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "有道")
			{
				OCR_youdao();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "微信")
			{
				OCR_WeChat();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "白描")
			{
				OCR_Baimiao();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "公式")
			{
				OCR_Math();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "百度表格")
			{
				BdTableOCR();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_table));
				return;
			}
			if (interface_flag == "阿里表格")
			{
				OCR_ali_table();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_table));
				return;
			}
			if (interface_flag == "日语" || interface_flag == "中英" || interface_flag == "韩语")
			{
				OCR_baidu();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
			}
			if (interface_flag == "百度-高精度")
			{
				OCR_baidu_accurate();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
			}
			// 处理竖排文字识别（从左向右或从右向左）
			if (interface_flag == "从左向右" || interface_flag == "从右向左")
			{
				shupai_Right_txt = "";
				var image = image_screen;
				var bitmap = new Bitmap(image.Width, image.Height);
				var graphics = Graphics.FromImage(bitmap);
				graphics.DrawImage(image, 0, 0, image.Width, image.Height);
				graphics.Save();
				graphics.Dispose();
				image_ori = bitmap;
				var image2 = new Image<Gray, byte>(bitmap);
				var image3 = new Image<Gray, byte>((Bitmap)FindBundingBox(image2.ToBitmap()));
				var draw = image3.Convert<Bgr, byte>();
				var image4 = image3.Clone();
				CvInvoke.Canny(image3, image4, 0.0, 0.0, 5, true);
				select_image(image4, draw);
				bitmap.Dispose();
				image2.Dispose();
				image3.Dispose();
			}
			image_screen.Dispose();
			GC.Collect();
		}

		/// <summary>
		/// OCR识别完成后的处理函数，负责处理识别结果、格式化文本、更新界面和执行后续操作
		/// </summary>
		public void Main_OCR_Thread_last()
		{
			image_screen.Dispose();
			StaticValue.IsCapture = false;
			var text = typeset_txt;
			text = check_str(text);
			split_txt = check_str(split_txt);
			// 如果文本没有标点符号，则使用拆分后的文本
			if (!punctuation_has_punctuation(text))
			{
				text = split_txt;
			}
			// 如果包含中文，则删除空格
			if (contain_ch(text.Trim()))
			{
				text = Del_Space(text);
			}
			if (text != "")
			{
				// 直接设置Text属性，因为AdvRichTextBox.Text的setter已经优化
				RichBoxBody.Text = text;
			}
			StaticValue.v_Split = split_txt;
			// 处理文本拆分选项
			if (bool.Parse(IniHelper.GetValue("工具栏", "拆分")) || set_split)
			{
				set_split = false;
				RichBoxBody.Text = split_txt;
			}
			// 处理文本合并选项
			if (bool.Parse(IniHelper.GetValue("工具栏", "合并")) || set_merge)
			{
				set_merge = false;
				RichBoxBody.Text = text.Replace("\n", "").Replace("\r", "");
			}
			// 计算识别耗时
			var timeSpan = new TimeSpan(DateTime.Now.Ticks);
			var timeSpan2 = timeSpan.Subtract(ts).Duration();
			var str = string.Concat(new[]
			{
				timeSpan2.Seconds.ToString(),
				".",
				Convert.ToInt32(timeSpan2.TotalMilliseconds).ToString(),
				"秒"
			});
			// 处理笔记相关功能
			if (RichBoxBody.Text != null)
			{
				p_note(RichBoxBody.Text);
				StaticValue.v_note = pubnote;
				if (fmNote.Created)
				{
					fmNote.TextNote = "";
				}
			}
			// 设置窗体是否置顶
			if (StaticValue.v_topmost)
			{
				TopMost = true;
			}
			else
			{
				TopMost = false;
			}
			Text = "耗时：" + str;
			minico.Visible = true;
			// 处理竖排文字显示
			if (interface_flag == "从右向左")
			{
				RichBoxBody.Text = shupai_Right_txt;
			}
			if (interface_flag == "从左向右")
			{
				RichBoxBody.Text = shupai_Left_txt;
			}
			// 处理粘贴板功能
			if (IniHelper.GetValue("截图音效", "粘贴板") == "True")
			{
				Clipboard.SetDataObject(RichBoxBody.Text);
			}
			// 处理百度搜索功能
			if (baidu_flags == "百度")
			{
				FormBorderStyle = FormBorderStyle.Sizable;
				Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
				Visible = false;
				WindowState = FormWindowState.Minimized;
				Show();
				Process.Start("https://www.baidu.com/s?wd=" + RichBoxBody.Text);
				baidu_flags = "";
				if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
				{
					var value = IniHelper.GetValue("快捷键", "翻译文本");
					var text2 = "None";
					var text3 = "F9";
					SetHotkey(text2, text3, value, 205);
				}
				HelpWin32.UnregisterHotKey(Handle, 222);
				return;
			}
			// 处理识别弹窗配置
			if (IniHelper.GetValue("配置", "识别弹窗") == "False")
			{
				FormBorderStyle = FormBorderStyle.Sizable;
				Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
				Visible = false;
				CommonHelper.ShowHelpMsg(RichBoxBody.Text == "***该区域未发现文本***" ? "无文本" : "已识别");
				if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
				{
					var value2 = IniHelper.GetValue("快捷键", "翻译文本");
					var text4 = "None";
					var text5 = "F9";
					SetHotkey(text4, text5, value2, 205);
				}
				HelpWin32.UnregisterHotKey(Handle, 222);
				return;
			}
			// 恢复窗体显示
			FormBorderStyle = FormBorderStyle.Sizable;
			Visible = true;
			Show();
			WindowState = FormWindowState.Normal;
			Size = new Size(form_width, form_height);
			HelpWin32.SetForegroundWindow(Handle);
			StaticValue.v_googleTranslate_txt = RichBoxBody.Text;
			// 处理自动翻译功能
			if (bool.Parse(IniHelper.GetValue("工具栏", "翻译")))
			{
				try
				{
					auto_fla = "";
					Invoke(new Translate(TransClick));
				}
				catch
				{
					//
				}
			}
			// 处理文本检查功能
			if (bool.Parse(IniHelper.GetValue("工具栏", "检查")))
			{
				try
				{
					RichBoxBody.Find = "";
				}
				catch
				{
					//
				}
			}
			// 重新设置热键
			if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
			{
				var value3 = IniHelper.GetValue("快捷键", "翻译文本");
				var text6 = "None";
				var text7 = "F9";
				SetHotkey(text6, text7, value3, 205);
			}
			HelpWin32.UnregisterHotKey(Handle, 222);
			// 移除不必要的Refresh()调用，避免重复重绘
			// RichBoxBody.Refresh();
		}

		/// <summary>
		/// 百度OCR中英文识别选项点击事件处理函数
		/// 设置百度OCR语言为中英文混合识别模式，并更新界面显示
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_Ch_and_En_Click(object sender, EventArgs e)
		{
			IniHelper.SetValue("密钥_百度", "language_code", "CHN_ENG");
			OCR_foreach("中英");
		}

		/// <summary>
		/// 百度OCR日语识别选项点击事件处理函数
		/// 设置百度OCR语言为日语识别模式，并更新界面显示
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_Jap_Click(object sender, EventArgs e)
		{
			IniHelper.SetValue("密钥_百度", "language_code", "JAP");
			OCR_foreach("日语");
		}

		/// <summary>
		/// 百度OCR韩语识别选项点击事件处理函数
		/// 设置百度OCR语言为韩语识别模式，并更新界面显示
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_Kor_Click(object sender, EventArgs e)
		{
			IniHelper.SetValue("密钥_百度", "language_code", "KOR");
			OCR_foreach("韩语");
		}

		/// <summary>
		/// 获取指定URL的网页HTML内容
		/// </summary>
		/// <param name="url">需要获取HTML内容的网址</param>
		/// <returns>返回从指定URL获取的HTML内容，如果获取失败则返回null</returns>
		public string Get_GoogletHtml(string url)
		{
			var text = "";
			var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			httpWebRequest.Method = "GET";
			httpWebRequest.Timeout = 5000;
			httpWebRequest.Headers.Add("Accept-Language: zh-CN;q=0.8,en-US;q=0.6,en;q=0.4");
			httpWebRequest.Headers.Add("Accept-Encoding: gzip,deflate");
			httpWebRequest.Headers.Add("Accept-Charset: utf-8");
			httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
			httpWebRequest.Host = "translate.google.cn";
			httpWebRequest.Accept = "*/*";
			httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)";
			string result;
			try
			{
				using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
					{
						text = streamReader.ReadToEnd();
						streamReader.Close();
						httpWebResponse.Close();
					}
				}
				result = text;
			}
			catch
			{
				result = null;
			}
			return result;
		}


		/// <summary>
		/// 检查并处理字符串中的标点符号
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>处理后的文本</returns>
		public string check_str(string text)
		{
			// 根据文本是否包含中文进行不同的标点符号处理
			if (contain_ch(text.Trim()))
			{
				text = CommonHelper.EnPunctuation2Ch(text.Trim());
				text = check_ch_en(text.Trim());
			}
			else
			{
				text = punctuation_ch_en(text.Trim());
				// 如果包含点号且包含其他特定符号，则删除标点符号周围的空格
				if (contain(text, ".") && (contain(text, ",") || contain(text, "!") || contain(text, "(") || contain(text, ")") || contain(text, "'")))
				{
					text = punctuation_Del_space(text);
				}
			}
			return text;
		}

		/// <summary>
		/// 将英文标点符号替换为中文标点符号
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>替换标点符号后的文本</returns>
		public static string punctuation_en_ch_x(string text)
		{
			var array = text.ToCharArray();
			// 遍历字符数组，将英文标点替换为对应的中文标点
			for (var i = 0; i < array.Length; i++)
			{
				var num = ".:;,?![]()".IndexOf(array[i]);
				if (num != -1)
				{
					array[i] = "。：；，？！【】（）"[num];
				}
			}
			return new string(array);
		}

		/// <summary>
		/// 通过POST方式向搜狗图片识别服务发送请求
		/// </summary>
		/// <param name="url">请求的目标URL</param>
		/// <param name="cookie">请求中使用的Cookie容器</param>
		/// <param name="content">要发送的字节内容</param>
		/// <returns>服务器响应的字符串结果，如果发生异常则返回null</returns>
		public string OCR_sougou_SogouPost(string url, CookieContainer cookie, byte[] content)
		{
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			httpWebRequest.CookieContainer = cookie;
			httpWebRequest.Timeout = 10000;
			httpWebRequest.Referer = "http://pic.sogou.com/resource/pic/shitu_intro/index.html";
			httpWebRequest.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary1ZZDB9E4sro7pf0g";
			httpWebRequest.Accept = "*/*";
			httpWebRequest.Headers.Add("Origin: http://pic.sogou.com");
			httpWebRequest.Headers.Add("Accept-Encoding: gzip,deflate");
			httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)";
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.ProtocolVersion = new Version(1, 1);
			httpWebRequest.ContentLength = content.Length;
			var requestStream = httpWebRequest.GetRequestStream();
			requestStream.Write(content, 0, content.Length);
			requestStream.Close();
			string result;
			try
			{
				var text = "";
				using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					var stream = httpWebResponse.GetResponseStream();
					// 处理gzip压缩的内容
					if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
					{
						stream = new GZipStream(stream, CompressionMode.Decompress);
					}
					using (var streamReader = new StreamReader(stream, Encoding.UTF8))
					{
						text = streamReader.ReadToEnd();
						streamReader.Close();
						httpWebResponse.Close();
					}
				}
				result = text;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 通过GET方式向搜狗图片识别服务发送请求
		/// </summary>
		/// <param name="url">请求的目标URL</param>
		/// <param name="cookie">请求中使用的Cookie容器</param>
		/// <param name="refer">请求的Referer头信息</param>
		/// <returns>服务器响应的字符串结果，如果发生异常则返回null</returns>
		public string OCR_sougou_SogouGet(string url, CookieContainer cookie, string refer)
		{
			var text = "";
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "GET";
			httpWebRequest.CookieContainer = cookie;
			httpWebRequest.Referer = refer;
			httpWebRequest.Timeout = 10000;
			httpWebRequest.Accept = "application/json";
			httpWebRequest.Headers.Add("X-Requested-With: XMLHttpRequest");
			httpWebRequest.Headers.Add("Accept-Encoding: gzip,deflate");
			httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
			httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)";
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.ProtocolVersion = new Version(1, 1);
			string result;
			try
			{
				using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					var stream = httpWebResponse.GetResponseStream();
					// 处理gzip压缩的内容
					if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
					{
						stream = new GZipStream(stream, CompressionMode.Decompress);
					}
					using (var streamReader = new StreamReader(stream, Encoding.UTF8))
					{
						text = streamReader.ReadToEnd();
						streamReader.Close();
						httpWebResponse.Close();
					}
				}
				result = text;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 使用搜狗OCR服务识别图片中的文字
		/// </summary>
		/// <param name="img">需要识别的图片</param>
		/// <returns>OCR识别结果的字符串，如果发生异常则返回null</returns>
		public string OCR_sougou_SogouOCR(Image img)
		{
			var cookie = new CookieContainer();
			var url = "http://pic.sogou.com/pic/upload_pic.jsp";
			var str = OCR_sougou_SogouPost(url, cookie, OCR_sougou_Content_Length(img));
			var url2 = "http://pic.sogou.com/pic/ocr/ocrOnline.jsp?query=" + str;
			var refer = "http://pic.sogou.com/resource/pic/shitu_intro/word_1.html?keyword=" + str;
			return OCR_sougou_SogouGet(url2, cookie, refer);
		}

		/// <summary>
		/// 将图像转换为搜狗OCR识别所需的字节数据格式
		/// </summary>
		/// <param name="img">需要进行OCR识别的图像</param>
		/// <returns>包含图像数据和表单信息的字节数组</returns>
		public byte[] OCR_sougou_Content_Length(Image img)
		{
			var bytes = Encoding.UTF8.GetBytes("------WebKitFormBoundary1ZZDB9E4sro7pf0g\r\nContent-Disposition: form-data; name=\"pic_path\"; filename=\"test2018.jpg\"\r\nContent-Type: image/jpeg\r\n\r\n");
			var array = OcrHelper.ImgToBytes(img);
			var bytes2 = Encoding.UTF8.GetBytes("\r\n------WebKitFormBoundary1ZZDB9E4sro7pf0g--\r\n");
			var array2 = new byte[bytes.Length + array.Length + bytes2.Length];
			bytes.CopyTo(array2, 0);
			array.CopyTo(array2, bytes.Length);
			bytes2.CopyTo(array2, bytes.Length + array.Length);
			return array2;
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
		/// 检查字符串是否包含标点符号
		/// </summary>
		/// <param name="str">待检查的字符串</param>
		/// <returns>如果包含标点符号则返回true，否则返回false</returns>
		public static bool contain_punctuation(string str)
		{
			return Regex.IsMatch(str, "\\p{P}");
		}

		/// <summary>
		/// 托盘帮助菜单点击事件处理函数
		/// 最小化当前窗口并打开帮助窗口
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void tray_help_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
			new FmHelp().Show();
		}

		/// <summary>
		/// 判断字符是否为特定标点符号
		/// </summary>
		/// <param name="text">待判断的字符</param>
		/// <returns>如果是指定标点符号返回true，否则返回false</returns>
		public bool Is_punctuation(string text)
		{
			return ",;:，（）、；".IndexOf(text) != -1;
		}

		/// <summary>
		/// 判断字符是否包含另一组特定标点符号
		/// </summary>
		/// <param name="text">待判断的字符</param>
		/// <returns>如果是指定标点符号返回true，否则返回false</returns>
		public bool has_punctuation(string text)
		{
			return ",;，；、<>《》()-（）".IndexOf(text) != -1;
		}

		/// <summary>
		/// 对OCR识别结果进行文本段落检查和处理，根据字符类型和规则进行智能换行
		/// </summary>
		/// <param name="jarray">包含OCR识别结果的JSON数组</param>
		/// <param name="lastlength">从文本末尾起取多少个字符进行换行判断</param>
		/// <param name="words">JSON对象中包含文本内容的字段名</param>
		public void checked_txt(JArray jarray, int lastlength, string words)
		{
			// 查找所有文本中最长的文本长度
			var num = 0;
			for (var i = 0; i < jarray.Count; i++)
			{
				var length = JObject.Parse(jarray[i].ToString())[words].ToString().Length;
				if (length > num)
				{
					num = length;
				}
			}
			var str = "";
			var text = "";
			// 遍历相邻的文本对，根据字符类型和规则判断是否需要换行
			for (var j = 0; j < jarray.Count - 1; j++)
			{
				var jobject = JObject.Parse(jarray[j].ToString());
				var array = jobject[words].ToString().ToCharArray();
				var jobject2 = JObject.Parse(jarray[j + 1].ToString());
				var array2 = jobject2[words].ToString().ToCharArray();
				var length2 = jobject[words].ToString().Length;
				var length3 = jobject2[words].ToString().Length;
				if (Math.Abs(length2 - length3) <= 0)
				{
					if (split_paragraph(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && Is_punctuation(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else
					{
						text += jobject[words].ToString().Trim();
					}
				}
				else if (split_paragraph(array[array.Length - lastlength].ToString()) && Math.Abs(length2 - length3) <= 1)
				{
					if (split_paragraph(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && Is_punctuation(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else
					{
						text += jobject[words].ToString().Trim();
					}
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && length2 <= num / 2)
				{
					text = text + jobject[words].ToString().Trim() + "\r\n";
				}
				else if (array2.Length > 1 && contain_ch(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()) && length3 - length2 < 4 && array2[1].ToString() == ".")
				{
					text = text + jobject[words].ToString().Trim() + "\r\n";
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (contain_en(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
				{
					text = text + jobject[words].ToString().Trim() + " ";
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (contain_en(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && Is_punctuation(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (Is_punctuation(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (Is_punctuation(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
				{
					text = text + jobject[words].ToString().Trim() + " ";
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (IsNum(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (IsNum(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else
				{
					text = text + jobject[words].ToString().Trim() + "\r\n";
				}
				// 如果当前文本包含特定标点符号，则添加额外换行
				if (has_punctuation(jobject[words].ToString()))
				{
					text += "\r\n";
				}
				str = str + jobject[words].ToString().Trim() + "\r\n";
			}
			// 将处理后的文本分别赋值给split_txt和typeset_txt字段
			split_txt = str + JObject.Parse(jarray[jarray.Count - 1].ToString())[words];
			typeset_txt = text.Replace("\r\n\r\n", "\r\n") + JObject.Parse(jarray[jarray.Count - 1].ToString())[words];
		}

		/// <summary>
		/// 根据名称设置OCR接口类型，并更新相关UI和配置文件
		/// </summary>
		/// <param name="name">OCR接口名称</param>
		private void OCR_foreach(string name)
		{
			OcrHelper.Dispose();
			var filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";
			switch (name)
			{
				case "韩语":
					interface_flag = "韩语";
					Refresh();
					baidu.Text = "百度√";
					kor.Text = "韩语√";
					break;
				case "日语":
					interface_flag = "日语";
					Refresh();
					baidu.Text = "百度√";
					jap.Text = "日语√";
					break;
				case "中英":
					interface_flag = "中英";
					Refresh();
					baidu.Text = "百度√";
					ch_en.Text = "中英√";
					break;
				case "搜狗":
					interface_flag = "搜狗";
					Refresh();
					sougou.Text = "搜狗√";
					break;
				case "腾讯":
					interface_flag = "腾讯";
					Refresh();
					tencent.Text = "腾讯√";
					break;
				case "腾讯-高精度":
					interface_flag = "腾讯-高精度";
					Refresh();
					tencent_accurate.Text = "腾讯-高精度√";
					break;
				case "有道":
					interface_flag = "有道";
					Refresh();
					youdao.Text = "有道√";
					break;
				case "微信":
					interface_flag = "微信";
					Refresh();
					wechat.Text = "微信√";
					break;
				case "白描":
					interface_flag = "白描";
					Refresh();
					baimiao.Text = "白描√";
					break;
				case "百度-高精度":
					interface_flag = "百度-高精度";
					Refresh();
					baidu_accurate.Text = "百度-高精度√";
					break;
				case "公式":
					interface_flag = "公式";
					Refresh();
					Mathfuntion.Text = "公式√";
					break;
				case "百度表格":
					interface_flag = "百度表格";
					Refresh();
					ocr_table.Text = "表格√";
					baidu_table.Text = "百度√";
					break;
				case "阿里表格":
					interface_flag = "阿里表格";
					Refresh();
					ocr_table.Text = "表格√";
					ali_table.Text = "阿里√";
					break;
				case "从左向右" when !File.Exists("cvextern.dll"):
					MessageBox.Show("请从蓝奏网盘中下载cvextern.dll大小约25m，点击确定自动弹出网页。\r\n将下载后的文件与 天若OCR文字识别.exe 这个文件放在一起。");
					Process.Start("https://www.lanzous.com/i1ab3vg");
					break;
				case "从左向右":
					interface_flag = "从左向右";
					Refresh();
					shupai.Text = "竖排√";
					left_right.Text = "从左向右√";
					break;
				case "从右向左" when !File.Exists("cvextern.dll"):
					MessageBox.Show("请从蓝奏网盘中下载cvextern.dll大小约25m，点击确定自动弹出网页。\r\n将下载后的文件与 天若OCR文字识别.exe 这个文件放在一起。");
					Process.Start("https://www.lanzous.com/i1ab3vg");
					return;
				case "从右向左":
					interface_flag = "从右向左";
					Refresh();
					shupai.Text = "竖排√";
					righ_left.Text = "从右向左√";
					break;
			}

			HelpWin32.IniFileHelper.SetValue("配置", "接口", interface_flag, filePath);
		}

		/// <summary>
		/// OCR识别方向设置为竖排的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_shupai_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// OCR识别设置为手写的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_write_Click(object sender, EventArgs e)
		{
			OCR_foreach("手写");
		}

		/// <summary>
		/// OCR识别方向设置为从左向右的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_lefttoright_Click(object sender, EventArgs e)
		{
			OCR_foreach("从左向右");
		}

		/// <summary>
		/// OCR识别方向设置为从右向左的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_righttoleft_Click(object sender, EventArgs e)
		{
			OCR_foreach("从右向左");
		}

		/// <summary>
		/// 使用百度OCR API进行文字识别
		/// 该函数通过百度云OCR服务对屏幕截图进行文字识别，并将识别结果保存到相关变量中
		/// </summary>
		public void OCR_baidu_acc()
		{
			split_txt = "";
			var text = "";
			try
			{
				// 获取百度云API访问令牌
				baidu_vip = CommonHelper.GetHtmlContent(string.Format("{0}?{1}", "https://aip.baidubce.com/oauth/2.0/token", "grant_type=client_credentials&client_id=" + StaticValue.BD_API_ID + "&client_secret=" + StaticValue.BD_API_KEY));
				if (baidu_vip == "")
				{
					MessageBox.Show("请检查密钥输入是否正确！", "提醒");
				}
				else
				{
					split_txt = "";
					var img = image_screen;
					var inArray = OcrHelper.ImgToBytes(img);
					var s = "image=" + HttpUtility.UrlEncode(Convert.ToBase64String(inArray));
					var bytes = Encoding.UTF8.GetBytes(s);
					// 创建百度OCR请求
					var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token=" + ((JObject)JsonConvert.DeserializeObject(baidu_vip))["access_token"]);
					httpWebRequest.Method = "POST";
					httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
					httpWebRequest.Timeout = 8000;
					httpWebRequest.ReadWriteTimeout = 5000;
					ServicePointManager.DefaultConnectionLimit = 512;
					using (var requestStream = httpWebRequest.GetRequestStream())
					{
						requestStream.Write(bytes, 0, bytes.Length);
					}
					// 获取并解析OCR识别结果
					var responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
					var value = text = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")).ReadToEnd();
					responseStream.Close();
					var jarray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["words_result"].ToString());
					var text2 = "";
					for (var i = 0; i < jarray.Count; i++)
					{
						var jobject = JObject.Parse(jarray[i].ToString());
						text2 += jobject["words"].ToString().Replace("\r", "").Replace("\n", "");
					}
					shupai_Right_txt = shupai_Right_txt + text2 + "\r\n";
					Thread.Sleep(600);
				}
			}
			catch
			{
				MessageBox.Show(text, "提醒");
				StaticValue.IsCapture = false;
				esc = "退出";
				fmloading.FmlClose = "窗体已关闭";
				esc_thread.Abort();
			}
		}

		/// <summary>
		/// 使用腾讯OCR API进行手写文字识别
		/// 该函数尝试通过腾讯云OCR服务对手写文字进行识别，但目前功能暂不可用
		/// </summary>
		public void OCR_Tencent_handwriting()
		{
			try
			{
				split_txt = "";
				var image = image_screen;
				// 根据图像尺寸调整图像大小以适应OCR识别要求
				if (image.Width > 90 && image.Height < 90)
				{
					var bitmap = new Bitmap(image.Width, 300);
					var graphics = Graphics.FromImage(bitmap);
					graphics.DrawImage(image, 5, 0, image.Width, image.Height);
					graphics.Save();
					graphics.Dispose();
					image = new Bitmap(bitmap);
				}
				else if (image.Width <= 90 && image.Height >= 90)
				{
					var bitmap2 = new Bitmap(300, image.Height);
					var graphics2 = Graphics.FromImage(bitmap2);
					graphics2.DrawImage(image, 0, 5, image.Width, image.Height);
					graphics2.Save();
					graphics2.Dispose();
					image = new Bitmap(bitmap2);
				}
				else if (image.Width < 90 && image.Height < 90)
				{
					var bitmap3 = new Bitmap(300, 300);
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
				var url = "https://ai.qq.com/cgi-bin/appdemo_handwritingocr";
				// This is a demo URL, and likely does not work with the new Tencent method.
				// For now, let's just show an error message.
				// In a future step, we would need to implement the correct API for handwriting.
				// 腾讯手写OCR功能当前不可用
				typeset_txt = "***腾讯手写功能暂不可用***";
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
			var image = new Image<Bgr, byte>(bitmap);
			var image2 = new Image<Gray, byte>(image.Width, image.Height);
			CvInvoke.CvtColor(image, image2, ColorConversion.Bgra2Gray);
			var structuringElement = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(4, 4), new Point(1, 1));
			CvInvoke.Erode(image2, image2, structuringElement, new Point(0, 2), 1, BorderType.Reflect101, default(MCvScalar));
			CvInvoke.Threshold(image2, image2, 100.0, 255.0, (ThresholdType)9);
			var image3 = new Image<Gray, byte>(image2.ToBitmap());
			var draw = image3.Convert<Bgr, byte>();
			var image4 = image3.Clone();
			CvInvoke.Canny(image3, image4, 255.0, 255.0, 5, true);
			return BoundingBox(image4, draw);
		}

		/// <summary>
		/// 捕获并保存图像的一部分到指定文件
		/// 该函数创建一个新的位图，在其中绘制指定区域的图像，然后保存到文件系统并进行OCR识别
		/// </summary>
		/// <param name="width">目标图像的宽度</param>
		/// <param name="gImage">源图像</param>
		/// <param name="saveFilePath">保存文件的路径</param>
		/// <param name="rect">要从源图像中截取的矩形区域</param>
		public void Captureimage(int width, Image gImage, string saveFilePath, Rectangle rect)
		{
			var bitmap = new Bitmap(width + 70, gImage.Size.Height);
			var graphics = Graphics.FromImage(bitmap);
			graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Size.Width, bitmap.Size.Height);
			graphics.DrawImage(gImage, 30, 0, rect, GraphicsUnit.Pixel);
			var bitmap2 = Image.FromHbitmap(bitmap.GetHbitmap());
			bitmap2.Save(saveFilePath, ImageFormat.Jpeg);
			image_screen = bitmap2;
			BaiduOcr();
			bitmap2.Dispose();
			bitmap.Dispose();
			graphics.Dispose();
		}

		/// <summary>
		/// 使用百度OCR服务识别屏幕截图中的文字内容，并将识别结果分别存储为左右排列格式
		/// </summary>
		public void BaiduOcr()
		{
			split_txt = "";
			try
			{
				// 设置OCR语言类型为中英文混合
				var str = "CHN_ENG";
				split_txt = "";
				// 获取待识别的图像
				var image = image_screen;
				// 将图像转换为字节数组
				var array = OcrHelper.ImgToBytes(image);
				// 构造POST数据，包含图像数据和语言类型
				var data = "type=general_location&image=data" + HttpUtility.UrlEncode(":image/jpeg;base64," + Convert.ToBase64String(array)) + "&language_type=" + str;
				// 向百度OCR接口发送请求并获取响应
				var value = CommonHelper.PostStrData("http://ai.baidu.com/tech/ocr/general", data);
				// 解析返回的JSON数据，提取文字识别结果
				var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["data"]["words_result"].ToString());
				var text = "";
				// 创建字符串数组存储每行识别结果
				var words = new string[jArray.Count];
				// 遍历识别结果，处理每行文字
				for (var i = 0; i < jArray.Count; i++)
				{
					var jObject = JObject.Parse(jArray[i].ToString());
					// 将识别的文字拼接到text变量中，并移除换行符
					text += jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
					// 将识别的文字按倒序存储到words数组中，并移除换行符
					words[jArray.Count - 1 - i] = jObject["words"].ToString().Replace("\r", "").Replace("\n", "");
				}
				// 构造倒序排列的文本内容
				var text2 = "";
				foreach (var t in words)
				{
					text2 += t;
				}
				// 将识别结果添加到右侧文本内容中，并避免出现连续的换行符
				shupai_Right_txt = (shupai_Right_txt + text + "\r\n").Replace("\r\n\r\n", "");
				// 处理左侧文本内容，避免出现连续的换行符
				shupai_Left_txt = text2.Replace("\r\n\r\n", "");
				// 显示识别结果
				MessageBox.Show(shupai_Left_txt);
				// 短暂延迟
				Thread.Sleep(10);
			}
			catch
			{
				// 异常处理
			}
		}

		/// <summary>
		/// 判断指定字符是否为段落分隔符
		/// </summary>
		/// <param name="text">需要判断的字符</param>
		/// <returns>如果是段落分隔符则返回true，否则返回false</returns>
		public bool split_paragraph(string text)
		{
			return "。？！?!：".IndexOf(text, StringComparison.Ordinal) != -1;
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
			catch(Exception)
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
		/// 使用百度OCR识别图片内容
		/// </summary>
		/// <param name="image">需要识别的图片</param>
		/// <param name="strImage">图片字符串参数（未使用）</param>
		public void OCR_baidu_image(Image image, string strImage)
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
		/// 处理拼音切换按钮点击事件，设置拼音标志并触发翻译操作
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_pinyin_Click(object sender, EventArgs e)
		{
			pinyin_flag = true;
			TransClick();
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
		/// 执行文本翻译操作，根据配置选择不同的翻译服务和语言方向
		/// </summary>
		public async void 翻译文本()
		{
			// 检查是否启用了快速翻译功能
			if (IniHelper.GetValue("配置", "快速翻译") == "True")
			{
				var data = "";
				try
				{
					// 根据焦点位置获取待翻译文本
					if (ContainsFocus)
					{
						if (RichBoxBody.richTextBox1.Focused)
						{
							trans_hotkey = RichBoxBody.richTextBox1.SelectedText;
						}
						else if (RichBoxBody_T.richTextBox1.Focused)
						{
							trans_hotkey = RichBoxBody_T.richTextBox1.SelectedText;
						}
						else
						{
							trans_hotkey = GetTextFromClipboard();
						}
					}
					else
					{
						trans_hotkey = GetTextFromClipboard();
					}
					if (string.IsNullOrEmpty(trans_hotkey)) return;

					// 获取当前翻译服务配置
					string transService = StaticValue.Translate_Current_API;
					string sectionName;
					switch (transService)
					{
						case "谷歌":
							sectionName = "Google";
							break;
						case "百度":
							sectionName = "Baidu";
							break;
						case "腾讯":
							sectionName = "Tencent";
							break;
						case "腾讯交互翻译":
							sectionName = "TencentInteractive";
							break;
						case "彩云小译":
							sectionName = "Caiyun";
							break;
						case "彩云小译2":
							sectionName = "Caiyun2";
							break;
						case "火山翻译":
							sectionName = "Volcano";
							break;
						default:
							sectionName = transService;
							break;
					}
					if (!StaticValue.Translate_Configs.TryGetValue(sectionName, out var config))
					{
						config = new StaticValue.TranslateConfig { Source = "auto", Target = "自动判断" };
					}

					// 确定源语言和目标语言
					string toLang;
					string fromLang = config.Source;

					// 自动判断目标语言
					if (config.Target == "自动判断")
					{
						toLang = "en"; // 默认翻译为英文
						// 中文<->英文互译逻辑
						if (StaticValue.ZH2EN)
						{
							if (ch_count(trans_hotkey.Trim()) > en_count(trans_hotkey.Trim()) || (en_count(trans_hotkey.Trim()) == 1 && ch_count(trans_hotkey.Trim()) == 1))
							{
								toLang = "en";
							}
							else
							{
								toLang = "zh-CN";
							}
						}
						// 中文<->日文互译逻辑
						else if (StaticValue.ZH2JP)
						{
							// 统计中文字符和日文字符数量来判断主要语言
							string textToCheck = trans_hotkey.Trim();
							int chineseCount = ch_count(textToCheck);
							// 对于日文，我们需要统计假名的数量，因为汉字在中日文都存在
							int japaneseKanaCount = 0;
							foreach (char c in textToCheck)
							{
								// 统计平假名 (U+3040-U+309F) 和片假名 (U+30A0-U+30FF)
								if ((c >= '\u3040' && c <= '\u309F') || (c >= '\u30A0' && c <= '\u30FF'))
								{
									japaneseKanaCount++;
								}
							}
							
							// 如果日文假名多于中文字符，说明是日文文本，翻译到中文
							// 否则翻译到日文
							if (japaneseKanaCount > 0 && japaneseKanaCount >= chineseCount / 2)
							{
								// 有相当数量的假名，判断为日文，翻译到中文
								toLang = "zh-CN";
							}
							else
							{
								// 中文字符占主导，翻译到日文
								toLang = "ja";
							}
						}
						// 中文<->韩文互译逻辑
						else if (StaticValue.ZH2KO)
						{
							if (contain_kor(trans_hotkey.Trim()))
							{
								toLang = "zh-CN";
							}
							else
							{
								toLang = "ko";
							}
						}
					}
					else
					{
						toLang = config.Target;
					}

					// 处理百度和腾讯翻译服务的语言代码映射
					if (transService == "百度")
					{
						if (fromLang == "zh-CN") fromLang = "zh";
						if (toLang == "zh-CN") toLang = "zh";
						if (fromLang == "ja") fromLang = "jp";
						if (toLang == "ja") toLang = "jp";
						if (fromLang == "ko") fromLang = "kor";
						if (toLang == "ko") toLang = "kor";
					}
					if (transService == "腾讯")
					{
						if (fromLang == "zh-CN") fromLang = "zh";
						if (toLang == "zh-CN") toLang = "zh";
					}

					// 调用相应的翻译服务进行翻译
					switch (transService)
					{
						case "谷歌":
							data = await GTranslateHelper.TranslateAsync(trans_hotkey, fromLang, toLang, "google");
							break;
						case "Bing":
							data = await BingTranslator.TranslateAsync(trans_hotkey, fromLang, toLang);
							break;
						case "Bing2":
						case "BingNew":
							data = await BingTranslator2.TranslateAsync(trans_hotkey, fromLang, toLang);
							break;
						case "Microsoft":
							data = await GTranslateHelper.TranslateAsync(trans_hotkey, fromLang, toLang, "microsoft");
							break;
						case "Yandex":
							data = await GTranslateHelper.TranslateAsync(trans_hotkey, fromLang, toLang, "yandex");
							break;
						case "百度":
							data = TranslateBaidu(trans_hotkey, fromLang, toLang, config.AppId, config.ApiKey);
							break;
						case "腾讯":
							data = Translate_Tencent(trans_hotkey, fromLang, toLang, config.AppId, config.ApiKey);
							break;
						case "腾讯交互翻译":
							data = await TencentTranslator.TranslateAsync(trans_hotkey, fromLang, toLang);
							break;
						case "彩云小译":
							data = await CaiyunTranslator.TranslateAsync(trans_hotkey, fromLang, toLang);
							break;
						case "彩云小译2":
							if (string.IsNullOrEmpty(config.ApiKey))
								data = "[彩云小译2]：未配置Token";
							else
								data = await CaiyunTranslator2.TranslateAsync(trans_hotkey, fromLang, toLang, config.ApiKey);
							break;
						case "火山翻译":
							data = await VolcanoTranslator.TranslateAsync(trans_hotkey, fromLang, toLang);
							break;
						default:
							data = await GTranslateHelper.TranslateAsync(trans_hotkey, fromLang, toLang, "google");
							break;
					}
					// 将翻译结果复制到剪贴板并粘贴到当前焦点位置
					Clipboard.SetData(DataFormats.UnicodeText, data);
					SendKeys.SendWait("^v");
					return;
				}
				catch
				{
					// 出现异常时也尝试粘贴当前结果
					Clipboard.SetData(DataFormats.UnicodeText, data);
					SendKeys.SendWait("^v");
					return;
				}
			}
			// 如果未启用快速翻译，则执行常规翻译流程
			SendKeys.SendWait("^c");
			SendKeys.Flush();
			RichBoxBody.Text = Clipboard.GetText();
			TransClick();
			FormBorderStyle = FormBorderStyle.Sizable;
			Visible = true;
			HelpWin32.SetForegroundWindow(StaticValue.mainHandle);
			Show();
			WindowState = FormWindowState.Normal;
			if (IniHelper.GetValue("工具栏", "顶置") == "True")
			{
				TopMost = true;
				return;
			}
			TopMost = false;
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
		/// 检查在给定的二维布尔数组中指定坐标位置是否存在且值为true
		/// </summary>
		/// <param name="colors">二维布尔数组</param>
		/// <param name="x">要检查位置的x坐标</param>
		/// <param name="y">要检查位置的y坐标</param>
		/// <returns>如果坐标在有效范围内且对应值为true则返回true，否则返回false</returns>
		public bool Exist(bool[][] colors, int x, int y)
		{
			return x >= 0 && y >= 0 && x < colors.Length && y < colors[0].Length && colors[x][y];
		}

		/// <summary>
		/// 检查矩形右侧是否存在值为true的相邻元素
		/// </summary>
		/// <param name="colors">二维布尔数组</param>
		/// <param name="rect">要检查的矩形区域</param>
		/// <returns>如果矩形右侧存在值为true的相邻元素则返回true，否则返回false</returns>
		public bool R_Exist(bool[][] colors, Rectangle rect)
		{
			if (rect.Right >= colors[0].Length || rect.Left < 0)
			{
				return false;
			}
			for (var i = 0; i < rect.Height; i++)
			{
				if (Exist(colors, rect.Top + i, rect.Right + 1))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 检查矩形底侧是否存在值为true的相邻元素
		/// </summary>
		/// <param name="colors">二维布尔数组</param>
		/// <param name="rect">要检查的矩形区域</param>
		/// <returns>如果矩形底侧存在值为true的相邻元素则返回true，否则返回false</returns>
		public bool D_Exist(bool[][] colors, Rectangle rect)
		{
			if (rect.Bottom >= colors.Length || rect.Top < 0)
			{
				return false;
			}
			for (var i = 0; i < rect.Width; i++)
			{
				if (Exist(colors, rect.Bottom + 1, rect.Left + i))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 检查矩形左侧是否存在值为true的相邻元素
		/// </summary>
		/// <param name="colors">二维布尔数组</param>
		/// <param name="rect">要检查的矩形区域</param>
		/// <returns>如果矩形左侧存在值为true的相邻元素则返回true，否则返回false</returns>
		public bool L_Exist(bool[][] colors, Rectangle rect)
		{
			if (rect.Right >= colors[0].Length || rect.Left < 0)
			{
				return false;
			}
			for (var i = 0; i < rect.Height; i++)
			{
				if (Exist(colors, rect.Top + i, rect.Left - 1))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 检查矩形顶侧是否存在值为true的相邻元素
		/// </summary>
		/// <param name="colors">二维布尔数组</param>
		/// <param name="rect">要检查的矩形区域</param>
		/// <returns>如果矩形顶侧存在值为true的相邻元素则返回true，否则返回false</returns>
		public bool U_Exist(bool[][] colors, Rectangle rect)
		{
			if (rect.Bottom >= colors.Length || rect.Top < 0)
			{
				return false;
			}
			for (var i = 0; i < rect.Width; i++)
			{
				if (Exist(colors, rect.Top - 1, rect.Left + i))
				{
					return true;
				}
			}
			return false;
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
			var image = new Image<Bgr, byte>(bitmap);
			var image2 = new Image<Gray, byte>(image.Width, image.Height);
			// 将彩色图像转换为灰度图像
			CvInvoke.CvtColor(image, image2, ColorConversion.Bgra2Gray);
			// 创建结构元素用于形态学操作
			var structuringElement = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(6, 20), new Point(1, 1));
			// 对图像进行腐蚀操作，增强围栏特征
			CvInvoke.Erode(image2, image2, structuringElement, new Point(0, 2), 1, BorderType.Reflect101, default(MCvScalar));
			// 应用阈值处理将图像二值化
			CvInvoke.Threshold(image2, image2, 100.0, 255.0, (ThresholdType)9);
			var image3 = new Image<Gray, byte>(image2.ToBitmap());
			var draw = image3.Convert<Bgr, byte>();
			// 复制图像用于边缘检测
			var image4 = image3.Clone();
			// 使用Canny算法检测图像边缘
			CvInvoke.Canny(image3, image4, 255.0, 255.0, 5, true);
			// 查找并标记边界框区域
			var image5 = BoundingBox_fences(image4, draw);
			var image6 = new Image<Gray, byte>((Bitmap)image5);
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
		/// 检查搜狗OCR识别结果并进行排版处理
		/// </summary>
		/// <param name="jarray">包含OCR识别结果的JSON数组</param>
		/// <param name="lastlength">用于判断文本结尾的长度参数</param>
		/// <param name="words">包含文本内容的字段名</param>
		/// <param name="location">包含位置信息的字段名</param>
		public void checked_location_sougou(JArray jarray, int lastlength, string words, string location)
		{
			paragraph = false;
			var num = 20000;
			var num2 = 0;
			// 遍历OCR识别结果，获取文本位置信息
			foreach (var t in jarray)
			{
				var jObject = JObject.Parse(t.ToString());
				var num3 = split_char_x(jObject[location][1].ToString()) - split_char_x(jObject[location][0].ToString());
				if (num3 > num2)
				{
					num2 = num3;
				}
				var num4 = split_char_x(jObject[location][0].ToString());
				if (num4 < num)
				{
					num = num4;
				}
			}
			var jobject2 = JObject.Parse(jarray[0].ToString());
			if (Math.Abs(split_char_x(jobject2[location][0].ToString()) - num) > 10)
			{
				paragraph = true;
			}
			var text = "";
			var text2 = "";
			// 根据位置信息对文本进行排版处理
			for (var j = 0; j < jarray.Count; j++)
			{
				var jobject3 = JObject.Parse(jarray[j].ToString());
				var array = jobject3[words].ToString().ToCharArray();
				var jobject4 = JObject.Parse(jarray[j].ToString());
				var flag = Math.Abs(split_char_x(jobject4[location][1].ToString()) - split_char_x(jobject4[location][0].ToString()) - num2) > 20;
				var flag2 = Math.Abs(split_char_x(jobject4[location][0].ToString()) - num) > 10;
				if (flag && flag2)
				{
					text = text.Trim() + "\r\n" + jobject4[words].ToString().Trim();
				}
				else if (array.Length > 1 && IsNum(array[0].ToString()) && !contain_ch(array[1].ToString()) && flag)
				{
					text = text.Trim() + "\r\n" + jobject4[words].ToString().Trim() + "\r\n";
				}
				else
				{
					text += jobject4[words].ToString().Trim();
				}
				if (contain_en(array[array.Length - lastlength].ToString()))
				{
					text = text + jobject3[words].ToString().Trim() + " ";
				}
				text2 = text2 + jobject4[words].ToString().Trim() + "\r\n";
			}
			split_txt = text2.Replace("\r\n\r\n", "\r\n");
			typeset_txt = text;
		}

		/// <summary>
		/// 从坐标字符串中提取X坐标值
		/// </summary>
		/// <param name="splitChar">格式为"x,y"的坐标字符串</param>
		/// <returns>X坐标值</returns>
		public int split_char_x(string splitChar)
		{
			return Convert.ToInt32(splitChar.Split(',')[0]);
		}

		/// <summary>
		/// 托盘图标双击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void tray_double_Click(object sender, EventArgs e)
		{
			HelpWin32.UnregisterHotKey(Handle, 205);
			menu.Hide();
			RichBoxBody.Hide = "";
			RichBoxBody_T.Hide = "";
			MainOCRQuickScreenShots();
		}

		/// <summary>
		/// 统计文本中的英文单词数量
		/// </summary>
		/// <param name="text">待统计的文本</param>
		/// <returns>英文单词数量</returns>
		public int en_count(string text)
		{
			return Regex.Matches(text, "\\s+").Count + 1;
		}

		/// <summary>
		/// 统计文本中的中文字符数量
		/// </summary>
		/// <param name="str">待统计的字符串</param>
		/// <returns>中文字符数量</returns>
		public int ch_count(string str)
		{
			var num = 0;
			var regex = new Regex("^[\\u4E00-\\u9FA5]{0,}$");
			for (var i = 0; i < str.Length; i++)
			{
				if (regex.IsMatch(str[i].ToString()))
				{
					num++;
				}
			}
			return num;
		}

		/// <summary>
		/// 谷歌翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_google_Click(object sender, EventArgs e)
		{
			Trans_foreach("谷歌");
		}

		/// <summary>
		/// 百度翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_baidu_Click(object sender, EventArgs e)
		{
			Trans_foreach("百度");
		}

		/// <summary>
		/// 腾讯翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_tencent_Click(object sender, EventArgs e)
		{
			Trans_foreach("腾讯");
		}

		/// <summary>
		/// Bing翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_bing_Click(object sender, EventArgs e)
		{
			Trans_foreach("Bing");
		}

		/// <summary>
		/// Bing2翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_bing2_Click(object sender, EventArgs e)
		{
			Trans_foreach("Bing2");
		}

		/// <summary>
		/// 微软翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_microsoft_Click(object sender, EventArgs e)
		{
			Trans_foreach("Microsoft");
		}

		/// <summary>
		/// Yandex翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_yandex_Click(object sender, EventArgs e)
		{
			Trans_foreach("Yandex");
		}

		/// <summary>
		/// 腾讯交互翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_tencentinteractive_Click(object sender, EventArgs e)
		{
			Trans_foreach("腾讯交互翻译");
		}

		/// <summary>
		/// 彩云小译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_caiyun_Click(object sender, EventArgs e)
		{
			Trans_foreach("彩云小译");
		}

		/// <summary>
		/// 火山翻译按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_volcano_Click(object sender, EventArgs e)
		{
			Trans_foreach("火山翻译");
		}

		/// <summary>
		/// 彩云小译2按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void Trans_caiyun2_Click(object sender, EventArgs e)
		{
			Trans_foreach("彩云小译2");
		}

		/// <summary>
		/// 翻译接口选择处理函数，更新界面显示并执行翻译
		/// </summary>
		/// <param name="name">翻译接口名称</param>
		private void Trans_foreach(string name)
		{
			// 重置所有翻译按钮文本
			trans_baidu.Text = "百度";
			trans_google.Text = "谷歌";
			trans_tencent.Text = "腾讯";
			trans_bing.Text = "Bing";
			trans_bing2.Text = "Bing2";
			trans_microsoft.Text = "Microsoft";
			trans_yandex.Text = "Yandex";
			trans_tencentinteractive.Text = "腾讯交互";
			trans_caiyun.Text = "彩云";
			trans_volcano.Text = "火山";
			trans_caiyun2.Text = "彩云2";

			// 根据选择的翻译接口设置对应按钮文本
			if (name == "百度")
			{
				trans_baidu.Text = "百度√";
			}
			if (name == "谷歌")
			{
				trans_google.Text = "谷歌√";
			}
			if (name == "腾讯")
			{
				trans_tencent.Text = "腾讯√";
			}
			if (name == "Bing")
			{
				trans_bing.Text = "Bing√";
			}
			if (name == "Bing2")
			{
				trans_bing2.Text = "Bing2√";
			}
			if (name == "Microsoft")
			{
				trans_microsoft.Text = "Microsoft√";
			}
			if (name == "Yandex")
			{
				trans_yandex.Text = "Yandex√";
			}
			if (name == "腾讯交互翻译")
			{
				trans_tencentinteractive.Text = "腾讯交互√";
			}
			if (name == "彩云小译")
			{
				trans_caiyun.Text = "彩云√";
			}
			if (name == "火山翻译")
			{
				trans_volcano.Text = "火山√";
			}
			if (name == "彩云小译2")
			{
				trans_caiyun2.Text = "彩云2√";
			}
			
			// 保存翻译接口配置
			IniHelper.SetValue("配置", "翻译接口", name);
			
			// 如果翻译功能已开启，则执行翻译
			if (transtalate_fla == "开启")
			{
				typeset_txt = RichBoxBody.Text;
				PictureBox1.Visible = true;
				PictureBox1.BringToFront();
				trans_Calculate();
			}
		}

		/// <summary>
		/// 百度翻译实现函数
		/// </summary>
		/// <param name="content">待翻译的内容</param>
		/// <param name="from">源语言</param>
		/// <param name="to">目标语言</param>
		/// <param name="appId">百度翻译APP ID</param>
		/// <param name="apiKey">百度翻译API密钥</param>
		/// <returns>翻译结果或错误信息</returns>
		private string TranslateBaidu(string content, string from, string to, string appId, string apiKey)
		{
			try
			{
				// 检查必要参数
				if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(apiKey))
				{
					return "[百度翻译]：未输入APP_ID或APP_KEY";
				}

				// 生成请求参数
				var rd = new Random();
				var salt = rd.Next(100000).ToString();
				var sign = EncryptString(appId + content + salt + apiKey);
				var url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
				url += "q=" + HttpUtility.UrlEncode(content);
				url += "&from=" + from;
				url += "&to=" + to;
				url += "&appid=" + appId;
				url += "&salt=" + salt;
				url += "&sign=" + sign;

				// 创建HTTP请求
				var request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.ContentType = "text/html;charset=UTF-8";
				request.UserAgent = null;
				request.Timeout = 6000;

				HttpWebResponse response;
				try
				{
					response = (HttpWebResponse)request.GetResponse();
				}
				catch (WebException)
				{
					return "[百度翻译]：网络请求超时，请检查网络连接。";
				}

				// 处理响应结果
				using (var myResponseStream = response.GetResponseStream())
				using (var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
				{
					var retString = myStreamReader.ReadToEnd();
					var result = JsonConvert.DeserializeObject<Rootobject>(retString);

					// 检查是否有错误
					if (!string.IsNullOrEmpty(result.error_code))
					{
						return $"[百度翻译]：API错误 {result.error_code} - {result.error_msg}";
					}

					// 提取翻译结果
					if (result.trans_result != null && result.trans_result.Any())
					{
						var result_temp = "";
						foreach (var trans_result_temp in result.trans_result)
						{
							result_temp += trans_result_temp.dst + Environment.NewLine;
						}
						return result_temp.TrimEnd('\r', '\n');
					}

					return "[百度翻译]：收到未知响应，无法解析译文。";
				}
			}
			catch (JsonException)
			{
				return "[百度翻译]：无法解析返回的JSON数据。";
			}
			catch (Exception ex)
			{
				return $"[百度翻译]：发生未知错误 - {ex.Message}";
			}
		}



		/// <summary>
		/// 腾讯翻译实现函数
		/// </summary>
		/// <param name="content">待翻译的内容</param>
		/// <param name="from">源语言</param>
		/// <param name="to">目标语言</param>
		/// <param name="appId">腾讯云SecretId</param>
		/// <param name="apiKey">腾讯云SecretKey</param>
		/// <returns>翻译结果或错误信息</returns>
		private string Translate_Tencent(string content, string from, string to, string appId, string apiKey)
		{
			// 检查必要参数
			if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(apiKey))
			{
				return "[腾讯翻译]：未输入SecretId或SecretKey";
			}

			try
			{
				// 构造腾讯云API凭证和配置
				Credential cred = new Credential
				{
					SecretId = appId,
					SecretKey = apiKey
				};

				ClientProfile clientProfile = new ClientProfile();
				HttpProfile httpProfile = new HttpProfile
				{
					Endpoint = "tmt.tencentcloudapi.com",
					Timeout = 5000 // 5 seconds
				};
				clientProfile.HttpProfile = httpProfile;

				// 初始化翻译客户端并发送请求
				TmtClient client = new TmtClient(cred, "ap-guangzhou", clientProfile);
				TextTranslateRequest req = new TextTranslateRequest
				{
					SourceText = content,
					Source = from,
					Target = to,
					ProjectId = 0
				};

				TextTranslateResponse resp = client.TextTranslateSync(req);
				return resp.TargetText;
			}
			catch (TencentCloudSDKException e)
			{
				return $"[腾讯翻译]：API错误 {e.ErrorCode} - {e.Message}";
			}
			catch (Exception ex)
			{
				return $"[腾讯翻译]：发生未知错误 - {ex.Message}";
			}
		}

		/// <summary>
		/// 使用百度API进行表格OCR识别
		/// 该方法会截取当前屏幕图像，调用百度表格OCR API进行识别，并将结果处理后显示在RichBoxBody中
		/// </summary>
		public void BdTableOCR()
		{
			typeset_txt = "[消息]：表格已下载！";
			split_txt = "";
			try
			{
				// 获取百度API访问令牌
				baidu_vip = CommonHelper.GetHtmlContent(string.Format("{0}?{1}", "https://aip.baidubce.com/oauth/2.0/token", "grant_type=client_credentials&client_id=" + StaticValue.BD_API_ID + "&client_secret=" + StaticValue.BD_API_KEY));
				if (baidu_vip == "")
				{
					MessageBox.Show("请检查密钥输入是否正确！", "提醒");
				}
				else
				{
					split_txt = "";
					var image = image_screen;
					var array = OcrHelper.ImgToBytes(image);
					var s = "image=" + HttpUtility.UrlEncode(Convert.ToBase64String(array));
					var bytes = Encoding.UTF8.GetBytes(s);
					// 创建请求并发送图像数据到百度表格OCR API
					var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://aip.baidubce.com/rest/2.0/solution/v1/form_ocr/request?access_token=" + ((JObject)JsonConvert.DeserializeObject(baidu_vip))["access_token"]);
					httpWebRequest.Proxy = null;
					httpWebRequest.Method = "POST";
					httpWebRequest.ContentType = "application/x-www-form-urlencoded";
					httpWebRequest.Timeout = 8000;
					httpWebRequest.ReadWriteTimeout = 5000;
					using (var requestStream = httpWebRequest.GetRequestStream())
					{
						requestStream.Write(bytes, 0, bytes.Length);
					}
					// 获取响应数据
					var responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
					var value = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")).ReadToEnd();
					responseStream.Close();
					// 构造获取结果的请求参数
					var postStr = "request_id=" + JObject.Parse(JArray.Parse(((JObject)JsonConvert.DeserializeObject(value))["result"].ToString())[0].ToString())["request_id"].ToString().Trim() + "&result_type=json";
					var text = "";
					// 轮询获取OCR结果，直到处理完成
					while (!text.Contains("已完成"))
					{
						if (text.Contains("image recognize error"))
						{
							RichBoxBody.Text = "[消息]：未发现表格！";
							break;
						}
						Thread.Sleep(120);
						text = CommonHelper.PostStrData("https://aip.baidubce.com/rest/2.0/solution/v1/form_ocr/get_request_result?access_token=" + ((JObject)JsonConvert.DeserializeObject(baidu_vip))["access_token"], postStr);
					}
					if (!text.Contains("image recognize error"))
					{
						get_table(text);
					}
				}
			}
			catch
			{
				RichBoxBody.Text = "[消息]：免费百度密钥50次已经耗完！请更换自己的密钥继续使用！";
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
		/// 解析并处理表格OCR结果数据
		/// 该方法将OCR识别结果解析为二维表格数据，并计算每列宽度，最后设置到剪贴板
		/// </summary>
		/// <param name="str">包含表格OCR识别结果的JSON字符串</param>
		private void get_table(string str)
		{
			// 解析JSON数据，提取表格内容
			var jArray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(((JObject)JsonConvert.DeserializeObject(str))["result"]["result_data"].ToString().Replace("\\", "")))["forms"][0]["body"].ToString());
			var array = new int[jArray.Count];
			var array2 = new int[jArray.Count];
			// 提取行列信息
			for (var i = 0; i < jArray.Count; i++)
			{
				var jObject = JObject.Parse(jArray[i].ToString());
				var value = jObject["column"].ToString().Replace("[", "").Replace("]", "").Replace("\r", "").Replace("\n", "").Trim();
				var value2 = jObject["row"].ToString().Replace("[", "").Replace("]", "").Replace("\r", "").Replace("\n", "").Trim();
				array[i] = Convert.ToInt32(value);
				array2[i] = Convert.ToInt32(value2);
			}
			// 创建二维数组存储表格数据
			var array3 = new string[array2.Max() + 1, array.Max() + 1];
			for (var j = 0; j < jArray.Count; j++)
			{
				var jObject = JObject.Parse(jArray[j].ToString());
				var value3 = jObject["column"].ToString().Replace("[", "").Replace("]", "").Replace("\r", "").Replace("\n", "").Trim();
				var value4 = jObject["row"].ToString().Replace("[", "").Replace("]", "").Replace("\r", "").Replace("\n", "").Trim();
				array[j] = Convert.ToInt32(value3);
				array2[j] = Convert.ToInt32(value4);
				var text = jObject["word"].ToString().Replace("[", "").Replace("]", "").Replace("\r", "").Replace("\n", "").Trim();
				array3[Convert.ToInt32(value4), Convert.ToInt32(value3)] = text;
			}
			// 计算每列的最佳显示宽度
			var graphics = CreateGraphics();
			var array4 = new int[array.Max() + 1];
			var num = 0;
			var size = new SizeF(10f, 10f);
			var num2 = Screen.PrimaryScreen.Bounds.Width / 4;
			for (var k = 0; k < array3.GetLength(1); k++)
			{
				for (var l = 0; l < array3.GetLength(0); l++)
				{
					size = graphics.MeasureString(array3[l, k], new Font("宋体", 12f));
					if (num < (int)size.Width)
					{
						num = (int)size.Width;
					}
					if (num > num2)
					{
						num = num2;
					}
				}
				array4[k] = num;
				num = 0;
			}
			graphics.Dispose();
			// 将表格数据设置到剪贴板
			setClipboard_Table(array3, array4);
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
				var dataObject = new DataObject();
				dataObject.SetData(DataFormats.Rtf, RichBoxBody.Rtx1Rtf);
				dataObject.SetData(DataFormats.UnicodeText, RichBoxBody.Text);
				RichBoxBody.Text = "[消息]：表格已复制到粘贴板！";
				Clipboard.SetDataObject(dataObject);
			}
			// 清理资源
			image_screen.Dispose();
			GC.Collect();
			StaticValue.IsCapture = false;
			FormBorderStyle = FormBorderStyle.Sizable;
			Visible = true;
			Show();
			WindowState = FormWindowState.Normal;
			Size = new Size(form_width, form_height);
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
				Clipboard.SetDataObject(typeset_txt);
				CopyHtmlToClipBoard(typeset_txt);
			}
		}

		/// <summary>
		/// 将表格数据转换为RTF格式并设置到RichBoxBody中
		/// </summary>
		/// <param name="wordo">包含表格数据的二维字符串数组</param>
		/// <param name="cc">包含每列宽度的整型数组</param>
		private void setClipboard_Table(string[,] wordo, int[] cc)
		{
			var str = "{\\rtf1\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052{\\fonttbl{\\f0\\fnil\\fprq2\\fcharset134";
			str += "\\'cb\\'ce\\'cc\\'e5;}{\\f1\\fnil\\fcharset134 \\'cb\\'ce\\'cc\\'e5;}}\\viewkind4\\uc1\\trowd\\trgaph108\\trleft-108";
			str += "\\trbrdrt\\brdrs\\brdrw10 \\trbrdrl\\brdrs\\brdrw10 \\trbrdrb\\brdrs\\brdrw10 \\trbrdrb\\brdrs\\brdrw10 ";
			var num = 0;
			// 构造RTF表格列定义
			for (var i = 1; i <= cc.Length; i++)
			{
				num += cc[i - 1] * 17;
				str = str + "\\clbrdrt\\brdrw15\\brdrs\\clbrdrl\\brdrw15\\brdrs\\clbrdrb\\brdrw15\\brdrs\\clbrdrr\\brdrw15\\brdrs \\cellx" + num;
			}
			var text = "";
			var str2 = "\\pard\\intbl\\kerning2\\f0";
			var str3 = "\\row\\pard\\lang2052\\kerning0\\f1\\fs18\\par}";
			// 构造RTF表格内容
			for (var j = 0; j < wordo.GetLength(0); j++)
			{
				for (var k = 0; k < wordo.GetLength(1); k++)
				{
					if (k == 0)
					{
						text = text + "\\fs24 " + wordo[j, k];
					}
					else
					{
						text = text + "\\cell " + wordo[j, k];
					}
				}
				if (j != wordo.GetLength(0) - 1)
				{
					text += "\\row\\intbl";
				}
			}
			RichBoxBody.Rtx1Rtf = str + str2 + text + str3;
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
		/// 阿里表格OCR识别按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_ailitable_Click(object sender, EventArgs e)
		{
			OCR_foreach("阿里表格");
		}

		/// <summary>
		/// 刷新ocr接口界面控件文本显示
		/// </summary>
		private new void Refresh()
		{
			sougou.Text = "搜狗";
			tencent.Text = "腾讯";
			tencent_accurate.Text = "腾讯-高精度";
			baidu.Text = "百度";
			youdao.Text = "有道";
			wechat.Text = "微信";
			baimiao.Text = "白描";
			baidu_accurate.Text = "百度-高精度";
			shupai.Text = "竖排";
			ocr_table.Text = "表格";
			ch_en.Text = "中英";
			jap.Text = "日语";
			kor.Text = "韩语";
			left_right.Text = "从左向右";
			righ_left.Text = "从右向左";
			baidu_table.Text = "百度";
			ali_table.Text = "阿里";
			Mathfuntion.Text = "公式";
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
			dataObject.SetData(DataFormats.Text, data);
			Clipboard.SetDataObject(dataObject);
		}

		/// <summary>
		/// OCR数学公式识别点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_Mathfuntion_Click(object sender, EventArgs e)
		{
			OCR_foreach("公式");
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

#endregion
		//控制右键菜单-接口项的可见性
		private void InitializeApiMenus()
		{
			// OCR 接口可见性设置
			SetMenuItemVisibility(sougou, "Ocr接口显示", "Sougou");
			SetMenuItemVisibility(tencent, "Ocr接口显示", "Tencent");
			SetMenuItemVisibility(tencent_accurate, "Ocr接口显示", "TencentAccurate");
			SetMenuItemVisibility(youdao, "Ocr接口显示", "Youdao");
			SetMenuItemVisibility(wechat, "Ocr接口显示", "WeChat");
			SetMenuItemVisibility(baimiao, "Ocr接口显示", "Baimiao");
			SetMenuItemVisibility(baidu, "Ocr接口显示", "Baidu");
			SetMenuItemVisibility(baidu_accurate, "Ocr接口显示", "BaiduAccurate");
			SetMenuItemVisibility(Mathfuntion, "Ocr接口显示", "Mathfuntion");
			SetMenuItemVisibility(ocr_table, "Ocr接口显示", "Table");
			SetMenuItemVisibility(shupai, "Ocr接口显示", "Shupai");

			// OCR 子菜单接口可见性设置
			SetMenuItemVisibility(baidu_table, "Ocr接口显示", "TableBaidu");
			SetMenuItemVisibility(ali_table, "Ocr接口显示", "TableAli");
			SetMenuItemVisibility(left_right, "Ocr接口显示", "ShupaiLR");
			SetMenuItemVisibility(righ_left, "Ocr接口显示", "ShupaiRL");

			// 翻译接口可见性设置
			SetMenuItemVisibility(trans_google, "翻译接口显示", "Google");
			SetMenuItemVisibility(trans_baidu, "翻译接口显示", "Baidu");
			SetMenuItemVisibility(trans_tencent, "翻译接口显示", "Tencent");
			SetMenuItemVisibility(trans_bing, "翻译接口显示", "Bing");
			SetMenuItemVisibility(trans_bing2, "翻译接口显示", "Bing2");
			SetMenuItemVisibility(trans_microsoft, "翻译接口显示", "Microsoft");
			SetMenuItemVisibility(trans_yandex, "翻译接口显示", "Yandex");
			SetMenuItemVisibility(trans_tencentinteractive, "翻译接口显示", "TencentInteractive");
			SetMenuItemVisibility(trans_caiyun, "翻译接口显示", "Caiyun");
			SetMenuItemVisibility(trans_volcano, "翻译接口显示", "Volcano");
			SetMenuItemVisibility(trans_caiyun2, "翻译接口显示", "Caiyun2");
		}

		/// <summary>
		/// 根据配置文件中指定节和键的值设置菜单项的可见性
		/// </summary>
		/// <param name="menuItem">要设置可见性的菜单项</param>
		/// <param name="section">配置文件中的节名称</param>
		/// <param name="key">节中的键名称</param>
		private void SetMenuItemVisibility(ToolStripItem menuItem, string section, string key)
		{
			if (menuItem != null)
			{
				string visibilityValue = IniHelper.GetValue(section, key);
				bool isVisible = visibilityValue == "发生错误" || Convert.ToBoolean(visibilityValue);
				menuItem.Visible = isVisible;
			}
		}

// ====================================================================================================================
		// **字段声明**
		//
		// 定义了 FmMain 类中使用的所有字段（成员变量）。
		// 这些字段用于存储窗体的状态、配置信息、OCR 和翻译结果、图像数据以及其他在整个类中需要共享的数据。
		// ====================================================================================================================
		#region 字段声明

		/// OCR接口标识，用于标识当前使用的OCR接口类型
		public string interface_flag;

		/// 语言标识，用于标识当前处理的文本语言类型
		public string language;

		
		/// 分割文本内容，用于存储OCR识别后经过分割处理的文本
		public string split_txt;

	
		/// 注释文本内容
		public string note;

		
		/// 空格字符，用于文本处理时的空格表示
		public string spacechar;

		
		/// RichTextBox1的注释内容
		public string richTextBox1_note;

		/// 翻译标志，用于标识翻译功能是否开启
		public string transtalate_fla;

		/// 加载窗口实例，用于显示加载动画
		public FmLoading fmloading;

		/// 线程实例，用于执行耗时操作
		public Thread thread;

		/// 菜单项实例，用于设置相关功能
		public MenuItem Set;

		/// Google翻译文本内容
		public string googleTranslate_txt;

		/// 成功计数器，用于记录操作成功的次数
		public int num_ok;

		/// 激活状态标识，用于标识当前窗口是否处于激活状态
		public bool bolActive;

		/// 腾讯VIP标识，用于标识是否使用腾讯VIP服务
		public bool tencent_vip_f;

		/// 自动标志，用于标识自动功能是否开启
		public string auto_fla;

		/// 百度VIP标识，用于标识是否使用百度VIP服务
		public string baidu_vip;

		/// HTML文本内容
		public string htmltxt;

		/// 提示文本，用于显示系统提示信息
		public static string TipText;

		/// 朗读状态标识，用于标识是否正在进行文本朗读
		public bool speaking;

		/// 朗读复制标识，用于标识是否需要复制并朗读文本
		public static bool speak_copy;

		/// 朗读复制标志，用于控制朗读复制功能
		public string speak_copyb;

		/// 朗读停止标志，用于控制文本朗读的停止
		public string speak_stop;

		/// TTS数据，用于存储文本转语音的音频数据
		public byte[] ttsData;

		/// 公共注释数组，用于存储公共注释内容
		public string[] pubnote;

		/// 注释窗口实例，用于显示注释内容
		public FmNote fmNote;

		/// 屏幕截图图像，用于存储屏幕截图内容
		public Image image_screen;

		/// 语音计数器，用于记录语音相关操作的次数
		public int voice_count;

		/// 窗体宽度，用于存储窗体的宽度值
		public int form_width;

		/// 窗体高度，用于存储窗体的高度值
		public int form_height;

		/// QQ截图更改标识，用于标识QQ截图功能是否启用
		public bool change_QQ_screenshot;

		/// 标志窗口实例，用于显示标志相关内容
		private FmFlags fmflags;

		/// 翻译热键，用于存储翻译功能的快捷键
		public string trans_hotkey;

		/// 时间间隔，用于存储时间间隔信息
		public TimeSpan ts;

		/// ESC定时器，用于ESC相关操作的定时控制
		public Timer esc_timer;

	
		/// ESC线程，用于执行ESC相关操作
		public Thread esc_thread;

		
		/// ESC标志，用于标识ESC操作的状态
		public string esc;

	
		/// 语言标志，用于标识当前使用的语言类型
		private string languagle_flag;

	
		/// 获取TKK的JavaScript代码，用于Google翻译相关功能
		public static string GetTkkJS;

	
		/// 排版文本，用于存储经过排版处理的文本内容
		public string typeset_txt;

	
		/// 百度标志，用于标识百度相关功能的状态
		public string baidu_flags;

		
		/// 截图排斥标识，用于控制截图功能的排斥行为
		public bool 截图排斥;

		
		/// 原始图像，用于存储处理前的原始图像内容
		private Image image_ori;

		
		/// 竖排右侧文本，用于存储竖排文本的右侧内容
		public string shupai_Right_txt;

		
		/// 自动重置事件，用于线程同步控制
		private AutoResetEvent are;

		
		/// 百度Cookie，用于百度相关服务的身份验证
		public string baiducookies;


		/// 竖排左侧文本，用于存储竖排文本的左侧内容
		public string shupai_Left_txt;

		
		/// 图像数组，用于存储多个图像对象
		public Image[] image_arr;

		
		/// 百度OCR参数A，用于百度OCR服务的参数配置
		public string OCR_baidu_a;

		
		/// 百度OCR参数B，用于百度OCR服务的参数配置
		public string OCR_baidu_b;

		
		/// 图像数组列表，用于存储图像对象列表
		public List<Image> imgArr;

		
		/// 图像列表，用于存储图像对象集合
		public List<Image> imagelist;

		
		/// 图像列表长度，用于存储图像列表的长度信息
		public int imagelist_lenght;

		
		/// 百度OCR参数D，用于百度OCR服务的参数配置
		public string OCR_baidu_d;

		
		/// 百度OCR参数C，用于百度OCR服务的参数配置
		public string OCR_baidu_c;

		
		/// 百度OCR参数E，用于百度OCR服务的参数配置
		public string OCR_baidu_e;

		
		/// 图像编号数组，用于存储图像的编号信息
		public int[] image_num;

		
		/// 代理标志，用于标识代理设置的状态
		public string Proxy_flag;

		
		/// 代理URL，用于配置代理服务器地址
		public string Proxy_url;

		
		/// 代理端口，用于配置代理服务器端口
		public string Proxy_port;

		
		/// 代理用户名，用于代理服务器身份验证
		public string Proxy_name;

		
		/// 代理密码，用于代理服务器身份验证
		public string Proxy_password;

		
		/// 拼音标志，用于标识是否启用拼音功能
		public bool pinyin_flag;

		
		/// 分割标志，用于标识文本分割功能是否启用
		public bool set_split;

		
		/// 合并标志，用于标识文本合并功能是否启用
		public bool set_merge;

		
		/// 翻译点击标识，用于标识翻译功能的点击状态
		public bool tranclick;

		
		/// 自定义文本框内容，用于存储自定义文本框的文本
		public string myjsTextBox;

		
		/// OCR订单标志，用于标识OCR订单相关状态
		private string flags_ocrorder;

		
		/// 首行标识，用于标识首行相关设置
		public int first_line;

		
		/// 段落标识，用于标识段落处理状态
		public bool paragraph;

		
		/// Web浏览器控件，用于内嵌浏览器功能
		private WebBrowser webBrowser;

		
		/// 腾讯Cookie，用于腾讯相关服务的身份验证
		public string tencent_cookie;

		
		/// 阿里表格实例，用于处理阿里表格相关功能
		private AliTable ailibaba;
#endregion

// ====================================================================================================================
		// **内部类、委托与枚举**
		//
		// 包含了 FmMain 类内部使用的辅助类型定义。
		// - 委托 (Delegates): 定义了用于跨线程调用的委托类型，如 `Translate` 和 `OcrThread`。
		// - 内部类 (Inner Classes):
		//   - AutoClosedMsgBox: 一个可以自动关闭的消息框。
		//   - TransObj, TransResult, Rootobject, Trans_result: 用于反序列化百度翻译 API 返回的 JSON 结果。
		//   - HtmlToText: 用于将 HTML 内容转换为纯文本。
		// - 枚举 (Enum):
		//   - MsgBoxStyle: 定义了消息框的样式。
		// ====================================================================================================================
		#region 内部类、委托与枚举
		public delegate void Translate();

		public delegate void OcrThread();

		public delegate int Dllinput(string command);

		public class AutoClosedMsgBox
		{

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			[DllImport("user32.dll")]
			private static extern bool EndDialog(IntPtr hDlg, int nResult);

			[DllImport("user32.dll")]
			private static extern int MessageBoxTimeout(IntPtr hwnd, string txt, string caption, int wtype, int wlange, int dwtimeout);

			public static int Show(string text, string caption, int milliseconds, MsgBoxStyle style)
			{
				return MessageBoxTimeout(IntPtr.Zero, text, caption, (int)style, 0, milliseconds);
			}

			public static int Show(string text, string caption, int milliseconds, int style)
			{
				return MessageBoxTimeout(IntPtr.Zero, text, caption, style, 0, milliseconds);
			}

			private const int WM_CLOSE = 16;
		}

		public enum MsgBoxStyle
		{

			OK,

			OKCancel,

			AbortRetryIgnore,

			YesNoCancel,

			YesNo,

			RetryCancel,

			CancelRetryContinue,

			RedCritical_OK = 16,

			RedCritical_OKCancel,

			RedCritical_AbortRetryIgnore,

			RedCritical_YesNoCancel,

			RedCritical_YesNo,

			RedCritical_RetryCancel,

			RedCritical_CancelRetryContinue,

			BlueQuestion_OK = 32,

			BlueQuestion_OKCancel,

			BlueQuestion_AbortRetryIgnore,

			BlueQuestion_YesNoCancel,

			BlueQuestion_YesNo,

			BlueQuestion_RetryCancel,

			BlueQuestion_CancelRetryContinue,

			YellowAlert_OK = 48,

			YellowAlert_OKCancel,

			YellowAlert_AbortRetryIgnore,

			YellowAlert_YesNoCancel,

			YellowAlert_YesNo,

			YellowAlert_RetryCancel,

			YellowAlert_CancelRetryContinue,

			BlueInfo_OK = 64,

			BlueInfo_OKCancel,

			BlueInfo_AbortRetryIgnore,

			BlueInfo_YesNoCancel,

			BlueInfo_YesNo,

			BlueInfo_RetryCancel,

			BlueInfo_CancelRetryContinue
		}

		[Serializable]
		public class TransObj
		{

			public string From
			{
				get => from;
				set => from = value;
			}

			public string To
			{
				get => to;
				set => to = value;
			}

			public List<TransResult> Data
			{
				get => data;
				set => data = value;
			}

			public List<TransResult> data;

			public string from;

			public string to;
		}

		[Serializable]
		public class TransResult
		{

			public string Src
			{
				get => src;
				set => src = value;
			}

			public string Dst
			{
				get => dst;
				set => dst = value;
			}

			public string dst;

			public string src;
		}

		private class HtmlToText
		{

			static HtmlToText()
			{
				Tags.Add("address", "\n");
				Tags.Add("blockquote", "\n");
				Tags.Add("div", "\n");
				Tags.Add("dl", "\n");
				Tags.Add("fieldset", "\n");
				Tags.Add("form", "\n");
				Tags.Add("h1", "\n");
				Tags.Add("/h1", "\n");
				Tags.Add("h2", "\n");
				Tags.Add("/h2", "\n");
				Tags.Add("h3", "\n");
				Tags.Add("/h3", "\n");
				Tags.Add("h4", "\n");
				Tags.Add("/h4", "\n");
				Tags.Add("h5", "\n");
				Tags.Add("/h5", "\n");
				Tags.Add("h6", "\n");
				Tags.Add("/h6", "\n");
				Tags.Add("p", "\n");
				Tags.Add("/p", "\n");
				Tags.Add("table", "\n");
				Tags.Add("/table", "\n");
				Tags.Add("ul", "\n");
				Tags.Add("/ul", "\n");
				Tags.Add("ol", "\n");
				Tags.Add("/ol", "\n");
				Tags.Add("/li", "\n");
				Tags.Add("br", "\n");
				Tags.Add("/td", "\t");
				Tags.Add("/tr", "\n");
				Tags.Add("/pre", "\n");
				IgnoreTags = new HashSet<string>();
				IgnoreTags.Add("script");
				IgnoreTags.Add("noscript");
				IgnoreTags.Add("style");
				IgnoreTags.Add("object");
			}

			public string Convert(string html)
			{
				_text = new TextBuilder();
				_html = html;
				_pos = 0;
				while (!EndOfText)
				{
					if (Peek() == '<')
					{
						bool flag;
						var text = ParseTag(out flag);
						if (text == "body")
						{
							_text.Clear();
						}
						else if (text == "/body")
						{
							_pos = _html.Length;
						}
						else if (text == "pre")
						{
							_text.Preformatted = true;
							EatWhitespaceToNextLine();
						}
						else if (text == "/pre")
						{
							_text.Preformatted = false;
						}
						string s;
						if (Tags.TryGetValue(text, out s))
						{
							_text.Write(s);
						}
						if (IgnoreTags.Contains(text))
						{
							EatInnerContent(text);
						}
					}
					else if (char.IsWhiteSpace(Peek()))
					{
						_text.Write(_text.Preformatted ? Peek() : ' ');
						MoveAhead();
					}
					else
					{
						_text.Write(Peek());
						MoveAhead();
					}
				}
				return HttpUtility.HtmlDecode(_text.ToString());
			}

			protected string ParseTag(out bool selfClosing)
			{
				var result = string.Empty;
				selfClosing = false;
				if (Peek() == '<')
				{
					MoveAhead();
					EatWhitespace();
					var pos = _pos;
					if (Peek() == '/')
					{
						MoveAhead();
					}
					while (!EndOfText && !char.IsWhiteSpace(Peek()) && Peek() != '/' && Peek() != '>')
					{
						MoveAhead();
					}
					result = _html.Substring(pos, _pos - pos).ToLower();
					while (!EndOfText && Peek() != '>')
					{
						if (Peek() == '"' || Peek() == '\'')
						{
							EatQuotedValue();
						}
						else
						{
							if (Peek() == '/')
							{
								selfClosing = true;
							}
							MoveAhead();
						}
					}
					MoveAhead();
				}
				return result;
			}

			protected void EatInnerContent(string tag)
			{
				var b = "/" + tag;
				while (!EndOfText)
				{
					if (Peek() == '<')
					{
						bool flag;
						if (ParseTag(out flag) == b)
						{
							return;
						}
						if (!flag && !tag.StartsWith("/"))
						{
							EatInnerContent(tag);
						}
					}
					else
					{
						MoveAhead();
					}
				}
			}

			protected bool EndOfText => _pos >= _html.Length;

			protected char Peek()
			{
				if (_pos >= _html.Length)
				{
					return '\0';
				}
				return _html[_pos];
			}

			protected void MoveAhead()
			{
				_pos = Math.Min(_pos + 1, _html.Length);
			}

			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(Peek()))
				{
					MoveAhead();
				}
			}

			private void EatWhitespaceToNextLine()
			{
				while (char.IsWhiteSpace(Peek()))
				{
					var num = (int)Peek();
					MoveAhead();
					if (num == 10)
					{
						break;
					}
				}
			}

			private void EatQuotedValue()
			{
				var c = Peek();
				if (c == '"' || c == '\'')
				{
					MoveAhead();
					_pos = _html.IndexOfAny(new[]
					{
						c,
						'\r',
						'\n'
					}, _pos);
					if (_pos < 0)
					{
						_pos = _html.Length;
						return;
					}
					MoveAhead();
				}
			}

			private static readonly Dictionary<string, string> Tags = new Dictionary<string, string>();

			private static readonly HashSet<string> IgnoreTags;

			protected TextBuilder _text;

			private string _html;

			private int _pos;

			protected class TextBuilder
			{

				public TextBuilder()
				{
					_text = new StringBuilder();
					_curLine = new StringBuilder();
					_emptyLines = 0;
					_preformatted = false;
				}

				public bool Preformatted
				{
					get => _preformatted;
					set
					{
						if (value)
						{
							if (_curLine.Length > 0)
							{
								FlushCurLine();
							}
							_emptyLines = 0;
						}
						_preformatted = value;
					}
				}

				public void Clear()
				{
					_text.Length = 0;
					_curLine.Length = 0;
					_emptyLines = 0;
				}

				public void Write(string s)
				{
					foreach (var c in s)
					{
						Write(c);
					}
				}

				public void Write(char c)
				{
					if (_preformatted)
					{
						_text.Append(c);
						return;
					}
					if (c != '\r')
					{
						if (c == '\n')
						{
							FlushCurLine();
							return;
						}
						if (char.IsWhiteSpace(c))
						{
							var length = _curLine.Length;
							if (length == 0 || !char.IsWhiteSpace(_curLine[length - 1]))
							{
								_curLine.Append(' ');
							}
						}
						else
						{
							_curLine.Append(c);
						}
					}
				}

				private void FlushCurLine()
				{
					var text = _curLine.ToString().Trim();
					if (text.Replace("\u00a0", string.Empty).Length == 0)
					{
						_emptyLines++;
						if (_emptyLines < 2 && _text.Length > 0)
						{
							_text.AppendLine(text);
						}
					}
					else
					{
						_emptyLines = 0;
						_text.AppendLine(text);
					}
					_curLine.Length = 0;
				}

				public override string ToString()
				{
					if (_curLine.Length > 0)
					{
						FlushCurLine();
					}
					return _text.ToString();
				}

				private readonly StringBuilder _text;

				private readonly StringBuilder _curLine;

				private int _emptyLines;

				private bool _preformatted;
			}
		}

		// 计算MD5值
		public static string EncryptString(string str)
		{
			var md5 = MD5.Create();
			// 将字符串转换成字节数组
			var byteOld = Encoding.UTF8.GetBytes(str);
			// 调用加密方法
			var byteNew = md5.ComputeHash(byteOld);
			// 将加密结果转换为字符串
			var sb = new StringBuilder();
			foreach (var b in byteNew)
				// 将字节转换成16进制表示的字符串，
				sb.Append(b.ToString("x2"));
			// 返回加密的字符串
			return sb.ToString();
		}
		#endregion
	}
	public class Rootobject
	{
		public string from { get; set; }
		public string to { get; set; }
		public Trans_result[] trans_result { get; set; }
		public string error_code { get; set; }
		public string error_msg { get; set; }
	}

	public class Trans_result
	{
		public string src { get; set; }
		public string dst { get; set; }
	}
}
