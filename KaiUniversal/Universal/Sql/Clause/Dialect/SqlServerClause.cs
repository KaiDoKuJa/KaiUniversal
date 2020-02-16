using Kai.Universal.Data;
using Kai.Universal.Sql.Type;
using System;
using System.Text;

namespace Kai.Universal.Sql.Clause.Dialect {
    public class SqlServerClause : QueryClause, LimitingResultClause {

        public SqlServerClause() {
            base.DbmsType = DbmsType.FromSqlServer2012;
        }

        public string GetFetchFirstSql(ModelInfo modelInfo) {
            sb = new StringBuilder();
            sb.Append("select top ");
            sb.Append(modelInfo.Top);
            sb.Append(" ");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            base.AppendWhereSql(modelInfo.Criterias);
            sb.Append(" ");
            base.AppendOrderBy(modelInfo.OrderBy);
            return sb.ToString();
        }


        public string GetPagingSql(ModelInfo modelInfo) {
            switch (this.DbmsType) {
                case DbmsType.FromSqlServer2005:
                    return this.getRowNumPagingSql(modelInfo);
                case DbmsType.FromSqlServer2012:
                    return this.getOffsetPagingSql(modelInfo);
                default:
                    throw new Exception("error dbtype!");
            }
        }

        /**
         * select PAGING_TABLE.*
         * from (
         * select ROW_NUMBER() OVER (order by {orderBy}) as ROW_NUM,
         * {selectColumns}
         * from {tableName}
         * where {WhereCondition}) as PAGING_TABLE
         * where PAGING_TABLE.ROW_NUM between {startNumber} and {endNumber}
         * @param dmlInfo
         * @param modelInfo
         * @param whereSql
         * @return
         * @throws Exception
         */
        private string getRowNumPagingSql(ModelInfo modelInfo) {
            int pageNumber = modelInfo.PageNumber;
            int eachPageSize = modelInfo.EachPageSize;

            sb = new StringBuilder();
            sb.Append("select PAGING_TABLE.* from (select ROW_NUMBER() OVER (");
            base.AppendOrderBy(modelInfo.OrderBy);
            sb.Append(") as ROW_NUM,");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);

            base.AppendWhereSql(modelInfo.Criterias);

            sb.Append(") as PAGING_TABLE where PAGING_TABLE.ROW_NUM between ");
            sb.Append(eachPageSize * pageNumber + 1);
            sb.Append(" and ");
            sb.Append(eachPageSize * (pageNumber + 1));
            sb.Append(" order by ROW_NUM ");
            return sb.ToString();
        }

        private string getOffsetPagingSql(ModelInfo modelInfo) {
            int pageNumber = modelInfo.PageNumber;
            int eachPageSize = modelInfo.EachPageSize;

            sb = new StringBuilder();
            sb.Append("select ");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(TEXT_FROM_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);

            base.AppendWhereSql(modelInfo.Criterias);
            sb.Append(" ");
            base.AppendOrderBy(modelInfo.OrderBy);
            sb.Append(" offset ");
            sb.Append(pageNumber * eachPageSize);
            sb.Append(" rows fetch first ");
            sb.Append(eachPageSize);
            sb.Append(" rows only");

            return sb.ToString();
        }

    }

}
