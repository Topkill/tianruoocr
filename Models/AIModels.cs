using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrOCR.Helper.Models
{  // 实体类定义
    //ai接口的模式文件json
    public class AIConfig
    {
        public string type { get; set; }
        public List<AIMode> modes { get; set; }
    }
    //ai接口的模式
    // 1. 给类加上这个特性，指定使用我们下面写的转换器
    [JsonConverter(typeof(AIModeConverter))]
    public class AIMode
    {
        public string mode { get; set; }
        public string? description { get; set; }
        public string? system_prompt { get; set; }
        //user_prompt
        public string? prompt { get; set; }
        public string? assistant_prompt { get; set; }
        // 改为可空类型 (double?)，如果 json 里没填，值为 null
        public double? temperature { get; set; }

        // 改为可空类型 (bool?)，如果 json 里没填，值为 null
        public bool? enable_thinking { get; set; }
        public bool? stream { get; set; }
        // 2. 新增一个列表，用来存储 "system_prompt", "prompt", "assistant_prompt" 的出现顺序
        // 使用 Ignore 避免再次序列化它自己
        [JsonIgnore]
        public List<string> PromptOrder { get; set; } = new List<string>();

        /// <summary>
        /// 确保 PromptOrder 不为空。如果为空，则根据当前属性值自动填充默认顺序。
        /// </summary>
        public void EnsureDefaultOrder()
        {
            if (PromptOrder == null) PromptOrder = new List<string>();

            if (PromptOrder.Count == 0)
            {
                // 按照标准顺序填充：System -> Assistant -> User
                if (!string.IsNullOrEmpty(system_prompt)) PromptOrder.Add("system_prompt");
                if (!string.IsNullOrEmpty(assistant_prompt)) PromptOrder.Add("assistant_prompt");

                // User Prompt 总是要有的，哪怕属性为空，通常也代表 User 消息的位置
                //PromptOrder.Add("prompt");
                //或者改为：只有当配置文件里真的写了 prompt 模板时，才加进列表
                // 如果没写，稍后 OCR/Translate 方法里的“最终兜底”会负责补发图片/原文
                if (!string.IsNullOrEmpty(prompt)) PromptOrder.Add("prompt");
            }
        }
    }
    // 3.  核心：自定义转换器 
    public class AIModeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AIMode);
        }

        // 反序列化（读 JSON）
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // 1. 加载整个 JSON 对象 (JObject 保留了文件里的顺序)
            JObject obj = JObject.Load(reader);

            // 2. 创建 AIMode 实例
            var mode = new AIMode();

            // 3. 填充标准属性 (利用 serializer 自动填充，省去手动赋值的麻烦)
            using (var subReader = obj.CreateReader())
            {
                serializer.Populate(subReader, mode);
            }

            // 4.  关键步骤：遍历 JObject 的属性顺序，记录提示词的顺序 
            mode.PromptOrder = new List<string>();
            foreach (var prop in obj.Properties())
            {
                string name = prop.Name;
                // 只记录这三个我们关心的提示词字段
                // if (name == "system_prompt" || name == "prompt" || name == "assistant_prompt")
                // {
                //     mode.PromptOrder.Add(name);
                // }
                //  修改后：忽略大小写匹配 (推荐) 
                // 使用 StringComparison.OrdinalIgnoreCase
                if (string.Equals(name, "system_prompt", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, "prompt", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, "assistant_prompt", StringComparison.OrdinalIgnoreCase))
                {
                    // 为了统一后续处理，建议添加时存入统一的小写 key，或者你代码里处理大写 key 也行
                    // 这里建议转为小写存入 list，这样 Translate 方法里的 switch/if 就不用改了
                    mode.PromptOrder.Add(name.ToLower());
                }
            }

            // 兜底逻辑：如果列表为空（可能是手动 new 出来的对象，可能是程序内置默认模式，可能文件不含提示词），给一个默认顺序
            if (mode.PromptOrder.Count == 0)
            {
                // 默认：System -> Assistant -> User (或者你喜欢的默认顺序)
                if (!string.IsNullOrEmpty(mode.system_prompt)) mode.PromptOrder.Add("system_prompt");
                if (!string.IsNullOrEmpty(mode.assistant_prompt)) mode.PromptOrder.Add("assistant_prompt");
                if (!string.IsNullOrEmpty(mode.prompt)) mode.PromptOrder.Add("prompt");
            }

            return mode;
        }

        // 序列化（写 JSON）：保持默认行为即可，或者你也想按顺序写出
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 如果不需要特殊写出逻辑，这里可以手动写，或者抛出异常让外层用默认处理
            // 为简单起见，这里我们禁用 Write 转换，让 Newtonsoft 使用默认逻辑
            throw new NotImplementedException();
        }

        // 告诉 Newtonsoft，写的时候不用这个转换器，只在读的时候用
        public override bool CanWrite => false;
    }
}
