using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Kai.Universal.Sql.Clause;
using Kai.Universal.Sql.Where;
using Kai.Universal.Text;
using System.Collections;
using System.Collections.Generic;

namespace Kai.Universal.Utility {

    /// <summary>
    /// KaiSql Utility
    /// </summary>
    public static class KaiSqlUtility {

        /// <summary>
        /// raise ModelInfo
        /// </summary>
        /// <param name="container"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ModelInfo RaiseModelInfo(CriteriaStrategyContainer container, object data) {
            if (container == null) return null;
            ModelInfo modelInfo = new ModelInfo();

            List<CriteriaStrategy> criterias = container.Criterias;
            List<CriteriaStrategy> beforeReplacements = container.BeforeReplacements;
            List<CriteriaStrategy> afterReplacements = container.AfterReplacements;

            CriteriaPool criteriaPool = new CriteriaPool();
            LoadCriterias(criterias, data, ref criteriaPool);
            LoadBeforeReplacements(beforeReplacements, data, ref criteriaPool);
            modelInfo.Criterias = criteriaPool;
            modelInfo.Replacements = RaiseReplacements(afterReplacements, data);

            return modelInfo;
        }

        /// <summary>
        /// beforeReplacements :
        ///     colName = exists (select 1 from tbl where xxx=${1})
        ///     replacePattern = ${1}
        ///     replaceMode = before
        /// 
        /// </summary>
        /// <param name="beforeReplacements"></param>
        /// <param name="data"></param>
        /// <param name="criteriaPool"></param>
        private static void LoadBeforeReplacements(List<CriteriaStrategy> beforeReplacements, object data, ref CriteriaPool criteriaPool) {
            bool isMapModel = false;
            var map = data as IDictionary;
            if (map != null) {
                isMapModel = true;
            }

            // 使用 colName, value, replacePattern
            foreach (CriteriaStrategy vo in beforeReplacements) {
                object value;
                if (isMapModel) {
                    value = map[vo.ColMapping];
                } else {
                    value = ReflectUtility.GetValue(data, vo.ColMapping);
                }

                string colName = vo.ColName;
                string s = GetSqlString(value);
                if (IsNullOrWhiteSpace(colName) || IsNullOrWhiteSpace(s)) {
                    continue;
                }
                criteriaPool.AddCriteria(Criteria.AndCondition(colName.Replace(vo.ReplacePattern, s)));
            }
        }

        private static void LoadCriterias(List<CriteriaStrategy> criterias, object data, ref CriteriaPool criteriaPool) {
            bool isMapModel = data is IDictionary;

            // 使用colName, criteriaType, value
            foreach (CriteriaStrategy vo in criterias) {
                string colName = vo.ColName;
                if (IsNullOrWhiteSpace(colName)) {
                    continue;
                }

                Criteria criteria = RaiseCriteria(vo, data, isMapModel);
                criteriaPool.AddCriteria(criteria);
            }
        }

        /// <summary>
        /// AFTER (事後針對TB_MODEL主體)  
        /// <para>case [without criteria] : "select * from tbl where 1=1 and seq=$1" -> replace $1 by {value}
        ///                  mandatory params : ColMapping, ReplacePattern</para>
        /// <para>case [with criteria] : "select * from tbl where 1=1 $1" -> replace $1 by "and {criteria}"
        ///                  mandatory params : ColName, CriteriaType, ColMapping, ReplacePattern</para>
        /// 
        /// </summary>
        /// <param name="afterReplacements"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<Replacement> RaiseReplacements(List<CriteriaStrategy> afterReplacements, object data) {

            bool isMapModel = data is IDictionary;

            List<Replacement> rPool = new List<Replacement>();
            foreach (CriteriaStrategy vo in afterReplacements) {
                Replacement r = new Replacement();
                string colName = vo.ColName;

                r.ReplacePattern = vo.ReplacePattern;
                if (!IsNullOrWhiteSpace(colName)) {
                    Criteria criteria = RaiseCriteria(vo, data, isMapModel);
                    if (criteria != null) {
                        r.Value = " and " + criteria.GetSql();
                        rPool.Add(r);
                    }
                } else {
                    object value;
                    if (isMapModel) {
                        var map = data as IDictionary;
                        value = map[vo.ColMapping];
                    } else {
                        value = ReflectUtility.GetValue(data, vo.ColMapping);
                    }
                    string s = GetSqlString(value);
                    if (!IsNullOrWhiteSpace(s)) {
                        r.Value = s;
                        rPool.Add(r);
                    }
                }
            }

            return rPool;
        }

        private static Criteria RaiseCriteria(CriteriaStrategy vo, object data, bool isMapModel) {
            Criteria result = null;

            if (CriteriaType.Direct != vo.CriteriaType) {
                object value;
                if (isMapModel) {
                    var map = data as IDictionary;
                    if (map == null || vo.ColMapping == null) {
                        value = null;
                    } else {
                        value = map[vo.ColMapping];
                    }
                } else {
                    value = ReflectUtility.GetValue(data, vo.ColMapping);
                }

                if (value == null) return result;

                if (CriteriaType.In != vo.CriteriaType) {
                    result = Criteria.AndCondition(vo.ColName, vo.CriteriaType, value);
                } else {
                    if (ReflectUtility.IsList(value)) {
                        var list = (IList)value;
                        var array = new object[list.Count];
                        list.CopyTo(array, 0);
                        if (list.Count > 0) {
                            result = Criteria.AndInCondition(vo.ColName, array);
                        }
                    } else {
                        result = Criteria.AndCondition(vo.ColName, CriteriaType.Equal, value);
                    }
                }
            } else {
                result = Criteria.AndCondition(vo.ColName); // correct, don't use value.
            }

            return result;
        }

        /// <summary>
        /// 1. List -> 'x','y', ...
        /// 2. string -> 'x'
        /// 3. not null -> to sql string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetSqlString(object value) {
            if (value == null) return null;
            string s = null;
            if (ReflectUtility.IsList(value)) {
                var list = (IList)value;
                var array = new object[list.Count];
                list.CopyTo(array, 0);
                if (list.Count > 0) {
                    s = OrmUtility.GetArraySqlString(array);
                }
            } else if (value is string x) {
                if (!IsNullOrWhiteSpace(x)) {
                    s = OrmUtility.GetSqlString(x);
                }
            } else {
                s = OrmUtility.GetSqlString(value);
            }
            return s;
        }

        /// <summary>
        /// is null or whitespace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(string value) {
            if (value == null) {
                return true;
            }
            for (int i = 0; i < value.Length; i++) {
                if (!char.IsWhiteSpace(value[i])) {
                    return false;
                }
            }
            return true;
        }
    }
}
