using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Kai.Universal.Db {
    public class SqlServerDao : SimpleDao {

        public override DbConnection GetConnection() {
            return new SqlConnection(DataSource.ConnectionString);
        }

    }
}
