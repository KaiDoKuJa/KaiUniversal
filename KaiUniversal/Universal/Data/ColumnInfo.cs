using System;

namespace Kai.Universal.Data {
    /// <summary>
    /// the table column info
    /// </summary>
    public class ColumnInfo {

        /// <summary>
        /// table column name
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// the mapping model property name
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// column type
        /// </summary>
        public Type ColType { get; set; }
    }
}
