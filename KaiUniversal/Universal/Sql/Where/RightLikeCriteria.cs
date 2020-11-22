using System;
using System.Text;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// Right Like Criteria
    /// </summary>
    public class RightLikeCriteria : Criteria {

        /// <summary>
        /// {0} {Symbol} '{1}%'
        /// </summary>
        /// <returns></returns>
        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            var c3 = base.ColValue as string;
            if (c3 != null) {
                sb.Append(base.ColName)
                  .Append(base.Symbol)
                  .Append("'")
                  .Append(c3.Replace("'", "''"))
                  .Append("%'");
            } else {
                throw new ArgumentException(LIKE_PATTERN_ERROR);
            }
            sb.Append(" ");
            return sb.ToString();
        }
    }
}
