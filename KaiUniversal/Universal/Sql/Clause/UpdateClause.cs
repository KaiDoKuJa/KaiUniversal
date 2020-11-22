using Kai.Universal.Data;
using Kai.Universal.Text;
using System;
using System.Collections;

namespace Kai.Universal.Sql.Clause {
    /// <summary>
    /// Update clause class
    /// </summary>
    public class UpdateClause : AbstractSqlClause {

        internal static readonly string NO_COLUMNS = "no update columns";
        internal static readonly string NO_WHERE_COLUMNS = "no where columns";
        internal static readonly string TEXT_UPDATE_WITH_SPACE = "update ";

        /// <summary>
        /// check columns / pk
        /// </summary>
        protected override void NecessaryCheck() {
            if (base.IsEmptyColumns()) {
                throw new ArgumentNullException(NO_COLUMNS);
            }
            if (base.IsEmptyNonQueryMandatoryColumns()) {
                throw new ArgumentNullException(NO_WHERE_COLUMNS);
            }
        }

        /// <summary>
        /// gen sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenSql(ModelInfo modelInfo) {
            string[] updateColumns = base.DmlInfo.Columns;
            string[] mainColumns = base.DmlInfo.NonQueryMandatoryColumns;
            object model = modelInfo.Model;
            object originModel = modelInfo.OriginModel;

            sb.Append(TEXT_UPDATE_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(" set ");
            this.AppendColsWithProp(updateColumns, model, ",");
            sb.Append(TEXT_WHERE_WITH_SPACE);
            if (originModel == null) {
                this.AppendColsWithProp(mainColumns, model, TEXT_AND_WITH_SPACE);
            } else {
                this.AppendColsWithProp(mainColumns, originModel, TEXT_AND_WITH_SPACE);
            }
        }

        /// <summary>
        /// gen prepared sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected override void GenPreparedSql(ModelInfo modelInfo) {
            string[] updateColumns = base.DmlInfo.Columns;
            string[] mainColumns = base.DmlInfo.NonQueryMandatoryColumns;
            object model = modelInfo.Model;

            sb.Append(TEXT_UPDATE_WITH_SPACE);
            sb.Append(base.DmlInfo.TableName);
            sb.Append(" set ");
            this.AppendColsWithPrepareProp(updateColumns, model, ",");
            sb.Append(TEXT_WHERE_WITH_SPACE);
            this.AppendColsWithPrepareProp(mainColumns, model, TEXT_AND_WITH_SPACE);
        }

        /// <summary>
        /// append set region clause by model's props
        /// <para>ex : update delimiter "," then gen AAA='aaa', BBB=1.0, CCC=getdate()</para>
        /// <para>ex : and delimiter "and"  then gen AAA='aaa' and BBB=1.0 and CCC=getdate()</para>
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="model"></param>
        /// <param name="delimiter"></param>
        protected void AppendColsWithProp(string[] cols, object model, string delimiter) {
            bool isMapModel = false;
            var map = model as IDictionary;
            if (map != null) {
                isMapModel = true;
            }
            for (int i = 0; i < cols.Length; i++) {
                if (i > 0) {
                    sb.Append(delimiter);
                }
                sb.Append(cols[i]);
                sb.Append('=');

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
        /// append set region clause with prepare pattern
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="model"></param>
        /// <param name="delimiter"></param>
        protected void AppendColsWithPrepareProp(string[] cols, object model, string delimiter) {
            bool isMapModel = false;
            var map = model as IDictionary;
            if (map != null) {
                isMapModel = true;
            }
            for (int i = 0; i < cols.Length; i++) {
                if (i > 0) {
                    sb.Append(delimiter);
                }
                sb.Append(cols[i]);
                sb.Append('=');

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
