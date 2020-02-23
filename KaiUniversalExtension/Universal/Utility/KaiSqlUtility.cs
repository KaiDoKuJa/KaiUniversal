using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Kai.Universal.Sql.Where;
using Kai.Universal.Text;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kai.Universal.Utility {
    //  com.kai.web.common.util > DynamicSqlUtil.java
    public class KaiSqlUtility {

        private KaiSqlUtility() { }

        public static ModelInfo RaiseModelInfo(CriteriaStrategyContainer container, object data) {
            if (container == null || data == null) return null;
            ModelInfo modelInfo = new ModelInfo();

            List<CriteriaStrategy> criterias = container.Criterias;
            List<CriteriaStrategy> beforeReplacements = container.BeforeReplacements;
            List<CriteriaStrategy> afterReplacements = container.AfterReplacements;

            modelInfo.Criterias = RaiseCriteriaPool(criterias, beforeReplacements, data);
            modelInfo.Replacements = RaiseReplacements(afterReplacements, data);

            return modelInfo;
        }

        /// <summary>
        /// AFTER (事後針對TB_MODEL主體)  
        /// case [without criteria] : "select * from tbl where 1=1 and seq=$1" -> replace $1 by {value}
        /// case [with criteria] : "select * from tbl where 1=1 $1" -> replace $1 by "and {criteria}"
        /// 
        /// only for sqlTemplate
        /// </summary>
        /// <param name="afterReplacements"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<Replacement> RaiseReplacements(List<CriteriaStrategy> afterReplacements, object data) {
            bool isMapModel = false;
            var map = data as Dictionary<string, object>;
            if (map != null) {
                isMapModel = true;
            }

            List<Replacement> rPool = new List<Replacement>();
            foreach (CriteriaStrategy vo in afterReplacements) {
                Replacement r = new Replacement();
                string colName = vo.ColName;

                object value;
                if (isMapModel) {
                    value = map[vo.ColMapping];
                } else {
                    value = ReflectUtility.GetValue(data, vo.ColMapping);
                }

                r.ReplacePattern = vo.ReplacePattern;
                if (!string.IsNullOrWhiteSpace(colName)) { 
                    Criteria criteria = RaiseCriteria(colName, vo.CriteriaType, value);
                    string s = "";
                    if (criteria != null) {
                        s = " and " + CriteriaUtility.GetCriteriaSql(criteria);
                    }
                    r.Value = s;
                } else {
                    string s = GetSqlString(value);
                    if (!string.IsNullOrWhiteSpace(s)) {
                        r.Value = s;
                    }
                }
                rPool.Add(r);
            }

            return rPool;
        }
        /// <summary>
        /// beforeReplacements :
        ///     colName = exists (select 1 from tbl where xxx=${1})
        ///     replacePattern = ${1}
        ///     replaceMode = before
        /// 
        /// </summary>
        /// <param name="criterias"></param>
        /// <param name="beforeReplacements"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static CriteriaPool RaiseCriteriaPool(List<CriteriaStrategy> criterias,
                List<CriteriaStrategy> beforeReplacements, object data) {
            bool isMapModel = false;
            var map = data as Dictionary<string, object>;
            if (map != null) {
                isMapModel = true;
            }

            CriteriaPool cPool = new CriteriaPool();
            // 使用colName, criteriaType, value
            foreach (CriteriaStrategy vo in criterias) {
                string colName = vo.ColName;
                if (string.IsNullOrWhiteSpace(colName)) {
                    continue;
                }
                
                object value;
                if (isMapModel) {
                    value = map[vo.ColMapping];
                } else {
                    value = ReflectUtility.GetValue(data, vo.ColMapping);
                }


                Criteria criteria = RaiseCriteria(colName, vo.CriteriaType, value);
                cPool.AddCriteria(criteria);
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
                if (string.IsNullOrWhiteSpace(colName) || string.IsNullOrWhiteSpace(s)) {
                    continue;
                }
                cPool.AddCriteria(Criteria.AndCondition(colName.Replace(vo.ReplacePattern, s)));
            }

            return cPool;
        }

        private static Criteria RaiseCriteria(string colName, CriteriaType criteriaType, object value) {
            Criteria result = null;
            try {
                switch (criteriaType) {
                    case CriteriaType.Direct:
                        result = Criteria.AndCondition(colName);
                        break;
                    case CriteriaType.In:
                        if (ReflectUtility.IsList(value)) {
                            // TODO : 需驗證 UT
                            var list = (IList)value;
                            var array = new object[list.Count];
                            list.CopyTo(array, 0);
                            if (list.Count > 0) {
                                result = Criteria.AndInCondition(colName, array);
                            }
                        } else {
                            result = Criteria.AndCondition(colName, CriteriaType.Equal, value);
                        }
                        break;
                    default:
                        result = Criteria.AndCondition(colName, criteriaType, value);
                        break;
                }
            } catch (Exception e) {
                return result;
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
                // TODO : 需驗證 UT
                var list = (IList)value;
                var array = new object[list.Count];
                list.CopyTo(array, 0);
                if (list.Count > 0) {
                    s = CriteriaUtility.GetCriteriaValues(array);
                }
            } else if (value is string x) {
                if (!string.IsNullOrWhiteSpace(x)) {
                    s = CriteriaUtility.GetCriteriaValue(x);
                }
            } else {
                s = CriteriaUtility.GetCriteriaValue(value);
            }
            return s;
        }

    }
}