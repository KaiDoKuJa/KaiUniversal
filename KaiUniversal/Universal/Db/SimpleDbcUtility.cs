using Kai.Universal.Data;
using Kai.Universal.Db.Fetch;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using Kai.Universal.Text;
using Kai.Universal.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#if !NET20
using System.Linq;
#endif

namespace Kai.Universal.Db {

    public class SimpleDbcUtility {

        private static readonly int DEFAULT_COMMAND_TIMEOUT = 30;
        private static readonly string NOT_CONNECT = "db is not connect...";
        private static readonly string EXEC_NON_QUERIES_ERR = "row: {0}, err-sql: {1}";

        private SimpleDbcUtility() {
        }

        public static int GetSelectCount(DbConnection connection, string sql) {
            int count = -1;

            if (connection == null) {
                throw new ArgumentException("connection", NOT_CONNECT);
            }

            DbCommand command = null;
            try {
                command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                object result = command.ExecuteScalar();
                if (result != null) {
                    count = Int32.Parse(result.ToString());
                }
            } finally {
                CloseUtility.DisposeSqlCommmand(ref command);
            }
            return count;
        }

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
            return GetData<T>(connection, DEFAULT_COMMAND_TIMEOUT, sql);
        }

        public static List<T> GetData<T>(DbConnection connection, DmlHandler handler) where T : new() {
            ModelFetch<T> fetch = new ModelFetch<T>();
            fetch.DmlInfo = handler.Clause.DmlInfo;
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }

        public static List<Dictionary<string, object>> GetMapData(DbConnection connection, int commandTimeout, String sql) {
            MapDataFetch fetch = new MapDataFetch();
            fetch.CommandTimeout = commandTimeout;
            fetch.Execute(connection, sql);
            return fetch.GetResult();
        }

        public static List<Dictionary<string, object>> GetMapData(DbConnection connection, String sql) {
            return GetMapData(connection, DEFAULT_COMMAND_TIMEOUT, sql);
        }

        public static List<Dictionary<string, object>> GetMapData(DbConnection connection, DmlHandler handler) {
            MapDataFetch fetch = new MapDataFetch();
            fetch.DmlInfo = handler.Clause.DmlInfo;
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }

        public static PagerData<Dictionary<string, object>> GetPagerMapData(DbConnection connection, DmlHandler handler, ModelInfo modelInfo) {
            PagerData<Dictionary<string, object>> pagerData = new PagerData<Dictionary<string, object>>();
            // select count
            int totalCount = GetSelectCount(connection, handler.GetSql(QueryType.SelectCnt, modelInfo));
            pagerData.SelectCount = totalCount;
            if (totalCount > 0) {
                // select paging sql
                handler.GetSql(QueryType.SelectPaging, modelInfo);
                List<Dictionary<string, object>> datas = GetMapData(connection, handler);
                pagerData.Datas = datas;
                // other info
                pagerData.PageNumber = modelInfo.PageNumber;
                pagerData.EachPageSize = modelInfo.EachPageSize;
            } else {
                pagerData.PageNumber = 0;
                pagerData.EachPageSize = 0;
            }
            return pagerData;
        }

        public static string GetJsonData(DbConnection connection, int commandTimeout, string sql) {
            JsonFetch fetch = new JsonFetch();
            fetch.CommandTimeout = commandTimeout;
            fetch.Execute(connection, sql);
            return fetch.GetResult();
        }

        public static string GetJsonData(DbConnection connection, string sql) {
            return GetJsonData(connection, DEFAULT_COMMAND_TIMEOUT, sql);
        }

        public static string GetJsonData(DbConnection connection, DmlHandler handler) {
            JsonFetch fetch = new JsonFetch();
            fetch.DmlInfo = handler.Clause.DmlInfo;
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }

        public static DataTable GetDataTable(DbConnection connection, int commandTimeout, string sql) {
            DataTableFetch fetch = new DataTableFetch();
            fetch.CommandTimeout = commandTimeout;
            fetch.Execute(connection, sql);
            return fetch.GetResult();
        }

        public static DataTable GetDataTable(DbConnection connection, string sql) {
            return GetDataTable(connection, 30, sql);
        }

        public static DataTable GetDataTable(DbConnection connection, DmlHandler handler) {
            DataTableFetch fetch = new DataTableFetch();
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }

        public static int ExecuteNonQuery(DbConnection connection, string sql) {
            int count = -1;

            if (connection == null) {
                throw new ArgumentException(NOT_CONNECT);
            }

            DbTransaction transaction = null;
            DbCommand command = null;
            try {
                transaction = connection.BeginTransaction();

                command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.Transaction = transaction;

                count = command.ExecuteNonQuery();

                transaction.Commit();
            } catch {
                RollbackTransaction(ref transaction);
                throw;
            } finally {
                CloseUtility.DisposeSqlCommmand(ref command);
            }
            return count;
        }

        public static int ExecuteNonQueries(DbCommand command, List<String> sqls) {
            int result = 0;
            try {
                foreach (string sql in sqls) {
                    if (sql == null || "".Equals(sql.Trim())) {
                        continue;
                    }
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    result++;
                }
            } catch (Exception e) {
                int n = result;
                if (n > 0) {
                    string errMsg = String.Format(EXEC_NON_QUERIES_ERR, n + 1, sqls[n]);
                    throw new InvalidOperationException(errMsg, e);
                } else {
                    throw;
                }
            }
            return result;
        }

        public static bool ExecuteNonQueries(DbConnection connection, List<string> sqls) {
            bool result = false;

            if (connection == null) {
                throw new ArgumentNullException("connection", NOT_CONNECT);
            }

            DbTransaction transaction = null;
            DbCommand command = null;
            try {
                transaction = connection.BeginTransaction();

                command = connection.CreateCommand();
                command.Transaction = transaction;
                ExecuteNonQueries(command, sqls);

                transaction.Commit();
                result = true;
            } catch {
                RollbackTransaction(ref transaction);
                throw;
            } finally {
                CloseUtility.DisposeSqlCommmand(ref command);
            }
            return result;
        }

        public static bool ExecuteNonQueries(DbConnection connection, string[] sqls) {
#if !NET20
            return ExecuteNonQueries(connection, sqls.ToList());
#else      
            return ExecuteNonQueries(connection, new List<string>(sqls));
#endif
        }

        public static void RollbackTransaction(ref DbTransaction transaction) {
            try {
                if (transaction != null) {
                    transaction.Rollback();
                }
            } catch {
                // do nothing
            } finally {
                transaction = null;
            }
        }


    }
}