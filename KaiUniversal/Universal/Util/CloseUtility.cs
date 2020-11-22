using System.Data.Common;

namespace Kai.Universal.Util {
    /// <summary>
    /// Close Utility
    /// </summary>
    public class CloseUtility {
        private CloseUtility() { }

        /// <summary>
        /// close Connection
        /// </summary>
        /// <param name="connection"></param>
        public static void CloseConnection(ref DbConnection connection) {
            try {
                if (connection != null) {
                    connection.Close();
                }
            } catch {
                // do nothing
            }
        }

        /// <summary>
        /// close DataReader
        /// </summary>
        /// <param name="reader"></param>
        public static void CloseDataReader(ref DbDataReader reader) {
            try {
                if (reader != null) {
                    reader.Close();
                }
            } catch {
                // do nothing
            }
        }

        /// <summary>
        /// Dispose DbCommand
        /// </summary>
        /// <param name="command"></param>
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

