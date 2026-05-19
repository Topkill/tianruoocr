﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace TrOCR
{
	public partial class FmMain
	{
		private void InitiateTranslationUI(string textToShow)
		{
				
		    RichBoxBody_T.Visible = false;
		    RichBoxBody_T.Text = "";

		    if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
            transtalate_fla = "关闭";
            this.Size = this.lastNormalSize;
            RichBoxBody.Dock = DockStyle.Fill;
  
		    RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
		    RichBoxBody.Text = textToShow;
		    RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged;

		    Show();
		    Activate();
		    Visible = true;
		    WindowState = FormWindowState.Normal;
		    TopMost = IniHelper.GetValue("工具栏", "顶置") == "True";

		    if (!string.IsNullOrEmpty(textToShow))
		    {
		        // 判断是否是需要默认隐藏原文的特殊场景
				bool shouldHideOriginal = isFromClipboardListener && StaticValue.ListenClipboardTranslationHideOriginal;
				// 将判断结果作为参数传递给 TransClick
		        TransClick(shouldHideOriginal);

		    }
		}

        private async Task<string> GetTranslationAsync(string textToTranslate, string overrideSource = null, string overrideTarget = null)
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
                case "百度2":
                    sectionName = "Baidu2";
                    break;
                case "火山翻译":
                    sectionName = "Volcano";
                    break;
                default:
                    sectionName = transService;
                    break;
            }

            // 尝试获取翻译配置，如果不存在则使用默认配置（非ai接口）
            if (!StaticValue.Translate_Configs.TryGetValue(sectionName, out var config))
            {
                config = new StaticValue.TranslateConfig { Source = "auto", Target = "自动判断" };
            }

            string toLang;
			string fromLang;
            //如果临时源语言(overrideSource)不为空，则使用它，否则才用配置文件中的
            if (sectionName != "CustomOpenAI")
			{
                fromLang = overrideSource ?? config.Source;
			}
			else
			{
				fromLang= overrideSource ?? _currentCustomTransProvider.Source;

            }

            // 【修改】优先使用临时目标语言
            if (!string.IsNullOrEmpty(overrideTarget))
            {
                toLang = overrideTarget;
            }
            // 根据目标语言配置自动判断需要翻译成的语言
            else if ((sectionName!= "CustomOpenAI" && config.Target == "自动判断")||((sectionName == "CustomOpenAI" && _currentCustomTransProvider.Target == "自动判断")) )
            {
                toLang = "en"; // 默认翻译为英文
                if (StaticValue.ZH2EN)
                {
                    //中文和英文互译逻辑
                    // 中文转英文逻辑：比较中英文字符数量确定源语言
                    if (ch_count(textToTranslate.Trim()) > en_count(textToTranslate.Trim()) || (en_count(textToTranslate.Trim()) == 1 && ch_count(textToTranslate.Trim()) == 1))
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
                    string textToCheck = textToTranslate.Trim();
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
                    if (contain_kor(textToTranslate.Trim()))
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
                    if (sectionName == "CustomOpenAI")
					{	
						toLang = _currentCustomTransProvider.Target;
					}
                    else
                    {
                        toLang = config.Target;
                    }
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
            if (transService == "CustomOpenAI")
            {
                if (fromLang == "en") fromLang = "英文";
                if (toLang == "en") toLang = "英文";
                if (fromLang == "zh-CN") fromLang = "简体中文";
                if (toLang == "zh-CN") toLang = "简体中文";
            }

            // 根据翻译服务调用相应的翻译方法
            switch (transService)
            {
                case "谷歌":
                    googleTranslate_txt = await GTranslateHelper.TranslateAsync(textToTranslate, fromLang, toLang, "google");
                    break;
                case "Bing":
                    googleTranslate_txt = await BingTranslator.TranslateAsync(textToTranslate, fromLang, toLang);
                    break;
                case "Bing2":
                case "BingNew":
                    googleTranslate_txt = await BingTranslator2.TranslateAsync(textToTranslate, fromLang, toLang);
                    break;
                case "Microsoft":
                    googleTranslate_txt = await GTranslateHelper.TranslateAsync(textToTranslate, fromLang, toLang, "microsoft");
                    break;
                case "Yandex":
                    googleTranslate_txt = await GTranslateHelper.TranslateAsync(textToTranslate, fromLang, toLang, "yandex");
                    break;
                case "百度":
                    googleTranslate_txt = TranslateBaidu(textToTranslate, fromLang, toLang, config.AppId, config.ApiKey);
                    break;
                case "腾讯":
                    googleTranslate_txt = Translate_Tencent(textToTranslate, fromLang, toLang, config.AppId, config.ApiKey);
                    break;
                case "腾讯交互翻译":
                    googleTranslate_txt = await TencentTranslator.TranslateAsync(textToTranslate, fromLang, toLang);
                    break;
                case "彩云小译":
                    googleTranslate_txt = await CaiyunTranslator.TranslateAsync(textToTranslate, fromLang, toLang);
                    break;
                case "彩云小译2":
                    if (string.IsNullOrEmpty(config.ApiKey))
                        googleTranslate_txt = "[彩云小译2]：未配置Token";
                    else
                        googleTranslate_txt = await CaiyunTranslator2.TranslateAsync(textToTranslate, fromLang, toLang, config.ApiKey);
                    break;
                case "火山翻译":
                    googleTranslate_txt = await VolcanoTranslator.TranslateAsync(textToTranslate, fromLang, toLang);
                    break;
                case "百度2":
                    googleTranslate_txt = await BaiduTranslator2Helper.TranslateAsync(textToTranslate, fromLang, toLang);
                    break;
                case "CustomOpenAI":
                    googleTranslate_txt = await Trans_OpenAICompatible(textToTranslate, fromLang, toLang);
                    break;
                // =============== 【新增代码结束】 ===============
                default:
                    googleTranslate_txt = await GTranslateHelper.TranslateAsync(textToTranslate, fromLang, toLang, "google");
                    break;
            }
            return googleTranslate_txt;
        }

		/// <summary>
		/// 加载翻译配置信息
		/// 从配置文件中读取各翻译服务的配置信息，包括源语言、目标语言和密钥信息，并存储到静态变量中
		/// </summary>
		private void LoadTranslateConfig()
		{
			StaticValue.Translate_Configs.Clear();
			var services = new[] { "Google", "Baidu", "Tencent", "Bing", "Bing2", "Microsoft", "Yandex", "TencentInteractive", "Caiyun", "Caiyun2", "Volcano","Baidu2" };
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
					// 如果发生错误，使用默认Token值
					if (apiKey == "发生错误")
					{
						apiKey = "3975l6lr5pcbvidl6jl2"; // 默认Token值
						// 保存默认Token到配置文件，避免下次启动时再次出现问题.
						// 这一步导致进入设置页然后关闭设置页写入配置文件时此翻译接口和其他写入的翻译接口配置不在一个区域了，所以我注释掉它了
						// IniHelper.SetValue(section, "Token", apiKey);
					}
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
        /// 启动翻译功能，调整窗体和控件布局以显示翻译界面
        /// </summary>
		public void TransClick()
        {
            // 调用下面的新方法，并传递默认值 false
            TransClick(false);
        }

        public void TransClick(bool hideOriginalDefault =false)
        {
            LogState("TransClick Start");
            // ====================【修复代码】开始 ====================
            // 用户手动点击了翻译，说明用户希望立即看到结果。
            // 此时必须停止自动翻译的倒计时，防止稍后定时器触发导致重复翻译。
            if (translationTimer != null)
            {
                translationTimer.Stop();
                System.Diagnostics.Debug.WriteLine("用户手动触发翻译，已停止自动翻译定时器");
            }
            // ====================【修复代码】结束 ====================

            // 【优化1】立即暂停布局，阻止任何中间状态的绘制
            this.SuspendLayout();

            try
            {
                typeset_txt = RichBoxBody.Text;
                // RichBoxBody_T.Visible = true;
                transtalate_fla = "开启";

				// 【修复】获取当前是否为最大化状态，不要直接强制设为 Normal
				bool isMaximized = (this.WindowState == FormWindowState.Maximized);

				// 如果不是最大化，才强制设为 Normal（防止最小化时点击没反应等情况）
				if (!isMaximized)
				{
					WindowState = FormWindowState.Normal;
				}

                // 解除 Dock，准备手动布局
                RichBoxBody.Dock = DockStyle.None;
                RichBoxBody_T.Dock = DockStyle.None;
                RichBoxBody_T.BorderStyle = BorderStyle.Fixed3D;
                RichBoxBody_T.Text = "";

                // 确保翻译框已初始化
                if (num_ok == 0)
                {
                    RichBoxBody_T.Name = "rich_trans";
                    RichBoxBody_T.TabIndex = 1;
                    RichBoxBody_T.Text_flag = "我是翻译文本框";
                    // 这里不需要设置 Size/Location，下面统一设置
                }
                num_ok++;

                // 显示分隔条
                panelSeparator.Visible = true;
                panelSeparator.Dock = DockStyle.None;

                // ====================【核心布局逻辑】====================

                // 1. 设置主窗口大小 (此时布局已挂起，界面不会闪烁)
                this.Size = new Size(this.lastNormalSize.Width * 2, this.lastNormalSize.Height);

                // 2. 统一设置控件位置和大小
                // 左侧：原文
                RichBoxBody.Visible = true;
                RichBoxBody.Location = new Point(0, 0);
                RichBoxBody.Size = new Size(this.ClientRectangle.Width / 2, this.ClientRectangle.Height);

                // 中间：分隔条 (确保位置紧贴原文)
                panelSeparator.Location = new Point(RichBoxBody.Right, 0);
                panelSeparator.Height = this.ClientRectangle.Height;

                // 右侧：译文
                RichBoxBody_T.Visible = true;
                RichBoxBody_T.Location = new Point(RichBoxBody.Width, 0); // 或者 panelSeparator.Right
                RichBoxBody_T.Size = new Size(RichBoxBody.Width, this.ClientRectangle.Height);

                // 3. 设置按钮
				//只有在全局开关未开启时，才显示和设置按钮
                if (!StaticValue.DisableToggleOriginalButton)
                {
                    btnToggleOriginalText.Visible = true;
                    btnToggleOriginalText.BringToFront();
                    isOriginalTextHidden = false;
                    btnToggleOriginalText.Text = "◀";
                    // 按钮位置跟随分隔条
					// btnToggleOriginalText.Left = RichBoxBody.Right - btnToggleOriginalText.Width - 10;
                    btnToggleOriginalText.Left = panelSeparator.Left - btnToggleOriginalText.Width - 10;
					// btnToggleOriginalText.Top = 5;//或者2
                }

                // 4. 处理“默认隐藏原文”的特殊逻辑
                // 如果参数要求默认隐藏原文，则在显示窗口前，提前模拟一次“隐藏”操作
                // 【核心修改】增加判断：必须在“显隐按钮”没有被全局禁用的前提下，才执行自动隐藏
                if (hideOriginalDefault && !StaticValue.DisableToggleOriginalButton)
                {
					// 更新状态
                    isOriginalTextHidden = true;
                    btnToggleOriginalText.Text = "▶";

                    // 隐藏原文相关的控件
                    RichBoxBody.Visible = false;
                    panelSeparator.Visible = false;

                    // 调整回单栏大小
                    this.Size = this.lastNormalSize;

                    // 译文填满
                    RichBoxBody_T.Location = new Point(0, 0);
                    RichBoxBody_T.Size = this.ClientRectangle.Size;

                    // 按钮靠右
                    btnToggleOriginalText.Left = this.ClientRectangle.Width - btnToggleOriginalText.Width;
                }

                // =========================================================
				//开启文本改变事件
                RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
                RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged;
                // 定位加载图标
                PositionLoadingIcon();
                RichBoxBody.Focus();
            }
            finally
            {
                // 【优化2】恢复布局并强制立即重绘一次，跳过所有中间帧
                this.ResumeLayout(true);
            }

            CheckForIllegalCrossThreadCalls = false;
            trans_Calculate();
            LogState("TransClick End");
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
		public async void trans_Calculate(string overrideSource = null, string overrideTarget = null)
		{
			PositionLoadingIcon();
			if (pinyin_flag)
			{
				try
				{
                    // 如果设置了拼音标志，则将文本转换为拼音
                    googleTranslate_txt = HanToPinyin.GetFullPinyin(typeset_txt);
                }catch (Exception ex)
				{
                    System.Diagnostics.Debug.WriteLine("拼音转换出错: " + ex.Message);
                    googleTranslate_txt = "拼音转换出错：" + ex.Message;
					pinyin_flag= false;
                }
            }
			else if (string.IsNullOrWhiteSpace(typeset_txt))
			{
				// 如果文本为空或只包含空白字符，则翻译结果也为空
				googleTranslate_txt = "";
			}
			else
			{
				googleTranslate_txt = await GetTranslationAsync(typeset_txt, overrideSource, overrideTarget);
			}

			// 隐藏进度图片并将其置于底层
			PictureBox1.Visible = false;
			PictureBox1.SendToBack();
			// 调用翻译完成后的处理方法
			Invoke(new Translate(translate_child));
			// 重置拼音标志
			pinyin_flag = false;
		}

		public void Trans_close_Click(object sender, EventArgs e)
		{
		    // 【修改2】调用新方法，并明确传递 isUserAction: true
		    Trans_close_Click(sender, e, true);
		}

		/// <summary>
		/// 关闭翻译功能的事件处理函数
		/// 当用户点击关闭翻译功能时，此函数将恢复主窗口到原始状态并隐藏翻译相关控件
		/// </summary>
		/// <param name="sender">触发事件的对象</param>
		/// <param name="e">事件参数</param>
		public void Trans_close_Click(object sender, EventArgs e, bool isUserAction = false)
		{
			Debug.WriteLine($"Trans_close_Click-----{sender}------{e}");
			LogState("Trans_close_Click Start"); // <--- 添加这一行
            //解绑文本改变事件
            RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
			 // 【新增】防止后台读秒
            if (translationTimer != null) translationTimer.Stop(); 
            // 只有当这是用户主动点击关闭时 (isUserAction 为 true)，才执行检查
            // ====================【新增代码】开始 ====================
            //这里的代码加不加都行，加上虽然更健壮，但是其实TranslationTimer_Tick里的双重检查就足够了
            // 1. 强制停止翻译定时器
            // 这样即使用户在第 9 秒关闭了窗口，第 10 秒也不会触发请求
            // if (translationTimer != null)
            // {
            // 	translationTimer.Stop();
            // 	Debug.WriteLine("窗口关闭，已强制停止翻译定时器");
            // }
            // ====================【新增代码】结束 ====================
            if (isUserAction && isOriginalTextHidden)
			{
				// 如果原文是隐藏的，则弹出提示，并阻止后续的关闭操作
				MessageBox.Show("请先点击 ▶ 按钮恢复原文，再关闭翻译窗口。", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return; // 直接返回，不执行关闭
			}
			// 重置隐藏/显示按钮和相关状态
			btnToggleOriginalText.Visible = false;
			isOriginalTextHidden = false;
			//RichBoxBody.Visible = true; // 确保原文窗口总是恢复可见，不加这一行也行，我注释了
			// --- 添加结束 ---
			// MinimumSize = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);			
			RichBoxBody_T.Visible = false;
			// ====================【新增代码】====================
			panelSeparator.Visible = false;
			// ===============================================
			PictureBox1.Visible = false;
			RichBoxBody_T.Text = "";
			if (WindowState == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Normal;
			}
			transtalate_fla = "关闭";
			// Size = new Size((int)font_base.Width * 23, (int)font_base.Height * 24);
			this.Size = this.lastNormalSize;
			RichBoxBody.Dock = DockStyle.Fill;
			LogState("Trans_close_Click End");
			// =================================================================
		}

        /// <summary>
        /// 解析自动翻译配置字符串，判断当前接口是否允许自动翻译，并获取延时时间
        /// </summary>
        /// <param name="configStr">配置字符串 (例如: "1000,百度,谷歌" 或 "1000,-Bing")</param>
        /// <param name="currentApi">当前选中的翻译接口名称</param>
        /// <param name="delayMs">输出：解析出的延时时间</param>
        /// <returns>是否允许自动翻译</returns>
        private bool CheckTextChangeAutoTranslateConfig(string configStr, string currentApi, out int delayMs)
        {
            delayMs = 0;
            if (string.IsNullOrWhiteSpace(configStr)) return false;

            // 1. 分割字符串，支持中文或英文逗号
            var parts = configStr.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(p => p.Trim()) // 去除空格
                                 .ToArray();

            // 2. 解析时间 (第一部分必须是数字)
            if (parts.Length == 0 || !int.TryParse(parts[0], out delayMs))
            {
                return false; // 格式错误，第一项不是数字
            }

            // 如果时间 <= 0，直接视为关闭
            if (delayMs <= 0) return false;

            // 3. 如果只有时间 (例如 "1000")，则对所有接口生效
            if (parts.Length == 1) return true;

            // 4. 获取后面的接口列表
            var filters = parts.Skip(1).ToList();

            // 5. 校验格式一致性：要么全是排除(-)，要么全是包含(无-)
            bool isBlacklist = filters.All(f => f.StartsWith("-"));
            bool isWhitelist = filters.All(f => !f.StartsWith("-"));

            if (!isBlacklist && !isWhitelist)
            {
                // 混用了（例如 "1000,百度,-谷歌"），视为配置错误，为了安全不执行
                System.Diagnostics.Debug.WriteLine($"[配置错误] 自动翻译配置不能混用黑白名单: {configStr}");
                return false;
            }

            // 6. 处理接口名称对比
            // 去掉开头的 "-" 号，并使用不区分大小写的比较
            var targetList = filters.Select(f => f.TrimStart('-')).ToHashSet(StringComparer.OrdinalIgnoreCase);
			//如果是ai翻译接口，判断当前的厂商名
			if(currentApi== "CustomOpenAI")
			{
				currentApi = _currentCustomTransProvider.Name;
			}
            if (isWhitelist)
            {
                // 白名单模式：当前接口 必须在 列表中才开启
                // 比如配置 "1000,百度,Bing"，当前是 "百度" -> true
                return targetList.Contains(currentApi);
            }
            else // isBlacklist
            {
                // 黑名单模式：当前接口 不能在 列表中才开启
                // 比如配置 "1000,-Bing"，当前是 "百度" -> true，当前是 "Bing" -> false
                return !targetList.Contains(currentApi);
            }
        }

		/// <summary>
		/// 延时翻译定时器的Tick事件，在用户停止输入后触发翻译
		/// </summary>
		private void TranslationTimer_Tick(object sender, EventArgs e)
		{
			Debug.WriteLine("--------------------------------------------------");
			Debug.WriteLine("===> TranslationTimer_Tick 事件触发！ <===");
			
			translationTimer.Stop();
			// ====================【新增代码】开始 ====================
    		// 【核心双重保险】
    		// 检查：如果翻译功能已关闭(transtalate_fla == "关闭") 或者 窗口不可见
    		// 直接返回，绝不发送请求！
    		if (transtalate_fla == "关闭" || !this.Visible)
    		{
        		Debug.WriteLine("警告：定时器触发时窗口已关闭或隐藏，拦截请求，不执行翻译。");
        		return; 
    		}
    		// ====================【新增代码】结束 ====================
			// 职责单一：只负责计算和更新翻译结果，不处理UI界面切换

			if (string.IsNullOrWhiteSpace(RichBoxBody.Text))
			{
				Debug.WriteLine("    |--> 文本为空，清空翻译结果并返回。");

				// 如果用户清空了原文，则也清空译文
				RichBoxBody_T.Text = "";

				Debug.WriteLine("===> Tick 事件结束。");

				return;
			}
			string textToTranslate = RichBoxBody.Text.Replace("\r", "\\r").Replace("\n", "\\n");
			Debug.WriteLine($"    |--> 文本不为空，准备翻译: \"{textToTranslate}\"");
			typeset_txt = RichBoxBody.Text;
			if (transtalate_fla == "关闭")
			{
				Debug.WriteLine("    |--> TransClick() 已调用，等待翻译结果...");
				TransClick(); 
			}
			else{
				trans_Calculate();
				Debug.WriteLine("    |--> trans_Calculate() 已调用，等待翻译结果...");
			}
			Debug.WriteLine("===> Tick 事件结束。");
		}

		/// <summary>
		/// 将googleTranslate_txt的内容赋值给RichBoxBody_T控件，并清空googleTranslate_txt变量
		/// </summary>
		private void translate_child()
		{
			try
			{
				// 1. 流式输出UI 防闪烁控制
				// 如果不是流式（普通翻译），可以先隐藏；如果是流式，不隐藏，否则打字机效果打完了会闪一下
				if (!isTransStreaming)
				{
					RichBoxBody_T.Visible = false; // 先隐藏
				}

				// 2. 赋值逻辑 (防闪烁核心)
				// 只有在“非流式”或者“流式内容有误(丢包)”的情况下，才强制覆盖文本
				if (!isTransStreaming || RichBoxBody_T.Text != googleTranslate_txt)
				{
					RichBoxBody_T.Text = googleTranslate_txt;
				}
				googleTranslate_txt = "";

				RichBoxBody_T.Visible = true; // 再显示

				// 翻译完成后的统一自动复制逻辑
				bool shouldCopy = false;
				//截图翻译模式是否自动复制译文
				if (isScreenshotTranslateMode)
				{
					shouldCopy = StaticValue.AutoCopyScreenshotTranslation;
				}
				// isContentFromOcr 为 true 意味着当前是对OCR结果的翻译（无论是自动还是手动）
				else if (isContentFromOcr)
				{
					// 检查“OCR翻译后复制”选项
					shouldCopy = StaticValue.AutoCopyOcrTranslation;
				}
				else if (isFromClipboardListener)
				{
					shouldCopy = StaticValue.AutoCopyListenClipboardTranslation;
				}
				else // 两个标志都为 false，则为手动输入,即输入翻译
				{
					shouldCopy = StaticValue.AutoCopyInputTranslation;
				}

				if (shouldCopy && !string.IsNullOrEmpty(RichBoxBody_T.Text))
				{
					SetClipboardWithLock(RichBoxBody_T.Text);
					Debug.WriteLine("翻译后复制成功");

				}

				// 只有在完成一次OCR翻译流程后，才考虑重置标记。如果不清，连续手动翻译OCR结果和编辑后自动重新翻译也能持续享受自动复制。
				// 如果希望每次OCR后只有第一次手动翻译能自动复制，可以在这里重置 isContentFromOcr = false;
				// isContentFromOcr = false;

				// 在每次翻译流程结束后，必须重置监听剪贴板状态标志。否则程序会“卡”在上次的状态，导致后续所有操作逻辑错乱,比如无限翻译。
				// 只重置“一次性”的事件标志，保留“持续性”的状态标志
				// 在每次翻译流程结束后，必须重置所有“一次性”的模式标志
				// 剪贴板监听是一次性事件，必须重置。
				isFromClipboardListener = false;
				isScreenshotTranslateMode = false; // 【重要】确保截图翻译的标志也被重置

				isOcrTranslation = false; // 重置“自动”翻译标记
										  
			}
			finally
			{
                //【状态重置】所有事情做完后，关闭流式标记
                isTransStreaming = false;
                if (RichBoxBody_T != null && RichBoxBody_T.richTextBox1 != null)
                {
                    RichBoxBody_T.SetToolbarEnabled(true);//恢复翻译框的工具栏
                    RichBoxBody_T.richTextBox1.ReadOnly = false;
                }
            }

        }

		/// <summary>
		/// 执行文本翻译操作，根据配置选择不同的翻译服务和语言方向
		/// </summary>
		public async void 翻译文本()
		{
			// 检查是否启用了快速翻译功能
			if (IniHelper.GetValue("配置", "快速翻译") == "True")
			{
                // 在这里也停止定时器，防止用户触发textchange启动翻译定时器后又选中文字后按F9，
                // 为了保险起见，手动介入时最好都停掉自动逻辑
                if (translationTimer != null) translationTimer.Stop();
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

                    data = await GetTranslationAsync(trans_hotkey);
                    // 将翻译结果复制到剪贴板并粘贴到当前焦点位置
                    SetClipboardDataWithLock(DataFormats.UnicodeText, data);        
					Debug.WriteLine("快速翻译译文复制到剪贴板");
					SendKeys.SendWait("^v");
					return;
				}
				catch
				{
                    // 出现异常时也尝试粘贴当前结果
                    SetClipboardDataWithLock(DataFormats.UnicodeText, data);
					Debug.WriteLine("出现异常快速翻译译文也复制到剪贴板");
					SendKeys.SendWait("^v");
					return;
				}
			}
			// 如果未启用快速翻译，则执行常规翻译流程
			SendKeys.SendWait("^c");
			SendKeys.Flush();
			RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
			RichBoxBody.Text = Clipboard.GetText();
			// RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged;
            // 1. 先让窗口以正常状态显示出来
            FormBorderStyle = FormBorderStyle.Sizable;
            Visible = true;
            Show();
            WindowState = FormWindowState.Normal;
            HelpWin32.SetForegroundWindow(StaticValue.mainHandle); // 激活窗口

            // 2. 在窗口已经可见且状态正常后，再调用 TransClick() 来设置双栏布局
            TransClick();

			if (IniHelper.GetValue("工具栏", "顶置") == "True")
			{
				TopMost = true;
				return;
			}
			TopMost = false;
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

		public void Trans_baidu2_Click(object sender, EventArgs e)
		{
			Trans_foreach("百度2");
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
			trans_baidu2.Text = "百度2";
			ai_menu_trans.Text = "AI";
          
            if (!string.IsNullOrEmpty(name) && name != "CustomOpenAI") // 只要不是点 AI
            {
                // 1. 清除 AI 主菜单的勾选和文字
                if (ai_menu_trans != null)
                {
                    //ai_menu_trans.Checked = false;
                    ai_menu_trans.Text = "AI"; // 恢复默认文字，去掉 "√" 或 "DeepSeek..."

                    // 2. 清除 AI 子菜单的勾选 (可选，保持子菜单选中状态也不错，看你习惯
                    // 遍历所有AI接口(厂商) (第二级)
                    foreach (ToolStripItem item in ai_menu_trans.DropDownItems)
                    {
                        if (item is ToolStripMenuItem providerItem)
                        {
                            // 清除厂商勾选
                            providerItem.Checked = false;

                            // 3.  关键：深入遍历模式 (第三级) 并清除勾选 
                            if (providerItem.HasDropDownItems)
                            {
                                foreach (ToolStripItem subItem in providerItem.DropDownItems)
                                {
                                    if (subItem is ToolStripMenuItem modeItem)
                                    {
                                        modeItem.Checked = false; // 清除模式勾选
                                    }
                                }
                            }
                        }
                    }
                }
            }

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
			if (name == "百度2")
			{
				trans_baidu2.Text = "百度2√";
			}			
			if (name == "CustomOpenAI")
			{
				ai_menu_trans.Text = "AI√";
			}

			// 保存翻译接口配置
			IniHelper.SetValue("配置", "翻译接口", name);
			
			// 同步更新StaticValue中的当前翻译接口
			StaticValue.Translate_Current_API = name;
			
			// 如果翻译功能已开启，则执行翻译
			if (transtalate_fla == "开启")
			{
                RichBoxBody.richTextBox1.TextChanged -= RichBoxBody_TextChanged;
                RichBoxBody.richTextBox1.TextChanged += RichBoxBody_TextChanged;
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
	}
}