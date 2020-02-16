using System.Data.Common;
#if NETSTANDARD2_0
using Microsoft.Data.SqlClient;
#else 
using System.Data.SqlClient;
#endif

namespace Kai.Universal.Db {
    public class SqlServerDao : SimpleDao {

        public override DbConnection GetConnection() {
            return new SqlConnection(DataSource.ConnectionString);
        }

    }
}
