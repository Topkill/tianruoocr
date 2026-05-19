using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TrOCR.Properties;

namespace TrOCR.Helper
{
    public class HanToPinyin
    {
        private static readonly Dictionary<string, string> WordsDictionary;
        private static readonly int MaxWordLength;
        static HanToPinyin()
        {
            var text = Resources.pinyin;
            WordsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            MaxWordLength = WordsDictionary.Keys.Any() ? WordsDictionary.Keys.Max(k => k.Length) : 0;
        }

        public static string GetFirstLetter(string input)
        {
            input = input.Split(new[] { ':', '-' }, StringSplitOptions.RemoveEmptyEntries)[0];
            input = Regex.Replace(input, @"[^\u4e00-\u9fa5]", "");
            var strArr = GetFullPinyin(input).Split(new[] {'\t', ' '}, StringSplitOptions.RemoveEmptyEntries);
            return strArr.Aggregate("", (current, s) => current + s[0]).ToUpper();
        }

        public static string GetFullPinyin(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input ?? string.Empty;
            }

            var builder = new StringBuilder();
            var index = 0;
            // 【新增状态标记】记录上一次追加的是不是字典匹配成功/是不是拼音
            bool lastWasPinyin = false;
            while (index < input.Length)
            {
                var lengthToCheck = Math.Min(MaxWordLength, input.Length - index);
                var matchedLength = 0;
                string matchedValue = null;

                // 采用“最长匹配”策略，减少拆字错误
                for (var i = lengthToCheck; i > 0; i--)
                {
                    var candidate = input.Substring(index, i);
                    if (WordsDictionary.TryGetValue(candidate, out matchedValue))
                    {
                        matchedLength = i;
                        break;
                    }
                }

                //  字典匹配成功的处理
                if (matchedLength > 0)
                {
                    // 修改前：
                    // builder.Append(matchedValue);

                    // 修改后：去除首尾空白（包括\t），然后手动加一个标准空格
                    // builder.Append(matchedValue.Trim() + " ");

                    // A. 清理字典自带的脏数据（去掉 \t 和空格）
                    string cleanPinyin = matchedValue.Trim();

                    // B. 智能加空格：如果 Builder 不为空，且最后一个字符不是空格，说明前面有内容（示例文本：“你好632 KB你好”，可能是英文KB，可能是数字632，也可能是上一个拼音）
                    //    此时在当前拼音前面加一个空格，隔开它们。
                    // 如果前面有内容，且前面紧挨着的是【字母或数字】时，就补一个空格。这既防止了粘连，又防止了原文本来就有空格时导致双重空格。
                    if (builder.Length > 0 && builder[builder.Length - 1] != ' ' && char.IsLetterOrDigit(builder[builder.Length - 1]))
                    {
                        builder.Append(" ");
                    }

                    builder.Append(cleanPinyin);


                    index += matchedLength;
                    // 标记：刚刚处理的是拼音
                    lastWasPinyin = true;
                    continue;
                }
                // === 分支：没匹配到 (隐式 Else) ===
                // 字典中不存在的字符直接原样附加，避免抛异常导致整段失败
                char currentChar = input[index];
                // 策略：如果上一个字典匹配成功/是拼音，且当前字符（英文、数字）没匹配到，当前字符前面加一个空格
                if (lastWasPinyin && char.IsLetterOrDigit(currentChar))
                {
                    // 防御判断，防止原文里本身就有空格导致双空格
                    if (builder.Length > 0 && builder[builder.Length - 1] != ' ')
                    {
                        builder.Append(" ");
                    }
                }
                builder.Append(currentChar);
                index++;

                lastWasPinyin = false; // 重置状态
            }
            // 最后返回时，把末尾多余的一个空格去掉
            return builder.ToString().Trim();
        }
    }
}