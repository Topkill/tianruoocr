namespace TrOCR
{

    // 设置窗口类，用于管理OCR和翻译接口的各种配置选项
    public sealed partial class FmSetting : global::System.Windows.Forms.Form
    {

        // 释放资源
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // 初始化设置界面组件
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmSetting));
            this.Page_翻译接口 = new System.Windows.Forms.TabPage();
            this.tabControl_Trans = new System.Windows.Forms.TabControl();
            this.tabPage_Google = new System.Windows.Forms.TabPage();
            this.groupBox_Google_Key = new System.Windows.Forms.GroupBox();
            this.label_Google_Key = new System.Windows.Forms.Label();
            this.groupBox_Google_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Google_Target = new System.Windows.Forms.Button();
            this.textBox_Google_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Google_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Google_Source = new System.Windows.Forms.Button();
            this.textBox_Google_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Baidu = new System.Windows.Forms.TabPage();
            this.groupBox_Baidu_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Baidu_SK = new System.Windows.Forms.TextBox();
            this.label_Baidu_SK = new System.Windows.Forms.Label();
            this.textBox_Baidu_AK = new System.Windows.Forms.TextBox();
            this.label_Baidu_AK = new System.Windows.Forms.Label();
            this.groupBox_Baidu_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Baidu_Target = new System.Windows.Forms.Button();
            this.textBox_Baidu_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Baidu_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Baidu_Source = new System.Windows.Forms.Button();
            this.textBox_Baidu_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Tencent = new System.Windows.Forms.TabPage();
            this.groupBox_Tencent_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Tencent_SK = new System.Windows.Forms.TextBox();
            this.label_Tencent_SK = new System.Windows.Forms.Label();
            this.textBox_Tencent_AK = new System.Windows.Forms.TextBox();
            this.label_Tencent_AK = new System.Windows.Forms.Label();
            this.groupBox_Tencent_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Tencent_Target = new System.Windows.Forms.Button();
            this.textBox_Tencent_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Tencent_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Tencent_Source = new System.Windows.Forms.Button();
            this.textBox_Tencent_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Bing = new System.Windows.Forms.TabPage();
            this.groupBox_Bing_Key = new System.Windows.Forms.GroupBox();
            this.label_Bing_Key = new System.Windows.Forms.Label();
            this.groupBox_Bing_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Bing_Target = new System.Windows.Forms.Button();
            this.textBox_Bing_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Bing_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Bing_Source = new System.Windows.Forms.Button();
            this.textBox_Bing_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Bing2 = new System.Windows.Forms.TabPage();
            this.groupBox_Bing2_Key = new System.Windows.Forms.GroupBox();
            this.label_Bing2_Notice = new System.Windows.Forms.Label();
            this.groupBox_Bing2_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Bing2_Target = new System.Windows.Forms.Button();
            this.textBox_Bing2_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Bing2_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Bing2_Source = new System.Windows.Forms.Button();
            this.textBox_Bing2_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Microsoft = new System.Windows.Forms.TabPage();
            this.groupBox_Microsoft_Key = new System.Windows.Forms.GroupBox();
            this.label_Microsoft_Key = new System.Windows.Forms.Label();
            this.groupBox_Microsoft_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Microsoft_Target = new System.Windows.Forms.Button();
            this.textBox_Microsoft_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Microsoft_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Microsoft_Source = new System.Windows.Forms.Button();
            this.textBox_Microsoft_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Yandex = new System.Windows.Forms.TabPage();
            this.groupBox_Yandex_Key = new System.Windows.Forms.GroupBox();
            this.label_Yandex_Key = new System.Windows.Forms.Label();
            this.groupBox_Yandex_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Yandex_Target = new System.Windows.Forms.Button();
            this.textBox_Yandex_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Yandex_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Yandex_Source = new System.Windows.Forms.Button();
            this.textBox_Yandex_Source = new System.Windows.Forms.TextBox();
            this.tabPage_TencentInteractive = new System.Windows.Forms.TabPage();
            this.groupBox_TencentInteractive_Key = new System.Windows.Forms.GroupBox();
            this.label_TencentInteractive_Key = new System.Windows.Forms.Label();
            this.groupBox_TencentInteractive_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_TencentInteractive_Target = new System.Windows.Forms.Button();
            this.textBox_TencentInteractive_Target = new System.Windows.Forms.TextBox();
            this.groupBox_TencentInteractive_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_TencentInteractive_Source = new System.Windows.Forms.Button();
            this.textBox_TencentInteractive_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Caiyun = new System.Windows.Forms.TabPage();
            this.groupBox_Caiyun_Key = new System.Windows.Forms.GroupBox();
            this.label_Caiyun_Key = new System.Windows.Forms.Label();
            this.groupBox_Caiyun_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Caiyun_Target = new System.Windows.Forms.Button();
            this.textBox_Caiyun_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Caiyun_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Caiyun_Source = new System.Windows.Forms.Button();
            this.textBox_Caiyun_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Volcano = new System.Windows.Forms.TabPage();
            this.groupBox_Volcano_Key = new System.Windows.Forms.GroupBox();
            this.label_Volcano_Key = new System.Windows.Forms.Label();
            this.groupBox_Volcano_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Volcano_Target = new System.Windows.Forms.Button();
            this.textBox_Volcano_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Volcano_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Volcano_Source = new System.Windows.Forms.Button();
            this.textBox_Volcano_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Caiyun2 = new System.Windows.Forms.TabPage();
            this.groupBox_Caiyun2_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Caiyun2_Token = new System.Windows.Forms.TextBox();
            this.label_Caiyun2_Token = new System.Windows.Forms.Label();
            this.groupBox_Caiyun2_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Caiyun2_Target = new System.Windows.Forms.Button();
            this.textBox_Caiyun2_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Caiyun2_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Caiyun2_Source = new System.Windows.Forms.Button();
            this.textBox_Caiyun2_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Baidu2 = new System.Windows.Forms.TabPage();
            this.groupBox_Baidu2_Key = new System.Windows.Forms.GroupBox();
            this.label40 = new System.Windows.Forms.Label();
            this.groupBox_Baidu2_Target = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Baidu2_Target = new System.Windows.Forms.Button();
            this.textBox_Baidu2_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Baidu2_Source = new System.Windows.Forms.GroupBox();
            this.btn_Reset_Baidu2_Source = new System.Windows.Forms.Button();
            this.textBox_Baidu2_Source = new System.Windows.Forms.TextBox();
            this.tabPage_白描接口 = new System.Windows.Forms.TabPage();
            this.BoxBaimiaoPassword = new System.Windows.Forms.TextBox();
            this.BoxBaimiaoUsername = new System.Windows.Forms.TextBox();
            this.label_BaimiaoPassword = new System.Windows.Forms.Label();
            this.label_BaimiaoUsername = new System.Windows.Forms.Label();
            this.Page_AI密钥 = new System.Windows.Forms.TabPage();
            this.groupBox_AI_Help = new System.Windows.Forms.GroupBox();
            this.label_AI_Help = new System.Windows.Forms.Label();
            this.tabControl_AI = new System.Windows.Forms.TabControl();
            this.tabPage_OpenAICompatible = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Del_Provider = new System.Windows.Forms.Button();
            this.btn_Add_Provider = new System.Windows.Forms.Button();
            this.lb_CustomProviders = new System.Windows.Forms.ListBox();
            this.groupBox51 = new System.Windows.Forms.GroupBox();
            this.txt_ConfigPath = new System.Windows.Forms.TextBox();
            this.txt_ModelName = new System.Windows.Forms.TextBox();
            this.txt_ApiKey = new System.Windows.Forms.TextBox();
            this.txt_ApiUrl = new System.Windows.Forms.TextBox();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.btn_BrowseConfig = new System.Windows.Forms.Button();
            this.label69 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.Page_显示的接口 = new System.Windows.Forms.TabPage();
            this.groupBox_翻译接口 = new System.Windows.Forms.GroupBox();
            this.checkBox_ShowBaidu2 = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowGoogle = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowBaidu = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowTencent = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowBing = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowBing2 = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowMicrosoft = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowYandex = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowTencentInteractive = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowCaiyun = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowVolcano = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowCaiyun2 = new System.Windows.Forms.CheckBox();
            this.groupBox_Ocr = new System.Windows.Forms.GroupBox();
            this.checkBox_ShowOcrTableTencent = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrPaddleOCR = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrPaddleOCR2 = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrRapidOCR = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrBaidu = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrBaiduAccurate = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrTencent = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrTencentAccurate = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrBaimiao = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrSougou = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrYoudao = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrWeChat = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrMathfuntion = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrTable = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrShupai = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrTableBaidu = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrTableAli = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrShupaiLR = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowOcrShupaiRL = new System.Windows.Forms.CheckBox();
            this.Page_About = new System.Windows.Forms.TabPage();
            this.label78 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label_AuthorInfo = new System.Windows.Forms.Label();
            this.label_VersionInfo = new System.Windows.Forms.Label();
            this.Page_代理 = new System.Windows.Forms.TabPage();
            this.代理Button = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chbox_代理服务器 = new System.Windows.Forms.CheckBox();
            this.text_密码 = new System.Windows.Forms.TextBox();
            this.text_端口 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.text_账号 = new System.Windows.Forms.TextBox();
            this.text_服务器 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.combox_代理 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Page_密钥 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label_OcrApiHelpText = new System.Windows.Forms.Label();
            this.百度_btn = new System.Windows.Forms.Button();
            this.密钥Button_apply = new System.Windows.Forms.Button();
            this.密钥Button = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage_百度OCR = new System.Windows.Forms.TabPage();
            this.tabControl_BaiduApiType = new System.Windows.Forms.TabControl();
            this.inPage_百度接口 = new System.Windows.Forms.TabPage();
            this.comboBox_Baidu_Language = new System.Windows.Forms.ComboBox();
            this.label_Baidu_Language = new System.Windows.Forms.Label();
            this.text_baidupassword = new System.Windows.Forms.TextBox();
            this.text_baiduaccount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.inPage_百度高精度接口 = new System.Windows.Forms.TabPage();
            this.text_baidu_accurate_secretkey = new System.Windows.Forms.TextBox();
            this.text_baidu_accurate_apikey = new System.Windows.Forms.TextBox();
            this.label_baidu_accurate_secretkey = new System.Windows.Forms.Label();
            this.label_baidu_accurate_apikey = new System.Windows.Forms.Label();
            this.comboBox_Baidu_Accurate_Language = new System.Windows.Forms.ComboBox();
            this.label_Baidu_Accurate_Language = new System.Windows.Forms.Label();
            this.inPage_百度表格 = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.inPage_百度手写 = new System.Windows.Forms.TabPage();
            this.comboBox_Baidu_Handwriting_Language = new System.Windows.Forms.ComboBox();
            this.label39 = new System.Windows.Forms.Label();
            this.text_baidu_handwriting_secretkey = new System.Windows.Forms.TextBox();
            this.text_baidu_handwriting_apikey = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.tabPage_腾讯OCR = new System.Windows.Forms.TabPage();
            this.tabControl_TXApiType = new System.Windows.Forms.TabControl();
            this.inPage_腾讯接口 = new System.Windows.Forms.TabPage();
            this.BoxTencentKey = new System.Windows.Forms.TextBox();
            this.BoxTencentId = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.comboBox_Tencent_Language = new System.Windows.Forms.ComboBox();
            this.label_Tencent_Language = new System.Windows.Forms.Label();
            this.inPage_腾讯高精度接口 = new System.Windows.Forms.TabPage();
            this.text_tencent_accurate_secretkey = new System.Windows.Forms.TextBox();
            this.text_tencent_accurate_secretid = new System.Windows.Forms.TextBox();
            this.label_tencent_accurate_secretkey = new System.Windows.Forms.Label();
            this.label_tencent_accurate_secretid = new System.Windows.Forms.Label();
            this.comboBox_Tencent_Accurate_Language = new System.Windows.Forms.ComboBox();
            this.label_Tencent_Accurate_Language = new System.Windows.Forms.Label();
            this.inPage_腾讯表格v3 = new System.Windows.Forms.TabPage();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.inPage_腾讯手写 = new System.Windows.Forms.TabPage();
            this.label33 = new System.Windows.Forms.Label();
            this.inPage_PaddleOCR = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.btn_PaddleOCR_Keys_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR_Keys = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR_Keys = new System.Windows.Forms.Label();
            this.btn_PaddleOCR_Rec_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR_Rec = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR_Rec = new System.Windows.Forms.Label();
            this.btn_PaddleOCR_Cls_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR_Cls = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR_Cls = new System.Windows.Forms.Label();
            this.btn_PaddleOCR_Det_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR_Det = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR_Det = new System.Windows.Forms.Label();
            this.inPage_PaddleOCR2 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.comboBox_PaddleOCR2_rec_Version = new System.Windows.Forms.ComboBox();
            this.comboBox_PaddleOCR2_cls_Version = new System.Windows.Forms.ComboBox();
            this.label_PaddleOCR2_rec_Version = new System.Windows.Forms.Label();
            this.label_PaddleOCR2_cls_Version = new System.Windows.Forms.Label();
            this.comboBox_PaddleOCR2_det_Version = new System.Windows.Forms.ComboBox();
            this.label_PaddleOCR2_det_Version = new System.Windows.Forms.Label();
            this.btn_PaddleOCR2_Keys_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR2_Keys = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR2_Keys = new System.Windows.Forms.Label();
            this.btn_PaddleOCR2_Rec_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR2_Rec = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR2_Rec = new System.Windows.Forms.Label();
            this.btn_PaddleOCR2_Cls_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR2_Cls = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR2_Cls = new System.Windows.Forms.Label();
            this.btn_PaddleOCR2_Det_Browse = new System.Windows.Forms.Button();
            this.textBox_PaddleOCR2_Det = new System.Windows.Forms.TextBox();
            this.label_PaddleOCR2_Det = new System.Windows.Forms.Label();
            this.inPage_RapidOCR = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.btn_RapidOCR_Keys_Browse = new System.Windows.Forms.Button();
            this.textBox_RapidOCR_Keys = new System.Windows.Forms.TextBox();
            this.label_RapidOCR_Keys = new System.Windows.Forms.Label();
            this.btn_RapidOCR_Rec_Browse = new System.Windows.Forms.Button();
            this.textBox_RapidOCR_Rec = new System.Windows.Forms.TextBox();
            this.label_RapidOCR_Rec = new System.Windows.Forms.Label();
            this.btn_RapidOCR_Cls_Browse = new System.Windows.Forms.Button();
            this.textBox_RapidOCR_Cls = new System.Windows.Forms.TextBox();
            this.label_RapidOCR_Cls = new System.Windows.Forms.Label();
            this.btn_RapidOCR_Det_Browse = new System.Windows.Forms.Button();
            this.textBox_RapidOCR_Det = new System.Windows.Forms.TextBox();
            this.label_RapidOCR_Det = new System.Windows.Forms.Label();
            this.groupBox_PaddleOCR_Config = new System.Windows.Forms.GroupBox();
            this.label_PaddleOCR_Status = new System.Windows.Forms.Label();
            this.groupBox_PaddleOCR2_Config = new System.Windows.Forms.GroupBox();
            this.label_PaddleOCR2_Status = new System.Windows.Forms.Label();
            this.groupBox_RapidOCR_Config = new System.Windows.Forms.GroupBox();
            this.label_RapidOCR_Status = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.Page_快捷键 = new System.Windows.Forms.TabPage();
            this.快捷键Button = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox_截图翻译 = new System.Windows.Forms.PictureBox();
            this.txtBox_截图翻译 = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.pictureBox_静默识别 = new System.Windows.Forms.PictureBox();
            this.txtBox_静默识别 = new System.Windows.Forms.TextBox();
            this.label_静默识别 = new System.Windows.Forms.Label();
            this.pictureBox_输入翻译 = new System.Windows.Forms.PictureBox();
            this.txtBox_输入翻译 = new System.Windows.Forms.TextBox();
            this.label_输入翻译 = new System.Windows.Forms.Label();
            this.pictureBox_识别界面 = new System.Windows.Forms.PictureBox();
            this.pictureBox_记录界面 = new System.Windows.Forms.PictureBox();
            this.pictureBox_翻译文本 = new System.Windows.Forms.PictureBox();
            this.pictureBox_文字识别 = new System.Windows.Forms.PictureBox();
            this.txtBox_识别界面 = new System.Windows.Forms.TextBox();
            this.txtBox_记录界面 = new System.Windows.Forms.TextBox();
            this.txtBox_翻译文本 = new System.Windows.Forms.TextBox();
            this.txtBox_文字识别 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.page_常规 = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chbox_save = new System.Windows.Forms.CheckBox();
            this.chbox_copy = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.btn_音效路径 = new System.Windows.Forms.Button();
            this.btn_音效 = new System.Windows.Forms.Button();
            this.text_音效path = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_浏览 = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBox_保存 = new System.Windows.Forms.CheckBox();
            this.pic_help = new System.Windows.Forms.PictureBox();
            this.常规Button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numbox_记录 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cobBox_动画 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbox_取色 = new System.Windows.Forms.CheckBox();
            this.cbBox_弹窗 = new System.Windows.Forms.CheckBox();
            this.cbBox_翻译 = new System.Windows.Forms.CheckBox();
            this.cbBox_开机 = new System.Windows.Forms.CheckBox();
            this.groupBox_OcrWorkflow = new System.Windows.Forms.GroupBox();
            this.checkBox_AutoCopyOcrResult = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoTranslateOcrResult = new System.Windows.Forms.CheckBox();
            this.groupBox_TranslateWorkflow = new System.Windows.Forms.GroupBox();
            this.checkBox_AutoCopyOcrTranslation = new System.Windows.Forms.CheckBox();
            this.tab_标签 = new System.Windows.Forms.TabControl();
            this.Page_常规2 = new System.Windows.Forms.TabPage();
            this.groupBox50 = new System.Windows.Forms.GroupBox();
            this.label76 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.textBox38 = new System.Windows.Forms.TextBox();
            this.label64 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.checkBox_AutoCopyInputTranslation = new System.Windows.Forms.CheckBox();
            this.cbBox_输入翻译自动翻译 = new System.Windows.Forms.CheckBox();
            this.cbBox_输入翻译剪贴板 = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.checkbox_NoWindowScreenshotTranslation = new System.Windows.Forms.CheckBox();
            this.checkbox_AutoCopyScreenshotTranslation = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label80 = new System.Windows.Forms.Label();
            this.textBox39 = new System.Windows.Forms.TextBox();
            this.label79 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.textBox37 = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.cbBox_禁用隐藏原文按钮 = new System.Windows.Forms.CheckBox();
            this.checkBox_EnableTabHoverSwitch = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbBox_ListenHideOriginal = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbBox_AutoCopyListenClipboardTranslation = new System.Windows.Forms.CheckBox();
            this.cbBox_ListenClipboard = new System.Windows.Forms.CheckBox();
            this.label34 = new System.Windows.Forms.Label();
            this.Page_AI翻译接口 = new System.Windows.Forms.TabPage();
            this.groupBox_AI_Trans_Help = new System.Windows.Forms.GroupBox();
            this.label60 = new System.Windows.Forms.Label();
            this.tabControl_AI_Trans = new System.Windows.Forms.TabControl();
            this.tabPage_Trans_OpenAICompatible = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_Trans_Del_Provider = new System.Windows.Forms.Button();
            this.btn_Trans_Add_Provider = new System.Windows.Forms.Button();
            this.lb_CustomTransProviders = new System.Windows.Forms.ListBox();
            this.groupBox52 = new System.Windows.Forms.GroupBox();
            this.txt_Trans_Target = new System.Windows.Forms.TextBox();
            this.txt_Trans_Source = new System.Windows.Forms.TextBox();
            this.label70 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.txt_Trans_ConfigPath = new System.Windows.Forms.TextBox();
            this.txt_Trans_ModelName = new System.Windows.Forms.TextBox();
            this.txt_Trans_ApiKey = new System.Windows.Forms.TextBox();
            this.txt_Trans_ApiUrl = new System.Windows.Forms.TextBox();
            this.txt_Trans_Name = new System.Windows.Forms.TextBox();
            this.btn_Trans_BrowseConfig = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.Page_工具栏 = new System.Windows.Forms.TabPage();
            this.groupBox_拆分文本 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_拆分文本 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox_拆分后自动复制 = new System.Windows.Forms.CheckBox();
            this.groupBox_合并文本 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_合并文本 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox_合并去除所有空格 = new System.Windows.Forms.CheckBox();
            this.checkBox_合并去除空格 = new System.Windows.Forms.CheckBox();
            this.checkBox_合并自动复制 = new System.Windows.Forms.CheckBox();
            this.Page_更新 = new System.Windows.Forms.TabPage();
            this.更新Button_check = new System.Windows.Forms.Button();
            this.更新Button = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.numbox_间隔时间 = new System.Windows.Forms.NumericUpDown();
            this.checkBox_更新间隔 = new System.Windows.Forms.CheckBox();
            this.check_检查更新 = new System.Windows.Forms.CheckBox();
            this.checkBox_PreRelease = new System.Windows.Forms.CheckBox();
            this.Page_webdav = new System.Windows.Forms.TabPage();
            this.label75 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.btnDownloadConfig = new System.Windows.Forms.Button();
            this.btnUploadConfig = new System.Windows.Forms.Button();
            this.txtWebDavPass = new System.Windows.Forms.TextBox();
            this.txtWebDavUrl = new System.Windows.Forms.TextBox();
            this.label71 = new System.Windows.Forms.Label();
            this.txtWebDavUser = new System.Windows.Forms.TextBox();
            this.Page_反馈 = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.反馈Button = new System.Windows.Forms.Button();
            this.txt_问题反馈 = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label_版本号 = new System.Windows.Forms.Label();
            this.label_更新日期 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.txt_更新说明 = new System.Windows.Forms.TextBox();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.groupBox49 = new System.Windows.Forms.GroupBox();
            this.textBox36 = new System.Windows.Forms.TextBox();
            this.button28 = new System.Windows.Forms.Button();
            this.groupBox48 = new System.Windows.Forms.GroupBox();
            this.textBox35 = new System.Windows.Forms.TextBox();
            this.button27 = new System.Windows.Forms.Button();
            this.groupBox47 = new System.Windows.Forms.GroupBox();
            this.label53 = new System.Windows.Forms.Label();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.groupBox46 = new System.Windows.Forms.GroupBox();
            this.textBox34 = new System.Windows.Forms.TextBox();
            this.button26 = new System.Windows.Forms.Button();
            this.groupBox45 = new System.Windows.Forms.GroupBox();
            this.textBox33 = new System.Windows.Forms.TextBox();
            this.button25 = new System.Windows.Forms.Button();
            this.groupBox44 = new System.Windows.Forms.GroupBox();
            this.label52 = new System.Windows.Forms.Label();
            this.textBox32 = new System.Windows.Forms.TextBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.groupBox43 = new System.Windows.Forms.GroupBox();
            this.textBox31 = new System.Windows.Forms.TextBox();
            this.button24 = new System.Windows.Forms.Button();
            this.groupBox42 = new System.Windows.Forms.GroupBox();
            this.textBox30 = new System.Windows.Forms.TextBox();
            this.button23 = new System.Windows.Forms.Button();
            this.groupBox41 = new System.Windows.Forms.GroupBox();
            this.label51 = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.groupBox40 = new System.Windows.Forms.GroupBox();
            this.textBox29 = new System.Windows.Forms.TextBox();
            this.button22 = new System.Windows.Forms.Button();
            this.groupBox39 = new System.Windows.Forms.GroupBox();
            this.textBox28 = new System.Windows.Forms.TextBox();
            this.button21 = new System.Windows.Forms.Button();
            this.groupBox38 = new System.Windows.Forms.GroupBox();
            this.label50 = new System.Windows.Forms.Label();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.groupBox37 = new System.Windows.Forms.GroupBox();
            this.textBox27 = new System.Windows.Forms.TextBox();
            this.button20 = new System.Windows.Forms.Button();
            this.groupBox36 = new System.Windows.Forms.GroupBox();
            this.textBox26 = new System.Windows.Forms.TextBox();
            this.button19 = new System.Windows.Forms.Button();
            this.groupBox35 = new System.Windows.Forms.GroupBox();
            this.label49 = new System.Windows.Forms.Label();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.groupBox34 = new System.Windows.Forms.GroupBox();
            this.textBox25 = new System.Windows.Forms.TextBox();
            this.button18 = new System.Windows.Forms.Button();
            this.groupBox33 = new System.Windows.Forms.GroupBox();
            this.textBox24 = new System.Windows.Forms.TextBox();
            this.button17 = new System.Windows.Forms.Button();
            this.groupBox32 = new System.Windows.Forms.GroupBox();
            this.label48 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.button16 = new System.Windows.Forms.Button();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.textBox22 = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.label47 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.button14 = new System.Windows.Forms.Button();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.button13 = new System.Windows.Forms.Button();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.label46 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.button12 = new System.Windows.Forms.Button();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.label45 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.label44 = new System.Windows.Forms.Label();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.label42 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label32 = new System.Windows.Forms.Label();
            this.Page_翻译接口.SuspendLayout();
            this.tabControl_Trans.SuspendLayout();
            this.tabPage_Google.SuspendLayout();
            this.groupBox_Google_Key.SuspendLayout();
            this.groupBox_Google_Target.SuspendLayout();
            this.groupBox_Google_Source.SuspendLayout();
            this.tabPage_Baidu.SuspendLayout();
            this.groupBox_Baidu_Key.SuspendLayout();
            this.groupBox_Baidu_Target.SuspendLayout();
            this.groupBox_Baidu_Source.SuspendLayout();
            this.tabPage_Tencent.SuspendLayout();
            this.groupBox_Tencent_Key.SuspendLayout();
            this.groupBox_Tencent_Target.SuspendLayout();
            this.groupBox_Tencent_Source.SuspendLayout();
            this.tabPage_Bing.SuspendLayout();
            this.groupBox_Bing_Key.SuspendLayout();
            this.groupBox_Bing_Target.SuspendLayout();
            this.groupBox_Bing_Source.SuspendLayout();
            this.tabPage_Bing2.SuspendLayout();
            this.groupBox_Bing2_Key.SuspendLayout();
            this.groupBox_Bing2_Target.SuspendLayout();
            this.groupBox_Bing2_Source.SuspendLayout();
            this.tabPage_Microsoft.SuspendLayout();
            this.groupBox_Microsoft_Key.SuspendLayout();
            this.groupBox_Microsoft_Target.SuspendLayout();
            this.groupBox_Microsoft_Source.SuspendLayout();
            this.tabPage_Yandex.SuspendLayout();
            this.groupBox_Yandex_Key.SuspendLayout();
            this.groupBox_Yandex_Target.SuspendLayout();
            this.groupBox_Yandex_Source.SuspendLayout();
            this.tabPage_TencentInteractive.SuspendLayout();
            this.groupBox_TencentInteractive_Key.SuspendLayout();
            this.groupBox_TencentInteractive_Target.SuspendLayout();
            this.groupBox_TencentInteractive_Source.SuspendLayout();
            this.tabPage_Caiyun.SuspendLayout();
            this.groupBox_Caiyun_Key.SuspendLayout();
            this.groupBox_Caiyun_Target.SuspendLayout();
            this.groupBox_Caiyun_Source.SuspendLayout();
            this.tabPage_Volcano.SuspendLayout();
            this.groupBox_Volcano_Key.SuspendLayout();
            this.groupBox_Volcano_Target.SuspendLayout();
            this.groupBox_Volcano_Source.SuspendLayout();
            this.tabPage_Caiyun2.SuspendLayout();
            this.groupBox_Caiyun2_Key.SuspendLayout();
            this.groupBox_Caiyun2_Target.SuspendLayout();
            this.groupBox_Caiyun2_Source.SuspendLayout();
            this.tabPage_Baidu2.SuspendLayout();
            this.groupBox_Baidu2_Key.SuspendLayout();
            this.groupBox_Baidu2_Target.SuspendLayout();
            this.groupBox_Baidu2_Source.SuspendLayout();
            this.tabPage_白描接口.SuspendLayout();
            this.Page_AI密钥.SuspendLayout();
            this.groupBox_AI_Help.SuspendLayout();
            this.tabControl_AI.SuspendLayout();
            this.tabPage_OpenAICompatible.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox51.SuspendLayout();
            this.Page_显示的接口.SuspendLayout();
            this.groupBox_翻译接口.SuspendLayout();
            this.groupBox_Ocr.SuspendLayout();
            this.Page_About.SuspendLayout();
            this.Page_代理.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.Page_密钥.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage_百度OCR.SuspendLayout();
            this.tabControl_BaiduApiType.SuspendLayout();
            this.inPage_百度接口.SuspendLayout();
            this.inPage_百度高精度接口.SuspendLayout();
            this.inPage_百度表格.SuspendLayout();
            this.inPage_百度手写.SuspendLayout();
            this.tabPage_腾讯OCR.SuspendLayout();
            this.tabControl_TXApiType.SuspendLayout();
            this.inPage_腾讯接口.SuspendLayout();
            this.inPage_腾讯高精度接口.SuspendLayout();
            this.inPage_腾讯表格v3.SuspendLayout();
            this.inPage_腾讯手写.SuspendLayout();
            this.inPage_PaddleOCR.SuspendLayout();
            this.inPage_PaddleOCR2.SuspendLayout();
            this.inPage_RapidOCR.SuspendLayout();
            this.Page_快捷键.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_截图翻译)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_静默识别)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_输入翻译)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_识别界面)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_记录界面)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_翻译文本)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_文字识别)).BeginInit();
            this.page_常规.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_help)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_记录)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox_OcrWorkflow.SuspendLayout();
            this.groupBox_TranslateWorkflow.SuspendLayout();
            this.tab_标签.SuspendLayout();
            this.Page_常规2.SuspendLayout();
            this.groupBox50.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.Page_AI翻译接口.SuspendLayout();
            this.groupBox_AI_Trans_Help.SuspendLayout();
            this.tabControl_AI_Trans.SuspendLayout();
            this.tabPage_Trans_OpenAICompatible.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox52.SuspendLayout();
            this.Page_工具栏.SuspendLayout();
            this.groupBox_拆分文本.SuspendLayout();
            this.tableLayoutPanel_拆分文本.SuspendLayout();
            this.groupBox_合并文本.SuspendLayout();
            this.tableLayoutPanel_合并文本.SuspendLayout();
            this.Page_更新.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_间隔时间)).BeginInit();
            this.Page_webdav.SuspendLayout();
            this.Page_反馈.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.SuspendLayout();
            // 
            // Page_翻译接口
            // 
            this.Page_翻译接口.BackColor = System.Drawing.Color.White;
            this.Page_翻译接口.Controls.Add(this.tabControl_Trans);
            this.Page_翻译接口.Location = new System.Drawing.Point(4, 25);
            this.Page_翻译接口.Margin = new System.Windows.Forms.Padding(4);
            this.Page_翻译接口.Name = "Page_翻译接口";
            this.Page_翻译接口.Size = new System.Drawing.Size(523, 515);
            this.Page_翻译接口.TabIndex = 10;
            this.Page_翻译接口.Text = "翻译接口";
            // 
            // tabControl_Trans
            // 
            this.tabControl_Trans.Controls.Add(this.tabPage_Google);
            this.tabControl_Trans.Controls.Add(this.tabPage_Baidu);
            this.tabControl_Trans.Controls.Add(this.tabPage_Tencent);
            this.tabControl_Trans.Controls.Add(this.tabPage_Bing);
            this.tabControl_Trans.Controls.Add(this.tabPage_Bing2);
            this.tabControl_Trans.Controls.Add(this.tabPage_Microsoft);
            this.tabControl_Trans.Controls.Add(this.tabPage_Yandex);
            this.tabControl_Trans.Controls.Add(this.tabPage_TencentInteractive);
            this.tabControl_Trans.Controls.Add(this.tabPage_Caiyun);
            this.tabControl_Trans.Controls.Add(this.tabPage_Volcano);
            this.tabControl_Trans.Controls.Add(this.tabPage_Caiyun2);
            this.tabControl_Trans.Controls.Add(this.tabPage_Baidu2);
            this.tabControl_Trans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Trans.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Trans.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl_Trans.Name = "tabControl_Trans";
            this.tabControl_Trans.SelectedIndex = 0;
            this.tabControl_Trans.Size = new System.Drawing.Size(523, 515);
            this.tabControl_Trans.TabIndex = 0;
            // 
            // tabPage_Google
            // 
            this.tabPage_Google.Controls.Add(this.groupBox_Google_Key);
            this.tabPage_Google.Controls.Add(this.groupBox_Google_Target);
            this.tabPage_Google.Controls.Add(this.groupBox_Google_Source);
            this.tabPage_Google.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Google.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Google.Name = "tabPage_Google";
            this.tabPage_Google.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage_Google.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Google.TabIndex = 0;
            this.tabPage_Google.Text = "Google";
            this.tabPage_Google.UseVisualStyleBackColor = true;
            // 
            // groupBox_Google_Key
            // 
            this.groupBox_Google_Key.Controls.Add(this.label_Google_Key);
            this.groupBox_Google_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Google_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Google_Key.Name = "groupBox_Google_Key";
            this.groupBox_Google_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Google_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Google_Key.TabIndex = 2;
            this.groupBox_Google_Key.TabStop = false;
            this.groupBox_Google_Key.Text = "密钥";
            // 
            // label_Google_Key
            // 
            this.label_Google_Key.AutoSize = true;
            this.label_Google_Key.Location = new System.Drawing.Point(200, 31);
            this.label_Google_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Google_Key.Name = "label_Google_Key";
            this.label_Google_Key.Size = new System.Drawing.Size(67, 15);
            this.label_Google_Key.TabIndex = 0;
            this.label_Google_Key.Text = "无需密钥";
            // 
            // groupBox_Google_Target
            // 
            this.groupBox_Google_Target.Controls.Add(this.btn_Reset_Google_Target);
            this.groupBox_Google_Target.Controls.Add(this.textBox_Google_Target);
            this.groupBox_Google_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Google_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Google_Target.Name = "groupBox_Google_Target";
            this.groupBox_Google_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Google_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Google_Target.TabIndex = 1;
            this.groupBox_Google_Target.TabStop = false;
            this.groupBox_Google_Target.Text = "目标语言";
            // 
            // btn_Reset_Google_Target
            // 
            this.btn_Reset_Google_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Google_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Google_Target.Name = "btn_Reset_Google_Target";
            this.btn_Reset_Google_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Google_Target.TabIndex = 1;
            this.btn_Reset_Google_Target.Text = "重置";
            this.btn_Reset_Google_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Google_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Google_Target
            // 
            this.textBox_Google_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Google_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Google_Target.Name = "textBox_Google_Target";
            this.textBox_Google_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Google_Target.TabIndex = 0;
            // 
            // groupBox_Google_Source
            // 
            this.groupBox_Google_Source.Controls.Add(this.btn_Reset_Google_Source);
            this.groupBox_Google_Source.Controls.Add(this.textBox_Google_Source);
            this.groupBox_Google_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Google_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Google_Source.Name = "groupBox_Google_Source";
            this.groupBox_Google_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Google_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Google_Source.TabIndex = 0;
            this.groupBox_Google_Source.TabStop = false;
            this.groupBox_Google_Source.Text = "源语言";
            // 
            // btn_Reset_Google_Source
            // 
            this.btn_Reset_Google_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Google_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Google_Source.Name = "btn_Reset_Google_Source";
            this.btn_Reset_Google_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Google_Source.TabIndex = 1;
            this.btn_Reset_Google_Source.Text = "重置";
            this.btn_Reset_Google_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Google_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Google_Source
            // 
            this.textBox_Google_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Google_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Google_Source.Name = "textBox_Google_Source";
            this.textBox_Google_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Google_Source.TabIndex = 0;
            // 
            // tabPage_Baidu
            // 
            this.tabPage_Baidu.Controls.Add(this.groupBox_Baidu_Key);
            this.tabPage_Baidu.Controls.Add(this.groupBox_Baidu_Target);
            this.tabPage_Baidu.Controls.Add(this.groupBox_Baidu_Source);
            this.tabPage_Baidu.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Baidu.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Baidu.Name = "tabPage_Baidu";
            this.tabPage_Baidu.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage_Baidu.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Baidu.TabIndex = 1;
            this.tabPage_Baidu.Text = "Baidu";
            this.tabPage_Baidu.UseVisualStyleBackColor = true;
            // 
            // groupBox_Baidu_Key
            // 
            this.groupBox_Baidu_Key.Controls.Add(this.textBox_Baidu_SK);
            this.groupBox_Baidu_Key.Controls.Add(this.label_Baidu_SK);
            this.groupBox_Baidu_Key.Controls.Add(this.textBox_Baidu_AK);
            this.groupBox_Baidu_Key.Controls.Add(this.label_Baidu_AK);
            this.groupBox_Baidu_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Baidu_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu_Key.Name = "groupBox_Baidu_Key";
            this.groupBox_Baidu_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu_Key.Size = new System.Drawing.Size(493, 100);
            this.groupBox_Baidu_Key.TabIndex = 5;
            this.groupBox_Baidu_Key.TabStop = false;
            this.groupBox_Baidu_Key.Text = "密钥";
            // 
            // textBox_Baidu_SK
            // 
            this.textBox_Baidu_SK.Location = new System.Drawing.Point(107, 59);
            this.textBox_Baidu_SK.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Baidu_SK.Name = "textBox_Baidu_SK";
            this.textBox_Baidu_SK.Size = new System.Drawing.Size(377, 25);
            this.textBox_Baidu_SK.TabIndex = 3;
            // 
            // label_Baidu_SK
            // 
            this.label_Baidu_SK.AutoSize = true;
            this.label_Baidu_SK.Location = new System.Drawing.Point(9, 62);
            this.label_Baidu_SK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Baidu_SK.Name = "label_Baidu_SK";
            this.label_Baidu_SK.Size = new System.Drawing.Size(95, 15);
            this.label_Baidu_SK.TabIndex = 2;
            this.label_Baidu_SK.Text = "Secret Key:";
            // 
            // textBox_Baidu_AK
            // 
            this.textBox_Baidu_AK.Location = new System.Drawing.Point(107, 25);
            this.textBox_Baidu_AK.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Baidu_AK.Name = "textBox_Baidu_AK";
            this.textBox_Baidu_AK.Size = new System.Drawing.Size(377, 25);
            this.textBox_Baidu_AK.TabIndex = 1;
            // 
            // label_Baidu_AK
            // 
            this.label_Baidu_AK.AutoSize = true;
            this.label_Baidu_AK.Location = new System.Drawing.Point(9, 29);
            this.label_Baidu_AK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Baidu_AK.Name = "label_Baidu_AK";
            this.label_Baidu_AK.Size = new System.Drawing.Size(63, 15);
            this.label_Baidu_AK.TabIndex = 0;
            this.label_Baidu_AK.Text = "APP ID:";
            // 
            // groupBox_Baidu_Target
            // 
            this.groupBox_Baidu_Target.Controls.Add(this.btn_Reset_Baidu_Target);
            this.groupBox_Baidu_Target.Controls.Add(this.textBox_Baidu_Target);
            this.groupBox_Baidu_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Baidu_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu_Target.Name = "groupBox_Baidu_Target";
            this.groupBox_Baidu_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Baidu_Target.TabIndex = 4;
            this.groupBox_Baidu_Target.TabStop = false;
            this.groupBox_Baidu_Target.Text = "目标语言";
            // 
            // btn_Reset_Baidu_Target
            // 
            this.btn_Reset_Baidu_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Baidu_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Baidu_Target.Name = "btn_Reset_Baidu_Target";
            this.btn_Reset_Baidu_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Baidu_Target.TabIndex = 1;
            this.btn_Reset_Baidu_Target.Text = "重置";
            this.btn_Reset_Baidu_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Baidu_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Baidu_Target
            // 
            this.textBox_Baidu_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Baidu_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Baidu_Target.Name = "textBox_Baidu_Target";
            this.textBox_Baidu_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Baidu_Target.TabIndex = 0;
            // 
            // groupBox_Baidu_Source
            // 
            this.groupBox_Baidu_Source.Controls.Add(this.btn_Reset_Baidu_Source);
            this.groupBox_Baidu_Source.Controls.Add(this.textBox_Baidu_Source);
            this.groupBox_Baidu_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Baidu_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu_Source.Name = "groupBox_Baidu_Source";
            this.groupBox_Baidu_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Baidu_Source.TabIndex = 3;
            this.groupBox_Baidu_Source.TabStop = false;
            this.groupBox_Baidu_Source.Text = "源语言";
            // 
            // btn_Reset_Baidu_Source
            // 
            this.btn_Reset_Baidu_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Baidu_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Baidu_Source.Name = "btn_Reset_Baidu_Source";
            this.btn_Reset_Baidu_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Baidu_Source.TabIndex = 1;
            this.btn_Reset_Baidu_Source.Text = "重置";
            this.btn_Reset_Baidu_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Baidu_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Baidu_Source
            // 
            this.textBox_Baidu_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Baidu_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Baidu_Source.Name = "textBox_Baidu_Source";
            this.textBox_Baidu_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Baidu_Source.TabIndex = 0;
            // 
            // tabPage_Tencent
            // 
            this.tabPage_Tencent.Controls.Add(this.groupBox_Tencent_Key);
            this.tabPage_Tencent.Controls.Add(this.groupBox_Tencent_Target);
            this.tabPage_Tencent.Controls.Add(this.groupBox_Tencent_Source);
            this.tabPage_Tencent.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Tencent.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Tencent.Name = "tabPage_Tencent";
            this.tabPage_Tencent.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Tencent.TabIndex = 2;
            this.tabPage_Tencent.Text = "Tencent";
            this.tabPage_Tencent.UseVisualStyleBackColor = true;
            // 
            // groupBox_Tencent_Key
            // 
            this.groupBox_Tencent_Key.Controls.Add(this.textBox_Tencent_SK);
            this.groupBox_Tencent_Key.Controls.Add(this.label_Tencent_SK);
            this.groupBox_Tencent_Key.Controls.Add(this.textBox_Tencent_AK);
            this.groupBox_Tencent_Key.Controls.Add(this.label_Tencent_AK);
            this.groupBox_Tencent_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Tencent_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Tencent_Key.Name = "groupBox_Tencent_Key";
            this.groupBox_Tencent_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Tencent_Key.Size = new System.Drawing.Size(493, 100);
            this.groupBox_Tencent_Key.TabIndex = 5;
            this.groupBox_Tencent_Key.TabStop = false;
            this.groupBox_Tencent_Key.Text = "密钥";
            // 
            // textBox_Tencent_SK
            // 
            this.textBox_Tencent_SK.Location = new System.Drawing.Point(107, 59);
            this.textBox_Tencent_SK.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Tencent_SK.Name = "textBox_Tencent_SK";
            this.textBox_Tencent_SK.Size = new System.Drawing.Size(377, 25);
            this.textBox_Tencent_SK.TabIndex = 3;
            // 
            // label_Tencent_SK
            // 
            this.label_Tencent_SK.AutoSize = true;
            this.label_Tencent_SK.Location = new System.Drawing.Point(9, 62);
            this.label_Tencent_SK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Tencent_SK.Name = "label_Tencent_SK";
            this.label_Tencent_SK.Size = new System.Drawing.Size(87, 15);
            this.label_Tencent_SK.TabIndex = 2;
            this.label_Tencent_SK.Text = "SecretKey:";
            // 
            // textBox_Tencent_AK
            // 
            this.textBox_Tencent_AK.Location = new System.Drawing.Point(107, 25);
            this.textBox_Tencent_AK.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Tencent_AK.Name = "textBox_Tencent_AK";
            this.textBox_Tencent_AK.Size = new System.Drawing.Size(377, 25);
            this.textBox_Tencent_AK.TabIndex = 1;
            // 
            // label_Tencent_AK
            // 
            this.label_Tencent_AK.AutoSize = true;
            this.label_Tencent_AK.Location = new System.Drawing.Point(9, 29);
            this.label_Tencent_AK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Tencent_AK.Name = "label_Tencent_AK";
            this.label_Tencent_AK.Size = new System.Drawing.Size(79, 15);
            this.label_Tencent_AK.TabIndex = 0;
            this.label_Tencent_AK.Text = "SecretId:";
            // 
            // groupBox_Tencent_Target
            // 
            this.groupBox_Tencent_Target.Controls.Add(this.btn_Reset_Tencent_Target);
            this.groupBox_Tencent_Target.Controls.Add(this.textBox_Tencent_Target);
            this.groupBox_Tencent_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Tencent_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Tencent_Target.Name = "groupBox_Tencent_Target";
            this.groupBox_Tencent_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Tencent_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Tencent_Target.TabIndex = 4;
            this.groupBox_Tencent_Target.TabStop = false;
            this.groupBox_Tencent_Target.Text = "目标语言";
            // 
            // btn_Reset_Tencent_Target
            // 
            this.btn_Reset_Tencent_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Tencent_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Tencent_Target.Name = "btn_Reset_Tencent_Target";
            this.btn_Reset_Tencent_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Tencent_Target.TabIndex = 1;
            this.btn_Reset_Tencent_Target.Text = "重置";
            this.btn_Reset_Tencent_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Tencent_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Tencent_Target
            // 
            this.textBox_Tencent_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Tencent_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Tencent_Target.Name = "textBox_Tencent_Target";
            this.textBox_Tencent_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Tencent_Target.TabIndex = 0;
            // 
            // groupBox_Tencent_Source
            // 
            this.groupBox_Tencent_Source.Controls.Add(this.btn_Reset_Tencent_Source);
            this.groupBox_Tencent_Source.Controls.Add(this.textBox_Tencent_Source);
            this.groupBox_Tencent_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Tencent_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Tencent_Source.Name = "groupBox_Tencent_Source";
            this.groupBox_Tencent_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Tencent_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Tencent_Source.TabIndex = 3;
            this.groupBox_Tencent_Source.TabStop = false;
            this.groupBox_Tencent_Source.Text = "源语言";
            // 
            // btn_Reset_Tencent_Source
            // 
            this.btn_Reset_Tencent_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Tencent_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Tencent_Source.Name = "btn_Reset_Tencent_Source";
            this.btn_Reset_Tencent_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Tencent_Source.TabIndex = 1;
            this.btn_Reset_Tencent_Source.Text = "重置";
            this.btn_Reset_Tencent_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Tencent_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Tencent_Source
            // 
            this.textBox_Tencent_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Tencent_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Tencent_Source.Name = "textBox_Tencent_Source";
            this.textBox_Tencent_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Tencent_Source.TabIndex = 0;
            // 
            // tabPage_Bing
            // 
            this.tabPage_Bing.Controls.Add(this.groupBox_Bing_Key);
            this.tabPage_Bing.Controls.Add(this.groupBox_Bing_Target);
            this.tabPage_Bing.Controls.Add(this.groupBox_Bing_Source);
            this.tabPage_Bing.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Bing.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Bing.Name = "tabPage_Bing";
            this.tabPage_Bing.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Bing.TabIndex = 3;
            this.tabPage_Bing.Text = "Bing";
            this.tabPage_Bing.UseVisualStyleBackColor = true;
            // 
            // groupBox_Bing_Key
            // 
            this.groupBox_Bing_Key.Controls.Add(this.label_Bing_Key);
            this.groupBox_Bing_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Bing_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing_Key.Name = "groupBox_Bing_Key";
            this.groupBox_Bing_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Bing_Key.TabIndex = 2;
            this.groupBox_Bing_Key.TabStop = false;
            this.groupBox_Bing_Key.Text = "密钥";
            // 
            // label_Bing_Key
            // 
            this.label_Bing_Key.AutoSize = true;
            this.label_Bing_Key.Location = new System.Drawing.Point(200, 31);
            this.label_Bing_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Bing_Key.Name = "label_Bing_Key";
            this.label_Bing_Key.Size = new System.Drawing.Size(67, 15);
            this.label_Bing_Key.TabIndex = 0;
            this.label_Bing_Key.Text = "无需密钥";
            // 
            // groupBox_Bing_Target
            // 
            this.groupBox_Bing_Target.Controls.Add(this.btn_Reset_Bing_Target);
            this.groupBox_Bing_Target.Controls.Add(this.textBox_Bing_Target);
            this.groupBox_Bing_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Bing_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing_Target.Name = "groupBox_Bing_Target";
            this.groupBox_Bing_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Bing_Target.TabIndex = 1;
            this.groupBox_Bing_Target.TabStop = false;
            this.groupBox_Bing_Target.Text = "目标语言";
            // 
            // btn_Reset_Bing_Target
            // 
            this.btn_Reset_Bing_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Bing_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Bing_Target.Name = "btn_Reset_Bing_Target";
            this.btn_Reset_Bing_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Bing_Target.TabIndex = 1;
            this.btn_Reset_Bing_Target.Text = "重置";
            this.btn_Reset_Bing_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Bing_Target
            // 
            this.textBox_Bing_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Bing_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Bing_Target.Name = "textBox_Bing_Target";
            this.textBox_Bing_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Bing_Target.TabIndex = 0;
            // 
            // groupBox_Bing_Source
            // 
            this.groupBox_Bing_Source.Controls.Add(this.btn_Reset_Bing_Source);
            this.groupBox_Bing_Source.Controls.Add(this.textBox_Bing_Source);
            this.groupBox_Bing_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Bing_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing_Source.Name = "groupBox_Bing_Source";
            this.groupBox_Bing_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Bing_Source.TabIndex = 0;
            this.groupBox_Bing_Source.TabStop = false;
            this.groupBox_Bing_Source.Text = "源语言";
            // 
            // btn_Reset_Bing_Source
            // 
            this.btn_Reset_Bing_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Bing_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Bing_Source.Name = "btn_Reset_Bing_Source";
            this.btn_Reset_Bing_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Bing_Source.TabIndex = 1;
            this.btn_Reset_Bing_Source.Text = "重置";
            this.btn_Reset_Bing_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Bing_Source
            // 
            this.textBox_Bing_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Bing_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Bing_Source.Name = "textBox_Bing_Source";
            this.textBox_Bing_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Bing_Source.TabIndex = 0;
            // 
            // tabPage_Bing2
            // 
            this.tabPage_Bing2.Controls.Add(this.groupBox_Bing2_Key);
            this.tabPage_Bing2.Controls.Add(this.groupBox_Bing2_Target);
            this.tabPage_Bing2.Controls.Add(this.groupBox_Bing2_Source);
            this.tabPage_Bing2.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Bing2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Bing2.Name = "tabPage_Bing2";
            this.tabPage_Bing2.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Bing2.TabIndex = 6;
            this.tabPage_Bing2.Text = "Bing2";
            this.tabPage_Bing2.UseVisualStyleBackColor = true;
            // 
            // groupBox_Bing2_Key
            // 
            this.groupBox_Bing2_Key.Controls.Add(this.label_Bing2_Notice);
            this.groupBox_Bing2_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Bing2_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing2_Key.Name = "groupBox_Bing2_Key";
            this.groupBox_Bing2_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing2_Key.Size = new System.Drawing.Size(493, 100);
            this.groupBox_Bing2_Key.TabIndex = 5;
            this.groupBox_Bing2_Key.TabStop = false;
            this.groupBox_Bing2_Key.Text = "说明";
            // 
            // label_Bing2_Notice
            // 
            this.label_Bing2_Notice.AutoSize = true;
            this.label_Bing2_Notice.Location = new System.Drawing.Point(8, 25);
            this.label_Bing2_Notice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Bing2_Notice.Name = "label_Bing2_Notice";
            this.label_Bing2_Notice.Size = new System.Drawing.Size(267, 45);
            this.label_Bing2_Notice.TabIndex = 0;
            this.label_Bing2_Notice.Text = "Bing2 使用 Microsoft Edge 翻译API\r\n无需密钥即可使用\r\n支持自动检测语言";
            // 
            // groupBox_Bing2_Target
            // 
            this.groupBox_Bing2_Target.Controls.Add(this.btn_Reset_Bing2_Target);
            this.groupBox_Bing2_Target.Controls.Add(this.textBox_Bing2_Target);
            this.groupBox_Bing2_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Bing2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing2_Target.Name = "groupBox_Bing2_Target";
            this.groupBox_Bing2_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing2_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Bing2_Target.TabIndex = 4;
            this.groupBox_Bing2_Target.TabStop = false;
            this.groupBox_Bing2_Target.Text = "目标语言";
            // 
            // btn_Reset_Bing2_Target
            // 
            this.btn_Reset_Bing2_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Bing2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Bing2_Target.Name = "btn_Reset_Bing2_Target";
            this.btn_Reset_Bing2_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Bing2_Target.TabIndex = 1;
            this.btn_Reset_Bing2_Target.Text = "重置";
            this.btn_Reset_Bing2_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing2_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Bing2_Target
            // 
            this.textBox_Bing2_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Bing2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Bing2_Target.Name = "textBox_Bing2_Target";
            this.textBox_Bing2_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Bing2_Target.TabIndex = 0;
            // 
            // groupBox_Bing2_Source
            // 
            this.groupBox_Bing2_Source.Controls.Add(this.btn_Reset_Bing2_Source);
            this.groupBox_Bing2_Source.Controls.Add(this.textBox_Bing2_Source);
            this.groupBox_Bing2_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Bing2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing2_Source.Name = "groupBox_Bing2_Source";
            this.groupBox_Bing2_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Bing2_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Bing2_Source.TabIndex = 3;
            this.groupBox_Bing2_Source.TabStop = false;
            this.groupBox_Bing2_Source.Text = "源语言";
            // 
            // btn_Reset_Bing2_Source
            // 
            this.btn_Reset_Bing2_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Bing2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Bing2_Source.Name = "btn_Reset_Bing2_Source";
            this.btn_Reset_Bing2_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Bing2_Source.TabIndex = 1;
            this.btn_Reset_Bing2_Source.Text = "重置";
            this.btn_Reset_Bing2_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing2_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Bing2_Source
            // 
            this.textBox_Bing2_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Bing2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Bing2_Source.Name = "textBox_Bing2_Source";
            this.textBox_Bing2_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Bing2_Source.TabIndex = 0;
            // 
            // tabPage_Microsoft
            // 
            this.tabPage_Microsoft.Controls.Add(this.groupBox_Microsoft_Key);
            this.tabPage_Microsoft.Controls.Add(this.groupBox_Microsoft_Target);
            this.tabPage_Microsoft.Controls.Add(this.groupBox_Microsoft_Source);
            this.tabPage_Microsoft.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Microsoft.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Microsoft.Name = "tabPage_Microsoft";
            this.tabPage_Microsoft.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Microsoft.TabIndex = 4;
            this.tabPage_Microsoft.Text = "Microsoft";
            this.tabPage_Microsoft.UseVisualStyleBackColor = true;
            // 
            // groupBox_Microsoft_Key
            // 
            this.groupBox_Microsoft_Key.Controls.Add(this.label_Microsoft_Key);
            this.groupBox_Microsoft_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Microsoft_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Microsoft_Key.Name = "groupBox_Microsoft_Key";
            this.groupBox_Microsoft_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Microsoft_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Microsoft_Key.TabIndex = 2;
            this.groupBox_Microsoft_Key.TabStop = false;
            this.groupBox_Microsoft_Key.Text = "密钥";
            // 
            // label_Microsoft_Key
            // 
            this.label_Microsoft_Key.AutoSize = true;
            this.label_Microsoft_Key.Location = new System.Drawing.Point(200, 31);
            this.label_Microsoft_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Microsoft_Key.Name = "label_Microsoft_Key";
            this.label_Microsoft_Key.Size = new System.Drawing.Size(67, 15);
            this.label_Microsoft_Key.TabIndex = 0;
            this.label_Microsoft_Key.Text = "无需密钥";
            // 
            // groupBox_Microsoft_Target
            // 
            this.groupBox_Microsoft_Target.Controls.Add(this.btn_Reset_Microsoft_Target);
            this.groupBox_Microsoft_Target.Controls.Add(this.textBox_Microsoft_Target);
            this.groupBox_Microsoft_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Microsoft_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Microsoft_Target.Name = "groupBox_Microsoft_Target";
            this.groupBox_Microsoft_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Microsoft_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Microsoft_Target.TabIndex = 1;
            this.groupBox_Microsoft_Target.TabStop = false;
            this.groupBox_Microsoft_Target.Text = "目标语言";
            // 
            // btn_Reset_Microsoft_Target
            // 
            this.btn_Reset_Microsoft_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Microsoft_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Microsoft_Target.Name = "btn_Reset_Microsoft_Target";
            this.btn_Reset_Microsoft_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Microsoft_Target.TabIndex = 1;
            this.btn_Reset_Microsoft_Target.Text = "重置";
            this.btn_Reset_Microsoft_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Microsoft_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Microsoft_Target
            // 
            this.textBox_Microsoft_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Microsoft_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Microsoft_Target.Name = "textBox_Microsoft_Target";
            this.textBox_Microsoft_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Microsoft_Target.TabIndex = 0;
            // 
            // groupBox_Microsoft_Source
            // 
            this.groupBox_Microsoft_Source.Controls.Add(this.btn_Reset_Microsoft_Source);
            this.groupBox_Microsoft_Source.Controls.Add(this.textBox_Microsoft_Source);
            this.groupBox_Microsoft_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Microsoft_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Microsoft_Source.Name = "groupBox_Microsoft_Source";
            this.groupBox_Microsoft_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Microsoft_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Microsoft_Source.TabIndex = 0;
            this.groupBox_Microsoft_Source.TabStop = false;
            this.groupBox_Microsoft_Source.Text = "源语言";
            // 
            // btn_Reset_Microsoft_Source
            // 
            this.btn_Reset_Microsoft_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Microsoft_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Microsoft_Source.Name = "btn_Reset_Microsoft_Source";
            this.btn_Reset_Microsoft_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Microsoft_Source.TabIndex = 1;
            this.btn_Reset_Microsoft_Source.Text = "重置";
            this.btn_Reset_Microsoft_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Microsoft_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Microsoft_Source
            // 
            this.textBox_Microsoft_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Microsoft_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Microsoft_Source.Name = "textBox_Microsoft_Source";
            this.textBox_Microsoft_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Microsoft_Source.TabIndex = 0;
            // 
            // tabPage_Yandex
            // 
            this.tabPage_Yandex.Controls.Add(this.groupBox_Yandex_Key);
            this.tabPage_Yandex.Controls.Add(this.groupBox_Yandex_Target);
            this.tabPage_Yandex.Controls.Add(this.groupBox_Yandex_Source);
            this.tabPage_Yandex.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Yandex.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Yandex.Name = "tabPage_Yandex";
            this.tabPage_Yandex.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Yandex.TabIndex = 5;
            this.tabPage_Yandex.Text = "Yandex";
            this.tabPage_Yandex.UseVisualStyleBackColor = true;
            // 
            // groupBox_Yandex_Key
            // 
            this.groupBox_Yandex_Key.Controls.Add(this.label_Yandex_Key);
            this.groupBox_Yandex_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Yandex_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Yandex_Key.Name = "groupBox_Yandex_Key";
            this.groupBox_Yandex_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Yandex_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Yandex_Key.TabIndex = 2;
            this.groupBox_Yandex_Key.TabStop = false;
            this.groupBox_Yandex_Key.Text = "密钥";
            // 
            // label_Yandex_Key
            // 
            this.label_Yandex_Key.AutoSize = true;
            this.label_Yandex_Key.Location = new System.Drawing.Point(200, 31);
            this.label_Yandex_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Yandex_Key.Name = "label_Yandex_Key";
            this.label_Yandex_Key.Size = new System.Drawing.Size(67, 15);
            this.label_Yandex_Key.TabIndex = 0;
            this.label_Yandex_Key.Text = "无需密钥";
            // 
            // groupBox_Yandex_Target
            // 
            this.groupBox_Yandex_Target.Controls.Add(this.btn_Reset_Yandex_Target);
            this.groupBox_Yandex_Target.Controls.Add(this.textBox_Yandex_Target);
            this.groupBox_Yandex_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Yandex_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Yandex_Target.Name = "groupBox_Yandex_Target";
            this.groupBox_Yandex_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Yandex_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Yandex_Target.TabIndex = 1;
            this.groupBox_Yandex_Target.TabStop = false;
            this.groupBox_Yandex_Target.Text = "目标语言";
            // 
            // btn_Reset_Yandex_Target
            // 
            this.btn_Reset_Yandex_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Yandex_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Yandex_Target.Name = "btn_Reset_Yandex_Target";
            this.btn_Reset_Yandex_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Yandex_Target.TabIndex = 1;
            this.btn_Reset_Yandex_Target.Text = "重置";
            this.btn_Reset_Yandex_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Yandex_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Yandex_Target
            // 
            this.textBox_Yandex_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Yandex_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Yandex_Target.Name = "textBox_Yandex_Target";
            this.textBox_Yandex_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Yandex_Target.TabIndex = 0;
            // 
            // groupBox_Yandex_Source
            // 
            this.groupBox_Yandex_Source.Controls.Add(this.btn_Reset_Yandex_Source);
            this.groupBox_Yandex_Source.Controls.Add(this.textBox_Yandex_Source);
            this.groupBox_Yandex_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Yandex_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Yandex_Source.Name = "groupBox_Yandex_Source";
            this.groupBox_Yandex_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Yandex_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Yandex_Source.TabIndex = 0;
            this.groupBox_Yandex_Source.TabStop = false;
            this.groupBox_Yandex_Source.Text = "源语言";
            // 
            // btn_Reset_Yandex_Source
            // 
            this.btn_Reset_Yandex_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Yandex_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Yandex_Source.Name = "btn_Reset_Yandex_Source";
            this.btn_Reset_Yandex_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Yandex_Source.TabIndex = 1;
            this.btn_Reset_Yandex_Source.Text = "重置";
            this.btn_Reset_Yandex_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Yandex_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Yandex_Source
            // 
            this.textBox_Yandex_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Yandex_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Yandex_Source.Name = "textBox_Yandex_Source";
            this.textBox_Yandex_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Yandex_Source.TabIndex = 0;
            // 
            // tabPage_TencentInteractive
            // 
            this.tabPage_TencentInteractive.Controls.Add(this.groupBox_TencentInteractive_Key);
            this.tabPage_TencentInteractive.Controls.Add(this.groupBox_TencentInteractive_Target);
            this.tabPage_TencentInteractive.Controls.Add(this.groupBox_TencentInteractive_Source);
            this.tabPage_TencentInteractive.Location = new System.Drawing.Point(4, 25);
            this.tabPage_TencentInteractive.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_TencentInteractive.Name = "tabPage_TencentInteractive";
            this.tabPage_TencentInteractive.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage_TencentInteractive.Size = new System.Drawing.Size(515, 486);
            this.tabPage_TencentInteractive.TabIndex = 7;
            this.tabPage_TencentInteractive.Text = "腾讯交互";
            this.tabPage_TencentInteractive.UseVisualStyleBackColor = true;
            // 
            // groupBox_TencentInteractive_Key
            // 
            this.groupBox_TencentInteractive_Key.Controls.Add(this.label_TencentInteractive_Key);
            this.groupBox_TencentInteractive_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_TencentInteractive_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_TencentInteractive_Key.Name = "groupBox_TencentInteractive_Key";
            this.groupBox_TencentInteractive_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_TencentInteractive_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_TencentInteractive_Key.TabIndex = 2;
            this.groupBox_TencentInteractive_Key.TabStop = false;
            this.groupBox_TencentInteractive_Key.Text = "密钥";
            // 
            // label_TencentInteractive_Key
            // 
            this.label_TencentInteractive_Key.AutoSize = true;
            this.label_TencentInteractive_Key.Location = new System.Drawing.Point(200, 31);
            this.label_TencentInteractive_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_TencentInteractive_Key.Name = "label_TencentInteractive_Key";
            this.label_TencentInteractive_Key.Size = new System.Drawing.Size(67, 15);
            this.label_TencentInteractive_Key.TabIndex = 0;
            this.label_TencentInteractive_Key.Text = "无需密钥";
            // 
            // groupBox_TencentInteractive_Target
            // 
            this.groupBox_TencentInteractive_Target.Controls.Add(this.btn_Reset_TencentInteractive_Target);
            this.groupBox_TencentInteractive_Target.Controls.Add(this.textBox_TencentInteractive_Target);
            this.groupBox_TencentInteractive_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_TencentInteractive_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_TencentInteractive_Target.Name = "groupBox_TencentInteractive_Target";
            this.groupBox_TencentInteractive_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_TencentInteractive_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_TencentInteractive_Target.TabIndex = 1;
            this.groupBox_TencentInteractive_Target.TabStop = false;
            this.groupBox_TencentInteractive_Target.Text = "目标语言";
            // 
            // btn_Reset_TencentInteractive_Target
            // 
            this.btn_Reset_TencentInteractive_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_TencentInteractive_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_TencentInteractive_Target.Name = "btn_Reset_TencentInteractive_Target";
            this.btn_Reset_TencentInteractive_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_TencentInteractive_Target.TabIndex = 1;
            this.btn_Reset_TencentInteractive_Target.Text = "重置";
            this.btn_Reset_TencentInteractive_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_TencentInteractive_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_TencentInteractive_Target
            // 
            this.textBox_TencentInteractive_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_TencentInteractive_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_TencentInteractive_Target.Name = "textBox_TencentInteractive_Target";
            this.textBox_TencentInteractive_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_TencentInteractive_Target.TabIndex = 0;
            // 
            // groupBox_TencentInteractive_Source
            // 
            this.groupBox_TencentInteractive_Source.Controls.Add(this.btn_Reset_TencentInteractive_Source);
            this.groupBox_TencentInteractive_Source.Controls.Add(this.textBox_TencentInteractive_Source);
            this.groupBox_TencentInteractive_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_TencentInteractive_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_TencentInteractive_Source.Name = "groupBox_TencentInteractive_Source";
            this.groupBox_TencentInteractive_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_TencentInteractive_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_TencentInteractive_Source.TabIndex = 0;
            this.groupBox_TencentInteractive_Source.TabStop = false;
            this.groupBox_TencentInteractive_Source.Text = "源语言";
            // 
            // btn_Reset_TencentInteractive_Source
            // 
            this.btn_Reset_TencentInteractive_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_TencentInteractive_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_TencentInteractive_Source.Name = "btn_Reset_TencentInteractive_Source";
            this.btn_Reset_TencentInteractive_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_TencentInteractive_Source.TabIndex = 1;
            this.btn_Reset_TencentInteractive_Source.Text = "重置";
            this.btn_Reset_TencentInteractive_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_TencentInteractive_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_TencentInteractive_Source
            // 
            this.textBox_TencentInteractive_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_TencentInteractive_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_TencentInteractive_Source.Name = "textBox_TencentInteractive_Source";
            this.textBox_TencentInteractive_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_TencentInteractive_Source.TabIndex = 0;
            // 
            // tabPage_Caiyun
            // 
            this.tabPage_Caiyun.Controls.Add(this.groupBox_Caiyun_Key);
            this.tabPage_Caiyun.Controls.Add(this.groupBox_Caiyun_Target);
            this.tabPage_Caiyun.Controls.Add(this.groupBox_Caiyun_Source);
            this.tabPage_Caiyun.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Caiyun.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Caiyun.Name = "tabPage_Caiyun";
            this.tabPage_Caiyun.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage_Caiyun.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Caiyun.TabIndex = 8;
            this.tabPage_Caiyun.Text = "彩云小译";
            this.tabPage_Caiyun.UseVisualStyleBackColor = true;
            // 
            // groupBox_Caiyun_Key
            // 
            this.groupBox_Caiyun_Key.Controls.Add(this.label_Caiyun_Key);
            this.groupBox_Caiyun_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Caiyun_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun_Key.Name = "groupBox_Caiyun_Key";
            this.groupBox_Caiyun_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Caiyun_Key.TabIndex = 2;
            this.groupBox_Caiyun_Key.TabStop = false;
            this.groupBox_Caiyun_Key.Text = "密钥";
            // 
            // label_Caiyun_Key
            // 
            this.label_Caiyun_Key.AutoSize = true;
            this.label_Caiyun_Key.Location = new System.Drawing.Point(200, 31);
            this.label_Caiyun_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Caiyun_Key.Name = "label_Caiyun_Key";
            this.label_Caiyun_Key.Size = new System.Drawing.Size(67, 15);
            this.label_Caiyun_Key.TabIndex = 0;
            this.label_Caiyun_Key.Text = "无需密钥";
            // 
            // groupBox_Caiyun_Target
            // 
            this.groupBox_Caiyun_Target.Controls.Add(this.btn_Reset_Caiyun_Target);
            this.groupBox_Caiyun_Target.Controls.Add(this.textBox_Caiyun_Target);
            this.groupBox_Caiyun_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Caiyun_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun_Target.Name = "groupBox_Caiyun_Target";
            this.groupBox_Caiyun_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Caiyun_Target.TabIndex = 1;
            this.groupBox_Caiyun_Target.TabStop = false;
            this.groupBox_Caiyun_Target.Text = "目标语言";
            // 
            // btn_Reset_Caiyun_Target
            // 
            this.btn_Reset_Caiyun_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Caiyun_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Caiyun_Target.Name = "btn_Reset_Caiyun_Target";
            this.btn_Reset_Caiyun_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Caiyun_Target.TabIndex = 1;
            this.btn_Reset_Caiyun_Target.Text = "重置";
            this.btn_Reset_Caiyun_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Caiyun_Target
            // 
            this.textBox_Caiyun_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Caiyun_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Caiyun_Target.Name = "textBox_Caiyun_Target";
            this.textBox_Caiyun_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Caiyun_Target.TabIndex = 0;
            // 
            // groupBox_Caiyun_Source
            // 
            this.groupBox_Caiyun_Source.Controls.Add(this.btn_Reset_Caiyun_Source);
            this.groupBox_Caiyun_Source.Controls.Add(this.textBox_Caiyun_Source);
            this.groupBox_Caiyun_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Caiyun_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun_Source.Name = "groupBox_Caiyun_Source";
            this.groupBox_Caiyun_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Caiyun_Source.TabIndex = 0;
            this.groupBox_Caiyun_Source.TabStop = false;
            this.groupBox_Caiyun_Source.Text = "源语言";
            // 
            // btn_Reset_Caiyun_Source
            // 
            this.btn_Reset_Caiyun_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Caiyun_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Caiyun_Source.Name = "btn_Reset_Caiyun_Source";
            this.btn_Reset_Caiyun_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Caiyun_Source.TabIndex = 1;
            this.btn_Reset_Caiyun_Source.Text = "重置";
            this.btn_Reset_Caiyun_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Caiyun_Source
            // 
            this.textBox_Caiyun_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Caiyun_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Caiyun_Source.Name = "textBox_Caiyun_Source";
            this.textBox_Caiyun_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Caiyun_Source.TabIndex = 0;
            // 
            // tabPage_Volcano
            // 
            this.tabPage_Volcano.Controls.Add(this.groupBox_Volcano_Key);
            this.tabPage_Volcano.Controls.Add(this.groupBox_Volcano_Target);
            this.tabPage_Volcano.Controls.Add(this.groupBox_Volcano_Source);
            this.tabPage_Volcano.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Volcano.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Volcano.Name = "tabPage_Volcano";
            this.tabPage_Volcano.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage_Volcano.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Volcano.TabIndex = 9;
            this.tabPage_Volcano.Text = "火山翻译";
            this.tabPage_Volcano.UseVisualStyleBackColor = true;
            // 
            // groupBox_Volcano_Key
            // 
            this.groupBox_Volcano_Key.Controls.Add(this.label_Volcano_Key);
            this.groupBox_Volcano_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Volcano_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Volcano_Key.Name = "groupBox_Volcano_Key";
            this.groupBox_Volcano_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Volcano_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Volcano_Key.TabIndex = 2;
            this.groupBox_Volcano_Key.TabStop = false;
            this.groupBox_Volcano_Key.Text = "密钥";
            // 
            // label_Volcano_Key
            // 
            this.label_Volcano_Key.AutoSize = true;
            this.label_Volcano_Key.Location = new System.Drawing.Point(200, 31);
            this.label_Volcano_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Volcano_Key.Name = "label_Volcano_Key";
            this.label_Volcano_Key.Size = new System.Drawing.Size(67, 15);
            this.label_Volcano_Key.TabIndex = 0;
            this.label_Volcano_Key.Text = "无需密钥";
            // 
            // groupBox_Volcano_Target
            // 
            this.groupBox_Volcano_Target.Controls.Add(this.btn_Reset_Volcano_Target);
            this.groupBox_Volcano_Target.Controls.Add(this.textBox_Volcano_Target);
            this.groupBox_Volcano_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Volcano_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Volcano_Target.Name = "groupBox_Volcano_Target";
            this.groupBox_Volcano_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Volcano_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Volcano_Target.TabIndex = 1;
            this.groupBox_Volcano_Target.TabStop = false;
            this.groupBox_Volcano_Target.Text = "目标语言";
            // 
            // btn_Reset_Volcano_Target
            // 
            this.btn_Reset_Volcano_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Volcano_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Volcano_Target.Name = "btn_Reset_Volcano_Target";
            this.btn_Reset_Volcano_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Volcano_Target.TabIndex = 1;
            this.btn_Reset_Volcano_Target.Text = "重置";
            this.btn_Reset_Volcano_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Volcano_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Volcano_Target
            // 
            this.textBox_Volcano_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Volcano_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Volcano_Target.Name = "textBox_Volcano_Target";
            this.textBox_Volcano_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Volcano_Target.TabIndex = 0;
            // 
            // groupBox_Volcano_Source
            // 
            this.groupBox_Volcano_Source.Controls.Add(this.btn_Reset_Volcano_Source);
            this.groupBox_Volcano_Source.Controls.Add(this.textBox_Volcano_Source);
            this.groupBox_Volcano_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Volcano_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Volcano_Source.Name = "groupBox_Volcano_Source";
            this.groupBox_Volcano_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Volcano_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Volcano_Source.TabIndex = 0;
            this.groupBox_Volcano_Source.TabStop = false;
            this.groupBox_Volcano_Source.Text = "源语言";
            // 
            // btn_Reset_Volcano_Source
            // 
            this.btn_Reset_Volcano_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Volcano_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Volcano_Source.Name = "btn_Reset_Volcano_Source";
            this.btn_Reset_Volcano_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Volcano_Source.TabIndex = 1;
            this.btn_Reset_Volcano_Source.Text = "重置";
            this.btn_Reset_Volcano_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Volcano_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Volcano_Source
            // 
            this.textBox_Volcano_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Volcano_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Volcano_Source.Name = "textBox_Volcano_Source";
            this.textBox_Volcano_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Volcano_Source.TabIndex = 0;
            // 
            // tabPage_Caiyun2
            // 
            this.tabPage_Caiyun2.Controls.Add(this.groupBox_Caiyun2_Key);
            this.tabPage_Caiyun2.Controls.Add(this.groupBox_Caiyun2_Target);
            this.tabPage_Caiyun2.Controls.Add(this.groupBox_Caiyun2_Source);
            this.tabPage_Caiyun2.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Caiyun2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Caiyun2.Name = "tabPage_Caiyun2";
            this.tabPage_Caiyun2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage_Caiyun2.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Caiyun2.TabIndex = 10;
            this.tabPage_Caiyun2.Text = "彩云小译2";
            this.tabPage_Caiyun2.UseVisualStyleBackColor = true;
            // 
            // groupBox_Caiyun2_Key
            // 
            this.groupBox_Caiyun2_Key.Controls.Add(this.textBox_Caiyun2_Token);
            this.groupBox_Caiyun2_Key.Controls.Add(this.label_Caiyun2_Token);
            this.groupBox_Caiyun2_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Caiyun2_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun2_Key.Name = "groupBox_Caiyun2_Key";
            this.groupBox_Caiyun2_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun2_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Caiyun2_Key.TabIndex = 2;
            this.groupBox_Caiyun2_Key.TabStop = false;
            this.groupBox_Caiyun2_Key.Text = "密钥";
            // 
            // textBox_Caiyun2_Token
            // 
            this.textBox_Caiyun2_Token.Location = new System.Drawing.Point(107, 25);
            this.textBox_Caiyun2_Token.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Caiyun2_Token.Name = "textBox_Caiyun2_Token";
            this.textBox_Caiyun2_Token.Size = new System.Drawing.Size(377, 25);
            this.textBox_Caiyun2_Token.TabIndex = 1;
            // 
            // label_Caiyun2_Token
            // 
            this.label_Caiyun2_Token.AutoSize = true;
            this.label_Caiyun2_Token.Location = new System.Drawing.Point(9, 29);
            this.label_Caiyun2_Token.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Caiyun2_Token.Name = "label_Caiyun2_Token";
            this.label_Caiyun2_Token.Size = new System.Drawing.Size(55, 15);
            this.label_Caiyun2_Token.TabIndex = 0;
            this.label_Caiyun2_Token.Text = "Token:";
            // 
            // groupBox_Caiyun2_Target
            // 
            this.groupBox_Caiyun2_Target.Controls.Add(this.btn_Reset_Caiyun2_Target);
            this.groupBox_Caiyun2_Target.Controls.Add(this.textBox_Caiyun2_Target);
            this.groupBox_Caiyun2_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Caiyun2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun2_Target.Name = "groupBox_Caiyun2_Target";
            this.groupBox_Caiyun2_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun2_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Caiyun2_Target.TabIndex = 1;
            this.groupBox_Caiyun2_Target.TabStop = false;
            this.groupBox_Caiyun2_Target.Text = "目标语言";
            // 
            // btn_Reset_Caiyun2_Target
            // 
            this.btn_Reset_Caiyun2_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Caiyun2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Caiyun2_Target.Name = "btn_Reset_Caiyun2_Target";
            this.btn_Reset_Caiyun2_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Caiyun2_Target.TabIndex = 1;
            this.btn_Reset_Caiyun2_Target.Text = "重置";
            this.btn_Reset_Caiyun2_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun2_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Caiyun2_Target
            // 
            this.textBox_Caiyun2_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Caiyun2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Caiyun2_Target.Name = "textBox_Caiyun2_Target";
            this.textBox_Caiyun2_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Caiyun2_Target.TabIndex = 0;
            // 
            // groupBox_Caiyun2_Source
            // 
            this.groupBox_Caiyun2_Source.Controls.Add(this.btn_Reset_Caiyun2_Source);
            this.groupBox_Caiyun2_Source.Controls.Add(this.textBox_Caiyun2_Source);
            this.groupBox_Caiyun2_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Caiyun2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun2_Source.Name = "groupBox_Caiyun2_Source";
            this.groupBox_Caiyun2_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Caiyun2_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Caiyun2_Source.TabIndex = 0;
            this.groupBox_Caiyun2_Source.TabStop = false;
            this.groupBox_Caiyun2_Source.Text = "源语言";
            // 
            // btn_Reset_Caiyun2_Source
            // 
            this.btn_Reset_Caiyun2_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Caiyun2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Caiyun2_Source.Name = "btn_Reset_Caiyun2_Source";
            this.btn_Reset_Caiyun2_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Caiyun2_Source.TabIndex = 1;
            this.btn_Reset_Caiyun2_Source.Text = "重置";
            this.btn_Reset_Caiyun2_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun2_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Caiyun2_Source
            // 
            this.textBox_Caiyun2_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Caiyun2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Caiyun2_Source.Name = "textBox_Caiyun2_Source";
            this.textBox_Caiyun2_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Caiyun2_Source.TabIndex = 0;
            // 
            // tabPage_Baidu2
            // 
            this.tabPage_Baidu2.Controls.Add(this.groupBox_Baidu2_Key);
            this.tabPage_Baidu2.Controls.Add(this.groupBox_Baidu2_Target);
            this.tabPage_Baidu2.Controls.Add(this.groupBox_Baidu2_Source);
            this.tabPage_Baidu2.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Baidu2.Name = "tabPage_Baidu2";
            this.tabPage_Baidu2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Baidu2.Size = new System.Drawing.Size(515, 486);
            this.tabPage_Baidu2.TabIndex = 11;
            this.tabPage_Baidu2.Text = "Baidu2";
            this.tabPage_Baidu2.UseVisualStyleBackColor = true;
            // 
            // groupBox_Baidu2_Key
            // 
            this.groupBox_Baidu2_Key.Controls.Add(this.label40);
            this.groupBox_Baidu2_Key.Location = new System.Drawing.Point(8, 160);
            this.groupBox_Baidu2_Key.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu2_Key.Name = "groupBox_Baidu2_Key";
            this.groupBox_Baidu2_Key.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu2_Key.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Baidu2_Key.TabIndex = 3;
            this.groupBox_Baidu2_Key.TabStop = false;
            this.groupBox_Baidu2_Key.Text = "密钥";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(111, 32);
            this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(232, 15);
            this.label40.TabIndex = 0;
            this.label40.Text = "无需密钥，此接口已失效，勿使用";
            // 
            // groupBox_Baidu2_Target
            // 
            this.groupBox_Baidu2_Target.Controls.Add(this.btn_Reset_Baidu2_Target);
            this.groupBox_Baidu2_Target.Controls.Add(this.textBox_Baidu2_Target);
            this.groupBox_Baidu2_Target.Location = new System.Drawing.Point(8, 84);
            this.groupBox_Baidu2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu2_Target.Name = "groupBox_Baidu2_Target";
            this.groupBox_Baidu2_Target.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu2_Target.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Baidu2_Target.TabIndex = 2;
            this.groupBox_Baidu2_Target.TabStop = false;
            this.groupBox_Baidu2_Target.Text = "目标语言";
            // 
            // btn_Reset_Baidu2_Target
            // 
            this.btn_Reset_Baidu2_Target.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Baidu2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Baidu2_Target.Name = "btn_Reset_Baidu2_Target";
            this.btn_Reset_Baidu2_Target.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Baidu2_Target.TabIndex = 1;
            this.btn_Reset_Baidu2_Target.Text = "重置";
            this.btn_Reset_Baidu2_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Baidu2_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            // 
            // textBox_Baidu2_Target
            // 
            this.textBox_Baidu2_Target.Location = new System.Drawing.Point(8, 25);
            this.textBox_Baidu2_Target.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Baidu2_Target.Name = "textBox_Baidu2_Target";
            this.textBox_Baidu2_Target.Size = new System.Drawing.Size(401, 25);
            this.textBox_Baidu2_Target.TabIndex = 0;
            // 
            // groupBox_Baidu2_Source
            // 
            this.groupBox_Baidu2_Source.Controls.Add(this.btn_Reset_Baidu2_Source);
            this.groupBox_Baidu2_Source.Controls.Add(this.textBox_Baidu2_Source);
            this.groupBox_Baidu2_Source.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Baidu2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu2_Source.Name = "groupBox_Baidu2_Source";
            this.groupBox_Baidu2_Source.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Baidu2_Source.Size = new System.Drawing.Size(493, 69);
            this.groupBox_Baidu2_Source.TabIndex = 1;
            this.groupBox_Baidu2_Source.TabStop = false;
            this.groupBox_Baidu2_Source.Text = "源语言";
            // 
            // btn_Reset_Baidu2_Source
            // 
            this.btn_Reset_Baidu2_Source.Location = new System.Drawing.Point(419, 24);
            this.btn_Reset_Baidu2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reset_Baidu2_Source.Name = "btn_Reset_Baidu2_Source";
            this.btn_Reset_Baidu2_Source.Size = new System.Drawing.Size(67, 29);
            this.btn_Reset_Baidu2_Source.TabIndex = 1;
            this.btn_Reset_Baidu2_Source.Text = "重置";
            this.btn_Reset_Baidu2_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Baidu2_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            // 
            // textBox_Baidu2_Source
            // 
            this.textBox_Baidu2_Source.Location = new System.Drawing.Point(8, 25);
            this.textBox_Baidu2_Source.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Baidu2_Source.Name = "textBox_Baidu2_Source";
            this.textBox_Baidu2_Source.Size = new System.Drawing.Size(401, 25);
            this.textBox_Baidu2_Source.TabIndex = 0;
            // 
            // tabPage_白描接口
            // 
            this.tabPage_白描接口.BackColor = System.Drawing.Color.White;
            this.tabPage_白描接口.Controls.Add(this.BoxBaimiaoPassword);
            this.tabPage_白描接口.Controls.Add(this.BoxBaimiaoUsername);
            this.tabPage_白描接口.Controls.Add(this.label_BaimiaoPassword);
            this.tabPage_白描接口.Controls.Add(this.label_BaimiaoUsername);
            this.tabPage_白描接口.Location = new System.Drawing.Point(4, 25);
            this.tabPage_白描接口.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_白描接口.Name = "tabPage_白描接口";
            this.tabPage_白描接口.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage_白描接口.Size = new System.Drawing.Size(496, 171);
            this.tabPage_白描接口.TabIndex = 2;
            this.tabPage_白描接口.Text = "白描识别接口";
            // 
            // BoxBaimiaoPassword
            // 
            this.BoxBaimiaoPassword.BackColor = System.Drawing.Color.White;
            this.BoxBaimiaoPassword.Location = new System.Drawing.Point(93, 69);
            this.BoxBaimiaoPassword.Margin = new System.Windows.Forms.Padding(4);
            this.BoxBaimiaoPassword.Name = "BoxBaimiaoPassword";
            this.BoxBaimiaoPassword.PasswordChar = '*';
            this.BoxBaimiaoPassword.Size = new System.Drawing.Size(345, 25);
            this.BoxBaimiaoPassword.TabIndex = 8;
            // 
            // BoxBaimiaoUsername
            // 
            this.BoxBaimiaoUsername.BackColor = System.Drawing.Color.White;
            this.BoxBaimiaoUsername.Location = new System.Drawing.Point(93, 25);
            this.BoxBaimiaoUsername.Margin = new System.Windows.Forms.Padding(4);
            this.BoxBaimiaoUsername.Name = "BoxBaimiaoUsername";
            this.BoxBaimiaoUsername.Size = new System.Drawing.Size(345, 25);
            this.BoxBaimiaoUsername.TabIndex = 7;
            // 
            // label_BaimiaoPassword
            // 
            this.label_BaimiaoPassword.AutoSize = true;
            this.label_BaimiaoPassword.Location = new System.Drawing.Point(12, 76);
            this.label_BaimiaoPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_BaimiaoPassword.Name = "label_BaimiaoPassword";
            this.label_BaimiaoPassword.Size = new System.Drawing.Size(45, 15);
            this.label_BaimiaoPassword.TabIndex = 6;
            this.label_BaimiaoPassword.Text = "密码:";
            // 
            // label_BaimiaoUsername
            // 
            this.label_BaimiaoUsername.AutoSize = true;
            this.label_BaimiaoUsername.Location = new System.Drawing.Point(12, 32);
            this.label_BaimiaoUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_BaimiaoUsername.Name = "label_BaimiaoUsername";
            this.label_BaimiaoUsername.Size = new System.Drawing.Size(45, 15);
            this.label_BaimiaoUsername.TabIndex = 5;
            this.label_BaimiaoUsername.Text = "账号:";
            // 
            // Page_AI密钥
            // 
            this.Page_AI密钥.BackColor = System.Drawing.Color.White;
            this.Page_AI密钥.Controls.Add(this.groupBox_AI_Help);
            this.Page_AI密钥.Controls.Add(this.tabControl_AI);
            this.Page_AI密钥.Location = new System.Drawing.Point(4, 25);
            this.Page_AI密钥.Name = "Page_AI密钥";
            this.Page_AI密钥.Padding = new System.Windows.Forms.Padding(4);
            this.Page_AI密钥.Size = new System.Drawing.Size(523, 515);
            this.Page_AI密钥.TabIndex = 14;
            this.Page_AI密钥.Text = "AI密钥";
            // 
            // groupBox_AI_Help
            // 
            this.groupBox_AI_Help.AutoSize = true;
            this.groupBox_AI_Help.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_AI_Help.Controls.Add(this.label_AI_Help);
            this.groupBox_AI_Help.Location = new System.Drawing.Point(0, 274);
            this.groupBox_AI_Help.Name = "groupBox_AI_Help";
            this.groupBox_AI_Help.Size = new System.Drawing.Size(369, 264);
            this.groupBox_AI_Help.TabIndex = 1;
            this.groupBox_AI_Help.TabStop = false;
            this.groupBox_AI_Help.Text = "说明";
            // 
            // label_AI_Help
            // 
            this.label_AI_Help.AutoSize = true;
            this.label_AI_Help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_AI_Help.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_AI_Help.Location = new System.Drawing.Point(3, 21);
            this.label_AI_Help.Name = "label_AI_Help";
            this.label_AI_Help.Size = new System.Drawing.Size(363, 240);
            this.label_AI_Help.TabIndex = 0;
            this.label_AI_Help.Text = "模式文件允许用户自定义提示词和请求参数。\r\n若不选择模式文件，将使用程序内置提示词和参数\r\n通过模式文件，可以使 AI 接口识别多种格式，比如：\r\n\r\n1. 公式" +
    "识别\r\n2. 表格识别（支持 Markdown 表格、Excel 表格等）\r\n3. 图片翻译\r\n4. 物体识别\r\n5. 总结图片内容\r\n......\r\n\r\n注意" +
    "：具体效果取决于所选模型的能力。";
            // 
            // tabControl_AI
            // 
            this.tabControl_AI.Controls.Add(this.tabPage_OpenAICompatible);
            this.tabControl_AI.Location = new System.Drawing.Point(4, 4);
            this.tabControl_AI.Name = "tabControl_AI";
            this.tabControl_AI.SelectedIndex = 0;
            this.tabControl_AI.Size = new System.Drawing.Size(515, 264);
            this.tabControl_AI.TabIndex = 0;
            // 
            // tabPage_OpenAICompatible
            // 
            this.tabPage_OpenAICompatible.Controls.Add(this.splitContainer1);
            this.tabPage_OpenAICompatible.Location = new System.Drawing.Point(4, 25);
            this.tabPage_OpenAICompatible.Name = "tabPage_OpenAICompatible";
            this.tabPage_OpenAICompatible.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_OpenAICompatible.Size = new System.Drawing.Size(507, 235);
            this.tabPage_OpenAICompatible.TabIndex = 0;
            this.tabPage_OpenAICompatible.Text = "OpenAICompatible";
            this.tabPage_OpenAICompatible.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.lb_CustomProviders);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox51);
            this.splitContainer1.Size = new System.Drawing.Size(501, 229);
            this.splitContainer1.SplitterDistance = 167;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Del_Provider);
            this.panel1.Controls.Add(this.btn_Add_Provider);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 189);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 40);
            this.panel1.TabIndex = 1;
            // 
            // btn_Del_Provider
            // 
            this.btn_Del_Provider.BackColor = System.Drawing.Color.White;
            this.btn_Del_Provider.Location = new System.Drawing.Point(89, 4);
            this.btn_Del_Provider.Name = "btn_Del_Provider";
            this.btn_Del_Provider.Size = new System.Drawing.Size(75, 23);
            this.btn_Del_Provider.TabIndex = 1;
            this.btn_Del_Provider.Text = "删除接口";
            this.btn_Del_Provider.UseVisualStyleBackColor = false;
            this.btn_Del_Provider.Click += new System.EventHandler(this.btn_Del_Provider_Click);
            // 
            // btn_Add_Provider
            // 
            this.btn_Add_Provider.BackColor = System.Drawing.Color.White;
            this.btn_Add_Provider.Location = new System.Drawing.Point(8, 4);
            this.btn_Add_Provider.Name = "btn_Add_Provider";
            this.btn_Add_Provider.Size = new System.Drawing.Size(75, 23);
            this.btn_Add_Provider.TabIndex = 0;
            this.btn_Add_Provider.Text = "添加接口";
            this.btn_Add_Provider.UseVisualStyleBackColor = false;
            this.btn_Add_Provider.Click += new System.EventHandler(this.btn_Add_Provider_Click);
            // 
            // lb_CustomProviders
            // 
            this.lb_CustomProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CustomProviders.FormattingEnabled = true;
            this.lb_CustomProviders.ItemHeight = 15;
            this.lb_CustomProviders.Location = new System.Drawing.Point(0, 0);
            this.lb_CustomProviders.Name = "lb_CustomProviders";
            this.lb_CustomProviders.Size = new System.Drawing.Size(167, 229);
            this.lb_CustomProviders.TabIndex = 0;
            this.lb_CustomProviders.SelectedIndexChanged += new System.EventHandler(this.Lb_CustomProviders_SelectedIndexChanged);
            // 
            // groupBox51
            // 
            this.groupBox51.Controls.Add(this.txt_ConfigPath);
            this.groupBox51.Controls.Add(this.txt_ModelName);
            this.groupBox51.Controls.Add(this.txt_ApiKey);
            this.groupBox51.Controls.Add(this.txt_ApiUrl);
            this.groupBox51.Controls.Add(this.txt_Name);
            this.groupBox51.Controls.Add(this.btn_BrowseConfig);
            this.groupBox51.Controls.Add(this.label69);
            this.groupBox51.Controls.Add(this.label68);
            this.groupBox51.Controls.Add(this.label67);
            this.groupBox51.Controls.Add(this.label66);
            this.groupBox51.Controls.Add(this.label65);
            this.groupBox51.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox51.Location = new System.Drawing.Point(0, 0);
            this.groupBox51.Name = "groupBox51";
            this.groupBox51.Size = new System.Drawing.Size(330, 229);
            this.groupBox51.TabIndex = 0;
            this.groupBox51.TabStop = false;
            this.groupBox51.Text = "参数设置";
            // 
            // txt_ConfigPath
            // 
            this.txt_ConfigPath.Location = new System.Drawing.Point(131, 161);
            this.txt_ConfigPath.Name = "txt_ConfigPath";
            this.txt_ConfigPath.Size = new System.Drawing.Size(150, 25);
            this.txt_ConfigPath.TabIndex = 9;
            this.txt_ConfigPath.TextChanged += new System.EventHandler(this.txt_ConfigPath_TextChanged);
            // 
            // txt_ModelName
            // 
            this.txt_ModelName.Location = new System.Drawing.Point(131, 129);
            this.txt_ModelName.Name = "txt_ModelName";
            this.txt_ModelName.Size = new System.Drawing.Size(100, 25);
            this.txt_ModelName.TabIndex = 8;
            this.txt_ModelName.TextChanged += new System.EventHandler(this.txt_ModelName_TextChanged);
            // 
            // txt_ApiKey
            // 
            this.txt_ApiKey.Location = new System.Drawing.Point(131, 93);
            this.txt_ApiKey.Name = "txt_ApiKey";
            this.txt_ApiKey.Size = new System.Drawing.Size(170, 25);
            this.txt_ApiKey.TabIndex = 7;
            this.txt_ApiKey.TextChanged += new System.EventHandler(this.txt_ApiKey_TextChanged);
            // 
            // txt_ApiUrl
            // 
            this.txt_ApiUrl.Location = new System.Drawing.Point(131, 60);
            this.txt_ApiUrl.Name = "txt_ApiUrl";
            this.txt_ApiUrl.Size = new System.Drawing.Size(170, 25);
            this.txt_ApiUrl.TabIndex = 6;
            this.txt_ApiUrl.TextChanged += new System.EventHandler(this.txt_ApiUrl_TextChanged);
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(131, 24);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(100, 25);
            this.txt_Name.TabIndex = 5;
            this.txt_Name.TextChanged += new System.EventHandler(this.txt_Name_TextChanged);
            // 
            // btn_BrowseConfig
            // 
            this.btn_BrowseConfig.Location = new System.Drawing.Point(287, 160);
            this.btn_BrowseConfig.Name = "btn_BrowseConfig";
            this.btn_BrowseConfig.Size = new System.Drawing.Size(37, 23);
            this.btn_BrowseConfig.TabIndex = 2;
            this.btn_BrowseConfig.Text = "...";
            this.btn_BrowseConfig.UseVisualStyleBackColor = true;
            this.btn_BrowseConfig.Click += new System.EventHandler(this.btn_BrowseConfig_Click);
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(7, 164);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(97, 15);
            this.label69.TabIndex = 4;
            this.label69.Text = "模式文件路径";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(7, 132);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(67, 15);
            this.label68.TabIndex = 3;
            this.label68.Text = "模型名称";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(7, 93);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(117, 15);
            this.label67.TabIndex = 2;
            this.label67.Text = "API 密钥 (Key)";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(7, 63);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(117, 15);
            this.label66.TabIndex = 1;
            this.label66.Text = "API 地址 (URL)";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(7, 34);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(67, 15);
            this.label65.TabIndex = 0;
            this.label65.Text = "显示名称";
            // 
            // Page_显示的接口
            // 
            this.Page_显示的接口.BackColor = System.Drawing.Color.White;
            this.Page_显示的接口.Controls.Add(this.groupBox_翻译接口);
            this.Page_显示的接口.Controls.Add(this.groupBox_Ocr);
            this.Page_显示的接口.Location = new System.Drawing.Point(4, 25);
            this.Page_显示的接口.Margin = new System.Windows.Forms.Padding(4);
            this.Page_显示的接口.Name = "Page_显示的接口";
            this.Page_显示的接口.Padding = new System.Windows.Forms.Padding(4);
            this.Page_显示的接口.Size = new System.Drawing.Size(523, 515);
            this.Page_显示的接口.TabIndex = 11;
            this.Page_显示的接口.Text = "显示的接口";
            // 
            // groupBox_翻译接口
            // 
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowBaidu2);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowGoogle);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowBaidu);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowTencent);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowBing);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowBing2);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowMicrosoft);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowYandex);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowTencentInteractive);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowCaiyun);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowVolcano);
            this.groupBox_翻译接口.Controls.Add(this.checkBox_ShowCaiyun2);
            this.groupBox_翻译接口.Location = new System.Drawing.Point(8, 252);
            this.groupBox_翻译接口.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_翻译接口.Name = "groupBox_翻译接口";
            this.groupBox_翻译接口.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_翻译接口.Size = new System.Drawing.Size(504, 206);
            this.groupBox_翻译接口.TabIndex = 1;
            this.groupBox_翻译接口.TabStop = false;
            this.groupBox_翻译接口.Text = "选择要显示的翻译接口";
            // 
            // checkBox_ShowBaidu2
            // 
            this.checkBox_ShowBaidu2.AutoSize = true;
            this.checkBox_ShowBaidu2.Location = new System.Drawing.Point(360, 106);
            this.checkBox_ShowBaidu2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowBaidu2.Name = "checkBox_ShowBaidu2";
            this.checkBox_ShowBaidu2.Size = new System.Drawing.Size(97, 19);
            this.checkBox_ShowBaidu2.TabIndex = 11;
            this.checkBox_ShowBaidu2.Text = "百度翻译2";
            this.checkBox_ShowBaidu2.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowGoogle
            // 
            this.checkBox_ShowGoogle.AutoSize = true;
            this.checkBox_ShowGoogle.Location = new System.Drawing.Point(20, 31);
            this.checkBox_ShowGoogle.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowGoogle.Name = "checkBox_ShowGoogle";
            this.checkBox_ShowGoogle.Size = new System.Drawing.Size(77, 19);
            this.checkBox_ShowGoogle.TabIndex = 0;
            this.checkBox_ShowGoogle.Text = "Google";
            this.checkBox_ShowGoogle.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowBaidu
            // 
            this.checkBox_ShowBaidu.AutoSize = true;
            this.checkBox_ShowBaidu.Location = new System.Drawing.Point(133, 31);
            this.checkBox_ShowBaidu.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowBaidu.Name = "checkBox_ShowBaidu";
            this.checkBox_ShowBaidu.Size = new System.Drawing.Size(69, 19);
            this.checkBox_ShowBaidu.TabIndex = 1;
            this.checkBox_ShowBaidu.Text = "Baidu";
            this.checkBox_ShowBaidu.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowTencent
            // 
            this.checkBox_ShowTencent.AutoSize = true;
            this.checkBox_ShowTencent.Location = new System.Drawing.Point(247, 31);
            this.checkBox_ShowTencent.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowTencent.Name = "checkBox_ShowTencent";
            this.checkBox_ShowTencent.Size = new System.Drawing.Size(85, 19);
            this.checkBox_ShowTencent.TabIndex = 2;
            this.checkBox_ShowTencent.Text = "Tencent";
            this.checkBox_ShowTencent.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowBing
            // 
            this.checkBox_ShowBing.AutoSize = true;
            this.checkBox_ShowBing.Location = new System.Drawing.Point(360, 31);
            this.checkBox_ShowBing.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowBing.Name = "checkBox_ShowBing";
            this.checkBox_ShowBing.Size = new System.Drawing.Size(61, 19);
            this.checkBox_ShowBing.TabIndex = 3;
            this.checkBox_ShowBing.Text = "Bing";
            this.checkBox_ShowBing.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowBing2
            // 
            this.checkBox_ShowBing2.AutoSize = true;
            this.checkBox_ShowBing2.Location = new System.Drawing.Point(20, 69);
            this.checkBox_ShowBing2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowBing2.Name = "checkBox_ShowBing2";
            this.checkBox_ShowBing2.Size = new System.Drawing.Size(69, 19);
            this.checkBox_ShowBing2.TabIndex = 4;
            this.checkBox_ShowBing2.Text = "Bing2";
            this.checkBox_ShowBing2.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowMicrosoft
            // 
            this.checkBox_ShowMicrosoft.AutoSize = true;
            this.checkBox_ShowMicrosoft.Location = new System.Drawing.Point(133, 69);
            this.checkBox_ShowMicrosoft.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowMicrosoft.Name = "checkBox_ShowMicrosoft";
            this.checkBox_ShowMicrosoft.Size = new System.Drawing.Size(101, 19);
            this.checkBox_ShowMicrosoft.TabIndex = 5;
            this.checkBox_ShowMicrosoft.Text = "Microsoft";
            this.checkBox_ShowMicrosoft.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowYandex
            // 
            this.checkBox_ShowYandex.AutoSize = true;
            this.checkBox_ShowYandex.Location = new System.Drawing.Point(247, 69);
            this.checkBox_ShowYandex.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowYandex.Name = "checkBox_ShowYandex";
            this.checkBox_ShowYandex.Size = new System.Drawing.Size(77, 19);
            this.checkBox_ShowYandex.TabIndex = 6;
            this.checkBox_ShowYandex.Text = "Yandex";
            this.checkBox_ShowYandex.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowTencentInteractive
            // 
            this.checkBox_ShowTencentInteractive.AutoSize = true;
            this.checkBox_ShowTencentInteractive.Location = new System.Drawing.Point(360, 69);
            this.checkBox_ShowTencentInteractive.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowTencentInteractive.Name = "checkBox_ShowTencentInteractive";
            this.checkBox_ShowTencentInteractive.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowTencentInteractive.TabIndex = 7;
            this.checkBox_ShowTencentInteractive.Text = "腾讯交互";
            this.checkBox_ShowTencentInteractive.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowCaiyun
            // 
            this.checkBox_ShowCaiyun.AutoSize = true;
            this.checkBox_ShowCaiyun.Location = new System.Drawing.Point(20, 106);
            this.checkBox_ShowCaiyun.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowCaiyun.Name = "checkBox_ShowCaiyun";
            this.checkBox_ShowCaiyun.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowCaiyun.TabIndex = 8;
            this.checkBox_ShowCaiyun.Text = "彩云小译";
            this.checkBox_ShowCaiyun.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowVolcano
            // 
            this.checkBox_ShowVolcano.AutoSize = true;
            this.checkBox_ShowVolcano.Location = new System.Drawing.Point(133, 106);
            this.checkBox_ShowVolcano.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowVolcano.Name = "checkBox_ShowVolcano";
            this.checkBox_ShowVolcano.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowVolcano.TabIndex = 9;
            this.checkBox_ShowVolcano.Text = "火山翻译";
            this.checkBox_ShowVolcano.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowCaiyun2
            // 
            this.checkBox_ShowCaiyun2.AutoSize = true;
            this.checkBox_ShowCaiyun2.Location = new System.Drawing.Point(247, 106);
            this.checkBox_ShowCaiyun2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowCaiyun2.Name = "checkBox_ShowCaiyun2";
            this.checkBox_ShowCaiyun2.Size = new System.Drawing.Size(97, 19);
            this.checkBox_ShowCaiyun2.TabIndex = 10;
            this.checkBox_ShowCaiyun2.Text = "彩云小译2";
            this.checkBox_ShowCaiyun2.UseVisualStyleBackColor = true;
            // 
            // groupBox_Ocr
            // 
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrTableTencent);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrPaddleOCR);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrPaddleOCR2);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrRapidOCR);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrBaidu);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrBaiduAccurate);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrTencent);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrTencentAccurate);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrBaimiao);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrSougou);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrYoudao);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrWeChat);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrMathfuntion);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrTable);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrShupai);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrTableBaidu);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrTableAli);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrShupaiLR);
            this.groupBox_Ocr.Controls.Add(this.checkBox_ShowOcrShupaiRL);
            this.groupBox_Ocr.Location = new System.Drawing.Point(8, 8);
            this.groupBox_Ocr.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Ocr.Name = "groupBox_Ocr";
            this.groupBox_Ocr.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Ocr.Size = new System.Drawing.Size(504, 236);
            this.groupBox_Ocr.TabIndex = 0;
            this.groupBox_Ocr.TabStop = false;
            this.groupBox_Ocr.Text = "选择要显示的ocr接口";
            // 
            // checkBox_ShowOcrTableTencent
            // 
            this.checkBox_ShowOcrTableTencent.AutoSize = true;
            this.checkBox_ShowOcrTableTencent.Location = new System.Drawing.Point(243, 144);
            this.checkBox_ShowOcrTableTencent.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrTableTencent.Name = "checkBox_ShowOcrTableTencent";
            this.checkBox_ShowOcrTableTencent.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowOcrTableTencent.TabIndex = 29;
            this.checkBox_ShowOcrTableTencent.Text = "腾讯表格";
            this.checkBox_ShowOcrTableTencent.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrPaddleOCR
            // 
            this.checkBox_ShowOcrPaddleOCR.AutoSize = true;
            this.checkBox_ShowOcrPaddleOCR.Location = new System.Drawing.Point(20, 181);
            this.checkBox_ShowOcrPaddleOCR.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrPaddleOCR.Name = "checkBox_ShowOcrPaddleOCR";
            this.checkBox_ShowOcrPaddleOCR.Size = new System.Drawing.Size(101, 19);
            this.checkBox_ShowOcrPaddleOCR.TabIndex = 28;
            this.checkBox_ShowOcrPaddleOCR.Text = "PaddleOCR";
            this.checkBox_ShowOcrPaddleOCR.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrPaddleOCR2
            // 
            this.checkBox_ShowOcrPaddleOCR2.AutoSize = true;
            this.checkBox_ShowOcrPaddleOCR2.Location = new System.Drawing.Point(133, 181);
            this.checkBox_ShowOcrPaddleOCR2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrPaddleOCR2.Name = "checkBox_ShowOcrPaddleOCR2";
            this.checkBox_ShowOcrPaddleOCR2.Size = new System.Drawing.Size(109, 19);
            this.checkBox_ShowOcrPaddleOCR2.TabIndex = 27;
            this.checkBox_ShowOcrPaddleOCR2.Text = "PaddleOCR2";
            this.checkBox_ShowOcrPaddleOCR2.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrRapidOCR
            // 
            this.checkBox_ShowOcrRapidOCR.AutoSize = true;
            this.checkBox_ShowOcrRapidOCR.Location = new System.Drawing.Point(243, 181);
            this.checkBox_ShowOcrRapidOCR.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrRapidOCR.Name = "checkBox_ShowOcrRapidOCR";
            this.checkBox_ShowOcrRapidOCR.Size = new System.Drawing.Size(93, 19);
            this.checkBox_ShowOcrRapidOCR.TabIndex = 26;
            this.checkBox_ShowOcrRapidOCR.Text = "RapidOCR";
            this.checkBox_ShowOcrRapidOCR.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrBaidu
            // 
            this.checkBox_ShowOcrBaidu.AutoSize = true;
            this.checkBox_ShowOcrBaidu.Location = new System.Drawing.Point(20, 31);
            this.checkBox_ShowOcrBaidu.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrBaidu.Name = "checkBox_ShowOcrBaidu";
            this.checkBox_ShowOcrBaidu.Size = new System.Drawing.Size(97, 19);
            this.checkBox_ShowOcrBaidu.TabIndex = 11;
            this.checkBox_ShowOcrBaidu.Text = "百度-标准";
            this.checkBox_ShowOcrBaidu.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrBaiduAccurate
            // 
            this.checkBox_ShowOcrBaiduAccurate.AutoSize = true;
            this.checkBox_ShowOcrBaiduAccurate.Location = new System.Drawing.Point(133, 31);
            this.checkBox_ShowOcrBaiduAccurate.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrBaiduAccurate.Name = "checkBox_ShowOcrBaiduAccurate";
            this.checkBox_ShowOcrBaiduAccurate.Size = new System.Drawing.Size(112, 19);
            this.checkBox_ShowOcrBaiduAccurate.TabIndex = 12;
            this.checkBox_ShowOcrBaiduAccurate.Text = "百度-高精度";
            this.checkBox_ShowOcrBaiduAccurate.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrTencent
            // 
            this.checkBox_ShowOcrTencent.AutoSize = true;
            this.checkBox_ShowOcrTencent.Location = new System.Drawing.Point(247, 31);
            this.checkBox_ShowOcrTencent.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrTencent.Name = "checkBox_ShowOcrTencent";
            this.checkBox_ShowOcrTencent.Size = new System.Drawing.Size(97, 19);
            this.checkBox_ShowOcrTencent.TabIndex = 13;
            this.checkBox_ShowOcrTencent.Text = "腾讯-标准";
            this.checkBox_ShowOcrTencent.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrTencentAccurate
            // 
            this.checkBox_ShowOcrTencentAccurate.AutoSize = true;
            this.checkBox_ShowOcrTencentAccurate.Location = new System.Drawing.Point(360, 31);
            this.checkBox_ShowOcrTencentAccurate.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrTencentAccurate.Name = "checkBox_ShowOcrTencentAccurate";
            this.checkBox_ShowOcrTencentAccurate.Size = new System.Drawing.Size(112, 19);
            this.checkBox_ShowOcrTencentAccurate.TabIndex = 14;
            this.checkBox_ShowOcrTencentAccurate.Text = "腾讯-高精度";
            this.checkBox_ShowOcrTencentAccurate.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrBaimiao
            // 
            this.checkBox_ShowOcrBaimiao.AutoSize = true;
            this.checkBox_ShowOcrBaimiao.Location = new System.Drawing.Point(20, 69);
            this.checkBox_ShowOcrBaimiao.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrBaimiao.Name = "checkBox_ShowOcrBaimiao";
            this.checkBox_ShowOcrBaimiao.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ShowOcrBaimiao.TabIndex = 15;
            this.checkBox_ShowOcrBaimiao.Text = "白描";
            this.checkBox_ShowOcrBaimiao.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrSougou
            // 
            this.checkBox_ShowOcrSougou.AutoSize = true;
            this.checkBox_ShowOcrSougou.Location = new System.Drawing.Point(133, 69);
            this.checkBox_ShowOcrSougou.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrSougou.Name = "checkBox_ShowOcrSougou";
            this.checkBox_ShowOcrSougou.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ShowOcrSougou.TabIndex = 16;
            this.checkBox_ShowOcrSougou.Text = "搜狗";
            this.checkBox_ShowOcrSougou.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrYoudao
            // 
            this.checkBox_ShowOcrYoudao.AutoSize = true;
            this.checkBox_ShowOcrYoudao.Location = new System.Drawing.Point(247, 69);
            this.checkBox_ShowOcrYoudao.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrYoudao.Name = "checkBox_ShowOcrYoudao";
            this.checkBox_ShowOcrYoudao.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ShowOcrYoudao.TabIndex = 17;
            this.checkBox_ShowOcrYoudao.Text = "有道";
            this.checkBox_ShowOcrYoudao.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrWeChat
            // 
            this.checkBox_ShowOcrWeChat.AutoSize = true;
            this.checkBox_ShowOcrWeChat.Location = new System.Drawing.Point(360, 69);
            this.checkBox_ShowOcrWeChat.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrWeChat.Name = "checkBox_ShowOcrWeChat";
            this.checkBox_ShowOcrWeChat.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ShowOcrWeChat.TabIndex = 18;
            this.checkBox_ShowOcrWeChat.Text = "微信";
            this.checkBox_ShowOcrWeChat.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrMathfuntion
            // 
            this.checkBox_ShowOcrMathfuntion.AutoSize = true;
            this.checkBox_ShowOcrMathfuntion.Location = new System.Drawing.Point(20, 106);
            this.checkBox_ShowOcrMathfuntion.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrMathfuntion.Name = "checkBox_ShowOcrMathfuntion";
            this.checkBox_ShowOcrMathfuntion.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ShowOcrMathfuntion.TabIndex = 19;
            this.checkBox_ShowOcrMathfuntion.Text = "公式";
            this.checkBox_ShowOcrMathfuntion.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrTable
            // 
            this.checkBox_ShowOcrTable.AutoSize = true;
            this.checkBox_ShowOcrTable.Location = new System.Drawing.Point(133, 106);
            this.checkBox_ShowOcrTable.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrTable.Name = "checkBox_ShowOcrTable";
            this.checkBox_ShowOcrTable.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ShowOcrTable.TabIndex = 20;
            this.checkBox_ShowOcrTable.Text = "表格";
            this.checkBox_ShowOcrTable.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrShupai
            // 
            this.checkBox_ShowOcrShupai.AutoSize = true;
            this.checkBox_ShowOcrShupai.Location = new System.Drawing.Point(247, 106);
            this.checkBox_ShowOcrShupai.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrShupai.Name = "checkBox_ShowOcrShupai";
            this.checkBox_ShowOcrShupai.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ShowOcrShupai.TabIndex = 21;
            this.checkBox_ShowOcrShupai.Text = "竖排";
            this.checkBox_ShowOcrShupai.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrTableBaidu
            // 
            this.checkBox_ShowOcrTableBaidu.AutoSize = true;
            this.checkBox_ShowOcrTableBaidu.Location = new System.Drawing.Point(20, 144);
            this.checkBox_ShowOcrTableBaidu.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrTableBaidu.Name = "checkBox_ShowOcrTableBaidu";
            this.checkBox_ShowOcrTableBaidu.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowOcrTableBaidu.TabIndex = 22;
            this.checkBox_ShowOcrTableBaidu.Text = "百度表格";
            this.checkBox_ShowOcrTableBaidu.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrTableAli
            // 
            this.checkBox_ShowOcrTableAli.AutoSize = true;
            this.checkBox_ShowOcrTableAli.Location = new System.Drawing.Point(133, 144);
            this.checkBox_ShowOcrTableAli.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrTableAli.Name = "checkBox_ShowOcrTableAli";
            this.checkBox_ShowOcrTableAli.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowOcrTableAli.TabIndex = 23;
            this.checkBox_ShowOcrTableAli.Text = "阿里表格";
            this.checkBox_ShowOcrTableAli.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrShupaiLR
            // 
            this.checkBox_ShowOcrShupaiLR.AutoSize = true;
            this.checkBox_ShowOcrShupaiLR.Location = new System.Drawing.Point(360, 106);
            this.checkBox_ShowOcrShupaiLR.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrShupaiLR.Name = "checkBox_ShowOcrShupaiLR";
            this.checkBox_ShowOcrShupaiLR.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowOcrShupaiLR.TabIndex = 24;
            this.checkBox_ShowOcrShupaiLR.Text = "从左向右";
            this.checkBox_ShowOcrShupaiLR.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowOcrShupaiRL
            // 
            this.checkBox_ShowOcrShupaiRL.AutoSize = true;
            this.checkBox_ShowOcrShupaiRL.Location = new System.Drawing.Point(360, 144);
            this.checkBox_ShowOcrShupaiRL.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_ShowOcrShupaiRL.Name = "checkBox_ShowOcrShupaiRL";
            this.checkBox_ShowOcrShupaiRL.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ShowOcrShupaiRL.TabIndex = 25;
            this.checkBox_ShowOcrShupaiRL.Text = "从右向左";
            this.checkBox_ShowOcrShupaiRL.UseVisualStyleBackColor = true;
            // 
            // Page_About
            // 
            this.Page_About.BackColor = System.Drawing.Color.White;
            this.Page_About.Controls.Add(this.label78);
            this.Page_About.Controls.Add(this.label77);
            this.Page_About.Controls.Add(this.label_AuthorInfo);
            this.Page_About.Controls.Add(this.label_VersionInfo);
            this.Page_About.Location = new System.Drawing.Point(4, 25);
            this.Page_About.Margin = new System.Windows.Forms.Padding(4);
            this.Page_About.Name = "Page_About";
            this.Page_About.Size = new System.Drawing.Size(523, 515);
            this.Page_About.TabIndex = 9;
            this.Page_About.Text = "关于";
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(30, 136);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(297, 45);
            this.label78.TabIndex = 3;
            this.label78.Text = "如果遇到问题，请到Github仓库反馈。\r\n\r\n如果能帮到你，可以到Github仓库给个star";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(30, 99);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(378, 15);
            this.label77.TabIndex = 2;
            this.label77.Text = "开源地址：https://github.com/Topkill/tianruoocr";
            // 
            // label_AuthorInfo
            // 
            this.label_AuthorInfo.AutoSize = true;
            this.label_AuthorInfo.Location = new System.Drawing.Point(27, 62);
            this.label_AuthorInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_AuthorInfo.Name = "label_AuthorInfo";
            this.label_AuthorInfo.Size = new System.Drawing.Size(52, 15);
            this.label_AuthorInfo.TabIndex = 1;
            this.label_AuthorInfo.Text = "作者：";
            // 
            // label_VersionInfo
            // 
            this.label_VersionInfo.AutoSize = true;
            this.label_VersionInfo.Location = new System.Drawing.Point(27, 25);
            this.label_VersionInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_VersionInfo.Name = "label_VersionInfo";
            this.label_VersionInfo.Size = new System.Drawing.Size(67, 15);
            this.label_VersionInfo.TabIndex = 0;
            this.label_VersionInfo.Text = "版本号：";
            // 
            // Page_代理
            // 
            this.Page_代理.BackColor = System.Drawing.Color.White;
            this.Page_代理.Controls.Add(this.代理Button);
            this.Page_代理.Controls.Add(this.groupBox4);
            this.Page_代理.Location = new System.Drawing.Point(4, 25);
            this.Page_代理.Margin = new System.Windows.Forms.Padding(4);
            this.Page_代理.Name = "Page_代理";
            this.Page_代理.Padding = new System.Windows.Forms.Padding(4);
            this.Page_代理.Size = new System.Drawing.Size(523, 515);
            this.Page_代理.TabIndex = 4;
            this.Page_代理.Text = "代理";
            // 
            // 代理Button
            // 
            this.代理Button.BackColor = System.Drawing.Color.White;
            this.代理Button.Location = new System.Drawing.Point(412, 241);
            this.代理Button.Margin = new System.Windows.Forms.Padding(4);
            this.代理Button.Name = "代理Button";
            this.代理Button.Size = new System.Drawing.Size(100, 29);
            this.代理Button.TabIndex = 9;
            this.代理Button.Text = "恢复默认";
            this.代理Button.UseVisualStyleBackColor = false;
            this.代理Button.Click += new System.EventHandler(this.代理Button_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chbox_代理服务器);
            this.groupBox4.Controls.Add(this.text_密码);
            this.groupBox4.Controls.Add(this.text_端口);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.text_账号);
            this.groupBox4.Controls.Add(this.text_服务器);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.combox_代理);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(4, 8);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(508, 229);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "代理";
            // 
            // chbox_代理服务器
            // 
            this.chbox_代理服务器.AutoSize = true;
            this.chbox_代理服务器.Location = new System.Drawing.Point(104, 105);
            this.chbox_代理服务器.Margin = new System.Windows.Forms.Padding(4);
            this.chbox_代理服务器.Name = "chbox_代理服务器";
            this.chbox_代理服务器.Size = new System.Drawing.Size(164, 19);
            this.chbox_代理服务器.TabIndex = 12;
            this.chbox_代理服务器.Text = "代理服务器需要密码";
            this.chbox_代理服务器.UseVisualStyleBackColor = true;
            // 
            // text_密码
            // 
            this.text_密码.Location = new System.Drawing.Point(104, 180);
            this.text_密码.Margin = new System.Windows.Forms.Padding(4);
            this.text_密码.Name = "text_密码";
            this.text_密码.Size = new System.Drawing.Size(169, 25);
            this.text_密码.TabIndex = 11;
            this.text_密码.TextChanged += new System.EventHandler(this.text_密码_TextChanged);
            // 
            // text_端口
            // 
            this.text_端口.Location = new System.Drawing.Point(331, 65);
            this.text_端口.Margin = new System.Windows.Forms.Padding(4);
            this.text_端口.Name = "text_端口";
            this.text_端口.Size = new System.Drawing.Size(72, 25);
            this.text_端口.TabIndex = 10;
            this.text_端口.Text = " ";
            this.text_端口.TextChanged += new System.EventHandler(this.text_端口_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(283, 71);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 15);
            this.label15.TabIndex = 9;
            this.label15.Text = "端口：";
            // 
            // text_账号
            // 
            this.text_账号.Location = new System.Drawing.Point(104, 138);
            this.text_账号.Margin = new System.Windows.Forms.Padding(4);
            this.text_账号.Name = "text_账号";
            this.text_账号.Size = new System.Drawing.Size(169, 25);
            this.text_账号.TabIndex = 8;
            this.text_账号.TextChanged += new System.EventHandler(this.text_账号_TextChanged);
            // 
            // text_服务器
            // 
            this.text_服务器.Location = new System.Drawing.Point(104, 65);
            this.text_服务器.Margin = new System.Windows.Forms.Padding(4);
            this.text_服务器.Name = "text_服务器";
            this.text_服务器.Size = new System.Drawing.Size(169, 25);
            this.text_服务器.TabIndex = 7;
            this.text_服务器.TextChanged += new System.EventHandler(this.text_服务器_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 184);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 15);
            this.label14.TabIndex = 6;
            this.label14.Text = "密码：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 142);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 15);
            this.label13.TabIndex = 5;
            this.label13.Text = "用户名：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 71);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 15);
            this.label12.TabIndex = 4;
            this.label12.Text = "服务器：";
            // 
            // combox_代理
            // 
            this.combox_代理.BackColor = System.Drawing.Color.White;
            this.combox_代理.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combox_代理.FormattingEnabled = true;
            this.combox_代理.Items.AddRange(new object[] {
            "不使用代理",
            "系统代理",
            "自定义代理"});
            this.combox_代理.Location = new System.Drawing.Point(104, 22);
            this.combox_代理.Margin = new System.Windows.Forms.Padding(4);
            this.combox_代理.Name = "combox_代理";
            this.combox_代理.Size = new System.Drawing.Size(125, 23);
            this.combox_代理.TabIndex = 3;
            this.combox_代理.SelectedIndexChanged += new System.EventHandler(this.combox_代理_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 28);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "代理类型：";
            // 
            // Page_密钥
            // 
            this.Page_密钥.BackColor = System.Drawing.Color.White;
            this.Page_密钥.Controls.Add(this.groupBox7);
            this.Page_密钥.Controls.Add(this.百度_btn);
            this.Page_密钥.Controls.Add(this.密钥Button_apply);
            this.Page_密钥.Controls.Add(this.密钥Button);
            this.Page_密钥.Controls.Add(this.tabControl2);
            this.Page_密钥.Location = new System.Drawing.Point(4, 25);
            this.Page_密钥.Margin = new System.Windows.Forms.Padding(4);
            this.Page_密钥.Name = "Page_密钥";
            this.Page_密钥.Padding = new System.Windows.Forms.Padding(4);
            this.Page_密钥.Size = new System.Drawing.Size(523, 515);
            this.Page_密钥.TabIndex = 3;
            this.Page_密钥.Text = "密钥";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label_OcrApiHelpText);
            this.groupBox7.Location = new System.Drawing.Point(12, 214);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(500, 160);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "说明";
            this.groupBox7.Visible = false;
            // 
            // label_OcrApiHelpText
            // 
            this.label_OcrApiHelpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_OcrApiHelpText.Location = new System.Drawing.Point(3, 21);
            this.label_OcrApiHelpText.Name = "label_OcrApiHelpText";
            this.label_OcrApiHelpText.Size = new System.Drawing.Size(494, 136);
            this.label_OcrApiHelpText.TabIndex = 24;
            this.label_OcrApiHelpText.Text = "说明1：此接口只支持64位，支持ppocrv5和低版本的模型\r\n说明2：需要cpu支持avx指令集，不支持请使用PaddleOCR2接口\r\n说明3：高级配置文件留" +
    "空则使用默认值，普通用户推荐留空\r\n";
            // 
            // 百度_btn
            // 
            this.百度_btn.BackColor = System.Drawing.Color.White;
            this.百度_btn.Location = new System.Drawing.Point(203, 214);
            this.百度_btn.Margin = new System.Windows.Forms.Padding(4);
            this.百度_btn.Name = "百度_btn";
            this.百度_btn.Size = new System.Drawing.Size(100, 29);
            this.百度_btn.TabIndex = 10;
            this.百度_btn.Text = "密钥测试";
            this.百度_btn.UseVisualStyleBackColor = false;
            this.百度_btn.Click += new System.EventHandler(this.百度_btn_Click);
            // 
            // 密钥Button_apply
            // 
            this.密钥Button_apply.BackColor = System.Drawing.Color.White;
            this.密钥Button_apply.Location = new System.Drawing.Point(8, 214);
            this.密钥Button_apply.Margin = new System.Windows.Forms.Padding(4);
            this.密钥Button_apply.Name = "密钥Button_apply";
            this.密钥Button_apply.Size = new System.Drawing.Size(100, 29);
            this.密钥Button_apply.TabIndex = 9;
            this.密钥Button_apply.Text = "接口申请";
            this.密钥Button_apply.UseVisualStyleBackColor = false;
            this.密钥Button_apply.Click += new System.EventHandler(this.百度申请_Click);
            // 
            // 密钥Button
            // 
            this.密钥Button.BackColor = System.Drawing.Color.White;
            this.密钥Button.Location = new System.Drawing.Point(412, 214);
            this.密钥Button.Margin = new System.Windows.Forms.Padding(4);
            this.密钥Button.Name = "密钥Button";
            this.密钥Button.Size = new System.Drawing.Size(100, 29);
            this.密钥Button.TabIndex = 8;
            this.密钥Button.Text = "恢复默认";
            this.密钥Button.UseVisualStyleBackColor = false;
            this.密钥Button.Click += new System.EventHandler(this.密钥Button_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage_百度OCR);
            this.tabControl2.Controls.Add(this.tabPage_腾讯OCR);
            this.tabControl2.Controls.Add(this.tabPage_白描接口);
            this.tabControl2.Controls.Add(this.inPage_PaddleOCR);
            this.tabControl2.Controls.Add(this.inPage_PaddleOCR2);
            this.tabControl2.Controls.Add(this.inPage_RapidOCR);
            this.tabControl2.Location = new System.Drawing.Point(8, 8);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(504, 200);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged_1);
            // 
            // tabPage_百度OCR
            // 
            this.tabPage_百度OCR.Controls.Add(this.tabControl_BaiduApiType);
            this.tabPage_百度OCR.Location = new System.Drawing.Point(4, 25);
            this.tabPage_百度OCR.Name = "tabPage_百度OCR";
            this.tabPage_百度OCR.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_百度OCR.Size = new System.Drawing.Size(496, 171);
            this.tabPage_百度OCR.TabIndex = 5;
            this.tabPage_百度OCR.Text = "百度OCR";
            this.tabPage_百度OCR.UseVisualStyleBackColor = true;
            // 
            // tabControl_BaiduApiType
            // 
            this.tabControl_BaiduApiType.Controls.Add(this.inPage_百度接口);
            this.tabControl_BaiduApiType.Controls.Add(this.inPage_百度高精度接口);
            this.tabControl_BaiduApiType.Controls.Add(this.inPage_百度表格);
            this.tabControl_BaiduApiType.Controls.Add(this.inPage_百度手写);
            this.tabControl_BaiduApiType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_BaiduApiType.Location = new System.Drawing.Point(3, 3);
            this.tabControl_BaiduApiType.Name = "tabControl_BaiduApiType";
            this.tabControl_BaiduApiType.SelectedIndex = 0;
            this.tabControl_BaiduApiType.Size = new System.Drawing.Size(490, 165);
            this.tabControl_BaiduApiType.TabIndex = 0;
            // 
            // inPage_百度接口
            // 
            this.inPage_百度接口.BackColor = System.Drawing.Color.White;
            this.inPage_百度接口.Controls.Add(this.comboBox_Baidu_Language);
            this.inPage_百度接口.Controls.Add(this.label_Baidu_Language);
            this.inPage_百度接口.Controls.Add(this.text_baidupassword);
            this.inPage_百度接口.Controls.Add(this.text_baiduaccount);
            this.inPage_百度接口.Controls.Add(this.label10);
            this.inPage_百度接口.Controls.Add(this.label9);
            this.inPage_百度接口.Location = new System.Drawing.Point(4, 25);
            this.inPage_百度接口.Margin = new System.Windows.Forms.Padding(4);
            this.inPage_百度接口.Name = "inPage_百度接口";
            this.inPage_百度接口.Padding = new System.Windows.Forms.Padding(4);
            this.inPage_百度接口.Size = new System.Drawing.Size(482, 136);
            this.inPage_百度接口.TabIndex = 0;
            this.inPage_百度接口.Text = "标准版";
            // 
            // comboBox_Baidu_Language
            // 
            this.comboBox_Baidu_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Baidu_Language.FormattingEnabled = true;
            this.comboBox_Baidu_Language.Location = new System.Drawing.Point(107, 92);
            this.comboBox_Baidu_Language.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_Baidu_Language.Name = "comboBox_Baidu_Language";
            this.comboBox_Baidu_Language.Size = new System.Drawing.Size(160, 23);
            this.comboBox_Baidu_Language.TabIndex = 5;
            // 
            // label_Baidu_Language
            // 
            this.label_Baidu_Language.AutoSize = true;
            this.label_Baidu_Language.Location = new System.Drawing.Point(9, 97);
            this.label_Baidu_Language.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Baidu_Language.Name = "label_Baidu_Language";
            this.label_Baidu_Language.Size = new System.Drawing.Size(82, 15);
            this.label_Baidu_Language.TabIndex = 4;
            this.label_Baidu_Language.Text = "识别语言：";
            // 
            // text_baidupassword
            // 
            this.text_baidupassword.BackColor = System.Drawing.Color.White;
            this.text_baidupassword.Location = new System.Drawing.Point(107, 52);
            this.text_baidupassword.Margin = new System.Windows.Forms.Padding(4);
            this.text_baidupassword.Name = "text_baidupassword";
            this.text_baidupassword.Size = new System.Drawing.Size(367, 25);
            this.text_baidupassword.TabIndex = 3;
            this.text_baidupassword.TextChanged += new System.EventHandler(this.text_baidupassword_TextChanged);
            // 
            // text_baiduaccount
            // 
            this.text_baiduaccount.BackColor = System.Drawing.Color.White;
            this.text_baiduaccount.Location = new System.Drawing.Point(107, 12);
            this.text_baiduaccount.Margin = new System.Windows.Forms.Padding(4);
            this.text_baiduaccount.Name = "text_baiduaccount";
            this.text_baiduaccount.Size = new System.Drawing.Size(367, 25);
            this.text_baiduaccount.TabIndex = 2;
            this.text_baiduaccount.TextChanged += new System.EventHandler(this.text_baiduaccount_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 57);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 15);
            this.label10.TabIndex = 1;
            this.label10.Text = "Secret Key:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 17);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "API Key:";
            // 
            // inPage_百度高精度接口
            // 
            this.inPage_百度高精度接口.BackColor = System.Drawing.Color.White;
            this.inPage_百度高精度接口.Controls.Add(this.text_baidu_accurate_secretkey);
            this.inPage_百度高精度接口.Controls.Add(this.text_baidu_accurate_apikey);
            this.inPage_百度高精度接口.Controls.Add(this.label_baidu_accurate_secretkey);
            this.inPage_百度高精度接口.Controls.Add(this.label_baidu_accurate_apikey);
            this.inPage_百度高精度接口.Controls.Add(this.comboBox_Baidu_Accurate_Language);
            this.inPage_百度高精度接口.Controls.Add(this.label_Baidu_Accurate_Language);
            this.inPage_百度高精度接口.Location = new System.Drawing.Point(4, 25);
            this.inPage_百度高精度接口.Margin = new System.Windows.Forms.Padding(4);
            this.inPage_百度高精度接口.Name = "inPage_百度高精度接口";
            this.inPage_百度高精度接口.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_百度高精度接口.Size = new System.Drawing.Size(482, 136);
            this.inPage_百度高精度接口.TabIndex = 1;
            this.inPage_百度高精度接口.Text = "高精度";
            this.inPage_百度高精度接口.UseVisualStyleBackColor = true;
            // 
            // text_baidu_accurate_secretkey
            // 
            this.text_baidu_accurate_secretkey.BackColor = System.Drawing.Color.White;
            this.text_baidu_accurate_secretkey.Location = new System.Drawing.Point(107, 52);
            this.text_baidu_accurate_secretkey.Margin = new System.Windows.Forms.Padding(4);
            this.text_baidu_accurate_secretkey.Name = "text_baidu_accurate_secretkey";
            this.text_baidu_accurate_secretkey.Size = new System.Drawing.Size(367, 25);
            this.text_baidu_accurate_secretkey.TabIndex = 9;
            // 
            // text_baidu_accurate_apikey
            // 
            this.text_baidu_accurate_apikey.BackColor = System.Drawing.Color.White;
            this.text_baidu_accurate_apikey.Location = new System.Drawing.Point(107, 12);
            this.text_baidu_accurate_apikey.Margin = new System.Windows.Forms.Padding(4);
            this.text_baidu_accurate_apikey.Name = "text_baidu_accurate_apikey";
            this.text_baidu_accurate_apikey.Size = new System.Drawing.Size(367, 25);
            this.text_baidu_accurate_apikey.TabIndex = 8;
            // 
            // label_baidu_accurate_secretkey
            // 
            this.label_baidu_accurate_secretkey.AutoSize = true;
            this.label_baidu_accurate_secretkey.Location = new System.Drawing.Point(9, 57);
            this.label_baidu_accurate_secretkey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_baidu_accurate_secretkey.Name = "label_baidu_accurate_secretkey";
            this.label_baidu_accurate_secretkey.Size = new System.Drawing.Size(95, 15);
            this.label_baidu_accurate_secretkey.TabIndex = 7;
            this.label_baidu_accurate_secretkey.Text = "Secret Key:";
            // 
            // label_baidu_accurate_apikey
            // 
            this.label_baidu_accurate_apikey.AutoSize = true;
            this.label_baidu_accurate_apikey.Location = new System.Drawing.Point(9, 17);
            this.label_baidu_accurate_apikey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_baidu_accurate_apikey.Name = "label_baidu_accurate_apikey";
            this.label_baidu_accurate_apikey.Size = new System.Drawing.Size(71, 15);
            this.label_baidu_accurate_apikey.TabIndex = 6;
            this.label_baidu_accurate_apikey.Text = "API Key:";
            // 
            // comboBox_Baidu_Accurate_Language
            // 
            this.comboBox_Baidu_Accurate_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Baidu_Accurate_Language.FormattingEnabled = true;
            this.comboBox_Baidu_Accurate_Language.Location = new System.Drawing.Point(107, 92);
            this.comboBox_Baidu_Accurate_Language.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_Baidu_Accurate_Language.Name = "comboBox_Baidu_Accurate_Language";
            this.comboBox_Baidu_Accurate_Language.Size = new System.Drawing.Size(160, 23);
            this.comboBox_Baidu_Accurate_Language.TabIndex = 5;
            // 
            // label_Baidu_Accurate_Language
            // 
            this.label_Baidu_Accurate_Language.AutoSize = true;
            this.label_Baidu_Accurate_Language.Location = new System.Drawing.Point(9, 97);
            this.label_Baidu_Accurate_Language.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Baidu_Accurate_Language.Name = "label_Baidu_Accurate_Language";
            this.label_Baidu_Accurate_Language.Size = new System.Drawing.Size(82, 15);
            this.label_Baidu_Accurate_Language.TabIndex = 4;
            this.label_Baidu_Accurate_Language.Text = "识别语言：";
            // 
            // inPage_百度表格
            // 
            this.inPage_百度表格.Controls.Add(this.label24);
            this.inPage_百度表格.Controls.Add(this.label25);
            this.inPage_百度表格.Controls.Add(this.label23);
            this.inPage_百度表格.Controls.Add(this.textBox2);
            this.inPage_百度表格.Controls.Add(this.textBox1);
            this.inPage_百度表格.Location = new System.Drawing.Point(4, 25);
            this.inPage_百度表格.Margin = new System.Windows.Forms.Padding(4);
            this.inPage_百度表格.Name = "inPage_百度表格";
            this.inPage_百度表格.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_百度表格.Size = new System.Drawing.Size(482, 136);
            this.inPage_百度表格.TabIndex = 2;
            this.inPage_百度表格.Text = "表格";
            this.inPage_百度表格.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(9, 97);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(247, 15);
            this.label24.TabIndex = 7;
            this.label24.Text = "说明：留空则默认使用标准版的密钥";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(9, 17);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(71, 15);
            this.label25.TabIndex = 8;
            this.label25.Text = "API Key:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(9, 57);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(95, 15);
            this.label23.TabIndex = 6;
            this.label23.Text = "Secret Key:";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(107, 12);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(367, 25);
            this.textBox2.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(107, 52);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(367, 25);
            this.textBox1.TabIndex = 4;
            // 
            // inPage_百度手写
            // 
            this.inPage_百度手写.Controls.Add(this.comboBox_Baidu_Handwriting_Language);
            this.inPage_百度手写.Controls.Add(this.label39);
            this.inPage_百度手写.Controls.Add(this.text_baidu_handwriting_secretkey);
            this.inPage_百度手写.Controls.Add(this.text_baidu_handwriting_apikey);
            this.inPage_百度手写.Controls.Add(this.label38);
            this.inPage_百度手写.Controls.Add(this.label37);
            this.inPage_百度手写.Controls.Add(this.label36);
            this.inPage_百度手写.Location = new System.Drawing.Point(4, 25);
            this.inPage_百度手写.Name = "inPage_百度手写";
            this.inPage_百度手写.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_百度手写.Size = new System.Drawing.Size(482, 136);
            this.inPage_百度手写.TabIndex = 3;
            this.inPage_百度手写.Text = "手写";
            this.inPage_百度手写.UseVisualStyleBackColor = true;
            // 
            // comboBox_Baidu_Handwriting_Language
            // 
            this.comboBox_Baidu_Handwriting_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Baidu_Handwriting_Language.FormattingEnabled = true;
            this.comboBox_Baidu_Handwriting_Language.Location = new System.Drawing.Point(104, 83);
            this.comboBox_Baidu_Handwriting_Language.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_Baidu_Handwriting_Language.Name = "comboBox_Baidu_Handwriting_Language";
            this.comboBox_Baidu_Handwriting_Language.Size = new System.Drawing.Size(160, 23);
            this.comboBox_Baidu_Handwriting_Language.TabIndex = 15;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(4, 86);
            this.label39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(82, 15);
            this.label39.TabIndex = 14;
            this.label39.Text = "识别语言：";
            // 
            // text_baidu_handwriting_secretkey
            // 
            this.text_baidu_handwriting_secretkey.BackColor = System.Drawing.Color.White;
            this.text_baidu_handwriting_secretkey.Location = new System.Drawing.Point(104, 47);
            this.text_baidu_handwriting_secretkey.Margin = new System.Windows.Forms.Padding(4);
            this.text_baidu_handwriting_secretkey.Name = "text_baidu_handwriting_secretkey";
            this.text_baidu_handwriting_secretkey.Size = new System.Drawing.Size(367, 25);
            this.text_baidu_handwriting_secretkey.TabIndex = 13;
            // 
            // text_baidu_handwriting_apikey
            // 
            this.text_baidu_handwriting_apikey.BackColor = System.Drawing.Color.White;
            this.text_baidu_handwriting_apikey.Location = new System.Drawing.Point(104, 14);
            this.text_baidu_handwriting_apikey.Margin = new System.Windows.Forms.Padding(4);
            this.text_baidu_handwriting_apikey.Name = "text_baidu_handwriting_apikey";
            this.text_baidu_handwriting_apikey.Size = new System.Drawing.Size(367, 25);
            this.text_baidu_handwriting_apikey.TabIndex = 12;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(4, 118);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(247, 15);
            this.label38.TabIndex = 11;
            this.label38.Text = "说明：留空则默认使用标准版的密钥";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(4, 54);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(95, 15);
            this.label37.TabIndex = 10;
            this.label37.Text = "Secret Key:";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(4, 17);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(71, 15);
            this.label36.TabIndex = 9;
            this.label36.Text = "API Key:";
            // 
            // tabPage_腾讯OCR
            // 
            this.tabPage_腾讯OCR.Controls.Add(this.tabControl_TXApiType);
            this.tabPage_腾讯OCR.Location = new System.Drawing.Point(4, 25);
            this.tabPage_腾讯OCR.Name = "tabPage_腾讯OCR";
            this.tabPage_腾讯OCR.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_腾讯OCR.Size = new System.Drawing.Size(496, 171);
            this.tabPage_腾讯OCR.TabIndex = 6;
            this.tabPage_腾讯OCR.Text = "腾讯OCR";
            this.tabPage_腾讯OCR.UseVisualStyleBackColor = true;
            // 
            // tabControl_TXApiType
            // 
            this.tabControl_TXApiType.Controls.Add(this.inPage_腾讯接口);
            this.tabControl_TXApiType.Controls.Add(this.inPage_腾讯高精度接口);
            this.tabControl_TXApiType.Controls.Add(this.inPage_腾讯表格v3);
            this.tabControl_TXApiType.Controls.Add(this.inPage_腾讯手写);
            this.tabControl_TXApiType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_TXApiType.Location = new System.Drawing.Point(3, 3);
            this.tabControl_TXApiType.Name = "tabControl_TXApiType";
            this.tabControl_TXApiType.SelectedIndex = 0;
            this.tabControl_TXApiType.Size = new System.Drawing.Size(490, 165);
            this.tabControl_TXApiType.TabIndex = 0;
            // 
            // inPage_腾讯接口
            // 
            this.inPage_腾讯接口.BackColor = System.Drawing.Color.White;
            this.inPage_腾讯接口.Controls.Add(this.BoxTencentKey);
            this.inPage_腾讯接口.Controls.Add(this.BoxTencentId);
            this.inPage_腾讯接口.Controls.Add(this.label17);
            this.inPage_腾讯接口.Controls.Add(this.label22);
            this.inPage_腾讯接口.Controls.Add(this.comboBox_Tencent_Language);
            this.inPage_腾讯接口.Controls.Add(this.label_Tencent_Language);
            this.inPage_腾讯接口.Location = new System.Drawing.Point(4, 25);
            this.inPage_腾讯接口.Margin = new System.Windows.Forms.Padding(4);
            this.inPage_腾讯接口.Name = "inPage_腾讯接口";
            this.inPage_腾讯接口.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_腾讯接口.Size = new System.Drawing.Size(482, 136);
            this.inPage_腾讯接口.TabIndex = 0;
            this.inPage_腾讯接口.Text = "标准版";
            this.inPage_腾讯接口.UseVisualStyleBackColor = true;
            // 
            // BoxTencentKey
            // 
            this.BoxTencentKey.BackColor = System.Drawing.Color.White;
            this.BoxTencentKey.Location = new System.Drawing.Point(107, 52);
            this.BoxTencentKey.Margin = new System.Windows.Forms.Padding(4);
            this.BoxTencentKey.Name = "BoxTencentKey";
            this.BoxTencentKey.Size = new System.Drawing.Size(367, 25);
            this.BoxTencentKey.TabIndex = 7;
            // 
            // BoxTencentId
            // 
            this.BoxTencentId.BackColor = System.Drawing.Color.White;
            this.BoxTencentId.Location = new System.Drawing.Point(107, 12);
            this.BoxTencentId.Margin = new System.Windows.Forms.Padding(4);
            this.BoxTencentId.Name = "BoxTencentId";
            this.BoxTencentId.Size = new System.Drawing.Size(367, 25);
            this.BoxTencentId.TabIndex = 6;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 57);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(87, 15);
            this.label17.TabIndex = 5;
            this.label17.Text = "SecretKey:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(9, 17);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(79, 15);
            this.label22.TabIndex = 4;
            this.label22.Text = "SecretId:";
            // 
            // comboBox_Tencent_Language
            // 
            this.comboBox_Tencent_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Tencent_Language.FormattingEnabled = true;
            this.comboBox_Tencent_Language.Location = new System.Drawing.Point(107, 92);
            this.comboBox_Tencent_Language.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_Tencent_Language.Name = "comboBox_Tencent_Language";
            this.comboBox_Tencent_Language.Size = new System.Drawing.Size(160, 23);
            this.comboBox_Tencent_Language.TabIndex = 9;
            // 
            // label_Tencent_Language
            // 
            this.label_Tencent_Language.AutoSize = true;
            this.label_Tencent_Language.Location = new System.Drawing.Point(9, 97);
            this.label_Tencent_Language.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Tencent_Language.Name = "label_Tencent_Language";
            this.label_Tencent_Language.Size = new System.Drawing.Size(82, 15);
            this.label_Tencent_Language.TabIndex = 8;
            this.label_Tencent_Language.Text = "识别语言：";
            // 
            // inPage_腾讯高精度接口
            // 
            this.inPage_腾讯高精度接口.BackColor = System.Drawing.Color.White;
            this.inPage_腾讯高精度接口.Controls.Add(this.text_tencent_accurate_secretkey);
            this.inPage_腾讯高精度接口.Controls.Add(this.text_tencent_accurate_secretid);
            this.inPage_腾讯高精度接口.Controls.Add(this.label_tencent_accurate_secretkey);
            this.inPage_腾讯高精度接口.Controls.Add(this.label_tencent_accurate_secretid);
            this.inPage_腾讯高精度接口.Controls.Add(this.comboBox_Tencent_Accurate_Language);
            this.inPage_腾讯高精度接口.Controls.Add(this.label_Tencent_Accurate_Language);
            this.inPage_腾讯高精度接口.Location = new System.Drawing.Point(4, 25);
            this.inPage_腾讯高精度接口.Margin = new System.Windows.Forms.Padding(4);
            this.inPage_腾讯高精度接口.Name = "inPage_腾讯高精度接口";
            this.inPage_腾讯高精度接口.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_腾讯高精度接口.Size = new System.Drawing.Size(482, 136);
            this.inPage_腾讯高精度接口.TabIndex = 1;
            this.inPage_腾讯高精度接口.Text = "高精度";
            this.inPage_腾讯高精度接口.UseVisualStyleBackColor = true;
            // 
            // text_tencent_accurate_secretkey
            // 
            this.text_tencent_accurate_secretkey.BackColor = System.Drawing.Color.White;
            this.text_tencent_accurate_secretkey.Location = new System.Drawing.Point(107, 52);
            this.text_tencent_accurate_secretkey.Margin = new System.Windows.Forms.Padding(4);
            this.text_tencent_accurate_secretkey.Name = "text_tencent_accurate_secretkey";
            this.text_tencent_accurate_secretkey.Size = new System.Drawing.Size(367, 25);
            this.text_tencent_accurate_secretkey.TabIndex = 9;
            // 
            // text_tencent_accurate_secretid
            // 
            this.text_tencent_accurate_secretid.BackColor = System.Drawing.Color.White;
            this.text_tencent_accurate_secretid.Location = new System.Drawing.Point(107, 12);
            this.text_tencent_accurate_secretid.Margin = new System.Windows.Forms.Padding(4);
            this.text_tencent_accurate_secretid.Name = "text_tencent_accurate_secretid";
            this.text_tencent_accurate_secretid.Size = new System.Drawing.Size(367, 25);
            this.text_tencent_accurate_secretid.TabIndex = 8;
            // 
            // label_tencent_accurate_secretkey
            // 
            this.label_tencent_accurate_secretkey.AutoSize = true;
            this.label_tencent_accurate_secretkey.Location = new System.Drawing.Point(9, 57);
            this.label_tencent_accurate_secretkey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_tencent_accurate_secretkey.Name = "label_tencent_accurate_secretkey";
            this.label_tencent_accurate_secretkey.Size = new System.Drawing.Size(87, 15);
            this.label_tencent_accurate_secretkey.TabIndex = 7;
            this.label_tencent_accurate_secretkey.Text = "SecretKey:";
            // 
            // label_tencent_accurate_secretid
            // 
            this.label_tencent_accurate_secretid.AutoSize = true;
            this.label_tencent_accurate_secretid.Location = new System.Drawing.Point(9, 17);
            this.label_tencent_accurate_secretid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_tencent_accurate_secretid.Name = "label_tencent_accurate_secretid";
            this.label_tencent_accurate_secretid.Size = new System.Drawing.Size(79, 15);
            this.label_tencent_accurate_secretid.TabIndex = 6;
            this.label_tencent_accurate_secretid.Text = "SecretId:";
            // 
            // comboBox_Tencent_Accurate_Language
            // 
            this.comboBox_Tencent_Accurate_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Tencent_Accurate_Language.FormattingEnabled = true;
            this.comboBox_Tencent_Accurate_Language.Location = new System.Drawing.Point(107, 92);
            this.comboBox_Tencent_Accurate_Language.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_Tencent_Accurate_Language.Name = "comboBox_Tencent_Accurate_Language";
            this.comboBox_Tencent_Accurate_Language.Size = new System.Drawing.Size(160, 23);
            this.comboBox_Tencent_Accurate_Language.TabIndex = 5;
            // 
            // label_Tencent_Accurate_Language
            // 
            this.label_Tencent_Accurate_Language.AutoSize = true;
            this.label_Tencent_Accurate_Language.Location = new System.Drawing.Point(9, 97);
            this.label_Tencent_Accurate_Language.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Tencent_Accurate_Language.Name = "label_Tencent_Accurate_Language";
            this.label_Tencent_Accurate_Language.Size = new System.Drawing.Size(82, 15);
            this.label_Tencent_Accurate_Language.TabIndex = 4;
            this.label_Tencent_Accurate_Language.Text = "识别语言：";
            // 
            // inPage_腾讯表格v3
            // 
            this.inPage_腾讯表格v3.Controls.Add(this.label27);
            this.inPage_腾讯表格v3.Controls.Add(this.label28);
            this.inPage_腾讯表格v3.Controls.Add(this.label26);
            this.inPage_腾讯表格v3.Controls.Add(this.textBox4);
            this.inPage_腾讯表格v3.Controls.Add(this.textBox3);
            this.inPage_腾讯表格v3.Location = new System.Drawing.Point(4, 25);
            this.inPage_腾讯表格v3.Margin = new System.Windows.Forms.Padding(4);
            this.inPage_腾讯表格v3.Name = "inPage_腾讯表格v3";
            this.inPage_腾讯表格v3.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_腾讯表格v3.Size = new System.Drawing.Size(482, 136);
            this.inPage_腾讯表格v3.TabIndex = 2;
            this.inPage_腾讯表格v3.Text = "表格v3";
            this.inPage_腾讯表格v3.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(9, 17);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(79, 15);
            this.label27.TabIndex = 11;
            this.label27.Text = "SecretId:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(9, 97);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(247, 15);
            this.label28.TabIndex = 12;
            this.label28.Text = "说明：留空则默认使用标准版的密钥";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(9, 57);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(87, 15);
            this.label26.TabIndex = 10;
            this.label26.Text = "SecretKey:";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.White;
            this.textBox4.Location = new System.Drawing.Point(107, 52);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(367, 25);
            this.textBox4.TabIndex = 9;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(107, 12);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(367, 25);
            this.textBox3.TabIndex = 8;
            // 
            // inPage_腾讯手写
            // 
            this.inPage_腾讯手写.Controls.Add(this.label33);
            this.inPage_腾讯手写.Location = new System.Drawing.Point(4, 25);
            this.inPage_腾讯手写.Name = "inPage_腾讯手写";
            this.inPage_腾讯手写.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_腾讯手写.Size = new System.Drawing.Size(482, 136);
            this.inPage_腾讯手写.TabIndex = 3;
            this.inPage_腾讯手写.Text = "手写";
            this.inPage_腾讯手写.UseVisualStyleBackColor = true;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(3, 16);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(457, 75);
            this.label33.TabIndex = 1;
            this.label33.Text = "腾讯手写识别已合并至高精度版，请使用腾讯高精度接口（支持手写\r\n识别）\r\n\r\n目前支持手写的识别接口有百度手写，腾讯高精度，以及使用支持\r\n手写识别的模型的离线O" +
    "CR接口\r\n";
            // 
            // inPage_PaddleOCR
            // 
            this.inPage_PaddleOCR.Controls.Add(this.button2);
            this.inPage_PaddleOCR.Controls.Add(this.textBox5);
            this.inPage_PaddleOCR.Controls.Add(this.label29);
            this.inPage_PaddleOCR.Controls.Add(this.btn_PaddleOCR_Keys_Browse);
            this.inPage_PaddleOCR.Controls.Add(this.textBox_PaddleOCR_Keys);
            this.inPage_PaddleOCR.Controls.Add(this.label_PaddleOCR_Keys);
            this.inPage_PaddleOCR.Controls.Add(this.btn_PaddleOCR_Rec_Browse);
            this.inPage_PaddleOCR.Controls.Add(this.textBox_PaddleOCR_Rec);
            this.inPage_PaddleOCR.Controls.Add(this.label_PaddleOCR_Rec);
            this.inPage_PaddleOCR.Controls.Add(this.btn_PaddleOCR_Cls_Browse);
            this.inPage_PaddleOCR.Controls.Add(this.textBox_PaddleOCR_Cls);
            this.inPage_PaddleOCR.Controls.Add(this.label_PaddleOCR_Cls);
            this.inPage_PaddleOCR.Controls.Add(this.btn_PaddleOCR_Det_Browse);
            this.inPage_PaddleOCR.Controls.Add(this.textBox_PaddleOCR_Det);
            this.inPage_PaddleOCR.Controls.Add(this.label_PaddleOCR_Det);
            this.inPage_PaddleOCR.Location = new System.Drawing.Point(4, 25);
            this.inPage_PaddleOCR.Name = "inPage_PaddleOCR";
            this.inPage_PaddleOCR.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_PaddleOCR.Size = new System.Drawing.Size(496, 171);
            this.inPage_PaddleOCR.TabIndex = 7;
            this.inPage_PaddleOCR.Text = "PaddleOCR";
            this.inPage_PaddleOCR.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(451, 132);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 25);
            this.button2.TabIndex = 14;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(142, 132);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(303, 25);
            this.textBox5.TabIndex = 13;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(13, 138);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(105, 15);
            this.label29.TabIndex = 12;
            this.label29.Text = "高级配置文件:";
            // 
            // btn_PaddleOCR_Keys_Browse
            // 
            this.btn_PaddleOCR_Keys_Browse.Location = new System.Drawing.Point(451, 102);
            this.btn_PaddleOCR_Keys_Browse.Name = "btn_PaddleOCR_Keys_Browse";
            this.btn_PaddleOCR_Keys_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_PaddleOCR_Keys_Browse.TabIndex = 11;
            this.btn_PaddleOCR_Keys_Browse.Text = "...";
            this.btn_PaddleOCR_Keys_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR_Keys
            // 
            this.textBox_PaddleOCR_Keys.Location = new System.Drawing.Point(142, 102);
            this.textBox_PaddleOCR_Keys.Name = "textBox_PaddleOCR_Keys";
            this.textBox_PaddleOCR_Keys.Size = new System.Drawing.Size(303, 25);
            this.textBox_PaddleOCR_Keys.TabIndex = 10;
            // 
            // label_PaddleOCR_Keys
            // 
            this.label_PaddleOCR_Keys.AutoSize = true;
            this.label_PaddleOCR_Keys.Location = new System.Drawing.Point(13, 108);
            this.label_PaddleOCR_Keys.Name = "label_PaddleOCR_Keys";
            this.label_PaddleOCR_Keys.Size = new System.Drawing.Size(123, 15);
            this.label_PaddleOCR_Keys.TabIndex = 9;
            this.label_PaddleOCR_Keys.Text = "字典文件(keys):";
            // 
            // btn_PaddleOCR_Rec_Browse
            // 
            this.btn_PaddleOCR_Rec_Browse.Location = new System.Drawing.Point(451, 72);
            this.btn_PaddleOCR_Rec_Browse.Name = "btn_PaddleOCR_Rec_Browse";
            this.btn_PaddleOCR_Rec_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_PaddleOCR_Rec_Browse.TabIndex = 8;
            this.btn_PaddleOCR_Rec_Browse.Text = "...";
            this.btn_PaddleOCR_Rec_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR_Rec
            // 
            this.textBox_PaddleOCR_Rec.Location = new System.Drawing.Point(126, 72);
            this.textBox_PaddleOCR_Rec.Name = "textBox_PaddleOCR_Rec";
            this.textBox_PaddleOCR_Rec.Size = new System.Drawing.Size(319, 25);
            this.textBox_PaddleOCR_Rec.TabIndex = 7;
            // 
            // label_PaddleOCR_Rec
            // 
            this.label_PaddleOCR_Rec.AutoSize = true;
            this.label_PaddleOCR_Rec.Location = new System.Drawing.Point(13, 78);
            this.label_PaddleOCR_Rec.Name = "label_PaddleOCR_Rec";
            this.label_PaddleOCR_Rec.Size = new System.Drawing.Size(115, 15);
            this.label_PaddleOCR_Rec.TabIndex = 6;
            this.label_PaddleOCR_Rec.Text = "识别模型(rec):";
            // 
            // btn_PaddleOCR_Cls_Browse
            // 
            this.btn_PaddleOCR_Cls_Browse.Location = new System.Drawing.Point(451, 42);
            this.btn_PaddleOCR_Cls_Browse.Name = "btn_PaddleOCR_Cls_Browse";
            this.btn_PaddleOCR_Cls_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_PaddleOCR_Cls_Browse.TabIndex = 5;
            this.btn_PaddleOCR_Cls_Browse.Text = "...";
            this.btn_PaddleOCR_Cls_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR_Cls
            // 
            this.textBox_PaddleOCR_Cls.Location = new System.Drawing.Point(126, 42);
            this.textBox_PaddleOCR_Cls.Name = "textBox_PaddleOCR_Cls";
            this.textBox_PaddleOCR_Cls.Size = new System.Drawing.Size(319, 25);
            this.textBox_PaddleOCR_Cls.TabIndex = 4;
            // 
            // label_PaddleOCR_Cls
            // 
            this.label_PaddleOCR_Cls.AutoSize = true;
            this.label_PaddleOCR_Cls.Location = new System.Drawing.Point(13, 48);
            this.label_PaddleOCR_Cls.Name = "label_PaddleOCR_Cls";
            this.label_PaddleOCR_Cls.Size = new System.Drawing.Size(115, 15);
            this.label_PaddleOCR_Cls.TabIndex = 3;
            this.label_PaddleOCR_Cls.Text = "方向模型(cls):";
            // 
            // btn_PaddleOCR_Det_Browse
            // 
            this.btn_PaddleOCR_Det_Browse.Location = new System.Drawing.Point(451, 12);
            this.btn_PaddleOCR_Det_Browse.Name = "btn_PaddleOCR_Det_Browse";
            this.btn_PaddleOCR_Det_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_PaddleOCR_Det_Browse.TabIndex = 2;
            this.btn_PaddleOCR_Det_Browse.Text = "...";
            this.btn_PaddleOCR_Det_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR_Det
            // 
            this.textBox_PaddleOCR_Det.Location = new System.Drawing.Point(126, 12);
            this.textBox_PaddleOCR_Det.Name = "textBox_PaddleOCR_Det";
            this.textBox_PaddleOCR_Det.Size = new System.Drawing.Size(319, 25);
            this.textBox_PaddleOCR_Det.TabIndex = 1;
            // 
            // label_PaddleOCR_Det
            // 
            this.label_PaddleOCR_Det.AutoSize = true;
            this.label_PaddleOCR_Det.Location = new System.Drawing.Point(13, 18);
            this.label_PaddleOCR_Det.Name = "label_PaddleOCR_Det";
            this.label_PaddleOCR_Det.Size = new System.Drawing.Size(115, 15);
            this.label_PaddleOCR_Det.TabIndex = 0;
            this.label_PaddleOCR_Det.Text = "检测模型(det):";
            // 
            // inPage_PaddleOCR2
            // 
            this.inPage_PaddleOCR2.Controls.Add(this.button3);
            this.inPage_PaddleOCR2.Controls.Add(this.textBox6);
            this.inPage_PaddleOCR2.Controls.Add(this.label30);
            this.inPage_PaddleOCR2.Controls.Add(this.comboBox_PaddleOCR2_rec_Version);
            this.inPage_PaddleOCR2.Controls.Add(this.comboBox_PaddleOCR2_cls_Version);
            this.inPage_PaddleOCR2.Controls.Add(this.label_PaddleOCR2_rec_Version);
            this.inPage_PaddleOCR2.Controls.Add(this.label_PaddleOCR2_cls_Version);
            this.inPage_PaddleOCR2.Controls.Add(this.comboBox_PaddleOCR2_det_Version);
            this.inPage_PaddleOCR2.Controls.Add(this.label_PaddleOCR2_det_Version);
            this.inPage_PaddleOCR2.Controls.Add(this.btn_PaddleOCR2_Keys_Browse);
            this.inPage_PaddleOCR2.Controls.Add(this.textBox_PaddleOCR2_Keys);
            this.inPage_PaddleOCR2.Controls.Add(this.label_PaddleOCR2_Keys);
            this.inPage_PaddleOCR2.Controls.Add(this.btn_PaddleOCR2_Rec_Browse);
            this.inPage_PaddleOCR2.Controls.Add(this.textBox_PaddleOCR2_Rec);
            this.inPage_PaddleOCR2.Controls.Add(this.label_PaddleOCR2_Rec);
            this.inPage_PaddleOCR2.Controls.Add(this.btn_PaddleOCR2_Cls_Browse);
            this.inPage_PaddleOCR2.Controls.Add(this.textBox_PaddleOCR2_Cls);
            this.inPage_PaddleOCR2.Controls.Add(this.label_PaddleOCR2_Cls);
            this.inPage_PaddleOCR2.Controls.Add(this.btn_PaddleOCR2_Det_Browse);
            this.inPage_PaddleOCR2.Controls.Add(this.textBox_PaddleOCR2_Det);
            this.inPage_PaddleOCR2.Controls.Add(this.label_PaddleOCR2_Det);
            this.inPage_PaddleOCR2.Location = new System.Drawing.Point(4, 25);
            this.inPage_PaddleOCR2.Name = "inPage_PaddleOCR2";
            this.inPage_PaddleOCR2.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_PaddleOCR2.Size = new System.Drawing.Size(496, 171);
            this.inPage_PaddleOCR2.TabIndex = 8;
            this.inPage_PaddleOCR2.Text = "PaddleOCR2";
            this.inPage_PaddleOCR2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(445, 132);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 25);
            this.button3.TabIndex = 21;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(142, 132);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(297, 25);
            this.textBox6.TabIndex = 20;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(13, 138);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(105, 15);
            this.label30.TabIndex = 19;
            this.label30.Text = "高级配置文件:";
            // 
            // comboBox_PaddleOCR2_rec_Version
            // 
            this.comboBox_PaddleOCR2_rec_Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PaddleOCR2_rec_Version.FormattingEnabled = true;
            this.comboBox_PaddleOCR2_rec_Version.Items.AddRange(new object[] {
            "v2",
            "v3",
            "v4",
            "v5"});
            this.comboBox_PaddleOCR2_rec_Version.Location = new System.Drawing.Point(445, 75);
            this.comboBox_PaddleOCR2_rec_Version.Name = "comboBox_PaddleOCR2_rec_Version";
            this.comboBox_PaddleOCR2_rec_Version.Size = new System.Drawing.Size(45, 23);
            this.comboBox_PaddleOCR2_rec_Version.TabIndex = 18;
            // 
            // comboBox_PaddleOCR2_cls_Version
            // 
            this.comboBox_PaddleOCR2_cls_Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PaddleOCR2_cls_Version.FormattingEnabled = true;
            this.comboBox_PaddleOCR2_cls_Version.Items.AddRange(new object[] {
            "v2",
            "v3",
            "v4",
            "v5"});
            this.comboBox_PaddleOCR2_cls_Version.Location = new System.Drawing.Point(445, 46);
            this.comboBox_PaddleOCR2_cls_Version.Name = "comboBox_PaddleOCR2_cls_Version";
            this.comboBox_PaddleOCR2_cls_Version.Size = new System.Drawing.Size(45, 23);
            this.comboBox_PaddleOCR2_cls_Version.TabIndex = 17;
            // 
            // label_PaddleOCR2_rec_Version
            // 
            this.label_PaddleOCR2_rec_Version.AutoSize = true;
            this.label_PaddleOCR2_rec_Version.Location = new System.Drawing.Point(394, 79);
            this.label_PaddleOCR2_rec_Version.Name = "label_PaddleOCR2_rec_Version";
            this.label_PaddleOCR2_rec_Version.Size = new System.Drawing.Size(45, 15);
            this.label_PaddleOCR2_rec_Version.TabIndex = 15;
            this.label_PaddleOCR2_rec_Version.Text = "版本:";
            // 
            // label_PaddleOCR2_cls_Version
            // 
            this.label_PaddleOCR2_cls_Version.AutoSize = true;
            this.label_PaddleOCR2_cls_Version.Location = new System.Drawing.Point(394, 48);
            this.label_PaddleOCR2_cls_Version.Name = "label_PaddleOCR2_cls_Version";
            this.label_PaddleOCR2_cls_Version.Size = new System.Drawing.Size(45, 15);
            this.label_PaddleOCR2_cls_Version.TabIndex = 14;
            this.label_PaddleOCR2_cls_Version.Text = "版本:";
            // 
            // comboBox_PaddleOCR2_det_Version
            // 
            this.comboBox_PaddleOCR2_det_Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PaddleOCR2_det_Version.FormattingEnabled = true;
            this.comboBox_PaddleOCR2_det_Version.Items.AddRange(new object[] {
            "v2",
            "v3",
            "v4",
            "v5"});
            this.comboBox_PaddleOCR2_det_Version.Location = new System.Drawing.Point(445, 14);
            this.comboBox_PaddleOCR2_det_Version.Name = "comboBox_PaddleOCR2_det_Version";
            this.comboBox_PaddleOCR2_det_Version.Size = new System.Drawing.Size(45, 23);
            this.comboBox_PaddleOCR2_det_Version.TabIndex = 13;
            // 
            // label_PaddleOCR2_det_Version
            // 
            this.label_PaddleOCR2_det_Version.AutoSize = true;
            this.label_PaddleOCR2_det_Version.Location = new System.Drawing.Point(394, 17);
            this.label_PaddleOCR2_det_Version.Name = "label_PaddleOCR2_det_Version";
            this.label_PaddleOCR2_det_Version.Size = new System.Drawing.Size(45, 15);
            this.label_PaddleOCR2_det_Version.TabIndex = 12;
            this.label_PaddleOCR2_det_Version.Text = "版本:";
            // 
            // btn_PaddleOCR2_Keys_Browse
            // 
            this.btn_PaddleOCR2_Keys_Browse.Location = new System.Drawing.Point(445, 104);
            this.btn_PaddleOCR2_Keys_Browse.Name = "btn_PaddleOCR2_Keys_Browse";
            this.btn_PaddleOCR2_Keys_Browse.Size = new System.Drawing.Size(40, 25);
            this.btn_PaddleOCR2_Keys_Browse.TabIndex = 11;
            this.btn_PaddleOCR2_Keys_Browse.Text = "...";
            this.btn_PaddleOCR2_Keys_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR2_Keys
            // 
            this.textBox_PaddleOCR2_Keys.Location = new System.Drawing.Point(142, 102);
            this.textBox_PaddleOCR2_Keys.Name = "textBox_PaddleOCR2_Keys";
            this.textBox_PaddleOCR2_Keys.Size = new System.Drawing.Size(297, 25);
            this.textBox_PaddleOCR2_Keys.TabIndex = 10;
            // 
            // label_PaddleOCR2_Keys
            // 
            this.label_PaddleOCR2_Keys.AutoSize = true;
            this.label_PaddleOCR2_Keys.Location = new System.Drawing.Point(13, 108);
            this.label_PaddleOCR2_Keys.Name = "label_PaddleOCR2_Keys";
            this.label_PaddleOCR2_Keys.Size = new System.Drawing.Size(123, 15);
            this.label_PaddleOCR2_Keys.TabIndex = 9;
            this.label_PaddleOCR2_Keys.Text = "字典文件(keys):";
            // 
            // btn_PaddleOCR2_Rec_Browse
            // 
            this.btn_PaddleOCR2_Rec_Browse.Location = new System.Drawing.Point(348, 74);
            this.btn_PaddleOCR2_Rec_Browse.Name = "btn_PaddleOCR2_Rec_Browse";
            this.btn_PaddleOCR2_Rec_Browse.Size = new System.Drawing.Size(40, 25);
            this.btn_PaddleOCR2_Rec_Browse.TabIndex = 8;
            this.btn_PaddleOCR2_Rec_Browse.Text = "...";
            this.btn_PaddleOCR2_Rec_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR2_Rec
            // 
            this.textBox_PaddleOCR2_Rec.Location = new System.Drawing.Point(126, 72);
            this.textBox_PaddleOCR2_Rec.Name = "textBox_PaddleOCR2_Rec";
            this.textBox_PaddleOCR2_Rec.Size = new System.Drawing.Size(217, 25);
            this.textBox_PaddleOCR2_Rec.TabIndex = 7;
            // 
            // label_PaddleOCR2_Rec
            // 
            this.label_PaddleOCR2_Rec.AutoSize = true;
            this.label_PaddleOCR2_Rec.Location = new System.Drawing.Point(13, 78);
            this.label_PaddleOCR2_Rec.Name = "label_PaddleOCR2_Rec";
            this.label_PaddleOCR2_Rec.Size = new System.Drawing.Size(115, 15);
            this.label_PaddleOCR2_Rec.TabIndex = 6;
            this.label_PaddleOCR2_Rec.Text = "识别模型(rec):";
            // 
            // btn_PaddleOCR2_Cls_Browse
            // 
            this.btn_PaddleOCR2_Cls_Browse.Location = new System.Drawing.Point(348, 43);
            this.btn_PaddleOCR2_Cls_Browse.Name = "btn_PaddleOCR2_Cls_Browse";
            this.btn_PaddleOCR2_Cls_Browse.Size = new System.Drawing.Size(40, 25);
            this.btn_PaddleOCR2_Cls_Browse.TabIndex = 5;
            this.btn_PaddleOCR2_Cls_Browse.Text = "...";
            this.btn_PaddleOCR2_Cls_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR2_Cls
            // 
            this.textBox_PaddleOCR2_Cls.Location = new System.Drawing.Point(126, 42);
            this.textBox_PaddleOCR2_Cls.Name = "textBox_PaddleOCR2_Cls";
            this.textBox_PaddleOCR2_Cls.Size = new System.Drawing.Size(217, 25);
            this.textBox_PaddleOCR2_Cls.TabIndex = 4;
            // 
            // label_PaddleOCR2_Cls
            // 
            this.label_PaddleOCR2_Cls.AutoSize = true;
            this.label_PaddleOCR2_Cls.Location = new System.Drawing.Point(13, 48);
            this.label_PaddleOCR2_Cls.Name = "label_PaddleOCR2_Cls";
            this.label_PaddleOCR2_Cls.Size = new System.Drawing.Size(115, 15);
            this.label_PaddleOCR2_Cls.TabIndex = 3;
            this.label_PaddleOCR2_Cls.Text = "方向模型(cls):";
            // 
            // btn_PaddleOCR2_Det_Browse
            // 
            this.btn_PaddleOCR2_Det_Browse.Location = new System.Drawing.Point(348, 12);
            this.btn_PaddleOCR2_Det_Browse.Name = "btn_PaddleOCR2_Det_Browse";
            this.btn_PaddleOCR2_Det_Browse.Size = new System.Drawing.Size(40, 25);
            this.btn_PaddleOCR2_Det_Browse.TabIndex = 2;
            this.btn_PaddleOCR2_Det_Browse.Text = "...";
            this.btn_PaddleOCR2_Det_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_PaddleOCR2_Det
            // 
            this.textBox_PaddleOCR2_Det.Location = new System.Drawing.Point(126, 12);
            this.textBox_PaddleOCR2_Det.Name = "textBox_PaddleOCR2_Det";
            this.textBox_PaddleOCR2_Det.Size = new System.Drawing.Size(217, 25);
            this.textBox_PaddleOCR2_Det.TabIndex = 1;
            // 
            // label_PaddleOCR2_Det
            // 
            this.label_PaddleOCR2_Det.AutoSize = true;
            this.label_PaddleOCR2_Det.Location = new System.Drawing.Point(13, 18);
            this.label_PaddleOCR2_Det.Name = "label_PaddleOCR2_Det";
            this.label_PaddleOCR2_Det.Size = new System.Drawing.Size(115, 15);
            this.label_PaddleOCR2_Det.TabIndex = 0;
            this.label_PaddleOCR2_Det.Text = "检测模型(det):";
            // 
            // inPage_RapidOCR
            // 
            this.inPage_RapidOCR.Controls.Add(this.button4);
            this.inPage_RapidOCR.Controls.Add(this.textBox7);
            this.inPage_RapidOCR.Controls.Add(this.label31);
            this.inPage_RapidOCR.Controls.Add(this.btn_RapidOCR_Keys_Browse);
            this.inPage_RapidOCR.Controls.Add(this.textBox_RapidOCR_Keys);
            this.inPage_RapidOCR.Controls.Add(this.label_RapidOCR_Keys);
            this.inPage_RapidOCR.Controls.Add(this.btn_RapidOCR_Rec_Browse);
            this.inPage_RapidOCR.Controls.Add(this.textBox_RapidOCR_Rec);
            this.inPage_RapidOCR.Controls.Add(this.label_RapidOCR_Rec);
            this.inPage_RapidOCR.Controls.Add(this.btn_RapidOCR_Cls_Browse);
            this.inPage_RapidOCR.Controls.Add(this.textBox_RapidOCR_Cls);
            this.inPage_RapidOCR.Controls.Add(this.label_RapidOCR_Cls);
            this.inPage_RapidOCR.Controls.Add(this.btn_RapidOCR_Det_Browse);
            this.inPage_RapidOCR.Controls.Add(this.textBox_RapidOCR_Det);
            this.inPage_RapidOCR.Controls.Add(this.label_RapidOCR_Det);
            this.inPage_RapidOCR.Location = new System.Drawing.Point(4, 25);
            this.inPage_RapidOCR.Name = "inPage_RapidOCR";
            this.inPage_RapidOCR.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_RapidOCR.Size = new System.Drawing.Size(496, 171);
            this.inPage_RapidOCR.TabIndex = 9;
            this.inPage_RapidOCR.Text = "RapidOCR";
            this.inPage_RapidOCR.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(443, 133);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(39, 25);
            this.button4.TabIndex = 14;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(138, 133);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(299, 25);
            this.textBox7.TabIndex = 13;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(13, 138);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(105, 15);
            this.label31.TabIndex = 12;
            this.label31.Text = "高级配置文件:";
            // 
            // btn_RapidOCR_Keys_Browse
            // 
            this.btn_RapidOCR_Keys_Browse.Location = new System.Drawing.Point(443, 100);
            this.btn_RapidOCR_Keys_Browse.Name = "btn_RapidOCR_Keys_Browse";
            this.btn_RapidOCR_Keys_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_RapidOCR_Keys_Browse.TabIndex = 11;
            this.btn_RapidOCR_Keys_Browse.Text = "...";
            this.btn_RapidOCR_Keys_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_RapidOCR_Keys
            // 
            this.textBox_RapidOCR_Keys.Location = new System.Drawing.Point(138, 102);
            this.textBox_RapidOCR_Keys.Name = "textBox_RapidOCR_Keys";
            this.textBox_RapidOCR_Keys.Size = new System.Drawing.Size(299, 25);
            this.textBox_RapidOCR_Keys.TabIndex = 10;
            // 
            // label_RapidOCR_Keys
            // 
            this.label_RapidOCR_Keys.AutoSize = true;
            this.label_RapidOCR_Keys.Location = new System.Drawing.Point(13, 108);
            this.label_RapidOCR_Keys.Name = "label_RapidOCR_Keys";
            this.label_RapidOCR_Keys.Size = new System.Drawing.Size(123, 15);
            this.label_RapidOCR_Keys.TabIndex = 9;
            this.label_RapidOCR_Keys.Text = "字典文件(keys):";
            // 
            // btn_RapidOCR_Rec_Browse
            // 
            this.btn_RapidOCR_Rec_Browse.Location = new System.Drawing.Point(443, 70);
            this.btn_RapidOCR_Rec_Browse.Name = "btn_RapidOCR_Rec_Browse";
            this.btn_RapidOCR_Rec_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_RapidOCR_Rec_Browse.TabIndex = 8;
            this.btn_RapidOCR_Rec_Browse.Text = "...";
            this.btn_RapidOCR_Rec_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_RapidOCR_Rec
            // 
            this.textBox_RapidOCR_Rec.Location = new System.Drawing.Point(127, 72);
            this.textBox_RapidOCR_Rec.Name = "textBox_RapidOCR_Rec";
            this.textBox_RapidOCR_Rec.Size = new System.Drawing.Size(310, 25);
            this.textBox_RapidOCR_Rec.TabIndex = 7;
            // 
            // label_RapidOCR_Rec
            // 
            this.label_RapidOCR_Rec.AutoSize = true;
            this.label_RapidOCR_Rec.Location = new System.Drawing.Point(13, 78);
            this.label_RapidOCR_Rec.Name = "label_RapidOCR_Rec";
            this.label_RapidOCR_Rec.Size = new System.Drawing.Size(115, 15);
            this.label_RapidOCR_Rec.TabIndex = 6;
            this.label_RapidOCR_Rec.Text = "识别模型(rec):";
            // 
            // btn_RapidOCR_Cls_Browse
            // 
            this.btn_RapidOCR_Cls_Browse.Location = new System.Drawing.Point(443, 40);
            this.btn_RapidOCR_Cls_Browse.Name = "btn_RapidOCR_Cls_Browse";
            this.btn_RapidOCR_Cls_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_RapidOCR_Cls_Browse.TabIndex = 5;
            this.btn_RapidOCR_Cls_Browse.Text = "...";
            this.btn_RapidOCR_Cls_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_RapidOCR_Cls
            // 
            this.textBox_RapidOCR_Cls.Location = new System.Drawing.Point(127, 42);
            this.textBox_RapidOCR_Cls.Name = "textBox_RapidOCR_Cls";
            this.textBox_RapidOCR_Cls.Size = new System.Drawing.Size(310, 25);
            this.textBox_RapidOCR_Cls.TabIndex = 4;
            // 
            // label_RapidOCR_Cls
            // 
            this.label_RapidOCR_Cls.AutoSize = true;
            this.label_RapidOCR_Cls.Location = new System.Drawing.Point(13, 48);
            this.label_RapidOCR_Cls.Name = "label_RapidOCR_Cls";
            this.label_RapidOCR_Cls.Size = new System.Drawing.Size(115, 15);
            this.label_RapidOCR_Cls.TabIndex = 3;
            this.label_RapidOCR_Cls.Text = "方向模型(cls):";
            // 
            // btn_RapidOCR_Det_Browse
            // 
            this.btn_RapidOCR_Det_Browse.Location = new System.Drawing.Point(443, 10);
            this.btn_RapidOCR_Det_Browse.Name = "btn_RapidOCR_Det_Browse";
            this.btn_RapidOCR_Det_Browse.Size = new System.Drawing.Size(39, 25);
            this.btn_RapidOCR_Det_Browse.TabIndex = 2;
            this.btn_RapidOCR_Det_Browse.Text = "...";
            this.btn_RapidOCR_Det_Browse.UseVisualStyleBackColor = true;
            // 
            // textBox_RapidOCR_Det
            // 
            this.textBox_RapidOCR_Det.Location = new System.Drawing.Point(127, 12);
            this.textBox_RapidOCR_Det.Name = "textBox_RapidOCR_Det";
            this.textBox_RapidOCR_Det.Size = new System.Drawing.Size(310, 25);
            this.textBox_RapidOCR_Det.TabIndex = 1;
            // 
            // label_RapidOCR_Det
            // 
            this.label_RapidOCR_Det.AutoSize = true;
            this.label_RapidOCR_Det.Location = new System.Drawing.Point(13, 18);
            this.label_RapidOCR_Det.Name = "label_RapidOCR_Det";
            this.label_RapidOCR_Det.Size = new System.Drawing.Size(115, 15);
            this.label_RapidOCR_Det.TabIndex = 0;
            this.label_RapidOCR_Det.Text = "检测模型(det):";
            // 
            // groupBox_PaddleOCR_Config
            // 
            this.groupBox_PaddleOCR_Config.Location = new System.Drawing.Point(0, 0);
            this.groupBox_PaddleOCR_Config.Name = "groupBox_PaddleOCR_Config";
            this.groupBox_PaddleOCR_Config.Size = new System.Drawing.Size(200, 100);
            this.groupBox_PaddleOCR_Config.TabIndex = 0;
            this.groupBox_PaddleOCR_Config.TabStop = false;
            // 
            // label_PaddleOCR_Status
            // 
            this.label_PaddleOCR_Status.Location = new System.Drawing.Point(0, 0);
            this.label_PaddleOCR_Status.Name = "label_PaddleOCR_Status";
            this.label_PaddleOCR_Status.Size = new System.Drawing.Size(100, 23);
            this.label_PaddleOCR_Status.TabIndex = 0;
            // 
            // groupBox_PaddleOCR2_Config
            // 
            this.groupBox_PaddleOCR2_Config.Location = new System.Drawing.Point(0, 0);
            this.groupBox_PaddleOCR2_Config.Name = "groupBox_PaddleOCR2_Config";
            this.groupBox_PaddleOCR2_Config.Size = new System.Drawing.Size(200, 100);
            this.groupBox_PaddleOCR2_Config.TabIndex = 0;
            this.groupBox_PaddleOCR2_Config.TabStop = false;
            // 
            // label_PaddleOCR2_Status
            // 
            this.label_PaddleOCR2_Status.Location = new System.Drawing.Point(0, 0);
            this.label_PaddleOCR2_Status.Name = "label_PaddleOCR2_Status";
            this.label_PaddleOCR2_Status.Size = new System.Drawing.Size(100, 23);
            this.label_PaddleOCR2_Status.TabIndex = 0;
            // 
            // groupBox_RapidOCR_Config
            // 
            this.groupBox_RapidOCR_Config.Location = new System.Drawing.Point(0, 0);
            this.groupBox_RapidOCR_Config.Name = "groupBox_RapidOCR_Config";
            this.groupBox_RapidOCR_Config.Size = new System.Drawing.Size(200, 100);
            this.groupBox_RapidOCR_Config.TabIndex = 0;
            this.groupBox_RapidOCR_Config.TabStop = false;
            // 
            // label_RapidOCR_Status
            // 
            this.label_RapidOCR_Status.Location = new System.Drawing.Point(0, 0);
            this.label_RapidOCR_Status.Name = "label_RapidOCR_Status";
            this.label_RapidOCR_Status.Size = new System.Drawing.Size(100, 23);
            this.label_RapidOCR_Status.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(330, 203);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "内存测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Page_快捷键
            // 
            this.Page_快捷键.BackColor = System.Drawing.Color.White;
            this.Page_快捷键.Controls.Add(this.快捷键Button);
            this.Page_快捷键.Controls.Add(this.label8);
            this.Page_快捷键.Controls.Add(this.groupBox3);
            this.Page_快捷键.Location = new System.Drawing.Point(4, 25);
            this.Page_快捷键.Margin = new System.Windows.Forms.Padding(4);
            this.Page_快捷键.Name = "Page_快捷键";
            this.Page_快捷键.Padding = new System.Windows.Forms.Padding(4);
            this.Page_快捷键.Size = new System.Drawing.Size(523, 515);
            this.Page_快捷键.TabIndex = 2;
            this.Page_快捷键.Text = "快捷键";
            // 
            // 快捷键Button
            // 
            this.快捷键Button.BackColor = System.Drawing.Color.White;
            this.快捷键Button.Location = new System.Drawing.Point(412, 325);
            this.快捷键Button.Margin = new System.Windows.Forms.Padding(4);
            this.快捷键Button.Name = "快捷键Button";
            this.快捷键Button.Size = new System.Drawing.Size(100, 29);
            this.快捷键Button.TabIndex = 7;
            this.快捷键Button.Text = "恢复默认";
            this.快捷键Button.UseVisualStyleBackColor = false;
            this.快捷键Button.Click += new System.EventHandler(this.快捷键Button_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 329);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(274, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "说明：按Backspace键可清除当前快捷键";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBox_截图翻译);
            this.groupBox3.Controls.Add(this.txtBox_截图翻译);
            this.groupBox3.Controls.Add(this.label35);
            this.groupBox3.Controls.Add(this.pictureBox_静默识别);
            this.groupBox3.Controls.Add(this.txtBox_静默识别);
            this.groupBox3.Controls.Add(this.label_静默识别);
            this.groupBox3.Controls.Add(this.pictureBox_输入翻译);
            this.groupBox3.Controls.Add(this.txtBox_输入翻译);
            this.groupBox3.Controls.Add(this.label_输入翻译);
            this.groupBox3.Controls.Add(this.pictureBox_识别界面);
            this.groupBox3.Controls.Add(this.pictureBox_记录界面);
            this.groupBox3.Controls.Add(this.pictureBox_翻译文本);
            this.groupBox3.Controls.Add(this.pictureBox_文字识别);
            this.groupBox3.Controls.Add(this.txtBox_识别界面);
            this.groupBox3.Controls.Add(this.txtBox_记录界面);
            this.groupBox3.Controls.Add(this.txtBox_翻译文本);
            this.groupBox3.Controls.Add(this.txtBox_文字识别);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(8, 8);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(504, 312);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "全局快捷键";
            // 
            // pictureBox_截图翻译
            // 
            this.pictureBox_截图翻译.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_截图翻译.Location = new System.Drawing.Point(468, 267);
            this.pictureBox_截图翻译.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_截图翻译.Name = "pictureBox_截图翻译";
            this.pictureBox_截图翻译.Size = new System.Drawing.Size(28, 26);
            this.pictureBox_截图翻译.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_截图翻译.TabIndex = 17;
            this.pictureBox_截图翻译.TabStop = false;
            // 
            // txtBox_截图翻译
            // 
            this.txtBox_截图翻译.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_截图翻译.Location = new System.Drawing.Point(104, 266);
            this.txtBox_截图翻译.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_截图翻译.Name = "txtBox_截图翻译";
            this.txtBox_截图翻译.Size = new System.Drawing.Size(345, 27);
            this.txtBox_截图翻译.TabIndex = 16;
            this.txtBox_截图翻译.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_截图翻译.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_截图翻译.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(9, 272);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(82, 15);
            this.label35.TabIndex = 15;
            this.label35.Text = "截图翻译：";
            // 
            // pictureBox_静默识别
            // 
            this.pictureBox_静默识别.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_静默识别.Location = new System.Drawing.Point(468, 106);
            this.pictureBox_静默识别.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_静默识别.Name = "pictureBox_静默识别";
            this.pictureBox_静默识别.Size = new System.Drawing.Size(28, 26);
            this.pictureBox_静默识别.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_静默识别.TabIndex = 14;
            this.pictureBox_静默识别.TabStop = false;
            // 
            // txtBox_静默识别
            // 
            this.txtBox_静默识别.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_静默识别.Location = new System.Drawing.Point(104, 106);
            this.txtBox_静默识别.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_静默识别.Name = "txtBox_静默识别";
            this.txtBox_静默识别.Size = new System.Drawing.Size(345, 27);
            this.txtBox_静默识别.TabIndex = 6;
            this.txtBox_静默识别.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_静默识别.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_静默识别.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // label_静默识别
            // 
            this.label_静默识别.AutoSize = true;
            this.label_静默识别.Location = new System.Drawing.Point(8, 114);
            this.label_静默识别.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_静默识别.Name = "label_静默识别";
            this.label_静默识别.Size = new System.Drawing.Size(82, 15);
            this.label_静默识别.TabIndex = 12;
            this.label_静默识别.Text = "静默识别：";
            // 
            // pictureBox_输入翻译
            // 
            this.pictureBox_输入翻译.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_输入翻译.Location = new System.Drawing.Point(468, 148);
            this.pictureBox_输入翻译.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_输入翻译.Name = "pictureBox_输入翻译";
            this.pictureBox_输入翻译.Size = new System.Drawing.Size(28, 26);
            this.pictureBox_输入翻译.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_输入翻译.TabIndex = 14;
            this.pictureBox_输入翻译.TabStop = false;
            // 
            // txtBox_输入翻译
            // 
            this.txtBox_输入翻译.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_输入翻译.Location = new System.Drawing.Point(104, 148);
            this.txtBox_输入翻译.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_输入翻译.Name = "txtBox_输入翻译";
            this.txtBox_输入翻译.Size = new System.Drawing.Size(345, 27);
            this.txtBox_输入翻译.TabIndex = 6;
            this.txtBox_输入翻译.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_输入翻译.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_输入翻译.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // label_输入翻译
            // 
            this.label_输入翻译.AutoSize = true;
            this.label_输入翻译.Location = new System.Drawing.Point(8, 155);
            this.label_输入翻译.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_输入翻译.Name = "label_输入翻译";
            this.label_输入翻译.Size = new System.Drawing.Size(82, 15);
            this.label_输入翻译.TabIndex = 12;
            this.label_输入翻译.Text = "输入翻译：";
            // 
            // pictureBox_识别界面
            // 
            this.pictureBox_识别界面.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_识别界面.Location = new System.Drawing.Point(468, 230);
            this.pictureBox_识别界面.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_识别界面.Name = "pictureBox_识别界面";
            this.pictureBox_识别界面.Size = new System.Drawing.Size(28, 26);
            this.pictureBox_识别界面.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_识别界面.TabIndex = 11;
            this.pictureBox_识别界面.TabStop = false;
            // 
            // pictureBox_记录界面
            // 
            this.pictureBox_记录界面.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_记录界面.Location = new System.Drawing.Point(468, 189);
            this.pictureBox_记录界面.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_记录界面.Name = "pictureBox_记录界面";
            this.pictureBox_记录界面.Size = new System.Drawing.Size(28, 26);
            this.pictureBox_记录界面.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_记录界面.TabIndex = 10;
            this.pictureBox_记录界面.TabStop = false;
            // 
            // pictureBox_翻译文本
            // 
            this.pictureBox_翻译文本.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_翻译文本.Location = new System.Drawing.Point(468, 65);
            this.pictureBox_翻译文本.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_翻译文本.Name = "pictureBox_翻译文本";
            this.pictureBox_翻译文本.Size = new System.Drawing.Size(28, 26);
            this.pictureBox_翻译文本.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_翻译文本.TabIndex = 9;
            this.pictureBox_翻译文本.TabStop = false;
            // 
            // pictureBox_文字识别
            // 
            this.pictureBox_文字识别.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_文字识别.Location = new System.Drawing.Point(468, 24);
            this.pictureBox_文字识别.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_文字识别.Name = "pictureBox_文字识别";
            this.pictureBox_文字识别.Size = new System.Drawing.Size(28, 26);
            this.pictureBox_文字识别.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_文字识别.TabIndex = 8;
            this.pictureBox_文字识别.TabStop = false;
            // 
            // txtBox_识别界面
            // 
            this.txtBox_识别界面.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_识别界面.Location = new System.Drawing.Point(104, 230);
            this.txtBox_识别界面.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_识别界面.Name = "txtBox_识别界面";
            this.txtBox_识别界面.Size = new System.Drawing.Size(345, 27);
            this.txtBox_识别界面.TabIndex = 8;
            this.txtBox_识别界面.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_识别界面.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_识别界面.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // txtBox_记录界面
            // 
            this.txtBox_记录界面.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_记录界面.Location = new System.Drawing.Point(104, 189);
            this.txtBox_记录界面.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_记录界面.Name = "txtBox_记录界面";
            this.txtBox_记录界面.Size = new System.Drawing.Size(345, 27);
            this.txtBox_记录界面.TabIndex = 7;
            this.txtBox_记录界面.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_记录界面.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_记录界面.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // txtBox_翻译文本
            // 
            this.txtBox_翻译文本.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_翻译文本.Location = new System.Drawing.Point(104, 65);
            this.txtBox_翻译文本.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_翻译文本.Name = "txtBox_翻译文本";
            this.txtBox_翻译文本.Size = new System.Drawing.Size(345, 27);
            this.txtBox_翻译文本.TabIndex = 5;
            this.txtBox_翻译文本.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_翻译文本.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_翻译文本.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // txtBox_文字识别
            // 
            this.txtBox_文字识别.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_文字识别.Location = new System.Drawing.Point(104, 24);
            this.txtBox_文字识别.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_文字识别.Name = "txtBox_文字识别";
            this.txtBox_文字识别.Size = new System.Drawing.Size(345, 27);
            this.txtBox_文字识别.TabIndex = 4;
            this.txtBox_文字识别.TabStop = false;
            this.txtBox_文字识别.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_文字识别.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_文字识别.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 238);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 3;
            this.label7.Text = "识别界面：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 196);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "记录界面：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 72);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "翻译文本：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "文字识别：";
            // 
            // page_常规
            // 
            this.page_常规.BackColor = System.Drawing.Color.White;
            this.page_常规.Controls.Add(this.groupBox10);
            this.page_常规.Controls.Add(this.groupBox6);
            this.page_常规.Controls.Add(this.pic_help);
            this.page_常规.Controls.Add(this.常规Button);
            this.page_常规.Controls.Add(this.groupBox2);
            this.page_常规.Controls.Add(this.groupBox1);
            this.page_常规.Controls.Add(this.groupBox_OcrWorkflow);
            this.page_常规.Controls.Add(this.groupBox_TranslateWorkflow);
            this.page_常规.Location = new System.Drawing.Point(4, 25);
            this.page_常规.Margin = new System.Windows.Forms.Padding(4);
            this.page_常规.Name = "page_常规";
            this.page_常规.Padding = new System.Windows.Forms.Padding(4);
            this.page_常规.Size = new System.Drawing.Size(523, 515);
            this.page_常规.TabIndex = 0;
            this.page_常规.Text = "常规";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.chbox_save);
            this.groupBox10.Controls.Add(this.chbox_copy);
            this.groupBox10.Controls.Add(this.label20);
            this.groupBox10.Controls.Add(this.btn_音效路径);
            this.groupBox10.Controls.Add(this.btn_音效);
            this.groupBox10.Controls.Add(this.text_音效path);
            this.groupBox10.Controls.Add(this.label18);
            this.groupBox10.Location = new System.Drawing.Point(8, 336);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox10.Size = new System.Drawing.Size(504, 108);
            this.groupBox10.TabIndex = 8;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "音效";
            // 
            // chbox_save
            // 
            this.chbox_save.AutoSize = true;
            this.chbox_save.Checked = true;
            this.chbox_save.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbox_save.Location = new System.Drawing.Point(223, 68);
            this.chbox_save.Margin = new System.Windows.Forms.Padding(4);
            this.chbox_save.Name = "chbox_save";
            this.chbox_save.Size = new System.Drawing.Size(119, 19);
            this.chbox_save.TabIndex = 10;
            this.chbox_save.Text = "截图自动保存";
            this.chbox_save.UseVisualStyleBackColor = true;
            this.chbox_save.CheckedChanged += new System.EventHandler(this.chbox_save_CheckedChanged);
            // 
            // chbox_copy
            // 
            this.chbox_copy.AutoSize = true;
            this.chbox_copy.Location = new System.Drawing.Point(71, 68);
            this.chbox_copy.Margin = new System.Windows.Forms.Padding(4);
            this.chbox_copy.Name = "chbox_copy";
            this.chbox_copy.Size = new System.Drawing.Size(119, 19);
            this.chbox_copy.TabIndex = 6;
            this.chbox_copy.Text = "截图到粘贴板";
            this.chbox_copy.UseVisualStyleBackColor = true;
            this.chbox_copy.CheckedChanged += new System.EventHandler(this.chbox_copy_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(19, 69);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(52, 15);
            this.label20.TabIndex = 9;
            this.label20.Text = "何时：";
            // 
            // btn_音效路径
            // 
            this.btn_音效路径.BackColor = System.Drawing.Color.White;
            this.btn_音效路径.Location = new System.Drawing.Point(443, 61);
            this.btn_音效路径.Margin = new System.Windows.Forms.Padding(4);
            this.btn_音效路径.Name = "btn_音效路径";
            this.btn_音效路径.Size = new System.Drawing.Size(53, 29);
            this.btn_音效路径.TabIndex = 8;
            this.btn_音效路径.Text = "更改";
            this.btn_音效路径.UseVisualStyleBackColor = false;
            this.btn_音效路径.Click += new System.EventHandler(this.btn_音效路径_Click);
            // 
            // btn_音效
            // 
            this.btn_音效.BackColor = System.Drawing.Color.White;
            this.btn_音效.Image = global::TrOCR.Properties.Resources.语音按钮;
            this.btn_音效.Location = new System.Drawing.Point(459, 21);
            this.btn_音效.Margin = new System.Windows.Forms.Padding(4);
            this.btn_音效.Name = "btn_音效";
            this.btn_音效.Size = new System.Drawing.Size(35, 29);
            this.btn_音效.TabIndex = 7;
            this.btn_音效.UseVisualStyleBackColor = false;
            this.btn_音效.Click += new System.EventHandler(this.btn_音效_Click);
            // 
            // text_音效path
            // 
            this.text_音效path.BackColor = System.Drawing.Color.White;
            this.text_音效path.Location = new System.Drawing.Point(68, 24);
            this.text_音效path.Margin = new System.Windows.Forms.Padding(4);
            this.text_音效path.Name = "text_音效path";
            this.text_音效path.ReadOnly = true;
            this.text_音效path.Size = new System.Drawing.Size(383, 25);
            this.text_音效path.TabIndex = 4;
            this.text_音效path.Text = "Data\\screenshot.wav";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(17, 28);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(52, 15);
            this.label18.TabIndex = 3;
            this.label18.Text = "文件：";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_浏览);
            this.groupBox6.Controls.Add(this.textBox_path);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.cbBox_保存);
            this.groupBox6.Location = new System.Drawing.Point(8, 450);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox6.Size = new System.Drawing.Size(504, 79);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            // 
            // btn_浏览
            // 
            this.btn_浏览.BackColor = System.Drawing.Color.White;
            this.btn_浏览.Location = new System.Drawing.Point(443, 30);
            this.btn_浏览.Margin = new System.Windows.Forms.Padding(4);
            this.btn_浏览.Name = "btn_浏览";
            this.btn_浏览.Size = new System.Drawing.Size(53, 29);
            this.btn_浏览.TabIndex = 7;
            this.btn_浏览.Text = "浏览";
            this.btn_浏览.UseVisualStyleBackColor = false;
            this.btn_浏览.Click += new System.EventHandler(this.btn_浏览_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.BackColor = System.Drawing.Color.White;
            this.textBox_path.Location = new System.Drawing.Point(68, 32);
            this.textBox_path.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.ReadOnly = true;
            this.textBox_path.Size = new System.Drawing.Size(365, 25);
            this.textBox_path.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "目录：";
            // 
            // cbBox_保存
            // 
            this.cbBox_保存.AutoSize = true;
            this.cbBox_保存.BackColor = System.Drawing.Color.White;
            this.cbBox_保存.Location = new System.Drawing.Point(8, 0);
            this.cbBox_保存.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_保存.Name = "cbBox_保存";
            this.cbBox_保存.Size = new System.Drawing.Size(89, 19);
            this.cbBox_保存.TabIndex = 2;
            this.cbBox_保存.Text = "自动保存";
            this.cbBox_保存.UseVisualStyleBackColor = false;
            this.cbBox_保存.CheckedChanged += new System.EventHandler(this.cbBox_保存_CheckedChanged);
            // 
            // pic_help
            // 
            this.pic_help.Image = global::TrOCR.Properties.Resources.帮助;
            this.pic_help.Location = new System.Drawing.Point(4, 537);
            this.pic_help.Margin = new System.Windows.Forms.Padding(4);
            this.pic_help.Name = "pic_help";
            this.pic_help.Size = new System.Drawing.Size(36, 29);
            this.pic_help.TabIndex = 7;
            this.pic_help.TabStop = false;
            this.pic_help.Click += new System.EventHandler(this.pic_help_Click);
            // 
            // 常规Button
            // 
            this.常规Button.BackColor = System.Drawing.Color.White;
            this.常规Button.Location = new System.Drawing.Point(412, 537);
            this.常规Button.Margin = new System.Windows.Forms.Padding(4);
            this.常规Button.Name = "常规Button";
            this.常规Button.Size = new System.Drawing.Size(100, 29);
            this.常规Button.TabIndex = 6;
            this.常规Button.Text = "恢复默认";
            this.常规Button.UseVisualStyleBackColor = false;
            this.常规Button.Click += new System.EventHandler(this.常规Button_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numbox_记录);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cobBox_动画);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(8, 88);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(504, 88);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "界面";
            // 
            // numbox_记录
            // 
            this.numbox_记录.Location = new System.Drawing.Point(368, 35);
            this.numbox_记录.Margin = new System.Windows.Forms.Padding(4);
            this.numbox_记录.Name = "numbox_记录";
            this.numbox_记录.Size = new System.Drawing.Size(84, 25);
            this.numbox_记录.TabIndex = 5;
            this.numbox_记录.ValueChanged += new System.EventHandler(this.numbox_记录_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(273, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "记录数目：";
            // 
            // cobBox_动画
            // 
            this.cobBox_动画.BackColor = System.Drawing.Color.White;
            this.cobBox_动画.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobBox_动画.FormattingEnabled = true;
            this.cobBox_动画.Items.AddRange(new object[] {
            "窗体",
            "少女",
            "罗小黑"});
            this.cobBox_动画.Location = new System.Drawing.Point(112, 38);
            this.cobBox_动画.Margin = new System.Windows.Forms.Padding(4);
            this.cobBox_动画.Name = "cobBox_动画";
            this.cobBox_动画.Size = new System.Drawing.Size(83, 23);
            this.cobBox_动画.TabIndex = 3;
            this.cobBox_动画.SelectedIndexChanged += new System.EventHandler(this.cobBox_动画_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "窗体动画：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbox_取色);
            this.groupBox1.Controls.Add(this.cbBox_弹窗);
            this.groupBox1.Controls.Add(this.cbBox_翻译);
            this.groupBox1.Controls.Add(this.cbBox_开机);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(504, 64);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // chbox_取色
            // 
            this.chbox_取色.AutoSize = true;
            this.chbox_取色.Location = new System.Drawing.Point(397, 25);
            this.chbox_取色.Margin = new System.Windows.Forms.Padding(4);
            this.chbox_取色.Name = "chbox_取色";
            this.chbox_取色.Size = new System.Drawing.Size(83, 19);
            this.chbox_取色.TabIndex = 6;
            this.chbox_取色.Text = "取色HEX";
            this.chbox_取色.UseVisualStyleBackColor = true;
            this.chbox_取色.CheckedChanged += new System.EventHandler(this.chbox_取色_CheckedChanged);
            // 
            // cbBox_弹窗
            // 
            this.cbBox_弹窗.AutoSize = true;
            this.cbBox_弹窗.Location = new System.Drawing.Point(272, 25);
            this.cbBox_弹窗.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_弹窗.Name = "cbBox_弹窗";
            this.cbBox_弹窗.Size = new System.Drawing.Size(89, 19);
            this.cbBox_弹窗.TabIndex = 5;
            this.cbBox_弹窗.Text = "识别弹窗";
            this.cbBox_弹窗.UseVisualStyleBackColor = true;
            this.cbBox_弹窗.CheckedChanged += new System.EventHandler(this.cbBox_弹窗_CheckedChanged);
            // 
            // cbBox_翻译
            // 
            this.cbBox_翻译.AutoSize = true;
            this.cbBox_翻译.Location = new System.Drawing.Point(143, 25);
            this.cbBox_翻译.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_翻译.Name = "cbBox_翻译";
            this.cbBox_翻译.Size = new System.Drawing.Size(89, 19);
            this.cbBox_翻译.TabIndex = 4;
            this.cbBox_翻译.Text = "快速翻译";
            this.cbBox_翻译.UseVisualStyleBackColor = true;
            this.cbBox_翻译.CheckedChanged += new System.EventHandler(this.cbBox_翻译_CheckedChanged);
            // 
            // cbBox_开机
            // 
            this.cbBox_开机.AutoSize = true;
            this.cbBox_开机.Location = new System.Drawing.Point(23, 25);
            this.cbBox_开机.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_开机.Name = "cbBox_开机";
            this.cbBox_开机.Size = new System.Drawing.Size(89, 19);
            this.cbBox_开机.TabIndex = 2;
            this.cbBox_开机.Text = "开机启动";
            this.cbBox_开机.UseVisualStyleBackColor = true;
            this.cbBox_开机.CheckedChanged += new System.EventHandler(this.cbBox_开机_CheckedChanged);
            // 
            // groupBox_OcrWorkflow
            // 
            this.groupBox_OcrWorkflow.Controls.Add(this.checkBox_AutoCopyOcrResult);
            this.groupBox_OcrWorkflow.Controls.Add(this.checkBox_AutoTranslateOcrResult);
            this.groupBox_OcrWorkflow.Location = new System.Drawing.Point(8, 184);
            this.groupBox_OcrWorkflow.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_OcrWorkflow.Name = "groupBox_OcrWorkflow";
            this.groupBox_OcrWorkflow.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_OcrWorkflow.Size = new System.Drawing.Size(504, 69);
            this.groupBox_OcrWorkflow.TabIndex = 9;
            this.groupBox_OcrWorkflow.TabStop = false;
            this.groupBox_OcrWorkflow.Text = "常规识别";
            // 
            // checkBox_AutoCopyOcrResult
            // 
            this.checkBox_AutoCopyOcrResult.AutoSize = true;
            this.checkBox_AutoCopyOcrResult.Location = new System.Drawing.Point(23, 31);
            this.checkBox_AutoCopyOcrResult.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_AutoCopyOcrResult.Name = "checkBox_AutoCopyOcrResult";
            this.checkBox_AutoCopyOcrResult.Size = new System.Drawing.Size(209, 19);
            this.checkBox_AutoCopyOcrResult.TabIndex = 0;
            this.checkBox_AutoCopyOcrResult.Text = "自动复制识别结果到剪贴板";
            this.checkBox_AutoCopyOcrResult.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoTranslateOcrResult
            // 
            this.checkBox_AutoTranslateOcrResult.AutoSize = true;
            this.checkBox_AutoTranslateOcrResult.Location = new System.Drawing.Point(272, 31);
            this.checkBox_AutoTranslateOcrResult.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_AutoTranslateOcrResult.Name = "checkBox_AutoTranslateOcrResult";
            this.checkBox_AutoTranslateOcrResult.Size = new System.Drawing.Size(149, 19);
            this.checkBox_AutoTranslateOcrResult.TabIndex = 1;
            this.checkBox_AutoTranslateOcrResult.Text = "自动翻译识别结果";
            this.checkBox_AutoTranslateOcrResult.UseVisualStyleBackColor = true;
            // 
            // groupBox_TranslateWorkflow
            // 
            this.groupBox_TranslateWorkflow.Controls.Add(this.checkBox_AutoCopyOcrTranslation);
            this.groupBox_TranslateWorkflow.Location = new System.Drawing.Point(8, 260);
            this.groupBox_TranslateWorkflow.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_TranslateWorkflow.Name = "groupBox_TranslateWorkflow";
            this.groupBox_TranslateWorkflow.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_TranslateWorkflow.Size = new System.Drawing.Size(504, 69);
            this.groupBox_TranslateWorkflow.TabIndex = 10;
            this.groupBox_TranslateWorkflow.TabStop = false;
            this.groupBox_TranslateWorkflow.Text = "常规翻译";
            // 
            // checkBox_AutoCopyOcrTranslation
            // 
            this.checkBox_AutoCopyOcrTranslation.AutoSize = true;
            this.checkBox_AutoCopyOcrTranslation.Location = new System.Drawing.Point(23, 31);
            this.checkBox_AutoCopyOcrTranslation.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_AutoCopyOcrTranslation.Name = "checkBox_AutoCopyOcrTranslation";
            this.checkBox_AutoCopyOcrTranslation.Size = new System.Drawing.Size(239, 19);
            this.checkBox_AutoCopyOcrTranslation.TabIndex = 0;
            this.checkBox_AutoCopyOcrTranslation.Text = "识别翻译后，自动复制翻译结果";
            this.checkBox_AutoCopyOcrTranslation.UseVisualStyleBackColor = true;
            // 
            // tab_标签
            // 
            this.tab_标签.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tab_标签.Controls.Add(this.page_常规);
            this.tab_标签.Controls.Add(this.Page_常规2);
            this.tab_标签.Controls.Add(this.Page_快捷键);
            this.tab_标签.Controls.Add(this.Page_密钥);
            this.tab_标签.Controls.Add(this.Page_AI密钥);
            this.tab_标签.Controls.Add(this.Page_翻译接口);
            this.tab_标签.Controls.Add(this.Page_AI翻译接口);
            this.tab_标签.Controls.Add(this.Page_显示的接口);
            this.tab_标签.Controls.Add(this.Page_工具栏);
            this.tab_标签.Controls.Add(this.Page_代理);
            this.tab_标签.Controls.Add(this.Page_更新);
            this.tab_标签.Controls.Add(this.Page_webdav);
            this.tab_标签.Controls.Add(this.Page_About);
            this.tab_标签.Location = new System.Drawing.Point(13, 2);
            this.tab_标签.Margin = new System.Windows.Forms.Padding(4);
            this.tab_标签.Name = "tab_标签";
            this.tab_标签.SelectedIndex = 0;
            this.tab_标签.Size = new System.Drawing.Size(531, 544);
            this.tab_标签.TabIndex = 0;
            this.tab_标签.SelectedIndexChanged += new System.EventHandler(this.tab_标签_SelectedIndexChanged);
            // 
            // Page_常规2
            // 
            this.Page_常规2.Controls.Add(this.groupBox50);
            this.Page_常规2.Controls.Add(this.groupBox13);
            this.Page_常规2.Controls.Add(this.groupBox12);
            this.Page_常规2.Controls.Add(this.groupBox11);
            this.Page_常规2.Controls.Add(this.groupBox9);
            this.Page_常规2.Location = new System.Drawing.Point(4, 25);
            this.Page_常规2.Name = "Page_常规2";
            this.Page_常规2.Padding = new System.Windows.Forms.Padding(3);
            this.Page_常规2.Size = new System.Drawing.Size(523, 515);
            this.Page_常规2.TabIndex = 13;
            this.Page_常规2.Text = "常规2";
            this.Page_常规2.UseVisualStyleBackColor = true;
            // 
            // groupBox50
            // 
            this.groupBox50.Controls.Add(this.label76);
            this.groupBox50.Controls.Add(this.label63);
            this.groupBox50.Controls.Add(this.textBox38);
            this.groupBox50.Controls.Add(this.label64);
            this.groupBox50.Location = new System.Drawing.Point(9, 277);
            this.groupBox50.Name = "groupBox50";
            this.groupBox50.Size = new System.Drawing.Size(511, 100);
            this.groupBox50.TabIndex = 5;
            this.groupBox50.TabStop = false;
            this.groupBox50.Text = "文本改变后自动翻译";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(6, 59);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(487, 15);
            this.label76.TabIndex = 5;
            this.label76.Text = "另外，使用高级语法可控制哪些接口开启和关闭此功能，具体看仓库文档";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(137, 30);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(393, 15);
            this.label63.TabIndex = 4;
            this.label63.Text = "(单位：毫秒，设为0可关闭此功能，建议设置大于1000)，";
            // 
            // textBox38
            // 
            this.textBox38.Location = new System.Drawing.Point(54, 27);
            this.textBox38.Multiline = true;
            this.textBox38.Name = "textBox38";
            this.textBox38.Size = new System.Drawing.Size(77, 20);
            this.textBox38.TabIndex = 3;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(6, 30);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(52, 15);
            this.label64.TabIndex = 2;
            this.label64.Text = "时间：";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.checkBox_AutoCopyInputTranslation);
            this.groupBox13.Controls.Add(this.cbBox_输入翻译自动翻译);
            this.groupBox13.Controls.Add(this.cbBox_输入翻译剪贴板);
            this.groupBox13.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox13.Location = new System.Drawing.Point(3, 3);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(517, 91);
            this.groupBox13.TabIndex = 3;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "输入翻译";
            // 
            // checkBox_AutoCopyInputTranslation
            // 
            this.checkBox_AutoCopyInputTranslation.AutoSize = true;
            this.checkBox_AutoCopyInputTranslation.Location = new System.Drawing.Point(4, 52);
            this.checkBox_AutoCopyInputTranslation.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_AutoCopyInputTranslation.Name = "checkBox_AutoCopyInputTranslation";
            this.checkBox_AutoCopyInputTranslation.Size = new System.Drawing.Size(239, 19);
            this.checkBox_AutoCopyInputTranslation.TabIndex = 10;
            this.checkBox_AutoCopyInputTranslation.Text = "输入翻译后，自动复制翻译结果";
            this.checkBox_AutoCopyInputTranslation.UseVisualStyleBackColor = true;
            // 
            // cbBox_输入翻译自动翻译
            // 
            this.cbBox_输入翻译自动翻译.AutoSize = true;
            this.cbBox_输入翻译自动翻译.Location = new System.Drawing.Point(254, 25);
            this.cbBox_输入翻译自动翻译.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_输入翻译自动翻译.Name = "cbBox_输入翻译自动翻译";
            this.cbBox_输入翻译自动翻译.Size = new System.Drawing.Size(194, 19);
            this.cbBox_输入翻译自动翻译.TabIndex = 9;
            this.cbBox_输入翻译自动翻译.Text = "输入翻译输入后自动翻译";
            this.cbBox_输入翻译自动翻译.UseVisualStyleBackColor = true;
            // 
            // cbBox_输入翻译剪贴板
            // 
            this.cbBox_输入翻译剪贴板.AutoSize = true;
            this.cbBox_输入翻译剪贴板.Location = new System.Drawing.Point(4, 25);
            this.cbBox_输入翻译剪贴板.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_输入翻译剪贴板.Name = "cbBox_输入翻译剪贴板";
            this.cbBox_输入翻译剪贴板.Size = new System.Drawing.Size(224, 19);
            this.cbBox_输入翻译剪贴板.TabIndex = 8;
            this.cbBox_输入翻译剪贴板.Text = "输入翻译默认使用剪贴板文本";
            this.cbBox_输入翻译剪贴板.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.checkbox_NoWindowScreenshotTranslation);
            this.groupBox12.Controls.Add(this.checkbox_AutoCopyScreenshotTranslation);
            this.groupBox12.Location = new System.Drawing.Point(7, 210);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(513, 61);
            this.groupBox12.TabIndex = 2;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "截图翻译";
            // 
            // checkbox_NoWindowScreenshotTranslation
            // 
            this.checkbox_NoWindowScreenshotTranslation.AutoSize = true;
            this.checkbox_NoWindowScreenshotTranslation.Enabled = false;
            this.checkbox_NoWindowScreenshotTranslation.Location = new System.Drawing.Point(232, 24);
            this.checkbox_NoWindowScreenshotTranslation.Name = "checkbox_NoWindowScreenshotTranslation";
            this.checkbox_NoWindowScreenshotTranslation.Size = new System.Drawing.Size(225, 19);
            this.checkbox_NoWindowScreenshotTranslation.TabIndex = 3;
            this.checkbox_NoWindowScreenshotTranslation.Text = "截图翻译不显示窗口(仅复制)";
            this.checkbox_NoWindowScreenshotTranslation.UseVisualStyleBackColor = true;
            // 
            // checkbox_AutoCopyScreenshotTranslation
            // 
            this.checkbox_AutoCopyScreenshotTranslation.AutoSize = true;
            this.checkbox_AutoCopyScreenshotTranslation.Location = new System.Drawing.Point(6, 24);
            this.checkbox_AutoCopyScreenshotTranslation.Name = "checkbox_AutoCopyScreenshotTranslation";
            this.checkbox_AutoCopyScreenshotTranslation.Size = new System.Drawing.Size(194, 19);
            this.checkbox_AutoCopyScreenshotTranslation.TabIndex = 2;
            this.checkbox_AutoCopyScreenshotTranslation.Text = "截图翻译后自动复制译文";
            this.checkbox_AutoCopyScreenshotTranslation.UseVisualStyleBackColor = true;
            this.checkbox_AutoCopyScreenshotTranslation.CheckedChanged += new System.EventHandler(this.checkbox_AutoCopyScreenshotTranslation_CheckedChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label80);
            this.groupBox11.Controls.Add(this.textBox39);
            this.groupBox11.Controls.Add(this.label79);
            this.groupBox11.Controls.Add(this.label62);
            this.groupBox11.Controls.Add(this.textBox37);
            this.groupBox11.Controls.Add(this.label61);
            this.groupBox11.Controls.Add(this.cbBox_禁用隐藏原文按钮);
            this.groupBox11.Controls.Add(this.checkBox_EnableTabHoverSwitch);
            this.groupBox11.Location = new System.Drawing.Point(9, 383);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(511, 122);
            this.groupBox11.TabIndex = 1;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "其他";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(185, 80);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(240, 15);
            this.label80.TabIndex = 9;
            this.label80.Text = "倍（1倍为不放大，重新识别生效）";
            // 
            // textBox39
            // 
            this.textBox39.Location = new System.Drawing.Point(121, 77);
            this.textBox39.Multiline = true;
            this.textBox39.Name = "textBox39";
            this.textBox39.Size = new System.Drawing.Size(58, 20);
            this.textBox39.TabIndex = 8;
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(1, 80);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(75, 15);
            this.label79.TabIndex = 7;
            this.label79.Text = "文字缩放:";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(185, 51);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(240, 15);
            this.label62.TabIndex = 5;
            this.label62.Text = "倍（1倍为不放大，重启软件生效）";
            // 
            // textBox37
            // 
            this.textBox37.Location = new System.Drawing.Point(121, 48);
            this.textBox37.Multiline = true;
            this.textBox37.Name = "textBox37";
            this.textBox37.Size = new System.Drawing.Size(58, 20);
            this.textBox37.TabIndex = 4;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(0, 51);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(120, 15);
            this.label61.TabIndex = 3;
            this.label61.Text = "工具栏图标放大:";
            // 
            // cbBox_禁用隐藏原文按钮
            // 
            this.cbBox_禁用隐藏原文按钮.AutoSize = true;
            this.cbBox_禁用隐藏原文按钮.Location = new System.Drawing.Point(3, 24);
            this.cbBox_禁用隐藏原文按钮.Name = "cbBox_禁用隐藏原文按钮";
            this.cbBox_禁用隐藏原文按钮.Size = new System.Drawing.Size(217, 19);
            this.cbBox_禁用隐藏原文按钮.TabIndex = 1;
            this.cbBox_禁用隐藏原文按钮.Text = "全局禁用显示/隐藏原文按钮";
            this.cbBox_禁用隐藏原文按钮.UseVisualStyleBackColor = true;
            // 
            // checkBox_EnableTabHoverSwitch
            // 
            this.checkBox_EnableTabHoverSwitch.AutoSize = true;
            this.checkBox_EnableTabHoverSwitch.Location = new System.Drawing.Point(248, 24);
            this.checkBox_EnableTabHoverSwitch.Name = "checkBox_EnableTabHoverSwitch";
            this.checkBox_EnableTabHoverSwitch.Size = new System.Drawing.Size(194, 19);
            this.checkBox_EnableTabHoverSwitch.TabIndex = 6;
            this.checkBox_EnableTabHoverSwitch.Text = "设置标签页悬停自动切换";
            this.checkBox_EnableTabHoverSwitch.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.tableLayoutPanel1);
            this.groupBox9.Location = new System.Drawing.Point(3, 100);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(517, 104);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "监听剪贴板";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 263F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.cbBox_ListenHideOriginal, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbBox_AutoCopyListenClipboardTranslation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbBox_ListenClipboard, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label34, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(511, 80);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cbBox_ListenHideOriginal
            // 
            this.cbBox_ListenHideOriginal.AutoSize = true;
            this.cbBox_ListenHideOriginal.Location = new System.Drawing.Point(3, 43);
            this.cbBox_ListenHideOriginal.Name = "cbBox_ListenHideOriginal";
            this.cbBox_ListenHideOriginal.Size = new System.Drawing.Size(239, 19);
            this.cbBox_ListenHideOriginal.TabIndex = 4;
            this.cbBox_ListenHideOriginal.Text = "监听剪贴板翻译后默认隐藏原文";
            this.cbBox_ListenHideOriginal.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(514, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(14, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "监听剪贴板并自动翻译";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cbBox_AutoCopyListenClipboardTranslation
            // 
            this.cbBox_AutoCopyListenClipboardTranslation.AutoSize = true;
            this.cbBox_AutoCopyListenClipboardTranslation.Location = new System.Drawing.Point(251, 3);
            this.cbBox_AutoCopyListenClipboardTranslation.Name = "cbBox_AutoCopyListenClipboardTranslation";
            this.cbBox_AutoCopyListenClipboardTranslation.Size = new System.Drawing.Size(239, 19);
            this.cbBox_AutoCopyListenClipboardTranslation.TabIndex = 1;
            this.cbBox_AutoCopyListenClipboardTranslation.Text = "监听剪贴板自动翻译后自动复制";
            this.cbBox_AutoCopyListenClipboardTranslation.UseVisualStyleBackColor = true;
            // 
            // cbBox_ListenClipboard
            // 
            this.cbBox_ListenClipboard.AutoSize = true;
            this.cbBox_ListenClipboard.Location = new System.Drawing.Point(3, 3);
            this.cbBox_ListenClipboard.Name = "cbBox_ListenClipboard";
            this.cbBox_ListenClipboard.Size = new System.Drawing.Size(179, 19);
            this.cbBox_ListenClipboard.TabIndex = 0;
            this.cbBox_ListenClipboard.Text = "监听剪贴板并自动翻译";
            this.cbBox_ListenClipboard.UseVisualStyleBackColor = true;
            // 
            // label34
            // 
            this.label34.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label34.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label34, 2);
            this.label34.Location = new System.Drawing.Point(273, 45);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(232, 30);
            this.label34.TabIndex = 2;
            this.label34.Text = "说明：可托盘菜单快速开启和关闭\r\n剪贴板监听";
            // 
            // Page_AI翻译接口
            // 
            this.Page_AI翻译接口.Controls.Add(this.groupBox_AI_Trans_Help);
            this.Page_AI翻译接口.Controls.Add(this.tabControl_AI_Trans);
            this.Page_AI翻译接口.Location = new System.Drawing.Point(4, 25);
            this.Page_AI翻译接口.Name = "Page_AI翻译接口";
            this.Page_AI翻译接口.Padding = new System.Windows.Forms.Padding(3);
            this.Page_AI翻译接口.Size = new System.Drawing.Size(523, 515);
            this.Page_AI翻译接口.TabIndex = 15;
            this.Page_AI翻译接口.Text = "AI翻译接口";
            this.Page_AI翻译接口.UseVisualStyleBackColor = true;
            // 
            // groupBox_AI_Trans_Help
            // 
            this.groupBox_AI_Trans_Help.AutoSize = true;
            this.groupBox_AI_Trans_Help.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_AI_Trans_Help.Controls.Add(this.label60);
            this.groupBox_AI_Trans_Help.Location = new System.Drawing.Point(6, 304);
            this.groupBox_AI_Trans_Help.Name = "groupBox_AI_Trans_Help";
            this.groupBox_AI_Trans_Help.Size = new System.Drawing.Size(458, 324);
            this.groupBox_AI_Trans_Help.TabIndex = 2;
            this.groupBox_AI_Trans_Help.TabStop = false;
            this.groupBox_AI_Trans_Help.Text = "说明";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label60.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label60.Location = new System.Drawing.Point(3, 21);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(452, 300);
            this.label60.TabIndex = 0;
            this.label60.Text = resources.GetString("label60.Text");
            // 
            // tabControl_AI_Trans
            // 
            this.tabControl_AI_Trans.Controls.Add(this.tabPage_Trans_OpenAICompatible);
            this.tabControl_AI_Trans.Location = new System.Drawing.Point(3, 3);
            this.tabControl_AI_Trans.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl_AI_Trans.Name = "tabControl_AI_Trans";
            this.tabControl_AI_Trans.SelectedIndex = 0;
            this.tabControl_AI_Trans.Size = new System.Drawing.Size(517, 294);
            this.tabControl_AI_Trans.TabIndex = 1;
            // 
            // tabPage_Trans_OpenAICompatible
            // 
            this.tabPage_Trans_OpenAICompatible.Controls.Add(this.splitContainer2);
            this.tabPage_Trans_OpenAICompatible.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Trans_OpenAICompatible.Name = "tabPage_Trans_OpenAICompatible";
            this.tabPage_Trans_OpenAICompatible.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Trans_OpenAICompatible.Size = new System.Drawing.Size(509, 265);
            this.tabPage_Trans_OpenAICompatible.TabIndex = 1;
            this.tabPage_Trans_OpenAICompatible.Text = "OpenAICompatible";
            this.tabPage_Trans_OpenAICompatible.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            this.splitContainer2.Panel1.Controls.Add(this.lb_CustomTransProviders);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox52);
            this.splitContainer2.Size = new System.Drawing.Size(503, 259);
            this.splitContainer2.SplitterDistance = 167;
            this.splitContainer2.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_Trans_Del_Provider);
            this.panel2.Controls.Add(this.btn_Trans_Add_Provider);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 219);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(167, 40);
            this.panel2.TabIndex = 1;
            // 
            // btn_Trans_Del_Provider
            // 
            this.btn_Trans_Del_Provider.Location = new System.Drawing.Point(89, 4);
            this.btn_Trans_Del_Provider.Name = "btn_Trans_Del_Provider";
            this.btn_Trans_Del_Provider.Size = new System.Drawing.Size(75, 23);
            this.btn_Trans_Del_Provider.TabIndex = 1;
            this.btn_Trans_Del_Provider.Text = "删除接口";
            this.btn_Trans_Del_Provider.UseVisualStyleBackColor = true;
            this.btn_Trans_Del_Provider.Click += new System.EventHandler(this.btn_Trans_Del_Provider_Click);
            // 
            // btn_Trans_Add_Provider
            // 
            this.btn_Trans_Add_Provider.Location = new System.Drawing.Point(8, 4);
            this.btn_Trans_Add_Provider.Name = "btn_Trans_Add_Provider";
            this.btn_Trans_Add_Provider.Size = new System.Drawing.Size(75, 23);
            this.btn_Trans_Add_Provider.TabIndex = 0;
            this.btn_Trans_Add_Provider.Text = "添加接口";
            this.btn_Trans_Add_Provider.UseVisualStyleBackColor = true;
            this.btn_Trans_Add_Provider.Click += new System.EventHandler(this.btn_Trans_Add_Provider_Click);
            // 
            // lb_CustomTransProviders
            // 
            this.lb_CustomTransProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CustomTransProviders.FormattingEnabled = true;
            this.lb_CustomTransProviders.ItemHeight = 15;
            this.lb_CustomTransProviders.Location = new System.Drawing.Point(0, 0);
            this.lb_CustomTransProviders.Name = "lb_CustomTransProviders";
            this.lb_CustomTransProviders.Size = new System.Drawing.Size(167, 259);
            this.lb_CustomTransProviders.TabIndex = 0;
            this.lb_CustomTransProviders.SelectedIndexChanged += new System.EventHandler(this.lb_CustomTransProviders_SelectedIndexChanged);
            // 
            // groupBox52
            // 
            this.groupBox52.Controls.Add(this.txt_Trans_Target);
            this.groupBox52.Controls.Add(this.txt_Trans_Source);
            this.groupBox52.Controls.Add(this.label70);
            this.groupBox52.Controls.Add(this.label59);
            this.groupBox52.Controls.Add(this.txt_Trans_ConfigPath);
            this.groupBox52.Controls.Add(this.txt_Trans_ModelName);
            this.groupBox52.Controls.Add(this.txt_Trans_ApiKey);
            this.groupBox52.Controls.Add(this.txt_Trans_ApiUrl);
            this.groupBox52.Controls.Add(this.txt_Trans_Name);
            this.groupBox52.Controls.Add(this.btn_Trans_BrowseConfig);
            this.groupBox52.Controls.Add(this.label54);
            this.groupBox52.Controls.Add(this.label55);
            this.groupBox52.Controls.Add(this.label56);
            this.groupBox52.Controls.Add(this.label57);
            this.groupBox52.Controls.Add(this.label58);
            this.groupBox52.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox52.Location = new System.Drawing.Point(0, 0);
            this.groupBox52.Name = "groupBox52";
            this.groupBox52.Size = new System.Drawing.Size(332, 259);
            this.groupBox52.TabIndex = 0;
            this.groupBox52.TabStop = false;
            this.groupBox52.Text = "参数设置";
            // 
            // txt_Trans_Target
            // 
            this.txt_Trans_Target.Location = new System.Drawing.Point(131, 223);
            this.txt_Trans_Target.Name = "txt_Trans_Target";
            this.txt_Trans_Target.Size = new System.Drawing.Size(100, 25);
            this.txt_Trans_Target.TabIndex = 13;
            this.txt_Trans_Target.TextChanged += new System.EventHandler(this.txt_Trans_Target_TextChanged);
            // 
            // txt_Trans_Source
            // 
            this.txt_Trans_Source.Location = new System.Drawing.Point(131, 192);
            this.txt_Trans_Source.Name = "txt_Trans_Source";
            this.txt_Trans_Source.Size = new System.Drawing.Size(100, 25);
            this.txt_Trans_Source.TabIndex = 12;
            this.txt_Trans_Source.TextChanged += new System.EventHandler(this.txt_Trans_Source_TextChanged);
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(6, 227);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(67, 15);
            this.label70.TabIndex = 11;
            this.label70.Text = "目标语言";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(7, 196);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(52, 15);
            this.label59.TabIndex = 10;
            this.label59.Text = "源语言";
            // 
            // txt_Trans_ConfigPath
            // 
            this.txt_Trans_ConfigPath.Location = new System.Drawing.Point(131, 161);
            this.txt_Trans_ConfigPath.Name = "txt_Trans_ConfigPath";
            this.txt_Trans_ConfigPath.Size = new System.Drawing.Size(150, 25);
            this.txt_Trans_ConfigPath.TabIndex = 9;
            this.txt_Trans_ConfigPath.TextChanged += new System.EventHandler(this.txt_Trans_ConfigPath_TextChanged);
            // 
            // txt_Trans_ModelName
            // 
            this.txt_Trans_ModelName.Location = new System.Drawing.Point(131, 129);
            this.txt_Trans_ModelName.Name = "txt_Trans_ModelName";
            this.txt_Trans_ModelName.Size = new System.Drawing.Size(100, 25);
            this.txt_Trans_ModelName.TabIndex = 8;
            this.txt_Trans_ModelName.TextChanged += new System.EventHandler(this.txt_Trans_ModelName_TextChanged);
            // 
            // txt_Trans_ApiKey
            // 
            this.txt_Trans_ApiKey.Location = new System.Drawing.Point(131, 93);
            this.txt_Trans_ApiKey.Name = "txt_Trans_ApiKey";
            this.txt_Trans_ApiKey.Size = new System.Drawing.Size(170, 25);
            this.txt_Trans_ApiKey.TabIndex = 7;
            this.txt_Trans_ApiKey.TextChanged += new System.EventHandler(this.txt_Trans_ApiKey_TextChanged);
            // 
            // txt_Trans_ApiUrl
            // 
            this.txt_Trans_ApiUrl.Location = new System.Drawing.Point(131, 60);
            this.txt_Trans_ApiUrl.Name = "txt_Trans_ApiUrl";
            this.txt_Trans_ApiUrl.Size = new System.Drawing.Size(170, 25);
            this.txt_Trans_ApiUrl.TabIndex = 6;
            this.txt_Trans_ApiUrl.TextChanged += new System.EventHandler(this.txt_Trans_ApiUrl_TextChanged);
            // 
            // txt_Trans_Name
            // 
            this.txt_Trans_Name.Location = new System.Drawing.Point(131, 24);
            this.txt_Trans_Name.Name = "txt_Trans_Name";
            this.txt_Trans_Name.Size = new System.Drawing.Size(100, 25);
            this.txt_Trans_Name.TabIndex = 5;
            this.txt_Trans_Name.TextChanged += new System.EventHandler(this.txt_Trans_Name_TextChanged);
            // 
            // btn_Trans_BrowseConfig
            // 
            this.btn_Trans_BrowseConfig.Location = new System.Drawing.Point(287, 160);
            this.btn_Trans_BrowseConfig.Name = "btn_Trans_BrowseConfig";
            this.btn_Trans_BrowseConfig.Size = new System.Drawing.Size(37, 23);
            this.btn_Trans_BrowseConfig.TabIndex = 2;
            this.btn_Trans_BrowseConfig.Text = "...";
            this.btn_Trans_BrowseConfig.UseVisualStyleBackColor = true;
            this.btn_Trans_BrowseConfig.Click += new System.EventHandler(this.btn_Trans_BrowseConfig_Click);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(7, 164);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(97, 15);
            this.label54.TabIndex = 4;
            this.label54.Text = "模式文件路径";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(7, 132);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(67, 15);
            this.label55.TabIndex = 3;
            this.label55.Text = "模型名称";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(7, 93);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(117, 15);
            this.label56.TabIndex = 2;
            this.label56.Text = "API 密钥 (Key)";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(7, 63);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(117, 15);
            this.label57.TabIndex = 1;
            this.label57.Text = "API 地址 (URL)";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(7, 34);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(67, 15);
            this.label58.TabIndex = 0;
            this.label58.Text = "显示名称";
            // 
            // Page_工具栏
            // 
            this.Page_工具栏.Controls.Add(this.groupBox_拆分文本);
            this.Page_工具栏.Controls.Add(this.groupBox_合并文本);
            this.Page_工具栏.Location = new System.Drawing.Point(4, 25);
            this.Page_工具栏.Margin = new System.Windows.Forms.Padding(4);
            this.Page_工具栏.Name = "Page_工具栏";
            this.Page_工具栏.Padding = new System.Windows.Forms.Padding(4);
            this.Page_工具栏.Size = new System.Drawing.Size(523, 515);
            this.Page_工具栏.TabIndex = 12;
            this.Page_工具栏.Text = "工具栏";
            this.Page_工具栏.UseVisualStyleBackColor = true;
            // 
            // groupBox_拆分文本
            // 
            this.groupBox_拆分文本.Controls.Add(this.tableLayoutPanel_拆分文本);
            this.groupBox_拆分文本.Location = new System.Drawing.Point(8, 109);
            this.groupBox_拆分文本.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_拆分文本.Name = "groupBox_拆分文本";
            this.groupBox_拆分文本.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_拆分文本.Size = new System.Drawing.Size(504, 98);
            this.groupBox_拆分文本.TabIndex = 1;
            this.groupBox_拆分文本.TabStop = false;
            this.groupBox_拆分文本.Text = "拆分文本";
            // 
            // tableLayoutPanel_拆分文本
            // 
            this.tableLayoutPanel_拆分文本.ColumnCount = 2;
            this.tableLayoutPanel_拆分文本.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.96825F));
            this.tableLayoutPanel_拆分文本.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.03175F));
            this.tableLayoutPanel_拆分文本.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_拆分文本.Controls.Add(this.checkBox_拆分后自动复制, 0, 0);
            this.tableLayoutPanel_拆分文本.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_拆分文本.Location = new System.Drawing.Point(4, 22);
            this.tableLayoutPanel_拆分文本.Name = "tableLayoutPanel_拆分文本";
            this.tableLayoutPanel_拆分文本.RowCount = 2;
            this.tableLayoutPanel_拆分文本.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_拆分文本.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_拆分文本.Size = new System.Drawing.Size(496, 72);
            this.tableLayoutPanel_拆分文本.TabIndex = 0;
            // 
            // checkBox_拆分后自动复制
            // 
            this.checkBox_拆分后自动复制.AutoSize = true;
            this.checkBox_拆分后自动复制.Location = new System.Drawing.Point(4, 4);
            this.checkBox_拆分后自动复制.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_拆分后自动复制.Name = "checkBox_拆分后自动复制";
            this.checkBox_拆分后自动复制.Size = new System.Drawing.Size(134, 19);
            this.checkBox_拆分后自动复制.TabIndex = 2;
            this.checkBox_拆分后自动复制.Text = "拆分后自动复制";
            this.checkBox_拆分后自动复制.UseVisualStyleBackColor = true;
            // 
            // groupBox_合并文本
            // 
            this.groupBox_合并文本.Controls.Add(this.tableLayoutPanel_合并文本);
            this.groupBox_合并文本.Location = new System.Drawing.Point(8, 8);
            this.groupBox_合并文本.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_合并文本.Name = "groupBox_合并文本";
            this.groupBox_合并文本.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_合并文本.Size = new System.Drawing.Size(504, 98);
            this.groupBox_合并文本.TabIndex = 0;
            this.groupBox_合并文本.TabStop = false;
            this.groupBox_合并文本.Text = "合并文本";
            // 
            // tableLayoutPanel_合并文本
            // 
            this.tableLayoutPanel_合并文本.ColumnCount = 2;
            this.tableLayoutPanel_合并文本.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.08871F));
            this.tableLayoutPanel_合并文本.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.91129F));
            this.tableLayoutPanel_合并文本.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_合并文本.Controls.Add(this.checkBox_合并去除所有空格, 1, 0);
            this.tableLayoutPanel_合并文本.Controls.Add(this.checkBox_合并去除空格, 0, 0);
            this.tableLayoutPanel_合并文本.Controls.Add(this.checkBox_合并自动复制, 0, 1);
            this.tableLayoutPanel_合并文本.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_合并文本.Location = new System.Drawing.Point(4, 22);
            this.tableLayoutPanel_合并文本.Name = "tableLayoutPanel_合并文本";
            this.tableLayoutPanel_合并文本.RowCount = 2;
            this.tableLayoutPanel_合并文本.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_合并文本.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_合并文本.Size = new System.Drawing.Size(496, 72);
            this.tableLayoutPanel_合并文本.TabIndex = 0;
            // 
            // checkBox_合并去除所有空格
            // 
            this.checkBox_合并去除所有空格.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_合并去除所有空格.AutoSize = true;
            this.checkBox_合并去除所有空格.Location = new System.Drawing.Point(183, 8);
            this.checkBox_合并去除所有空格.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_合并去除所有空格.Name = "checkBox_合并去除所有空格";
            this.checkBox_合并去除所有空格.Size = new System.Drawing.Size(164, 19);
            this.checkBox_合并去除所有空格.TabIndex = 3;
            this.checkBox_合并去除所有空格.Text = "合并时去除所有空格";
            this.checkBox_合并去除所有空格.UseVisualStyleBackColor = true;
            this.checkBox_合并去除所有空格.CheckedChanged += new System.EventHandler(this.checkBox_合并去除所有空格_CheckedChanged);
            // 
            // checkBox_合并去除空格
            // 
            this.checkBox_合并去除空格.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_合并去除空格.AutoSize = true;
            this.checkBox_合并去除空格.Location = new System.Drawing.Point(4, 8);
            this.checkBox_合并去除空格.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_合并去除空格.Name = "checkBox_合并去除空格";
            this.checkBox_合并去除空格.Size = new System.Drawing.Size(164, 19);
            this.checkBox_合并去除空格.TabIndex = 2;
            this.checkBox_合并去除空格.Text = "合并时智能去除空格";
            this.checkBox_合并去除空格.UseVisualStyleBackColor = true;
            this.checkBox_合并去除空格.CheckedChanged += new System.EventHandler(this.checkBox_合并去除空格_CheckedChanged);
            // 
            // checkBox_合并自动复制
            // 
            this.checkBox_合并自动复制.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_合并自动复制.AutoSize = true;
            this.checkBox_合并自动复制.Location = new System.Drawing.Point(4, 44);
            this.checkBox_合并自动复制.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_合并自动复制.Name = "checkBox_合并自动复制";
            this.checkBox_合并自动复制.Size = new System.Drawing.Size(134, 19);
            this.checkBox_合并自动复制.TabIndex = 1;
            this.checkBox_合并自动复制.Text = "合并后自动复制";
            this.checkBox_合并自动复制.UseVisualStyleBackColor = true;
            // 
            // Page_更新
            // 
            this.Page_更新.BackColor = System.Drawing.Color.White;
            this.Page_更新.Controls.Add(this.更新Button_check);
            this.Page_更新.Controls.Add(this.更新Button);
            this.Page_更新.Controls.Add(this.groupBox5);
            this.Page_更新.Location = new System.Drawing.Point(4, 25);
            this.Page_更新.Margin = new System.Windows.Forms.Padding(4);
            this.Page_更新.Name = "Page_更新";
            this.Page_更新.Padding = new System.Windows.Forms.Padding(4);
            this.Page_更新.Size = new System.Drawing.Size(523, 515);
            this.Page_更新.TabIndex = 5;
            this.Page_更新.Text = "更新";
            this.Page_更新.UseVisualStyleBackColor = true;
            // 
            // 更新Button_check
            // 
            this.更新Button_check.BackColor = System.Drawing.Color.White;
            this.更新Button_check.Location = new System.Drawing.Point(8, 104);
            this.更新Button_check.Margin = new System.Windows.Forms.Padding(4);
            this.更新Button_check.Name = "更新Button_check";
            this.更新Button_check.Size = new System.Drawing.Size(100, 29);
            this.更新Button_check.TabIndex = 11;
            this.更新Button_check.Text = "检查更新";
            this.更新Button_check.UseVisualStyleBackColor = false;
            // 
            // 更新Button
            // 
            this.更新Button.BackColor = System.Drawing.Color.White;
            this.更新Button.Location = new System.Drawing.Point(412, 104);
            this.更新Button.Margin = new System.Windows.Forms.Padding(4);
            this.更新Button.Name = "更新Button";
            this.更新Button.Size = new System.Drawing.Size(100, 29);
            this.更新Button.TabIndex = 10;
            this.更新Button.Text = "恢复默认";
            this.更新Button.UseVisualStyleBackColor = false;
            this.更新Button.Click += new System.EventHandler(this.更新Button_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.numbox_间隔时间);
            this.groupBox5.Controls.Add(this.checkBox_更新间隔);
            this.groupBox5.Controls.Add(this.check_检查更新);
            this.groupBox5.Controls.Add(this.checkBox_PreRelease);
            this.groupBox5.Location = new System.Drawing.Point(8, 18);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(504, 81);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(131, 39);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(37, 15);
            this.label16.TabIndex = 3;
            this.label16.Text = "小时";
            // 
            // numbox_间隔时间
            // 
            this.numbox_间隔时间.Location = new System.Drawing.Point(72, 34);
            this.numbox_间隔时间.Margin = new System.Windows.Forms.Padding(4);
            this.numbox_间隔时间.Name = "numbox_间隔时间";
            this.numbox_间隔时间.Size = new System.Drawing.Size(49, 25);
            this.numbox_间隔时间.TabIndex = 2;
            this.numbox_间隔时间.ValueChanged += new System.EventHandler(this.numbox_间隔时间_ValueChanged);
            // 
            // checkBox_更新间隔
            // 
            this.checkBox_更新间隔.AutoSize = true;
            this.checkBox_更新间隔.Location = new System.Drawing.Point(8, 39);
            this.checkBox_更新间隔.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_更新间隔.Name = "checkBox_更新间隔";
            this.checkBox_更新间隔.Size = new System.Drawing.Size(59, 19);
            this.checkBox_更新间隔.TabIndex = 1;
            this.checkBox_更新间隔.Text = "每隔";
            this.checkBox_更新间隔.UseVisualStyleBackColor = true;
            this.checkBox_更新间隔.CheckedChanged += new System.EventHandler(this.checkBox_更新间隔_CheckedChanged);
            // 
            // check_检查更新
            // 
            this.check_检查更新.AutoSize = true;
            this.check_检查更新.BackColor = System.Drawing.Color.White;
            this.check_检查更新.Location = new System.Drawing.Point(8, 0);
            this.check_检查更新.Margin = new System.Windows.Forms.Padding(4);
            this.check_检查更新.Name = "check_检查更新";
            this.check_检查更新.Size = new System.Drawing.Size(134, 19);
            this.check_检查更新.TabIndex = 0;
            this.check_检查更新.Text = "启动时检查更新";
            this.check_检查更新.UseVisualStyleBackColor = false;
            this.check_检查更新.CheckedChanged += new System.EventHandler(this.check_检查更新_CheckedChanged);
            // 
            // checkBox_PreRelease
            // 
            this.checkBox_PreRelease.AutoSize = true;
            this.checkBox_PreRelease.BackColor = System.Drawing.Color.White;
            this.checkBox_PreRelease.Location = new System.Drawing.Point(173, 39);
            this.checkBox_PreRelease.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_PreRelease.Name = "checkBox_PreRelease";
            this.checkBox_PreRelease.Size = new System.Drawing.Size(164, 19);
            this.checkBox_PreRelease.TabIndex = 4;
            this.checkBox_PreRelease.Text = "是否检查预览版更新";
            this.checkBox_PreRelease.UseVisualStyleBackColor = true;
            // 
            // Page_webdav
            // 
            this.Page_webdav.BackColor = System.Drawing.Color.White;
            this.Page_webdav.Controls.Add(this.label75);
            this.Page_webdav.Controls.Add(this.label74);
            this.Page_webdav.Controls.Add(this.label73);
            this.Page_webdav.Controls.Add(this.label72);
            this.Page_webdav.Controls.Add(this.btnDownloadConfig);
            this.Page_webdav.Controls.Add(this.btnUploadConfig);
            this.Page_webdav.Controls.Add(this.txtWebDavPass);
            this.Page_webdav.Controls.Add(this.txtWebDavUrl);
            this.Page_webdav.Controls.Add(this.label71);
            this.Page_webdav.Controls.Add(this.txtWebDavUser);
            this.Page_webdav.Location = new System.Drawing.Point(4, 25);
            this.Page_webdav.Margin = new System.Windows.Forms.Padding(4);
            this.Page_webdav.Name = "Page_webdav";
            this.Page_webdav.Padding = new System.Windows.Forms.Padding(4);
            this.Page_webdav.Size = new System.Drawing.Size(523, 515);
            this.Page_webdav.TabIndex = 16;
            this.Page_webdav.Text = "WebDav备份(实验性)";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label75.Location = new System.Drawing.Point(8, 254);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(396, 80);
            this.label75.TabIndex = 21;
            this.label75.Text = "注意：\r\nwebdav是实验性功能，备份和恢复只包括Data文件夹，\r\n如果离线接口的模型及高级配置文件、ai接口的模式文件，\r\n不在Data文件夹下，需要自己手" +
    "动备份\r\n";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(68, 112);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(52, 15);
            this.label74.TabIndex = 20;
            this.label74.Text = "密码：";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(68, 72);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(52, 15);
            this.label73.TabIndex = 19;
            this.label73.Text = "账户：";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(68, 32);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(100, 15);
            this.label72.TabIndex = 18;
            this.label72.Text = "Webdav地址：";
            // 
            // btnDownloadConfig
            // 
            this.btnDownloadConfig.Location = new System.Drawing.Point(344, 185);
            this.btnDownloadConfig.Name = "btnDownloadConfig";
            this.btnDownloadConfig.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadConfig.TabIndex = 17;
            this.btnDownloadConfig.Text = "恢复";
            this.btnDownloadConfig.UseVisualStyleBackColor = false;
            this.btnDownloadConfig.Click += new System.EventHandler(this.btnDownloadConfig_Click);
            // 
            // btnUploadConfig
            // 
            this.btnUploadConfig.Location = new System.Drawing.Point(121, 185);
            this.btnUploadConfig.Name = "btnUploadConfig";
            this.btnUploadConfig.Size = new System.Drawing.Size(75, 23);
            this.btnUploadConfig.TabIndex = 16;
            this.btnUploadConfig.Text = "备份";
            this.btnUploadConfig.UseVisualStyleBackColor = false;
            this.btnUploadConfig.Click += new System.EventHandler(this.btnUploadConfig_Click);
            // 
            // txtWebDavPass
            // 
            this.txtWebDavPass.Location = new System.Drawing.Point(174, 109);
            this.txtWebDavPass.Name = "txtWebDavPass";
            this.txtWebDavPass.Size = new System.Drawing.Size(300, 25);
            this.txtWebDavPass.TabIndex = 15;
            this.txtWebDavPass.Leave += new System.EventHandler(this.txtWebDavPass_Leave);
            // 
            // txtWebDavUrl
            // 
            this.txtWebDavUrl.Location = new System.Drawing.Point(174, 29);
            this.txtWebDavUrl.Name = "txtWebDavUrl";
            this.txtWebDavUrl.Size = new System.Drawing.Size(300, 25);
            this.txtWebDavUrl.TabIndex = 13;
            this.txtWebDavUrl.Leave += new System.EventHandler(this.txtWebDavUrl_Leave);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(9, 189);
            this.label71.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(0, 15);
            this.label71.TabIndex = 12;
            // 
            // txtWebDavUser
            // 
            this.txtWebDavUser.BackColor = System.Drawing.Color.White;
            this.txtWebDavUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWebDavUser.Location = new System.Drawing.Point(174, 69);
            this.txtWebDavUser.Margin = new System.Windows.Forms.Padding(4);
            this.txtWebDavUser.Multiline = true;
            this.txtWebDavUser.Name = "txtWebDavUser";
            this.txtWebDavUser.Size = new System.Drawing.Size(300, 25);
            this.txtWebDavUser.TabIndex = 5;
            this.txtWebDavUser.Leave += new System.EventHandler(this.txtWebDavUser_Leave);
            // 
            // Page_反馈
            // 
            this.Page_反馈.BackColor = System.Drawing.Color.White;
            this.Page_反馈.Controls.Add(this.label21);
            this.Page_反馈.Controls.Add(this.反馈Button);
            this.Page_反馈.Controls.Add(this.txt_问题反馈);
            this.Page_反馈.Location = new System.Drawing.Point(4, 25);
            this.Page_反馈.Margin = new System.Windows.Forms.Padding(4);
            this.Page_反馈.Name = "Page_反馈";
            this.Page_反馈.Padding = new System.Windows.Forms.Padding(4);
            this.Page_反馈.Size = new System.Drawing.Size(523, 515);
            this.Page_反馈.TabIndex = 8;
            this.Page_反馈.Text = "反馈";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(9, 189);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(0, 15);
            this.label21.TabIndex = 12;
            // 
            // 反馈Button
            // 
            this.反馈Button.BackColor = System.Drawing.Color.White;
            this.反馈Button.Location = new System.Drawing.Point(412, 182);
            this.反馈Button.Margin = new System.Windows.Forms.Padding(4);
            this.反馈Button.Name = "反馈Button";
            this.反馈Button.Size = new System.Drawing.Size(100, 29);
            this.反馈Button.TabIndex = 11;
            this.反馈Button.Text = "提交";
            this.反馈Button.UseVisualStyleBackColor = false;
            this.反馈Button.Click += new System.EventHandler(this.反馈Button_Click);
            // 
            // txt_问题反馈
            // 
            this.txt_问题反馈.BackColor = System.Drawing.Color.White;
            this.txt_问题反馈.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_问题反馈.Location = new System.Drawing.Point(8, 8);
            this.txt_问题反馈.Margin = new System.Windows.Forms.Padding(4);
            this.txt_问题反馈.Multiline = true;
            this.txt_问题反馈.Name = "txt_问题反馈";
            this.txt_问题反馈.Size = new System.Drawing.Size(503, 167);
            this.txt_问题反馈.TabIndex = 5;
            // 
            // groupBox8
            // 
            this.groupBox8.Location = new System.Drawing.Point(117, 14);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(255, 83);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(13, 56);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(141, 17);
            this.label19.TabIndex = 5;
            // 
            // label_版本号
            // 
            this.label_版本号.AutoSize = true;
            this.label_版本号.Location = new System.Drawing.Point(13, 13);
            this.label_版本号.Name = "label_版本号";
            this.label_版本号.Size = new System.Drawing.Size(95, 12);
            this.label_版本号.TabIndex = 4;
            // 
            // label_更新日期
            // 
            this.label_更新日期.AutoSize = true;
            this.label_更新日期.Location = new System.Drawing.Point(13, 34);
            this.label_更新日期.Name = "label_更新日期";
            this.label_更新日期.Size = new System.Drawing.Size(125, 12);
            this.label_更新日期.TabIndex = 6;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.White;
            this.pictureBox6.Image = global::TrOCR.Properties.Resources.头像;
            this.pictureBox6.Location = new System.Drawing.Point(12, 15);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(84, 82);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 5;
            this.pictureBox6.TabStop = false;
            // 
            // txt_更新说明
            // 
            this.txt_更新说明.BackColor = System.Drawing.Color.White;
            this.txt_更新说明.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_更新说明.Location = new System.Drawing.Point(6, 20);
            this.txt_更新说明.Multiline = true;
            this.txt_更新说明.Name = "txt_更新说明";
            this.txt_更新说明.Size = new System.Drawing.Size(366, 155);
            this.txt_更新说明.TabIndex = 4;
            // 
            // tabPage12
            // 
            this.tabPage12.Location = new System.Drawing.Point(4, 25);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(515, 486);
            this.tabPage12.TabIndex = 11;
            this.tabPage12.Text = "Baidu2";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // groupBox49
            // 
            this.groupBox49.Location = new System.Drawing.Point(8, 8);
            this.groupBox49.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox49.Name = "groupBox49";
            this.groupBox49.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox49.Size = new System.Drawing.Size(493, 69);
            this.groupBox49.TabIndex = 1;
            this.groupBox49.TabStop = false;
            // 
            // textBox36
            // 
            this.textBox36.Location = new System.Drawing.Point(8, 25);
            this.textBox36.Margin = new System.Windows.Forms.Padding(4);
            this.textBox36.Name = "textBox36";
            this.textBox36.Size = new System.Drawing.Size(401, 25);
            this.textBox36.TabIndex = 0;
            // 
            // button28
            // 
            this.button28.Location = new System.Drawing.Point(419, 24);
            this.button28.Margin = new System.Windows.Forms.Padding(4);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(67, 29);
            this.button28.TabIndex = 1;
            this.button28.Text = "重置";
            this.button28.UseVisualStyleBackColor = true;
            // 
            // groupBox48
            // 
            this.groupBox48.Location = new System.Drawing.Point(8, 84);
            this.groupBox48.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox48.Name = "groupBox48";
            this.groupBox48.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox48.Size = new System.Drawing.Size(493, 69);
            this.groupBox48.TabIndex = 2;
            this.groupBox48.TabStop = false;
            // 
            // textBox35
            // 
            this.textBox35.Location = new System.Drawing.Point(8, 25);
            this.textBox35.Margin = new System.Windows.Forms.Padding(4);
            this.textBox35.Name = "textBox35";
            this.textBox35.Size = new System.Drawing.Size(401, 25);
            this.textBox35.TabIndex = 0;
            // 
            // button27
            // 
            this.button27.Location = new System.Drawing.Point(419, 24);
            this.button27.Margin = new System.Windows.Forms.Padding(4);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(67, 29);
            this.button27.TabIndex = 1;
            this.button27.Text = "重置";
            this.button27.UseVisualStyleBackColor = true;
            // 
            // groupBox47
            // 
            this.groupBox47.Location = new System.Drawing.Point(8, 160);
            this.groupBox47.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox47.Name = "groupBox47";
            this.groupBox47.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox47.Size = new System.Drawing.Size(493, 69);
            this.groupBox47.TabIndex = 3;
            this.groupBox47.TabStop = false;
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(111, 32);
            this.label53.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(232, 15);
            this.label53.TabIndex = 0;
            // 
            // tabPage11
            // 
            this.tabPage11.Location = new System.Drawing.Point(4, 25);
            this.tabPage11.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage11.Size = new System.Drawing.Size(515, 486);
            this.tabPage11.TabIndex = 10;
            this.tabPage11.Text = "彩云小译2";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // groupBox46
            // 
            this.groupBox46.Location = new System.Drawing.Point(8, 8);
            this.groupBox46.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox46.Name = "groupBox46";
            this.groupBox46.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox46.Size = new System.Drawing.Size(493, 69);
            this.groupBox46.TabIndex = 0;
            this.groupBox46.TabStop = false;
            // 
            // textBox34
            // 
            this.textBox34.Location = new System.Drawing.Point(8, 25);
            this.textBox34.Margin = new System.Windows.Forms.Padding(4);
            this.textBox34.Name = "textBox34";
            this.textBox34.Size = new System.Drawing.Size(401, 25);
            this.textBox34.TabIndex = 0;
            // 
            // button26
            // 
            this.button26.Location = new System.Drawing.Point(419, 24);
            this.button26.Margin = new System.Windows.Forms.Padding(4);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(67, 29);
            this.button26.TabIndex = 1;
            this.button26.Text = "重置";
            this.button26.UseVisualStyleBackColor = true;
            // 
            // groupBox45
            // 
            this.groupBox45.Location = new System.Drawing.Point(8, 84);
            this.groupBox45.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox45.Name = "groupBox45";
            this.groupBox45.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox45.Size = new System.Drawing.Size(493, 69);
            this.groupBox45.TabIndex = 1;
            this.groupBox45.TabStop = false;
            // 
            // textBox33
            // 
            this.textBox33.Location = new System.Drawing.Point(8, 25);
            this.textBox33.Margin = new System.Windows.Forms.Padding(4);
            this.textBox33.Name = "textBox33";
            this.textBox33.Size = new System.Drawing.Size(401, 25);
            this.textBox33.TabIndex = 0;
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(419, 24);
            this.button25.Margin = new System.Windows.Forms.Padding(4);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(67, 29);
            this.button25.TabIndex = 1;
            this.button25.Text = "重置";
            this.button25.UseVisualStyleBackColor = true;
            // 
            // groupBox44
            // 
            this.groupBox44.Location = new System.Drawing.Point(8, 160);
            this.groupBox44.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox44.Name = "groupBox44";
            this.groupBox44.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox44.Size = new System.Drawing.Size(493, 69);
            this.groupBox44.TabIndex = 2;
            this.groupBox44.TabStop = false;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(9, 29);
            this.label52.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(55, 15);
            this.label52.TabIndex = 0;
            // 
            // textBox32
            // 
            this.textBox32.Location = new System.Drawing.Point(107, 25);
            this.textBox32.Margin = new System.Windows.Forms.Padding(4);
            this.textBox32.Name = "textBox32";
            this.textBox32.Size = new System.Drawing.Size(377, 25);
            this.textBox32.TabIndex = 1;
            // 
            // tabPage10
            // 
            this.tabPage10.Location = new System.Drawing.Point(4, 25);
            this.tabPage10.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage10.Size = new System.Drawing.Size(515, 486);
            this.tabPage10.TabIndex = 9;
            this.tabPage10.Text = "火山翻译";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // groupBox43
            // 
            this.groupBox43.Location = new System.Drawing.Point(8, 8);
            this.groupBox43.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox43.Name = "groupBox43";
            this.groupBox43.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox43.Size = new System.Drawing.Size(493, 69);
            this.groupBox43.TabIndex = 0;
            this.groupBox43.TabStop = false;
            // 
            // textBox31
            // 
            this.textBox31.Location = new System.Drawing.Point(8, 25);
            this.textBox31.Margin = new System.Windows.Forms.Padding(4);
            this.textBox31.Name = "textBox31";
            this.textBox31.Size = new System.Drawing.Size(401, 25);
            this.textBox31.TabIndex = 0;
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(419, 24);
            this.button24.Margin = new System.Windows.Forms.Padding(4);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(67, 29);
            this.button24.TabIndex = 1;
            this.button24.Text = "重置";
            this.button24.UseVisualStyleBackColor = true;
            // 
            // groupBox42
            // 
            this.groupBox42.Location = new System.Drawing.Point(8, 84);
            this.groupBox42.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox42.Name = "groupBox42";
            this.groupBox42.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox42.Size = new System.Drawing.Size(493, 69);
            this.groupBox42.TabIndex = 1;
            this.groupBox42.TabStop = false;
            // 
            // textBox30
            // 
            this.textBox30.Location = new System.Drawing.Point(8, 25);
            this.textBox30.Margin = new System.Windows.Forms.Padding(4);
            this.textBox30.Name = "textBox30";
            this.textBox30.Size = new System.Drawing.Size(401, 25);
            this.textBox30.TabIndex = 0;
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(419, 24);
            this.button23.Margin = new System.Windows.Forms.Padding(4);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(67, 29);
            this.button23.TabIndex = 1;
            this.button23.Text = "重置";
            this.button23.UseVisualStyleBackColor = true;
            // 
            // groupBox41
            // 
            this.groupBox41.Location = new System.Drawing.Point(8, 160);
            this.groupBox41.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox41.Name = "groupBox41";
            this.groupBox41.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox41.Size = new System.Drawing.Size(493, 69);
            this.groupBox41.TabIndex = 2;
            this.groupBox41.TabStop = false;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(200, 31);
            this.label51.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(67, 15);
            this.label51.TabIndex = 0;
            // 
            // tabPage9
            // 
            this.tabPage9.Location = new System.Drawing.Point(4, 25);
            this.tabPage9.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage9.Size = new System.Drawing.Size(515, 486);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = "彩云小译";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // groupBox40
            // 
            this.groupBox40.Location = new System.Drawing.Point(8, 8);
            this.groupBox40.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox40.Name = "groupBox40";
            this.groupBox40.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox40.Size = new System.Drawing.Size(493, 69);
            this.groupBox40.TabIndex = 0;
            this.groupBox40.TabStop = false;
            // 
            // textBox29
            // 
            this.textBox29.Location = new System.Drawing.Point(8, 25);
            this.textBox29.Margin = new System.Windows.Forms.Padding(4);
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new System.Drawing.Size(401, 25);
            this.textBox29.TabIndex = 0;
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(419, 24);
            this.button22.Margin = new System.Windows.Forms.Padding(4);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(67, 29);
            this.button22.TabIndex = 1;
            this.button22.Text = "重置";
            this.button22.UseVisualStyleBackColor = true;
            // 
            // groupBox39
            // 
            this.groupBox39.Location = new System.Drawing.Point(8, 84);
            this.groupBox39.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox39.Name = "groupBox39";
            this.groupBox39.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox39.Size = new System.Drawing.Size(493, 69);
            this.groupBox39.TabIndex = 1;
            this.groupBox39.TabStop = false;
            // 
            // textBox28
            // 
            this.textBox28.Location = new System.Drawing.Point(8, 25);
            this.textBox28.Margin = new System.Windows.Forms.Padding(4);
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new System.Drawing.Size(401, 25);
            this.textBox28.TabIndex = 0;
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(419, 24);
            this.button21.Margin = new System.Windows.Forms.Padding(4);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(67, 29);
            this.button21.TabIndex = 1;
            this.button21.Text = "重置";
            this.button21.UseVisualStyleBackColor = true;
            // 
            // groupBox38
            // 
            this.groupBox38.Location = new System.Drawing.Point(8, 160);
            this.groupBox38.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox38.Name = "groupBox38";
            this.groupBox38.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox38.Size = new System.Drawing.Size(493, 69);
            this.groupBox38.TabIndex = 2;
            this.groupBox38.TabStop = false;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(200, 31);
            this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(67, 15);
            this.label50.TabIndex = 0;
            // 
            // tabPage8
            // 
            this.tabPage8.Location = new System.Drawing.Point(4, 25);
            this.tabPage8.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage8.Size = new System.Drawing.Size(515, 486);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "腾讯交互";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // groupBox37
            // 
            this.groupBox37.Location = new System.Drawing.Point(8, 8);
            this.groupBox37.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox37.Name = "groupBox37";
            this.groupBox37.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox37.Size = new System.Drawing.Size(493, 69);
            this.groupBox37.TabIndex = 0;
            this.groupBox37.TabStop = false;
            // 
            // textBox27
            // 
            this.textBox27.Location = new System.Drawing.Point(8, 25);
            this.textBox27.Margin = new System.Windows.Forms.Padding(4);
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new System.Drawing.Size(401, 25);
            this.textBox27.TabIndex = 0;
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(419, 24);
            this.button20.Margin = new System.Windows.Forms.Padding(4);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(67, 29);
            this.button20.TabIndex = 1;
            this.button20.Text = "重置";
            this.button20.UseVisualStyleBackColor = true;
            // 
            // groupBox36
            // 
            this.groupBox36.Location = new System.Drawing.Point(8, 84);
            this.groupBox36.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox36.Name = "groupBox36";
            this.groupBox36.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox36.Size = new System.Drawing.Size(493, 69);
            this.groupBox36.TabIndex = 1;
            this.groupBox36.TabStop = false;
            // 
            // textBox26
            // 
            this.textBox26.Location = new System.Drawing.Point(8, 25);
            this.textBox26.Margin = new System.Windows.Forms.Padding(4);
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new System.Drawing.Size(401, 25);
            this.textBox26.TabIndex = 0;
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(419, 24);
            this.button19.Margin = new System.Windows.Forms.Padding(4);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(67, 29);
            this.button19.TabIndex = 1;
            this.button19.Text = "重置";
            this.button19.UseVisualStyleBackColor = true;
            // 
            // groupBox35
            // 
            this.groupBox35.Location = new System.Drawing.Point(8, 160);
            this.groupBox35.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox35.Name = "groupBox35";
            this.groupBox35.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox35.Size = new System.Drawing.Size(493, 69);
            this.groupBox35.TabIndex = 2;
            this.groupBox35.TabStop = false;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(200, 31);
            this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(67, 15);
            this.label49.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Location = new System.Drawing.Point(4, 25);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(515, 486);
            this.tabPage7.TabIndex = 5;
            this.tabPage7.Text = "Yandex";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // groupBox34
            // 
            this.groupBox34.Location = new System.Drawing.Point(8, 8);
            this.groupBox34.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox34.Name = "groupBox34";
            this.groupBox34.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox34.Size = new System.Drawing.Size(493, 69);
            this.groupBox34.TabIndex = 0;
            this.groupBox34.TabStop = false;
            // 
            // textBox25
            // 
            this.textBox25.Location = new System.Drawing.Point(8, 25);
            this.textBox25.Margin = new System.Windows.Forms.Padding(4);
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new System.Drawing.Size(401, 25);
            this.textBox25.TabIndex = 0;
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(419, 24);
            this.button18.Margin = new System.Windows.Forms.Padding(4);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(67, 29);
            this.button18.TabIndex = 1;
            this.button18.Text = "重置";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // groupBox33
            // 
            this.groupBox33.Location = new System.Drawing.Point(8, 84);
            this.groupBox33.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox33.Name = "groupBox33";
            this.groupBox33.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox33.Size = new System.Drawing.Size(493, 69);
            this.groupBox33.TabIndex = 1;
            this.groupBox33.TabStop = false;
            // 
            // textBox24
            // 
            this.textBox24.Location = new System.Drawing.Point(8, 25);
            this.textBox24.Margin = new System.Windows.Forms.Padding(4);
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new System.Drawing.Size(401, 25);
            this.textBox24.TabIndex = 0;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(419, 24);
            this.button17.Margin = new System.Windows.Forms.Padding(4);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(67, 29);
            this.button17.TabIndex = 1;
            this.button17.Text = "重置";
            this.button17.UseVisualStyleBackColor = true;
            // 
            // groupBox32
            // 
            this.groupBox32.Location = new System.Drawing.Point(8, 160);
            this.groupBox32.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox32.Name = "groupBox32";
            this.groupBox32.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox32.Size = new System.Drawing.Size(493, 69);
            this.groupBox32.TabIndex = 2;
            this.groupBox32.TabStop = false;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(200, 31);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(67, 15);
            this.label48.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(515, 486);
            this.tabPage6.TabIndex = 4;
            this.tabPage6.Text = "Microsoft";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // groupBox31
            // 
            this.groupBox31.Location = new System.Drawing.Point(8, 8);
            this.groupBox31.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox31.Size = new System.Drawing.Size(493, 69);
            this.groupBox31.TabIndex = 0;
            this.groupBox31.TabStop = false;
            // 
            // textBox23
            // 
            this.textBox23.Location = new System.Drawing.Point(8, 25);
            this.textBox23.Margin = new System.Windows.Forms.Padding(4);
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new System.Drawing.Size(401, 25);
            this.textBox23.TabIndex = 0;
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(419, 24);
            this.button16.Margin = new System.Windows.Forms.Padding(4);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(67, 29);
            this.button16.TabIndex = 1;
            this.button16.Text = "重置";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // groupBox30
            // 
            this.groupBox30.Location = new System.Drawing.Point(8, 84);
            this.groupBox30.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox30.Size = new System.Drawing.Size(493, 69);
            this.groupBox30.TabIndex = 1;
            this.groupBox30.TabStop = false;
            // 
            // textBox22
            // 
            this.textBox22.Location = new System.Drawing.Point(8, 25);
            this.textBox22.Margin = new System.Windows.Forms.Padding(4);
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new System.Drawing.Size(401, 25);
            this.textBox22.TabIndex = 0;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(419, 24);
            this.button15.Margin = new System.Windows.Forms.Padding(4);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(67, 29);
            this.button15.TabIndex = 1;
            this.button15.Text = "重置";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // groupBox29
            // 
            this.groupBox29.Location = new System.Drawing.Point(8, 160);
            this.groupBox29.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox29.Size = new System.Drawing.Size(493, 69);
            this.groupBox29.TabIndex = 2;
            this.groupBox29.TabStop = false;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(200, 31);
            this.label47.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(67, 15);
            this.label47.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(515, 486);
            this.tabPage5.TabIndex = 6;
            this.tabPage5.Text = "Bing2";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox28
            // 
            this.groupBox28.Location = new System.Drawing.Point(8, 8);
            this.groupBox28.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox28.Size = new System.Drawing.Size(493, 69);
            this.groupBox28.TabIndex = 3;
            this.groupBox28.TabStop = false;
            // 
            // textBox21
            // 
            this.textBox21.Location = new System.Drawing.Point(8, 25);
            this.textBox21.Margin = new System.Windows.Forms.Padding(4);
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new System.Drawing.Size(401, 25);
            this.textBox21.TabIndex = 0;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(419, 24);
            this.button14.Margin = new System.Windows.Forms.Padding(4);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(67, 29);
            this.button14.TabIndex = 1;
            this.button14.Text = "重置";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // groupBox27
            // 
            this.groupBox27.Location = new System.Drawing.Point(8, 84);
            this.groupBox27.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox27.Size = new System.Drawing.Size(493, 69);
            this.groupBox27.TabIndex = 4;
            this.groupBox27.TabStop = false;
            // 
            // textBox20
            // 
            this.textBox20.Location = new System.Drawing.Point(8, 25);
            this.textBox20.Margin = new System.Windows.Forms.Padding(4);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(401, 25);
            this.textBox20.TabIndex = 0;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(419, 24);
            this.button13.Margin = new System.Windows.Forms.Padding(4);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(67, 29);
            this.button13.TabIndex = 1;
            this.button13.Text = "重置";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // groupBox26
            // 
            this.groupBox26.Location = new System.Drawing.Point(8, 160);
            this.groupBox26.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox26.Size = new System.Drawing.Size(493, 100);
            this.groupBox26.TabIndex = 5;
            this.groupBox26.TabStop = false;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(8, 25);
            this.label46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(267, 45);
            this.label46.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(515, 486);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Bing";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox25
            // 
            this.groupBox25.Location = new System.Drawing.Point(8, 8);
            this.groupBox25.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox25.Size = new System.Drawing.Size(493, 69);
            this.groupBox25.TabIndex = 0;
            this.groupBox25.TabStop = false;
            // 
            // textBox19
            // 
            this.textBox19.Location = new System.Drawing.Point(8, 25);
            this.textBox19.Margin = new System.Windows.Forms.Padding(4);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(401, 25);
            this.textBox19.TabIndex = 0;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(419, 24);
            this.button12.Margin = new System.Windows.Forms.Padding(4);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(67, 29);
            this.button12.TabIndex = 1;
            this.button12.Text = "重置";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // groupBox24
            // 
            this.groupBox24.Location = new System.Drawing.Point(8, 84);
            this.groupBox24.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox24.Size = new System.Drawing.Size(493, 69);
            this.groupBox24.TabIndex = 1;
            this.groupBox24.TabStop = false;
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(8, 25);
            this.textBox18.Margin = new System.Windows.Forms.Padding(4);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(401, 25);
            this.textBox18.TabIndex = 0;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(419, 24);
            this.button11.Margin = new System.Windows.Forms.Padding(4);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(67, 29);
            this.button11.TabIndex = 1;
            this.button11.Text = "重置";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // groupBox23
            // 
            this.groupBox23.Location = new System.Drawing.Point(8, 160);
            this.groupBox23.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox23.Size = new System.Drawing.Size(493, 69);
            this.groupBox23.TabIndex = 2;
            this.groupBox23.TabStop = false;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(200, 31);
            this.label45.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(67, 15);
            this.label45.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(515, 486);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tencent";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox22
            // 
            this.groupBox22.Location = new System.Drawing.Point(8, 8);
            this.groupBox22.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox22.Size = new System.Drawing.Size(493, 69);
            this.groupBox22.TabIndex = 3;
            this.groupBox22.TabStop = false;
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(8, 25);
            this.textBox17.Margin = new System.Windows.Forms.Padding(4);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(401, 25);
            this.textBox17.TabIndex = 0;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(419, 24);
            this.button10.Margin = new System.Windows.Forms.Padding(4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(67, 29);
            this.button10.TabIndex = 1;
            this.button10.Text = "重置";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // groupBox21
            // 
            this.groupBox21.Location = new System.Drawing.Point(8, 84);
            this.groupBox21.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox21.Size = new System.Drawing.Size(493, 69);
            this.groupBox21.TabIndex = 4;
            this.groupBox21.TabStop = false;
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(8, 25);
            this.textBox16.Margin = new System.Windows.Forms.Padding(4);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(401, 25);
            this.textBox16.TabIndex = 0;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(419, 24);
            this.button9.Margin = new System.Windows.Forms.Padding(4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(67, 29);
            this.button9.TabIndex = 1;
            this.button9.Text = "重置";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // groupBox20
            // 
            this.groupBox20.Location = new System.Drawing.Point(8, 160);
            this.groupBox20.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox20.Size = new System.Drawing.Size(493, 100);
            this.groupBox20.TabIndex = 5;
            this.groupBox20.TabStop = false;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(9, 29);
            this.label44.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(79, 15);
            this.label44.TabIndex = 0;
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(107, 25);
            this.textBox15.Margin = new System.Windows.Forms.Padding(4);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(377, 25);
            this.textBox15.TabIndex = 1;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(9, 62);
            this.label43.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(87, 15);
            this.label43.TabIndex = 2;
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(107, 59);
            this.textBox14.Margin = new System.Windows.Forms.Padding(4);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(377, 25);
            this.textBox14.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(515, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Baidu";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox19
            // 
            this.groupBox19.Location = new System.Drawing.Point(8, 8);
            this.groupBox19.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox19.Size = new System.Drawing.Size(493, 69);
            this.groupBox19.TabIndex = 3;
            this.groupBox19.TabStop = false;
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(8, 25);
            this.textBox13.Margin = new System.Windows.Forms.Padding(4);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(401, 25);
            this.textBox13.TabIndex = 0;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(419, 24);
            this.button8.Margin = new System.Windows.Forms.Padding(4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(67, 29);
            this.button8.TabIndex = 1;
            this.button8.Text = "重置";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // groupBox18
            // 
            this.groupBox18.Location = new System.Drawing.Point(8, 84);
            this.groupBox18.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox18.Size = new System.Drawing.Size(493, 69);
            this.groupBox18.TabIndex = 4;
            this.groupBox18.TabStop = false;
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(8, 25);
            this.textBox12.Margin = new System.Windows.Forms.Padding(4);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(401, 25);
            this.textBox12.TabIndex = 0;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(419, 24);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(67, 29);
            this.button7.TabIndex = 1;
            this.button7.Text = "重置";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            this.groupBox17.Location = new System.Drawing.Point(8, 160);
            this.groupBox17.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox17.Size = new System.Drawing.Size(493, 100);
            this.groupBox17.TabIndex = 5;
            this.groupBox17.TabStop = false;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(9, 29);
            this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(63, 15);
            this.label42.TabIndex = 0;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(107, 25);
            this.textBox11.Margin = new System.Windows.Forms.Padding(4);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(377, 25);
            this.textBox11.TabIndex = 1;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(9, 62);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(95, 15);
            this.label41.TabIndex = 2;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(107, 59);
            this.textBox10.Margin = new System.Windows.Forms.Padding(4);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(377, 25);
            this.textBox10.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(509, 480);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Google";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox16
            // 
            this.groupBox16.Location = new System.Drawing.Point(8, 8);
            this.groupBox16.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox16.Size = new System.Drawing.Size(493, 69);
            this.groupBox16.TabIndex = 0;
            this.groupBox16.TabStop = false;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(8, 25);
            this.textBox9.Margin = new System.Windows.Forms.Padding(4);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(401, 25);
            this.textBox9.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(419, 24);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(67, 29);
            this.button6.TabIndex = 1;
            this.button6.Text = "重置";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // groupBox15
            // 
            this.groupBox15.Location = new System.Drawing.Point(8, 84);
            this.groupBox15.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox15.Size = new System.Drawing.Size(493, 69);
            this.groupBox15.TabIndex = 1;
            this.groupBox15.TabStop = false;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(8, 25);
            this.textBox8.Margin = new System.Windows.Forms.Padding(4);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(401, 25);
            this.textBox8.TabIndex = 0;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(419, 24);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(67, 29);
            this.button5.TabIndex = 1;
            this.button5.Text = "重置";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Location = new System.Drawing.Point(8, 160);
            this.groupBox14.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox14.Size = new System.Drawing.Size(493, 69);
            this.groupBox14.TabIndex = 2;
            this.groupBox14.TabStop = false;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(200, 31);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(67, 15);
            this.label32.TabIndex = 0;
            // 
            // FmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(556, 560);
            this.Controls.Add(this.tab_标签);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Page_翻译接口.ResumeLayout(false);
            this.tabControl_Trans.ResumeLayout(false);
            this.tabPage_Google.ResumeLayout(false);
            this.groupBox_Google_Key.ResumeLayout(false);
            this.groupBox_Google_Key.PerformLayout();
            this.groupBox_Google_Target.ResumeLayout(false);
            this.groupBox_Google_Target.PerformLayout();
            this.groupBox_Google_Source.ResumeLayout(false);
            this.groupBox_Google_Source.PerformLayout();
            this.tabPage_Baidu.ResumeLayout(false);
            this.groupBox_Baidu_Key.ResumeLayout(false);
            this.groupBox_Baidu_Key.PerformLayout();
            this.groupBox_Baidu_Target.ResumeLayout(false);
            this.groupBox_Baidu_Target.PerformLayout();
            this.groupBox_Baidu_Source.ResumeLayout(false);
            this.groupBox_Baidu_Source.PerformLayout();
            this.tabPage_Tencent.ResumeLayout(false);
            this.groupBox_Tencent_Key.ResumeLayout(false);
            this.groupBox_Tencent_Key.PerformLayout();
            this.groupBox_Tencent_Target.ResumeLayout(false);
            this.groupBox_Tencent_Target.PerformLayout();
            this.groupBox_Tencent_Source.ResumeLayout(false);
            this.groupBox_Tencent_Source.PerformLayout();
            this.tabPage_Bing.ResumeLayout(false);
            this.groupBox_Bing_Key.ResumeLayout(false);
            this.groupBox_Bing_Key.PerformLayout();
            this.groupBox_Bing_Target.ResumeLayout(false);
            this.groupBox_Bing_Target.PerformLayout();
            this.groupBox_Bing_Source.ResumeLayout(false);
            this.groupBox_Bing_Source.PerformLayout();
            this.tabPage_Bing2.ResumeLayout(false);
            this.groupBox_Bing2_Key.ResumeLayout(false);
            this.groupBox_Bing2_Key.PerformLayout();
            this.groupBox_Bing2_Target.ResumeLayout(false);
            this.groupBox_Bing2_Target.PerformLayout();
            this.groupBox_Bing2_Source.ResumeLayout(false);
            this.groupBox_Bing2_Source.PerformLayout();
            this.tabPage_Microsoft.ResumeLayout(false);
            this.groupBox_Microsoft_Key.ResumeLayout(false);
            this.groupBox_Microsoft_Key.PerformLayout();
            this.groupBox_Microsoft_Target.ResumeLayout(false);
            this.groupBox_Microsoft_Target.PerformLayout();
            this.groupBox_Microsoft_Source.ResumeLayout(false);
            this.groupBox_Microsoft_Source.PerformLayout();
            this.tabPage_Yandex.ResumeLayout(false);
            this.groupBox_Yandex_Key.ResumeLayout(false);
            this.groupBox_Yandex_Key.PerformLayout();
            this.groupBox_Yandex_Target.ResumeLayout(false);
            this.groupBox_Yandex_Target.PerformLayout();
            this.groupBox_Yandex_Source.ResumeLayout(false);
            this.groupBox_Yandex_Source.PerformLayout();
            this.tabPage_TencentInteractive.ResumeLayout(false);
            this.groupBox_TencentInteractive_Key.ResumeLayout(false);
            this.groupBox_TencentInteractive_Key.PerformLayout();
            this.groupBox_TencentInteractive_Target.ResumeLayout(false);
            this.groupBox_TencentInteractive_Target.PerformLayout();
            this.groupBox_TencentInteractive_Source.ResumeLayout(false);
            this.groupBox_TencentInteractive_Source.PerformLayout();
            this.tabPage_Caiyun.ResumeLayout(false);
            this.groupBox_Caiyun_Key.ResumeLayout(false);
            this.groupBox_Caiyun_Key.PerformLayout();
            this.groupBox_Caiyun_Target.ResumeLayout(false);
            this.groupBox_Caiyun_Target.PerformLayout();
            this.groupBox_Caiyun_Source.ResumeLayout(false);
            this.groupBox_Caiyun_Source.PerformLayout();
            this.tabPage_Volcano.ResumeLayout(false);
            this.groupBox_Volcano_Key.ResumeLayout(false);
            this.groupBox_Volcano_Key.PerformLayout();
            this.groupBox_Volcano_Target.ResumeLayout(false);
            this.groupBox_Volcano_Target.PerformLayout();
            this.groupBox_Volcano_Source.ResumeLayout(false);
            this.groupBox_Volcano_Source.PerformLayout();
            this.tabPage_Caiyun2.ResumeLayout(false);
            this.groupBox_Caiyun2_Key.ResumeLayout(false);
            this.groupBox_Caiyun2_Key.PerformLayout();
            this.groupBox_Caiyun2_Target.ResumeLayout(false);
            this.groupBox_Caiyun2_Target.PerformLayout();
            this.groupBox_Caiyun2_Source.ResumeLayout(false);
            this.groupBox_Caiyun2_Source.PerformLayout();
            this.tabPage_Baidu2.ResumeLayout(false);
            this.groupBox_Baidu2_Key.ResumeLayout(false);
            this.groupBox_Baidu2_Key.PerformLayout();
            this.groupBox_Baidu2_Target.ResumeLayout(false);
            this.groupBox_Baidu2_Target.PerformLayout();
            this.groupBox_Baidu2_Source.ResumeLayout(false);
            this.groupBox_Baidu2_Source.PerformLayout();
            this.tabPage_白描接口.ResumeLayout(false);
            this.tabPage_白描接口.PerformLayout();
            this.Page_AI密钥.ResumeLayout(false);
            this.Page_AI密钥.PerformLayout();
            this.groupBox_AI_Help.ResumeLayout(false);
            this.groupBox_AI_Help.PerformLayout();
            this.tabControl_AI.ResumeLayout(false);
            this.tabPage_OpenAICompatible.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox51.ResumeLayout(false);
            this.groupBox51.PerformLayout();
            this.Page_显示的接口.ResumeLayout(false);
            this.groupBox_翻译接口.ResumeLayout(false);
            this.groupBox_翻译接口.PerformLayout();
            this.groupBox_Ocr.ResumeLayout(false);
            this.groupBox_Ocr.PerformLayout();
            this.Page_About.ResumeLayout(false);
            this.Page_About.PerformLayout();
            this.Page_代理.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.Page_密钥.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage_百度OCR.ResumeLayout(false);
            this.tabControl_BaiduApiType.ResumeLayout(false);
            this.inPage_百度接口.ResumeLayout(false);
            this.inPage_百度接口.PerformLayout();
            this.inPage_百度高精度接口.ResumeLayout(false);
            this.inPage_百度高精度接口.PerformLayout();
            this.inPage_百度表格.ResumeLayout(false);
            this.inPage_百度表格.PerformLayout();
            this.inPage_百度手写.ResumeLayout(false);
            this.inPage_百度手写.PerformLayout();
            this.tabPage_腾讯OCR.ResumeLayout(false);
            this.tabControl_TXApiType.ResumeLayout(false);
            this.inPage_腾讯接口.ResumeLayout(false);
            this.inPage_腾讯接口.PerformLayout();
            this.inPage_腾讯高精度接口.ResumeLayout(false);
            this.inPage_腾讯高精度接口.PerformLayout();
            this.inPage_腾讯表格v3.ResumeLayout(false);
            this.inPage_腾讯表格v3.PerformLayout();
            this.inPage_腾讯手写.ResumeLayout(false);
            this.inPage_腾讯手写.PerformLayout();
            this.inPage_PaddleOCR.ResumeLayout(false);
            this.inPage_PaddleOCR.PerformLayout();
            this.inPage_PaddleOCR2.ResumeLayout(false);
            this.inPage_PaddleOCR2.PerformLayout();
            this.inPage_RapidOCR.ResumeLayout(false);
            this.inPage_RapidOCR.PerformLayout();
            this.Page_快捷键.ResumeLayout(false);
            this.Page_快捷键.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_截图翻译)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_静默识别)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_输入翻译)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_识别界面)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_记录界面)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_翻译文本)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_文字识别)).EndInit();
            this.page_常规.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_help)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_记录)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_OcrWorkflow.ResumeLayout(false);
            this.groupBox_OcrWorkflow.PerformLayout();
            this.groupBox_TranslateWorkflow.ResumeLayout(false);
            this.groupBox_TranslateWorkflow.PerformLayout();
            this.tab_标签.ResumeLayout(false);
            this.Page_常规2.ResumeLayout(false);
            this.groupBox50.ResumeLayout(false);
            this.groupBox50.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.Page_AI翻译接口.ResumeLayout(false);
            this.Page_AI翻译接口.PerformLayout();
            this.groupBox_AI_Trans_Help.ResumeLayout(false);
            this.groupBox_AI_Trans_Help.PerformLayout();
            this.tabControl_AI_Trans.ResumeLayout(false);
            this.tabPage_Trans_OpenAICompatible.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox52.ResumeLayout(false);
            this.groupBox52.PerformLayout();
            this.Page_工具栏.ResumeLayout(false);
            this.groupBox_拆分文本.ResumeLayout(false);
            this.tableLayoutPanel_拆分文本.ResumeLayout(false);
            this.tableLayoutPanel_拆分文本.PerformLayout();
            this.groupBox_合并文本.ResumeLayout(false);
            this.tableLayoutPanel_合并文本.ResumeLayout(false);
            this.tableLayoutPanel_合并文本.PerformLayout();
            this.Page_更新.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_间隔时间)).EndInit();
            this.Page_webdav.ResumeLayout(false);
            this.Page_webdav.PerformLayout();
            this.Page_反馈.ResumeLayout(false);
            this.Page_反馈.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabPage Page_AI密钥;
        private System.Windows.Forms.TabControl tabControl_AI;
        private System.Windows.Forms.TabPage tabPage_OpenAICompatible;
        private System.Windows.Forms.TabPage Page_显示的接口;
        private System.Windows.Forms.GroupBox groupBox_翻译接口;
        private System.Windows.Forms.GroupBox groupBox_Ocr;
        private System.Windows.Forms.CheckBox checkBox_ShowGoogle;
        private System.Windows.Forms.CheckBox checkBox_ShowBaidu;
        private System.Windows.Forms.CheckBox checkBox_ShowTencent;
        private System.Windows.Forms.CheckBox checkBox_ShowBing;
        private System.Windows.Forms.CheckBox checkBox_ShowBing2;
        private System.Windows.Forms.CheckBox checkBox_ShowMicrosoft;
        private System.Windows.Forms.CheckBox checkBox_ShowYandex;
        private System.Windows.Forms.CheckBox checkBox_ShowTencentInteractive;
        private System.Windows.Forms.CheckBox checkBox_ShowCaiyun;
        private System.Windows.Forms.CheckBox checkBox_ShowVolcano;
        private System.Windows.Forms.CheckBox checkBox_ShowCaiyun2;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrBaidu;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrBaiduAccurate;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrTencent;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrTencentAccurate;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrBaimiao;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrSougou;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrYoudao;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrWeChat;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrMathfuntion;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrTable;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrShupai;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrTableBaidu;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrTableAli;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrShupaiLR;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrShupaiRL;

        private global::System.ComponentModel.IContainer components;
        private global::System.Windows.Forms.TabPage Page_代理;
        private global::System.Windows.Forms.TabPage Page_密钥;
        private global::System.Windows.Forms.TabPage Page_快捷键;
        private global::System.Windows.Forms.TabPage page_常规;
        private global::System.Windows.Forms.PictureBox pic_help;
        private global::System.Windows.Forms.Button 常规Button;
        private global::System.Windows.Forms.GroupBox groupBox2;
        private global::System.Windows.Forms.NumericUpDown numbox_记录;
        private global::System.Windows.Forms.Label label3;
        private global::System.Windows.Forms.ComboBox cobBox_动画;
        private global::System.Windows.Forms.Label label2;
        private global::System.Windows.Forms.GroupBox groupBox1;
        private global::System.Windows.Forms.CheckBox cbBox_开机;
        private global::System.Windows.Forms.TabControl tab_标签;
        private global::System.Windows.Forms.TabPage Page_更新;
        private global::System.Windows.Forms.GroupBox groupBox3;
        private global::System.Windows.Forms.PictureBox pictureBox_翻译文本;
        private global::System.Windows.Forms.PictureBox pictureBox_文字识别;
        private global::System.Windows.Forms.TextBox txtBox_识别界面;
        private global::System.Windows.Forms.TextBox txtBox_记录界面;
        private global::System.Windows.Forms.TextBox txtBox_翻译文本;
        private global::System.Windows.Forms.TextBox txtBox_文字识别;
        private global::System.Windows.Forms.Label label7;
        private global::System.Windows.Forms.Label label6;
        private global::System.Windows.Forms.Label label5;
        private global::System.Windows.Forms.Label label4;
        private global::System.Windows.Forms.Label label_输入翻译;
        private global::System.Windows.Forms.PictureBox pictureBox_识别界面;
        private global::System.Windows.Forms.PictureBox pictureBox_记录界面;
        private global::System.Windows.Forms.PictureBox pictureBox_输入翻译;
        private global::System.Windows.Forms.TextBox txtBox_输入翻译;
        private global::System.Windows.Forms.Label label_静默识别; 
        private global::System.Windows.Forms.TextBox txtBox_静默识别; 
        private global::System.Windows.Forms.PictureBox pictureBox_静默识别; 
        private global::System.Windows.Forms.Label label8;
        private global::System.Windows.Forms.CheckBox cbBox_翻译;
        private global::System.Windows.Forms.TabControl tabControl2;
        private global::System.Windows.Forms.Button 快捷键Button;
        private global::System.Windows.Forms.Button 密钥Button_apply;
        private global::System.Windows.Forms.Button 密钥Button;
        private global::System.Windows.Forms.GroupBox groupBox4;
        private global::System.Windows.Forms.ComboBox combox_代理;
        private global::System.Windows.Forms.Label label11;
        private global::System.Windows.Forms.Button 代理Button;
        private global::System.Windows.Forms.CheckBox chbox_代理服务器;
        private global::System.Windows.Forms.TextBox text_密码;
        private global::System.Windows.Forms.TextBox text_端口;
        private global::System.Windows.Forms.Label label15;
        private global::System.Windows.Forms.TextBox text_账号;
        private global::System.Windows.Forms.TextBox text_服务器;
        private global::System.Windows.Forms.Label label14;
        private global::System.Windows.Forms.Label label13;
        private global::System.Windows.Forms.Label label12;
        private global::System.Windows.Forms.Button 更新Button_check;
        private global::System.Windows.Forms.Button 更新Button;
        private global::System.Windows.Forms.GroupBox groupBox5;
        private global::System.Windows.Forms.Label label16;
        private global::System.Windows.Forms.NumericUpDown numbox_间隔时间;
        private global::System.Windows.Forms.CheckBox checkBox_更新间隔;
        private global::System.Windows.Forms.CheckBox check_检查更新;
        private System.Windows.Forms.CheckBox checkBox_PreRelease;
        private global::System.Windows.Forms.CheckBox cbBox_弹窗;
        private global::System.Windows.Forms.GroupBox groupBox6;
        private global::System.Windows.Forms.TextBox textBox_path;
        private global::System.Windows.Forms.Label label1;
        private global::System.Windows.Forms.CheckBox cbBox_保存;
        private global::System.Windows.Forms.Button btn_浏览;
        private global::System.Windows.Forms.Button 百度_btn;
        private global::System.Windows.Forms.GroupBox groupBox10;
        private global::System.Windows.Forms.Button btn_音效;
        private global::System.Windows.Forms.TextBox text_音效path;
        private global::System.Windows.Forms.Label label18;
        private global::System.Windows.Forms.CheckBox chbox_save;
        private global::System.Windows.Forms.CheckBox chbox_copy;
        private global::System.Windows.Forms.Label label20;
        private global::System.Windows.Forms.Button btn_音效路径;
        private global::System.Windows.Forms.CheckBox chbox_取色;
        private System.Windows.Forms.TabPage Page_反馈;
        private System.Windows.Forms.TabPage Page_About;
        private System.Windows.Forms.TabPage Page_翻译接口;
        private System.Windows.Forms.Label label_VersionInfo;
        private System.Windows.Forms.Label label_AuthorInfo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button 反馈Button;
        private System.Windows.Forms.TextBox txt_问题反馈;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label_版本号;
        private System.Windows.Forms.Label label_更新日期;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.TextBox txt_更新说明;
        private System.Windows.Forms.TextBox BoxTencentKey;
        private System.Windows.Forms.TextBox BoxTencentId;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TabControl tabControl_Trans;
        private System.Windows.Forms.TabPage tabPage_Google;
        private System.Windows.Forms.TabPage tabPage_Baidu;
        private System.Windows.Forms.TabPage tabPage_Tencent;
        private System.Windows.Forms.TabPage tabPage_Bing;
        private System.Windows.Forms.TabPage tabPage_Bing2;
        private System.Windows.Forms.TabPage tabPage_Microsoft;
        private System.Windows.Forms.TabPage tabPage_Yandex;
        private System.Windows.Forms.GroupBox groupBox_Google_Key;
        private System.Windows.Forms.Label label_Google_Key;
        private System.Windows.Forms.GroupBox groupBox_Google_Target;
        private System.Windows.Forms.TextBox textBox_Google_Target;
        private System.Windows.Forms.GroupBox groupBox_Google_Source;
        private System.Windows.Forms.TextBox textBox_Google_Source;
        private System.Windows.Forms.GroupBox groupBox_Baidu_Key;
        private System.Windows.Forms.TextBox textBox_Baidu_SK;
        private System.Windows.Forms.Label label_Baidu_SK;
        private System.Windows.Forms.TextBox textBox_Baidu_AK;
        private System.Windows.Forms.Label label_Baidu_AK;
        private System.Windows.Forms.GroupBox groupBox_Baidu_Target;
        private System.Windows.Forms.TextBox textBox_Baidu_Target;
        private System.Windows.Forms.GroupBox groupBox_Baidu_Source;
        private System.Windows.Forms.TextBox textBox_Baidu_Source;
        private System.Windows.Forms.GroupBox groupBox_Tencent_Key;
        private System.Windows.Forms.TextBox textBox_Tencent_SK;
        private System.Windows.Forms.Label label_Tencent_SK;
        private System.Windows.Forms.TextBox textBox_Tencent_AK;
        private System.Windows.Forms.Label label_Tencent_AK;
        private System.Windows.Forms.GroupBox groupBox_Tencent_Target;
        private System.Windows.Forms.TextBox textBox_Tencent_Target;
        private System.Windows.Forms.GroupBox groupBox_Tencent_Source;
        private System.Windows.Forms.TextBox textBox_Tencent_Source;
        private System.Windows.Forms.GroupBox groupBox_Bing_Key;
        private System.Windows.Forms.Label label_Bing_Key;
        private System.Windows.Forms.GroupBox groupBox_Bing_Target;
        private System.Windows.Forms.TextBox textBox_Bing_Target;
        private System.Windows.Forms.GroupBox groupBox_Bing_Source;
        private System.Windows.Forms.TextBox textBox_Bing_Source;
        private System.Windows.Forms.GroupBox groupBox_Bing2_Key;
        private System.Windows.Forms.Label label_Bing2_Notice;
        private System.Windows.Forms.GroupBox groupBox_Bing2_Target;
        private System.Windows.Forms.TextBox textBox_Bing2_Target;
        private System.Windows.Forms.GroupBox groupBox_Bing2_Source;
        private System.Windows.Forms.TextBox textBox_Bing2_Source;
        private System.Windows.Forms.GroupBox groupBox_Microsoft_Key;
        private System.Windows.Forms.Label label_Microsoft_Key;
        private System.Windows.Forms.GroupBox groupBox_Microsoft_Target;
        private System.Windows.Forms.TextBox textBox_Microsoft_Target;
        private System.Windows.Forms.GroupBox groupBox_Microsoft_Source;
        private System.Windows.Forms.TextBox textBox_Microsoft_Source;
        private System.Windows.Forms.GroupBox groupBox_Yandex_Key;
        private System.Windows.Forms.Label label_Yandex_Key;
        private System.Windows.Forms.GroupBox groupBox_Yandex_Target;
        private System.Windows.Forms.TextBox textBox_Yandex_Target;
        private System.Windows.Forms.GroupBox groupBox_Yandex_Source;
        private System.Windows.Forms.TextBox textBox_Yandex_Source;
        private System.Windows.Forms.Button btn_Reset_Google_Source;
        private System.Windows.Forms.Button btn_Reset_Google_Target;
        private System.Windows.Forms.Button btn_Reset_Baidu_Source;
        private System.Windows.Forms.Button btn_Reset_Baidu_Target;
        private System.Windows.Forms.Button btn_Reset_Tencent_Source;
        private System.Windows.Forms.Button btn_Reset_Tencent_Target;
        private System.Windows.Forms.Button btn_Reset_Bing_Source;
        private System.Windows.Forms.Button btn_Reset_Bing_Target;
        private System.Windows.Forms.Button btn_Reset_Bing2_Source;
        private System.Windows.Forms.Button btn_Reset_Bing2_Target;
        private System.Windows.Forms.Button btn_Reset_Microsoft_Source;
        private System.Windows.Forms.Button btn_Reset_Microsoft_Target;
        private System.Windows.Forms.Button btn_Reset_Yandex_Source;
        private System.Windows.Forms.Button btn_Reset_Yandex_Target;
        private System.Windows.Forms.TabPage tabPage_TencentInteractive;
        private System.Windows.Forms.GroupBox groupBox_TencentInteractive_Source;
        private System.Windows.Forms.GroupBox groupBox_TencentInteractive_Target;
        private System.Windows.Forms.GroupBox groupBox_TencentInteractive_Key;
        private System.Windows.Forms.TextBox textBox_TencentInteractive_Source;
        private System.Windows.Forms.TextBox textBox_TencentInteractive_Target;
        private System.Windows.Forms.Button btn_Reset_TencentInteractive_Source;
        private System.Windows.Forms.Button btn_Reset_TencentInteractive_Target;
        private System.Windows.Forms.Label label_TencentInteractive_Key;
        private System.Windows.Forms.TabPage tabPage_Caiyun;
        private System.Windows.Forms.GroupBox groupBox_Caiyun_Source;
        private System.Windows.Forms.GroupBox groupBox_Caiyun_Target;
        private System.Windows.Forms.GroupBox groupBox_Caiyun_Key;
        private System.Windows.Forms.TextBox textBox_Caiyun_Source;
        private System.Windows.Forms.TextBox textBox_Caiyun_Target;
        private System.Windows.Forms.Button btn_Reset_Caiyun_Source;
        private System.Windows.Forms.Button btn_Reset_Caiyun_Target;
        private System.Windows.Forms.Label label_Caiyun_Key;
        private System.Windows.Forms.TabPage tabPage_Volcano;
        private System.Windows.Forms.GroupBox groupBox_Volcano_Source;
        private System.Windows.Forms.GroupBox groupBox_Volcano_Target;
        private System.Windows.Forms.GroupBox groupBox_Volcano_Key;
        private System.Windows.Forms.TextBox textBox_Volcano_Source;
        private System.Windows.Forms.TextBox textBox_Volcano_Target;
        private System.Windows.Forms.Button btn_Reset_Volcano_Source;
        private System.Windows.Forms.Button btn_Reset_Volcano_Target;
        private System.Windows.Forms.Label label_Volcano_Key;
        private System.Windows.Forms.TabPage tabPage_Caiyun2;
        private System.Windows.Forms.GroupBox groupBox_Caiyun2_Source;
        private System.Windows.Forms.Button btn_Reset_Caiyun2_Source;
        private System.Windows.Forms.TextBox textBox_Caiyun2_Source;
        private System.Windows.Forms.GroupBox groupBox_Caiyun2_Target;
        private System.Windows.Forms.Button btn_Reset_Caiyun2_Target;
        private System.Windows.Forms.TextBox textBox_Caiyun2_Target;
        private System.Windows.Forms.GroupBox groupBox_Caiyun2_Key;
        private System.Windows.Forms.TextBox textBox_Caiyun2_Token;
        private System.Windows.Forms.Label label_Caiyun2_Token;
        private System.Windows.Forms.TabPage tabPage_白描接口;
        private System.Windows.Forms.TextBox BoxBaimiaoPassword;
        private System.Windows.Forms.TextBox BoxBaimiaoUsername;
        private System.Windows.Forms.Label label_BaimiaoPassword;
        private System.Windows.Forms.Label label_BaimiaoUsername;
        private System.Windows.Forms.TextBox text_baidu_accurate_secretkey;
        private System.Windows.Forms.TextBox text_baidu_accurate_apikey;
        private System.Windows.Forms.Label label_baidu_accurate_secretkey;
        private System.Windows.Forms.Label label_baidu_accurate_apikey;
        private System.Windows.Forms.ComboBox comboBox_Baidu_Accurate_Language;
        private System.Windows.Forms.Label label_Baidu_Accurate_Language;
        private System.Windows.Forms.ComboBox comboBox_Tencent_Language;
        private System.Windows.Forms.Label label_Tencent_Language;
        private System.Windows.Forms.TextBox text_tencent_accurate_secretkey;
        private System.Windows.Forms.TextBox text_tencent_accurate_secretid;
        private System.Windows.Forms.Label label_tencent_accurate_secretkey;
        private System.Windows.Forms.Label label_tencent_accurate_secretid;
        private System.Windows.Forms.ComboBox comboBox_Tencent_Accurate_Language;
        private System.Windows.Forms.Label label_Tencent_Accurate_Language;
        private System.Windows.Forms.GroupBox groupBox_OcrWorkflow;
        private System.Windows.Forms.CheckBox checkBox_AutoCopyOcrResult;
        private System.Windows.Forms.CheckBox checkBox_AutoTranslateOcrResult;
        private System.Windows.Forms.GroupBox groupBox_TranslateWorkflow;
        private System.Windows.Forms.CheckBox checkBox_AutoCopyOcrTranslation;
        private System.Windows.Forms.TabPage tabPage_百度OCR;
        private System.Windows.Forms.TabControl tabControl_BaiduApiType;
        private System.Windows.Forms.TabPage inPage_百度接口;
        private System.Windows.Forms.TabPage inPage_百度高精度接口;
        private System.Windows.Forms.ComboBox comboBox_Baidu_Language;
        private System.Windows.Forms.TextBox text_baidupassword;
        private System.Windows.Forms.TextBox text_baiduaccount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPage_腾讯OCR;
        private System.Windows.Forms.TabControl tabControl_TXApiType;
        private System.Windows.Forms.TabPage inPage_腾讯接口;
        private System.Windows.Forms.TabPage inPage_腾讯高精度接口;
        private System.Windows.Forms.TabPage Page_工具栏;
        private System.Windows.Forms.GroupBox groupBox_合并文本;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_合并文本;
        private System.Windows.Forms.CheckBox checkBox_合并自动复制;
        private System.Windows.Forms.CheckBox checkBox_合并去除空格;
        private System.Windows.Forms.GroupBox groupBox_拆分文本;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_拆分文本;
        private System.Windows.Forms.CheckBox checkBox_拆分后自动复制;
        private System.Windows.Forms.TabPage inPage_百度表格;
        private System.Windows.Forms.Label label_Baidu_Language;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage inPage_腾讯表格v3;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage inPage_PaddleOCR;
        private System.Windows.Forms.TabPage inPage_PaddleOCR2;
        private System.Windows.Forms.TabPage inPage_RapidOCR;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrTableTencent;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrPaddleOCR;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrPaddleOCR2;
        private System.Windows.Forms.CheckBox checkBox_ShowOcrRapidOCR;
        
        // PaddleOCR配置控件
        private System.Windows.Forms.GroupBox groupBox_PaddleOCR_Config;
        private System.Windows.Forms.Label label_PaddleOCR_Det;
        private System.Windows.Forms.TextBox textBox_PaddleOCR_Det;
        private System.Windows.Forms.Button btn_PaddleOCR_Det_Browse;
        private System.Windows.Forms.Label label_PaddleOCR_Cls;
        private System.Windows.Forms.TextBox textBox_PaddleOCR_Cls;
        private System.Windows.Forms.Button btn_PaddleOCR_Cls_Browse;
        private System.Windows.Forms.Label label_PaddleOCR_Rec;
        private System.Windows.Forms.TextBox textBox_PaddleOCR_Rec;
        private System.Windows.Forms.Button btn_PaddleOCR_Rec_Browse;
        private System.Windows.Forms.Label label_PaddleOCR_Keys;
        private System.Windows.Forms.TextBox textBox_PaddleOCR_Keys;
        private System.Windows.Forms.Button btn_PaddleOCR_Keys_Browse;
        private System.Windows.Forms.Label label_PaddleOCR_Status;
        
        // PaddleOCR2配置控件
        private System.Windows.Forms.GroupBox groupBox_PaddleOCR2_Config;
        private System.Windows.Forms.Label label_PaddleOCR2_Det;
        private System.Windows.Forms.TextBox textBox_PaddleOCR2_Det;
        private System.Windows.Forms.Button btn_PaddleOCR2_Det_Browse;
        private System.Windows.Forms.Label label_PaddleOCR2_Cls;
        private System.Windows.Forms.TextBox textBox_PaddleOCR2_Cls;
        private System.Windows.Forms.Button btn_PaddleOCR2_Cls_Browse;
        private System.Windows.Forms.Label label_PaddleOCR2_Rec;
        private System.Windows.Forms.TextBox textBox_PaddleOCR2_Rec;
        private System.Windows.Forms.Button btn_PaddleOCR2_Rec_Browse;
        private System.Windows.Forms.Label label_PaddleOCR2_Keys;
        private System.Windows.Forms.TextBox textBox_PaddleOCR2_Keys;
        private System.Windows.Forms.Button btn_PaddleOCR2_Keys_Browse;
        private System.Windows.Forms.Label label_PaddleOCR2_det_Version;
        private System.Windows.Forms.ComboBox comboBox_PaddleOCR2_det_Version;
        private System.Windows.Forms.Label label_PaddleOCR2_Status;
        
        // RapidOCR配置控件
        private System.Windows.Forms.GroupBox groupBox_RapidOCR_Config;
        private System.Windows.Forms.Label label_RapidOCR_Det;
        private System.Windows.Forms.TextBox textBox_RapidOCR_Det;
        private System.Windows.Forms.Button btn_RapidOCR_Det_Browse;
        private System.Windows.Forms.Label label_RapidOCR_Cls;
        private System.Windows.Forms.TextBox textBox_RapidOCR_Cls;
        private System.Windows.Forms.Button btn_RapidOCR_Cls_Browse;
        private System.Windows.Forms.Label label_RapidOCR_Rec;
        private System.Windows.Forms.TextBox textBox_RapidOCR_Rec;
        private System.Windows.Forms.Button btn_RapidOCR_Rec_Browse;
        private System.Windows.Forms.Label label_RapidOCR_Keys;
        private System.Windows.Forms.TextBox textBox_RapidOCR_Keys;
        private System.Windows.Forms.Button btn_RapidOCR_Keys_Browse;
        private System.Windows.Forms.Label label_RapidOCR_Status;
        private System.Windows.Forms.ComboBox comboBox_PaddleOCR2_rec_Version;
        private System.Windows.Forms.ComboBox comboBox_PaddleOCR2_cls_Version;
        private System.Windows.Forms.Label label_PaddleOCR2_rec_Version;
        private System.Windows.Forms.Label label_PaddleOCR2_cls_Version;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label_OcrApiHelpText;
        private System.Windows.Forms.TabPage Page_常规2;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox cbBox_ListenClipboard;
        private System.Windows.Forms.CheckBox cbBox_AutoCopyListenClipboardTranslation;
        private System.Windows.Forms.TabPage inPage_百度手写;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.CheckBox cbBox_ListenHideOriginal;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage inPage_腾讯手写;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.CheckBox cbBox_禁用隐藏原文按钮;
        private System.Windows.Forms.CheckBox checkBox_EnableTabHoverSwitch;
        private System.Windows.Forms.CheckBox checkbox_AutoCopyScreenshotTranslation;
        private System.Windows.Forms.CheckBox checkbox_NoWindowScreenshotTranslation;
        private System.Windows.Forms.PictureBox pictureBox_截图翻译;
        private System.Windows.Forms.TextBox txtBox_截图翻译;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.CheckBox checkBox_AutoCopyInputTranslation;
        private System.Windows.Forms.CheckBox cbBox_输入翻译自动翻译;
        private System.Windows.Forms.CheckBox cbBox_输入翻译剪贴板;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox text_baidu_handwriting_secretkey;
        private System.Windows.Forms.TextBox text_baidu_handwriting_apikey;
        private System.Windows.Forms.ComboBox comboBox_Baidu_Handwriting_Language;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TabPage tabPage_Baidu2;
        private System.Windows.Forms.GroupBox groupBox_Baidu2_Key;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox_Baidu2_Target;
        private System.Windows.Forms.Button btn_Reset_Baidu2_Target;
        private System.Windows.Forms.TextBox textBox_Baidu2_Target;
        private System.Windows.Forms.GroupBox groupBox_Baidu2_Source;
        private System.Windows.Forms.Button btn_Reset_Baidu2_Source;
        private System.Windows.Forms.TextBox textBox_Baidu2_Source;
        private System.Windows.Forms.CheckBox checkBox_ShowBaidu2;
        private System.Windows.Forms.CheckBox checkBox_合并去除所有空格;
        private System.Windows.Forms.GroupBox groupBox_AI_Help;
        private System.Windows.Forms.Label label_AI_Help;
        private System.Windows.Forms.TabPage Page_AI翻译接口;
        private System.Windows.Forms.TabControl tabControl_AI_Trans;
        private System.Windows.Forms.TabPage tabPage_Trans_OpenAICompatible;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.GroupBox groupBox49;
        private System.Windows.Forms.TextBox textBox36;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.GroupBox groupBox48;
        private System.Windows.Forms.TextBox textBox35;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.GroupBox groupBox47;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.GroupBox groupBox46;
        private System.Windows.Forms.TextBox textBox34;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.GroupBox groupBox45;
        private System.Windows.Forms.TextBox textBox33;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.GroupBox groupBox44;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.TextBox textBox32;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.GroupBox groupBox43;
        private System.Windows.Forms.TextBox textBox31;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.GroupBox groupBox42;
        private System.Windows.Forms.TextBox textBox30;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.GroupBox groupBox41;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.GroupBox groupBox40;
        private System.Windows.Forms.TextBox textBox29;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.GroupBox groupBox39;
        private System.Windows.Forms.TextBox textBox28;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.GroupBox groupBox38;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.GroupBox groupBox37;
        private System.Windows.Forms.TextBox textBox27;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.GroupBox groupBox36;
        private System.Windows.Forms.TextBox textBox26;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.GroupBox groupBox35;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.GroupBox groupBox34;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.GroupBox groupBox33;
        private System.Windows.Forms.TextBox textBox24;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.GroupBox groupBox32;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.GroupBox groupBox31;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.GroupBox groupBox30;
        private System.Windows.Forms.TextBox textBox22;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.GroupBox groupBox29;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.GroupBox groupBox_AI_Trans_Help;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.GroupBox groupBox50;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.TextBox textBox38;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.TextBox textBox37;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lb_CustomProviders;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Del_Provider;
        private System.Windows.Forms.Button btn_Add_Provider;
        private System.Windows.Forms.GroupBox groupBox51;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.TextBox txt_ConfigPath;
        private System.Windows.Forms.TextBox txt_ModelName;
        private System.Windows.Forms.TextBox txt_ApiKey;
        private System.Windows.Forms.TextBox txt_ApiUrl;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.Button btn_BrowseConfig;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_Trans_Del_Provider;
        private System.Windows.Forms.Button btn_Trans_Add_Provider;
        private System.Windows.Forms.ListBox lb_CustomTransProviders;
        private System.Windows.Forms.GroupBox groupBox52;
        private System.Windows.Forms.TextBox txt_Trans_ConfigPath;
        private System.Windows.Forms.TextBox txt_Trans_ModelName;
        private System.Windows.Forms.TextBox txt_Trans_ApiKey;
        private System.Windows.Forms.TextBox txt_Trans_ApiUrl;
        private System.Windows.Forms.TextBox txt_Trans_Name;
        private System.Windows.Forms.Button btn_Trans_BrowseConfig;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.TextBox txt_Trans_Target;
        private System.Windows.Forms.TextBox txt_Trans_Source;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.TabPage Page_webdav;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.TextBox txtWebDavUser;
        private System.Windows.Forms.TextBox txtWebDavUrl;
        private System.Windows.Forms.Button btnDownloadConfig;
        private System.Windows.Forms.Button btnUploadConfig;
        private System.Windows.Forms.TextBox txtWebDavPass;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.TextBox textBox39;
    }
}
