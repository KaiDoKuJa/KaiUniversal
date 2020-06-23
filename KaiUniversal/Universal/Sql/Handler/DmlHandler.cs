using Kai.Universal.Data;
using Kai.Universal.Sql.Clause;
using Kai.Universal.Sql.Clause.Dialect;
using Kai.Universal.Sql.Type;
using System;
using System.Threading;

namespace Kai.Universal.Sql.Handler {
    public class DmlHandler {

        private object _locker = new object();
        public AbstractSqlClause Clause { get; set; }

        public DmlHandler() { }
        public DmlHandler(AbstractSqlClause clause) {
            this.Clause = clause;
        }

        public static DmlHandler CreateHandler(DmlInfo dmlInfo) {
            return CreateHandler(DbmsType.Default, dmlInfo);
        }

        public static DmlHandler CreateHandler(DbmsType dbmsType, DmlInfo dmlInfo) {
            AbstractSqlClause clause = CreateClause(dbmsType, dmlInfo);
            return new DmlHandler(clause);
        }

        private static AbstractSqlClause CreateClause(DbmsType dbmsType, DmlInfo dmlInfo) {
            AbstractSqlClause clause = null;
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
                    throw new Exception("no such dml type!");
            }
            clause.DmlInfo = dmlInfo;
            clause.DbmsType = dbmsType;
            return clause;
        }

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
                        case QueryType.SelectAll:
                            return queryClause.GetSelectAllSql();
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

        public string GetLastSql() {
            return Clause.GetLastSql();
        }
    }
}
