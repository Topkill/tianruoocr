﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using TrOCR.Helper;

namespace TrOCR
{
	public partial class FmMain
	{
		/// <summary>
		/// 将文本中的标点符号转换为中文标点格式
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_Chinese_Click(object sender, EventArgs e)
		{
			language = "中文标点";
			// 只有当文本内容不为空时才执行标点符号转换
			if (typeset_txt != "")
			{
				RichBoxBody.Text = punctuation_en_ch_x(RichBoxBody.Text);
				RichBoxBody.Text = punctuation_quotation(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将文本中的标点符号转换为英文标点格式
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_English_Click(object sender, EventArgs e)
		{
			language = "英文标点";
			// 只有当文本内容不为空时才执行标点符号转换
			if (typeset_txt != "")
			{
				RichBoxBody.Text = punctuation_ch_en(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将中文标点符号转换为对应的英文标点符号
		/// </summary>
		/// <param name="text">需要转换的文本</param>
		/// <returns>转换后的文本</returns>
		public static string punctuation_ch_en(string text)
		{
			// 将字符串转换为字符数组以便逐个处理
			var array = text.ToCharArray();
			// 定义中文标点符号字符串
			var chinesePunctuation = "：。；，？！“”‘’【】（）";
			// 定义对应的英文标点符号字符串
			var englishPunctuation = ":.;,?!\"\"''[]()";
			
			// 遍历每个字符，查找是否为需要转换的中文标点
			for (var i = 0; i < array.Length; i++)
			{
				// 查找当前字符在中文标点字符串中的位置
				var num = chinesePunctuation.IndexOf(array[i]);
				// 如果找到了对应的中文标点，则替换为对应的英文标点
				if (num != -1)
				{
					array[i] = englishPunctuation[num];
				}
			}
			// 将处理后的字符数组重新组合成字符串并返回
			return new string(array);
		}

		/// <summary>
		/// 判断字符串是否为纯数字
		/// </summary>
		/// <param name="str">待检测的字符串</param>
		/// <returns>如果是纯数字返回true，否则返回false</returns>
		public static bool IsNum(string str)
		{
			return Regex.IsMatch(str, "^\\d+$");
		}

		/// <summary>
		/// 判断字符串是否为标点符号
		/// </summary>
		/// <param name="text">待检测的字符串</param>
		/// <returns>如果在预定义标点符号列表中返回true，否则返回false</returns>
		public bool own_punctuation(string text)
		{
			return ",;，、<>《》()-（）.。".IndexOf(text, StringComparison.Ordinal) != -1;
		}

		/// <summary>
		/// 处理标点符号与文字间的空格
		/// </summary>
		/// <param name="text">待处理的文本</param>
		/// <returns>处理后的文本</returns>
		public static string punctuation_Del_space(string text)
		{
			var pattern = "(?<=.)([^\\*]+)(?=.)";
			string result;
			if (Regex.Match(text, pattern).ToString().IndexOf(" ", StringComparison.Ordinal) >= 0)
			{
				// 在特定标点符号后添加空格
				text = Regex.Replace(text, "(?<=[\\p{P}*])([a-zA-Z])(?=[a-zA-Z])", " $1");
				// 清理文本末尾空格并处理特殊符号组合
				text = text.TrimEnd(null).Replace("- ", "-").Replace("_ ", "_").Replace("( ", "(").Replace("/ ", "/").Replace("\" ", "\"");
				result = text;
			}
			else
			{
				result = text;
			}
			return result;
		}

		/// <summary>
		/// 判断字符串是否包含中文字符
		/// </summary>
		/// <param name="str">待检测的字符串</param>
		/// <returns>如果包含中文字符返回true，否则返回false</returns>
		public static bool contain_ch(string str)
		{
			return Regex.IsMatch(str, "[\\u4e00-\\u9fa5]");
		}

		/// <summary>
		/// 检查文本中是否包含指定子字符串
		/// </summary>
		/// <param name="text">要检查的完整文本</param>
		/// <param name="subStr">要查找的子字符串</param>
		/// <returns>如果text包含subStr则返回true，否则返回false</returns>
		public bool contain(string text, string subStr)
		{
			return text.Contains(subStr);
		}

		/// <summary>
		/// 检查字符串中是否包含英文字母
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果字符串包含英文字母则返回true，否则返回false</returns>
		public static bool contain_en(string str)
		{
			return Regex.IsMatch(str, "[a-zA-Z]");
		}

		/// <summary>
		/// 检查字符串是否包含标点符号（根据中英文使用不同的标点符号集）
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果字符串包含标点符号则返回true，否则返回false</returns>
		public static bool punctuation_has_punctuation(string str)
		{
			var pattern = contain_ch(str) ? "[\\；\\，\\。\\！\\？]" : "[\\;\\,\\.\\!\\?]";
			return Regex.IsMatch(str, pattern);
		}

		/// <summary>
		/// 处理字符串中的引号符号，将英文引号替换为中文引号
		/// </summary>
		/// <param name="pStr">需要处理的字符串</param>
		/// <returns>处理后的字符串，引号已替换为中文引号</returns>
		private string punctuation_quotation(string pStr)
		{
			pStr = pStr.Replace("“", "\"").Replace("”", "\"");
			var array = pStr.Split('"');
			var text = "";
			for (var i = 1; i <= array.Length; i++)
			{
				if (i % 2 == 0)
				{
					text = text + array[i - 1] + "”";
				}
				else
				{
					text = text + array[i - 1] + "“";
				}
			}
			return text.Substring(0, text.Length - 1);
		}

		/// <summary>
		/// 删除文本中的多余空格
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>删除多余空格后的文本</returns>
		public static string Del_Space(string text)
		{
			text = Regex.Replace(text, "([\\p{P}]+)", "**&&**$1**&&**");
			text = text.TrimEnd(null).Replace(" **&&**", "").Replace("**&&** ", "").Replace("**&&**", "");
			return text;
		}

		/// <summary>
		/// 检查字符串是否包含韩文字符
		/// </summary>
		/// <param name="str">要检查的字符串</param>
		/// <returns>如果包含韩文字符则返回true，否则返回false</returns>
		public static bool contain_kor(string str)
		{
			return Regex.IsMatch(str, "[\\uac00-\\ud7ff]");
		}

		/// <summary>
		/// 将字符串转换为简体中文
		/// </summary>
		/// <param name="source">需要转换的源字符串</param>
		/// <returns>转换后的简体中文字符串</returns>
		public static string ToSimplified(string source)
		{
			var text = new string(' ', source.Length);
			HelpWin32.LCMapString(2048, 33554432, source, source.Length, text, source.Length);
			return text;
		}

		/// <summary>
		/// 将字符串转换为繁体中文
		/// </summary>
		/// <param name="source">需要转换的源字符串</param>
		/// <returns>转换后的繁体中文字符串</returns>
		public static string ToTraditional(string source)
		{
			var text = new string(' ', source.Length);
			HelpWin32.LCMapString(2048, 67108864, source, source.Length, text, source.Length);
			return text;
		}

		/// <summary>
		/// 将文本框中的文本转换为繁体中文
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_zh_tra_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = ToTraditional(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将文本框中的文本转换为简体中文
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_tra_zh_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = ToSimplified(RichBoxBody.Text);
			}
		}

		/// <summary>
		/// 将文本框中的文本转换为大写
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_str_Upper_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = RichBoxBody.Text.ToUpper();
			}
		}

		/// <summary>
		/// 将文本框中的文本转换为小写
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_Upper_str_Click(object sender, EventArgs e)
		{
			if (RichBoxBody.Text != null)
			{
				RichBoxBody.Text = RichBoxBody.Text.ToLower();
			}
		}

		/// <summary>
		/// 检查并处理字符串中的标点符号
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>处理后的文本</returns>
		public string check_str(string text)
		{
			// 根据文本是否包含中文进行不同的标点符号处理
			if (contain_ch(text.Trim()))
			{
				text = CommonHelper.EnPunctuation2Ch(text.Trim());
				text = check_ch_en(text.Trim());
			}
			else
			{
				text = punctuation_ch_en(text.Trim());
				// 如果包含点号且包含其他特定符号，则删除标点符号周围的空格
				if (contain(text, ".") && (contain(text, ",") || contain(text, "!") || contain(text, "(") || contain(text, ")") || contain(text, "'")))
				{
					text = punctuation_Del_space(text);
				}
			}
			return text;
		}

		/// <summary>
		/// 将英文标点符号替换为中文标点符号
		/// </summary>
		/// <param name="text">需要处理的文本</param>
		/// <returns>替换标点符号后的文本</returns>
		public static string punctuation_en_ch_x(string text)
		{
			var array = text.ToCharArray();
			// 遍历字符数组，将英文标点替换为对应的中文标点
			for (var i = 0; i < array.Length; i++)
			{
				var num = ".:;,?![]()".IndexOf(array[i]);
				if (num != -1)
				{
					array[i] = "。：；，？！【】（）"[num];
				}
			}
			return new string(array);
		}

		/// <summary>
		/// 检查字符串是否包含标点符号
		/// </summary>
		/// <param name="str">待检查的字符串</param>
		/// <returns>如果包含标点符号则返回true，否则返回false</returns>
		public static bool contain_punctuation(string str)
		{
			return Regex.IsMatch(str, "\\p{P}");
		}

		/// <summary>
		/// 判断字符是否为特定标点符号
		/// </summary>
		/// <param name="text">待判断的字符</param>
		/// <returns>如果是指定标点符号返回true，否则返回false</returns>
		public bool Is_punctuation(string text)
		{
			return ",;:，（）、；".IndexOf(text) != -1;
		}

		/// <summary>
		/// 判断字符是否包含另一组特定标点符号
		/// </summary>
		/// <param name="text">待判断的字符</param>
		/// <returns>如果是指定标点符号返回true，否则返回false</returns>
		public bool has_punctuation(string text)
		{
			return ",;，；、<>《》()-（）".IndexOf(text) != -1;
		}

		/// <summary>
		/// 对OCR识别结果进行文本段落检查和处理，根据字符类型和规则进行智能换行
		/// </summary>
		/// <param name="jarray">包含OCR识别结果的JSON数组</param>
		/// <param name="lastlength">从文本末尾起取多少个字符进行换行判断</param>
		/// <param name="words">JSON对象中包含文本内容的字段名</param>
		public void checked_txt(JArray jarray, int lastlength, string words)
		{
			// 查找所有文本中最长的文本长度
			var num = 0;
			for (var i = 0; i < jarray.Count; i++)
			{
				var length = JObject.Parse(jarray[i].ToString())[words].ToString().Length;
				if (length > num)
				{
					num = length;
				}
			}
			var str = "";
			var text = "";
			// 遍历相邻的文本对，根据字符类型和规则判断是否需要换行
			for (var j = 0; j < jarray.Count - 1; j++)
			{
				var jobject = JObject.Parse(jarray[j].ToString());
				var array = jobject[words].ToString().ToCharArray();
				var jobject2 = JObject.Parse(jarray[j + 1].ToString());
				var array2 = jobject2[words].ToString().ToCharArray();
				var length2 = jobject[words].ToString().Length;
				var length3 = jobject2[words].ToString().Length;
				if (Math.Abs(length2 - length3) <= 0)
				{
					if (split_paragraph(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && Is_punctuation(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else
					{
						text += jobject[words].ToString().Trim();
					}
				}
				else if (split_paragraph(array[array.Length - lastlength].ToString()) && Math.Abs(length2 - length3) <= 1)
				{
					if (split_paragraph(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else if (split_paragraph(array[array.Length - lastlength].ToString()) && Is_punctuation(array2[0].ToString()))
					{
						text = text + jobject[words].ToString().Trim() + "\r\n";
					}
					else
					{
						text += jobject[words].ToString().Trim();
					}
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && length2 <= num / 2)
				{
					text = text + jobject[words].ToString().Trim() + "\r\n";
				}
				else if (array2.Length > 1 && contain_ch(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()) && length3 - length2 < 4 && array2[1].ToString() == ".")
				{
					text = text + jobject[words].ToString().Trim() + "\r\n";
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (contain_en(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
				{
					text = text + jobject[words].ToString().Trim() + " ";
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (contain_en(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && Is_punctuation(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (Is_punctuation(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (Is_punctuation(array[array.Length - lastlength].ToString()) && contain_en(array2[0].ToString()))
				{
					text = text + jobject[words].ToString().Trim() + " ";
				}
				else if (contain_ch(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (IsNum(array[array.Length - lastlength].ToString()) && contain_ch(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else if (IsNum(array[array.Length - lastlength].ToString()) && IsNum(array2[0].ToString()))
				{
					text += jobject[words].ToString().Trim();
				}
				else
				{
					text = text + jobject[words].ToString().Trim() + "\r\n";
				}
				// 如果当前文本包含特定标点符号，则添加额外换行
				if (has_punctuation(jobject[words].ToString()))
				{
					text += "\r\n";
				}
				str = str + jobject[words].ToString().Trim() + "\r\n";
			}
			// 将处理后的文本分别赋值给split_txt和typeset_txt字段
			split_txt = str + JObject.Parse(jarray[jarray.Count - 1].ToString())[words];
			typeset_txt = text.Replace("\r\n\r\n", "\r\n") + JObject.Parse(jarray[jarray.Count - 1].ToString())[words];
		}

		/// <summary>
		/// 判断指定字符是否为段落分隔符
		/// </summary>
		/// <param name="text">需要判断的字符</param>
		/// <returns>如果是段落分隔符则返回true，否则返回false</returns>
		public bool split_paragraph(string text)
		{
			return "。？！?!：".IndexOf(text, StringComparison.Ordinal) != -1;
		}

		/// <summary>
		/// 处理拼音切换按钮点击事件，设置拼音标志并触发翻译操作
		/// </summary>
		/// <param name="sender">事件发送者</param>
		/// <param name="e">事件参数</param>
		public void change_pinyin_Click(object sender, EventArgs e)
		{
			pinyin_flag = true;
			TransClick();
		}

		/// <summary>
		/// “盘古之白排版”菜单项点击事件处理
		/// </summary>
		private void pangu_spacing_Click(object sender, EventArgs e)
		{

			// 判断当前哪个文本框是活动的
			if (RichBoxBody.richTextBox1.Focused)
			{
				string originalText = RichBoxBody.Text;
                RichBoxBody.Text = Pangu.Net.Pangu.SpacingText(originalText);
                Debug.WriteLine($"输入: '{originalText}'\n输出: -------'{RichBoxBody.Text}'", "盘古之白功能测试");
            }
			else if (RichBoxBody_T.richTextBox1.Focused)
			{
				string originalText = RichBoxBody_T.Text;
				RichBoxBody_T.Text = Pangu.Net.Pangu.SpacingText(originalText);
                Debug.WriteLine($"输入: '{originalText}'\n输出: '{RichBoxBody.Text}'", "盘古之白功能测试");
            }
			else
			{
				// 如果两个文本框都没有焦点，可以默认处理原文框
				string originalText = RichBoxBody.Text;
				RichBoxBody.Text = Pangu.Net.Pangu.SpacingText(originalText);
                Debug.WriteLine($"输入: '{originalText}'\n输出: '{RichBoxBody.Text}'", "盘古之白功能测试");
            }
		}

		/// <summary>
		/// 检查搜狗OCR识别结果并进行排版处理
		/// </summary>
		/// <param name="jarray">包含OCR识别结果的JSON数组</param>
		/// <param name="lastlength">用于判断文本结尾的长度参数</param>
		/// <param name="words">包含文本内容的字段名</param>
		/// <param name="location">包含位置信息的字段名</param>
		public void checked_location_sougou(JArray jarray, int lastlength, string words, string location)
		{
			paragraph = false;
			var num = 20000;
			var num2 = 0;
			// 遍历OCR识别结果，获取文本位置信息
			foreach (var t in jarray)
			{
				var jObject = JObject.Parse(t.ToString());
				var num3 = split_char_x(jObject[location][1].ToString()) - split_char_x(jObject[location][0].ToString());
				if (num3 > num2)
				{
					num2 = num3;
				}
				var num4 = split_char_x(jObject[location][0].ToString());
				if (num4 < num)
				{
					num = num4;
				}
			}
			var jobject2 = JObject.Parse(jarray[0].ToString());
			if (Math.Abs(split_char_x(jobject2[location][0].ToString()) - num) > 10)
			{
				paragraph = true;
			}
			var text = "";
			var text2 = "";
			// 根据位置信息对文本进行排版处理
			for (var j = 0; j < jarray.Count; j++)
			{
				var jobject3 = JObject.Parse(jarray[j].ToString());
				var array = jobject3[words].ToString().ToCharArray();
				var jobject4 = JObject.Parse(jarray[j].ToString());
				var flag = Math.Abs(split_char_x(jobject4[location][1].ToString()) - split_char_x(jobject4[location][0].ToString()) - num2) > 20;
				var flag2 = Math.Abs(split_char_x(jobject4[location][0].ToString()) - num) > 10;
				if (flag && flag2)
				{
					text = text.Trim() + "\r\n" + jobject4[words].ToString().Trim();
				}
				else if (array.Length > 1 && IsNum(array[0].ToString()) && !contain_ch(array[1].ToString()) && flag)
				{
					text = text.Trim() + "\r\n" + jobject4[words].ToString().Trim() + "\r\n";
				}
				else
				{
					text += jobject4[words].ToString().Trim();
				}
				if (contain_en(array[array.Length - lastlength].ToString()))
				{
					text = text + jobject3[words].ToString().Trim() + " ";
				}
				text2 = text2 + jobject4[words].ToString().Trim() + "\r\n";
			}
			split_txt = text2.Replace("\r\n\r\n", "\r\n");
			typeset_txt = text;
		}

		/// <summary>
		/// 从坐标字符串中提取X坐标值
		/// </summary>
		/// <param name="splitChar">格式为"x,y"的坐标字符串</param>
		/// <returns>X坐标值</returns>
		public int split_char_x(string splitChar)
		{
			return Convert.ToInt32(splitChar.Split(',')[0]);
		}

		/// <summary>
		/// 统计文本中的英文单词数量
		/// </summary>
		/// <param name="text">待统计的文本</param>
		/// <returns>英文单词数量</returns>
		public int en_count(string text)
		{
			return Regex.Matches(text, "\\s+").Count + 1;
		}

		/// <summary>
		/// 统计文本中的中文字符数量
		/// </summary>
		/// <param name="str">待统计的字符串</param>
		/// <returns>中文字符数量</returns>
		public int ch_count(string str)
		{
			var num = 0;
			var regex = new Regex("^[\\u4E00-\\u9FA5]{0,}$");
			for (var i = 0; i < str.Length; i++)
			{
				if (regex.IsMatch(str[i].ToString()))
				{
					num++;
				}
			}
			return num;
		}

		/// <summary>
		/// 对单行字符串进行智能空格清理：移除不必要的空格，并在中英文/数字间补全必要空格。
		/// </summary>
		/// <param name="line">需要处理的单行文本</param>
		/// <returns>经过智能空格处理后的文本</returns>
		private string SmartSpaceClean(string line)
		{
		    // 1. 规范化：将一行中连续的多个空格（半角/全角）替换为单个半角空格，并去除首尾空格
		    string normalizedLine = Regex.Replace(line, @"[ \　]+", " ").Trim();
		    if (normalizedLine.Length <= 1)
		    {
		        return normalizedLine;
		    }

		    StringBuilder lineSb = new StringBuilder();
		    lineSb.Append(normalizedLine[0]);

		    for (int j = 1; j < normalizedLine.Length; j++)
		    {
		        char lastChar = normalizedLine[j - 1];
		        char currentChar = normalizedLine[j];

		        // 2. 修正：移除中文汉字之间的空格
		        if (lastChar >= 0x4E00 && lastChar <= 0x9FA5 && currentChar == ' ' && (j + 1 < normalizedLine.Length) && (normalizedLine[j + 1] >= 0x4E00 && normalizedLine[j + 1] <= 0x9FA5))
		        {
		            continue; // 跳过这个空格，不添加到结果中
		        }

		        // 3. 补充：在需要且当前没有空格的地方添加空格
		        bool lastIsEnglish = (lastChar >= 'a' && lastChar <= 'z') || (lastChar >= 'A' && lastChar <= 'Z');
		        bool lastIsNumber = char.IsDigit(lastChar);
		        bool currentIsEnglish = (currentChar >= 'a' && currentChar <= 'z') || (currentChar >= 'A' && currentChar <= 'Z');
		        bool currentIsNumber = char.IsDigit(currentChar);
		        bool lastIsHanzi = lastChar >= 0x4E00 && lastChar <= 0x9FA5;
				bool currentIsHanzi = currentChar >= 0x4E00 && currentChar <= 0x9FA5;

		        bool spaceNeeded = (lastIsHanzi && (currentIsEnglish || currentIsNumber)) ||
		                           ((lastIsEnglish || lastIsNumber) && currentIsHanzi);

		        if (spaceNeeded && lastChar != ' ')
		        {
		            lineSb.Append(" ");
		        }

		        lineSb.Append(currentChar);
		    }
		    return lineSb.ToString();
		}

		/// <summary>
		/// 对多行或单行文本执行统一的智能合并操作
		/// </summary>
		/// <param name="inputText">需要合并的原始文本</param>
		/// <param name="enableSmartSpacing">是否启用智能空格处理模式</param>
		/// <returns>合并后的文本</returns>
		private string PerformIntelligentMerge(string inputText, bool enableSmartSpacing)
		{
		    if (string.IsNullOrEmpty(inputText))
		        return string.Empty;

		    string[] lines = inputText.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

		    StringBuilder sb = new StringBuilder();
		    for (int i = 0; i < lines.Length; i++)
		    {
		        string processedLine;
		        if (enableSmartSpacing)
		        {
		            processedLine = SmartSpaceClean(lines[i]);
		        }
		        else
		        {
		            processedLine = lines[i].Trim();
		        }

		        if (string.IsNullOrEmpty(processedLine)) continue;

		        sb.Append(processedLine);

		        if (i < lines.Length - 1)
		        {
		            string nextLineRaw = lines[i + 1];
		            if (!string.IsNullOrWhiteSpace(nextLineRaw))
		            {
		                char lastChar = processedLine.LastOrDefault();
		                string nextLineProcessed = enableSmartSpacing ? SmartSpaceClean(nextLineRaw) : nextLineRaw.Trim();

		                if (!string.IsNullOrEmpty(nextLineProcessed))
		                {
		                    char firstChar = nextLineProcessed.FirstOrDefault();

		                     // --- 【核心修改】细分字符类型 ---
		                    bool lastIsEnglish = (lastChar >= 'a' && lastChar <= 'z') || (lastChar >= 'A' && lastChar <= 'Z');
		                    bool lastIsNumber = char.IsDigit(lastChar);
		                    bool firstIsEnglish = (firstChar >= 'a' && firstChar <= 'z') || (firstChar >= 'A' && firstChar <= 'Z');
		                    bool firstIsNumber = char.IsDigit(firstChar);
		                    bool lastIsHanzi = lastChar >= 0x4E00 && lastChar <= 0x9FA5;
		                    bool firstIsHanzi = firstChar >= 0x4E00 && firstChar <= 0x9FA5;

		                    // --- 【核心修改】更新添加空格的规则 ---
                    		if ( (lastIsEnglish && firstIsEnglish) ||                                 // 英文-英文
                    		     (lastIsHanzi && (firstIsEnglish || firstIsNumber)) ||               // 中文-英文/数字
                    		     ((lastIsEnglish || lastIsNumber) && firstIsHanzi) )                  // 英文/数字-中文
		                    {
		                        sb.Append(" ");
		                    }
		                }
		            }
		        }
		    }
		    return sb.ToString();
		}
	}
}