using System;
using System.Linq;

namespace Kai.Universal.Sql.Where {

    /// <summary>
    /// v1.4.2020.2.22
    /// v1.5.2020.6.25 to abstract
    /// </summary>
    public abstract class Criteria {

        public static readonly string LIKE_PATTERN_ERROR = "like condition only support String object";

        public string ColName { get; set; }
        public object ColValue { get; set; }
        public object[] ColValues { get; set; }
        public string Symbol { get; set; }

        public abstract string GetSql();

        /// <summary>
        /// 直接加條件
        /// </summary>
        /// <param name="condition">條件</param>
        public static Criteria AndCondition(string condition) {
            if (condition == null || "".Equals(condition.Trim())) {
                return null;
            }

            Criteria c = new DirectCriteria();
            c.ColValue = condition;
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
            Criteria c = GetCriteriaByType(criteriaType);
            c.ColName = col;
            c.ColValue = val;
            return c;
        }

        private static Criteria GetCriteriaByType(CriteriaType criteriaType) {
            var attr = GetAttribute(criteriaType);
            var c = (Criteria) Activator.CreateInstance(attr.TypeofCriteria);
            c.Symbol = attr.Symbol;
            return c;
        }
        private static CriteriaReflectAttribute GetAttribute(CriteriaType value) {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            //value.GetType().GetProperties().FirstOrDefault()
            return enumType.GetField(name).GetCustomAttributes(false).OfType<CriteriaReflectAttribute>().SingleOrDefault();
        }

        public static Criteria AndInCondition(string col, object[] vals) {
            if (vals == null || vals.Length == 0) {
                return null;
            }

            Criteria c = new InCriteria();
            c.ColName = col;
            c.ColValues = vals;
            return c;
        }

        public static Criteria AndEmptyCondition(string col) {
            Criteria c = new CompareCriteria();
            c.ColName = col;
            c.Symbol = " = ";
            c.ColValue = "";
            return c;
        }

        // v1.3.2016.7.14
        public static Criteria AndIsNullCondition(string col) {
            Criteria c = new NullCriteria();
            c.ColName = col;
            c.Symbol = " is null";
            return c;
        }

        // v1.3.2016.7.14
        public static Criteria AndIsNotNullCondition(string col) {
            Criteria c = new NullCriteria();
            c.ColName = col;
            c.Symbol = " is not null";
            return c;
        }
        
    }
}