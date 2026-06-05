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

        // 创建所有控件
        grpConnection = new GroupBox();
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

        grpSend = new GroupBox();
        rbHex = new RadioButton();
        rbAscii = new RadioButton();
        txtSend = new TextBox();
        btnSend = new Button();
        btnClearSend = new Button();

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
        grpReceive.SuspendLayout();
        grpLog.SuspendLayout();
        SuspendLayout();

        //
        // _statsTimer
        //
        _statsTimer.Interval = 1000;
        _statsTimer.Tick += StatsTimer_Tick;

        //
        // grpConnection
        //
        grpConnection.Controls.Add(lblPort);
        grpConnection.Controls.Add(cmbPort);
        grpConnection.Controls.Add(btnRefresh);
        grpConnection.Controls.Add(lblBaud);
        grpConnection.Controls.Add(cmbBaudRate);
        grpConnection.Controls.Add(lblDataBits);
        grpConnection.Controls.Add(cmbDataBits);
        grpConnection.Controls.Add(lblParity);
        grpConnection.Controls.Add(cmbParity);
        grpConnection.Controls.Add(lblStopBits);
        grpConnection.Controls.Add(cmbStopBits);
        grpConnection.Controls.Add(chkSimulation);
        grpConnection.Controls.Add(btnConnect);
        grpConnection.Controls.Add(lblStatus);
        grpConnection.Controls.Add(lblStats);
        grpConnection.Location = new Point(10, 10);
        grpConnection.Name = "grpConnection";
        grpConnection.Size = new Size(280, 260);
        grpConnection.TabIndex = 0;
        grpConnection.TabStop = false;
        grpConnection.Text = "连接配置";
        //
        // lblPort
        //
        lblPort.Location = new Point(15, 30);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(50, 23);
        lblPort.TabIndex = 0;
        lblPort.Text = "串口:";
        //
        // cmbPort
        //
        cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPort.Location = new Point(70, 27);
        cmbPort.Name = "cmbPort";
        cmbPort.Size = new Size(130, 23);
        cmbPort.TabIndex = 1;
        //
        // btnRefresh
        //
        btnRefresh.Location = new Point(205, 26);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(60, 25);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "刷新";
        btnRefresh.Click += BtnRefresh_Click;
        //
        // lblBaud
        //
        lblBaud.Location = new Point(15, 60);
        lblBaud.Name = "lblBaud";
        lblBaud.Size = new Size(50, 23);
        lblBaud.TabIndex = 3;
        lblBaud.Text = "波特率:";
        //
        // cmbBaudRate
        //
        cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
        cmbBaudRate.Location = new Point(70, 57);
        cmbBaudRate.Name = "cmbBaudRate";
        cmbBaudRate.Size = new Size(130, 23);
        cmbBaudRate.TabIndex = 4;
        //
        // lblDataBits
        //
        lblDataBits.Location = new Point(15, 90);
        lblDataBits.Name = "lblDataBits";
        lblDataBits.Size = new Size(50, 23);
        lblDataBits.TabIndex = 5;
        lblDataBits.Text = "数据位:";
        //
        // cmbDataBits
        //
        cmbDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDataBits.Items.AddRange(new object[] { "7", "8" });
        cmbDataBits.Location = new Point(70, 87);
        cmbDataBits.Name = "cmbDataBits";
        cmbDataBits.Size = new Size(130, 23);
        cmbDataBits.TabIndex = 6;
        //
        // lblParity
        //
        lblParity.Location = new Point(15, 120);
        lblParity.Name = "lblParity";
        lblParity.Size = new Size(50, 23);
        lblParity.TabIndex = 7;
        lblParity.Text = "校验位:";
        //
        // cmbParity
        //
        cmbParity.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
        cmbParity.Location = new Point(70, 117);
        cmbParity.Name = "cmbParity";
        cmbParity.Size = new Size(130, 23);
        cmbParity.TabIndex = 8;
        //
        // lblStopBits
        //
        lblStopBits.Location = new Point(15, 150);
        lblStopBits.Name = "lblStopBits";
        lblStopBits.Size = new Size(50, 23);
        lblStopBits.TabIndex = 9;
        lblStopBits.Text = "停止位:";
        //
        // cmbStopBits
        //
        cmbStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStopBits.Items.AddRange(new object[] { "1", "1.5", "2" });
        cmbStopBits.Location = new Point(70, 147);
        cmbStopBits.Name = "cmbStopBits";
        cmbStopBits.Size = new Size(130, 23);
        cmbStopBits.TabIndex = 10;
        //
        // chkSimulation
        //
        chkSimulation.Location = new Point(15, 180);
        chkSimulation.Name = "chkSimulation";
        chkSimulation.Size = new Size(100, 23);
        chkSimulation.TabIndex = 11;
        chkSimulation.Text = "模拟模式";
        chkSimulation.CheckedChanged += ChkSimulation_CheckedChanged;
        //
        // btnConnect
        //
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(15, 210);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(120, 35);
        btnConnect.TabIndex = 12;
        btnConnect.Text = "打开串口";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += BtnConnect_Click;
        //
        // lblStatus
        //
        lblStatus.Font = new Font(Font.FontFamily, 10f, FontStyle.Bold);
        lblStatus.ForeColor = Color.Red;
        lblStatus.Location = new Point(140, 210);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(125, 35);
        lblStatus.TabIndex = 13;
        lblStatus.Text = "已断开";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        //
        // lblStats
        //
        lblStats.ForeColor = Color.Gray;
        lblStats.Location = new Point(15, 245);
        lblStats.Name = "lblStats";
        lblStats.Size = new Size(250, 18);
        lblStats.TabIndex = 14;
        lblStats.Text = "收: 0 字节 | 发: 0 字节";

        //
        // grpSend
        //
        grpSend.Controls.Add(rbHex);
        grpSend.Controls.Add(rbAscii);
        grpSend.Controls.Add(txtSend);
        grpSend.Controls.Add(btnSend);
        grpSend.Controls.Add(btnClearSend);
        grpSend.Location = new Point(10, 275);
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
        grpReceive.TabIndex = 2;
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
        grpLog.TabIndex = 3;
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
        // SerialPortControl
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(grpConnection);
        Controls.Add(grpSend);
        Controls.Add(grpReceive);
        Controls.Add(grpLog);
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
