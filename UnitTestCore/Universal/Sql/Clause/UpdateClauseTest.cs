using Kai.Universal.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UnitTestCore.Models;

namespace Kai.Universal.Sql.Clause {
    [TestClass()]
    public class UpdateClauseTest {

        public static IEnumerable<object[]> GetData() {
            DmlInfo dmlInfo = new DmlInfo();
            dmlInfo.TableName = "EMmployes";
            dmlInfo.Columns = new string[] { "LastName", "FirstName", "Notes" };
            dmlInfo.NoAttachQuoteColumns = new string[] { "Notes" };
            dmlInfo.NonQueryMandatoryColumns = new string[] { "EmployeeId" };
            ModelInfo modelInfo = new ModelInfo();
            var emp = new Employees();
            emp.EmployeeId = 1;
            emp.LastName = "Kai";
            emp.FirstName = "DoKuJa";
            emp.Notes = "Min(Date('now'))";
            modelInfo.Model = emp;
            yield return new object[] { dmlInfo, modelInfo };
        }

        [DataTestMethod()]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void GetSqlTest(DmlInfo dmlInfo, ModelInfo modelInfo) {
            var clause = new UpdateClause();
            clause.DmlInfo = dmlInfo;
            var expected = "update EMmployes set LastName='Kai',FirstName='DoKuJa',Notes=Min(Date('now')) where EmployeeId=1";
            var sql = clause.GetSql(modelInfo);
            Assert.AreEqual(expected, sql);
        }
    }
}