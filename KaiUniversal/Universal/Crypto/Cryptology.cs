namespace Kai.Universal.Crypto {


    public interface Cryptology {

        string Encrypt(string message);

        string Decrypt(string message);

        void SetCrptoKey(string key);
    }
}