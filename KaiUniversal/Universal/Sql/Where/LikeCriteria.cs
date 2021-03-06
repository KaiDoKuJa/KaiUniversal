﻿using System;
using System.Text;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// Like Criteria
    /// </summary>
    public class LikeCriteria : Criteria {

        /// <summary>
        /// {0} {Symbol} '%{1}%'
        /// </summary>
        /// <returns></returns>
        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            var c1 = base.ColValue as string;
            if (c1 != null) {
                sb.Append(base.ColName)
                  .Append(base.Symbol)
                  .Append("'%")
                  .Append(c1.Replace("'", "''"))
                  .Append("%'");
            } else {
                throw new ArgumentException(LIKE_PATTERN_ERROR);
            }
            sb.Append(" ");
            return sb.ToString();
        }
    }
}
