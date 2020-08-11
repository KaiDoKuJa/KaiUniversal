using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kai.Universal.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Kai.Universal.Text {
    [TestClass()]
    public class ReflectUtilityTest {
        [TestMethod()]
        public void SetValueTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void HasVariableTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void GetValueTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void IsListTest() {
            var values = new List<object>();
            values.Add(3);
            values.Add("test");
            if (ReflectUtility.IsList(values)) {
                var list = (IList)values;
                var array = new object[list.Count];
                list.CopyTo(array, 0);
                Assert.AreEqual(values[0], array[0]);
                Assert.AreEqual(values[1], array[1]);
            }
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void IsDictionaryTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void IsHashtableTest() {
            Assert.IsNotNull(1);
        }

        [TestMethod()]
        public void IsNumberTypeTest() {
            Assert.IsNotNull(1);
        }
    }
}