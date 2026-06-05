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

        // === grpConnection ===
        grpConnection = new GroupBox();
        rbRtu = new RadioButton();
        rbTcp = new RadioButton();
        // RTU controls
        lblPort = new Label();
        cmbPort = new ComboBox();
        btnRefreshPorts = new Button();
        lblBaudRate = new Label();
        cmbBaudRate = new ComboBox();
        chkSimulation = new CheckBox();
        // TCP controls
        lblHost = new Label();
        txtHost = new TextBox();
        lblTcpPort = new Label();
        nudTcpPort = new NumericUpDown();
        // Common
        lblSlaveAddr = new Label();
        nudSlaveAddr = new NumericUpDown();
        btnConnect = new Button();
        lblStatus = new Label();

        grpConnection.Controls.AddRange([
            rbRtu, rbTcp,
            lblPort, cmbPort, btnRefreshPorts, lblBaudRate, cmbBaudRate, chkSimulation,
            lblHost, txtHost, lblTcpPort, nudTcpPort,
            lblSlaveAddr, nudSlaveAddr, btnConnect, lblStatus
        ]);
        grpConnection.Location = new Point(10, 10);
        grpConnection.Size = new Size(280, 310);
        grpConnection.Text = "连接配置";

        // rbRtu
        rbRtu.Checked = true;
        rbRtu.Location = new Point(15, 25);
        rbRtu.Size = new Size(60, 23);
        rbRtu.Text = "RTU";
        rbRtu.TabStop = true;

        // rbTcp
        rbTcp.Location = new Point(80, 25);
        rbTcp.Size = new Size(60, 23);
        rbTcp.Text = "TCP";

        // RTU controls
        lblPort.Text = "串口:";
        lblPort.Location = new Point(15, 55);
        lblPort.Size = new Size(50, 23);
        cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPort.Location = new Point(70, 52);
        cmbPort.Size = new Size(130, 23);
        btnRefreshPorts.Text = "刷新";
        btnRefreshPorts.Location = new Point(205, 51);
        btnRefreshPorts.Size = new Size(60, 25);

        lblBaudRate.Text = "波特率:";
        lblBaudRate.Location = new Point(15, 85);
        lblBaudRate.Size = new Size(50, 23);
        cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBaudRate.Items.AddRange(["9600", "19200", "38400", "57600", "115200"]);
        cmbBaudRate.Location = new Point(70, 82);
        cmbBaudRate.Size = new Size(130, 23);

        chkSimulation.Text = "模拟模式";
        chkSimulation.Location = new Point(15, 112);
        chkSimulation.Size = new Size(100, 23);

        // TCP controls
        lblHost.Text = "主机:";
        lblHost.Location = new Point(15, 55);
        lblHost.Size = new Size(50, 23);
        lblHost.Visible = false;
        txtHost.Text = "127.0.0.1";
        txtHost.Location = new Point(70, 52);
        txtHost.Size = new Size(130, 23);
        txtHost.Visible = false;

        lblTcpPort.Text = "端口:";
        lblTcpPort.Location = new Point(15, 85);
        lblTcpPort.Size = new Size(50, 23);
        lblTcpPort.Visible = false;
        nudTcpPort.Minimum = 1;
        nudTcpPort.Maximum = 65535;
        nudTcpPort.Value = 502;
        nudTcpPort.Location = new Point(70, 82);
        nudTcpPort.Size = new Size(130, 23);
        nudTcpPort.Visible = false;

        // Common
        lblSlaveAddr.Text = "站号:";
        lblSlaveAddr.Location = new Point(15, 145);
        lblSlaveAddr.Size = new Size(50, 23);
        nudSlaveAddr.Minimum = 1;
        nudSlaveAddr.Maximum = 247;
        nudSlaveAddr.Value = 1;
        nudSlaveAddr.Location = new Point(70, 142);
        nudSlaveAddr.Size = new Size(80, 23);

        btnConnect.Text = "连接";
        btnConnect.Location = new Point(15, 175);
        btnConnect.Size = new Size(120, 35);
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.ForeColor = Color.White;
        btnConnect.FlatStyle = FlatStyle.Flat;

        lblStatus.Text = "未连接";
        lblStatus.Location = new Point(140, 175);
        lblStatus.Size = new Size(125, 35);
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        lblStatus.Font = new Font(Font.FontFamily, 10f, FontStyle.Bold);
        lblStatus.ForeColor = Color.Red;

        // === grpRequest ===
        grpRequest = new GroupBox();
        lblFunction = new Label();
        cmbFunction = new ComboBox();
        lblStartAddr = new Label();
        nudStartAddr = new NumericUpDown();
        lblQuantity = new Label();
        nudQuantity = new NumericUpDown();
        lblValue = new Label();
        txtValue = new TextBox();
        btnSend = new Button();
        btnRead = new Button();

        grpRequest.Controls.AddRange([
            lblFunction, cmbFunction,
            lblStartAddr, nudStartAddr,
            lblQuantity, nudQuantity,
            lblValue, txtValue,
            btnSend, btnRead
        ]);
        grpRequest.Location = new Point(10, 325);
        grpRequest.Size = new Size(280, 220);
        grpRequest.Text = "请求配置";

        lblFunction.Text = "功能码:";
        lblFunction.Location = new Point(15, 30);
        lblFunction.Size = new Size(50, 23);
        cmbFunction.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFunction.Location = new Point(70, 27);
        cmbFunction.Size = new Size(195, 23);

        lblStartAddr.Text = "起始地址:";
        lblStartAddr.Location = new Point(15, 60);
        lblStartAddr.Size = new Size(55, 23);
        nudStartAddr.Maximum = 65535;
        nudStartAddr.Location = new Point(75, 57);
        nudStartAddr.Size = new Size(80, 23);

        lblQuantity.Text = "数量:";
        lblQuantity.Location = new Point(165, 60);
        lblQuantity.Size = new Size(35, 23);
        nudQuantity.Minimum = 1;
        nudQuantity.Maximum = 125;
        nudQuantity.Value = 1;
        nudQuantity.Location = new Point(200, 57);
        nudQuantity.Size = new Size(65, 23);

        lblValue.Text = "写入值:";
        lblValue.Location = new Point(15, 90);
        lblValue.Size = new Size(55, 23);
        txtValue.Location = new Point(75, 87);
        txtValue.Size = new Size(190, 23);
        txtValue.PlaceholderText = "多个值用逗号分隔";

        btnRead.Text = "读取";
        btnRead.Location = new Point(15, 125);
        btnRead.Size = new Size(80, 30);
        btnRead.BackColor = Color.FromArgb(0, 120, 215);
        btnRead.ForeColor = Color.White;
        btnRead.FlatStyle = FlatStyle.Flat;

        btnSend.Text = "写入";
        btnSend.Location = new Point(100, 125);
        btnSend.Size = new Size(80, 30);
        btnSend.BackColor = Color.FromArgb(0, 153, 51);
        btnSend.ForeColor = Color.White;
        btnSend.FlatStyle = FlatStyle.Flat;

        // === grpData ===
        grpData = new GroupBox();
        dgvRegisters = new DataGridView();
        cmbDataType = new ComboBox();
        lblDataType = new Label();

        grpData.Controls.AddRange([dgvRegisters, lblDataType, cmbDataType]);
        grpData.Location = new Point(300, 10);
        grpData.Size = new Size(580, 535);
        grpData.Text = "寄存器数据";

        lblDataType.Text = "数据类型:";
        lblDataType.Location = new Point(15, 25);
        lblDataType.Size = new Size(60, 23);
        cmbDataType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDataType.Items.AddRange(["UInt16", "Int16", "UInt32", "Float32", "Hex", "ASCII"]);
        cmbDataType.Location = new Point(80, 22);
        cmbDataType.Size = new Size(100, 23);

        dgvRegisters.Location = new Point(15, 52);
        dgvRegisters.Size = new Size(550, 470);
        dgvRegisters.ReadOnly = true;
        dgvRegisters.AllowUserToAddRows = false;
        dgvRegisters.AllowUserToDeleteRows = false;
        dgvRegisters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRegisters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // === grpLog ===
        grpLog = new GroupBox();
        rtbLog = new RichTextBox();
        btnClearLog = new Button();

        grpLog.Controls.AddRange([rtbLog, btnClearLog]);
        grpLog.Location = new Point(300, 550);
        grpLog.Size = new Size(580, 130);
        grpLog.Text = "通信日志";

        rtbLog.BackColor = Color.Black;
        rtbLog.Font = new Font("Consolas", 9f);
        rtbLog.ForeColor = Color.LimeGreen;
        rtbLog.Location = new Point(15, 23);
        rtbLog.Size = new Size(550, 80);
        rtbLog.ReadOnly = true;
        rtbLog.WordWrap = false;

        btnClearLog.Text = "清空";
        btnClearLog.Location = new Point(15, 100);
        btnClearLog.Size = new Size(60, 25);

        // === ModbusControl ===
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.AddRange([grpConnection, grpRequest, grpData, grpLog]);
        Name = "ModbusControl";
        Size = new Size(890, 690);

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
