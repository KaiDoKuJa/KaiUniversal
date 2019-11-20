using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kai.Universal.Sql.Text {
    public class SpecialString {

        public string Value { get; set; }

        public SpecialString(string value) {
            this.Value = value;
        }

        public override string ToString() {
            return this.Value;
        }
    }
}
