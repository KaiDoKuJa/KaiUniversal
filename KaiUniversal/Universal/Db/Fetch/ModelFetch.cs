using Kai.Universal.Data;
using Kai.Universal.Text;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Db.Fetch {

    public class ModelFetch<T> : AbstractFetchHandler where T : new() {

        private List<ColumnInfo> columnInfos;
        private readonly List<T> datas = new List<T>();
        public DmlInfo DmlInfo { get; set; }

        public List<T> GetResult() {
            return datas;
        }

        protected override void FetchAllColumnInfo(DbDataReader reader) {
            if (DmlInfo == null) DmlInfo = new DmlInfo();
            Dictionary<string, string> customerMapping = DmlInfo.CustomerMapping;
            WordCase columnWordCase = DmlInfo.ColumnWordCase;
            WordCase mapModelWordCase = WordCase.UpperCamel;

            List<ColumnInfo> columnInfos0 = DataReaderUtility.GetAllColumnInfo(reader, customerMapping, columnWordCase, mapModelWordCase);
            this.columnInfos = ReduceColumn(columnInfos0);
        }

        protected override void DoProcessDataReader(DbDataReader reader) {
            while (reader.Read()) {
                T t = new T();
                foreach (ColumnInfo columnInfo in columnInfos) {
                    string colName = columnInfo.ColName;
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
            foreach (ColumnInfo columnInfo in columnInfos) {
                if (ReflectUtility.HasVariable(typeof(T), columnInfo.ModelName, columnInfo.ColType)) {
                    reducedColumns.Add(columnInfo);
                }
            }
            return reducedColumns;
        }

    }
}
