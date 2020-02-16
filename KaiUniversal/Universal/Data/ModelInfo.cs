using Kai.Universal.Sql.Where;
using Kai.Universal.Sql.Result;
using System;
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

        public SqlGeneratorMode Mode { get; set; }

        public ModelInfo() { }
        public ModelInfo(Object model) {
            this.Model = model;
        }

    }
}
