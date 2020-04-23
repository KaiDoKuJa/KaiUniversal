using Kai.Universal.Data;
using Kai.Universal.Sql.Where;
using System;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    public class QueryClause : AbstractSqlClause {

        public static readonly string NO_COLUMNS = "no select columns";

        protected override void NecessaryCheck() {
            if (base.IsEmptyColumns()) {
                throw new ArgumentException(NO_COLUMNS);
            }
        }

        protected override void GenSql(ModelInfo modelInfo) {
            sb.Append("select ");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);

            this.AppendWhereSql(modelInfo.Criterias);
            this.AppendOrderBy(modelInfo.OrderBy);
            this.AppendGroupBy(modelInfo.GroupBy);
        }


        protected override void GenPreparedSql(ModelInfo modelInfo) {
            throw new Exception("not support method!");
        }

        public string GetSelectCntSql(ModelInfo modelInfo) {
            sb = new StringBuilder();
            sb.Append("select count(1) ");
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            this.AppendWhereSql(modelInfo.Criterias);

            return sb.ToString();
        }

        public string GetSelectAllSql() {
            sb = new StringBuilder();
            NecessaryCheck();

            sb.Append("select ");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);

            // no gen where
            this.AppendOrderBy(null);

            return sb.ToString();
        }

        protected void AppendWhereSql(CriteriaPool criteriaPool) {
            if (criteriaPool != null) {
                string whereSql = criteriaPool.GetWhereSql();
                if (whereSql != null && !"".Equals(whereSql.Trim())) {
                    sb.Append(TEXT_WHERE_WITH_SPACE).Append(whereSql);
                }
            }
        }

        protected void AppendOrderBy(string modelOrderBy) {
            string orderBy = modelOrderBy;
            if (orderBy == null || "".Equals(orderBy.Trim())) {
                orderBy = base.DmlInfo.OrderBy;
            }

            if (orderBy != null && !"".Equals(orderBy.Trim())) {
                sb.Append(" order by ");
                sb.Append(orderBy);
            }
        }
        protected void AppendGroupBy(string modelGroupBy) {
            string GroupBy = modelGroupBy;
            if (GroupBy == null || "".Equals(GroupBy.Trim())) {
                GroupBy = base.DmlInfo.GroupBy;
            }

            if (GroupBy != null && !"".Equals(GroupBy.Trim())) {
                sb.Append(" group by ");
                sb.Append(GroupBy);
            }
        }

    }

}
