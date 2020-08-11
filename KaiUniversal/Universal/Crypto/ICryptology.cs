namespace Kai.Universal.Crypto {


    public interface ICryptology {

        string Encrypt(string message);

        string Decrypt(string message);

        void SetCrptoKey(string key);
    }
}
