using System.Collections.Generic;

namespace Kai.Universal.DataModel {
    /// <summary>
    /// CriteriaStrategy container
    /// <para>分類CriteriaStrategy</para>
    /// </summary>
    public class CriteriaStrategyContainer {

        /// <summary>
        /// Criterias
        /// </summary>
        public List<CriteriaStrategy> Criterias { get; set; } = new List<CriteriaStrategy>();
        /// <summary>
        /// BeforeReplace
        /// </summary>
        public List<CriteriaStrategy> BeforeReplacements { get; set; } = new List<CriteriaStrategy>();
        /// <summary>
        /// AfterReplace
        /// </summary>
        public List<CriteriaStrategy> AfterReplacements { get; set; } = new List<CriteriaStrategy>();

    }
}
