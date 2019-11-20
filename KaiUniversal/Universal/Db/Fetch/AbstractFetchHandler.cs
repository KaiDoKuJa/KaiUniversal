using Kai.Universal.Data;
using Kai.Universal.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Kai.Universal.Db.Fetch {
    public abstract class AbstractFetchHandler {

        private static readonly string NOT_CONNECT = "db is not connect...";
        private static readonly string EMPTY_SQL = "sql is empty...";

        public int CommandTimeout { get; set; }

        protected abstract void FetchAllColumnInfo(DbDataReader reader);

        protected abstract void DoProcessDataReader(DbDataReader reader);

        /**
         * when execute cause exception, then calling abandon method.
         */
        protected abstract void Abandon();

        public void Execute(DbConnection connection, String sql) {
            if (connection == null) throw new ArgumentException(NOT_CONNECT);
            if (sql == null || "".Equals(sql.Trim())) throw new ArgumentException(EMPTY_SQL);

            DbCommand command = null;
            DbDataReader reader = null;

            try {
                command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (CommandTimeout != 0) {
                    command.CommandTimeout = this.CommandTimeout;
                }

                reader = command.ExecuteReader(CommandBehavior.SequentialAccess);

                // load columns
                this.FetchAllColumnInfo(reader);

                // load Data
                this.DoProcessDataReader(reader);

            } catch (Exception e) {
                this.Abandon();
                throw e;
            } finally {
                CloseUtility.CloseDataReader(ref reader);
                CloseUtility.DisposeSqlCommmand(ref command);
            }
        }
    }
}
