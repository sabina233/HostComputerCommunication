using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.TcpSocket;

namespace HostComputerCommunication.UI.Controls;

public partial class TcpSocketControl : UserControl
{
    private TcpClientManager? _tcpClient;
    private TcpServerManager? _tcpServer;
    private readonly Logger _logger = new();
    private bool _isHexMode = true;

    private bool IsClientMode => rbClient.Checked;
    private bool IsConnected => IsClientMode
        ? (_tcpClient?.IsConnected ?? false)
        : (_tcpServer?.IsRunning ?? false);

    public TcpSocketControl()
    {
        InitializeComponent();
        SetupEvents();
        UpdateModeUI();
    }

    private void SetupEvents()
    {
        _logger.LogReceived += OnLogReceived;
        rbClient.CheckedChanged += (s, e) => UpdateModeUI();
        btnConnect.Click += (s, e) => ToggleConnection();
        btnSend.Click += (s, e) => SendData();
        btnClearSend.Click += (s, e) => txtSend.Clear();
        btnClearReceive.Click += (s, e) => rtbReceive.Clear();
        btnClearLog.Click += (s, e) => rtbLog.Clear();
        btnExport.Click += (s, e) => ExportLog();
        rbHex.CheckedChanged += (s, e) => _isHexMode = rbHex.Checked;
        txtSend.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && e.Control) { SendData(); e.SuppressKeyPress = true; } };
    }

    private void UpdateModeUI()
    {
        bool isClient = rbClient.Checked;
        lblHost.Text = isClient ? "主机:" : "监听:";
        txtHost.Enabled = isClient;
        chkAutoReconnect.Visible = isClient;
        grpClients.Visible = !isClient;
    }

    private void ToggleConnection()
    {
        if (IsConnected)
            Disconnect();
        else
            Connect();
    }

    private async void Connect()
    {
        if (IsClientMode)
        {
            var config = new TcpConfig
            {
                Host = txtHost.Text.Trim(),
                Port = (int)nudPort.Value,
                AutoReconnect = chkAutoReconnect.Checked
            };
            _tcpClient ??= new TcpClientManager(_logger, config);
            _tcpClient.DataReceived += OnClientDataReceived;
            _tcpClient.ConnectionStateChanged += OnClientConnectionStateChanged;

            await _tcpClient.ConnectAsync();
        }
        else
        {
            var config = new TcpConfig { Port = (int)nudPort.Value };
            _tcpServer ??= new TcpServerManager(_logger, config);
            _tcpServer.DataReceived += OnServerDataReceived;
            _tcpServer.ClientConnected += OnClientConnected;
            _tcpServer.ClientDisconnected += OnClientDisconnected;

            await _tcpServer.StartAsync();
            UpdateConnectionUI(true);
        }
    }

    private void Disconnect()
    {
        if (IsClientMode)
        {
            _tcpClient?.Disconnect();
        }
        else
        {
            _tcpServer?.Stop();
            UpdateConnectionUI(false);
        }
    }

    private async void SendData()
    {
        string text = txtSend.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        byte[] data;
        try
        {
            data = _isHexMode ? ByteHelper.FromHexString(text) : System.Text.Encoding.ASCII.GetBytes(text);
        }
        catch (Exception ex)
        {
            AppendReceiveText(Color.Red, $"[格式错误] {ex.Message}\n");
            return;
        }

        bool success;
        if (IsClientMode)
        {
            success = _tcpClient != null && await _tcpClient.SendAsync(data);
        }
        else
        {
            if (lstClients.SelectedItem is string clientId)
            {
                success = _tcpServer != null && await _tcpServer.SendAsync(clientId, data);
            }
            else
            {
                AppendReceiveText(Color.Yellow, "[提示] 请先选择一个客户端\n");
                return;
            }
        }

        if (success)
        {
            string hex = ByteHelper.ToHexString(data);
            string display = _isHexMode ? hex : System.Text.Encoding.ASCII.GetString(data);
            AppendReceiveText(Color.Cyan, $"[{DateTime.Now:HH:mm:ss.fff}] TX >> {display}\n");
        }
        else
        {
            AppendReceiveText(Color.Red, "[发送失败]\n");
        }
    }

    private void OnClientDataReceived(object? sender, byte[] data)
    {
        if (InvokeRequired) { Invoke(() => OnClientDataReceived(sender, data)); return; }
        DisplayReceivedData(null, data);
    }

    private void OnServerDataReceived(object? sender, (string ClientId, byte[] Data) e)
    {
        if (InvokeRequired) { Invoke(() => OnServerDataReceived(sender, e)); return; }
        DisplayReceivedData(e.ClientId, e.Data);
    }

    private void DisplayReceivedData(string? clientId, byte[] data)
    {
        string timestamp = chkShowTimestamp.Checked ? $"[{DateTime.Now:HH:mm:ss.fff}] " : "";
        string prefix = clientId != null ? $"{clientId} " : "";
        string display = _isHexMode ? ByteHelper.ToHexString(data) : System.Text.Encoding.ASCII.GetString(data);

        AppendReceiveText(Color.LimeGreen, $"{timestamp}{prefix}RX >> {display}\n");
    }

    private void OnClientConnectionStateChanged(object? sender, bool connected)
    {
        if (InvokeRequired) { Invoke(() => OnClientConnectionStateChanged(sender, connected)); return; }
        UpdateConnectionUI(connected);
    }

    private void OnClientConnected(object? sender, string clientId)
    {
        if (InvokeRequired) { Invoke(() => OnClientConnected(sender, clientId)); return; }
        lstClients.Items.Add(clientId);
        lblClientCount.Text = $"已连接: {_tcpServer?.ClientCount ?? 0}";
    }

    private void OnClientDisconnected(object? sender, string clientId)
    {
        if (InvokeRequired) { Invoke(() => OnClientDisconnected(sender, clientId)); return; }
        lstClients.Items.Remove(clientId);
        lblClientCount.Text = $"已连接: {_tcpServer?.ClientCount ?? 0}";
    }

    private void UpdateConnectionUI(bool connected)
    {
        if (InvokeRequired) { Invoke(() => UpdateConnectionUI(connected)); return; }

        if (connected)
        {
            btnConnect.Text = IsClientMode ? "断开" : "停止";
            btnConnect.BackColor = Color.FromArgb(200, 50, 50);
            lblStatus.Text = IsClientMode ? "已连接" : "监听中";
            lblStatus.ForeColor = Color.Green;
        }
        else
        {
            btnConnect.Text = IsClientMode ? "连接" : "启动";
            btnConnect.BackColor = Color.FromArgb(0, 120, 215);
            lblStatus.Text = "未连接";
            lblStatus.ForeColor = Color.Red;
        }
        SetConfigEnabled(!connected);
    }

    private void SetConfigEnabled(bool enabled)
    {
        rbClient.Enabled = enabled;
        rbServer.Enabled = enabled;
        txtHost.Enabled = enabled && IsClientMode;
        nudPort.Enabled = enabled;
        chkAutoReconnect.Enabled = enabled;
    }

    private void AppendReceiveText(Color color, string text)
    {
        rtbReceive.SelectionStart = rtbReceive.TextLength;
        rtbReceive.SelectionLength = 0;
        rtbReceive.SelectionColor = color;
        rtbReceive.AppendText(text);

        if (chkAutoScroll.Checked)
        {
            rtbReceive.SelectionStart = rtbReceive.TextLength;
            rtbReceive.ScrollToCaret();
        }
    }

    private void OnLogReceived(object? sender, LogEventArgs e)
    {
        if (InvokeRequired) { Invoke(() => OnLogReceived(sender, e)); return; }

        Color color = e.Level switch
        {
            LogLevel.Debug => Color.Gray,
            LogLevel.Info => Color.White,
            LogLevel.Warning => Color.Yellow,
            LogLevel.Error => Color.Red,
            _ => Color.White
        };

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.SelectionLength = 0;
        rtbLog.SelectionColor = color;
        rtbLog.AppendText($"[{e.Timestamp:HH:mm:ss}] {e.Message}\n");
        rtbLog.ScrollToCaret();
    }

    private void ExportLog()
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            FileName = $"TcpLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(dialog.FileName, rtbReceive.Text);
        }
    }
}
