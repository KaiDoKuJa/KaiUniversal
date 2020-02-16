using Kai.Universal.Sql.Text;
using Kai.Universal.Text;
using System;

namespace Kai.Universal.Sql.Clause {
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

        public static string GetSqlString(object propValue) {
            return GetSqlString(propValue, false);
        }

        public static string GetSqlString(object propValue, bool useUnicodePrefix) {
            if (propValue is bool) {
                var b = (bool)propValue;
                return b ? "1" : "0";
            } else if (ReflectUtility.IsNumberType(propValue)) {
                return Convert.ToString(propValue);
            } else if (propValue is DateTime dttm) {
                return string.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", dttm.Year, dttm.Month, dttm.Day, dttm.Hour, dttm.Minute, dttm.Second);
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
    }
}
