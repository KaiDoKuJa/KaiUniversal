using Kai.Universal.Util;
using System;
using System.Data;
using System.Data.Common;

namespace Kai.Universal.Db.Fetch {
    /// <summary>
    /// base fetch engine
    /// </summary>
    public abstract class AbstractFetchHandler {

        private static readonly string NOT_CONNECT = "db is not connect...";
        private static readonly string EMPTY_SQL = "sql is empty...";

        /// <summary>
        /// command timeout
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// (abs-method) load column (metadata)
        /// </summary>
        /// <param name="reader"></param>
        protected abstract void FetchAllColumnInfo(DbDataReader reader);

        /// <summary>
        /// (abs-method) load data
        /// </summary>
        /// <param name="reader"></param>
        protected abstract void DoProcessDataReader(DbDataReader reader);

        /// <summary>
        /// when execute cause exception, then calling abandon method.
        /// </summary>
        protected abstract void Abandon();

        /// <summary>
        /// execute sql
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        public void Execute(DbConnection connection, String sql) {
            if (connection == null) throw new ArgumentException(NOT_CONNECT);
            if (sql == null || "".Equals(sql.Trim())) throw new ArgumentException(EMPTY_SQL);

            DbCommand command = null;
            DbDataReader reader = null;

            try {
                command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (CommandTimeout > 0) {
                    command.CommandTimeout = this.CommandTimeout;
                }

                reader = command.ExecuteReader(CommandBehavior.SequentialAccess);

                // load columns
                this.FetchAllColumnInfo(reader);

                // load Data
                this.DoProcessDataReader(reader);

            } catch {
                this.Abandon();
                throw;
            } finally {
                CloseUtility.CloseDataReader(ref reader);
                CloseUtility.DisposeSqlCommmand(ref command);
            }
        }
    }
}
