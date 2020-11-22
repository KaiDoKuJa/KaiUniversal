using Kai.Universal.Data;

namespace Kai.Universal.Sql.Clause.Dialect {
    /// <summary>
    /// the limiting result clause
    /// </summary>
    public interface ILimitingResultClause {

        /// <summary>
        /// get fetch first sql
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        string GetFetchFirstSql(ModelInfo modelInfo);

        /// <summary>
        /// get paging sql
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        string GetPagingSql(ModelInfo modelInfo);

    }
}
