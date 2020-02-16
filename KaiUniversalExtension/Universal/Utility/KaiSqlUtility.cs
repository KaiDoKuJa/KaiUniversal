using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Kai.Universal.Sql.Where;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Kai.Universal.Utility {
    //  com.kai.web.common.util > DynamicSqlUtil.java
    public class KaiSqlUtility {

        private KaiSqlUtility() { }

        private static void LoadModelInfo(ref ModelInfo modelInfo, CriteriaStrategyContainer criteriaStrategyPool, Dictionary<string, object> data) {
            // TODO : modelInfo no check!
            // prepare criteria
            if (criteriaStrategyPool == null || data == null || data.Count == 0) return;
            List<CriteriaStrategy> criterias = criteriaStrategyPool.Criterias;
            List<CriteriaStrategy> beforeReplacements = criteriaStrategyPool.BeforeReplacements;
            List<CriteriaStrategy> afterReplacements = criteriaStrategyPool.AfterReplacements;

            CriteriaPool cPool = RaiseCriteriaPool(criterias, beforeReplacements, data);
            List<Replacement> rPool = RaiseReplacements(afterReplacements, data);
            modelInfo.Criterias = cPool;
            modelInfo.Replacements = rPool;
        }

        public static ModelInfo RaiseModelInfo(CriteriaStrategyContainer criteriaStrategyPool, Dictionary<string, object> data) {
            ModelInfo modelInfo = new ModelInfo();
            LoadModelInfo(ref modelInfo, criteriaStrategyPool, data);
            return modelInfo;
        }

        private static List<Replacement> RaiseReplacements(List<CriteriaStrategy> afterReplacements,
                Dictionary<string, object> data) {
            List<Replacement> rPool = new List<Replacement>();
            foreach (CriteriaStrategy vo in afterReplacements) {
                Replacement r = new Replacement();
                string colName = vo.ColName;
                object value = data[vo.ColMapping];

                r.ReplacePattern = vo.ReplacePattern;
                if (!string.IsNullOrWhiteSpace(colName)) {
                    Criteria criteria = RaiseCriteria(colName, vo.CriteriaType, value);
                    string s = "";
                    if (criteria != null) {
                        s = " and " + CriteriaUtility.GetCriteriaSql(criteria);
                    }
                    r.Value = s;
                } else {
                    string s = GetBeforeReplaceString(value);
                    if (!string.IsNullOrWhiteSpace(colName)) {
                        r.Value = s;
                    }
                }
                rPool.Add(r);
            }

            return rPool;
        }

        private static CriteriaPool RaiseCriteriaPool(List<CriteriaStrategy> criterias,
                List<CriteriaStrategy> beforeReplacements, Dictionary<string, object> data) {
            CriteriaPool cPool = new CriteriaPool();
            // 使用colName, criteriaType, value
            foreach (CriteriaStrategy vo in criterias) {
                string colName = vo.ColName;
                if (string.IsNullOrWhiteSpace(colName)) {
                    continue;
                }
                object value = data[vo.ColMapping];
                Criteria criteria = RaiseCriteria(colName, vo.CriteriaType, value);
                cPool.AddCriteria(criteria);
            }
            // 使用 colName, value, replacePattern
            foreach (CriteriaStrategy vo in beforeReplacements) {
                object value = data[vo.ColMapping];
                string colName = vo.ColName;
                string s = GetBeforeReplaceString(value);
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
                        if (IsList(value)) {
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

        private static string GetBeforeReplaceString(object value) {
            string s = null;
            if (IsList(value)) {
                // TODO : 需驗證 UT
                var list = (IList)value;
                var array = new object[list.Count];
                list.CopyTo(array, 0);
                if (list.Count > 0) {
                    s = CriteriaUtility.GetCriteriaValues(array);
                }
            } else if (value is string) {
                if (!string.IsNullOrWhiteSpace((string)value)) {  //StringUtil.isNotBlank((string)value)) {
                    s = CriteriaUtility.GetCriteriaValue(value);
                }
            } else if (value != null) {
                s = CriteriaUtility.GetCriteriaValue(value);
            }
            return s;
        }

        public static bool IsList(object o) {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsDictionary(object o) {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }


    }
}