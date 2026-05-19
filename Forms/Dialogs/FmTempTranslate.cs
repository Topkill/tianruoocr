using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
    public partial class FmTempTranslate : Form
    {
        // 公共属性，用于让外部获取用户输入的值
        public string SourceLanguage { get; private set; }
        public string TargetLanguage { get; private set; }

        public FmTempTranslate()
        {
            InitializeComponent();
            // 预填充一些常用值，方便用户
            txtSourceLang.Text = "en";
            txtTargetLang.Text = "zh";
            // 从静态变量加载上次使用的语言
            txtSourceLang.Text = StaticValue.LastTempSourceLang;
            txtTargetLang.Text = StaticValue.LastTempTargetLang;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // 在点击OK时，保存文本框的值到公共属性
            SourceLanguage = txtSourceLang.Text.Trim();
            TargetLanguage = txtTargetLang.Text.Trim();
            // DialogResult 已在设计器中设置，这里不需要额外代码
        }
    }
}
