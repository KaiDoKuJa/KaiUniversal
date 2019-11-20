using Kai.Universal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    public class DeleteClause : UpdateClause {

        public static readonly string TEXT_DELETE_WITH_SPACE = "delete ";

        protected override void NecessaryCheck() {
            if (base.IsEmptyNonQueryMandatoryColumns()) {
                throw new Exception(NO_WHERE_COLUMNS);
            }
        }

        protected override string GenSql(ModelInfo modelInfo) {
            string[] mainColumns = base.DmlInfo.NonQueryMandatoryColumns;
            object model = modelInfo.Model;

            sb = new StringBuilder();
            sb.Append(TEXT_DELETE_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(TEXT_WHERE_WITH_SPACE);
            this.AppendColsWithProp(mainColumns, model, TEXT_AND_WITH_SPACE);

            return sb.ToString();
        }

        protected override string GenPreparedSql(ModelInfo modelInfo) {
            string[] mainColumns = base.DmlInfo.NonQueryMandatoryColumns;
            object model = modelInfo.Model;

            sb = new StringBuilder();
            sb.Append(TEXT_DELETE_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(TEXT_WHERE_WITH_SPACE);
            this.AppendColsWithPrepareProp(mainColumns, model, TEXT_AND_WITH_SPACE);

            return sb.ToString();
        }

    }
}