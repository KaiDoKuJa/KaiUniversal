using Kai.Universal.Data;
using Kai.Universal.Text;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Db.Fetch {

    /// <summary>
    /// the entity model fetch engine
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelFetch<T> : AbstractFetchHandler where T : new() {

        private List<ColumnInfo> columnInfos;
        private readonly List<T> datas = new List<T>();
        /// <summary>
        /// dml info
        /// </summary>
        public DmlInfo DmlInfo { get; set; }
		
        /// <summary>
        /// get result
        /// </summary>
        /// <returns></returns>
        public List<T> GetResult() {
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
            WordCase mapModelWordCase = WordCase.UpperCamel;

            List<ColumnInfo> columnInfos0 = DataReaderUtility.GetAllColumnInfo(reader, customerMapping, columnWordCase, mapModelWordCase);
            this.columnInfos = ReduceColumn(columnInfos0);
        }

        /// <summary>
        /// load data
        /// </summary>
        /// <param name="reader"></param>
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

        /// <summary>
        /// abandon
        /// </summary>
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
