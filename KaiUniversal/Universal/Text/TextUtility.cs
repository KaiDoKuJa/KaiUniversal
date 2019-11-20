using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kai.Universal.Text {

    public class TextUtility {

        public static readonly string STRING_HYPHEN = "-";
        public static readonly string STRING_UNDERSCORE = "_";

        private TextUtility() { }

        public static string ToString(DateTime dateTime, string type) {
            string result = "";
            switch (type) {
                case "DATE":
                    result = String.Format("{0}-{1:00}-{2:00}", dateTime.Year, dateTime.Month, dateTime.Day);
                    break;

                case "DTTM":
                    result = String.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                    break;

                case "YYYYMMDD":
                    result = String.Format("{0}{1:00}{2:00}", dateTime.Year, dateTime.Month, dateTime.Day);
                    break;

                default:
                    break;
            }
            return result;
        }

        public static string ToDefaultString(string source, string defaultValue) {
            if (source == null || source == "" || source.Trim() == "") {
                return defaultValue;
            }
            return source;
        }

        //            //Regex.Replace(s, @"(?<!_)([A-Z])", "_$1"); 
        //            //Regex.Replace(s, @"(?<=[a-z])([A-Z])", @"_$1");

        //            Regex upperCaseRegex = new Regex(@"[A-Z]{1}[a-z]*");
        //// TODO : Pattern upperCaseRegex = Pattern.compile("(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])");
        ////        Here is how the improved regex splits the example data:
        ////
        ////        value -> value
        ////        camelValue -> camel / Value
        ////        TitleValue -> Title / Value
        ////        VALUE -> VALUE
        ////        eclipseRCPExt -> eclipse / RCP / Ext
        //            string[] words = upperCaseRegex.Matches(caseString).Cast<Match>().Select(m => m.Value).ToArray();


        private static string CamelCaseToDelimiterCase(string s, string delimiter, bool toLowerCase) {
            string caseString = Char.ToUpper(s[0]) + s.Substring(1);

            Regex upperCaseRegex = new Regex(@"[A-Z]{1}[a-z]*");
            //TODO : check -->
            //Pattern upperCaseRegex = Pattern.compile("(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])");
            //string[] words = upperCaseRegex.Split(caseString.ToCharArray());
            string[] words = upperCaseRegex.Matches(caseString).Cast<Match>().Select(m => m.Value).ToArray();

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

        public static string ConvertWordCase(string s, WordCase srcCase, WordCase outCase) {
            if (s == null || "".Equals(s)) {
                return "";
            }
            if (s.Length == 1) {
                return ConvertCharCase(s[0], outCase).ToString();
            }
            if (srcCase == outCase) {
                return s;
            }

            switch (outCase) {
                case WordCase.LOWER_CAMEL:
                    return TextUtility.ToCamelCase(s, srcCase, true);
                case WordCase.UPPER_CAMEL:
                    return TextUtility.ToCamelCase(s, srcCase, false);
                case WordCase.LOWER_UNDERSCORE:
                    return TextUtility.ToUnderscoreCase(s, srcCase, true);
                case WordCase.UPPER_UNDERSCORE:
                    return TextUtility.ToUnderscoreCase(s, srcCase, false);
                case WordCase.LOWER_HYPHEN:
                    return TextUtility.ToHyphenCase(s, srcCase, true);
                case WordCase.UPPER_HYPHEN:
                    return TextUtility.ToHyphenCase(s, srcCase, false);

                default:
                    throw new NotSupportedException();
            }

        }

        public static char ConvertCharCase(char c, WordCase outCase) {
            switch (outCase) {
                case WordCase.LOWER_CAMEL:
                case WordCase.LOWER_UNDERSCORE:
                case WordCase.LOWER_HYPHEN:
                    return Char.ToLower(c);
                case WordCase.UPPER_CAMEL:
                case WordCase.UPPER_UNDERSCORE:
                case WordCase.UPPER_HYPHEN:
                    return Char.ToUpper(c);
                default:
                    throw new NotSupportedException();
            }
        }

        private static string ToCamelCase(string s, WordCase srcCase, bool toLowerCamel) {
            string result = s;
            switch (srcCase) {
                case WordCase.LOWER_CAMEL:
                    if (!toLowerCamel) {
                        result = Char.ToUpper(s[0]) + s.Substring(1);
                    }
                    break;
                case WordCase.UPPER_CAMEL:
                    if (toLowerCamel) {
                        result = Char.ToLower(s[0]) + s.Substring(1);
                    }
                    break;
                case WordCase.LOWER_UNDERSCORE:
                case WordCase.UPPER_UNDERSCORE:
                    result = DelimiterCaseToCamelCase(s, STRING_UNDERSCORE, toLowerCamel);
                    break;
                case WordCase.LOWER_HYPHEN:
                case WordCase.UPPER_HYPHEN:
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
                case WordCase.LOWER_CAMEL:
                case WordCase.UPPER_CAMEL:
                    result = CamelCaseToDelimiterCase(s, STRING_UNDERSCORE, toLowerCase);
                    break;
                case WordCase.LOWER_UNDERSCORE:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    break;
                case WordCase.UPPER_UNDERSCORE:
                    if (toLowerCase) {
                        result = s.ToLower();
                    }
                    break;
                case WordCase.LOWER_HYPHEN:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    result = result.Replace(STRING_HYPHEN, STRING_UNDERSCORE);
                    break;
                case WordCase.UPPER_HYPHEN:
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
                case WordCase.LOWER_CAMEL:
                case WordCase.UPPER_CAMEL:
                    result = CamelCaseToDelimiterCase(s, STRING_HYPHEN, toLowerCase);
                    break;
                case WordCase.LOWER_UNDERSCORE:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    result = result.Replace(STRING_UNDERSCORE, STRING_HYPHEN);
                    break;
                case WordCase.UPPER_UNDERSCORE:
                    if (toLowerCase) {
                        result = s.ToLower();
                    }
                    result = result.Replace(STRING_UNDERSCORE, STRING_HYPHEN);
                    break;
                case WordCase.LOWER_HYPHEN:
                    if (!toLowerCase) {
                        result = s.ToUpper();
                    }
                    break;
                case WordCase.UPPER_HYPHEN:
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