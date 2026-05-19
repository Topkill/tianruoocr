// FmLoadingExport.cs
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrOCR
{
    public partial class FmLoadingExport : Form
    {
        // 这个Action变量将用来存储耗时的“任务”
        private readonly Action _work;
        private readonly Action<Exception> _onError;

        public FmLoadingExport(Action work, Action<Exception> onError)
        {
            _work = work;
            _onError = onError;

            InitializeComponent();

            // 设置所有控件的样式
             // ---  在这里用代码设置所有新样式  ---
            // 1. 设置边框为固定的对话框样式
            this.FormBorderStyle = FormBorderStyle.FixedDialog; 
            
            // 2. 使用系统默认的控件背景色，而不是纯白
            this.BackColor = SystemColors.Control; 

            // 3. 其他核心属性保持不变
            this.StartPosition = FormStartPosition.CenterParent;
            this.ControlBox = false; // 不显示关闭、最大化、最小化按钮
            this.ShowInTaskbar = false;
            this.Size = new Size(320, 60); // 可以稍微调整大小以适应边框
            
            // 设置标签样式
            Label lblMessage = new Label
            {
                Text = "正在生成 Excel 文件，请稍候...",
                // Font = new Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134), // 字体可以微调
                Font = new Font("微软雅黑", 10F),
                AutoSize = false,
                Size = this.ClientSize,
                TextAlign = ContentAlignment.MiddleCenter
            };


    

            this.Controls.Add(lblMessage);
        }

        // 当窗体第一次显示时，触发这个事件
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // 启动一个后台任务来执行耗时的工作
            Task.Run(() =>
            {
                try
                {
                    // 执行我们传进来的“任务”
                    _work.Invoke();

                    // 任务成功完成后，在UI线程上关闭自己
                    this.Invoke((MethodInvoker)delegate {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    });
                }
                catch (Exception ex)
                {
                    // 如果任务出错，在UI线程上调用错误处理并关闭
                    this.Invoke((MethodInvoker)delegate {
                        _onError.Invoke(ex);
                        this.DialogResult = DialogResult.Abort;
                        this.Close();
                    });
                }
            });
        }

        #region Windows Form Designer generated code
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "FmLoadingExport";
            this.ResumeLayout(false);
        }
        #endregion
    }
}