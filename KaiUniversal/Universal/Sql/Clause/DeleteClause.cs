using Kai.Universal.Data;
using System;

namespace Kai.Universal.Sql.Clause {
    /// <summary>
    /// Delete Clause class
    /// </summary>
    public class DeleteClause : UpdateClause {

        internal static readonly string TEXT_DELETE_WITH_SPACE = "delete ";

        /// <summary>
        /// check pk
        /// </summary>
        protected override void NecessaryCheck() {
            if (base.IsEmptyNonQueryMandatoryColumns()) {
                throw new ArgumentException(NO_WHERE_COLUMNS);
            }
        }

        /// <summary>
        /// gen sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenSql(ModelInfo modelInfo) {
            string[] mainColumns = base.DmlInfo.NonQueryMandatoryColumns;
            object model = modelInfo.Model;

            sb.Append(TEXT_DELETE_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(TEXT_WHERE_WITH_SPACE);
            this.AppendColsWithProp(mainColumns, model, TEXT_AND_WITH_SPACE);
        }

        /// <summary>
        /// gen prepared sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenPreparedSql(ModelInfo modelInfo) {
            string[] mainColumns = base.DmlInfo.NonQueryMandatoryColumns;
            object model = modelInfo.Model;

            sb.Append(TEXT_DELETE_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(TEXT_WHERE_WITH_SPACE);
            this.AppendColsWithPrepareProp(mainColumns, model, TEXT_AND_WITH_SPACE);
        }

    }
}