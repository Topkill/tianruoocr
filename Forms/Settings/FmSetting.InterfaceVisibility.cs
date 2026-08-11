using System;
using System.Windows.Forms;
using TrOCR.Helper;

namespace TrOCR
{
    /// <summary>
    /// FmSetting 的 partial：接口显隐元数据表与刷新逻辑。
    /// 决策由 InterfaceVisibility 纯函数承担，本文件只做控件增删执行。
    /// </summary>
    public sealed partial class FmSetting
    {
        /// <summary>
        /// 单个接口的显隐元数据。
        /// TabPage/ParentTabControl 为 null 表示该接口在设置页无对应 TabPage（只控主窗口右键菜单）。
        /// </summary>
        private struct InterfaceVisibilityEntry
        {
            public string Name;
            public string Section;
            public string Key;
            public CheckBox CheckBox;
            public TabPage TabPage;
            public TabControl ParentTabControl;
            public int OriginalIndex;
            public bool DefaultVisible;
            public string InUseSource;
            public string[] InUseValues;
        }

        private InterfaceVisibilityEntry[] _interfaceEntries;

        /// <summary>
        /// 构建 31 条接口元数据表（翻译 12 + OCR 19）。
        /// 必须在 InitializeComponent 之后调用（引用实例控件）。
        /// 数据已与原绑定段/读取段/保存段/使用中检查四处硬编码逐条核对一致。
        /// </summary>
        private void BuildInterfaceEntries()
        {
            _interfaceEntries = new[]
            {
                // ===== 翻译接口（12）— 父容器 tabControl_Trans，使用中查 INI key "翻译接口" =====
                Entry("Google",             "翻译接口显示", "Google",             checkBox_ShowGoogle,             tabPage_Google,             tabControl_Trans, 0,  true, "翻译接口", "谷歌"),
                Entry("Baidu",              "翻译接口显示", "Baidu",              checkBox_ShowBaidu,              tabPage_Baidu,              tabControl_Trans, 1,  true, "翻译接口", "百度"),
                Entry("Tencent",            "翻译接口显示", "Tencent",            checkBox_ShowTencent,            tabPage_Tencent,            tabControl_Trans, 2,  true, "翻译接口", "腾讯"),
                Entry("Bing",               "翻译接口显示", "Bing",               checkBox_ShowBing,               tabPage_Bing,               tabControl_Trans, 3,  true, "翻译接口", "Bing"),
                Entry("Bing2",              "翻译接口显示", "Bing2",              checkBox_ShowBing2,              tabPage_Bing2,              tabControl_Trans, 4,  true, "翻译接口", "Bing2"),
                Entry("Microsoft",          "翻译接口显示", "Microsoft",          checkBox_ShowMicrosoft,          tabPage_Microsoft,          tabControl_Trans, 5,  true, "翻译接口", "Microsoft"),
                Entry("Yandex",             "翻译接口显示", "Yandex",             checkBox_ShowYandex,             tabPage_Yandex,             tabControl_Trans, 6,  true, "翻译接口", "Yandex"),
                Entry("TencentInteractive", "翻译接口显示", "TencentInteractive", checkBox_ShowTencentInteractive, tabPage_TencentInteractive, tabControl_Trans, 7,  false, "翻译接口", "腾讯交互翻译"),
                Entry("Caiyun",             "翻译接口显示", "Caiyun",             checkBox_ShowCaiyun,             tabPage_Caiyun,             tabControl_Trans, 8,  false, "翻译接口", "彩云小译"),
                Entry("Volcano",            "翻译接口显示", "Volcano",            checkBox_ShowVolcano,            tabPage_Volcano,            tabControl_Trans, 9,  false, "翻译接口", "火山翻译"),
                Entry("Caiyun2",            "翻译接口显示", "Caiyun2",            checkBox_ShowCaiyun2,            tabPage_Caiyun2,            tabControl_Trans, 10, true, "翻译接口", "彩云小译2"),
                Entry("Baidu2",             "翻译接口显示", "Baidu2",             checkBox_ShowBaidu2,             tabPage_Baidu2,             tabControl_Trans, 11, false, "翻译接口", "百度2"),

                // ===== OCR 接口（19）— 使用中查 INI key "接口" =====
                // 嵌套 TabControl：百度（修复 Q1——原代码错误地传给 tabControl2，导致 Remove 空操作）
                Entry("Baidu",              "Ocr接口显示", "Baidu",              checkBox_ShowOcrBaidu,           inPage_百度接口,      tabControl_BaiduApiType, 0, true, "接口", "中英", "日语", "韩语"),
                Entry("BaiduAccurate",      "Ocr接口显示", "BaiduAccurate",      checkBox_ShowOcrBaiduAccurate,   inPage_百度高精度接口, tabControl_BaiduApiType, 1, true, "接口", "百度-高精度"),
                // 嵌套 TabControl：腾讯
                Entry("Tencent",            "Ocr接口显示", "Tencent",            checkBox_ShowOcrTencent,         inPage_腾讯接口,      tabControl_TXApiType,    0, true, "接口", "腾讯"),
                Entry("TencentAccurate",    "Ocr接口显示", "TencentAccurate",    checkBox_ShowOcrTencentAccurate, inPage_腾讯高精度接口, tabControl_TXApiType,    1, true, "接口", "腾讯-高精度"),
                // tabControl2 直接子页（注意：inPage_PaddleOCR/RapidOCR 虽叫 inPage_ 但是 tabControl2 直接子页）
                Entry("Baimiao",            "Ocr接口显示", "Baimiao",            checkBox_ShowOcrBaimiao,         tabPage_白描接口,     tabControl2,             2, false, "接口", "白描"),
                Entry("PaddleOCR",          "Ocr接口显示", "PaddleOCR",          checkBox_ShowOcrPaddleOCR,       inPage_PaddleOCR,     tabControl2,             3, true, "接口", "PaddleOCR"),
                Entry("PaddleOCR2",         "Ocr接口显示", "PaddleOCR2",         checkBox_ShowOcrPaddleOCR2,      inPage_PaddleOCR2,    tabControl2,             4, true, "接口", "PaddleOCR2"),
                Entry("RapidOCR",           "Ocr接口显示", "RapidOCR",           checkBox_ShowOcrRapidOCR,        inPage_RapidOCR,      tabControl2,             5, true, "接口", "RapidOCR"),
                // 无设置页（只控主窗口右键菜单显隐，TabPage/Parent 传 null）
                Entry("Sougou",             "Ocr接口显示", "Sougou",            checkBox_ShowOcrSougou,          null, null, -1, true, "接口", "搜狗"),
                Entry("Youdao",             "Ocr接口显示", "Youdao",            checkBox_ShowOcrYoudao,          null, null, -1, true, "接口", "有道"),
                Entry("WeChat",             "Ocr接口显示", "WeChat",            checkBox_ShowOcrWeChat,          null, null, -1, true, "接口", "微信"),
                Entry("Mathfuntion",        "Ocr接口显示", "Mathfuntion",       checkBox_ShowOcrMathfuntion,     null, null, -1, true, "接口", "公式"),
                Entry("Table",              "Ocr接口显示", "Table",             checkBox_ShowOcrTable,           null, null, -1, true, "接口", "百度表格", "阿里表格"),
                Entry("Shupai",             "Ocr接口显示", "Shupai",            checkBox_ShowOcrShupai,          null, null, -1, true, "接口", "从左向右", "从右向左"),
                Entry("TableBaidu",         "Ocr接口显示", "TableBaidu",        checkBox_ShowOcrTableBaidu,      null, null, -1, true, "接口", "百度表格"),
                Entry("TableAli",           "Ocr接口显示", "TableAli",          checkBox_ShowOcrTableAli,        null, null, -1, true, "接口", "阿里表格"),
                Entry("ShupaiLR",           "Ocr接口显示", "ShupaiLR",          checkBox_ShowOcrShupaiLR,        null, null, -1, true, "接口", "从左向右"),
                Entry("ShupaiRL",           "Ocr接口显示", "ShupaiRL",          checkBox_ShowOcrShupaiRL,        null, null, -1, true, "接口", "从右向左"),
                Entry("TencentTable",       "Ocr接口显示", "TencentTable",      checkBox_ShowOcrTableTencent,    null, null, -1, true, "接口", "腾讯表格"),
            };
        }

        /// <summary>元数据条目构造辅助（params 简化 InUseValues 书写）</summary>
        private static InterfaceVisibilityEntry Entry(
            string name, string section, string key,
            CheckBox checkBox, TabPage tabPage, TabControl parent, int originalIndex,
            bool defaultVisible, string inUseSource, params string[] inUseValues)
        {
            return new InterfaceVisibilityEntry
            {
                Name = name,
                Section = section,
                Key = key,
                CheckBox = checkBox,
                TabPage = tabPage,
                ParentTabControl = parent,
                OriginalIndex = originalIndex,
                DefaultVisible = defaultVisible,
                InUseSource = inUseSource,
                InUseValues = inUseValues
            };
        }

        /// <summary>
        /// 幂等刷新：按当前 CheckBox.Checked 状态，对每个有 TabPage 的条目
        /// 决定 Insert/Remove/None，使设置页 TabPage 显隐与勾选状态一致。
        /// 用 OriginalIndex 插回以保持顺序稳定。
        /// </summary>
        private void RefreshInterfaceTabPages()
        {
            if (_interfaceEntries == null) return;

            foreach (var e in _interfaceEntries)
            {
                if (e.TabPage == null || e.ParentTabControl == null) continue;

                bool currentlyInParent = e.ParentTabControl.TabPages.Contains(e.TabPage);
                switch (InterfaceVisibility.ComputePageAction(e.CheckBox.Checked, currentlyInParent))
                {
                    case InterfaceVisibility.PageAction.Insert:
                        e.ParentTabControl.TabPages.Insert(e.OriginalIndex, e.TabPage);
                        break;
                    case InterfaceVisibility.PageAction.Remove:
                        e.ParentTabControl.TabPages.Remove(e.TabPage);
                        break;
                }
            }
        }
    }
}
