using Kai.Universal.Sql.Where;
using System.Collections.Generic;

namespace Kai.Universal.Data {
    /// <summary>
    /// Kai Model info (input data) for generate sql
    /// </summary>
    public class ModelInfo {

        /// <summary>
        /// mapping model
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// the origin model (origin value), this is for update XXX set Col1='new' where Col1='old'
        /// </summary>
        public object OriginModel { get; set; }

        /// <summary>
        /// the criterias pattern
        /// </summary>
        public CriteriaPool Criterias { get; set; }

        /// <summary>
        /// the replacements pattern
        /// </summary>
        public List<Replacement> Replacements { get; set; }

        /// <summary>
        /// if define, will overwrite
        /// <seealso cref="Kai.Universal.Data.DmlInfo.OrderBy" />
        /// this time
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// if define, will overwrite
        /// <seealso cref="Kai.Universal.Data.DmlInfo.GroupBy" />
        /// this time
        /// </summary>
        public string GroupBy { get; set; }

        /// <summary>
        /// to generate top sql
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// to generate paging sql
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// to generate paging sql
        /// </summary>
        public int EachPageSize { get; set; }

        /// <summary>
        /// empty constructor
        /// </summary>
        public ModelInfo() { }

        /// <summary>
        /// constructor with model
        /// </summary>
        /// <param name="model"></param>
        public ModelInfo(object model) {
            this.Model = model;
        }

    }
}
