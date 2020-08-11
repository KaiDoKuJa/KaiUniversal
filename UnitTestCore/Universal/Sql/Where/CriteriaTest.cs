using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kai.Universal.Sql.Where {
    [TestClass()]
    public class CriteriaTest {

        [TestMethod()]
        public void testCriteriaDirect() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition(""));
            pool.AddCriteria(Criteria.AndCondition("COL_A = '123'"));
            pool.AddCriteria(Criteria.AndCondition("COL_B = 123"));
            pool.AddCriteria(Criteria.AndCondition("COL_C in (select COL from TABLE)"));
            Assert.AreEqual("COL_A = '123' and COL_B = 123 and COL_C in (select COL from TABLE) ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void AndIsNullConditionTest() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndIsNullCondition("COL_A"));
            pool.AddCriteria(Criteria.AndIsNotNullCondition("COL_B"));
            Assert.AreEqual("COL_A is null and COL_B is not null ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void testCriteriaEqual() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition(" ", CriteriaType.Equal, ""));
            pool.AddCriteria(Criteria.AndCondition("COL_A", CriteriaType.Equal, "123"));
            pool.AddCriteria(Criteria.AndCondition("COL_A", CriteriaType.NotEqual, "456"));
            pool.AddCriteria(Criteria.AndCondition("COL_C", CriteriaType.Equal, true));
            pool.AddCriteria(Criteria.AndCondition("COL_D", CriteriaType.NotEqual, false));
            Assert.AreEqual("COL_A = '123' and COL_A <> '456' and COL_C = 1 and COL_D <> 0 ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void testCriteriaLessThan() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition("COL_A", CriteriaType.LessThan, 123));
            pool.AddCriteria(Criteria.AndCondition("COL_B", CriteriaType.LessThanEqual, 456));
            Assert.AreEqual("COL_A < 123 and COL_B <= 456 ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void testCriteriaGreaterThan() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition("COL_A", CriteriaType.GreaterThan, 123));
            pool.AddCriteria(Criteria.AndCondition("COL_B", CriteriaType.GreaterThanEqual, 456));
            Assert.AreEqual("COL_A > 123 and COL_B >= 456 ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void testCriteriaLike() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition(" ", CriteriaType.Like, ""));
            pool.AddCriteria(Criteria.AndCondition("COL_A", CriteriaType.Like, "123"));
            pool.AddCriteria(Criteria.AndCondition("COL_B", CriteriaType.NotLike, "123"));
            Assert.AreEqual("COL_A like '%123%' and COL_B not like '%123%' ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void testCriteriaLeftLike() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition(" ", CriteriaType.LeftLike, ""));
            pool.AddCriteria(Criteria.AndCondition("COL_C", CriteriaType.LeftLike, "456"));
            pool.AddCriteria(Criteria.AndCondition("COL_D", CriteriaType.NotLeftLike, "456"));
            Assert.AreEqual("COL_C like '%456' and COL_D not like '%456' ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void testCriteriaRightLike() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndCondition(" ", CriteriaType.RightLike, ""));
            pool.AddCriteria(Criteria.AndCondition("COL_E", CriteriaType.RightLike, "789"));
            pool.AddCriteria(Criteria.AndCondition("COL_F", CriteriaType.NotRightLike, "789"));
            Assert.AreEqual("COL_E like '789%' and COL_F not like '789%' ", pool.GetWhereSql());
        }

        [TestMethod()]
        public void AndInConditionTest() {
            CriteriaPool pool = new CriteriaPool();
            pool.AddCriteria(Criteria.AndInCondition(" ", null));
            pool.AddCriteria(Criteria.AndInCondition("COL_A", new string[] { "123", "456" }));
            pool.AddCriteria(Criteria.AndInCondition("COL_B", new object[] { 1, 2 }));
            // TODO : 這個太特別，要調整
            pool.AddCriteria(Criteria.AndCondition("COL_C", CriteriaType.In, "(select COL from TABLE)"));
            Assert.AreEqual("COL_A in ('123','456') and COL_B in (1,2) and COL_C in (select COL from TABLE) ", pool.GetWhereSql());
        }
    }

}

