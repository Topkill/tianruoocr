using System.Windows.Forms;

namespace TrOCR
{
	// 主窗口类，负责OCR识别和翻译功能的主界面
	public sealed partial class FmMain : global::System.Windows.Forms.Form
	{
		// 释放资源
		protected override void Dispose(bool disposing)
		{
			global::TrOCR.Helper.HelpWin32.ChangeClipboardChain(base.Handle, this.nextClipboardViewer);
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// 初始化界面组件
		private void InitializeComponent()
		{
			// 组件容器
			this.components = new global::System.ComponentModel.Container();
			// 
			// btnToggleOriginalText
			// 
			this.btnToggleOriginalText = new System.Windows.Forms.Button();
			this.btnToggleOriginalText.Location = new System.Drawing.Point(250, 5); // 这是一个临时的初始位置，后续代码会控制它
			this.btnToggleOriginalText.Name = "btnToggleOriginalText";
			this.btnToggleOriginalText.Size = new System.Drawing.Size(24, 24);
			this.btnToggleOriginalText.TabIndex = 201; // 使用一个较高的TabIndex避免冲突
			this.btnToggleOriginalText.Text = "◀";
			this.btnToggleOriginalText.UseVisualStyleBackColor = true;
			this.btnToggleOriginalText.Visible = false; // 初始状态为不可见
			this.btnToggleOriginalText.Click += new System.EventHandler(this.btnToggleOriginalText_Click);
			this.btnToggleOriginalText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnToggleOriginalText_MouseUp);
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TrOCR.FmMain));
			
			//导出excel菜单项
			this.ExportExcel = new global::System.Windows.Forms.ToolStripMenuItem();
			//
			//ExportExcel
			//

			this.ExportExcel.Name = "ExportExcel";
			this.ExportExcel.Size = new System.Drawing.Size(180, 22);
			this.ExportExcel.Text = "导出表格";
			this.ExportExcel.Click += new System.EventHandler(this.ExportExcel_Click);
			// 托盘图标控件
			this.minico = new global::System.Windows.Forms.NotifyIcon(this.components);
			// ====================【在此处添加代码块】====================
            // 
            // panelSeparator
            // 
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.panelSeparator.SuspendLayout(); // 这是一个好习惯，虽然对于Panel来说不是必须的
            this.panelSeparator.Location = new System.Drawing.Point(350, 0); // 任意临时位置
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(1, 400); // 任意临时大小
            this.panelSeparator.TabIndex = 202; // 确保TabIndex不与其他控件冲突
            this.panelSeparator.Visible = false; // 默认不可见
            this.panelSeparator.BackColor = System.Drawing.Color.Gainsboro;
            this.panelSeparator.ResumeLayout(false);
            // ==========================================================
			
			// 分隔符
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			
			// 工具栏菜单项
			this.toolStrip = new global::System.Windows.Forms.ToolStripMenuItem();
			
			// 翻译接口菜单项
			this.trans_input = new global::System.Windows.Forms.ToolStripMenuItem();
			this.trans_google = new global::System.Windows.Forms.ToolStripMenuItem();       // 谷歌翻译
			this.trans_baidu = new global::System.Windows.Forms.ToolStripMenuItem();        // 百度翻译
			this.trans_tencent = new global::System.Windows.Forms.ToolStripMenuItem();      // 腾讯翻译
			this.trans_bing = new global::System.Windows.Forms.ToolStripMenuItem();         // Bing翻译
			this.trans_bing2 = new global::System.Windows.Forms.ToolStripMenuItem();        // Bing翻译2
			this.trans_microsoft = new global::System.Windows.Forms.ToolStripMenuItem();    // Microsoft翻译
			this.trans_yandex = new global::System.Windows.Forms.ToolStripMenuItem();       // Yandex翻译
			this.trans_tencentinteractive = new global::System.Windows.Forms.ToolStripMenuItem(); // 腾讯交互翻译
			this.trans_caiyun = new global::System.Windows.Forms.ToolStripMenuItem();       // 彩云小译
			this.trans_volcano = new global::System.Windows.Forms.ToolStripMenuItem();      // 火山翻译
			this.trans_caiyun2 = new global::System.Windows.Forms.ToolStripMenuItem();      // 彩云小译2
			this.trans_baidu2 = new global::System.Windows.Forms.ToolStripMenuItem();      // 百度翻译2

			
			
			
			// 主右键菜单
			this.menu = new global::System.Windows.Forms.ContextMenuStrip();
			this.menu.Renderer = new global::TrOCR.Helper.HelpRepaint.MenuItemRendererT();
			
			// 百度OCR语言选项
			this.ch_en = new global::System.Windows.Forms.ToolStripMenuItem();              // 中英文混合
			this.jap = new global::System.Windows.Forms.ToolStripMenuItem();                // 日语
			this.kor = new global::System.Windows.Forms.ToolStripMenuItem();                // 韩语
			
			// 拼音转换菜单项
			this.pinyin = new global::System.Windows.Forms.ToolStripMenuItem();
			
			// 代理设置菜单项
			this.customize_Proxy = new global::System.Windows.Forms.ToolStripMenuItem();    // 自定义代理
			this.null_Proxy = new global::System.Windows.Forms.ToolStripMenuItem();         // 不使用代理
			this.system_Proxy = new global::System.Windows.Forms.ToolStripMenuItem();       // 系统代理
			this.Proxy = new global::System.Windows.Forms.ToolStripMenuItem();              // 代理主菜单项
			
			// 竖排OCR菜单项
			this.left_right = new global::System.Windows.Forms.ToolStripMenuItem();         // 从左向右
			this.righ_left = new global::System.Windows.Forms.ToolStripMenuItem();          // 从右向左
			
			// 主菜单项
			this.Main_copy = new global::System.Windows.Forms.ToolStripMenuItem();          // 复制
			this.Main_paste = new global::System.Windows.Forms.ToolStripMenuItem();         // 粘贴
			this.Main_selectall = new global::System.Windows.Forms.ToolStripMenuItem();     // 全选
			this.Main_jiekou = new global::System.Windows.Forms.ToolStripMenuItem();        // 接口
			this.Main_exit = new global::System.Windows.Forms.ToolStripMenuItem();          // 退出
			this.Main_change = new global::System.Windows.Forms.ToolStripMenuItem();        // 转换
			this.zh_tra = new global::System.Windows.Forms.ToolStripMenuItem();             // 简体转繁体
			this.tra_zh = new global::System.Windows.Forms.ToolStripMenuItem();             // 繁体转简体
			this.str_Upper = new global::System.Windows.Forms.ToolStripMenuItem();          // 英文大写
			this.Upper_str = new global::System.Windows.Forms.ToolStripMenuItem();          // 英文小写
			
			// 语音菜单项
			this.speak = new global::System.Windows.Forms.ToolStripMenuItem();              // 朗读
			
			// 翻译界面菜单项
			this.Trans_copy = new global::System.Windows.Forms.ToolStripMenuItem();         // 复制
			this.Trans_paste = new global::System.Windows.Forms.ToolStripMenuItem();        // 粘贴
			this.Trans_SelectAll = new global::System.Windows.Forms.ToolStripMenuItem();    // 全选
			this.Trans_close = new global::System.Windows.Forms.ToolStripMenuItem();        // 关闭
			this.Trans_Voice = new global::System.Windows.Forms.ToolStripMenuItem();        // 朗读
			
			// OCR接口菜单项
			this.sougou = new global::System.Windows.Forms.ToolStripMenuItem();             // 搜狗OCR
			this.Mathfuntion = new global::System.Windows.Forms.ToolStripMenuItem();        // 数学公式OCR
			this.tencent = new global::System.Windows.Forms.ToolStripMenuItem();            // 腾讯OCR
			this.baidu = new global::System.Windows.Forms.ToolStripMenuItem();              // 百度OCR
			this.shupai = new global::System.Windows.Forms.ToolStripMenuItem();             // 竖排OCR
			this.write = new global::System.Windows.Forms.ToolStripMenuItem();              // 手写OCR
			this.tencent_v = new global::System.Windows.Forms.ToolStripMenuItem();          // 腾讯VIP OCR
			this.baidu_s = new global::System.Windows.Forms.ToolStripMenuItem();            // 百度搜索
			this.baidu_v = new global::System.Windows.Forms.ToolStripMenuItem();            // 百度VIP OCR
			this.youdao = new global::System.Windows.Forms.ToolStripMenuItem();             // 有道OCR
			this.wechat = new global::System.Windows.Forms.ToolStripMenuItem();             // 微信OCR
			// 表格OCR菜单项
			this.baidu_table = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tx_table = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ali_table = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ocr_table = new global::System.Windows.Forms.ToolStripMenuItem();
			// AI_OCR菜单项
			this.ai_menu = new global::System.Windows.Forms.ToolStripMenuItem();
			// this.ai_openai_compatible = new global::System.Windows.Forms.ToolStripMenuItem();
			// AI_Trans菜单项
			this.ai_menu_trans = new global::System.Windows.Forms.ToolStripMenuItem();
			// this.ai_openai_compatible_trans = new global::System.Windows.Forms.ToolStripMenuItem();
			
			// 文本转换菜单项
			this.Chinese = new global::System.Windows.Forms.ToolStripMenuItem();            // 中文
			this.English = new global::System.Windows.Forms.ToolStripMenuItem();            // 英文
			this.Split = new global::System.Windows.Forms.ToolStripMenuItem();              // 分割
			this.Restore = new global::System.Windows.Forms.ToolStripMenuItem();            // 恢复
			
			// 翻译界面右键菜单
			this.menu_copy = new global::System.Windows.Forms.ContextMenuStrip();
			this.menu_copy.Renderer = new global::TrOCR.Helper.HelpRepaint.MenuItemRendererT();

			

			
			// 图片框控件
			this.PictureBox1 = new global::System.Windows.Forms.PictureBox();
			
			// 富文本框控件
			this.RichBoxBody = new global::TrOCR.AdvRichTextBox();                         // 主文本框
			this.RichBoxBody_T = new global::TrOCR.AdvRichTextBox();                       // 翻译文本框
			
			// 托盘图标设置
			this.minico.BalloonTipIcon = global::System.Windows.Forms.ToolTipIcon.Info;
			this.minico.BalloonTipText = "最小化到任务栏";
			this.minico.BalloonTipTitle = "提示";
			this.minico.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("minico.Icon");
			this.minico.Text = "双击开始截图识别";
			this.minico.Visible = true;
			// this.minico.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.tray_double_Click);
			// this.minico.Click += new System.EventHandler(this.tray_SingleClick_StartTimer);
			// 【修改】移除 Click 和 DoubleClick 事件，改用 MouseDown
			this.minico.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.tray_MouseDown);
			
			// 【新增】初始化单击/双击区分定时器
            this.trayClickTimer = new System.Windows.Forms.Timer(this.components);
            this.trayClickTimer.Interval = SystemInformation.DoubleClickTime; // 使用系统定义的双击间隔
            this.trayClickTimer.Tick += new System.EventHandler(this.trayClickTimer_Tick);
            //

            // 字体基础尺寸
            this.font_base.Width = 18f * this.F_factor;
			this.font_base.Height = 17f * this.F_factor;
			
			// 翻译文本框初始状态
			this.RichBoxBody_T.Visible = false;
			
			// 主文本框设置
			this.RichBoxBody.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.RichBoxBody.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.RichBoxBody.Location = new global::System.Drawing.Point(0, 0);
			this.RichBoxBody.Name = "htmlTextBoxBody";
			this.RichBoxBody.ImeMode = global::System.Windows.Forms.ImeMode.Inherit;
			this.RichBoxBody.TabIndex = 200;
			this.RichBoxBody.Text_flag = "天若幽心";
			
			// 翻译文本框设置
			this.RichBoxBody_T.ImeMode = global::System.Windows.Forms.ImeMode.Inherit;
			
			// 翻译界面菜单项设置
			this.Trans_copy.Text = "复制";
			this.Trans_copy.Click += new global::System.EventHandler(this.Trans_copy_Click);
			this.Trans_paste.Text = "粘贴";
			this.Trans_paste.Click += new global::System.EventHandler(this.Trans_paste_Click);
			this.Trans_SelectAll.Text = "全选";
			this.Trans_SelectAll.Click += new global::System.EventHandler(this.Trans_SelectAll_Click);
			this.Trans_close.Text = "关闭";
			this.Trans_close.Click += new global::System.EventHandler(this.Trans_close_Click);
			this.Trans_Voice.Text = "朗读";
			this.Trans_Voice.Click += new global::System.EventHandler(this.Trans_Voice_Click);
			
			// 翻译接口菜单项设置
			this.trans_input.Text = "接口";
			this.trans_input.Click += new global::System.EventHandler(this.Trans_SelectAll_Click);
			this.trans_google.Text = "谷歌√";
			this.trans_google.Click += new global::System.EventHandler(this.Trans_google_Click);
			this.trans_baidu.Text = "百度";
			this.trans_baidu.Click += new global::System.EventHandler(this.Trans_baidu_Click);
			this.trans_tencent.Text = "腾讯";
			this.trans_tencent.Click += new global::System.EventHandler(this.Trans_tencent_Click);
			this.trans_bing.Text = "Bing";
			this.trans_bing.Click += new global::System.EventHandler(this.Trans_bing_Click);
			this.trans_bing2.Text = "Bing2";
			this.trans_bing2.Click += new global::System.EventHandler(this.Trans_bing2_Click);
			this.trans_microsoft.Text = "Microsoft";
			this.trans_microsoft.Click += new global::System.EventHandler(this.Trans_microsoft_Click);
			this.trans_yandex.Text = "Yandex";
			this.trans_yandex.Click += new global::System.EventHandler(this.Trans_yandex_Click);
			this.trans_tencentinteractive.Text = "腾讯交互";
			this.trans_tencentinteractive.Click += new global::System.EventHandler(this.Trans_tencentinteractive_Click);
			this.trans_caiyun.Text = "彩云";
			this.trans_caiyun.Click += new global::System.EventHandler(this.Trans_caiyun_Click);
			this.trans_volcano.Text = "火山";
			this.trans_volcano.Click += new global::System.EventHandler(this.Trans_volcano_Click);
			this.trans_caiyun2.Text = "彩云2";
			this.trans_caiyun2.Click += new global::System.EventHandler(this.Trans_caiyun2_Click);
			this.trans_baidu2.Text = "彩云2";
			this.trans_baidu2.Click += new global::System.EventHandler(this.Trans_baidu2_Click);
			
			// 翻译界面右键菜单设置
			this.menu_copy.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.Trans_copy,
				this.Trans_paste,
				this.Trans_SelectAll,
				this.Trans_Voice,
				this.trans_input,
				this.Trans_close
			});
			
			// 翻译接口菜单项设置
			this.trans_input.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.ai_menu_trans,
				this.trans_google,
				this.trans_baidu,
				this.trans_tencent,
				this.trans_bing,
				this.trans_bing2,
				this.trans_microsoft,
				this.trans_yandex,
				this.trans_tencentinteractive,
				this.trans_caiyun,
				this.trans_volcano,
				this.trans_caiyun2,
				// this.trans_baidu2,
			});
			
			// 翻译界面右键菜单字体设置
			this.menu_copy.Font = new global::System.Drawing.Font("微软雅黑", 9f / global::TrOCR.Helper.StaticValue.DpiFactor, global::System.Drawing.FontStyle.Regular);
			
			// 主菜单项设置
			this.Main_copy.Text = "复制";
			this.Main_copy.Click += new global::System.EventHandler(this.MainCopyClick);
			this.Main_paste.Text = "粘贴";
			this.Main_paste.Click += new global::System.EventHandler(this.Main_paste_Click);
			this.Main_selectall.Text = "全选";
			this.Main_selectall.Click += new global::System.EventHandler(this.Main_SelectAll_Click);
			this.speak.Text = "朗读";
			this.speak.Click += new global::System.EventHandler(this.Main_Voice_Click);
			this.baidu_s.Text = "搜索";
			this.baidu_s.Click += new global::System.EventHandler(this.SearchSelText);
			this.Main_change.Text = "转换";
			this.Main_jiekou.Text = "接口";
			this.Main_exit.Text = "退出";
			this.Main_exit.Click += new global::System.EventHandler(this.trayExitClick);
			
			// 主右键菜单设置
			this.menu.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.Main_copy,
				this.Main_paste,
				this.Main_selectall,
				this.speak,
				this.baidu_s,
				this.ExportExcel, // 在这里添加新的菜单项
				this.Main_change,
				this.Main_jiekou,
				this.Main_exit
			});
			
			// 主右键菜单字体设置
			this.menu.Font = new global::System.Drawing.Font("微软雅黑", 9f / global::TrOCR.Helper.StaticValue.DpiFactor, global::System.Drawing.FontStyle.Regular);
			
			// OCR接口菜单项设置
			this.sougou.Text = "搜狗√";
			this.sougou.Click += new global::System.EventHandler(this.OCR_sougou_Click);
			this.Mathfuntion.Text = "公式";
			this.Mathfuntion.Click += new global::System.EventHandler(this.OCR_Mathfuntion_Click);
			this.tencent.Text = "腾讯";
			this.tencent.Click += new global::System.EventHandler(this.OCR_tencent_Click);
			this.tencent_accurate = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tencent_accurate.Text = "腾讯-高精度";
			this.tencent_accurate.Click += new global::System.EventHandler(this.OCR_tencent_accurate_Click);
			this.baidu.Text = "百度";
			this.baidu.Click += new global::System.EventHandler(this.OCR_baidu_Click);
			this.youdao.Text = "有道";
			this.youdao.Click += new global::System.EventHandler(this.OCR_youdao_Click);
			this.wechat.Text = "微信";
			this.wechat.Click += new global::System.EventHandler(this.OCR_wechat_Click);
			this.baimiao = new global::System.Windows.Forms.ToolStripMenuItem();
			this.baimiao.Text = "白描";
			this.baimiao.Click += new global::System.EventHandler(this.OCR_baimiao_Click);
			this.baidu_accurate = new global::System.Windows.Forms.ToolStripMenuItem();
			this.baidu_accurate.Text = "百度-高精度";
			this.baidu_accurate.Click += new global::System.EventHandler(this.OCR_baidu_accurate_Click);
			// 【新增】初始化百度手写识别菜单项
			this.baidu_handwriting = new global::System.Windows.Forms.ToolStripMenuItem();
			this.baidu_handwriting.Name = "baidu_handwriting";//这行加不加都行
			this.baidu_handwriting.Text = "百度手写";
			this.baidu_handwriting.Click += new global::System.EventHandler(this.OCR_baidu_handwriting_Click);
			this.tencent_handwriting = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tencent_handwriting.Name = "tencent_handwriting";//这行加不加都行
			this.tencent_handwriting.Text = "腾讯手写";
			this.tencent_handwriting.Click += new global::System.EventHandler(this.OCR_tencent_handwriting_Click);
			this.paddleocr = new global::System.Windows.Forms.ToolStripMenuItem();
			this.paddleocr.Text = "PaddleOCR";
			this.paddleocr.Click += new global::System.EventHandler(this.OCR_paddleocr_Click);
			this.paddleocr2 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.paddleocr2.Text = "PaddleOCR2";
			this.paddleocr2.Click += new global::System.EventHandler(this.OCR_paddleocr2_Click);
			this.rapidocr = new global::System.Windows.Forms.ToolStripMenuItem();
			this.rapidocr.Text = "RapidOCR";
			this.rapidocr.Click += new global::System.EventHandler(this.OCR_rapidocr_Click);
			this.ocr_table.Text = "表格";
			this.baidu_table.Text = "百度";
			this.baidu_table.Click += new global::System.EventHandler(this.OCR_baidutable_Click);
			this.tx_table.Text = "腾讯";
			this.tx_table.Click += new global::System.EventHandler(this.OCR_txtable_Click);
			this.ali_table.Text = "阿里";
			this.ali_table.Click += new global::System.EventHandler(this.OCR_ailitable_Click);
			this.ocr_table.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.baidu_table,
				this.tx_table,
				this.ali_table
			});
			this.ai_menu.Name = "ai_menu";
			this.ai_menu.Text = "AI";
			// this.ai_openai_compatible.Name = "ai_openai_compatible";
			// this.ai_openai_compatible.Text = "OpenAICompatible";
			// //this.ai_openai_compatible.Click += new global::System.EventHandler(this.OCR_ai_openai_compatible_Click);
			// this.ai_menu.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			// {
			// 	this.ai_openai_compatible
			// });
			this.ai_menu_trans.Name = "ai_menu_trans";
			this.ai_menu_trans.Text = "AI";
			// this.ai_openai_compatible_trans.Name = "ai_openai_compatible_trans";
			// this.ai_openai_compatible_trans.Text = "OpenAICompatible";
			//this.ai_openai_compatible_trans.Click += new global::System.EventHandler(this.Trans_ai_openai_compatible_Click);
			// this.ai_menu_trans.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			// {
			// 	this.ai_openai_compatible_trans
			// });
			this.shupai.Text = "竖排";
			this.shupai.Click += new global::System.EventHandler(this.OCR_shupai_Click);
			this.write.Text = "手写";
			// this.write.Click += new global::System.EventHandler(this.OCR_write_Click);
			this.write.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.baidu_handwriting,
				this.tencent_handwriting
				
			});//手写菜单添加子菜单
			this.Chinese.Text = "中文标点";
			this.Chinese.Click += new global::System.EventHandler(this.change_Chinese_Click);
			this.English.Text = "英文标点";
			this.English.Click += new global::System.EventHandler(this.change_English_Click);
			this.zh_tra.Text = "中文繁体";
			this.zh_tra.Click += new global::System.EventHandler(this.change_zh_tra_Click);
			this.tra_zh.Text = "中文简体";
			this.tra_zh.Click += new global::System.EventHandler(this.change_tra_zh_Click);
			this.str_Upper.Text = "英文大写";
			this.str_Upper.Click += new global::System.EventHandler(this.change_str_Upper_Click);
			this.Upper_str.Text = "英文小写";
			this.Upper_str.Click += new global::System.EventHandler(this.change_Upper_str_Click);
			this.pinyin.Text = "汉语拼音";
			this.pinyin.Click += new global::System.EventHandler(this.change_pinyin_Click);
			// 【新增】初始化“盘古之白”菜单项
			this.pangu_spacing = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pangu_spacing.Name = "pangu_spacing";
			this.pangu_spacing.Text = "盘古之白";
			this.pangu_spacing.Click += new global::System.EventHandler(this.pangu_spacing_Click);

			this.change_button = this.Main_change;
			this.change_button.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.pangu_spacing,
				this.Chinese,
				this.English,
				this.zh_tra,
				this.tra_zh,
				this.str_Upper,
				this.Upper_str,
				this.pinyin,
				
			});
			this.interface_button = this.Main_jiekou;
			this.interface_button.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
                this.ai_menu,
                this.sougou,
				this.tencent,
				this.tencent_accurate,
				this.youdao,
				this.wechat,
				this.baimiao,
				this.baidu,
				this.baidu_accurate,
				this.paddleocr,
				this.paddleocr2,
				this.rapidocr,
				this.toolStripSeparator1,
				this.Mathfuntion,
				this.ocr_table,
				this.shupai,
				this.write, // 【新增】将“手写”父菜单添加到这里
				
				
			});
			if (global::TrOCR.Helper.IniHelper.GetValue("配置", "接口") == "百度")
			{
				this.ch_en.Text = "中英√";
			}
			else
			{
				this.ch_en.Text = "中英";
			}
			this.ch_en.Click += new global::System.EventHandler(this.OCR_baidu_Ch_and_En_Click);
			this.jap.Text = "日语";
			this.jap.Click += new global::System.EventHandler(this.OCR_baidu_Jap_Click);
			this.kor.Text = "韩语";
			this.kor.Click += new global::System.EventHandler(this.OCR_baidu_Kor_Click);
			((global::System.Windows.Forms.ToolStripDropDownItem)this.baidu).DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.ch_en,
				this.jap,
				this.kor
			});
			this.left_right.Text = "从左向右";
			this.left_right.Click += new global::System.EventHandler(this.OCR_lefttoright_Click);
			this.righ_left.Text = "从右向左";
			this.righ_left.Click += new global::System.EventHandler(this.OCR_righttoleft_Click);
			((global::System.Windows.Forms.ToolStripDropDownItem)this.shupai).DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.left_right,
				this.righ_left
			});
			this.RichBoxBody.ContextMenuStrip1 = this.menu;
			this.RichBoxBody_T.ContextMenuStrip1 = this.menu_copy;
			this.PictureBox1.Image = (global::System.Drawing.Image)new global::System.ComponentModel.ComponentResourceManager(typeof(global::TrOCR.FmMain)).GetObject("loadcat.gif");
			this.PictureBox1.Size = new global::System.Drawing.Size(85, 85);
			this.PictureBox1.Location = (global::System.Drawing.Point)new global::System.Drawing.Size((int)this.font_base.Width * 34 - this.PictureBox1.Size.Width / 2, (int)(110f * this.F_factor));
			this.PictureBox1.BackColor = global::System.Drawing.Color.White;
			this.PictureBox1.Visible = false;
			base.SuspendLayout();
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.Manual;
			base.Location = (global::System.Drawing.Point)new global::System.Drawing.Size(global::System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - global::System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 10, global::System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 - global::System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 6);
			base.Size = new global::System.Drawing.Size((int)this.font_base.Width * 23, (int)this.font_base.Height * 24);
			base.Controls.Add(this.RichBoxBody_T);
			  // 将 panelSeparator 添加到两个 RichTextBox 之间
            base.Controls.Add(this.panelSeparator); 
			base.Controls.Add(this.PictureBox1);
			base.Controls.Add(this.RichBoxBody);
			base.Controls.Add(this.btnToggleOriginalText);
			base.Load += new global::System.EventHandler(this.Load_Click);
			base.Resize += new global::System.EventHandler(this.Form_Resize);
			base.Name = "Form1";
			this.Text = "耗时：";
			if (global::TrOCR.Helper.IniHelper.GetValue("工具栏", "顶置") == "True")
			{
				base.TopMost = true;
			}
			else
			{
				base.TopMost = false;
			}
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("minico.Icon");
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private global::System.ComponentModel.IContainer components;
		// 【新增】添加一个定时器用于区分单击和双击
        private global::System.Windows.Forms.Timer trayClickTimer;

		/// <summary>
		/// 托盘图标
		/// </summary>
		public global::System.Windows.Forms.NotifyIcon minico;

		/// <summary>
		/// 主富文本框右键菜单
		/// </summary>
		public global::System.Windows.Forms.ContextMenuStrip menu;

		private global::System.Windows.Forms.ToolStripMenuItem toolStrip;

		#region 主菜单 (menu)
		/// <summary>
		/// 主菜单 - 复制
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Main_copy;

		/// <summary>
		/// 主菜单 - 粘贴
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Main_paste;

		/// <summary>
		/// 主菜单 - 全选
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Main_selectall;

		/// <summary>
		/// 主菜单 - 退出
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Main_exit;

		/// <summary>
		/// 主菜单 - "接口" 菜单
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Main_jiekou;

		/// <summary>
		/// "接口" 菜单的实例, 用于动态添加菜单项
		/// </summary>
		public global::System.Windows.Forms.ToolStripDropDownItem interface_button;

		/// <summary>
		/// 主菜单 - "转换" 菜单
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Main_change;

		/// <summary>
		/// "转换" 菜单的实例, 用于动态添加菜单项
		/// </summary>
		public global::System.Windows.Forms.ToolStripDropDownItem change_button;

		/// <summary>
		/// 主菜单 - 朗读
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem speak;

		/// <summary>
		/// 主菜单 - 搜索
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem baidu_s;
		#endregion

		#region "接口" 子菜单 (Main_jiekou)
		/// <summary>
		/// 接口 - 搜狗
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem sougou;

		/// <summary>
		/// 接口 - 腾讯
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem tencent;

		/// <summary>
		/// 接口 - 腾讯(高精度)
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem tencent_accurate;

		/// <summary>
		/// 接口 - 百度
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem baidu;

		/// <summary>
		/// 接口 - 有道
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem youdao;

		/// <summary>
		/// 接口 - 微信
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem wechat;

		/// <summary>
		/// 接口 - 白描
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem baimiao;

		/// <summary>
		/// 接口 - 百度(高精度)
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem baidu_accurate;

		/// <summary>
		/// 接口 - PaddleOCR
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem paddleocr;

		/// <summary>
		/// 接口 - PaddleOCR2
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem paddleocr2;

		/// <summary>
		/// 接口 - RapidOCR
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem rapidocr;

		/// <summary>
		/// 接口 - 竖排
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem shupai;

		/// <summary>
		/// 接口 - 手写
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem  write;

		/// <summary>
		/// 接口 - 公式
		/// </summary>
		public global::System.Windows.Forms.ToolStripItem Mathfuntion;

		/// <summary>
		/// 接口 - "表格" 子菜单
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem ocr_table;

		/// <summary>
		/// 接口 - 分隔符
		/// </summary>
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		#endregion

		#region "转换" 子菜单 (Main_change)
		/// <summary>
		/// 转换 - 中文标点
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Chinese;

		/// <summary>
		/// 转换 - 英文标点
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem English;

		/// <summary>
		/// 转换 - 中文繁体
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem zh_tra;

		/// <summary>
		/// 转换 - 中文简体
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem tra_zh;

		/// <summary>
		/// 转换 - 英文大写
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem str_Upper;

		/// <summary>
		/// 转换 - 英文小写
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem Upper_str;

		/// <summary>
		/// 转换 - 汉语拼音
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem pinyin;
		//转换 - 盘古之白
		private global::System.Windows.Forms.ToolStripMenuItem pangu_spacing; 

		//导出excel
		private global::System.Windows.Forms.ToolStripMenuItem ExportExcel; // 新增


		#endregion

		#region 翻译文本框菜单 (menu_copy)
		/// <summary>
		/// 翻译结果的富文本框
		/// </summary>
		public global::TrOCR.AdvRichTextBox RichBoxBody_T;

		/// <summary>
		/// 翻译文本框的右键菜单
		/// </summary>
		public global::System.Windows.Forms.ContextMenuStrip menu_copy;

		/// <summary>
		/// 翻译菜单 - 复制
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Trans_copy;

		/// <summary>
		/// 翻译菜单 - 粘贴
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Trans_paste;

		/// <summary>
		/// 翻译菜单 - 全选
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Trans_SelectAll;

		/// <summary>
		/// 翻译菜单 - 关闭
		/// </summary>
		public global::System.Windows.Forms.ToolStripMenuItem Trans_close;

		/// <summary>
		/// 翻译菜单 - 朗读
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem Trans_Voice;

		/// <summary>
		/// 翻译菜单 - "接口" 子菜单
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem trans_input;
		#endregion

		#region 翻译接口子菜单 (trans_input)
		private global::System.Windows.Forms.ToolStripMenuItem trans_google;
		private global::System.Windows.Forms.ToolStripMenuItem trans_baidu;
		private global::System.Windows.Forms.ToolStripMenuItem trans_tencent;
		private global::System.Windows.Forms.ToolStripMenuItem trans_bing;
		private global::System.Windows.Forms.ToolStripMenuItem trans_bing2;
		private global::System.Windows.Forms.ToolStripMenuItem trans_microsoft;
		private global::System.Windows.Forms.ToolStripMenuItem trans_yandex;
		private global::System.Windows.Forms.ToolStripMenuItem trans_tencentinteractive;
		private global::System.Windows.Forms.ToolStripMenuItem trans_caiyun;
		private global::System.Windows.Forms.ToolStripMenuItem trans_volcano;
		private global::System.Windows.Forms.ToolStripMenuItem trans_caiyun2;
		private global::System.Windows.Forms.ToolStripMenuItem trans_baidu2;

		#endregion

		#region 其他子菜单

		#region "百度" OCR 语言子菜单
		/// <summary>
		/// 百度OCR语言 - 中英
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem ch_en;

		/// <summary>
		/// 百度OCR语言 - 日语
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem jap;

		/// <summary>
		/// 百度OCR语言 - 韩语
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem kor;
		#endregion

		#region "竖排" 子菜单
		/// <summary>
		/// 竖排 - 从左向右
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem left_right;

		/// <summary>
		/// 竖排 - 从右向左
		/// </summary>
		private global::System.Windows.Forms.ToolStripMenuItem righ_left;
		#endregion

		//百度手写
		public global::System.Windows.Forms.ToolStripItem baidu_handwriting; 
		public global::System.Windows.Forms.ToolStripItem tencent_handwriting; 

		#region 代理 (Proxy) 子菜单
		private global::System.Windows.Forms.ToolStripMenuItem customize_Proxy;
		private global::System.Windows.Forms.ToolStripMenuItem null_Proxy;
		private global::System.Windows.Forms.ToolStripMenuItem system_Proxy;
		private global::System.Windows.Forms.ToolStripMenuItem Proxy;
		#endregion

		#region "表格" 子菜单
		private global::System.Windows.Forms.ToolStripMenuItem baidu_table;
		private global::System.Windows.Forms.ToolStripMenuItem tx_table;
		private global::System.Windows.Forms.ToolStripMenuItem ali_table;
		#endregion
		//"AI_OCR"菜单
		public global::System.Windows.Forms.ToolStripMenuItem ai_menu;
		//"AI_OCR"子菜单
		// public global::System.Windows.Forms.ToolStripMenuItem ai_openai_compatible;
        //"AI_Trans"菜单
        public global::System.Windows.Forms.ToolStripMenuItem ai_menu_trans;
        //"AI_Trans"子菜单
        // public global::System.Windows.Forms.ToolStripMenuItem ai_openai_compatible_trans;

        #endregion

        #region 其他UI控件和变量

        public global::System.Drawing.SizeF font_base;

		public global::System.Windows.Forms.PictureBox PictureBox1;

		public global::System.Windows.Forms.ToolStripMenuItem Split;

		public global::System.Windows.Forms.ToolStripMenuItem Restore;

		public float F_factor;

		private global::TrOCR.AdvRichTextBox RichBoxBody;

		private global::System.IntPtr nextClipboardViewer;
		private global::System.Windows.Forms.Button btnToggleOriginalText;
		private global::System.Windows.Forms.Panel panelSeparator;

		#endregion

		#region 未分类或未使用的菜单项
		public global::System.Windows.Forms.ToolStripMenuItem baidu_v;
		public global::System.Windows.Forms.ToolStripMenuItem tencent_v;
		#endregion
	}
}
