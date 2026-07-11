using NUnit.Framework;
using TrOCR.Services.ScreenCapture;

namespace TrOCR.Tests
{
    /// <summary>
    /// ScreenCaptureRequest 值对象测试：验证工厂方法产生正确的默认参数。
    /// </summary>
    [TestFixture]
    public class ScreenCaptureRequestTests
    {
        /// <summary>
        /// 验证 ForOcr() 工厂方法返回的放大镜设置与业务需求一致：
        /// 关闭放大镜显示、使用圆形放大镜、15像素采样、10倍放大。
        /// </summary>
        [Test]
        public void ForOcr_ReturnsExpectedMagnifierSettings()
        {
            var request = ScreenCaptureRequest.ForOcr();

            Assert.That(request.ShowMagnifier, Is.False);
            Assert.That(request.UseSquareMagnifier, Is.False);
            Assert.That(request.MagnifierPixelCount, Is.EqualTo(15));
            Assert.That(request.MagnifierPixelSize, Is.EqualTo(10));
        }
    }
}
