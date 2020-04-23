using Kai.Universal.Text;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kai.Universal.Crypto {

    /**
     *
     * <pre>
     * Description: Advanced Encryption Standard
     *  java : AES/ECB/PKCS5Padding with no iv
     *  c#   : AES/ECB/PKCS7 with no iv
     * History:
     *     2017-10-20, Created by KaiDoKuJa
     *     2018-03-23, renew exception handling
     * </pre>
     *
     * @author KaiDoKuJa
     */
    public class SimpleAesCrypto : Cryptology {

        private byte[] crptoKey;

        private Encoding encoding = Encoding.UTF8;

        public string Encrypt(string message) {
            string result = "";

            MemoryStream ms = null;
            AesCryptoServiceProvider provider = null;
            CryptoStream cs = null;

            try {
                provider = GetProvider();


                byte[] input = encoding.GetBytes(message);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                // 輸出
                StringBuilder sb = new StringBuilder();
                foreach (byte b in ms.ToArray()) {
                    sb.AppendFormat("{0:X2}", b);
                }

                result = sb.ToString();
            } catch (Exception e) {
                throw e;
            } finally {
                if (cs != null)
                    cs.Dispose();
#if !NET35
                if (provider != null)
                    provider.Dispose();
#endif
                if (ms != null)
                    ms.Dispose();
            }

            return result;
        }

        public string Decrypt(string message) {
            string result = "";

            MemoryStream ms = null;
            AesCryptoServiceProvider provider = null;
            CryptoStream cs = null;

            try {
                provider = GetProvider();

                int n = message.Length / 2;
                byte[] input = HexUtility.HexToBytes(message);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(input, 0, n);
                cs.FlushFinalBlock();

                result = encoding.GetString(ms.ToArray());
            } catch (Exception e) {
                throw e;
            } finally {
                if (cs != null)
                    cs.Dispose();
#if !NET35
                if (provider != null)
                    provider.Dispose();
#endif
                if (ms != null)
                    ms.Dispose();
            }

            return result;
        }

        public void SetCrptoKey(string key) {
            if (key == null || "".Equals(key.Trim())) {
                throw new ArgumentNullException("key", "The crptoKey is empty!");
            }

            crptoKey = ASCIIEncoding.ASCII.GetBytes(key);

            if (!(crptoKey.Length == 16 || crptoKey.Length == 24 || crptoKey.Length == 32)) {
                throw new ArgumentException("Crpto key must be length 16 or 24 or 32 !", "key");
            }
        }

        private AesCryptoServiceProvider GetProvider() {
            AesCryptoServiceProvider provider = null;

            try {
                provider = new AesCryptoServiceProvider();
                provider.Mode = CipherMode.ECB;
                provider.Padding = PaddingMode.PKCS7;
                provider.Key = crptoKey;
            } catch (Exception e) {
                throw e;
            }

            return provider;
        }

        public Encoding GetEncoding() {
            return encoding;
        }

        public void SetEncoding(Encoding encoding) {
            this.encoding = encoding;
        }

    }
}