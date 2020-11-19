using Kai.Universal.Data;
using Kai.Universal.Db.Constant;
using Kai.Universal.Db.Fetch;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Text;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Db.Utility {

    public static class ModelDbcUtility {
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

        public static List<T> GetData<T>(DbConnection connection, string sql) where T : new() {
            return GetData<T>(connection, DbConstant.DEFAULT_COMMAND_TIMEOUT, sql);
        }

        public static List<T> GetData<T>(DbConnection connection, DmlHandler handler) where T : new() {
            ModelFetch<T> fetch = new ModelFetch<T>();
            fetch.DmlInfo = handler.Clause.DmlInfo;
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }


    }
}