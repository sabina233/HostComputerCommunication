namespace HostComputerCommunication.UI.Controls;

partial class TcpSocketControl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _tcpClient?.Dispose();
            _tcpServer?.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        SuspendLayout();

        // 创建所有控件
        grpConnection = new GroupBox();
        rbClient = new RadioButton();
        rbServer = new RadioButton();
        lblHost = new Label();
        txtHost = new TextBox();
        lblPort = new Label();
        nudPort = new NumericUpDown();
        chkAutoReconnect = new CheckBox();
        btnConnect = new Button();
        lblStatus = new Label();
        lblClientCount = new Label();

        grpSend = new GroupBox();
        rbHex = new RadioButton();
        rbAscii = new RadioButton();
        txtSend = new TextBox();
        btnSend = new Button();
        btnClearSend = new Button();

        grpClients = new GroupBox();
        lstClients = new ListBox();

        grpReceive = new GroupBox();
        chkAutoScroll = new CheckBox();
        chkShowTimestamp = new CheckBox();
        rtbReceive = new RichTextBox();
        btnClearReceive = new Button();
        btnExport = new Button();

        grpLog = new GroupBox();
        rtbLog = new RichTextBox();
        btnClearLog = new Button();

        grpConnection.SuspendLayout();
        grpSend.SuspendLayout();
        grpClients.SuspendLayout();
        grpReceive.SuspendLayout();
        grpLog.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();

        //
        // grpConnection
        //
        grpConnection.Controls.Add(rbClient);
        grpConnection.Controls.Add(rbServer);
        grpConnection.Controls.Add(lblHost);
        grpConnection.Controls.Add(txtHost);
        grpConnection.Controls.Add(lblPort);
        grpConnection.Controls.Add(nudPort);
        grpConnection.Controls.Add(chkAutoReconnect);
        grpConnection.Controls.Add(btnConnect);
        grpConnection.Controls.Add(lblStatus);
        grpConnection.Controls.Add(lblClientCount);
        grpConnection.Location = new Point(10, 10);
        grpConnection.Name = "grpConnection";
        grpConnection.Size = new Size(280, 220);
        grpConnection.TabIndex = 0;
        grpConnection.TabStop = false;
        grpConnection.Text = "连接配置";
        //
        // rbClient
        //
        rbClient.Checked = true;
        rbClient.Location = new Point(15, 25);
        rbClient.Name = "rbClient";
        rbClient.Size = new Size(60, 23);
        rbClient.TabIndex = 0;
        rbClient.TabStop = true;
        rbClient.Text = "客户端";
        rbClient.CheckedChanged += RbClient_CheckedChanged;
        //
        // rbServer
        //
        rbServer.Location = new Point(80, 25);
        rbServer.Name = "rbServer";
        rbServer.Size = new Size(60, 23);
        rbServer.TabIndex = 1;
        rbServer.Text = "服务端";
        //
        // lblHost
        //
        lblHost.Location = new Point(15, 55);
        lblHost.Name = "lblHost";
        lblHost.Size = new Size(40, 23);
        lblHost.TabIndex = 2;
        lblHost.Text = "主机:";
        //
        // txtHost
        //
        txtHost.Location = new Point(60, 52);
        txtHost.Name = "txtHost";
        txtHost.Size = new Size(145, 23);
        txtHost.TabIndex = 3;
        txtHost.Text = "127.0.0.1";
        //
        // lblPort
        //
        lblPort.Location = new Point(15, 85);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(40, 23);
        lblPort.TabIndex = 4;
        lblPort.Text = "端口:";
        //
        // nudPort
        //
        nudPort.Location = new Point(60, 82);
        nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudPort.Name = "nudPort";
        nudPort.Size = new Size(100, 23);
        nudPort.TabIndex = 5;
        nudPort.Value = new decimal(new int[] { 8080, 0, 0, 0 });
        //
        // chkAutoReconnect
        //
        chkAutoReconnect.Checked = true;
        chkAutoReconnect.CheckState = CheckState.Checked;
        chkAutoReconnect.Location = new Point(15, 112);
        chkAutoReconnect.Name = "chkAutoReconnect";
        chkAutoReconnect.Size = new Size(100, 23);
        chkAutoReconnect.TabIndex = 6;
        chkAutoReconnect.Text = "自动重连";
        //
        // btnConnect
        //
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(15, 145);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(120, 35);
        btnConnect.TabIndex = 7;
        btnConnect.Text = "连接";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += BtnConnect_Click;
        //
        // lblStatus
        //
        lblStatus.Font = new Font(Font.FontFamily, 10f, FontStyle.Bold);
        lblStatus.ForeColor = Color.Red;
        lblStatus.Location = new Point(140, 145);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(125, 35);
        lblStatus.TabIndex = 8;
        lblStatus.Text = "未连接";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        //
        // lblClientCount
        //
        lblClientCount.ForeColor = Color.Gray;
        lblClientCount.Location = new Point(15, 190);
        lblClientCount.Name = "lblClientCount";
        lblClientCount.Size = new Size(250, 18);
        lblClientCount.TabIndex = 9;
        lblClientCount.Text = "";

        //
        // grpSend
        //
        grpSend.Controls.Add(rbHex);
        grpSend.Controls.Add(rbAscii);
        grpSend.Controls.Add(txtSend);
        grpSend.Controls.Add(btnSend);
        grpSend.Controls.Add(btnClearSend);
        grpSend.Location = new Point(10, 235);
        grpSend.Name = "grpSend";
        grpSend.Size = new Size(280, 150);
        grpSend.TabIndex = 1;
        grpSend.TabStop = false;
        grpSend.Text = "发送区";
        //
        // rbHex
        //
        rbHex.Checked = true;
        rbHex.Location = new Point(15, 25);
        rbHex.Name = "rbHex";
        rbHex.Size = new Size(60, 23);
        rbHex.TabIndex = 0;
        rbHex.TabStop = true;
        rbHex.Text = "HEX";
        rbHex.CheckedChanged += RbHex_CheckedChanged;
        //
        // rbAscii
        //
        rbAscii.Location = new Point(80, 25);
        rbAscii.Name = "rbAscii";
        rbAscii.Size = new Size(60, 23);
        rbAscii.TabIndex = 1;
        rbAscii.Text = "ASCII";
        //
        // txtSend
        //
        txtSend.Location = new Point(15, 55);
        txtSend.Multiline = true;
        txtSend.Name = "txtSend";
        txtSend.ScrollBars = ScrollBars.Vertical;
        txtSend.Size = new Size(250, 50);
        txtSend.TabIndex = 2;
        txtSend.KeyDown += TxtSend_KeyDown;
        //
        // btnSend
        //
        btnSend.BackColor = Color.FromArgb(0, 153, 51);
        btnSend.FlatStyle = FlatStyle.Flat;
        btnSend.ForeColor = Color.White;
        btnSend.Location = new Point(15, 110);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(80, 30);
        btnSend.TabIndex = 3;
        btnSend.Text = "发送";
        btnSend.UseVisualStyleBackColor = false;
        btnSend.Click += BtnSend_Click;
        //
        // btnClearSend
        //
        btnClearSend.Location = new Point(100, 110);
        btnClearSend.Name = "btnClearSend";
        btnClearSend.Size = new Size(60, 30);
        btnClearSend.TabIndex = 4;
        btnClearSend.Text = "清空";
        btnClearSend.Click += BtnClearSend_Click;

        //
        // grpClients
        //
        grpClients.Controls.Add(lstClients);
        grpClients.Location = new Point(10, 390);
        grpClients.Name = "grpClients";
        grpClients.Size = new Size(280, 150);
        grpClients.TabIndex = 2;
        grpClients.TabStop = false;
        grpClients.Text = "已连接客户端";
        grpClients.Visible = false;
        //
        // lstClients
        //
        lstClients.Location = new Point(15, 23);
        lstClients.Name = "lstClients";
        lstClients.Size = new Size(250, 120);
        lstClients.TabIndex = 0;

        //
        // grpReceive
        //
        grpReceive.Controls.Add(chkAutoScroll);
        grpReceive.Controls.Add(chkShowTimestamp);
        grpReceive.Controls.Add(rtbReceive);
        grpReceive.Controls.Add(btnClearReceive);
        grpReceive.Controls.Add(btnExport);
        grpReceive.Location = new Point(300, 10);
        grpReceive.Name = "grpReceive";
        grpReceive.Size = new Size(580, 415);
        grpReceive.TabIndex = 3;
        grpReceive.TabStop = false;
        grpReceive.Text = "接收区";
        //
        // chkAutoScroll
        //
        chkAutoScroll.Checked = true;
        chkAutoScroll.CheckState = CheckState.Checked;
        chkAutoScroll.Location = new Point(15, 20);
        chkAutoScroll.Name = "chkAutoScroll";
        chkAutoScroll.Size = new Size(85, 23);
        chkAutoScroll.TabIndex = 0;
        chkAutoScroll.Text = "自动滚动";
        //
        // chkShowTimestamp
        //
        chkShowTimestamp.Checked = true;
        chkShowTimestamp.CheckState = CheckState.Checked;
        chkShowTimestamp.Location = new Point(105, 20);
        chkShowTimestamp.Name = "chkShowTimestamp";
        chkShowTimestamp.Size = new Size(100, 23);
        chkShowTimestamp.TabIndex = 1;
        chkShowTimestamp.Text = "显示时间戳";
        //
        // rtbReceive
        //
        rtbReceive.BackColor = Color.Black;
        rtbReceive.Font = new Font("Consolas", 9.5f);
        rtbReceive.ForeColor = Color.LimeGreen;
        rtbReceive.Location = new Point(15, 48);
        rtbReceive.Name = "rtbReceive";
        rtbReceive.ReadOnly = true;
        rtbReceive.ScrollBars = RichTextBoxScrollBars.Both;
        rtbReceive.Size = new Size(550, 330);
        rtbReceive.TabIndex = 2;
        rtbReceive.WordWrap = false;
        //
        // btnClearReceive
        //
        btnClearReceive.Location = new Point(15, 383);
        btnClearReceive.Name = "btnClearReceive";
        btnClearReceive.Size = new Size(80, 25);
        btnClearReceive.TabIndex = 3;
        btnClearReceive.Text = "清空接收";
        btnClearReceive.Click += BtnClearReceive_Click;
        //
        // btnExport
        //
        btnExport.Location = new Point(100, 383);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(80, 25);
        btnExport.TabIndex = 4;
        btnExport.Text = "导出日志";
        btnExport.Click += BtnExport_Click;

        //
        // grpLog
        //
        grpLog.Controls.Add(rtbLog);
        grpLog.Controls.Add(btnClearLog);
        grpLog.Location = new Point(300, 430);
        grpLog.Name = "grpLog";
        grpLog.Size = new Size(580, 150);
        grpLog.TabIndex = 4;
        grpLog.TabStop = false;
        grpLog.Text = "系统日志";
        //
        // rtbLog
        //
        rtbLog.BackColor = Color.FromArgb(30, 30, 30);
        rtbLog.Font = new Font("Consolas", 9f);
        rtbLog.ForeColor = Color.White;
        rtbLog.Location = new Point(15, 23);
        rtbLog.Name = "rtbLog";
        rtbLog.ReadOnly = true;
        rtbLog.Size = new Size(550, 100);
        rtbLog.TabIndex = 0;
        rtbLog.WordWrap = false;
        //
        // btnClearLog
        //
        btnClearLog.Location = new Point(15, 120);
        btnClearLog.Name = "btnClearLog";
        btnClearLog.Size = new Size(60, 25);
        btnClearLog.TabIndex = 1;
        btnClearLog.Text = "清空日志";
        btnClearLog.Click += BtnClearLog_Click;

        //
        // TcpSocketControl
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(grpConnection);
        Controls.Add(grpSend);
        Controls.Add(grpClients);
        Controls.Add(grpReceive);
        Controls.Add(grpLog);
        Name = "TcpSocketControl";
        Size = new Size(890, 590);

        grpConnection.ResumeLayout(false);
        grpSend.ResumeLayout(false);
        grpSend.PerformLayout();
        grpClients.ResumeLayout(false);
        grpReceive.ResumeLayout(false);
        grpReceive.PerformLayout();
        grpLog.ResumeLayout(false);
        grpLog.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
        ResumeLayout(false);
    }

    #endregion

    // Connection
    private GroupBox grpConnection;
    private RadioButton rbClient;
    private RadioButton rbServer;
    private Label lblHost;
    private TextBox txtHost;
    private Label lblPort;
    private NumericUpDown nudPort;
    private CheckBox chkAutoReconnect;
    private Button btnConnect;
    private Label lblStatus;
    private Label lblClientCount;

    // Send
    private GroupBox grpSend;
    private RadioButton rbHex;
    private RadioButton rbAscii;
    private TextBox txtSend;
    private Button btnSend;
    private Button btnClearSend;

    // Clients
    private GroupBox grpClients;
    private ListBox lstClients;

    // Receive
    private GroupBox grpReceive;
    private CheckBox chkAutoScroll;
    private CheckBox chkShowTimestamp;
    private RichTextBox rtbReceive;
    private Button btnClearReceive;
    private Button btnExport;

    // Log
    private GroupBox grpLog;
    private RichTextBox rtbLog;
    private Button btnClearLog;
}
