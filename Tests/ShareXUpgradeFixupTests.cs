using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TrOCR.Tests
{
    /// <summary>
    /// ShareX 源码集成验证测试：确保适配层隔离、子模块路径正确、
    /// 卫星资源不泄漏、以及悬停截图逻辑已正确注入。
    /// </summary>
    [TestFixture]
    public class ShareXUpgradeFixupTests
    {
        /// <summary>
        /// 验证 Forms 层源码中不直接引用 ShareX.ScreenCaptureLib 的任何公共类型，
        /// 保证业务代码完全通过 IScreenCaptureService 适配层访问截图功能。
        /// </summary>
        [Test]
        public void Forms_DoNotReferenceShareXTypesDirectly()
        {
            var root = FindRepositoryRoot();
            var formsDirectory = Path.Combine(root, "Forms");
            var forbiddenTokens = new[]
            {
                "ShareX.ScreenCaptureLib",
                "RegionCaptureOptions",
                "RegionCaptureForm",
                "RegionCaptureMode",
                "RegionCaptureTasks"
            };

            var violations = Directory
                .EnumerateFiles(formsDirectory, "*.cs", SearchOption.AllDirectories)
                .SelectMany(file => File.ReadLines(file)
                    .Select((line, index) => new { file, line, lineNumber = index + 1 }))
                .Where(item => forbiddenTokens.Any(token => item.line.Contains(token)))
                .Select(item => item.file + ":" + item.lineNumber + ": " + item.line.Trim())
                .ToArray();

            Assert.That(
                violations,
                Is.Empty,
                "Forms code must depend on TrOCR.Services.ScreenCapture instead of ShareX types:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, violations));
        }

        /// <summary>
        /// 验证 ShareX 子模块 ProjectReference 路径为 external\ShareX\，
        /// 确保从旧路径 references\ShareX\ 迁移后不会回退。
        /// </summary>
        [Test]
        public void ShareXProjectReferences_UseExternalPath()
        {
            var project = File.ReadAllText(Path.Combine(FindRepositoryRoot(), "TrOCR.csproj"));

            Assert.That(project, Does.Contain(@"external\ShareX\ShareX.HelpersLib\ShareX.HelpersLib.csproj"));
            Assert.That(project, Does.Contain(@"external\ShareX\ShareX.ScreenCaptureLib\ShareX.ScreenCaptureLib.csproj"));
            Assert.That(project, Does.Not.Contain(@"references\ShareX\"));
        }

        /// <summary>
        /// 验证 x64 Release 构建产物中只保留 zh-CN 的 ShareX satellite resource assemblies。
        /// </summary>
        [Test]
        public void ReleaseOutput_KeepsOnlyZhCnShareXSatelliteResourceAssemblies()
        {
            var root = FindRepositoryRoot();
            var releaseDirectory = Path.Combine(root, "bin", "x64", "Release");
            if (!Directory.Exists(releaseDirectory))
            {
                Assert.Inconclusive("Run x64 Release build before this test.");
                return;
            }

            var shareXResourceAssemblies = Directory
                .EnumerateFiles(releaseDirectory, "ShareX.*.resources.dll", SearchOption.AllDirectories)
                .Select(file => ToRepositoryRelativePath(releaseDirectory, file))
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            var nonZhCnAssemblies = shareXResourceAssemblies
                .Where(path => !path.StartsWith("lib/zh-CN/", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            Assert.That(nonZhCnAssemblies, Is.Empty,
                "Only Simplified Chinese ShareX satellite resources should be shipped:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, nonZhCnAssemblies));

            var expectedZhCnAssemblies = new[]
            {
                "lib/zh-CN/ShareX.HelpersLib.resources.dll",
                "lib/zh-CN/ShareX.ImageEffectsLib.resources.dll",
                "lib/zh-CN/ShareX.MediaLib.resources.dll",
                "lib/zh-CN/ShareX.ScreenCaptureLib.resources.dll"
            };

            foreach (var expected in expectedZhCnAssemblies)
            {
                Assert.That(shareXResourceAssemblies, Does.Contain(expected));
            }
        }

        /// <summary>
        /// 验证 ShareX fork 中天若快捷键映射使用了悬停感知的 CloseTianruoRegionWindow()，
        /// 确保用户未拖选矩形时按 Space/A/Q 能正确捕获当前悬停区域而非返回 null。
        /// </summary>
        [Test]
        public void ShareXFork_TianruoRegionShortcutsUseHoverAwareClose()
        {
            var root = FindRepositoryRoot();
            var regionCaptureFormPath = Path.Combine(root, "external", "ShareX", "ShareX.ScreenCaptureLib", "Forms", "RegionCaptureForm.cs");
            if (!File.Exists(regionCaptureFormPath))
            {
                Assert.Inconclusive("ShareX submodule not initialized at external/ShareX.");
                return;
            }

            var source = File.ReadAllText(regionCaptureFormPath);

            Assert.That(source, Does.Contain("private void CloseTianruoRegionWindow()"));
            Assert.That(source, Does.Contain("ShapeManager.CurrentHoverShape.AddShapePath(regionFillPath)"));
        }

        /// <summary>
        /// 验证 GetRegionCaptureOptions 克隆时保留 InputDelay 字段。
        /// </summary>
        [Test]
        public void ShareXFork_RegionCaptureOptionsClonePreservesInputDelay()
        {
            var source = ReadRepositoryText("external", "ShareX", "ShareX.ScreenCaptureLib", "RegionCaptureTasks.cs");
            var methodBody = ExtractMethodBody(source, "private static RegionCaptureOptions GetRegionCaptureOptions");

            Assert.That(methodBody, Does.Contain("InputDelay = options.InputDelay"));
        }

        /// <summary>
        /// 验证天若快捷键统一绕过 ShareX 启动输入延迟。
        /// </summary>
        [Test]
        public void ShareXFork_TianruoRegionShortcutsBypassStartupInputDelay()
        {
            var source = ReadRepositoryText("external", "ShareX", "ShareX.ScreenCaptureLib", "Forms", "RegionCaptureForm.cs");
            var keyDownBody = ExtractMethodBody(source, "internal void RegionCaptureForm_KeyDown");
            var shortcutBody = ExtractMethodBody(source, "private static bool IsTianruoShortcut");

            Assert.That(keyDownBody, Does.Contain("bool isTianruoShortcut = imageGet && IsTianruoShortcut(e.KeyData);"));
            Assert.That(keyDownBody, Does.Contain("!isTianruoShortcut && timerStart.ElapsedMilliseconds < Options.InputDelay"));

            var expectedKeys = new[]
            {
                "Keys.Tab",
                "Keys.Space",
                "Keys.A",
                "Keys.S",
                "Keys.Q",
                "Keys.C",
                "Keys.B",
                "Keys.E",
                "Keys.D1",
                "Keys.D2"
            };

            foreach (var expectedKey in expectedKeys)
            {
                Assert.That(shortcutBody, Does.Contain(expectedKey));
            }
        }

        /// <summary>
        /// 验证 TryHandleTianruoShortcut 覆盖所有截图模式且不排除多选状态。
        /// </summary>
        [Test]
        public void ShareXFork_TianruoShortcutHandlerCoversAllScreenshotModes()
        {
            var source = ReadRepositoryText("external", "ShareX", "ShareX.ScreenCaptureLib", "Forms", "RegionCaptureForm.cs");
            var handlerBody = ExtractMethodBody(source, "private bool TryHandleTianruoShortcut");

            Assert.That(handlerBody, Does.Not.Contain("modeFlag != \"区域多选\""));

            var expectedTokens = new[]
            {
                "case Keys.Tab:",
                "case Keys.Space:",
                "case Keys.A:",
                "case Keys.S:",
                "case Keys.Q:",
                "case Keys.C:",
                "case Keys.B:",
                "case Keys.E:",
                "case Keys.D1:",
                "case Keys.D2:",
                "\"区域多选\"",
                "\"截图\"",
                "\"自动保存\"",
                "\"多区域自动保存\"",
                "\"保存\"",
                "\"贴图\"",
                "\"取色\"",
                "\"百度\"",
                "\"高级截图\"",
                "\"拆分\"",
                "\"合并\""
            };

            foreach (var expectedToken in expectedTokens)
            {
                Assert.That(handlerBody, Does.Contain(expectedToken));
            }
        }

        /// <summary>
        /// 验证 Program.cs 在 WinForms 初始化之前设置 zh-CN UI Culture。
        /// </summary>
        [Test]
        public void Program_ConfiguresZhCnUiCultureBeforeWinFormsInitialization()
        {
            var source = ReadRepositoryText("Program.cs");

            var callIndex = source.IndexOf("ConfigureDefaultUiCulture();", StringComparison.Ordinal);
            var winFormsIndex = source.IndexOf("Application.EnableVisualStyles();", StringComparison.Ordinal);

            Assert.That(callIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(winFormsIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(callIndex, Is.LessThan(winFormsIndex));

            var methodBody = ExtractMethodBody(source, "private static void ConfigureDefaultUiCulture");
            Assert.That(methodBody, Does.Contain("CultureInfo.GetCultureInfo(\"zh-CN\")"));
            Assert.That(methodBody, Does.Contain("Thread.CurrentThread.CurrentUICulture = zhCnCulture;"));
            Assert.That(methodBody, Does.Contain("CultureInfo.DefaultThreadCurrentUICulture = zhCnCulture;"));
        }

        /// <summary>
        /// 验证高级截图（Annotation）窗口的 Space 窄拦截调用位于天若门控
        /// （isTianruoShortcut 判据行）之后、上游通用 switch (e.KeyData) 之前。
        /// </summary>
        [Test]
        public void RegionCaptureFormKeyDown_AnnotationSpaceIntercept_OrderedAfterTianruoGateBeforeUpstreamSwitch()
        {
            var source = ReadRegionCaptureFormSource();
            var keyDownBody = ExtractMethodBody(source, "internal void RegionCaptureForm_KeyDown");

            var gateIndex = keyDownBody.IndexOf("bool isTianruoShortcut = imageGet && IsTianruoShortcut(e.KeyData);", StringComparison.Ordinal);
            var interceptIndex = keyDownBody.IndexOf("HandleAnnotationSpace();", StringComparison.Ordinal);
            var upstreamSwitchIndex = keyDownBody.IndexOf("switch (e.KeyData)", StringComparison.Ordinal);

            Assert.That(gateIndex, Is.GreaterThanOrEqualTo(0), "天若门控判据行应存在于 KeyDown 中。");
            Assert.That(upstreamSwitchIndex, Is.GreaterThanOrEqualTo(0), "上游通用 switch 应存在于 KeyDown 中。");
            Assert.That(interceptIndex, Is.GreaterThanOrEqualTo(0), "缺少标注窗口 Space 拦截调用 HandleAnnotationSpace()。");
            Assert.That(interceptIndex, Is.GreaterThan(gateIndex), "Space 拦截应位于天若门控之后。");
            Assert.That(interceptIndex, Is.LessThan(upstreamSwitchIndex), "Space 拦截应位于上游通用 switch 之前。");
        }

        /// <summary>
        /// 验证标注窗口 Space 拦截调用以 IsAnnotationMode 为生效判据且携带 Keys.Space 判据，
        /// 不使用 IsEditorMode（Annotation 模式下恒为 false 的术语盲区）。
        /// </summary>
        [Test]
        public void RegionCaptureFormKeyDown_AnnotationSpaceIntercept_GatedByIsAnnotationModeNotIsEditorMode()
        {
            var source = ReadRegionCaptureFormSource();
            var interceptLine = ExtractLineContaining(source, "HandleAnnotationSpace();");

            Assert.That(interceptLine, Does.Contain("IsAnnotationMode"), "拦截调用应以 IsAnnotationMode 为生效判据。");
            Assert.That(interceptLine, Does.Contain("Keys.Space"), "拦截调用应针对 Space 键。");
            Assert.That(interceptLine, Does.Not.Contain("IsEditorMode"), "不得以 IsEditorMode 作为判据（Annotation 下恒为 false）。");
        }

        /// <summary>
        /// 验证 HandleAnnotationSpace 的形状无效/非框选工具路径为吞键 no-op：
        /// 方法体内只允许有效路径唯一一次 CloseWindow(RegionResult.Region)，
        /// 无效路径不产生任何关窗调用（绝不回退全屏行为）。
        /// </summary>
        [Test]
        public void HandleAnnotationSpace_InvalidShapePath_DoesNotCallCloseWindow()
        {
            var source = ReadRegionCaptureFormSource();
            var methodBody = ExtractMethodBody(source, "private void HandleAnnotationSpace");

            Assert.That(methodBody, Does.Contain("CloseWindow(RegionResult.Region)"));
            Assert.That(
                Regex.Matches(methodBody, "CloseWindow").Count,
                Is.EqualTo(1),
                "形状无效路径不得产生任何 CloseWindow 调用（方法体内只允许有效路径的一次 Region 关窗）。");
        }

        /// <summary>
        /// 围栏契约：上游通用 switch 中 Space→CloseWindow(RegionResult.Fullscreen) 的 case 原样保留，
        /// 且标注窗口拦截位于其之前，使上游全屏行为对 Annotation 窗口不可达（普通截图仍由天若层先行拦截）。
        /// </summary>
        [Test]
        public void RegionCaptureFormKeyDown_UpstreamSpaceFullscreenCase_PreservedBehindAnnotationIntercept()
        {
            var source = ReadRegionCaptureFormSource();
            var keyDownBody = ExtractMethodBody(source, "internal void RegionCaptureForm_KeyDown");

            var spaceCaseIndex = keyDownBody.IndexOf("case Keys.Space:", StringComparison.Ordinal);
            Assert.That(spaceCaseIndex, Is.GreaterThanOrEqualTo(0), "上游 switch 中的 case Keys.Space: 应原样保留。");

            var fullscreenCallIndex = keyDownBody.IndexOf("CloseWindow(RegionResult.Fullscreen)", spaceCaseIndex, StringComparison.Ordinal);
            Assert.That(fullscreenCallIndex, Is.GreaterThan(spaceCaseIndex), "case Keys.Space: 分支应保留 CloseWindow(RegionResult.Fullscreen)。");

            var interceptIndex = keyDownBody.IndexOf("HandleAnnotationSpace();", StringComparison.Ordinal);
            Assert.That(interceptIndex, Is.GreaterThanOrEqualTo(0), "缺少标注窗口 Space 拦截调用 HandleAnnotationSpace()。");
            Assert.That(interceptIndex, Is.LessThan(spaceCaseIndex), "标注窗口拦截应先于上游 Space case，使全屏行为对 Annotation 窗口不可达。");
        }

        private static string ExtractLineContaining(string source, string token)
        {
            var line = source
                .Split('\n')
                .FirstOrDefault(candidate => candidate.Contains(token));

            if (line == null)
                Assert.Fail("Line containing token not found: " + token);

            return line;
        }

        private static string ReadRepositoryText(params string[] relativeParts)
        {
            var parts = new string[relativeParts.Length + 1];
            parts[0] = FindRepositoryRoot();
            Array.Copy(relativeParts, 0, parts, 1, relativeParts.Length);
            return File.ReadAllText(Path.Combine(parts));
        }

        private static string ReadRegionCaptureFormSource()
        {
            return ReadRepositoryText("external", "ShareX", "ShareX.ScreenCaptureLib", "Forms", "RegionCaptureForm.cs");
        }

        private static string ToRepositoryRelativePath(string root, string path)
        {
            var relative = path.Substring(root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length)
                .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return relative.Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
        }

        private static string ExtractMethodBody(string source, string signature)
        {
            var signatureIndex = source.IndexOf(signature, StringComparison.Ordinal);
            if (signatureIndex < 0)
                Assert.Fail("Method signature not found: " + signature);

            var bodyStart = source.IndexOf('{', signatureIndex);
            if (bodyStart < 0)
                Assert.Fail("Method body not found: " + signature);

            var depth = 0;
            for (var i = bodyStart; i < source.Length; i++)
            {
                if (source[i] == '{')
                {
                    depth++;
                }
                else if (source[i] == '}')
                {
                    depth--;
                    if (depth == 0)
                    {
                        return source.Substring(bodyStart, i - bodyStart + 1);
                    }
                }
            }

            Assert.Fail("Method body is not balanced: " + signature);
            return string.Empty;
        }

        private static string FindRepositoryRoot()
        {
            var directory = TestContext.CurrentContext.TestDirectory;
            while (!string.IsNullOrEmpty(directory))
            {
                if (File.Exists(Path.Combine(directory, "TrOCR.sln")))
                    return directory;
                directory = Directory.GetParent(directory)?.FullName;
            }
            throw new DirectoryNotFoundException("无法从测试输出目录定位仓库根目录: " + TestContext.CurrentContext.TestDirectory);
        }
    }
}
