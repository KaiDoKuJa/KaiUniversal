using System.Text;

namespace Kai.Universal.Sql.Where {
    public class DirectCriteria : Criteria {

        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            return sb.Append(base.ColValue).Append(" ").ToString();
        }
    }
}
