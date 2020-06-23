using Kai.Universal.Sql.Clause;
using System.Text;

namespace Kai.Universal.Sql.Where {
    public class CompareCriteria : Criteria {

        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            return sb.Append(base.ColName)
                     .Append(base.Symbol)
                     .Append(OrmUtility.GetSqlString(base.ColValue))
                     .Append(" ").ToString();
        }
    }
}
