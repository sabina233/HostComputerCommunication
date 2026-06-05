namespace HostComputerCommunication.UI.Controls;

partial class ModbusControl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _rtuClient?.Dispose();
            _tcpClient?.Dispose();
            _serialPort?.Dispose();
            _simulator?.Dispose();
            _tcpManager?.Dispose();
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
        rbRtu = new RadioButton();
        rbTcp = new RadioButton();
        lblPort = new Label();
        cmbPort = new ComboBox();
        btnRefreshPorts = new Button();
        lblBaudRate = new Label();
        cmbBaudRate = new ComboBox();
        chkSimulation = new CheckBox();
        lblHost = new Label();
        txtHost = new TextBox();
        lblTcpPort = new Label();
        nudTcpPort = new NumericUpDown();
        lblSlaveAddr = new Label();
        nudSlaveAddr = new NumericUpDown();
        btnConnect = new Button();
        lblStatus = new Label();

        grpRequest = new GroupBox();
        lblFunction = new Label();
        cmbFunction = new ComboBox();
        lblStartAddr = new Label();
        nudStartAddr = new NumericUpDown();
        lblQuantity = new Label();
        nudQuantity = new NumericUpDown();
        lblValue = new Label();
        txtValue = new TextBox();
        btnRead = new Button();
        btnSend = new Button();

        grpData = new GroupBox();
        dgvRegisters = new DataGridView();
        cmbDataType = new ComboBox();
        lblDataType = new Label();

        grpLog = new GroupBox();
        rtbLog = new RichTextBox();
        btnClearLog = new Button();

        grpConnection.SuspendLayout();
        grpRequest.SuspendLayout();
        grpData.SuspendLayout();
        grpLog.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudTcpPort).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudSlaveAddr).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudStartAddr).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudQuantity).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvRegisters).BeginInit();

        //
        // grpConnection
        //
        grpConnection.Controls.Add(rbRtu);
        grpConnection.Controls.Add(rbTcp);
        grpConnection.Controls.Add(lblPort);
        grpConnection.Controls.Add(cmbPort);
        grpConnection.Controls.Add(btnRefreshPorts);
        grpConnection.Controls.Add(lblBaudRate);
        grpConnection.Controls.Add(cmbBaudRate);
        grpConnection.Controls.Add(chkSimulation);
        grpConnection.Controls.Add(lblHost);
        grpConnection.Controls.Add(txtHost);
        grpConnection.Controls.Add(lblTcpPort);
        grpConnection.Controls.Add(nudTcpPort);
        grpConnection.Controls.Add(lblSlaveAddr);
        grpConnection.Controls.Add(nudSlaveAddr);
        grpConnection.Controls.Add(btnConnect);
        grpConnection.Controls.Add(lblStatus);
        grpConnection.Location = new Point(10, 10);
        grpConnection.Name = "grpConnection";
        grpConnection.Size = new Size(280, 310);
        grpConnection.TabIndex = 0;
        grpConnection.TabStop = false;
        grpConnection.Text = "连接配置";
        //
        // rbRtu
        //
        rbRtu.Checked = true;
        rbRtu.Location = new Point(15, 25);
        rbRtu.Name = "rbRtu";
        rbRtu.Size = new Size(60, 23);
        rbRtu.TabIndex = 0;
        rbRtu.TabStop = true;
        rbRtu.Text = "RTU";
        rbRtu.CheckedChanged += RbRtu_CheckedChanged;
        //
        // rbTcp
        //
        rbTcp.Location = new Point(80, 25);
        rbTcp.Name = "rbTcp";
        rbTcp.Size = new Size(60, 23);
        rbTcp.TabIndex = 1;
        rbTcp.Text = "TCP";
        //
        // lblPort
        //
        lblPort.Location = new Point(15, 55);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(50, 23);
        lblPort.TabIndex = 2;
        lblPort.Text = "串口:";
        //
        // cmbPort
        //
        cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPort.Location = new Point(70, 52);
        cmbPort.Name = "cmbPort";
        cmbPort.Size = new Size(130, 23);
        cmbPort.TabIndex = 3;
        //
        // btnRefreshPorts
        //
        btnRefreshPorts.Location = new Point(205, 51);
        btnRefreshPorts.Name = "btnRefreshPorts";
        btnRefreshPorts.Size = new Size(60, 25);
        btnRefreshPorts.TabIndex = 4;
        btnRefreshPorts.Text = "刷新";
        btnRefreshPorts.Click += BtnRefreshPorts_Click;
        //
        // lblBaudRate
        //
        lblBaudRate.Location = new Point(15, 85);
        lblBaudRate.Name = "lblBaudRate";
        lblBaudRate.Size = new Size(50, 23);
        lblBaudRate.TabIndex = 5;
        lblBaudRate.Text = "波特率:";
        //
        // cmbBaudRate
        //
        cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
        cmbBaudRate.Location = new Point(70, 82);
        cmbBaudRate.Name = "cmbBaudRate";
        cmbBaudRate.Size = new Size(130, 23);
        cmbBaudRate.TabIndex = 6;
        //
        // chkSimulation
        //
        chkSimulation.Location = new Point(15, 112);
        chkSimulation.Name = "chkSimulation";
        chkSimulation.Size = new Size(100, 23);
        chkSimulation.TabIndex = 7;
        chkSimulation.Text = "模拟模式";
        chkSimulation.CheckedChanged += ChkSimulation_CheckedChanged;
        //
        // lblHost
        //
        lblHost.Location = new Point(15, 55);
        lblHost.Name = "lblHost";
        lblHost.Size = new Size(50, 23);
        lblHost.TabIndex = 8;
        lblHost.Text = "主机:";
        lblHost.Visible = false;
        //
        // txtHost
        //
        txtHost.Location = new Point(70, 52);
        txtHost.Name = "txtHost";
        txtHost.Size = new Size(130, 23);
        txtHost.TabIndex = 9;
        txtHost.Text = "127.0.0.1";
        txtHost.Visible = false;
        //
        // lblTcpPort
        //
        lblTcpPort.Location = new Point(15, 85);
        lblTcpPort.Name = "lblTcpPort";
        lblTcpPort.Size = new Size(50, 23);
        lblTcpPort.TabIndex = 10;
        lblTcpPort.Text = "端口:";
        lblTcpPort.Visible = false;
        //
        // nudTcpPort
        //
        nudTcpPort.Location = new Point(70, 82);
        nudTcpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudTcpPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudTcpPort.Name = "nudTcpPort";
        nudTcpPort.Size = new Size(130, 23);
        nudTcpPort.TabIndex = 11;
        nudTcpPort.Value = new decimal(new int[] { 502, 0, 0, 0 });
        nudTcpPort.Visible = false;
        //
        // lblSlaveAddr
        //
        lblSlaveAddr.Location = new Point(15, 145);
        lblSlaveAddr.Name = "lblSlaveAddr";
        lblSlaveAddr.Size = new Size(50, 23);
        lblSlaveAddr.TabIndex = 12;
        lblSlaveAddr.Text = "站号:";
        //
        // nudSlaveAddr
        //
        nudSlaveAddr.Location = new Point(70, 142);
        nudSlaveAddr.Maximum = new decimal(new int[] { 247, 0, 0, 0 });
        nudSlaveAddr.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSlaveAddr.Name = "nudSlaveAddr";
        nudSlaveAddr.Size = new Size(80, 23);
        nudSlaveAddr.TabIndex = 13;
        nudSlaveAddr.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // btnConnect
        //
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(15, 175);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(120, 35);
        btnConnect.TabIndex = 14;
        btnConnect.Text = "连接";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += BtnConnect_Click;
        //
        // lblStatus
        //
        lblStatus.Font = new Font(Font.FontFamily, 10f, FontStyle.Bold);
        lblStatus.ForeColor = Color.Red;
        lblStatus.Location = new Point(140, 175);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(125, 35);
        lblStatus.TabIndex = 15;
        lblStatus.Text = "未连接";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;

        //
        // grpRequest
        //
        grpRequest.Controls.Add(lblFunction);
        grpRequest.Controls.Add(cmbFunction);
        grpRequest.Controls.Add(lblStartAddr);
        grpRequest.Controls.Add(nudStartAddr);
        grpRequest.Controls.Add(lblQuantity);
        grpRequest.Controls.Add(nudQuantity);
        grpRequest.Controls.Add(lblValue);
        grpRequest.Controls.Add(txtValue);
        grpRequest.Controls.Add(btnRead);
        grpRequest.Controls.Add(btnSend);
        grpRequest.Location = new Point(10, 325);
        grpRequest.Name = "grpRequest";
        grpRequest.Size = new Size(280, 220);
        grpRequest.TabIndex = 1;
        grpRequest.TabStop = false;
        grpRequest.Text = "请求配置";
        //
        // lblFunction
        //
        lblFunction.Location = new Point(15, 30);
        lblFunction.Name = "lblFunction";
        lblFunction.Size = new Size(50, 23);
        lblFunction.TabIndex = 0;
        lblFunction.Text = "功能码:";
        //
        // cmbFunction
        //
        cmbFunction.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFunction.Location = new Point(70, 27);
        cmbFunction.Name = "cmbFunction";
        cmbFunction.Size = new Size(195, 23);
        cmbFunction.TabIndex = 1;
        cmbFunction.SelectedIndexChanged += CmbFunction_SelectedIndexChanged;
        //
        // lblStartAddr
        //
        lblStartAddr.Location = new Point(15, 60);
        lblStartAddr.Name = "lblStartAddr";
        lblStartAddr.Size = new Size(55, 23);
        lblStartAddr.TabIndex = 2;
        lblStartAddr.Text = "起始地址:";
        //
        // nudStartAddr
        //
        nudStartAddr.Location = new Point(75, 57);
        nudStartAddr.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudStartAddr.Name = "nudStartAddr";
        nudStartAddr.Size = new Size(80, 23);
        nudStartAddr.TabIndex = 3;
        //
        // lblQuantity
        //
        lblQuantity.Location = new Point(165, 60);
        lblQuantity.Name = "lblQuantity";
        lblQuantity.Size = new Size(35, 23);
        lblQuantity.TabIndex = 4;
        lblQuantity.Text = "数量:";
        //
        // nudQuantity
        //
        nudQuantity.Location = new Point(200, 57);
        nudQuantity.Maximum = new decimal(new int[] { 125, 0, 0, 0 });
        nudQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudQuantity.Name = "nudQuantity";
        nudQuantity.Size = new Size(65, 23);
        nudQuantity.TabIndex = 5;
        nudQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // lblValue
        //
        lblValue.Location = new Point(15, 90);
        lblValue.Name = "lblValue";
        lblValue.Size = new Size(55, 23);
        lblValue.TabIndex = 6;
        lblValue.Text = "写入值:";
        //
        // txtValue
        //
        txtValue.Location = new Point(75, 87);
        txtValue.Name = "txtValue";
        txtValue.PlaceholderText = "多个值用逗号分隔";
        txtValue.Size = new Size(190, 23);
        txtValue.TabIndex = 7;
        //
        // btnRead
        //
        btnRead.BackColor = Color.FromArgb(0, 120, 215);
        btnRead.FlatStyle = FlatStyle.Flat;
        btnRead.ForeColor = Color.White;
        btnRead.Location = new Point(15, 125);
        btnRead.Name = "btnRead";
        btnRead.Size = new Size(80, 30);
        btnRead.TabIndex = 8;
        btnRead.Text = "读取";
        btnRead.UseVisualStyleBackColor = false;
        btnRead.Click += BtnRead_Click;
        //
        // btnSend
        //
        btnSend.BackColor = Color.FromArgb(0, 153, 51);
        btnSend.FlatStyle = FlatStyle.Flat;
        btnSend.ForeColor = Color.White;
        btnSend.Location = new Point(100, 125);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(80, 30);
        btnSend.TabIndex = 9;
        btnSend.Text = "写入";
        btnSend.UseVisualStyleBackColor = false;
        btnSend.Click += BtnSend_Click;

        //
        // grpData
        //
        grpData.Controls.Add(dgvRegisters);
        grpData.Controls.Add(lblDataType);
        grpData.Controls.Add(cmbDataType);
        grpData.Location = new Point(300, 10);
        grpData.Name = "grpData";
        grpData.Size = new Size(580, 535);
        grpData.TabIndex = 2;
        grpData.TabStop = false;
        grpData.Text = "寄存器数据";
        //
        // lblDataType
        //
        lblDataType.Location = new Point(15, 25);
        lblDataType.Name = "lblDataType";
        lblDataType.Size = new Size(60, 23);
        lblDataType.TabIndex = 0;
        lblDataType.Text = "数据类型:";
        //
        // cmbDataType
        //
        cmbDataType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDataType.Items.AddRange(new object[] { "UInt16", "Int16", "UInt32", "Float32", "Hex", "ASCII" });
        cmbDataType.Location = new Point(80, 22);
        cmbDataType.Name = "cmbDataType";
        cmbDataType.Size = new Size(100, 23);
        cmbDataType.TabIndex = 1;
        //
        // dgvRegisters
        //
        dgvRegisters.AllowUserToAddRows = false;
        dgvRegisters.AllowUserToDeleteRows = false;
        dgvRegisters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvRegisters.Location = new Point(15, 52);
        dgvRegisters.Name = "dgvRegisters";
        dgvRegisters.ReadOnly = true;
        dgvRegisters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRegisters.Size = new Size(550, 470);
        dgvRegisters.TabIndex = 2;

        //
        // grpLog
        //
        grpLog.Controls.Add(rtbLog);
        grpLog.Controls.Add(btnClearLog);
        grpLog.Location = new Point(300, 550);
        grpLog.Name = "grpLog";
        grpLog.Size = new Size(580, 130);
        grpLog.TabIndex = 3;
        grpLog.TabStop = false;
        grpLog.Text = "通信日志";
        //
        // rtbLog
        //
        rtbLog.BackColor = Color.Black;
        rtbLog.Font = new Font("Consolas", 9f);
        rtbLog.ForeColor = Color.LimeGreen;
        rtbLog.Location = new Point(15, 23);
        rtbLog.Name = "rtbLog";
        rtbLog.ReadOnly = true;
        rtbLog.Size = new Size(550, 80);
        rtbLog.TabIndex = 0;
        rtbLog.WordWrap = false;
        //
        // btnClearLog
        //
        btnClearLog.Location = new Point(15, 100);
        btnClearLog.Name = "btnClearLog";
        btnClearLog.Size = new Size(60, 25);
        btnClearLog.TabIndex = 1;
        btnClearLog.Text = "清空";
        btnClearLog.Click += BtnClearLog_Click;

        //
        // ModbusControl
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(grpConnection);
        Controls.Add(grpRequest);
        Controls.Add(grpData);
        Controls.Add(grpLog);
        Name = "ModbusControl";
        Size = new Size(890, 690);

        grpConnection.ResumeLayout(false);
        grpRequest.ResumeLayout(false);
        grpRequest.PerformLayout();
        grpData.ResumeLayout(false);
        grpLog.ResumeLayout(false);
        grpLog.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudTcpPort).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSlaveAddr).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudStartAddr).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudQuantity).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvRegisters).EndInit();
        ResumeLayout(false);
    }

    #endregion

    // Connection
    private GroupBox grpConnection;
    private RadioButton rbRtu;
    private RadioButton rbTcp;
    private Label lblPort;
    private ComboBox cmbPort;
    private Button btnRefreshPorts;
    private Label lblBaudRate;
    private ComboBox cmbBaudRate;
    private CheckBox chkSimulation;
    private Label lblHost;
    private TextBox txtHost;
    private Label lblTcpPort;
    private NumericUpDown nudTcpPort;
    private Label lblSlaveAddr;
    private NumericUpDown nudSlaveAddr;
    private Button btnConnect;
    private Label lblStatus;

    // Request
    private GroupBox grpRequest;
    private Label lblFunction;
    private ComboBox cmbFunction;
    private Label lblStartAddr;
    private NumericUpDown nudStartAddr;
    private Label lblQuantity;
    private NumericUpDown nudQuantity;
    private Label lblValue;
    private TextBox txtValue;
    private Button btnRead;
    private Button btnSend;

    // Data
    private GroupBox grpData;
    private DataGridView dgvRegisters;
    private Label lblDataType;
    private ComboBox cmbDataType;

    // Log
    private GroupBox grpLog;
    private RichTextBox rtbLog;
    private Button btnClearLog;
}
