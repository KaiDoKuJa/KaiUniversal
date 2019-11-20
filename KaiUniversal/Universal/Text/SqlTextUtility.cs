using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kai.Universal.Text {
    public class SqlTextUtility {

        public static string GetPropertyString(object refVal) {
            string result = null;

            if (refVal is Boolean) {
                bool b = (bool)refVal;
                if (b) {
                    result = "1";
                } else {
                    result = "0";
                }
            } else if (refVal is DateTime) {
                DateTime dttm = (DateTime)refVal;
                result = String.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", dttm.Year, dttm.Month, dttm.Day, dttm.Hour, dttm.Minute, dttm.Second);
            } else if (refVal is String) {
                result = refVal.ToString();
            } else {
                if (refVal != null) {
                    result = refVal.ToString();
                }
            }

            return result;
        }
    }
}