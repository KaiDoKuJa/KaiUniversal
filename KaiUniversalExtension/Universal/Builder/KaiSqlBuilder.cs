using Kai.Universal.DataModel;
using Kai.Universal.Helper;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using System.Collections.Generic;

namespace Kai.Universal.Builder {
    public class KaiSqlBuilder {

        private DbmsType dbmsType;
        private readonly KaiSqlHelper sqlHelper;

        public KaiSqlBuilder() {
            sqlHelper = new KaiSqlHelper();
        }

        public KaiSqlBuilder SetDbmsType(DbmsType dbmsType) {
            this.dbmsType = dbmsType;
            return this;
        }

        public KaiSqlBuilder SetDmlInfos(List<DmlInfoExtension> dmlInfos) {
            if (dmlInfos == null || dmlInfos.Count == 0) return this;
            Dictionary<string, Dictionary<string, DmlHandler>> dmlHandlers = new Dictionary<string, Dictionary<string, DmlHandler>>();
            sqlHelper.DmlInfos = dmlInfos;
            foreach (var dmlInfo in dmlInfos) {
                var dmlId = dmlInfo.DmlId;
                var groupId = dmlInfo.GroupId;
                DmlHandler dmlHandler = DmlHandler.CreateHandler(dbmsType, dmlInfo);
                if (dmlHandlers.ContainsKey(groupId)) {
                    dmlHandlers[groupId].Add(dmlId, dmlHandler);
                } else {
                    Dictionary<string, DmlHandler> map = new Dictionary<string, DmlHandler>();
                    map.Add(dmlId, dmlHandler);
                    dmlHandlers.Add(groupId, map);
                }
            }
            sqlHelper.DmlHandlers = dmlHandlers;
            return this;
        }

        public KaiSqlBuilder SetCriteriaStrategies(List<CriteriaStrategy> criteriaStrategies) {
            if (criteriaStrategies == null || criteriaStrategies.Count == 0) return this;
            Dictionary<string, CriteriaStrategyContainer> containers = new Dictionary<string, CriteriaStrategyContainer>();
            foreach (var criteriaStrategy in criteriaStrategies) {
                var criteriaId = criteriaStrategy.CriteriaId;
                CriteriaStrategyContainer container;
                if (containers.ContainsKey(criteriaId)) {
                    container = containers[criteriaId];
                } else {
                    container = new CriteriaStrategyContainer();
                    containers.Add(criteriaId, container);
                }

                switch (criteriaStrategy.ReplaceMode) {
                    case ReplaceMode.After:
                        container.AfterReplacements.Add(criteriaStrategy);
                        break;
                    case ReplaceMode.Before:
                        container.BeforeReplacements.Add(criteriaStrategy);
                        break;
                    default:
                        container.Criterias.Add(criteriaStrategy);
                        break;
                }
            }
            sqlHelper.CriteriaStrategyContainers = containers;
            return this;
        }

        public KaiSqlHelper Build() {
            return this.sqlHelper;
        }

    }
}