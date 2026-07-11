using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;
using TrOCR.Services.ScreenCapture;

namespace TrOCR
{
    public partial class FmMain
    {

		/// <summary>
		/// 主OCR快速截图功能
		/// 启动截图功能，隐藏主窗口，调用ShareX库进行区域捕捉
		/// 根据用户的操作（如截图、贴图、保存、多区域选择等）执行不同逻辑
		/// </summary>
		public void MainOCRQuickScreenShots()
		{
			// 如果正在截图则直接返回
			if (StaticValue.IsCapture) return;
            // 【新增】进入截图模式前，停止所有翻译定时器
            if (translationTimer != null) translationTimer.Stop();
            try
			{
				// 隐藏主窗口并准备截图
				change_QQ_screenshot = false;

				// === 纵深防御: 挂起 lastNormalSize 自动更新 ===
				isProgrammaticResize = true;

				// === 先关闭翻译模式，再操作 FormBorderStyle ===
				transtalate_fla = "关闭";

				// 如果工具栏翻译功能关闭，则执行关闭翻译操作
				if (IniHelper.GetValue("工具栏", "翻译") == "False")
				{
					Trans_close_Click(null, EventArgs.Empty, false);
				}

				// === 现在安全地改变 FormBorderStyle ===
				FormBorderStyle = FormBorderStyle.None;
				Visible = false;
				Thread.Sleep(100);

				form_width = Width;
				
				// 初始化相关变量
				shupai_Right_txt = "";
				shupai_Left_txt = "";
				form_height = Height;
				// === 恢复 lastNormalSize 自动更新 ===
				isProgrammaticResize = false;
				minico.Visible = false;
				minico.Visible = true;
				menu.Close();
				menu_copy.Close();
				auto_fla = "开启";
				split_txt = "";

				// --- 步骤 1: 暂时断开事件处理程序 ---
				RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;

				try
				{
				    // --- 步骤 2: 执行“静默”更新 ---
				    // 避免不必要的文本更新
				    if (RichBoxBody.Text != "***该区域未发现文本***")
				    {
				        RichBoxBody.Text = "***该区域未发现文本***";
				    }
				    RichBoxBody_T.Text = "";
				    typeset_txt = "";
				}
				finally
				{
				    // 没必要重新绑定了
				    //RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged;
				}
				
				// 重置窗口大小和边框样式
				// Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
				FormBorderStyle = FormBorderStyle.Sizable;
				
				// 设置截图状态为进行中
				StaticValue.IsCapture = true;
				image_screen?.Dispose();
				// 调用截图功能获取屏幕图像
				var captureResult = screenCaptureService.CaptureForOcr(ScreenCaptureRequest.ForOcr());

				image_screen = captureResult.Image;
				var modeFlag = captureResult.ModeFlag;
				var point = captureResult.FlagLocation;
				var buildRects = captureResult.SelectedRectangles;

				// 如果是静默模式，强制进行OCR，忽略截图工具栏的其他按钮功能
				if (isSilentMode && image_screen != null)
				{
				    modeFlag = "SilentOcrTrigger"; // 使用一个不存在的标志来触发switch case默认的OCR流程
				}
				
				// 如果是高级截图模式，则启动高级截图窗体
				if (modeFlag == "高级截图")
				{
					var annotationResult = screenCaptureService.CaptureAnnotation(image_screen);
					image_screen?.Dispose();
					image_screen = annotationResult.Image;
					modeFlag = annotationResult.ModeFlag;
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
									using (var copy = ImageHelper.CloneBitmap(image_screen))
									{
										if (!ClipboardHelper.TrySetDataObject(copy, out var errorMessage))
										{
											CommonHelper.ShowHelpMsg("剪贴板被占用，截图复制失败", 1600u);
											Debug.WriteLine(errorMessage);
										}
										else
										{
											if (IniHelper.GetValue("截图音效", "粘贴板") == "True")
											{
												PlaySong(IniHelper.GetValue("截图音效", "音效路径"));
											}

											CommonHelper.ShowHelpMsg("已复制截图");
										}
									}

									if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
									{
										var value5 = IniHelper.GetValue("快捷键", "翻译文本");
										var text9 = "None";
										var text10 = "F9";
										SetHotkey(text9, text10, value5, 205);
									}
									HelpWin32.UnregisterHotKey(Handle, 222);
									StaticValue.IsCapture = false;
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
			string qrCodeResult = ScanQRCode();
			if (!string.IsNullOrEmpty(qrCodeResult))
			{
				typeset_txt = qrCodeResult;
                split_txt = qrCodeResult;
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
			if (interface_flag == "腾讯表格")
			{
				TxTableOCR();
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
			if (interface_flag == "CustomOpenAI")
			{
				// 调用 FmMain.AI.cs 里的执行方法
				// OCR_Custom_Router(); 
				OCR_OpenAICompatible();
				
				// 善后工作 (关闭加载窗，显示主窗)
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			// 【新增】百度手写识别的分支
			if (interface_flag == "百度手写")
			{
			    OCR_baidu_handwriting();
			    fmloading.FmlClose = "窗体已关闭";
			    Invoke(new OcrThread(Main_OCR_Thread_last));
			    return;
			}
			if (interface_flag == "PaddleOCR")
			{
				OCR_PaddleOCR();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "PaddleOCR2")
			{
				OCR_PaddleOCR2();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
			}
			if (interface_flag == "RapidOCR")
			{
				OCR_RapidOCR();
				fmloading.FmlClose = "窗体已关闭";
				Invoke(new OcrThread(Main_OCR_Thread_last));
				return;
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
				var image2 = bitmap.ToImage<Gray, byte>();
                var image3 = ((Bitmap)FindBundingBox(image2.ToBitmap())).ToImage<Gray, byte>();
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
		public async void Main_OCR_Thread_last()
		{
			try
			{
				LogState("Main_OCR_Thread_last Start");
				// --- 新增的静默模式处理逻辑 ---
				if (isSilentMode)
				{
					isSilentMode = false; // 为下一次操作重置标志

					// 检查识别是否成功
					bool success = typeset_txt != null &&
								   !typeset_txt.Contains("***该区域未发现文本***") &&
								   !string.IsNullOrWhiteSpace(typeset_txt);

					if (success)
					{
						SetClipboardWithLock(typeset_txt);
						Debug.WriteLine("静默[OCR] 识别成功，已复制到剪贴板");
						CommonHelper.ShowHelpMsg("已复制到剪贴板");
					}
					else
					{
						string errorMessage = string.IsNullOrWhiteSpace(typeset_txt) ? "未识别到文本" : typeset_txt.Replace("***", "").Trim();
						CommonHelper.ShowHelpMsg("静默识别失败：" + errorMessage);
					}

					HelpWin32.UnregisterHotKey(Handle, 222); // 注销ESC热键
					StaticValue.IsCapture = false; // 确保截图状态被重置
					image_screen?.Dispose(); // 释放图像资源
					return; // 结束方法，不显示主窗口
				}
				// --- 步骤 1: 数据处理和准备 ---
				// 关键新增：标记当前内容来源于OCR
				isContentFromOcr = true;
				isFromClipboardListener = false;
				image_screen.Dispose();
				StaticValue.IsCapture = false;
				var text = typeset_txt;
				// 【ai流式输出模式下，跳过旧式的标点和空格清洗，信任 AI 的原样输出】
				if (!isStreaming)
				{
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
				}
                StaticValue.v_Split = split_txt;

                string finalTextToShow = text;
				bool shouldPerformCopy = false;
				string textToCopy = "";

				var autoTranslate = bool.Parse(IniHelper.GetValue("工具栏", "翻译"));
				var autoCopyOcr = StaticValue.AutoCopyOcrResult;
				var autoCopyTranslate = StaticValue.AutoCopyOcrTranslation;
				// 处理文本拆分选项
				if ((bool.Parse(IniHelper.GetValue("工具栏", "拆分")) && !isStreaming) || set_split)
				{
					set_split = false;
					finalTextToShow = split_txt;
					// --- 新增: 拆分后自动复制 ---
					if (StaticValue.IsSplitAutoCopy && !string.IsNullOrEmpty(finalTextToShow))
					{
						shouldPerformCopy = true;
						textToCopy = finalTextToShow;
					}
				}
				// 处理文本合并选项
				else if ((bool.Parse(IniHelper.GetValue("工具栏", "合并")) && !isStreaming) || set_merge)
				{
					set_merge = false;
					if (StaticValue.IsMergeRemoveAllSpace)
					{
						finalTextToShow = Regex.Replace(text, @"[\r\n 　]+", "");
					}
					else
					{
						// 只有在“非移除所有空格”模式下，才调用原来的智能合并方法
						finalTextToShow = PerformIntelligentMerge(text, StaticValue.IsMergeRemoveSpace);
					}

				}

				// 计算识别耗时
				var timeSpan = new TimeSpan(DateTime.Now.Ticks);
				var timeSpan2 = timeSpan.Subtract(ts).Duration();
				var str = $"{timeSpan2.Seconds}.{Convert.ToInt32(timeSpan2.TotalMilliseconds)}秒";

				// 处理笔记相关功能
				if (finalTextToShow != null)
				{
					p_note(finalTextToShow);
					StaticValue.v_note = pubnote;
					if (fmNote.Created)
					{
						fmNote.TextNote = "";
					}
				}
				//处理无弹窗配置
				if (IniHelper.GetValue("配置", "识别弹窗") == "False")
				{
					FormBorderStyle = FormBorderStyle.Sizable;
					// Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
					this.Size = this.lastNormalSize;
					Visible = false;
					RichBoxBody.Text = finalTextToShow;
					if (RichBoxBody.Text != "***该区域未发现文本***" && !string.IsNullOrWhiteSpace(RichBoxBody.Text))
					{
						SetClipboardWithLock(RichBoxBody.Text);

						Debug.WriteLine("无弹窗模式复制识别结果成功");
						CommonHelper.ShowHelpMsg("已识别并复制");
					}
					else
					{
						CommonHelper.ShowHelpMsg("无文本");
					}
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

				// --- 步骤 2: 集中进行所有UI更新 ---
				// a. 先让窗口框架稳定
				Text = "耗时：" + str;
				FormBorderStyle = FormBorderStyle.Sizable;
				// 在设置尺寸之前记录一次，这是看到 Bug 的关键
				System.Diagnostics.Debug.WriteLine("Main_OCR_Thread_last: About to set final size.");
				LogState("Main_OCR_Thread_last Before Set Size");
				//Size = new Size(form_width, form_height);
				this.Size = this.lastNormalSize;
				// 在设置尺寸之后再记录一次
				LogState("Main_OCR_Thread_last After Set Size");
				if (StaticValue.v_topmost)
				{
					TopMost = true;
				}
				else
				{
					TopMost = false;
				}
				minico.Visible = true;
				Visible = true;
				Show();
				WindowState = FormWindowState.Normal;
				HelpWin32.SetForegroundWindow(Handle);
                // ====================【核心修改开始】====================

                // a. 隐藏控件，暂停渲染
                // 非流式才隐藏，防止闪烁
                if (!isStreaming)
                {
                    RichBoxBody.Visible = false;
                }

                // b. 在窗口稳定后，再填充文本内容
                RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
                //流式输出时防闪烁赋值逻辑
                if (!isStreaming || RichBoxBody.Text != finalTextToShow)
                {
                    RichBoxBody.Text = finalTextToShow;
                }
                // RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged;
				
				// c. 处理竖排文本（如果需要）
				if (interface_flag == "从右向左")
				{
					RichBoxBody.Text = shupai_Right_txt;
				}
				if (interface_flag == "从左向右")
				{
					RichBoxBody.Text = shupai_Left_txt;
				}
				// (可选) 强制滚动到底部或顶部，有助于解决滚动条问题
				// RichBoxBody.SelectionStart = 0;
				// RichBoxBody.ScrollToCaret();

				// 3. 再次显示控件，强制进行一次完整的、干净的重绘
				RichBoxBody.Visible = true;
				// 检查是否为截图翻译模式(初版)
				// if (isScreenshotTranslateMode)
				// {
				//     // 检查OCR是否成功获取到文本
				//     bool success = !string.IsNullOrWhiteSpace(finalTextToShow) && 
				//                    !finalTextToShow.Contains("***该区域未发现文本***");

				//     if (success)
				//     {
				//         // OCR成功，执行核心操作：
				//         // 1. 将OCR结果放入原文框
				//         RichBoxBody.Text = finalTextToShow;

				//         // 2. 调用翻译方法，并传入 true 来默认隐藏原文
				//         TransClick(true); 
				//     }
				//     else
				//     {
				//         // OCR失败，给用户一个提示，但不显示主窗口
				//         string errorMessage = string.IsNullOrWhiteSpace(typeset_txt) ? "未识别到文本" : typeset_txt.Replace("***", "").Trim();
				//         CommonHelper.ShowHelpMsg("截图翻译失败：" + errorMessage);
				//     }

				//     // 清理并退出，不再执行后续的常规显示逻辑
				//     HelpWin32.UnregisterHotKey(Handle, 222); 
				//     StaticValue.IsCapture = false; 
				//     image_screen?.Dispose();
				//     return; // 【关键】直接返回，中断后续的标准流程
				// }
				// 检查是否为截图翻译模式（包含了显示窗口和不显示窗口两种情况）
				if (isScreenshotTranslateMode)
				{
					bool success = !string.IsNullOrWhiteSpace(finalTextToShow) &&
								   !finalTextToShow.Contains("***该区域未发现文本***");

					if (success)
					{
						// 根据“不显示窗口”的设置决定走哪个流程
						if (StaticValue.NoWindowScreenshotTranslation)
						{
							// 【流程 A：不显示窗口，直接复制（真·静默翻译）】
							this.Hide();
							// 或者
							// this.Visible = false;

							// 异步获取翻译结果
							string translationResult = await GetTranslationAsync(finalTextToShow);

							// 检查翻译是否成功，这行代码会等到翻译完成后才执行，此时 translationResult 是有值的
							if (!string.IsNullOrEmpty(translationResult) && !translationResult.Contains("]："))
							{
								SetClipboardWithLock(translationResult);
								CommonHelper.ShowHelpMsg("译文已复制");
							}
							else
							{
								// 如果翻译出错，也提示用户
								CommonHelper.ShowHelpMsg("翻译失败：" + translationResult);
							}
							// 【关键修正】在此流程的末尾，手动重置标志位
							isScreenshotTranslateMode = false;
						}
						else
						{
							// 【流程 B：显示窗口】

							// 1. 将OCR结果放入原文框
							RichBoxBody.Text = finalTextToShow;

							// 2. 调用翻译方法，并传入 true 来默认隐藏原文
							TransClick(true);
						}
					}
					else
					{
						// OCR失败，不会有翻译流程，所以在这里重置标志是安全的
						isScreenshotTranslateMode = false;
						// OCR失败的提示
						string errorMessage = string.IsNullOrWhiteSpace(typeset_txt) ? "未识别到文本" : typeset_txt.Replace("***", "").Trim();
						CommonHelper.ShowHelpMsg("截图翻译失败：" + errorMessage);
					}

					// 统一的清理工作
					HelpWin32.UnregisterHotKey(Handle, 222);
					StaticValue.IsCapture = false;
					image_screen?.Dispose();
					return; // 中断后续的标准流程
				}

				// ====================【核心修改结束】====================

				// 原有的 RichBoxBody.Refresh() 不再需要，可以删除

				// --- 步骤 3: 在UI完全稳定后，才执行所有可能阻塞的操作（如剪贴板） ---

				if (shouldPerformCopy)
				{
					SetClipboardWithLock(textToCopy);
					Debug.WriteLine("拆分后自动复制成功");

				}
				else
				{
					//// 处理识别后自动复制功能 (只有同时开启了 ① 识别后自动复制 和 ② 自动翻译 和 ③ 翻译后自动复制，才不复制识别结果)
					if (autoCopyOcr && (!autoTranslate || !autoCopyTranslate))
					{
						SetClipboardWithLock(RichBoxBody.Text);
						Debug.WriteLine("识别后自动复制成功");

					}
				}

				// --- 步骤 4: 触发后续逻辑 ---
				// (百度搜索和无弹窗模式会提前返回)
				if (baidu_flags == "百度")
				{
					FormBorderStyle = FormBorderStyle.Sizable;
					// Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
					this.Size = this.lastNormalSize;
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

				// 处理识别后自动翻译功能
				if (autoTranslate)
				{
					try
					{
						auto_fla = "";
						isOcrTranslation = true;
						BeginInvoke(new Translate(TransClick)); // 使用BeginInvoke避免阻塞
					}
					catch { }
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

				// --- 步骤 5: 最后收尾 ---
				// 重新设置热键
				if (IniHelper.GetValue("快捷键", "翻译文本") != "请按下快捷键")
				{
					var value3 = IniHelper.GetValue("快捷键", "翻译文本");
					SetHotkey("None", "F9", value3, 205);
				}
				HelpWin32.UnregisterHotKey(Handle, 222);
			}
			finally
			{
				// ==================== 【状态重置点】 ====================

				// 1. 无论如何，重置流式标记
				isStreaming = false;
				
                if (transtalate_fla == "开启")
                {
                    this.RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
                    this.RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged;
                }
                else
                {
                    this.RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
					 // 【新增】防止后台读秒
            		if (translationTimer != null) translationTimer.Stop(); 
                }

                // 2. 无论如何，确保文本框解锁 (防止万一报错导致文本框锁死)
                // 即使是静默模式，解锁一下也无害
                if (RichBoxBody != null && RichBoxBody.richTextBox1 != null)
				{
					//恢复ocr结果框的工具栏
                    RichBoxBody.SetToolbarEnabled(true);
                    RichBoxBody.richTextBox1.ReadOnly = false;
				}

				// 3. 截图翻译模式重置 (如果之前是在 return 前做的，也可以移到这里)
				// 注意：如果你希望仅在特定情况下重置，保留在上面；
				// 但通常这种状态标记都适合在这里兜底重置
				// isScreenshotTranslateMode = false; 

				LogState("Main_OCR_Thread_last End (Cleanup Done)");
			}
		}


        private void LogState(string eventName)
		{
			// C# 6.0+ string interpolation, simpler
			System.Diagnostics.Debug.WriteLine($"--- {eventName} ---");
			System.Diagnostics.Debug.WriteLine($"  Timestamp: {DateTime.Now:HH:mm:ss.fff}");
			System.Diagnostics.Debug.WriteLine($"  WindowState: {this.WindowState}");
			System.Diagnostics.Debug.WriteLine($"  Size: {this.Size}");
			System.Diagnostics.Debug.WriteLine($"  ClientRectangle.Size: {this.ClientRectangle.Size}");
			System.Diagnostics.Debug.WriteLine($"  transtalate_fla: {this.transtalate_fla ?? "null"}");
			System.Diagnostics.Debug.WriteLine($"  isOriginalTextHidden: {this.isOriginalTextHidden}");
			System.Diagnostics.Debug.WriteLine($"  lastNormalSize: {this.lastNormalSize}");
			System.Diagnostics.Debug.WriteLine("--------------------");
		}

    }
}
