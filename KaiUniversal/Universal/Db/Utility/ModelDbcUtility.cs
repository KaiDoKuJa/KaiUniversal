using Kai.Universal.Data;
using Kai.Universal.Db.Constant;
using Kai.Universal.Db.Fetch;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Text;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Db.Utility {

    /// <summary>
    /// Entity Model dbc utility
    /// </summary>
    public static class ModelDbcUtility {

        /// <summary>
        /// get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> GetData<T>(DbConnection connection, int commandTimeout, string sql) where T : new() {
            DmlInfo dmlInfo = new DmlInfo();
            dmlInfo.ColumnWordCase = WordCase.UpperUnderscore;
            dmlInfo.MapModelWordCase = WordCase.LowerCamel;

            ModelFetch<T> fetch = new ModelFetch<T>();
            fetch.CommandTimeout = commandTimeout;
            fetch.DmlInfo = dmlInfo;
            fetch.Execute(connection, sql);
            return fetch.GetResult();
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> GetData<T>(DbConnection connection, string sql) where T : new() {
            return GetData<T>(connection, DbConstant.DEFAULT_COMMAND_TIMEOUT, sql);
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static List<T> GetData<T>(DbConnection connection, DmlHandler handler) where T : new() {
            ModelFetch<T> fetch = new ModelFetch<T>();
            fetch.DmlInfo = handler.Clause.DmlInfo;
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }


    }
}