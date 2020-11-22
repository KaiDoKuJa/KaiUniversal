using System;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// the criteria reflect attr, for generate sql symbol 
    /// </summary>
    public class CriteriaReflectAttribute : Attribute {

        /// <summary>
        /// the type of criteria
        /// </summary>
        public System.Type TypeofCriteria { get; set; }
        /// <summary>
        /// the symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="typeofCrietria"></param>
        public CriteriaReflectAttribute(System.Type typeofCrietria) {
            TypeofCriteria = typeofCrietria;
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="typeofCrietria"></param>
        /// <param name="symbol"></param>
        public CriteriaReflectAttribute(System.Type typeofCrietria, string symbol) {
            TypeofCriteria = typeofCrietria;
            Symbol = symbol;
        }
    }
}
