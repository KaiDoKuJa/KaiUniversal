using Kai.Universal.Crypto;

namespace Kai.Universal.Db {

    /// <summary>
    /// DbDataSource 的摘要描述。
    /// </summary>
    public class SimpleDataSource {

        /// <summary>
        /// connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// the encryption of connection string
        /// </summary>
        public ICryptology Cryptology { get; set; }

        /// <summary>
        /// to decrytp conn string
        /// </summary>
        public void DecryptConnectionString() {
            if (Cryptology != null) {
                ConnectionString = Cryptology.Decrypt(this.ConnectionString);
            }
        }
    }
}