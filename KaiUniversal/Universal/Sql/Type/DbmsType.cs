namespace Kai.Universal.Sql.Type {

    /// <summary>
    /// DBMS type
    /// </summary>
    public enum DbmsType {
        /// <summary>
        ///  standard sql
        /// </summary>
        Default,
        /// <summary>
        /// sqlserver 2005+ mode sql (for paging sql)
        /// </summary>
        FromSqlServer2005,
        /// <summary>
        /// sqlserver 2012+ mode sql (for paging sql)
        /// </summary>
        FromSqlServer2012,
    }
}
