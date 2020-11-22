using System.Data;
using System.Data.Common;

namespace Kai.Universal.Db.Fetch {
    
    /// <summary>
    /// the .net DataTable fetch engine
    /// </summary>
    public class DataTableFetch : AbstractFetchHandler {

        private readonly DataTable datas = new DataTable();

        /// <summary>
        /// get result
        /// </summary>
        /// <returns></returns>
        public DataTable GetResult() {
            return datas;
        }

        /// <summary>
        /// load columns
        /// </summary>
        /// <param name="reader"></param>
        protected override void FetchAllColumnInfo(DbDataReader reader) {
            datas.BeginLoadData();
            // load  DataColumn
            for (int i = 0; i < reader.FieldCount; i++) {
                datas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
        }

        /// <summary>
        /// load data
        /// </summary>
        /// <param name="reader"></param>
        protected override void DoProcessDataReader(DbDataReader reader) {
            // load DataRow
            while (reader.Read()) {
                object[] items = new object[reader.FieldCount];
                reader.GetValues(items);
                datas.LoadDataRow(items, true);
            }

            datas.EndLoadData();
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
