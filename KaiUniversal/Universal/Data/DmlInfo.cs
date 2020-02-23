using Kai.Universal.Sql.Type;
using Kai.Universal.Text;
using System.Collections.Generic;

namespace Kai.Universal.Data {
    public class DmlInfo {

        public DmlType DmlType { get; set; } // select/insert/update/delete
        public string TableName { get; set; }
        public string[] Columns { get; set; }
        public string[] NonQueryMandatoryColumns { get; set; } // for ins/upd/del where columns
        public string[] NoAttachQuoteColumns { get; set; } // for function string noneed quote
        public string OrderBy { get; set; }
        public string GroupBy { get; set; }
        public string SqlTemplate { get; set; }

        public WordCase ColumnWordCase { get; set; } = WordCase.UpperCamel;
        public WordCase MapModelWordCase { get; set; } = WordCase.UpperCamel;
        public Dictionary<string, string> CustomerMapping { get; set; } // customer colName -> modelName

        public bool UseUnicodePrefix { get; set; } = false;

    }
}
