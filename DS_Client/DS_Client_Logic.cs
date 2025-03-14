using DS_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DS_Client
{
    public class DS_Client_Logic
    {
        private DS eds = new DS();
        private byte[] signatureClient;
        private byte[] signatureServer;
        private RSAParameters _serverPublicKey;
        private static Guid ClientGuid = Guid.NewGuid();
        private static string baseKeysPath = "../../../keys/";
        private static string basePath = baseKeysPath + "keys_" + ClientGuid.ToString() + "/";
        private string publicKey_client = basePath + "publicKey_client.pem";
        private string privateKey_client = basePath + "privateKey_client.pem";
        private string publicKey_server = basePath + "publicKey_server.pem";

        private async Task<string> SendRequestToServer(string request)
        {
            using (var client = new TcpClient("127.0.0.1", 5000))
            using (var stream = client.GetStream())
            {
                byte[] requestData = Encoding.UTF8.GetBytes(request);
                await stream.WriteAsync(requestData, 0, requestData.Length);

                var buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
        }
        public string Init()
        {            
            Directory.CreateDirectory(baseKeysPath);
            Directory.CreateDirectory(basePath);
            GenerateKeys();
            return ClientGuid.ToString();
        }
        public static RSAParameters DeserializeKeyPem(string key)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(key);
                return rsa.ExportParameters(false);
            }
        }
        public void GenerateKeys()
        {
            eds.GenerateKeys(out string pbKey, out string prKey);
            File.WriteAllBytes(publicKey_client, Encoding.ASCII.GetBytes(pbKey));
            File.WriteAllBytes(privateKey_client, Encoding.ASCII.GetBytes(prKey));
            MessageBox.Show("Ключи созданы успешно!", "Keys Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public async Task<(string, bool)> GenerateMessage()
        {
            try
            {
                string response = await SendRequestToServer("GET_RANDOM_MESSAGE");
                var parts = response.Split(':');
                string randomMessage = parts[0];
                signatureServer = Convert.FromBase64String(parts[1]);
                
                return (randomMessage, true);                
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message, "Информация о генерации сообщения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ("", true);
            }            
        }
        public async void GetServerPublicKey()
        {
            try
            {
                string response = await SendRequestToServer("GET_PUBLIC_KEY");
                _serverPublicKey = DeserializeKeyPem(response);
                MessageBox.Show("Публичный ключ сервера получен!", "Получение публичного ключа сервера", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Получение публичного ключа сервера", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public bool SignData(string clientMsg)
        {
            if (clientMsg != "")
            {
                try
                {
                    signatureClient = eds.SignData(clientMsg);                    
                    MessageBox.Show("Сообщение подписано!", "Информация о создании ЭЦП", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Информация о создании ЭЦП", MessageBoxButtons.OK, MessageBoxIcon.Warning);                    
                }
            }
            else
            {
                MessageBox.Show("Отсутствует сообщение!", "Информация о сообщении клиента", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
            return false;
        }

        public async void VerifyClientData(string clientMsg)
        {
            try
            {
                string message = clientMsg;
                string pbKey = eds.getPublicKeyPem();
                string response = await SendRequestToServer($"VERIFY_SIGNATURE:{message}:{Convert.ToBase64String(signatureClient)}:{pbKey}");
                if (Convert.ToBoolean(response))
                {
                    MessageBox.Show("Подпись верифицирована!", "Информация о верификации", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Подпись не верифицирована!", "Информация о верификации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Информация о верификации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void VerifyServerData(string serverMsg)
        {
            if (_serverPublicKey.Exponent != null)
            {
                bool isVerified = eds.VerifyData(serverMsg, signatureServer, _serverPublicKey);
                if (isVerified)
                {
                    MessageBox.Show("Сообщение верифицировано!", "Информация о верификации", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Сообщение не верифицировано!", "Информация о верификации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Отсутствует публичный ключ сервера!", "Получение публичного ключа сервера", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void DeleteDirectory()
        {
            if (Directory.Exists(basePath))
                Directory.Delete(basePath, true);
        }
    }
}
