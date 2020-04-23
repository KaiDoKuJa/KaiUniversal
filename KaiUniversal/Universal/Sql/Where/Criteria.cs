namespace Kai.Universal.Sql.Where {

    /// <summary>
    /// v1.4.2020.2.22
    /// </summary>
    public class Criteria {

        public string ColName { get; set; }
        public CriteriaType CriteriaType { get; set; }
        public object ColValue { get; set; }
        public object[] ColValues { get; set; }

        /// <summary>
        /// 直接加條件
        /// </summary>
        /// <param name="condition">條件</param>
        public static Criteria AndCondition(string condition) {
            if (condition == null || "".Equals(condition.Trim())) {
                return null;
            }

            Criteria c = new Criteria();
            c.ColValue = condition;
            c.CriteriaType = CriteriaType.Direct;
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
            c.ColName = col;
            c.ColValue = val;
            c.CriteriaType = criteriaType;
            return c;
        }

        public static Criteria AndInCondition(string col, object[] vals) {
            if (vals == null || vals.Length == 0) {
                return null;
            }

            Criteria c = new Criteria();
            c.ColName = col;
            c.ColValues = vals;
            c.CriteriaType = CriteriaType.In;
            return c;
        }

        public static Criteria AndEmptyCondition(string col) {
            Criteria c = new Criteria();
            c.ColName = col;
            c.CriteriaType = CriteriaType.Equal;
            c.ColValue = "";
            return c;
        }

        // v1.3.2016.7.14
        public static Criteria AndIsNullCondition(string col) {
            Criteria c = new Criteria();
            c.ColName = col;
            c.CriteriaType = CriteriaType.IsNull;
            return c;
        }

        // v1.3.2016.7.14
        public static Criteria AndIsNotNullCondition(string col) {
            Criteria c = new Criteria();
            c.ColName = col;
            c.CriteriaType = CriteriaType.IsNotNull;
            return c;
        }

        public static Criteria AndLikeCondition(string col, string val) {
            if (val == null || "".Equals(val)) {
                return null;
            }

            Criteria c = new Criteria();
            c.ColName = col;
            c.ColValue = val;
            c.CriteriaType = CriteriaType.Like;
            return c;
        }

        public static Criteria AndLeftLikeCondition(string col, string val) {
            if (val == null || "".Equals(val)) {
                return null;
            }

            Criteria c = new Criteria();
            c.ColName = col;
            c.ColValue = val;
            c.CriteriaType = CriteriaType.LeftLike;
            return c;
        }

        public static Criteria AndRightLikeCondition(string col, string val) {
            if (val == null || "".Equals(val)) {
                return null;
            }

            Criteria c = new Criteria();
            c.ColName = col;
            c.ColValue = val;
            c.CriteriaType = CriteriaType.RightLike;
            return c;
        }
    }
}