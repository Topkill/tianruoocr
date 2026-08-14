using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Microsoft.Win32;
using TrOCR.Helper;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
		/// <summary>
		/// 反馈按钮点击事件处理函数
		/// 当用户点击反馈按钮时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 反馈Button_Click(object sender, EventArgs e)
		{
			new Thread(反馈send).Start();
		}

		/// <summary>
		/// 发送POST请求并获取HTML响应内容
		/// </summary>
		/// <param name="url">请求的目标URL地址</param>
		/// <param name="post_str">POST请求的数据字符串</param>
		/// <returns>服务器返回的HTML响应内容</returns>
		public string Post_Html(string url, string post_str)
		{
			var bytes = Encoding.UTF8.GetBytes(post_str);
			var result = "";
			var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			httpWebRequest.Method = "POST";
			httpWebRequest.Timeout = 6000;
			httpWebRequest.Proxy = null;
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			try
			{
				using (var requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				var responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
				var streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
				result = streamReader.ReadToEnd();
				responseStream.Close();
				streamReader.Close();
				httpWebRequest.Abort();
			}
			catch
			{
			}
			return result;
		}

        /// <summary>
        /// 设置程序是否开机自动启动
        /// </summary>
        /// <param name="isAuto">true表示设置为开机自启，false表示取消开机自启</param>
        public static void AutoStart(bool isAuto)
		{
			try
			{
				// 获取当前程序路径并格式化
				var value = Application.ExecutablePath.Replace("/", "\\");
				if (isAuto)
				{
					// 在注册表中添加开机启动项
					var currentUser = Registry.CurrentUser;
					var registryKey = currentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
					registryKey.SetValue("tianruoOCR", value);
					registryKey.Close();
					currentUser.Close();
				}
				else
				{
					// 从注册表中删除开机启动项
					var currentUser2 = Registry.CurrentUser;
					var registryKey2 = currentUser2.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
					registryKey2.DeleteValue("tianruoOCR", false);
					registryKey2.Close();
					currentUser2.Close();
				}
			}
			catch (Exception)
			{
				// 权限不足时提示用户
				MessageBox.Show("您需要管理员权限修改", "提示");
			}
		}

		/// <summary>
		/// 发送用户反馈信息到服务器
		/// </summary>
		private void 反馈send()
		{
			// 检查反馈内容是否为空
			if (string.IsNullOrEmpty(txt_问题反馈.Text))
			{
                CommonHelper.ShowHelpMsg("反馈文本不能为空");
                return;
			}
            // 构造请求参数并发送反馈
            var str = "sm=%E5%A4%A9%E8%8B%A5OCR%E6%96%87%E5%AD%97%E8%AF%86%E5%88%AB" + StaticValue.CurrentVersion + "&nr=";
            Post_Html("http://cd.ys168.com/f_ht/ajcx/lyd.aspx?cz=lytj&pdgk=1&pdgly=0&pdzd=0&tou=1&yzm=undefined&_dlmc=tianruoyouxin&_dlmm=", str + HttpUtility.UrlEncode(txt_问题反馈.Text));
            txt_问题反馈.Text = "";
            CommonHelper.ShowHelpMsg("感谢您的反馈！");
		}

		/// <summary>
		/// 播放指定的音频文件
		/// </summary>
		/// <param name="file">要播放的音频文件路径</param>
		public void PlaySong(string file)
		{
			HelpWin32.mciSendString("close media", null, 0, IntPtr.Zero);
			HelpWin32.mciSendString("open \"" + file + "\" type mpegvideo alias media", null, 0, IntPtr.Zero);
			HelpWin32.mciSendString("play media notify", null, 0, Handle);
		}

		/// <summary>
		/// 音效按钮点击事件处理函数，播放音效文件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void btn_音效_Click(object sender, EventArgs e)
		{
			PlaySong(text_音效path.Text);
		}

		/// <summary>
		/// 音效路径选择按钮点击事件处理函数，打开文件选择对话框选择音效文件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void btn_音效路径_Click(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "请选择音效文件";
			openFileDialog.Filter = "All files（*.*）|*.*|All files(*.*)|*.* ";
			openFileDialog.RestoreDirectory = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				text_音效path.Text = Path.GetFullPath(openFileDialog.FileName);
			}
		}

		/// <summary>
		/// 复制到剪贴板复选框状态改变事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void chbox_copy_CheckedChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 截图自动保存复选框状态改变事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void chbox_save_CheckedChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 取色器类型复选框状态改变事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void chbox_取色_CheckedChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 重置指定翻译服务的源语言设置为默认值
		/// </summary>
		/// <param name="sender">触发事件的按钮控件</param>
		/// <param name="e">事件参数</param>
		private void btn_Reset_Source_Click(object sender, EventArgs e)
		{
			var button = sender as Button;
			if (button == null) return;

			// 从按钮名称 "btn_Reset_Google_Source" 中提取 "Google"
			string serviceName = button.Name.Split('_')[2];
    
			// 构建目标TextBox的名称 "textBox_Google_Source"
			string textBoxName = $"textBox_{serviceName}_Source";

			// 在窗体中递归查找该TextBox控件
			var textBox = this.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

			if (textBox != null)
			{
				textBox.Text = "auto";
				string sectionName = $"Translate_{serviceName}";
				IniHelper.SetValue(sectionName, "Source", "auto");
			}
		}

		/// <summary>
		/// 重置指定翻译服务的目标语言设置为默认值
		/// </summary>
		/// <param name="sender">触发事件的按钮控件</param>
		/// <param name="e">事件参数</param>
		private void btn_Reset_Target_Click(object sender, EventArgs e)
		{
			var button = sender as Button;
			if (button == null) return;

			// 从按钮名称 "btn_Reset_Google_Target" 中提取 "Google"
			string serviceName = button.Name.Split('_')[2];
				
			// 构建目标TextBox的名称 "textBox_Google_Target"
			string textBoxName = $"textBox_{serviceName}_Target";

			// 在窗体中递归查找该TextBox控件
			var textBox = this.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

			if (textBox != null)
			{
				textBox.Text = "自动判断";
				string sectionName = $"Translate_{serviceName}";
				IniHelper.SetValue(sectionName, "Target", "自动判断");
			}
		}

		/// <summary>
		/// 设置属性，用于设置开始选项卡索引为5
		/// </summary>
		public string Start_set
		{
			set
			{
				tab_标签.SelectedIndex = 5;
			}
		}

		/// <summary>
		/// 存储控件的原始位置信息
		/// </summary>
		/// <param name="container">需要存储控件位置的容器控件</param>
		private void StoreOriginalLocations(Control container)
		{
		    if (_originalControlLocations == null)
		    {
		        _originalControlLocations = new Dictionary<Control, Point>();
		    }
		    foreach (Control control in container.Controls)
		    {
		        if (!_originalControlLocations.ContainsKey(control))
		        {
		            _originalControlLocations[control] = control.Location;
		        }
		        if (control.Controls.Count > 0)
		        {
		            StoreOriginalLocations(control);
		        }
		    }
		}

		/// <summary>
		/// 重置控件到原始位置
		/// </summary>
		private void ResetControlLocations()
		{
		    if (_originalControlLocations == null) return;
		    foreach (var entry in _originalControlLocations)
		    {
		        entry.Key.Location = entry.Value;
		    }
		}

		/// <summary>
		/// 根据选中的选项卡调整页面大小
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void AdjustPageSize(object sender, EventArgs e)
		{
		    ResetControlLocations();

			// Determine which TabPage is ultimately visible.
			TabPage selectedPage = tab_标签.SelectedTab;

			int newHeight;
    		
        	if (selectedPage == Page_密钥)
        	{
        		selectedPage = tabControl2.SelectedTab;
        	}
        	else if (selectedPage == Page_翻译接口)
        	{
        	    selectedPage = tabControl_Trans.SelectedTab;
        	}
	
        	if (selectedPage == null) return;

			// Calculate the bottom-most point of any visible control on that page.
			int maxBottom = 0;
        	var visibleControls = selectedPage.Controls.OfType<Control>().Where(c => c.Visible).ToList();
        	if (visibleControls.Any())
        	{
        	    maxBottom = visibleControls.Max(c => c.Bottom);
        	}
			int requiredContentHeight; 

			if (selectedPage == page_常规)
			{
				requiredContentHeight = maxBottom + 80; // Add 80px padding.

			} else {
				// This is the total height required for the content within the main TabControl area.
			 	requiredContentHeight = maxBottom + 40; // Add 40px padding.
			}
			

        	// The total form height is the position of the main TabControl plus the content height.
        	int requiredFormHeight = tab_标签.Top + requiredContentHeight;
	
        	const int minFormHeight = 435;
        	newHeight = Math.Max(requiredFormHeight, minFormHeight);

		    // Apply DPI scaling and update the form's client size.
		    int scaledHeight = (int)(newHeight / StaticValue.DpiFactor);
		    if (this.ClientSize.Height != scaledHeight)
		    {
		         this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, scaledHeight);
		    }
		}

		/// <summary>
		/// 检查接口可见性复选框的状态变化
		/// </summary>
		private void ApiVisibility_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null) return;

			if (_interfaceEntries == null) return;

			// 使用中检查只在取消勾选时做（勾选是恢复显示，无需保护）
			if (!checkBox.Checked)
			{
				foreach (var entry in _interfaceEntries)
				{
					if (!ReferenceEquals(entry.CheckBox, checkBox)) continue;

					// 使用中检查：命中"当前接口"任一匹配值则阻止隐藏
					if (entry.InUseValues != null && entry.InUseValues.Length > 0)
					{
						string currentApi = IniHelper.GetValue("配置", entry.InUseSource);
						if (Array.IndexOf(entry.InUseValues, currentApi) >= 0)
						{
							MessageBox.Show("该接口正在使用中，不能隐藏。", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
							// 重新勾选复选框，临时移除事件处理程序以避免无限循环
							checkBox.CheckedChanged -= ApiVisibility_CheckedChanged;
							checkBox.Checked = true;
							checkBox.CheckedChanged += ApiVisibility_CheckedChanged;
							return; // 强制勾回，不刷新
						}
					}
					break; // 命中触发者且未在用，跳出检查，落到下方刷新
				}
			}

			// 同步显隐到对应 tab（勾选恢复 / 取消勾选移除，均实时刷新）
			RefreshInterfaceTabPages();
		}
    }
}
