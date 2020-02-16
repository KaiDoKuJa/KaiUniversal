using Kai.Universal.Data;
using Kai.Universal.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    public class InsertClause : AbstractSqlClause {

        public static readonly string NO_COLUMNS = "no insert columns";
        public static readonly string TEXT_INSERT_INTO_WITH_SPACE = "insert into ";

        protected override void NecessaryCheck() {
            if (base.IsEmptyColumns()) {
                throw new ArgumentNullException(NO_COLUMNS);
            }
        }

        protected override string GenSql(ModelInfo modelInfo) {
            sb = new StringBuilder();
            sb.Append(TEXT_INSERT_INTO_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(" (");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(") values (");
            this.AppendProps(modelInfo);
            sb.Append(")");
            return sb.ToString();
        }


        protected override string GenPreparedSql(ModelInfo modelInfo) {
            string[] insertColumns = base.DmlInfo.Columns;
            object model = modelInfo.Model;

            sb = new StringBuilder();
            sb.Append(TEXT_INSERT_INTO_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(" (");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(") values (");
            this.AppendPrepareProps(insertColumns, model);
            sb.Append(")");
            return sb.ToString();
        }

        /**
         * append model's props
         * ex: 'aaa', 1.0, 'bbb', getdate()
         * @param cols colNames
         * @param model value object
         * @param specialCols special colNames
         */
        protected void AppendProps(ModelInfo modelInfo) {
            bool isMapModel = false;
            object model = modelInfo.Model;
            string[] cols = base.DmlInfo.Columns;

            var map = model as Dictionary<object, object>;
            if (map != null) { 
                isMapModel = true;
            }
            for (int i = 0; i < cols.Length; i++) {
                if (i > 0) {
                    sb.Append(',');
                }

                bool isNoAttachQuoteColumn = IsNoAttachQuoteColumn(cols[i]);

                object propValue = null;
                string key = GetColumnMapping(cols[i], isMapModel);
                if (isMapModel) {
                    propValue = map[key];
                } else {
                    propValue = ReflectUtility.GetValue(model, key);
                }
                this.AppendPropValue(propValue, isNoAttachQuoteColumn);
            }
        }

        /**
         * append prepare props
         * ex: ?,?,getdate(),?
         * @param cols colNames
         * @param model value object
         * @param specialCols special colNames
         */
        protected void AppendPrepareProps(string[] cols, object model) {
            bool isMapModel = false;
            var map = model as Dictionary<object, object>;
            if (map != null) {
                isMapModel = true;
            }
            for (int i = 0; i < cols.Length; i++) {
                if (i > 0) {
                    sb.Append(',');
                }

                object propValue = "?";
                if (IsNoAttachQuoteColumn(cols[i])) {
                    string key = GetColumnMapping(cols[i], isMapModel);
                    if (isMapModel) {
                        propValue = map[key];
                    } else {
                        propValue = ReflectUtility.GetValue(model, key);
                    }
                }
                // all are special column
                this.AppendPropValue(propValue, true);
            }
        }

    }

}
