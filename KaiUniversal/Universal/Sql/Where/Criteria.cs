using System;
#if !NET20
using System.Linq;
#endif

namespace Kai.Universal.Sql.Where {

    /// <summary>
    /// v1.4.2020.2.22
    /// v1.5.2020.6.25 to abstract
    /// </summary>
    public abstract class Criteria {

        internal static readonly string LIKE_PATTERN_ERROR = "like condition only support String object";

        /// <summary>
        /// the column name for generate
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// the column value for generate
        /// </summary>
        public object ColValue { get; set; }
        /// <summary>
        /// for generate some values
        /// <para>ex: in ('a','b',...)</para>
        /// </summary>
        public object[] ColValues { get; set; }
        /// <summary>
        /// the symbol for name and value
        /// <para>ex: like / equals / not equals / ...</para>
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// (abs-method) to generate sql
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// add criteria
        /// </summary>
        /// <param name="col"></param>
        /// <param name="criteriaType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
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
            var c = (Criteria)Activator.CreateInstance(attr.TypeofCriteria);
            c.Symbol = attr.Symbol;
            return c;
        }
        private static CriteriaReflectAttribute GetAttribute(CriteriaType value) {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
#if !NET20
            return enumType.GetField(name).GetCustomAttributes(false).OfType<CriteriaReflectAttribute>().SingleOrDefault();
#else
            return (CriteriaReflectAttribute)enumType.GetField(name).GetCustomAttributes(typeof(CriteriaReflectAttribute), false)[0];
#endif
        }

        /// <summary>
        /// add <see cref="InCriteria"/>
        /// </summary>
        /// <param name="col"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static Criteria AndInCondition(string col, object[] vals) {
            if (vals == null || vals.Length == 0) {
                return null;
            }

            Criteria c = new InCriteria();
            c.ColName = col;
            c.ColValues = vals;
            c.Symbol = " in ";
            return c;
        }

        /// <summary>
        /// add <see cref="CompareCriteria"/> with empty value
        /// <para>because default criteria will remove condition when empty/null value</para>
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Criteria AndEmptyCondition(string col) {
            Criteria c = new CompareCriteria();
            c.ColName = col;
            c.ColValue = "";
            c.Symbol = " = ";
            return c;
        }

        /// <summary>
        /// add <see cref="NullCriteria"/>
        /// <para>v1.3.2016.7.14 </para>
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Criteria AndIsNullCondition(string col) {
            Criteria c = new NullCriteria();
            c.ColName = col;
            c.Symbol = " is null";
            return c;
        }

        /// <summary>
        /// add <see cref="NullCriteria"/>
        /// <para>v1.3.2016.7.14</para>
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Criteria AndIsNotNullCondition(string col) {
            Criteria c = new NullCriteria();
            c.ColName = col;
            c.Symbol = " is not null";
            return c;
        }

    }
}