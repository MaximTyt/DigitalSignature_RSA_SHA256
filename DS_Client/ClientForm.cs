namespace DS_Client
{
    public partial class ClientForm : Form
    {
        private IDS_Client_Logic _ds_client;
        public ClientForm(IDS_Client_Logic ds_client)
        {
            _ds_client = ds_client;
            InitializeComponent();
            lbl_ClientGUID.Text = _ds_client.Init();
        }

        private async void btn_GenerateMessage_Click(object sender, EventArgs e)
        {
            (rtb_serverMsg.Text, btn_VerifyServerData.Enabled) = await _ds_client.GenerateMessageClick();
        }

        private void btn_GetServerPublicKey_Click(object sender, EventArgs e)
        {
            _ds_client.GetServerPublicKeyClick();
        }


        private void btn_SignData_Click(object sender, EventArgs e)
        {
            _ds_client.SignDataClick(rtb_clientMsg.Text, btn_VerifyClientData);
        }

        private void btn_VerifyClientData_Click(object sender, EventArgs e)
        {
            _ds_client.VerifyClientDataClick(rtb_clientMsg.Text);
        }

        private void btn_VerifyServerData_Click(object sender, EventArgs e)
        {
            _ds_client.VerifyServerDataClick(rtb_serverMsg.Text);
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ds_client.DeleteDirectory();
        }
    }
}
