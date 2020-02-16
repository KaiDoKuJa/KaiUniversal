using Kai.Universal.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace UnitTestCore {
    [TestClass]
    public class TextUtilityTest {

        [TestMethod]
        public void ConvertWordCase() {
            string r = TextUtility.ConvertWordCase("abcBcd", WordCase.LOWER_CAMEL, WordCase.LOWER_HYPHEN);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LOWER_CAMEL, WordCase.LOWER_UNDERSCORE);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LOWER_CAMEL, WordCase.UPPER_CAMEL);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LOWER_CAMEL, WordCase.UPPER_HYPHEN);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("abcBcd", WordCase.LOWER_CAMEL, WordCase.UPPER_UNDERSCORE);
            Debug.WriteLine(r);
            Debug.WriteLine("---");
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UPPER_CAMEL, WordCase.LOWER_HYPHEN);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UPPER_CAMEL, WordCase.LOWER_UNDERSCORE);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UPPER_CAMEL, WordCase.LOWER_CAMEL);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UPPER_CAMEL, WordCase.UPPER_HYPHEN);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("ABcBcd", WordCase.UPPER_CAMEL, WordCase.UPPER_UNDERSCORE);
            Debug.WriteLine(r);
            Debug.WriteLine("---");
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UPPER_UNDERSCORE, WordCase.LOWER_HYPHEN);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UPPER_UNDERSCORE, WordCase.LOWER_UNDERSCORE);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UPPER_UNDERSCORE, WordCase.LOWER_CAMEL);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UPPER_UNDERSCORE, WordCase.UPPER_HYPHEN);
            Debug.WriteLine(r);
            r = TextUtility.ConvertWordCase("A_BC_BCD", WordCase.UPPER_UNDERSCORE, WordCase.UPPER_CAMEL);
            Debug.WriteLine(r);
        }

    }
}
