using Kai.Universal.Db.Fetch;
using Kai.Universal.Sql.Handler;
using System.Data;
using System.Data.Common;

namespace Kai.Universal.Db.Utility {

    public static class DataTableDbcUtility {

        public static DataTable GetData(DbConnection connection, int commandTimeout, string sql) {
            DataTableFetch fetch = new DataTableFetch();
            fetch.CommandTimeout = commandTimeout;
            fetch.Execute(connection, sql);
            return fetch.GetResult();
        }

        public static DataTable GetData(DbConnection connection, string sql) {
            return GetData(connection, 30, sql);
        }

        public static DataTable GetData(DbConnection connection, DmlHandler handler) {
            DataTableFetch fetch = new DataTableFetch();
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }

    }
}