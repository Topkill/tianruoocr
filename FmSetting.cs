using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Win32;
using TrOCR.Helper;
using TrOCR.Properties;

namespace TrOCR
{
    // 设置窗口类，用于管理OCR和翻译接口的各种配置选项
	public sealed partial class FmSetting
	{
		private Dictionary<Control, Point> _originalControlLocations;

		// 构造函数，初始化设置窗口
		public FmSetting()
		{
			Font = new Font(Font.Name, 9f / StaticValue.DpiFactor, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
			InitializeComponent();
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

			var value_input_translate = IniHelper.GetValue("快捷键", "输入翻译");
			txtBox_输入翻译.Text = value_input_translate;
			if (value_input_translate == "发生错误")
			{
				txtBox_输入翻译.Text = "请按下快捷键";
			}
			pictureBox_输入翻译.Image = txtBox_输入翻译.Text == "请按下快捷键" ? Resources.快捷键_0 : Resources.快捷键_1;
			
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
			comboBox_Tencent_Language.SelectedItem = string.IsNullOrEmpty(tencentLangName) ? "中英文混合" : tencentLangName;
	
			// 读取腾讯高精度OCR密钥信息
			var valueTencentAccurateId = IniHelper.GetValue("密钥_腾讯高精度", "secret_id");
			text_tencent_accurate_secretid.Text = valueTencentAccurateId == "发生错误" ? "" : valueTencentAccurateId;
			var valueTencentAccurateKey = IniHelper.GetValue("密钥_腾讯高精度", "secret_key");
			text_tencent_accurate_secretkey.Text = valueTencentAccurateKey == "发生错误" ? "" : valueTencentAccurateKey;
			
			// 读取腾讯高精度OCR语言设置
			var valueTencentAccurateLanguage = IniHelper.GetValue("密钥_腾讯高精度", "language_code");
			TencentOcrHelper.GetAccurateLanguages().TryGetValue(valueTencentAccurateLanguage, out string tencentAccurateLangName);
			comboBox_Tencent_Accurate_Language.SelectedItem = string.IsNullOrEmpty(tencentAccurateLangName) ? "自动检测" : tencentAccurateLangName;

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
				if (apiName == "TencentInteractive" || apiName == "Caiyun" || apiName == "Volcano")
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
				IniHelper.SetValue("翻译接口显示", apiName, checkBox.Checked.ToString());
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
				IniHelper.SetValue("Ocr接口显示", apiName, checkBox.Checked.ToString());
			};

			setOcrVisibility("Baidu", checkBox_ShowOcrBaidu, inPage_百度接口);
			setOcrVisibility("BaiduAccurate", checkBox_ShowOcrBaiduAccurate, inPage_百度高精度接口);
			setOcrVisibility("Tencent", checkBox_ShowOcrTencent, inPage腾讯接口);
			setOcrVisibility("TencentAccurate", checkBox_ShowOcrTencentAccurate, inPage腾讯高精度接口);
			setOcrVisibility("Baimiao", checkBox_ShowOcrBaimiao, inPage白描接口);
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
			inPage腾讯高精度接口.Controls.Add(btnResetTencentAccurateLang);
			
			// 为腾讯标准版添加重置按钮
			Button btnResetTencentLang = new Button();
			btnResetTencentLang.Name = "btnResetTencentLang";
			btnResetTencentLang.Text = "重置";
			btnResetTencentLang.Size = new Size(50, 23);
			btnResetTencentLang.Location = new Point(comboBox_Tencent_Language.Right + 6, comboBox_Tencent_Language.Top - 2);
			btnResetTencentLang.Click += (s, ev) => { comboBox_Tencent_Language.SelectedItem = "中英文混合"; };
			inPage腾讯接口.Controls.Add(btnResetTencentLang);

			readIniFile();
			// 使用程序集的实际版本号，而不是写死的值
			label_VersionInfo.Text = "版本号：" + System.Windows.Forms.Application.ProductVersion;
			label_AuthorInfo.Text = "作者：topkill";
			chbox_代理服务器.CheckedChanged += chbox_代理服务器_CheckedChanged;
			更新Button_check.Click += 更新Button_check_Click;

			StoreOriginalLocations(this);
			tab_标签.SelectedIndexChanged += AdjustPageSize;
			tabControl2.SelectedIndexChanged += AdjustPageSize;
			tabControl_Trans.SelectedIndexChanged += AdjustPageSize;
			AdjustPageSize(tab_标签, EventArgs.Empty);
		}

		/// <summary>
		/// ocr接口申请按钮点击事件处理函数，根据当前选中的标签页打开相应的OCR服务申请页面
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 百度申请_Click(object sender, EventArgs e)
		{
			if (tabControl2.SelectedTab == inPage腾讯接口 || tabControl2.SelectedTab == inPage腾讯高精度接口)
			{
				Process.Start("https://console.cloud.tencent.com/ocr/general");
			}
			else if (tabControl2.SelectedTab == inPage白描接口)
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
		/// 通过HTTP POST请求获取指定URL的响应内容
		/// </summary>
		/// <param name="url">需要请求的URL地址</param>
		/// <returns>服务器返回的响应内容，如果请求失败则返回空字符串</returns>

		public static string Get_html(string url)
		{
			string result;
			var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			try
			{
				using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
						streamReader.Close();
						httpWebResponse.Close();
					}
				}
				httpWebRequest.Abort();
			}
			catch
			{
				result = "";
			}
			return result;
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
		/// 密钥按钮点击事件处理函数，根据当前选中的标签页恢复对应接口的默认设置
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 密钥Button_Click(object sender, EventArgs e)
		{
			// 根据当前选中的标签页，只恢复对应接口的默认设置
			if (tabControl2.SelectedTab == inPage_百度接口)
			{
				// 恢复百度OCR默认密钥
				text_baiduaccount.Text = "YsZKG1wha34PlDOPYaIrIIKO";
				text_baidupassword.Text = "HPRZtdOHrdnnETVsZM2Nx7vbDkMfxrkD";
				comboBox_Baidu_Language.SelectedItem = "中英文混合";
			}
				       else if (tabControl2.SelectedTab == inPage_百度高精度接口)
				       {
				           text_baidu_accurate_apikey.Text = "";
				           text_baidu_accurate_secretkey.Text = "";
				           comboBox_Baidu_Accurate_Language.SelectedItem = "中英文混合";
				       }
			else if (tabControl2.SelectedTab == inPage腾讯接口)
			{
				// 清空腾讯OCR密钥
				BoxTencentId.Text = "";
				BoxTencentKey.Text = "";
				comboBox_Tencent_Language.SelectedItem = "中英文混合";
			}
				       else if (tabControl2.SelectedTab == inPage腾讯高精度接口)
				       {
				           text_tencent_accurate_secretid.Text = "";
				           text_tencent_accurate_secretkey.Text = "";
				           comboBox_Tencent_Accurate_Language.SelectedItem = "自动检测";
				       }
			else if (tabControl2.SelectedTab == inPage白描接口)
			{
				// 清空白描账号密码
				BoxBaimiaoUsername.Text = "";
				BoxBaimiaoPassword.Text = "";
			}
		}

		/// <summary>
		/// 文件夹浏览器对话框帮助请求事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
		{
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
		}

		/// <summary>
		/// 文本框按键抬起事件处理函数，用于设置快捷键
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">按键事件参数</param>
		private void txtBox_KeyUp(object sender, KeyEventArgs e)
		{
			// 获取触发事件的文本框控件
			var textBox = sender as TextBox;
			// 创建正则表达式匹配中文字符
			var regex = new Regex("[一-龥]+");
			var str = "";
			// 提取文本框名称中的中文字符
			foreach (var obj in regex.Matches(textBox.Name))
			{
				str = ((Match)obj).ToString();
			}
			// 构造对应的图片控件名称并查找
			var key = "pictureBox_" + str;
			var pictureBox = (PictureBox)Controls.Find(key, true)[0];
			// 创建资源管理器实例
			new ComponentResourceManager(typeof(FmSetting));
			// 处理退格键事件
			if (e.KeyData == Keys.Back)
			{
				// 重置文本框文本和图片
				textBox.Text = "请按下快捷键";
				pictureBox.Image = Resources.快捷键_0;
				// 根据文本框名称保存对应的快捷键设置
				if (textBox.Name.Contains("文字识别"))
				{
					IniHelper.SetValue("快捷键", "文字识别", txtBox_文字识别.Text);
				}
				if (textBox.Name.Contains("翻译文本"))
				{
					IniHelper.SetValue("快捷键", "翻译文本", txtBox_翻译文本.Text);
				}
				if (textBox.Name.Contains("记录界面"))
				{
					IniHelper.SetValue("快捷键", "记录界面", txtBox_记录界面.Text);
				}
				if (textBox.Name.Contains("识别界面"))
				{
					IniHelper.SetValue("快捷键", "识别界面", txtBox_识别界面.Text);
					           }
				if (textBox.Name.Contains("输入翻译"))
				{
					IniHelper.SetValue("快捷键", "输入翻译", txtBox_输入翻译.Text);
				}
			}
			// 处理非功能键的按键事件
			else if (e.KeyValue != 16 && e.KeyValue != 17 && e.KeyValue != 18)
			{
				// 解析按键组合并格式化显示
				var array = e.KeyData.ToString().Replace(" ", "").Replace("Control", "Ctrl").Split(',');
				pictureBox.Image = Resources.快捷键_1;
				// 显示单个按键或组合按键
				if (array.Length == 1)
				{
					textBox.Text = array[0];
				}
				if (array.Length == 2)
				{
					textBox.Text = array[1] + "+" + array[0];
				}
				// 保存快捷键设置
				if (array.Length <= 2)
				{
					if (textBox.Name.Contains("文字识别"))
					{
						IniHelper.SetValue("快捷键", "文字识别", txtBox_文字识别.Text);
					}
					if (textBox.Name.Contains("翻译文本"))
					{
						IniHelper.SetValue("快捷键", "翻译文本", txtBox_翻译文本.Text);
					}
					if (textBox.Name.Contains("记录界面"))
					{
						IniHelper.SetValue("快捷键", "记录界面", txtBox_记录界面.Text);
					}
					if (textBox.Name.Contains("识别界面"))
					{
						IniHelper.SetValue("快捷键", "识别界面", txtBox_识别界面.Text);
					}
					if (textBox.Name.Contains("输入翻译"))
					{
						IniHelper.SetValue("快捷键", "输入翻译", txtBox_输入翻译.Text);
					}
				}
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
			return (keyData == Keys.Tab && txtBox_文字识别.Focused) || (keyData == Keys.Tab && txtBox_翻译文本.Focused) || (keyData == Keys.Tab && txtBox_记录界面.Focused) || (keyData == Keys.Tab && txtBox_识别界面.Focused) || (keyData == Keys.Tab && txtBox_输入翻译.Focused);
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
		}

		/// <summary>
		/// OCR验证按钮点击事件处理函数
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private async void 百度_btn_Click(object sender, EventArgs e)
		{
		         if (tabControl2.SelectedTab == inPage_百度接口)
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
		         else if (tabControl2.SelectedTab == inPage_百度高精度接口)
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
		         else if (tabControl2.SelectedTab == inPage白描接口)
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
			else if (tabControl2.SelectedTab == inPage腾讯接口 || tabControl2.SelectedTab == inPage腾讯高精度接口)
			{
			            string secretId, secretKey;
			            if (tabControl2.SelectedTab == inPage腾讯接口)
			            {
			                secretId = BoxTencentId.Text;
			                secretKey = BoxTencentKey.Text;
			            }
			            else
			            {
			                secretId = text_tencent_accurate_secretid.Text;
			                secretKey = text_tencent_accurate_secretkey.Text;
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
		/// 端口文本框输入拒绝事件处理函数
		/// 当端口文本框拒绝输入时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">掩码输入拒绝事件参数</param>
		private void text_端口_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
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
		/// 更新按钮点击事件处理函数
		/// 当用户点击更新设置按钮时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 更新Button_Click(object sender, EventArgs e)
		{
			numbox_间隔时间.Value = 24m;
			check_检查更新.Checked = true;
			checkBox_更新间隔.Checked = true;
		}

		/// <summary>
		/// 更新检查按钮点击事件处理函数
		/// 当用户点击检查更新按钮时触发此事件
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void 更新Button_check_Click(object sender, EventArgs e)
		{
			new Thread(Program.CheckUpdate).Start();
		}

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

		// 窗口关闭事件处理函数，保存所有设置到配置文件
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			// 保存基本配置
			IniHelper.SetValue("配置", "开机自启", cbBox_开机.Checked.ToString());
			IniHelper.SetValue("配置", "快速翻译", cbBox_翻译.Checked.ToString());
			IniHelper.SetValue("配置", "识别弹窗", cbBox_弹窗.Checked.ToString());
			IniHelper.SetValue("配置", "InputTranslateClipboard", cbBox_输入翻译剪贴板.Checked.ToString());
			IniHelper.SetValue("配置", "窗体动画", cobBox_动画.Text);
			IniHelper.SetValue("配置", "记录数目", numbox_记录.Text);
			IniHelper.SetValue("配置", "自动保存", cbBox_保存.Checked.ToString());
			IniHelper.SetValue("配置", "截图位置", textBox_path.Text);
			
			// 保存快捷键设置
			IniHelper.SetValue("快捷键", "文字识别", txtBox_文字识别.Text);
			IniHelper.SetValue("快捷键", "翻译文本", txtBox_翻译文本.Text);
			IniHelper.SetValue("快捷键", "记录界面", txtBox_记录界面.Text);
			IniHelper.SetValue("快捷键", "识别界面", txtBox_识别界面.Text);
			IniHelper.SetValue("快捷键", "输入翻译", txtBox_输入翻译.Text);
			
			// 保存百度OCR密钥和语言设置
			IniHelper.SetValue("密钥_百度", "secret_id", text_baiduaccount.Text);
			IniHelper.SetValue("密钥_百度", "secret_key", text_baidupassword.Text);
			var selectedLang = comboBox_Baidu_Language.SelectedItem?.ToString();
			var langCode = BaiduOcrHelper.GetStandardLanguages().FirstOrDefault(x => x.Value == selectedLang).Key;
			IniHelper.SetValue("密钥_百度", "language_code", langCode ?? "auto_detect");

			// 保存百度高精度OCR密钥和语言设置
			IniHelper.SetValue("密钥_百度高精度", "secret_id", text_baidu_accurate_apikey.Text);
			IniHelper.SetValue("密钥_百度高精度", "secret_key", text_baidu_accurate_secretkey.Text);
			var selectedAccurateLang = comboBox_Baidu_Accurate_Language.SelectedItem?.ToString();
			var accurateLangCode = BaiduOcrHelper.GetAccurateLanguages().FirstOrDefault(x => x.Value == selectedAccurateLang).Key;
			IniHelper.SetValue("密钥_百度高精度", "language_code", accurateLangCode ?? "auto_detect");

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
	
			DialogResult = DialogResult.OK;
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
		/// 快速保存图片复选框状态改变事件处理函数
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

		    // This is the total height required for the content within the main TabControl area.
		    int requiredContentHeight = maxBottom + 40; // Add 40px padding.

		    // The total form height is the position of the main TabControl plus the content height.
		    int requiredFormHeight = tab_标签.Top + requiredContentHeight;
		          
		    const int minFormHeight = 435;
		    int newHeight = Math.Max(requiredFormHeight, minFormHeight);

		    // Apply DPI scaling and update the form's client size.
		    int scaledHeight = (int)(newHeight / StaticValue.DpiFactor);
		    if (this.ClientSize.Height != scaledHeight)
		    {
		         this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, scaledHeight);
		    }
		}
	}
}
