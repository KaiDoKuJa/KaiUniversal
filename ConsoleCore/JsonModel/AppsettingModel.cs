using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCore.JsonModel {
    public class AppsettingModel {
        public string[] Exclude { get; set; }
        public Dictionary<string, object> KeyValue { get; set; }
    }
}