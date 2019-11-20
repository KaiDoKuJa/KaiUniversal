using Kai.Universal.Text;
using Kai.Universal.Sql.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    public class OrmUtility {
        private OrmUtility() { }

        internal static bool IsArrayEmpty(string[] arrayString) {
            return (arrayString == null || arrayString.Length <= 0) ? true : false;
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
            if (propValue is Boolean) {
                bool b = (bool)propValue;
                return b ? "1" : "0";
            } else if (ReflectUtility.IsNumberType(propValue)) {
                return Convert.ToString(propValue);
            } else if (propValue is DateTime dttm) {
                return String.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", dttm.Year, dttm.Month, dttm.Day, dttm.Hour, dttm.Minute, dttm.Second);
// java ->                return String.format("'%s'", DateTimeUtility.getString((Date)propValue, DateTimeUtility.DTTM));
            } else if (propValue is SpecialString) {
                return ((SpecialString)propValue).Value;
            } else if (propValue is String s) {
                s = s.Replace("'", "''");
                return useUnicodePrefix ? String.Format("N'{0}'", s) : String.Format("'{0}'", s);
            } else {
                // no support date, because the date format too much kind, plz use string!
                return String.Format("'{0}'", propValue.ToString());
            }
        }
    }
}
