using System.Text;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// Direct Criteria
    /// </summary>
    public class DirectCriteria : Criteria {

        /// <summary>
        /// {0}
        /// </summary>
        /// <returns></returns>
        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            return sb.Append(base.ColValue).Append(" ").ToString();
        }
    }
}
