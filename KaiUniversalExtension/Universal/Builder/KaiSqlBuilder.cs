using Kai.Universal.DataModel;
using Kai.Universal.Service;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using System.Collections.Generic;

namespace Kai.Universal.Builder {
    /// <summary>
    /// KaiSqlBuilder
    /// </summary>
    public class KaiSqlBuilder {

        private DbmsType dbmsType;
        private readonly KaiSqlService sqlService;

        /// <summary>
        /// default constructor
        /// </summary>
        public KaiSqlBuilder() {
            sqlService = new KaiSqlService();
        }

        /// <summary>
        /// set DBMS type
        /// </summary>
        /// <param name="dbmsType"></param>
        /// <returns></returns>
        public KaiSqlBuilder SetDbmsType(DbmsType dbmsType) {
            this.dbmsType = dbmsType;
            return this;
        }

        /// <summary>
        /// set all dmlinfos
        /// </summary>
        /// <param name="dmlInfos"></param>
        /// <returns></returns>
        public KaiSqlBuilder SetDmlInfos(List<DmlInfoExtension> dmlInfos) {
            if (dmlInfos == null || dmlInfos.Count == 0) return this;
            Dictionary<string, Dictionary<string, DmlHandler>> dmlHandlers = new Dictionary<string, Dictionary<string, DmlHandler>>();
            sqlService.DmlInfos = dmlInfos;
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
            sqlService.DmlHandlers = dmlHandlers;
            return this;
        }

        /// <summary>
        /// set all criterias
        /// </summary>
        /// <param name="criteriaStrategies"></param>
        /// <returns></returns>
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
            sqlService.CriteriaStrategyContainers = containers;
            return this;
        }

        /// <summary>
        /// build
        /// </summary>
        /// <returns></returns>
        public KaiSqlService Build() {
            return this.sqlService;
        }

    }
}