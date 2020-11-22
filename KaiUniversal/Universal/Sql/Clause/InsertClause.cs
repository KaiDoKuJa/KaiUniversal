using Kai.Universal.Data;
using Kai.Universal.Text;
using System;
using System.Collections;

namespace Kai.Universal.Sql.Clause {
    /// <summary>
    /// Insert Clause class
    /// </summary>
    public class InsertClause : AbstractSqlClause {

        internal static readonly string NO_COLUMNS = "no insert columns";
        internal static readonly string TEXT_INSERT_INTO_WITH_SPACE = "insert into ";

        /// <summary>
        /// check columns
        /// </summary>
        protected override void NecessaryCheck() {
            if (base.IsEmptyColumns()) {
                throw new ArgumentNullException(NO_COLUMNS);
            }
        }

        /// <summary>
        /// gen sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenSql(ModelInfo modelInfo) {
            sb.Append(TEXT_INSERT_INTO_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(" (");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(") values (");
            this.AppendProps(modelInfo);
            sb.Append(")");
        }

        /// <summary>
        /// gen prepared sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenPreparedSql(ModelInfo modelInfo) {
            string[] insertColumns = base.DmlInfo.Columns;
            object model = modelInfo.Model;

            sb.Append(TEXT_INSERT_INTO_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(" (");
            base.AppendCols(base.DmlInfo.Columns, ',');
            sb.Append(") values (");
            this.AppendPrepareProps(insertColumns, model);
            sb.Append(")");
        }

        /// <summary>
        /// append model's props
        /// <para>ex: 'aaa', 1.0, 'bbb', getdate()</para>
        /// </summary>
        /// <param name="modelInfo"></param>
        protected void AppendProps(ModelInfo modelInfo) {
            bool isMapModel = false;
            object model = modelInfo.Model;
            string[] cols = base.DmlInfo.Columns;

            var map = model as IDictionary;
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

        /// <summary>
        /// append prepare props
        /// <para>ex: ?,?,getdate(),?</para>
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="model"></param>
        protected void AppendPrepareProps(string[] cols, object model) {
            bool isMapModel = false;
            var map = model as IDictionary;
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
