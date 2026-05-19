using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
	public partial class FmMain
	{
		private void HandleClipboardChange()
		{
			Debug.WriteLine("HandleClipboardChange执行了");
    		// 最优先检查：如果程序正处于自动复制后的静默期，则忽略所有剪贴板事件
    		if (isAutoCopying)
    		{
    		    Debug.WriteLine("忽略剪贴板事件：自动复制锁激活中");
    		    return;
    		}

			// 在程序刚启动时，忽略第一次自动触发的剪贴板消息
			if (isAppLoading)
			{
				isAppLoading = false; // 将标志位置为false，确保此逻辑只执行一次
                // 使用 try-catch 保护剪贴板读取，防止启动时其他程序占用剪贴板导致崩溃
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        // 同步初始的剪贴板内容，以便下一次真正的复制可以被正确比较
                        lastClipboardText = Clipboard.GetText();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("程序启动时同步剪贴板状态失败: " + ex.Message);
                    // 启动时读取失败没关系，直接忽略即可，不影响后续监听
                }
                return; // 关键：直接退出，不执行任何后续的翻译操作
			}

			//  检查功能是否开启
			if (!StaticValue.ListenClipboardTranslation)
			{
				return;
			}

			// 不立即处理，而是重置防抖定时器
			// 无论来多少次通知，都只是不断地重置计时器
			clipboardDebounceTimer.Stop();
			clipboardDebounceTimer.Start();
			//定时器防抖解决监听剪贴板时多次翻译的问题,此问题原因是剪贴板软件获取多次系统消息导致:应注意剪贴板查看器链(不同剪贴板软件和系统的交互链路不同)

		}
		private void ClipboardDebounceTimer_Tick(object sender, EventArgs e)
		{
		    // 1. 首先停止定时器，防止重复执行
		    clipboardDebounceTimer.Stop();

            string clipboardText = null;

            // 2. 将 ContainsText 和 GetText 全部放入 try-catch 中
            try
            {
                if (Clipboard.ContainsText())
                {
                    clipboardText = Clipboard.GetText();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("获取剪贴板文本失败，可能被其他程序占用: " + ex.Message);
                return; // 获取失败则直接返回，等待下一次用户复制
            }

            // 3. 检查是否是新的、非空的文本
            if (!string.IsNullOrEmpty(clipboardText) && clipboardText != lastClipboardText)
			{
				// 更新最后一次的文本记录，防止重复触发
				lastClipboardText = clipboardText;

				// 显示提示
				CommonHelper.ShowHelpMsg("已捕获剪贴板，正在翻译...");

				// 设置正确的标志位
				isContentFromOcr = false;
				isFromClipboardListener = true;

				// 调用UI启动方法
				InitiateTranslationUI(clipboardText);

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

        /// <summary>
        /// 使用"限时状态锁"安全地将数据对象设置到剪贴板，以防止无限循环。
        /// </summary>
        /// <param name="data">要复制到剪贴板的对象 (可以是 string, DataObject, Image 等)。</param>
        public void SetClipboardWithLock(object data)
        {
            // 检查传入的对象是否为 null
            if (data == null) return;

            // 如果传入的是字符串，额外检查它是否为空或仅包含空白字符
            if (data is string textData && string.IsNullOrWhiteSpace(textData))
            {
                return;
            }

            try
            {
                // 1. 激活状态锁
                isAutoCopying = true;
                Debug.WriteLine("--- 剪贴板锁已激活 ---");

                // 2. 执行复制操作
                // 这个调用现在是通用的，可以处理 string、DataObject 等多种类型
                Clipboard.SetDataObject(data, true, 5, 100);

                // 3. 启动定时器，创建静默期
                autoCopyLockTimer.Stop(); // 确保定时器被重置
                autoCopyLockTimer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"带锁设置剪贴板失败: {ex.Message}");
                // 如果复制失败，必须立即解除锁定
                isAutoCopying = false;
                autoCopyLockTimer.Stop();
            }
        }
        /// <summary>
        /// 使用"限时状态锁"安全地为特定格式设置剪贴板数据。
        /// 主要用于处理"快速翻译"后的粘贴操作。
        /// </summary>
        /// <param name="format">剪贴板格式 (例如 DataFormats.UnicodeText)。</param>
        /// <param name="data">要为该格式复制的数据。</param>
        private void SetClipboardDataWithLock(string format, object data)
        {
            if (string.IsNullOrEmpty(format) || data == null) return;

            try
            {
                isAutoCopying = true;
                Debug.WriteLine("--- 剪贴板锁已激活 (Data) ---");
                // 此调用处理 Clipboard.SetData 的情况
                Clipboard.SetData(format, data);
                autoCopyLockTimer.Stop();
                autoCopyLockTimer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"带锁设置剪贴板数据失败: {ex.Message}");
                isAutoCopying = false;
                autoCopyLockTimer.Stop();
            }
        }
        /// <summary>
        /// 原始文本框内容改变事件，用于实现编辑后自动翻译
        /// </summary>
        private void RichBoxBody_TextChanged(object sender, EventArgs e)
		{
            Debug.WriteLine("启动了RichBoxBody_TextChanged");
            // 【新增】如果正在流式输出，直接忽略，不启动定时器
            if (isStreaming) return;

            // 【新增】拦截逻辑
            // 如果 RichBoxBody 正在进行字体切换，直接返回，不启动翻译倒计时
            if (RichBoxBody.IsFontChanging)
            {
                // 可选：打印日志确认拦截成功
                 System.Diagnostics.Debug.WriteLine("字体切换中，忽略 TextChanged 事件");
                return;
            }
            // === 【修改步骤 1】读取原始配置字符串 ===
            string rawConfig = StaticValue.TextChangeAutotranslateDelayRaw;
            // === 【修改步骤 2】调用解析方法 ===
            // CheckTextChangeAutoTranslateConfig 会处理：
            // 1. 解析时间，如果非数字或<=0，返回 false
            // 2. 解析黑白名单，判断当前接口是否允许
            // 3. 将解析出的时间赋值给 validDelay
            if (!CheckTextChangeAutoTranslateConfig(rawConfig, StaticValue.Translate_Current_API, out int validDelay))
            {
                // 如果不允许（时间为0、格式错误、或当前接口被排除），直接退出
                return;
            }

            // --- 日志: 事件触发入口 ---
            // 为了日志清晰，将换行符替换为可见的转义字符
            string currentTextForLog = RichBoxBody.Text.Replace("\r", "\\r").Replace("\n", "\\n");
			Debug.WriteLine($"---> TextChanged 事件触发。文本: \"{currentTextForLog}\" | isContentFromOcr: {isContentFromOcr} | transtalate_fla: {transtalate_fla}");
			// 关键修复：添加一个"守卫"，如果文本是默认占位符，则直接忽略，不执行任何逻辑。
			// 这一步不做也行，因为下面2880行做了事件临时解绑。
			//这一步做了有个小问题：当OCR识别的结果恰好是 ***该区域未发现文本*** 时，也会直接return,需要优化
    		// if (RichBoxBody.Text == "***该区域未发现文本***")
    		// {
    		//     return;
    		// }

			// 使用安全的字符串比较方式，避免因 "发生错误" 或空值导致异常
			bool autoTranslateInputEnabled = StaticValue.InputTranslateAutoTranslate;
			// 定义纯手动输入状态：非OCR 也 非剪贴板
        	bool isPureManualInput = !isContentFromOcr && !isFromClipboardListener;

			 // 场景1: 当翻译窗口关闭时 (只处理纯手动输入的情况)
    		if (transtalate_fla == "关闭")
    		{
				Debug.WriteLine("    |--> 满足 [场景1：输入翻译 & 翻译窗口关闭]");
        		if (isPureManualInput && autoTranslateInputEnabled && !string.IsNullOrWhiteSpace(RichBoxBody.Text))
				{
					Debug.WriteLine("        |--> 文本不为空，准备调用 TransClick() 来打开翻译窗口...");
					translationTimer.Stop();
                    // 【修改后新增】在此处动态更新 Timer 的间隔
                    // 这样可以确保：
                    // 1. 此时 currentDelay 肯定 > 0，赋值安全，不会崩溃
                    // 2. 如果用户在设置里修改了时间，这里会立即生效，无需重启
                    // === 【修改步骤 3】使用解析出的有效时间 ===
                    translationTimer.Interval = validDelay;
                    translationTimer.Start();
				}
				Debug.WriteLine("    |<-- 场景1 结束,定时器已开始或重置。");
				Debug.WriteLine("---> TextChanged 事件结束。");
    		    return;
    		}

    		// 场景2: 当翻译窗口已经打开时
    		if (transtalate_fla == "开启")
    		{
				// 允许重新翻译的条件是：
				// 1. 内容来源于OCR（允许用户随时修改OCR结果并重翻）。
				// 2. 或者，内容来源于剪贴板监听（无条件允许重翻）。
				// 3. 或者，是纯手动输入状态 并且 开启了"输入时自动翻译"功能。
				//上面的不对,现在改成任意情况下,只要是双栏窗口,就可以进行编辑后自动重新翻译
        		// if (isContentFromOcr || (isPureManualInput && autoTranslateInputEnabled))
				// {
					Debug.WriteLine("        |--> 满足 [isContentFromOcr 或 autoTranslateInputEnabled]，准备启动/重置 定时器...");
        		    translationTimer.Stop();
                // 【修改后新增】在此处动态更新 Timer 的间隔
                // 这样可以确保：
                // 1. 此时 currentDelay 肯定 > 0，赋值安全，不会崩溃
                // 2. 如果用户在设置里修改了时间，这里会立即生效，无需重启
                // === 【修改步骤 3】使用解析出的有效时间 ===
                translationTimer.Interval = validDelay;
                translationTimer.Start();

					Debug.WriteLine("        |--> 定时器已重置。");

        		// }
    		}
			Debug.WriteLine("---> TextChanged 事件结束。");

		}

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
	}
}
