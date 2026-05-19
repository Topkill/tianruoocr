using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
        // 通用的“失去焦点时尝试转相对路径”事件处理器
        private void PathTextBox_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox tb)
            {
                // 1. 去除首尾空格 AND 去除首尾双引号
                // Windows "复制为路径" 会带引号，必须去掉
                string inputPath = tb.Text.Trim().Trim('"');

                // 如果用户粘贴了带引号的路径，我们顺手帮他在 UI 上也修正过来
                if (inputPath != tb.Text)
                {
                    tb.Text = inputPath;
                }

                // 2. 如果内容为空，或者已经是相对路径，直接跳过
                if (string.IsNullOrEmpty(inputPath) || !Path.IsPathRooted(inputPath))
                {
                    return;
                }

                // 3. 调用转换方法
                string newPath = TrOCRUtils.ConvertToRelativePathIfPossible(inputPath);

                // 4. 更新 UI
                if (newPath != inputPath)
                {
                    tb.Text = newPath;
                }
            }
        }

        /// <summary>
        /// 读取OCR模型配置
        /// </summary>
        private void ReadOcrModelConfigs()
		{
			// 读取PaddleOCR配置
			textBox_PaddleOCR_Det.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR", "Det", PaddleOCRHelper.DefaultDetModel);
			textBox_PaddleOCR_Cls.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR", "Cls", PaddleOCRHelper.DefaultClsModel);
			textBox_PaddleOCR_Rec.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR", "Rec", PaddleOCRHelper.DefaultRecModel);
			textBox_PaddleOCR_Keys.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR", "Keys", PaddleOCRHelper.DefaultKeys);
			textBox5.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR", "AdvancedConfig", "");


			// 读取PaddleOCR2配置
			textBox_PaddleOCR2_Det.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR2", "Det", PaddleOCR2Helper.DefaultDetModel);
			textBox_PaddleOCR2_Cls.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR2", "Cls", PaddleOCR2Helper.DefaultClsModel);
			textBox_PaddleOCR2_Rec.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR2", "Rec", PaddleOCR2Helper.DefaultRecModel);
			textBox_PaddleOCR2_Keys.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR2", "Keys", PaddleOCR2Helper.DefaultKeys);
			textBox6.Text = TrOCRUtils.LoadSetting("模型配置_PaddleOCR2", "AdvancedConfig", "");

			var paddleOcr2DetVersion = GetConfigValue("模型配置_PaddleOCR2", "Det_Version");
			var paddleOcr2ClsVersion = GetConfigValue("模型配置_PaddleOCR2", "Cls_Version");
			var paddleOcr2RecVersion = GetConfigValue("模型配置_PaddleOCR2", "Rec_Version");
			comboBox_PaddleOCR2_det_Version.SelectedItem = string.IsNullOrEmpty(paddleOcr2DetVersion) ? "v5" : paddleOcr2DetVersion;
			comboBox_PaddleOCR2_cls_Version.SelectedItem = string.IsNullOrEmpty(paddleOcr2ClsVersion) ? "v5" : paddleOcr2ClsVersion;
			comboBox_PaddleOCR2_rec_Version.SelectedItem = string.IsNullOrEmpty(paddleOcr2RecVersion) ? "v5" : paddleOcr2RecVersion;


			// 读取RapidOCR配置
			textBox_RapidOCR_Det.Text = TrOCRUtils.LoadSetting("模型配置_RapidOCR", "Det",RapidOCRHelper.DefaultDetModel);
			textBox_RapidOCR_Cls.Text = TrOCRUtils.LoadSetting("模型配置_RapidOCR", "Cls",RapidOCRHelper.DefaultClsModel);
			textBox_RapidOCR_Rec.Text = TrOCRUtils.LoadSetting("模型配置_RapidOCR", "Rec",RapidOCRHelper.DefaultRecModel);
			textBox_RapidOCR_Keys.Text = TrOCRUtils.LoadSetting("模型配置_RapidOCR", "Keys",RapidOCRHelper.DefaultKeys);
			textBox7.Text = TrOCRUtils.LoadSetting("模型配置_RapidOCR", "AdvancedConfig","");
        }

        /// <summary>
        /// 获取配置值的辅助方法
        /// </summary>
        private string GetConfigValue(string section, string key)
        {
            var value = IniHelper.GetValue(section, key);
            return value == "发生错误" ? "" : value;
        }

        /// <summary>
        /// 浏览模型文件的通用方法
        /// </summary>
        private void BrowseModelFile(TextBox textBox, string description)
        {
			// 1. 创建 VistaFolderBrowserDialog 实例
    		using (var vistaFolderDialog = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog())
    		{
    		    // 2. 设置属性，它和新版文件选择框的体验一致
    		    vistaFolderDialog.Description = description;
    		    vistaFolderDialog.UseDescriptionForTitle = true; // 将描述作为标题

        		// 3. 显示对话框并获取结果
        		if (vistaFolderDialog.ShowDialog() == DialogResult.OK)
                {  
                    textBox.Text = TrOCRUtils.ConvertToRelativePathIfPossible(vistaFolderDialog.SelectedPath); 
        		}
    		}
        }

		/// <summary>
        /// 浏览模型字典文件的通用方法
        /// </summary>
        private void BrowseKeysModelFile(TextBox textBox, string description)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = $"请选择{description}";
                openFileDialog.Filter = "字典文件 (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = TrOCRUtils.ConvertToRelativePathIfPossible(openFileDialog.FileName);
                }
            }
        }

        /// <summary>
		/// 浏览RapidOCR模型文件的专用方法（只选择onnx文件）
		/// </summary>
		private void BrowseRapidOcrModelFile(TextBox textBox, string description)
		{
			using (var openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Title = $"请选择{description}";
				openFileDialog.Filter = "ONNX模型文件 (*.onnx)|*.onnx";
				openFileDialog.FilterIndex = 1;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					textBox.Text = TrOCRUtils.ConvertToRelativePathIfPossible(openFileDialog.FileName);
				}
			}
		}

		/// <summary>
        /// AI接口和离线接口浏览高级配置文件的专用方法（只选择json文件）
        /// </summary>
        private void BrowseAdvancedConfigModelFile(TextBox textBox, string description)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = $"请选择{description}";
                openFileDialog.Filter = "json文件 (*.json)|*.json";
                openFileDialog.FilterIndex = 1;
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = TrOCRUtils.ConvertToRelativePathIfPossible(openFileDialog.FileName);
                }
            }
        }

        // PaddleOCR事件处理方法
		private void Btn_PaddleOCR_Det_Browse_Click(object sender, EventArgs e) => BrowseModelFile(textBox_PaddleOCR_Det, "PaddleOCR检测模型");

        private void Btn_PaddleOCR_Cls_Browse_Click(object sender, EventArgs e) => BrowseModelFile(textBox_PaddleOCR_Cls, "PaddleOCR方向模型");

        private void Btn_PaddleOCR_Rec_Browse_Click(object sender, EventArgs e) => BrowseModelFile(textBox_PaddleOCR_Rec, "PaddleOCR识别模型");

        private void Btn_PaddleOCR_Keys_Browse_Click(object sender, EventArgs e) => BrowseKeysModelFile(textBox_PaddleOCR_Keys, "PaddleOCR字典文件");

		private void Btn_PaddleOCR_AdvancedConfig_Browse_Click(object sender, EventArgs e) => BrowseAdvancedConfigModelFile(textBox5, "PaddleOCR高级配置文件");

        private void TextBox_PaddleOCR_TextChanged(object sender, EventArgs e)
		{
			this.paddleOcrConfigChanged = true; // 只设置标志位
		}

        // PaddleOCR2事件处理方法
        private void Btn_PaddleOCR2_Det_Browse_Click(object sender, EventArgs e) => BrowseModelFile(textBox_PaddleOCR2_Det, "PaddleOCR2检测模型");

        private void Btn_PaddleOCR2_Cls_Browse_Click(object sender, EventArgs e) => BrowseModelFile(textBox_PaddleOCR2_Cls, "PaddleOCR2方向模型");

        private void Btn_PaddleOCR2_Rec_Browse_Click(object sender, EventArgs e) => BrowseModelFile(textBox_PaddleOCR2_Rec, "PaddleOCR2识别模型");

        private void Btn_PaddleOCR2_Keys_Browse_Click(object sender, EventArgs e) => BrowseKeysModelFile(textBox_PaddleOCR2_Keys, "PaddleOCR2字典文件");

		private void Btn_PaddleOCR2_AdvancedConfig_Browse_Click(object sender, EventArgs e) => BrowseAdvancedConfigModelFile(textBox6, "PaddleOCR2高级配置文件");

        private void TextBox_PaddleOCR2_TextChanged(object sender, EventArgs e)
		{
			this.paddleOcr2ConfigChanged = true;
		}

        private void ComboBox_PaddleOCR2_Version_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.paddleOcr2ConfigChanged = true;
        }

        // RapidOCR事件处理方法
        private void Btn_RapidOCR_Det_Browse_Click(object sender, EventArgs e) => BrowseRapidOcrModelFile(textBox_RapidOCR_Det, "RapidOCR检测模型");

        private void Btn_RapidOCR_Cls_Browse_Click(object sender, EventArgs e) => BrowseRapidOcrModelFile(textBox_RapidOCR_Cls, "RapidOCR方向模型");

        private void Btn_RapidOCR_Rec_Browse_Click(object sender, EventArgs e) => BrowseRapidOcrModelFile(textBox_RapidOCR_Rec, "RapidOCR识别模型");

        private void Btn_RapidOCR_Keys_Browse_Click(object sender, EventArgs e) => BrowseKeysModelFile(textBox_RapidOCR_Keys, "RapidOCR字典文件");

		private void Btn_RapidOCR_AdvancedConfig_Browse_Click(object sender, EventArgs e) => BrowseAdvancedConfigModelFile(textBox7, "RapidOCR高级配置文件");

        private void TextBox_RapidOCR_TextChanged(object sender, EventArgs e)
  {
   this.rapidOcrConfigChanged = true;
  }

        /// <summary>
        /// 配置变更时重置OCR引擎
        /// </summary>
        private void ResetOcrEngineOnConfigChange()
        {
			try
			{
				// 检查标志位，只重置那些配置被修改过的引擎
				if (this.paddleOcrConfigChanged)
				{
					PaddleOCRHelper.Reset();
					Debug.WriteLine("PaddleOCR configuration changed, engine has been reset.");
				}

				if (this.paddleOcr2ConfigChanged)
				{
					PaddleOCR2Helper.Reset();
					Debug.WriteLine("PaddleOCR2 configuration changed, engine has been reset.");
				}

				if (this.rapidOcrConfigChanged)
				{
					RapidOCRHelper.Reset();
					Debug.WriteLine("RapidOCR configuration changed, engine has been reset.");
    			}

			}
			catch (Exception ex)
			{
				// 处理重置异常，避免影响用户体验
				System.Diagnostics.Debug.WriteLine($"重置引擎时发生异常: {ex.Message}");
				MessageBox.Show($"重置引擎时发生异常: {ex.Message}");
			}
        }

        private void tabControl2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // 获取当前在 tabControl2 中被选中的选项卡页
            TabPage selectedTab = tabControl2.SelectedTab;

            // 默认先显示按钮和说明区域
            bool showApiButtons = true;
            bool showHelpBox = false;
            string helpText = "";

            // 使用 switch 语句，让逻辑更清晰
            switch (selectedTab.Name)
            {
                case "inPage_PaddleOCR":
                    showApiButtons = false; // 隐藏在线API按钮
                    showHelpBox = true;   // 显示说明区域
                    helpText = "说明1：此接口仅支持64位操作系统，支持ppocrv5及更低版本模型，推荐优先使用。\n\n" +
                               "说明2：需要CPU支持AVX指令集，不支持请使用PaddleOCR2接口。\n\n" +
                               "说明3：高级配置文件留空则使用默认值，普通用户推荐留空";
                    break;

                case "inPage_PaddleOCR2":
                    showApiButtons = false;
                    showHelpBox = true;
                    helpText = "说明1：此接口仅支持64位操作系统，支持ppocrv5及更低版本模型。\n\n" +
                               "说明2：无需CPU支持AVX指令集，兼容性更好，但识别速度可能较慢。\n\n" +
                               "说明3：高级配置文件留空则使用默认值，普通用户推荐留空";
                    break;

                case "inPage_RapidOCR":
                    showApiButtons = false;
                    showHelpBox = true;
                    helpText = "说明1：此接口支持32位和64位操作系统，不支持ppocrv5版本模型，支持ppocrv4及更低版本模型。\n\n" +
                               "说明2：不知道CPU是否需要支持AVX指令集，自行测试。\n\n" +
                               "说明3：高级配置文件留空则使用默认值，普通用户推荐留空";
                    break;

                // 默认情况，适用于百度OCR、腾讯OCR等所有其他选项卡
                default:
                    showApiButtons = true;  // 显示在线API按钮
                    showHelpBox = false;  // 隐藏说明区域
                    helpText = "";
                    break;
            }

            // --- 统一更新UI ---

            // 更新三个按钮的可见性
            密钥Button_apply.Visible = showApiButtons; // "接口申请"
            百度_btn.Visible = showApiButtons;         // "密钥测试"
            密钥Button.Visible = showApiButtons;       // "恢复默认"

            // 更新说明区域的可见性和文本内容
            // (请将 "groupBox_Help" 替换为您在第一步中添加的GroupBox的实际名称)
            groupBox7.Visible = showHelpBox;
            label_OcrApiHelpText.Text = helpText;
        }
    }
}
