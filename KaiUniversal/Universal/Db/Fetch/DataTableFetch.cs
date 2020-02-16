using System.Data;
using System.Data.Common;

namespace Kai.Universal.Db.Fetch {

    public class DataTableFetch : AbstractFetchHandler {

        private readonly DataTable datas = new DataTable();

        public DataTable GetResult() {
            return datas;
        }

        protected override void FetchAllColumnInfo(DbDataReader reader) {
            datas.BeginLoadData();
            // load  DataColumn
            for (int i = 0; i < reader.FieldCount; i++) {
                datas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
        }

        protected override void DoProcessDataReader(DbDataReader reader) {
            // load DataRow
            while (reader.Read()) {
                object[] items = new object[reader.FieldCount];
                reader.GetValues(items);
                datas.LoadDataRow(items, true);
            }

            datas.EndLoadData();
        }

        protected override void Abandon() {
            if (datas != null) {
                datas.Clear();
            }
        }

    }
}
