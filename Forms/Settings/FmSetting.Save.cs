using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;
using TrOCR.Helper.Models;

namespace TrOCR
{
    public sealed partial class FmSetting
    {
		// 窗口关闭事件处理函数，保存所有设置到配置文件
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
            // 【新增】停止并释放计时器，手动显式停止更好些，虽然不手动窗口关闭时也会自动停止定时器
            if (_tabHoverTimer != null)
            {
                _tabHoverTimer.Stop();
                _tabHoverTimer.Dispose();
                _tabHoverTimer = null;
            }
            saveSettings();
            DialogResult = DialogResult.OK;
        }

		public void saveSettings()
		{
            // 保存基本配置
            IniHelper.SetValue("配置", "开机自启", cbBox_开机.Checked.ToString());
            IniHelper.SetValue("配置", "快速翻译", cbBox_翻译.Checked.ToString());
            IniHelper.SetValue("配置", "识别弹窗", cbBox_弹窗.Checked.ToString());
            IniHelper.SetValue("配置", "InputTranslateClipboard", cbBox_输入翻译剪贴板.Checked.ToString());
            IniHelper.SetValue("配置", "InputTranslateAutoTranslate", cbBox_输入翻译自动翻译.Checked.ToString());
            IniHelper.SetValue("配置", "AutoCopyInputTranslation", checkBox_AutoCopyInputTranslation.Checked.ToString());
            IniHelper.SetValue("配置", "窗体动画", cobBox_动画.Text);
            IniHelper.SetValue("配置", "记录数目", numbox_记录.Text);
            IniHelper.SetValue("配置", "自动保存", cbBox_保存.Checked.ToString());
            IniHelper.SetValue("配置", "截图位置", textBox_path.Text);
            IniHelper.SetValue("常规识别", "AutoCopyOcrResult", checkBox_AutoCopyOcrResult.Checked.ToString());
            IniHelper.SetValue("工具栏", "翻译", checkBox_AutoTranslateOcrResult.Checked.ToString());
            IniHelper.SetValue("常规翻译", "AutoCopyOcrTranslation", checkBox_AutoCopyOcrTranslation.Checked.ToString());

            IniHelper.SetValue("工具栏", "IsMergeRemoveSpace", checkBox_合并去除空格.Checked.ToString());
            IniHelper.SetValue("工具栏", "IsMergeRemoveAllSpace", checkBox_合并去除所有空格.Checked.ToString());
            IniHelper.SetValue("工具栏", "IsMergeAutoCopy", checkBox_合并自动复制.Checked.ToString());
            IniHelper.SetValue("工具栏", "IsSplitAutoCopy", checkBox_拆分后自动复制.Checked.ToString());

            // 保存快捷键设置
            IniHelper.SetValue("快捷键", "文字识别", txtBox_文字识别.Text);
            IniHelper.SetValue("快捷键", "翻译文本", txtBox_翻译文本.Text);
            IniHelper.SetValue("快捷键", "记录界面", txtBox_记录界面.Text);
            IniHelper.SetValue("快捷键", "识别界面", txtBox_识别界面.Text);
            IniHelper.SetValue("快捷键", "输入翻译", txtBox_输入翻译.Text);
            IniHelper.SetValue("快捷键", "静默识别", txtBox_静默识别.Text);
            IniHelper.SetValue("快捷键", "截图翻译", txtBox_截图翻译.Text);

            // 【新增】保存剪贴板相关的设置
            IniHelper.SetValue("配置", "ListenClipboard", cbBox_ListenClipboard.Checked.ToString());
            IniHelper.SetValue("配置", "AutoCopyListenClipboardTranslation", cbBox_AutoCopyListenClipboardTranslation.Checked.ToString());
            IniHelper.SetValue("配置", "ListenClipboardTranslationHideOriginal", cbBox_ListenHideOriginal.Checked.ToString());
            IniHelper.SetValue("配置", "DisableToggleOriginalButton", cbBox_禁用隐藏原文按钮.Checked.ToString());

            //保存截图翻译相关配置
            IniHelper.SetValue("配置", "AutoCopyScreenshotTranslation", checkbox_AutoCopyScreenshotTranslation.Checked.ToString());
            IniHelper.SetValue("配置", "NoWindowScreenshotTranslation", checkbox_NoWindowScreenshotTranslation.Checked.ToString());
            // 保存"标签页悬停自动切换"设置
            IniHelper.SetValue("配置", "EnableTabHoverSwitch", checkBox_EnableTabHoverSwitch.Checked.ToString());
            // 保存百度OCR密钥和语言设置
            IniHelper.SetValue("密钥_百度", "secret_id", text_baiduaccount.Text);
            IniHelper.SetValue("密钥_百度", "secret_key", text_baidupassword.Text);
            var selectedLang = comboBox_Baidu_Language.SelectedItem?.ToString();
            var langCode = BaiduOcrHelper.GetStandardLanguages().FirstOrDefault(x => x.Value == selectedLang).Key;
            IniHelper.SetValue("密钥_百度", "language_code", langCode ?? "CHN_ENG");

            // 保存百度高精度OCR密钥和语言设置
            IniHelper.SetValue("密钥_百度高精度", "secret_id", text_baidu_accurate_apikey.Text);
            IniHelper.SetValue("密钥_百度高精度", "secret_key", text_baidu_accurate_secretkey.Text);
            var selectedAccurateLang = comboBox_Baidu_Accurate_Language.SelectedItem?.ToString();
            var accurateLangCode = BaiduOcrHelper.GetAccurateLanguages().FirstOrDefault(x => x.Value == selectedAccurateLang).Key;
            IniHelper.SetValue("密钥_百度高精度", "language_code", accurateLangCode ?? "CHN_ENG");

            // 保存百度表格识别密钥
            IniHelper.SetValue("密钥_百度表格", "secret_id", textBox2.Text);
            IniHelper.SetValue("密钥_百度表格", "secret_key", textBox1.Text);

            // 【新增】保存百度手写识别密钥
            IniHelper.SetValue("密钥_百度手写", "secret_id", text_baidu_handwriting_apikey.Text);
            IniHelper.SetValue("密钥_百度手写", "secret_key", text_baidu_handwriting_secretkey.Text);
            // 【新增】保存百度手写识别语言设置
            var selectedHandwritingLang = comboBox_Baidu_Handwriting_Language.SelectedItem?.ToString();
            var handwritingLangCode = BaiduOcrHelper.GetAccurateLanguages().FirstOrDefault(x => x.Value == selectedHandwritingLang).Key;
            IniHelper.SetValue("密钥_百度手写", "language_code", handwritingLangCode ?? "CHN_ENG");

            // 保存腾讯OCR密钥和语言设置
            IniHelper.SetValue("密钥_腾讯", "secret_id", BoxTencentId.Text);
            IniHelper.SetValue("密钥_腾讯", "secret_key", BoxTencentKey.Text);
            var selectedTencentLang = comboBox_Tencent_Language.SelectedItem?.ToString();
            var tencentLangCode = TencentOcrHelper.GetStandardLanguages().FirstOrDefault(x => x.Value == selectedTencentLang).Key;
            IniHelper.SetValue("密钥_腾讯", "language_code", tencentLangCode ?? "zh");

            // 保存腾讯高精度OCR密钥和语言设置
            IniHelper.SetValue("密钥_腾讯高精度", "secret_id", text_tencent_accurate_secretid.Text);
            IniHelper.SetValue("密钥_腾讯高精度", "secret_key", text_tencent_accurate_secretkey.Text);
            var selectedTencentAccurateLang = comboBox_Tencent_Accurate_Language.SelectedItem?.ToString();
            var tencentAccurateLangCode = TencentOcrHelper.GetAccurateLanguages().FirstOrDefault(x => x.Value == selectedTencentAccurateLang).Key;
            IniHelper.SetValue("密钥_腾讯高精度", "language_code", tencentAccurateLangCode ?? "auto");

            // 保存腾讯表格API密钥信息
            IniHelper.SetValue("密钥_腾讯表格v3", "secret_id", textBox3.Text);
            IniHelper.SetValue("密钥_腾讯表格v3", "secret_key", textBox4.Text);

            // 保存白描OCR账号信息
            IniHelper.SetValue("密钥_白描", "username", BoxBaimiaoUsername.Text);
            IniHelper.SetValue("密钥_白描", "password", BoxBaimiaoPassword.Text);

            // 保存代理设置
            IniHelper.SetValue("代理", "代理类型", combox_代理.Text);
            IniHelper.SetValue("代理", "服务器", text_服务器.Text);
            IniHelper.SetValue("代理", "端口", text_端口.Text);
            IniHelper.SetValue("代理", "需要密码", chbox_代理服务器.Checked.ToString());
            IniHelper.SetValue("代理", "服务器账号", text_账号.Text);
            IniHelper.SetValue("代理", "服务器密码", text_密码.Text);

            // 保存更新设置
            IniHelper.SetValue("更新", "检测更新", check_检查更新.Checked.ToString());
            IniHelper.SetValue("更新", "更新间隔", checkBox_更新间隔.Checked.ToString());
            IniHelper.SetValue("更新", "间隔时间", numbox_间隔时间.Value.ToString());
            IniHelper.SetValue("更新", "CheckPreRelease", checkBox_PreRelease.Checked.ToString());

            // 保存截图音效设置
            IniHelper.SetValue("截图音效", "自动保存", chbox_save.Checked.ToString());
            IniHelper.SetValue("截图音效", "音效路径", text_音效path.Text);
            IniHelper.SetValue("截图音效", "粘贴板", chbox_copy.Checked.ToString());

            // 保存取色器设置
            if (!chbox_取色.Checked)
            {
                IniHelper.SetValue("取色器", "类型", "RGB");
            }
            if (chbox_取色.Checked)
            {
                IniHelper.SetValue("取色器", "类型", "HEX");
            }

            // 保存各翻译接口设置
            IniHelper.SetValue("Translate_Google", "Source", textBox_Google_Source.Text);
            IniHelper.SetValue("Translate_Google", "Target", textBox_Google_Target.Text);

            IniHelper.SetValue("Translate_Baidu", "Source", textBox_Baidu_Source.Text);
            IniHelper.SetValue("Translate_Baidu", "Target", textBox_Baidu_Target.Text);
            IniHelper.SetValue("Translate_Baidu", "APP_ID", textBox_Baidu_AK.Text);
            IniHelper.SetValue("Translate_Baidu", "APP_KEY", textBox_Baidu_SK.Text);

            IniHelper.SetValue("Translate_Tencent", "Source", textBox_Tencent_Source.Text);
            IniHelper.SetValue("Translate_Tencent", "Target", textBox_Tencent_Target.Text);
            IniHelper.SetValue("Translate_Tencent", "SecretId", textBox_Tencent_AK.Text);
            IniHelper.SetValue("Translate_Tencent", "SecretKey", textBox_Tencent_SK.Text);

            IniHelper.SetValue("Translate_Bing", "Source", textBox_Bing_Source.Text);
            IniHelper.SetValue("Translate_Bing", "Target", textBox_Bing_Target.Text);

            IniHelper.SetValue("Translate_Bing2", "Source", textBox_Bing2_Source.Text);
            IniHelper.SetValue("Translate_Bing2", "Target", textBox_Bing2_Target.Text);

            IniHelper.SetValue("Translate_Microsoft", "Source", textBox_Microsoft_Source.Text);
            IniHelper.SetValue("Translate_Microsoft", "Target", textBox_Microsoft_Target.Text);

            IniHelper.SetValue("Translate_Yandex", "Source", textBox_Yandex_Source.Text);
            IniHelper.SetValue("Translate_Yandex", "Target", textBox_Yandex_Target.Text);

            // 腾讯交互翻译
            IniHelper.SetValue("Translate_TencentInteractive", "Source", textBox_TencentInteractive_Source.Text);
            IniHelper.SetValue("Translate_TencentInteractive", "Target", textBox_TencentInteractive_Target.Text);

            // 彩云小译
            IniHelper.SetValue("Translate_Caiyun", "Source", textBox_Caiyun_Source.Text);
            IniHelper.SetValue("Translate_Caiyun", "Target", textBox_Caiyun_Target.Text);

            // 火山翻译
            IniHelper.SetValue("Translate_Volcano", "Source", textBox_Volcano_Source.Text);
            IniHelper.SetValue("Translate_Volcano", "Target", textBox_Volcano_Target.Text);

            //百度翻译2
            IniHelper.SetValue("Translate_Baidu2", "Source", textBox_Baidu2_Source.Text);
            IniHelper.SetValue("Translate_Baidu2", "Target", textBox_Baidu2_Target.Text);

            // 彩云小译2
            IniHelper.SetValue("Translate_Caiyun2", "Source", textBox_Caiyun2_Source.Text);
            IniHelper.SetValue("Translate_Caiyun2", "Target", textBox_Caiyun2_Target.Text);
            IniHelper.SetValue("Translate_Caiyun2", "Token", textBox_Caiyun2_Token.Text);

            // 保存翻译接口显示设置
            IniHelper.SetValue("翻译接口显示", "Google", checkBox_ShowGoogle.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Baidu", checkBox_ShowBaidu.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Tencent", checkBox_ShowTencent.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Bing", checkBox_ShowBing.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Bing2", checkBox_ShowBing2.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Microsoft", checkBox_ShowMicrosoft.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Yandex", checkBox_ShowYandex.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "TencentInteractive", checkBox_ShowTencentInteractive.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Caiyun", checkBox_ShowCaiyun.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Volcano", checkBox_ShowVolcano.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Caiyun2", checkBox_ShowCaiyun2.Checked.ToString());
            IniHelper.SetValue("翻译接口显示", "Baidu2", checkBox_ShowBaidu2.Checked.ToString());

            // 保存OCR接口显示设置
            IniHelper.SetValue("Ocr接口显示", "Baidu", checkBox_ShowOcrBaidu.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "BaiduAccurate", checkBox_ShowOcrBaiduAccurate.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "Tencent", checkBox_ShowOcrTencent.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "TencentAccurate", checkBox_ShowOcrTencentAccurate.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "Baimiao", checkBox_ShowOcrBaimiao.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "Sougou", checkBox_ShowOcrSougou.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "Youdao", checkBox_ShowOcrYoudao.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "WeChat", checkBox_ShowOcrWeChat.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "Mathfuntion", checkBox_ShowOcrMathfuntion.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "Table", checkBox_ShowOcrTable.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "Shupai", checkBox_ShowOcrShupai.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "TableBaidu", checkBox_ShowOcrTableBaidu.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "TableAli", checkBox_ShowOcrTableAli.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "ShupaiLR", checkBox_ShowOcrShupaiLR.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "ShupaiRL", checkBox_ShowOcrShupaiRL.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "TencentTable", checkBox_ShowOcrTableTencent.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "PaddleOCR", checkBox_ShowOcrPaddleOCR.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "PaddleOCR2", checkBox_ShowOcrPaddleOCR2.Checked.ToString());
            IniHelper.SetValue("Ocr接口显示", "RapidOCR", checkBox_ShowOcrRapidOCR.Checked.ToString());

            // 保存OCR模型配置
            // PaddleOCR配置
            IniHelper.SetValue("模型配置_PaddleOCR", "Det", textBox_PaddleOCR_Det.Text);
            IniHelper.SetValue("模型配置_PaddleOCR", "Cls", textBox_PaddleOCR_Cls.Text);
            IniHelper.SetValue("模型配置_PaddleOCR", "Rec", textBox_PaddleOCR_Rec.Text);
            IniHelper.SetValue("模型配置_PaddleOCR", "Keys", textBox_PaddleOCR_Keys.Text);
            IniHelper.SetValue("模型配置_PaddleOCR", "AdvancedConfig", textBox5.Text);


            // PaddleOCR2配置
            IniHelper.SetValue("模型配置_PaddleOCR2", "Det", textBox_PaddleOCR2_Det.Text);
            IniHelper.SetValue("模型配置_PaddleOCR2", "Cls", textBox_PaddleOCR2_Cls.Text);
            IniHelper.SetValue("模型配置_PaddleOCR2", "Rec", textBox_PaddleOCR2_Rec.Text);
            IniHelper.SetValue("模型配置_PaddleOCR2", "Keys", textBox_PaddleOCR2_Keys.Text);
            IniHelper.SetValue("模型配置_PaddleOCR2", "AdvancedConfig", textBox6.Text);
            IniHelper.SetValue("模型配置_PaddleOCR2", "Det_Version", comboBox_PaddleOCR2_det_Version.SelectedItem?.ToString() ?? "v5");
            IniHelper.SetValue("模型配置_PaddleOCR2", "Cls_Version", comboBox_PaddleOCR2_cls_Version.SelectedItem?.ToString() ?? "v5");
            IniHelper.SetValue("模型配置_PaddleOCR2", "Rec_Version", comboBox_PaddleOCR2_rec_Version.SelectedItem?.ToString() ?? "v5");

            // RapidOCR配置
            IniHelper.SetValue("模型配置_RapidOCR", "Det", textBox_RapidOCR_Det.Text);
            IniHelper.SetValue("模型配置_RapidOCR", "Cls", textBox_RapidOCR_Cls.Text);
            IniHelper.SetValue("模型配置_RapidOCR", "Rec", textBox_RapidOCR_Rec.Text);
            IniHelper.SetValue("模型配置_RapidOCR", "Keys", textBox_RapidOCR_Keys.Text);
            IniHelper.SetValue("模型配置_RapidOCR", "AdvancedConfig", textBox7.Text);

            // 保存OpenAICompatible OCR配置
            // === 新增：保存自定义 AI 接口列表 ===
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "CustomOpenAIProviders.json");
                string json = JsonConvert.SerializeObject(_customProviders, Formatting.Indented);
                File.WriteAllText(jsonPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存自定义接口失败: " + ex.Message);
            }
            // 保存OpenAICompatible 翻译配置
            // === 新增：保存自定义 AI 接口列表 ===
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "CustomOpenAITransProviders.json");
                string json = JsonConvert.SerializeObject(_customTransProviders, Formatting.Indented);
                File.WriteAllText(jsonPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存自定义翻译接口失败: " + ex.Message);
            }

            //文本改变后自动翻译的延时
            IniHelper.SetValue("配置", "文本改变自动翻译延时", textBox38.Text);
            //工具栏图标放大倍数
            IniHelper.SetValue("工具栏", "图标放大倍数", textBox37.Text);
			//文字缩放倍数
			 IniHelper.SetValue("配置", "文字缩放倍数", textBox39.Text);


            ResetOcrEngineOnConfigChange();
            StaticValue.LoadConfig();

           
        }

        /// <summary>
        /// 检测关键配置文件是否位于 Data 目录之外
        /// </summary>
        /// <returns>返回外部文件的描述列表，如果为空则表示都在 Data 目录下</returns>
        /// <summary>
        /// 全面检测关键数据文件（包括INI配置和JSON中的AI模式文件）是否位于 Data 目录之外
        /// </summary>
        /// <returns>返回外部文件的描述列表，如果为空则表示数据都在 Data 目录下</returns>
        private List<string> CheckForExternalFiles()
        {
            List<string> externalFiles = new List<string>();

            // 1. 获取 Data 目录的绝对路径 (作为判断基准)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dataDir = Path.GetFullPath(Path.Combine(baseDir, "Data"));

            // 确保路径以分隔符结尾，防止 "DataBackup" 这种文件夹被误判为在 "Data" 内
            if (!dataDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                dataDir += Path.DirectorySeparatorChar;
            }
            // =========================================================
            // Part A: 检查 INI 文件中的常规路径配置
            // =========================================================
            var iniKeysToCheck = new List<(string Section, string Key, string Description)>
            {
                ("模型配置_PaddleOCR", "AdvancedConfig", "PaddleOCR 高级配置文件"),
                ("模型配置_PaddleOCR2", "AdvancedConfig", "PaddleOCR2 高级配置文件"),
                ("模型配置_RapidOCR", "AdvancedConfig", "RapidOCR 高级配置文件"),
            };

            foreach (var item in iniKeysToCheck)
            {
                string pathStr = IniHelper.GetValue(item.Section, item.Key);
                //PaddleOCR 使用默认高级配置文件即默认配置时，弹窗警告
                if (item.Section == "模型配置_PaddleOCR" && string.IsNullOrWhiteSpace(pathStr))
                {
					pathStr = @"PaddleOCR_data\win_x64\inference\PaddleOCR.config.json";
                }
                CheckPath(pathStr, $"[INI设置] {item.Description}", baseDir, dataDir, externalFiles);
            }

            // =========================================================
            // Part B: 检查 AI 接口 JSON 中的模式文件路径
            // =========================================================

            // 需要检查的两个 JSON 文件
            var jsonFiles = new List<(string FileName, string Description)>
			{
				("CustomOpenAIProviders.json", "AI OCR 接口配置"),
				("CustomOpenAITransProviders.json", "AI 翻译接口配置")
			};

            foreach (var jsonInfo in jsonFiles)
            {
                string jsonFilePath = Path.Combine(dataDir, jsonInfo.FileName);
                if (!File.Exists(jsonFilePath)) continue;

                try
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    // 使用 JArray 动态解析，不需要依赖具体的实体类定义
                    var providers = JArray.Parse(jsonContent);

                    foreach (var provider in providers)
                    {
                        // 获取接口名称，方便提示
                        string providerName = provider["Name"]?.ToString() ?? "未命名接口";
                        // 获取模式文件路径
                        string modelConfigPath = provider["ModelConfigPath"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(modelConfigPath))
                        {
                            CheckPath(modelConfigPath,
                                $"[{jsonInfo.Description}] {providerName} 的模式文件",
                                baseDir, dataDir, externalFiles);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"解析 {jsonInfo.FileName} 失败: {ex.Message}");
                }
            }

            return externalFiles;
        }

        /// <summary>
        /// 辅助方法：验证单个路径是否在 Data 目录外
        /// </summary>
        private void CheckPath(string pathStr, string description, string baseDir, string dataDir, List<string> results)
        {
            if (string.IsNullOrWhiteSpace(pathStr) || pathStr == "发生错误") return;

            try
            {
                // 1. 处理路径：转换为绝对路径
                string fullPath;
                if (Path.IsPathRooted(pathStr))
                {
                    fullPath = Path.GetFullPath(pathStr);
                }
                else
                {
                    fullPath = Path.GetFullPath(Path.Combine(baseDir, pathStr));
                }

                // 2. 检查文件/文件夹是否存在（不存在的文件不用警告，反正也备份不了）
                bool exists = File.Exists(fullPath) || Directory.Exists(fullPath);
                if (!exists) return;

                // 3. 核心判断：是否以 Data 目录路径开头
                if (!fullPath.StartsWith(dataDir, StringComparison.CurrentCultureIgnoreCase))
                {
                    results.Add($"{description}:\n    {pathStr}");
                }
            }
            catch(Exception ex)
            {
                // 容错处理：防止非法路径字符串导致崩溃
                Debug.WriteLine($"路径检查出错: {ex.Message}");
            }
        }
    }
}
