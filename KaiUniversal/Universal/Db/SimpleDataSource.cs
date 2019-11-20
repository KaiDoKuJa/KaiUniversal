using Kai.Universal.Crypto;

namespace Kai.Universal.Db {

    /// <summary>
    /// DbDataSource 的摘要描述。
    /// </summary>
    public class SimpleDataSource {

        public string ConnectionString { get; set; }
        public Cryptology Cryptology { get; set; }

        public void DecryptConnectionString() {
            if (Cryptology != null) {
                ConnectionString = Cryptology.Decrypt(this.ConnectionString);
            }
        }
    }
}