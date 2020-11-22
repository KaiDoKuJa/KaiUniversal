using System;
using System.Text;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// Left Like Criteria
    /// </summary>
    public class LeftLikeCriteria : Criteria {

        /// <summary>
        /// {0} {Symbol} '%{1}'
        /// </summary>
        /// <returns></returns>
        public override string GetSql() {
            var c2 = base.ColValue as string;
            if (c2 == null) {
                throw new ArgumentException(LIKE_PATTERN_ERROR);
            }
            StringBuilder sb = new StringBuilder();
            return sb.Append(base.ColName)
                     .Append(base.Symbol)
                     .Append("'%")
                     .Append(c2.Replace("'", "''"))
                     .Append("' ").ToString();
        }
    }
}
