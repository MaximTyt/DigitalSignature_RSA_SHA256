using DS_Lib;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace DS_Client
{
    public partial class ClientForm : Form
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
        public ClientForm()
        {
            InitializeComponent();
            lbl_ClientGUID.Text = ClientGuid.ToString();
            Directory.CreateDirectory(baseKeysPath);
            Directory.CreateDirectory(basePath);
            //Создаём ключи для клиента
            GenerateKeys();
        }

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

        private static RSAParameters DeserializeKeyPem(string key)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(key);
                return rsa.ExportParameters(false);
            }
        }

        private void GenerateKeys()
        {
            eds.GenerateKeys(out string pbKey, out string prKey);
            File.WriteAllBytes(publicKey_client, Encoding.ASCII.GetBytes(pbKey));
            File.WriteAllBytes(privateKey_client, Encoding.ASCII.GetBytes(prKey));
            MessageBox.Show(this, "Ключи созданы успешно!", "Keys Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btn_GenerateMessage_Click(object sender, EventArgs e)
        {
            try
            {
                string response = await SendRequestToServer("GET_RANDOM_MESSAGE");
                var parts = response.Split(':');
                string randomMessage = parts[0];
                signatureServer = Convert.FromBase64String(parts[1]);
                rtb_serverMsg.Text = randomMessage;
                btn_VerifyServerData.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Информация о генерации сообщения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btn_GetServerPublicKey_Click(object sender, EventArgs e)
        {
            try
            {
                string response = await SendRequestToServer("GET_PUBLIC_KEY");
                _serverPublicKey = DeserializeKeyPem(response);
                MessageBox.Show("Публичный ключ сервера получен!", "Получение публичного ключа сервера", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Получение публичного ключа сервера", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btn_SignData_Click(object sender, EventArgs e)
        {
            if (rtb_clientMsg.Text != "")
            {
                try
                {
                    signatureClient = eds.SignData(rtb_clientMsg.Text);
                    btn_VerifyClientData.Enabled = true;
                    MessageBox.Show("Сообщение подписано!", "Информация о создании ЭЦП", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private async void btn_VerifyClientData_Click(object sender, EventArgs e)
        {
            try
            {
                string message = rtb_clientMsg.Text;
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

        private void btn_VerifyServerData_Click(object sender, EventArgs e)
        {
            if(_serverPublicKey.Exponent != null)
            {
                bool isVerified = eds.VerifyData(rtb_serverMsg.Text, signatureServer, _serverPublicKey);
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

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(basePath))
                Directory.Delete(basePath, true);
        }
    }
}
