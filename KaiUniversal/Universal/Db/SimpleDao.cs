using Kai.Universal.Data;
using Kai.Universal.Db.Utility;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Kai.Universal.Db {

    /// <summary>
    /// the basic dao
    /// </summary>
    public abstract class SimpleDao {

        /// <summary>
        /// basic datasource
        /// </summary>
        public SimpleDataSource DataSource { get; set; }
        private int defaultCommandTimeout = 30;

        /// <summary>
        /// get select count(1) result
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int GetSelectCount(string sql, int commandTimeout = 30) {
            int count = -1;
            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                count = SimpleDbcUtility.GetSelectCount(connection, sql);
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return count;
        }

#if NET20
        delegate T2 Func<in T, in T1, out T2>(T arg1, T1 arg2);
        delegate T3 Func<in T, in T1, in T2, out T3>(T arg1, T1 arg2, T2 arg3);
#endif

        private TResult DoGetData<TResult>(int commandTimeout, string sql, Func<DbConnection, int, string, TResult> generator) where TResult : class {
            if (sql != null && !"".Equals(sql.Trim())) return null;

            TResult res = null;
            DbConnection connection = null;
            try {
                connection = this.GetConnection();
                connection.Open();
                if (commandTimeout <= 0) {
                    commandTimeout = defaultCommandTimeout;
                }
                res = generator(connection, commandTimeout, sql);
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return res;
        }

        private TResult DoGetData<TResult>(DmlHandler handler, Func<DbConnection, DmlHandler, TResult> generator) where TResult : class {
            TResult res = null;
            DbConnection connection = null;
            try {
                connection = this.GetConnection();
                connection.Open();
                res = generator(connection, handler);
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return res;
        }

        /// <summary>
        /// execute select sql to List of Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public List<T> GetData<T>(string sql, int commandTimeout = 30) where T : new() {
            return DoGetData(commandTimeout, sql, ModelDbcUtility.GetData<T>);
        }

        /// <summary>
        /// execute select sql to List of Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        public List<T> GetData<T>(DmlHandler handler) where T : new() {
            return DoGetData(handler, ModelDbcUtility.GetData<T>);
        }

        /// <summary>
        /// execute select sql to List of Dictionary(key->value)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetMapData(string sql, int commandTimeout = 30) {

            return DoGetData(commandTimeout, sql, MapDataDbcUtility.GetData);
        }

        /// <summary>
        /// execute select sql to List of Dictionary(key->value)
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetMapData(DmlHandler handler) {
            return DoGetData(handler, MapDataDbcUtility.GetData);
        }

        /// <summary>
        /// execute paging sql to List of Dictionary(key->value)
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public PagerData<Dictionary<string, object>> GetPagerMapData(DmlHandler handler, ModelInfo modelInfo) {
            DbConnection connection = this.GetConnection();
            connection.Open();
            PagerData<Dictionary<string, object>> pagerData = MapDataDbcUtility.GetPagerData(connection, handler, modelInfo);
            CloseUtility.CloseConnection(ref connection);
            return pagerData;
        }

        /// <summary>
        /// execute select sql to DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, int commandTimeout = 30) {
            return DoGetData(commandTimeout, sql, DataTableDbcUtility.GetData);
        }

        /// <summary>
        /// execute insert/update/delete
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql) {
            int count = -1;

            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                count = SimpleDbcUtility.ExecuteNonQuery(connection, sql);
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return count;
        }

        /// <summary>
        /// execute insert/update/delete
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExecuteNonQueries(List<string> sqls) {
            bool result = false;

            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                result = SimpleDbcUtility.ExecuteNonQueries(connection, sqls);
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return result;
        }

        /// <summary>
        /// execute insert/update/delete
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExecuteNonQueries(string[] sqls) {
            bool result = false;

            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                result = SimpleDbcUtility.ExecuteNonQueries(connection, sqls);
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return result;
        }

        /// <summary>
        /// the abstract of get connection, override for MSSQL/Oracle/...
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection GetConnection();

        /// <summary>
        /// the default sql timeout is 30
        /// </summary>
        protected int DefaultCommandTimeout {
            get {
                return defaultCommandTimeout;
            }
            set {
                if (value <= 30) {
                    value = 30;
                }
                defaultCommandTimeout = value;
            }
        }
    }
}