using Kai.Universal.Db.Fetch;
using Kai.Universal.Sql.Handler;
using System.Data;
using System.Data.Common;

namespace Kai.Universal.Db.Utility {

    /// <summary>
    /// DataTable dbc utility
    /// </summary>
    public static class DataTableDbcUtility {

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetData(DbConnection connection, int commandTimeout, string sql) {
            DataTableFetch fetch = new DataTableFetch();
            fetch.CommandTimeout = commandTimeout;
            fetch.Execute(connection, sql);
            return fetch.GetResult();
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetData(DbConnection connection, string sql) {
            return GetData(connection, 30, sql);
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static DataTable GetData(DbConnection connection, DmlHandler handler) {
            DataTableFetch fetch = new DataTableFetch();
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }

    }
}