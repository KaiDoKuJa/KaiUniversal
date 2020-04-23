using Kai.Universal.Data;
using System;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    public class DeleteClause : UpdateClause {

        public static readonly string TEXT_DELETE_WITH_SPACE = "delete ";

        protected override void NecessaryCheck() {
            if (base.IsEmptyNonQueryMandatoryColumns()) {
                throw new ArgumentException(NO_WHERE_COLUMNS);
            }
        }

        protected override void GenSql(ModelInfo modelInfo) {
            string[] mainColumns = base.DmlInfo.NonQueryMandatoryColumns;
            object model = modelInfo.Model;

            sb.Append(TEXT_DELETE_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(TEXT_WHERE_WITH_SPACE);
            this.AppendColsWithProp(mainColumns, model, TEXT_AND_WITH_SPACE);
        }

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