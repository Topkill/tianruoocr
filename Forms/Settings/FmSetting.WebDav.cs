using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
        private void txtWebDavUrl_Leave(object sender, EventArgs e)
        {
            // 写入配置文件，节点名为 "WebDav"，键名为 "Url"
            IniHelper.SetValue("WebDav", "Url", txtWebDavUrl.Text);
        }

        // WebDav配置：账户输入框失去焦点后保存
        private void txtWebDavUser_Leave(object sender, EventArgs e)
        {
            IniHelper.SetValue("WebDav", "User", txtWebDavUser.Text);
        }

        // WebDav配置：密码输入框失去焦点后保存
        private void txtWebDavPass_Leave(object sender, EventArgs e)
        {
            // 建议加密保存，此处先明文
            IniHelper.SetValue("WebDav", "Password", txtWebDavPass.Text);
        }

        // === 备份（上传）按钮事件 ===
        private async void btnUploadConfig_Click(object sender, EventArgs e)
        {   // --- 新增：外部数据检测逻辑开始 ---
            List<string> externalFiles = CheckForExternalFiles();

            if (externalFiles.Count > 0)
            {
             MessageBox.Show(
            "离线接口的模型、字典、高级配置文件 和 AI接口的模式文件 不在程序的Data目录，不会被备份!!! 请注意手动备份!",
            "备份范围警告",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
			);
            }
            //这样有个问题，特例：如果paddleocr接口高级配置文件保持默认空，它也会使用json配置文件，这时externalFiles.Count为0，不会弹窗提醒，这样假如用户修改了默认的高级配置文件，可能会忘记备份.
			//所以我在上面特殊判断，遇到paddleocr使用默认高级配置文件即默认配置的情况，也添加进来，数量>0.这样就会提醒，不过缺陷就是用户即使没使用过paddleocr，只要没有设置高级配置文件，会一直判断为提醒
            // --- 新增：外部数据检测逻辑结束 ---

            //防止用户改动设置后没有关闭设置窗口保存就直接点击备份
            saveSettings();
            // 1. 获取 WebDav 配置
            string url = txtWebDavUrl.Text.Trim();
            string user = txtWebDavUser.Text.Trim();
            // 如果之前用了加密，这里记得解密；如果是明文，直接用 Text
            string pass = txtWebDavPass.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show(this,"请先填写 WebDav 地址！");
                return;
            }
            btnUploadConfig.Enabled = false;
            btnUploadConfig.Text = "正在打包上传...";

            // 2. 确定要备份的本地文件路径
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
			//if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            // 定义你要备份的文件类型，比如 ini 和 json
            string[] patterns = new[] { "*.ini", "*.json" };
            if (!Directory.Exists(dataDir))
            {
                MessageBox.Show($"源文件夹不存在：{dataDir}\n无法执行备份。", "提示");
                return;
            }
            btnUploadConfig.Enabled = false;
            btnUploadConfig.Text = "备份中...";

            try
            {
                
                // 上传带时间戳的备份 
                bool success = await WebDavHelper.BackupConfigAsync(url, user, pass, dataDir, patterns);
                if (success)
                {
                    MessageBox.Show(this,$"备份成功！\n已上传带时间戳的归档文件。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 设置 OK 会关闭窗口 -> 触发 FormClosed -> 再次执行 saveSettings()，增强健壮性-> 主程序收到 OK
                    this.DialogResult = DialogResult.OK;
                }
                //else
                //{
                //	MessageBox.Show(this,"备份取消：在 Data 文件夹中未找到符合条件的文件。", "提示");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"备份失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUploadConfig.Enabled = true;
                btnUploadConfig.Text = "备份"; // 恢复按钮文字
            }
        }

        // === 恢复（下载）按钮事件 ===
        private async void btnDownloadConfig_Click(object sender, EventArgs e)
        {
            string url = txtWebDavUrl.Text.Trim();
            string user = txtWebDavUser.Text.Trim();
            string pass = txtWebDavPass.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show(this, "请先填写 WebDav 地址！");
                return;
            }

            if (MessageBox.Show(this, "确定要从云端恢复配置吗？\n这将覆盖当前的本地设置！\n如果恢复成功需要重启软件！", "确认恢复", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            btnDownloadConfig.Enabled = false;
            btnDownloadConfig.Text = "恢复中...";

            try
            {
                string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

                // 恢复逻辑：下载 最新zip 并解压覆盖
                bool success = await WebDavHelper.RestoreLatestConfigAsync(url, user, pass, dataDir);
                if (success)
                {
                    MessageBox.Show(this, "配置恢复成功！软件即将重启刷新状态。", "成功",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"恢复失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDownloadConfig.Enabled = true;
                btnDownloadConfig.Text = "恢复";
            }
        }
    }
}
