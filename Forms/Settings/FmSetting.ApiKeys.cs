using System;
using System.Diagnostics;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
        /// <summary>
        /// ocr接口申请按钮点击事件处理函数，根据当前选中的标签页打开相应的OCR服务申请页面
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void 百度申请_Click(object sender, EventArgs e)
		{
			if (tabControl2.SelectedTab == tabPage_腾讯OCR)
    		{
    		    Process.Start("https://console.cloud.tencent.com/ocr/general");
    		}
			else if (tabControl2.SelectedTab == tabPage_白描接口)
			{
				// 白描不提供传统的API申请，显示提示信息
				MessageBox.Show("白描OCR使用账号登录方式，无需申请API密钥。\n\n请直接输入您的白描账号（手机号/邮箱）和密码即可使用。\n\n如需注册账号，请前往白描官网或下载白描App。",
					"白描OCR说明", MessageBoxButtons.OK, MessageBoxIcon.Information);
				// 可选：打开白描官网
				// Process.Start("https://web.baimiaoapp.com");
			}
			else
			{
				Process.Start("https://console.bce.baidu.com/ai/");
			}
		}

		/// <summary>
		/// 密钥按钮点击事件处理函数，根据当前选中的标签页恢复对应接口的默认设置
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 密钥Button_Click(object sender, EventArgs e)
		{
			// 根据当前选中的标签页，只恢复对应接口的默认设置
			if (tabControl2.SelectedTab == tabPage_百度OCR)
			{
				// 再判断内部是哪个子选项卡
				if (tabControl_BaiduApiType.SelectedTab == inPage_百度接口)
				{
					text_baiduaccount.Text = "YsZKG1wha34PlDOPYaIrIIKO";
					text_baidupassword.Text = "HPRZtdOHrdnnETVsZM2Nx7vbDkMfxrkD";
					comboBox_Baidu_Language.SelectedItem = "中英文混合";
				}
				else if (tabControl_BaiduApiType.SelectedTab == inPage_百度高精度接口)
				{
					text_baidu_accurate_apikey.Text = "";
					text_baidu_accurate_secretkey.Text = "";
					comboBox_Baidu_Accurate_Language.SelectedItem = "中英文混合";
				}
				else if (tabControl_BaiduApiType.SelectedTab == inPage_百度表格)
				{
					textBox2.Text = "";
					textBox1.Text = "";
				}
				else if (tabControl_BaiduApiType.SelectedTab == inPage_百度手写)
        		{
        		    // 清空专用密钥输入框
        		    text_baidu_handwriting_apikey.Text = "";
        		    text_baidu_handwriting_secretkey.Text = "";
        		    // 将语言下拉框重置为默认值
        		    comboBox_Baidu_Handwriting_Language.SelectedItem = "中英文混合";
        		}
			}
			else if (tabControl2.SelectedTab == tabPage_腾讯OCR)
			{
				// 同理，判断腾讯的内部子选项卡
				if (tabControl_TXApiType.SelectedTab == inPage_腾讯接口)
				{
					BoxTencentId.Text = "";
					BoxTencentKey.Text = "";
					comboBox_Tencent_Language.SelectedItem = "中英混合";
				}
				else if (tabControl_TXApiType.SelectedTab == inPage_腾讯高精度接口)
				{
					text_tencent_accurate_secretid.Text = "";
					text_tencent_accurate_secretkey.Text = "";
					comboBox_Tencent_Accurate_Language.SelectedItem = "自动检测";
				}
				else if (tabControl_TXApiType.SelectedTab == inPage_腾讯表格v3)
				{
					textBox3.Text = "";
					textBox4.Text = "";
				}
			}
			else if (tabControl2.SelectedTab == tabPage_白描接口)
			{
				// 清空白描账号密码
				BoxBaimiaoUsername.Text = "";
				BoxBaimiaoPassword.Text = "";
			}
		}

		/// <summary>
		/// OCR验证按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private async void 百度_btn_Click(object sender, EventArgs e)
		{
			if (tabControl2.SelectedTab == tabPage_百度OCR)
			{
				// 判断内部子选项卡
				if (tabControl_BaiduApiType.SelectedTab == inPage_百度接口)
				{
					if (await BaiduOcrHelper.VerifyKeys(text_baiduaccount.Text, text_baidupassword.Text))
					{
						MessageBox.Show("密钥正确!", "提醒");
					}
					else
					{
						MessageBox.Show("请确保密钥正确!", "提醒");
					}
				}
				else if (tabControl_BaiduApiType.SelectedTab == inPage_百度高精度接口)
				{
					if (await BaiduOcrHelper.VerifyKeys(text_baidu_accurate_apikey.Text, text_baidu_accurate_secretkey.Text))
					{
						MessageBox.Show("密钥正确!", "提醒");
					}
					else
					{
						MessageBox.Show("请确保密钥正确!", "提醒");
					}
				}
				else if (tabControl_BaiduApiType.SelectedTab == inPage_百度表格)
        		{
					if (textBox2.Text != "" || textBox1.Text != "")
					{
						if (await BaiduOcrHelper.VerifyKeys(textBox2.Text, textBox1.Text))
						{
							MessageBox.Show("密钥正确!", "提醒");
						}
						else
						{
							MessageBox.Show("请确保密钥正确!", "提醒");
						}
					}
					else
					{
						MessageBox.Show("使用的百度标准版密钥,请验证标准版密钥是否有效!", "提醒");
					}
				    
        		}
    		}
			else if (tabControl2.SelectedTab == tabPage_白描接口)
			{
				string username = BoxBaimiaoUsername.Text;
				string password = BoxBaimiaoPassword.Text;

				if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				{
					MessageBox.Show("账号和密码不能为空！", "提醒");
					return;
				}

				// 禁用按钮，防止重复点击
				var button = sender as Button;
				if (button != null)
				{
					button.Enabled = false;
					button.Text = "验证中...";
				}

				try
				{
					// 异步调用白描登录验证
					var loginResult = await OcrHelper.BaimiaoVerifyAccount(username, password);

					if (loginResult != null && loginResult.ContainsKey("code"))
					{
						int code = Convert.ToInt32(loginResult["code"]);
						string message = loginResult.ContainsKey("message") ? loginResult["message"].ToString() : "";
						bool success = loginResult.ContainsKey("success") ? (bool)loginResult["success"] : false;

						// 白描API: code=1 表示成功
						if (code == 1 || success)
						{
							MessageBox.Show("白描账号验证成功！", "验证成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else if (code == 0 && message.Contains("密码错误"))
						{
							MessageBox.Show("账号或密码错误！\n\n请确认：\n1. 账号（手机号/邮箱）输入正确\n2. 密码输入正确\n3. 该账号已在白描App或网页版注册", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							// 显示原始的code和msg
							MessageBox.Show($"验证失败\n\n错误码(code): {code}\n错误信息(msg): {message}", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
					else
					{
						MessageBox.Show("验证失败：未收到有效响应\n\n请检查网络连接后重试", "无响应", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				catch (System.Threading.Tasks.TaskCanceledException)
				{
					MessageBox.Show("验证超时，请检查网络连接后重试", "超时", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"验证时发生异常：{ex.Message}", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					// 恢复按钮状态
					if (button != null)
					{
						button.Enabled = true;
						button.Text = "验证";
					}
				}
			}
			else if (tabControl2.SelectedTab == tabPage_腾讯OCR)
			{
				string secretId, secretKey;
				if (tabControl_TXApiType.SelectedTab == inPage_腾讯接口)
				{
					secretId = BoxTencentId.Text;
					secretKey = BoxTencentKey.Text;
				}
				else if (tabControl_TXApiType.SelectedTab == inPage_腾讯高精度接口)
				{
					secretId = text_tencent_accurate_secretid.Text;
					secretKey = text_tencent_accurate_secretkey.Text;
				}
				else if (tabControl_TXApiType.SelectedTab == inPage_腾讯表格v3)
        		{
					secretId = textBox3.Text;
					secretKey = textBox4.Text;
        		}else
				{
					secretId = "";
					secretKey = "";
				}

				if (string.IsNullOrEmpty(secretId) || string.IsNullOrEmpty(secretKey) || secretId.Contains("secret_id"))
				{
					MessageBox.Show("SecretId 和 SecretKey 不能为空!", "提醒");
					return;
				}

				try
				{
					string jsonResult = TencentOcrHelper.VerifyTencentKey(secretId, secretKey);
					var jObject = Newtonsoft.Json.Linq.JObject.Parse(jsonResult);
					var error = jObject?["Response"]?["Error"];

					if (error == null)
					{
						MessageBox.Show("测试响应异常，未检测到错误信息，请重试。", "未知状态", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}

					string code = error["Code"]?.ToString();
					string message = error["Message"]?.ToString();

					if (code.StartsWith("AuthFailure"))
					{
						MessageBox.Show($"密钥验证失败！请确保密钥正确无误且服务已开通。\n\n错误码: {code}\n信息: {message}", "密钥无效", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else if (code.Contains("InvalidParameter") || code.Contains("MissingParameter"))
					{
						MessageBox.Show("密钥有效，接口可正常访问！", "验证成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						MessageBox.Show($"测试时发生未知API错误。\n\n错误码: {code}\n信息: {message}", "测试失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
				catch (Newtonsoft.Json.JsonReaderException)
				{
					MessageBox.Show("测试失败，无法解析API返回的非JSON格式响应。请检查网络或代理设置。", "解析失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"测试时发生代码异常: {ex.Message}", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			//else
			//{
			//	if (Get_html(string.Format("{0}?{1}", "https://aip.baidubce.com/oauth/2.0/token", "grant_type=client_credentials&client_id=" + text_baiduaccount.Text + "&client_secret=" + text_baidupassword.Text)) != "")
			//	{
			//		MessageBox.Show("密钥正确!", "提醒");
			//		return;
			//	}
			//	MessageBox.Show("请确保密钥正确!", "提醒");
			//}
		}
    }
}
