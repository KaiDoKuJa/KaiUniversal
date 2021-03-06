﻿using Kai.Universal.Data;
using Kai.Universal.Sql.Type;
using Kai.Universal.Sql.Where;
using Kai.Universal.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kai.Universal.Sql.Clause {
    /// <summary>
    /// all of the Clause Class are non thread safe
    /// please use DmlHandler to control thread safe
    /// </summary>
    public abstract class AbstractSqlClause {

        internal static readonly string NO_TABLE_NAME = "no table name";
        internal static readonly string TEXT_FROM_WITH_SPACE = " from ";
        internal static readonly string TEXT_WHERE_WITH_SPACE = " where ";
        internal static readonly string TEXT_AND_WITH_SPACE = " and ";
        internal static readonly string NO_DML_INFO = "no DmlInfo";

        /// <summary>
        /// this clause dbms type
        /// </summary>
        public DbmsType DbmsType { get; set; }

        /// <summary>
        /// this clause setting
        /// </summary>
        public DmlInfo DmlInfo { get; set; }

        /// <summary>
        /// sql persistence
        /// </summary>
        protected StringBuilder sb;

        private void BaseNecessaryCheck() {
            if (this.IsEmptyTableName()) {
                throw new ArgumentNullException(NO_TABLE_NAME);
            }
            NecessaryCheck();
        }

        /// <summary>
        /// (abs-method) necessary check
        /// </summary>
        protected abstract void NecessaryCheck();
        
        /// <summary>
        /// (abs-method) gen sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected abstract void GenSql(ModelInfo modelInfo);

        /// <summary>
        /// (abs-method) gen prepare stmt sql
        /// </summary>
        /// <param name="modelInfo"></param>
        protected abstract void GenPreparedSql(ModelInfo modelInfo);

        /// <summary>
        /// gen direct sql by sqlTemplate
        /// <para>modelInfo usage :</para>
        /// <para>(m) sqlTemplate - ex: select xxx from xxx where xxx=#1 and yyy=${2} and zzz=:3</para>
        /// <para>(o) replacements - set replacePattern to replace target string like #1, ${2}, :3</para>
        /// <para>(o) criterias - after replaced sqlTemplate, append " and {criterias.whereSql}"</para>
        /// <para>                ps: but select sql template has "order by" can't append this clause</para>
        /// </summary>
        /// <param name="modelInfo"></param>
        protected void GenDirectSql(ModelInfo modelInfo) {
            sb.Append(DmlInfo.SqlTemplate);
            string whereSql = null;
            if (modelInfo != null) {
                CriteriaPool criterias = modelInfo.Criterias;
                if (criterias != null) {
                    whereSql = criterias.GetWhereSql();
                }
            }

            if (whereSql != null && !"".Equals(whereSql.Trim())) {
                sb.Append(TEXT_AND_WITH_SPACE);
                sb.Append(whereSql);
            }
        }

        /// <summary>
        /// Main generate sql
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public string GetSql(ModelInfo modelInfo) {
            if (DmlInfo == null) throw new ArgumentNullException(NO_DML_INFO);
            sb = new StringBuilder();
            if (!UseSqlTemplate(DmlInfo.SqlTemplate)) {
                BaseNecessaryCheck();
                GenSql(modelInfo);
            } else {
                GenDirectSql(modelInfo);
            }
            if (modelInfo != null) {
                var replacements = modelInfo.Replacements;
                if (replacements != null && replacements.Count > 0) {
                    DoReplacementsSql(replacements);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get generated sql, if not generate will be call GetSql(null).
        /// </summary>
        /// <returns></returns>
        public string GetLastSql() {
            if (sb != null)
                return sb.ToString();
            return GetSql(null);
        }

        private void DoReplacementsSql(List<Replacement> replacements) {
            foreach (Replacement r in replacements) {
                sb.Replace(r.ReplacePattern, r.Value);
            }
        }

        /// <summary>
        /// check {TableName} is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyTableName() {
            return (DmlInfo.TableName == null || "".Equals(DmlInfo.TableName));
        }

        /// <summary>
        /// check {Columns} is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyColumns() {
            string[] columns = DmlInfo.Columns;
            return OrmUtility.IsArrayEmpty(columns);
        }

        /// <summary>
        /// check {NonQueryMandatoryColumns} is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyNonQueryMandatoryColumns() {
            string[] columns = DmlInfo.NonQueryMandatoryColumns;
            return OrmUtility.IsArrayEmpty(columns);
        }

        /// <summary>
        /// check {NoAttachQuoteColumns} is empty
        /// </summary>
        /// <returns></returns>
        public bool IsNoAttachQuoteColumn(string col) {
            string[] noAttachQuoteColumns = DmlInfo.NoAttachQuoteColumns;
            if (noAttachQuoteColumns == null) return false;
            return OrmUtility.IsStringInArray(col, noAttachQuoteColumns);
        }

        /// <summary>
        /// use sqlTemplate property
        /// </summary>
        /// <param name="sqlTemplate"></param>
        /// <returns></returns>
        protected bool UseSqlTemplate(string sqlTemplate) {
            return (sqlTemplate != null && !"".Equals(sqlTemplate.Trim()));
        }

        /// <summary>
        /// append columns with delimiter
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="delimiter"></param>
        protected void AppendCols(string[] cols, char delimiter) {
            for (int i = 0; i < cols.Length; i++) {
                if (i > 0) {
                    sb.Append(delimiter);
                }
                sb.Append(cols[i]);
            }
        }

        /// <summary>
        /// append propery value and with quote or not
        /// </summary>
        /// <param name="propValue"></param>
        /// <param name="isNoAttachQuoteColumn"></param>
        protected void AppendPropValue(object propValue, bool isNoAttachQuoteColumn) {
            if (propValue == null) {
                sb.Append("null");
            } else {
                if (!isNoAttachQuoteColumn) {
                    AppendPropValueByChkClazz(propValue);
                } else {
                    // noneed control when propValue is null.
                    if (propValue is string) {
                        sb.Append(string.Format("{0}", propValue));
                    } else {
                        // Kai : 未來建立exception時可由上層抓取colName
                        throw new ArgumentException("propValue is not string for special column!", "propValue");
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
        protected void AppendPropValueByChkClazz(object propValue) {
            if (!(propValue is byte[])) {
                string propValueString = OrmUtility.GetSqlString(propValue, DmlInfo.UseUnicodePrefix);
                sb.Append(propValueString);
            } else {
                switch (DbmsType) {
                    // oracle : hextoraw('453d7a34')
                    case DbmsType.FromSqlServer2005:
                    case DbmsType.FromSqlServer2012:
                        // mssql : 0x453d7a34
                        sb.Append(string.Format("0x{0}", HexUtility.BytesToHex((byte[])propValue)));
                        break;
                    case DbmsType.Default:
                    default:
                        throw new ArgumentException("byte array need select one db type");
                }
            }
        }

        /// <summary>
        /// get the column mapping to model column name
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="isMapModel"></param>
        /// <returns></returns>
        protected string GetColumnMapping(string colName, bool isMapModel) {
            string wordCase = colName;
            Dictionary<string, string> customerMapping = DmlInfo.CustomerMapping;
            if (customerMapping == null) {
                WordCase mapModelWordCase = DmlInfo.MapModelWordCase;
                if (!isMapModel) mapModelWordCase = WordCase.UpperCamel;
                WordCase columnWordCase = DmlInfo.ColumnWordCase;
                if (mapModelWordCase != columnWordCase) {
                    wordCase = TextUtility.ConvertWordCase(colName, columnWordCase, mapModelWordCase);
                }
            } else {
                wordCase = customerMapping[colName];
                if (!isMapModel) wordCase = char.ToUpper(wordCase[0]) + wordCase.Substring(1);
            }
            return wordCase;
        }

    }
}
