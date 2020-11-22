using Kai.Universal.Sql.Where;

namespace Kai.Universal.DataModel {
    /// <summary>
    /// Criteria Strategy Object
    /// </summary>
    public class CriteriaStrategy {
        /// <summary>
        /// Criteria ID (noneed unique)
        /// </summary>
        public string CriteriaId { get; set; }
        /// <summary>
        /// ColName
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// CriteriaType
        /// </summary>
        public CriteriaType CriteriaType { get; set; }
        /// <summary>
        /// Column Mapping
        /// </summary>
        public string ColMapping { get; set; }
        /// <summary>
        /// Replace Pattern
        /// <para>ex: {0}, #Ext, :Ext, {Ext}</para>
        /// </summary>
        public string ReplacePattern { get; set; }
        /// <summary>
        /// ReplaceMode (before / after)
        /// </summary>
        public ReplaceMode ReplaceMode { get; set; }
    }
}
