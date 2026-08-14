using NUnit.Framework;
using TrOCR.Helper;

namespace TrOCR.Tests
{
    /// <summary>
    /// BingTranslator2（bing2 免费翻译通道）纯函数单测。
    /// 背景：bing2 已从旧 Edge auth 端点迁移到免鉴权 translatetext 端点。
    /// 本测试只覆盖迁移抽出的两个纯函数（BuildRequestBody/ParseResponse），
    /// 不含真实 HTTP smoke（后续统一规划免费 API 可用性测试时再加）。
    /// </summary>
    [TestFixture]
    public class BingTranslator2Tests
    {
        /// <summary>
        /// 普通文本应序列化为单元素字符串数组 JSON。
        /// translatetext 端点要求 body 为 ["text"]，旧的对象数组 [{"Text":""}] 已被拒绝(400)。
        /// </summary>
        [Test]
        public void BuildRequestBody_PlainText_ReturnsStringArrayJson()
        {
            Assert.That(BingTranslator2.BuildRequestBody("hello"), Is.EqualTo("[\"hello\"]"));
        }

        /// <summary>
        /// 含双引号/反斜杠/中文/换行的文本必须经 JSON 正确转义，
        /// 且反序列化回来仍为单元素数组——同时验证形状与转义。
        /// </summary>
        [Test]
        public void BuildRequestBody_SpecialCharacters_ProperlyEscaped()
        {
            var json = BingTranslator2.BuildRequestBody("他说\"你好\"\n");
            var roundTripped = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(json);
            Assert.That(roundTripped, Is.EqualTo(new[] { "他说\"你好\"\n" }));
        }

        /// <summary>
        /// translatetext 成功响应(probe 实测样本)外层是数组：
        /// [{"translations":[{"text":"...","to":"..."}]}]，解析 [0].translations[0].text 得译文。
        /// </summary>
        [Test]
        public void ParseResponse_ValidSample_ReturnsTranslatedText()
        {
            var json = "[{\"translations\":[{\"text\":\"你好\",\"to\":\"zh-Hans\"}]}]";
            Assert.That(BingTranslator2.ParseResponse(json), Is.EqualTo("你好"));
        }

        /// <summary>
        /// translations 为空数组时返回空串，不崩溃。守护原 Count>0 判断。
        /// </summary>
        [Test]
        public void ParseResponse_EmptyTranslations_ReturnsEmpty()
        {
            var json = "[{\"translations\":[]}]";
            Assert.That(BingTranslator2.ParseResponse(json), Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// 缺 translations 字段时返回空串，不崩溃。守护原 null 判断。
        /// </summary>
        [Test]
        public void ParseResponse_MissingTranslations_ReturnsEmpty()
        {
            var json = "[{}]";
            Assert.That(BingTranslator2.ParseResponse(json), Is.EqualTo(string.Empty));
        }
    }
}
