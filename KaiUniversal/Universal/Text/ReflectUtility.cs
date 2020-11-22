using Kai.Universal.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Kai.Universal.Text {

    /**
     * variable - all of the variable (property,field)
     * property - .Net property //public object A {get;set;}
     * field  - public variable //public object a;
     * methodinfo - GetXxx SetXxx
     * 2020/2/22 : default no use fieldInfo or methodInfo
     **/
    public static class ReflectUtility {

        /// <summary>
        /// use <seealso cref="PropertyInfo"/>to set value
        /// </summary>
        /// <param name="model"></param>
        /// <param name="variableName"></param>
        /// <param name="val"></param>
        public static void SetValue(object model, string variableName, object val) {
            if (model == null || variableName == null || "".Equals(variableName.Trim())
                 || val == null || val == DBNull.Value) return;
            Type type = model.GetType();
            PropertyInfo property = type.GetProperty(variableName, BindingFlags.Instance | BindingFlags.Public);
            if (property != null) {
                try {
                    SetPropertyValue(property, model, val);
                } catch {
                    // do nothing
                }
            }
        }

        private static void SetPropertyValue(PropertyInfo property, object model, object val) {
            Type propertyType = property.PropertyType;
            Type variableType = val.GetType();
            if (variableType.Equals(propertyType) || propertyType.IsAssignableFrom(variableType)) { // same type
                property.SetValue(model, val, null);
            } else if (typeof(string).Equals(propertyType)) { // property type is string
                property.SetValue(model, GetValueString(val), null);
            } else if (typeof(string).Equals(variableType) && propertyType.IsEnum) { // val type is string and property is enum
                try {
                    object val2Enum = Enum.Parse(propertyType, (string)val, true);
                    property.SetValue(model, val2Enum, null);
                } catch {
                    // do nothing (val is empty or val is not really enum type)
                }
            } else if (val != DBNull.Value) { // other not dbnull type
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
                return Convert.ToBase64String((byte[])val);
            }

            return val.ToString();
        }

        /// <summary>
        /// check the classOfT has this variableName and check var type
        /// <para>用這個取代java modelFetch 裡的getField, 因為 c# field/prop各自不同，java的部分要改名為var避免兩種混淆</para>
        /// </summary>
        /// <param name="classOfT"></param>
        /// <param name="variableName"></param>
        /// <param name="variableType"></param>
        /// <param name="isFieldNameUpperCase"></param>
        /// <returns></returns>
        public static bool HasVariable(Type classOfT, string variableName, Type variableType, bool isFieldNameUpperCase = true) {
            bool result = false;
            string propertyName = isFieldNameUpperCase ? variableName : TextUtility.ConvertWordCase(variableName, WordCase.LowerCamel, WordCase.UpperCamel);
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

        /// <summary>
        /// get value by variableName
        /// <para>1. use PropertyInfo get value</para>
        /// <para>2. if null, use FieldInfo get value</para>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public static object GetValue(object model, string variableName) {
            object refVal = null;
            if (model == null || variableName == null || "".Equals(variableName.Trim())) return null;
            Type type = model.GetType();
            PropertyInfo property = type.GetProperty(variableName, BindingFlags.Instance | BindingFlags.Public);
            if (property != null) {
                refVal = property.GetValue(model, null);
            } else {
                string lowerCase = TextUtility.ConvertWordCase(variableName, WordCase.UpperCamel, WordCase.LowerCamel);
                FieldInfo field = model.GetType().GetField(lowerCase, BindingFlags.Instance | BindingFlags.Public);
                if (field != null) {
                    refVal = field.GetValue(model);
                }
            }
            return refVal;
        }

        /// <summary>
        /// is list
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsList(object o) {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        /// <summary>
        /// is dictionary
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsDictionary(object o) {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }

        /// <summary>
        /// is hashtable
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsHashtable(object o) {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsAssignableFrom(typeof(Hashtable));
        }

        /// <summary>
        /// is number type
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
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
