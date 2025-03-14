namespace DS_Client
{
    partial class ClientForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rtb_clientMsg = new RichTextBox();
            rtb_serverMsg = new RichTextBox();
            btn_SignData = new Button();
            btn_VerifyServerData = new Button();
            btn_GenerateMessage = new Button();
            btn_GetServerPublicKey = new Button();
            btn_VerifyClientData = new Button();
            lbl_ClientData = new Label();
            lbl_ServerData = new Label();
            lbl_ClientGUID = new Label();
            SuspendLayout();
            // 
            // rtb_clientMsg
            // 
            rtb_clientMsg.Location = new Point(36, 49);
            rtb_clientMsg.Name = "rtb_clientMsg";
            rtb_clientMsg.Size = new Size(327, 116);
            rtb_clientMsg.TabIndex = 0;
            rtb_clientMsg.Text = "";
            // 
            // rtb_serverMsg
            // 
            rtb_serverMsg.Location = new Point(420, 49);
            rtb_serverMsg.Name = "rtb_serverMsg";
            rtb_serverMsg.Size = new Size(327, 116);
            rtb_serverMsg.TabIndex = 1;
            rtb_serverMsg.Text = "";
            // 
            // btn_SignData
            // 
            btn_SignData.Location = new Point(36, 178);
            btn_SignData.Name = "btn_SignData";
            btn_SignData.Size = new Size(151, 51);
            btn_SignData.TabIndex = 2;
            btn_SignData.Text = "Подписать сообщение";
            btn_SignData.UseVisualStyleBackColor = true;
            btn_SignData.Click += btn_SignData_Click;
            // 
            // btn_VerifyServerData
            // 
            btn_VerifyServerData.Enabled = false;
            btn_VerifyServerData.Location = new Point(607, 179);
            btn_VerifyServerData.Name = "btn_VerifyServerData";
            btn_VerifyServerData.Size = new Size(140, 89);
            btn_VerifyServerData.TabIndex = 3;
            btn_VerifyServerData.Text = "Верифицировать подписанное сообщение сервера";
            btn_VerifyServerData.UseVisualStyleBackColor = true;
            btn_VerifyServerData.Click += btn_VerifyServerData_Click;
            // 
            // btn_GenerateMessage
            // 
            btn_GenerateMessage.Location = new Point(420, 235);
            btn_GenerateMessage.Name = "btn_GenerateMessage";
            btn_GenerateMessage.Size = new Size(157, 51);
            btn_GenerateMessage.TabIndex = 4;
            btn_GenerateMessage.Text = "Сгенирировать сообщение";
            btn_GenerateMessage.UseVisualStyleBackColor = true;
            btn_GenerateMessage.Click += btn_GenerateMessage_Click;
            // 
            // btn_GetServerPublicKey
            // 
            btn_GetServerPublicKey.Location = new Point(420, 179);
            btn_GetServerPublicKey.Name = "btn_GetServerPublicKey";
            btn_GetServerPublicKey.Size = new Size(157, 50);
            btn_GetServerPublicKey.TabIndex = 5;
            btn_GetServerPublicKey.Text = "Получить публичный ключ сервера";
            btn_GetServerPublicKey.UseVisualStyleBackColor = true;
            btn_GetServerPublicKey.Click += btn_GetServerPublicKey_Click;
            // 
            // btn_VerifyClientData
            // 
            btn_VerifyClientData.Enabled = false;
            btn_VerifyClientData.Location = new Point(212, 171);
            btn_VerifyClientData.Name = "btn_VerifyClientData";
            btn_VerifyClientData.Size = new Size(151, 93);
            btn_VerifyClientData.TabIndex = 6;
            btn_VerifyClientData.Text = "Верифицировать подписанное сообщение клиента";
            btn_VerifyClientData.UseVisualStyleBackColor = true;
            btn_VerifyClientData.Click += btn_VerifyClientData_Click;
            // 
            // lbl_ClientData
            // 
            lbl_ClientData.AutoSize = true;
            lbl_ClientData.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lbl_ClientData.Location = new Point(92, 9);
            lbl_ClientData.Name = "lbl_ClientData";
            lbl_ClientData.Size = new Size(238, 31);
            lbl_ClientData.TabIndex = 7;
            lbl_ClientData.Text = "Сообщение клиента";
            // 
            // lbl_ServerData
            // 
            lbl_ServerData.AutoSize = true;
            lbl_ServerData.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lbl_ServerData.Location = new Point(473, 9);
            lbl_ServerData.Name = "lbl_ServerData";
            lbl_ServerData.Size = new Size(237, 31);
            lbl_ServerData.TabIndex = 8;
            lbl_ServerData.Text = "Сообщение сервера";
            // 
            // lbl_ClientGUID
            // 
            lbl_ClientGUID.AutoSize = true;
            lbl_ClientGUID.Dock = DockStyle.Bottom;
            lbl_ClientGUID.Location = new Point(0, 271);
            lbl_ClientGUID.Name = "lbl_ClientGUID";
            lbl_ClientGUID.Size = new Size(0, 20);
            lbl_ClientGUID.TabIndex = 9;
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(800, 291);
            Controls.Add(lbl_ClientGUID);
            Controls.Add(lbl_ServerData);
            Controls.Add(lbl_ClientData);
            Controls.Add(btn_VerifyClientData);
            Controls.Add(btn_GetServerPublicKey);
            Controls.Add(btn_GenerateMessage);
            Controls.Add(btn_VerifyServerData);
            Controls.Add(btn_SignData);
            Controls.Add(rtb_serverMsg);
            Controls.Add(rtb_clientMsg);
            MaximizeBox = false;
            Name = "ClientForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Создание и Проверка ЭЦП ";
            FormClosing += ClientForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtb_clientMsg;
        private RichTextBox rtb_serverMsg;
        private Button btn_SignData;
        private Button btn_VerifyServerData;
        private Button btn_GenerateMessage;
        private Button btn_GetServerPublicKey;
        private Button btn_VerifyClientData;
        private Label lbl_ClientData;
        private Label lbl_ServerData;
        private Label lbl_ClientGUID;
    }
}
