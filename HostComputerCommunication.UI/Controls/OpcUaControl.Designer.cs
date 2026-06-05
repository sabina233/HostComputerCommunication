namespace HostComputerCommunication.UI.Controls;

partial class OpcUaControl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _opcUaClient?.Dispose();
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
        lblEndpoint = new Label();
        txtEndpoint = new TextBox();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnConnect = new Button();
        btnDisconnect = new Button();
        lblStatus = new Label();

        grpConnection.Controls.AddRange([
            lblEndpoint, txtEndpoint,
            lblUsername, txtUsername,
            lblPassword, txtPassword,
            btnConnect, btnDisconnect, lblStatus
        ]);
        grpConnection.Location = new Point(10, 10);
        grpConnection.Size = new Size(280, 200);
        grpConnection.Text = "连接配置";

        lblEndpoint.Text = "端点:";
        lblEndpoint.Location = new Point(15, 30);
        lblEndpoint.Size = new Size(40, 23);
        txtEndpoint.Text = "opc.tcp://localhost:4840";
        txtEndpoint.Location = new Point(60, 27);
        txtEndpoint.Size = new Size(205, 23);

        lblUsername.Text = "用户名:";
        lblUsername.Location = new Point(15, 60);
        lblUsername.Size = new Size(50, 23);
        txtUsername.Location = new Point(65, 57);
        txtUsername.Size = new Size(200, 23);

        lblPassword.Text = "密码:";
        lblPassword.Location = new Point(15, 90);
        lblPassword.Size = new Size(40, 23);
        txtPassword.PasswordChar = '*';
        txtPassword.Location = new Point(60, 87);
        txtPassword.Size = new Size(205, 23);

        btnConnect.Text = "连接";
        btnConnect.Location = new Point(15, 125);
        btnConnect.Size = new Size(80, 30);
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.ForeColor = Color.White;
        btnConnect.FlatStyle = FlatStyle.Flat;

        btnDisconnect.Text = "断开";
        btnDisconnect.Location = new Point(100, 125);
        btnDisconnect.Size = new Size(80, 30);
        btnDisconnect.Enabled = false;

        lblStatus.Text = "未连接";
        lblStatus.Location = new Point(15, 165);
        lblStatus.Size = new Size(250, 23);
        lblStatus.ForeColor = Color.Red;
        lblStatus.Font = new Font(Font.FontFamily, 9f, FontStyle.Bold);

        // === grpBrowse ===
        grpBrowse = new GroupBox();
        tvNodes = new TreeView();
        btnBrowse = new Button();

        grpBrowse.Controls.AddRange([tvNodes, btnBrowse]);
        grpBrowse.Location = new Point(10, 215);
        grpBrowse.Size = new Size(280, 350);
        grpBrowse.Text = "节点浏览";

        tvNodes.Location = new Point(15, 25);
        tvNodes.Size = new Size(250, 280);

        btnBrowse.Text = "刷新";
        btnBrowse.Location = new Point(15, 310);
        btnBrowse.Size = new Size(80, 30);

        // === grpReadWrite ===
        grpReadWrite = new GroupBox();
        lblNodeId = new Label();
        txtNodeId = new TextBox();
        lblValue = new Label();
        txtValue = new TextBox();
        btnRead = new Button();
        btnWrite = new Button();
        btnSubscribe = new Button();

        grpReadWrite.Controls.AddRange([
            lblNodeId, txtNodeId, lblValue, txtValue,
            btnRead, btnWrite, btnSubscribe
        ]);
        grpReadWrite.Location = new Point(300, 10);
        grpReadWrite.Size = new Size(580, 120);
        grpReadWrite.Text = "数据读写";

        lblNodeId.Text = "节点ID:";
        lblNodeId.Location = new Point(15, 30);
        lblNodeId.Size = new Size(50, 23);
        txtNodeId.Text = "ns=2;i=1";
        txtNodeId.Location = new Point(70, 27);
        txtNodeId.Size = new Size(200, 23);

        lblValue.Text = "值:";
        lblValue.Location = new Point(280, 30);
        lblValue.Size = new Size(30, 23);
        txtValue.Location = new Point(315, 27);
        txtValue.Size = new Size(150, 23);

        btnRead.Text = "读取";
        btnRead.Location = new Point(15, 60);
        btnRead.Size = new Size(80, 30);
        btnRead.BackColor = Color.FromArgb(0, 120, 215);
        btnRead.ForeColor = Color.White;
        btnRead.FlatStyle = FlatStyle.Flat;

        btnWrite.Text = "写入";
        btnWrite.Location = new Point(100, 60);
        btnWrite.Size = new Size(80, 30);
        btnWrite.BackColor = Color.FromArgb(0, 153, 51);
        btnWrite.ForeColor = Color.White;
        btnWrite.FlatStyle = FlatStyle.Flat;

        btnSubscribe.Text = "订阅";
        btnSubscribe.Location = new Point(185, 60);
        btnSubscribe.Size = new Size(80, 30);

        // === grpData ===
        grpData = new GroupBox();
        dgvData = new DataGridView();

        grpData.Controls.Add(dgvData);
        grpData.Location = new Point(300, 135);
        grpData.Size = new Size(580, 250);
        grpData.Text = "订阅数据";

        dgvData.Location = new Point(15, 25);
        dgvData.Size = new Size(550, 215);
        dgvData.ReadOnly = true;
        dgvData.AllowUserToAddRows = false;
        dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // === grpLog ===
        grpLog = new GroupBox();
        rtbLog = new RichTextBox();
        btnClearLog = new Button();

        grpLog.Controls.AddRange([rtbLog, btnClearLog]);
        grpLog.Location = new Point(300, 390);
        grpLog.Size = new Size(580, 175);
        grpLog.Text = "系统日志";

        rtbLog.BackColor = Color.FromArgb(30, 30, 30);
        rtbLog.Font = new Font("Consolas", 9f);
        rtbLog.ForeColor = Color.White;
        rtbLog.Location = new Point(15, 23);
        rtbLog.Size = new Size(550, 120);
        rtbLog.ReadOnly = true;
        rtbLog.WordWrap = false;

        btnClearLog.Text = "清空";
        btnClearLog.Location = new Point(15, 145);
        btnClearLog.Size = new Size(60, 25);

        // === OpcUaControl ===
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.AddRange([grpConnection, grpBrowse, grpReadWrite, grpData, grpLog]);
        Name = "OpcUaControl";
        Size = new Size(890, 575);

        ResumeLayout(false);
    }

    #endregion

    // Connection
    private GroupBox grpConnection;
    private Label lblEndpoint;
    private TextBox txtEndpoint;
    private Label lblUsername;
    private TextBox txtUsername;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnConnect;
    private Button btnDisconnect;
    private Label lblStatus;

    // Browse
    private GroupBox grpBrowse;
    private TreeView tvNodes;
    private Button btnBrowse;

    // Read/Write
    private GroupBox grpReadWrite;
    private Label lblNodeId;
    private TextBox txtNodeId;
    private Label lblValue;
    private TextBox txtValue;
    private Button btnRead;
    private Button btnWrite;
    private Button btnSubscribe;

    // Data
    private GroupBox grpData;
    private DataGridView dgvData;

    // Log
    private GroupBox grpLog;
    private RichTextBox rtbLog;
    private Button btnClearLog;
}
