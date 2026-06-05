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

        // === grpConnection ===
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

        grpConnection.Controls.AddRange([
            rbClient, rbServer,
            lblHost, txtHost, lblPort, nudPort,
            chkAutoReconnect, btnConnect, lblStatus, lblClientCount
        ]);
        grpConnection.Location = new Point(10, 10);
        grpConnection.Size = new Size(280, 220);
        grpConnection.Text = "连接配置";

        rbClient.Checked = true;
        rbClient.Location = new Point(15, 25);
        rbClient.Size = new Size(60, 23);
        rbClient.Text = "客户端";
        rbClient.TabStop = true;

        rbServer.Location = new Point(80, 25);
        rbServer.Size = new Size(60, 23);
        rbServer.Text = "服务端";

        lblHost.Text = "主机:";
        lblHost.Location = new Point(15, 55);
        lblHost.Size = new Size(40, 23);
        txtHost.Text = "127.0.0.1";
        txtHost.Location = new Point(60, 52);
        txtHost.Size = new Size(145, 23);

        lblPort.Text = "端口:";
        lblPort.Location = new Point(15, 85);
        lblPort.Size = new Size(40, 23);
        nudPort.Minimum = 1;
        nudPort.Maximum = 65535;
        nudPort.Value = 8080;
        nudPort.Location = new Point(60, 82);
        nudPort.Size = new Size(100, 23);

        chkAutoReconnect.Text = "自动重连";
        chkAutoReconnect.Checked = true;
        chkAutoReconnect.Location = new Point(15, 112);
        chkAutoReconnect.Size = new Size(100, 23);

        btnConnect.Text = "连接";
        btnConnect.Location = new Point(15, 145);
        btnConnect.Size = new Size(120, 35);
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.ForeColor = Color.White;
        btnConnect.FlatStyle = FlatStyle.Flat;

        lblStatus.Text = "未连接";
        lblStatus.Location = new Point(140, 145);
        lblStatus.Size = new Size(125, 35);
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        lblStatus.Font = new Font(Font.FontFamily, 10f, FontStyle.Bold);
        lblStatus.ForeColor = Color.Red;

        lblClientCount.Text = "";
        lblClientCount.Location = new Point(15, 190);
        lblClientCount.Size = new Size(250, 18);
        lblClientCount.ForeColor = Color.Gray;

        // === grpSend ===
        grpSend = new GroupBox();
        rbHex = new RadioButton();
        rbAscii = new RadioButton();
        txtSend = new TextBox();
        btnSend = new Button();
        btnClearSend = new Button();

        grpSend.Controls.AddRange([rbHex, rbAscii, txtSend, btnSend, btnClearSend]);
        grpSend.Location = new Point(10, 235);
        grpSend.Size = new Size(280, 150);
        grpSend.Text = "发送区";

        rbHex.Text = "HEX";
        rbHex.Location = new Point(15, 25);
        rbHex.Size = new Size(60, 23);
        rbHex.Checked = true;

        rbAscii.Text = "ASCII";
        rbAscii.Location = new Point(80, 25);
        rbAscii.Size = new Size(60, 23);

        txtSend.Location = new Point(15, 55);
        txtSend.Multiline = true;
        txtSend.Size = new Size(250, 50);
        txtSend.ScrollBars = ScrollBars.Vertical;

        btnSend.Text = "发送";
        btnSend.Location = new Point(15, 110);
        btnSend.Size = new Size(80, 30);
        btnSend.BackColor = Color.FromArgb(0, 153, 51);
        btnSend.ForeColor = Color.White;
        btnSend.FlatStyle = FlatStyle.Flat;

        btnClearSend.Text = "清空";
        btnClearSend.Location = new Point(100, 110);
        btnClearSend.Size = new Size(60, 30);

        // === grpClients ===
        grpClients = new GroupBox();
        lstClients = new ListBox();

        grpClients.Controls.Add(lstClients);
        grpClients.Location = new Point(10, 390);
        grpClients.Size = new Size(280, 150);
        grpClients.Text = "已连接客户端";
        grpClients.Visible = false;

        lstClients.Location = new Point(15, 23);
        lstClients.Size = new Size(250, 120);

        // === grpReceive ===
        grpReceive = new GroupBox();
        chkAutoScroll = new CheckBox();
        chkShowTimestamp = new CheckBox();
        rtbReceive = new RichTextBox();
        btnClearReceive = new Button();
        btnExport = new Button();

        grpReceive.Controls.AddRange([chkAutoScroll, chkShowTimestamp, rtbReceive, btnClearReceive, btnExport]);
        grpReceive.Location = new Point(300, 10);
        grpReceive.Size = new Size(580, 415);
        grpReceive.Text = "接收区";

        chkAutoScroll.Text = "自动滚动";
        chkAutoScroll.Checked = true;
        chkAutoScroll.Location = new Point(15, 20);
        chkAutoScroll.Size = new Size(85, 23);

        chkShowTimestamp.Text = "显示时间戳";
        chkShowTimestamp.Checked = true;
        chkShowTimestamp.Location = new Point(105, 20);
        chkShowTimestamp.Size = new Size(100, 23);

        rtbReceive.BackColor = Color.Black;
        rtbReceive.Font = new Font("Consolas", 9.5f);
        rtbReceive.ForeColor = Color.LimeGreen;
        rtbReceive.Location = new Point(15, 48);
        rtbReceive.Size = new Size(550, 330);
        rtbReceive.ReadOnly = true;
        rtbReceive.WordWrap = false;
        rtbReceive.ScrollBars = RichTextBoxScrollBars.Both;

        btnClearReceive.Text = "清空接收";
        btnClearReceive.Location = new Point(15, 383);
        btnClearReceive.Size = new Size(80, 25);

        btnExport.Text = "导出日志";
        btnExport.Location = new Point(100, 383);
        btnExport.Size = new Size(80, 25);

        // === grpLog ===
        grpLog = new GroupBox();
        rtbLog = new RichTextBox();
        btnClearLog = new Button();

        grpLog.Controls.AddRange([rtbLog, btnClearLog]);
        grpLog.Location = new Point(300, 430);
        grpLog.Size = new Size(580, 150);
        grpLog.Text = "系统日志";

        rtbLog.BackColor = Color.FromArgb(30, 30, 30);
        rtbLog.Font = new Font("Consolas", 9f);
        rtbLog.ForeColor = Color.White;
        rtbLog.Location = new Point(15, 23);
        rtbLog.Size = new Size(550, 100);
        rtbLog.ReadOnly = true;
        rtbLog.WordWrap = false;

        btnClearLog.Text = "清空日志";
        btnClearLog.Location = new Point(15, 120);
        btnClearLog.Size = new Size(80, 25);

        // === TcpSocketControl ===
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.AddRange([grpConnection, grpSend, grpClients, grpReceive, grpLog]);
        Name = "TcpSocketControl";
        Size = new Size(890, 590);

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

    // Clients (server mode)
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
