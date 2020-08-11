using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kai.Universal.Sql.Clause {
    [TestClass]
    public class OrmUtilityTest {

        [TestMethod]
        public void GetSqlString() {
            Assert.AreEqual("1", OrmUtility.GetSqlString(true));
            Assert.AreEqual("0", OrmUtility.GetSqlString(false));
            Assert.AreEqual("1.2345", OrmUtility.GetSqlString(1.2345));
            Assert.AreEqual("12345", OrmUtility.GetSqlString(12345));
            Assert.AreEqual("'1.2345'", OrmUtility.GetSqlString("1.2345"));
        }

        [TestMethod]
        public void GetArraySqlString() {
            Assert.AreEqual("1,0", OrmUtility.GetArraySqlString(new object[] { true, false }));
            Assert.AreEqual("12345,1.2345", OrmUtility.GetArraySqlString(new object[] { 12345, 1.2345 }));
            Assert.AreEqual("'12345','1.2345'", OrmUtility.GetArraySqlString(new string[] { "12345", "1.2345" }));
        }
    }
}
