using Kai.Universal.Data;
using Kai.Universal.Db.Constant;
using Kai.Universal.Db.Fetch;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Db.Utility {

    /// <summary>
    /// Dictionary(k->v) dbc utility
    /// </summary>
    public static class MapDataDbcUtility {

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetData(DbConnection connection, int commandTimeout, String sql) {
            MapDataFetch fetch = new MapDataFetch();
            fetch.CommandTimeout = commandTimeout;
            fetch.Execute(connection, sql);
            return fetch.GetResult();
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetData(DbConnection connection, String sql) {
            return GetData(connection, DbConstant.DEFAULT_COMMAND_TIMEOUT, sql);
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetData(DbConnection connection, DmlHandler handler) {
            MapDataFetch fetch = new MapDataFetch();
            fetch.DmlInfo = handler.Clause.DmlInfo;
            fetch.Execute(connection, handler.GetLastSql());
            return fetch.GetResult();
        }

        /// <summary>
        /// get pager data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="handler"></param>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        public static PagerData<Dictionary<string, object>> GetPagerData(DbConnection connection, DmlHandler handler, ModelInfo modelInfo) {
            PagerData<Dictionary<string, object>> pagerData = new PagerData<Dictionary<string, object>>();
            // select count
            int totalCount = SimpleDbcUtility.GetSelectCount(connection, handler.GetSql(QueryType.SelectCnt, modelInfo));
            pagerData.TotalCount = totalCount;
            if (totalCount > 0) {
                // select paging sql
                handler.GetSql(QueryType.SelectPaging, modelInfo);
                List<Dictionary<string, object>> datas = GetData(connection, handler);
                pagerData.Datas = datas;
                // other info
                pagerData.PageNumber = modelInfo.PageNumber;
                pagerData.EachPageSize = modelInfo.EachPageSize;
            } else {
                pagerData.PageNumber = 0;
                pagerData.EachPageSize = 0;
            }
            return pagerData;
        }


    }
}