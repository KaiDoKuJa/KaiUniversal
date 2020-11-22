using Kai.Universal.Data;

namespace Kai.Universal.DataModel {
    /// <summary>
    /// the DmlInfo extension for configable
    /// </summary>
    public class DmlInfoExtension : DmlInfo {

        /// <summary>
        /// Dml ID (DmlId+GroupId unique)
        /// </summary>
        public string DmlId { get; set; }
        /// <summary>
        /// Group ID (DmlId+GroupId unique)
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// relation of CriteriaId
        /// </summary>
        public string CriteriaId { get; set; }
        /// <summary>
        /// connection id
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// descrption
        /// </summary>
        public string Descript { get; set; }

    }
}
