using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace TrOCR.Tests
{
    /// <summary>
    /// Newtonsoft.Json NuGet 包兼容性测试：验证从本地 DLL 迁移后
    /// 业务中实际使用的 JSON 反序列化场景均正常工作。
    /// </summary>
    [TestFixture]
    public class JsonCompatibilityTests
    {
        /// <summary>
        /// 验证自定义 OCR 提供商列表的 JSON 反序列化——该格式由设置文件和用户配置使用。
        /// </summary>
        [Test]
        public void Newtonsoft_CanParseCustomProviderList()
        {
            const string json = @"[
  {
    ""Name"": ""OpenAI Compatible"",
    ""Url"": ""https://example.invalid/v1/chat/completions"",
    ""Key"": ""local-test-key"",
    ""Model"": ""vision-model""
  }
]";

            var providers = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            Assert.That(providers, Has.Count.EqualTo(1));
            Assert.That(providers[0]["Name"], Is.EqualTo("OpenAI Compatible"));
            Assert.That(providers[0]["Model"], Is.EqualTo("vision-model"));
        }

        /// <summary>
        /// 验证 RapidOCR 高级配置 JSON 的 JObject 解析——含浮点数、布尔值等混合类型字段。
        /// </summary>
        [Test]
        public void Newtonsoft_CanParseRapidOcrAdvancedConfig()
        {
            const string json = @"{
  ""RapidOCR"": {
    ""padding"": 50,
    ""imgResize"": 1024,
    ""boxScoreThresh"": 0.5,
    ""boxThresh"": 0.3,
    ""unClipRatio"": 1.6,
    ""doAngle"": true,
    ""mostAngle"": true,
    ""numThreads"": 4
  }
}";

            var obj = JObject.Parse(json);
            var rapid = obj["RapidOCR"];

            Assert.That(rapid["padding"].Value<int>(), Is.EqualTo(50));
            Assert.That(rapid["doAngle"].Value<bool>(), Is.True);
            Assert.That(rapid["boxScoreThresh"].Value<float>(), Is.EqualTo(0.5f).Within(0.0001f));
        }

        /// <summary>
        /// 验证 Newtonsoft.Json 主版本号为 13，防止意外降级破坏序列化兼容性。
        /// </summary>
        [Test]
        public void Newtonsoft_AssemblyVersionRemains13()
        {
            var assemblyName = typeof(JsonConvert).Assembly.GetName();

            Assert.That(assemblyName.Name, Is.EqualTo("Newtonsoft.Json"));
            Assert.That(assemblyName.Version.Major, Is.EqualTo(13));
        }
    }
}
