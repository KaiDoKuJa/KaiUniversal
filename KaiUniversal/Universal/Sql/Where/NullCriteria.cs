using System.Text;

namespace Kai.Universal.Sql.Where {
    public class NullCriteria : Criteria {

        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            return sb.Append(base.ColName)
                     .Append(base.Symbol)
                     .Append(" ").ToString();
        }
    }
}
