using System.Drawing;
using NUnit.Framework;
using TrOCR.Services.ScreenCapture;

namespace TrOCR.Tests
{
    /// <summary>
    /// ScreenCaptureResult 值对象测试：验证构造函数的空值归一化与字段保持行为。
    /// </summary>
    [TestFixture]
    public class ScreenCaptureResultTests
    {
        /// <summary>
        /// 当 ModeFlag 为 null 时构造函数应将其归一化为空字符串，避免下游 NullReferenceException。
        /// </summary>
        [Test]
        public void Constructor_NormalizesNullModeFlag()
        {
            var result = new ScreenCaptureResult(null, null, Point.Empty, null);

            Assert.That(result.ModeFlag, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// 当 rectangles 参数为 null 时应归一化为空数组，保证 SelectedRectangles 永不为 null。
        /// </summary>
        [Test]
        public void Constructor_NormalizesNullRectangles()
        {
            var result = new ScreenCaptureResult(null, "截图", Point.Empty, null);

            Assert.That(result.SelectedRectangles, Is.Not.Null);
            Assert.That(result.SelectedRectangles, Is.Empty);
        }

        /// <summary>
        /// 验证构造函数正确保持传入的 Image 引用、坐标点和矩形数组，无副本/无转换。
        /// </summary>
        [Test]
        public void Constructor_PreservesImageAndPointAndRectangles()
        {
            using (var image = new Bitmap(10, 10))
            {
                var point = new Point(3, 4);
                var rectangles = new[] { new Rectangle(1, 2, 3, 4) };

                var result = new ScreenCaptureResult(image, "区域多选", point, rectangles);

                Assert.That(result.Image, Is.SameAs(image));
                Assert.That(result.ModeFlag, Is.EqualTo("区域多选"));
                Assert.That(result.FlagLocation, Is.EqualTo(point));
                Assert.That(result.SelectedRectangles, Is.EqualTo(rectangles));
            }
        }
    }
}
