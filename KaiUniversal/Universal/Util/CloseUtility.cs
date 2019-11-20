﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Kai.Universal.Util {
    public class CloseUtility {
        private CloseUtility() { }

        public static void CloseConnection(ref DbConnection connection) {
            try {
                if (connection != null) {
                    connection.Close();
                }
            } catch { }
        }

        public static void CloseDataReader(ref DbDataReader reader) {
            try {
                if (reader != null) {
                    reader.Close();
                }
            } catch { }
        }

        public static void DisposeSqlCommmand(ref DbCommand command) {
            try {
                if (command != null) {
                    command.Dispose();
                }
            } catch {
            } finally { }
        }
    }
}
