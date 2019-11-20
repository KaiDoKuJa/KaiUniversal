using Kai.Universal.Crypto;
using Kai.Universal.Text;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace KaiUniversalConsole {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            SimpleAesCrypto aesCrypto = new SimpleAesCrypto();
            aesCrypto.SetCrptoKey("aims2016aims2016");
            String s = aesCrypto.Encrypt("abcd1234abcd1234");
            Console.WriteLine(s);
            String p = "D57528E90F7157B4E7987FD68A9F14EDD701DE721D22C3C27757C3DD488F2563";
            String re = aesCrypto.Decrypt(p);
            Console.WriteLine(p);
            Console.WriteLine(re);
            int a = 5;
            Add1(a);
            Console.WriteLine(a);
            Add1(ref a);
            Console.WriteLine(a);

            TestMethod();

            Console.ReadLine(); // pause
        }

        static void Add1(int a) {
            a = a + 1;
        }
        static void Add1(ref int a) {
            a = a + 1;
        }

        //[TestMethod]
        public static void TestMethod() {
            TestClass test = new TestClass();
            PropertyInfo[] pro = test.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] fil = test.GetType().GetFields( BindingFlags.Instance | BindingFlags.Public);
            MethodInfo[] men = test.GetType().GetMethods();

            object o1 = ReflectUtility.GetValue(test, "a");
            object o2 = ReflectUtility.GetValue(test, "a");
            if (o1 == null && o2 == null) Debug.WriteLine("are null");
            Debug.WriteLine(o1);
            Debug.WriteLine(o2);
            foreach (var item in pro)
            {
                Debug.WriteLine("PropertyInfo:" + item.GetValue(test) + "/" + item.Name);
            }
            foreach (FieldInfo item in fil)
            {
                Debug.WriteLine("FieldInfo:" + item.GetValue(test) + "/" + item.Name);
            }
            foreach (MethodInfo item in men)
            {
                Debug.WriteLine("Method:" + item.Name);
            }
        }

        public class TestClass {
            private int a = 1;
            public int b { get; set; } = 2;
            public int c = 3;

            public int GetA() {
                return a;
            }
        }

    }
}
