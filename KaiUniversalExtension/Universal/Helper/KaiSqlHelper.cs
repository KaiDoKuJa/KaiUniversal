using Kai.Universal.Data;
using Kai.Universal.DataModel;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using Kai.Universal.Utility;
using System.Collections.Generic;

namespace Kai.Universal.Helper {
    public class KaiSqlHelper {

        // groupId -> dmlId -> Dmlhandler
        public Dictionary<string, Dictionary<string, DmlHandler>> DmlHandlers { get; set; }
        public List<DmlInfoExtension> DmlInfos { get; set; }

        // criteriaId -> Container
        public Dictionary<string, CriteriaStrategyContainer> CriteriaStrategyContainers { get; set; }

        public DmlInfoExtension GetDmlInfo(string dmlId, string groupId) {
            if (string.IsNullOrWhiteSpace(dmlId) || string.IsNullOrWhiteSpace(groupId)) return null;
            if (DmlInfos == null || DmlInfos.Count == 0) return null;
            foreach (var item in DmlInfos) {
                if (dmlId.Equals(item.DmlId) && groupId.Equals(item.GroupId))
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

        

    }
}