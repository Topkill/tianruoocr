using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
	public partial class FmMain
	{
		/// <summary>
		/// 读取配置文件(config.ini)，按照配置文件初始化应用程序配置到内存，包括OCR接口、翻译接口、快捷键和各种API密钥
		/// 读取配置文件，并根据其内容将应用程序的各项配置初始化到内存中。这包括设置OCR与翻译接口、注册全局快捷键以及加载各种API密钥。
		/// </summary>
		private void InitConfig()
		{
			// 初始化API菜单
			InitializeApiMenus();
			// 【新增】加载 AI 动态菜单
			LoadCustomOpenAIMenus();
			LoadCustomOpenAITransMenus();

			// 初始化OCR接口配置
			interface_flag = GetConfigValueSafely("配置", "接口", "搜狗");
			if (string.IsNullOrEmpty(interface_flag))
			{
				interface_flag = "搜狗";
				IniHelper.SetValue("配置", "接口", interface_flag);
			}
			OCR_foreach(interface_flag);

			// 初始化翻译接口配置
			StaticValue.Translate_Current_API = GetConfigValueSafely("配置", "翻译接口", "Bing2");
			if (string.IsNullOrEmpty(StaticValue.Translate_Current_API))
			{
				StaticValue.Translate_Current_API = "Bing2";
			}
			Trans_foreach(StaticValue.Translate_Current_API);
			LoadTranslateConfig();

			// 初始化快捷键配置
			var filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";

			// 安全加载热键配置的辅助方法
			Action<string, string, int> loadHotkey = (section, key, flagId) =>
			{
				try
				{
					var hotkeyValue = IniHelper.GetValue(section, key);
					if (!string.IsNullOrEmpty(hotkeyValue) &&
					    hotkeyValue != "请按下快捷键" &&
					    hotkeyValue != "发生错误")
					{
						SetHotkey("None", "", hotkeyValue, flagId);
					}
				}
				catch (Exception ex)
				{
					// 记录错误但不中断程序执行
					System.Diagnostics.Debug.WriteLine($"加载热键配置失败 [{section}][{key}]: {ex.Message}");
				}
			};

			// 加载各个热键配置
			loadHotkey("快捷键", "文字识别", 200);
			loadHotkey("快捷键", "翻译文本", 205);
			loadHotkey("快捷键", "记录界面", 206);
			loadHotkey("快捷键", "识别界面", 235);
			loadHotkey("快捷键", "输入翻译", 240);
			loadHotkey("快捷键", "静默识别", 250);
			loadHotkey("快捷键", "截图翻译", 260);

			// --- 加载OCR密钥 ---
			// 加载百度OCR密钥
			StaticValue.BD_API_ID = TrOCRUtils.LoadSetting("密钥_百度", "secret_id", "");
			StaticValue.BD_API_KEY = TrOCRUtils.LoadSetting("密钥_百度", "secret_key","");
			StaticValue.BD_LANGUAGE = TrOCRUtils.LoadSetting("密钥_百度", "language_code","CHN_ENG");

			// 加载百度表格识别密钥
			StaticValue.BD_TABLE_API_ID = TrOCRUtils.LoadSetting("密钥_百度表格", "secret_id","");
			StaticValue.BD_TABLE_API_KEY = TrOCRUtils.LoadSetting("密钥_百度表格", "secret_key","");

			// 【新增】加载百度手写识别密钥
			StaticValue.BD_HANDWRITING_API_ID = TrOCRUtils.LoadSetting("密钥_百度手写", "secret_id", "");
			StaticValue.BD_HANDWRITING_API_KEY = TrOCRUtils.LoadSetting("密钥_百度手写", "secret_key", "");
			StaticValue.BD_HANDWRITING_LANGUAGE = TrOCRUtils.LoadSetting("密钥_百度手写", "language_code", "CHN_ENG");

			// 加载腾讯OCR密钥
			StaticValue.TX_API_ID = TrOCRUtils.LoadSetting("密钥_腾讯", "secret_id","");
			StaticValue.TX_API_KEY = TrOCRUtils.LoadSetting("密钥_腾讯", "secret_key","");
			StaticValue.TX_LANGUAGE = TrOCRUtils.LoadSetting("密钥_腾讯", "language_code","zh");

			// 加载腾讯高精度OCR密钥
			StaticValue.TX_ACCURATE_API_ID = TrOCRUtils.LoadSetting("密钥_腾讯高精度", "secret_id","");
			StaticValue.TX_ACCURATE_API_KEY = TrOCRUtils.LoadSetting("密钥_腾讯高精度", "secret_key","");
			StaticValue.TX_ACCURATE_LANGUAGE = TrOCRUtils.LoadSetting("密钥_腾讯高精度", "language","auto");

			// 加载腾讯表格v3的OCR密钥
			StaticValue.TX_TABLE_API_ID = TrOCRUtils.LoadSetting("密钥_腾讯表格v3", "secret_id","");
			StaticValue.TX_TABLE_API_KEY = TrOCRUtils.LoadSetting("密钥_腾讯表格v3", "secret_key","");

			// 加载百度高精度OCR密钥
			StaticValue.BD_ACCURATE_API_ID = TrOCRUtils.LoadSetting("密钥_百度高精度", "secret_id","");
			StaticValue.BD_ACCURATE_API_KEY = TrOCRUtils.LoadSetting("密钥_百度高精度", "secret_key","");
			StaticValue.BD_ACCURATE_LANGUAGE = TrOCRUtils.LoadSetting("密钥_百度高精度", "language_code","CHN_ENG");

			// --- 加载白描OCR凭据 ---
			StaticValue.BaimiaoUsername = TrOCRUtils.LoadSetting("密钥_白描", "username","");
			StaticValue.BaimiaoPassword = TrOCRUtils.LoadSetting("密钥_白描", "password","");

			// 加载持久化的token信息
			string savedToken = IniHelper.GetValue("密钥_白描", "token");
			string savedExpiry = IniHelper.GetValue("密钥_白描", "token_expiry");
			string savedUsername = IniHelper.GetValue("密钥_白描", "token_username");
			string savedUuid = IniHelper.GetValue("密钥_白描", "device_uuid");

			if (!string.IsNullOrEmpty(savedToken) && savedToken != "发生错误" &&
			    !string.IsNullOrEmpty(savedUsername) && savedUsername != "发生错误" &&
			    savedUsername == StaticValue.BaimiaoUsername && // 确保token属于当前用户
			    DateTime.TryParse(savedExpiry, out DateTime expiry) && DateTime.Now < expiry)
			{
			 StaticValue.BaimiaoToken = savedToken;
			 StaticValue.BaimiaoTokenExpiry = expiry;
			}

			if (!string.IsNullOrEmpty(savedUuid) && savedUuid != "发生错误")
			{
			 StaticValue.BaimiaoDeviceUuid = savedUuid;
			}
		}

		/// <summary>
		/// 安全获取配置值的辅助方法
		/// </summary>
		/// <param name="section">配置节</param>
		/// <param name="key">配置键</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns>配置值或默认值</returns>
		private string GetConfigValueSafely(string section, string key, string defaultValue = "")
		{
			try
			{
				var value = IniHelper.GetValue(section, key);
				return (string.IsNullOrEmpty(value) || value == "发生错误") ? defaultValue : value;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"读取配置失败 [{section}][{key}]: {ex.Message}");
				return defaultValue;
			}
		}

		/// <summary>
		/// 保存当前选择的OCR接口配置到配置文件中
		/// </summary>
		public void saveIniFile()
		{
			IniHelper.SetValue("配置", "接口", interface_flag);
		}

		/// <summary>
		/// 检查并替换文本中的中文冒号为英文冒号
		/// 当中文冒号前后都是英文字符或标点符号时，将其替换为英文冒号
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>处理后的文本，其中符合条件的中文冒号已被替换为英文冒号</returns>
		public static string check_ch_en(string text)
		{
			var array = text.ToCharArray();
			for (var i = 0; i < array.Length; i++)
			{
				var num = "：".IndexOf(array[i]);
				if (num != -1 && i - 1 >= 0 && i + 1 < array.Length && contain_en(array[i - 1].ToString()) && contain_en(array[i + 1].ToString()))
				{
					array[i] = ":"[num];
				}
				if (num != -1 && i - 1 >= 0 && i + 1 < array.Length && contain_en(array[i - 1].ToString()) && contain_punctuation(array[i + 1].ToString()))
				{
					array[i] = ":"[num];
				}
			}
			return new string(array);
		}

		//控制右键菜单-接口项的可见性
		private void InitializeApiMenus()
		{
			// OCR 接口可见性设置
			SetMenuItemVisibility(sougou, "Ocr接口显示", "Sougou");
			SetMenuItemVisibility(tencent, "Ocr接口显示", "Tencent");
			SetMenuItemVisibility(tencent_accurate, "Ocr接口显示", "TencentAccurate");
			SetMenuItemVisibility(youdao, "Ocr接口显示", "Youdao");
			SetMenuItemVisibility(wechat, "Ocr接口显示", "WeChat");
			SetMenuItemVisibility(baimiao, "Ocr接口显示", "Baimiao");
			SetMenuItemVisibility(baidu, "Ocr接口显示", "Baidu");
			SetMenuItemVisibility(baidu_accurate, "Ocr接口显示", "BaiduAccurate");
			SetMenuItemVisibility(Mathfuntion, "Ocr接口显示", "Mathfuntion");
			SetMenuItemVisibility(ocr_table, "Ocr接口显示", "Table");
			SetMenuItemVisibility(shupai, "Ocr接口显示", "Shupai");
			SetMenuItemVisibility(paddleocr, "Ocr接口显示", "PaddleOCR");
			SetMenuItemVisibility(paddleocr2, "Ocr接口显示", "PaddleOCR2");
			SetMenuItemVisibility(rapidocr, "Ocr接口显示", "RapidOCR");

			// OCR 子菜单接口可见性设置
			SetMenuItemVisibility(baidu_table, "Ocr接口显示", "TableBaidu");
			SetMenuItemVisibility(tx_table, "Ocr接口显示", "TencentTable");
			SetMenuItemVisibility(ali_table, "Ocr接口显示", "TableAli");
			SetMenuItemVisibility(left_right, "Ocr接口显示", "ShupaiLR");
			SetMenuItemVisibility(righ_left, "Ocr接口显示", "ShupaiRL");

			// 翻译接口可见性设置
			SetMenuItemVisibility(trans_google, "翻译接口显示", "Google");
			SetMenuItemVisibility(trans_baidu, "翻译接口显示", "Baidu");
			SetMenuItemVisibility(trans_tencent, "翻译接口显示", "Tencent");
			SetMenuItemVisibility(trans_bing, "翻译接口显示", "Bing");
			SetMenuItemVisibility(trans_bing2, "翻译接口显示", "Bing2");
			SetMenuItemVisibility(trans_microsoft, "翻译接口显示", "Microsoft");
			SetMenuItemVisibility(trans_yandex, "翻译接口显示", "Yandex");
			SetMenuItemVisibility(trans_tencentinteractive, "翻译接口显示", "TencentInteractive");
			SetMenuItemVisibility(trans_caiyun, "翻译接口显示", "Caiyun");
			SetMenuItemVisibility(trans_volcano, "翻译接口显示", "Volcano");
			SetMenuItemVisibility(trans_caiyun2, "翻译接口显示", "Caiyun2");
			SetMenuItemVisibility(trans_baidu2, "翻译接口显示", "Baidu2");
		}

		/// <summary>
		/// 根据配置文件中指定节和键的值设置菜单项的可见性
		/// </summary>
		/// <param name="menuItem">要设置可见性的菜单项</param>
		/// <param name="section">配置文件中的节名称</param>
		/// <param name="key">节中的键名称</param>
		private void SetMenuItemVisibility(ToolStripItem menuItem, string section, string key)
		{
			if (menuItem != null)
			{
				string visibilityValue = IniHelper.GetValue(section, key);
				bool isVisible;

				if (bool.TryParse(visibilityValue, out isVisible))
				{
					// Value was "True" or "False", isVisible is now set correctly.
				}
				else // Value was "发生错误" or something else. Apply default logic.
				{
					if (section == "翻译接口显示")
					{
						switch (key)
						{
							case "TencentInteractive":
							case "Caiyun":
							case "Volcano":
							case "Baidu2":
								isVisible = false;
								break;
							default:
								isVisible = true;
								break;
						}
					}
					else if (section == "Ocr接口显示")
					{
						switch (key)
						{
							case "Baimiao":
								isVisible = false;
								break;
							default:
								isVisible = true;
								break;
						}
					}
					else
					{
						isVisible = true; // Default for any other section
					}
				}

				menuItem.Visible = isVisible;
			}
		}

		// 这是一个专门用来"拦截并报错"的事件
		private void ShowConfigWarning_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			// 判断是哪个菜单触发的，显示对应的提示
			string msg = "检测到您尚未配置 AI 接口。\n\n请先去设置里配置";
			if (sender == ai_menu_trans) msg = "检测到您尚未配置 AI 翻译接口。\n\n请先去设置里配置";

			MessageBox.Show(msg, "配置提示", MessageBoxButtons.OK, MessageBoxIcon.None);

		}

		/// <summary>
		/// 点击加载按钮时触发的事件处理函数，将窗体最小化并隐藏
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		private void Load_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
			Visible = false;
		}

		/// <summary>
		/// 刷新ocr接口界面控件文本显示
		/// </summary>
		private new void Refresh()
		{
			sougou.Text = "搜狗";
			tencent.Text = "腾讯";
			tencent_accurate.Text = "腾讯-高精度";
			baidu.Text = "百度";
			youdao.Text = "有道";
			wechat.Text = "微信";
			baimiao.Text = "白描";
			baidu_accurate.Text = "百度-高精度";
			shupai.Text = "竖排";
			ocr_table.Text = "表格";
			ch_en.Text = "中英";
			jap.Text = "日语";
			kor.Text = "韩语";
			left_right.Text = "从左向右";
			righ_left.Text = "从右向左";
			baidu_table.Text = "百度";
			tx_table.Text = "腾讯";
			ali_table.Text = "阿里";
			Mathfuntion.Text = "公式";
			paddleocr.Text = "PaddleOCR";
			paddleocr2.Text = "PaddleOCR2";
			rapidocr.Text = "RapidOCR";
			write.Text = "手写";
			baidu_handwriting.Text = "百度手写";
			ai_menu.Text = "AI";

		}
	}
}
