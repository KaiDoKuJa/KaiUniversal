using System;
using System.Collections;
using System.Collections.Generic;
using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Kai.Universal.Helper;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using Kai.Universal.Sql.Where;
using Kai.Universal.Text;

namespace Kai.Universal.Utility {
    //  com.kai.web.common.util > DynamicSqlUtil.java
    public class KaiSqlUtility {

        private KaiSqlUtility () { }

        public static string GetSelectCntSql (KaiSqlHelper helper, DmlInfoExtension dmlInfo, object data) {
            DmlHandler handler = helper.GetDmlHandler (dmlInfo);
            CriteriaStrategyContainer pool = helper.GetCriteriaStrategyContainer (dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo (pool, data);
            return handler.GetSql (QueryType.SelectCnt, modelInfo);
        }

        public static string GetSelectCntSql (KaiSqlHelper helper, string dmlId, string groupId, object data) {
            var dmlInfo = helper.GetDmlInfo (dmlId, groupId);
            return GetSelectCntSql(helper, dmlInfo, data);
        }

        public static string GetSelectTopSql (KaiSqlHelper helper, DmlInfoExtension dmlInfo, object data, int top) {
            DmlHandler handler = helper.GetDmlHandler (dmlInfo);
            CriteriaStrategyContainer pool = helper.GetCriteriaStrategyContainer (dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo (pool, data);
            modelInfo.Top = top;
            return handler.GetSql (QueryType.SelectTop, modelInfo);
        }

        public static string GetSelectTopSql (KaiSqlHelper helper, string dmlId, string groupId, object data, int top) {
            var dmlInfo = helper.GetDmlInfo (dmlId, groupId);
            return GetSelectTopSql (helper, dmlInfo, data, top);
        }

        public static DmlHandler GetExecutedDmlHandler (KaiSqlHelper helper, DmlInfoExtension dmlInfo, object data) {
            DmlHandler handler = helper.GetDmlHandler (dmlInfo);
            CriteriaStrategyContainer pool = helper.GetCriteriaStrategyContainer (dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo (pool, data);

            handler.GetSql (modelInfo);
            return handler;
        }

        public static DmlHandler GetExecutedDmlHandler (KaiSqlHelper helper, string dmlId, string groupId, object data) {
            var dmlInfo = helper.GetDmlInfo (dmlId, groupId);
            return GetExecutedDmlHandler(helper, dmlInfo, data);
        }

        private static ModelInfo RaiseModelInfo (CriteriaStrategyContainer container, object data) {
            if (container == null) return null;
            ModelInfo modelInfo = new ModelInfo ();

            List<CriteriaStrategy> criterias = container.Criterias;
            List<CriteriaStrategy> beforeReplacements = container.BeforeReplacements;
            List<CriteriaStrategy> afterReplacements = container.AfterReplacements;

            CriteriaPool criteriaPool = new CriteriaPool ();
            LoadCriterias (criterias, data, ref criteriaPool);
            LoadBeforeReplacements (beforeReplacements, data, ref criteriaPool);
            modelInfo.Criterias = criteriaPool;
            modelInfo.Replacements = RaiseReplacements (afterReplacements, data);

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
        private static void LoadBeforeReplacements (List<CriteriaStrategy> beforeReplacements, object data, ref CriteriaPool criteriaPool) {
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
                    value = ReflectUtility.GetValue (data, vo.ColMapping);
                }

                string colName = vo.ColName;
                string s = GetSqlString (value);
                if (string.IsNullOrWhiteSpace (colName) || string.IsNullOrWhiteSpace (s)) {
                    continue;
                }
                criteriaPool.AddCriteria (Criteria.AndCondition (colName.Replace (vo.ReplacePattern, s)));
            }
        }

        private static void LoadCriterias (List<CriteriaStrategy> criterias, object data, ref CriteriaPool criteriaPool) {
            bool isMapModel = data is IDictionary;

            // 使用colName, criteriaType, value
            foreach (CriteriaStrategy vo in criterias) {
                string colName = vo.ColName;
                if (string.IsNullOrWhiteSpace (colName)) {
                    continue;
                }

                Criteria criteria = RaiseCriteria (vo, data, isMapModel);
                criteriaPool.AddCriteria (criteria);
            }
        }

        /// <summary>
        /// AFTER (事後針對TB_MODEL主體)  
        /// case [without criteria] : "select * from tbl where 1=1 and seq=$1" -> replace $1 by {value}
        /// case [with criteria] : "select * from tbl where 1=1 $1" -> replace $1 by "and {criteria}"
        /// 
        /// </summary>
        /// <param name="afterReplacements"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<Replacement> RaiseReplacements (List<CriteriaStrategy> afterReplacements, object data) {

            bool isMapModel = data is IDictionary;

            List<Replacement> rPool = new List<Replacement> ();
            foreach (CriteriaStrategy vo in afterReplacements) {
                Replacement r = new Replacement ();
                string colName = vo.ColName;

                r.ReplacePattern = vo.ReplacePattern;
                if (!string.IsNullOrWhiteSpace (colName)) {
                    Criteria criteria = RaiseCriteria (vo, data, isMapModel);
                    if (criteria != null) {
                        r.Value = " and " + CriteriaUtility.GetCriteriaSql (criteria);
                        rPool.Add (r);
                    }
                } else {
                    object value;
                    if (isMapModel) {
                        var map = data as IDictionary;
                        value = map[vo.ColMapping];
                    } else {
                        value = ReflectUtility.GetValue (data, vo.ColMapping);
                    }
                    string s = GetSqlString (value);
                    if (!string.IsNullOrWhiteSpace (s)) {
                        r.Value = s;
                        rPool.Add (r);
                    }
                }
            }

            return rPool;
        }

        private static Criteria RaiseCriteria (CriteriaStrategy vo, object data, bool isMapModel) {
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
                    value = ReflectUtility.GetValue (data, vo.ColMapping);
                }

                if (CriteriaType.In != vo.CriteriaType) {
                    result = Criteria.AndCondition (vo.ColName, vo.CriteriaType, value);
                } else {
                    if (ReflectUtility.IsList (value)) {
                        // TODO : 需驗證 UT
                        var list = (IList) value;
                        var array = new object[list.Count];
                        list.CopyTo (array, 0);
                        if (list.Count > 0) {
                            result = Criteria.AndInCondition (vo.ColName, array);
                        }
                    } else {
                        result = Criteria.AndCondition (vo.ColName, CriteriaType.Equal, value);
                    }
                }
            } else {
                result = Criteria.AndCondition (vo.ColName); // correct, don't use value.
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
        private static string GetSqlString (object value) {
            if (value == null) return null;
            string s = null;
            if (ReflectUtility.IsList (value)) {
                // TODO : 需驗證 UT
                var list = (IList) value;
                var array = new object[list.Count];
                list.CopyTo (array, 0);
                if (list.Count > 0) {
                    s = CriteriaUtility.GetCriteriaValues (array);
                }
            } else if (value is string x) {
                if (!string.IsNullOrWhiteSpace (x)) {
                    s = CriteriaUtility.GetCriteriaValue (x);
                }
            } else {
                s = CriteriaUtility.GetCriteriaValue (value);
            }
            return s;
        }

    }
}