using Kai.Universal.Data;
using Kai.Universal.Sql.Where;
using System;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    /// <summary>
    /// Select clause class
    /// </summary>
    public class QueryClause : AbstractSqlClause {

        internal static readonly string NO_COLUMNS = "no select columns";

        /// <summary>
        /// check columns
        /// </summary>
        protected override void NecessaryCheck() {
            if (base.IsEmptyColumns()) {
                throw new ArgumentException(NO_COLUMNS);
            }
        }

        /// <summary>
        /// gen sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenSql(ModelInfo modelInfo) {
            sb.Append("select ");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);

            this.AppendWhereSql(modelInfo.Criterias);
            this.AppendOrderBy(modelInfo.OrderBy);
            this.AppendGroupBy(modelInfo.GroupBy);
        }

        /// <summary>
        /// this is not implement
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenPreparedSql(ModelInfo modelInfo) {
            throw new NotImplementedException("select not implemented.");
        }

        /// <summary>
        /// select cnt sql
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public string GetSelectCntSql(ModelInfo modelInfo) {
            sb = new StringBuilder();
            sb.Append("select count(1) ");
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            this.AppendWhereSql(modelInfo.Criterias);

            return sb.ToString();
        }

        /// <summary>
        /// select all wanted columns sql
        /// </summary>
        /// <returns></returns>
        public string GetSelectSql() {
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

        /// <summary>
        /// gen where sql by criteria pool
        /// </summary>
        /// <param name="criteriaPool"></param>
        protected void AppendWhereSql(CriteriaPool criteriaPool) {
            if (criteriaPool != null) {
                string whereSql = criteriaPool.GetWhereSql();
                if (whereSql != null && !"".Equals(whereSql.Trim())) {
                    sb.Append(TEXT_WHERE_WITH_SPACE).Append(whereSql);
                }
            }
        }

        /// <summary>
        /// append order by clause
        /// </summary>
        /// <param name="modelOrderBy"></param>
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

        /// <summary>
        /// append group by clause
        /// </summary>
        /// <param name="modelGroupBy"></param>
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
