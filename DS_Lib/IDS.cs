using System.Security.Cryptography;

namespace DS_Lib
{
    interface IDS
    {
        public void GenerateKeys(out string publicKey, out string privateKey);
        public byte[] SignData(string data);
        public RSAParameters getPublicKey();
        public RSAParameters getPrivateKey();
        public string getPublicKeyPem();
        public string getPrivateKeyPem();
        public bool VerifyData(string data, byte[] signedHash, RSAParameters publicKey);
    }
}
