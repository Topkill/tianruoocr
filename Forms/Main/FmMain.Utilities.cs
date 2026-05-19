using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using TrOCR.Helper;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace TrOCR
{
	public partial class FmMain
	{
		/// <summary>
		/// 设置并注册全局热键
		/// </summary>
		/// <param name="text">修饰键（如Ctrl、Alt等）</param>
		/// <param name="text2">按键（如A、B、F1等）</param>
		/// <param name="value">完整的快捷键字符串，格式如"Ctrl+Alt+A"或"Alt+A"</param>
		/// <param name="flag">热键标识符</param>
		public void SetHotkey(string text, string text2, string value, int flag)
		{
			var parts = value.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
			if (parts.Count == 0) return;

			Keys key = (Keys)Enum.Parse(typeof(Keys), parts.Last());
			HelpWin32.KeyModifiers keyModifiers = HelpWin32.KeyModifiers.None;

			foreach (var part in parts.Take(parts.Count - 1))
			{
				if (part.Equals("Ctrl", StringComparison.OrdinalIgnoreCase))
					keyModifiers |= HelpWin32.KeyModifiers.Ctrl;
				else if (part.Equals("Shift", StringComparison.OrdinalIgnoreCase))
					keyModifiers |= HelpWin32.KeyModifiers.Shift;
				else if (part.Equals("Alt", StringComparison.OrdinalIgnoreCase))
					keyModifiers |= HelpWin32.KeyModifiers.Alt;
			}
			try
			{
				if (!HelpWin32.RegisterHotKey(Handle, flag, keyModifiers, key))
				{
					CommonHelper.ShowHelpMsg($"快捷键 '{value}' 注册失败，可能已被其他程序占用！");

				}
			}
			catch (Exception ex)
			{
				// 捕获任何异常，避免程序崩溃
				CommonHelper.ShowHelpMsg($"热键注册失败: {ex.Message}");
			}
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
		/// 扫描屏幕图像中的二维码内容
		/// </summary>
		/// <returns>二维码中的文本内容，如果扫描失败则返回空字符串</returns>
		public string ScanQRCode()
		{
			var result = "";
			try
			{
				var image = new BinaryBitmap(new HybridBinarizer(new BitmapLuminanceSource((System.Drawing.Bitmap)image_screen)));
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
        /// 停止所有正在运行的逻辑定时器 (用于重置状态、关闭窗口或切换模式时)，暂时不使用.手动处理了
		/// 最核心的原则是：只要发生了"模式切换"或"窗口状态改变"，都应该注意是否停止 translationTimer等定时器。
        /// </summary>
        private void StopAllActiveTimers()
        {
            if (translationTimer != null)
                translationTimer.Stop();

            if (clipboardDebounceTimer != null)
                clipboardDebounceTimer.Stop();

            // autoCopyLockTimer 一般不需要强制停止，因为它负责释放锁，强制停止可能导致锁死。
            // trayClickTimer 处理托盘点击，也不建议随意停止。
        }

		// 【新增】临时翻译事件的处理器
		private void RichBoxBody_T_OnTemporaryTranslateRequested(object sender, TempTranslateEventArgs e)
		{
					// 【新增调试代码】弹窗显示接收到的语言代码
					// MessageBox.Show($"接收到临时翻译请求：\n源语言: {e.SourceLanguage}\n目标语言: {e.TargetLanguage}");
					// 确保翻译的文本是最新的
					typeset_txt = RichBoxBody.Text;

			if (string.IsNullOrWhiteSpace(typeset_txt))
			{
				MessageBox.Show("请输入需要翻译的文本！");
				return;
			}

			// 调用我们改造后的翻译方法，并传入临时的语言代码
			trans_Calculate(e.SourceLanguage, e.TargetLanguage);
		}
	}
}
