using Kai.Universal.Sql.Where;
using Kai.Universal.Sql.Result;
using Kai.Universal.Sql.Where;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private SqlGeneratorMode mode = SqlGeneratorMode.Statement; // for insert.update.delete
        public SqlGeneratorMode Mode { get => mode; set => mode = value; }

        public ModelInfo() { }
        public ModelInfo(Object model) {
            this.Model = model;
        }

    }
}
