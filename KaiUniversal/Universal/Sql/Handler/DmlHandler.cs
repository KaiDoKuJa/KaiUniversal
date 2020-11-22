using Kai.Universal.Data;
using Kai.Universal.Sql.Clause;
using Kai.Universal.Sql.Clause.Dialect;
using Kai.Universal.Sql.Type;
using System.Threading;

namespace Kai.Universal.Sql.Handler {
    /// <summary>
    /// The DmlHandler for SqlClause
    /// </summary>
    public class DmlHandler {

        private readonly object _locker = new object();
        /// <summary>
        /// The clause
        /// </summary>
        public AbstractSqlClause Clause { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public DmlHandler() { }
        /// <summary>
        /// constructor with clause
        /// </summary>
        /// <param name="clause"></param>
        public DmlHandler(AbstractSqlClause clause) {
            this.Clause = clause;
        }

        /// <summary>
        /// create DmlHandler by dmlInfo
        /// </summary>
        /// <param name="dmlInfo"></param>
        /// <returns></returns>
        public static DmlHandler CreateHandler(DmlInfo dmlInfo) {
            return CreateHandler(DbmsType.Default, dmlInfo);
        }

        /// <summary>
        /// create DmlHandler by dmlInfo
        /// </summary>
        /// <param name="dbmsType"></param>
        /// <param name="dmlInfo"></param>
        /// <returns></returns>
        public static DmlHandler CreateHandler(DbmsType dbmsType, DmlInfo dmlInfo) {
            AbstractSqlClause clause = CreateClause(dbmsType, dmlInfo);
            return new DmlHandler(clause);
        }

        /// <summary>
        /// create clause by dmlInfo and dbms type
        /// </summary>
        /// <param name="dbmsType"></param>
        /// <param name="dmlInfo"></param>
        /// <returns></returns>
        private static AbstractSqlClause CreateClause(DbmsType dbmsType, DmlInfo dmlInfo) {
            AbstractSqlClause clause;
            switch (dmlInfo.DmlType) {
                case DmlType.Select:
                    switch (dbmsType) {
                        case DbmsType.FromSqlServer2005:
                        case DbmsType.FromSqlServer2012:
                            clause = new SqlServerClause();
                            break;
                        default:
                            clause = new QueryClause();
                            break;
                    }
                    break;
                case DmlType.Insert:
                    clause = new InsertClause();
                    break;
                case DmlType.Update:
                    clause = new UpdateClause();
                    break;
                case DmlType.Delete:
                    clause = new DeleteClause();
                    break;
                default:
                    return null;
            }
            clause.DmlInfo = dmlInfo;
            clause.DbmsType = dbmsType;
            return clause;
        }

        /// <summary>
        /// Get generated sql, calling Clause.GetSql by Monitor.Enter lock
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public string GetSql(ModelInfo modelInfo) {
            bool __isLocked = false;
            try {
#if NETSTANDARD2_0
                Monitor.Enter(_locker, ref __isLocked);
#else
                __isLocked = true;
                Monitor.Enter(_locker);
#endif
                if (modelInfo == null) return Clause.GetSql(null);
                return Clause.GetSql(modelInfo);
            } finally {
                if (__isLocked) Monitor.Exit(_locker);
            }
        }

        /// <summary>
        /// Get generated sql, calling Clause.GetSql by Monitor.Enter lock
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public string GetSql(QueryType queryType, ModelInfo modelInfo) {
            bool __isLocked = false;
            try {
#if NETSTANDARD2_0
                Monitor.Enter(_locker, ref __isLocked);
#else
                __isLocked = true;
                Monitor.Enter(_locker);
#endif
                if (modelInfo == null) return Clause.GetSql(null);

                var limitingResultClause = Clause as ILimitingResultClause;
                if (limitingResultClause != null) {
                    switch (queryType) {
                        case QueryType.SelectPaging:
                            return limitingResultClause.GetPagingSql(modelInfo);
                        case QueryType.SelectTop:
                            return limitingResultClause.GetFetchFirstSql(modelInfo);
                        default:
                            break;
                    }
                }
                var queryClause = Clause as QueryClause;
                if (queryClause != null) {
                    switch (queryType) {
                        case QueryType.Select:
                            return queryClause.GetSelectSql();
                        case QueryType.SelectCnt:
                            return queryClause.GetSelectCntSql(modelInfo);
                        default:
                            break;
                    }
                }
                return Clause.GetSql(modelInfo);
            } finally {
                if (__isLocked) Monitor.Exit(_locker);
            }
        }

        /// <summary>
        /// Get last generated sql
        /// </summary>
        /// <returns></returns>
        public string GetLastSql() {
            return Clause.GetLastSql();
        }
    }
}
