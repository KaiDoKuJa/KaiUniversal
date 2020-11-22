namespace Kai.Universal.Crypto {

    /// <summary>
    /// ICrypto
    /// </summary>
    public interface ICryptology {

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string Encrypt(string message);

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string Decrypt(string message);

        /// <summary>
        /// Set CryptoKey
        /// </summary>
        /// <param name="key"></param>
        void SetCrptoKey(string key);
    }
}
