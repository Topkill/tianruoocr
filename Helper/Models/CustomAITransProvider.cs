using Newtonsoft.Json;
using System;

namespace TrOCR.Helper.Models 
{
    public class CustomAITransProvider : CustomAIProvider
    {
        public CustomAITransProvider()
        {
            // 在这里将默认路径改为翻译的配置文件
            this.ModelConfigPath = @"Data\modes\TranslateModes.json";
        }

        //源语言
        // 添加 Order 属性将它们移到最后 
        [JsonProperty(Order = 100)]
        public string Source { get; set; } = "auto detect";
        //目标语言
        // 添加 Order 属性将它们移到最后 
        [JsonProperty(Order = 101)]
        public string Target { get; set; } = "自动判断";

      
    }
}