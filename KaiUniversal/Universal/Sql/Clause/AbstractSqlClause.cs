using System;
using System.Collections.Generic;
using System.Text;
using Kai.Universal.Data;
using Kai.Universal.Sql.Type;
using Kai.Universal.Sql.Where;
using Kai.Universal.Text;

namespace Kai.Universal.Sql.Clause {
    public abstract class AbstractSqlClause {

        private static readonly string NO_TABLE_NAME = "no table name";
        public static readonly string TEXT_FROM_WITH_SPACE = " from ";
        public static readonly string TEXT_WHERE_WITH_SPACE = " where ";
        public static readonly string TEXT_AND_WITH_SPACE = " and ";
        public static readonly string NO_DML_INFO = "no DmlInfo";

        public DbmsType DbmsType { get; set; }
        public DmlInfo DmlInfo { get; set; }

        protected StringBuilder sb;

        private void BaseNecessaryCheck () {
            if (this.IsEmptyTableName ()) {
                throw new ArgumentNullException (NO_TABLE_NAME);
            }
            NecessaryCheck ();
        }

        protected abstract void NecessaryCheck ();
        // protected abstract <T extends ModelInfo> string genSql(T modelInfo)
        protected abstract string GenSql (ModelInfo modelInfo);

        protected abstract string GenPreparedSql (ModelInfo modelInfo);

        /**
         * gen direct sql by sqlTemplate
         * modelInfo usage :
         * (m) sqlTemplate - ex: select xxx from xxx where xxx=#1 and yyy=${2} and zzz=:3
         * (o) replacements - set replacePattern to replace target string like #1, ${2}, :3
         * (o) criterias - after replaced sqlTemplate, append " and {criterias.whereSql}"
         *                 ps: but select sql template has "order by" can't append this clause
         * @param modelInfo
         * @return
         */
        protected string GenDirectSql (ModelInfo modelInfo) {
            sb = new StringBuilder ();
            sb.Append (DmlInfo.SqlTemplate);

            string whereSql = null;
            List<Replacement> replacements = null;
            if (modelInfo != null) {
                replacements = modelInfo.Replacements;
                CriteriaPool criterias = modelInfo.Criterias;
                if (criterias != null) {
                    whereSql = criterias.GetWhereSql ();
                }
            }

            if (whereSql != null && !"".Equals (whereSql.Trim ())) {
                sb.Append (TEXT_AND_WITH_SPACE);
                sb.Append (whereSql);
            }

            string sql = sb.ToString ();
            if (replacements != null && replacements.Count > 0) {
                DoReplacementsSql (ref sql, replacements);
            }

            return sql;
        }

        public string GetSql (ModelInfo modelInfo) {
            if (DmlInfo == null) throw new ArgumentNullException (NO_DML_INFO);
            if (UseSqlTemplate (DmlInfo.SqlTemplate)) return GenDirectSql (modelInfo);
            BaseNecessaryCheck ();
            string sql = GenSql (modelInfo);
            if (modelInfo != null) {
                if (modelInfo.Replacements != null && modelInfo.Replacements.Count > 0) {
                    DoReplacementsSql (ref sql, modelInfo.Replacements);
                }
            }
            return sql;
        }

        public string GetPreparedSql (ModelInfo modelInfo) {
            throw new NotImplementedException ("not implement");
        }

        public string GetLastSql () {
            if (sb != null)
                return sb.ToString ();
            return GetSql (null);
        }

        private void DoReplacementsSql (ref string sql, List<Replacement> replacements) {
            foreach (Replacement r in replacements) {
                sql = sql.Replace (r.ReplacePattern, r.Value);
            }
        }

        public bool IsEmptyTableName () {
            return (DmlInfo.TableName == null ||"".Equals (DmlInfo.TableName));
        }

        public bool IsEmptyColumns () {
            string[] columns = DmlInfo.Columns;
            return OrmUtility.IsArrayEmpty (columns);
        }

        public bool IsEmptyNonQueryMandatoryColumns () {
            string[] columns = DmlInfo.NonQueryMandatoryColumns;
            return OrmUtility.IsArrayEmpty (columns);
        }

        public bool IsNoAttachQuoteColumn (string col) {
            string[] noAttachQuoteColumns = DmlInfo.NoAttachQuoteColumns;
            if (noAttachQuoteColumns == null) return false;
            return OrmUtility.IsStringInArray (col, noAttachQuoteColumns);
        }

        protected bool UseSqlTemplate (string sqlTemplate) {
            return (sqlTemplate != null && !"".Equals (sqlTemplate.Trim ()));
        }

        protected void AppendCols (string[] cols, char delimiter) {
            for (int i = 0; i < cols.Length; i++) {
                if (i > 0) {
                    sb.Append (delimiter);
                }
                sb.Append (cols[i]);
            }
        }

        protected void AppendPropValue (object propValue, bool isNoAttachQuoteColumn) {
            if (propValue == null) {
                sb.Append ("null");
            } else {
                if (!isNoAttachQuoteColumn) {
                    AppendPropValueByChkClazz (propValue);
                } else {
                    // noneed control when propValue is null.
                    if (propValue is string) {
                        sb.Append (string.Format ("{0}", propValue));
                    } else {
                        // Kai : 未來建立exception時可由上層抓取colName
                        throw new ArgumentException ("propValue is not string for special column!", "propValue");
                    }
                }
            }
        }

        /**
         * no check null, so propValue must be not null!
         *
         * @param propValue
         *            the property value
         */
        protected void AppendPropValueByChkClazz (object propValue) {
            if (!(propValue is byte[])) {
                string propValueString = OrmUtility.GetSqlString (propValue, DmlInfo.UseUnicodePrefix);
                sb.Append (propValueString);
            } else {
                switch (DbmsType) {
                    // oracle : hextoraw('453d7a34')
                    case DbmsType.FromSqlServer2005:
                    case DbmsType.FromSqlServer2012:
                        // mssql : 0x453d7a34
                        sb.Append (string.Format ("0x{0}", HexUtility.BytesToHex ((byte[]) propValue)));
                        break;
                    case DbmsType.Default:
                    default:
                        throw new ArgumentException ("byte array need select one db type");
                }
            }
        }

        protected string GetColumnMapping (string colName, bool isMapModel) {
            string wordCase = colName;
            Dictionary<string, string> customerMapping = DmlInfo.CustomerMapping;
            if (customerMapping == null) {
                WordCase mapModelWordCase = DmlInfo.MapModelWordCase;
                if (!isMapModel) mapModelWordCase = WordCase.UpperCamel;
                WordCase columnWordCase = DmlInfo.ColumnWordCase;
                if (mapModelWordCase != columnWordCase) {
                    wordCase = TextUtility.ConvertWordCase (colName, columnWordCase, mapModelWordCase);
                }
            } else {
                wordCase = customerMapping[colName];
                if (!isMapModel) wordCase = Char.ToUpper (wordCase[0]) + wordCase.Substring (1);
            }
            return wordCase;
        }

    }
}