using Kai.Universal.Sql.Text;
using Kai.Universal.Text;
using System;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    /// <summary>
    /// the ORM utility
    /// </summary>
    public class OrmUtility {
        private OrmUtility() { }

        internal static bool IsArrayEmpty(string[] arrayString) {
            return (arrayString == null || arrayString.Length <= 0);
        }

        internal static bool IsStringInArray(string key, string[] array) {
            bool result = false;
            foreach (string checkString in array) {
                if (key.Equals(checkString)) {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// property value to sql string
        /// </summary>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public static string GetSqlString(object propValue) {
            return GetSqlString(propValue, false);
        }

        /// <summary>
        /// property value to sql string
        /// </summary>
        /// <param name="propValue"></param>
        /// <param name="useUnicodePrefix"></param>
        /// <returns></returns>
        public static string GetSqlString(object propValue, bool useUnicodePrefix) {
            if (propValue is bool) {
                var b = (bool)propValue;
                return b ? "1" : "0";
            } else if (ReflectUtility.IsNumberType(propValue)) {
                return Convert.ToString(propValue);
            } else if (propValue is DateTime dttm) {
                return string.Format("'{0}'", dttm.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            } else if (propValue is SpecialString speicalString) {
                return speicalString.Value;
            } else if (propValue is string s) {
                s = s.Replace("'", "''");
                return useUnicodePrefix ? string.Format("N'{0}'", s) : string.Format("'{0}'", s);
            } else {
                // no support date, because the date format too much kind, plz use string!
                return string.Format("'{0}'", propValue.ToString());
            }
        }

        /// <summary>
        /// property values to sql string
        /// <para>ex: new string[] {"a","b"} => 'a','b'</para>
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static string GetArraySqlString(object[] vals) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < vals.Length; i++) {
                if (i > 0) {
                    sb.Append(",");
                }
                sb.Append(OrmUtility.GetSqlString(vals[i]));
            }
            return sb.ToString();
        }
    }
}
