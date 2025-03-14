using DS_Lib;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace DS_Client
{
    public partial class ClientForm : Form
    {
        private DS_Client_Logic client_BL = new DS_Client_Logic();
        public ClientForm()
        {
            InitializeComponent();
            lbl_ClientGUID.Text = client_BL.Init();
        }

        private async void btn_GenerateMessage_Click(object sender, EventArgs e)
        {
            (rtb_serverMsg.Text, btn_VerifyServerData.Enabled) = await client_BL.GenerateMessage();
        }

        private void btn_GetServerPublicKey_Click(object sender, EventArgs e)
        {
            client_BL.GetServerPublicKey();
        }


        private void btn_SignData_Click(object sender, EventArgs e)
        {
            btn_VerifyClientData.Enabled = client_BL.SignData(rtb_clientMsg.Text);
        }

        private void btn_VerifyClientData_Click(object sender, EventArgs e)
        {
            client_BL.VerifyClientData(rtb_clientMsg.Text);
        }

        private void btn_VerifyServerData_Click(object sender, EventArgs e)
        {
            client_BL.VerifyServerData(rtb_serverMsg.Text);
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client_BL.DeleteDirectory();
        }
    }
}
