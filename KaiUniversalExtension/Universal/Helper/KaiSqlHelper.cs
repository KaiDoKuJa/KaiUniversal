using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Kai.Universal.Db;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using Kai.Universal.Utility;
using System;
using System.Collections.Generic;

namespace Kai.Universal.Helper {
    public abstract class KaiSqlHelper {

        // TODO : 1. dmlinfos -> dmlhandler
        // TODO : 2. criteria data to container
        // groupId -> dmlId -> Dmlhandler
        Dictionary<string, Dictionary<string, DmlHandler>> DmlHandlers { get; set; }

        public List<DmlInfoExtension> DmlInfos { get; set; }
        // criteriaId -> Container
        public Dictionary<string, CriteriaStrategyContainer> CriteriaStrategyContainers { get; set; }

        public IDao dao { get; set; }

        private DmlInfoExtension GetDmlInfo(string dmlId, string groupId) {
            if (string.IsNullOrWhiteSpace(dmlId) || string.IsNullOrWhiteSpace(groupId)) return null;
            if (DmlInfos == null || DmlInfos.Count == 0) return null;
            foreach (var item in DmlInfos) {
                if (dmlId.Equals(item.DmlId) && groupId.Equals(item.DmlId))
                    return item;
            }
            return null;
        }

        private CriteriaStrategyContainer GetCriteriaStrategyContainer(string criteriaId) {
            return CriteriaStrategyContainers[criteriaId];
        }

        public CriteriaStrategyContainer GetCriteriaStrategyContainer(DmlInfoExtension dmlInfo) {
            if (dmlInfo == null || dmlInfo.CriteriaId == null) return null;
            return GetCriteriaStrategyContainer(dmlInfo.CriteriaId);
        }

        public CriteriaStrategyContainer GetCriteriaStrategyContainer(string dmlId, string groupId) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            return GetCriteriaStrategyContainer(dmlInfo);
        }

        public DmlHandler GetDmlHandler(string dmlId, string groupId) {
            return DmlHandlers[groupId][dmlId];
        }
        public DmlHandler GetDmlHandler(DmlInfoExtension dmlInfo) {
            return DmlHandlers[dmlInfo.GroupId][dmlInfo.DmlId];
        }

        public string GetSelectCntSql(string dmlId, string groupId, Dictionary<string, object> map) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            DmlHandler handler = GetDmlHandler(dmlInfo);
            CriteriaStrategyContainer pool = GetCriteriaStrategyContainer(dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo(pool, map);
            return handler.getSql(QueryType.SelectCnt, modelInfo);
        }

        public List<Dictionary<string, object>> getMapData(string dmlId, string groupId, Dictionary<string, object> map) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            DmlHandler handler = GetDmlHandler(dmlInfo);

            CriteriaStrategyContainer pool = GetCriteriaStrategyContainer(dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo(pool, map);

            string sql = handler.getSql(modelInfo);
            return dao.GetMapData(handler);
        }

        // 該廢除於此
        public PagerData<Dictionary<string, object>> getPagerData(string dmlId, string groupId, Dictionary<string, object> map) {
            var dmlInfo = GetDmlInfo(dmlId, groupId);
            DmlHandler handler = GetDmlHandler(dmlInfo);
            CriteriaStrategyContainer pool = GetCriteriaStrategyContainer(dmlInfo);
            ModelInfo modelInfo = KaiSqlUtility.RaiseModelInfo(pool, map);
            modelInfo.PageNumber = (int)map["pageNumber"];
            modelInfo.EachPageSize = (int)map["pageSize"];

            return dao.GetPagerMapData(handler, modelInfo);
        }

    }
}