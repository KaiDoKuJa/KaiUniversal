using System.Text;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// Null Criteria
    /// </summary>
    public class NullCriteria : Criteria {

        /// <summary>
        /// {0} {Symbol}
        /// </summary>
        /// <returns></returns>
        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            return sb.Append(base.ColName)
                     .Append(base.Symbol)
                     .Append(" ").ToString();
        }
    }
}
