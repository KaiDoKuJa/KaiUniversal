using System;
namespace Kai.Universal.Sql.Where {

    /// <summary>
    /// v1.3.2016.7.14
    /// </summary>
    public class Criteria {
        public string colName;
        public CriteriaType criteriaType;
        public object colValue;
        public object[] colValues;

        /// <summary>
        /// 直接加條件
        /// </summary>
        /// <param name="condition">條件</param>
        public static Criteria AndCondition(string condition) {
            if (condition == null || "".Equals(condition.Trim())) {
                return null;
            }

            Criteria c = new Criteria();
            c.colValue = condition;
            c.criteriaType = CriteriaType.Direct;
            return c;
        }

        public static Criteria AndCondition(string col, CriteriaType criteriaType, object val) {
            if (col == null || "".Equals(col.Trim())) {
                return null;
            }
            if (val == null) {
                return null;
            }
            if (val is string && "".Equals(val)) {
                return null;
            }

            Criteria c = new Criteria();
            c.colName = col;
            c.colValue = val;
            c.criteriaType = criteriaType;
            return c;
        }

        public static Criteria AndInCondition(string col, object[] vals) {
            if (vals == null || vals.Length == 0) {
                return null;
            }

            Criteria c = new Criteria();
            c.colName = col;
            c.colValues = vals;
            c.criteriaType = CriteriaType.In;
            return c;
        }

        public static Criteria AndEmptyCondition(string col) {
            Criteria c = new Criteria();
            c.colName = col;
            c.criteriaType = CriteriaType.Equal;
            c.colValue = "";
            return c;
        }

        // v1.3.2016.7.14
        public static Criteria AndIsNullCondition(string col) {
            Criteria c = new Criteria();
            c.colName = col;
            c.criteriaType = CriteriaType.IsNull;
            return c;
        }

        // v1.3.2016.7.14
        public static Criteria AndIsNotNullCondition(string col) {
            Criteria c = new Criteria();
            c.colName = col;
            c.criteriaType = CriteriaType.IsNotNull;
            return c;
        }

        public static Criteria AndLikeCondition(string col, string val) {
            if (val == null || "".Equals(val)) {
                return null;
            }
            
            Criteria c = new Criteria();
            c.colName = col;
            c.colValue = val;
            c.criteriaType = CriteriaType.Like;
            return c;
        }

        public static Criteria AndLeftLikeCondition(string col, string val) {
            if (val == null || "".Equals(val)) {
                return null;
            }

            Criteria c = new Criteria();
            c.colName = col;
            c.colValue = val;
            c.criteriaType = CriteriaType.LeftLike;
            return c;
        }

        public static Criteria AndRightLikeCondition(string col, string val) {
            if (val == null || "".Equals(val)) {
                return null;
            }

            Criteria c = new Criteria();
            c.colName = col;
            c.colValue = val;
            c.criteriaType = CriteriaType.RightLike;
            return c;
        }
    }
}