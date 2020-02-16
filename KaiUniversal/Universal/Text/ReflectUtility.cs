using Kai.Universal.Util;
using System;
using System.Reflection;

namespace Kai.Universal.Text {

    /**
     * variable - all of the variable (property,field)
     * property - .Net property //public object A {get;set;}
     * field  - public variable //public object a;
     * methodinfo - GetXxx SetXxx
     * 2020/2/22 : 預設不使用fieldInfo or methodInfo的判斷
     **/
    public class ReflectUtility {

        private ReflectUtility() { }

        public static void SetValue(object model, string variableName, object val) {
            if (model == null || variableName == null || "".Equals(variableName.Trim())
                 || val == null || val == DBNull.Value) return;
            Type type = model.GetType();
            PropertyInfo property = type.GetProperty(variableName, BindingFlags.Instance | BindingFlags.Public);
            if (property != null) {
                try {
                    SetPropertyValue(property, model, val);
                } catch {}
            }
        }

        private static void SetPropertyValue(PropertyInfo property, object model, object val) {
            Type propertyType = property.PropertyType;
            Type variableType = val.GetType();
            if (variableType.Equals(propertyType) || propertyType.IsAssignableFrom(variableType)) { // 同型態
                property.SetValue(model, val, null);
            } else if (typeof(string).Equals(propertyType)) { // 標的屬性string
                property.SetValue(model, GetValueString(val), null);
            } else if (typeof(string).Equals(variableType) && propertyType.IsEnum) { // 標的Enum 來源string
                // TODO : val == "" or val is not really enum type
                object val2Enum = Enum.Parse(propertyType, (string)val, true);
                property.SetValue(model, val2Enum, null);
            } else if (val != DBNull.Value) {
                property.SetValue(model, val, null);
            }
        }

        private static string GetValueString(object val) {
            Type valType = val.GetType();
            if (Type.GetType("System.DateTime").Equals(valType)) {
                DateTime dttm = (DateTime)val;
                return dttm.ToString(DateTimeUtility.ISO8601);
            }
            
            if (val is byte[]) {
                return Convert.ToBase64String((byte[]) val);
            }
            
            return val.ToString();
        }

        //用這個取帶java modelFetch 裡的getField, 因為 c# field/prop各自不同，java的部分要改名為var避免兩種混淆
        public static bool HasVariable(Type classOfT, string variableName, Type variableType, bool isFieldNameUpperCase = true) {
            bool result = false;
            string propertyName = isFieldNameUpperCase ? variableName : TextUtility.ConvertWordCase(variableName, WordCase.LOWER_CAMEL, WordCase.UPPER_CAMEL);
            PropertyInfo property = classOfT.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property == null) return result;

            Type propertyType = property.PropertyType;
            if (variableType.Equals(propertyType) || propertyType.IsAssignableFrom(variableType)) { // 同型態
                result = true;
            } else if (typeof(string).Equals(propertyType)) { // 標的屬性string
                result = true;
            } else if (typeof(string).Equals(variableType) && propertyType.IsEnum) { // 標的Enum 來源string
                // not suuport Enum from int
                result = true;
            }

            return result;
        }

        public static FieldInfo GetField(Type type, String fieldName) {
            // property or field  TODO : 這個仍要在改寫只是先完成出來而以  set/get or GetXxx/SetXxx
            return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        }

        public static object GetValue(object model, string variableName) {
            object refVal = null;
            if (model == null || variableName == null || "".Equals(variableName.Trim())) return null;
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
                );
        }
    }
}
