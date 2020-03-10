using Kai.Universal.Data;
using Kai.Universal.Db;
using Kai.Universal.Sql.Clause;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Sql.Type;
using Kai.Universal.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text;

namespace FrameworkConsole {
    class Program {
        static void Main(string[] args) {
            string hex = HexUtility.BytesToHex(Encoding.UTF8.GetBytes("test123"));
            Debug.WriteLine(hex);

            DbConnection connection = OpenConnection("");
            DmlInfo dmlInfo = new DmlInfo();
            dmlInfo.SqlTemplate = "select EmployeeID,LastName,Region,Photo from Employees limit 1";
            dmlInfo.ColumnWordCase = WordCase.UpperCamel;
            dmlInfo.MapModelWordCase = WordCase.LowerCamel;
            QueryClause clause = new QueryClause();
            clause.DmlInfo = dmlInfo;
            DmlHandler handler = new DmlHandler(clause);
            String sql = handler.GetSql(null);
            Debug.WriteLine(String.Format("sql : {0}", sql));
            List<Dictionary<string, object>> list0 = SimpleDbcUtility.GetMapData(connection, handler);
            String jsonString = SimpleDbcUtility.GetJsonData(connection, handler);
            
            //foreach (var map in list0) {
            //    foreach (var pair in map) {
            //        Debug.WriteLine(string.Format("{0}:{1}", pair.Key, pair.Value));
            //    }
            //}
            Debug.WriteLine(jsonString);
            Debug.WriteLine(JsonConvert.SerializeObject(list0));
            Console.ReadLine();
        }

        public static SQLiteConnection OpenConnection(string database) {
            string path = @"C:\Kai\Bitbucket\KaiUniversal\Console35";
            var conn = new SQLiteConnection() {
                //
                ConnectionString = $"data source="+ path + "\\sample\\db\\Northwind.sl3; Version=3;New=False;Compress=True;"
            };
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            return conn;
        }

    }
}
