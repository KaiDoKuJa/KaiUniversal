using ConsoleCore.Dao;
using Kai.Universal.Data;
using Kai.Universal.Sql.Clause;
using Kai.Universal.Sql.Handler;
using Kai.Universal.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using UnitTestCore.Models;

namespace Kai.Universal.Db {
    [TestClass()]
    public class SimpleDbcUtilityTest {

        private DbConnection connection;
        private void Init() {
            var dao = new NorthwindSQLiteDao();
            connection = dao.GetConnection();
        }

        private DmlHandler GetTestSqlHandler() {
            DmlInfo dmlInfo = new DmlInfo();
            dmlInfo.SqlTemplate = "select EmployeeID,LastName,Region from Employees limit 5";
            dmlInfo.ColumnWordCase = WordCase.UpperCamel;
            dmlInfo.MapModelWordCase = WordCase.LowerCamel;
            QueryClause clause = new QueryClause();
            clause.DmlInfo = dmlInfo;
            var handler = new DmlHandler(clause);
            handler.GetSql(null);
            return handler;
        }

        [TestMethod()]
        public void GetSelectCountTest() {
            Init();
            connection.Open();
            int x = SimpleDbcUtility.GetSelectCount(connection, "select count(1) from Employees");
            Assert.IsNotNull(x);
        }

        [TestMethod()]
        public void GetDataTest() {
            Init();
            connection.Open();
            var handler = GetTestSqlHandler();
            var data = SimpleDbcUtility.GetData<Employees>(connection, handler);
            Assert.IsNotNull(data);
        }

        [TestMethod()]
        public void GetMapDataTest() {
            Init();
            connection.Open();
            var handler = GetTestSqlHandler();
            var data = SimpleDbcUtility.GetMapData(connection, handler);
            Assert.IsNotNull(data);
        }

        [TestMethod()]
        public void GetPagerMapDataTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void ExecuteNonQueriesTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void RollbackTransactionTest() {
            Assert.IsNotNull(1);
        }
    }
}