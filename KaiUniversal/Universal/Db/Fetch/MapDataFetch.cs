using Kai.Universal.Data;
using Kai.Universal.Text;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Db.Fetch {

    /// <summary>
    /// the map data fetch engine
    /// </summary>
    public class MapDataFetch : AbstractFetchHandler {

        private List<ColumnInfo> columnInfos;
        private readonly List<Dictionary<string, object>> datas = new List<Dictionary<string, object>>();
        /// <summary>
        /// dml info
        /// </summary>
        public DmlInfo DmlInfo { get; set; }

        /// <summary>
        /// get result
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetResult() {
            return datas;
        }

        /// <summary>
        /// load columns
        /// </summary>
        /// <param name="reader"></param>
        protected override void FetchAllColumnInfo(DbDataReader reader) {
            if (DmlInfo == null) DmlInfo = new DmlInfo();
            Dictionary<string, string> customerMapping = DmlInfo.CustomerMapping;
            WordCase columnWordCase = DmlInfo.ColumnWordCase;
            WordCase mapModelWordCase = DmlInfo.MapModelWordCase;

            this.columnInfos = DataReaderUtility.GetAllColumnInfo(reader, customerMapping, columnWordCase, mapModelWordCase);
        }

        /// <summary>
        /// load data
        /// </summary>
        /// <param name="reader"></param>
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

        /// <summary>
        /// abandon
        /// </summary>
        protected override void Abandon() {
            if (datas != null) {
                datas.Clear();
            }
        }

    }
}
