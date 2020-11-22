using Kai.Universal.Text;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Kai.Universal.Data {

    /// <summary>
    /// DataReader utility
    /// </summary>
    public class DataReaderUtility {

        private DataReaderUtility() { }

        /// <summary>
        /// get all column info, the DbDataReady need open db connection
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<ColumnInfo> GetAllColumnInfo(DbDataReader reader) {
            List<ColumnInfo> result = new List<ColumnInfo>();
            List<string> chkColNames = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++) {
                string sqlColName = reader.GetName(i);
                if (chkColNames.Contains(sqlColName)) {
                    continue;
                } else {
                    chkColNames.Add(sqlColName);
                }
                ColumnInfo info = new ColumnInfo();
                info.ColName = sqlColName;
                info.ColType = reader.GetFieldType(i);
                info.ModelName = sqlColName;
                result.Add(info);
            }
            return result;
        }

        /// <summary>
        /// get all column info, the DbDataReady need open db connection
        /// <para>cust mapping : cust define col mapping to model name</para>
        /// <para>wordCase mapping : from colum name wordCase to mapping model name wordCase</para>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="customerMapping"></param>
        /// <param name="columnWordCase"></param>
        /// <param name="mapModelWordCase"></param>
        /// <returns></returns>
        public static List<ColumnInfo> GetAllColumnInfo(DbDataReader reader, Dictionary<string, string> customerMapping, WordCase columnWordCase, WordCase mapModelWordCase) {
            bool noCustomerMapping = customerMapping == null || customerMapping.Count == 0;
            bool noConvertWordCase = columnWordCase == mapModelWordCase;
            if (noCustomerMapping && noConvertWordCase) return GetAllColumnInfo(reader);

            List<ColumnInfo> result = new List<ColumnInfo>();
            List<string> colNames = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++) {
                string sqlColName = reader.GetName(i);
                if (colNames.Contains(sqlColName)) {
                    continue;
                } else {
                    colNames.Add(sqlColName);
                }
                ColumnInfo info = new ColumnInfo();
                info.ColName = sqlColName;
                info.ColType = reader.GetFieldType(i);
                info.ModelName = sqlColName;

                if (noCustomerMapping || !customerMapping.ContainsKey(sqlColName)) {
                    if (noConvertWordCase) {
                        info.ModelName = sqlColName;
                    } else {
                        string t = TextUtility.ConvertWordCase(sqlColName, columnWordCase, mapModelWordCase);
                        info.ModelName = t;
                    }
                } else {
                    info.ModelName = customerMapping[sqlColName];
                }
                result.Add(info);
            }
            return result;
        }
    }
}
