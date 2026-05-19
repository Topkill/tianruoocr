using System;

namespace TrOCR.Helper.Models 
{
    public class CustomAIProvider
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // 接口名称 (ListBox显示用)
        public string Name { get; set; } = "新建接口";

        // API 地址
        public string ApiUrl { get; set; } = "";

        // API 密钥
        public string ApiKey { get; set; } = "";

        // 模型名称 (API调用时传给厂商的参数，如 gpt-4)
        public string ModelName { get; set; } = "";

        // 核心：指向你的那个 JSON 配置文件的路径
        // 如果留空，可以使用程序内置的默认逻辑；如果填了，就加载该文件生成菜单。
        public string ModelConfigPath { get; set; } = @"Data\modes\OCRModes.json";

        public override string ToString()
        {
            return Name;
        }
    }
}