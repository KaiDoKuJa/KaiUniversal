using Kai.Universal.Data;

namespace Kai.Universal.Sql.Clause.Dialect {
    public interface LimitingResultClause {

        string GetFetchFirstSql(ModelInfo modelInfo);
        string GetPagingSql(ModelInfo modelInfo);

    }
}
