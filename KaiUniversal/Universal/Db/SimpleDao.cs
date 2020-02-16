using Kai.Universal.Data;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Kai.Universal.Db {

    public abstract class SimpleDao : IDao {

        public SimpleDataSource DataSource { get; set; }
        private int defaultCommandTimeout = 30;

        public int GetSelectCount(string sql) {
            int count = -1;
            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                count = SimpleDbcUtility.GetSelectCount(connection, sql);
            } catch {
                throw;
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return count;
        }

        public List<T> GetData<T>(int commandTimeout, string sql) where T : new() {
            List<T> res = null;
            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                res = SimpleDbcUtility.GetData0<T>(connection, commandTimeout, sql);
            } catch {
                throw;
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return res;
        }

        public List<T> GetData<T>(string sql) where T : new() {
            return GetData<T>(defaultCommandTimeout, sql);
        }

        public List<Dictionary<string, object>> GetMapData(int commandTimeout, string sql) {
            List<Dictionary<string, object>> res = null;
            DbConnection connection = this.GetConnection();
            connection.Open();
            res = SimpleDbcUtility.GetMapData(connection, commandTimeout, sql);
            CloseUtility.CloseConnection(ref connection);
            return res;
        }

        public List<Dictionary<string, object>> GetMapData(string sql) {
            return GetMapData(defaultCommandTimeout, sql);
        }

        public List<Dictionary<string, object>> GetMapData(DmlHandler handler) {
            List<Dictionary<string, object>> res = null;
            DbConnection connection = this.GetConnection();
            connection.Open();
            res = SimpleDbcUtility.GetMapData(connection, handler);
            CloseUtility.CloseConnection(ref connection);
            return res;
        }

        public PagerData<Dictionary<string, object>> GetPagerMapData(DmlHandler handler, ModelInfo modelInfo) {
            DbConnection connection = this.GetConnection();
            connection.Open();
            PagerData<Dictionary<string, object>> pagerData = SimpleDbcUtility.GetPagerMapData(connection, handler, modelInfo);
            CloseUtility.CloseConnection(ref connection);
            return pagerData;
        }

        public DataTable GetDataTable(int commandTimeout, string sql) {
            DataTable dt = null;
            DbConnection connection = null;

            if (sql != null && !"".Equals(sql.Trim())) {
                try {
                    connection = this.GetConnection();
                    connection.Open();
                    dt = SimpleDbcUtility.GetDataTable(connection, commandTimeout, sql);
                } catch {
                    throw;
                } finally {
                    CloseUtility.CloseConnection(ref connection);
                }
            }

            return dt;
        }

        public DataTable GetDataTable(string sql) {
            return GetDataTable(defaultCommandTimeout, sql);
        }

        public int ExecuteNonQuery(string sql) {
            int count = -1;

            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                count = SimpleDbcUtility.ExecuteNonQuery(connection, sql);
            } catch {
                throw;
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return count;
        }

        public bool ExecuteNonQueries(List<string> sqls) {
            bool result = false;

            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                result = SimpleDbcUtility.ExecuteNonQueries(connection, sqls);
            } catch {
                throw;
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return result;
        }

        public bool ExecuteNonQueries(string[] sqls) {
            bool result = false;

            DbConnection connection = null;

            try {
                connection = this.GetConnection();
                connection.Open();
                result = SimpleDbcUtility.ExecuteNonQueries(connection, sqls);
            } catch {
                throw;
            } finally {
                CloseUtility.CloseConnection(ref connection);
            }

            return result;
        }

        public abstract DbConnection GetConnection();

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