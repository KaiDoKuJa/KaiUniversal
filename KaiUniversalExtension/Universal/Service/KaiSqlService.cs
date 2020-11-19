using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using Kai.Universal.Utility;
using System;
using System.Collections.Generic;

namespace Kai.Universal.Service {
    /// <summary>
    /// KaiSqlService
    /// </summary>
    public class KaiSqlService {

        /// <summary>
        /// the DmlHandlers
        /// <para>groupId -> dmlId -> Dmlhandler</para>
        /// </summary>
        public Dictionary<string, Dictionary<string, DmlHandler>> DmlHandlers { get; set; }
        /// <summary>
        /// all DmlInfos
        /// </summary>
        public List<DmlInfoExtension> DmlInfos { get; set; }

        /// <summary>
        /// all criteriaStrategy
        /// <para>criteriaId -> Container</para>
        /// </summary>
        public Dictionary<string, CriteriaStrategyContainer> CriteriaStrategyContainers { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public KaiSqlService() { }

        /// <summary>
        /// get DmlInfo by dmlId+groupId
        /// </summary>
        /// <param name="dmlId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public DmlInfoExtension GetDmlInfo(string dmlId, string groupId) {
            if (KaiSqlUtility.IsNullOrWhiteSpace(dmlId) || KaiSqlUtility.IsNullOrWhiteSpace(groupId)) return null;
            if (DmlInfos == null || DmlInfos.Count == 0) return null;
            foreach (var item in DmlInfos) {
                if (dmlId.Equals(item.DmlId) && groupId.Equals(item.GroupId))
                    return item;
            }
            return null;
        }

        /// <summary>
        /// get Criteria by id
        /// </summary>
        /// <param name="criteriaId"></param>
        /// <returns></returns>
        private CriteriaStrategyContainer GetCriteriaStrategyContainer(string criteriaId) {
            return CriteriaStrategyContainers[criteriaId];
        }

        /// <summary>
        /// get Criteria by dmlInfo.CriteriaId
        /// </summary>
        /// <param name="dmlInfo"></param>
        /// <returns></returns>
        public CriteriaStrategyContainer GetCriteriaStrategyContainer(DmlInfoExtension dmlInfo) {
            if (dmlInfo == null || dmlInfo.CriteriaId == null) return null;
            return GetCriteriaStrategyContainer(dmlInfo.CriteriaId);
        }

        /// <summary>
        /// get Criteria by dmlId+groupId to find DmlInfo.CriteriaId
        /// </summary>
        /// <param name="dmlId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public CriteriaStrategyContainer GetCriteriaStrategyContainer(string dmlId, string groupId) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            return GetCriteriaStrategyContainer(dmlInfo);
        }

        /// <summary>
        /// get DmlHandler by dmlId+groupId
        /// </summary>
        /// <param name="dmlId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public DmlHandler GetDmlHandler(string dmlId, string groupId) {
            return DmlHandlers[groupId][dmlId];
        }

        /// <summary>
        /// get DmlHandler by DmlInfo.DmlId + DmlInfo.GroupId
        /// </summary>
        /// <param name="dmlInfo"></param>
        /// <returns></returns>
        public DmlHandler GetDmlHandler(DmlInfoExtension dmlInfo) {
            return DmlHandlers[dmlInfo.GroupId][dmlInfo.DmlId];
        }

        /// <summary>
        /// gen select cnt sql by dmlId+groupId and condition data
        /// </summary>
        /// <param name="dmlId"></param>
        /// <param name="groupId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetSelectCntSql(string dmlId, string groupId, object data) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            return GetSelectCntSql(dmlInfo, data);
        }

        /// <summary>
        /// gen select cnt sql by dmlInfo and condition data
        /// </summary>
        /// <param name="dmlInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetSelectCntSql(DmlInfoExtension dmlInfo, object data) {
            DmlHandler handler = GetDmlHandler(dmlInfo);
            CriteriaStrategyContainer pool = GetCriteriaStrategyContainer(dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo(pool, data);
            return handler.GetSql(QueryType.SelectCnt, modelInfo);
        }

        /// <summary>
        /// gen select top sql by dmlId+groupId and condition data and top number
        /// </summary>
        /// <param name="dmlId"></param>
        /// <param name="groupId"></param>
        /// <param name="data"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public string GetSelectTopSql(string dmlId, string groupId, object data, int top) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            return GetSelectTopSql(dmlInfo, data, top);
        }

        /// <summary>
        /// gen select top sql by DmlInfo and condition data and top number
        /// </summary>
        /// <param name="dmlInfo"></param>
        /// <param name="data"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public string GetSelectTopSql(DmlInfoExtension dmlInfo, object data, int top) {
            DmlHandler handler = GetDmlHandler(dmlInfo);
            CriteriaStrategyContainer pool = GetCriteriaStrategyContainer(dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo(pool, data);
            modelInfo.Top = top;
            return handler.GetSql(QueryType.SelectTop, modelInfo);
        }

        /// <summary>
        /// gen select sql by dmlId+groupId and condition data 
        /// </summary>
        /// <param name="dmlId"></param>
        /// <param name="groupId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public DmlHandler GenSql(string dmlId, string groupId, object data) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            string sql = "";
            return GenSql(dmlInfo, data, ref sql);
        }

        /// <summary>
        /// gen select sql by dmlId+groupId and condition data 
        /// </summary>
        /// <param name="dmlId"></param>
        /// <param name="groupId"></param>
        /// <param name="data"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DmlHandler GenSql(string dmlId, string groupId, object data, ref string sql) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            return GenSql(dmlInfo, data, ref sql);
        }

        /// <summary>
        /// gen select sql by DmlInfo and condition data 
        /// </summary>
        /// <param name="dmlInfo"></param>
        /// <param name="data"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DmlHandler GenSql(DmlInfoExtension dmlInfo, object data, ref string sql) {
            DmlHandler handler = GetDmlHandler(dmlInfo);
            CriteriaStrategyContainer pool = GetCriteriaStrategyContainer(dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo(pool, data);

            sql = handler.GetSql(modelInfo);
            return handler;
        }

    }
}