using System;
#if !NET20
using System.Linq;
#endif
using System.Text;
using System.Text.RegularExpressions;

namespace Kai.Universal.Text {

    /// <summary>
    /// Text utility
    /// </summary>
    public class TextUtility {

        private static readonly string STRING_HYPHEN = "-";
        private static readonly string STRING_UNDERSCORE = "_";

        private TextUtility() { }

        /// <summary>
        /// if source is null or blank, change to default string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ToDefaultString(string source, string defaultValue) {
            if (source == null || "".Equals(source.Trim())) {
                return defaultValue;
            }
            return source;
        }

        private static string CamelCaseToDelimiterCase(string s, string delimiter, bool toLowerCase) {
            string caseString = Char.ToUpper(s[0]) + s.Substring(1);

            Regex upperCaseRegex = new Regex(@"[A-Z]{1}[a-z]*");
#if !NET20
            string[] words = upperCaseRegex.Matches(caseString).Cast<Match>().Select(m => m.Value).ToArray();
#else
            var matches = upperCaseRegex.Matches(caseString);
            string[] words = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++) {
                words[i] = matches[i].Value;
            }
#endif
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < words.Length; i++) {
                if (i > 0) {
                    sb.Append(delimiter);
                }
                if (toLowerCase) {
                    sb.Append(words[i].ToLower());
                } else {
                    sb.Append(words[i].ToUpper());
                }
            }
            return sb.ToString();
        }

        private static string DelimiterCaseToCamelCase(string s, string delimiter, bool toLowerCamel) {
            StringBuilder sb = new StringBuilder();

            string[] words = s.ToLower().Split(delimiter.ToCharArray());

            for (int i = 0; i < words.Length; i++) {
                if (i == 0 && toLowerCamel) {
                    sb.Append(words[i]);
                } else {
                    sb.Append(words[i].Substring(0, 1).ToUpper());
                    sb.Append(words[i].Substring(1));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// change word case
        /// </summary>
        /// <param name="s"></param>
        /// <param name="srcCase"></param>
        /// <param name="outCase"></param>
        /// <returns></returns>
        public static string ConvertWordCase(string s, WordCase srcCase, WordCase outCase) {
            if (s == null || "".Equals(s.Trim())) {
                return "";
            }
            if (s.Length == 1) {
                return ConvertCharCase(s[0], outCase).ToString();
            }
            if (srcCase == outCase) {
                return s;
            }

            switch (outCase) {
                case WordCase.LowerCamel:
                    return TextUtility.ToCamelCase(s, srcCase, true);
                case WordCase.UpperCamel:
                    return TextUtility.ToCamelCase(s, srcCase, false);
                case WordCase.LowerUnderscore:
                    return TextUtility.ToUnderscoreCase(s, srcCase, true);
                case WordCase.UpperUnderscore:
                    return TextUtility.ToUnderscoreCase(s, srcCase, false);
                case WordCase.LowerHyphen:
                    return TextUtility.ToHyphenCase(s, srcCase, true);
                case WordCase.UpperHyphen:
                    return TextUtility.ToHyphenCase(s, srcCase, false);

                default:
                    throw new NotSupportedException();
            }

        }

        private static char ConvertCharCase(char c, WordCase outCase) {
            switch (outCase) {
                case WordCase.LowerCamel:
                case WordCase.LowerUnderscore:
                case WordCase.LowerHyphen:
                    return Char.ToLower(c);
                case WordCase.UpperCamel:
                case WordCase.UpperUnderscore:
                case WordCase.UpperHyphen:
                    return Char.ToUpper(c);
                default:
                    throw new NotSupportedException();
            }
        }

        private static string ToCamelCase(string s, WordCase srcCase, bool toLowerCamel) {
            string result = s;
            switch (srcCase) {
                case WordCase.LowerCamel:
                    if (!toLowerCamel) {
                        result = Char.ToUpper(s[0]) + s.Substring(1);
                    }
                    break;
                case WordCase.UpperCamel:
                    if (toLowerCamel) {
                        result = Char.ToLower(s[0]) + s.Substring(1);
                    }
                    break;
                case WordCase.LowerUnderscore:
                case WordCase.UpperUnderscore:
                    result = DelimiterCaseToCamelCase(s, STRING_UNDERSCORE, toLowerCamel);
                    break;
                case WordCase.LowerHyphen:
                case WordCase.UpperHyphen:
                    result = DelimiterCaseToCamelCase(s, STRING_HYPHEN, toLowerCamel);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        private static string ToUnderscoreCase(string s, WordCase srcCase, bool toLowerCase) {
            string result = s;
            switch (srcCase) {
                case WordCase.LowerCamel:
                case WordCase.UpperCamel:
                    result = CamelCaseToDelimiterCase(s, STRING_UNDERSCORE, toLowerCase);
                    break;
                case WordCase.LowerUnderscore:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    break;
                case WordCase.UpperUnderscore:
                    if (toLowerCase) {
                        result = s.ToLower();
                    }
                    break;
                case WordCase.LowerHyphen:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    result = result.Replace(STRING_HYPHEN, STRING_UNDERSCORE);
                    break;
                case WordCase.UpperHyphen:
                    if (toLowerCase) {
                        result = s.ToLower();
                    }
                    result = result.Replace(STRING_HYPHEN, STRING_UNDERSCORE);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        private static string ToHyphenCase(string s, WordCase srcCase, bool toLowerCase) {
            string result = s;
            switch (srcCase) {
                case WordCase.LowerCamel:
                case WordCase.UpperCamel:
                    result = CamelCaseToDelimiterCase(s, STRING_HYPHEN, toLowerCase);
                    break;
                case WordCase.LowerUnderscore:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    result = result.Replace(STRING_UNDERSCORE, STRING_HYPHEN);
                    break;
                case WordCase.UpperUnderscore:
                    if (toLowerCase) {
                        result = s.ToLower();
                    }
                    result = result.Replace(STRING_UNDERSCORE, STRING_HYPHEN);
                    break;
                case WordCase.LowerHyphen:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    break;
                case WordCase.UpperHyphen:
                    if (toLowerCase) {
                        result = s.ToLower();
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }
    }
}