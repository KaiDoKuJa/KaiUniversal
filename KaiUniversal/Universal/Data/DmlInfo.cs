using Kai.Universal.Text;
using Kai.Universal.Sql.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kai.Universal.Data {
    public class DmlInfo {
        public string Name { get; set; }  // this info name
        public string Group { get; set; } // this info name of group
        public DmlType DmlType { get; set; } // select/insert/update/delete
        public string TableName { get; set; }
        public string[] Columns { get; set; }
        public string[] NonQueryMandatoryColumns { get; set; } // for ins/upd/del where columns
        public string[] NoAttachQuoteColumns { get; set; } // for function string noneed quote
        public string OrderBy { get; set; }
        public string GroupBy { get; set; }
        public string SqlTemplate { get; set; }

        private WordCase columnWordCase = WordCase.UPPER_UNDERSCORE;
        public WordCase ColumnWordCase { get => columnWordCase; set => columnWordCase = value; }
        private WordCase mapModelWordCase = WordCase.UPPER_UNDERSCORE;
        public WordCase MapModelWordCase { get => mapModelWordCase; set => mapModelWordCase = value; }
        public Dictionary<string, string> CustomerMapping { get; set; } // customer colName -> modelName

        private bool useUnicodePrefix = false;
        public bool UseUnicodePrefix { get => useUnicodePrefix; set => useUnicodePrefix = value; }

    }
}
