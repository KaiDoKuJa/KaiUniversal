﻿using Kai.Universal.Sql.Clause;
using Kai.Universal.Sql.Text;
using System.Text;

namespace Kai.Universal.Sql.Where {
    /// <summary>
    /// In Criteria
    /// </summary>
    public class InCriteria : Criteria {

        /// <summary>
        /// {0} in ({1}, {2}, {3}, ...)
        /// </summary>
        /// <returns></returns>
        public override string GetSql() {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ColName)
              .Append(base.Symbol);
            if (base.ColValues != null && base.ColValues.Length > 0) {
                sb.Append("(")
                  .Append(OrmUtility.GetArraySqlString(base.ColValues))
                  .Append(")");
            } else if (base.ColValue != null) {
                var specialString = base.ColValue as SpecialString;
                if (specialString != null) {
                    sb.Append(specialString.Value);
                } else if (base.ColValue is string) {
                    sb.Append(base.ColValue);
                }
            }
            sb.Append(" ");
            return sb.ToString();
        }

    }
}
