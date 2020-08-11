using System.Data.Common;

namespace Kai.Universal.Util {
    public class CloseUtility {
        private CloseUtility() { }

        public static void CloseConnection(ref DbConnection connection) {
            try {
                if (connection != null) {
                    connection.Close();
                }
            } catch {
                // do nothing
            }
        }

        public static void CloseDataReader(ref DbDataReader reader) {
            try {
                if (reader != null) {
                    reader.Close();
                }
            } catch {
                // do nothing
            }
        }

        public static void DisposeSqlCommmand(ref DbCommand command) {
            try {
                if (command != null) {
                    command.Dispose();
                }
            } catch {
                // do nothing
            }
        }
    }
}

