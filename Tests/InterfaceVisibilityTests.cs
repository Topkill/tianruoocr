using NUnit.Framework;
using TrOCR.Helper;

namespace TrOCR.Tests
{
    /// <summary>
    /// InterfaceVisibility 纯函数单测。
    /// 覆盖显隐动作决策（4 种组合）与初始可见性解析（正常/缺失/畸形 INI 值）。
    /// 保护"决策逻辑写反"和"默认值抄错"两类重构失误。
    /// </summary>
    [TestFixture]
    public class InterfaceVisibilityTests
    {
        // ---- ComputePageAction：4 种输入组合 ----

        [Test]
        public void ComputePageAction_VisibleButAbsent_ReturnsInsert()
        {
            Assert.That(InterfaceVisibility.ComputePageAction(true, false),
                Is.EqualTo(InterfaceVisibility.PageAction.Insert));
        }

        [Test]
        public void ComputePageAction_HiddenButPresent_ReturnsRemove()
        {
            Assert.That(InterfaceVisibility.ComputePageAction(false, true),
                Is.EqualTo(InterfaceVisibility.PageAction.Remove));
        }

        [Test]
        public void ComputePageAction_VisibleAndPresent_ReturnsNone()
        {
            Assert.That(InterfaceVisibility.ComputePageAction(true, true),
                Is.EqualTo(InterfaceVisibility.PageAction.None));
        }

        [Test]
        public void ComputePageAction_HiddenAndAbsent_ReturnsNone()
        {
            Assert.That(InterfaceVisibility.ComputePageAction(false, false),
                Is.EqualTo(InterfaceVisibility.PageAction.None));
        }

        // ---- ResolveInitialVisibility ----

        [TestCase("True",  true,  ExpectedResult = true)]
        [TestCase("False", true,  ExpectedResult = false)]
        [TestCase("True",  false, ExpectedResult = true)]
        [TestCase("False", false, ExpectedResult = false)]
        public bool ResolveInitialVisibility_NormalValue_Parses(string v, bool def)
            => InterfaceVisibility.ResolveInitialVisibility(v, def);

        [TestCase(true,  ExpectedResult = true)]  // 默认可见接口，INI 缺失 → 显示
        [TestCase(false, ExpectedResult = false)] // 默认隐藏接口，INI 缺失 → 隐藏
        public bool ResolveInitialVisibility_MissingKey_ReturnsDefault(bool def)
            => InterfaceVisibility.ResolveInitialVisibility("发生错误", def);

        [TestCase(true,  ExpectedResult = true)]  // 畸形值兜底默认
        [TestCase(false, ExpectedResult = false)]
        public bool ResolveInitialVisibility_MalformedValue_ReturnsDefault(bool def)
            => InterfaceVisibility.ResolveInitialVisibility("garbage", def);
    }
}
