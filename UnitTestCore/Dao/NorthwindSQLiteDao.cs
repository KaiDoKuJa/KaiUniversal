using Kai.Universal.Db;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace ConsoleCore.Dao {
    public class NorthwindSQLiteDao : SimpleDao {

        private static readonly string SAMPLE_FILE_PATH = @"\Sample\Db\Northwind.sl3";

        public override DbConnection GetConnection() {
            var path = Directory.GetCurrentDirectory();

            var conn = new SQLiteConnection() {
                ConnectionString = $"data source={path}{SAMPLE_FILE_PATH}; Version=3;New=False;Compress=True;",
                ParseViaFramework = true
            };
            if (conn.State == ConnectionState.Open) conn.Close();
            return conn;
        }

    }
}
