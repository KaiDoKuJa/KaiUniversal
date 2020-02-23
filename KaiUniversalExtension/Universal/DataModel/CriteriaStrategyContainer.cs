using System.Collections.Generic;

namespace Kai.Universal.DataModel {
    public class CriteriaStrategyContainer {

        public List<CriteriaStrategy> Criterias { get; set; } = new List<CriteriaStrategy>();
        public List<CriteriaStrategy> BeforeReplacements { get; set; } = new List<CriteriaStrategy>();
        public List<CriteriaStrategy> AfterReplacements { get; set; } = new List<CriteriaStrategy>();

    }
}
