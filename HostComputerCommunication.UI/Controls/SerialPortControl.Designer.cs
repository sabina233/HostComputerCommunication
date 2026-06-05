namespace HostComputerCommunication.UI.Controls;

partial class SerialPortControl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _serialPort?.Dispose();
            _simulator?.Dispose();
            _statsTimer?.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        _statsTimer = new System.Windows.Forms.Timer(components);

        // 创建所有 GroupBox（必须在 SuspendLayout 之前）
        grpConnection = new GroupBox();
        grpSend = new GroupBox();
        grpReceive = new GroupBox();
        grpLog = new GroupBox();

        // 创建所有子控件
        lblPort = new Label();
        cmbPort = new ComboBox();
        btnRefresh = new Button();
        lblBaud = new Label();
        cmbBaudRate = new ComboBox();
        lblDataBits = new Label();
        cmbDataBits = new ComboBox();
        lblParity = new Label();
        cmbParity = new ComboBox();
        lblStopBits = new Label();
        cmbStopBits = new ComboBox();
        chkSimulation = new CheckBox();
        btnConnect = new Button();
        lblStatus = new Label();
        lblStats = new Label();

        rbHex = new RadioButton();
        rbAscii = new RadioButton();
        txtSend = new TextBox();
        btnSend = new Button();
        btnClearSend = new Button();

        chkAutoScroll = new CheckBox();
        chkShowTimestamp = new CheckBox();
        rtbReceive = new RichTextBox();
        btnClearReceive = new Button();
        btnExport = new Button();

        rtbLog = new RichTextBox();
        btnClearLog = new Button();

        grpConnection.SuspendLayout();
        grpSend.SuspendLayout();
        grpReceive.SuspendLayout();
        grpLog.SuspendLayout();
        SuspendLayout();

        // --- grpConnection ---
        grpConnection.Controls.AddRange(new Control[] {
            lblPort, cmbPort, btnRefresh,
            lblBaud, cmbBaudRate,
            lblDataBits, cmbDataBits,
            lblParity, cmbParity,
            lblStopBits, cmbStopBits,
            chkSimulation, btnConnect, lblStatus, lblStats
        });
        grpConnection.Location = new Point(10, 10);
        grpConnection.Name = "grpConnection";
        grpConnection.Size = new Size(280, 260);
        grpConnection.TabIndex = 0;
        grpConnection.TabStop = false;
        grpConnection.Text = "连接配置";

        // lblPort
        lblPort.Location = new Point(15, 30);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(50, 23);
        lblPort.Text = "串口:";

        // cmbPort
        cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPort.Location = new Point(70, 27);
        cmbPort.Name = "cmbPort";
        cmbPort.Size = new Size(130, 23);

        // btnRefresh
        btnRefresh.Location = new Point(205, 26);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(60, 25);
        btnRefresh.Text = "刷新";

        // lblBaud
        lblBaud.Location = new Point(15, 60);
        lblBaud.Name = "lblBaud";
        lblBaud.Size = new Size(50, 23);
        lblBaud.Text = "波特率:";

        // cmbBaudRate
        cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
        cmbBaudRate.Location = new Point(70, 57);
        cmbBaudRate.Name = "cmbBaudRate";
        cmbBaudRate.Size = new Size(130, 23);

        // lblDataBits
        lblDataBits.Location = new Point(15, 90);
        lblDataBits.Name = "lblDataBits";
        lblDataBits.Size = new Size(50, 23);
        lblDataBits.Text = "数据位:";

        // cmbDataBits
        cmbDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDataBits.Items.AddRange(new object[] { "7", "8" });
        cmbDataBits.Location = new Point(70, 87);
        cmbDataBits.Name = "cmbDataBits";
        cmbDataBits.Size = new Size(130, 23);

        // lblParity
        lblParity.Location = new Point(15, 120);
        lblParity.Name = "lblParity";
        lblParity.Size = new Size(50, 23);
        lblParity.Text = "校验位:";

        // cmbParity
        cmbParity.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
        cmbParity.Location = new Point(70, 117);
        cmbParity.Name = "cmbParity";
        cmbParity.Size = new Size(130, 23);

        // lblStopBits
        lblStopBits.Location = new Point(15, 150);
        lblStopBits.Name = "lblStopBits";
        lblStopBits.Size = new Size(50, 23);
        lblStopBits.Text = "停止位:";

        // cmbStopBits
        cmbStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStopBits.Items.AddRange(new object[] { "1", "1.5", "2" });
        cmbStopBits.Location = new Point(70, 147);
        cmbStopBits.Name = "cmbStopBits";
        cmbStopBits.Size = new Size(130, 23);

        // chkSimulation
        chkSimulation.Location = new Point(15, 180);
        chkSimulation.Name = "chkSimulation";
        chkSimulation.Size = new Size(100, 23);
        chkSimulation.Text = "模拟模式";

        // btnConnect
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(15, 210);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(120, 35);
        btnConnect.TabIndex = 10;
        btnConnect.Text = "打开串口";
        btnConnect.UseVisualStyleBackColor = false;

        // lblStatus
        lblStatus.Font = new Font(Font.FontFamily, 10f, FontStyle.Bold);
        lblStatus.ForeColor = Color.Red;
        lblStatus.Location = new Point(140, 210);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(125, 35);
        lblStatus.TabIndex = 11;
        lblStatus.Text = "已断开";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;

        // lblStats
        lblStats.ForeColor = Color.Gray;
        lblStats.Location = new Point(15, 245);
        lblStats.Name = "lblStats";
        lblStats.Size = new Size(250, 18);
        lblStats.Text = "收: 0 字节 | 发: 0 字节";

        // === grpSend ===
        grpSend.Controls.AddRange(new Control[] { rbHex, rbAscii, txtSend, btnSend, btnClearSend });
        grpSend.Location = new Point(10, 275);
        grpSend.Name = "grpSend";
        grpSend.Size = new Size(280, 150);
        grpSend.TabIndex = 1;
        grpSend.TabStop = false;
        grpSend.Text = "发送区";

        // rbHex
        rbHex.Checked = true;
        rbHex.Location = new Point(15, 25);
        rbHex.Name = "rbHex";
        rbHex.Size = new Size(60, 23);
        rbHex.Text = "HEX";
        rbHex.TabStop = true;

        // rbAscii
        rbAscii.Location = new Point(80, 25);
        rbAscii.Name = "rbAscii";
        rbAscii.Size = new Size(60, 23);
        rbAscii.Text = "ASCII";

        // txtSend
        txtSend.Location = new Point(15, 55);
        txtSend.Multiline = true;
        txtSend.Name = "txtSend";
        txtSend.ScrollBars = ScrollBars.Vertical;
        txtSend.Size = new Size(250, 50);

        // btnSend
        btnSend.BackColor = Color.FromArgb(0, 153, 51);
        btnSend.FlatStyle = FlatStyle.Flat;
        btnSend.ForeColor = Color.White;
        btnSend.Location = new Point(15, 110);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(80, 30);
        btnSend.Text = "发送";
        btnSend.UseVisualStyleBackColor = false;

        // btnClearSend
        btnClearSend.Location = new Point(100, 110);
        btnClearSend.Name = "btnClearSend";
        btnClearSend.Size = new Size(60, 30);
        btnClearSend.Text = "清空";

        // === grpReceive ===
        grpReceive.Controls.AddRange(new Control[] { chkAutoScroll, chkShowTimestamp, rtbReceive, btnClearReceive, btnExport });
        grpReceive.Location = new Point(300, 10);
        grpReceive.Name = "grpReceive";
        grpReceive.Size = new Size(580, 415);
        grpReceive.TabIndex = 2;
        grpReceive.TabStop = false;
        grpReceive.Text = "接收区";

        // chkAutoScroll
        chkAutoScroll.Checked = true;
        chkAutoScroll.CheckState = CheckState.Checked;
        chkAutoScroll.Location = new Point(15, 20);
        chkAutoScroll.Name = "chkAutoScroll";
        chkAutoScroll.Size = new Size(85, 23);
        chkAutoScroll.Text = "自动滚动";

        // chkShowTimestamp
        chkShowTimestamp.Checked = true;
        chkShowTimestamp.CheckState = CheckState.Checked;
        chkShowTimestamp.Location = new Point(105, 20);
        chkShowTimestamp.Name = "chkShowTimestamp";
        chkShowTimestamp.Size = new Size(100, 23);
        chkShowTimestamp.Text = "显示时间戳";

        // rtbReceive
        rtbReceive.BackColor = Color.Black;
        rtbReceive.Font = new Font("Consolas", 9.5f);
        rtbReceive.ForeColor = Color.LimeGreen;
        rtbReceive.Location = new Point(15, 48);
        rtbReceive.Name = "rtbReceive";
        rtbReceive.ReadOnly = true;
        rtbReceive.ScrollBars = RichTextBoxScrollBars.Both;
        rtbReceive.Size = new Size(550, 330);
        rtbReceive.WordWrap = false;

        // btnClearReceive
        btnClearReceive.Location = new Point(15, 383);
        btnClearReceive.Name = "btnClearReceive";
        btnClearReceive.Size = new Size(80, 25);
        btnClearReceive.Text = "清空接收";

        // btnExport
        btnExport.Location = new Point(100, 383);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(80, 25);
        btnExport.Text = "导出日志";

        // === grpLog ===
        grpLog.Controls.AddRange(new Control[] { rtbLog, btnClearLog });
        grpLog.Location = new Point(300, 430);
        grpLog.Name = "grpLog";
        grpLog.Size = new Size(580, 150);
        grpLog.TabIndex = 3;
        grpLog.TabStop = false;
        grpLog.Text = "系统日志";

        // rtbLog
        rtbLog.BackColor = Color.FromArgb(30, 30, 30);
        rtbLog.Font = new Font("Consolas", 9f);
        rtbLog.ForeColor = Color.White;
        rtbLog.Location = new Point(15, 23);
        rtbLog.Name = "rtbLog";
        rtbLog.ReadOnly = true;
        rtbLog.Size = new Size(550, 100);
        rtbLog.WordWrap = false;

        // btnClearLog
        btnClearLog.Location = new Point(15, 120);
        btnClearLog.Name = "btnClearLog";
        btnClearLog.Size = new Size(80, 25);
        btnClearLog.Text = "清空日志";

        // === _statsTimer ===
        _statsTimer.Interval = 1000;

        // === SerialPortControl ===
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.AddRange(new Control[] { grpConnection, grpSend, grpReceive, grpLog });
        Name = "SerialPortControl";
        Size = new Size(890, 590);

        grpConnection.ResumeLayout(false);
        grpSend.ResumeLayout(false);
        grpSend.PerformLayout();
        grpReceive.ResumeLayout(false);
        grpReceive.PerformLayout();
        grpLog.ResumeLayout(false);
        grpLog.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    // Connection
    private GroupBox grpConnection;
    private Label lblPort;
    private ComboBox cmbPort;
    private Button btnRefresh;
    private Label lblBaud;
    private ComboBox cmbBaudRate;
    private Label lblDataBits;
    private ComboBox cmbDataBits;
    private Label lblParity;
    private ComboBox cmbParity;
    private Label lblStopBits;
    private ComboBox cmbStopBits;
    private CheckBox chkSimulation;
    private Button btnConnect;
    private Label lblStatus;
    private Label lblStats;

    // Send
    private GroupBox grpSend;
    private RadioButton rbHex;
    private RadioButton rbAscii;
    private TextBox txtSend;
    private Button btnSend;
    private Button btnClearSend;

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

    // Timer
    private System.Windows.Forms.Timer _statsTimer;
}
