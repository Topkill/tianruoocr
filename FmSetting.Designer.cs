namespace TrOCR
{

    public sealed partial class FmSetting : global::System.Windows.Forms.Form
    {

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            //翻译接口设置页
            this.tabControl_Trans = new System.Windows.Forms.TabControl();
            this.tabPage_Google = new System.Windows.Forms.TabPage();
            this.groupBox_Google_Key = new System.Windows.Forms.GroupBox();
            this.label_Google_Key = new System.Windows.Forms.Label();
            this.groupBox_Google_Target = new System.Windows.Forms.GroupBox();
            this.textBox_Google_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Google_Source = new System.Windows.Forms.GroupBox();
            this.textBox_Google_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Baidu = new System.Windows.Forms.TabPage();
            this.groupBox_Baidu_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Baidu_SK = new System.Windows.Forms.TextBox();
            this.label_Baidu_SK = new System.Windows.Forms.Label();
            this.textBox_Baidu_AK = new System.Windows.Forms.TextBox();
            this.label_Baidu_AK = new System.Windows.Forms.Label();
            this.groupBox_Baidu_Target = new System.Windows.Forms.GroupBox();
            this.textBox_Baidu_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Baidu_Source = new System.Windows.Forms.GroupBox();
            this.textBox_Baidu_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Tencent = new System.Windows.Forms.TabPage();
            this.groupBox_Tencent_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Tencent_SK = new System.Windows.Forms.TextBox();
            this.label_Tencent_SK = new System.Windows.Forms.Label();
            this.textBox_Tencent_AK = new System.Windows.Forms.TextBox();
            this.label_Tencent_AK = new System.Windows.Forms.Label();
            this.groupBox_Tencent_Target = new System.Windows.Forms.GroupBox();
            this.textBox_Tencent_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Tencent_Source = new System.Windows.Forms.GroupBox();
            this.textBox_Tencent_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Bing = new System.Windows.Forms.TabPage();
            this.groupBox_Bing_Key = new System.Windows.Forms.GroupBox();
            this.label_Bing_Key = new System.Windows.Forms.Label();
            this.groupBox_Bing_Target = new System.Windows.Forms.GroupBox();
            this.textBox_Bing_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Bing_Source = new System.Windows.Forms.GroupBox();
            this.textBox_Bing_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Bing2 = new System.Windows.Forms.TabPage();
            this.groupBox_Bing2_Key = new System.Windows.Forms.GroupBox();
            this.label_Bing2_Notice = new System.Windows.Forms.Label();
            this.groupBox_Bing2_Target = new System.Windows.Forms.GroupBox();
            this.textBox_Bing2_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Bing2_Source = new System.Windows.Forms.GroupBox();
            this.textBox_Bing2_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Microsoft = new System.Windows.Forms.TabPage();
            this.groupBox_Microsoft_Key = new System.Windows.Forms.GroupBox();
            this.label_Microsoft_Key = new System.Windows.Forms.Label();
            this.groupBox_Microsoft_Target = new System.Windows.Forms.GroupBox();
            this.textBox_Microsoft_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Microsoft_Source = new System.Windows.Forms.GroupBox();
            this.textBox_Microsoft_Source = new System.Windows.Forms.TextBox();
            this.tabPage_Yandex = new System.Windows.Forms.TabPage();
            this.groupBox_Yandex_Key = new System.Windows.Forms.GroupBox();
            this.label_Yandex_Key = new System.Windows.Forms.Label();
            this.groupBox_Yandex_Target = new System.Windows.Forms.GroupBox();
            this.textBox_Yandex_Target = new System.Windows.Forms.TextBox();
            this.groupBox_Yandex_Source = new System.Windows.Forms.GroupBox();
            this.textBox_Yandex_Source = new System.Windows.Forms.TextBox();
            this.tabPage_TencentInteractive = new System.Windows.Forms.TabPage();
            this.groupBox_TencentInteractive_Source = new System.Windows.Forms.GroupBox();
            this.groupBox_TencentInteractive_Target = new System.Windows.Forms.GroupBox();
            this.groupBox_TencentInteractive_Key = new System.Windows.Forms.GroupBox();
            this.textBox_TencentInteractive_Source = new System.Windows.Forms.TextBox();
            this.textBox_TencentInteractive_Target = new System.Windows.Forms.TextBox();
            this.label_TencentInteractive_Key = new System.Windows.Forms.Label();
            this.tabPage_Caiyun = new System.Windows.Forms.TabPage();
            this.groupBox_Caiyun_Source = new System.Windows.Forms.GroupBox();
            this.groupBox_Caiyun_Target = new System.Windows.Forms.GroupBox();
            this.groupBox_Caiyun_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Caiyun_Source = new System.Windows.Forms.TextBox();
            this.textBox_Caiyun_Target = new System.Windows.Forms.TextBox();
            this.label_Caiyun_Key = new System.Windows.Forms.Label();
            this.tabPage_Volcano = new System.Windows.Forms.TabPage();
            this.groupBox_Volcano_Source = new System.Windows.Forms.GroupBox();
            this.groupBox_Volcano_Target = new System.Windows.Forms.GroupBox();
            this.groupBox_Volcano_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Volcano_Source = new System.Windows.Forms.TextBox();
            this.textBox_Volcano_Target = new System.Windows.Forms.TextBox();
            this.label_Volcano_Key = new System.Windows.Forms.Label();
            this.tabPage_Caiyun2 = new System.Windows.Forms.TabPage();
            this.groupBox_Caiyun2_Source = new System.Windows.Forms.GroupBox();
            this.groupBox_Caiyun2_Target = new System.Windows.Forms.GroupBox();
            this.groupBox_Caiyun2_Key = new System.Windows.Forms.GroupBox();
            this.textBox_Caiyun2_Source = new System.Windows.Forms.TextBox();
            this.textBox_Caiyun2_Target = new System.Windows.Forms.TextBox();
            this.textBox_Caiyun2_Token = new System.Windows.Forms.TextBox();
            this.label_Caiyun2_Token = new System.Windows.Forms.Label();

            // 白描接口控件
            this.inPage白描接口 = new System.Windows.Forms.TabPage();
            this.BoxBaimiaoPassword = new System.Windows.Forms.TextBox();
            this.BoxBaimiaoUsername = new System.Windows.Forms.TextBox();
            this.label_BaimiaoPassword = new System.Windows.Forms.Label();
            this.label_BaimiaoUsername = new System.Windows.Forms.Label();

            //重置按钮区域
            this.btn_Reset_Google_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Google_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Baidu_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Baidu_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Tencent_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Tencent_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Bing_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Bing_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Bing2_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Bing2_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Microsoft_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Microsoft_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Yandex_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Yandex_Target = new System.Windows.Forms.Button();
            this.btn_Reset_TencentInteractive_Source = new System.Windows.Forms.Button();
            this.btn_Reset_TencentInteractive_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Caiyun_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Caiyun_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Volcano_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Volcano_Target = new System.Windows.Forms.Button();
            this.btn_Reset_Caiyun2_Source = new System.Windows.Forms.Button();
            this.btn_Reset_Caiyun2_Target = new System.Windows.Forms.Button();


            this.Page_About = new System.Windows.Forms.TabPage();
            this.Page_翻译接口 = new System.Windows.Forms.TabPage();
            this.label_VersionInfo = new System.Windows.Forms.Label();
            this.label_AuthorInfo = new System.Windows.Forms.Label();
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
            this.百度_btn = new System.Windows.Forms.Button();
            this.密钥Button_apply = new System.Windows.Forms.Button();
            this.密钥Button = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.inPage_百度接口 = new System.Windows.Forms.TabPage();
            this.text_baidupassword = new System.Windows.Forms.TextBox();
            this.text_baiduaccount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_Baidu_Language = new System.Windows.Forms.ComboBox();
            this.label_Baidu_Language = new System.Windows.Forms.Label();
            this.inPage_百度高精度接口 = new System.Windows.Forms.TabPage();
            this.text_baidu_accurate_secretkey = new System.Windows.Forms.TextBox();
            this.text_baidu_accurate_apikey = new System.Windows.Forms.TextBox();
            this.label_baidu_accurate_secretkey = new System.Windows.Forms.Label();
            this.label_baidu_accurate_apikey = new System.Windows.Forms.Label();
            this.comboBox_Baidu_Accurate_Language = new System.Windows.Forms.ComboBox();
            this.label_Baidu_Accurate_Language = new System.Windows.Forms.Label();
            this.comboBox_Tencent_Language = new System.Windows.Forms.ComboBox();
            this.label_Tencent_Language = new System.Windows.Forms.Label();
            this.inPage腾讯接口 = new System.Windows.Forms.TabPage();
            this.inPage腾讯高精度接口 = new System.Windows.Forms.TabPage();
            this.text_tencent_accurate_secretkey = new System.Windows.Forms.TextBox();
            this.text_tencent_accurate_secretid = new System.Windows.Forms.TextBox();
            this.label_tencent_accurate_secretkey = new System.Windows.Forms.Label();
            this.label_tencent_accurate_secretid = new System.Windows.Forms.Label();
            this.comboBox_Tencent_Accurate_Language = new System.Windows.Forms.ComboBox();
            this.label_Tencent_Accurate_Language = new System.Windows.Forms.Label();
            this.BoxTencentKey = new System.Windows.Forms.TextBox();
            this.BoxTencentId = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.Page_快捷键 = new System.Windows.Forms.TabPage();
            this.快捷键Button = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtBox_识别界面 = new System.Windows.Forms.TextBox();
            this.txtBox_记录界面 = new System.Windows.Forms.TextBox();
            this.txtBox_翻译文本 = new System.Windows.Forms.TextBox();
            this.txtBox_文字识别 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_输入翻译 = new System.Windows.Forms.Label();
            this.txtBox_输入翻译 = new System.Windows.Forms.TextBox();
            this.pictureBox_输入翻译 = new System.Windows.Forms.PictureBox();
            this.page_常规 = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chbox_save = new System.Windows.Forms.CheckBox();
            this.chbox_copy = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.btn_音效路径 = new System.Windows.Forms.Button();
            this.text_音效path = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_浏览 = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBox_保存 = new System.Windows.Forms.CheckBox();
            this.常规Button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numbox_记录 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cobBox_动画 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbBox_输入翻译剪贴板 = new System.Windows.Forms.CheckBox();
            this.chbox_取色 = new System.Windows.Forms.CheckBox();
            this.cbBox_弹窗 = new System.Windows.Forms.CheckBox();
            this.cbBox_翻译 = new System.Windows.Forms.CheckBox();
            this.cbBox_开机 = new System.Windows.Forms.CheckBox();
            this.tab_标签 = new System.Windows.Forms.TabControl();
            this.Page_更新 = new System.Windows.Forms.TabPage();
            this.更新Button_check = new System.Windows.Forms.Button();
            this.更新Button = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.numbox_间隔时间 = new System.Windows.Forms.NumericUpDown();
            this.checkBox_更新间隔 = new System.Windows.Forms.CheckBox();
            this.check_检查更新 = new System.Windows.Forms.CheckBox();
            this.btn_音效 = new System.Windows.Forms.Button();
            this.pic_help = new System.Windows.Forms.PictureBox();
            this.pictureBox_识别界面 = new System.Windows.Forms.PictureBox();
            this.pictureBox_记录界面 = new System.Windows.Forms.PictureBox();
            this.pictureBox_翻译文本 = new System.Windows.Forms.PictureBox();
            this.pictureBox_文字识别 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_输入翻译)).BeginInit();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label_版本号 = new System.Windows.Forms.Label();
            this.label_更新日期 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.txt_更新说明 = new System.Windows.Forms.TextBox();
            this.txt_问题反馈 = new System.Windows.Forms.TextBox();
            this.反馈Button = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.Page_反馈 = new System.Windows.Forms.TabPage();
            this.Page_代理.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.Page_密钥.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.inPage_百度接口.SuspendLayout();
            this.inPage腾讯接口.SuspendLayout();
            this.Page_快捷键.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.page_常规.SuspendLayout();
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
            this.tabPage_Caiyun2.SuspendLayout();
            this.groupBox_Caiyun2_Source.SuspendLayout();
            this.groupBox_Caiyun2_Target.SuspendLayout();
            this.groupBox_Caiyun2_Key.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_记录)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tab_标签.SuspendLayout();
            this.Page_更新.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_间隔时间)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_help)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_识别界面)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_记录界面)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_翻译文本)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_文字识别)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.Page_反馈.SuspendLayout();
            this.Page_About.SuspendLayout();
            this.SuspendLayout();
            // 
            // Page_代理
            // 
            this.Page_代理.BackColor = System.Drawing.Color.White;
            this.Page_代理.Controls.Add(this.代理Button);
            this.Page_代理.Controls.Add(this.groupBox4);
            this.Page_代理.Location = new System.Drawing.Point(4, 22);
            this.Page_代理.Name = "Page_代理";
            this.Page_代理.Padding = new System.Windows.Forms.Padding(3);
            this.Page_代理.Size = new System.Drawing.Size(390, 329);
            this.Page_代理.TabIndex = 4;
            this.Page_代理.Text = "代理";
            // 
            // 代理Button
            // 
            this.代理Button.BackColor = System.Drawing.Color.White;
            this.代理Button.Location = new System.Drawing.Point(309, 193);
            this.代理Button.Name = "代理Button";
            this.代理Button.Size = new System.Drawing.Size(75, 23);
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
            this.groupBox4.Location = new System.Drawing.Point(3, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(381, 183);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "代理";
            // 
            // chbox_代理服务器
            // 
            this.chbox_代理服务器.AutoSize = true;
            this.chbox_代理服务器.Location = new System.Drawing.Point(78, 84);
            this.chbox_代理服务器.Name = "chbox_代理服务器";
            this.chbox_代理服务器.Size = new System.Drawing.Size(132, 16);
            this.chbox_代理服务器.TabIndex = 12;
            this.chbox_代理服务器.Text = "代理服务器需要密码";
            this.chbox_代理服务器.UseVisualStyleBackColor = true;
            // 
            // text_密码
            // 
            this.text_密码.Location = new System.Drawing.Point(78, 144);
            this.text_密码.Name = "text_密码";
            this.text_密码.Size = new System.Drawing.Size(128, 21);
            this.text_密码.TabIndex = 11;
            this.text_密码.TextChanged += new System.EventHandler(this.text_密码_TextChanged);
            // 
            // text_端口
            // 
            this.text_端口.Location = new System.Drawing.Point(248, 52);
            this.text_端口.Name = "text_端口";
            this.text_端口.Size = new System.Drawing.Size(55, 21);
            this.text_端口.TabIndex = 10;
            this.text_端口.Text = " ";
            this.text_端口.TextChanged += new System.EventHandler(this.text_端口_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(212, 57);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 9;
            this.label15.Text = "端口：";
            // 
            // text_账号
            // 
            this.text_账号.Location = new System.Drawing.Point(78, 110);
            this.text_账号.Name = "text_账号";
            this.text_账号.Size = new System.Drawing.Size(128, 21);
            this.text_账号.TabIndex = 8;
            this.text_账号.TextChanged += new System.EventHandler(this.text_账号_TextChanged);
            // 
            // text_服务器
            // 
            this.text_服务器.Location = new System.Drawing.Point(78, 52);
            this.text_服务器.Name = "text_服务器";
            this.text_服务器.Size = new System.Drawing.Size(128, 21);
            this.text_服务器.TabIndex = 7;
            this.text_服务器.TextChanged += new System.EventHandler(this.text_服务器_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 147);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 6;
            this.label14.Text = "密码：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 5;
            this.label13.Text = "用户名：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
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
            this.combox_代理.Location = new System.Drawing.Point(78, 18);
            this.combox_代理.Name = "combox_代理";
            this.combox_代理.Size = new System.Drawing.Size(95, 20);
            this.combox_代理.TabIndex = 3;
            this.combox_代理.SelectedIndexChanged += new System.EventHandler(this.combox_代理_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "代理类型：";
            // 
            // Page_密钥
            // 
            this.Page_密钥.BackColor = System.Drawing.Color.White;
            this.Page_密钥.Controls.Add(this.百度_btn);
            this.Page_密钥.Controls.Add(this.密钥Button_apply);
            this.Page_密钥.Controls.Add(this.密钥Button);
            this.Page_密钥.Controls.Add(this.tabControl2);
            this.Page_密钥.Location = new System.Drawing.Point(4, 22);
            this.Page_密钥.Name = "Page_密钥";
            this.Page_密钥.Padding = new System.Windows.Forms.Padding(3);
            this.Page_密钥.Size = new System.Drawing.Size(390, 329);
            this.Page_密钥.TabIndex = 3;
            this.Page_密钥.Text = "密钥";
            // 
            // 百度_btn
            // 
            this.百度_btn.BackColor = System.Drawing.Color.White;
            this.百度_btn.Location = new System.Drawing.Point(152, 162);
            this.百度_btn.Name = "百度_btn";
            this.百度_btn.Size = new System.Drawing.Size(75, 23);
            this.百度_btn.TabIndex = 10;
            this.百度_btn.Text = "密钥测试";
            this.百度_btn.UseVisualStyleBackColor = false;
            this.百度_btn.Click += new System.EventHandler(this.百度_btn_Click);
            // 
            // 密钥Button_apply
            // 
            this.密钥Button_apply.BackColor = System.Drawing.Color.White;
            this.密钥Button_apply.Location = new System.Drawing.Point(6, 162);
            this.密钥Button_apply.Name = "密钥Button_apply";
            this.密钥Button_apply.Size = new System.Drawing.Size(75, 23);
            this.密钥Button_apply.TabIndex = 9;
            this.密钥Button_apply.Text = "接口申请";
            this.密钥Button_apply.UseVisualStyleBackColor = false;
            this.密钥Button_apply.Click += new System.EventHandler(this.百度申请_Click);
            // 
            // 密钥Button
            // 
            this.密钥Button.BackColor = System.Drawing.Color.White;
            this.密钥Button.Location = new System.Drawing.Point(309, 162);
            this.密钥Button.Name = "密钥Button";
            this.密钥Button.Size = new System.Drawing.Size(75, 23);
            this.密钥Button.TabIndex = 8;
            this.密钥Button.Text = "恢复默认";
            this.密钥Button.UseVisualStyleBackColor = false;
            this.密钥Button.Click += new System.EventHandler(this.密钥Button_Click);
            // 
            // tabControl2
            //
            this.tabControl2.Controls.Add(this.inPage_百度接口);
            this.tabControl2.Controls.Add(this.inPage_百度高精度接口);
            this.tabControl2.Controls.Add(this.inPage腾讯接口);
            this.tabControl2.Controls.Add(this.inPage腾讯高精度接口);
            this.tabControl2.Controls.Add(this.inPage白描接口);
            this.tabControl2.Location = new System.Drawing.Point(6, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(378, 150);
            this.tabControl2.TabIndex = 0;
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
            this.inPage_百度接口.Location = new System.Drawing.Point(4, 22);
            this.inPage_百度接口.Name = "inPage_百度接口";
            this.inPage_百度接口.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_百度接口.Size = new System.Drawing.Size(370, 120);
            this.inPage_百度接口.TabIndex = 0;
            this.inPage_百度接口.Text = "百度-标准版";
            // 
            // text_baidupassword
            // 
            this.text_baidupassword.BackColor = System.Drawing.Color.White;
            this.text_baidupassword.Location = new System.Drawing.Point(80, 42);
            this.text_baidupassword.Name = "text_baidupassword";
            this.text_baidupassword.Size = new System.Drawing.Size(284, 21);
            this.text_baidupassword.TabIndex = 3;
            this.text_baidupassword.TextChanged += new System.EventHandler(this.text_baidupassword_TextChanged);
            // 
            // text_baiduaccount
            // 
            this.text_baiduaccount.BackColor = System.Drawing.Color.White;
            this.text_baiduaccount.Location = new System.Drawing.Point(80, 10);
            this.text_baiduaccount.Name = "text_baiduaccount";
            this.text_baiduaccount.Size = new System.Drawing.Size(284, 21);
            this.text_baiduaccount.TabIndex = 2;
            this.text_baiduaccount.TextChanged += new System.EventHandler(this.text_baiduaccount_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "Secret Key:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "API Key:";
            //
            // comboBox_Baidu_Language
            //
            this.comboBox_Baidu_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Baidu_Language.FormattingEnabled = true;
            this.comboBox_Baidu_Language.Location = new System.Drawing.Point(80, 74);
            this.comboBox_Baidu_Language.Name = "comboBox_Baidu_Language";
            this.comboBox_Baidu_Language.Size = new System.Drawing.Size(121, 20);
            this.comboBox_Baidu_Language.TabIndex = 5;
            //
            // label_Baidu_Language
            //
            this.label_Baidu_Language.AutoSize = true;
            this.label_Baidu_Language.Location = new System.Drawing.Point(6, 77);
            this.label_Baidu_Language.Name = "label_Baidu_Language";
            this.label_Baidu_Language.Size = new System.Drawing.Size(65, 12);
            this.label_Baidu_Language.TabIndex = 4;
            this.label_Baidu_Language.Text = "识别语言：";
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
            this.inPage_百度高精度接口.Location = new System.Drawing.Point(4, 22);
            this.inPage_百度高精度接口.Name = "inPage_百度高精度接口";
            this.inPage_百度高精度接口.Padding = new System.Windows.Forms.Padding(3);
            this.inPage_百度高精度接口.Size = new System.Drawing.Size(370, 120);
            this.inPage_百度高精度接口.TabIndex = 3;
            this.inPage_百度高精度接口.Text = "百度-高精度";

            //
            // text_baidu_accurate_secretkey
            //
            this.text_baidu_accurate_secretkey.BackColor = System.Drawing.Color.White;
            this.text_baidu_accurate_secretkey.Location = new System.Drawing.Point(80, 42);
            this.text_baidu_accurate_secretkey.Name = "text_baidu_accurate_secretkey";
            this.text_baidu_accurate_secretkey.Size = new System.Drawing.Size(284, 21);
            this.text_baidu_accurate_secretkey.TabIndex = 9;
            //
            // text_baidu_accurate_apikey
            //
            this.text_baidu_accurate_apikey.BackColor = System.Drawing.Color.White;
            this.text_baidu_accurate_apikey.Location = new System.Drawing.Point(80, 10);
            this.text_baidu_accurate_apikey.Name = "text_baidu_accurate_apikey";
            this.text_baidu_accurate_apikey.Size = new System.Drawing.Size(284, 21);
            this.text_baidu_accurate_apikey.TabIndex = 8;
            //
            // label_baidu_accurate_secretkey
            //
            this.label_baidu_accurate_secretkey.AutoSize = true;
            this.label_baidu_accurate_secretkey.Location = new System.Drawing.Point(6, 45);
            this.label_baidu_accurate_secretkey.Name = "label_baidu_accurate_secretkey";
            this.label_baidu_accurate_secretkey.Size = new System.Drawing.Size(71, 12);
            this.label_baidu_accurate_secretkey.TabIndex = 7;
            this.label_baidu_accurate_secretkey.Text = "Secret Key:";
            //
            // label_baidu_accurate_apikey
            //
            this.label_baidu_accurate_apikey.AutoSize = true;
            this.label_baidu_accurate_apikey.Location = new System.Drawing.Point(6, 13);
            this.label_baidu_accurate_apikey.Name = "label_baidu_accurate_apikey";
            this.label_baidu_accurate_apikey.Size = new System.Drawing.Size(47, 12);
            this.label_baidu_accurate_apikey.TabIndex = 6;
            this.label_baidu_accurate_apikey.Text = "API Key:";
            //
            // comboBox_Baidu_Accurate_Language
            //
            this.comboBox_Baidu_Accurate_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Baidu_Accurate_Language.FormattingEnabled = true;
            this.comboBox_Baidu_Accurate_Language.Location = new System.Drawing.Point(80, 74);
            this.comboBox_Baidu_Accurate_Language.Name = "comboBox_Baidu_Accurate_Language";
            this.comboBox_Baidu_Accurate_Language.Size = new System.Drawing.Size(121, 20);
            this.comboBox_Baidu_Accurate_Language.TabIndex = 5;
            //
            // label_Baidu_Accurate_Language
            //
            this.label_Baidu_Accurate_Language.AutoSize = true;
            this.label_Baidu_Accurate_Language.Location = new System.Drawing.Point(6, 77);
            this.label_Baidu_Accurate_Language.Name = "label_Baidu_Accurate_Language";
            this.label_Baidu_Accurate_Language.Size = new System.Drawing.Size(65, 12);
            this.label_Baidu_Accurate_Language.TabIndex = 4;
            this.label_Baidu_Accurate_Language.Text = "识别语言：";
            //
            // inPage腾讯接口
            //
            this.inPage腾讯接口.BackColor = System.Drawing.Color.White;
            this.inPage腾讯接口.Controls.Add(this.BoxTencentKey);
            this.inPage腾讯接口.Controls.Add(this.BoxTencentId);
            this.inPage腾讯接口.Controls.Add(this.label17);
            this.inPage腾讯接口.Controls.Add(this.label22);
            this.inPage腾讯接口.Controls.Add(this.comboBox_Tencent_Language);
            this.inPage腾讯接口.Controls.Add(this.label_Tencent_Language);
            this.inPage腾讯接口.Location = new System.Drawing.Point(4, 22);
            this.inPage腾讯接口.Name = "inPage腾讯接口";
            this.inPage腾讯接口.Padding = new System.Windows.Forms.Padding(3);
            this.inPage腾讯接口.Size = new System.Drawing.Size(370, 98);
            this.inPage腾讯接口.TabIndex = 1;
            this.inPage腾讯接口.Text = "腾讯识别接口";
            // 
            // BoxTencentKey
            // 
            this.BoxTencentKey.BackColor = System.Drawing.Color.White;
            this.BoxTencentKey.Location = new System.Drawing.Point(80, 42);
            this.BoxTencentKey.Name = "BoxTencentKey";
            this.BoxTencentKey.Size = new System.Drawing.Size(284, 21);
            this.BoxTencentKey.TabIndex = 7;
            // 
            // BoxTencentId
            // 
            this.BoxTencentId.BackColor = System.Drawing.Color.White;
            this.BoxTencentId.Location = new System.Drawing.Point(80, 10);
            this.BoxTencentId.Name = "BoxTencentId";
            this.BoxTencentId.Size = new System.Drawing.Size(284, 21);
            this.BoxTencentId.TabIndex = 6;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 5;
            this.label17.Text = "SecretKey:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 13);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(59, 12);
            this.label22.TabIndex = 4;
            this.label22.Text = "SecretId:";
           //
           // comboBox_Tencent_Language
           //
           this.comboBox_Tencent_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.comboBox_Tencent_Language.FormattingEnabled = true;
           this.comboBox_Tencent_Language.Location = new System.Drawing.Point(80, 74);
           this.comboBox_Tencent_Language.Name = "comboBox_Tencent_Language";
           this.comboBox_Tencent_Language.Size = new System.Drawing.Size(121, 20);
           this.comboBox_Tencent_Language.TabIndex = 9;
           //
           // label_Tencent_Language
           //
           this.label_Tencent_Language.AutoSize = true;
           this.label_Tencent_Language.Location = new System.Drawing.Point(6, 77);
           this.label_Tencent_Language.Name = "label_Tencent_Language";
           this.label_Tencent_Language.Size = new System.Drawing.Size(65, 12);
           this.label_Tencent_Language.TabIndex = 8;
           this.label_Tencent_Language.Text = "识别语言：";
            //
            // inPage腾讯高精度接口
            //
            this.inPage腾讯高精度接口.BackColor = System.Drawing.Color.White;
            this.inPage腾讯高精度接口.Controls.Add(this.text_tencent_accurate_secretkey);
            this.inPage腾讯高精度接口.Controls.Add(this.text_tencent_accurate_secretid);
            this.inPage腾讯高精度接口.Controls.Add(this.label_tencent_accurate_secretkey);
            this.inPage腾讯高精度接口.Controls.Add(this.label_tencent_accurate_secretid);
            this.inPage腾讯高精度接口.Controls.Add(this.comboBox_Tencent_Accurate_Language);
            this.inPage腾讯高精度接口.Controls.Add(this.label_Tencent_Accurate_Language);
            this.inPage腾讯高精度接口.Location = new System.Drawing.Point(4, 22);
            this.inPage腾讯高精度接口.Name = "inPage腾讯高精度接口";
            this.inPage腾讯高精度接口.Size = new System.Drawing.Size(370, 98);
            this.inPage腾讯高精度接口.TabIndex = 4;
            this.inPage腾讯高精度接口.Text = "腾讯-高精度";
            //
            // text_tencent_accurate_secretkey
            //
            this.text_tencent_accurate_secretkey.BackColor = System.Drawing.Color.White;
            this.text_tencent_accurate_secretkey.Location = new System.Drawing.Point(80, 42);
            this.text_tencent_accurate_secretkey.Name = "text_tencent_accurate_secretkey";
            this.text_tencent_accurate_secretkey.Size = new System.Drawing.Size(284, 21);
            this.text_tencent_accurate_secretkey.TabIndex = 9;
            //
            // text_tencent_accurate_secretid
            //
            this.text_tencent_accurate_secretid.BackColor = System.Drawing.Color.White;
            this.text_tencent_accurate_secretid.Location = new System.Drawing.Point(80, 10);
            this.text_tencent_accurate_secretid.Name = "text_tencent_accurate_secretid";
            this.text_tencent_accurate_secretid.Size = new System.Drawing.Size(284, 21);
            this.text_tencent_accurate_secretid.TabIndex = 8;
            //
            // label_tencent_accurate_secretkey
            //
            this.label_tencent_accurate_secretkey.AutoSize = true;
            this.label_tencent_accurate_secretkey.Location = new System.Drawing.Point(6, 45);
            this.label_tencent_accurate_secretkey.Name = "label_tencent_accurate_secretkey";
            this.label_tencent_accurate_secretkey.Size = new System.Drawing.Size(65, 12);
            this.label_tencent_accurate_secretkey.TabIndex = 7;
            this.label_tencent_accurate_secretkey.Text = "SecretKey:";
            //
            // label_tencent_accurate_secretid
            //
            this.label_tencent_accurate_secretid.AutoSize = true;
            this.label_tencent_accurate_secretid.Location = new System.Drawing.Point(6, 13);
            this.label_tencent_accurate_secretid.Name = "label_tencent_accurate_secretid";
            this.label_tencent_accurate_secretid.Size = new System.Drawing.Size(59, 12);
            this.label_tencent_accurate_secretid.TabIndex = 6;
            this.label_tencent_accurate_secretid.Text = "SecretId:";
            //
            // comboBox_Tencent_Accurate_Language
            //
            this.comboBox_Tencent_Accurate_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Tencent_Accurate_Language.FormattingEnabled = true;
            this.comboBox_Tencent_Accurate_Language.Location = new System.Drawing.Point(80, 74);
            this.comboBox_Tencent_Accurate_Language.Name = "comboBox_Tencent_Accurate_Language";
            this.comboBox_Tencent_Accurate_Language.Size = new System.Drawing.Size(121, 20);
            this.comboBox_Tencent_Accurate_Language.TabIndex = 5;
            //
            // label_Tencent_Accurate_Language
            //
            this.label_Tencent_Accurate_Language.AutoSize = true;
            this.label_Tencent_Accurate_Language.Location = new System.Drawing.Point(6, 77);
            this.label_Tencent_Accurate_Language.Name = "label_Tencent_Accurate_Language";
            this.label_Tencent_Accurate_Language.Size = new System.Drawing.Size(65, 12);
            this.label_Tencent_Accurate_Language.TabIndex = 4;
            this.label_Tencent_Accurate_Language.Text = "识别语言：";
            // inPage白描接口
            //
            this.inPage白描接口.BackColor = System.Drawing.Color.White;
            this.inPage白描接口.Controls.Add(this.BoxBaimiaoPassword);
            this.inPage白描接口.Controls.Add(this.BoxBaimiaoUsername);
            this.inPage白描接口.Controls.Add(this.label_BaimiaoPassword);
            this.inPage白描接口.Controls.Add(this.label_BaimiaoUsername);
            this.inPage白描接口.Location = new System.Drawing.Point(4, 22);
            this.inPage白描接口.Name = "inPage白描接口";
            this.inPage白描接口.Padding = new System.Windows.Forms.Padding(3);
            this.inPage白描接口.Size = new System.Drawing.Size(370, 98);
            this.inPage白描接口.TabIndex = 2;
            this.inPage白描接口.Text = "白描识别接口";
            //
            // BoxBaimiaoPassword
            //
            this.BoxBaimiaoPassword.BackColor = System.Drawing.Color.White;
            this.BoxBaimiaoPassword.Location = new System.Drawing.Point(70, 55);
            this.BoxBaimiaoPassword.Name = "BoxBaimiaoPassword";
            this.BoxBaimiaoPassword.PasswordChar = '*';
            this.BoxBaimiaoPassword.Size = new System.Drawing.Size(260, 21);
            this.BoxBaimiaoPassword.TabIndex = 8;
            //
            // BoxBaimiaoUsername
            //
            this.BoxBaimiaoUsername.BackColor = System.Drawing.Color.White;
            this.BoxBaimiaoUsername.Location = new System.Drawing.Point(70, 20);
            this.BoxBaimiaoUsername.Name = "BoxBaimiaoUsername";
            this.BoxBaimiaoUsername.Size = new System.Drawing.Size(260, 21);
            this.BoxBaimiaoUsername.TabIndex = 7;
            //
            // label_BaimiaoPassword
            //
            this.label_BaimiaoPassword.AutoSize = true;
            this.label_BaimiaoPassword.Location = new System.Drawing.Point(6, 58);
            this.label_BaimiaoPassword.Name = "label_BaimiaoPassword";
            this.label_BaimiaoPassword.Size = new System.Drawing.Size(41, 12);
            this.label_BaimiaoPassword.TabIndex = 6;
            this.label_BaimiaoPassword.Text = "密码:";
            //
            // label_BaimiaoUsername
            //
            this.label_BaimiaoUsername.AutoSize = true;
            this.label_BaimiaoUsername.Location = new System.Drawing.Point(6, 23);
            this.label_BaimiaoUsername.Name = "label_BaimiaoUsername";
            this.label_BaimiaoUsername.Size = new System.Drawing.Size(53, 12);
            this.label_BaimiaoUsername.TabIndex = 5;
            this.label_BaimiaoUsername.Text = "账号:";
            //
            // Page_快捷键
            // 
            this.Page_快捷键.BackColor = System.Drawing.Color.White;
            this.Page_快捷键.Controls.Add(this.快捷键Button);
            this.Page_快捷键.Controls.Add(this.label8);
            this.Page_快捷键.Controls.Add(this.groupBox3);
            this.Page_快捷键.Location = new System.Drawing.Point(4, 22);
            this.Page_快捷键.Name = "Page_快捷键";
            this.Page_快捷键.Padding = new System.Windows.Forms.Padding(3);
            this.Page_快捷键.Size = new System.Drawing.Size(390, 329);
            this.Page_快捷键.TabIndex = 2;
            this.Page_快捷键.Text = "快捷键";
            // 
            // 快捷键Button
            //
            this.快捷键Button.BackColor = System.Drawing.Color.White;
            this.快捷键Button.Location = new System.Drawing.Point(309, 195);
            this.快捷键Button.Name = "快捷键Button";
            this.快捷键Button.Size = new System.Drawing.Size(75, 23);
            this.快捷键Button.TabIndex = 7;
            this.快捷键Button.Text = "恢复默认";
            this.快捷键Button.UseVisualStyleBackColor = false;
            this.快捷键Button.Click += new System.EventHandler(this.快捷键Button_Click);
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(215, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "说明：按Backspace键可清除当前快捷键";
            //
            // groupBox3
            //
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
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 185);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "全局快捷键";
            //
            // txtBox_识别界面
            //
            this.txtBox_识别界面.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_识别界面.Location = new System.Drawing.Point(78, 151);
            this.txtBox_识别界面.Name = "txtBox_识别界面";
            this.txtBox_识别界面.Size = new System.Drawing.Size(260, 23);
            this.txtBox_识别界面.TabIndex = 8;
            this.txtBox_识别界面.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_识别界面.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_识别界面.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            //
            // txtBox_记录界面
            //
            this.txtBox_记录界面.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_记录界面.Location = new System.Drawing.Point(78, 118);
            this.txtBox_记录界面.Name = "txtBox_记录界面";
            this.txtBox_记录界面.Size = new System.Drawing.Size(260, 23);
            this.txtBox_记录界面.TabIndex = 7;
            this.txtBox_记录界面.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_记录界面.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_记录界面.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            //
            // txtBox_翻译文本
            //
            this.txtBox_翻译文本.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_翻译文本.Location = new System.Drawing.Point(78, 52);
            this.txtBox_翻译文本.Name = "txtBox_翻译文本";
            this.txtBox_翻译文本.Size = new System.Drawing.Size(260, 23);
            this.txtBox_翻译文本.TabIndex = 5;
            this.txtBox_翻译文本.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_翻译文本.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_翻译文本.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            //
            // txtBox_文字识别
            //
            this.txtBox_文字识别.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_文字识别.Location = new System.Drawing.Point(78, 19);
            this.txtBox_文字识别.Name = "txtBox_文字识别";
            this.txtBox_文字识别.Size = new System.Drawing.Size(260, 23);
            this.txtBox_文字识别.TabIndex = 4;
            this.txtBox_文字识别.TabStop = false;
            this.txtBox_文字识别.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_文字识别.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_文字识别.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "识别界面：";
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "记录界面：";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "翻译文本：";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
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
            this.page_常规.Location = new System.Drawing.Point(4, 22);
            this.page_常规.Name = "page_常规";
            this.page_常规.Padding = new System.Windows.Forms.Padding(3);
            this.page_常规.Size = new System.Drawing.Size(390, 329);
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
            this.groupBox10.Location = new System.Drawing.Point(6, 166);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(378, 86);
            this.groupBox10.TabIndex = 8;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "音效";
            // 
            // chbox_save
            // 
            this.chbox_save.AutoSize = true;
            this.chbox_save.Checked = true;
            this.chbox_save.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbox_save.Location = new System.Drawing.Point(167, 54);
            this.chbox_save.Name = "chbox_save";
            this.chbox_save.Size = new System.Drawing.Size(96, 16);
            this.chbox_save.TabIndex = 10;
            this.chbox_save.Text = "快速保存图片";
            this.chbox_save.UseVisualStyleBackColor = true;
            this.chbox_save.CheckedChanged += new System.EventHandler(this.chbox_save_CheckedChanged);
            // 
            // chbox_copy
            // 
            this.chbox_copy.AutoSize = true;
            this.chbox_copy.Location = new System.Drawing.Point(53, 54);
            this.chbox_copy.Name = "chbox_copy";
            this.chbox_copy.Size = new System.Drawing.Size(96, 16);
            this.chbox_copy.TabIndex = 6;
            this.chbox_copy.Text = "复制到粘贴板";
            this.chbox_copy.UseVisualStyleBackColor = true;
            this.chbox_copy.CheckedChanged += new System.EventHandler(this.chbox_copy_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(14, 55);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 12);
            this.label20.TabIndex = 9;
            this.label20.Text = "何时：";
            // 
            // btn_音效路径
            // 
            this.btn_音效路径.BackColor = System.Drawing.Color.White;
            this.btn_音效路径.Location = new System.Drawing.Point(332, 49);
            this.btn_音效路径.Name = "btn_音效路径";
            this.btn_音效路径.Size = new System.Drawing.Size(40, 23);
            this.btn_音效路径.TabIndex = 8;
            this.btn_音效路径.Text = "更改";
            this.btn_音效路径.UseVisualStyleBackColor = false;
            this.btn_音效路径.Click += new System.EventHandler(this.btn_音效路径_Click);
            // 
            // text_音效path
            // 
            this.text_音效path.BackColor = System.Drawing.Color.White;
            this.text_音效path.Location = new System.Drawing.Point(51, 19);
            this.text_音效path.Name = "text_音效path";
            this.text_音效path.ReadOnly = true;
            this.text_音效path.Size = new System.Drawing.Size(288, 21);
            this.text_音效path.TabIndex = 4;
            this.text_音效path.Text = "Data\\screenshot.wav";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(13, 22);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 12);
            this.label18.TabIndex = 3;
            this.label18.Text = "文件：";
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
            this.tabControl_Trans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Trans.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Trans.Name = "tabControl_Trans";
            this.tabControl_Trans.SelectedIndex = 0;
            this.tabControl_Trans.Size = new System.Drawing.Size(390, 329);
            this.tabControl_Trans.TabIndex = 0;
            //
            // tabPage_Google
            //
            this.tabPage_Google.Controls.Add(this.groupBox_Google_Key);
            this.tabPage_Google.Controls.Add(this.groupBox_Google_Target);
            this.tabPage_Google.Controls.Add(this.groupBox_Google_Source);
            this.tabPage_Google.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Google.Name = "tabPage_Google";
            this.tabPage_Google.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Google.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Google.TabIndex = 0;
            this.tabPage_Google.Text = "Google";
            this.tabPage_Google.UseVisualStyleBackColor = true;
            //
            // groupBox_Google_Key
            //
            this.groupBox_Google_Key.Controls.Add(this.label_Google_Key);
            this.groupBox_Google_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Google_Key.Name = "groupBox_Google_Key";
            this.groupBox_Google_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Google_Key.TabIndex = 2;
            this.groupBox_Google_Key.TabStop = false;
            this.groupBox_Google_Key.Text = "密钥";
            //
            // label_Google_Key
            //
            this.label_Google_Key.AutoSize = true;
            this.label_Google_Key.Location = new System.Drawing.Point(150, 25);
            this.label_Google_Key.Name = "label_Google_Key";
            this.label_Google_Key.Size = new System.Drawing.Size(53, 12);
            this.label_Google_Key.TabIndex = 0;
            this.label_Google_Key.Text = "无需密钥";
            //
            // groupBox_Google_Target
            //
            this.groupBox_Google_Target.Controls.Add(this.btn_Reset_Google_Target);
            this.groupBox_Google_Target.Controls.Add(this.textBox_Google_Target);
            this.groupBox_Google_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Google_Target.Name = "groupBox_Google_Target";
            this.groupBox_Google_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Google_Target.TabIndex = 1;
            this.groupBox_Google_Target.TabStop = false;
            this.groupBox_Google_Target.Text = "目标语言";
            //
            // btn_Reset_Google_Target
            //
            this.btn_Reset_Google_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Google_Target.Name = "btn_Reset_Google_Target";
            this.btn_Reset_Google_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Google_Target.TabIndex = 1;
            this.btn_Reset_Google_Target.Text = "重置";
            this.btn_Reset_Google_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Google_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Google_Target
            //
            this.textBox_Google_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Google_Target.Name = "textBox_Google_Target";
            this.textBox_Google_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Google_Target.TabIndex = 0;
            //
            // groupBox_Google_Source
            //
            this.groupBox_Google_Source.Controls.Add(this.btn_Reset_Google_Source);
            this.groupBox_Google_Source.Controls.Add(this.textBox_Google_Source);
            this.groupBox_Google_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Google_Source.Name = "groupBox_Google_Source";
            this.groupBox_Google_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Google_Source.TabIndex = 0;
            this.groupBox_Google_Source.TabStop = false;
            this.groupBox_Google_Source.Text = "源语言";
            //
            // btn_Reset_Google_Source
            //
            this.btn_Reset_Google_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Google_Source.Name = "btn_Reset_Google_Source";
            this.btn_Reset_Google_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Google_Source.TabIndex = 1;
            this.btn_Reset_Google_Source.Text = "重置";
            this.btn_Reset_Google_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Google_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Google_Source
            //
            this.textBox_Google_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Google_Source.Name = "textBox_Google_Source";
            this.textBox_Google_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Google_Source.TabIndex = 0;
            //
            // tabPage_Baidu
            //
            this.tabPage_Baidu.Controls.Add(this.groupBox_Baidu_Key);
            this.tabPage_Baidu.Controls.Add(this.groupBox_Baidu_Target);
            this.tabPage_Baidu.Controls.Add(this.groupBox_Baidu_Source);
            this.tabPage_Baidu.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Baidu.Name = "tabPage_Baidu";
            this.tabPage_Baidu.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Baidu.Size = new System.Drawing.Size(382, 303);
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
            this.groupBox_Baidu_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Baidu_Key.Name = "groupBox_Baidu_Key";
            this.groupBox_Baidu_Key.Size = new System.Drawing.Size(370, 80);
            this.groupBox_Baidu_Key.TabIndex = 5;
            this.groupBox_Baidu_Key.TabStop = false;
            this.groupBox_Baidu_Key.Text = "密钥";
            //
            // textBox_Baidu_SK
            //
            this.textBox_Baidu_SK.Location = new System.Drawing.Point(80, 47);
            this.textBox_Baidu_SK.Name = "textBox_Baidu_SK";
            this.textBox_Baidu_SK.Size = new System.Drawing.Size(284, 21);
            this.textBox_Baidu_SK.TabIndex = 3;
            //
            // label_Baidu_SK
            //
            this.label_Baidu_SK.AutoSize = true;
            this.label_Baidu_SK.Location = new System.Drawing.Point(7, 50);
            this.label_Baidu_SK.Name = "label_Baidu_SK";
            this.label_Baidu_SK.Size = new System.Drawing.Size(71, 12);
            this.label_Baidu_SK.TabIndex = 2;
            this.label_Baidu_SK.Text = "Secret Key:";
            //
            // textBox_Baidu_AK
            //
            this.textBox_Baidu_AK.Location = new System.Drawing.Point(80, 20);
            this.textBox_Baidu_AK.Name = "textBox_Baidu_AK";
            this.textBox_Baidu_AK.Size = new System.Drawing.Size(284, 21);
            this.textBox_Baidu_AK.TabIndex = 1;
            //
            // label_Baidu_AK
            //
            this.label_Baidu_AK.AutoSize = true;
            this.label_Baidu_AK.Location = new System.Drawing.Point(7, 23);
            this.label_Baidu_AK.Name = "label_Baidu_AK";
            this.label_Baidu_AK.Size = new System.Drawing.Size(47, 12);
            this.label_Baidu_AK.TabIndex = 0;
            this.label_Baidu_AK.Text = "APP ID:";
            //
            // groupBox_Baidu_Target
            //
            this.groupBox_Baidu_Target.Controls.Add(this.btn_Reset_Baidu_Target);
            this.groupBox_Baidu_Target.Controls.Add(this.textBox_Baidu_Target);
            this.groupBox_Baidu_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Baidu_Target.Name = "groupBox_Baidu_Target";
            this.groupBox_Baidu_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Baidu_Target.TabIndex = 4;
            this.groupBox_Baidu_Target.TabStop = false;
            this.groupBox_Baidu_Target.Text = "目标语言";
            //
            // btn_Reset_Baidu_Target
            //
            this.btn_Reset_Baidu_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Baidu_Target.Name = "btn_Reset_Baidu_Target";
            this.btn_Reset_Baidu_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Baidu_Target.TabIndex = 1;
            this.btn_Reset_Baidu_Target.Text = "重置";
            this.btn_Reset_Baidu_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Baidu_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Baidu_Target
            //
            this.textBox_Baidu_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Baidu_Target.Name = "textBox_Baidu_Target";
            this.textBox_Baidu_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Baidu_Target.TabIndex = 0;
            //
            // groupBox_Baidu_Source
            //
            this.groupBox_Baidu_Source.Controls.Add(this.btn_Reset_Baidu_Source);
            this.groupBox_Baidu_Source.Controls.Add(this.textBox_Baidu_Source);
            this.groupBox_Baidu_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Baidu_Source.Name = "groupBox_Baidu_Source";
            this.groupBox_Baidu_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Baidu_Source.TabIndex = 3;
            this.groupBox_Baidu_Source.TabStop = false;
            this.groupBox_Baidu_Source.Text = "源语言";
            //
            // btn_Reset_Baidu_Source
            //
            this.btn_Reset_Baidu_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Baidu_Source.Name = "btn_Reset_Baidu_Source";
            this.btn_Reset_Baidu_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Baidu_Source.TabIndex = 1;
            this.btn_Reset_Baidu_Source.Text = "重置";
            this.btn_Reset_Baidu_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Baidu_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Baidu_Source
            //
            this.textBox_Baidu_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Baidu_Source.Name = "textBox_Baidu_Source";
            this.textBox_Baidu_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Baidu_Source.TabIndex = 0;
            //
            // tabPage_Tencent
            //
            this.tabPage_Tencent.Controls.Add(this.groupBox_Tencent_Key);
            this.tabPage_Tencent.Controls.Add(this.groupBox_Tencent_Target);
            this.tabPage_Tencent.Controls.Add(this.groupBox_Tencent_Source);
            this.tabPage_Tencent.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Tencent.Name = "tabPage_Tencent";
            this.tabPage_Tencent.Size = new System.Drawing.Size(382, 303);
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
            this.groupBox_Tencent_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Tencent_Key.Name = "groupBox_Tencent_Key";
            this.groupBox_Tencent_Key.Size = new System.Drawing.Size(370, 80);
            this.groupBox_Tencent_Key.TabIndex = 5;
            this.groupBox_Tencent_Key.TabStop = false;
            this.groupBox_Tencent_Key.Text = "密钥";
            //
            // textBox_Tencent_SK
            //
            this.textBox_Tencent_SK.Location = new System.Drawing.Point(80, 47);
            this.textBox_Tencent_SK.Name = "textBox_Tencent_SK";
            this.textBox_Tencent_SK.Size = new System.Drawing.Size(284, 21);
            this.textBox_Tencent_SK.TabIndex = 3;
            //
            // label_Tencent_SK
            //
            this.label_Tencent_SK.AutoSize = true;
            this.label_Tencent_SK.Location = new System.Drawing.Point(7, 50);
            this.label_Tencent_SK.Name = "label_Tencent_SK";
            this.label_Tencent_SK.Size = new System.Drawing.Size(65, 12);
            this.label_Tencent_SK.TabIndex = 2;
            this.label_Tencent_SK.Text = "SecretKey:";
            //
            // textBox_Tencent_AK
            //
            this.textBox_Tencent_AK.Location = new System.Drawing.Point(80, 20);
            this.textBox_Tencent_AK.Name = "textBox_Tencent_AK";
            this.textBox_Tencent_AK.Size = new System.Drawing.Size(284, 21);
            this.textBox_Tencent_AK.TabIndex = 1;
            //
            // label_Tencent_AK
            //
            this.label_Tencent_AK.AutoSize = true;
            this.label_Tencent_AK.Location = new System.Drawing.Point(7, 23);
            this.label_Tencent_AK.Name = "label_Tencent_AK";
            this.label_Tencent_AK.Size = new System.Drawing.Size(59, 12);
            this.label_Tencent_AK.TabIndex = 0;
            this.label_Tencent_AK.Text = "SecretId:";
            //
            // groupBox_Tencent_Target
            //
            this.groupBox_Tencent_Target.Controls.Add(this.btn_Reset_Tencent_Target);
            this.groupBox_Tencent_Target.Controls.Add(this.textBox_Tencent_Target);
            this.groupBox_Tencent_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Tencent_Target.Name = "groupBox_Tencent_Target";
            this.groupBox_Tencent_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Tencent_Target.TabIndex = 4;
            this.groupBox_Tencent_Target.TabStop = false;
            this.groupBox_Tencent_Target.Text = "目标语言";
            //
            // btn_Reset_Tencent_Target
            //
            this.btn_Reset_Tencent_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Tencent_Target.Name = "btn_Reset_Tencent_Target";
            this.btn_Reset_Tencent_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Tencent_Target.TabIndex = 1;
            this.btn_Reset_Tencent_Target.Text = "重置";
            this.btn_Reset_Tencent_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Tencent_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Tencent_Target
            //
            this.textBox_Tencent_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Tencent_Target.Name = "textBox_Tencent_Target";
            this.textBox_Tencent_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Tencent_Target.TabIndex = 0;
            //
            // groupBox_Tencent_Source
            //
            this.groupBox_Tencent_Source.Controls.Add(this.btn_Reset_Tencent_Source);
            this.groupBox_Tencent_Source.Controls.Add(this.textBox_Tencent_Source);
            this.groupBox_Tencent_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Tencent_Source.Name = "groupBox_Tencent_Source";
            this.groupBox_Tencent_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Tencent_Source.TabIndex = 3;
            this.groupBox_Tencent_Source.TabStop = false;
            this.groupBox_Tencent_Source.Text = "源语言";
            //
            // btn_Reset_Tencent_Source
            //
            this.btn_Reset_Tencent_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Tencent_Source.Name = "btn_Reset_Tencent_Source";
            this.btn_Reset_Tencent_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Tencent_Source.TabIndex = 1;
            this.btn_Reset_Tencent_Source.Text = "重置";
            this.btn_Reset_Tencent_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Tencent_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Tencent_Source
            //
            this.textBox_Tencent_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Tencent_Source.Name = "textBox_Tencent_Source";
            this.textBox_Tencent_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Tencent_Source.TabIndex = 0;
            //
            // tabPage_Bing
            //
            this.tabPage_Bing.Controls.Add(this.groupBox_Bing_Key);
            this.tabPage_Bing.Controls.Add(this.groupBox_Bing_Target);
            this.tabPage_Bing.Controls.Add(this.groupBox_Bing_Source);
            this.tabPage_Bing.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Bing.Name = "tabPage_Bing";
            this.tabPage_Bing.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Bing.TabIndex = 3;
            this.tabPage_Bing.Text = "Bing";
            this.tabPage_Bing.UseVisualStyleBackColor = true;
            //
            // groupBox_Bing_Key
            //
            this.groupBox_Bing_Key.Controls.Add(this.label_Bing_Key);
            this.groupBox_Bing_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Bing_Key.Name = "groupBox_Bing_Key";
            this.groupBox_Bing_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Bing_Key.TabIndex = 2;
            this.groupBox_Bing_Key.TabStop = false;
            this.groupBox_Bing_Key.Text = "密钥";
            //
            // label_Bing_Key
            //
            this.label_Bing_Key.AutoSize = true;
            this.label_Bing_Key.Location = new System.Drawing.Point(150, 25);
            this.label_Bing_Key.Name = "label_Bing_Key";
            this.label_Bing_Key.Size = new System.Drawing.Size(53, 12);
            this.label_Bing_Key.TabIndex = 0;
            this.label_Bing_Key.Text = "无需密钥";
            //
            // groupBox_Bing_Target
            //
            this.groupBox_Bing_Target.Controls.Add(this.btn_Reset_Bing_Target);
            this.groupBox_Bing_Target.Controls.Add(this.textBox_Bing_Target);
            this.groupBox_Bing_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Bing_Target.Name = "groupBox_Bing_Target";
            this.groupBox_Bing_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Bing_Target.TabIndex = 1;
            this.groupBox_Bing_Target.TabStop = false;
            this.groupBox_Bing_Target.Text = "目标语言";
            //
            // btn_Reset_Bing_Target
            //
            this.btn_Reset_Bing_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Bing_Target.Name = "btn_Reset_Bing_Target";
            this.btn_Reset_Bing_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Bing_Target.TabIndex = 1;
            this.btn_Reset_Bing_Target.Text = "重置";
            this.btn_Reset_Bing_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Bing_Target
            //
            this.textBox_Bing_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Bing_Target.Name = "textBox_Bing_Target";
            this.textBox_Bing_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Bing_Target.TabIndex = 0;
            //
            // groupBox_Bing_Source
            //
            this.groupBox_Bing_Source.Controls.Add(this.btn_Reset_Bing_Source);
            this.groupBox_Bing_Source.Controls.Add(this.textBox_Bing_Source);
            this.groupBox_Bing_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Bing_Source.Name = "groupBox_Bing_Source";
            this.groupBox_Bing_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Bing_Source.TabIndex = 0;
            this.groupBox_Bing_Source.TabStop = false;
            this.groupBox_Bing_Source.Text = "源语言";
            //
            // btn_Reset_Bing_Source
            //
            this.btn_Reset_Bing_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Bing_Source.Name = "btn_Reset_Bing_Source";
            this.btn_Reset_Bing_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Bing_Source.TabIndex = 1;
            this.btn_Reset_Bing_Source.Text = "重置";
            this.btn_Reset_Bing_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Bing_Source
            //
            this.textBox_Bing_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Bing_Source.Name = "textBox_Bing_Source";
            this.textBox_Bing_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Bing_Source.TabIndex = 0;
            //
            // tabPage_Bing2
            //
            this.tabPage_Bing2.Controls.Add(this.groupBox_Bing2_Key);
            this.tabPage_Bing2.Controls.Add(this.groupBox_Bing2_Target);
            this.tabPage_Bing2.Controls.Add(this.groupBox_Bing2_Source);
            this.tabPage_Bing2.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Bing2.Name = "tabPage_Bing2";
            this.tabPage_Bing2.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Bing2.TabIndex = 6;
            this.tabPage_Bing2.Text = "Bing2";
            this.tabPage_Bing2.UseVisualStyleBackColor = true;
            //
            // groupBox_Bing2_Key
            //
            this.groupBox_Bing2_Key.Controls.Add(this.label_Bing2_Notice);
            this.groupBox_Bing2_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Bing2_Key.Name = "groupBox_Bing2_Key";
            this.groupBox_Bing2_Key.Size = new System.Drawing.Size(370, 80);
            this.groupBox_Bing2_Key.TabIndex = 5;
            this.groupBox_Bing2_Key.TabStop = false;
            this.groupBox_Bing2_Key.Text = "说明";
            //
            // label_Bing2_Notice
            //
            this.label_Bing2_Notice.AutoSize = true;
            this.label_Bing2_Notice.Location = new System.Drawing.Point(6, 20);
            this.label_Bing2_Notice.Name = "label_Bing2_Notice";
            this.label_Bing2_Notice.Size = new System.Drawing.Size(200, 48);
            this.label_Bing2_Notice.TabIndex = 0;
            this.label_Bing2_Notice.Text = "Bing2 使用 Microsoft Edge 翻译API\r\n无需密钥即可使用\r\n支持自动检测语言";
            //
            // groupBox_Bing2_Target
            //
            this.groupBox_Bing2_Target.Controls.Add(this.btn_Reset_Bing2_Target);
            this.groupBox_Bing2_Target.Controls.Add(this.textBox_Bing2_Target);
            this.groupBox_Bing2_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Bing2_Target.Name = "groupBox_Bing2_Target";
            this.groupBox_Bing2_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Bing2_Target.TabIndex = 4;
            this.groupBox_Bing2_Target.TabStop = false;
            this.groupBox_Bing2_Target.Text = "目标语言";
            //
            // btn_Reset_Bing2_Target
            //
            this.btn_Reset_Bing2_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Bing2_Target.Name = "btn_Reset_Bing2_Target";
            this.btn_Reset_Bing2_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Bing2_Target.TabIndex = 1;
            this.btn_Reset_Bing2_Target.Text = "重置";
            this.btn_Reset_Bing2_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing2_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Bing2_Target
            //
            this.textBox_Bing2_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Bing2_Target.Name = "textBox_Bing2_Target";
            this.textBox_Bing2_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Bing2_Target.TabIndex = 0;
            //
            // groupBox_Bing2_Source
            //
            this.groupBox_Bing2_Source.Controls.Add(this.btn_Reset_Bing2_Source);
            this.groupBox_Bing2_Source.Controls.Add(this.textBox_Bing2_Source);
            this.groupBox_Bing2_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Bing2_Source.Name = "groupBox_Bing2_Source";
            this.groupBox_Bing2_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Bing2_Source.TabIndex = 3;
            this.groupBox_Bing2_Source.TabStop = false;
            this.groupBox_Bing2_Source.Text = "源语言";
            //
            // btn_Reset_Bing2_Source
            //
            this.btn_Reset_Bing2_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Bing2_Source.Name = "btn_Reset_Bing2_Source";
            this.btn_Reset_Bing2_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Bing2_Source.TabIndex = 1;
            this.btn_Reset_Bing2_Source.Text = "重置";
            this.btn_Reset_Bing2_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Bing2_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Bing2_Source
            //
            this.textBox_Bing2_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Bing2_Source.Name = "textBox_Bing2_Source";
            this.textBox_Bing2_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Bing2_Source.TabIndex = 0;
            //
            // tabPage_Microsoft
            //
            this.tabPage_Microsoft.Controls.Add(this.groupBox_Microsoft_Key);
            this.tabPage_Microsoft.Controls.Add(this.groupBox_Microsoft_Target);
            this.tabPage_Microsoft.Controls.Add(this.groupBox_Microsoft_Source);
            this.tabPage_Microsoft.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Microsoft.Name = "tabPage_Microsoft";
            this.tabPage_Microsoft.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Microsoft.TabIndex = 4;
            this.tabPage_Microsoft.Text = "Microsoft";
            this.tabPage_Microsoft.UseVisualStyleBackColor = true;
            //
            // groupBox_Microsoft_Key
            //
            this.groupBox_Microsoft_Key.Controls.Add(this.label_Microsoft_Key);
            this.groupBox_Microsoft_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Microsoft_Key.Name = "groupBox_Microsoft_Key";
            this.groupBox_Microsoft_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Microsoft_Key.TabIndex = 2;
            this.groupBox_Microsoft_Key.TabStop = false;
            this.groupBox_Microsoft_Key.Text = "密钥";
            //
            // label_Microsoft_Key
            //
            this.label_Microsoft_Key.AutoSize = true;
            this.label_Microsoft_Key.Location = new System.Drawing.Point(150, 25);
            this.label_Microsoft_Key.Name = "label_Microsoft_Key";
            this.label_Microsoft_Key.Size = new System.Drawing.Size(53, 12);
            this.label_Microsoft_Key.TabIndex = 0;
            this.label_Microsoft_Key.Text = "无需密钥";
            //
            // groupBox_Microsoft_Target
            //
            this.groupBox_Microsoft_Target.Controls.Add(this.btn_Reset_Microsoft_Target);
            this.groupBox_Microsoft_Target.Controls.Add(this.textBox_Microsoft_Target);
            this.groupBox_Microsoft_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Microsoft_Target.Name = "groupBox_Microsoft_Target";
            this.groupBox_Microsoft_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Microsoft_Target.TabIndex = 1;
            this.groupBox_Microsoft_Target.TabStop = false;
            this.groupBox_Microsoft_Target.Text = "目标语言";
            //
            // btn_Reset_Microsoft_Target
            //
            this.btn_Reset_Microsoft_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Microsoft_Target.Name = "btn_Reset_Microsoft_Target";
            this.btn_Reset_Microsoft_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Microsoft_Target.TabIndex = 1;
            this.btn_Reset_Microsoft_Target.Text = "重置";
            this.btn_Reset_Microsoft_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Microsoft_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Microsoft_Target
            //
            this.textBox_Microsoft_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Microsoft_Target.Name = "textBox_Microsoft_Target";
            this.textBox_Microsoft_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Microsoft_Target.TabIndex = 0;
            //
            // groupBox_Microsoft_Source
            //
            this.groupBox_Microsoft_Source.Controls.Add(this.btn_Reset_Microsoft_Source);
            this.groupBox_Microsoft_Source.Controls.Add(this.textBox_Microsoft_Source);
            this.groupBox_Microsoft_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Microsoft_Source.Name = "groupBox_Microsoft_Source";
            this.groupBox_Microsoft_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Microsoft_Source.TabIndex = 0;
            this.groupBox_Microsoft_Source.TabStop = false;
            this.groupBox_Microsoft_Source.Text = "源语言";
            //
            // btn_Reset_Microsoft_Source
            //
            this.btn_Reset_Microsoft_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Microsoft_Source.Name = "btn_Reset_Microsoft_Source";
            this.btn_Reset_Microsoft_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Microsoft_Source.TabIndex = 1;
            this.btn_Reset_Microsoft_Source.Text = "重置";
            this.btn_Reset_Microsoft_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Microsoft_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Microsoft_Source
            //
            this.textBox_Microsoft_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Microsoft_Source.Name = "textBox_Microsoft_Source";
            this.textBox_Microsoft_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Microsoft_Source.TabIndex = 0;
            //
            // tabPage_Yandex
            //
            this.tabPage_Yandex.Controls.Add(this.groupBox_Yandex_Key);
            this.tabPage_Yandex.Controls.Add(this.groupBox_Yandex_Target);
            this.tabPage_Yandex.Controls.Add(this.groupBox_Yandex_Source);
            this.tabPage_Yandex.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Yandex.Name = "tabPage_Yandex";
            this.tabPage_Yandex.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Yandex.TabIndex = 5;
            this.tabPage_Yandex.Text = "Yandex";
            this.tabPage_Yandex.UseVisualStyleBackColor = true;
            //
            // groupBox_Yandex_Key
            //
            this.groupBox_Yandex_Key.Controls.Add(this.label_Yandex_Key);
            this.groupBox_Yandex_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Yandex_Key.Name = "groupBox_Yandex_Key";
            this.groupBox_Yandex_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Yandex_Key.TabIndex = 2;
            this.groupBox_Yandex_Key.TabStop = false;
            this.groupBox_Yandex_Key.Text = "密钥";
            //
            // label_Yandex_Key
            //
            this.label_Yandex_Key.AutoSize = true;
            this.label_Yandex_Key.Location = new System.Drawing.Point(150, 25);
            this.label_Yandex_Key.Name = "label_Yandex_Key";
            this.label_Yandex_Key.Size = new System.Drawing.Size(53, 12);
            this.label_Yandex_Key.TabIndex = 0;
            this.label_Yandex_Key.Text = "无需密钥";
            //
            // groupBox_Yandex_Target
            //
            this.groupBox_Yandex_Target.Controls.Add(this.btn_Reset_Yandex_Target);
            this.groupBox_Yandex_Target.Controls.Add(this.textBox_Yandex_Target);
            this.groupBox_Yandex_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Yandex_Target.Name = "groupBox_Yandex_Target";
            this.groupBox_Yandex_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Yandex_Target.TabIndex = 1;
            this.groupBox_Yandex_Target.TabStop = false;
            this.groupBox_Yandex_Target.Text = "目标语言";
            //
            // btn_Reset_Yandex_Target
            //
            this.btn_Reset_Yandex_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Yandex_Target.Name = "btn_Reset_Yandex_Target";
            this.btn_Reset_Yandex_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Yandex_Target.TabIndex = 1;
            this.btn_Reset_Yandex_Target.Text = "重置";
            this.btn_Reset_Yandex_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Yandex_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Yandex_Target
            //
            this.textBox_Yandex_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Yandex_Target.Name = "textBox_Yandex_Target";
            this.textBox_Yandex_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Yandex_Target.TabIndex = 0;
            //
            // groupBox_Yandex_Source
            //
            this.groupBox_Yandex_Source.Controls.Add(this.btn_Reset_Yandex_Source);
            this.groupBox_Yandex_Source.Controls.Add(this.textBox_Yandex_Source);
            this.groupBox_Yandex_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Yandex_Source.Name = "groupBox_Yandex_Source";
            this.groupBox_Yandex_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Yandex_Source.TabIndex = 0;
            this.groupBox_Yandex_Source.TabStop = false;
            this.groupBox_Yandex_Source.Text = "源语言";
            //
            // btn_Reset_Yandex_Source
            //
            this.btn_Reset_Yandex_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Yandex_Source.Name = "btn_Reset_Yandex_Source";
            this.btn_Reset_Yandex_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Yandex_Source.TabIndex = 1;
            this.btn_Reset_Yandex_Source.Text = "重置";
            this.btn_Reset_Yandex_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Yandex_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Yandex_Source
            //
            this.textBox_Yandex_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Yandex_Source.Name = "textBox_Yandex_Source";
            this.textBox_Yandex_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Yandex_Source.TabIndex = 0;
            //
            // tabPage_TencentInteractive
            //
            this.tabPage_TencentInteractive.Controls.Add(this.groupBox_TencentInteractive_Key);
            this.tabPage_TencentInteractive.Controls.Add(this.groupBox_TencentInteractive_Target);
            this.tabPage_TencentInteractive.Controls.Add(this.groupBox_TencentInteractive_Source);
            this.tabPage_TencentInteractive.Location = new System.Drawing.Point(4, 22);
            this.tabPage_TencentInteractive.Name = "tabPage_TencentInteractive";
            this.tabPage_TencentInteractive.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_TencentInteractive.Size = new System.Drawing.Size(382, 303);
            this.tabPage_TencentInteractive.TabIndex = 7;
            this.tabPage_TencentInteractive.Text = "腾讯交互";
            this.tabPage_TencentInteractive.UseVisualStyleBackColor = true;
            //
            // groupBox_TencentInteractive_Key
            //
            this.groupBox_TencentInteractive_Key.Controls.Add(this.label_TencentInteractive_Key);
            this.groupBox_TencentInteractive_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_TencentInteractive_Key.Name = "groupBox_TencentInteractive_Key";
            this.groupBox_TencentInteractive_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_TencentInteractive_Key.TabIndex = 2;
            this.groupBox_TencentInteractive_Key.TabStop = false;
            this.groupBox_TencentInteractive_Key.Text = "密钥";
            //
            // label_TencentInteractive_Key
            //
            this.label_TencentInteractive_Key.AutoSize = true;
            this.label_TencentInteractive_Key.Location = new System.Drawing.Point(150, 25);
            this.label_TencentInteractive_Key.Name = "label_TencentInteractive_Key";
            this.label_TencentInteractive_Key.Size = new System.Drawing.Size(53, 12);
            this.label_TencentInteractive_Key.TabIndex = 0;
            this.label_TencentInteractive_Key.Text = "无需密钥";
            //
            // groupBox_TencentInteractive_Target
            //
            this.groupBox_TencentInteractive_Target.Controls.Add(this.btn_Reset_TencentInteractive_Target);
            this.groupBox_TencentInteractive_Target.Controls.Add(this.textBox_TencentInteractive_Target);
            this.groupBox_TencentInteractive_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_TencentInteractive_Target.Name = "groupBox_TencentInteractive_Target";
            this.groupBox_TencentInteractive_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_TencentInteractive_Target.TabIndex = 1;
            this.groupBox_TencentInteractive_Target.TabStop = false;
            this.groupBox_TencentInteractive_Target.Text = "目标语言";
            //
            // btn_Reset_TencentInteractive_Target
            //
            this.btn_Reset_TencentInteractive_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_TencentInteractive_Target.Name = "btn_Reset_TencentInteractive_Target";
            this.btn_Reset_TencentInteractive_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_TencentInteractive_Target.TabIndex = 1;
            this.btn_Reset_TencentInteractive_Target.Text = "重置";
            this.btn_Reset_TencentInteractive_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_TencentInteractive_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_TencentInteractive_Target
            //
            this.textBox_TencentInteractive_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_TencentInteractive_Target.Name = "textBox_TencentInteractive_Target";
            this.textBox_TencentInteractive_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_TencentInteractive_Target.TabIndex = 0;
            //
            // groupBox_TencentInteractive_Source
            //
            this.groupBox_TencentInteractive_Source.Controls.Add(this.btn_Reset_TencentInteractive_Source);
            this.groupBox_TencentInteractive_Source.Controls.Add(this.textBox_TencentInteractive_Source);
            this.groupBox_TencentInteractive_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_TencentInteractive_Source.Name = "groupBox_TencentInteractive_Source";
            this.groupBox_TencentInteractive_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_TencentInteractive_Source.TabIndex = 0;
            this.groupBox_TencentInteractive_Source.TabStop = false;
            this.groupBox_TencentInteractive_Source.Text = "源语言";
            //
            // btn_Reset_TencentInteractive_Source
            //
            this.btn_Reset_TencentInteractive_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_TencentInteractive_Source.Name = "btn_Reset_TencentInteractive_Source";
            this.btn_Reset_TencentInteractive_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_TencentInteractive_Source.TabIndex = 1;
            this.btn_Reset_TencentInteractive_Source.Text = "重置";
            this.btn_Reset_TencentInteractive_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_TencentInteractive_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_TencentInteractive_Source
            //
            this.textBox_TencentInteractive_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_TencentInteractive_Source.Name = "textBox_TencentInteractive_Source";
            this.textBox_TencentInteractive_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_TencentInteractive_Source.TabIndex = 0;
            //
            // tabPage_Caiyun
            //
            this.tabPage_Caiyun.Controls.Add(this.groupBox_Caiyun_Key);
            this.tabPage_Caiyun.Controls.Add(this.groupBox_Caiyun_Target);
            this.tabPage_Caiyun.Controls.Add(this.groupBox_Caiyun_Source);
            this.tabPage_Caiyun.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Caiyun.Name = "tabPage_Caiyun";
            this.tabPage_Caiyun.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Caiyun.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Caiyun.TabIndex = 8;
            this.tabPage_Caiyun.Text = "彩云小译";
            this.tabPage_Caiyun.UseVisualStyleBackColor = true;
            //
            // groupBox_Caiyun_Key
            //
            this.groupBox_Caiyun_Key.Controls.Add(this.label_Caiyun_Key);
            this.groupBox_Caiyun_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Caiyun_Key.Name = "groupBox_Caiyun_Key";
            this.groupBox_Caiyun_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Caiyun_Key.TabIndex = 2;
            this.groupBox_Caiyun_Key.TabStop = false;
            this.groupBox_Caiyun_Key.Text = "密钥";
            //
            // label_Caiyun_Key
            //
            this.label_Caiyun_Key.AutoSize = true;
            this.label_Caiyun_Key.Location = new System.Drawing.Point(150, 25);
            this.label_Caiyun_Key.Name = "label_Caiyun_Key";
            this.label_Caiyun_Key.Size = new System.Drawing.Size(53, 12);
            this.label_Caiyun_Key.TabIndex = 0;
            this.label_Caiyun_Key.Text = "无需密钥";
            //
            // groupBox_Caiyun_Target
            //
            this.groupBox_Caiyun_Target.Controls.Add(this.btn_Reset_Caiyun_Target);
            this.groupBox_Caiyun_Target.Controls.Add(this.textBox_Caiyun_Target);
            this.groupBox_Caiyun_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Caiyun_Target.Name = "groupBox_Caiyun_Target";
            this.groupBox_Caiyun_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Caiyun_Target.TabIndex = 1;
            this.groupBox_Caiyun_Target.TabStop = false;
            this.groupBox_Caiyun_Target.Text = "目标语言";
            //
            // btn_Reset_Caiyun_Target
            //
            this.btn_Reset_Caiyun_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Caiyun_Target.Name = "btn_Reset_Caiyun_Target";
            this.btn_Reset_Caiyun_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Caiyun_Target.TabIndex = 1;
            this.btn_Reset_Caiyun_Target.Text = "重置";
            this.btn_Reset_Caiyun_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Caiyun_Target
            //
            this.textBox_Caiyun_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Caiyun_Target.Name = "textBox_Caiyun_Target";
            this.textBox_Caiyun_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Caiyun_Target.TabIndex = 0;
            //
            // groupBox_Caiyun_Source
            //
            this.groupBox_Caiyun_Source.Controls.Add(this.btn_Reset_Caiyun_Source);
            this.groupBox_Caiyun_Source.Controls.Add(this.textBox_Caiyun_Source);
            this.groupBox_Caiyun_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Caiyun_Source.Name = "groupBox_Caiyun_Source";
            this.groupBox_Caiyun_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Caiyun_Source.TabIndex = 0;
            this.groupBox_Caiyun_Source.TabStop = false;
            this.groupBox_Caiyun_Source.Text = "源语言";
            //
            // btn_Reset_Caiyun_Source
            //
            this.btn_Reset_Caiyun_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Caiyun_Source.Name = "btn_Reset_Caiyun_Source";
            this.btn_Reset_Caiyun_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Caiyun_Source.TabIndex = 1;
            this.btn_Reset_Caiyun_Source.Text = "重置";
            this.btn_Reset_Caiyun_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Caiyun_Source
            //
            this.textBox_Caiyun_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Caiyun_Source.Name = "textBox_Caiyun_Source";
            this.textBox_Caiyun_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Caiyun_Source.TabIndex = 0;
            //
            // tabPage_Volcano
            //
            this.tabPage_Volcano.Controls.Add(this.groupBox_Volcano_Key);
            this.tabPage_Volcano.Controls.Add(this.groupBox_Volcano_Target);
            this.tabPage_Volcano.Controls.Add(this.groupBox_Volcano_Source);
            this.tabPage_Volcano.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Volcano.Name = "tabPage_Volcano";
            this.tabPage_Volcano.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Volcano.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Volcano.TabIndex = 9;
            this.tabPage_Volcano.Text = "火山翻译";
            this.tabPage_Volcano.UseVisualStyleBackColor = true;
            //
            // groupBox_Volcano_Key
            //
            this.groupBox_Volcano_Key.Controls.Add(this.label_Volcano_Key);
            this.groupBox_Volcano_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Volcano_Key.Name = "groupBox_Volcano_Key";
            this.groupBox_Volcano_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Volcano_Key.TabIndex = 2;
            this.groupBox_Volcano_Key.TabStop = false;
            this.groupBox_Volcano_Key.Text = "密钥";
            //
            // label_Volcano_Key
            //
            this.label_Volcano_Key.AutoSize = true;
            this.label_Volcano_Key.Location = new System.Drawing.Point(150, 25);
            this.label_Volcano_Key.Name = "label_Volcano_Key";
            this.label_Volcano_Key.Size = new System.Drawing.Size(53, 12);
            this.label_Volcano_Key.TabIndex = 0;
            this.label_Volcano_Key.Text = "无需密钥";
            //
            // groupBox_Volcano_Target
            //
            this.groupBox_Volcano_Target.Controls.Add(this.btn_Reset_Volcano_Target);
            this.groupBox_Volcano_Target.Controls.Add(this.textBox_Volcano_Target);
            this.groupBox_Volcano_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Volcano_Target.Name = "groupBox_Volcano_Target";
            this.groupBox_Volcano_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Volcano_Target.TabIndex = 1;
            this.groupBox_Volcano_Target.TabStop = false;
            this.groupBox_Volcano_Target.Text = "目标语言";
            //
            // btn_Reset_Volcano_Target
            //
            this.btn_Reset_Volcano_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Volcano_Target.Name = "btn_Reset_Volcano_Target";
            this.btn_Reset_Volcano_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Volcano_Target.TabIndex = 1;
            this.btn_Reset_Volcano_Target.Text = "重置";
            this.btn_Reset_Volcano_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Volcano_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Volcano_Target
            //
            this.textBox_Volcano_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Volcano_Target.Name = "textBox_Volcano_Target";
            this.textBox_Volcano_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Volcano_Target.TabIndex = 0;
            //
            // groupBox_Volcano_Source
            //
            this.groupBox_Volcano_Source.Controls.Add(this.btn_Reset_Volcano_Source);
            this.groupBox_Volcano_Source.Controls.Add(this.textBox_Volcano_Source);
            this.groupBox_Volcano_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Volcano_Source.Name = "groupBox_Volcano_Source";
            this.groupBox_Volcano_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Volcano_Source.TabIndex = 0;
            this.groupBox_Volcano_Source.TabStop = false;
            this.groupBox_Volcano_Source.Text = "源语言";
            //
            // btn_Reset_Volcano_Source
            //
            this.btn_Reset_Volcano_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Volcano_Source.Name = "btn_Reset_Volcano_Source";
            this.btn_Reset_Volcano_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Volcano_Source.TabIndex = 1;
            this.btn_Reset_Volcano_Source.Text = "重置";
            this.btn_Reset_Volcano_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Volcano_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Volcano_Source
            //
            this.textBox_Volcano_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Volcano_Source.Name = "textBox_Volcano_Source";
            this.textBox_Volcano_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Volcano_Source.TabIndex = 0;
            //
            // tabPage_Caiyun2
            //
            this.tabPage_Caiyun2.Controls.Add(this.groupBox_Caiyun2_Key);
            this.tabPage_Caiyun2.Controls.Add(this.groupBox_Caiyun2_Target);
            this.tabPage_Caiyun2.Controls.Add(this.groupBox_Caiyun2_Source);
            this.tabPage_Caiyun2.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Caiyun2.Name = "tabPage_Caiyun2";
            this.tabPage_Caiyun2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Caiyun2.Size = new System.Drawing.Size(382, 303);
            this.tabPage_Caiyun2.TabIndex = 10;
            this.tabPage_Caiyun2.Text = "彩云小译2";
            this.tabPage_Caiyun2.UseVisualStyleBackColor = true;
            //
            // groupBox_Caiyun2_Key
            //
            this.groupBox_Caiyun2_Key.Controls.Add(this.textBox_Caiyun2_Token);
            this.groupBox_Caiyun2_Key.Controls.Add(this.label_Caiyun2_Token);
            this.groupBox_Caiyun2_Key.Location = new System.Drawing.Point(6, 128);
            this.groupBox_Caiyun2_Key.Name = "groupBox_Caiyun2_Key";
            this.groupBox_Caiyun2_Key.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Caiyun2_Key.TabIndex = 2;
            this.groupBox_Caiyun2_Key.TabStop = false;
            this.groupBox_Caiyun2_Key.Text = "密钥";
            //
            // label_Caiyun2_Token
            //
            this.label_Caiyun2_Token.AutoSize = true;
            this.label_Caiyun2_Token.Location = new System.Drawing.Point(7, 23);
            this.label_Caiyun2_Token.Name = "label_Caiyun2_Token";
            this.label_Caiyun2_Token.Size = new System.Drawing.Size(41, 12);
            this.label_Caiyun2_Token.TabIndex = 0;
            this.label_Caiyun2_Token.Text = "Token:";
            //
            // textBox_Caiyun2_Token
            //
            this.textBox_Caiyun2_Token.Location = new System.Drawing.Point(80, 20);
            this.textBox_Caiyun2_Token.Name = "textBox_Caiyun2_Token";
            this.textBox_Caiyun2_Token.Size = new System.Drawing.Size(284, 21);
            this.textBox_Caiyun2_Token.TabIndex = 1;
            //
            // groupBox_Caiyun2_Target
            //
            this.groupBox_Caiyun2_Target.Controls.Add(this.btn_Reset_Caiyun2_Target);
            this.groupBox_Caiyun2_Target.Controls.Add(this.textBox_Caiyun2_Target);
            this.groupBox_Caiyun2_Target.Location = new System.Drawing.Point(6, 67);
            this.groupBox_Caiyun2_Target.Name = "groupBox_Caiyun2_Target";
            this.groupBox_Caiyun2_Target.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Caiyun2_Target.TabIndex = 1;
            this.groupBox_Caiyun2_Target.TabStop = false;
            this.groupBox_Caiyun2_Target.Text = "目标语言";
            //
            // btn_Reset_Caiyun2_Target
            //
            this.btn_Reset_Caiyun2_Target.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Caiyun2_Target.Name = "btn_Reset_Caiyun2_Target";
            this.btn_Reset_Caiyun2_Target.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Caiyun2_Target.TabIndex = 1;
            this.btn_Reset_Caiyun2_Target.Text = "重置";
            this.btn_Reset_Caiyun2_Target.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun2_Target.Click += new System.EventHandler(this.btn_Reset_Target_Click);
            //
            // textBox_Caiyun2_Target
            //
            this.textBox_Caiyun2_Target.Location = new System.Drawing.Point(6, 20);
            this.textBox_Caiyun2_Target.Name = "textBox_Caiyun2_Target";
            this.textBox_Caiyun2_Target.Size = new System.Drawing.Size(302, 21);
            this.textBox_Caiyun2_Target.TabIndex = 0;
            //
            // groupBox_Caiyun2_Source
            //
            this.groupBox_Caiyun2_Source.Controls.Add(this.btn_Reset_Caiyun2_Source);
            this.groupBox_Caiyun2_Source.Controls.Add(this.textBox_Caiyun2_Source);
            this.groupBox_Caiyun2_Source.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Caiyun2_Source.Name = "groupBox_Caiyun2_Source";
            this.groupBox_Caiyun2_Source.Size = new System.Drawing.Size(370, 55);
            this.groupBox_Caiyun2_Source.TabIndex = 0;
            this.groupBox_Caiyun2_Source.TabStop = false;
            this.groupBox_Caiyun2_Source.Text = "源语言";
            //
            // btn_Reset_Caiyun2_Source
            //
            this.btn_Reset_Caiyun2_Source.Location = new System.Drawing.Point(314, 19);
            this.btn_Reset_Caiyun2_Source.Name = "btn_Reset_Caiyun2_Source";
            this.btn_Reset_Caiyun2_Source.Size = new System.Drawing.Size(50, 23);
            this.btn_Reset_Caiyun2_Source.TabIndex = 1;
            this.btn_Reset_Caiyun2_Source.Text = "重置";
            this.btn_Reset_Caiyun2_Source.UseVisualStyleBackColor = true;
            this.btn_Reset_Caiyun2_Source.Click += new System.EventHandler(this.btn_Reset_Source_Click);
            //
            // textBox_Caiyun2_Source
            //
            this.textBox_Caiyun2_Source.Location = new System.Drawing.Point(6, 20);
            this.textBox_Caiyun2_Source.Name = "textBox_Caiyun2_Source";
            this.textBox_Caiyun2_Source.Size = new System.Drawing.Size(302, 21);
            this.textBox_Caiyun2_Source.TabIndex = 0;
            //
            // groupBox6
            //
            this.groupBox6.Controls.Add(this.btn_浏览);
            this.groupBox6.Controls.Add(this.textBox_path);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.cbBox_保存);
            this.groupBox6.Location = new System.Drawing.Point(6, 257);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(378, 63);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            // 
            // btn_浏览
            // 
            this.btn_浏览.BackColor = System.Drawing.Color.White;
            this.btn_浏览.Location = new System.Drawing.Point(332, 24);
            this.btn_浏览.Name = "btn_浏览";
            this.btn_浏览.Size = new System.Drawing.Size(40, 23);
            this.btn_浏览.TabIndex = 7;
            this.btn_浏览.Text = "浏览";
            this.btn_浏览.UseVisualStyleBackColor = false;
            this.btn_浏览.Click += new System.EventHandler(this.btn_浏览_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.BackColor = System.Drawing.Color.White;
            this.textBox_path.Location = new System.Drawing.Point(51, 26);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.ReadOnly = true;
            this.textBox_path.Size = new System.Drawing.Size(275, 21);
            this.textBox_path.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "目录：";
            // 
            // cbBox_保存
            // 
            this.cbBox_保存.AutoSize = true;
            this.cbBox_保存.BackColor = System.Drawing.Color.White;
            this.cbBox_保存.Location = new System.Drawing.Point(6, 0);
            this.cbBox_保存.Name = "cbBox_保存";
            this.cbBox_保存.Size = new System.Drawing.Size(72, 16);
            this.cbBox_保存.TabIndex = 2;
            this.cbBox_保存.Text = "自动保存";
            this.cbBox_保存.UseVisualStyleBackColor = false;
            this.cbBox_保存.CheckedChanged += new System.EventHandler(this.cbBox_保存_CheckedChanged);
            // 
            // 常规Button
            //
            this.常规Button.BackColor = System.Drawing.Color.White;
            this.常规Button.Location = new System.Drawing.Point(309, 327);
            this.常规Button.Name = "常规Button";
            this.常规Button.Size = new System.Drawing.Size(75, 23);
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
            this.groupBox2.Location = new System.Drawing.Point(6, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(378, 70);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "界面";
            // 
            // numbox_记录
            // 
            this.numbox_记录.Location = new System.Drawing.Point(276, 28);
            this.numbox_记录.Name = "numbox_记录";
            this.numbox_记录.Size = new System.Drawing.Size(63, 21);
            this.numbox_记录.TabIndex = 5;
            this.numbox_记录.ValueChanged += new System.EventHandler(this.numbox_记录_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
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
            this.cobBox_动画.Location = new System.Drawing.Point(84, 30);
            this.cobBox_动画.Name = "cobBox_动画";
            this.cobBox_动画.Size = new System.Drawing.Size(63, 20);
            this.cobBox_动画.TabIndex = 3;
            this.cobBox_动画.SelectedIndexChanged += new System.EventHandler(this.cobBox_动画_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "窗体动画：";
            // 
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.cbBox_输入翻译剪贴板);
            this.groupBox1.Controls.Add(this.chbox_取色);
            this.groupBox1.Controls.Add(this.cbBox_弹窗);
            this.groupBox1.Controls.Add(this.cbBox_翻译);
            this.groupBox1.Controls.Add(this.cbBox_开机);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 78);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            //
            // cbBox_输入翻译剪贴板
            //
            this.cbBox_输入翻译剪贴板.AutoSize = true;
            this.cbBox_输入翻译剪贴板.Location = new System.Drawing.Point(17, 46);
            this.cbBox_输入翻译剪贴板.Name = "cbBox_输入翻译剪贴板";
            this.cbBox_输入翻译剪贴板.Size = new System.Drawing.Size(192, 16);
            this.cbBox_输入翻译剪贴板.TabIndex = 7;
            this.cbBox_输入翻译剪贴板.Text = "输入翻译默认使用剪贴板文本";
            this.cbBox_输入翻译剪贴板.UseVisualStyleBackColor = true;
            //
            // chbox_取色
            //
            this.chbox_取色.AutoSize = true;
            this.chbox_取色.Location = new System.Drawing.Point(298, 20);
            this.chbox_取色.Name = "chbox_取色";
            this.chbox_取色.Size = new System.Drawing.Size(66, 16);
            this.chbox_取色.TabIndex = 6;
            this.chbox_取色.Text = "取色HEX";
            this.chbox_取色.UseVisualStyleBackColor = true;
            this.chbox_取色.CheckedChanged += new System.EventHandler(this.chbox_取色_CheckedChanged);
            //
            // cbBox_弹窗
            //
            this.cbBox_弹窗.AutoSize = true;
            this.cbBox_弹窗.Location = new System.Drawing.Point(204, 20);
            this.cbBox_弹窗.Name = "cbBox_弹窗";
            this.cbBox_弹窗.Size = new System.Drawing.Size(72, 16);
            this.cbBox_弹窗.TabIndex = 5;
            this.cbBox_弹窗.Text = "识别弹窗";
            this.cbBox_弹窗.UseVisualStyleBackColor = true;
            this.cbBox_弹窗.CheckedChanged += new System.EventHandler(this.cbBox_弹窗_CheckedChanged);
            //
            // cbBox_翻译
            //
            this.cbBox_翻译.AutoSize = true;
            this.cbBox_翻译.Location = new System.Drawing.Point(107, 20);
            this.cbBox_翻译.Name = "cbBox_翻译";
            this.cbBox_翻译.Size = new System.Drawing.Size(72, 16);
            this.cbBox_翻译.TabIndex = 4;
            this.cbBox_翻译.Text = "快速翻译";
            this.cbBox_翻译.UseVisualStyleBackColor = true;
            this.cbBox_翻译.CheckedChanged += new System.EventHandler(this.cbBox_翻译_CheckedChanged);
            //
            // cbBox_开机
            //
            this.cbBox_开机.AutoSize = true;
            this.cbBox_开机.Location = new System.Drawing.Point(17, 20);
            this.cbBox_开机.Name = "cbBox_开机";
            this.cbBox_开机.Size = new System.Drawing.Size(72, 16);
            this.cbBox_开机.TabIndex = 2;
            this.cbBox_开机.Text = "开机启动";
            this.cbBox_开机.UseVisualStyleBackColor = true;
            this.cbBox_开机.CheckedChanged += new System.EventHandler(this.cbBox_开机_CheckedChanged);
            // 
            // tab_标签
            // 
            this.tab_标签.Controls.Add(this.page_常规);
            this.tab_标签.Controls.Add(this.Page_快捷键);
            this.tab_标签.Controls.Add(this.Page_密钥);
            this.tab_标签.Controls.Add(this.Page_翻译接口);
            this.tab_标签.Controls.Add(this.Page_代理);
            this.tab_标签.Controls.Add(this.Page_更新);
            this.tab_标签.Controls.Add(this.Page_反馈);
            this.tab_标签.Controls.Add(this.Page_About);
            this.tab_标签.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tab_标签.Location = new System.Drawing.Point(10, 2);
            this.tab_标签.Name = "tab_标签";
            this.tab_标签.SelectedIndex = 0;
            this.tab_标签.Size = new System.Drawing.Size(398, 435);
            this.tab_标签.TabIndex = 0;
            this.tab_标签.SelectedIndexChanged += new System.EventHandler(this.tab_标签_SelectedIndexChanged);
            // 
            // Page_更新
            // 
            this.Page_更新.BackColor = System.Drawing.Color.White;
            this.Page_更新.Controls.Add(this.更新Button_check);
            this.Page_更新.Controls.Add(this.更新Button);
            this.Page_更新.Controls.Add(this.groupBox5);
            this.Page_更新.Location = new System.Drawing.Point(4, 22);
            this.Page_更新.Name = "Page_更新";
            this.Page_更新.Padding = new System.Windows.Forms.Padding(3);
            this.Page_更新.Size = new System.Drawing.Size(390, 329);
            this.Page_更新.TabIndex = 5;
            this.Page_更新.Text = "更新";
            this.Page_更新.UseVisualStyleBackColor = true;
            // 
            // 更新Button_check
            // 
            this.更新Button_check.BackColor = System.Drawing.Color.White;
            this.更新Button_check.Location = new System.Drawing.Point(6, 83);
            this.更新Button_check.Name = "更新Button_check";
            this.更新Button_check.Size = new System.Drawing.Size(75, 23);
            this.更新Button_check.TabIndex = 11;
            this.更新Button_check.Text = "检查更新";
            this.更新Button_check.UseVisualStyleBackColor = false;
            // 
            // 更新Button
            // 
            this.更新Button.BackColor = System.Drawing.Color.White;
            this.更新Button.Location = new System.Drawing.Point(309, 83);
            this.更新Button.Name = "更新Button";
            this.更新Button.Size = new System.Drawing.Size(75, 23);
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
            this.groupBox5.Location = new System.Drawing.Point(6, 14);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(378, 65);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(98, 31);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 3;
            this.label16.Text = "小时";
            // 
            // numbox_间隔时间
            // 
            this.numbox_间隔时间.Location = new System.Drawing.Point(54, 27);
            this.numbox_间隔时间.Name = "numbox_间隔时间";
            this.numbox_间隔时间.Size = new System.Drawing.Size(37, 21);
            this.numbox_间隔时间.TabIndex = 2;
            this.numbox_间隔时间.ValueChanged += new System.EventHandler(this.numbox_间隔时间_ValueChanged);
            // 
            // checkBox_更新间隔
            // 
            this.checkBox_更新间隔.AutoSize = true;
            this.checkBox_更新间隔.Location = new System.Drawing.Point(6, 31);
            this.checkBox_更新间隔.Name = "checkBox_更新间隔";
            this.checkBox_更新间隔.Size = new System.Drawing.Size(48, 16);
            this.checkBox_更新间隔.TabIndex = 1;
            this.checkBox_更新间隔.Text = "每隔";
            this.checkBox_更新间隔.UseVisualStyleBackColor = true;
            this.checkBox_更新间隔.CheckedChanged += new System.EventHandler(this.checkBox_更新间隔_CheckedChanged);
            // 
            // check_检查更新
            // 
            this.check_检查更新.AutoSize = true;
            this.check_检查更新.BackColor = System.Drawing.Color.White;
            this.check_检查更新.Location = new System.Drawing.Point(6, 0);
            this.check_检查更新.Name = "check_检查更新";
            this.check_检查更新.Size = new System.Drawing.Size(108, 16);
            this.check_检查更新.TabIndex = 0;
            this.check_检查更新.Text = "启动时检查更新";
            this.check_检查更新.UseVisualStyleBackColor = false;
            this.check_检查更新.CheckedChanged += new System.EventHandler(this.check_检查更新_CheckedChanged);
            // 
            // btn_音效
            // 
            this.btn_音效.BackColor = System.Drawing.Color.White;
            this.btn_音效.Image = global::TrOCR.Properties.Resources.语音按钮;
            this.btn_音效.Location = new System.Drawing.Point(344, 17);
            this.btn_音效.Name = "btn_音效";
            this.btn_音效.Size = new System.Drawing.Size(26, 23);
            this.btn_音效.TabIndex = 7;
            this.btn_音效.UseVisualStyleBackColor = false;
            this.btn_音效.Click += new System.EventHandler(this.btn_音效_Click);
            // 
            // pic_help
            // 
            this.pic_help.Image = global::TrOCR.Properties.Resources.帮助;
            this.pic_help.Location = new System.Drawing.Point(7, 378);
            this.pic_help.Name = "pic_help";
            this.pic_help.Size = new System.Drawing.Size(27, 23);
            this.pic_help.TabIndex = 7;
            this.pic_help.TabStop = false;
            this.pic_help.Click += new System.EventHandler(this.pic_help_Click);
            // 
            // pictureBox_识别界面
            //
            this.pictureBox_识别界面.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_识别界面.Location = new System.Drawing.Point(351, 151);
            this.pictureBox_识别界面.Name = "pictureBox_识别界面";
            this.pictureBox_识别界面.Size = new System.Drawing.Size(21, 21);
            this.pictureBox_识别界面.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_识别界面.TabIndex = 11;
            this.pictureBox_识别界面.TabStop = false;
            //
            // pictureBox_记录界面
            //
            this.pictureBox_记录界面.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_记录界面.Location = new System.Drawing.Point(351, 118);
            this.pictureBox_记录界面.Name = "pictureBox_记录界面";
            this.pictureBox_记录界面.Size = new System.Drawing.Size(21, 21);
            this.pictureBox_记录界面.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_记录界面.TabIndex = 10;
            this.pictureBox_记录界面.TabStop = false;
            // 
            // pictureBox_翻译文本
            // 
            this.pictureBox_翻译文本.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_翻译文本.Location = new System.Drawing.Point(351, 52);
            this.pictureBox_翻译文本.Name = "pictureBox_翻译文本";
            this.pictureBox_翻译文本.Size = new System.Drawing.Size(21, 21);
            this.pictureBox_翻译文本.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_翻译文本.TabIndex = 9;
            this.pictureBox_翻译文本.TabStop = false;
            // 
            // pictureBox_文字识别
            // 
            this.pictureBox_文字识别.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_文字识别.Location = new System.Drawing.Point(351, 19);
            this.pictureBox_文字识别.Name = "pictureBox_文字识别";
            this.pictureBox_文字识别.Size = new System.Drawing.Size(21, 21);
            this.pictureBox_文字识别.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_文字识别.TabIndex = 8;
            this.pictureBox_文字识别.TabStop = false;
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
            // txt_问题反馈
            // 
            this.txt_问题反馈.BackColor = System.Drawing.Color.White;
            this.txt_问题反馈.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 移除韩文输入法模式，使用默认输入法以支持所有语言
            // this.txt_问题反馈.ImeMode = System.Windows.Forms.ImeMode.HangulFull;
            this.txt_问题反馈.Location = new System.Drawing.Point(6, 6);
            this.txt_问题反馈.Multiline = true;
            this.txt_问题反馈.Name = "txt_问题反馈";
            this.txt_问题反馈.Size = new System.Drawing.Size(378, 134);
            this.txt_问题反馈.TabIndex = 5;
            // 
            // 反馈Button
            // 
            this.反馈Button.BackColor = System.Drawing.Color.White;
            this.反馈Button.Location = new System.Drawing.Point(309, 146);
            this.反馈Button.Name = "反馈Button";
            this.反馈Button.Size = new System.Drawing.Size(75, 23);
            this.反馈Button.TabIndex = 11;
            this.反馈Button.Text = "提交";
            this.反馈Button.UseVisualStyleBackColor = false;
            this.反馈Button.Click += new System.EventHandler(this.反馈Button_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(7, 151);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(0, 12);
            this.label21.TabIndex = 12;
            // 
            // Page_反馈
            // 
            this.Page_反馈.BackColor = System.Drawing.Color.White;
            this.Page_反馈.Controls.Add(this.label21);
            this.Page_反馈.Controls.Add(this.反馈Button);
            this.Page_反馈.Controls.Add(this.txt_问题反馈);
            this.Page_反馈.Location = new System.Drawing.Point(4, 22);
            this.Page_反馈.Name = "Page_反馈";
            this.Page_反馈.Padding = new System.Windows.Forms.Padding(3);
            this.Page_反馈.Size = new System.Drawing.Size(390, 329);
            this.Page_反馈.TabIndex = 8;
            this.Page_反馈.Text = "反馈";
            //
            // Page_翻译接口
            //
            this.Page_翻译接口.BackColor = System.Drawing.Color.White;
            this.Page_翻译接口.Controls.Add(this.tabControl_Trans);
            this.Page_翻译接口.Location = new System.Drawing.Point(4, 22);
            this.Page_翻译接口.Name = "Page_翻译接口";
            this.Page_翻译接口.Size = new System.Drawing.Size(390, 329);
            this.Page_翻译接口.TabIndex = 10;
            this.Page_翻译接口.Text = "翻译接口";
            //
            // Page_About
            //
            this.Page_About.BackColor = System.Drawing.Color.White;
            this.Page_About.Controls.Add(this.label_AuthorInfo);
            this.Page_About.Controls.Add(this.label_VersionInfo);
            this.Page_About.Location = new System.Drawing.Point(4, 22);
            this.Page_About.Name = "Page_About";
            this.Page_About.Size = new System.Drawing.Size(390, 329);
            this.Page_About.TabIndex = 9;
            this.Page_About.Text = "关于";
            //
            // label_VersionInfo
            //
            this.label_VersionInfo.AutoSize = true;
            this.label_VersionInfo.Location = new System.Drawing.Point(20, 20);
            this.label_VersionInfo.Name = "label_VersionInfo";
            this.label_VersionInfo.Size = new System.Drawing.Size(53, 12);
            this.label_VersionInfo.TabIndex = 0;
            this.label_VersionInfo.Text = "版本号：";
            //
            // label_AuthorInfo
            //
            this.label_AuthorInfo.AutoSize = true;
            this.label_AuthorInfo.Location = new System.Drawing.Point(20, 50);
            this.label_AuthorInfo.Name = "label_AuthorInfo";
            this.label_AuthorInfo.Size = new System.Drawing.Size(41, 12);
            this.label_AuthorInfo.TabIndex = 1;
            this.label_AuthorInfo.Text = "作者：";
            //
            // FmSetting
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(417, 448);
            this.Controls.Add(this.tab_标签);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Page_代理.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.Page_密钥.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.inPage_百度接口.ResumeLayout(false);
            this.inPage_百度接口.PerformLayout();
            this.inPage_百度高精度接口.ResumeLayout(false);
            this.inPage_百度高精度接口.PerformLayout();
            this.inPage腾讯接口.ResumeLayout(false);
            this.inPage腾讯接口.PerformLayout();
            this.inPage腾讯高精度接口.ResumeLayout(false);
            this.inPage腾讯高精度接口.PerformLayout();
            this.inPage白描接口.ResumeLayout(false);
            this.inPage白描接口.PerformLayout();
            this.Page_快捷键.ResumeLayout(false);
            this.Page_快捷键.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            //
            // label_输入翻译
            //
            this.label_输入翻译.AutoSize = true;
            this.label_输入翻译.Location = new System.Drawing.Point(6, 91);
            this.label_输入翻译.Name = "label_输入翻译";
            this.label_输入翻译.Size = new System.Drawing.Size(65, 12);
            this.label_输入翻译.TabIndex = 12;
            this.label_输入翻译.Text = "输入翻译：";
            //
            // txtBox_输入翻译
            //
            this.txtBox_输入翻译.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBox_输入翻译.Location = new System.Drawing.Point(78, 85);
            this.txtBox_输入翻译.Name = "txtBox_输入翻译";
            this.txtBox_输入翻译.Size = new System.Drawing.Size(260, 23);
            this.txtBox_输入翻译.TabIndex = 6;
            this.txtBox_输入翻译.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_输入翻译.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtBox_输入翻译.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            //
            // pictureBox_输入翻译
            //
            this.pictureBox_输入翻译.Image = global::TrOCR.Properties.Resources.快捷键_1;
            this.pictureBox_输入翻译.Location = new System.Drawing.Point(351, 85);
            this.pictureBox_输入翻译.Name = "pictureBox_输入翻译";
            this.pictureBox_输入翻译.Size = new System.Drawing.Size(21, 21);
            this.pictureBox_输入翻译.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_输入翻译.TabIndex = 14;
            this.pictureBox_输入翻译.TabStop = false;
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_记录)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
            this.tabPage_Caiyun2.ResumeLayout(false);
            this.groupBox_Caiyun2_Source.ResumeLayout(false);
            this.groupBox_Caiyun2_Source.PerformLayout();
            this.groupBox_Caiyun2_Target.ResumeLayout(false);
            this.groupBox_Caiyun2_Target.PerformLayout();
            this.groupBox_Caiyun2_Key.ResumeLayout(false);
            this.groupBox_Caiyun2_Key.PerformLayout();
            this.page_常规.ResumeLayout(false);
            this.Page_翻译接口.ResumeLayout(false);
            this.tab_标签.ResumeLayout(false);
            this.Page_更新.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbox_间隔时间)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_help)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_识别界面)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_记录界面)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_翻译文本)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_文字识别)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_输入翻译)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.Page_反馈.ResumeLayout(false);
            this.Page_反馈.PerformLayout();
            this.Page_About.ResumeLayout(false);
            this.Page_About.PerformLayout();
            this.ResumeLayout(false);

        }

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
        private global::System.Windows.Forms.CheckBox cbBox_输入翻译剪贴板;
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
        private global::System.Windows.Forms.Label label8;
        private global::System.Windows.Forms.CheckBox cbBox_翻译;
        private global::System.Windows.Forms.TabControl tabControl2;
        private global::System.Windows.Forms.TabPage inPage_百度接口;
        private global::System.Windows.Forms.TabPage inPage_百度高精度接口;
        private global::System.Windows.Forms.TextBox text_baidupassword;
        private global::System.Windows.Forms.TextBox text_baiduaccount;
        private global::System.Windows.Forms.Label label10;
        private global::System.Windows.Forms.Label label9;
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
        private System.Windows.Forms.TabPage inPage腾讯接口;
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
        private System.Windows.Forms.TabPage inPage白描接口;
        private System.Windows.Forms.TextBox BoxBaimiaoPassword;
        private System.Windows.Forms.TextBox BoxBaimiaoUsername;
        private System.Windows.Forms.Label label_BaimiaoPassword;
        private System.Windows.Forms.Label label_BaimiaoUsername;
        private System.Windows.Forms.ComboBox comboBox_Baidu_Language;
        private System.Windows.Forms.Label label_Baidu_Language;
        private System.Windows.Forms.TextBox text_baidu_accurate_secretkey;
        private System.Windows.Forms.TextBox text_baidu_accurate_apikey;
        private System.Windows.Forms.Label label_baidu_accurate_secretkey;
        private System.Windows.Forms.Label label_baidu_accurate_apikey;
        private System.Windows.Forms.ComboBox comboBox_Baidu_Accurate_Language;
        private System.Windows.Forms.Label label_Baidu_Accurate_Language;
        private System.Windows.Forms.ComboBox comboBox_Tencent_Language;
        private System.Windows.Forms.Label label_Tencent_Language;
        private System.Windows.Forms.TabPage inPage腾讯高精度接口;
        private System.Windows.Forms.TextBox text_tencent_accurate_secretkey;
        private System.Windows.Forms.TextBox text_tencent_accurate_secretid;
        private System.Windows.Forms.Label label_tencent_accurate_secretkey;
        private System.Windows.Forms.Label label_tencent_accurate_secretid;
        private System.Windows.Forms.ComboBox comboBox_Tencent_Accurate_Language;
        private System.Windows.Forms.Label label_Tencent_Accurate_Language;
    }
}
