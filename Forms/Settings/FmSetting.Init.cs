using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrOCR.Helper;
using TrOCR.Properties;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
		// 构造函数，初始化设置窗口
		public FmSetting()
		{
			Font = new Font(Font.Name, 9f / StaticValue.DpiFactor, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
			InitializeComponent();
			// 1. 确保边框是可以拉伸的
			this.FormBorderStyle = FormBorderStyle.Sizable;

			// 2. 锁定宽度
			// 让最小宽度等于当前设计的高度
			this.MinimumSize = new Size(this.Width,0); // 限制最小宽度
            EnableTabScroll(this.tab_标签);
            EnableTabScroll(this.tabControl_Trans);;
            EnableTabScroll(this.tabControl2);
            EnableTabScroll(this.tabControl_BaiduApiType);
            EnableTabScroll(this.tabControl_TXApiType);
            // 1. 绑定滚轮事件
            //this.tab_标签.MouseWheel += tab_标签_MouseWheel;

            // 2. 关键优化：鼠标移入 TabControl 时自动获取焦点
            // 如果不加这句，你必须先点一下 TabControl，滚轮才生效，体验很差
            //this.tab_标签.MouseEnter += (s, e) => { this.tab_标签.Focus(); };

            // 3. 离开时（可选）：把焦点还给别的控件，或者不做处理
            // === 【新增】启用悬停自动切换（受配置控制） ===
            if (StaticValue.EnableTabHoverSwitch)
            {
                EnableTabHoverSwitch(this.tab_标签);
                EnableTabHoverSwitch(this.tabControl_Trans);
                EnableTabHoverSwitch(this.tabControl2);
                EnableTabHoverSwitch(this.tabControl_BaiduApiType);
                EnableTabHoverSwitch(this.tabControl_TXApiType);
            }

        }

        /// <summary>
        /// 【通用方法】给指定的 TabControl 启用滚轮切换功能
        /// </summary>
        private void EnableTabScroll(TabControl tc)
        {
            if (tc == null) return;

            // 1. 绑定滚轮事件 (复用同一个处理方法)
            tc.MouseWheel += Shared_TabControl_MouseWheel;

            // 2. 绑定自动聚焦 (为了让滚轮能直接生效)
            // 这里用 Lambda 表达式直接捕获当前的 tc 变量
            tc.MouseEnter += (s, e) => { tc.Focus(); };

			//可选：
			// 新增：鼠标离开时，把积攒的“能量”清零，防止误触
			//防抖，移出时清空“积攒”的滚动量，避免下次回来瞬间切换
			tc.MouseLeave += (s, e) =>
			{
				if (_scrollAccumulators.ContainsKey(tc))
					_scrollAccumulators[tc] = 0;
			};
		}

        /// <summary>
        /// 【核心逻辑】通用的滚轮处理事件
        /// </summary>
        private void Shared_TabControl_MouseWheel(object sender, MouseEventArgs e)
        {
			// 关键点：使用 sender 拿到当前正在滚动的那个控件，而不是写死 tabControl1
			if (sender is TabControl tc)
			{
				// 1. 区域判断（鼠标必须在标题栏）
				Rectangle headerRect = tc.DisplayRectangle;
				if (e.Location.Y >= headerRect.Y) return; // 如果在内容区，直接退出，保持默认滚动

				// 2. 初始化该控件的计数器
				if (!_scrollAccumulators.ContainsKey(tc))
				{
					_scrollAccumulators[tc] = 0;
				}

				// 3. 累加滚动值 (e.Delta 通常是 +120 或 -120)
				_scrollAccumulators[tc] += e.Delta;

				// 4. 判断是否达到阈值
				// 使用 Abs 绝对值判断，不管向上滚还是向下滚
				if (Math.Abs(_scrollAccumulators[tc]) >= SCROLL_THRESHOLD)
				{
                    int index = tc.SelectedIndex;
                    int count = tc.TabCount;

                    // 判定方向：累加值大于0是向上/前，小于0是向下/后
                    if (_scrollAccumulators[tc] > 0)
					{
                        if (index > 0)
                            tc.SelectedIndex = index - 1; // 正常：往前切
                        else
                            tc.SelectedIndex = count - 1; // 循环：开头 -> 跳到结尾
                    }
					else
					{
                        if (index < count - 1)
                            tc.SelectedIndex = index + 1; // 正常：往后切
                        else
                            tc.SelectedIndex = 0;         // 循环：结尾 -> 跳到开头
                    }
                    // === 【新增】 ===
                    // 切换完瞬间，再次强制聚焦 TabControl 自身
                    // 这会让 WinForms 放弃去寻找子控件的焦点
                    tc.Focus();

                    // 5. 【关键】触发切换后，清零计数器，准备下一次积累
                    _scrollAccumulators[tc] = 0;
				}

				// 6. 阻止冒泡
				if (e is HandledMouseEventArgs he) he.Handled = true;
			}
        }

        /// <summary>
        /// 【通用方法】给指定的 TabControl 启用鼠标悬停自动切换功能
        /// </summary>
        /// <param name="tc">目标 TabControl</param>
        private void EnableTabHoverSwitch(TabControl tc)
        {
            if (tc == null) return;

            // 初始化全局悬停计时器（单例模式，整个窗体共用一个）
            if (_tabHoverTimer == null)
            {
                _tabHoverTimer = new System.Windows.Forms.Timer();
                _tabHoverTimer.Interval = HOVER_SWITCH_DELAY;
                _tabHoverTimer.Tick += TabHoverTimer_Tick;
            }

            // 绑定鼠标移动事件（检测是否悬停在标题上）
            tc.MouseMove += TabControl_Hover_MouseMove;

            // 绑定鼠标离开事件（取消计时）
            tc.MouseLeave += TabControl_Hover_MouseLeave;
        }

        /// <summary>
        /// 鼠标在 TabControl 上移动时触发，检测是否在某个未选中的 Tab 标题上
        /// </summary>
        private void TabControl_Hover_MouseMove(object sender, MouseEventArgs e)
        {
            // === 实时读取复选框的 UI 状态，而不是全局变量 ===
            // 只要用户在界面上取消了勾选，哪怕还没关闭窗口保存，也能立刻生效阻断
            if (!this.checkBox_EnableTabHoverSwitch.Checked)
            {
                return;
            }
            if (sender is TabControl tc)
            {
                int hoveredIndex = -1;

                // 遍历所有标签页，判断鼠标是否在其标题区域内
                for (int i = 0; i < tc.TabCount; i++)
                {
                    if (tc.GetTabRect(i).Contains(e.Location))
                    {
                        hoveredIndex = i;
                        break;
                    }
                }

                // 如果鼠标在某个标签标题上，且该标签不是当前选中的标签
                if (hoveredIndex != -1 && hoveredIndex != tc.SelectedIndex)
                {
                    // 如果是一个新的悬停目标（即鼠标从别的标签移过来，或者刚移入）
                    if (tc != _currentHoverTabControl || hoveredIndex != _currentHoverTabIndex)
                    {
                        _currentHoverTabControl = tc;
                        _currentHoverTabIndex = hoveredIndex;

                        // 重置并启动计时器
                        _tabHoverTimer.Stop();
                        _tabHoverTimer.Start();
                    }
                    // 如果目标没变，则让计时器继续走，不做操作
                }
                else
                {
                    // 鼠标不在任何标题上，或者在当前选中的标题上 -> 取消计时
                    if (_tabHoverTimer != null) _tabHoverTimer.Stop();
                    _currentHoverTabControl = null;
                    _currentHoverTabIndex = -1;
                }
            }
        }

        /// <summary>
        /// 鼠标离开 TabControl 时，停止计时
        /// </summary>
        private void TabControl_Hover_MouseLeave(object sender, EventArgs e)
        {
            if (_tabHoverTimer != null)
            {
                _tabHoverTimer.Stop();
            }
            _currentHoverTabControl = null;
            _currentHoverTabIndex = -1;
        }

        /// <summary>
        /// 计时器时间到，执行切换
        /// </summary>
        private void TabHoverTimer_Tick(object sender, EventArgs e)
        {
            _tabHoverTimer.Stop(); // 停止计时，防止重复触发

            if (_currentHoverTabControl != null &&
                !_currentHoverTabControl.IsDisposed &&
                _currentHoverTabIndex >= 0 &&
                _currentHoverTabIndex < _currentHoverTabControl.TabCount)
            {
                // 如果目标索引已经是当前选中的索引，则无需切换
                if (_currentHoverTabControl.SelectedIndex != _currentHoverTabIndex)
                {
                    // 执行切换
                    _currentHoverTabControl.SelectedIndex = _currentHoverTabIndex;

                    // 可选：切换后让控件获取焦点
                    // 修改为：仅当当前窗体是激活状态（用户正在操作本软件）时，才获取焦点。
                    // 这样如果用户在记事本打字，鼠标划过这里，只会切换画面，不会打断打字。
                    if (Form.ActiveForm == this)
                    {
                        _currentHoverTabControl.Focus();
                    }
                }
                
            }
            //无论是否切换成功，都清理状态，防止逻辑死锁
            // 重置状态
            _currentHoverTabControl = null;
            _currentHoverTabIndex = -1;
        }

        // 从配置文件读取设置信息并初始化设置界面控件
        public void readIniFile()
		{
			// 读取基本配置项
			var value = IniHelper.GetValue("配置", "开机自启");
			if (value == "发生错误")
			{
				cbBox_开机.Checked = true;
			}
			try
			{
				cbBox_开机.Checked = Convert.ToBoolean(value);
			}
			catch
			{
				cbBox_开机.Checked = true;
			}

			var value2 = IniHelper.GetValue("配置", "快速翻译");
			if (value2 == "发生错误")
			{
				cbBox_翻译.Checked = true;
			}
			try
			{
				cbBox_翻译.Checked = Convert.ToBoolean(value2);
			}
			catch
			{
				cbBox_翻译.Checked = true;
			}

			var value3 = IniHelper.GetValue("配置", "识别弹窗");
			if (value3 == "发生错误")
			{
				cbBox_弹窗.Checked = true;
			}
			try
			{
				cbBox_弹窗.Checked = Convert.ToBoolean(value3);
			}
			catch
			{
				cbBox_弹窗.Checked = true;
			}

			var value_input_translate_clipboard = IniHelper.GetValue("配置", "InputTranslateClipboard");
			if (value_input_translate_clipboard == "发生错误")
			{
				cbBox_输入翻译剪贴板.Checked = false;
			}
			try
			{
				cbBox_输入翻译剪贴板.Checked = Convert.ToBoolean(value_input_translate_clipboard);
			}
			catch
			{
				cbBox_输入翻译剪贴板.Checked = false;
			}

			var value_input_translate_auto = IniHelper.GetValue("配置", "InputTranslateAutoTranslate");
			if (value_input_translate_auto == "发生错误")
			{
				cbBox_输入翻译自动翻译.Checked = false;
			}
			try
			{
				cbBox_输入翻译自动翻译.Checked = Convert.ToBoolean(value_input_translate_auto);
			}
			catch
			{
				cbBox_输入翻译自动翻译.Checked = false;
			}

			var value_autoCopy = IniHelper.GetValue("常规识别", "AutoCopyOcrResult");
			if (value_autoCopy == "发生错误")
			{
				checkBox_AutoCopyOcrResult.Checked = false;
			}
			try
			{
				checkBox_AutoCopyOcrResult.Checked = Convert.ToBoolean(value_autoCopy);
			}
			catch
			{
				checkBox_AutoCopyOcrResult.Checked = false;
			}
			var value_autoTranslate = IniHelper.GetValue("工具栏", "翻译");
			if (value_autoTranslate == "发生错误")
			{
				checkBox_AutoTranslateOcrResult.Checked = false;
			}
			try
			{
				checkBox_AutoTranslateOcrResult.Checked = Convert.ToBoolean(value_autoTranslate);
			}
			catch
			{
				checkBox_AutoTranslateOcrResult.Checked = false;
			}

			var value_autoCopyOcr = IniHelper.GetValue("常规翻译", "AutoCopyOcrTranslation");
			if (value_autoCopyOcr == "发生错误")
			{
				checkBox_AutoCopyOcrTranslation.Checked = false;
			}
			try
			{
				checkBox_AutoCopyOcrTranslation.Checked = Convert.ToBoolean(value_autoCopyOcr);
			}
			catch
			{
				checkBox_AutoCopyOcrTranslation.Checked = false;
			}

			var value_autoCopyInput = IniHelper.GetValue("配置", "AutoCopyInputTranslation");
			if (value_autoCopyInput == "发生错误")
			{
				checkBox_AutoCopyInputTranslation.Checked = false;
			}
			try
			{
				checkBox_AutoCopyInputTranslation.Checked = Convert.ToBoolean(value_autoCopyInput);
			}
			catch
			{
				checkBox_AutoCopyInputTranslation.Checked = false;
			}

			var value_IsMergeRemoveSpace = IniHelper.GetValue("工具栏", "IsMergeRemoveSpace");
			if (value_IsMergeRemoveSpace == "发生错误")
			{
				checkBox_合并去除空格.Checked = false;
			}
			try
			{
				checkBox_合并去除空格.Checked = Convert.ToBoolean(value_IsMergeRemoveSpace);
			}
			catch
			{
				checkBox_合并去除空格.Checked = false;
			}

            checkBox_合并去除所有空格.Checked = TrOCRUtils.LoadSetting("工具栏", "IsMergeRemoveAllSpace", false);

			var value_IsMergeAutoCopy = IniHelper.GetValue("工具栏", "IsMergeAutoCopy");
			if (value_IsMergeAutoCopy == "发生错误")
			{
				checkBox_合并自动复制.Checked = false;
			}
			try
			{
				checkBox_合并自动复制.Checked = Convert.ToBoolean(value_IsMergeAutoCopy);
			}
			catch
			{
				checkBox_合并自动复制.Checked = false;
			}
			var value_IsSplitAutoCopy = IniHelper.GetValue("工具栏", "IsSplitAutoCopy");
			if (value_IsSplitAutoCopy == "发生错误")
			{
				checkBox_拆分后自动复制.Checked = false;
			}
			try
			{
				checkBox_拆分后自动复制.Checked = Convert.ToBoolean(value_IsSplitAutoCopy);
			}
			catch
			{
				checkBox_拆分后自动复制.Checked = false;
			}



			var value4 = IniHelper.GetValue("配置", "窗体动画");
			cobBox_动画.Text = value4;
			if (value4 == "发生错误")
			{
				cobBox_动画.Text = "窗体";
			}

			var value5 = IniHelper.GetValue("配置", "记录数目");
			numbox_记录.Value = Convert.ToInt32(value5);
			if (value5 == "发生错误")
			{
				numbox_记录.Value = 20m;
			}

			var value6 = IniHelper.GetValue("配置", "自动保存");
			if (value6 == "发生错误")
			{
				cbBox_保存.Checked = false;
			}
			try
			{
				cbBox_保存.Checked = Convert.ToBoolean(value6);
			}
			catch
			{
				cbBox_保存.Checked = false;
			}

			if (cbBox_保存.Checked)
			{
				textBox_path.Enabled = true;
				btn_浏览.Enabled = true;
			}
			if (!cbBox_保存.Checked)
			{
				textBox_path.Enabled = false;
				btn_浏览.Enabled = false;
			}

			var value7 = IniHelper.GetValue("配置", "截图位置");
			textBox_path.Text = value7;
			if (value7 == "发生错误")
			{
				textBox_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			}

			// 读取快捷键设置
			var value8 = IniHelper.GetValue("快捷键", "文字识别");
			txtBox_文字识别.Text = value8;
			if (value8 == "发生错误")
			{
				txtBox_文字识别.Text = "F4";
			}

			var value9 = IniHelper.GetValue("快捷键", "翻译文本");
			txtBox_翻译文本.Text = value9;
			if (value9 == "发生错误")
			{
				txtBox_翻译文本.Text = "F9";
			}

			var value10 = IniHelper.GetValue("快捷键", "记录界面");
			txtBox_记录界面.Text = value10;
			if (value10 == "发生错误")
			{
				txtBox_记录界面.Text = "请按下快捷键";
			}

			var value11 = IniHelper.GetValue("快捷键", "识别界面");
			txtBox_识别界面.Text = value11;
			if (value11 == "发生错误")
			{
				txtBox_识别界面.Text = "请按下快捷键";
			}

			// 设置快捷键图标状态
			pictureBox_文字识别.Image = txtBox_文字识别.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;
			pictureBox_翻译文本.Image = txtBox_翻译文本.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;
			pictureBox_记录界面.Image = txtBox_记录界面.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;
			pictureBox_识别界面.Image = txtBox_识别界面.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;

			// 【新增】加载“监听剪贴板”设置
			var value_listenClipboard = IniHelper.GetValue("配置", "ListenClipboard");
			try
			{
				cbBox_ListenClipboard.Checked = Convert.ToBoolean(value_listenClipboard);
			}
			catch
			{
				cbBox_ListenClipboard.Checked = false;
			}

			// 【新增】加载“监听后自动复制”设置
			var value_autoCopyListenClipboard = IniHelper.GetValue("配置", "AutoCopyListenClipboardTranslation");
			try
			{
				cbBox_AutoCopyListenClipboardTranslation.Checked = Convert.ToBoolean(value_autoCopyListenClipboard);
			}
			catch
			{
				cbBox_AutoCopyListenClipboardTranslation.Checked = false;
			}
			// 【新增】加载“监听剪贴板翻译后默认隐藏原文”设置
			var value_ListenClipboardHideOriginal = IniHelper.GetValue("配置", "ListenClipboardTranslationHideOriginal");
			try
			{
				cbBox_ListenHideOriginal.Checked = Convert.ToBoolean(value_ListenClipboardHideOriginal);
			}
			catch
			{
				cbBox_ListenHideOriginal.Checked = false;
			}
			//【新增】加载“全局禁用显示/隐藏原文按钮”设置
			cbBox_禁用隐藏原文按钮.Checked = TrOCRUtils.LoadSetting("配置", "DisableToggleOriginalButton", false);
			//加载“截图翻译后自动复制译文”设置
			checkbox_AutoCopyScreenshotTranslation.Checked = TrOCRUtils.LoadSetting("配置", "AutoCopyScreenshotTranslation", false);
			// 加载“无窗口截图翻译”设置
			checkbox_NoWindowScreenshotTranslation.Checked = TrOCRUtils.LoadSetting("配置", "NoWindowScreenshotTranslation", false);
			// 加载"标签页悬停自动切换"设置
			checkBox_EnableTabHoverSwitch.Checked = TrOCRUtils.LoadSetting("配置", "EnableTabHoverSwitch", true);
			// 【重要】在加载后，立即执行一次联动逻辑，以确保初始状态正确
			checkbox_AutoCopyScreenshotTranslation_CheckedChanged(null, null);

			var value_input_translate = IniHelper.GetValue("快捷键", "输入翻译");
			txtBox_输入翻译.Text = value_input_translate;
			if (value_input_translate == "发生错误")
			{
				txtBox_输入翻译.Text = "请按下快捷键";
			}
			pictureBox_输入翻译.Image = txtBox_输入翻译.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;

			var value_silent_ocr = IniHelper.GetValue("快捷键", "静默识别");
			txtBox_静默识别.Text = value_silent_ocr;
			if (value_silent_ocr == "发生错误")
			{
				txtBox_静默识别.Text = "请按下快捷键";
			}
			pictureBox_静默识别.Image = txtBox_静默识别.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;

			txtBox_截图翻译.Text=TrOCRUtils.LoadSetting("快捷键", "截图翻译", "请按下快捷键");
			pictureBox_截图翻译.Image = txtBox_截图翻译.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;


			// 读取百度OCR密钥信息
			var value12 = IniHelper.GetValue("密钥_百度", "secret_id");
			text_baiduaccount.Text = value12;
			if (value12 == "发生错误")
			{
				text_baiduaccount.Text = "YsZKG1wha34PlDOPYaIrIIKO";
			}

			var value13 = IniHelper.GetValue("密钥_百度", "secret_key");
			text_baidupassword.Text = value13;
			if (value13 == "发生错误")
			{
				text_baidupassword.Text = "HPRZtdOHrdnnETVsZM2Nx7vbDkMfxrkD";
			}

			// 读取百度OCR语言设置
			var valueBaiduLanguage = IniHelper.GetValue("密钥_百度", "language_code");
			BaiduOcrHelper.GetStandardLanguages().TryGetValue(valueBaiduLanguage, out string langName);
			comboBox_Baidu_Language.SelectedItem = string.IsNullOrEmpty(langName) ? "中英文混合" : langName;

			// 读取百度高精度OCR密钥信息
			var valueBaiduAccurateId = IniHelper.GetValue("密钥_百度高精度", "secret_id");
			text_baidu_accurate_apikey.Text = valueBaiduAccurateId == "发生错误" ? "" : valueBaiduAccurateId;
			var valueBaiduAccurateKey = IniHelper.GetValue("密钥_百度高精度", "secret_key");
			text_baidu_accurate_secretkey.Text = valueBaiduAccurateKey == "发生错误" ? "" : valueBaiduAccurateKey;

			// 读取百度高精度OCR语言设置
			var valueBaiduAccurateLanguage = IniHelper.GetValue("密钥_百度高精度", "language_code");
			BaiduOcrHelper.GetAccurateLanguages().TryGetValue(valueBaiduAccurateLanguage, out string accurateLangName);
			comboBox_Baidu_Accurate_Language.SelectedItem = string.IsNullOrEmpty(accurateLangName) ? "中英文混合" : accurateLangName;

			// 读取百度表格识别密钥信息
			var valueBaiduTableId = IniHelper.GetValue("密钥_百度表格", "secret_id");
			textBox2.Text = valueBaiduTableId == "发生错误" ? "" : valueBaiduTableId;
			var valueBaiduTableKey = IniHelper.GetValue("密钥_百度表格", "secret_key");
			textBox1.Text = valueBaiduTableKey == "发生错误" ? "" : valueBaiduTableKey;

			// 【新增】加载百度手写识别密钥
			text_baidu_handwriting_apikey.Text = TrOCRUtils.LoadSetting("密钥_百度手写", "secret_id", "");
			text_baidu_handwriting_secretkey.Text = TrOCRUtils.LoadSetting("密钥_百度手写", "secret_key", "");
			// 【新增】加载百度手写识别语言设置
			var valueBaiduHandwritingLang = IniHelper.GetValue("密钥_百度手写", "language_code");
			BaiduOcrHelper.GetAccurateLanguages().TryGetValue(valueBaiduHandwritingLang, out string handwritingLangName);
			comboBox_Baidu_Handwriting_Language.SelectedItem = string.IsNullOrEmpty(handwritingLangName) ? "中英文混合" : handwritingLangName;


			// 读取腾讯OCR密钥信息
			var valueTencentId = IniHelper.GetValue("密钥_腾讯", "secret_id");
			BoxTencentId.Text = valueTencentId;
			if (valueTencentId == "发生错误")
			{
				BoxTencentId.Text = "";
			}

			var valueTencentKey = IniHelper.GetValue("密钥_腾讯", "secret_key");
			BoxTencentKey.Text = valueTencentKey;
			if (valueTencentKey == "发生错误")
			{
				BoxTencentKey.Text = "";
			}

			// 读取腾讯OCR语言设置
			var valueTencentLanguage = IniHelper.GetValue("密钥_腾讯", "language_code");
			TencentOcrHelper.GetStandardLanguages().TryGetValue(valueTencentLanguage, out string tencentLangName);
			comboBox_Tencent_Language.SelectedItem = string.IsNullOrEmpty(tencentLangName) ? "中英混合" : tencentLangName;

			// 读取腾讯高精度OCR密钥信息
			var valueTencentAccurateId = IniHelper.GetValue("密钥_腾讯高精度", "secret_id");
			text_tencent_accurate_secretid.Text = valueTencentAccurateId == "发生错误" ? "" : valueTencentAccurateId;
			var valueTencentAccurateKey = IniHelper.GetValue("密钥_腾讯高精度", "secret_key");
			text_tencent_accurate_secretkey.Text = valueTencentAccurateKey == "发生错误" ? "" : valueTencentAccurateKey;

			// 读取腾讯高精度OCR语言设置
			var valueTencentAccurateLanguage = IniHelper.GetValue("密钥_腾讯高精度", "language_code");
			TencentOcrHelper.GetAccurateLanguages().TryGetValue(valueTencentAccurateLanguage, out string tencentAccurateLangName);
			comboBox_Tencent_Accurate_Language.SelectedItem = string.IsNullOrEmpty(tencentAccurateLangName) ? "自动检测" : tencentAccurateLangName;

			// 读取腾讯表格API密钥信息
			var valueTencentTableId = IniHelper.GetValue("密钥_腾讯表格v3", "secret_id");
			textBox3.Text = valueTencentTableId == "发生错误" ? "" : valueTencentTableId;
			var valueTencentTableKey = IniHelper.GetValue("密钥_腾讯表格v3", "secret_key");
			textBox4.Text = valueTencentTableKey == "发生错误" ? "" : valueTencentTableKey;

			// 读取白描账号信息
			var valueBaimiaoUsername = IniHelper.GetValue("密钥_白描", "username");
			BoxBaimiaoUsername.Text = valueBaimiaoUsername;
			if (valueBaimiaoUsername == "发生错误")
			{
				BoxBaimiaoUsername.Text = "";
			}

			var valueBaimiaoPassword = IniHelper.GetValue("密钥_白描", "password");
			BoxBaimiaoPassword.Text = valueBaimiaoPassword;
			if (valueBaimiaoPassword == "发生错误")
			{
				BoxBaimiaoPassword.Text = "";
			}

			// 读取代理设置
			var value14 = IniHelper.GetValue("代理", "代理类型");
			combox_代理.Text = value14;
			if (value14 == "发生错误")
			{
				combox_代理.Text = "系统代理";
			}

			if (combox_代理.Text == "不使用代理" || combox_代理.Text == "系统代理")
			{
				text_账号.Enabled = false;
				text_密码.Enabled = false;
				chbox_代理服务器.Enabled = false;
				text_端口.Enabled = false;
				text_服务器.Enabled = false;
			}

			if (combox_代理.Text == "自定义代理")
			{
				text_端口.Enabled = true;
				text_服务器.Enabled = true;
			}

			var value15 = IniHelper.GetValue("代理", "服务器");
			text_服务器.Text = value15;
			if (value15 == "发生错误")
			{
				text_服务器.Text = "127.0.0.1";
			}

			var value16 = IniHelper.GetValue("代理", "端口");
			text_端口.Text = value16;
			if (value16 == "发生错误")
			{
				text_端口.Text = "1080";
			}

			var value17 = IniHelper.GetValue("代理", "需要密码");
			if (value17 == "发生错误")
			{
				chbox_代理服务器.Checked = false;
			}

			try
			{
				chbox_代理服务器.Checked = Convert.ToBoolean(value17);
			}
			catch
			{
				chbox_代理服务器.Checked = false;
			}

			var value18 = IniHelper.GetValue("代理", "服务器账号");
			text_账号.Text = value18;
			if (value18 == "发生错误")
			{
				text_账号.Text = "";
			}

			var value19 = IniHelper.GetValue("代理", "服务器密码");
			text_密码.Text = value19;
			if (value19 == "发生错误")
			{
				text_密码.Text = "";
			}

			if (chbox_代理服务器.Checked)
			{
				text_账号.Enabled = true;
				text_密码.Enabled = true;
			}

			if (!chbox_代理服务器.Checked)
			{
				text_账号.Enabled = false;
				text_密码.Enabled = false;
			}

			// 读取更新设置
			var value20 = IniHelper.GetValue("更新", "检测更新");
			if (value20 == "发生错误")
			{
				check_检查更新.Checked = false;
			}

			try
			{
				check_检查更新.Checked = Convert.ToBoolean(value20);
			}
			catch
			{
				check_检查更新.Checked = false;
			}

			if (check_检查更新.Checked)
			{
				checkBox_更新间隔.Enabled = true;
			}

			if (!check_检查更新.Checked)
			{
				checkBox_更新间隔.Enabled = false;
				numbox_间隔时间.Enabled = false;
			}

			var value21 = IniHelper.GetValue("更新", "更新间隔");
			if (value21 == "发生错误")
			{
				checkBox_更新间隔.Checked = false;
			}

			try
			{
				checkBox_更新间隔.Checked = Convert.ToBoolean(value21);
			}
			catch
			{
				checkBox_更新间隔.Checked = false;
			}

			if (checkBox_更新间隔.Checked)
			{
				numbox_间隔时间.Enabled = true;
			}

			if (!checkBox_更新间隔.Checked)
			{
				numbox_间隔时间.Enabled = false;
			}

			var value22 = IniHelper.GetValue("更新", "间隔时间");
			numbox_间隔时间.Value = Convert.ToInt32(value22);
			if (value5 == "发生错误")
			{
				numbox_间隔时间.Value = 24m;
			}

			var value_pre_release = IniHelper.GetValue("更新", "CheckPreRelease");
			if (value_pre_release == "发生错误")
			{
				checkBox_PreRelease.Checked = false;
			}
			try
			{
				checkBox_PreRelease.Checked = Convert.ToBoolean(value_pre_release);
			}
			catch
			{
				checkBox_PreRelease.Checked = false;
			}

			// 读取截图音效设置
			var value23 = IniHelper.GetValue("截图音效", "粘贴板");
			if (value23 == "发生错误")
			{
				chbox_copy.Checked = false;
			}

			try
			{
				chbox_copy.Checked = Convert.ToBoolean(value23);
			}
			catch
			{
				chbox_copy.Checked = false;
			}

			var value24 = IniHelper.GetValue("截图音效", "自动保存");
			if (value24 == "发生错误")
			{
				chbox_save.Checked = true;
			}

			try
			{
				chbox_save.Checked = Convert.ToBoolean(value24);
			}
			catch
			{
				chbox_save.Checked = true;
			}

			var value25 = IniHelper.GetValue("截图音效", "音效路径");
			text_音效path.Text = value25;
			if (value25 == "发生错误")
			{
				text_音效path.Text = "Data\\screenshot.wav";
			}

			// 读取取色器设置
			var value26 = IniHelper.GetValue("取色器", "类型");
			if (value26 == "发生错误")
			{
				chbox_取色.Checked = false;
			}

			if (value26 == "RGB")
			{
				chbox_取色.Checked = false;
			}

			if (value26 == "HEX")
			{
				chbox_取色.Checked = true;
			}

			// 读取各翻译接口设置
			var googleSource = IniHelper.GetValue("Translate_Google", "Source");
			textBox_Google_Source.Text = (googleSource == "发生错误") ? "auto" : googleSource;
			var googleTarget = IniHelper.GetValue("Translate_Google", "Target");
			textBox_Google_Target.Text = (googleTarget == "发生错误") ? "自动判断" : googleTarget;

			var baiduSource = IniHelper.GetValue("Translate_Baidu", "Source");
			textBox_Baidu_Source.Text = (baiduSource == "发生错误") ? "auto" : baiduSource;
			var baiduTarget = IniHelper.GetValue("Translate_Baidu", "Target");
			textBox_Baidu_Target.Text = (baiduTarget == "发生错误") ? "自动判断" : baiduTarget;
			var baiduAK = IniHelper.GetValue("Translate_Baidu", "APP_ID");
			textBox_Baidu_AK.Text = (baiduAK == "发生错误") ? "" : baiduAK;
			var baiduSK = IniHelper.GetValue("Translate_Baidu", "APP_KEY");
			textBox_Baidu_SK.Text = (baiduSK == "发生错误") ? "" : baiduSK;

			var tencentSource = IniHelper.GetValue("Translate_Tencent", "Source");
			textBox_Tencent_Source.Text = (tencentSource == "发生错误") ? "auto" : tencentSource;
			var tencentTarget = IniHelper.GetValue("Translate_Tencent", "Target");
			textBox_Tencent_Target.Text = (tencentTarget == "发生错误") ? "自动判断" : tencentTarget;
			var tencentAK = IniHelper.GetValue("Translate_Tencent", "SecretId");
			textBox_Tencent_AK.Text = (tencentAK == "发生错误") ? "" : tencentAK;
			var tencentSK = IniHelper.GetValue("Translate_Tencent", "SecretKey");
			textBox_Tencent_SK.Text = (tencentSK == "发生错误") ? "" : tencentSK;

			var bingSource = IniHelper.GetValue("Translate_Bing", "Source");
			textBox_Bing_Source.Text = (bingSource == "发生错误") ? "auto" : bingSource;
			var bingTarget = IniHelper.GetValue("Translate_Bing", "Target");
			textBox_Bing_Target.Text = (bingTarget == "发生错误") ? "自动判断" : bingTarget;

			var bing2Source = IniHelper.GetValue("Translate_Bing2", "Source");
			textBox_Bing2_Source.Text = (bing2Source == "发生错误") ? "auto" : bing2Source;
			var bing2Target = IniHelper.GetValue("Translate_Bing2", "Target");
			textBox_Bing2_Target.Text = (bing2Target == "发生错误") ? "自动判断" : bing2Target;

			var microsoftSource = IniHelper.GetValue("Translate_Microsoft", "Source");
			textBox_Microsoft_Source.Text = (microsoftSource == "发生错误") ? "auto" : microsoftSource;
			var microsoftTarget = IniHelper.GetValue("Translate_Microsoft", "Target");
			textBox_Microsoft_Target.Text = (microsoftTarget == "发生错误") ? "自动判断" : microsoftTarget;

			var yandexSource = IniHelper.GetValue("Translate_Yandex", "Source");
			textBox_Yandex_Source.Text = (yandexSource == "发生错误") ? "auto" : yandexSource;
			var yandexTarget = IniHelper.GetValue("Translate_Yandex", "Target");
			textBox_Yandex_Target.Text = (yandexTarget == "发生错误") ? "自动判断" : yandexTarget;

			// 腾讯交互翻译
			var tencentInteractiveSource = IniHelper.GetValue("Translate_TencentInteractive", "Source");
			textBox_TencentInteractive_Source.Text = (tencentInteractiveSource == "发生错误") ? "auto" : tencentInteractiveSource;
			var tencentInteractiveTarget = IniHelper.GetValue("Translate_TencentInteractive", "Target");
			textBox_TencentInteractive_Target.Text = (tencentInteractiveTarget == "发生错误") ? "自动判断" : tencentInteractiveTarget;

			// 彩云小译
			var caiyunSource = IniHelper.GetValue("Translate_Caiyun", "Source");
			textBox_Caiyun_Source.Text = (caiyunSource == "发生错误") ? "auto" : caiyunSource;
			var caiyunTarget = IniHelper.GetValue("Translate_Caiyun", "Target");
			textBox_Caiyun_Target.Text = (caiyunTarget == "发生错误") ? "自动判断" : caiyunTarget;

			// 火山翻译
			var volcanoSource = IniHelper.GetValue("Translate_Volcano", "Source");
			textBox_Volcano_Source.Text = (volcanoSource == "发生错误") ? "auto" : volcanoSource;
			var volcanoTarget = IniHelper.GetValue("Translate_Volcano", "Target");
			textBox_Volcano_Target.Text = (volcanoTarget == "发生错误") ? "自动判断" : volcanoTarget;

			//百度翻译2
			TrOCRUtils.LoadSetting("Translate_Baidu2", "Source", "auto");
			TrOCRUtils.LoadSetting("Translate_Baidu2", "Target", "自动判断");
			// 彩云小译2
			var caiyun2Source = IniHelper.GetValue("Translate_Caiyun2", "Source");
			textBox_Caiyun2_Source.Text = (caiyun2Source == "发生错误") ? "auto" : caiyun2Source;
			var caiyun2Target = IniHelper.GetValue("Translate_Caiyun2", "Target");
			textBox_Caiyun2_Target.Text = (caiyun2Target == "发生错误") ? "自动判断" : caiyun2Target;
			var caiyun2Token = IniHelper.GetValue("Translate_Caiyun2", "Token");
			textBox_Caiyun2_Token.Text = (caiyun2Token == "发生错误") ? "3975l6lr5pcbvidl6jl2" : caiyun2Token;

			// 设置页的翻译接口可见性
			Action<string, CheckBox, TabPage> setTranVisibility = (apiName, checkBox, tabPage) =>
			{
				string visibilityValue = IniHelper.GetValue("翻译接口显示", apiName);
				bool isVisible;
				if (apiName == "TencentInteractive" || apiName == "Caiyun" || apiName == "Volcano" || apiName == "Baidu2")
				{
					isVisible = visibilityValue != "发生错误" && Convert.ToBoolean(visibilityValue);
				}
				else
				{
					isVisible = visibilityValue == "发生错误" || Convert.ToBoolean(visibilityValue);
				}
				checkBox.Checked = isVisible;
				if (!isVisible)
				{
					tabControl_Trans.TabPages.Remove(tabPage);
				}
			};

			setTranVisibility("Google", checkBox_ShowGoogle, tabPage_Google);
			setTranVisibility("Baidu", checkBox_ShowBaidu, tabPage_Baidu);
			setTranVisibility("Tencent", checkBox_ShowTencent, tabPage_Tencent);
			setTranVisibility("Bing", checkBox_ShowBing, tabPage_Bing);
			setTranVisibility("Bing2", checkBox_ShowBing2, tabPage_Bing2);
			setTranVisibility("Microsoft", checkBox_ShowMicrosoft, tabPage_Microsoft);
			setTranVisibility("Yandex", checkBox_ShowYandex, tabPage_Yandex);
			setTranVisibility("TencentInteractive", checkBox_ShowTencentInteractive, tabPage_TencentInteractive);
			setTranVisibility("Caiyun", checkBox_ShowCaiyun, tabPage_Caiyun);
			setTranVisibility("Volcano", checkBox_ShowVolcano, tabPage_Volcano);
			setTranVisibility("Caiyun2", checkBox_ShowCaiyun2, tabPage_Caiyun2);
			setTranVisibility("Baidu2", checkBox_ShowBaidu2, tabPage_Baidu2);

			// 设置页的OCR接口可见性
			Action<string, CheckBox, TabPage> setOcrVisibility = (apiName, checkBox, tabPage) =>
			{
				string visibilityValue = IniHelper.GetValue("Ocr接口显示", apiName);
				bool isVisible;
				if (apiName == "Baimiao")
				{
					isVisible = visibilityValue != "发生错误" && Convert.ToBoolean(visibilityValue);
				}
				else
				{
					isVisible = visibilityValue == "发生错误" || Convert.ToBoolean(visibilityValue);
				}
				checkBox.Checked = isVisible;
				if (!isVisible && tabPage != null)
				{
					tabControl2.TabPages.Remove(tabPage);
				}
			};
			//此处百度和腾讯的接口隐藏后移除密钥设置页的标签页功能已失效，暂时就这样吧
			setOcrVisibility("Baidu", checkBox_ShowOcrBaidu, inPage_百度接口);
			setOcrVisibility("BaiduAccurate", checkBox_ShowOcrBaiduAccurate, inPage_百度高精度接口);
			setOcrVisibility("Tencent", checkBox_ShowOcrTencent, inPage_腾讯接口);
			setOcrVisibility("TencentAccurate", checkBox_ShowOcrTencentAccurate, inPage_腾讯高精度接口);
			setOcrVisibility("Baimiao", checkBox_ShowOcrBaimiao, tabPage_白描接口);
			setOcrVisibility("Sougou", checkBox_ShowOcrSougou, null);
			setOcrVisibility("Youdao", checkBox_ShowOcrYoudao, null);
			setOcrVisibility("WeChat", checkBox_ShowOcrWeChat, null);
			setOcrVisibility("Mathfuntion", checkBox_ShowOcrMathfuntion, null);
			setOcrVisibility("Table", checkBox_ShowOcrTable, null);
			setOcrVisibility("Shupai", checkBox_ShowOcrShupai, null);
			setOcrVisibility("TableBaidu", checkBox_ShowOcrTableBaidu, null);
			setOcrVisibility("TableAli", checkBox_ShowOcrTableAli, null);
			setOcrVisibility("ShupaiLR", checkBox_ShowOcrShupaiLR, null);
			setOcrVisibility("ShupaiRL", checkBox_ShowOcrShupaiRL, null);
			setOcrVisibility("TencentTable", checkBox_ShowOcrTableTencent, null);
			setOcrVisibility("PaddleOCR", checkBox_ShowOcrPaddleOCR, inPage_PaddleOCR);
			setOcrVisibility("PaddleOCR2", checkBox_ShowOcrPaddleOCR2, inPage_PaddleOCR2);
			setOcrVisibility("RapidOCR", checkBox_ShowOcrRapidOCR, inPage_RapidOCR);

			// 读取OCR模型配置
			ReadOcrModelConfigs();

			
			//文本改变后自动翻译的延时
			textBox38.Text=TrOCRUtils.LoadSetting("配置", "文本改变自动翻译延时", "5000");
			//工具栏图标放大倍数
			textBox37.Text=TrOCRUtils.LoadSetting("工具栏", "图标放大倍数", "1.0");
			//文字缩放倍数
			textBox39.Text=TrOCRUtils.LoadSetting("配置", "文字缩放倍数", "1.0");

            LoadCustomAIProviders();
            LoadCustomAITransProviders();

			txtWebDavUrl.Text = TrOCRUtils.LoadSetting("WebDav", "Url", "");
            txtWebDavUser.Text = TrOCRUtils.LoadSetting("WebDav", "User", "");
            txtWebDavPass.Text = TrOCRUtils.LoadSetting("WebDav", "Password", "");


            }

		/// <summary>
		/// 窗口加载事件处理函数，用于初始化界面控件和读取配置文件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void Form1_Load(object sender, EventArgs e)
		{
			var componentResourceManager = new ComponentResourceManager(typeof(FmMain));
			Icon = (Icon)componentResourceManager.GetObject("minico.Icon");

			// 设置记录数目的数值范围
			var numericUpDown = numbox_记录;
			var array = new int[4];
			array[0] = 99;
			numericUpDown.Maximum = new decimal(array);

			var numericUpDown2 = numbox_记录;
			var array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);

			var numericUpDown3 = numbox_记录;
			var array3 = new int[4];
			array3[0] = 1;
			numericUpDown3.Value = new decimal(array3);

			// 设置更新间隔时间的数值范围
			var numericUpDown4 = numbox_间隔时间;
			var array4 = new int[4];
			array4[0] = 24;
			numericUpDown4.Maximum = new decimal(array4);

			var numericUpDown5 = numbox_间隔时间;
			var array5 = new int[4];
			array5[0] = 1;
			numericUpDown5.Minimum = new decimal(array5);

			var numericUpDown6 = numbox_间隔时间;
			var array6 = new int[4];
			array6[0] = 1;
			numericUpDown6.Value = new decimal(array6);

			// 添加ocr语言选项到下拉列表
			comboBox_Baidu_Language.Items.AddRange(BaiduOcrHelper.GetStandardLanguages().Values.ToArray());
			comboBox_Baidu_Accurate_Language.Items.AddRange(BaiduOcrHelper.GetAccurateLanguages().Values.ToArray());
			// 【新增】填充百度手写识别的语言下拉框
			comboBox_Baidu_Handwriting_Language.Items.AddRange(BaiduOcrHelper.GetAccurateLanguages().Values.ToArray());
			comboBox_Tencent_Accurate_Language.Items.AddRange(TencentOcrHelper.GetAccurateLanguages().Values.ToArray());
			comboBox_Tencent_Language.Items.AddRange(TencentOcrHelper.GetStandardLanguages().Values.ToArray());

			// 为百度标准版添加重置按钮
			Button btnResetBaiduLang = new Button();
			btnResetBaiduLang.Name = "btnResetBaiduLang";
			btnResetBaiduLang.Text = "重置";
			btnResetBaiduLang.Size = new Size(50, 23);
			btnResetBaiduLang.Location = new Point(comboBox_Baidu_Language.Right + 6, comboBox_Baidu_Language.Top - 2);
			btnResetBaiduLang.Click += (s, ev) => { comboBox_Baidu_Language.SelectedItem = "中英文混合"; };
			inPage_百度接口.Controls.Add(btnResetBaiduLang);

			// 为百度高精度版添加重置按钮
			Button btnResetBaiduAccurateLang = new Button();
			btnResetBaiduAccurateLang.Name = "btnResetBaiduAccurateLang";
			btnResetBaiduAccurateLang.Text = "重置";
			btnResetBaiduAccurateLang.Size = new Size(50, 23);
			btnResetBaiduAccurateLang.Location = new Point(comboBox_Baidu_Accurate_Language.Right + 6, comboBox_Baidu_Accurate_Language.Top - 2);
			btnResetBaiduAccurateLang.Click += (s, ev) => { comboBox_Baidu_Accurate_Language.SelectedItem = "中英文混合"; };
			inPage_百度高精度接口.Controls.Add(btnResetBaiduAccurateLang);

			// 为腾讯高精度版添加重置按钮
			Button btnResetTencentAccurateLang = new Button();
			btnResetTencentAccurateLang.Name = "btnResetTencentAccurateLang";
			btnResetTencentAccurateLang.Text = "重置";
			btnResetTencentAccurateLang.Size = new Size(50, 23);
			btnResetTencentAccurateLang.Location = new Point(comboBox_Tencent_Accurate_Language.Right + 6, comboBox_Tencent_Accurate_Language.Top - 2);
			btnResetTencentAccurateLang.Click += (s, ev) => { comboBox_Tencent_Accurate_Language.SelectedItem = "自动检测"; };
			inPage_腾讯高精度接口.Controls.Add(btnResetTencentAccurateLang);

			// 为腾讯标准版添加重置按钮
			Button btnResetTencentLang = new Button();
			btnResetTencentLang.Name = "btnResetTencentLang";
			btnResetTencentLang.Text = "重置";
			btnResetTencentLang.Size = new Size(50, 23);
			btnResetTencentLang.Location = new Point(comboBox_Tencent_Language.Right + 6, comboBox_Tencent_Language.Top - 2);
			btnResetTencentLang.Click += (s, ev) => { comboBox_Tencent_Language.SelectedItem = "中英混合"; };
			inPage_腾讯接口.Controls.Add(btnResetTencentLang);

			readIniFile();
			// 使用程序集的实际版本号，而不是写死的值
			label_VersionInfo.Text = "版本号：" + System.Windows.Forms.Application.ProductVersion.Split('+')[0];
			label_AuthorInfo.Text = "作者：topkill";
			chbox_代理服务器.CheckedChanged += chbox_代理服务器_CheckedChanged;
			更新Button_check.Click += 更新Button_check_Click;

			StoreOriginalLocations(this);
			tab_标签.SelectedIndexChanged += AdjustPageSize;
			tabControl2.SelectedIndexChanged += AdjustPageSize;
			tabControl_Trans.SelectedIndexChanged += AdjustPageSize;
			AdjustPageSize(tab_标签, EventArgs.Empty);

			// 为所有接口可见性复选框附加事件处理程序
			checkBox_ShowOcrBaidu.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrBaiduAccurate.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrTencent.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrTencentAccurate.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrBaimiao.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrSougou.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrYoudao.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrWeChat.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrMathfuntion.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrTable.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrShupai.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrTableBaidu.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrTableAli.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrShupaiLR.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrShupaiRL.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrTableTencent.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrPaddleOCR.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrPaddleOCR2.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowOcrRapidOCR.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowGoogle.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowBaidu.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowTencent.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowBing.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowBing2.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowMicrosoft.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowYandex.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowTencentInteractive.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowCaiyun.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowVolcano.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowCaiyun2.CheckedChanged += ApiVisibility_CheckedChanged;
			checkBox_ShowBaidu2.CheckedChanged += ApiVisibility_CheckedChanged;

			// 为OCR模型配置按钮添加事件处理程序
			// PaddleOCR事件
			btn_PaddleOCR_Det_Browse.Click += Btn_PaddleOCR_Det_Browse_Click;
			btn_PaddleOCR_Cls_Browse.Click += Btn_PaddleOCR_Cls_Browse_Click;
			btn_PaddleOCR_Rec_Browse.Click += Btn_PaddleOCR_Rec_Browse_Click;
			btn_PaddleOCR_Keys_Browse.Click += Btn_PaddleOCR_Keys_Browse_Click;
			button2.Click += Btn_PaddleOCR_AdvancedConfig_Browse_Click;
			textBox_PaddleOCR_Det.TextChanged += TextBox_PaddleOCR_TextChanged;
			textBox_PaddleOCR_Cls.TextChanged += TextBox_PaddleOCR_TextChanged;
			textBox_PaddleOCR_Rec.TextChanged += TextBox_PaddleOCR_TextChanged;
			textBox_PaddleOCR_Keys.TextChanged += TextBox_PaddleOCR_TextChanged;
			textBox5.TextChanged += TextBox_PaddleOCR_TextChanged;;

			// PaddleOCR2事件
			btn_PaddleOCR2_Det_Browse.Click += Btn_PaddleOCR2_Det_Browse_Click;
			btn_PaddleOCR2_Cls_Browse.Click += Btn_PaddleOCR2_Cls_Browse_Click;
			btn_PaddleOCR2_Rec_Browse.Click += Btn_PaddleOCR2_Rec_Browse_Click;
			btn_PaddleOCR2_Keys_Browse.Click += Btn_PaddleOCR2_Keys_Browse_Click;
			button3.Click += Btn_PaddleOCR2_AdvancedConfig_Browse_Click;
			textBox_PaddleOCR2_Det.TextChanged += TextBox_PaddleOCR2_TextChanged;
			textBox_PaddleOCR2_Cls.TextChanged += TextBox_PaddleOCR2_TextChanged;
			textBox_PaddleOCR2_Rec.TextChanged += TextBox_PaddleOCR2_TextChanged;
			textBox_PaddleOCR2_Keys.TextChanged += TextBox_PaddleOCR2_TextChanged;
			textBox6.TextChanged += TextBox_PaddleOCR2_TextChanged;
			comboBox_PaddleOCR2_det_Version.SelectedIndexChanged += ComboBox_PaddleOCR2_Version_SelectedIndexChanged;
			comboBox_PaddleOCR2_cls_Version.SelectedIndexChanged += ComboBox_PaddleOCR2_Version_SelectedIndexChanged;
			comboBox_PaddleOCR2_rec_Version.SelectedIndexChanged += ComboBox_PaddleOCR2_Version_SelectedIndexChanged;

			// RapidOCR事件
			btn_RapidOCR_Det_Browse.Click += Btn_RapidOCR_Det_Browse_Click;
			btn_RapidOCR_Cls_Browse.Click += Btn_RapidOCR_Cls_Browse_Click;
			btn_RapidOCR_Rec_Browse.Click += Btn_RapidOCR_Rec_Browse_Click;
			btn_RapidOCR_Keys_Browse.Click += Btn_RapidOCR_Keys_Browse_Click;
			button4.Click += Btn_RapidOCR_AdvancedConfig_Browse_Click;
			textBox_RapidOCR_Det.TextChanged += TextBox_RapidOCR_TextChanged;
			textBox_RapidOCR_Cls.TextChanged += TextBox_RapidOCR_TextChanged;
			textBox_RapidOCR_Rec.TextChanged += TextBox_RapidOCR_TextChanged;
			textBox_RapidOCR_Keys.TextChanged += TextBox_RapidOCR_TextChanged;
			textBox7.TextChanged += TextBox_RapidOCR_TextChanged;

            // 批量绑定事件
            textBox_PaddleOCR_Det.Leave += PathTextBox_Leave;
            textBox_PaddleOCR_Cls.Leave += PathTextBox_Leave;
            textBox_PaddleOCR_Rec.Leave += PathTextBox_Leave;
            textBox_PaddleOCR_Keys.Leave += PathTextBox_Leave;
            textBox5.Leave += PathTextBox_Leave;
            textBox_PaddleOCR2_Det.Leave += PathTextBox_Leave;
            textBox_PaddleOCR2_Cls.Leave += PathTextBox_Leave;
            textBox_PaddleOCR2_Rec.Leave += PathTextBox_Leave;
            textBox_PaddleOCR2_Keys.Leave += PathTextBox_Leave;
            textBox6.Leave += PathTextBox_Leave;
            textBox_RapidOCR_Det.Leave += PathTextBox_Leave;
            textBox_RapidOCR_Cls.Leave += PathTextBox_Leave;
            textBox_RapidOCR_Rec.Leave += PathTextBox_Leave;
            textBox_RapidOCR_Keys.Leave += PathTextBox_Leave;
            textBox7.Leave += PathTextBox_Leave;
			
			txt_ConfigPath.Leave += PathTextBox_Leave;
			txt_Trans_ConfigPath.Leave += PathTextBox_Leave;


        }
    }
}
