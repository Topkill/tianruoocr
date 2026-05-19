using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pangu.Net
{
    public static class Pangu
    {
        //对应 pangu.js7.2.0版本

        // CJK is short for Chinese, Japanese, and Korean:
        // \u2e80-\u2eff CJK Radicals Supplement
        // \u2f00-\u2fdf Kangxi Radicals
        // \u3040-\u309f Hiragana
        // \u30a0-\u30ff Katakana
        // \u3100-\u312f Bopomofo
        // \u3200-\u32ff Enclosed CJK Letters and Months
        // \u3400-\u4dbf CJK Unified Ideographs Extension A
        // \u4e00-\u9fff CJK Unified Ideographs
        // \uf900-\ufaff CJK Compatibility Ideographs
        private const string CJK = "\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30fa\u30fc-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff";

        // Basic character classes
        private const string AN = "A-Za-z0-9";
        private const string A = "A-Za-z";
        private const string UPPER_AN = "A-Z0-9"; // For FIX_CJK_COLON_ANS

        // Operators
        private const string OPERATORS_BASE = @"\+\*=&";
        private const string OPERATORS_WITH_HYPHEN = OPERATORS_BASE + @"\-"; // For CJK patterns
        private const string OPERATORS_NO_HYPHEN = OPERATORS_BASE; // For ANS_OPERATOR_ANS only
        private const string GRADE_OPERATORS = @"\+\-\*"; // For single letter grades

        // Quotes
        private const string QUOTES = "`\"\u05f4";

        // Brackets
        private const string LEFT_BRACKETS_BASIC = @"\(\[\{";
        private const string RIGHT_BRACKETS_BASIC = @"\)\]\}";
        private const string LEFT_BRACKETS_EXTENDED = @"\(\[\{<>\u201c";
        private const string RIGHT_BRACKETS_EXTENDED = @"\)\]\}<>\u201d";

        // ANS extended sets
        private const string ANS_CJK_AFTER = A + @"\u0370-\u03ff0-9@\$\%\^\&\*\-\+\\\=\u00a1-\u00ff\u2150-\u218f\u2700—\u27bf";
        private const string ANS_BEFORE_CJK = A + @"\u0370-\u03ff0-9\$\%\^\&\*\-\+\\\=\u00a1-\u00ff\u2150-\u218f\u2700—\u27bf";

        // File path components
        private const string FILE_PATH_DIRS = @"home|root|usr|etc|var|opt|tmp|dev|mnt|proc|sys|bin|boot|lib|media|run|sbin|srv|node_modules|path|project|src|dist|test|tests|docs|templates|assets|public|static|config|scripts|tools|build|out|target|your|\.claude|\.git|\.vscode";
        private const string FILE_PATH_CHARS = @"[A-Za-z0-9_\-\. @\+\*]+";

        // Regex instances
        private static readonly Regex UNIX_ABSOLUTE_FILE_PATH = new Regex($@"/(?:\.?(?:{FILE_PATH_DIRS})|\.(?:[A-Za-z0-9_\-]+))(?:/{FILE_PATH_CHARS})*");
        private static readonly Regex UNIX_RELATIVE_FILE_PATH = new Regex($@"(?:\./)?(?:{FILE_PATH_DIRS})(?:/{FILE_PATH_CHARS})+");
        private static readonly Regex WINDOWS_FILE_PATH = new Regex(@"[A-Z]:\\(?:[A-Za-z0-9_\-\. ]+\\?)+");

        public static readonly Regex ANY_CJK = new Regex($"[{CJK}]");

        private static readonly Regex CJK_PUNCTUATION = new Regex($"([{CJK}])([!;,\\?:]+)(?=[{CJK}{AN}])");
        private static readonly Regex AN_PUNCTUATION_CJK = new Regex($"([{AN}])([!;,\\?]+)([{CJK}])");
        private static readonly Regex CJK_TILDE = new Regex($"([{CJK}])(~+)(?!=)(?=[{CJK}{AN}])");
        private static readonly Regex CJK_TILDE_EQUALS = new Regex($"([{CJK}])(~=)");
        private static readonly Regex CJK_PERIOD = new Regex($@"([{CJK}])(\.)(?![{AN}\./])(?=[{CJK}{AN}])");
        private static readonly Regex AN_PERIOD_CJK = new Regex($@"([{AN}])(\.)([{CJK}])");
        private static readonly Regex AN_COLON_CJK = new Regex($@"([{AN}])(:)([{CJK}])");
        private static readonly Regex DOTS_CJK = new Regex($@"([\.]{{2,}}|\u2026)([{CJK}])");
        private static readonly Regex FIX_CJK_COLON_ANS = new Regex($"([{CJK}]):([{UPPER_AN}\\(\\)])");

        private static readonly Regex CJK_QUOTE = new Regex($"([{CJK}])([{QUOTES}])");
        private static readonly Regex QUOTE_CJK = new Regex($"([{QUOTES}])([{CJK}])");
        private static readonly Regex FIX_QUOTE_ANY_QUOTE = new Regex($"([{QUOTES}]+)[ ]*(.+?)[ ]*([{QUOTES}]+)");

        private static readonly Regex QUOTE_AN = new Regex($"([\u201d])([{AN}])");
        private static readonly Regex CJK_QUOTE_AN = new Regex($"([{CJK}])(\")([{AN}])");

        private static readonly Regex CJK_SINGLE_QUOTE_BUT_POSSESSIVE = new Regex($"([{CJK}])('[^s])");
        private static readonly Regex SINGLE_QUOTE_CJK = new Regex($"(')([{CJK}])");
        private static readonly Regex FIX_POSSESSIVE_SINGLE_QUOTE = new Regex($"([{AN}{CJK}])( )('s)");

        private static readonly Regex HASH_ANS_CJK_HASH = new Regex($"([{CJK}])(#)([{CJK}]+)(#)([{CJK}])");
        private static readonly Regex CJK_HASH = new Regex($"([{CJK}])(#([^ ]))");
        private static readonly Regex HASH_CJK = new Regex($"(([^ ])#)([{CJK}])");

        private static readonly Regex CJK_OPERATOR_ANS = new Regex($"([{CJK}])([{OPERATORS_WITH_HYPHEN}])([{AN}])");
        private static readonly Regex ANS_OPERATOR_CJK = new Regex($"([{AN}])([{OPERATORS_WITH_HYPHEN}])([{CJK}])");
        private static readonly Regex ANS_OPERATOR_ANS = new Regex($"([{AN}])([{OPERATORS_NO_HYPHEN}])([{AN}])");

        private static readonly Regex ANS_HYPHEN_ANS_NOT_COMPOUND = new Regex($@"([A-Za-z])(-(?![a-z]))([A-Za-z0-9])|([A-Za-z]+[0-9]+)(-(?![a-z]))([0-9])|([0-9])(-(?![a-z0-9]))([A-Za-z])");

        private static readonly Regex CJK_SLASH_CJK = new Regex($"([{CJK}])([/])([{CJK}])");
        private static readonly Regex CJK_SLASH_ANS = new Regex($"([{CJK}])([/])([{AN}])");
        private static readonly Regex ANS_SLASH_CJK = new Regex($"([{AN}])([/])([{CJK}])");
        private static readonly Regex ANS_SLASH_ANS = new Regex($"([{AN}])([/])([{AN}])");

        private static readonly Regex SINGLE_LETTER_GRADE_CJK = new Regex($@"\b([{A}])([{GRADE_OPERATORS}])([{CJK}])");

        private static readonly Regex CJK_LESS_THAN = new Regex($"([{CJK}])(<)([{AN}])");
        private static readonly Regex LESS_THAN_CJK = new Regex($"([{AN}])(<)([{CJK}])");
        private static readonly Regex CJK_GREATER_THAN = new Regex($"([{CJK}])(>)([{AN}])");
        private static readonly Regex GREATER_THAN_CJK = new Regex($"([{AN}])(>)([{CJK}])");
        private static readonly Regex ANS_LESS_THAN_ANS = new Regex($"([{AN}])(<)([{AN}])");
        private static readonly Regex ANS_GREATER_THAN_ANS = new Regex($"([{AN}])(>)([{AN}])");

        private static readonly Regex CJK_LEFT_BRACKET = new Regex($"([{CJK}])([{LEFT_BRACKETS_EXTENDED}])");
        private static readonly Regex RIGHT_BRACKET_CJK = new Regex($"([{RIGHT_BRACKETS_EXTENDED}])([{CJK}])");
        private static readonly Regex ANS_CJK_LEFT_BRACKET_ANY_RIGHT_BRACKET = new Regex($"([{AN}{CJK}])[ ]*([\u201c])([{AN}{CJK}\\-_ ]+)([\u201d])");
        private static readonly Regex LEFT_BRACKET_ANY_RIGHT_BRACKET_ANS_CJK = new Regex($"([\u201c])([{AN}{CJK}\\-_ ]+)([\u201d])[ ]*([{AN}{CJK}])");

        private static readonly Regex AN_LEFT_BRACKET = new Regex($@"([{AN}])(?<!\.[{AN}]*)([{LEFT_BRACKETS_BASIC}])");
        private static readonly Regex RIGHT_BRACKET_AN = new Regex($"([{RIGHT_BRACKETS_BASIC}])([{AN}])");

        private static readonly Regex CJK_UNIX_ABSOLUTE_FILE_PATH = new Regex($"([{CJK}])({UNIX_ABSOLUTE_FILE_PATH})");
        private static readonly Regex CJK_UNIX_RELATIVE_FILE_PATH = new Regex($"([{CJK}])({UNIX_RELATIVE_FILE_PATH})");
        private static readonly Regex CJK_WINDOWS_PATH = new Regex($"([{CJK}])({WINDOWS_FILE_PATH})");

        private static readonly Regex UNIX_ABSOLUTE_FILE_PATH_SLASH_CJK = new Regex($"({UNIX_ABSOLUTE_FILE_PATH}/)([{CJK}])");
        private static readonly Regex UNIX_RELATIVE_FILE_PATH_SLASH_CJK = new Regex($"({UNIX_RELATIVE_FILE_PATH}/)([{CJK}])");

        private static readonly Regex CJK_ANS = new Regex($"([{CJK}])([{ANS_CJK_AFTER}])");
        private static readonly Regex ANS_CJK = new Regex($"([{ANS_BEFORE_CJK}])([{CJK}])");

        private static readonly Regex S_A = new Regex($"(%)([{A}])");
        private static readonly Regex MIDDLE_DOT = new Regex(@"([ ]*)([\u00b7\u2022\u2027])([ ]*)");

        private class PlaceholderReplacer
        {
            private List<string> items = new List<string>();
            private int index = 0;
            private readonly Regex pattern;
            private readonly string placeholder;
            private readonly string startDelimiter;
            private readonly string endDelimiter;

            public PlaceholderReplacer(string placeholder, string startDelimiter, string endDelimiter)
            {
                this.placeholder = placeholder;
                this.startDelimiter = startDelimiter;
                this.endDelimiter = endDelimiter;

                string escapedStart = Regex.Escape(this.startDelimiter);
                string escapedEnd = Regex.Escape(this.endDelimiter);
                this.pattern = new Regex($"{escapedStart}{this.placeholder}(\\d+){escapedEnd}");
            }

            public string Store(string item)
            {
                items.Add(item);
                string result = $"{startDelimiter}{placeholder}{index++}{endDelimiter}";
                return result;
            }

            public string Restore(string text)
            {
                return pattern.Replace(text, match =>
                {
                    if (int.TryParse(match.Groups[1].Value, out int itemIndex) && itemIndex < items.Count)
                    {
                        return items[itemIndex];
                    }
                    return "";
                });
            }

            public void Reset()
            {
                items.Clear();
                index = 0;
            }
        }

        public static string SpacingText(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= 1 || !ANY_CJK.IsMatch(text))
            {
                return text;
            }

            string newText = text;

            var backtickManager = new PlaceholderReplacer("BACKTICK_CONTENT_", "\uE004", "\uE005");
            newText = Regex.Replace(newText, "`([^`]+)`", match => $"`{backtickManager.Store(match.Groups[1].Value)}`");

            var htmlTagManager = new PlaceholderReplacer("HTML_TAG_PLACEHOLDER_", "\uE000", "\uE001");
            bool hasHtmlTags = false;

            if (newText.Contains("<"))
            {
                hasHtmlTags = true;
                var HTML_TAG_PATTERN = new Regex(@"</?[a-zA-Z][a-zA-Z0-9]*(?:\s+[^>]*)?>");

                newText = HTML_TAG_PATTERN.Replace(newText, match =>
                {
                    string processedTag = Regex.Replace(match.Value, "(\\w+)=\"([^\"]*)\"", attrMatch =>
                    {
                        string attrName = attrMatch.Groups[1].Value;
                        string attrValue = attrMatch.Groups[2].Value;
                        string processedValue = SpacingText(attrValue);
                        return $"{attrName}=\"{processedValue}\"";
                    });
                    return htmlTagManager.Store(processedTag);
                });
            }

            newText = DOTS_CJK.Replace(newText, "$1 $2");
            newText = CJK_PUNCTUATION.Replace(newText, "$1$2 ");
            newText = AN_PUNCTUATION_CJK.Replace(newText, "$1$2 $3");
            newText = CJK_TILDE.Replace(newText, "$1$2 ");
            newText = CJK_TILDE_EQUALS.Replace(newText, "$1 $2 ");
            newText = CJK_PERIOD.Replace(newText, "$1$2 ");
            newText = AN_PERIOD_CJK.Replace(newText, "$1$2 $3");
            newText = AN_COLON_CJK.Replace(newText, "$1$2 $3");
            newText = FIX_CJK_COLON_ANS.Replace(newText, "$1：$2");

            newText = CJK_QUOTE.Replace(newText, "$1 $2");
            newText = QUOTE_CJK.Replace(newText, "$1 $2");
            newText = FIX_QUOTE_ANY_QUOTE.Replace(newText, "$1$2$3");

            newText = QUOTE_AN.Replace(newText, "$1 $2");
            newText = CJK_QUOTE_AN.Replace(newText, "$1$2 $3");

            newText = FIX_POSSESSIVE_SINGLE_QUOTE.Replace(newText, "$1's");

            var singleQuoteCJKManager = new PlaceholderReplacer("SINGLE_QUOTE_CJK_PLACEHOLDER_", "\uE030", "\uE031");
            var SINGLE_QUOTE_PURE_CJK = new Regex($"(')([{CJK}]+)(')");

            newText = SINGLE_QUOTE_PURE_CJK.Replace(newText, match => singleQuoteCJKManager.Store(match.Value));

            newText = CJK_SINGLE_QUOTE_BUT_POSSESSIVE.Replace(newText, "$1 $2");
            newText = SINGLE_QUOTE_CJK.Replace(newText, "$1 $2");

            newText = singleQuoteCJKManager.Restore(newText);

            int textLength = newText.Length;
            int slashCount = newText.Count(c => c == '/');

            if (slashCount <= 1)
            {
                if (textLength >= 5)
                {
                    newText = HASH_ANS_CJK_HASH.Replace(newText, "$1 $2$3$4 $5");
                }
                newText = CJK_HASH.Replace(newText, "$1 $2");
                newText = HASH_CJK.Replace(newText, "$1 $3");
            }
            else
            {
                if (textLength >= 5)
                {
                    newText = HASH_ANS_CJK_HASH.Replace(newText, "$1 $2$3$4 $5");
                }
                newText = new Regex($"([^/])([{CJK}])(#[A-Za-z0-9]+)$").Replace(newText, "$1$2 $3");
            }

            var compoundWordManager = new PlaceholderReplacer("COMPOUND_WORD_PLACEHOLDER_", "\uE010", "\uE011");
            var COMPOUND_WORD_PATTERN = new Regex(@"\b(?:[A-Za-z0-9]*[a-z][A-Za-z0-9]*-[A-Za-z0-9]+|[A-Za-z0-9]+-[A-Za-z0-9]*[a-z][A-Za-z0-9]*|[A-Za-z]+-[0-9]+|[A-Za-z]+[0-9]+-[A-Za-z0-9]+)(?:-[A-Za-z0-9]+)*\b");

            newText = COMPOUND_WORD_PATTERN.Replace(newText, match => compoundWordManager.Store(match.Value));

            newText = SINGLE_LETTER_GRADE_CJK.Replace(newText, "$1$2 $3");

            newText = CJK_OPERATOR_ANS.Replace(newText, "$1 $2 $3");
            newText = ANS_OPERATOR_CJK.Replace(newText, "$1 $2 $3");
            newText = ANS_OPERATOR_ANS.Replace(newText, "$1 $2 $3");

            newText = ANS_HYPHEN_ANS_NOT_COMPOUND.Replace(newText, match =>
            {
                if (match.Groups[1].Success && match.Groups[2].Success && match.Groups[3].Success)
                {
                    return $"{match.Groups[1].Value} {match.Groups[2].Value} {match.Groups[3].Value}";
                }
                else if (match.Groups[4].Success && match.Groups[5].Success && match.Groups[6].Success)
                {
                    return $"{match.Groups[4].Value} {match.Groups[5].Value} {match.Groups[6].Value}";
                }
                else if (match.Groups[7].Success && match.Groups[8].Success && match.Groups[9].Success)
                {
                    return $"{match.Groups[7].Value} {match.Groups[8].Value} {match.Groups[9].Value}";
                }
                return match.Value;
            });

            newText = CJK_LESS_THAN.Replace(newText, "$1 $2 $3");
            newText = LESS_THAN_CJK.Replace(newText, "$1 $2 $3");
            newText = ANS_LESS_THAN_ANS.Replace(newText, "$1 $2 $3");
            newText = CJK_GREATER_THAN.Replace(newText, "$1 $2 $3");
            newText = GREATER_THAN_CJK.Replace(newText, "$1 $2 $3");
            newText = ANS_GREATER_THAN_ANS.Replace(newText, "$1 $2 $3");

            newText = CJK_UNIX_ABSOLUTE_FILE_PATH.Replace(newText, "$1 $2");
            newText = CJK_UNIX_RELATIVE_FILE_PATH.Replace(newText, "$1 $2");
            newText = CJK_WINDOWS_PATH.Replace(newText, "$1 $2");

            newText = UNIX_ABSOLUTE_FILE_PATH_SLASH_CJK.Replace(newText, "$1 $2");
            newText = UNIX_RELATIVE_FILE_PATH_SLASH_CJK.Replace(newText, "$1 $2");

            if (slashCount == 1)
            {
                var filePathManager = new PlaceholderReplacer("FILE_PATH_PLACEHOLDER_", "\uE020", "\uE021");
                var allFilePathPattern = new Regex($"({UNIX_ABSOLUTE_FILE_PATH}|{UNIX_RELATIVE_FILE_PATH})");
                newText = allFilePathPattern.Replace(newText, match => filePathManager.Store(match.Value));

                newText = CJK_SLASH_CJK.Replace(newText, "$1 $2 $3");
                newText = CJK_SLASH_ANS.Replace(newText, "$1 $2 $3");
                newText = ANS_SLASH_CJK.Replace(newText, "$1 $2 $3");
                newText = ANS_SLASH_ANS.Replace(newText, "$1 $2 $3");

                newText = filePathManager.Restore(newText);
            }

            newText = compoundWordManager.Restore(newText);

            newText = CJK_LEFT_BRACKET.Replace(newText, "$1 $2");
            newText = RIGHT_BRACKET_CJK.Replace(newText, "$1 $2");
            newText = ANS_CJK_LEFT_BRACKET_ANY_RIGHT_BRACKET.Replace(newText, "$1 $2$3$4");
            newText = LEFT_BRACKET_ANY_RIGHT_BRACKET_ANS_CJK.Replace(newText, "$1$2$3 $4");

            newText = AN_LEFT_BRACKET.Replace(newText, "$1 $2");
            newText = RIGHT_BRACKET_AN.Replace(newText, "$1 $2");

            newText = CJK_ANS.Replace(newText, "$1 $2");
            newText = ANS_CJK.Replace(newText, "$1 $2");

            newText = S_A.Replace(newText, "$1 $2");

            newText = MIDDLE_DOT.Replace(newText, "・");

            newText = FixBracketSpacing(newText);

            if (hasHtmlTags)
            {
                newText = htmlTagManager.Restore(newText);
            }

            newText = backtickManager.Restore(newText);

            return newText;
        }

        private static string FixBracketSpacing(string text)
        {
            var bracketPatterns = new[]
            {
            new { Pattern = new Regex("<([^<>]*)>"), Open = '<', Close = '>' },
            new { Pattern = new Regex("\\(([^()]*)\\)"), Open = '(', Close = ')' },
            new { Pattern = new Regex("\\[([^\\[\\]]*)\\]"), Open = '[', Close = ']' },
            new { Pattern = new Regex("\\{([^{}]*)\\}"), Open = '{', Close = '}' },
        };

            string result = text;
            foreach (var item in bracketPatterns)
            {
                result = item.Pattern.Replace(result, match =>
                {
                    string innerContent = match.Groups[1].Value;
                    if (string.IsNullOrEmpty(innerContent))
                    {
                        return $"{item.Open}{item.Close}";
                    }
                    // In C#, Trim() removes whitespace from both ends.
                    string trimmedContent = innerContent.Trim();
                    return $"{item.Open}{trimmedContent}{item.Close}";
                });
            }
            return result;
        }

        public static bool HasProperSpacing(string text)
        {
            return SpacingText(text) == text;
        }
    }
}