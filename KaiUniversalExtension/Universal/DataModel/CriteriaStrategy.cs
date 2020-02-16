using Kai.Universal.Sql.Where;

namespace Kai.Universal.DataModel {
    public class CriteriaStrategy {

        public string CriteriaId { get; set; }
        public string ColName { get; set; }
        public CriteriaType CriteriaType { get; set; }
        public string ColMapping { get; set; }
        public string ReplacePattern { get; set; }
        public ReplaceMode ReplaceMode { get; set; }
    }
}
