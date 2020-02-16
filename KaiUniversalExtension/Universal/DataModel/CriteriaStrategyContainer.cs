using System.Collections.Generic;

namespace Kai.Universal.DataModel {
    public class CriteriaStrategyContainer {

        public List<CriteriaStrategy> Criterias { get; set; }
        public List<CriteriaStrategy> BeforeReplacements { get; set; }
        public List<CriteriaStrategy> AfterReplacements { get; set; }

    }
}
