using Kai.Universal.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace UnitTestCore {
    [TestClass]
    public class TextUtilityTest {

        [TestMethod]
        public void ConvertWordCase() {
            string r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.LowerHyphen);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.LowerUnderscore);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.UpperCamel);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.UpperHyphen);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LowerCamel, WordCase.UpperUnderscore);
            Debug.WriteLine(r);
            Debug.WriteLine("---");
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.LowerHyphen);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.LowerUnderscore);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.LowerCamel);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.UpperHyphen);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UpperCamel, WordCase.UpperUnderscore);
            Debug.WriteLine(r);
            Debug.WriteLine("---");
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.LowerHyphen);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.LowerUnderscore);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.LowerCamel);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.UpperHyphen);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UpperUnderscore, WordCase.UpperCamel);
            Debug.WriteLine(r);
        }

    }
}
