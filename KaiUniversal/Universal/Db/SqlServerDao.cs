using System.Data.Common;
#if NETSTANDARD2_0
using Microsoft.Data.SqlClient;
#else 
using System.Data.SqlClient;
#endif

namespace Kai.Universal.Db {
    public class SqlServerDao : SimpleDao {

        public SqlServerDao() { }

        public SqlServerDao(SimpleDataSource ds) {
            this.DataSource = ds;
        }

        public SqlServerDao(string connectionString) {
            var ds = new SimpleDataSource();
            ds.ConnectionString = connectionString;
            this.DataSource = ds;
        }

        public override DbConnection GetConnection() {
            return new SqlConnection(DataSource.ConnectionString);
        }

    }
}
