using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
	public partial class FmMain
	{
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
					new MenuItem("静默识别", traySilentOcrClick),
					new MenuItem("截图翻译", trayScreenshotTranslateClick),
					new MenuItem("输入翻译", trayInputTranslateClick),
					new MenuItem("监听翻译", trayClipListenTranslateClick),
					new MenuItem("显示", trayShowClick),
					new MenuItem("设置", tray_Set_Click),
					new MenuItem("更新", tray_update_Click),
					new MenuItem("帮助", tray_help_Click),
					new MenuItem("重启", trayRestartClick), // 【新增】重启菜单项
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
		/// 托盘图标双击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		/// <summary>
		/// 【新增】托盘图标鼠标按下事件
		/// </summary>
		private void tray_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// 每当左键按下，计数器加 1
				trayClickCount++;

				if (trayClickCount == 1)
				{
					// 如果这是第一次点击，启动定时器
					// 定时器将等待系统双击那么长的时间
					trayClickTimer.Start();
				}
			}
		}

	/// <summary>
    /// 【修改】定时器触发事件：检查时间窗口内的总点击次数
    /// </summary>
    private void trayClickTimer_Tick(object sender, EventArgs e)
    {
        // 1. 定时器到期，立即停止
        trayClickTimer.Stop();

        // 2. 检查在时间窗口内总共发生了几次点击
        if (trayClickCount == 1)
        {
            // 如果只有 1 次点击，判定为“单击”，执行“切换显示/隐藏”
				if (this.Visible)
				{ 	
					// 隐藏：先解绑
            		RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
					 // 【新增】防止后台读秒
            		if (translationTimer != null) translationTimer.Stop(); 
					// 如果窗口当前可见，则隐藏它
					this.Hide();
					this.Visible = false;
				}
				else
				{
					// 如果窗口当前不可见，则调用 trayShowClick (它会负责 Show, Activate, TopMost 等)
					trayShowClick(sender, e);
				}
        }
        else if (trayClickCount >= 2)
        {
            // 如果有 2 次或更多点击，判定为“双击”
            // (这里我们执行原 tray_double_Click 的逻辑)
            HelpWin32.UnregisterHotKey(Handle, 205);
            menu.Hide();
            RichBoxBody.Hide = "";
            RichBoxBody_T.Hide = "";
            MainOCRQuickScreenShots();
        }

        // 3. 无论结果如何，重置计数器
        trayClickCount = 0;
    }

		/// <summary>
		/// 托盘菜单"显示"选项点击事件处理函数
		/// 显示并激活主窗口
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void trayShowClick(object sender, EventArgs e)
		{
			Debug.WriteLine("托盘菜单点击了显示主窗口");
			Show();
			Activate();
			Visible = true;
			WindowState = FormWindowState.Normal;
			TopMost = IniHelper.GetValue("工具栏", "顶置") == "True";
		
			// 1. 【核心修正】等窗口完全显示、状态稳定了，再绑定事件
			// 这样可以避开 Show() 过程中可能产生的任何“误触”
			RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged; 
			RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged; 
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
			SaveWindowState();
			OcrHelper.Dispose();
			PaddleOCRHelper.Reset();
			PaddleOCR2Helper.Reset();
            RapidOCRHelper.Reset();
            Process.GetCurrentProcess().Kill();
		}

		/// <summary>
		/// 托盘菜单"重启"选项点击事件处理函数
		/// 保存配置、释放资源并重启应用程序
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void trayRestartClick(object sender, EventArgs e)
		{
            // 1. 隐藏并释放托盘图标，防止出现“幽灵图标”
            minico.Visible = false;
			minico.Dispose();
            
            // 2. 保存当前状态
			saveIniFile();
			SaveWindowState();
            
            // 3. 释放引擎资源
			OcrHelper.Dispose();
			PaddleOCRHelper.Reset();
			PaddleOCR2Helper.Reset();
            RapidOCRHelper.Reset();

            // 4. 重启应用程序
            Application.Restart();
            
            // 5. 确保当前进程被完全结束
			Process.GetCurrentProcess().Kill();
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
			HelpWin32.UnregisterHotKey(Handle, 250);
			HelpWin32.UnregisterHotKey(Handle, 260);
			
			WindowState = FormWindowState.Minimized;
			var fmSetting = new FmSetting();
			if (settingWindowSize.Width > 574) 
			{
                fmSetting.Size = settingWindowSize;
			}
			fmSetting.TopMost = true;
			fmSetting.ShowDialog();
            //设置窗口关闭后
            settingWindowSize = fmSetting.Size; // 窗口关闭后，记录它最后的大小

			 //刷新 AI 菜单，这行代码写在fmsetting里也行，写在这里也行
			LoadCustomOpenAIMenus();
			LoadCustomOpenAITransMenus();
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
				if (IniHelper.GetValue("快捷键", "静默识别") != "请按下快捷键")
				{
					var value5 = IniHelper.GetValue("快捷键", "静默识别");
					SetHotkey("None", "", value5, 250);
				}
				if (IniHelper.GetValue("快捷键", "截图翻译") != "请按下快捷键")
				{
					var value5 = IniHelper.GetValue("快捷键", "截图翻译");
					SetHotkey("None", "", value5, 260);
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
				StaticValue.TX_TABLE_API_ID = IniHelper.GetValue("密钥_腾讯表格v3", "secret_id");
				if (StaticValue.TX_TABLE_API_ID == "发生错误")
				{
					StaticValue.TX_TABLE_API_ID = "";
				}
				StaticValue.TX_TABLE_API_KEY = IniHelper.GetValue("密钥_腾讯表格v3", "secret_key");
				if (StaticValue.TX_TABLE_API_KEY == "发生错误")
				{
					StaticValue.TX_TABLE_API_KEY = "";
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
				// 重新加载百度表格识别密钥
				StaticValue.BD_TABLE_API_ID = IniHelper.GetValue("密钥_百度表格", "secret_id");
				if (StaticValue.BD_TABLE_API_ID == "发生错误")
				{
					StaticValue.BD_TABLE_API_ID = "";
				}
				StaticValue.BD_TABLE_API_KEY = IniHelper.GetValue("密钥_百度表格", "secret_key");
				if (StaticValue.BD_TABLE_API_KEY == "发生错误")
				{
					StaticValue.BD_TABLE_API_KEY = "";
				}
				// 【新增】重新加载百度手写识别密钥
				StaticValue.BD_HANDWRITING_API_ID = TrOCRUtils.LoadSetting("密钥_百度手写", "secret_id", "");
				StaticValue.BD_HANDWRITING_API_KEY = TrOCRUtils.LoadSetting("密钥_百度手写", "secret_key", "");
				StaticValue.BD_HANDWRITING_LANGUAGE = TrOCRUtils.LoadSetting("密钥_百度手写", "language_code", "CHN_ENG");
	
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
					StaticValue.Translate_Current_API = "Bing2";
				}
				LoadTranslateConfig();
				InitializeApiMenus();
				  // --- 【关键新增】刷新工具栏图标状态 ---
        		this.RichBoxBody.readIniFile();
        		this.RichBoxBody_T.readIniFile();
			}
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
		/// 点击托盘更新菜单项时检查程序更新
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void tray_update_Click(object sender, EventArgs e)
		{
			UpdateChecker.CheckUpdate();
		}

		/// <summary>
		/// 托盘菜单"静默识别"选项点击事件处理函数
		/// </summary>
		private void traySilentOcrClick(object sender, EventArgs e)
		{
            MainSilentOcr();
            // // 抛出一个带有自定义消息的通用异常
            // throw new Exception("这是一个测试异常，用于验证 PDB 路径映射是否生效！");

        }

		/// <summary>
		/// 主静默OCR功能
		/// </summary>
		public void MainSilentOcr()
		{
		    isSilentMode = true;
		    MainOCRQuickScreenShots();
		}

		private void trayScreenshotTranslateClick(object sender, EventArgs e)
		{
		    // 1. 激活截图翻译模式标志
		    isScreenshotTranslateMode = true;

		    // 2. （可选但推荐）确保其他模式的标志是关闭的，避免冲突
		    isSilentMode = false;

		    // 3. 调用通用的截图方法，启动流程
		    MainOCRQuickScreenShots();
		}

        /// <summary>
        /// 托盘菜单"输入翻译"选项点击事件处理函数
        /// 重置翻译界面并显示主输入窗口，根据配置填充剪贴板内容
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void trayInputTranslateClick(object sender, EventArgs e)
		{
			// 标记这不是OCR流程 
			isContentFromOcr = false;
			isFromClipboardListener = false;

            // 【新增】重置界面时，确保没有残留的自动翻译任务
            if (translationTimer != null) translationTimer.Stop();

            // 1. 重置翻译界面，确保只显示主输入窗口			
            RichBoxBody_T.Visible = false;
			PictureBox1.Visible = false;
			RichBoxBody_T.Text = "";

			// 2. 恢复原始窗口大小
			if (WindowState == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Normal;
			}
            transtalate_fla = "关闭";
			// MinimumSize = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
            // Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
            this.Size = this.lastNormalSize;
            RichBoxBody.Dock = DockStyle.Fill;          

			// 3. 准备文本内容
			bool hasContentToTranslate = false;
			RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged; // 关键：在设置文本前，先断开事件处理，避免触发不必要的逻辑
			try
			{
				if (StaticValue.InputTranslateClipboard && Clipboard.ContainsText())
				{
					string clipboardText = Clipboard.GetText();
					string textToDisplay = clipboardText; // 默认显示原始剪贴板文本

					// --- 新增的核心逻辑：检查并执行自动合并 ---
					if (bool.Parse(IniHelper.GetValue("工具栏", "合并"))) // 检查是否开启了自动合并
					{
						if (!string.IsNullOrEmpty(textToDisplay))
						{
							string finalText;
                            if (StaticValue.IsMergeRemoveAllSpace)
                            {
                                finalText = Regex.Replace(textToDisplay, @"[\r\n 　]+", "");
                            }
							else
                            {
                                // 只有在“非移除所有空格”模式下，才调用原来的智能合并方法
                                finalText = PerformIntelligentMerge(textToDisplay, StaticValue.IsMergeRemoveSpace);
                            }
							textToDisplay = finalText;

							// 应用“合并后自动复制”设置
							if (StaticValue.IsMergeAutoCopy && !string.IsNullOrEmpty(finalText))
							{
								SetClipboardWithLock(finalText);
								Debug.WriteLine("合并后自动复制成功：" + finalText);								
							}
						}
					}

					RichBoxBody.Text = textToDisplay; // 将最终处理好的文本设置到输入框

					if (!string.IsNullOrEmpty(clipboardText))
					{
						hasContentToTranslate = true;
					}
				}
				else
				{
					RichBoxBody.Text = "";
				}
			}
			finally
			{
				RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged; // 关键：重新订阅事件，以便后续的手动编辑和自动翻译功能正常工作
			}

			// 4. 显示并激活窗口
			Show();
			Activate();
			Visible = true;
			WindowState = FormWindowState.Normal;
			TopMost = IniHelper.GetValue("工具栏", "顶置") == "True";

			// --- 【核心修正】采用更可靠的三步刷新逻辑 ---
			// 步骤 a: 显式地将焦点设置到文本框，确保它是活动控件
			RichBoxBody.Focus();
			// 步骤 b: 处理当前所有Windows消息，确保窗体已完全加载并获得焦点
			Application.DoEvents();
			// 步骤 c: 在窗体和控件完全就绪后，再强制刷新，确保渲染正确
			RichBoxBody.Refresh();
			// --- 刷新逻辑结束 ---

			// 5. 如果有内容且开启了自动翻译，则手动启动翻译流程
			if (hasContentToTranslate && StaticValue.InputTranslateAutoTranslate)
			{
				TransClick();
			}
		}

        /// <summary>
        /// 托盘菜单"监听翻译"选项点击事件处理函数
        /// 用于开启或关闭监听剪贴板进行翻译的功能
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void trayClipListenTranslateClick(object sender, EventArgs e)
		{
			// 1. 切换核心功能状态 (将 StaticValue 中的布尔值取反)
			StaticValue.ListenClipboardTranslation = !StaticValue.ListenClipboardTranslation;

			// 2. 持久化设置：将新的状态保存到 config.ini 文件
			IniHelper.SetValue("配置", "ListenClipboard", StaticValue.ListenClipboardTranslation.ToString());
            // 【新增】如果用户选择关闭监听，立即停止正在等待的防抖定时器
            if (!StaticValue.ListenClipboardTranslation)
            {
                if (clipboardDebounceTimer != null)
                {
                    clipboardDebounceTimer.Stop();
                }
            }

            // 3. 给用户一个明确的反馈提示
            if (StaticValue.ListenClipboardTranslation)
			{
				CommonHelper.ShowHelpMsg("监听剪贴板翻译已开启");
			}
			else
			{
				CommonHelper.ShowHelpMsg("监听剪贴板翻译已关闭");
			}
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
		/// 动态计算加载图标(PictureBox1)的位置，使其在翻译文本框(RichBoxBody_T)中居中显示。
		/// </summary>
		private void PositionLoadingIcon()
		{
			// 确保翻译框是可见的，否则无法定位
			if (!RichBoxBody_T.Visible)
			{
				RichBoxBody_T.Visible = true;
			}
		
			// 1. 计算图标应该在的X, Y坐标
			//    公式: 容器的起始位置 + (容器宽度 - 图标宽度) / 2
			int centerX = RichBoxBody_T.Left + (RichBoxBody_T.Width - PictureBox1.Width) / 2;
			int centerY = RichBoxBody_T.Top + (RichBoxBody_T.Height - PictureBox1.Height) / 2;
		
			// 2. 应用新的坐标
			//    使用 Math.Max 确保图标不会因窗口过小而跑到负坐标
			PictureBox1.Location = new Point(Math.Max(0, centerX), Math.Max(0, centerY));
		
			// 3. 确保图标可见并位于最顶层
			PictureBox1.Visible = true;
			PictureBox1.BringToFront();
		}
	}
}
