using Kai.Universal.Builder;
using Kai.Universal.DataModel;
using Kai.Universal.Helper;
using System;
using System.Collections.Generic;

namespace UnitTestCore.Sample {
    class UseCaseTest {

        public void Test() {

            var envVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            //var i = ReadFromAppSettings();
            //var i = 
            //var n = i.GetSection("dmlInfos").Get<List<DmlInfoExtension>>();
            var n = new List<DmlInfoExtension>();

            var sqlBox = new KaiSqlBuilder().SetDmlInfos(n).Build();
            string sql = sqlBox.GetSelectCntSql("employeeByCfg", "sample", null);
            string sql2 = sqlBox.GetSelectCntSql("employeeByDirectSql", "sample", null);
            //i.GetValue<"">
            Console.WriteLine("Hello World!");

        }
    }
}
