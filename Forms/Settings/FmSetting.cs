using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using TrOCR.Helper;
using TrOCR.Helper.Models;
using TrOCR.Properties;


namespace TrOCR
{
    // 设置窗口类，用于管理OCR和翻译接口的各种配置选项
	public sealed partial class FmSetting
	{
        // === 自定义 AI 接口相关变量 ===

        // 使用 BindingList 可以在数据修改时自动通知 UI 刷新
        private BindingList<CustomAIProvider> _customProviders;

        // 当前正在编辑的对象引用
        private CustomAIProvider _currentEditingProvider = null;

        // 防抖标志：防止代码赋值 Text 属性时触发 TextChanged 事件导致死循环
        private bool _isUserAction = true;

        // 使用 BindingList 可以在数据修改时自动通知 UI 刷新
        private BindingList<CustomAITransProvider> _customTransProviders;

        // 当前正在编辑的对象引用
        private CustomAITransProvider _currentEditingTransProvider = null;

        // 防抖标志：防止代码赋值 Text 属性时触发 TextChanged 事件导致死循环
        private bool _isUserActionTrans = true;

        private Dictionary<Control, Point> _originalControlLocations;

		private readonly Dictionary<string, string> shortcutMappings = new Dictionary<string, string>
		{
			{ "txtBox_文字识别", "文字识别" },
			{ "txtBox_翻译文本", "翻译文本" },
			{ "txtBox_记录界面", "记录界面" },
			{ "txtBox_识别界面", "识别界面" },
			{ "txtBox_输入翻译", "输入翻译" },
			{ "txtBox_静默识别", "静默识别" },
			{ "txtBox_截图翻译", "截图翻译" },

		};
		private bool paddleOcrConfigChanged = false;
    	private bool paddleOcr2ConfigChanged = false;
    	private bool rapidOcrConfigChanged = false;

        // 滚轮事件处理逻辑
        //private void tab_标签_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    // 获取当前鼠标位置，判断是否在“标签头”区域
        //    // 如果你不加这个判断，当你在 TabPage 内容里滚动（比如看长文本）时，也会触发切页，会把人气死
        //    Rectangle headerRect = this.tab_标签.DisplayRectangle;
        //    // DisplayRectangle 是内容区，我们判断鼠标如果不在内容区，那就在标题区
        //    // 或者简单粗暴判断鼠标 Y 坐标小于 30 (假设标签高度是30)

        //    // 这里使用更严谨的判断：只当鼠标在组件顶部范围时才切页
        //    bool isOverHeader = e.Location.Y < headerRect.Y;

        //    if (isOverHeader)
        //    {
        //        // 获取当前选中索引
        //        int index = this.tab_标签.SelectedIndex;

        //        // e.Delta > 0 代表滚轮向上推 (向前翻)
        //        if (e.Delta > 0)
        //        {
        //            if (index > 0)
        //                this.tab_标签.SelectedIndex = index - 1;
        //        }
        //        // e.Delta < 0 代表滚轮向下滚 (向后翻)
        //        else
        //        {
        //            if (index < this.tab_标签.TabCount - 1)
        //                this.tab_标签.SelectedIndex = index + 1;
        //        }

        //        // 标记事件已处理，防止冒泡
        //        if (e is HandledMouseEventArgs he) he.Handled = true;
        //    }
        //}
        // 用一个字典来记录每个 TabControl 当前积攒的滚动值
        private Dictionary<object, int> _scrollAccumulators = new Dictionary<object, int>();

        // 设定阈值：值越大，切换越慢/越难
        // 标准鼠标滚动一格通常是 120。
        // 设为 120 = 滚1格切一次（太快）
        // 设为 240 = 滚2格切一次（适中）
        // 设为 360 = 滚3格切一次（比较稳）
        private const int SCROLL_THRESHOLD = 240;
		//阈值随意，不是120的倍数也行，任意整数皆可

        // === 鼠标悬停自动切换标签页相关变量 ===
        private System.Windows.Forms.Timer _tabHoverTimer;// 当前正在计时的 TabControl
        private TabControl _currentHoverTabControl;// 当前正在计时的 Tab 索引
        private int _currentHoverTabIndex = -1;
        private const int HOVER_SWITCH_DELAY = 600; // 悬停触发延迟（毫秒），可根据需要调整


    }
}
