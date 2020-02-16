using Kai.Universal.Sql.Clause;
using Kai.Universal.Sql.Text;
using System;
using System.Text;

namespace Kai.Universal.Sql.Where {
    public class CriteriaUtility {

        private static readonly string LIKE_PATTERN_ERROR = "like condition only support String object";

        private CriteriaUtility() { }

        public static string GetCriteriaValues(object[] vals) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < vals.Length; i++) {
                if (i > 0) {
                    sb.Append(",");
                }
                sb.Append(GetCriteriaValue(vals[i]));
            }
            return sb.ToString();
        }

        public static string GetCriteriaValue(object propValue) {
            return OrmUtility.GetSqlString(propValue);
        }
   
        public static string GetCriterialTypeFormula(CriteriaType criteriaType) {
            string result = "";
            switch (criteriaType) {
                case CriteriaType.IsNull:
                    result = " is null";
                    break;

                case CriteriaType.IsNotNull:
                    result = " is not null";
                    break;

                case CriteriaType.Equal:
                    result = " = ";
                    break;

                case CriteriaType.NotEqual:
                    result = " <> ";
                    break;

                case CriteriaType.LessThan:
                    result = " < ";
                    break;

                case CriteriaType.LessThanEqual:
                    result = " <= ";
                    break;

                case CriteriaType.GreaterThan:
                    result = " > ";
                    break;

                case CriteriaType.GreaterThanEqual:
                    result = " >= ";
                    break;

                case CriteriaType.Like:
                case CriteriaType.LeftLike:
                case CriteriaType.RightLike:
                    result = " like ";
                    break;

                case CriteriaType.NotLike:
                case CriteriaType.NotLeftLike:
                case CriteriaType.NotRightLike:
                    result = " not like ";
                    break;

                case CriteriaType.In:
                    result = " in ";
                    break;

                default:
                    break;
            }
            return result;
        }
        public static string GetCriteriaSql(Criteria c) {
            StringBuilder sb = new StringBuilder();
            switch (c.CriteriaType) {
                case CriteriaType.Direct:
                    sb.Append(c.ColValue);
                    break;

                case CriteriaType.IsNull:
                case CriteriaType.IsNotNull:
                    sb.Append(c.ColName);
                    sb.Append(CriteriaUtility.GetCriterialTypeFormula(c.CriteriaType));
                    break;

                case CriteriaType.Equal:
                case CriteriaType.NotEqual:
                case CriteriaType.LessThan:
                case CriteriaType.LessThanEqual:
                case CriteriaType.GreaterThan:
                case CriteriaType.GreaterThanEqual:
                    sb.Append(c.ColName);
                    sb.Append(CriteriaUtility.GetCriterialTypeFormula(c.CriteriaType));
                    sb.Append(CriteriaUtility.GetCriteriaValue(c.ColValue));
                    break;

                case CriteriaType.Like:
                case CriteriaType.NotLike:
                    var c1 = c.ColValue as string;
                    if (c1 != null) {
                        sb.Append(c.ColName);
                        sb.Append(CriteriaUtility.GetCriterialTypeFormula(c.CriteriaType));
                        sb.Append("'%");
                        sb.Append(c1.Replace("'", "''"));
                        sb.Append("%'");
                    } else {
                        throw new ArgumentException(LIKE_PATTERN_ERROR);
                    }
                    break;

                case CriteriaType.LeftLike:
                case CriteriaType.NotLeftLike:
                    var c2 = c.ColValue as string;
                    if (c2 != null) {
                        sb.Append(c.ColName);
                        sb.Append(CriteriaUtility.GetCriterialTypeFormula(c.CriteriaType));
                        sb.Append("'%");
                        sb.Append(c2.Replace("'", "''"));
                        sb.Append("'");
                    } else {
                        throw new ArgumentException(LIKE_PATTERN_ERROR);
                    }
                    break;

                case CriteriaType.RightLike:
                case CriteriaType.NotRightLike:
                    var c3 = c.ColValue as string;
                    if (c3 != null) {
                        sb.Append(c.ColName);
                        sb.Append(CriteriaUtility.GetCriterialTypeFormula(c.CriteriaType));
                        sb.Append("'");
                        sb.Append(c3.Replace("'", "''"));
                        sb.Append("%'");
                    } else {
                        throw new ArgumentException(LIKE_PATTERN_ERROR);
                    }
                    break;

                case CriteriaType.In:
                    sb.Append(c.ColName);
                    sb.Append(CriteriaUtility.GetCriterialTypeFormula(c.CriteriaType));
                    if (c.ColValues != null && c.ColValues.Length > 0) {
                        sb.Append("(");
                        sb.Append(GetCriteriaValues(c.ColValues));
                        sb.Append(")");
                    } else if (c.ColValue != null) {
                        var specialString = c.ColValue as SpecialString;
                        if (specialString != null) {
                            sb.Append(specialString.Value);
                        } else if (c.ColValue is string) {
                            sb.Append(c.ColValue);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            sb.Append(" ");
            return sb.ToString();
        }

    }
}
