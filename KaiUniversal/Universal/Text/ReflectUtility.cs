using System;
using System.Reflection;
using System.Data;

namespace Kai.Universal.Text {

    /**
     * variable - all of the variable (property,field)
     * property - .Net property //public object A {get;set;}
     * field  - public variable //public object a;
     * TODO:  private object 需考慮用set, get來處理!!  
     * 
     * 
     * TODO : 用 #ifdef 的方式 來處理 field , methodinfo
     * TODO : 新增 methodinfo 來抓取 GetXxx SetXxx
     **/
    public class ReflectUtility {

        private ReflectUtility() { }

        public static void SetValue(object model, string variableName, object val) {
            if (model == null || !String.IsNullOrWhiteSpace(variableName) || val == null) return;
            Type type = model.GetType();
            PropertyInfo property = type.GetProperty(variableName, BindingFlags.Instance | BindingFlags.Public);
            if (property != null) {
                SetPropertyValue(property, model, val);
            } else {
                string lowerCase = TextUtility.ConvertWordCase(variableName, WordCase.UPPER_CAMEL, WordCase.LOWER_CAMEL);
                FieldInfo field = type.GetField(lowerCase, BindingFlags.Instance | BindingFlags.Public);
                if (field != null) {
                    SetFieldValue(field, model, val);
                }
            }
        }

        // 須淘汰
        // TODO : 確認GetValueString 傳入的沒問題，針對bool型態，JAVA沒使用GetValueString
        private static void SetFieldValue(FieldInfo field, object model, object val) {
            if (field.FieldType == typeof(string)) {
                field.SetValue(model, GetValueString(val));
            } else {
                if (val != DBNull.Value) {
                    field.SetValue(model, val);
                }
            }
        }

        private static void SetPropertyValue(PropertyInfo property, object model, object val) {
            if (property.PropertyType == typeof(string)) {
                property.SetValue(model, GetValueString(val), null);
            } else {
                if (val != DBNull.Value) {
                    property.SetValue(model, val, null);
                }
            }
        }

        // TODO 在評估轉換的問題
        private static string GetValueString(object val) {
            string result = "";

            if (val.GetType() == Type.GetType("System.Boolean")) {
                bool b = (bool)val;
                if (b) {
                    result = "1";
                } else {
                    result = "0";
                }
            } else if (val.GetType() == Type.GetType("System.DateTime")) {
                DateTime dttm = (DateTime)val;
                result = String.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", dttm.Year, dttm.Month, dttm.Day, dttm.Hour, dttm.Minute, dttm.Second);
            } else if (val.GetType() == Type.GetType("System.TimeSpan")) {
                TimeSpan ts = (TimeSpan)val;
                result = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            } else {
                if (val.GetType() == Type.GetType("System.String")) {
                    result = val.ToString();
                }
            }
            return result;
        }


        //用這個取帶java modelFetch 裡的getField, 因為 c# field/prop各自不同，java的部分要改名為var避免兩種混淆
        public static bool HasVariable(Type type, string variableName, bool isFieldNameUpperCase = true) {
            bool result = false;

            string propertyName = isFieldNameUpperCase ? variableName : TextUtility.ConvertWordCase(variableName, WordCase.LOWER_CAMEL, WordCase.UPPER_CAMEL);
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property != null) {
                result = true;
            } else {
                string fieldName = !isFieldNameUpperCase ? variableName : TextUtility.ConvertWordCase(variableName, WordCase.UPPER_CAMEL, WordCase.LOWER_CAMEL);
                FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
                if (field != null) {
                    result = true;
                }
            }
            return result;
        }

        public static FieldInfo GetField(Type type, String fieldName) {
            // property or field  TODO : 這個仍要在改寫只是先完成出來而以  set/get or GetXxx/SetXxx
            return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        }

        public static object GetValue(object model, string variableName) {
            object refVal = null;
            if (model == null || !String.IsNullOrWhiteSpace(variableName)) return null;
            Type type = model.GetType();
            PropertyInfo property = type.GetProperty(variableName, BindingFlags.Instance | BindingFlags.Public);
            if (property != null) {
                refVal = property.GetValue(model, null);
            } else {
                string lowerCase = TextUtility.ConvertWordCase(variableName, WordCase.UPPER_CAMEL, WordCase.LOWER_CAMEL);
                FieldInfo field = model.GetType().GetField(lowerCase, BindingFlags.Instance | BindingFlags.Public);
                if (field != null) {
                    refVal = field.GetValue(model);
                }
            }
            return refVal;
        }

        public static bool IsNumberType(object val) {
            return (val is double
                || val is float
                || val is long
                || val is int
                || val is uint
                || val is short
                || val is ushort
                || val is byte
                || val is sbyte
                || val is decimal
                )
            ? true : false;
    }
}
}