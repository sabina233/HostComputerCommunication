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

        // 创建所有控件
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

        grpBrowse = new GroupBox();
        tvNodes = new TreeView();
        btnBrowse = new Button();

        grpReadWrite = new GroupBox();
        lblNodeId = new Label();
        txtNodeId = new TextBox();
        lblValue = new Label();
        txtValue = new TextBox();
        btnRead = new Button();
        btnWrite = new Button();
        btnSubscribe = new Button();

        grpData = new GroupBox();
        dgvData = new DataGridView();

        grpLog = new GroupBox();
        rtbLog = new RichTextBox();
        btnClearLog = new Button();

        grpConnection.SuspendLayout();
        grpBrowse.SuspendLayout();
        grpReadWrite.SuspendLayout();
        grpData.SuspendLayout();
        grpLog.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();

        //
        // grpConnection
        //
        grpConnection.Controls.Add(lblEndpoint);
        grpConnection.Controls.Add(txtEndpoint);
        grpConnection.Controls.Add(lblUsername);
        grpConnection.Controls.Add(txtUsername);
        grpConnection.Controls.Add(lblPassword);
        grpConnection.Controls.Add(txtPassword);
        grpConnection.Controls.Add(btnConnect);
        grpConnection.Controls.Add(btnDisconnect);
        grpConnection.Controls.Add(lblStatus);
        grpConnection.Location = new Point(10, 10);
        grpConnection.Name = "grpConnection";
        grpConnection.Size = new Size(280, 200);
        grpConnection.TabIndex = 0;
        grpConnection.TabStop = false;
        grpConnection.Text = "连接配置";
        //
        // lblEndpoint
        //
        lblEndpoint.Location = new Point(15, 30);
        lblEndpoint.Name = "lblEndpoint";
        lblEndpoint.Size = new Size(40, 23);
        lblEndpoint.TabIndex = 0;
        lblEndpoint.Text = "端点:";
        //
        // txtEndpoint
        //
        txtEndpoint.Location = new Point(60, 27);
        txtEndpoint.Name = "txtEndpoint";
        txtEndpoint.Size = new Size(205, 23);
        txtEndpoint.TabIndex = 1;
        txtEndpoint.Text = "opc.tcp://localhost:4840";
        //
        // lblUsername
        //
        lblUsername.Location = new Point(15, 60);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(50, 23);
        lblUsername.TabIndex = 2;
        lblUsername.Text = "用户名:";
        //
        // txtUsername
        //
        txtUsername.Location = new Point(65, 57);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(200, 23);
        txtUsername.TabIndex = 3;
        //
        // lblPassword
        //
        lblPassword.Location = new Point(15, 90);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(40, 23);
        lblPassword.TabIndex = 4;
        lblPassword.Text = "密码:";
        //
        // txtPassword
        //
        txtPassword.Location = new Point(60, 87);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(205, 23);
        txtPassword.TabIndex = 5;
        //
        // btnConnect
        //
        btnConnect.BackColor = Color.FromArgb(0, 120, 215);
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(15, 125);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(80, 30);
        btnConnect.TabIndex = 6;
        btnConnect.Text = "连接";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += BtnConnect_Click;
        //
        // btnDisconnect
        //
        btnDisconnect.Enabled = false;
        btnDisconnect.Location = new Point(100, 125);
        btnDisconnect.Name = "btnDisconnect";
        btnDisconnect.Size = new Size(80, 30);
        btnDisconnect.TabIndex = 7;
        btnDisconnect.Text = "断开";
        btnDisconnect.Click += BtnDisconnect_Click;
        //
        // lblStatus
        //
        lblStatus.Font = new Font(Font.FontFamily, 9f, FontStyle.Bold);
        lblStatus.ForeColor = Color.Red;
        lblStatus.Location = new Point(15, 165);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(250, 23);
        lblStatus.TabIndex = 8;
        lblStatus.Text = "未连接";

        //
        // grpBrowse
        //
        grpBrowse.Controls.Add(tvNodes);
        grpBrowse.Controls.Add(btnBrowse);
        grpBrowse.Location = new Point(10, 215);
        grpBrowse.Name = "grpBrowse";
        grpBrowse.Size = new Size(280, 350);
        grpBrowse.TabIndex = 1;
        grpBrowse.TabStop = false;
        grpBrowse.Text = "节点浏览";
        //
        // tvNodes
        //
        tvNodes.Location = new Point(15, 25);
        tvNodes.Name = "tvNodes";
        tvNodes.Size = new Size(250, 280);
        tvNodes.TabIndex = 0;
        tvNodes.AfterSelect += TvNodes_AfterSelect;
        //
        // btnBrowse
        //
        btnBrowse.Location = new Point(15, 310);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(80, 30);
        btnBrowse.TabIndex = 1;
        btnBrowse.Text = "刷新";
        btnBrowse.Click += BtnBrowse_Click;

        //
        // grpReadWrite
        //
        grpReadWrite.Controls.Add(lblNodeId);
        grpReadWrite.Controls.Add(txtNodeId);
        grpReadWrite.Controls.Add(lblValue);
        grpReadWrite.Controls.Add(txtValue);
        grpReadWrite.Controls.Add(btnRead);
        grpReadWrite.Controls.Add(btnWrite);
        grpReadWrite.Controls.Add(btnSubscribe);
        grpReadWrite.Location = new Point(300, 10);
        grpReadWrite.Name = "grpReadWrite";
        grpReadWrite.Size = new Size(580, 120);
        grpReadWrite.TabIndex = 2;
        grpReadWrite.TabStop = false;
        grpReadWrite.Text = "数据读写";
        //
        // lblNodeId
        //
        lblNodeId.Location = new Point(15, 30);
        lblNodeId.Name = "lblNodeId";
        lblNodeId.Size = new Size(50, 23);
        lblNodeId.TabIndex = 0;
        lblNodeId.Text = "节点ID:";
        //
        // txtNodeId
        //
        txtNodeId.Location = new Point(70, 27);
        txtNodeId.Name = "txtNodeId";
        txtNodeId.Size = new Size(200, 23);
        txtNodeId.TabIndex = 1;
        txtNodeId.Text = "ns=2;i=1";
        //
        // lblValue
        //
        lblValue.Location = new Point(280, 30);
        lblValue.Name = "lblValue";
        lblValue.Size = new Size(30, 23);
        lblValue.TabIndex = 2;
        lblValue.Text = "值:";
        //
        // txtValue
        //
        txtValue.Location = new Point(315, 27);
        txtValue.Name = "txtValue";
        txtValue.Size = new Size(150, 23);
        txtValue.TabIndex = 3;
        //
        // btnRead
        //
        btnRead.BackColor = Color.FromArgb(0, 120, 215);
        btnRead.FlatStyle = FlatStyle.Flat;
        btnRead.ForeColor = Color.White;
        btnRead.Location = new Point(15, 60);
        btnRead.Name = "btnRead";
        btnRead.Size = new Size(80, 30);
        btnRead.TabIndex = 4;
        btnRead.Text = "读取";
        btnRead.UseVisualStyleBackColor = false;
        btnRead.Click += BtnRead_Click;
        //
        // btnWrite
        //
        btnWrite.BackColor = Color.FromArgb(0, 153, 51);
        btnWrite.FlatStyle = FlatStyle.Flat;
        btnWrite.ForeColor = Color.White;
        btnWrite.Location = new Point(100, 60);
        btnWrite.Name = "btnWrite";
        btnWrite.Size = new Size(80, 30);
        btnWrite.TabIndex = 5;
        btnWrite.Text = "写入";
        btnWrite.UseVisualStyleBackColor = false;
        btnWrite.Click += BtnWrite_Click;
        //
        // btnSubscribe
        //
        btnSubscribe.Location = new Point(185, 60);
        btnSubscribe.Name = "btnSubscribe";
        btnSubscribe.Size = new Size(80, 30);
        btnSubscribe.TabIndex = 6;
        btnSubscribe.Text = "订阅";
        btnSubscribe.Click += BtnSubscribe_Click;

        //
        // grpData
        //
        grpData.Controls.Add(dgvData);
        grpData.Location = new Point(300, 135);
        grpData.Name = "grpData";
        grpData.Size = new Size(580, 250);
        grpData.TabIndex = 3;
        grpData.TabStop = false;
        grpData.Text = "订阅数据";
        //
        // dgvData
        //
        dgvData.AllowUserToAddRows = false;
        dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvData.Location = new Point(15, 25);
        dgvData.Name = "dgvData";
        dgvData.ReadOnly = true;
        dgvData.Size = new Size(550, 215);
        dgvData.TabIndex = 0;

        //
        // grpLog
        //
        grpLog.Controls.Add(rtbLog);
        grpLog.Controls.Add(btnClearLog);
        grpLog.Location = new Point(300, 390);
        grpLog.Name = "grpLog";
        grpLog.Size = new Size(580, 175);
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
        rtbLog.Size = new Size(550, 120);
        rtbLog.TabIndex = 0;
        rtbLog.WordWrap = false;
        //
        // btnClearLog
        //
        btnClearLog.Location = new Point(15, 145);
        btnClearLog.Name = "btnClearLog";
        btnClearLog.Size = new Size(60, 25);
        btnClearLog.TabIndex = 1;
        btnClearLog.Text = "清空";
        btnClearLog.Click += BtnClearLog_Click;

        //
        // OpcUaControl
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(grpConnection);
        Controls.Add(grpBrowse);
        Controls.Add(grpReadWrite);
        Controls.Add(grpData);
        Controls.Add(grpLog);
        Name = "OpcUaControl";
        Size = new Size(890, 575);

        grpConnection.ResumeLayout(false);
        grpConnection.PerformLayout();
        grpBrowse.ResumeLayout(false);
        grpReadWrite.ResumeLayout(false);
        grpReadWrite.PerformLayout();
        grpData.ResumeLayout(false);
        grpLog.ResumeLayout(false);
        grpLog.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
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
