using System;
using System.Collections.Generic;
using TrOCR.Helper;

namespace TrOCR.Models
{
    // 接口类型枚举
    public enum OCRInterfaceType
    {
        Baidu,
        Tencent,
        OpenAI,
        Google
    }

    // 基类，另一种方案
    //public class InterfaceProfile
    //{
    //    // 1. 公共字段（所有接口都有的）
    //    public string Id { get; set; } = Guid.NewGuid().ToString();
    //    public string Name { get; set; }       // 用户起的名，如 "公司GPT"
    //    public InterfaceType Type { get; set; } // 关键：区分这是什么接口

    //    //// 2. 核心数据包（不管你是Key、Secret还是Url，全放这里）
    //    //public Dictionary<string, string> Config { get; set; }

    //    //public InterfaceProfile()
    //    //{
    //    //    Config = new Dictionary<string, string>();
    //    //}
    //}
    // 【父类】大家都有的属性：ID、名字、类型
    public abstract class OCRBaseProfile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        // 强制子类必须声明自己是什么类型
        public abstract OCRInterfaceType Type { get; }

        // 获取该配置在 INI 中的 Section 名称
        // 结果类似: "Profile_b3d4a678-..."
        public virtual string GetIniSection()
        {
            return $"{Type}_OCR_{Id}";
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

    // 百度 OCR 的具体细分模式
    public enum BaiduOcrModel
    {
        GeneralBasic,       // 通用文字识别（标准版）
        GeneralAccurate,    // 通用文字识别（高精度版）
        Table,            // 表格识别
        Handwriting,        // 手写文字识别
    }
    // 【子类】百度专用配置
    public class BaiduOCRProfile : OCRBaseProfile
    {
        public override OCRInterfaceType Type => OCRInterfaceType.Baidu;

        // 【新增】具体的识别模式，给个默认值
        public BaiduOcrModel ModelType { get; set; } = BaiduOcrModel.GeneralBasic;

        // 这里定义百度独有的字段
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string language_code { get; set; }

      

        public override void SaveToIni()
        {
            base.SaveToIni(); // 保存 ID, Name, Type

            string section = GetIniSection();
            IniHelper.SetValue(section, "api_key", ApiKey);
            IniHelper.SetValue(section, "secret_key", SecretKey);
            IniHelper.SetValue(section, "language_code", language_code);

            // 【新增】保存细分模式
            IniHelper.SetValue(section, "ModelType", ModelType.ToString());
        }

        public override void LoadFromIni()
        {
            base.LoadFromIni();

            string section = GetIniSection();
            ApiKey = IniHelper.GetValue(section, "AK");
            SecretKey = IniHelper.GetValue(section, "SK");

            // 【新增】读取细分模式
            string modelStr = IniHelper.GetValue(section, "ModelType");
            if (Enum.TryParse(modelStr, out BaiduOcrModel model))
            {
                ModelType = model;
            }
        }
    }

    

    // 【子类】腾讯专用配置
    public class TencentOCRProfile : OCRBaseProfile
    {
        public override OCRInterfaceType Type => OCRInterfaceType.Tencent;

        public string SecretId { get; set; }
        public string SecretKey { get; set; }
    }
}