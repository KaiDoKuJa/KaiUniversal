using Kai.Universal.Sql.Type;
using Kai.Universal.Text;
using System.Collections.Generic;

namespace Kai.Universal.Data {
    /// <summary>
    /// Kai ORM info object
    /// </summary>
    public class DmlInfo {

        /// <summary>
        /// DmlType : select/insert/update/delete
        /// </summary>
        public DmlType DmlType { get; set; }
        /// <summary>
        /// table name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// wanted columns
        /// </summary>
        public string[] Columns { get; set; }

        /// <summary>
        /// the mandatory column for ins/upd/del table (pk or wanted condition)
        /// <para>for sql : where mandatory_column = 'xxx'</para>
        /// </summary>
        public string[] NonQueryMandatoryColumns { get; set; }

        /// <summary>
        /// the condition don't generate quote
        /// <para>PS: this is for function like (MSSQL) getdate() or (Oracle) sysdate</para>
        /// <para>this property is for configurable, not for client ui keyin.</para>
        /// </summary>
        public string[] NoAttachQuoteColumns { get; set; }

        /// <summary>
        /// order by sql
        /// <para>ex: "Col1 desc, Col2 asc"</para>
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// group by sql, becareful your columns setting.
        /// <para>ex: "Col1 desc, Col2 asc"</para>
        /// <para>ex: "Col1 desc, Col2 asc having count(1) > 2"</para>
        /// </summary>
        public string GroupBy { get; set; }

        /// <summary>
        /// to define sql template, can use any pattern {0}, {Col1}, #Col1, :Col1 ...
        /// </summary>
        public string SqlTemplate { get; set; }

        /// <summary>
        /// table column word case, default is uppercamel
        /// </summary>
        public WordCase ColumnWordCase { get; set; } = WordCase.UpperCamel;

        /// <summary>
        /// mapping model word case, default is uppercamel
        /// </summary>
        public WordCase MapModelWordCase { get; set; } = WordCase.UpperCamel;

        /// <summary>
        /// define your customer orm (colName -> modelName)
        /// </summary>
        public Dictionary<string, string> CustomerMapping { get; set; }

        /// <summary>
        /// use unicode prefix
        /// <para>ex: (MSSQL) N'xxx'</para>
        /// </summary>
        public bool UseUnicodePrefix { get; set; } = false;

    }
}
