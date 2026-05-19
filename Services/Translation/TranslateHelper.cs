using GTranslate.Translators;
using System.Threading.Tasks;

namespace TrOCR.Helper
{
    public class GTranslateHelper
    {
        private static readonly GoogleTranslator _googleTranslator = new GoogleTranslator();
        private static readonly MicrosoftTranslator _microsoftTranslator = new MicrosoftTranslator();
        private static readonly YandexTranslator _yandexTranslator = new YandexTranslator();

        public static async Task<string> TranslateAsync(string text, string fromLanguage, string toLanguage, string service)
        {
            try
            {
                // 处理无需密钥的翻译服务
                switch (service.ToLower())
                {
                    case "bing":
                        return await BingTranslator.TranslateAsync(text, fromLanguage, toLanguage);
                    
                    case "bing2":
                    case "bingnew":
                        return await BingTranslator2.TranslateAsync(text, fromLanguage, toLanguage);
                    
                    case "tencent2":
                    case "tencentnew":
                    case "腾讯交互":
                        return await TencentTranslator.TranslateAsync(text, fromLanguage, toLanguage);
                    
                    case "caiyun":
                    case "彩云":
                    case "彩云小译":
                        return await CaiyunTranslator.TranslateAsync(text, fromLanguage, toLanguage);
                    
                    case "volcano":
                    case "volc":
                    case "火山":
                        return await VolcanoTranslator.TranslateAsync(text, fromLanguage, toLanguage);
                }

                // 处理 GTranslate 库支持的翻译服务
                ITranslator translator;
                switch (service.ToLower())
                {
                    case "google":
                        translator = _googleTranslator;
                        break;
                    case "microsoft":
                        translator = _microsoftTranslator;
                        break;
                    case "yandex":
                        translator = _yandexTranslator;
                        break;
                    default:
                        // Fallback to Google by default
                        translator = _googleTranslator;
                        break;
                }

                var result = await translator.TranslateAsync(text, toLanguage, fromLanguage.Equals("auto", System.StringComparison.OrdinalIgnoreCase) ? null : fromLanguage);
                return result.Translation;
            }
            catch (System.Exception e)
            {
                return $"Translation failed: {e.Message}";
            }
        }
    }
}