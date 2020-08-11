using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kai.Universal.Sql.Where {
    [TestClass()]
    public class CriteriaPoolTest {
        [TestMethod()]
        public void IsEmptyTest() {
            CriteriaPool pool = new CriteriaPool();
            Assert.AreEqual(true, pool.IsEmpty());
        }

        [TestMethod()]
        public void IsNotEmptyTest() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition("COL_A = '123'"));
            Assert.AreEqual(false, pool.IsEmpty());
        }

    }
}