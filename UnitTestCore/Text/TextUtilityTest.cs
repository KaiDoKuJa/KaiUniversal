using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kai.Universal.Text {
    [TestClass]
    public class TextUtilityTest {

        [TestMethod]
        public void ConvertWordCase() {
            string r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.LowerHyphen);
            Assert.AreEqual("abc-bcd", r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.LowerUnderscore);
            Assert.AreEqual("abc_bcd", r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.UpperCamel);
            Assert.AreEqual("AbcBcd", r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.UpperHyphen);
            Assert.AreEqual("ABC-BCD", r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.UpperUnderscore);
            Assert.AreEqual("ABC_BCD", r);

            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.LowerHyphen);
            Assert.AreEqual("a-bc-bcd", r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.LowerUnderscore);
            Assert.AreEqual("a_bc_bcd", r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.LowerCamel);
            Assert.AreEqual("aBcBcd", r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.UpperHyphen);
            Assert.AreEqual("A-BC-BCD", r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.UpperUnderscore);
            Assert.AreEqual("A_BC_BCD", r);

            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.LowerHyphen);
            Assert.AreEqual("a-bc-bcd", r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.LowerUnderscore);
            Assert.AreEqual("a_bc_bcd", r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.LowerCamel);
            Assert.AreEqual("aBcBcd", r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.UpperHyphen);
            Assert.AreEqual("A-BC-BCD", r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.UpperCamel);
            Assert.AreEqual("ABcBcd", r);
        }

    }
}
