using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace TrOCR.Tests
{
    /// <summary>
    /// 依赖边界守卫测试：确保 DLL 迁移为 NuGet 包引用后不出现回退，
    /// 且 Forms 层不直接调用 ShareX 私有 API。
    /// </summary>
    [TestFixture]
    public class DependencyBoundaryTests
    {
        /// <summary>
        /// 验证 csproj 不包含指向 DLL\ 目录的旧式 HintPath 引用，
        /// 防止误将已迁移的包退回为本地 DLL 引用。
        /// </summary>
        [Test]
        public void Project_DoesNotReferenceLegacyNewtonsoftOrEmguWorld()
        {
            var project = File.ReadAllText(Path.Combine(FindRepositoryRoot(), "TrOCR.csproj"));

            Assert.That(project, Does.Not.Contain(@"DLL\Newtonsoft\Newtonsoft.Json.dll"));
            Assert.That(project, Does.Not.Contain(@"DLL\Emgu\CV\Emgu.CV.World.dll"));
        }

        /// <summary>
        /// 确保主项目目标框架保持 .NET Framework 4.8.1，
        /// 不被意外升级为 .NET 9 等新框架（会破坏运行时兼容性）。
        /// </summary>
        [Test]
        public void Project_StillTargetsNetFramework481()
        {
            var project = File.ReadAllText(Path.Combine(FindRepositoryRoot(), "TrOCR.csproj"));

            Assert.That(project, Does.Contain("<TargetFrameworkVersion>v4.8.1</TargetFrameworkVersion>"));
            Assert.That(project, Does.Not.Contain("<TargetFramework>net9.0-windows</TargetFramework>"));
        }

        /// <summary>
        /// 验证 Forms 层代码不直接调用 ShareX 的 RegionCaptureTasks 私有 API，
        /// 必须通过 IScreenCaptureService 适配层调用。
        /// </summary>
        [Test]
        public void Forms_DoNotCallShareXRegionCaptureTasksDirectly()
        {
            var root = FindRepositoryRoot();
            var formsDirectory = Path.Combine(root, "Forms");

            var violations = Directory
                .EnumerateFiles(formsDirectory, "*.cs", SearchOption.AllDirectories)
                .SelectMany(file => File.ReadLines(file)
                    .Select((line, index) => new { file, line, lineNumber = index + 1 }))
                .Where(item => item.line.Contains("RegionCaptureTasks.GetRegionImage_Mo"))
                .Select(item => item.file + ":" + item.lineNumber + ": " + item.line.Trim())
                .ToArray();

            Assert.That(
                violations,
                Is.Empty,
                "Forms code should use IScreenCaptureService instead of ShareX private API:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, violations));
        }

        private static string FindRepositoryRoot()
        {
            var directory = TestContext.CurrentContext.TestDirectory;
            while (!string.IsNullOrEmpty(directory))
            {
                if (File.Exists(Path.Combine(directory, "TrOCR.sln")))
                {
                    return directory;
                }

                directory = Directory.GetParent(directory)?.FullName;
            }

            throw new DirectoryNotFoundException("无法从测试输出目录定位仓库根目录: " + TestContext.CurrentContext.TestDirectory);
        }
    }
}
