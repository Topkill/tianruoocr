using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
    public partial class FmMain
    {

		/// <summary>
		/// 搜狗OCR接口选择事件处理函数
		/// 切换当前OCR接口为搜狗OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_sougou_Click(object sender, EventArgs e)
		{
			OCR_foreach("搜狗");
		}


		/// <summary>
		/// 腾讯OCR接口选择事件处理函数
		/// 切换当前OCR接口为腾讯OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_tencent_Click(object sender, EventArgs e)
		{
			OCR_foreach("腾讯");
		}


		/// <summary>
		/// 腾讯高精度OCR接口选择事件处理函数
		/// 切换当前OCR接口为腾讯高精度OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_tencent_accurate_Click(object sender, EventArgs e)
		{
			OCR_foreach("腾讯-高精度");
		}


		/// <summary>
		/// 百度OCR接口选择事件处理函数
		/// 切换当前OCR接口为百度OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_Click(object sender, EventArgs e)
		{
		}


		/// <summary>
		/// 有道OCR接口选择事件处理函数
		/// 切换当前OCR接口为有道OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_youdao_Click(object sender, EventArgs e)
		{
			OCR_foreach("有道");
		}


		/// <summary>
		/// 微信OCR接口选择事件处理函数
		/// 切换当前OCR接口为微信OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_wechat_Click(object sender, EventArgs e)
		{
			OCR_foreach("微信");
		}


		/// <summary>
		/// 白描OCR接口选择事件处理函数
		/// 切换当前OCR接口为白描OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void OCR_baimiao_Click(object sender, EventArgs e)
		{
			OCR_foreach("白描");
		}


		/// <summary>
		/// 百度高精度OCR接口选择事件处理函数
		/// 切换当前OCR接口为百度高精度OCR
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_accurate_Click(object sender, EventArgs e)
		{
			OCR_foreach("百度-高精度");
		}


		/// <summary>
		/// PaddleOCR菜单点击事件处理
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_paddleocr_Click(object sender, EventArgs e)
		{
			OCR_foreach("PaddleOCR");
		}


		/// <summary>
		/// PaddleOCR2菜单点击事件处理
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_paddleocr2_Click(object sender, EventArgs e)
		{
			OCR_foreach("PaddleOCR2");
		}


		/// <summary>
		/// RapidOCR菜单点击事件处理
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_rapidocr_Click(object sender, EventArgs e)
		{
			OCR_foreach("RapidOCR");
		}


        /// <summary>
        /// 百度OCR中英文识别选项点击事件处理函数
        /// 设置百度OCR语言为中英文混合识别模式，并更新界面显示
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OCR_baidu_Ch_and_En_Click(object sender, EventArgs e)
		{
			IniHelper.SetValue("密钥_百度", "language_code", "CHN_ENG");
			OCR_foreach("中英");
		}


		/// <summary>
		/// 百度OCR日语识别选项点击事件处理函数
		/// 设置百度OCR语言为日语识别模式，并更新界面显示
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_Jap_Click(object sender, EventArgs e)
		{
			IniHelper.SetValue("密钥_百度", "language_code", "JAP");
			OCR_foreach("日语");
		}


		/// <summary>
		/// 百度OCR韩语识别选项点击事件处理函数
		/// 设置百度OCR语言为韩语识别模式，并更新界面显示
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_baidu_Kor_Click(object sender, EventArgs e)
		{
			IniHelper.SetValue("密钥_百度", "language_code", "KOR");
			OCR_foreach("韩语");
		}


		/// <summary>
		/// 根据名称设置OCR接口类型，并更新相关UI和配置文件
		/// </summary>
		/// <param name="name">OCR接口名称</param>
		private void OCR_foreach(string name)
		{
			OcrHelper.Dispose();
			var filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";

			if (name == "腾讯手写")
			{
				
				MessageBox.Show("请使用腾讯-高精度接口进行手写识别。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;

			}
			
			// --- 【核心判断逻辑：检查是否在冲突引擎间切换】 ---
			// 1. 定义哪些引擎是互相冲突的
			var conflictingEngines = new[] { "PaddleOCR", "PaddleOCR2" };

			// 2. 判断：如果 当前接口 和 新接口 都属于冲突列表，并且两者不相同
			if (interface_flag != name &&
			    conflictingEngines.Contains(interface_flag) &&
			    conflictingEngines.Contains(name))
			{
				HelpWin32.IniFileHelper.SetValue("配置", "接口", name, filePath);

				// 1. 准备询问对话框的文本内容
				string message = "PaddleOCR和PaddleOCR2互相切换，需要重启程序才能生效。如果不重启，直接进行识别程序会闪退\n\n是否立即重启？";
				string caption = "重启应用程序";
				MessageBoxButtons buttons = MessageBoxButtons.YesNo;
				DialogResult result;

				// 2. 显示对话框，并获取用户的选择结果
				result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);

				// 3. 判断用户是否点击了“是”
				if (result == DialogResult.Yes)
				{
					// 4. 如果是，则调用内置的重启方法
					Application.Restart();
					// 也可以使用手动重启方案以保证可靠性
					// System.Diagnostics.Process.Start(Application.ExecutablePath);
					// Application.Exit();
				}
				
			}

			// 当切换到其他OCR接口时，释放PaddleOCR资源
			if (interface_flag == "PaddleOCR" && name != "PaddleOCR")
			{
				// 如果是从 PaddleOCR 切换到其他接口，就调用 Reset 释放资源
				try
				{
					PaddleOCRHelper.Reset();

				}
				catch (Exception ex)
				{
					// 记录可能发生的释放错误
					CommonHelper.AddLog($"释放 PaddleOCR 引擎时出错: {ex.Message}");
				}
			}
			
			// 当切换到其他OCR接口时，释放PaddleOCR2资源
			if (interface_flag == "PaddleOCR2" && name != "PaddleOCR2")
			{
				// 如果是从 PaddleOCR2 切换到其他接口，就调用 Reset 释放资源
				try
				{
					PaddleOCR2Helper.Reset();
        		}
				catch (Exception ex)
				{
					// 记录可能发生的释放错误
					CommonHelper.AddLog($"释放 PaddleOCR2 引擎时出错: {ex.Message}");
				}
			}
			
			// 当切换到其他OCR接口时，释放RapidOCR资源
			if (interface_flag == "RapidOCR" && name != "RapidOCR")
			{
				// 如果是从 RapidOCR 切换到其他接口，就调用 Reset 释放资源
				try
				{
					RapidOCRHelper.Reset();
        		}
				catch (Exception ex)
				{
					// 记录可能发生的释放错误
					CommonHelper.AddLog($"释放 RapidOCR 引擎时出错: {ex.Message}");
				}
			}
          
            // === 【核心修复】: 如果选的不是 AI 接口，强制清除 AI 菜单的状态 ===
            if (!string.IsNullOrEmpty(name) && name != "CustomOpenAI") // 只要不是点 AI
            {
                // 1. 清除 AI 主菜单的勾选和文字
                if (ai_menu != null)
                {
                    //ai_menu.Checked = false;
                    ai_menu.Text = "AI"; // 恢复默认文字，去掉 "√" 或 "DeepSeek..."

                    // 2. 清除 AI 子菜单的勾选 (可选，保持子菜单选中状态也不错，看你习惯
					// 遍历所有AI接口(厂商) (第二级)
                    foreach (ToolStripItem item in ai_menu.DropDownItems)
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

            switch (name)
			{
				case "韩语":
					interface_flag = "韩语";
					Refresh();
					baidu.Text = "百度√";
					kor.Text = "韩语√";
					break;
				case "日语":
					interface_flag = "日语";
					Refresh();
					baidu.Text = "百度√";
					jap.Text = "日语√";
					break;
				case "中英":
					interface_flag = "中英";
					Refresh();
					baidu.Text = "百度√";
					ch_en.Text = "中英√";
					break;
				case "搜狗":
					interface_flag = "搜狗";
					Refresh();
					sougou.Text = "搜狗√";
					break;
				case "腾讯":
					interface_flag = "腾讯";
					Refresh();
					tencent.Text = "腾讯√";
					break;
				case "腾讯-高精度":
					interface_flag = "腾讯-高精度";
					Refresh();
					tencent_accurate.Text = "腾讯-高精度√";
					break;
				case "有道":
					interface_flag = "有道";
					Refresh();
					youdao.Text = "有道√";
					break;
				case "微信":
					interface_flag = "微信";
					Refresh();
					wechat.Text = "微信√";
					break;
				case "白描":
					interface_flag = "白描";
					Refresh();
					baimiao.Text = "白描√";
					break;
				case "百度-高精度":
					interface_flag = "百度-高精度";
					Refresh();
					baidu_accurate.Text = "百度-高精度√";
					break;
				 case "百度手写":
        			interface_flag = "百度手写";
        			Refresh(); // 这个方法会重置所有√
        			write.Text = "手写√"; // 将"手写"按钮标记为选中
					baidu_handwriting.Text = "百度手写√";
        			break;		
				case "公式":
					interface_flag = "公式";
					Refresh();
					Mathfuntion.Text = "公式√";
					break;
				case "百度表格":
					interface_flag = "百度表格";
					Refresh();
					ocr_table.Text = "表格√";
					baidu_table.Text = "百度√";
					break;
				case "腾讯表格":
					interface_flag = "腾讯表格";
					Refresh();
					ocr_table.Text = "表格√";
					tx_table.Text = "腾讯√";
				break;
				case "阿里表格":
					interface_flag = "阿里表格";
					Refresh();
					ocr_table.Text = "表格√";
					ali_table.Text = "阿里√";
					break;
				case "PaddleOCR":
					interface_flag = "PaddleOCR";
					Refresh();
					paddleocr.Text = "PaddleOCR√";
					break;
				case "PaddleOCR2":
					interface_flag = "PaddleOCR2";
					Refresh();
					paddleocr2.Text = "PaddleOCR2√";
					break;
				case "RapidOCR":
					interface_flag = "RapidOCR";
					Refresh();
					rapidocr.Text = "RapidOCR√";
					break;
                case "CustomOpenAI":
                    interface_flag = "CustomOpenAI";
                    Refresh(); // 先重置所有菜单文字
                               // 这里先设置一个基础状态，具体的 "AI: DeepSeek..." 文字
                               // 会由 SwitchToCustomAI 方法在后面覆盖更新
                    ai_menu.Text = "AI√";
                    break;
                case "从左向右" when !File.Exists("cvextern.dll"):
					MessageBox.Show("请从蓝奏网盘中下载cvextern.dll大小约25m，点击确定自动弹出网页。\r\n将下载后的文件与 天若OCR文字识别.exe 这个文件放在一起。");
					Process.Start("https://www.lanzous.com/i1ab3vg");
					break;
				case "从左向右":
					interface_flag = "从左向右";
					Refresh();
					shupai.Text = "竖排√";
					left_right.Text = "从左向右√";
					break;
				case "从右向左" when !File.Exists("cvextern.dll"):
					MessageBox.Show("请从蓝奏网盘中下载cvextern.dll大小约25m，点击确定自动弹出网页。\r\n将下载后的文件与 天若OCR文字识别.exe 这个文件放在一起。");
					Process.Start("https://www.lanzous.com/i1ab3vg");
					return;
				case "从右向左":
					interface_flag = "从右向左";
					Refresh();
					shupai.Text = "竖排√";
					righ_left.Text = "从右向左√";
					break;
			}

			HelpWin32.IniFileHelper.SetValue("配置", "接口", interface_flag, filePath);
		}


		/// <summary>
		/// OCR识别方向设置为竖排的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_shupai_Click(object sender, EventArgs e)
		{
		}


		/// <summary>
		/// OCR识别设置为手写的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		// private void OCR_write_Click(object sender, EventArgs e)
		// {
		// 	OCR_foreach("手写");
		// }
		private void OCR_baidu_handwriting_Click (object sender, EventArgs e)
		{
			OCR_foreach("百度手写");
		}


		private void OCR_tencent_handwriting_Click (object sender, EventArgs e)
		{
			OCR_foreach("腾讯手写");
		}


		/// <summary>
		/// OCR识别方向设置为从左向右的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_lefttoright_Click(object sender, EventArgs e)
		{
			OCR_foreach("从左向右");
		}


		/// <summary>
		/// OCR识别方向设置为从右向左的事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_righttoleft_Click(object sender, EventArgs e)
		{
			OCR_foreach("从右向左");
		}


		/// <summary>
		/// OCR数学公式识别点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void OCR_Mathfuntion_Click(object sender, EventArgs e)
		{
			OCR_foreach("公式");
		}

    }
}
