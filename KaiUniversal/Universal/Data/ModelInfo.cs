using Kai.Universal.Sql.Where;
using System.Collections.Generic;

namespace Kai.Universal.Data {
    public class ModelInfo {

        public object Model { get; set; }
        public object OriginModel { get; set; } // before value

        public CriteriaPool Criterias { get; set; }
        public List<Replacement> Replacements { get; set; }

        public string OrderBy { get; set; }
        public string GroupBy { get; set; }

        public int Top { get; set; }
        public int PageNumber { get; set; }
        public int EachPageSize { get; set; }

        public ModelInfo() { }
        public ModelInfo(object model) {
            this.Model = model;
        }

    }
}
