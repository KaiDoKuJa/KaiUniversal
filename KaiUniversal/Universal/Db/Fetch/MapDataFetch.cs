using System;
using System.Collections.Generic;
using System.Data.Common;
using Kai.Universal.Text;
using Kai.Universal.Data;

namespace Kai.Universal.Db.Fetch {

    public class MapDataFetch : AbstractFetchHandler {

        private List<ColumnInfo> columnInfos;
        private readonly List<Dictionary<string, object>> datas = new List<Dictionary<string, object>>();
        public DmlInfo DmlInfo { get; set; }

        public List<Dictionary<string, object>> GetResult() {
            return datas;
        }

        protected override void FetchAllColumnInfo(DbDataReader reader) {
            if (DmlInfo == null) DmlInfo = new DmlInfo();
            Dictionary<string, string> customerMapping = DmlInfo.CustomerMapping;
            WordCase columnWordCase = DmlInfo.ColumnWordCase;
            WordCase mapModelWordCase = DmlInfo.MapModelWordCase;

            this.columnInfos = DataReaderUtility.GetAllColumnInfo(reader, customerMapping, columnWordCase, mapModelWordCase);
        }

        protected override void DoProcessDataReader(DbDataReader reader) {
            while (reader.Read()) {
                Dictionary<string, object> map = new Dictionary<string, object>();
                foreach (ColumnInfo columnInfo in columnInfos) {
                    String colName = columnInfo.ColName;
                    map.Add(colName, reader[colName]);
                }
                datas.Add(map);
            }
        }

        protected override void Abandon() {
            if (datas != null) {
                datas.Clear();
            }
        }

    }
}
