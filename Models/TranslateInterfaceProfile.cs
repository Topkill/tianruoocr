using System;
using System.Collections.Generic;
using TrOCR.Helper;

namespace TrOCR.Models
{
    // 接口类型枚举
    public enum TranslateInterfaceType
    {
        Baidu,
        Tencent,
        OpenAI,
        Google
    }

    // 【父类】大家都有的属性：ID、名字、类型
    public abstract class TranslateBaseProfile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        // 强制子类必须声明自己是什么类型
        public abstract TranslateInterfaceType Type { get; }

        // 获取该配置在 INI 中的 Section 名称
        // 结果类似: "Profile_b3d4a678-..."
        public virtual string GetIniSection()
        {
            return $"{Type}_Translate_{Id}";
        }

        // 抽象方法：让子类自己决定怎么保存特有的字段
        public virtual void SaveToIni()
        {
            string section = GetIniSection();
            // 保存基础信息
            IniHelper.SetValue(section, "Type", Type.ToString());
            IniHelper.SetValue(section, "Name", Name);
        }

        // 抽象方法：让子类自己决定怎么读取
        public virtual void LoadFromIni()
        {
            string section = GetIniSection();
            // Name 已经在外部读取了，这里可以读其他通用的
        }
    }
    // 【子类】百度专用配置
    public class BaiduTranslateProfile : TranslateBaseProfile
    {
        public override TranslateInterfaceType Type => TranslateInterfaceType.Baidu;

        // 这里定义百度独有的字段
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
    }


    // 【子类】腾讯专用配置
    public class TencentTranslateProfile : TranslateBaseProfile
    {
        public override TranslateInterfaceType Type => TranslateInterfaceType.Tencent;

        public string SecretId { get; set; }
        public string SecretKey { get; set; }
    }
}