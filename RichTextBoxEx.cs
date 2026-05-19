using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{

	public class RichTextBoxEx : HelpRepaint.AdvRichTextBox
    {
        // // 1. 加入这个构造函数
        // public RichTextBoxEx()
        // {
        //     // 如果程序运行起来没有弹窗，说明你运行的根本不是这份代码！
        //     // 请检查上述“第1点：编译失败”
        //     MessageBox.Show("当前 RichTextBoxEx 代码已加载！", "代码验证");
        // }

        // // 2. 加入这个重写方法
        // protected override CreateParams CreateParams
        // {
        //     get
        //     {
        //         CreateParams cp = base.CreateParams;
        //         // 弹窗显示当前实际请求的类名
        //         MessageBox.Show("当前请求的内核 ClassName 是: " + cp.ClassName, "内核验证");
        //         return cp;
        //     }
        // }

        protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string path);


        [Bindable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[SettingsBindable(true)]
		[DefaultValue(false)]
		[Category("Appearance")]
		public string Rtf2
		{
			get
			{
				return Rtf;
			}
			set
			{
				Rtf = value;
			}
		}

		private IContainer components;

		private static IntPtr moduleHandle;
	}
}
