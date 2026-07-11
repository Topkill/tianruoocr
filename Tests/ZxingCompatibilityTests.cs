using System.Drawing;
using NUnit.Framework;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace TrOCR.Tests
{
    /// <summary>
    /// ZXing NuGet 包兼容性测试：验证从本地 DLL 迁移为 NuGet 包引用后
    /// 二维码编解码功能和程序集版本均正常。
    /// </summary>
    [TestFixture]
    public class ZxingCompatibilityTests
    {
        /// <summary>
        /// 端到端验证：生成 QR 码并立即解码，确认 NuGet 版本的编解码路径完整可用。
        /// </summary>
        [Test]
        public void QrCode_CanRoundTripUsingCurrentReaderPath()
        {
            const string expected = "https://github.com/tianruoocr/test";

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 180,
                    Height = 180,
                    Margin = 1
                }
            };

            using (var bitmap = writer.Write(expected))
            {
                var binaryBitmap = new BinaryBitmap(new HybridBinarizer(new BitmapLuminanceSource(bitmap)));
                var result = new QRCodeReader().decode(binaryBitmap);

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Text, Is.EqualTo(expected));
            }
        }

        /// <summary>
        /// 验证 ZXing 程序集版本为 0.16.11+，防止意外降级到旧版本。
        /// </summary>
        [Test]
        public void Zxing_AssemblyVersionIsModern()
        {
            var assemblyName = typeof(QRCodeReader).Assembly.GetName();

            Assert.That(assemblyName.Name, Is.EqualTo("zxing"));
            Assert.That(assemblyName.Version.Major, Is.EqualTo(0));
            Assert.That(assemblyName.Version.Minor, Is.EqualTo(16));
            Assert.That(assemblyName.Version.Build, Is.GreaterThanOrEqualTo(11));
        }
    }
}
