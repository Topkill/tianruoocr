using System;

namespace TrOCR.Helper
{
    /// <summary>
    /// 接口显隐的纯决策逻辑。
    /// 决策与执行分离：本类只决定"该做什么动作"，
    /// 控件增删由 FmSetting.RefreshInterfaceTabPages 执行。
    /// </summary>
    public static class InterfaceVisibility
    {
        /// <summary>TabPage 增删动作</summary>
        public enum PageAction
        {
            None,
            Insert,
            Remove
        }

        /// <summary>
        /// 根据"期望可见"与"当前是否在父容器中"决定增删动作。
        /// 期望可见但不在 → Insert；期望不可见但在 → Remove；其余 → None。
        /// </summary>
        public static PageAction ComputePageAction(bool desiredVisible, bool currentlyInParent)
        {
            if (desiredVisible && !currentlyInParent) return PageAction.Insert;
            if (!desiredVisible && currentlyInParent) return PageAction.Remove;
            return PageAction.None;
        }

        /// <summary>
        /// 解析 INI 值为初始可见性。
        /// IniHelper 以 "发生错误" 表示键缺失，此时取 defaultVisible；
        /// 畸形值（非 True/False）同样兜底取 defaultVisible，与项目其它读取模板一致。
        /// </summary>
        public static bool ResolveInitialVisibility(string iniValue, bool defaultVisible)
        {
            if (iniValue == "发生错误") return defaultVisible;
            try { return Convert.ToBoolean(iniValue); }
            catch { return defaultVisible; }
        }
    }
}
