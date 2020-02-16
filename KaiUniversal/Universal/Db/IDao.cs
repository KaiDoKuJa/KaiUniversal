using Kai.Universal.Data;
using Kai.Universal.Sql.Handler;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Db {
    public interface IDao {

        int GetSelectCount(string sql);
        List<T> GetData<T>(string sql) where T : new();
        List<Dictionary<string, object>> GetMapData(string sql);
        List<Dictionary<string, object>> GetMapData(DmlHandler handler);
        PagerData<Dictionary<string, object>> GetPagerMapData(DmlHandler handler, ModelInfo modelInfo);

        int ExecuteNonQuery(string sql);
        bool ExecuteNonQueries(List<string> sqls);
        bool ExecuteNonQueries(string[] sqls);

        DbConnection GetConnection();
    }
}
