using Kai.Universal.Data;
//#if NETSTANDARD2_0

//#else 
//using System.Web.Script.Serialization;
//#endif
using Kai.Universal.Text;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Kai.Universal.Db.Fetch {

    public class JsonFetch : AbstractFetchHandler {

        private StringBuilder sb = new StringBuilder();
        private List<ColumnInfo> columnInfos;
        public DmlInfo DmlInfo { get; set; }

        public string GetResult() {
            return sb.ToString();
        }

        protected override void FetchAllColumnInfo(DbDataReader reader) {
            if (DmlInfo == null) DmlInfo = new DmlInfo();
            Dictionary<string, string> customerMapping = DmlInfo.CustomerMapping;
            WordCase columnWordCase = DmlInfo.ColumnWordCase;
            WordCase mapModelWordCase = DmlInfo.MapModelWordCase;

            this.columnInfos = DataReaderUtility.GetAllColumnInfo(reader, customerMapping, columnWordCase, mapModelWordCase);
        }

        protected override void DoProcessDataReader(DbDataReader reader) {
#if NETSTANDARD2_0
#else
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
#endif
            sb = new StringBuilder();
            sb.Append("[");
            int j = 0;
            while (reader.Read()) {
                if (j > 0) sb.Append(",");
                sb.Append("{");
                for (int i = 0; i < columnInfos.Count; i++) {
                    if (i > 0) sb.Append(",");
                    ColumnInfo columnInfo = columnInfos[i];
                    string colName = columnInfo.ColName;
                    Type colType = columnInfo.ColType;
                    string val = GetJsonString(reader[colName], colType);
                    sb.Append(string.Format("\"{0}\":{1}", columnInfo.ModelName, val));
                }
                sb.Append("}");
                j++;
            }
            sb.Append("]");
        }

        protected override void Abandon() {
            // do nothing
        }

        public static string GetJsonString(object val, Type valueType) {
            string jsonStringTemplate = "\"{0}\"";
            if (valueType == typeof(bool)) {
                if ((bool)val == true) {
                    return "true";
                }
                return "false";
            } else if (ReflectUtility.IsNumberType(val)) {
                return val.ToString();
            } else if (val is byte[]) { // TODO : is all of blob~binary
                // JSON spec is not hex words
                return string.Format(jsonStringTemplate, HexUtility.BytesToHex((byte[])val));
            } else { // clob can use this?
                return string.Format(jsonStringTemplate, EscapeJsonQuote(val.ToString()));
            }
            // TODO : datetime 

        }

        private static string EscapeJsonQuote(string s) {
            if (s == null) return "";
            if (!s.Contains("\"")) {
                return s;
            } else {
                return s.Replace("\"", "\\\"");
            }
        }

    }
}
