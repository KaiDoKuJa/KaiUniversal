using System;
using System.Collections.Generic;
using System.Data.Common;
using Kai.Universal.Text;
using Kai.Universal.Data;

namespace Kai.Universal.Db.Fetch {

    public class ModelFetch<T> : AbstractFetchHandler where T : new() {

        private List<ColumnInfo> columnInfos;
        private List<T> datas = new List<T>();
        public DmlInfo DmlInfo { get; set; }

        public List<T> GetResult() {
            return datas;
        }

        protected override void FetchAllColumnInfo(DbDataReader reader) {
            if (DmlInfo == null) DmlInfo = new DmlInfo();
            Dictionary<string, string> customerMapping = DmlInfo.CustomerMapping;
            WordCase columnWordCase = DmlInfo.ColumnWordCase;
            WordCase mapModelWordCase = DmlInfo.MapModelWordCase;

            List<ColumnInfo> columnInfos = DataReaderUtility.GetAllColumnInfo(reader, customerMapping, columnWordCase, mapModelWordCase);
            this.columnInfos = ReduceColumn(columnInfos);
        }

        protected override void DoProcessDataReader(DbDataReader reader) {
            while (reader.Read()) {
                T t = new T();
                foreach (ColumnInfo columnInfo in columnInfos) {
                    string colName = columnInfo.ColName;
                    Type colType = columnInfo.ColType;
                    object val = reader[colName];
                    ReflectUtility.SetValue(t, columnInfo.ModelName, val);
                }
                datas.Add(t);
            }

        }

        protected override void Abandon() {
            if (datas != null) {
                datas.Clear();
            }
        }

        private List<ColumnInfo> ReduceColumn(List<ColumnInfo> columnInfos) {
            List<ColumnInfo> reducedColumns = new List<ColumnInfo>();
            List<ColumnInfo> removeItems = new List<ColumnInfo>();
            foreach (ColumnInfo columnInfo in columnInfos) {
                try {
                    if (ReflectUtility.HasVariable(columnInfo.ColType, columnInfo.ModelName)) {
                        reducedColumns.Add(columnInfo);
                    }
                } catch { }
            }
            return reducedColumns;
        }

    }
}
