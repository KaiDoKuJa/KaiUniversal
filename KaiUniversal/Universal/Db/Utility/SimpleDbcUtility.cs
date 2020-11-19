using Kai.Universal.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#if !NET20
using System.Linq;
#endif

namespace Kai.Universal.Db.Utility {

    /// <summary>
    /// Simple dbc utility
    /// </summary>
    public class SimpleDbcUtility {

        private static readonly string NOT_CONNECT = "db is not connect...";
        private static readonly string EXEC_NON_QUERIES_ERR = "row: {0}, err-sql: {1}";

        private SimpleDbcUtility() {
        }

        /// <summary>
        /// get select count number
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
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

        /// <summary>
        /// execute non query (insert/update/delete)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
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

        /// <summary>
        /// execute non query (insert/update/delete)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sqls"></param>
        /// <returns></returns>
        protected static int ExecuteNonQueries(DbCommand command, List<string> sqls) {
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
                    string errMsg = string.Format(EXEC_NON_QUERIES_ERR, n + 1, sqls[n]);
                    throw new InvalidOperationException(errMsg, e);
                } else {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// execute non query (insert/update/delete)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sqls"></param>
        /// <returns></returns>
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

        /// <summary>
        /// execute non query (insert/update/delete)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public static bool ExecuteNonQueries(DbConnection connection, string[] sqls) {
#if !NET20
            return ExecuteNonQueries(connection, sqls.ToList());
#else      
            return ExecuteNonQueries(connection, new List<string>(sqls));
#endif
        }

        /// <summary>
        /// execute rollback command
        /// </summary>
        /// <param name="transaction"></param>
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