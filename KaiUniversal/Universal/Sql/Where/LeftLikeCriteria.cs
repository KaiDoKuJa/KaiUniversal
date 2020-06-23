using System;
using System.Text;

namespace Kai.Universal.Sql.Where {
    public class LeftLikeCriteria : Criteria {

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
