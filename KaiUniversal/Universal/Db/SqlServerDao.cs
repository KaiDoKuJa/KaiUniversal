using System.Data.Common;
#if NETSTANDARD2_0
using Microsoft.Data.SqlClient;
#else 
using System.Data.SqlClient;
#endif

namespace Kai.Universal.Db {
    /// <summary>
    /// the sqlserver Dao
    /// </summary>
    public class SqlServerDao : SimpleDao {

        /// <summary>
        /// empty constructor
        /// </summary>
        public SqlServerDao() { }

        /// <summary>
        /// consctructor use datasource
        /// </summary>
        /// <param name="ds"></param>
        public SqlServerDao(SimpleDataSource ds) {
            this.DataSource = ds;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlServerDao(string connectionString) {
            var ds = new SimpleDataSource();
            ds.ConnectionString = connectionString;
            this.DataSource = ds;
        }

        /// <summary>
        /// get connection
        /// </summary>
        /// <returns></returns>
        public override DbConnection GetConnection() {
            return new SqlConnection(DataSource.ConnectionString);
        }

    }
}
