using System;
using System.Collections.Generic;
using System.Text;

namespace Kai.Universal.Sql.Where {
    public class CriteriaReflectAttribute : Attribute {

        public System.Type TypeofCriteria { get; set; }
        public string Symbol { get; set; }

        public CriteriaReflectAttribute(System.Type typeofCrietria) {
            TypeofCriteria = typeofCrietria;
        }
        public CriteriaReflectAttribute(System.Type typeofCrietria, string symbol) {
            TypeofCriteria = typeofCrietria;
            Symbol = symbol;
        }
    }
}
