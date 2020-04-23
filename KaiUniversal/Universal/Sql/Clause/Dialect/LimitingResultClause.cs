using Kai.Universal.Data;

namespace Kai.Universal.Sql.Clause.Dialect {
    public interface ILimitingResultClause {

        string GetFetchFirstSql(ModelInfo modelInfo);
        string GetPagingSql(ModelInfo modelInfo);

    }
}
