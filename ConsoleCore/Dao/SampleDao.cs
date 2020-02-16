using Kai.Universal.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ConsoleCore.Dao {
    public class SampleDao : SimpleDao {

        private static readonly string SAMPLE_FILE_PATH = @"\Sample\Db\Northwind.sl3";

        public override DbConnection GetConnection() {
            var path = Directory.GetCurrentDirectory();
            
            var conn = new SQLiteConnection() {
                ConnectionString = $"data source={path}{SAMPLE_FILE_PATH}; Version=3;New=False;Compress=True;"
            };

            if (conn.State == ConnectionState.Open) conn.Close();
            return conn;
        }

    }
}
