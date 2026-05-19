namespace TrOCR
{

	public partial class FmLoading : global::System.Windows.Forms.Form
	{

		protected override void Dispose(bool disposing)
		{
			if (disposing)
    		{
    		    // --- 修复关键点 开始 ---

    		    // 1. 停止并释放 Timer
    		    if (this.timer != null)
    		    {
    		        this.timer.Stop();
    		        this.timer.Dispose();
    		        this.timer = null;
    		    }

    		    // 2. 释放最后持有的 Image 对象
    		    if (this.bgImg != null)
    		    {
    		        this.bgImg.Dispose();
    		        this.bgImg = null;
    		    }

    		    // --- 修复关键点 结束 ---

    		    // 保留设计器生成的代码
    		    if (this.components != null)
    		    {
    		        this.components.Dispose();
    		    }
    		}
    		base.Dispose(disposing);
			// bool flag = disposing && this.components != null;
			// bool flag2 = flag;
			// bool flag3 = flag2;
			// bool flag4 = flag3;
			// bool flag5 = flag4;
			// bool flag6 = flag5;
			// bool flag7 = flag6;
			// bool flag8 = flag7;
			// bool flag9 = flag8;
			// if (flag9)
			// {
			// 	this.components.Dispose();
			// }
			// base.Dispose(disposing);
		}

		public global::System.ComponentModel.IContainer components;
	}
}
