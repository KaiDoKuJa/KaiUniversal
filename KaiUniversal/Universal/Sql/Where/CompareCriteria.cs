using Kai.Universal.Sql.Clause;
using System.Text;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// Compare criteria
    /// </summary>
    public class CompareCriteria : Criteria {

        /// <summary>
        /// {0} {Symbol} {1}
        /// </summary>
        /// <returns></returns>
        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            return sb.Append(base.ColName)
                     .Append(base.Symbol)
                     .Append(OrmUtility.GetSqlString(base.ColValue))
                     .Append(" ").ToString();
        }
    }
}
