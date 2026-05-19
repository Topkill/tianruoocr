using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TrOCR.Helper;
using TrOCR.Properties;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
        private void checkbox_AutoCopyScreenshotTranslation_CheckedChanged(object sender, EventArgs e)
		{
		    // 联动逻辑：只有当“自动复制”被勾选时，“不显示窗口”选项才可用
		    checkbox_NoWindowScreenshotTranslation.Enabled = checkbox_AutoCopyScreenshotTranslation.Checked;

		    // 如果“自动复制”被取消勾选，则“不显示窗口”也必须被取消勾选
		    if (!checkbox_AutoCopyScreenshotTranslation.Checked)
		    {
		        checkbox_NoWindowScreenshotTranslation.Checked = false;
		    }
		}

		/// <summary>
		/// 标签页选中索引变更事件处理函数，用于调整页面大小以适应内容
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void tab_标签_SelectedIndexChanged(object sender, EventArgs e)
		{
		          AdjustPageSize(sender, e);
		}

		/// <summary>
		/// 帮助图片点击事件处理函数，用于打开帮助窗口
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void pic_help_Click(object sender, EventArgs e)
		{
			new FmHelp().Show();
		}

		/// <summary>
		/// 开机自启复选框状态变更事件处理函数，用于设置程序是否开机自启
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void cbBox_开机_CheckedChanged(object sender, EventArgs e)
		{
			AutoStart(cbBox_开机.Checked);
		}

		/// <summary>
		/// 翻译复选框状态变更事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void cbBox_翻译_CheckedChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 弹窗复选框状态变更事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void cbBox_弹窗_CheckedChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 动画下拉框选中索引变更事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void cobBox_动画_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 记录数值框值变更事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void numbox_记录_ValueChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 保存复选框状态变更事件处理函数，用于控制路径文本框和浏览按钮的启用状态
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void cbBox_保存_CheckedChanged(object sender, EventArgs e)
		{
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
		}

		/// <summary>
		/// 浏览按钮点击事件处理函数，用于选择文件夹路径
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void btn_浏览_Click(object sender, EventArgs e)
		{
			var folderBrowserDialog = new FolderBrowserDialog();
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				textBox_path.Text = folderBrowserDialog.SelectedPath;
			}
		}

		/// <summary>
		/// 常规设置按钮点击事件处理函数，恢复常规设置的默认值
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 常规Button_Click(object sender, EventArgs e)
		{
			// 启用开机启动
			cbBox_开机.Checked = true;
			// 启用翻译功能
			cbBox_翻译.Checked = true;
			// 启用弹窗功能
			cbBox_弹窗.Checked = true;
			// 设置动画效果为第一项
			cobBox_动画.SelectedIndex = 0;
			// 设置记录数量为20条
			numbox_记录.Value = 20m;
			// 启用保存功能
			cbBox_保存.Checked = true;
			// 启用路径文本框和浏览按钮
			textBox_path.Enabled = true;
			textBox_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			btn_浏览.Enabled = true;
			// 设置默认保存路径为桌面
			textBox_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			// 启用保存选项
			chbox_save.Checked = true;
			// 设置音效路径
			text_音效path.Text = "Data\\screenshot.wav";
			// 禁用复制和取色功能
			chbox_copy.Checked = false;
			chbox_取色.Checked = false;
			// 禁用输入翻译剪贴板功能
			cbBox_输入翻译剪贴板.Checked = false;
			cbBox_输入翻译自动翻译.Checked = false;
			checkBox_AutoCopyOcrResult.Checked = false;
			checkBox_AutoTranslateOcrResult.Checked = false;
			checkBox_AutoCopyOcrTranslation.Checked = false;
			checkBox_AutoCopyInputTranslation.Checked = false;
		}

		/// <summary>
		/// 文本框按键抬起事件处理函数，用于设置快捷键
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">按键事件参数</param>
		private void txtBox_KeyUp(object sender, KeyEventArgs e)
		{
			var textBox = sender as TextBox;
			if (textBox == null) return;
			if (!shortcutMappings.TryGetValue(textBox.Name, out string shortcutName)) return;

			var pictureBox = this.Controls.Find("pictureBox_" + shortcutName, true).FirstOrDefault() as PictureBox;
			if (pictureBox == null) return;

			if (e.KeyData == Keys.Back)
			{
				textBox.Text = "请按下快捷键";
				pictureBox.Image = Resources.快捷键_0;
				IniHelper.SetValue("快捷键", shortcutName, textBox.Text);
				return;
			}

			if (e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.Menu)
			{
				var sb = new StringBuilder();
				if (e.Control) sb.Append("Ctrl + ");
				if (e.Shift) sb.Append("Shift + ");
				if (e.Alt) sb.Append("Alt + ");
				sb.Append(e.KeyCode);

				textBox.Text = sb.ToString();
				pictureBox.Image = Resources.快捷键_1;
				IniHelper.SetValue("快捷键", shortcutName, textBox.Text);
			}
		}

		/// <summary>
		/// 处理命令键事件，重写此方法以自定义按键处理逻辑
		/// </summary>
		/// <param name="msg">通过引用传递的Windows消息</param>
		/// <param name="keyData">表示按下的键的Keys值</param>
		/// <returns>如果处理了命令键则返回true，否则返回false</returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			return (keyData == Keys.Tab && txtBox_文字识别.Focused) || (keyData == Keys.Tab && txtBox_翻译文本.Focused) || (keyData == Keys.Tab && txtBox_记录界面.Focused) || (keyData == Keys.Tab && txtBox_识别界面.Focused)
			|| (keyData == Keys.Tab && txtBox_输入翻译.Focused) || (keyData == Keys.Tab && txtBox_静默识别.Focused) || (keyData == Keys.Tab && txtBox_截图翻译.Focused) ;
		}

		/// <summary>
		/// 文本框按键按下事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">按键事件参数</param>
		private void txtBox_KeyDown(object sender, KeyEventArgs e)
		{
			// 阻止按键声音
			e.SuppressKeyPress = true;
		}

		/// <summary>
		/// 快捷键按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 快捷键Button_Click(object sender, EventArgs e)
		{
			new ComponentResourceManager(typeof(FmSetting));
			txtBox_文字识别.Text = "F4";
			pictureBox_文字识别.Image = Resources.快捷键_1;
			txtBox_翻译文本.Text = "F9";
			pictureBox_翻译文本.Image = Resources.快捷键_1;
			txtBox_记录界面.Text = "请按下快捷键";
			pictureBox_记录界面.Image = Resources.快捷键_0;
			txtBox_识别界面.Text = "请按下快捷键";
			pictureBox_识别界面.Image = Resources.快捷键_0;
			txtBox_输入翻译.Text = "请按下快捷键";
			pictureBox_输入翻译.Image = Resources.快捷键_0;
			txtBox_静默识别.Text = "请按下快捷键";
			pictureBox_静默识别.Image = Resources.快捷键_0;
			txtBox_截图翻译.Text = "请按下快捷键";
			pictureBox_截图翻译.Image = Resources.快捷键_0;
		}

		/// <summary>
		/// 代理下拉框选项改变事件处理函数
		/// 当用户在代理下拉框中选择不同选项时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void combox_代理_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 当选择"不使用代理"或"系统代理"时，禁用相关输入框并清空内容
			if (combox_代理.Text == "不使用代理" || combox_代理.Text == "系统代理")
			{
				text_账号.Enabled = false;
				text_密码.Enabled = false;
				chbox_代理服务器.Enabled = false;
				text_端口.Enabled = false;
				chbox_代理服务器.Checked = false;
				text_服务器.Enabled = false;
				text_服务器.Text = "";
				text_端口.Text = "";
				text_服务器.Text = "";
				text_账号.Text = "";
				text_密码.Text = "";
			}
			// 当选择"自定义代理"时，启用相关输入框
			if (combox_代理.Text == "自定义代理")
			{
				text_端口.Enabled = true;
				text_服务器.Enabled = true;
				chbox_代理服务器.Enabled = true;
			}
		}

		/// <summary>
		/// 百度账号文本框文本改变事件处理函数
		/// 当百度账号文本框内容发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void text_baiduaccount_TextChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 百度密码文本框文本改变事件处理函数
		/// 当百度密码文本框内容发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void text_baidupassword_TextChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 服务器文本框文本改变事件处理函数
		/// 当服务器文本框内容发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void text_服务器_TextChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 端口文本框文本改变事件处理函数
		/// 当端口文本框内容发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void text_端口_TextChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 代理服务器复选框状态改变事件处理函数
		/// 当代理服务器复选框选中状态发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void chbox_代理服务器_CheckedChanged(object sender, EventArgs e)
		{
			// 当复选框被选中时，启用账号和密码输入框
			if (chbox_代理服务器.Checked)
			{
				text_账号.Enabled = true;
				text_密码.Enabled = true;
			}
			// 当复选框未被选中时，禁用账号和密码输入框
			if (!chbox_代理服务器.Checked)
			{
				text_账号.Enabled = false;
				text_密码.Enabled = false;
			}
		}

		/// <summary>
		/// 账号文本框文本改变事件处理函数
		/// 当账号文本框内容发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void text_账号_TextChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 密码文本框文本改变事件处理函数
		/// 当密码文本框内容发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void text_密码_TextChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 代理按钮点击事件处理函数
		/// 当用户点击代理设置按钮时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 代理Button_Click(object sender, EventArgs e)
		{
			combox_代理.Text = "系统代理";
			text_账号.Enabled = false;
			text_密码.Enabled = false;
			chbox_代理服务器.Enabled = false;
			text_端口.Enabled = false;
			text_服务器.Enabled = false;
		}

        /// <summary>
        /// 【新增】处理“智能去除空格”复选框的互斥逻辑
        /// </summary>
        private void checkBox_合并去除空格_CheckedChanged(object sender, EventArgs e)
        {
            // 只有当用户“勾选”此项时，才触发互斥
            if (checkBox_合并去除空格.Checked)
            {
                // 移除另一个事件的监听，以防止循环触发
                this.checkBox_合并去除所有空格.CheckedChanged -= this.checkBox_合并去除所有空格_CheckedChanged;

                // 取消“去除所有空格”的勾选
                checkBox_合并去除所有空格.Checked = false;

                // 重新订阅事件
                this.checkBox_合并去除所有空格.CheckedChanged += new System.EventHandler(this.checkBox_合并去除所有空格_CheckedChanged);
            }
        }

        /// <summary>
        /// 【新增】处理“去除所有空格”复选框的互斥逻辑
        /// </summary>
        private void checkBox_合并去除所有空格_CheckedChanged(object sender, EventArgs e)
        {
            // 只有当用户“勾选”此项时，才触发互斥
            if (checkBox_合并去除所有空格.Checked)
            {
                // 移除另一个事件的监听，以防止循环触发
                this.checkBox_合并去除空格.CheckedChanged -= this.checkBox_合并去除空格_CheckedChanged;

                // 取消“智能去除空格”的勾选
                checkBox_合并去除空格.Checked = false;

                // 重新订阅事件
                this.checkBox_合并去除空格.CheckedChanged += new System.EventHandler(this.checkBox_合并去除空格_CheckedChanged);
            }
        }

        /// <summary>
        /// 检查更新复选框状态改变事件处理函数
        /// 当检查更新复选框选中状态发生改变时触发此事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void check_检查更新_CheckedChanged(object sender, EventArgs e)
		{
			// 当复选框被选中时，启用更新间隔相关控件
			if (check_检查更新.Checked)
			{
				checkBox_更新间隔.Enabled = true;
				checkBox_更新间隔.Checked = true;
				numbox_间隔时间.Enabled = true;
			}
			// 当复选框未被选中时，禁用更新间隔相关控件
			if (!check_检查更新.Checked)
			{
				checkBox_更新间隔.Checked = false;
				checkBox_更新间隔.Enabled = false;
				numbox_间隔时间.Enabled = false;
			}
		}

		/// <summary>
		/// 更新间隔复选框状态改变事件处理函数
		/// 当更新间隔复选框选中状态发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void checkBox_更新间隔_CheckedChanged(object sender, EventArgs e)
		{
			// 当复选框被选中时，启用间隔时间数字框
			if (checkBox_更新间隔.Checked)
			{
				numbox_间隔时间.Enabled = true;
			}
			// 当复选框未被选中时，禁用间隔时间数字框
			if (!checkBox_更新间隔.Checked)
			{
				numbox_间隔时间.Enabled = false;
			}
		}

		/// <summary>
		/// 间隔时间数字框值改变事件处理函数
		/// 当间隔时间数字框的值发生改变时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void numbox_间隔时间_ValueChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 更新设置页的恢复默认按钮点击事件处理函数
		/// 当用户点击更新设置的恢复默认按钮时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 更新Button_Click(object sender, EventArgs e)
		{
			numbox_间隔时间.Value = 24m;
			check_检查更新.Checked = true;
			checkBox_更新间隔.Checked = true;
			checkBox_PreRelease.Checked = false;
		}

		/// <summary>
		/// 更新检查按钮点击事件处理函数
		/// 当用户点击检查更新按钮时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 更新Button_check_Click(object sender, EventArgs e)
		{
			new Thread(UpdateChecker.CheckUpdate).Start();
		}
    }
}
