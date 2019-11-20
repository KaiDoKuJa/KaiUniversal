using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Kai.Universal.Text;
using Kai.Universal.Data;

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
            sb = new StringBuilder();
            sb.Append("[");
            int j = 0;
            while (reader.Read()) { // j++;
                if (j > 0) sb.Append(","); //太亂了，沒Stringjoiner很難寫
                sb.Append("{");
                for (int i = 0; i < columnInfos.Count; i++) {
                    if (i > 0) sb.Append(","); // ?? TODO :需驗證
                    ColumnInfo columnInfo = columnInfos[i];
                    String colName = columnInfo.ColName;
                    Type colType = columnInfo.ColType;
                    object val = reader[colName];
                    // 這個尚未翻譯
                    // TODO :                 String val = ResultSetUtility.getJsonString(resultSet, colType, colName);
                }
                sb.Append("}");
            }
            sb.Append("]");
        }

        protected override void Abandon() {
            // do nothing
        }

    }
}
