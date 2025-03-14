using System.Security.Cryptography;
using System.Text;

namespace DS_Lib
{
    public class DS : IDS
    {
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        //Создание ключей
        public void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
                publicKey = rsa.ExportRSAPublicKeyPem();
                privateKey = rsa.ExportRSAPrivateKeyPem();
            }
        }

        // Создание подписи
        public byte[] SignData(string data)
        {
            SHA256 alg = SHA256.Create();            
            byte[] hash = alg.ComputeHash(Encoding.ASCII.GetBytes(data));
            byte[] signedHash;
                        
            using (RSA rsa = RSA.Create())
            {                
               rsa.ImportParameters(_privateKey);

                RSAPKCS1SignatureFormatter rsaFormatter = new(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA256));

               signedHash = rsaFormatter.CreateSignature(hash);
            }
            return signedHash;
        }
        public RSAParameters getPublicKey() => _publicKey;
        public RSAParameters getPrivateKey() => _privateKey;
        public string getPublicKeyPem() 
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(_publicKey);
                return rsa.ExportRSAPublicKeyPem();                
            }
        }
        public string getPrivateKeyPem()
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(_privateKey);
                return rsa.ExportRSAPrivateKeyPem();
            }
        }

        //Проверка подписи
        public bool VerifyData(string data, byte[] signature, RSAParameters publicKey)
        {
            SHA256 alg = SHA256.Create();
            byte[] hash = alg.ComputeHash(Encoding.ASCII.GetBytes(data));            
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                RSAPKCS1SignatureDeformatter rsaDeformatter = new(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA256));
                return rsaDeformatter.VerifySignature(hash, signature);
            }             
        }

        
    }
}
